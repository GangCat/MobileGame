Shader "Custom/BackgroundShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Absorption ("Absorption", Range(10, 100)) = 50
        _CamDist ("Camera Distance", Range(10, 100)) = 25
        _Opacity ("Opacity", Range(10, 100)) = 50
        _LightDirection ("Light Direction", Vector) = (1, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Absorption;
            float _CamDist;
            float _Opacity;
            float4 _LightDirection;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // Hash function
            float hash(float n)
            {
                return frac(sin(n) * 43758.5453);
            }

            // Noise function
            float noise(float3 x)
            {
                float3 p = floor(x);
                float3 f = frac(x);

                f = f * f * (3.0 - 2.0 * f);

                float n = p.x + p.y * 57.0 + 113.0 * p.z;

                return lerp(
                    lerp(lerp(hash(n + 0.0), hash(n + 1.0), f.x),
                         lerp(hash(n + 57.0), hash(n + 58.0), f.x), f.y),
                    lerp(lerp(hash(n + 113.0), hash(n + 114.0), f.x),
                         lerp(hash(n + 170.0), hash(n + 171.0), f.x), f.y), f.z);
            }

            // Fractal Brownian motion
            float fbm(float3 p)
            {
                float f;
                float3x3 m = float3x3(0.00,  0.80,  0.60,
                                      -0.80,  0.36, -0.48,
                                      -0.60, -0.48,  0.64);
                f  = 0.5000 * noise(p); p = mul(m, p * 2.02);
                f += 0.2500 * noise(p); p = mul(m, p * 2.03);
                f += 0.1250 * noise(p);
                return f;
            }

            // Scene function
            float scene(float3 pos)
            {
                return 0.1 - length(pos) * 0.05 + fbm(pos * 0.3);
            }

            // Main fragment function
            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = (i.uv * 2.0 - 1.0) * float2(1, _ScreenParams.y / _ScreenParams.x);
                float3 ro = _CamDist * normalize(float3(cos(2.75 - 3.0 * i.uv.x), 0.7 - 1.0 * (i.uv.y - 1.0), sin(2.75 - 3.0 * i.uv.x)));
                float3 ta = float3(0.0, 1.0, 0.0);
                
                float3 forward = normalize(ta - ro);
                float3 right = normalize(cross(float3(0.0, 1.0, 0.0), forward));
                float3 up = cross(forward, right);
                float3x3 cam = float3x3(right, up, forward);
                
                float3 dir = mul(cam, normalize(float3(uv, 1.3)));

                const int sampleCount = 64;
                float zMax = 40.0;
                float zstep = zMax / float(sampleCount);
                float3 p = ro;
                float T = 1.0;
                float3 sun_direction = normalize(_LightDirection.xyz);
                float4 color = float4(0.0, 0.0, 0.0, 0.0);

                for (int i = 0; i < sampleCount; i++)
                {
                    float density = scene(p);
                    if (density > 0.0)
                    {
                        float tmp = density / float(sampleCount);
                        T *= 1.0 - (tmp * _Absorption);

                        if (T <= 0.01)
                            break;

                        float k = _Opacity * tmp * T;
                        color += float4(1.0, 1.0, 1.0, 1.0) * k;
                    }
                    p += dir * zstep;
                }

                float3 bg = lerp(float3(0.3, 0.1, 0.8), float3(0.7, 0.7, 1.0), 1.0 - (uv.y + 1.0) * 0.5);
                color.rgb += bg;
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
