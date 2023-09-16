using Unity.Mathematics;

namespace Unidork.DOTS.Math
{
    public static class FloatExtensions
    {
        #region Constants
        
        public static readonly float2 Float2Up = new(0f, 1f);
        public static readonly float2 Float2Down = new(0f, -1f);
        public static readonly float2 Float2Right = new(1f, 0f);
        public static readonly float2 Float2Left = new(-1f, 0f);
        
        public static readonly float3 Float3Up = new(0f, 1f, 0f);
        public static readonly float3 Float3Down = new(0f, -1f, 0f);
        public static readonly float3 Float3Right = new(1f, 0f, 0f);
        public static readonly float3 Float3Left = new(-1f, 0f, 0f);
        public static readonly float3 Float3Forward = new(0f, 0f, 1f);
        public static readonly float3 Float3Back = new(0f, 0f, -1f);
        
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
        /// <param name="float2">Float2.</param>
        /// <returns>True if both components are zero, False otherwise.</returns>
        public static bool IsZero(this float2 float2)
        {
            return float2.Equals(float2.zero);
        }
        
        #endregion
        
        #region Float3
        
        /// <summary>
        /// Checks if given float3 is equal to float3.zero.
        /// </summary>
        /// <param name="float3">Float3.</param>
        /// <returns>True if all components are zero, False otherwise.</returns>
        public static bool IsZero(this float3 float3)
        {
            return float3.Equals(float3.zero);
        }
        
        /// <summary>
        /// Gets the angle between two float3 vectors.
        /// </summary>
        /// <param name="firstVector">First vector.</param>
        /// <param name="secondVector">Second vector.</param>
        /// <returns>A float representing angle between vectors in degrees.</returns>
        public static float AngleDegrees(float3 firstVector, float3 secondVector)
        {
            float dot = math.dot(firstVector, secondVector);
            return dot < 0.999998986721039f ? math.acos(math.min(math.abs(dot), 1f)) * 2f : 0f;
        }

        /// <summary>
        /// Gets the angle between two float3 vectors.
        /// </summary>
        /// <param name="firstVector">First vector.</param>
        /// <param name="secondVector">Second vector.</param>
        /// <returns>A float representing angle between vectors in radians.</returns>
        public static float AngleRadians(float3 firstVector, float3 secondVector)
        {
            return math.radians(AngleDegrees(firstVector, secondVector));
        }
        
        /// <summary>
        /// Rotates a float3 vectors towards a target.
        /// </summary>
        /// <param name="startVector">Start vector.</param>
        /// <param name="targetVector">Target vector.</param>
        /// <param name="maxDegrees">Maximum allowed frame rotation in degrees.</param>
        /// <returns>Rotated float3.</returns>
        public static float3 RotateTowards(float3 startVector, float3 targetVector, float maxDegrees)
        {
            float angleBetweenQuaternions = AngleDegrees(startVector, targetVector);

            if (angleBetweenQuaternions < float.Epsilon)
            {
                return startVector;
            }
            
            float lerpAmount = math.min(1f, maxDegrees / angleBetweenQuaternions);
            return math.lerp(startVector, targetVector, lerpAmount);
        }
        
        #endregion
    }
}