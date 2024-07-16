using Unity.Entities;
using UnityEngine;

namespace Unidork.DOTS.Math
{
    public class RandomIntRangeAuthoring : MonoBehaviour
    {
        public int Min;
        public int Max;

        private class RandomIntRangeBaker : Baker<RandomIntRangeAuthoring>
        {
            public override void Bake(RandomIntRangeAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new RandomIntRange
                {
                    Min = authoring.Min,
                    Max = authoring.Max
                });
            }
        }
    }
}