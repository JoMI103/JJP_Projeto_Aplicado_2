Shader "Custom/shader1"
{
   Properties
    {
        _mainTex ("Albedo", 2D) = "defaulttexture" {}
        _EmissionTex ("Emisison", 2D) = "defaulttexture" {}
        
    }

    SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert 


        struct Input {
        float2 uv_mainTex;
        float2 uv_emission;
        };

        sampler2D _mainTex;
        sampler2D _EmissionTex;
      

        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D(_mainTex, IN.uv_mainTex);
          //o.Alpha = 1;
        }

        ENDCG
    }
        FallBack "Diffuse"
}
