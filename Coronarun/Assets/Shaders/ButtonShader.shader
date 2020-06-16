Shader "UI/ButtonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _WidthVar("Width Variable", Range(0.0, 1.0)) = 0.5
        _Mult ("Left or Right", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
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
            float4 _Color;
            float _WidthVar;
            float _Mult;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                float2 uv = i.uv;
                uv.x = i.uv.x - 0.5;
                uv.y = i.uv.y - 0.5;
                float2 zero = {1, 0};
                float deg = dot(uv, zero);
                float dist = sqrt(uv.x * uv.x + uv.y * uv.y);
                deg = acos(deg / dist);
                [branch] if(uv.y < 0)
                {
                    deg = 6.283185 - deg;
                }
                uv.x = 0.5 + cos(deg + _Mult) * dist;
                uv.y = 0.5 + sin(deg + _Mult) * dist;
                clip(_WidthVar - ((uv.x * 0.9) + 0.05) + sin(uv.y + _Time.y * 2) * 0.04);
                return _Color;
            }
            ENDCG
        }
    }
}