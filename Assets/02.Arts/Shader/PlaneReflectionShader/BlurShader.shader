Shader "Custom/Blur"
{
    Properties
    {
        
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "Blit"
            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            SAMPLER(sampler_BlitTexture);

            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
                float2 uv = IN.texcoord;
                
                half4 col = 0;
                half sum = 0;
                for(int i = -2; i < 2; i++)
                {
                    for(int j = -2; j < 2; j++)
                    {
                        col += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, uv + float2(i, j) / _ScreenParams.xy);
                        sum += 1;
                    }
                }

                col /= sum;

                return half4(col.rgb, 1);
            }
            ENDHLSL
        }
    }
}
