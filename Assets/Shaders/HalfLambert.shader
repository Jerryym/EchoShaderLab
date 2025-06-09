Shader "Custom/HalfLambert"
{
    Properties
	{
		//材质的漫反射颜色
		_Diffuse ("Diffuse Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Pass
		{
			//基础光照通道: 前向渲染
			Tags {"LightMode" = "ForwardBase"}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "Lighting.cginc"

			//材质的漫反射颜色
			fixed4 _Diffuse;

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				v2f o;
				
				//将顶点从模型控件坐标转成投影空间坐标
				o.pos = UnityObjectToClipPos(v.vertex);
				//最终颜色 = 环境光 + 漫反射
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
				
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

				//计算漫反射强度 = max(dot(N, L), 0)
				fixed halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * halfLambert;

				//颜色 = 环境光 + 漫反射
				fixed3 color = ambient + diffuse;

				return fixed4(color, 1.0);
			}

			ENDCG
		}
	}

	FallBack "Diffuse"
}
