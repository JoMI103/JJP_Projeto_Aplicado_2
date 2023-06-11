Shader "Custom/ClickMat"
{
   Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Amount ("Extrusion Amount", Range(-1,1)) = 0.5
      _Valu ("Float Valu", Float) = 0
      _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      struct Input {
          float2 uv_MainTex;
      };
      float _Amount;
      float _Valu;
      float3 _Color;
      void vert (inout appdata_full v) {
          if(_Valu == 1){v.vertex.xyz += v.normal * _Amount; //aumentar
          //if(_Valu == 1){v.vertex.xyz -= v.normal * _Amount; //reduzir
          _Valu = 0;
          }

      }
      sampler2D _MainTex;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color.rgb;
      }
      ENDCG
    } 
    Fallback "Diffuse"
}
