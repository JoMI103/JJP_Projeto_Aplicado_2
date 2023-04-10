Shader "Custom/NewSurfaceShader"
{
     Properties
    {
     _Front ("Front ", 2D) = "defaulttexture" {}
     _Back ("Back", 2D) = "defaulttexture" {}
    }

    SubShader
    {

      //Segundo pass Corresponde a parte de Tras
        Cull Front
        CGPROGRAM
        #pragma surface surf Lambert alpha 


        struct Input {
        float2 uv_Front;
        };

      
        sampler2D _Back;
        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D(_Back, IN.uv_Front);
          o.Alpha = tex2D(_Back, IN.uv_Front).a;
        }

        ENDCG

        //Corresponde FRENTE
        Cull Back
        CGPROGRAM
        #pragma surface surf Lambert alpha 


        struct Input {
        float2 uv_Front;
        };

      
        sampler2D _Front;
        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D(_Front, IN.uv_Front);
          o.Alpha = tex2D(_Front, IN.uv_Front).a;
        }

        ENDCG

      

    }
        FallBack "Diffuse"
}
