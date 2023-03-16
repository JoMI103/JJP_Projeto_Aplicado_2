Shader "Custom/zTeste"
{
    Properties
    {
      _slider("slider",Range(0,1)) = 0  
    }

    SubShader
    {
     
        CGPROGRAM
            #pragma surface surf Lambert 


        struct Input {
        float3 viewDir;
        };

      
      float _slider;

        void surf(Input IN, inout SurfaceOutput o) {
          
          o.Albedo = float3(1,0,0);

          half dotPR = dot( normalize(o.Normal),normalize(IN.viewDir));

          if(dotPR > 0 && dotPR< _slider){
          o.Albedo = float3(0,1,0);
          }
          else{
          discard;
          }
        }

        ENDCG
    }
        FallBack "Diffuse"
}
