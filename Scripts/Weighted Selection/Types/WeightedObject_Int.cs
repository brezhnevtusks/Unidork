using System;
using UnityEngine;

namespace Unidork.WeightedSelection
{
    /// <summary>
    /// Weighted object that stores an integer value.
    /// </summary>
    [Serializable]
    public class WeightedObject_Int : WeightedObjectBase
    {
        public new int Object => @object;
        
        public override Type GetWeightedObjectType() => typeof(int);
        
        [SerializeField]
        protected new int @object;
        
#if UNITY_EDITOR
        [SerializeField]
        protected new string editorLabel = "Int";
#endif
    } 
}