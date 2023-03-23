Shader "Custom/OiShader"
{
    Properties
    {
        _mainColor("A cor Bro!", Color)=(1,1,1,1)
        //_Texture("Texture", 2D) = "defaulttexture" {}
    }

    SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert 


        struct Input {

        float2 uv_texture;

        };

        fixed3 _mainColor; 

        void surf(Input IN, inout SurfaceOutput o) {
            //o.Albedo.rbg = tex2D(_Texture, IN);
            //o.Albedo=tex2D(_Texture, IN.uv_texture).rgd;
            o.Albedo = _mainColor.rgb;
            o.Albedo= o.Normal;

           // o.Normal=abs(o.Normal);
            if(o.Normal.y>0.9f){
            discard;
            }
        }

        ENDCG
    }
        FallBack "Diffuse"

}

// this.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);