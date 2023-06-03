using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unidork.WeightedSelection
{
    /// <summary>
    /// Base class for objects storing data about items selected via weighted random.
    /// </summary>
    [Serializable]
    public class WeightedObjectBase
    {
        #region Properties
        
        /// <summary>
        /// Selected object or value.
        /// </summary>
        /// <value>
        /// Gets the value of the field @object.
        /// </value>
        public object Object { get => @object; set => @object = value; }

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
        /// Selected bject or value.
        /// </summary>
        [SerializeField]
        protected object @object;

        /// <summary>
        /// Weight of this object or value for the purposes of random selection.
        /// </summary>
        [SerializeField]
        private float weight = 1f;

        /// <summary>
        /// Chance of this object or value to be selected randomly.
        /// </summary>
        [SerializeField, ReadOnly]
        private float selectionChance;
        
        #endregion

        #region Get

        // Gets the type of the weighted object.
        public virtual Type GetWeightedObjectType() => typeof(object);
        
#if UNITY_EDITOR
        protected string editorLabel = "No label provided";
#endif

        #endregion
    }
}