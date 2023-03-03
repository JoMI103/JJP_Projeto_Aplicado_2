Shader "Exer1/DangerEmission"
{
    Properties
    {
        _mainTex ("Albedo", 2D) = "defaulttexture" {}
        _emissionText ("Emission", 2D) = "defaulttexture" {}
        _playerDistance ("display name", Float) = 0
    }

    SubShader
    {
        Cull Off

        CGPROGRAM
         #pragma surface surf Lambert alpha


        struct Input {
            float2 uv_mainTex;
            float2 uv_emissionText;
        };

        float _playerDistance;
        sampler2D _mainTex;
        sampler2D _emissionText;

        float2 RotateImg(float2 ponto, float deg){
            //deg = Radians(deg);
            float2x2 matRotacao = {cos(deg), -sin(deg),
                                   sin(deg), cos(deg)};

           return mul(matRotacao, ponto);
           }

        void surf(Input IN, inout SurfaceOutput o) {

            
                 o.Emission.x = tex2D(_emissionText, IN.uv_emissionText) * _playerDistance;
            
         //float2 uvRodadas =  RotateImg(IN.uv_mainTex, _Time.a);
          o.Albedo = tex2D(_mainTex, IN.uv_mainTex);
          o.Alpha = tex2D(_mainTex, IN.uv_mainTex).a;
          
         
          
        }

        ENDCG
    }
        FallBack "Diffuse"
}
