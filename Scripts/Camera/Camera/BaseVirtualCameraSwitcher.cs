#if CINEMACHINE

using Cinemachine;
using System.Collections;
using Sirenix.OdinInspector;
using Unidork.Events;
using UnityEngine;

namespace Unidork.CameraUtility
{
	/// <summary>
	/// Switches between Cinemachine's virtual cameras in response to in-game events.
	/// </summary>
	public class BaseVirtualCameraSwitcher : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Array of object storing data about events that should be raised when switching to specific virtual cameras.
		/// </summary>	
		[Space, Title("CAMERA SETTINGS", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]			
		[SerializeField]
		private VirtualCameraSwitchEventData[] cameraSwitchEventData = null;
		
		/// <summary>
		/// Cinemachine component that drives virtual camera behavior.
		/// </summary>
		private CinemachineBrain cinemachineBrain;

		#endregion

		#region Constants

		/// <summary>
		/// Priority value to set on an active camera.
		/// </summary>
		private const int ActiveCameraPriority = 100;

		/// <summary>
		/// Priority value to set on an inactive camera.
		/// </summary>
		private const int InactiveCameraPriority = 1;

		#endregion

		#region Init

		private void Awake() => cinemachineBrain = FindObjectOfType<CinemachineBrain>();

		#endregion

		#region Switch

		/// <summary>
		/// Makes the passed Cinemachine virtual camera active.
		/// </summary>
		/// <param name="newActiveCamera">New active camera.</param>
		public void SwitchToCamera(CinemachineVirtualCamera newActiveCamera)
		{
			var previousCamera = (CinemachineVirtualCamera)cinemachineBrain.ActiveVirtualCamera;

			if (previousCamera != null)
			{
				previousCamera.Priority = InactiveCameraPriority;
			}

			newActiveCamera.Priority = ActiveCameraPriority;

			VirtualCameraSwitchEventData cameraSwitchEventData = GetSwitchEventData(previousCamera, newActiveCamera);

			if (cameraSwitchEventData == null)
			{
				return;
			}			

			if (cameraSwitchEventData.RaiseEventAfterBlendFinishes)
			{
				StopAllCoroutines();
				_ = StartCoroutine(RaiseEventAfterCameraBlendsFinishes(cameraSwitchEventData.FromCamera, cameraSwitchEventData.EventToRaise));
			}
			else
			{
				cameraSwitchEventData.EventToRaise.Raise();
			}
		}

		/// <summary>
		/// Gets the camera switch event data for the new virtual camera.
		/// </summary>
		/// <param name="previousCamera">Camera that switcher switched from</param>
		/// <param name="newCamera">Camera that switcher just switched to.</param>
		/// <returns>
		/// An instance of <see cref="VirtualCameraSwitchEventData"/> from the <see cref="cameraSwitchEventData"/>
		/// or null if no members match the passed virtual cameras.
		/// </returns>
		private VirtualCameraSwitchEventData GetSwitchEventData(CinemachineVirtualCamera previousCamera, CinemachineVirtualCamera newCamera)
		{
			foreach (VirtualCameraSwitchEventData data in cameraSwitchEventData)
			{
				if (data.FromCamera == previousCamera && data.ToCamera == newCamera)
				{
					return data;
				}
			}

			return null;
		} 

		#endregion
		
		#region Events
		
		/// <summary>
		/// Raises the passed event after the passed virtual camera finishes blending.
		/// </summary>
		/// <param name="camera">Virtual camera.</param>
		/// <param name="eventToRaise">Event to raise.</param>
		/// <returns>
		/// An IEnumerator.
		/// </returns>
		private IEnumerator RaiseEventAfterCameraBlendsFinishes(CinemachineVirtualCamera camera, GameEvent eventToRaise)
		{
			yield return new WaitWhile(() => CinemachineCore.Instance.IsLive(camera));
			eventToRaise.Raise();
		}

		#endregion
	}
}

#endif