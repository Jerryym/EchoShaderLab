// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SingleTexture"
{
	Properties
	{
		_Color ("颜色", Color) = (1, 1, 1, 1)
		_MainTex ("主纹理", 2D) = "white" {}
		_Specular ("高光反射颜色", Color) = (1, 1, 1, 1)
		_Gloss ("光泽度", Range(8.0, 256)) = 20
	}

	SubShader
	{
		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "Lighting.cginc"

			fixed4 _Color;
			sampler2D _MainTex;//纹理
			float4 _MainTex_ST;//纹理属性
			fixed4 _Specular;
			float _Gloss;

			struct a2v
			{
				float4 vertex :POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};

			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//计算材质反射率
				fixed3 albedo = tex2D(_MainTex, i.uv).rgb * _Color.rgb;

				//获取环境光的颜色和强度
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				//获取世界坐标系下的法向量
				fixed3 worldNormal = normalize(i.worldNormal);
				//获取光源方向
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				//计算漫反射强度
				fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));

				//获取视角方向
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				//获取反射方向
				fixed3 halfDir = normalize(worldLightDir + viewDir);
				//计算高光反射(BlinnPhone模型)
				fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(worldNormal, halfDir)), _Gloss);

				fixed3 color = ambient + diffuse + specular;
				return fixed4(color, 1.0);
			}
			ENDCG
		}
	}
	FallBack "Specular"
}
