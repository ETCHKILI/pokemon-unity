Shader "8Bit/GrayScaleDithering" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DitheringTex ("Dithering(R)", 2D) = "white" {}
		_ColorSteps ("Color Steps", Float) = 4
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
			uniform float _ColorStepsA;
			uniform float _ColorStepsB;


			fixed4 frag (v2f i) : COLOR
			{		
				fixed4 original = tex2D(_MainTex, i.uv);
				float dithering = tex2D(_DitheringTex, i.uvd).r;

				float Gray = (original.r + original.g + original.b) * _ColorStepsA;
				Gray = (floor(Gray) + (frac(Gray) > dithering)) * _ColorStepsB;
				
				return fixed4(Gray.xxx, original.a);
			}
			ENDCG
		}
	}
	Fallback off
}