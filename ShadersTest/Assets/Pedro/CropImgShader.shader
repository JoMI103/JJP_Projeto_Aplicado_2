Shader "Custom/CROPIM"
{
     Properties
    {
        _mainTex ("Albedo", 2D) = "defaulttexture" {}
        _emissionText ("Emission", 2D) = "defaulttexture" {}
    }

    SubShader
    {
        Cull off

        CGPROGRAM
         #pragma surface surf Lambert alpha


        struct Input {
            float2 uv_mainTex;
            float2 uv_emissionText;
        };

        sampler2D _mainTex;
        sampler2D _emissionText;

        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D(_mainTex, IN.uv_mainTex);
          o.Alpha = tex2D(_mainTex, IN.uv_mainTex).a;

          o.Emission.x = tex2D(_emissionText, IN.uv_emissionText).a;
        }

        ENDCG
    }
        FallBack "Diffuse"
}

