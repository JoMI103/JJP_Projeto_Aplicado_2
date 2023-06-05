Shader "Custom/Unlit"
{ 
    Properties
    {
   
        _TexturaDeBaixo ("Texture", 2D) = "white" {} 
        _scale("scale", Range(0,5)) = 1
         _scale2("scale 2", Range(0,5)) = 1
    }

    SubShader
    {
     Cull off
     CGPROGRAM
     #pragma surface surf Lambert 


        struct Input {
            float3 worldPos;
            float3 worldNormal;
            float2 uv_TexturaDeBaixo;
        };

      sampler2D _TexturaDeBaixo;
      float _scale;
      float _scale2;
        void surf(Input IN, inout SurfaceOutput o) {
          
          float3 triplanarUV = IN.worldPos * _scale;
          float3 peso =pow(abs(IN.worldNormal),_scale2) ;
          
          peso /= dot(peso, 1);

          float4 projX = tex2D(_TexturaDeBaixo, triplanarUV.yz);
          float4 projY = tex2D(_TexturaDeBaixo, triplanarUV.xz);
          float4 projZ = tex2D(_TexturaDeBaixo, triplanarUV.xy);



          o.Albedo = projX* peso.x+ projY* peso.y  + projZ* peso.z ;


        }

        ENDCG
    }
        FallBack "Diffuse"
        
}
