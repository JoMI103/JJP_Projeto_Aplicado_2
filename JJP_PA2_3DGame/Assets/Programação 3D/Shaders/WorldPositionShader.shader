Shader "Prog3D/WorldPositionShader"
{
    Properties
    {
        _Transparency("Transparencia", Range(0,1)) = 1
        _WorldPositionCutoffSuperior("Cut-off no Mundo Positivo", Range(-100, 100)) = 0.5
        _WorldPositionCutoffInferior("Cut-off no Mundo Negativo", Range(-100, 100)) = 0.5
        _NumeroDeTiras("Intensidade das Tiras", Range(0,1)) = 0
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

    fixed _Transparency;
    float _WorldPositionCutoffSuperior;
    float _WorldPositionCutoffInferior;
    float _NumeroDeTiras;
    sampler2D _texture;




        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = IN.worldPos;
            
            o.Alpha = 1;
            if (IN.worldPos.y >= _WorldPositionCutoffInferior && IN.worldPos.y <= _WorldPositionCutoffSuperior) {

                o.Alpha = 1 - _Transparency;
            }

            if (abs(IN.worldPos.y % 2) >= _NumeroDeTiras) {
                
                o.Albedo= tex2D(_texture,IN.uv_texture);
                //o.Alpha = 0;
            }


        }

        ENDCG
    }
        FallBack "Diffuse"
}



