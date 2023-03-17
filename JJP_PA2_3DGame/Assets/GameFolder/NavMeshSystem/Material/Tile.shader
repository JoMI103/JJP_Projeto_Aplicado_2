Shader "NavMeshSystem/Tile"
{
    Properties
    {
        _Textura ("display name", 2D) = "defaulttexture" {}
   
        _Alpha("Alpha", Range (0, 1)) = 0
    }

    SubShader
    {
    //Cull off 
    CGPROGRAM

    #pragma surface surf Lambert alpha

        struct Input {
        float2 uv_Textura;
        };

        sampler2D _Textura;

        float _Alpha;

        void surf(Input IN, inout SurfaceOutput o) {


            o.Albedo.rgb = tex2D(_Textura, IN.uv_Textura).rgb;
            o.Alpha = _Alpha;
        }

        ENDCG
    }
    FallBack "Diffuse"
}

/*
   Properties
   {
     _Textura ("Texture", 2D) = "defaulttexture" {}
   }

   SubShader
   {
   CGPROGRAM
   #pragma surface surf Lambert

   struct Input {
   float2 uv_Textura;
   };

   sampler2D _Textura;

   void surf(Input IN, inout SurfaceOutput o) {
       o.Albedo.rgb = tex2D(_Textura, IN.uv_Textura).grb;
    }

ENDCG
}
FallBack "Diffuse"
}
*/
/*
        Properties
    {
        _RimColor("rim color", Color) = (1,1,1,1)
        _RimColor1("rim color", Color) = (1,1,1,1)
        _Albedo("Albedo", 2D) = "defaulttexture" {}
        _Exponencial ("Exponencial", Range (-5, 10)) = 1
        _Limite ("limite", Range (0, 1)) = 0
    }

    SubShader
    {
    //Cull off 
    CGPROGRAM

    #pragma surface surf Lambert alpha:fade

        struct Input {
        float2 uv_Albedo;
        float3 viewDir;
        float2 uv_Text;
        };

        float3 _RimColor;
        float3 _RimColor1;

        sampler2D _Albedo;
        float _Exponencial;
        float _Limite;

        void surf(Input IN, inout SurfaceOutput o) {

        //Calculo do dot product
        float dotp=saturate(pow(1-dot(normalize(IN.viewDir),o.Normal),_Exponencial)) ;
        
        if(dotp > _Limite){
         o.Emission=_RimColor;
        }else{
         o.Emission=_RimColor1;
        }
       
        // o.Albedo=tex2D(_Albedo,IN.uv_Albedo);
        o.Alpha = (dotp);

        }
        ENDCG
    }
    FallBack "Diffuse"

    */