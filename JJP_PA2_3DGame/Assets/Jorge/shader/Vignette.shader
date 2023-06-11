Shader "Custom/Vignette"
{
        Properties
        {
            _MainTex("Texture", 2D) = "white" {}
            _DamageActive("Damage Active", Range(0, 0.5)) = 0
            _VignetteColor("Vignette Color", Color) = (0, 0, 0, 1)
            _VignetteStrength("Vignette Strength", Range(0, 0.5)) = 0.5
        }

            SubShader
            {
                Tags { "RenderType" = "Opaque" }
                LOD 100

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

                    float _DamageActive;
                    float4 _VignetteColor;
                    float _VignetteStrength;
                    sampler2D _MainTex;

                    v2f vert(appdata v)
                    {
                        v2f o;
                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.uv = v.uv;
                        return o;
                    }

                    fixed4 frag(v2f i) : SV_Target
                    {
                        fixed4 col = tex2D(_MainTex, i.uv);
                        float dist = length(i.uv - 0.5);
                        float strength = smoothstep(_VignetteStrength, 0.5, dist);

                        if (_DamageActive > 0)
                        {
                            col.rgb += _VignetteColor.rgb * strength * _DamageActive;
                        }
                        else
                        {
                            return col;
                        }

                        return col;
                    }

                    ENDCG
                }
            }
    }
