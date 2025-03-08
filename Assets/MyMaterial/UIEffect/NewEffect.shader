Shader "UGUI/NewEffect"
{
    Properties
    {
        [PerRendererData] _MainTex ("MainTex", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)

        [Header(Stencil)]
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        [Header(Pixelate)]
        _PixelSize ("Pixel Size", Range(0.01, 10)) = 1

        [Header(Glitch)]
        _GlitchStrength ("Glitch Strength", Range(0, 1)) = 0.1
        _GlitchFrequency ("Glitch Frequency", Range(0, 10)) = 1

        [Header(Hologram)]
        _HologramStrength ("Hologram Strength", Range(0, 1)) = 0.5
        _HologramFrequency ("Hologram Frequency", Range(0, 10)) = 5
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref 0
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _PixelSize;
            float _GlitchStrength;
            float _GlitchFrequency;
            float _HologramStrength;
            float _HologramFrequency;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // 像素化函数
            fixed4 Pixelate(float2 uv)
            {
                uv = floor(uv / _PixelSize) * _PixelSize;
                return tex2D(_MainTex, uv) * _Color;
            }

            // 故障函数
            fixed4 Glitch(float2 uv)
            {
                float glitchValue = frac(sin(dot(uv, float2(_GlitchFrequency, _GlitchFrequency))) * 10000.0) * 2.0 - 1.0;
                glitchValue = saturate(glitchValue * _GlitchStrength);
                return tex2D(_MainTex, uv + glitchValue) * _Color;
            }

            // 全息函数
            fixed4 Hologram(float2 uv)
            {
                float hologramValue = sin(dot(uv, float2(_HologramFrequency, _HologramFrequency)) + _Time.y * 5.0) * 0.5 + 0.5;
                hologramValue = saturate(hologramValue * _HologramStrength);
                return tex2D(_MainTex, uv) * _Color * hologramValue;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 这里可以根据需求选择使用哪种效果，或者混合多种效果
                // 例如，只使用像素化效果：
                // return Pixelate(i.uv);
                // 只使用故障效果：
                // return Glitch(i.uv);
                // 只使用全息效果：
                // return Hologram(i.uv);
                // 混合像素化和故障效果：
                // return lerp(Pixelate(i.uv), Glitch(i.uv), 0.5);
                // 混合所有三种效果：
                fixed4 pixelated = Pixelate(i.uv);
                fixed4 glitched = Glitch(i.uv);
                fixed4 hologrammed = Hologram(i.uv);
                return lerp(pixelated, lerp(glitched, hologrammed, 0.5), 0.5);
            }
            ENDCG
        }
    }
}