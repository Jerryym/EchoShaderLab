// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NormalMapTangentSpace"
{
    Properties
    {
        _Color ("颜色", Color) = (1, 1, 1, 1)
		_MainTex ("主纹理", 2D) = "white" {}
        _BumpMap ("法线纹理", 2D) = "bump" {}//bump-Unity内置的法线纹理
        _BumpScale ("凹凸比例", Range(0, 1)) = 1
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
			sampler2D _MainTex;//主纹理
			float4 _MainTex_ST;//主纹理属性
            sampler2D _BumpMap;//法线纹理
            float4 _BumpMap_ST;//法线纹理属性
            float _BumpScale;//凹凸比例
			fixed4 _Specular;
			float _Gloss;

            struct a2v
			{
				float4 vertex :POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
                float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
                float3 lightDir : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
			};

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

                //计算副法线
                TANGENT_SPACE_ROTATION;

                //计算光照方向
                o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex)).xyz;
                //计算视角方向
                o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex)).xyz;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed3 tangentLightDir = normalize(i.lightDir);
                fixed3 tangentViewDir = normalize(i.viewDir);

                fixed4 packedNormal = tex2D(_BumpMap, i.uv.zw);
                fixed3 tangentNormal = UnpackNormal(packedNormal);
                tangentNormal.xy *= _BumpScale;
                tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));

                fixed3 albedo = tex2D(_MainTex, i.uv.xy).rgb * _Color.rgb;

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo;

                fixed3 diffuse = _LightColor0.rgb * albedo * saturate(dot(tangentNormal, tangentLightDir));

                fixed3 halfDir = normalize(tangentLightDir + tangentViewDir);
                fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(tangentNormal, halfDir)), _Gloss);

                return fixed4(ambient + diffuse + specular, 1.0);
            }

            ENDCG
        }
    }
    FallBack "Specular"
}
