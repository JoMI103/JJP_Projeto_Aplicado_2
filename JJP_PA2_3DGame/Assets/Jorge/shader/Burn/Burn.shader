Shader "Custom/Burn"  {
    Properties{
        
        _MainTex("Main Texture", 2D) = "white" {}
        _BurnTex("Burn Texture", 2D) = "white" {}
        _BurnThreshold("Burn Threshold", Range(0, 1)) = 0.5
        _BurnFrequency("Burn Frequency", Range(0, 10)) = 1.0
        _BurnAmplitude("Burn Amplitude", Range(0, 1)) = 0.5
        _TimeMultiplier("Time Multiplier", Range(0, 1)) = 0.1

    }

        SubShader{
            
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            sampler2D _BurnTex;
            float _BurnThreshold;
            float _BurnFrequency;
            float _BurnAmplitude;
            float _TimeMultiplier;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutput o) {
               

                fixed4 mainColor = tex2D(_MainTex, IN.uv_MainTex);
                fixed4 burnColor = tex2D(_BurnTex, IN.uv_MainTex);
                float burnAmount = step(_BurnThreshold, burnColor.r);
                float adjustedTime = _Time.y * _TimeMultiplier;
                float burnIntensity = sin(_BurnFrequency * adjustedTime) * _BurnAmplitude;
                burnAmount *= burnIntensity;
                fixed3 finalColor = lerp(mainColor.rgb, burnColor.rgb, burnAmount);
                o.Albedo = finalColor;
            
            }
            ENDCG
        }
            FallBack "Diffuse"
}

