Shader "Unlit/breathTimeShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Percentage ("Percentage", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float1 _Percentage;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 colr = (1, 1, 1, 1);
                colr.w = 0.3;
                colr.gb = _Percentage;
                clip(_Percentage - (i.uv.x - 0.5) * 0.9 - 0.5 + (i.uv.y - 0.5) * 0.05);
                return colr;
            }
            ENDCG
        }
    }
}
