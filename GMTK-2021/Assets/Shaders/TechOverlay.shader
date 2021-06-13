Shader "Hidden/TechOverlay"
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
            int _Width;
            int _Height;
            float _Aspect;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(
                    tex2D(_MainTex, (i.uv - 0.5) * 1.005 + 0.5).r,
                    tex2D(_MainTex, (i.uv - 0.5) * 1.005 + 0.5).g,
                    tex2D(_MainTex, (i.uv - 0.5) * 1 + 0.5).b,
                    1.0);

                col.g *= 0.9F;
                col.r *= 1.15F;
                col.b *= 1.2F;
                col.rgb += 0.04F;

                if (floor((i.uv.y + _Time.x) * 288) % 2 == 0)
                    col.rgb *= 0.95F;
                
                return col;
            }
            ENDCG
        }
    }
}
