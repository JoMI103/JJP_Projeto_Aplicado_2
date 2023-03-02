Shader "Prog3D/Alex"
{
    Properties
    {
      _slider ("controlo das tiras", Range (0, 1)) = 0
      _texture ("texture ref", 2D) = "defaulttexture" {}
    }

    SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert


        struct Input {

        float2 uv_texture;
        float3 worldPos;
        float3 viewDir;
        };


        float _slider;
        sampler2D _texture;


        void surf(Input IN, inout SurfaceOutput o) {
            float d = dot(normalize(IN.viewDir),o.Normal);
          o.Albedo.r = d;
          o.Alpha =1;
        // if(IN.worldPos.y >= -0.5 && IN.worldPos.y<=0.5) o.Alpha=0;
       //  if(abs(IN.worldPos.y % 2) >= _slider) o.Alpha=0;
        if(abs(IN.worldPos.y % 2) >= _slider) o.Albedo= tex2D(_texture,IN.uv_texture);

       }

        ENDCG
    }
        FallBack "Diffuse"
}