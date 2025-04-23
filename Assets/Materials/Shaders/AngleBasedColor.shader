Shader "Unlit/AngleBasedColor"
{
    Properties
    {
        _Color ("Flat Color", Color) = (1, 0.5, 0.2, 1)
        _DispTex ("Displacement Texture", 2D) = "gray" {}
        _Displacement ("Displacement Amount", Range(0, 1.0)) = 0.1
        _NoiseScale ("Noise UV Scale", Float) = 1.0
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        HLSLPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        struct Attributes
        {
            float4 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionHCS : SV_POSITION;
        };

        CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            TEXTURE2D(_DispTex);
            SAMPLER(sampler_DispTex);
            float _Displacement;
            float _NoiseScale;
            float _ScrollSpeed;
        CBUFFER_END

        Varyings vert(Attributes IN)
        {
            Varyings OUT;

            float2 scrolledUV = IN.uv * _NoiseScale + float2(_Time.y * _ScrollSpeed, 0);
            float displacement = SAMPLE_TEXTURE2D(_DispTex, sampler_DispTex, scrolledUV).r;

            float3 displacedVertex = IN.positionOS.xyz + IN.normalOS * displacement * _Displacement;
            OUT.positionHCS = TransformObjectToHClip(float4(displacedVertex, 1.0));

            return OUT;
        }

        half4 frag(Varyings IN) : SV_Target
        {
            return _Color;
        }
        ENDHLSL
    }
    FallBack "Diffuse"
}
