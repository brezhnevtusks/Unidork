#if PLAYMAKER

using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unidork.PlayMaker
{
	/// <summary>
	/// Custom PlayMaker action that performs a Physics2D overlap check and stores acquired data based on inspector settings.
	/// </summary>
	[ActionCategory("Physics2D")]
	[HutongGames.PlayMaker.Tooltip("Performs a Physics2D.OverlapBox action.")]
	public class Overlap2D : BaseUpdateAction
	{
		#region Enums

		/// <summary>
		/// 2D shape used for overlap checks.
		/// </summary>
		public enum Shape2D
		{
			Box,
			Circle
		}

		/// <summary>
		/// Type of value to use when determining the origin of a check.
		/// </summary>
		public enum CheckOriginType
		{
			GameObject,
			Collider,
			Vector2
		}

		/// <summary>
		/// Defines how a box is rotated around the z axis to perform an overlap check.
		/// </summary>
		public enum BoxRotationType
		{
			/// <summary>
			/// Rotation will match rotation of the game object.
			/// </summary>
			MatchGameObject,
			
			/// <summary>
			/// Rotation is set explicitly in the inspector.
			/// </summary>
			[UsedImplicitly]
			Explicit
		}

		/// <summary>
		/// Defines how the size of a box or circle overlap check is set.
		/// </summary>
		public enum SizeSourceType
		{
			/// <summary>
			/// Size assigned in the inspector will be used.
			/// </summary>
			Explicit,
			
			/// <summary>
			/// Size of a collider assigned in the inspector will be used: extents for box checks and radius for circle checks.
			/// </summary>
			Collider
		}

		#endregion
		
		#region Fields

		/// <summary>
		/// 2D shape to use for overlap checks.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("2D shape to use for overlap checks.")]
		public Shape2D Shape;
		
		/// <summary>
		/// Layer mask used to detect overlapping objects.
		/// </summary>
		[UIHint(UIHint.Layer)] 
		[HutongGames.PlayMaker.Tooltip("Layer mask used to detect overlapping objects.")]
		public FsmInt LayerMask = default;

		/// <summary>
		/// Type of value to use as an origin for a box or circle overlap check.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Type of value to use as an origin for a box or circle overlap check.")]
		public CheckOriginType OriginType;
		
		/// <summary>
		/// Type of value that defines how the size of a box or circle overlap check is set.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Type of value that defines how the size of a box or circle overlap check is set.")]
		public SizeSourceType SizeSource;

		/// <summary>
		/// Game object to use as the origin of a box or circle overlap check.
		/// </summary>
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Game object to use as the origin of a box or circle overlap check.")]
		public FsmOwnerDefault OriginGameObject;

		/// <summary>
		/// Collider to use as the origin or size references for box or circle overlap checks.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Collider to use as the origin or size references for box or circle overlap checks.")]
		public FsmObject Collider;

		/// <summary>
		/// Game object to use as the origin of a box or circle overlap check.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Game object to use as the origin of a box or circle overlap check.")]
		public FsmVector2 OriginPosition;

		/// <summary>
		/// Size of the overlap box extents.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Size of the overlap box extents.")]
		public FsmVector2 BoxExtents;

		/// <summary>
		/// Type of rotation for the box used in the overlap check.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Type of rotation for the box used in the overlap check.")]
		public BoxRotationType RotationType;
		
		/// <summary>
		/// Rotation of the box used for overlap check.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Rotation of the box used for overlap check.")]
		public FsmFloat RotationAngle;

		/// <summary>
		/// Radius of the circle to use for the overlap check.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Radius of the circle to use for the overlap check.")]
		public FsmFloat Radius;

		/// <summary>
		/// Variable to store the overlap count.
		/// </summary>
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Variable to store the overlap count.")]
		[ShowIf("@this.hide == true")]
		public FsmInt OverlapCount = default;

		/// <summary>
		/// Variable to store the overlapping colliders.
		/// <remarks>Note that colliders are stored as Object values and require casting.</remarks>
		/// </summary>
		[UIHint(UIHint.Variable)]
		[ArrayEditor(typeof(Collider2D))]
		[HutongGames.PlayMaker.Tooltip("Variable to store the overlapping colliders.")]
		public FsmArray OverlappingColliders;

		/// <summary>
		/// Array of colliders overlapping with the object on current state update.
		/// </summary>
		private Collider2D[] overlappingColliders;

		#endregion
		
		#region Action

		public override void OnEnter()
		{
			switch (Shape)
			{
				case Shape2D.Box:
					OverlapBox();
					break;
				case Shape2D.Circle:
					OverlapCircle();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (everyFrame)
			{
				return;
			}
			
			Finish();
		}
		
		public override void OnActionUpdate()
		{
			OnEnter();
		}

		/// <summary>
		/// Performs a Physics2D.OverlapBoxAll check and stores results in respective variables.
		/// </summary>
		private void OverlapBox()
		{
			var boxOrigin = GetOriginPosition();

			Vector2 size;

			if (SizeSource == SizeSourceType.Collider)
			{
				if (Collider.IsNone)
				{
					Debug.LogError("A box collider is not assigned in Overlap2D PlayMaker action!");
					return;
				}

				var collider = (BoxCollider2D)Collider.Value;

				if (collider == null)
				{
					Debug.LogError("A box collider is not assigned in Overlap2D PlayMaker action!");
					return;
				}

				size = collider.bounds.extents;
			}
			else
			{
				size = BoxExtents.Value;
			}
			
			var rotation = 0f;
			
			if (RotationType == BoxRotationType.MatchGameObject)
			{
				GameObject rotationGo = Fsm.GetOwnerDefaultTarget(OriginGameObject);

				if (rotationGo != null)
				{
					rotation = rotationGo.transform.rotation.eulerAngles.z;
				}
			}
			else
			{
				rotation = RotationAngle.Value;
			}
			
			overlappingColliders = Physics2D.OverlapBoxAll(boxOrigin, size, rotation, 1 << LayerMask.Value);

			SetVariableValues();
		}

		/// <summary>
		/// Performs a Physics2D.OverlapCircleAll check and stores results in respective variables.
		/// </summary>
		private void OverlapCircle()
		{
			var circleOrigin = GetOriginPosition();

			float radius = SizeSource switch
			{
				SizeSourceType.Collider => ((CircleCollider2D)Collider.Value).radius,
				SizeSourceType.Explicit => Radius.Value,
				_ => throw new ArgumentOutOfRangeException()
			};
			
			overlappingColliders = Physics2D.OverlapCircleAll(circleOrigin, radius, 1 << LayerMask.Value);
			
			SetVariableValues();
		}

		/// <summary>
		/// Gets the origin position for a box or circle overlap check.
		/// </summary>
		/// <returns>
		/// A Vector2 representing the origin of an overlap check.
		/// </returns>
		private Vector2 GetOriginPosition()
		{
			var origin = Vector2.zero;

			switch (OriginType)
			{
				case CheckOriginType.GameObject:
				{
					GameObject originGo = Fsm.GetOwnerDefaultTarget(OriginGameObject);

					if (originGo == null)
					{
						Debug.LogError($"Origin game object is not assigned in Overlap2D PlayMaker action!");
					}

					origin = originGo.transform.position;
					break;
				}
				case CheckOriginType.Collider:
					origin = ((Collider2D)Collider.Value).bounds.center;
					break;
				case CheckOriginType.Vector2:
					origin = OriginPosition.Value;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return origin;
		}

		/// <summary>
		/// Sets the value of the variables using data stored in <see cref="overlappingColliders"/>.
		/// </summary>
		private void SetVariableValues()
		{
			int overlappingColliderCount = overlappingColliders.Length;
				
			if (!OverlapCount.IsNone)
			{
				OverlapCount.Value = overlappingColliderCount;
			}

			if (OverlappingColliders.IsNone)
			{
				return;
			}

			OverlappingColliders.Values = new object[overlappingColliderCount];

			for (var i = 0; i < overlappingColliders.Length; i++)
			{
				OverlappingColliders.Values[i] = overlappingColliders[i];
			}
		}

		#endregion
	}
}
	
#endif