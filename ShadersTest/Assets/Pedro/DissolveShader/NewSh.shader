Shader "Custom/NewSh"
{
    Properties
    {
    _SpecColor("cor specolor", Color) = (1,1,1,1)
         _mainTex ("main tex", 2D) = "defaulttexture" {}
         _Specular("specular",Range(0,1)) = 0
         _Gloss("Gloss", Range(0,1)) = 0
    }

    SubShader
    {
     Cull off
        CGPROGRAM
        #pragma surface surf BlinnPhong 


        struct Input {
        float2 uv_mainTex;
        };

        float _Specular;
        float _Gloss;
        //SpecColor

         sampler2D _mainTex;
        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D(_mainTex,IN.uv_mainTex);
          o.Gloss = _Gloss;
          o.Specular = _Specular;
        }

        ENDCG
    }
        FallBack "Diffuse"
}

