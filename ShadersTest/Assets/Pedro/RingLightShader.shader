Shader "Custom/RingLightShader"
{
    Properties
    {
        _RimColor("rim color", Color) = (1,1,1,1)
         _Albedo("Albedo", 2D) = "defaulttexture" {}
         _Exponencial ("Exponencial", Range(-3, 10)) = 1
    }

    SubShader
    {
     //Cull off
        CGPROGRAM
            #pragma surface surf Lambert 

        struct Input {
        float2 uv_Albedo;
        float3 viewDir;
        float2 uv_Text;
        };

        float3 _RimColor;
        sampler2D _Albedo;
        float _Exponencial;
        void surf(Input IN, inout SurfaceOutput o) {

        //Calculo do dot product  valor entre -1 e 1 de acordo como nos olhamos

        float dotp=pow(1-dot(normalize(IN.viewDir),o.Normal),_Exponencial) ;
        o.Emission=_RimColor* dotp;
        o.Albedo=tex2D(_Albedo,IN.uv_Albedo);

        }

        ENDCG
    }
        FallBack "Diffuse"
}
