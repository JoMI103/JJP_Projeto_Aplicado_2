Shader "Unlit/Burn 1"
{
    Properties {
        _MainTex("Main Texture", 2D) = "white" {}
        _BurnTex("Burn Texture", 2D) = "white" {}
        _Value("Value", Range(0, 1)) = 0.1
        _Fresnel("Fresnel Power", Range(0, 10)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float viewDirDotNormal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BurnTex;
            float4 _BurnTex_ST;
            float _Value;
            float _Fresnel;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
              
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 viewDir = UnityWorldSpaceViewDir(worldPos);
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);

                o.viewDirDotNormal = dot(viewDir, worldNormal);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 col1 = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_BurnTex, i.uv);
                float fresnel = 1.0 + saturate(pow(i.viewDirDotNormal, _Fresnel));
                fixed4 finalColor = lerp(col1, col2, _Value) * fresnel;
                return finalColor;
            }
            ENDCG
        }
    }
}