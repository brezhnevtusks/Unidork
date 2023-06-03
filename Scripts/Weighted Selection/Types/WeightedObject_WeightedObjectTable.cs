using System;
using UnityEngine;

namespace Unidork.WeightedSelection
{
    /// <summary>
    /// Weighted object that stores a reference to a weighted object table.
    /// </summary>
    [Serializable]
    public class WeightedObject_WeightedObjectTable : WeightedObjectBase
    {
        public new WeightedObjectTableBase Object => @object;
        
        public override Type GetWeightedObjectType() => typeof(WeightedObjectTableBase);
        
        [SerializeField]
        protected new WeightedObjectTableBase @object;

#if UNITY_EDITOR
        [SerializeField]
        protected new string editorLabel = "Table";
#endif
    } 
}