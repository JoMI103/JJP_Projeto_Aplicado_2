Shader "Hidden/Moon_shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _slide("slider", Range(0, 1)) = 1
        _slideXcenter("sliderXc", Range(0, 1)) = 1
        _slideYcenter("sliderYc", Range(0, 1)) = 1
        _slide2("slider2", Range(0, 1)) = 1
        _slideXcenter2("sliderXc2", Range(0, 1)) = 1
        _slideYcenter2("sliderYc2", Range(0, 1)) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
            float _slide;
            float _slideXcenter;
            float _slideYcenter;
            float _slide2;
            float _slideXcenter2;
            float _slideYcenter2;

            //procura se ta dentro do quadrado
              int isInsideSquare(float2 center, float lado, float2 ponto){
            if(ponto.x> center.x-lado && ponto.x<center.x+lado && 
               ponto.y>center.y-lado && ponto.y<center.y+lado ){
            return 1;
             }
            else return 0;

            }



   bool isPointInsideCircle(float2 centro, float raio, float2 pontoTestar) {
    float distance = sqrt(pow(pontoTestar.x - centro.x, 2) + pow(pontoTestar.y - centro.y, 2));
    return distance <= raio;
    }
           

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
         
                //if(isInsideSquare(float2(_slideXcenter, _slideYcenter), _slide, i.uv)){
                //    return float4(0,1,0,1);
                //} 

                //half moon
                if(isPointInsideCircle(float2(_slideXcenter, _slideYcenter), _slide, i.uv)){
                  if(!isPointInsideCircle(float2(_slideXcenter2, _slideYcenter2), _slide2, i.uv)){
                 return float4(1,0,1,0);
                }
  
                }
                //if(isPointInsideCircle(float2(_slideXcenter2, _slideYcenter2), _slide2, i.uv)){
                // return float4(0,0,1,1);
                //}
                return col;


            }
            ENDCG
        }
    }
}
