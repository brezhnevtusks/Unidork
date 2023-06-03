using System.Threading;
using Cysharp.Threading.Tasks;
using Unidork.AddressableAssetsUtility;
using Unidork.Attributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Unidork.OffScreenTargets
{
	public class OffScreenTargetIndicator : MonoBehaviour
	{
		#region Properties

		/// <summary>
		/// Target associated with this indicator.
		/// </summary>
		public IOffScreenTarget Target => targetData.Target;

		#endregion
		
		#region Fields

		/// <summary>
		/// Image that represents an individual target.
		/// </summary>
		[Space, ComponentsHeader, Space] 
		[Tooltip("Image that represents an individual target.")]
		[SerializeField]
		private Image targetIcon = null;

		/// <summary>
		/// Data about target associated with this indicator.
		/// </summary>
		private OffScreenTargetData targetData;

		/// <summary>
		/// Cancellation token source used to cancel the async enable of the indicator if it gets removed from the screen before the icon is loaded.
		/// </summary>
		private CancellationTokenSource cancellationTokenSource;

		#endregion

		#region Setup

		/// <summary>
		/// Sets the off screen target data associated with this indicator and initiates the async enable.
		/// </summary>
		/// <param name="targetData">Target data.</param>
		public void Enable(OffScreenTargetData targetData)
		{
			this.targetData = targetData;

			if (targetIcon == null)
			{
				return;
			}

			cancellationTokenSource = new CancellationTokenSource();
			EnableAsync(cancellationTokenSource.Token).Forget();
		}

		private async UniTaskVoid EnableAsync(CancellationToken cancellationToken)
		{
			AssetReference iconAssetReference = await targetData.Target.GetIconAssetReference(cancellationToken);

			if (!cancellationToken.CanBeCanceled || cancellationToken.IsCancellationRequested)
			{
				return;
			}

			AddressableLoadOperationResult<Sprite> loadResult = 
				await AddressablesManager.LoadAssetByReferenceAsyncWithTask<Sprite>(iconAssetReference, cancellationToken: cancellationToken);
			
			if (!cancellationToken.CanBeCanceled || cancellationToken.IsCancellationRequested)
			{
				return;
			}

			if (!loadResult.Succeeded)
			{
				Debug.LogError("Failed to load off-screen target icon!");
				return;
			}
			
			targetIcon.sprite = loadResult.Value;
			gameObject.SetActive(true);
		}

		#endregion

		#region Position

		/// <summary>
		/// Positions and rotates the indicator to match the passed data. Keeps individual target icon, if one is active, at zero rotation.
		/// </summary>
		/// <param name="screenPosition">Indicator screen position.</param>
		/// <param name="rotationAngle">Indicator rotation angle.</param>
		public void SetPositionAndRotation(Vector3 screenPosition, float rotationAngle)
		{
			transform.position = screenPosition;
			transform.rotation = Quaternion.Euler(0, 0, rotationAngle * Mathf.Rad2Deg);
			
			if (targetIcon == null)
			{
				return;
			}
			
			targetIcon.transform.rotation = Quaternion.identity;
		}

		#endregion

		#region Icon

		/// <summary>
		/// Enables the icon that represents the type of an individual target.
		/// </summary>
		public void EnableTargetIcon() => targetIcon.enabled = true;

		/// <summary>
		/// Disables the icon that represents the type of an individual target.
		/// </summary>
		public void DisableTargetIcon()
		{
			if (targetIcon == null)
			{
				return;
			}
			
			targetIcon.enabled = false;
		}

		#endregion

		#region Destroy

		private void OnDestroy()
		{
			cancellationTokenSource?.Dispose();
		}

		#endregion
	}
}