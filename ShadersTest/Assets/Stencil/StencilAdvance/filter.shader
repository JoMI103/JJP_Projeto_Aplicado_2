Shader "Custom/filter"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        [Toggle]
        _showAlfa ("Show", Int) = 0

        [Header(Stencil)]
		_Stencil ("Stencil ID [0;255]", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Int) = 3
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
              
  
       Stencil{
 	        Ref [_Stencil]
	        Comp [_StencilComp]
	        Pass [_StencilOp]
        }   


        CGPROGRAM

        #pragma surface surf Standard alpha

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

 
        fixed4 _Color;
        int _showAlfa;
  
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            if(_showAlfa == 1){
            o.Alpha = 1;
            }else{
            o.Alpha = 0;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
