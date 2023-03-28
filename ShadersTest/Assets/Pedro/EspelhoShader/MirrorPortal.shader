Shader "Custom/MirrorPortal"
{
     Properties
    {
        _mainTex ("Albedo", 2D) = "defaulttexture" {}
        _mainTex2 ("Albedo", 2D) = "defaulttexture" {}
    }

    SubShader
    {
        Cull off

        CGPROGRAM
         #pragma surface surf Lambert alpha


        struct Input {
            float2 uv_mainTex;
             
        };

        sampler2D _mainTex;
        sampler2D _mainTex2;

        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D(_mainTex, IN.uv_mainTex);
          o.Alpha = tex2D(_mainTex2, IN.uv_mainTex);



        }

        ENDCG
    }
        FallBack "Diffuse"
}
