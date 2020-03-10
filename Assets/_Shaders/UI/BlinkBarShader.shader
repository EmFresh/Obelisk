Shader "BlinkBarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            sampler2D _MainTex;
            float blink = 1;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 uv = i.uv;
               // uv.y = uv.y*uv.x / cos(_Time.y);
                fixed4 col = tex2D(_MainTex, uv) * i.color;
                // just invert the colors
                col.a = col.a * float(uv.x < blink);
                //col = float4(blink,blink,blink,1);
                return col;
            }
            ENDCG
        }
    }
}
