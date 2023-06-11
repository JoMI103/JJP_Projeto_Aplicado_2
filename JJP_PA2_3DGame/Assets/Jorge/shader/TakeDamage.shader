Shader "Custom/TakeDamage"
{
    Properties
    {
        _TintColor("Tint Color", Color) = (1, 0, 0, 1)
        _EdgeThickness("Edge Thickness", Range(0.01, 0.5)) = 0.1
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _TintColor;
            float _EdgeThickness;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                
                float2 screenPos = i.vertex.xy / i.vertex.w;                
                float distance = 1.0 - length(screenPos);
                float edge = smoothstep(_EdgeThickness, 0.0, distance);
                fixed4 c = tex2D(_MainTex, i.uv) * _TintColor;              
                c.rgb *= edge;
                return c;
            }
            ENDCG
        }
    }
        FallBack "Diffuse"
}