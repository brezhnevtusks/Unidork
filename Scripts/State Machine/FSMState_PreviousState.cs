using UnityEngine;

namespace Unidork.StateMachine
{
    /// <summary>
    /// Dummy state used to tell the state controller it needs to transition to the previous state.
    /// </summary>
    /// <remarks>Make sure you don't rename the scriptable object!</remarks>
    [CreateAssetMenu(fileName = "PreviousState", menuName = "State Machine/Common/Previous State")]
    public class FSMState_PreviousState : FSMState
    {
        #region Init

        private void OnEnable()
        {
            stateName = "PreviousState";
        }

        #endregion
    }
}