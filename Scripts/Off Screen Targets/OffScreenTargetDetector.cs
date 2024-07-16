using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnderdorkStudios.UnderTools.Extensions;
using Unidork.Attributes;
using Unidork.Extensions;
using Unidork.Utility;
using UniRx;
using UnityEngine;

namespace Unidork.OffScreenTargets
{
	/// <summary>
	/// Detects targets that are off screen based on settings. Targets must implement the <see cref="IOffScreenTarget"/> interface.
	/// </summary>
	public class OffScreenTargetDetector : MonoBehaviour, IPausable
	{
		#region Enums

		/// <summary>
		/// Defines the shape within which to check for targets.
		/// </summary>
		private enum DetectionShape
		{
			Sphere,
			Cube
		}

		/// <summary>
		/// Defines how the origin for target detection is assigned.
		/// </summary>
		private enum OriginAssignmentType
		{
			Inspector,
			FindWithTag,
			Other
		}

		#endregion
		
		#region Properties

		/// <summary>
		/// Detector's name.
		/// </summary>
		/// <value>
		/// Gets the value of the string field name.
		/// </value>
		private string Name => detectorName;

		/// <summary>
		/// Transform that represents the origin point relative to which the target detection happens.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the field origin.
		/// </value>
		public Transform DetectionOrigin { get => detectionOrigin; set => detectionOrigin = value; }

		/// <summary>
		/// Data about targets that are currently detected by this component.
		/// </summary>
		/// <value>
		/// Gets the value of the field targetData.
		/// </value>
		public List<OffScreenTargetData> TargetData => targetData;
		
		/// <summary>
		/// Is this component currently paused?
		/// </summary>
		public ReactiveProperty<bool> IsPaused { get; private set; }

		#endregion
		
		#region Fields

		/// <summary>
		/// List containing all detectors for query purposes.
		/// </summary>
		private static List<OffScreenTargetDetector> detectors;

		/// <summary>
		/// Reference to game's main camera.
		/// </summary>
		private static Camera mainCamera;
		
		/// <summary>
		/// Detector's name.
		/// </summary>
		/// <remarks>
		/// If you have more than one detector in your game, make sure they have unique names!
		/// </remarks>
		[Space, BaseHeader, Space]
		[Tooltip("Detector's name." +
		         "If you have more than one detector in your game, make sure they have unique names!")]
		[SerializeField]
		private string detectorName = "OffScreenTargetDetector";

		/// <summary>
		/// Defines how the origin for target detection will be assigned.
		/// </summary>
		[Tooltip("Defines how the origin for target detection will be assigned.\n" +
		         "Inspector - target is assigned in inspector.\n" +
		         "FindWithTag - target will be located with GameObject.FindWithTag.\n" +
		         "Other - target will be assigned by other means: properties, method calls etc.")]
		[SerializeField]
		private OriginAssignmentType originAssignmentType = OriginAssignmentType.FindWithTag;

		/// <summary>
		/// Transform that represents the origin point relative to which the target detection happens.
		/// </summary>
		[ShowIf("@this.originAssignmentType", OriginAssignmentType.Inspector)]
		[Tooltip("Transform that represents the origin point relative to which the target detection happens.")]
		[SerializeField]
		protected Transform detectionOrigin;
		
		/// <summary>
		/// Tag to use when searching for detection origin with Unity's GameObject.FindWithTag().
		/// </summary>
		[ShowIf("@this.originAssignmentType", OriginAssignmentType.FindWithTag)]
		[Tooltip("Tag to use when searching for detection origin with Unity's GameObject.FindWithTag().")]
		[SerializeField]
		private new string tag = "Player";

		/// <summary>
		/// Layer mask to use for detecting off-screen targets.
		/// </summary>
		[SerializeField]
		protected LayerMask targetLayerMask;

		/// <summary>
		/// Shape within which to check for targets.
		/// </summary>
		[Tooltip("Shape within which we will check for targets.")]
		[SerializeField]
		private DetectionShape shape = DetectionShape.Sphere;

		/// <summary>
		/// Radius of the sphere within which we will check for targets.
		/// </summary>
		[ShowIf("@this.shape", DetectionShape.Sphere)]
		[Tooltip("Radius of the sphere within which we will check for targets.")]
		[SerializeField]
		private float sphereRadius = 20f;

