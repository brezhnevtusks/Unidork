using Unidork.Extensions;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Unidork.DOTS.Camera
{
    #region Components
    
    /// <summary>
    /// Struct to use for converting world position to screen coordinates.
    /// </summary>
    public struct WorldToScreen : IComponentData
    {
        /// <summary>
        /// Camera projection matrix.
        /// </summary>
        public float4x4 ProjectionMatrix;
     
        /// <summary>
        /// Camera position.
        /// </summary>
        public float3 CameraPosition;
           
        /// <summary>
        /// Camera up vector.
        /// </summary>
        public float3 Up;
     
        /// <summary>
        /// Camera right vector.
        /// </summary>
        public float3 Right;
     
        /// <summary>
        /// Camera forward vector.
        /// </summary>
        public float3 Forward;
     
        /// <summary>
        /// Camera view width in pixels.
        /// </summary>
        public float PixelWidth;
     
        /// <summary>
        /// Camera view height in pixels.
        /// </summary>
        public float PixelHeight;
     
        
    }
    
    #endregion

    #region Systems

    /// <summary>
    /// System that creates and updates a <see cref="WorldToScreen"/> component that can be used
    /// for camera point calculations like converting world position to screen.
    /// Needs to be created manually.
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]
    public partial class WorldToScreenUpdateSystem : SystemBase
    {
        private UnityEngine.Camera MainCamera
        {
            get
            {
                if (mainCamera == null)
                {
                    GameObject mainCameraGo = GameObject.FindWithTag(Constants.CommonTags.MainCamera);

                    if (mainCameraGo != null)
                    {
                        mainCamera = mainCameraGo.GetComponent<UnityEngine.Camera>();
                    }
                }

                return mainCamera;
            }
        }
        
        private UnityEngine.Camera mainCamera;
            
        protected override void OnCreate()
        {
            CheckedStateRef.EntityManager.CreateSingleton<WorldToScreen>();
        }

        protected override void OnUpdate()
        {
            if (MainCamera != null)
            {
                if (SystemAPI.TryGetSingletonEntity<WorldToScreen>(out Entity entity))
                {
                    WorldToScreen worldToScreen = CameraUtility.CreateWorldToScreen(MainCamera);
                    CheckedStateRef.EntityManager.SetComponentData(entity, worldToScreen);
                }
            }
        }
    }

    #endregion

    #region Utility

    public static class CameraUtility
    {
        /// <summary>
        /// Creates a <see cref="WorldToScreen"/> struct from camera data.
        /// </summary>
        [BurstDiscard]
        public static WorldToScreen CreateWorldToScreen(UnityEngine.Camera camera)
        {
            Transform cameraTransform = camera.transform;
            
            WorldToScreen worldToScreen = new WorldToScreen
            {
                ProjectionMatrix = camera.projectionMatrix,
                CameraPosition = cameraTransform.position,
                Up = cameraTransform.up,
                Right = cameraTransform.right,
                Forward = cameraTransform.forward,
                PixelWidth = camera.pixelWidth,
                PixelHeight = camera.pixelHeight
            };
     
            return worldToScreen;
        }
        
        /// <summary>
        /// Converts a position from world space to screen space.
        /// </summary>
        /// <param name="worldPosition">World space position to convert.</param>
        /// <param name="worldToScreen">Component containing data used to convert world to screen position.</param>
        /// <returns>
        /// A float2 representing screen coordinates of the converted world position.
        /// </returns>
        public static float2 WorldToScreenPosition(float3 worldPosition, WorldToScreen worldToScreen)
        {
            return WorldToScreenPosition(worldPosition, worldToScreen.CameraPosition, worldToScreen.ProjectionMatrix,
                                         worldToScreen.Up, worldToScreen.Right, worldToScreen.Forward,
                                         worldToScreen.PixelWidth, worldToScreen.PixelHeight);
        }
           
        /// <summary>
        /// Converts a position from world space to screen space.
        /// </summary>
        /// <param name="worldPosition">World space position to convert.</param>
        /// <param name="cameraPosition">Camera position in world space</param>
        /// <param name="cameraProjectionMatrix">Camera projection matrix</param>
        /// <param name="cameraUpVector">Camera up vector in world space</param>
        /// <param name="cameraRightVector">Camera Right vector in world space</param>
        /// <param name="cameraForwardVector">Camera Forward vector in world space</param>
        /// <param name="pixelWidth">Screen pixel width</param>
        /// <param name="pixelHeight">Screen pixel height</param>
        /// <returns>
        /// A float2 representing screen coordinates of the converted world position.
        /// </returns>
        public static float2 WorldToScreenPosition(float3 worldPosition, float3 cameraPosition, float4x4 cameraProjectionMatrix,
                                                   float3 cameraUpVector, float3 cameraRightVector, float3 cameraForwardVector,
                                                   float pixelWidth, float pixelHeight)
        {
            float4 cameraSpacePosition = WorldToCameraPosition(worldPosition, cameraPosition, cameraUpVector, cameraRightVector, cameraForwardVector);
            float4 clippedCameraSpacePosition = math.mul(cameraProjectionMatrix, cameraSpacePosition);
            float4 normalizedDeviceCoordinatesPosition = clippedCameraSpacePosition / clippedCameraSpacePosition.w;
            float2 screenPosition;
            screenPosition.x = pixelWidth  / 2.0f * (normalizedDeviceCoordinatesPosition.x + 1);
            screenPosition.y = pixelHeight / 2.0f * (normalizedDeviceCoordinatesPosition.y + 1);
     
            return screenPosition;
        }
     
        /// <summary>
        /// Converts world space position to camera space position.
        /// </summary>
        /// <param name="worldPosition">World position to convert.</param>
        /// <param name="cameraPosition">Camera position in world space.</param>
        /// <param name="cameraUp">Camera up vector in world space.</param>
        /// <param name="cameraRight">Camera right vector in world space.</param>
        /// <param name="cameraForward">Camera forward vector in world space.</param>
        /// <returns>
        /// A float4 that represents converted position.
        /// </returns>
        private static float4 WorldToCameraPosition(float3 worldPosition, float3 cameraPosition, float3 cameraUp,
                                                    float3 cameraRight, float3 cameraForward)
        {
            float4 translatedPosition = new float4(worldPosition - cameraPosition, 1f);
     
            float4x4 transformationMatrix = float4x4.identity;
            transformationMatrix.c0 = new float4(cameraRight.x, cameraUp.x, -cameraForward.x, 0);
            transformationMatrix.c1 = new float4(cameraRight.y, cameraUp.y, -cameraForward.y, 0);
            transformationMatrix.c2 = new float4(cameraRight.z, cameraUp.z, -cameraForward.z, 0);
     
            float4 transformedPosition = math.mul(transformationMatrix, translatedPosition);
     
            return transformedPosition;
        }
    }

    #endregion
}