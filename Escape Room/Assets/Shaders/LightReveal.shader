
Shader "Custom/LightReveal" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)

		// Background Texture
		_MainTex ("Background Texture", 2D) = "white" {}
		
		// Decal Texture
		_DecalTex ("Hidden Texture", 2D) = "black" {}

		//Normalmap
		_BumpMap ("Normalmap", 2D) = "bump" {}

		// Default direction of the spotlight cone
		_LightDirection("Light Direction", Vector) = (0,0,1,0)

		// Default position of the light source
		_LightPosition("Light Position", Vector) = (0,0,0,0)

		// The default spotlight angle
		_LightAngle("Light Angle", Range(0,180)) = 45

		// Intensity scalar
		_IntensityScalar("Intensity", Float) = 0

		
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
		sampler2D _DecalTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DecalTex;
			float2 uv_BumpMap;
			float3 worldPos;
		};

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
			float2 uv_hidden = IN.uv_DecalTex;
			

			// Load Background Texture and Decal Texture
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 decal = tex2D(_DecalTex, uv_hidden) * _Color;

			// Albedo is the decal color multiplied by the alpha channel within the light cone
			// And the background color multiplied by the color tint in the light cone
			o.Albedo = (decal.rgb * decal.a * intensity * 2) + c.rgb * (1 - intensity) + (c.rgb * _Color * intensity);
			o.Emission = decal.rgb * decal.a * intensity;

			//Add normalmap
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));

			// Metallic and smoothness come from slider variables
			o.Alpha = c.a;
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}