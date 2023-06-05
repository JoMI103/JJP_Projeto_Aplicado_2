Shader "Custom/aaaa"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap("normalmap", 2D) = "bump"{}
        _ScaleUV ("Scale", Range(1,20)) = 1

        
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent"}

        GrabPass{}
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
                float4 uvgrab : TEXCOORD1;
                float4 uvbump : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _GrabTexture;
            float _GrabTexture_TexelSize;

            sampler2D _MainTex;
            float4 _MainTex_ST;
    
   
            
       

         
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
              
                
                o.uvgrab.xy = (float2(o.vertex.x, -o.vertex.y) + o.vertex.w) * 0.5;
                o.uvgrab.zw = o.vertex.zw;
             
                o.uv = TRANSFORM_TEX( v.uv, _MainTex );
   
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab)); //* float4(0.5,0,0,1);
    
      
                return col;
            }
            ENDCG
        }
    }
}
