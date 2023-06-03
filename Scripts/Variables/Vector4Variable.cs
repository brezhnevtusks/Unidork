using Sirenix.OdinInspector;
using Unidork.Events;
using UnityEngine;

namespace Unidork.Variables
{
    /// <summary>
    /// A scriptable object that holds a Vector4 value.
    /// </summary>
    [CreateAssetMenu(fileName = "Vector4Variable", menuName = "Scriptable Objects/Variables/Vector4 Variable", order = 4)]
    public class Vector4Variable : ScriptableObject
    {
        #region Properties

        /// <summary>
        /// Value of the Vector4 variable.
        /// </summary>
        /// <value>
        /// Gets and sets the value of the Vector4 field currentValue. Raises an event if one is assigned in the <see cref="eventToRaise"/> field.
        /// </value>
        public Vector4 Value
        {
            get => currentValue;

            set
            {
                currentValue = value;

                if (eventToRaise != null)
                {
                    eventToRaise.Raise();
                }
            }
        }

        #endregion

        #region Fields

#if UNITY_EDITOR
        /// <summary>
        /// Optional description to view in the inspector.
        /// </summary>
        [Multiline]
        [SerializeField]
#pragma warning disable CS0414
        private string description = "";
#pragma warning restore CS0414
#endif

        /// <summary>
        /// Optional event to raise when this variable's value changes.
        /// </summary>        
        [Tooltip("Event to raise when this variable's value changes.")]
        [SerializeField]
        private GameEvent eventToRaise = null;

        /// <summary>
        /// Current value of this variable.
        /// </summary>
        [ReadOnly]
        [SerializeField]
        private Vector4 currentValue;

        #endregion
    }
}
