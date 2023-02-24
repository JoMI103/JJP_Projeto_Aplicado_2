Shader "Custom/NewShader"
{
    Properties
    {
        _Textura ("Main Texture", 2D) = "defaulttexture" {}
        _TexturaEmissao ("Texture Emission", 2D) = "defaulttexture" {}
        _Slider  ("display name", Range (-1, 10)) = 1
    }
    SubShader
    {
        CGPROGRAM

        #pragma surface surf Lambert



        struct Input
        {
            float2 uv_Textura;
            float2 uv_TexturaEmissao;
        };

        sampler2D _Textura;
        sampler2D _TexturaEmissao;
        float _Slider;
      

        void surf (Input IN, inout SurfaceOutput o)
        {
            //o.Albedo = (tex2D(_Textura, IN.uv_Textura).rgb) * _Slider ;

            //IN.uv_Textura.x *= _Slider;
            //o.Albedo = tex2D(_Textura, IN.uv_Textura).rgb;
            //o.Emission.rg = tex2D(_TexturaEmissao, IN.uv_Textura).rgb * 0.2f;


            o.Albedo =( tex2D(_Textura, IN.uv_Textura).rgb +_Slider *tex2D(_TexturaEmissao, IN.uv_Textura).rgb);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
