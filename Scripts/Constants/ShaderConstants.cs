using UnityEngine;

namespace Unidork.Constants
{
    /// <summary>
    /// Common shader constants.
    /// </summary>
    public static class ShaderConstants
    {
        public static readonly int Texture = Shader.PropertyToID("_Tex");
        public static readonly int MainTexture = Shader.PropertyToID("_MainTex");
        public static readonly int BaseMapTilingOffset = Shader.PropertyToID("_BaseMap_ST");

        public static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        public static readonly int UnderlayColor = Shader.PropertyToID("_UnderlayColor");
        public static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        
        public static readonly int Color = Shader.PropertyToID("_Color");
        public static readonly int ShadedColor = Shader.PropertyToID("_ColorDim");

        public static readonly int UnderlayOffsetX = Shader.PropertyToID("_UnderlayOffsetX");
        public static readonly int UnderlayOffsetY = Shader.PropertyToID("_UnderlayOffsetY");
        public static readonly int UnderlayDilate = Shader.PropertyToID("_UnderlayDilate");
        public static readonly int UnderlaySoftness = Shader.PropertyToID("_UnderlaySoftness");

    }
}