using Sirenix.OdinInspector;
using UnityEngine;

namespace Unidork.WeightedSelection
{
    /// <summary>
    /// Base class for objects storing data about items selected via weighted random based on their rarity.
    /// </summary>
    /// <typeparam name="TObject">Object.</typeparam>
    /// <typeparam name="TRarity">Object rarity.</typeparam>
    [System.Serializable]
    public class WeightedObjectWithRarity<TObject, TRarity> : WeightedObject<TObject> where TRarity : System.Enum
    {
        #region Properties

        /// <summary>
        /// Rarity of this type of object or value.
        /// </summary>
        /// <value>
        /// Gets the value of the field rarity.
        /// </value>
        public TRarity Rarity => rarity;

        #endregion

        #region Fields

        /// <summary>
        /// Rarity of this type of object or value.
        /// </summary>
        [PropertyOrder(1)]
        [HideIf("@this.IsDerivedClass()")]
        [Tooltip("Rarity of this type of object or value.")]
        [SerializeField] 
        private TRarity rarity;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value stored in this weighted object.</param>
        public WeightedObjectWithRarity(TObject value) : base(value)
        {
        }
        
        #endregion

#if UNITY_EDITOR
        
        #region Editor

        private bool IsDerivedClass()
        {
            return GetType().IsSubclassOf(typeof(WeightedObjectWithRarity<TObject, TRarity>));
        }

        #endregion
        
#endif
    }
}