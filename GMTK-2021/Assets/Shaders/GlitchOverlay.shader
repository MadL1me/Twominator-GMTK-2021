Shader "Hidden/GlitchOverlay"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Intensity;

            float rand(float3 co)
             {
                 return frac(sin( dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
             }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvr = i.uv;
                float timeGap = floor(_Time.x * 2400);
                
                for (int j = 0; j < _Intensity * 10; j++)
                {
                    float width = rand(timeGap + j);
                    float height = rand(timeGap + j + 0.5);
                    float x = rand(timeGap + j + 0.25) * 2 - 1;
                    float y = rand(timeGap + j + 0.75) * 2 - 1;
                    float2 off = float2(rand(timeGap + j + 0.1) * 2 - 1, rand(timeGap + j + 0.2) * 2 - 1);

                    if (i.uv.x >= x && i.uv.y >= y && i.uv.x < x + width && i.uv.y < y + height)
                        uvr += off;
                }

                fixed4 col = tex2D(_MainTex, uvr);
                
                return col;
            }
            ENDCG
        }
    }
}
