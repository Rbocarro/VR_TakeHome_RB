Shader "Unlit/AngleBasedColor"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _TargetColor ("Target Color", Color) = (1, 0, 0, 1)
        _DotProduct ("Dot Product", Range(-1, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO //Insert
                //docs.unity3d.com/Manual/SinglePassInstancing.html
            };

            fixed4 _BaseColor;
            fixed4 _TargetColor;
            float _DotProduct;
            
            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {   
                //lerp remapped dot product from the range -1, 1 to 0,1
                fixed4 col = lerp(_TargetColor, _BaseColor, saturate((_DotProduct + 1) / 2.0));
                return col;
            }

            ENDHLSL
        }
    }
}