Shader "UI/RadialSlider"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _FillAmount("Fill Amount", Range(0,1)) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Overlay" }
            Pass
            {
                Cull Off
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha

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
                float _FillAmount;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 uv = i.uv - 0.5; // ���������� ��������
                    float angle = atan2(uv.y, uv.x) / (2 * UNITY_PI) + 0.5; // ������������ ����
                    float dist = length(uv); // ���������� �� ������

                    if (angle > _FillAmount || dist > 0.5)
                        discard; // �������� ������

                    return tex2D(_MainTex, i.uv);
                }
                ENDCG
            }
        }
}
