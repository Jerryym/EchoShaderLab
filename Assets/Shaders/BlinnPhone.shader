// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/BlinnPhone"
{
	Properties
	{
		_Diffuse ("漫反射颜色", Color) = (1, 1, 1, 1)
		_Specular ("高光反射颜色", Color) = (1, 1, 1, 1)
		_Gloss ("光泽度", Range(8.0, 256)) = 20
	}

	SubShader
	{
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "Lighting.cginc"

			fixed4 _Diffuse;
			fixed4 _Specular;
			float _Gloss;

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION; 
				float3 worldNormal : TEXCOORD0;
				fixed3 worldPos : TEXCOORD1;
			};

			v2f vert(a2v v)
			{
				v2f o;
				//将顶点从模型空间坐标转成投影空间坐标
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//获取环境光的颜色和强度
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				//获取世界坐标系下的法向量
				fixed3 worldNormal = normalize(i.worldNormal);
				//获取光源方向
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				//计算漫反射强度
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal, worldLightDir));

				//获取视角方向
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
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
