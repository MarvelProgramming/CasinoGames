Shader "Unlit/CardShader"
{
    Properties
    {
        _OutlineTint ("Outline Tint", COLOR) = (1,1,1,1)
        _OutlineTex ("Texture", 2D) = "white" {}
        _FrontTex ("Texture", 2D) = "white" {}
        _BackTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull off

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
                float3 normal : NORMAL;
                float3 viewdir : TEXCOORD2;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 viewdir : TEXCOORD2;
                fixed4 color : COLOR;
            };

            fixed4 _OutlineTint;
            sampler2D _OutlineTex;
            sampler2D _FrontTex;
            sampler2D _BackTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = v.normal;
                o.color = v.color;
                o.viewdir = mul((float3x3) UNITY_MATRIX_V, float3(0, 0, 1));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 outlineCol = tex2D(_OutlineTex, i.uv) * _OutlineTint;
                fixed4 col;
    
                if (dot(i.viewdir, i.normal) < 0)
                {
                    col = tex2D(_FrontTex, i.uv) * i.color;
                    col = lerp(col, outlineCol, outlineCol.a);
                }
                else
                {
                    col = tex2D(_BackTex, i.uv) * i.color;
                }
    
                return col;
            }
            ENDCG
        }
    }
}
