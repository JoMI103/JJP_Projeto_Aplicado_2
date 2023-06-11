Shader "Unlit/Morte"
{
    Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_ScaleFactor ("Scale Factor", float) = 0.5
		_ExpandSlider ("Expand Slider", Range(-1,5)) = 1
		_InsideTexture ("Inside Texture", 2D) = "white" {}
		_Color1("Color1", Color) = (1,1,1,1)
		_Color2("Color2", Color) = (1,1,1,1)
		_Slide("Slide Lerp", Range(0,1)) = 0

		_TopTexture("Top Texture", 2D) = "white" {}


		_SlideUVS("SliderUVS", Range(0,1)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

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
				float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;

				float3 localVertex : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
				float3 vertexWorld : TEXCOORD3;
			
            };

             
           
			sampler2D _InsideTexture;
			fixed4 _Color1;
			fixed4 _Color2;
			float _Slide;
            float _DeathSlider;
			float _SlideUVS;

            v2f vert (appdata v) 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

				//Corret uvs
				o.localVertex = v.vertex;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);

                o.uv = v.uv;
                return o;
            }

           

            fixed4 frag (v2f i) : SV_Target
            {
			
				float3 UVS = i.vertexWorld * _SlideUVS;
				float3 peso =pow(abs(i.worldNormal), 10) ;
				peso /= dot(peso, 1);

				float4 projX = tex2D(_InsideTexture, UVS.yz);
                float4 projY = tex2D(_InsideTexture, UVS.xz);
                float4 projZ = tex2D(_InsideTexture, UVS.xy);
                fixed4 col  = projX* peso.x+ projY* peso.y  + projZ* peso.z ;


				if(col.a > 0){

				return col * 5 * lerp(_Color1, _Color2, _Slide * _SinTime.w);
				}

                return col;
                
            }
            ENDCG
        }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			#include "UnityCG.cginc"

			fixed4 _Color;
			float _ScaleFactor;
			float _ExpandSlider;
			sampler2D _InsideTexture;
			sampler2D _TopTexture;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct g2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			appdata vert (appdata v)
			{
				return v;
			}

			// Geometry Shader
			[maxvertexcount(3)]
			void geom (triangle appdata input[3], inout TriangleStream<g2f> stream)
			{
				// Calculate Normal Vector
				float3 vec1 = input[1].vertex - input[0].vertex;
				float3 vec2 = input[2].vertex - input[0].vertex;
				float3 normal = normalize(cross(vec1, vec2));

				[unroll]
				for(int i = 0; i < 3; i++)
				{
					appdata v = input[i];
					g2f o;
					// Move vertex along normal vector
					v.vertex.xyz += normal * (_ExpandSlider * 0.5 + 0.5) * _ScaleFactor; 
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					stream.Append(o);
				}
				stream.RestartStrip();
			}
			
			fixed4 frag (g2f i) : SV_Target
			{
				fixed4 col = tex2D(_TopTexture, i.uv);
				//fixed4 col = _Color;

				return col;
			}
			ENDCG
		}

		
	}
	FallBack "Unlit/Color"
}
