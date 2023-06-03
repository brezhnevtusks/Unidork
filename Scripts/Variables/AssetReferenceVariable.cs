using Sirenix.OdinInspector;
using Unidork.Events;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.Variables
{
    /// <summary>
    /// A scriptable object that holds an asset reference and raises an optional event depending on current value.
    /// </summary>
    [CreateAssetMenu(fileName = "Asset Reference Variable", menuName = "Scriptable Objects/Variables/Asset Reference Variable",
        order = 6)]
    public class AssetReferenceVariable : ScriptableObject
    {
        #region Properties

        /// <summary>
        /// Value of the asset reference variable.
        /// </summary>
        /// <value>
        /// Gets and sets the value of the asset reference field currentValue. Raises an event if new value does not match the old one.
        /// </value>
        public AssetReference Value
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
                    if (currentValue != null)
                    {
                        if (eventWhenNotNull != null)
                        {
                            eventWhenNotNull.Raise();
                        }
                    }
                    else
                    {
                        if (eventWhenNull != null)
                        {
                            eventWhenNull.Raise();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// UniRx reactive value of this object that other objects can subscribe to
        /// instead of using the game event listener flow.
        /// </summary>
        public ReactiveProperty<AssetReference> ReactiveValue { get; private set; }

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
        /// Optional event to raise when this variable's value becomes not null.
        /// </summary>
        [Tooltip("Optional event to raise when this variable's value becomes not null.")]
        [SerializeField]
        private GameEvent eventWhenNotNull = null;

        /// <summary>
        /// Optional event to raise when this variable's value becomes null.
        /// </summary>
        [Tooltip("Optional event to raise when this variable's value becomes null.")]
        [SerializeField]
        private GameEvent eventWhenNull = null;

        /// <summary>
        /// Should the connected events be raised only when the new
        /// value is different from the previos one or on each assignment?
        /// </summary>
        [Tooltip("Should the connected events be raised only when the new" +
                 "value is different from the previos one or on each assignment?")]
        [SerializeField]
        private bool onlyRaiseWhenNewValueIsDifferent = false;

#if UNITY_EDITOR
        [ReadOnly]
        [SerializeField]
#endif
        private AssetReference currentValue;

        #endregion

        #region Init

        private void OnEnable()
        {
            ReactiveValue = new ReactiveProperty<AssetReference>(currentValue);
        }

        #endregion
    }
}