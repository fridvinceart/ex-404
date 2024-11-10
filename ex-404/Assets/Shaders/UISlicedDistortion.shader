Shader "fridvince/UISlicedDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        // Distortion Offsets for corners
        _TopLeftOffset ("Top Left Offset", Vector) = (0,0,0,0)
        _TopRightOffset ("Top Right Offset", Vector) = (0,0,0,0)
        _BottomLeftOffset ("Bottom Left Offset", Vector) = (0,0,0,0)
        _BottomRightOffset ("Bottom Right Offset", Vector) = (0,0,0,0)

        // Slicing Borders (Normalized UV Coordinates)
        _BorderLeft ("Border Left (0-1)", Range(0,1)) = 0.25
        _BorderBottom ("Border Bottom (0-1)", Range(0,1)) = 0.25
        _BorderRight ("Border Right (0-1)", Range(0,1)) = 0.75
        _BorderTop ("Border Top (0-1)", Range(0,1)) = 0.75

        // Alpha Cutoff
        _UseAlphaCutoff ("Use Alpha Cutoff", Float) = 1.0 // 1 = enabled, 0 = disabled
        _AlphaCutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="UI" "Queue"="Transparent" } // Use Overlay for better compatibility
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Default blend mode for UI
            ZWrite Off // Disable depth writing for UI
            Cull Off // Disable culling to show both sides

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

            // Distortion offsets for corners
            float2 _TopLeftOffset;
            float2 _TopRightOffset;
            float2 _BottomLeftOffset;
            float2 _BottomRightOffset;

            // Slicing Borders
            float _BorderLeft;
            float _BorderBottom;
            float _BorderRight;
            float _BorderTop;

            float _UseAlphaCutoff; // Flag to control alpha cutoff
            float _AlphaCutoff;

            v2f vert (appdata_t v)
            {
                v2f o;
                float2 uv = v.uv;
                float4 pos = UnityObjectToClipPos(v.vertex);

                // Determine if the vertex is in a specific region (corners)
                bool isTopLeft = (uv.x <= _BorderLeft) && (uv.y >= _BorderTop);
                bool isTopRight = (uv.x >= _BorderRight) && (uv.y >= _BorderTop);
                bool isBottomLeft = (uv.x <= _BorderLeft) && (uv.y <= _BorderBottom);
                bool isBottomRight = (uv.x >= _BorderRight) && (uv.y <= _BorderBottom);

                float2 offset = float2(0, 0);

                // Apply the respective offset based on the region (corners)
                if (isTopLeft)
                    offset += _TopLeftOffset.xy;
                else if (isTopRight)
                    offset += _TopRightOffset.xy;
                else if (isBottomLeft)
                    offset += _BottomLeftOffset.xy;
                else if (isBottomRight)
                    offset += _BottomRightOffset.xy;

                // Apply offset to the vertex position
                pos.xy += offset;

                o.pos = pos;
                o.uv = uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);

                // If alpha cutoff is enabled, apply it
                if (_UseAlphaCutoff > 0.5 && texColor.a < _AlphaCutoff)
                    discard;

                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}