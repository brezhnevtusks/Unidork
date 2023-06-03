using System;
using UniRx;
using UnityEngine;

namespace Unidork.UniRx
{
    /// <summary>
    /// Reactive property that keeps tracks of its value before latest change.
    /// </summary>
    /// <typeparam name="T">Type of property.</typeparam>
    [Serializable]
    public class ReactivePropertyWithOldValue<T> : ReactiveProperty<T>
    {
        #region Properties

        /// <summary>
        /// Value of the reactive property before the latest change of value> field.
        /// </summary>
        public T OldValue { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ReactivePropertyWithOldValue() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        public ReactivePropertyWithOldValue(T initialValue) : base(initialValue) { OldValue = initialValue; }

        #endregion

        #region Value

        /// <summary>
        /// Sets the value of the property. Stores old value in <see cref="OldValue"/>.
        /// </summary>
        /// <param name="newValue">New value.</param>
        protected override void SetValue(T newValue)
        {
            OldValue = Value;
            base.SetValue(newValue);
        }

        #endregion
    }
}