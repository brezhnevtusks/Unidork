using Sirenix.OdinInspector;
using Unidork.Events;
using UniRx;
using UnityEngine;

namespace Unidork.Variables
{
    /// <summary>
    /// A scriptable object that holds an integer value and raises an optional event when that value changes.
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "Int Variable", menuName = "Scriptable Objects/Variables/Int Variable", order = 1)]
    public class IntVariable : ScriptableObject
    {
        #region Properties

        /// <summary>
        /// Value of the integer variable.
        /// </summary>
        /// <value>
        /// Gets and sets the value of the integer field intValue. Raises an event if one is assigned in the <see cref="eventToRaise"/> field.
        /// </value>
        public int Value
        {
            get => currentValue;

            set
            {
                currentValue = Clamp(value);

                ReactiveValue.SetValueAndForceNotify(currentValue);

                if (eventToRaiseOnCurrentValueChange == null)
				{
                    return;
				}

                eventToRaiseOnCurrentValueChange.Raise();
            }
        }       

        /// <summary>
        /// Minimum allowed value of this variable.
        /// </summary>
        /// <value>
        /// Gets and sets the value of the int field minValue.
        /// </value>
        public int MinValue
        {
            get => minValue;

            set
            {
                minValue = value;

                ReactiveMinValue.SetValueAndForceNotify(minValue);

                if (eventToRaiseOnMinValueChange == null)
				{
                    return;
				}

				eventToRaiseOnMinValueChange.Raise();
            }
        }

        /// <summary>
        /// Maximum allowed value of this variable.
        /// </summary>
        /// <value>
        /// Gets and sets the value of the int field maxValue.
        /// </value>
        public int MaxValue
        {
            get => maxValue;

            set
            {
                maxValue = value;

                ReactiveMaxValue.SetValueAndForceNotify(maxValue);

                if (eventToRaiseOnMaxValueChange == null)
                {
                    return;
                }

                eventToRaiseOnMaxValueChange.Raise();
            }
        }        

        /// <summary>
        /// Has this variable reached its minimum value?
        /// </summary>        
        /// <value>
        /// Gets a boolean result of comparing field currentValue and minValue.
        /// </value>
        public bool IsAtMinValue => currentValue == minValue;

        /// <summary>
        /// Has this variable reached its maximum value?
        /// </summary>        
        /// <value>
        /// Gets a boolean result of comparing field currentValue and maxValue.
        /// </value>
        public bool IsAtMaxValue => currentValue == maxValue;

        /// <summary>
		/// Gets the value of this variable normalized between min and max.
		/// </summary>
		/// <value>
		/// A float in the range [0..1].
		/// </value>
		public float NormalizedValue => Mathf.Clamp01((currentValue - minValue) / (float)(maxValue - minValue));

        /// <summary>
        /// UniRx reactive value of this object that other objects can subscribe to
        /// instead of using the game event listener flow.
        /// </summary>
        public ReactiveProperty<int> ReactiveValue { get; private set; }

        /// <summary>
        /// UniRx reactive min value of this object that other objects can subscribe to
        /// instead of using the game event listener flow.
        /// </summary>
        public ReactiveProperty<int> ReactiveMinValue { get; private set; }

        /// <summary>
        /// UniRx reactive max value of this object that other objects can subscribe to
        /// instead of using the game event listener flow.
        /// </summary>
        public ReactiveProperty<int> ReactiveMaxValue { get; private set; }        

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
        /// Minimum allowed value of this variable.
        /// </summary>
        [Title("Values")]
        [Tooltip("Minimum allowed value of this variable.")]
        [SerializeField]
        private int minValue = 0;

        /// <summary>
        /// Maximum allowed value of this variable.
        /// </summary>
        [Tooltip("Maximum allowed value of this variable.")]
        [SerializeField]
        private int maxValue = 0;

        /// <summary>
        /// Current value of this variable.
        /// </summary>
        [ReadOnly]
        [SerializeField]
        private int currentValue;

        /// <summary>
        /// Start value type for this variable.
        /// </summary>
        [Tooltip("Start value type for this variable.")]
        [SerializeField, EnumPaging]
        private VariableStartValueType variableStartValueType = VariableStartValueType.Default;

        /// <summary>
        /// Custom start value to use if <see cref="variableStartValueType"/> is set to <see cref="VariableStartValueType.Custom"/>.
        /// </summary>
        [Tooltip("Custom start value to use if variableStartValueTypeis set to VariableStartValueType.Custom")]
        [SerializeField, HideIf("@this.variableStartValueType != VariableStartValueType.Custom")]
        private int customStartValue = 0;

        /// <summary>
        /// Optional event to raise when this variable's current value changes.
        /// </summary>
        [Title("Events")]
        [Tooltip("Optional event to raise when this variable's value changes.")]
        [SerializeField]
        private GameEvent eventToRaiseOnCurrentValueChange = null;

        /// <summary>
        /// Optional event to raise when this variable's minimum value changes.
        /// </summary>
        [Tooltip("Optional event to raise when this variable's minimum value changes.")]
        [SerializeField]
        private GameEvent eventToRaiseOnMinValueChange = null;

        /// <summary>
        /// Optional event to raise when this variable's maximum value changes.
        /// </summary>
        [Tooltip("Optional event to raise when this variable's maximum value changes.")]
        [SerializeField]
        private GameEvent eventToRaiseOnMaxValueChange = null;

        #endregion

        #region Init

        private void OnEnable()
        {
            switch (variableStartValueType)
            {
                case VariableStartValueType.Default:
                    currentValue = default;
                    break;
                case VariableStartValueType.Min:
                    currentValue = minValue;
                    break;
                case VariableStartValueType.Max:
                    currentValue = maxValue;
                    break;
                case VariableStartValueType.Custom:
                    currentValue = customStartValue;
                    break;
            }

            ReactiveValue = new ReactiveProperty<int>(currentValue);
            ReactiveMinValue = new ReactiveProperty<int>(minValue);
            ReactiveMaxValue = new ReactiveProperty<int>(maxValue);
        }

        #endregion

        #region Value

        /// <summary>
        /// Sets value to the value of the passed <see cref="IntVariable"/>.
        /// </summary>
        /// <param name="value">IntVariable containing new value.</param>
        public void Set(IntVariable value) => Value = value.Value;

        /// <summary>
        /// Adds an integer to current value.
        /// </summary>
        /// <param name="amount">Integer value to add.</param>
        public void Add(int amount) => Value += amount;

        /// <summary>
        /// Adds the value of the passed <see cref="IntVariable"/> to current value.
        /// </summary>
        /// <param name="amount">IntVariable whose value needs to be added.</param>
        public void Add(IntVariable amount) => Value += amount.Value;

        /// <summary>
        /// Clamps the value between <see cref="minValue"/> and <see cref="maxValue"/>. If both are 0, returns the passed value.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <returns>A integer.</returns>
        private int Clamp(int value)
        {
            return (minValue == 0 && maxValue == 0) ? value : Mathf.Clamp(value, minValue, maxValue);
        }

        #endregion
    }
}
