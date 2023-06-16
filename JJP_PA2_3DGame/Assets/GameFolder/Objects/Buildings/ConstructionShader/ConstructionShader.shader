Shader "Unlit/ConstructionShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _TranspMod ("Transparent Modifier", Range(0,1)) = 0
        _cRadius("construction Radius", float) = 3
        _Generate ("Generate construction", Range(0,1)) = 0
 
        _NoiseTex ("Noise Texture", 2D) = "white" {} 
        _ColorA ("Color A", Color) = (0,0,0,1)
        _ColorB ("Color B", Color) = (0,0,0,1)
        _Bloom ("Bloom", Range(1,3)) = 1
        
        _tScale1 ("Triplanar Scale 1", Range(0,100)) = 0
        _Scale("ScaleOftheObject", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        cull off
        Pass
        {
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 localVertex : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 vertexWorld : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TranspMod,_Generate,_cRadius,_Bloom;

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float4 _ColorA, _ColorB;
            float _tScale1, _Scale;


            Interpolators vert (MeshData v) {
                Interpolators o;
                
                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                o.localVertex = v.vertex * _Scale;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }

            
            fixed4 frag (Interpolators i) : SV_Target {
                float noiseValue = tex2D(_NoiseTex, i.localVertex.xz / (_cRadius * 2 ) + float2(0.5,0.5)).x;
                float p =  lerp(0,_cRadius*2,_Generate);
                float distanceToCenter = distance(float3(0,0,0),i.localVertex);

                
                if(distanceToCenter > p){
                    return 0;
                }else{
                    if(distanceToCenter < p -0.05){
                        
                        float3 triplanarUV = i.localVertex * _tScale1;
                        float3 peso =pow(abs(i.worldNormal), 10) ;
                        
                        peso /= dot(peso, 1);

                        float4 projX = tex2D(_MainTex, triplanarUV.yz);
                        float4 projY = tex2D(_MainTex, triplanarUV.xz);
                        float4 projZ = tex2D(_MainTex, triplanarUV.xy);

                        fixed4 col  = projX* peso.x+ projY* peso.y  + projZ* peso.z ;
                        
                        col.w = col.z + _TranspMod;
                        
                        return col;
                    }else{
                        _ColorA.xyz *=_Bloom;
                        _ColorB.xyz *=_Bloom;
                        
                        return lerp(_ColorA , _ColorB , noiseValue); 
                    }
                } 
            }
            ENDCG
        }
    }
}
