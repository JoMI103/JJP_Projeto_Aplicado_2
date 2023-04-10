Shader "Custom/posshader"
{
    Properties
    {
        _mainColor("A color bro!", Color)=(1,1,1,1) //ad
        _Transparency("Transparencia", Range(0,1)) = 1
        _WorldPositionCutoffSuperior("Cut-off no Mundo Positivo", Range(-100, 100)) = 0.5
        _WorldPositionCutoffInferior("Cut-off no Mundo Negativo", Range(-100, 100)) = 0.5
        _NumeroDeTiras("Range", Range(150,200)) = 150
        _texture("texture ref", 2D) = "defaulttexture" {}
    }

    SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert alpha


        struct Input {
        float3 worldPos;
        float2 uv_texture;
        };

        fixed3 _mainColor; //ad

    fixed _Transparency;
    float _WorldPositionCutoffSuperior;
    float _WorldPositionCutoffInferior;
    float _NumeroDeTiras;
    sampler2D _texture;




        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = IN.worldPos;
            o.Alpha = 1;
             o.Albedo = _mainColor.rgb; //ad
            //o.Emission = float3(0,1,1);

            if(IN.worldPos.y < _NumeroDeTiras && IN.worldPos.x < 70){
            o.Emission = float3(1,1,1);
            }
            else if(IN.worldPos.y < _NumeroDeTiras && IN.worldPos.x > 70){
            o.Emission = float3(1,0,1);
            }
            else o.Emission = float3(1,1,0);

            //if(IN.worldPos.y < _NumeroDeTiras){
            //o.Emission = float3(1,1,1);}
            //else discard;


        }

        ENDCG
    }
        FallBack "Diffuse"
}
