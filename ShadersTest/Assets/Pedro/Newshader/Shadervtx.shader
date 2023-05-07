Shader "Custom/Shadervtx"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Slider ("Slider", Range(-1,1)) = 0
        _SecondTex ("SecondTexture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            sampler2D _SecondTex;
            float4 _MainTex_ST;
            float _Slider;

            float3 vertPass;
            v2f vert (appdata v)
            {
                v2f o;


                o.vertexOriginal = v.vertex;

                //a
                if(v.vertex.x > _Slider){
                v.vertex.x = _Slider;
                }
                //a


                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
           
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                
                //a
                if(i.vertexOriginal.x < _Slider){
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
                }
                else {
                //i.vertexOriginal.x = _Slider;
                fixed4 col = tex2D(_SecondTex, i.uv);
               
                return col;
                }
                //a


                //a
                //if(i.vertexOriginal.x < _Slider){
                //discard;
                //}
                //a
                
                //return col;
            }
            ENDCG
        }
    }
}

