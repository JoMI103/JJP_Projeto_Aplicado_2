Shader "Unlit/DrawInCamera"
{
     Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GridSize("Grid Size", Range(0.2,1)) = 1
        _EspessuraGrid("Expessura Grid", Range(0.001,0.01)) = 0.001
        _GridColor("Grid Color", Color) = (1,1,1,1)


        _RectangeColor("Rectangle Color", Color) = (1,1,1,1)
        _slideXcenter("sliderXc", Range(0, 1)) = 1
        _slideYcenter("sliderYc", Range(0, 1)) = 1
        _slide("Size Slider", Range(0, 1)) = 1
    }
    SubShader
    {
        //    Tags { "RenderType"="Opaque" }
        //LOD 100

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
            float _GridSize;
            float _EspessuraGrid;
            fixed4 _GridColor;

            fixed4 _RectangeColor;
            float _slideXcenter;
            float _slideYcenter;
            float _slide;

            float GridTest(float2 r) {
                float result;

                for (float i = 0.0; i <= 1; i += _GridSize) {
                    for (int j = 0; j < 2; j++) {
                        result += 1.0 - smoothstep(0.0, _EspessuraGrid,abs(r[j] - i));
                    }
                }

                return result;
            }

            
            int isInsideSquare(float2 center, float lado, float2 ponto){
                if(ponto.x> center.x-lado && ponto.x<center.x+lado && ponto.y>center.y-lado && ponto.y<center.y+lado ){
                return 1;
                }
                else return 0;

            }

            bool isInsideCircle(float2 centro, float raio, float2 pontoTestar) {
                 float distance = sqrt(pow(pontoTestar.x - centro.x, 2) + pow(pontoTestar.y - centro.y, 2));
                 return distance <= raio;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);                      
                //return col;
                fixed4 col;

                if(isInsideSquare(float2(_slideXcenter, _slideYcenter), _slide, i.uv)){
                    return _RectangeColor;
                }
                else col = tex2D(_MainTex, i.uv) + ((_GridColor * _SinTime) * GridTest(i.uv));
                

                //col = tex2D(_MainTex, i.uv) + ((_GridColor * _SinTime) * GridTest(i.uv));
                return col;
            }
            ENDCG
        }
    }
}
