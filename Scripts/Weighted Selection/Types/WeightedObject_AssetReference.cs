using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.WeightedSelection
{
    /// <summary>
    /// Weighted object that stores an Addressables asset reference.
    /// </summary>
    [Serializable]
    public class WeightedObject_AssetReference : WeightedObjectBase
    {
        public new AssetReference Object => @object;
        
        public override Type GetWeightedObjectType() => typeof(AssetReference);
        
        [SerializeField]
        protected new AssetReference @object;
        
#if UNITY_EDITOR
        [SerializeField]
        protected new string editorLabel = "Asset Ref";
#endif
    }
}