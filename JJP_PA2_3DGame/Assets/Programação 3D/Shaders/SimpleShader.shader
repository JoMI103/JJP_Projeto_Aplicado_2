Shader "Prog3D/SimpleShader"
{

Properties{
    _chosenColor("Example Colour", Color) = (1,1,1,1)
    _chosenEmission("Example Emission", Color) = (1,1,1,1)
    
}
SubShader{

CGPROGRAM
#pragma surface surf Standard

struct Input {
float2 uvMainTex;
};

fixed4 _chosenColor;
fixed4 _chosenEmission;


void surf(Input IN, inout SurfaceOutputStandard o) {
o.Albedo = _chosenColor.rgb;

o.Emission = _chosenEmission.rgb;
o.Metallic = _chosenEmission.r;
}

ENDCG
}
FallBack "Diffuse"
}
