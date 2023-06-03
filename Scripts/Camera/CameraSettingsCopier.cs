using Unidork.Attributes;
using Unidork.Extensions;
using UnityEngine;

namespace Unidork.CameraUtility
{
	/// <summary>
	/// Copies settings like position, rotation and FOX from one Unity physics camera to other cameras.
	/// </summary>
	public class CameraSettingsCopier : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Camera to copy settings from.
        /// </summary>
        [Tooltip("Camera to copy settings from.")]
        [Space, SettingsHeader, Space]
        [SerializeField]
        private UnityEngine.Camera fromCamera = null;

        /// <summary>
        /// Cameras to copy settings to.
        /// </summary>
        [Tooltip("Cameras to copy settings to.")]
        [SerializeField]
        private UnityEngine.Camera[] toCameras = null;

        /// <summary>
        /// Should camera's position be copied?
        /// </summary>
        [Tooltip("Should camera's position be copied?")]
        [SerializeField]
        private bool copyPosition = false;

        /// <summary>
        /// Should camera's rotation be copied?
        /// </summary>
        [Tooltip("Should camera's rotation be copied?")]
        [SerializeField]
        private bool copyRotation = false;

        /// <summary>
        /// Should camera's field of view be copied?
        /// </summary>
        [Tooltip("Should camera's field of view be copied?")]
        [SerializeField]
        private bool copyFieldOfView = false;

		#endregion

		#region Init

		private void Start()
		{
			if (fromCamera == null)
			{
                Debug.LogError("Camera Settings Copier doesn't have a camera to copy settings from!", this);
                enabled = false;
                return;
			}

            if (toCameras.IsNullOrEmpty())
			{
                Debug.LogError("Camera Settings Copier doesn't have cameras to copy settings to!", this);
                enabled = false;
			}
		}

		#endregion

		#region Update

		private void Update()
		{
            foreach (UnityEngine.Camera toCamera in toCameras)
			{
                if (copyPosition)
				{
                    toCamera.transform.position = fromCamera.transform.position;
				}

                if (copyRotation)
				{
                    toCamera.transform.rotation = fromCamera.transform.rotation;
				}

                if (copyFieldOfView)
				{
                    toCamera.fieldOfView = fromCamera.fieldOfView;
;				}
			}
		}

		#endregion
	}
}