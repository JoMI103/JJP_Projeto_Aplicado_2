Shader "Unlit/ExpandSides"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _slide("Slide Amount", Range(0, 1)) = 1

        _ExpText("ExpText", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always
        ZWrite On
       
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
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

             sampler2D _MainTex;
            fixed4 _Color;
            sampler2D _ExpText;
            //float _slide;

            v2f vert (appdata v)
            {
                v2f o;
                //v.vertex += v.normal * _slide;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

           

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_ExpText, i.uv);                      
                //return fixed4(1,0,0,0); //retorna vermelho
                return col;
                
            }
            ENDCG
        }


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
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

             sampler2D _MainTex;
            fixed4 _Color;
            float _slide;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex += v.normal * _slide;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

           

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);                      
                return col * _Color;
                
            }
            ENDCG
        }

        
    }
}
