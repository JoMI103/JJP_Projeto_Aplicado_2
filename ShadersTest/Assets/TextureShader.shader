Shader "Custom/TextureShader"
{
     Properties
     {
       _Textura ("Texture", 2D) = "defaulttexture" {}
       _TexturaEmissao ("Texture Emission", 2D) = "defaulttexture" {}
       _Slider ("Color Multiplier", Range (-10, 10)) = 1
     }

   SubShader{
     CGPROGRAM
        #pragma surface surf Lambert

     struct Input {

        float2 uv_Textura;
        float2 uv_TexturaEmissao;
        };


     sampler2D _Textura;
     sampler2D _TexturaEmissao;
     float _Slider;

        void surf(Input IN, inout SurfaceOutput o) {
            //o.Albedo.rgb = (tex2D(_Textura, IN.uv_Textura).rgb) * _Slider;\

            IN.uv_Textura.x *= _Slider;
            o.Albedo.rgb = tex2D(_Textura, IN.uv_Textura ).rgb ;
            o.Emission = float3(0,0,1);



        }

        ENDCG


    }

        FallBack "Diffuse"
}