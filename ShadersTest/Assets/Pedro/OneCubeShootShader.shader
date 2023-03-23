Shader "Custom/OneCubeShootShader"
{
    Properties
    {
        _art ("w Cuidado", 2D) = "defaulttexture" {}
        _distanceFix ("distance", Float) = 0
        
        _centro ("centro", Vector) = (0,0,0,0)
    }

    SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert alpha


        struct Input {
            float2 uv_art;
            float3 worldPos;
        };

        //Recorrer as Properties
        sampler2D _art;
        float _distanceFix;
        float3 _centro;

        //Function 
        int isInsideSquare(float2 centro, float distance, float2 pontoATestar){
                if(pontoATestar.y < centro.y + distance && pontoATestar.y > centro.y - distance 
                 &&pontoATestar.x < centro.x + distance && pontoATestar.x > centro.x - distance){
                 return 1;
                 }else{
                    return 0;
                 }
        }

        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo = float3(1,0,0);
          if(isInsideSquare(_centro,_distanceFix,IN.worldPos.xy)){
           o.Albedo = tex2D(_art, IN.uv_art);
          }
          o.Alpha = 1;

        
        }

        ENDCG
    }
        FallBack "Diffuse"
}
