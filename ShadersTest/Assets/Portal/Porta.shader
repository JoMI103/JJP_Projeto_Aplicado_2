Shader "Custom/Porta"
{
   Properties
    {
        _mainTex ("Albedo", 2D) = "defaulttexture" {}
        _emissionText ("Emission", 2D) = "defaulttexture" {}
    }

    SubShader
    {


        CGPROGRAM
         #pragma surface surf Lambert alpha


        struct Input {
            float2 uv_mainTex;
            float2 uv_emissionText;
        };

        sampler2D _mainTex;
        sampler2D _emissionText;


        float2 RotateImg(float2 ponto, float deg){
        
            //deg= radians(deg);

            float2x2 matRotacao = { cos(deg), -sin(deg),
                                    sin(deg), cos(deg) };                      
        
            return mul(matRotacao, ponto);
        }

        void surf(Input IN, inout SurfaceOutput o) {


            o.Alpha = tex2D(_mainTex, IN.uv_mainTex);
            
       
          o.Albedo = tex2D(_emissionText, IN.uv_mainTex);


         
        }

        ENDCG
    }
        FallBack "Diffuse"
}