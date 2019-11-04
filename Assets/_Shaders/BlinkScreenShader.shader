Shader "Hidden/BlinkScreenShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture",2D) = "white"{}
        _Red ("Red", Range(0,1)) = 1            
        _Blue ("Blue", Range(0,1)) = 1
        _ScreenMix("ScreenMix", Range(0,1)) = 1

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
            sampler2D _NoiseTex;
            float _Red, _Blue,_ScreenMix;
            float time_speed = 0.5;
            float3 color = float3(.153,0,.204);

            
            fixed4 frag (v2f i) : SV_Target
            {
                // Normalized pixel coordinates (from 0 to 1)
                float2 uv = i.uv;
                // Uv in screen space
                float2 uv_screen = uv * float2(2,2) - float2(1,1);
                // modify it to go off the edge
                uv_screen *=_ScreenMix;
                
                // Repeating Noise texture section
                
                // Time value
                float t = _Time.y;
                float r = _Red;
                float b = _Blue;
                // Noise Texture sampling [modified by time and wave effect]
                float2 sample1 = uv + float2(t * 1.35,t * 1.2) + float2(sin(t+uv.y*5.0f)*0.5,0);
                float2 sample2 = uv + float2(t * 0.4,t * 0.7)  + float2(0, sin(t+uv.x*5.0f)*0.5);

                // Acquire both samples and combine
                float noise_1 = tex2D(_NoiseTex, sample1).r;
                float noise_2 = tex2D(_NoiseTex, sample2).r;
                float noise_mix = max(noise_1,noise_2);
                
                
                // End of Section
                
                // Create cheap vignette from uv screen space
                float vignette = dot(uv_screen, uv_screen);
                vignette = vignette * vignette;
                
                // Combine vignette and noise
                float result = vignette * noise_mix;
                float3 result_color = (float3(1,1,1) - color) * result; 
                
                // Base Scene Texture
                float4 Albedo = tex2D(_MainTex, uv);
                Albedo.rgb = dot(Albedo.rgb,float3(0.3,.59,.11));
                // Debug check the noise
                //fragColor = float4(float3(noise_mix),1);
                
                // Debug, check effect only
                //fragColor = float4(result_color,1);
                
                // Final composite
                float4 fragColor = Albedo - (float4(result_color,1) * float4(r,1,b,1));

                //fragColor = float4(result_color,1) ;

                return fragColor;





                // float t = _Time.y * timeSpeed;

                // float4 col = tex2D(_MainTex,i.uv);
                
                // float3 color = float3(.153,0,.204);

                // float2 uvScreen = i.uv * float2(2,2) - float2(1,1);
                // uvScreen *=.7;
                
                // float2 sample1 = uvScreen * float2(t * 1.35,  t* 1.2) + float2(sin(t+i.uv.y  * 5)*0.5,0);
                // float2 sample2 = uvScreen * float2(t * 0.40,  t*0.7) + float2 (0,sin(t+i.uv.x* 5)*0.5);

                // float noise1 = tex2D(_NoiseTex,sample1);
                // float noise2 = tex2D(_NoiseTex,sample2);
                // float noise_mix = max(noise1,noise2);

                // float vignette = dot(uvScreen,uvScreen);
                // //vignette*=vignette;

                // float result = vignette * noise_mix;
                // float3 result_color = (color * result);

                // float4 Albedo = tex2D(_MainTex,i.uv);
                // Albedo.rgb = dot(Albedo.rgb,float3(0.3,.59,.11));
                
                // col = Albedo + float4(result_color,1);

                // //col = float4(result_color, 1);

                // //col.rgb=dot(col.rgb,float3(0.3,0.59,0.11));
                // //col.rgb+=float3(148,0,211);
                // return col;
            }
            ENDCG
        }
    }
}
