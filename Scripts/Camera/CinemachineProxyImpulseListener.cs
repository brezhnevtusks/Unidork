using Cinemachine;
using UnityEngine;

namespace Unidork.CameraUtility
{
    /// <summary>
    /// Component that changes the behavior of CinemachineImpulseListener and allows to attach it to calculate impulse
    /// at a position of a specified transform instead of transform that the camera is attached to 
    /// </summary>
    public class CinemachineProxyImpulseListener : CinemachineImpulseListener
    {
        #region Fields

        /// <summary>
        /// Reference to the transform whose position is used for actual impulse listening.
        /// </summary>
        [SerializeField] protected Transform impulseListenerTransform;

        #endregion

        #region Set

        /// <summary>
        /// Sets the impulseListenerTransform to the passed value.
        /// </summary>
        /// <param name="impulseListenerTransform">Impulse listener transform.</param>
        public void SetImpulseListenerTransform(Transform impulseListenerTransform) => this.impulseListenerTransform = impulseListenerTransform;

        #endregion

        #region Cinemachine

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == m_ApplyAfter && deltaTime >= 0)
            {
                if (impulseListenerTransform == null)
                {
                    Debug.LogError("Impulse listener transform is null!");
                    return;
                }
                
                bool haveImpulse = CinemachineImpulseManager.Instance.GetImpulseAt(impulseListenerTransform.position, m_Use2DDistance, m_ChannelMask, 
                    out var impulsePos, out var impulseRot);
                
                bool haveReaction = m_ReactionSettings.GetReaction(deltaTime, impulsePos, out var reactionPos, out var reactionRot);

                if (haveImpulse)
                {
                    impulseRot = Quaternion.SlerpUnclamped(Quaternion.identity, impulseRot, m_Gain);
                    impulsePos *= m_Gain;
                }
                if (haveReaction)
                {
                    impulsePos += reactionPos;
                    impulseRot *= reactionRot;
                }
                if (haveImpulse || haveReaction)
                {
                    if (m_UseCameraSpace)
                    {
                        impulsePos = state.RawOrientation * impulsePos;
                    }
                        
                    state.PositionCorrection += impulsePos;
                    state.OrientationCorrection *= impulseRot;
                }
            }
        }

        #endregion
        
    }
}