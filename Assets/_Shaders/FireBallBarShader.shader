Shader "FireBallBarShader"
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
            float fire = 0;
            float timer;
            int sway;
            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 uv = i.uv;
                half pie = 3.1415926535897932384626433832795;
                half tm  = sin(timer) * cos(uv.y * pie * 2) * 0.35 * sway;
                uv.x += (uv.y) * (uv.x * tm);//make the fire sway

                fixed4 col = tex2D(_MainTex, uv) * i.color;
                col.a = col.a * float(uv.x < fire);
                //col = float4(fire,fire,fire,1);
                return col;
            }
            ENDCG
        }
    }
}
