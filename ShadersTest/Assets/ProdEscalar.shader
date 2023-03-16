Shader "Custom/ProdEscalar"
{Properties
    {
        _Albedo("Albedo", 2D) = "defaulttexture" {}
        _Area("Area",Range(0,1))=0
        _Text("TExture", 2D) = "defaulttexture" {}

    }

    SubShader
    {
     //Cull off
        CGPROGRAM
            #pragma surface surf Lambert 

        struct Input {
        float2 uv_Albedo;
        float3 viewDir;
        float2 uv_Text;
        };

        sampler2D _Albedo;
        fixed _Area;
        sampler2D _Text;
        void surf(Input IN, inout SurfaceOutput o) {

        if(dot(normalize(IN.viewDir),o.Normal)<_Area)
            discard;
          //o.Albedo = tex2D(_Albedo,IN.uv_Albedo);
          else 
          o.Albedo= tex2D(_Text,IN.uv_Text);

        }

        ENDCG
    }
        FallBack "Diffuse"
}
