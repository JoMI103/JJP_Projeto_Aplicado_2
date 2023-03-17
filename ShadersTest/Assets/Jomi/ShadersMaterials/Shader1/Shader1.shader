Shader "Custom/Shader1"
{
    Properties
   {
        _activeTime ("ActiveEffect" ,Range(0,1)) = 0
   }

   SubShader
   {
     cull Back
     CGPROGRAM
        #pragma surface surf Lambert 

        struct Input {
            float3 viewDir;
        };
        
        float _activeTime;

        void surf(Input IN, inout SurfaceOutput o) {
            if(o.Normal.y>0.0f){
                 o.Albedo= normalize(IN.viewDir)+o.Normal + _SinTime.x * _activeTime;
            
            }else{
                 o.Albedo= normalize(IN.viewDir)-o.Normal - _SinTime.x * _activeTime;
            }
        }

        ENDCG
        }
        FallBack "Diffuse"
}
