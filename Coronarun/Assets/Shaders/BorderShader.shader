Shader "Custom/BorderShader"
{
    Properties
    {
        _OutTex ("Outline Texture", 2D) = "black" {}
        _OutColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth("Outline Width", Range(0.0, 1.0)) = 0.03
        _Transparency("Transparency", Range(0.0, 1.0)) = 0.25
    }
    SubShader
    {

        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100

        Pass
        {
            Name "OUTLINE"

            Cull Front
            ZWrite On
            //ZTest Less
            Blend SrcAlpha OneMinusSrcAlpha


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _OutTex;
            float4 _OutTex_ST;
            float _Transparency;
            float1 _OutlineWidth;
            fixed4 _OutColor;

            v2f vert (appdata IN)
            {
                v2f OUT;
                //IN.vertex.xyz += ((IN.color * 2.0) - 1.0) * _OutlineWidth;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.color.xyz = _OutColor;
                OUT.color.w = _Transparency;
                return OUT;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                return IN.color;
            }
            ENDCG
        }
    }
}
