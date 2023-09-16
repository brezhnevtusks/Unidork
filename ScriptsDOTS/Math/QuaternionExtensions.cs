using Unity.Mathematics;

namespace Unidork.DOTS.Math
{
    public static class QuaternionExtensions
    {
        /// <summary>
        /// Gets the angle between two quaternions in degrees.
        /// </summary>
        /// <param name="firstQuaternion">First quaternion.</param>
        /// <param name="secondQuaternion">Second quaternion.</param>
        /// <returns>A float representing angle between quaternions in degrees.</returns>
        public static float AngleDegrees(quaternion firstQuaternion, quaternion secondQuaternion)
        {
            float dot = math.dot(firstQuaternion, secondQuaternion);
            return dot < 0.999998986721039f ? math.acos(math.min(math.abs(dot), 1f)) * 2f : 0f;
        }

        /// <summary>
        /// Gets the angle between two quaternions in radians.
        /// </summary>
        /// <param name="firstQuaternion">First quaternion.</param>
        /// <param name="secondQuaternion">Second quaternion.</param>
        /// <returns></returns>
        public static float AngleRadians(quaternion firstQuaternion, quaternion secondQuaternion)
        {
            return math.radians(AngleDegrees(firstQuaternion, secondQuaternion));
        }

        /// <summary>
        /// Rotates a quaternion towards a target.
        /// </summary>
        /// <param name="startQuaternion">Start quaternion.</param>
        /// <param name="targetQuaternion">Target quaternion.</param>
        /// <param name="maxDegrees">Maximum allowed frame rotation in degrees.</param>
        /// <returns>Rotated quaternion.</returns>
        public static quaternion RotateTowards(quaternion startQuaternion, quaternion targetQuaternion, float maxDegrees)
        {
            float angleBetweenQuaternions = AngleDegrees(startQuaternion, targetQuaternion);

            if (angleBetweenQuaternions < float.Epsilon)
            {
                return startQuaternion;
            }

            float lerpAmount = math.min(1f, maxDegrees / angleBetweenQuaternions);
            return math.slerp(startQuaternion, targetQuaternion, lerpAmount);
        }
    }
}