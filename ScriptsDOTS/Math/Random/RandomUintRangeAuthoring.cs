using Unity.Entities;
using UnityEngine;

namespace Unidork.DOTS.Math
{
    public class RandomUintRangeAuthoring : MonoBehaviour
    {
        public uint Min;
        public uint Max;

        private class RandomIntRangeBaker : Baker<RandomUintRangeAuthoring>
        {
            public override void Bake(RandomUintRangeAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new RandomUintRange
                {
                    Min = authoring.Min,
                    Max = authoring.Max
                });
            }
        } 
    }
}