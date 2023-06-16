Shader "Unlit/BulletTrail"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        [Normal] _DistortionGuide("Distortion guide", 2D) = "bump" {}
        _DistortionAmount("Distortion amount", float) = 0
        _Value("value", Range(0,1)) = 0
        _Velocity ("Velocity", float) = 1
        _Size("Size", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Cull off    
        GrabPass
        {
            "_GrabTexture"
        }
        Pass {

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
             
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators{
                float2 uv : TEXCOORD0;
                float4 grabPassUV : TEXCOORD1;
                float2 distortionUV : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DistortionGuide;
            float4 _DistortionGuide_ST;
            
            sampler2D _GrabTexture;
            float _Velocity, _Size,_Value ,_DistortionAmount;
            
            float InverseLerp(float a ,float b, float v){
                return (v-a)/(b-a);
            }

            
            Interpolators vert(MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.grabPassUV = ComputeGrabScreenPos(o.vertex);
                o.distortionUV = TRANSFORM_TEX(v.uv, _DistortionGuide);
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                
                i.uv.y = length(InverseLerp(_Value - _Size , _Value + _Size, i.uv.y));
                fixed4 mask = tex2D(_MainTex, i.uv);
                float2 distortion = UnpackNormal(tex2D(_DistortionGuide, i.distortionUV)).xy;
                distortion *= _DistortionAmount * mask ;
                i.grabPassUV.xy += distortion * i.grabPassUV.z;
                
    
                fixed4 col = tex2Dproj(_GrabTexture, i.grabPassUV);
                
                if(i.uv.y > 0 && i.uv.y < 1){ 
                    
                    return col * (1-i.uv.y); 
                
                } 
                else { return 0; }
            }
            ENDCG
        }
    }
}
