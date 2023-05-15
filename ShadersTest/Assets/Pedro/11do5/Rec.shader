Shader "Hidden/Rec"
{
     Properties
    {
        [HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}
        _Rec ("RecTexture", 2D) = "white" {}
        [NoScaleOffset]
        _DistorTexture ("Distortion", 2D) = "white" {}

        _range("range", Range(0,1)) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always 
         Blend SrcAlpha OneMinusSrcAlpha 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _DistorTexture;
            sampler2D _Rec;
            half _range;

            fixed4 frag (v2f i) : SV_Target
            {

                float2 newUV = i.uv;

                newUV.y += _Time.y;
                fixed4 disTex = tex2D(_DistorTexture, newUV);

                
                fixed4 col = tex2D(_Rec, i.uv  ) * disTex;
                fixed4 col2 = tex2D(_MainTex, i.uv );
                //fixed4 col3 = tex2D(_MainTex, i.uv);
            
       
                if(col.a  > 0) {
                 col.rgb = col.r+col.g+col.z/3;
                return col;}
               
                return col2;
            }
            ENDCG
        }
    }
}
