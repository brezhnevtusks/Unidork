using Sirenix.OdinInspector;
using UnityEngine;

namespace Unidork.WeightedSelection
{
    /// <summary>
    /// Base class for objects storing data about items selected via weighted random.
    /// </summary>
    /// <typeparam name="TObject">Object.</typeparam>
    [System.Serializable]
    public class WeightedObject<TObject>
    {
        #region Properties

        /// <summary>
        /// Selected object or value.
        /// </summary>
        /// <value>
        /// Gets the value of the field @object.
        /// </value>
        public TObject Object => @object;

        /// <summary>
        /// Weight of this object for the purposes of random selection.
        /// </summary>
        /// <value>
        /// Gets and sets the value of the float field weight.
        /// </value>
        public float Weight { get => weight; set => weight = value; }

        /// <summary>
        /// Low bound of this object's weight range.
        /// </summary>
        public float RangeFrom { get; set; }

        /// <summary>
        /// High bound of this object's weight range.
        /// </summary>
        public float RangeTo { get; set; }

        /// <summary>
        /// Chance of this object to be selected randomly.
        /// </summary>
        /// <value>
        /// Gets and sets the value of the float field selectionChance.
        /// </value>
        public float SelectionChance { get => selectionChance; set => selectionChance = value; }

        #endregion

        #region Fields

        /// <summary>
        /// Selected object or value.
        /// </summary>
        [PropertyOrder(2)]
        [Tooltip("Selected object or value.")]
        [SerializeField]
        protected TObject @object;
        
        /// <summary>
        /// Weight of this object or value for the purposes of random selection.
        /// </summary>
        [PropertyOrder(3)]
        [Tooltip("Weight of this object or value for the purposes of random selection.")]
        [SerializeField]
        private float weight = 1f;

        /// <summary>
        /// Chance of this object or value to be selected randomly.
        /// </summary>
        [PropertyOrder(4)]
        [SerializeField, ReadOnly]
        private float selectionChance;
        
        #endregion
        
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value stored in this object.</param>
        public WeightedObject(TObject value)
        {
            @object = value;
        }
        
        #endregion
    }
}