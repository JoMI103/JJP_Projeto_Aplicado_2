Shader "Custom/LS"
{
    Properties
    {
        _RimColor("rim color", Color) = (1,1,1,1)
        _SecondColor("second color", Color) = (1,1,1,1)
         _Albedo("Albedo", 2D) = "defaulttexture" {}
         _Exponencial ("Exponencial", Range (0, 5)) = 1
    }

    SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert 

        struct Input {
        float2 uv_Albedo;
        float3 viewDir;
        float2 uv_Text;
        };

        float3 _RimColor;
        float3 _SecondColor;
        sampler2D _Albedo;
        float _Exponencial;

        void surf(Input IN, inout SurfaceOutput o) {

        //Calculo do dot product
        //float dotp=saturate(pow(1-dot(normalize(IN.viewDir),o.Normal),_Exponencial)) ;
        float dotp=dot(normalize(IN.viewDir),o.Normal);
       
     

        if(dotp < 0){
        o.Emission = float3(0,0,0);
        }
       else if(dotp > 0 && dotp < 0.4){
        o.Emission=_RimColor;
        }
        else if(dotp > 0.4 && dotp <0.5){
        o.Emission = _SecondColor;
        }
        else if(dotp > 0.5 && dotp <0.9){
        o.Emission = float3(1,1,0);
        }
        else discard;

        //if(dotp > 0.4){
        //o.Emission=_RimColor;
        //}
        //else{
        //o.Emission = float3(1,1,1);
        //}


        //if(o.Normal.y<0.9f){
        //    o.Emission = float3(1,1,1);
        //    }

        }

        ENDCG
    }
        FallBack "Diffuse"
}
