Shader "Custom/NormalMapShader"
{
   Properties
    {
        
        _Albedo ("Albedo", 2D) = "defaulttexture" {}
        
        [Toggle]
        _ActiveNormal("Active normal",Int) = 0
        
        [Normal]
        _MapaNormal("Mapa normal",2D) = "defaulttexture" {}
    }

    SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert 


        struct Input {
            float2 uv_Albedo;
            float2 uv_MapaNormal;
        };
        
        sampler2D _Albedo;
        sampler2D _MapaNormal;
        int _ActiveNormal;
      
        samplerCUBE _cube;
        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D(_Albedo,IN.uv_Albedo);

          if(_ActiveNormal){
          o.Normal = UnpackNormal(tex2D(_MapaNormal,IN.uv_MapaNormal));
          }
        }

        ENDCG
    }
        FallBack "Diffuse"
}