		/// <summary>
		/// Dimensions of the cube within which we will check for targets.
		/// </summary>
		[ShowIf("@this.shape", DetectionShape.Cube)]
		[Tooltip("Dimensions of the cube within which we will check for targets.")]
		[SerializeField]
		private Vector3 dimensions = Vector3.one * 20f;

		/// <summary>
		/// Should the maximum number of targets be limited? If set to True, closer targets will be preferred to distant ones.
		/// </summary>
		[Tooltip("Should the maximum number of targets be limited? If set to True, closer targets will be preferred to distant ones.")]
		[SerializeField]
		private bool limitMaximumTargets;

		/// <summary>
		/// Maximum number of targets to detect. If total number of targets is higher, targets closer to the detection origin will be preferred.
		/// </summary>
		[ShowIf("@this.limitMaximumTargets == true")]
		[Tooltip("Maximum number of targets to detect. If total number of targets is higher, targets closer to the detection origin will be preferred.")]
		[SerializeField]
		private int maximumNumberOfTargets = 30;

		/// <summary>
		/// Should this component be enabled after initialization?
		/// </summary>
		[Tooltip("Should this component be enabled after initialization?")]
		[SerializeField]
		private bool enableOnInit = true;
		
		/// <summary>
		/// Targets currently detected by this component.
		/// </summary>
		private readonly List<OffScreenTargetData> targetData = new();

		/// <summary>
		/// Collider array to use for the non-alloc queries if maximum number of targets is limited.
		/// </summary>
		private Collider[] hitColliders;

		#endregion

		#region Init

		public virtual void Init()
		{
			if (detectors.IsNullOrEmpty())
			{
				detectors = new List<OffScreenTargetDetector>();
			} 
			
			detectors.Add(this);

			if (mainCamera == null)
			{
				mainCamera = Camera.main;
			}

			if (limitMaximumTargets)
			{
				hitColliders = new Collider[maximumNumberOfTargets];
			}

			if (originAssignmentType == OriginAssignmentType.FindWithTag)
			{
				detectionOrigin = GameObject.FindWithTag(tag)?.transform;
			}
			
			if (enableOnInit)
			{
				Enable();
			}
			else
			{
				Disable();
			}
			
			PauseManager.RegisterPausable(this);
		}

		#endregion

		#region Detect

		private void Update()
		{
			if (IsPaused.Value || detectionOrigin == null)
			{
				return;
			}

			DetectTargets();
			ExcludeVisibleTargets();
		}

		/// <summary>
		/// Detects the targets based on current settings.
		/// </summary>
		protected virtual void DetectTargets()
		{
			targetData.Clear();
			
			if (limitMaximumTargets)
			{
				DetectTargetsWithLimit();
				return;
			}
			
			DetectTargetsWithNoLimit();
		}

		/// <summary>
		/// Detects targets with no limit on the maximum number of targets.
		/// </summary>
		protected virtual void DetectTargetsWithNoLimit()
		{
			Collider[] targetColliders = shape switch
			{
				DetectionShape.Sphere => Physics.OverlapSphere(detectionOrigin.position, sphereRadius, targetLayerMask,
				                                               QueryTriggerInteraction.Collide),
				DetectionShape.Cube => Physics.OverlapBox(detectionOrigin.position, dimensions * 0.5f,
				                                          Quaternion.identity, targetLayerMask,
				                                          QueryTriggerInteraction.Collide),
				_ => null
			};
			
			AddTargets(targetColliders);
		}

		/// <summary>
		/// Detects targets with a limited maximum number of targets.
		/// </summary>
		protected virtual void DetectTargetsWithLimit()
		{
			int numberOfTargets = shape switch
			{
				DetectionShape.Sphere => Physics.OverlapSphereNonAlloc(detectionOrigin.position, sphereRadius,
				                                                       hitColliders, targetLayerMask,
				                                                       QueryTriggerInteraction.Collide),
				DetectionShape.Cube => Physics.OverlapBoxNonAlloc(detectionOrigin.position, dimensions * 0.5f,
				                                                  hitColliders, Quaternion.identity, targetLayerMask,
				                                                  QueryTriggerInteraction.Collide),
				_ => 0
			};

			var colliders = new List<Collider>();
			
			for (var i = 0; i < numberOfTargets; i++)
			{
				colliders.Add(hitColliders[i]); 
			}
			
			AddTargets(colliders);
		}

