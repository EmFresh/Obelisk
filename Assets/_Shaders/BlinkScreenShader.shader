Shader "Hidden/BlinkScreenShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture",2D) = "white"{}
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
            float timeSpeed = 0.5f;

            fixed4 frag (v2f i) : SV_Target
            {
                float3 color = float3(0.153/.255,.50/.255,.204/.255);

                float t = timeSpeed * _Time.y;
                
                float2 noise = tex2D(_NoiseTex,i.uv).xy;
                float4 col = tex2D(_MainTex, i.uv);

                float2 sample1 = float2 (t*1.35, t*1.2) + float2(sin(t+noise.y*5.0f)*0.5,0);
                float sample2 = float2(t*0.4,t*0.7) + float2(0,sin(t+noise.x * 5.0f)*0.5);

                float noise_1   =tex2D(_NoiseTex,sample1);
                float noise_2   =tex2D(_NoiseTex,sample2);
                float noise_mix =max(noise_1,noise_2);
                float vignette  =(dot(col,col));
                vignette*=vignette; 
                float result = noise_mix;
                float3 resultColor =(color) * result;
                float4 albedo      =tex2D(_MainTex,i.uv);
                
                //col.rgb=dot(col.rgb,float3(0.3,0.59,0.11));
                //col.rgb+=float3(148,0,211);
                col = albedo - float4(resultColor,1);
                return col;
            }
            ENDCG
        }
    }
}
