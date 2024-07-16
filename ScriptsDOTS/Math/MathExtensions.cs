using Unity.Mathematics;

namespace Unidork.DOTS.Math
{
    public static class MathExtensions
    {
        #region Angle

        /// <summary>
        /// Gets unsigned angle between two vectors. Faster than <see cref="GetAngle"/> but noisy at small angles.
        /// </summary>
        /// <param name="v1">First vector.</param>
        /// <param name="v2">Second vector.</param>
        /// <returns>
        /// A float representing an unsigned angle in degrees.
        /// </returns>
        public static float GetAngleFast(float3 v1, float3 v2)
        {
            return math.degrees(math.acos(math.dot(math.normalize(v1), math.normalize(v2))));
        }

        /// <summary>
        /// Gets unsigned angle between two vectors. Slower than <see cref="GetAngleFast"/> but more precise at small angles.
        /// See https://forum.kerbalspaceprogram.com/topic/164418-vector3angle-more-accurate-and-numerically-stable-at-small-angles-version/
        /// </summary>
        /// <param name="v1">First vector.</param>
        /// <param name="v2">Second vector.</param>
        /// <returns>
        /// A float representing an unsigned angle in degrees.
        /// </returns>
        public static float GetAngle(float3 v1, float3 v2)
        {
            float3 abm = v1 * math.length(v2);
            float3 bam = v2 * math.length(v1);
            return math.degrees(2f * math.atan2(math.length(abm - bam), math.length(abm + bam)));
        }
        
        /// <summary>
        /// Gets the signed angle between two vectors in relation to an axis.
        /// </summary>
        /// <param name="v1">First vector.</param>
        /// <param name="v2">Second vector.</param>
        /// <param name="up">Up axis.</param>
        /// <returns>
        /// A float representing a signed angle in degrees.
        /// </returns>
        public static float GetAngleSigned(float3 v1, float3 v2, float3 up)
        {
            float angle = math.acos(math.dot(math.normalize(v1), math.normalize(v2)));
            float sign = math.sign(math.dot(up, math.cross(v1, v2)));
            return math.degrees(angle * sign);
        }

        #endregion
    }
}