		/// <summary>
		/// Gets targets from a collection of colliders and adds them to the list of current targets.
		/// </summary>
		/// <param name="colliders">Colliders.</param>
		private void AddTargets(ICollection<Collider> colliders)
		{
			foreach (Collider collider in colliders)
			{
				IOffScreenTarget target = collider.gameObject.GetComponentInParent<IOffScreenTarget>();

				if (target is not { OffScreenTargetIsActive: true })
				{
					continue;
				}
				
				targetData.Add(new OffScreenTargetData {Target = target, ViewportPosition = Vector3.zero});
			}
		}

		/// <summary>
		/// Excludes the targets that are actually visible to the player from list of current targets.
		/// </summary>
		private void ExcludeVisibleTargets()
		{
			if (targetData.IsEmpty())
			{
				return;
			}

			for (int i = targetData.Count - 1; i >= 0; i--)
			{
				OffScreenTargetData currentTargetData = targetData[i];
				
				IOffScreenTarget target = currentTargetData.Target;
				
				Vector3 targetWorldPosition = target.GetPosition();

				Vector2 targetViewportPosition = mainCamera.WorldToViewportPoint(targetWorldPosition);
				Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetWorldPosition); 
				
				if (screenPosition.z < 0f)
				{
					targetViewportPosition *= -1f;
				}
				
				if (screenPosition.z < 0f || !ViewportPositionIsVisible(targetViewportPosition))
				{
					currentTargetData.ViewportPosition = targetViewportPosition;
					continue;
				}
				
				targetData.RemoveAt(i);
			}
		}

		/// <summary>
		/// Checks whether the passed viewport position is visible to the player.
		/// </summary>
		/// <param name="viewportPosition">Viewport position.</param>
		/// <returns>
		/// True if position is visible, False otherwise.
		/// </returns>
		private bool ViewportPositionIsVisible(Vector2 viewportPosition)
		{
			float xPosition = viewportPosition.x;
			float yPosition = viewportPosition.y;
			
			return xPosition is > 0 and <= 1f && yPosition is > 0 and <= 1f;
		}

		#endregion

		#region Layer Mask

		/// <summary>
		/// Adds a layer to target layer mask.
		/// </summary>
		/// <param name="layerName">Name of the layer to remove.</param>
		public void AddTargetLayer(string layerName) => targetLayerMask = targetLayerMask.AddLayer(layerName);

		/// <summary>
		/// Adds a later to target layer mask.
		/// </summary>
		/// <param name="layer">Layer to remove.</param>
		public void AddTargetLayer(int layer) => targetLayerMask = targetLayerMask.AddLayer(layer);

		/// <summary>
		/// Removes a layer from target layer mask.
		/// </summary>
		/// <param name="layerName">Name of the layer to remove..</param>
		public void RemoveLayer(string layerName) => targetLayerMask = targetLayerMask.RemoveLayer(layerName);

		/// <summary>
		/// Removes a layer from target layer mask.
		/// </summary>
		/// <param name="layer">Layer to remove.</param>
		public void RemoveLayer(int layer) => targetLayerMask = targetLayerMask.RemoveLayer(layer);

		#endregion

		#region Get

		/// <summary>
		/// Tries to get an off screen detector whose name matches the passed value.
		/// </summary>
		/// <param name="name">Detector name.</param>
		/// <param name="targetDetector">Target detector.</param>
		/// <returns>
		/// True if a detector with the respective name is successfully located, False otherwise.
		/// </returns>
		public static bool TryGetDetectorWithName(string name, out OffScreenTargetDetector targetDetector)
		{
			targetDetector = null;
			
			if (detectors.IsNullOrEmpty())
			{
				Debug.LogError($"There aren't any off screen target detectors registered!");
				return false;
			}

			foreach (OffScreenTargetDetector detector in detectors)
			{
				if (!string.Equals(detector.Name, name))
				{
					continue;
				}

				targetDetector = detector;
				return true;
			}

			Debug.LogError($"Failed to find off screen target detector with name {name}!");
			return false;
		}

		#endregion
		
		#region Toggle

		/// <summary>
		/// Enables the detector.
		/// </summary>
		public virtual void Enable()
		{
			PauseManager.RegisterPausable(this);
			enabled = true;
		}

		/// <summary>
		/// Disables the detector.
		/// </summary>
		public virtual void Disable()
		{
			PauseManager.UnregisterPausable(this);
			enabled = false;
		}

		#endregion

		#region Destroy

		private void OnDestroy()
		{
			PauseManager.UnregisterPausable(this);
		}

		#endregion
	}
}