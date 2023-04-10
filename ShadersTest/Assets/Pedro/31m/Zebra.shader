Shader "Custom/Zebra"
{
    Properties
    {
        _StripeColor("Stripe Color", Color) = (0,0,0,1)
        _Strength("Strength", Range(0,10)) = 1

    }
    SubShader
    {
        Cull off
        CGPROGRAM
            #pragma surface surf Lambert

        struct Input 
        {
            float2 uv_Albedo;
            float2 uv_Normal;
            float3 viewDir;   
        };

        fixed4 _StripeColor;
        fixed _Strength;

        void surf(Input IN, inout SurfaceOutput o) 
        {
            fixed4 d = dot(normalize(IN.viewDir),o.Normal);
            o.Albedo = _StripeColor * sin(d * (_Time.y / _Strength));
        }

        ENDCG
    }
        FallBack "Diffuse"
}
