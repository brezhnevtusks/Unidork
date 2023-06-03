using Sirenix.OdinInspector;
using Unidork.Events;
using UniRx;
using UnityEngine;

namespace Unidork.Variables
{
    /// <summary>
    /// A scriptable object that holds a boolean value and raises an optional event depending on current value.
    /// </summary>
    [CreateAssetMenu(fileName = "Bool Variable", menuName = "Scriptable Objects/Variables/Bool Variable", order = 0)]    
    public class BoolVariable : ScriptableObject
    {
        #region Properties

        /// <summary>
        /// Value of the boolean variable.
        /// </summary>
        /// <value>
        /// Gets and sets the value of the boolean field currentValue. Raises an event if new value does not match the old one.
        /// </value>
        public bool Value
        {
            get => currentValue;

            set
            {
                var previousValue = currentValue;

                currentValue = value;

                if (onlyRaiseWhenNewValueIsDifferent)
                {
                    ReactiveValue.Value = currentValue;
                }
                else
                {
                    ReactiveValue.SetValueAndForceNotify(currentValue);
                }

                if (!onlyRaiseWhenNewValueIsDifferent || (onlyRaiseWhenNewValueIsDifferent && previousValue != currentValue))
                {
                    if (currentValue)
                    {
                        if (eventWhenTrue != null)
                        {
                            eventWhenTrue.Raise();
                        }
                    }
                    else
                    {
                        if (eventWhenFalse != null)
                        {
                            eventWhenFalse.Raise();
                        }
                    }
                }             
            }
        }

        /// <summary>
        /// Should this variable be true on start?
        /// </summary>
        /// <value>
        /// Gets and sets the value of the boolean field trueOnStart.
        /// </value>
        public bool TrueOnStart { get => trueOnStart; set => trueOnStart = value; }

        /// <summary>
        /// UniRx reactive value of this object that other objects can subscribe to
        /// instead of using the game event listener flow.
        /// </summary>
        public ReactiveProperty<bool> ReactiveValue { get; private set; }

        #endregion

        #region Fields

#if UNITY_EDITOR
        /// <summary>
        /// Optional description to view in the inspector.
        /// </summary>
        [Multiline]
        [SerializeField]
#pragma warning disable CS0414
        private string Description = "";
#pragma warning restore CS0414
#endif

        /// <summary>
        /// Optional event to raise when this variable's value becomes true.
        /// </summary>
        [Tooltip("Optional event to raise when this variable's value becomes true.")]
        [SerializeField]
        private GameEvent eventWhenTrue = null;

        /// <summary>
        /// Optional event to raise when this variable's value becomes false.
        /// </summary>
        [Tooltip("Optional event to raise when this variable's value becomes false.")]
        [SerializeField]
        private GameEvent eventWhenFalse = null;

        /// <summary>
        /// Should the connected events be raised only when the new
        /// value is different from the previos one or on each assignment?
        /// </summary>
        [Tooltip("Should the connected events be raised only when the new" +
                 "value is different from the previos one or on each assignment?")]
        [SerializeField]
        private bool onlyRaiseWhenNewValueIsDifferent = false;

        /// <summary>
        /// Should this variable be true on start?
        /// </summary>
        [Tooltip("Should this variable be true on start?")]
        [SerializeField]
        private bool trueOnStart = false;

#if UNITY_EDITOR
        [ReadOnly]
        [SerializeField]
#endif
        private bool currentValue;

        #endregion

        #region Set

        /// <summary>
        /// Sets the boolean value to the value of the passed BoolVariable.
        /// </summary>
        /// <param name="value">Boolean variable containing new value.</param>
        public void Set(BoolVariable value) => Value = value.Value;

        /// <summary>
        /// Changes value to the opposite.
        /// </summary>
        [Button("Toggle", ButtonSizes.Large), GUIColor(0f, 1f, 0f)]
        public void Toggle() => Value = !currentValue;

        #endregion

        #region Init

        private void OnEnable()
        {
            currentValue = trueOnStart;
            ReactiveValue = new ReactiveProperty<bool>(currentValue);
        }

        #endregion
    }
}