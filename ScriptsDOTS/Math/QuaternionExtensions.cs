using Unity.Mathematics;

namespace Unidork.DOTS.Math
{
    public static class QuaternionExtensions
    {
        /// <summary>
        /// Gets the angle between two quaternions in degrees.
        /// </summary>
        /// <param name="q1">First quaternion.</param>
        /// <param name="q2">Second quaternion.</param>
        /// <returns>A float representing angle between quaternions in degrees.</returns>
        public static float AngleDegrees(quaternion q1, quaternion q2)
        {
            float dot = math.dot(q1, q2);
            return dot < 0.999998986721039f ? math.acos(math.min(math.abs(dot), 1f)) * 2f : 0f;
        }

        /// <summary>
        /// Gets the angle between two quaternions in radians.
        /// </summary>
        /// <param name="q1">First quaternion.</param>
        /// <param name="q2">Second quaternion.</param>
        /// <returns></returns>
        public static float AngleRadians(quaternion q1, quaternion q2)
        {
            return math.radians(AngleDegrees(q1, q2));
        }

        /// <summary>
        /// Rotates a quaternion towards a target.
        /// </summary>
        /// <param name="start">Start quaternion.</param>
        /// <param name="target">Target quaternion.</param>
        /// <param name="maxDegrees">Maximum allowed frame rotation in degrees.</param>
        /// <returns>Rotated quaternion.</returns>
        public static quaternion RotateTowards(quaternion start, quaternion target, float maxDegrees)
        {
            float angleBetweenQuaternions = AngleDegrees(start, target);

            if (angleBetweenQuaternions < float.Epsilon)
            {
                return start;
            }

            float lerpAmount = math.min(1f, maxDegrees / angleBetweenQuaternions);
            return math.slerp(start, target, lerpAmount);
        }
    }
}