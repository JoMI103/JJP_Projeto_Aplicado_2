Shader "JoMI/EditableShader"
{Properties
    {
        _RimColor1("rim color1", Color) = (1,1,1,1)
        _RimColor2("rim color2", Color) = (1,1,1,1)
        
        _RimColor3("rim color2", Color) = (1,1,1,1)
        _RimColor4("rim color2", Color) = (1,1,1,1)
        _RimColor5("rim color2", Color) = (1,1,1,1)
        _RimColor6("rim color2", Color) = (1,1,1,1)
        _RimColor7("rim color2", Color) = (1,1,1,1)

         
         _Exponencial ("Exponencial", Range (-5, 10)) = 1
         _Value ("Exponencial",Range (-1, 1)) = 1
    }

    SubShader
    {
     Cull back
        CGPROGRAM
            #pragma surface surf Lambert alpha:fade

        struct Input {
        float2 uv_Albedo;
        float3 viewDir;
        float2 uv_Text;
        };

        float3 _RimColor1;
        float3 _RimColor2;
        float3 _RimColor3;
        float3 _RimColor4;
        float3 _RimColor5;
        float3 _RimColor6;
        float3 _RimColor7;
        float _Exponencial;
        float _Value;

        void surf(Input IN, inout SurfaceOutput o) {

        float dotp=saturate(pow(1-dot(normalize(IN.viewDir),o.Normal),_Exponencial)) ;
        //Calculo do dot product

        //dotp +=  _Value;

        dotp +=  _Value * _SinTime.z;

        if(dotp > 0.9){
             o.Emission=_RimColor1;
        }else{
            if(dotp> 0.7)
                o.Emission=_RimColor2;
                else{
                    if(dotp> 0.5)
                    o.Emission=_RimColor3;
                    else{
            
                        if(dotp> 0.3)
                        o.Emission=_RimColor4;
                        else{
            
                            if(dotp> 0.1)
                             o.Emission=_RimColor5;
                            else{
                                if(dotp > -0.2)
                                 o.Emission = _RimColor6;
                                 else o.Emission = _RimColor7;
                                }
                            }
                    }
                }
            }
       
       
        o.Alpha = (dotp);

        }

        ENDCG
    }
        FallBack "Diffuse"
}

//name ("display name", Range (min, max)) = number
//name ("display name", Float) = number
//name ("display name", Int) = number

//name ("display name", Color) = (number,number,number,number)
//name ("display name", Vector) = (number,number,number,number)

//name ("display name", 2D) = "defaulttexture" {}
//name ("display name", Cube) = "defaulttexture" {}
//name ("display name", 3D) = "defaulttexture" {}
//name ("display name", 2DArray) = "defaulttexture" {}
