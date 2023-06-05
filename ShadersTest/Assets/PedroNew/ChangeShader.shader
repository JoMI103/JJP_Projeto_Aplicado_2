Shader "Custom/ChangeShader"
{
     Properties
    {     
        _Strength("Distort Strength", Range(0,10)) = 1.0
        _Noise("Noise Texture", 2D) = "white" {}

        _slider("slide", Range(0,0.1)) = 0
             _sliderINTE("slideInt", Range(0,100)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
          
        GrabPass{}
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

                float4 uvgrab : TEXCOORD1;
            };

            sampler2D _Noise;
            float4 _Noise_ST;
            float _Strength;

            sampler2D _GrabTexture;
            float _GrabTexture_TexelSize;
            float4 _GrabTexture_ST;
            
            float _slider;
            float _sliderINTE;
            sampler2D _CameraDepthTexture ;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _Noise);

                o.uvgrab.xy = (float2(o.vertex.x, -o.vertex.y) + o.vertex.w) * 0.5;
                o.uvgrab.zw = o.vertex.zw;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
             fixed4 col = tex2Dproj( _CameraDepthTexture, UNITY_PROJ_COORD(i.uvgrab)) ;
             
             fixed4 col2 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab)) ;
            
                col.x*=_sliderINTE;
                col2.g += col.r;
                return col2;
            }
            ENDCG
        }
}
}