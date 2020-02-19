Shader "Nature/Terrain/MyCustomTerrainShader_NoTransitions" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Scale ("Scale", Range(0,1)) = 0.5 

		// Main Textures data //
		_Texture0 ("Texture 1", 2D) = "white" {}
		_Texture1 ("Texture 2", 2D) = "white" {}
		_Texture2 ("Texture 3", 2D) = "white" {}
		_Texture3 ("Texture 4", 2D) = "white" {}
		_Texture4 ("Texture 5", 2D) = "white" {}
		
		_NormalTex0 ("Normal 1", 2D) = "bump" {}
		_NormalTex1 ("Normal 2", 2D) = "bump" {}
		_NormalTex2 ("Normal 3", 2D) = "bump" {}
		_NormalTex3 ("Normal 4", 2D) = "bump" {}
		_NormalTex4 ("Normal 5", 2D) = "bump" {}

		_Tex2Height ("Texture2-Height", Range(0,1)) = 1
		_Tex2Blend ("Texture2-Blend", Range(0,100)) = 0
		
		_Tex3Height ("Texture3-Height", Range(0,1)) = 1
		_Tex3Blend ("Texture3-Blend", Range(0,100)) = 0

		_Tex4Height ("Texture4-Height", Range(0,1)) = 1
		_Tex4Blend ("Texture4-Blend", Range(0,100)) = 0

		_Tex5Height ("Texture5-Height", Range(0,1)) = 1
		_Tex5Blend ("Texture5-Blend", Range(0,100)) = 0


		// Cliff data //
		_Cliff ("Cliff", 2D) = "white" {}
		_CliffBump ("CliffBump", 2D) = "bump" {}
		_CliffBlend("CliffBlend", Range(0,2)) = 1
		_CliffFade("CliffFade", Range(0,1)) = 1
		_CliffMinH ("CliffMinH", Range(-1,1)) = -1
		_CliffMaxH ("CliffMaxH", Range(-1,1)) = 1
		_CliffFadeBottom("CliffFadeBottom", Range(0,200)) = 0
		_CliffFadeTop("CliffFadeTop", Range(0,200)) = 0
		_CliffFadeTreshold("CliffFadeTreshold", Range(1, 10)) = 1
		_SteepNes("SteepNes", Range(0,1)) = 0
		_SteepNesBlend("SteepNesBlend", Range(0,20)) = 0

		_MapHeight ("MapHeight", Float) = 600		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
				
		CGPROGRAM
		#pragma surface surf Standard vertex:vert
		#pragma target 3.0
		
		sampler2D _Texture0,_Texture1,_Texture2,_Texture3,_Texture4;
		sampler2D _Cliff, _CliffBump;
		sampler2D _NormalTex0,_NormalTex1,_NormalTex2,_NormalTex3, _NormalTex4, _NormalTex5;

		struct Input {			
			float3 worldPos;
			float2 uv_Cliff; 
			float3 myNormal;
			INTERNAL_DATA
		};

		void vert (inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
			v.tangent.xyz = cross(v.normal, float3(0,0,1));
			v.tangent.w = -1;
			data.myNormal = v.normal; 
		}

		half _Glossiness;
		half _Metallic;
		
		fixed _MapHeight;
		fixed _Tex1Height, _Tex2Height, _Tex3Height, _Tex4Height, _Tex5Height, _Tex6Height, _Tex7Height;
		fixed _Tex1Blend, _Tex2Blend, _Tex3Blend, _Tex4Blend, _Tex5Blend, _Tex6Blend, _Tex7Blend;
		fixed _CliffMinH, _CliffMaxH, _SteepNes;	
		fixed _CliffBlend, _SteepNesBlend, _CliffFade;	
		fixed _CliffFadeBottom, _CliffFadeTop;
		fixed _CliffFadeTreshold;

		fixed4 _Color, col;
		fixed c; fixed4 nrm;
		fixed _Scale;
		float2 uv;


		void DrawCliff(Input IN)
		{
			float4 cliff = tex2D(_Cliff, IN.uv_Cliff);
			float4 cliffB = tex2D(_CliffBump, IN.uv_Cliff);
			fixed start;
			
			float4 colfade = col;
			float4 nrmfade = nrm;

			if (IN.worldPos.y*_CliffFadeTreshold < _MapHeight * _CliffMaxH)
			{
				start = _CliffMinH * _MapHeight;
				c = clamp((IN.worldPos.y-start), 0, (1+_CliffFadeBottom));
				colfade = lerp(colfade, cliff, c/(1+_CliffFadeBottom));
				nrmfade = lerp(nrmfade, cliffB, c/(1+_CliffFadeBottom));
				
				c = clamp(abs(IN.myNormal.x)-(1-_CliffFade), 0, 1);
				col = lerp(col, colfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
				nrm = lerp(nrm, nrmfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
			}
			else
			{
				start = _CliffMaxH * _MapHeight;
				c = clamp((start - IN.worldPos.y), 0, (1+_CliffFadeTop));
				colfade = lerp(colfade, cliff, c/(1+_CliffFadeTop));
				nrmfade = lerp(nrmfade, cliffB, c/(1+_CliffFadeTop));
				
				c = clamp(abs(IN.myNormal.x)-(1-_CliffFade), 0, 1);
				col = lerp(col, colfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
				nrm = lerp(nrm, nrmfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
			}

			if (c>_SteepNes)
			{
				col = lerp(col, cliff, (c * _CliffBlend * _SteepNesBlend)/(_MapHeight*0.05));
			}

			colfade = col;
			nrmfade = nrm;
			
			if (IN.worldPos.y*_CliffFadeTreshold < _MapHeight * _CliffMaxH)
			{
				start = _CliffMinH * _MapHeight;
				c = clamp((IN.worldPos.y-start), 0, (1+_CliffFadeBottom));
				colfade = lerp(colfade, cliff, c/(1+_CliffFadeBottom));
				nrmfade = lerp(nrmfade, cliffB, c/(1+_CliffFadeBottom));
				
				c = clamp(abs(IN.myNormal.z)-(1-_CliffFade), 0, 1);
				col = lerp(col, colfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
				nrm = lerp(nrm, nrmfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
			}
			else
			{
				start = _CliffMaxH * _MapHeight;
				c = clamp((start - IN.worldPos.y), 0, (1+_CliffFadeTop));
				colfade = lerp(colfade, cliff, c/(1+_CliffFadeTop));
				nrmfade = lerp(nrmfade, cliffB, c/(1+_CliffFadeTop));
				
				c = clamp(abs(IN.myNormal.z)-(1-_CliffFade), 0, 1);
				col = lerp(col, colfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
				nrm = lerp(nrm, nrmfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
			}
			
			if (c>_SteepNes)
			{
				col = lerp(col, cliff, (c * _CliffBlend * _SteepNesBlend)/(_MapHeight*0.05));
			}
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			uv = IN.worldPos.zx;
            uv.x *= _Scale;
            uv.y *= _Scale;

			fixed start;
			float4 texture1, texture2, texture3, texture4, texture5;
			float4 normal1, normal2, normal3, normal4, normal5, normal6;
			texture1 = tex2D(_Texture0, uv); texture2 = tex2D(_Texture1, uv); 
			texture3 = tex2D(_Texture2, uv); texture4 = tex2D(_Texture3, uv);
			texture5 = tex2D(_Texture4, uv); 

			normal1 = tex2D(_NormalTex0, uv); normal2 = tex2D(_NormalTex1, uv);
			normal3 = tex2D(_NormalTex2, uv); normal4 = tex2D(_NormalTex3, uv);
			normal5 = tex2D(_NormalTex4, uv); //normal6 = tex2D(_NormalTex5, uv);
	
			col = texture1;											// · Initialize with first texture 
			nrm = normal1;
																	
			start = _Tex2Height * _MapHeight;						// · Normalize height start value
			c = clamp((IN.worldPos.y - start), 0, 1+_Tex2Blend);	// · Calcule clamp value in current position Y
			col = lerp(col, texture2, c/(_Tex2Blend+1));			// · Calcule interpolated value with current texture and 
			nrm = lerp(nrm, normal2, c/(_Tex2Blend+1));				//   clamp value. Gets smoothed transition in limit values
			
			start = _Tex3Height * _MapHeight;
			c = clamp((IN.worldPos.y - start), 0, 1+_Tex3Blend);
			col = lerp(col, texture3, c/(_Tex3Blend+1));
			nrm = lerp(nrm, normal3, c/(_Tex3Blend+1));
			
			start = _Tex4Height * _MapHeight;
			c = clamp((IN.worldPos.y - start), 0, 1+_Tex4Blend);
			col = lerp(col, texture4, c/(_Tex4Blend+1));
			nrm = lerp(nrm, normal4, c/(_Tex4Blend+1));

			start = _Tex5Height * _MapHeight;
			c = clamp((IN.worldPos.y - start), 0, 1+_Tex5Blend);
			col = lerp(col, texture5, c/(_Tex5Blend+1));
			nrm = lerp(nrm, normal5, c/(_Tex5Blend+1));

			DrawCliff(IN);

			o.Albedo = col.rgb * _Color;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = col.a;

			//fixed splatSum = dot(col, fixed4(1,1,1,1));
			//fixed4 flatNormal = fixed4(0.5,0.5,1,0.5); // this is "flat normal" in both DXT5nm and xyz*2-1 cases
			//nrm = lerp(flatNormal, nrm, splatSum);
			o.Normal = UnpackNormal(nrm);
			//o.Normal = UnpackNormal( tex2D(_CliffBump, uv) );
		}
		ENDCG
	} 

	

	Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Standard-AddPass"
	Dependency "BaseMapShader" = "Hidden/TerrainEngine/Splatmap/Standard-Base"

	Fallback "Nature/Terrain/Diffuse"
}
