using Unity.Entities;
using UnityEngine;

namespace Unidork.DOTS.Math
{
    public class RandomFloatRangeAuthoring : MonoBehaviour
    {
        public float Min;
        public float Max;

        private class RandomFloatRangeBaker : Baker<RandomFloatRangeAuthoring>
        {
            public override void Bake(RandomFloatRangeAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new RandomFloatRange
                {
                    Min = authoring.Min,
                    Max = authoring.Max
                });
            }
        }
    }
}