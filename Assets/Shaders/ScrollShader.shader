Shader "Unlit/ScrollShader"
{
    Properties
    {
        _Color ("Colour (RGBA)", Color) = (1,1,1,1)
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Offset("Offset", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector" = "True" "PreviewType"="Plane" }
        LOD 300

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest LEqual
        Lighting Off
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
            fixed4 _Color;
            half2 _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv + _Offset;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return _Color * col;
            }
            ENDCG
        }
    }
}
