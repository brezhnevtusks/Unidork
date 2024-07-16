using Unity.Mathematics;

namespace Unidork.DOTS.Math
{
    public static class FloatExtensions
    {
        #region Constants
        
        public static readonly float2 F2Up = new(0f, 1f);
        public static readonly float2 F2Down = -F2Up;
        public static readonly float2 F2Right = new(1f, 0f);
        public static readonly float2 F2Left = -F2Right;
        
        public static readonly float3 F3Up = new(0f, 1f, 0f);
        public static readonly float3 F3Down = -F3Up;
        public static readonly float3 F3Right = new(1f, 0f, 0f);
        public static readonly float3 F3Left = -F3Right;
        public static readonly float3 F3Forward = new(0f, 0f, 1f);
        public static readonly float3 F3Back = -F3Forward;
        
        #endregion
        
        #region Float

        /// <summary>
        /// Checks if two float values are approximately the same.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>True if values are approximately the same, False otherwise.</returns>
        public static bool Approximately(float a, float b)
        {
            // https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Mathf.cs#L279
            return math.abs(b - a) < math.max(0.000001f * math.max(math.abs(a), math.abs(b)), math.EPSILON * 8);
        }
        
        #endregion
        
        #region Float2
        
        /// <summary>
        /// Checks if given float2 is equal to float2.zero.
        /// </summary>
        /// <param name="f2">Float2.</param>
        /// <returns>True if both components are zero, False otherwise.</returns>
        public static bool IsZero(this float2 f2)
        {
            return f2.Equals(float2.zero);
        }
        
        #endregion
        
        #region Float3
        
        /// <summary>
        /// Checks if given float3 is equal to float3.zero.
        /// </summary>
        /// <param name="f3">Float3.</param>
        /// <returns>True if all components are zero, False otherwise.</returns>
        public static bool IsZero(this float3 f3)
        {
            return f3.Equals(float3.zero);
        }
        
        #endregion

        #region Smooth damp

        /// <summary>
        /// Equivalent of Unity's SmoothDamp() using Unity.Mathematics. Smoothly interpolates from one value to another.
        /// </summary>
        /// <param name="current">Value to interpolate from.</param>
        /// <param name="target">Value to interpolate to.</param>
        /// <param name="currentChangeRate">Current change rate (per frame).</param>
        /// <param name="smoothTime">Interpolation duration.</param>
        /// <param name="deltaTime">Delta time.</param>
        /// <param name="maxChangeRate">Max change rate (per frame).</param>
        /// <returns>
        /// A float representing interpolated value on this frame.
        /// </returns>
        public static float SmoothDamp(float current, float target, ref float currentChangeRate, 
                                       float smoothTime, float deltaTime, float maxChangeRate = float.MaxValue)
        {
            smoothTime = math.max(0.0001f, smoothTime);
            
            float num1 = 2f / smoothTime;
            float num2 = num1 * deltaTime;
            float num3 = 1f / (1f + num2 + 0.47999998927116394f * num2 * num2 + 0.23499999940395355f * num2 * num2 * num2);
            float num4 = current - target;
            float num5 = target;
            float max = maxChangeRate * smoothTime;
            float num6 = math.clamp(num4, -max, max);
            target = current - num6;
            float num7 = (currentChangeRate + num1 * num6) * deltaTime;
            currentChangeRate = (currentChangeRate - num1 * num7) * num3;
            float num8 = target + (num6 + num7) * num3;
            
            if (num5 - (double)current > 0f == num8 > (double)num5)
            {
                num8 = num5;
                currentChangeRate = (num8 - num5) / deltaTime;
            }
            
            return num8;
        }

        #endregion
    }
}