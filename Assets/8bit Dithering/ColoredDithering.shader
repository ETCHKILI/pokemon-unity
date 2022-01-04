Shader "8Bit/ColoredDithering" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DitheringTex ("Dithering(R)", 2D) = "white" {}
		_ColorStepsA ("Color Steps", Vector) = (4, 4, 4, 0)
		_ColorStepsB ("Color Steps", Vector) = (0.25, 0.25, 0.25, 0)
		_SceenAndTex ("SceenAndTex", Vector) = (1, 1, 0, 0)
	}

	SubShader 
	{
		Pass 
		{
			ZTest Always ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			// Author: O.V.Pavlov http://ovpavlov.blogspot.ru/ 

			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#include "DitheringFuncs.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _DitheringTex;
			uniform float3 _ColorStepsA;
			uniform float3 _ColorStepsB;


			fixed4 frag (v2f i) : COLOR
			{		
				fixed4 original = tex2D(_MainTex, i.uv);
				float dithering = tex2D(_DitheringTex, i.uvd).r;

				float3 Color = original.rgb * _ColorStepsA;
				Color = (floor(Color) + (frac(Color) > dithering)) * _ColorStepsB;

				return float4(Color.rgb, original.a);
			}
			ENDCG
		}
	}
	Fallback off
}