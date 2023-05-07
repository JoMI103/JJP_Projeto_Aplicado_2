Shader "Custom/v2f"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _MainTex2("Texture_Interior", 2D) = "white" {}
        _slider("slider", Range(-1, 1)) = 1
        _slider2("slider_white", Range(0, 0.6)) = 0.01
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100
            Blend SrcAlpha OneMinusSrcAlpha
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

                    float3 vertexOriginal : TEXCOORD1;
                    float3 vertexWorld : TEXCOORD2;


                };

                sampler2D _MainTex;
                sampler2D _MainTex2;
                float4 _MainTex_ST;
                float4 _MainTex2_ST;


                float3 vertPass;
                float _slider;
                float _slider2;

                v2f vert(appdata v)
                {
                    v2f o;


                    o.vertexOriginal = v.vertex;
                    if (o.vertexOriginal.x < _slider){
                        v.vertex.x = _slider;
                        
                    }
                    o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed4 col2 = tex2D(_MainTex2, i.uv);

                    if (i.vertexOriginal.x < _slider) {
                       
                        if (i.vertexOriginal.x > 0) {

                        }
                        if (abs(i.vertexOriginal.x) <= abs(_slider - _slider2)) return fixed4(1, 1, 1, 1);
                        
                        if (i.vertex.x < 0) col2; 

                        return col2;
                    }
                    
                    return col;
                }
                ENDCG
            }
        }
}