using System;
using UnityEngine;

namespace Unidork.WeightedSelection
{
    /// <summary>
    /// Weighted object that stores a float value.
    /// </summary>
    [Serializable]
    public class WeightedObject_Float : WeightedObjectBase
    {
        public new float Object => @object;
        
        public override Type GetWeightedObjectType() => typeof(float);
        
        [SerializeField]
        protected new float @object;

#if UNITY_EDITOR
        [SerializeField]
        protected new string editorLabel = "Float";
#endif
    } 
}