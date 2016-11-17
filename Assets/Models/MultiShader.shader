Shader "Custom/MultiShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex1 ("Albedo1 (RGB)", 2D) = "white" {}
		_MainTex2 ("Albedo2 (RGB)", 2D) = "white" {}
		_texBlend ("Albedo Blend", Range(0,1)) = 0.0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex1;
		sampler2D _MainTex2;

		struct Input {
			float2 uv_MainTex1;
			float2 uv_MainTex2;
		};

		float _texBlend;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c1 = tex2D (_MainTex1, IN.uv_MainTex1) * _Color; 
			fixed4 c2 = tex2D (_MainTex2, IN.uv_MainTex2) * _Color; 

		   float finalR = (1.0 - _texBlend) * c1.r + (_texBlend) * c2.r;
		   float finalG = (1.0 - _texBlend) * c1.g + (_texBlend) * c2.g;
		   float finalB = (1.0 - _texBlend) * c1.b + (_texBlend) * c2.b;

           float4 final;
           final.r = finalR;
           final.g = finalG;
           final.b = finalB;    

			o.Albedo = final;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
