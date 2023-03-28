Shader "Custom/Dissolve2"
{
    Properties
    {
        _mainTex("main tex",2D) = "defalttexture" {}
        _dissolveTex("dissolve",2D) = "defalttexture" {}
       _Color ("Cor", Color) =(0,0,0,0)
        _dissolveamount ("dissolve Amount", Range (0, 1)) = 0
        _rangeEdge ("dissolve Amount", Range (0, 0.1)) = 0
        
    }

    SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert  addshadow


        struct Input {
        float2 uv_mainTex;
        float2 uv_dissolveTex;
        };

        sampler2D _mainTex;
        sampler2D _dissolveTex;
        float _dissolveamount;
        float _rangeEdge;
        float3 _Color;
      

        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo= tex2D(_mainTex,IN.uv_mainTex);

          float3 dissolveF = tex2D(_dissolveTex, IN.uv_dissolveTex);
          
          clip(dissolveF - _dissolveamount);
          
          // Para mudar a cor com o tempo 
          //if(dissolveF.x - _dissolveamount <_rangeEdge) o.Albedo= _SinTime;
          if(dissolveF.x - _dissolveamount <_rangeEdge) o.Albedo= _Color;
          //step(dissolveF, _dissolveamount);
          o.Alpha = 1;
        }

        ENDCG
    }
        FallBack "Diffuse"
}
