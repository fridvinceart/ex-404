Shader "fridvince/UIDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _TopLeftOffset ("Top Left Offset", Vector) = (0,0,0,0)
        _TopRightOffset ("Top Right Offset", Vector) = (0,0,0,0)
        _BottomLeftOffset ("Bottom Left Offset", Vector) = (0,0,0,0)
        _BottomRightOffset ("Bottom Right Offset", Vector) = (0,0,0,0)

        _BorderLeft ("Border Left (0-1)", Range(0,1)) = 0.25
        _BorderBottom ("Border Bottom (0-1)", Range(0,1)) = 0.25
        _BorderRight ("Border Right (0-1)", Range(0,1)) = 0.75
        _BorderTop ("Border Top (0-1)", Range(0,1)) = 0.75

        _UseAlphaCutoff ("Use Alpha Cutoff", Float) = 1.0
        _AlphaCutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="UI" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;

            float2 _TopLeftOffset;
            float2 _TopRightOffset;
            float2 _BottomLeftOffset;
            float2 _BottomRightOffset;

            float _BorderLeft;
            float _BorderBottom;
            float _BorderRight;
            float _BorderTop;

            float _UseAlphaCutoff;
            float _AlphaCutoff;

            v2f vert (appdata_t v)
            {
                v2f o;
                float2 uv = v.uv;
                float4 pos = UnityObjectToClipPos(v.vertex);

                bool isTopLeft = (uv.x <= _BorderLeft) && (uv.y >= _BorderTop);
                bool isTopRight = (uv.x >= _BorderRight) && (uv.y >= _BorderTop);
                bool isBottomLeft = (uv.x <= _BorderLeft) && (uv.y <= _BorderBottom);
                bool isBottomRight = (uv.x >= _BorderRight) && (uv.y <= _BorderBottom);

                float2 offset = float2(0, 0);

                if (isTopLeft)
                    offset += _TopLeftOffset.xy;
                else if (isTopRight)
                    offset += _TopRightOffset.xy;
                else if (isBottomLeft)
                    offset += _BottomLeftOffset.xy;
                else if (isBottomRight)
                    offset += _BottomRightOffset.xy;

                pos.xy += offset;

                o.pos = pos;
                o.uv = uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);

                if (_UseAlphaCutoff > 0.5 && texColor.a < _AlphaCutoff)
                    discard;

                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}