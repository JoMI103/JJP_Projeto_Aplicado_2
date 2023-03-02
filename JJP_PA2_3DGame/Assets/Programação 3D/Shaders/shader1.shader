Shader "Custom/shader1"
{
   Properties
    {
        _MainTex ("Albedo", 2D) = "defaulttexture" {}
        _EmisionTex ("Emision", 2D) = "defaulttexture" {}
        
    }

    SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert 


        struct Input {
        float2 uvMainTex;
        };

      

        void surf(Input IN, inout SurfaceOutput o) {
          
        }

        ENDCG
    }
        FallBack "Diffuse"
}
