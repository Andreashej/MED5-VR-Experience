
Shader "Custom/LightReveal" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		// Default direction of the spotlight cone
		_LightDirection("Light Direction", Vector) = (0,0,1,0)

		// Default position of the light source
		_LightPosition("Light Position", Vector) = (0,0,0,0)

		// The default spotlight angle
		_LightAngle("Light Angle", Range(0,180)) = 45

		// Intensity scalar
		_IntensityScalar("Intensity", Float) = 50
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _LightPosition;
		float4 _LightDirection;
		float _LightAngle;
		float _IntensityScalar;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			// Making a directional vector from light position to pixel with a magnitude of 1
			float3 direction = normalize(IN.worldPos - _LightPosition);

			//Finding dot product between direction and lightdirection vector
			float scale = dot(direction, _LightDirection);
			
			//If the pixel is outside the lightcone, the intensity will get a negative value
			float intensity = scale - cos((_LightAngle / 2) * (3.14 / 180.0));

			//If the intensity is bigger than 1 it returns 1. If it's between 0 and one it returns the value. If it's smaller than 0 it returns 0.
			intensity = min(max(intensity * _IntensityScalar, 0), 1);

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Emission = c.rgb * c.a * intensity;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = intensity * c.a ;
		}
		ENDCG
	}
	FallBack "Diffuse"
}