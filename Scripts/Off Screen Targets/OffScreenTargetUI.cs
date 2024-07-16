using System.Collections.Generic;
using Unidork.Attributes;
using Unidork.Tweens;
using Unidork.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Unidork.OffScreenTargets
{
	/// <summary>
	/// Handles creating, destroying, settings up and positioning indicators that point to off screen targets detected by <see cref="offScreenTargetDetector"/>.
	/// </summary>
	public class OffScreenTargetUI : MonoBehaviour, IPausable
	{
		#region Properties

		/// <summary>
		/// Is this UI currently paused?
		/// </summary>
		public ReactiveProperty<bool> IsPaused { get; private set; }

		#endregion

		#region Fields

		/// <summary>
		/// Name of the target detector that this UI will get off screen targets from.
		/// </summary>
		[Space, BaseHeader, Space]
		[Tooltip("Name of the target detector that this UI will get off screen targets from.")]
		[SerializeField] private string detectorName = "OffScreenTargetDetector";

		/// <summary>
		/// Reference to the off screen indicator asset.
		/// </summary>
		[Tooltip("Reference to the off screen indicator asset.")]
		[SerializeField] private AssetReference indicatorAssetReference = null;

		/// <summary>
		/// Transform that serves as a holder for the off screen target indicators.
		/// </summary>
		[SerializeField] private Transform indicatorHolder = null;
		
		/// <summary>
		/// When set to True, indicators will have an additional image enabled to represent individual target types.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("When set to True, indicators will have an additional image enabled to represent individual target types.")]
		[SerializeField] private bool indicatorsHaveIndividualTargetIcons = false;
		
		/// <summary>
		/// Offset from the center of the screen to the indicator position.
		/// </summary>
		[Range(0.5f, 1f)]
		[Tooltip("Offset from the center of the screen to the indicator position.")]
		[SerializeField] private float indicatorOffset = 0.9f;

		/// <summary>
		///  Optional settings to use for scaling up an indicator when it is enabled.
		/// </summary>
		[Tooltip("Optional settings to use for scaling up an indicator when it is enabled.")]
		[SerializeField] private FloatTweenSettings indicatorScaleUpTweenSettings;
		
		/// <summary>
		///  Optional settings to use for scaling down an indicator when it is disabled.
		/// </summary>
		[Tooltip("Optional settings to use for scaling up an indicator when it is disabled.")]
		[SerializeField] private FloatTweenSettings indicatorScaleDownTweenSettings;
		
		/// <summary>
		/// Should this component be paused on start?
		/// </summary>
		[Tooltip("Should this component be paused on start?")]
		[SerializeField] private bool enableOnInit = true;

		/// <summary>
		/// List of active indicators.
		/// </summary>
		private readonly List<OffScreenTargetIndicator> activeIndicators = new();
		
		/// <summary>
		/// Component that detects off screen targets.
		/// </summary>
		private OffScreenTargetDetector offScreenTargetDetector;
		
		/// <summary>
		/// Has the indicator asset been loaded?
		/// </summary>
		private bool indicatorAssetLoaded;

		/// <summary>
		/// Load handle that stores the off screen indicator asset.
		/// </summary>
		private AsyncOperationHandle<GameObject> indicatorLoadHandle;
		
		/// <summary>
		/// Bounds of the screen that take into account indicator offset.
		/// </summary>
		private Vector3 viewportBounds;
		
		#endregion

		#region Constants

		/// <summary>
		/// Coordinates of the center of the viewport.
		/// </summary>
		private readonly Vector3 viewportCenter = Vector3.one * 0.5f;
		
		#endregion

		#region Init

		public virtual void Init()
		{
			UpdateScreenData();
			
			if (enableOnInit)
			{
				Enable();
			}
			else
			{
				Disable();
			}
		}

		public virtual void UpdateScreenData()
		{
			viewportBounds = viewportCenter * indicatorOffset;
		}

		#endregion

		#region Update

		private void Update()
		{
			if (IsPaused.Value || !indicatorAssetLoaded)
			{
				return;
			}

			List<OffScreenTargetData> targetData = offScreenTargetDetector.TargetData;

			if (activeIndicators.Count > 0)
			{
				for (int i = activeIndicators.Count - 1; i >= 0; i--)
				{
					OffScreenTargetIndicator currentIndicator = activeIndicators[i];
					
					var shouldRemoveIndicator = true;
					
					foreach (OffScreenTargetData data in targetData)
					{
						if (data.Target == currentIndicator.Target)
						{
							shouldRemoveIndicator = false;
							break;
						}
					}

					if (!shouldRemoveIndicator)
					{
						continue;
					}
					
					currentIndicator.Disable(indicatorScaleDownTweenSettings);
					activeIndicators.RemoveAt(i);
				}
			}
			
			foreach (OffScreenTargetData data in targetData)
			{
				OffScreenTargetIndicator indicator = GetIndicator(data);
				PositionAndRotateIndicator(indicator, data);
			}
		}

		private void PositionAndRotateIndicator(OffScreenTargetIndicator indicator, OffScreenTargetData targetData)
		{
			Vector3 screenPosition = targetData.ViewportPosition;

			screenPosition -= Vector3.one * 0.5f;
			
            float rotationAngle = Mathf.Atan2(screenPosition.y, screenPosition.x);
            float slope = Mathf.Tan(rotationAngle);
			
            screenPosition = screenPosition.x > 0 ? 
	            new Vector3(viewportBounds.x, viewportBounds.x * slope, 0) : 
	            new Vector3(-viewportBounds.x, -viewportBounds.x * slope, 0);
			
            if(screenPosition.y > viewportBounds.y)
            {
                screenPosition = new Vector3(viewportBounds.y / slope, viewportBounds.y, 0);
            }
            else if(screenPosition.y < -viewportBounds.y)
            {
                screenPosition = new Vector3(-viewportBounds.y / slope, -viewportBounds.y, 0);
            }
            screenPosition += Vector3.one * 0.5f;
            
            screenPosition.x *= Screen.width;
            screenPosition.y *= Screen.height;

			indicator.SetPositionAndRotation(screenPosition, rotationAngle);
		}
		
		#endregion
		
		#region Assets

		/// <summary>
		/// Loads the indicator asset.
		/// </summary>
		public void LoadIndicatorAsset()
		{
			indicatorAssetLoaded = false;

			indicatorLoadHandle = Addressables.LoadAssetAsync<GameObject>(indicatorAssetReference);
			indicatorLoadHandle.Completed += completionHandle =>
			{
				if (completionHandle.Status != AsyncOperationStatus.Succeeded)
				{
					Debug.LogError("Failed to load off screen indicator asset!");
				}
				else
				{
					indicatorAssetLoaded = true;
				}
			};
		}

		/// <summary>
		/// Destroys all indicators and releases the indicator load handle.
		/// </summary>
		public void DestroyIndicators()
		{
			for (int i = activeIndicators.Count - 1; i >= 0; i--)
			{
				Destroy(activeIndicators[i].gameObject);
			}
			
			activeIndicators.Clear();
			Addressables.Release(indicatorAssetLoaded);
		}
		
		#endregion

		#region Indicators

		/// <summary>
		/// Gets an indicator that matches passed target data. If such data doesn't exist, a new indicator is instantiated.
		/// </summary>
		/// <param name="targetData"></param>
		/// <returns>
		/// An <see cref="OffScreenTargetIndicator"/>.
		/// </returns>
		private OffScreenTargetIndicator GetIndicator(OffScreenTargetData targetData)
		{
			OffScreenTargetIndicator activeIndicator = GetActiveIndicatorByTarget(targetData);
			
			if (activeIndicator != null)
			{
				return activeIndicator;
			}
			
			GameObject indicatorGo = Instantiate(indicatorLoadHandle.Result, indicatorHolder);
			// Indicator will enable itself after the icon asset is loaded
			indicatorGo.SetActive(false);
			var indicator = indicatorGo.GetComponent<OffScreenTargetIndicator>();
			
			indicator.Enable(targetData, indicatorScaleUpTweenSettings);
			
			activeIndicators.Add(indicator);

			if (!indicatorsHaveIndividualTargetIcons)
			{
				indicator.DisableTargetIcon();
			}
			else
			{
				indicator.EnableTargetIcon();
			}
			
			return indicator;
		}

		/// <summary>
		/// Gets an active indicator that matches the passed target data.
		/// </summary>
		/// <param name="targetData">Target data.</param>
		/// <returns>
		/// An <see cref="OffScreenTargetIndicator"/> or null if no indicator matching the passed target data exists.
		/// </returns>
		private OffScreenTargetIndicator GetActiveIndicatorByTarget(OffScreenTargetData targetData)
		{
			IOffScreenTarget target = targetData.Target;

			foreach (OffScreenTargetIndicator activeIndicator in activeIndicators)
			{
				if (activeIndicator.Target != target)
				{
					continue;
				}

				return activeIndicator;
			}
				
			return null;
		}

		#endregion

		#region Toggle

		/// <summary>
		/// Enables this component.
		/// </summary>
		public virtual void Enable()
		{
			if (offScreenTargetDetector == null)
			{
				if (!OffScreenTargetDetector.TryGetDetectorWithName(detectorName, out OffScreenTargetDetector detector))
				{
					return;
				}

				offScreenTargetDetector = detector;
			}

			PauseManager.RegisterPausable(this);
			enabled = true;
		}

		/// <summary>
		/// Disables this component.
		/// </summary>
		public virtual void Disable()
		{
			PauseManager.UnregisterPausable(this);
			enabled = false;
		}

		#endregion
	}
}