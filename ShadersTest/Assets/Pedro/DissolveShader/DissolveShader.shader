Shader "Custom/DissolveShader"
{
      Properties
    {
        _mainTex ("Albedo", 2D) = "defaulttexture" {}
        _dissolveTex ("Albedo", 2D) = "defaulttexture" {}
        _dissolveAmount("Dissolve Amount", Range(0,1)) = 1
    }

    SubShader
    {
        Cull off

        CGPROGRAM
         #pragma surface surf Lambert addshadow


        struct Input {
            float2 uv_mainTex;
            float2 uv_dissolveTex;
             
        };

        sampler2D _mainTex;
        sampler2D _dissolveTex;
        float _dissolveAmount;

        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D(_mainTex, IN.uv_mainTex);
          
          float3 dissolveF = tex2D(_dissolveTex, IN.uv_dissolveTex);
          clip(dissolveF - _dissolveAmount); //se isto der negativo, tira o pixel em que tá negativo



        }

        ENDCG
    }
        FallBack "Diffuse"
}
