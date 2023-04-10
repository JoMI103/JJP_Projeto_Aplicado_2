Shader "Custom/Ola"
{
 Properties
    {
        _Frequency ("Frequency", Range(0, 10)) = 1
    }

    SubShader
    {
        CGPROGRAM

        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_Albedo;
            float3 viewDir;
        };

        float3 squareWave(float a, float f)
        {
            return sin(a * f) > 0 ? float3(1,0,0) : 0;//uma cor ou outra, no caso vermelho ou 0 = preto
        }

        float _Frequency;

        void surf(Input IN, inout SurfaceOutput o)
        {
            float dotP = dot(IN.viewDir, o.Normal);

            o.Albedo = squareWave(10 * dotP, _Frequency);
        }

        ENDCG
    }

    FallBack "Diffuse"
}
