Shader "Custom/PixelPerfectOutline"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.001, 0.1)) = 0.02
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_particles

            #include "UnityCG.cginc"

            struct appdata_t
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

            sampler2D _MainTex;
            float4 _Color;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);
                
                // 원본 색상
                float4 finalColor = texColor * _Color * i.color;

                // 외곽선 효과 적용
                float outlineFactor = 0.0;
                float2 offset[4] = { float2(-_OutlineThickness, 0), float2(_OutlineThickness, 0), float2(0, -_OutlineThickness), float2(0, _OutlineThickness) };

                for (int j = 0; j < 4; j++)
                {
                   // 알파값이 1인 경우에만 outlineFactor 증가
                    if (tex2D(_MainTex, i.uv + offset[j]).a >= 0.5)
                    {
                        outlineFactor += 1.0;
                    }
                }

                // 원본 픽셀이 투명하면 외곽선 색 적용
                float4 outlineColor = _OutlineColor * outlineFactor;

                return outlineFactor == 1.0 ? outlineColor : finalColor;
            }
            ENDCG
        }
    }
}