#if CINEMACHINE

using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Unidork.Attributes;
using Unity.Cinemachine;
using UnityEngine;

namespace Unidork.CameraUtility
{
	/// <summary>
	/// Scales the horizontal field of view of a camera based on camera's aspect ratio.
	/// </summary>
	public class CameraFovScaler : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Fixed horizontal field of view we want to keep.
        /// </summary>
        [Space, SettingsHeader, Space]
        [Tooltip("Fixed horizontal field of view we want to keep.")]
        [SerializeField]
        private float fixedHorizontalFov = 62.48343f;

#if UNITY_EDITOR

        [Button("Grab Horizontal FOV")]
        [UsedImplicitly]
        private void GrabCurrentFov()
		{
            var mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

            var virtualCamera = GetComponent<CinemachineCamera>();

            if (virtualCamera != null)
            {
                fixedHorizontalFov = GetHorizontalFieldOfView(mainCamera.aspect, virtualCamera.Lens.FieldOfView);
                return;
            }

            var camera = GetComponent<Camera>();

            if (camera != null)
            {
                fixedHorizontalFov = GetHorizontalFieldOfView(mainCamera.aspect, camera.fieldOfView);
                return;
            }

            Debug.LogError($"There is a CameraFovScaler attached to {gameObject.name} " +
                           $"but no physical or virtual camera!");
        }

#endif

        #endregion

        #region Init

        private void Start()
		{
            var virtualCamera = GetComponent<CinemachineCamera>();

            if (virtualCamera != null)
            {
                ScaleVerticalFovBasedOnFixedHorizontal(virtualCamera);
                return;
            }

            var camera = GetComponent<Camera>();

            if (camera != null)
			{
                ScaleVerticalFovBasedOnFixedHorizontal(camera);
                return;
			}

            Debug.LogError($"There is a CameraFovScaler attached to {gameObject.name} " +
                           $"but no physical or virtual camera!");
		}

		#endregion

		#region FOV

        /// <summary>
        /// Scales the camera vertical field of view to retain the fixed horizontal field of view value.
        /// </summary>
        /// <param name="camera">Physical camera.</param>
        private void ScaleVerticalFovBasedOnFixedHorizontal(Camera camera)
		{
            var mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            float newFieldOfView = CalculateVerticalFovBasedOnFixedHorizontal(mainCamera.aspect);
            camera.fieldOfView = newFieldOfView;
		}

        /// <summary>
        /// Scales the camera vertical field of view to retain the fixed horizontal field of view value.
        /// </summary>
        /// <param name="camera">Virtual camera.</param>
        private void ScaleVerticalFovBasedOnFixedHorizontal(CinemachineCamera camera)
		{
            var mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            float newFieldOfView = CalculateVerticalFovBasedOnFixedHorizontal(mainCamera.aspect);
            camera.Lens.FieldOfView = newFieldOfView;
		}

        /// <summary>
        /// Calculates camera's 
        /// </summary>
        /// <param name="aspectRatio">Camera's aspect ratio.</param>
        /// <returns>
        /// A float.
        /// </returns>
        private float CalculateVerticalFovBasedOnFixedHorizontal(float aspectRatio)
        {
            return 2 * Mathf.Atan(Mathf.Tan(fixedHorizontalFov * Mathf.Deg2Rad * 0.5f) / aspectRatio) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Gets the horizontal field of view based on the value of the vertical field of view.
        /// </summary>
        /// <param name="verticalFov">Vertical field of view.</param>
        /// <returns>
        /// A float.
        /// </returns>
        private float GetHorizontalFieldOfView(float aspectRatio, float verticalFov)
		{
            float verticalFovRadians = verticalFov * Mathf.Deg2Rad;
            float cameraHeightAt1 = Mathf.Tan(verticalFovRadians * .5f);
            return Mathf.Atan(cameraHeightAt1 * aspectRatio) * 2f * Mathf.Rad2Deg;
        }
    }

	#endregion	
}

#endif