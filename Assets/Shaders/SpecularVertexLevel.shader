// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SpecularVertexLevel"
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
				fixed3 color : COLOR;
			};

			v2f vert(a2v v)
			{
				//获取环境光的颜色和强度
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				//将法向量从模型空间坐标转成投影空间坐标
				fixed3 worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
				//获取光源方向
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				//计算漫反射强度
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal, worldLightDir));
				//获取反射方向
				fixed3 reflectDir = normalize(reflect(-worldLightDir, worldNormal));
				//获取视角方向
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
				//计算高光反射
				fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(reflectDir, viewDir)), _Gloss);
				
				v2f o;
				//将顶点从模型空间坐标转成投影空间坐标
				o.pos = UnityObjectToClipPos(v.vertex);
				//颜色 = 环境光 + 漫反射 + 高光反射
				o.color = ambient + diffuse + specular;
				
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return fixed4(i.color, 1.0);
			}

			ENDCG
		}
	}
	FallBack "Specular"
}
