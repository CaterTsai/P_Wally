﻿Shader "AlphaGlowShader" {
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}

	_MKGlowColor("Glow Color", Color) = (1,1,1,1)
		_MKGlowPower("Glow Power", Range(0.0,5.0)) = 2.5
		_MKGlowTex("Glow Texture", 2D) = "black" {}
	_MKGlowTexColor("Glow Texture Color", Color) = (1,1,1,1)
		_MKGlowTexStrength("Glow Texture Strength ", Range(0.0,1.0)) = 1.0
		_MKGlowOffSet("Glow Width ", Range(0.0,0.075)) = 0.0

		_Alpha("Alpha", Range(0, 1)) = 1
		[HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "MKGlow" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		half2 texcoord : TEXCOORD0;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;

	sampler2D _MKGlowTex;
	half _MKGlowTexStrength;
	fixed4 _MKGlowTexColor;
	uniform float _Alpha;

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		float4 finalColor_;
		fixed4 col = tex2D(_MainTex, i.texcoord);
		fixed4 d = tex2D(_MKGlowTex, i.texcoord) * _MKGlowTexColor;
		if (col.a > 0.5)
		{
			//col += (d * _MKGlowTexStrength);
			finalColor_ = float4(col.rgb, col.a*_Alpha);
			return finalColor_;
		}
		else
		{			
			finalColor_ = float4(d.rgb * _MKGlowTexStrength, d.a * _Alpha * _MKGlowTexStrength);
			return finalColor_;
		}
	}
		ENDCG
	}
	}

}