
Shader "Custom/LightReveal" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		// Direction of the spotlight cone
		_LightDirection("Light Direction", Vector) = (0,0,1,0)

		// Position of the light source
		_LightPosition("Light Position", Vector) = (0,0,0,0)

		// The spotlight angle
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

			
			float3 direction = normalize(_LightPosition - IN.worldPos);

			float scale = dot(direction, _LightDirection);
			float intensity = scale - cos(_LightAngle * (3.14 / 360.0));
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