Shader "Unlit/SnowFlakesExplosion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorA ("color A", color) = (1,1,1,1)
        _ColorB ("color B", color) = (1,1,1,1)
        _ExplosionTime ("Time", Range(0,20)) = 0
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            }
        Pass {

            Cull Off
            ZWrite Off
            ZTest Lequal 
            Blend One One 
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ExplosionTime;
            float4 _ColorA, _ColorB;


            struct MeshData 
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.normal * _ExplosionTime);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
     
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                float Alpha = tex2D(_MainTex, i.uv).x; 
                float4 color =  lerp(_ColorA,_ColorB, Alpha);
                color.w = Alpha;
                return color;
              // return col;
            }
            ENDCG
        }
    }
}
