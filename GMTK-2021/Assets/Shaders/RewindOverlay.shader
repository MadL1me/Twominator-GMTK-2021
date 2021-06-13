Shader "Hidden/RewindOverlay"
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
            float _Strength;
            float _Aspect;

            float rand(float3 co)
             {
                 return frac(sin( dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
             }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 origColor = tex2D(_MainTex, i.uv);
                float2 uvr = (i.uv - 0.5) * (1 - distance(i.uv, float2(0.5, 0.5)) * 0.2 * min(1, _Intensity)) + 0.5;
                fixed4 col = tex2D(_MainTex, uvr);
                fixed intn = (col.r + col.g + col.b) / 3.0;
                intn = pow(intn, 1.2);
                intn *= 2.5 * _Intensity;
                col = fixed4(intn, intn, intn, 1.0);
                col *= 1.0 + rand(_Time.xyz + floor(i.uv.xy * float2(288 / _Aspect, 288)).xyx) * max(0, 1.0 - _Intensity) * 0.4;
                
                return lerp(origColor, col, _Strength);
            }
            ENDCG
        }
    }
}
