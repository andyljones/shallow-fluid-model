Shader "VertexColor" {

	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader { 
		Tags { "RenderType"="Opaque" }
		LOD 400

		CGPROGRAM

		#pragma surface surf BlinnPhong

		fixed4 _Color;

		struct Input {
			half4 color : COLOR0;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color.rgb * IN.color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG	
	}
	FallBack "Diffuse"
}