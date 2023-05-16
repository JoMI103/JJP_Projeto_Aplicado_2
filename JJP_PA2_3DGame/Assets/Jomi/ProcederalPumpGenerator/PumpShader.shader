Shader "Unlit/PumpShader"
{
    Properties{
        _MainTex ("Texture", 2D) = "white" {}
        _Size ("SphereSize", Range(0,1)) = 0.01
        _Scale ("SphereScale", Range(0.1,10)) = 1
        _uvCoords ("_uvCoords", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
      
            #include "UnityCG.cginc"
            
            
            #define TAU 6.28318530718


            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Scale,_Size,_uvCoords;

            
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
                float3 worldPos: TEXCOORD1;
            };

            float InverseLerp(float a ,float b, float v){
                return (v-a)/(b-a);
            }

            
            Interpolators vert (MeshData v)
            {
                Interpolators o;
                
                float minThreshold =  saturate(1- _Time.x) - _Size; 
                //float minThreshold = _uvCoords - _Size;
                float maxThreshold = saturate(1- _Time.x )+ _Size; 
                // float maxThreshold = _uvCoords + _Size;
                
                float mask = v.uv.y  < minThreshold || v.uv.y >maxThreshold;
                float t = 0;
           
                if(mask != 1)
                {
          
                    float pos = saturate(InverseLerp(minThreshold ,maxThreshold, v.uv.y ));
                    t = saturate(-cos(pos * TAU));
                    t = (1-mask) * t;
                    t *=_Scale;
                
                }


                     v.vertex.xyz += (v.normal  * t);
                o.vertex = UnityObjectToClipPos( v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                
                
                fixed4 col = tex2D(_MainTex, i.worldPos);
                return col;
            }
            ENDCG
        }
    }
}
