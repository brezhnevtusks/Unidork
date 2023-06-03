using UnityEngine;

namespace Unidork.StateMachine
{
    /// <summary>
    /// State that doesn't have entry/exit callbacks but can still have actions
    /// and transitions assigned to it in the inspector.
    /// </summary>
    [CreateAssetMenu(fileName = "GenericState", menuName = "State Machine/Common/Generic State")]
    public class FSMState_Generic : FSMState
    {        
    }
}