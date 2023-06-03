using Sirenix.OdinInspector;
using Unidork.Events;
using UniRx;
using UnityEngine;

namespace Unidork.Variables
{
	/// <summary>
	/// A scriptable object that holds a float value and raises an optional event when that value changes.
	/// </summary>
	[CreateAssetMenu(fileName = "Float Variable", menuName = "Scriptable Objects/Variables/Float Variable", order = 0)]
    public class FloatVariable : ScriptableObject
    {
        #region Properties

        /// <summary>
        /// Value of the float variable.
        /// </summary>
        /// <value>
        /// Gets and sets the value of the float field floatValue.
        /// </value>
        public float Value
        {
            get => currentValue;

            set
            {
                currentValue = Clamp(value);

                ReactiveValue.Value = currentValue;

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
        /// Gets and sets the value of the float field minValue.
        /// </value>
        public float MinValue
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
        /// Gets and sets the value of the float field maxValue.
        /// </value>
        public float MaxValue
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
		public float NormalizedValue => Mathf.Clamp01((currentValue - minValue) / (maxValue - minValue));

        /// <summary>
        /// UniRx reactive value of this object that other objects can subscribe to
        /// instead of using the game event listener flow.
        /// </summary>
        public ReactiveProperty<float> ReactiveValue { get; private set; }

        /// <summary>
        /// UniRx reactive min value of this object that other objects can subscribe to
        /// instead of using the game event listener flow.
        /// </summary>
        public ReactiveProperty<float> ReactiveMinValue { get; private set; }

        /// <summary>
        /// UniRx reactive max value of this object that other objects can subscribe to
        /// instead of using the game event listener flow.
        /// </summary>
        public ReactiveProperty<float> ReactiveMaxValue { get; private set; }

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
        [Tooltip("Minimum allowed value of this vari")]
        [SerializeField]
        private float minValue = 0f;

        /// <summary>
        /// Maximum allowed value of this variable.
        /// </summary>
        [Tooltip("Maximum allowed value of this variable.")]
        [SerializeField]
        private float maxValue = 0f;

        /// <summary>
        /// Current value of this variable.
        /// </summary>
        [ReadOnly]
        [SerializeField]
        private float currentValue = 0f;

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
        private float customStartValue = 0;

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

            ReactiveValue = new ReactiveProperty<float>(currentValue);
            ReactiveMinValue = new ReactiveProperty<float>(minValue);
            ReactiveMaxValue = new ReactiveProperty<float>(maxValue);
        }

        #endregion

        #region Value

        /// <summary>
        /// Sets value to the value of the passed <see cref="FloatVariable"/>.
        /// </summary>
        /// <param name="value">FloatVariable containing new value.</param>
        public void Set(FloatVariable value) => Value = Clamp(value.Value);

        /// <summary>
        /// Adds a float to current value.
        /// </summary>
        /// <param name="amount">Float value to add.</param>
        public void Add(float amount) => Value = Clamp(Value += amount);

        /// <summary>
        /// Adds the value of the passed <see cref="FloatVariable"/>"/> to current value.
        /// </summary>
        /// <param name="amount">FloatVariable whose value needs to be added.</param>
        public void Add(FloatVariable amount) => Value = Clamp(Value += amount.Value);

        /// <summary>
        /// Clamps the value between <see cref="MinValue"/> and <see cref="MaxValue"/>. If both are 0, returns the passed value.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <returns>
        /// Passed value clamped between min and max or the value itself if no clapming is specified.
        /// </returns>
        private float Clamp(float value)
        {
            return (MinValue == 0f && MaxValue == 0f) ? value : Mathf.Clamp(value, MinValue, MaxValue);
        }

        /// <summary>
        /// Gets current and maximum value of a float variable.
        /// </summary>
        /// <returns>
        /// A tuple consisting of variable's current and max value.
        /// </returns>        
        public (float, float) GetCurrentAndMax() => (currentValue, maxValue);

        #endregion

        #region ToString

        /// <summary>
        /// Returns current value as a string based on passed formatting options.
        /// </summary>
        /// <param name="formattingOptions">Formatting options.</param>
        /// <returns>
        /// A string representation of current value.
        /// </returns>
        public string CurrentToString(FloatVariableFormattingOptions formattingOptions)
        {
            return ToString(currentValue, formattingOptions);
        }

        /// <summary>
        /// Returns current max value as a string based on passed formatting options.
        /// </summary>
        /// <param name="formattingOptions">Formatting options.</param>
        /// <returns>
        /// A string representation of max value.
        /// </returns>
        public string MaxToString(FloatVariableFormattingOptions formattingOptions)
        {
            return ToString(maxValue, formattingOptions);
        }

        /// <summary>
        /// Returns current min value as a string based on passed formatting options.
        /// </summary>
        /// <param name="formattingOptions">Formatting options.</param>
        /// <returns>
        /// A string representation of min value.
        /// </returns>
        public string MinToString(FloatVariableFormattingOptions formattingOptions)
        {
            return ToString(minValue, formattingOptions);
        }

        /// <summary>
        /// Returns a passed value as a string based on passed formatting options.
        /// </summary>
        /// <param name="valueToReturn">Value to return.</param>
        /// <param name="formattingOptions">Formatting options.</param>
        /// <returns>
        /// A string representation of the passed value.
        /// </returns>
        private string ToString(float valueToReturn, FloatVariableFormattingOptions formattingOptions)
        {
            switch (formattingOptions)
            {
                case FloatVariableFormattingOptions.WholePath:
                    return valueToReturn.ToString("F0");
                case FloatVariableFormattingOptions.OneDecimal:
                    return valueToReturn.ToString("F1");
                case FloatVariableFormattingOptions.TwoDecimals:
                    return valueToReturn.ToString("F2");
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
