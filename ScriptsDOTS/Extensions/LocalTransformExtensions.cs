using Unidork.DOTS.Math;
using Unity.Mathematics;
using Unity.Transforms;

namespace Unidork.DOTS.Extensions
{
    public static class LocalTransformExtensions
    {
        #region Translation

        public static LocalTransform TranslateX(this LocalTransform localTransform, float translation)
        {
            return localTransform.Translate(new float3(translation, 0f, 0f));
        }

        public static LocalTransform TranslateY(this LocalTransform localTransform, float translation)
        {
            return localTransform.Translate(new float3(0f, translation, 0f));
        }

        public static LocalTransform TranslateZ(this LocalTransform localTransform, float translation)
        {
            return localTransform.Translate(new float3(0f, 0f, translation));
        }
        
        #endregion

        #region Rotate

        public static LocalTransform RotateXWorld(this LocalTransform localTransform, float angle)
        {
            float3 forward = math.mul(quaternion.Euler(angle, 0f, 0f), localTransform.Forward());
            LocalTransform finalTransform = localTransform;
            finalTransform.Rotation = quaternion.LookRotation(forward, FloatExtensions.F3Up);
            return finalTransform;
        }
        
        public static LocalTransform RotateYWorldS(this LocalTransform localTransform, float angle)
        {
            float3 forward = math.mul(quaternion.Euler(0f, angle, 0f), localTransform.Forward());
            LocalTransform finalTransform = localTransform;
            finalTransform.Rotation = quaternion.LookRotation(forward, FloatExtensions.F3Up);
            return finalTransform;
        }
        
        public static LocalTransform RotateZWorld(this LocalTransform localTransform, float angle)
        {
            float3 forward = math.mul(quaternion.Euler(0f, 0f, angle), localTransform.Forward());
            LocalTransform finalTransform = localTransform;
            finalTransform.Rotation = quaternion.LookRotation(forward, FloatExtensions.F3Up);
            return finalTransform;
        }
            
        #endregion
    }
}