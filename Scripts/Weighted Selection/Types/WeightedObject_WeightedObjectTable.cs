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
        public new WeightedTableBase Object => @object;
        
        public override Type GetWeightedObjectType() => typeof(WeightedTableBase);
        
        [SerializeField]
        protected new WeightedTableBase @object;

#if UNITY_EDITOR
        [SerializeField]
        protected new string editorLabel = "Table";
#endif
    } 
}