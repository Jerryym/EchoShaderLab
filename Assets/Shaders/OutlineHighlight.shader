Shader "Custom/OutlineHighlight"
{
	Properties
	{
		//高亮颜色: 默认青色
		m_HighlightColor ("Hightlight Color", Color) = (0, 1, 1, 1)
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 m_HighlightColor;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return m_HighlightColor;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
