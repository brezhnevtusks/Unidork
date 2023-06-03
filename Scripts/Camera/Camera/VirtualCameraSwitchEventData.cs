#if CINEMACHINE

using Cinemachine;
using Unidork.Events;
using UnityEngine;

namespace Unidork.CameraUtility
{
    /// <summary>
    /// Stores data about an event that should be raised by the <see cref="BaseVirtualCameraSwitcher"/>
    /// when Cinemachine switches from a specific camera to a specific camera.
    /// </summary>
    [System.Serializable]
    public class VirtualCameraSwitchEventData
    {
        #region Properties

        /// <summary>
        /// Virtual camera to transition from.
        /// </summary>
        /// <value>
        /// Gets the value of the field fromCamera.
        /// </value>
        public CinemachineVirtualCamera FromCamera  => fromCamera;

        /// <summary>
        /// Virtual camera to transition to.
        /// </summary>
        /// <value>
        /// Gets the value of the field toCamera.
        /// </value>
        public CinemachineVirtualCamera ToCamera => toCamera;

        /// <summary>
        /// Event to raise when Cinemachine switches to specified virtual camera.
        /// </summary>
        /// <value>
        /// Gets the value of the field eventToRaise.
        /// </value>
        public GameEvent EventToRaise => eventToRaise;

        /// <summary>
        /// When True, event is raised after the camera blend finishes, when False, it is raised immediately.
        /// </summary>
        /// <value>
        /// Gets the value of the field raiseEventAfterBlendFinishes.
        /// </value>
        public bool RaiseEventAfterBlendFinishes => raiseEventAfterBlendFinishes;

        #endregion

        #region Fields

        /// <summary>
        /// Virtual camera to transition from.
        /// </summary>
        [Tooltip(" Virtual camera to transition from.")]
        [SerializeField]
        private CinemachineVirtualCamera fromCamera = null;

        /// <summary>
        /// Virtual camera to transition to.
        /// </summary>
        [Tooltip(" Virtual camera to transition to.")]
        [SerializeField]
        private CinemachineVirtualCamera toCamera = null;

        /// <summary>
        /// Event to raise when Cinemachine switches to specified virtual camera.
        /// </summary>
        [Tooltip("Event to raise when Cinemachine switches to specified virtual camera.")]
        [SerializeField]
        private GameEvent eventToRaise = null;

        /// <summary>
        /// When True, event is raised after the camera blend finishes, when False, it is raised immediately.
        /// </summary>
        [Tooltip("When True, event is raised after the camera blend finishes, when False, it is raised immediately.")]
        [SerializeField]
        private bool raiseEventAfterBlendFinishes = true;		

		#endregion
	}
}

#endif