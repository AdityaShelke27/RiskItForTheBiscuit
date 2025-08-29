Shader "Custom/ChromaticEcho"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RedOffset ("Red UV Offset", Vector) = (0.01, 0, 0, 0)
        _GreenOffset ("Green UV Offset", Vector) = (0, 0, 0, 0)
        _BlueOffset ("Blue UV Offset", Vector) = (-0.01, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _RedOffset;
            float4 _GreenOffset;
            float4 _BlueOffset;

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
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample 3 times with offsets
                float2 uvR = i.uv + _RedOffset.xy;
                float2 uvG = i.uv + _GreenOffset.xy;
                float2 uvB = i.uv + _BlueOffset.xy;

                fixed4 colR = tex2D(_MainTex, uvR);
                fixed4 colG = tex2D(_MainTex, uvG);
                fixed4 colB = tex2D(_MainTex, uvB);

                // Recombine channels
                fixed4 finalCol;
                finalCol.r = colR.r;
                finalCol.g = colG.g;
                finalCol.b = colB.b;
                finalCol.a = colG.a; // alpha from green sample

                return finalCol;
            }
            ENDCG
        }
    }
}
