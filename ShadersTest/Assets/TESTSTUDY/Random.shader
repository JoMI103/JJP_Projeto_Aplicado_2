Shader "Unlit/Random"
{
     Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _RecTxt ("Rec Texture", 2D) = "white" {}
        _RandomTxt ("Random Texture", 2D) = "white" {}
    }
    SubShader
    {
            Tags { "RenderType"="Opaque" }
        LOD 100

        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

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
            sampler2D _RandomTxt;
            sampler2D _RecTxt;

            float randomit(in float2 uv)
            {
            float2 noise = (frac(sin(dot(uv ,float2(12.9898,78.233)*2.0)) * 43758.5453));
             return abs(noise.x + noise.y) * 0.5;
            }


            fixed4 frag (v2f i) : SV_Target
            {
            
                float2 newUV = randomit(i.uv);

                newUV.y += _Time.y;
                fixed4 disTex = tex2D(_RandomTxt, newUV); //* frac(sin(dot(i.uv,float2(12.9898,78.233)))*  43758.5453123))
                         
      
                fixed4 col2 = tex2D(_MainTex, i.uv) * disTex ;
                col2.rgb = col2.r+col2.g+col2.z/3;

                fixed4 col = tex2D(_RecTxt, i.uv);   
                
                //if(col.a  > 0) {
                // 
                //return col;}
               if(col.a>0) return col;
                return col2;
            }
            ENDCG
        }
    }
}
