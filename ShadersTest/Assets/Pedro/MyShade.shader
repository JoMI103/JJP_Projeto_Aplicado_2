Shader "Custom/MyShade"
{
    Properties
    {
        _RimColor("rim color", Color) = (1,1,1,1)
         _Albedo("Albedo", 2D) = "defaulttexture" {}
         _Exponencial ("Exponencial", Range(-3, 10)) = 1

         _Transparency("Transparencia", Range(0,1)) = 1
        _WorldPositionCutoffSuperior("Cut-off no Mundo Positivo", Range(-100, 100)) = 0.5
        _WorldPositionCutoffInferior("Cut-off no Mundo Negativo", Range(-100, 100)) = 0.5
        _NumeroDeTiras("Intensidade das Tiras", Range(0,1)) = 0
    }

    SubShader
    {
     //Cull off
        CGPROGRAM
            #pragma surface surf Lambert 

        struct Input {
        float2 uv_Albedo;
        float3 viewDir;
         float3 worldPos;
        float2 uv_Text;
        float2 uv_texture;
        };

            fixed _Transparency;
    float _WorldPositionCutoffSuperior;
    float _WorldPositionCutoffInferior;
    float _NumeroDeTiras;

        float3 _RimColor;
        sampler2D _Albedo;
        float _Exponencial;
        void surf(Input IN, inout SurfaceOutput o) {
        o.Albedo = IN.worldPos;
            o.Alpha = 1;
        //Calculo do dot product  valor entre -1 e 1 de acordo como nos olhamos

        float dotp=pow(1-dot(normalize(IN.viewDir),o.Normal),_Exponencial) ;

        o.Emission=_RimColor* dotp;
        o.Albedo=tex2D(_Albedo,IN.uv_Albedo);
         if (IN.worldPos.y >= _WorldPositionCutoffInferior && IN.worldPos.y <= _WorldPositionCutoffSuperior) {

                o.Alpha = 1 - _Transparency;
            }

            if (abs(IN.worldPos.y % 2) >= _NumeroDeTiras) {
                o.Albedo = tex2D(_Albedo, IN.uv_texture);
                //o.Alpha = 0;
            }
        }

        ENDCG
    }
        FallBack "Diffuse"
}
