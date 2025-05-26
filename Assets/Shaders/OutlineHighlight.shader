Shader "Custom/OutlineHighlight"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (1,1,0,1) // 黄色
		_OutlineWidth ("Outline Width", Float) = 0.02
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }

		// Pass 1: Outline
		Pass
		{
			Name "OUTLINE"
			Cull Front
			ZWrite On
			ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 _OutlineColor;
			float _OutlineWidth;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			float4 _Object2World0;
			float4 _Object2World1;
			float4 _Object2World2;

			v2f vert (appdata v)
			{
				v2f o;
				float3 norm = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
				o.pos = UnityObjectToClipPos(v.vertex + float4(norm * _OutlineWidth, 0));
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return _OutlineColor;
			}
			ENDCG
		}

		// Pass 2: Normal Object
		Pass
		{
			Name "BASE"
			Cull Back
			ZWrite On
			ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
