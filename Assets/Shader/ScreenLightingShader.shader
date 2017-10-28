// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ScreenEffect/ScreenLighting"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MultTex("MultTexture", 2D) = "white" {}
		_AmbientColor("AmbientColor", COLOR) = (0, 0, 0, 0)
		_Saturation("Saturation", Range(0.0, 2.0)) = 1.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off 
		ZWrite Off 
		//used to make the "depth" of the cameras working:
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vertexShader
			#pragma fragment fragmentShader
			
			#include "UnityCG.cginc"

			struct vertInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct fragInput
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			fragInput vertexShader(vertInput input)
			{
				fragInput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = input.uv;
				return output;
			}
			
			sampler2D _MainTex;
			sampler2D _MultTex;
			float4 _AmbientColor;
			float _Saturation;

			float3 grayout(float3 inColor)
			{
				return dot(inColor, float3(0.33, 0.33, 0.34));
			}

			float4 fragmentShader(fragInput input) : SV_Target
			{		
				float4 worldColor = tex2D(_MainTex, input.uv) + _AmbientColor;

				float3 shadowColor = tex2D(_MultTex, input.uv).rgb;

				float3 gray = grayout(worldColor.rgb);
				
				float4 finalColor = float4((1.0 - _Saturation) * gray + _Saturation * worldColor.rgb, worldColor.a);

				finalColor.rgb *= shadowColor;

				return finalColor;
			}

			ENDCG
		}
	}
}
