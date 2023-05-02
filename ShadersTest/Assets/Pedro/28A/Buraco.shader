Shader "Custom/Buraco"
{
    Properties
    {
        
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

    
        ZTest always
       Cull off
       Stencil{
 	        Ref 1 //Referencia dentro do stencil
	        Comp always //diz como deve ser feita a comparação
	        Pass replace //subestitui o que estive no stencil
        }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };





        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
     
            o.Alpha = 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
