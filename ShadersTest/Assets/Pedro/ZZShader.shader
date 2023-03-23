Shader "Custom/ZZShader"
{
    Properties
    {
        _mainColor("A color bro!", Color)=(1,1,1,1) //ad
        _Transparency("Transparencia", Range(0,1)) = 1
        _WorldPositionCutoffSuperior("Cut-off no Mundo Positivo", Range(-100, 100)) = 0.5
        _WorldPositionCutoffInferior("Cut-off no Mundo Negativo", Range(-100, 100)) = 0.5
        _NumeroDeTiras("Intensidade das Tiras", Range(0,1)) = 0
        _texture("texture ref", 2D) = "defaulttexture" {}
        _RimColor("rim color", Color) = (1,1,1,1)
        _Exponencial ("Exponencial", Range(-3, 10)) = 1
    }

    SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert alpha


        struct Input {
        float3 worldPos;
        float2 uv_texture;
        float3 viewDir;
        };

        fixed3 _mainColor; //ad

    fixed _Transparency;
    float _WorldPositionCutoffSuperior;
    float _WorldPositionCutoffInferior;
    float _NumeroDeTiras;
    sampler2D _texture;

    float3 _RimColor;
    float _Exponencial;


         float2 RotateImg(float2 ponto, float deg){
        
            //deg= radians(deg);

            float2x2 matRotacao = { cos(deg), -sin(deg),
                                    sin(deg), cos(deg) };                      
        
            return mul(matRotacao, ponto);
        }

        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = IN.worldPos;
            o.Alpha = 1;
             o.Albedo = _mainColor.rgb; //ad
            //o.Emission = float3(0,1,1);

            float2 uvRodadas =RotateImg(IN.uv_texture, (sin(_Time)*20));
             o.Albedo = tex2D(_texture, uvRodadas);
             o.Alpha = tex2D(_texture, uvRodadas).a;

            if (IN.worldPos.y >= _WorldPositionCutoffInferior && IN.worldPos.y <= _WorldPositionCutoffSuperior) {

                o.Alpha = 1 - _Transparency;
            }

            if (abs(IN.worldPos.y % 2) >= _NumeroDeTiras) {
                o.Albedo = tex2D(_texture, IN.uv_texture);
                o.Alpha = _Transparency;
            }
            float dotp=pow(1-dot(normalize(IN.viewDir),o.Normal),_Exponencial);
            o.Emission=_RimColor* dotp * sin(_Time)/100;
           
           
            

            

        }

        ENDCG
    }
        FallBack "Diffuse"
}
