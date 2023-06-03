using UnityEngine;

namespace Unidork.Constants
{
    /// <summary>
    /// Common shader constants.
    /// </summary>
    public static class ShaderConstants
    {

        public static readonly int MainTextureId = Shader.PropertyToID("_MainTex");

        public static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        public static readonly int UnderlayColorId = Shader.PropertyToID("_UnderlayColor");
        public static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");
        
        public static readonly int Color = Shader.PropertyToID("_Color");
        public static readonly int ShadedColor = Shader.PropertyToID("_ColorDim");

        public static readonly int UnderlayOffsetXId = Shader.PropertyToID("_UnderlayOffsetX");
        public static readonly int UnderlayOffsetYId = Shader.PropertyToID("_UnderlayOffsetY");
        public static readonly int UnderlayDilateId = Shader.PropertyToID("_UnderlayDilate");
        public static readonly int UnderlaySoftnessId = Shader.PropertyToID("_UnderlaySoftness");

    }
}