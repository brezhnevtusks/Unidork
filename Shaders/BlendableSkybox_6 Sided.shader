Shader "Unidork/Skybox/Blendable_6 Sided"
{
    Properties
    {
        _Tint ("Tint Color", Color) = (.5, .5, .5, .5)
        [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0

         [Space, Space]
        _XRotation ("X Rotation", Range(0, 360)) = 0
        _YRotation ("Y Rotation", Range(0, 360)) = 0

        _BlendValue ("Blend Value", Range(0, 1)) = 0

        [Space, Space]
        [NoScaleOffset] _FirstFrontTex ("First Front [+Z]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _FirstBackTex ("First Back [-Z]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _FirstLeftTex ("First Left [+X]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _FirstRightTex ("First Right [-X]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _FirstUpTex ("First Up [+Y]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _FirstDownTex ("First Down [-Y]   (HDR)", 2D) = "grey" {}

        [Space, Space]
        [NoScaleOffset] _SecondFrontTex ("Second Front [+Z]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _SecondBackTex ("Second Back [-Z]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _SecondLeftTex ("Second Left [+X]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _SecondRightTex ("Second Right [-X]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _SecondUpTex ("Second Up [+Y]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _SecondDownTex ("Second Down [-Y]   (HDR)", 2D) = "grey" {}
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off

        CGINCLUDE
        #include "UnityCG.cginc"

        half4 _Tint;
        half _Exposure;
        float _XRotation;
        float _YRotation;
        float _BlendValue;

        float4x4 CreateSkyboxRotation(float3 axis, float angle)
        {
            axis = normalize(axis);
            float s = sin(angle);
            float c = cos(angle);
            float oc = 1.0 - c;

            return float4x4(oc * axis.x * axis.x + c, oc * axis.x * axis.y - axis.z * s, oc * axis.z * axis.x + axis.y * s, 0.0,
                            oc * axis.x * axis.y + axis.z * s, oc * axis.y * axis.y + c, oc * axis.y * axis.z - axis.x * s, 0.0,
                            oc * axis.z * axis.x - axis.y * s, oc * axis.y * axis.z + axis.x * s, oc * axis.z * axis.z + c, 0.0,
                            0.0, 0.0, 0.0, 1.0);
        }

        struct appdata_t
        {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f
        {
            float4 vertex : SV_POSITION;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        v2f vert (appdata_t v)
        {
            v2f o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            float3 rotated = mul(CreateSkyboxRotation(normalize(float3(0, 1, 0)), _YRotation * UNITY_PI / 180.0), v.vertex).xyz;
            rotated = mul(CreateSkyboxRotation(normalize(float3(1, 0, 0)), _XRotation * UNITY_PI / 180.0), rotated).xyz;

            o.vertex = UnityObjectToClipPos(rotated);
            o.texcoord = v.texcoord;
            return o;
        }

        half4 skybox_frag (v2f i, sampler2D smp, half4 smpDecode)
        {
            half4 tex = tex2D (smp, i.texcoord);
            half3 c = DecodeHDR (tex, smpDecode);
            c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
            c *= _Exposure;
            return half4(c, 1);
        }

        half4 skybox_lerped_frag (v2f i, sampler2D firstSampler, sampler2D secondSampler, half4 firstSamplerDecode, half4 secondSamplerDecode)
        {
            half4 firstTex = tex2D (firstSampler, i.texcoord);
            half3 firstColor = DecodeHDR (firstTex, firstSamplerDecode);
            firstColor = firstColor * _Tint.rgb * unity_ColorSpaceDouble.rgb;
            firstColor *= _Exposure;

            if (_BlendValue == 0)
            {
                return half4(firstColor, 1);
            }

            half4 secondTex = tex2D (secondSampler, i.texcoord);
            half3 secondColor = DecodeHDR (secondTex, secondSamplerDecode);
            secondColor = secondColor * _Tint.rgb * unity_ColorSpaceDouble.rgb;
            secondColor *= _Exposure;

            return half4(lerp(firstColor, secondColor, _BlendValue), 1);
        }

        ENDCG

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _FirstFrontTex;
            sampler2D _SecondFrontTex;
            half4 _FirstFrontTex_HDR;
            half4 _SecondFrontTex_HDR;
            half4 frag (v2f i) : SV_Target { return skybox_lerped_frag(i, _FirstFrontTex, _SecondFrontTex, _FirstFrontTex_HDR, _SecondFrontTex_HDR); }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _FirstBackTex;
            sampler2D _SecondBackTex;
            half4 _FirstBackTex_HDR;
            half4 _SecondBackTex_HDR;
            half4 frag (v2f i) : SV_Target { return skybox_lerped_frag(i, _FirstBackTex, _SecondBackTex, _FirstBackTex_HDR, _SecondBackTex_HDR); }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _FirstLeftTex;
            sampler2D _SecondLeftTex;
            half4 _FirstLeftTex_HDR;
            half4 _SecondLeftTex_HDR;
            half4 frag (v2f i) : SV_Target { return skybox_lerped_frag(i, _FirstLeftTex, _SecondLeftTex, _FirstLeftTex_HDR, _SecondLeftTex_HDR); }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _FirstRightTex;
            sampler2D _SecondRightTex;
            half4 _FirstRightTex_HDR;
            half4 _SecondRightTex_HDR;
            half4 frag (v2f i) : SV_Target { return skybox_lerped_frag(i, _FirstRightTex, _SecondRightTex, _FirstRightTex_HDR, _SecondRightTex_HDR); }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _FirstUpTex;
            sampler2D _SecondUpTex;
            half4 _FirstUpTex_HDR;
            half4 _SecondUpTex_HDR;
            half4 frag (v2f i) : SV_Target { return skybox_lerped_frag(i, _FirstUpTex, _SecondUpTex, _FirstUpTex_HDR, _SecondUpTex_HDR); }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _FirstDownTex;
            sampler2D _SecondDownTex;
            half4 _FirstDownTex_HDR;
            half4 _SecondDownTex_HDR;
            half4 frag (v2f i) : SV_Target { return skybox_lerped_frag(i, _FirstDownTex, _SecondDownTex, _FirstDownTex_HDR, _SecondDownTex_HDR); }
            ENDCG
        }
    }
}
