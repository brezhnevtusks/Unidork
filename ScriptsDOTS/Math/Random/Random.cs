using System;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace Unidork.DOTS.Math
{
    #region Components

    public struct RandomIntRange : IComponentData
    {
        public int Min;
        public int Max;
        public int MaxExclusive => Max + 1;
        
        public static int GetRandom(RandomIntRange range, ref Random random)
        {
            return random.NextInt(range.Min, range.MaxExclusive);
        }
        
        public static implicit operator int2(RandomIntRange randomIntRange)
        {
            return new int2(randomIntRange.Min, randomIntRange.Max);
        }

        public static implicit operator RandomIntRange(int2 i2)
        {
            return new RandomIntRange
            {
                Min = i2.x,
                Max = i2.y
            };
        }
    }

    [Serializable]
    public struct RandomUintRange : IComponentData
    {
        // ReSharper disable InconsistentNaming
        public uint Min;
        public uint Max;
        // ReSharper restore InconsistentNaming
        public uint MaxExclusive => Max + 1;

        public static uint GetRandom(RandomUintRange range, ref Random random)
        {
            return random.NextUInt(range.Min, range.MaxExclusive);
        }
        
        public static implicit operator uint2(RandomUintRange randomUintRange)
        {
            return new uint2(randomUintRange.Min, randomUintRange.Max);
        }

        public static implicit operator RandomUintRange(uint2 u2)
        {
            return new RandomUintRange
            {
                Min = u2.x,
                Max = u2.y
            };
        }
    }

    [Serializable]
    public struct RandomFloatRange : IComponentData
    {
        // ReSharper disable InconsistentNaming
        public float Min;
        public float Max;
        // ReSharper restore InconsistentNaming
        
        public static float GetRandom(RandomFloatRange range, ref Random random)
        {
            return random.NextFloat(range.Min, range.Max);
        }
        
        public static implicit operator float2(RandomFloatRange randomFloatRange)
        {
            return new float2(randomFloatRange.Min, randomFloatRange.Max);
        }

        public static implicit operator RandomFloatRange(float2 f2)
        {
            return new RandomFloatRange
            {
                Min = f2.x,
                Max = f2.y
            };
        }
    }

    #endregion
}