Shader "fridvince/UIDistortionMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TopLeftOffset ("Top Left Offset", Vector) = (0,0,0,0)
        _TopRightOffset ("Top Right Offset", Vector) = (0,0,0,0)
        _BottomLeftOffset ("Bottom Left Offset", Vector) = (0,0,0,0)
        _BottomRightOffset ("Bottom Right Offset", Vector) = (0,0,0,0)
        _StencilRef ("Stencil Reference", Range(0, 255)) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "UI" "Queue" = "Transparent" }
        Pass
        {
            Stencil
            {
                Ref [_StencilRef]
                Comp always
                Pass replace
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float2 _TopLeftOffset;
            float2 _TopRightOffset;
            float2 _BottomLeftOffset;
            float2 _BottomRightOffset;

            v2f vert (appdata_t v)
            {
                v2f o;
                float2 uv = v.uv;

                float2 topLeftOffset = _TopLeftOffset;
                float2 topRightOffset = _TopRightOffset;
                float2 bottomLeftOffset = _BottomLeftOffset;
                float2 bottomRightOffset = _BottomRightOffset;

                float2 offset = lerp(
                    lerp(bottomLeftOffset, topLeftOffset, uv.y),
                    lerp(bottomRightOffset, topRightOffset, uv.y),
                    uv.x
                );

                o.pos = UnityObjectToClipPos(v.vertex);
                o.pos.xy += offset;
                o.uv = uv;
                o.color = v.color;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);
                return texColor * i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}