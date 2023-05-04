Shader "Custom/dis"
{
   Properties
    {
        _mainTex ("Albedo", 2D) = "defaulttexture" {}
        _emissionText ("Emission", 2D) = "defaulttexture" {}
        _dissolveAm ("display name", Range (0, 1)) = 0
    }

    SubShader
    {


        CGPROGRAM
         #pragma surface surf Lambert alpha


        struct Input {
            float2 uv_mainTex;
            float2 uv_dissolveTex;
        };

        sampler2D _mainTex;
        sampler2D _emissionText;
        float _dissolveAm;

        

        void surf(Input IN, inout SurfaceOutput o) {


           // o.Alpha = tex2D(_mainTex, IN.uv_mainTex);
            
       
          o.Albedo = tex2D(_mainTex, IN.uv_mainTex);
          float3 disF = tex2D(_emissionText, IN.uv_dissolveTex)
          clip(disF-_dissolveAm);
         
        }

        ENDCG
    }
        FallBack "Diffuse"
}