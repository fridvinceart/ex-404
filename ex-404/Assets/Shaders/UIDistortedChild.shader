Shader "fridvince/DistortedChildMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaCutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
        _StencilRef ("Stencil Reference", Range(0, 255)) = 1 // Add the stencil reference property
    }
    SubShader
    {
        Tags { "RenderType" = "UI" "Queue" = "Transparent" }

        Pass
        {
            Stencil
            {
                Ref [_StencilRef] // Correct reference to the stencil property
                Comp equal // Only pass where stencil value is equal to the reference
                Pass keep // Keep the existing stencil value
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // Color from Unity's Image component
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // Pass color to fragment shader
            };

            sampler2D _MainTex;
            float _AlphaCutoff; // Alpha cutoff value

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color; // Pass the vertex color from Unity's Image component
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);

                // Discard pixels where the alpha is below the cutoff threshold
                if (texColor.a < _AlphaCutoff)
                {
                    discard;
                }

                // Multiply the texture color with the color from the Image component (vertex color)
                return texColor * i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}