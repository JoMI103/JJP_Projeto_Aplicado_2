Shader "Custom/HOLOGRAM"
{
    Properties
   {
        _RimColor("rim color", Color) = (1,1,1,1)
         _Albedo("Albedo", 2D) = "defaulttexture" {}
         _Exponencial ("Exponencial", Range (-5, 10)) = 1
    }

    SubShader
    {
     //Cull off
        CGPROGRAM
            #pragma surface surf Lambert alpha:fade

        struct Input {
        float2 uv_Albedo;
        float3 viewDir;
        float2 uv_Text;
        };

        float3 _RimColor;
        sampler2D _Albedo;
        float _Exponencial;
        void surf(Input IN, inout SurfaceOutput o) {

        //Calculo do dot product

        float dotp=saturate(pow(1-dot(normalize(IN.viewDir),o.Normal),_Exponencial)) ;
        o.Emission=_RimColor;
        o.Albedo=tex2D(_Albedo,IN.uv_Albedo);
        o.Alpha = (dotp) ;

        }

        ENDCG
    }
}
