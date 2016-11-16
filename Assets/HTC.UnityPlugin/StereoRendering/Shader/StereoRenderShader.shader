//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

Shader "Custom/StereoRenderShader"
{
	Properties
	{
		_LeftEyeTexture("Left Eye Texture", 2D) = "white" {}
	_RightEyeTexture("Right Eye Texture", 2D) = "white" {}
	_LeftTextureBound("Left Texture Bound", Vector) = (0, 1, 0, 1)
		_RightTextureBound("Right Texture Bound", Vector) = (0, 1, 0, 1)
	}

		CGINCLUDE
#include "UnityCG.cginc"
#include "UnityInstancing.cginc"
		ENDCG

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#pragma multi_compile __ STEREO_RENDER
#pragma target 3.0

		struct v2f
	{
		float2 uv : TEXCOORD0;
	};

	v2f vert(appdata_full i, out float4 outpos : SV_POSITION)
	{
		v2f o;
		outpos = mul(UNITY_MATRIX_MVP, i.vertex);

		o.uv = i.texcoord.xy;
		return o;
	}

	sampler2D _LeftEyeTexture;
	sampler2D _RightEyeTexture;
	fixed4 _LeftTextureBound;
	fixed4 _RightTextureBound;

	fixed4 frag(v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
	{
		float2 screenUV = screenPos.xy / _ScreenParams.xy;
		float2 finalUV = screenUV;

		fixed4 col = fixed4(0, 0, 0, 0);

#ifdef UNITY_SINGLE_PASS_STEREO
		if (unity_StereoEyeIndex == 0)
		{
			screenUV.x *= 2;
			finalUV.x = _LeftTextureBound.x + screenUV.x * (_LeftTextureBound.y - _LeftTextureBound.x);
			finalUV.y = _LeftTextureBound.z + screenUV.y * (_LeftTextureBound.w - _LeftTextureBound.z);

			col = tex2D(_LeftEyeTexture, finalUV);
		}
		else
		{
			screenUV.x = (screenUV.x - 0.5) * 2;
			finalUV.x = _RightTextureBound.x + screenUV.x * (_RightTextureBound.y - _RightTextureBound.x);
			finalUV.y = _RightTextureBound.z + screenUV.y * (_RightTextureBound.w - _RightTextureBound.z);

			col = tex2D(_RightEyeTexture, finalUV);
		}
#else
		if (unity_CameraProjection[0][2] < 0)
		{
			finalUV.x = _LeftTextureBound.x + screenUV.x * (_LeftTextureBound.y - _LeftTextureBound.x);
			finalUV.y = _LeftTextureBound.z + screenUV.y * (_LeftTextureBound.w - _LeftTextureBound.z);

			col = tex2D(_LeftEyeTexture, finalUV);
		}
		else
		{
			finalUV.x = _RightTextureBound.x + screenUV.x * (_RightTextureBound.y - _RightTextureBound.x);
			finalUV.y = _RightTextureBound.z + screenUV.y * (_RightTextureBound.w - _RightTextureBound.z);

			col = tex2D(_RightEyeTexture, finalUV);
		}
#endif

		return col;
	}

		ENDCG
	}
	}

		FallBack "Diffuse"
}
