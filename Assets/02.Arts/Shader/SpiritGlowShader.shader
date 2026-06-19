Shader "Custom/Spirit Glow"
{
    Properties
    {
        [MainColor][HDR] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Tags {"LightMode" = "Glow"}

            Stencil {
                Ref 1
                Comp always
                Pass replace
            }

            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
            CBUFFER_END

            float Unity_Random(float3 Seed)
            {
                float randomno = frac(sin(dot(Seed, float3(12.9898, 78.233, 36.3391)))*43758.5453);
                return randomno;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 dir = normalize(IN.positionOS.xyz);
                dir *= sin(Unity_Random(IN.positionOS.xyz) * 1000 + _Time.y * 2) * 0.08;
                dir.y += sin(_Time.y * 2) * 0.2;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz + dir);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = _BaseColor;
                return color;
            }
            ENDHLSL
        }

    }
}
