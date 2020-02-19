Shader "Nature/Terrain/MyCustomTerrainShader" {
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

		_Tex2Height ("Texture2-Height", Range(0,1)) = 0.5
		_Tex2Blend ("Texture2-Blend", Range(0,100)) = 0
		
		_Tex3Height ("Texture3-Height", Range(0,1)) = 0.5
		_Tex3Blend ("Texture3-Blend", Range(0,100)) = 0

		_Tex4Height ("Texture4-Height", Range(0,1)) = 0.5
		_Tex4Blend ("Texture4-Blend", Range(0,100)) = 0


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


		// Transition data //
		_Transition1 ("Transition1", Float) = 2000	
		_Transition2 ("Transition2", Float) = 2000
	
		_TransitionTex1 ("TransitionTex1", 2D) = "white" {}
		_TransitionTex2 ("TransitionTex2", 2D) = "white" {}
		_TransitionTex3 ("TransitionTex3", 2D) = "white" {}
		_TransitionTex4 ("TransitionTex4", 2D) = "white" {}

		_TransitionTex11 ("TransitionTex11", 2D) = "white" {}
		_TransitionTex12 ("TransitionTex12", 2D) = "white" {}
		_TransitionTex13 ("TransitionTex13", 2D) = "white" {}
		_TransitionTex14 ("TransitionTex14", 2D) = "white" {}
	
	
		_TransitionPos1 ("TransitionPos1", Float) = 0
		_TransitionPos2 ("TransitionPos2", Float) = 0
	
		_TransitionType1 ("TransitionType1", Float) = 0
		_TransitionType2 ("TransitionType2", Float) = 0
	
		_LimitMin ("LimitMin", Float) = 0
		_LimitMax ("LimitMax", Float) = 2000
		

		_MapHeight ("MapHeight", Float) = 600		
		_MapSize ("MapSize", Float) = 500
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		
		CGPROGRAM
		#pragma surface surf Standard vertex:vert
		#pragma target 3.0
		
		sampler2D _Texture0,_Texture1,_Texture2,_Texture3;
		sampler2D _Cliff, _CliffBump;

		struct Input {			
			float3 localPos;
			float2 uv_Cliff;
			float3 worldNormal;
			INTERNAL_DATA
		};

		void vert (inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
			v.tangent.xyz = cross(v.normal, float3(0,0,1));
			v.tangent.w = -1;
			data.localPos = v.vertex;
		}

		half _Glossiness;
		half _Metallic;

		fixed _MapHeight, _MapSize;
		fixed _Tex1Height, _Tex2Height, _Tex3Height, _Tex4Height;
		fixed _Tex1Blend, _Tex2Blend, _Tex3Blend, _Tex4Blend;
		fixed _CliffMinH, _CliffMaxH, _SteepNes;	
		fixed _CliffBlend, _SteepNesBlend, _CliffFade;	 
		fixed _CliffFadeBottom, _CliffFadeTop;
		fixed _CliffFadeTreshold;


		fixed _Transition1, _Transition2; 
		sampler2D _TransitionTex1, _TransitionTex2, _TransitionTex3, _TransitionTex4;
		sampler2D _TransitionTex11, _TransitionTex12, _TransitionTex13, _TransitionTex14;
		fixed _TransitionPos1, _TransitionPos2;
		fixed _TransitionType1, _TransitionType2;
		fixed _LimitMax, _LimitMin;
		fixed startPos, startPos1;
		fixed limit, limit1;

		fixed4 _Color, col;
		fixed c; fixed4 nrm;
		fixed _Scale;
		fixed2 uv;

		void DrawCliff(Input IN)
		{
			fixed4 cliff = tex2D(_Cliff, IN.uv_Cliff);
			fixed start;
			
			float4 colfade = col;

			if (IN.localPos.y*_CliffFadeTreshold < _MapHeight * _CliffMaxH)
			{
				start = _CliffMinH * _MapHeight;
				c = clamp((IN.localPos.y-start), 0, (1+_CliffFadeBottom));
				colfade = lerp(colfade, cliff, c/(1+_CliffFadeBottom));
				
				c = clamp(abs(IN.worldNormal.x)-(1-_CliffFade), 0, 1);
				col = lerp(col, colfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
			}
			else
			{
				start = _CliffMaxH * _MapHeight;
				c = clamp((start - IN.localPos.y), 0, (1+_CliffFadeTop));
				colfade = lerp(colfade, cliff, c/(1+_CliffFadeTop));
				
				c = clamp(abs(IN.worldNormal.x)-(1-_CliffFade), 0, 1);
				col = lerp(col, colfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
			}

			if (c>_SteepNes)
			{
				col = lerp(col, cliff, (c * _CliffBlend * _SteepNesBlend)/(_MapHeight*0.05));
			}

			colfade = col;
			
			if (IN.localPos.y*_CliffFadeTreshold < _MapHeight * _CliffMaxH)
			{
				start = _CliffMinH * _MapHeight;
				c = clamp((IN.localPos.y-start), 0, (1+_CliffFadeBottom));
				colfade = lerp(colfade, cliff, c/(1+_CliffFadeBottom));
				
				c = clamp(abs(IN.worldNormal.z)-(1-_CliffFade), 0, 1);
				col = lerp(col, colfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
			}
			else
			{
				start = _CliffMaxH * _MapHeight;
				c = clamp((start - IN.localPos.y), 0, (1+_CliffFadeTop));
				colfade = lerp(colfade, cliff, c/(1+_CliffFadeTop));
				
				c = clamp(abs(IN.worldNormal.z)-(1-_CliffFade), 0, 1);
				col = lerp(col, colfade, (c * _CliffBlend * 8)/(_MapHeight*0.05));
			}
			
			if (c>_SteepNes)
			{
				col = lerp(col, cliff, (c * _CliffBlend * _SteepNesBlend)/(_MapHeight*0.05));
			}
		}

		void SetTransitionPosition(Input IN, fixed transitionPos)
		{
			fixed m_transitionPos = transitionPos;
		
			if (m_transitionPos == 1)
			{
				startPos = IN.localPos.x;
				limit = IN.localPos.z;
			}
			else if (m_transitionPos == 2)
			{
				startPos = _MapSize - IN.localPos.z; 
				limit = IN.localPos.x;
			}
			else if (m_transitionPos == 3)
			{
				startPos = _MapSize - IN.localPos.x;
				limit = _MapSize - IN.localPos.z;
			}
			else if (m_transitionPos == 4)
			{
				startPos = IN.localPos.z;
				limit = _MapSize - IN.localPos.x;
			}
		}
		
		fixed ResolveTransitionType(fixed transitionType, fixed transitionId)
		{
			fixed m_transitionType = transitionType;
			fixed tam = _LimitMax - _LimitMin;
			fixed transitionlimit;
			fixed transitionLenght = (transitionId == 1 ? _Transition1 : _Transition2);
		
			if (m_transitionType == 1)
				transitionlimit = (limit - _LimitMin);
			else if (m_transitionType == 2)
				transitionlimit = tam - limit - _LimitMin;
			else if (m_transitionType == 3)
			{
				/*transitionlimit = (limit-_LimitMin)*2;
				if (limit >= (tam*0.5))
					transitionlimit = (tam - limit - _LimitMin)*2;*/
			
				startPos = _MapSize - startPos;
				limit = _MapSize - limit;
		
				float mitad = _MapSize * 0.5;
				limit = limit - mitad;
				transitionlimit = ((limit * limit)/(_MapSize*0.3))*1.5 + transitionLenght;
				//transitionlimit = ((2*(limit * limit * limit)/50) - (3*(limit * limit)) - (3*limit) + 2 ) / 100;
		
				if (limit < 0)
					_LimitMin = _LimitMin - mitad;
			}
			else
				transitionlimit = transitionLenght;
		
		
			transitionlimit = (transitionlimit/_MapSize) * transitionLenght;
		
			return transitionlimit;
		}
		
		void DrawTransition(Input IN, fixed faderate, fixed transitionId)
		{
			fixed4 transcol;fixed start;
			float4 texture1, texture2, texture3, texture4;
		
			if (transitionId == 1)
			{
				texture1 = tex2D(_TransitionTex1, uv);
				texture2 = tex2D(_TransitionTex2, uv);
				texture3 = tex2D(_TransitionTex3, uv);
				texture4 = tex2D(_TransitionTex4, uv);
			}
			else
			{
				texture1 = tex2D(_TransitionTex11, uv);
				texture2 = tex2D(_TransitionTex12, uv);
				texture3 = tex2D(_TransitionTex13, uv);
				texture4 = tex2D(_TransitionTex14, uv);
			}
				
			transcol = texture1;

			start = _Tex2Height * _MapHeight;
			c = clamp((IN.localPos.y - start), 0, 1+_Tex2Blend);
			transcol = lerp(transcol, texture2, c/(_Tex2Blend+1));
		
			start = _Tex3Height * _MapHeight;
			c = clamp((IN.localPos.y - start), 0, 1+_Tex3Blend);
			transcol = lerp(transcol, texture3, c/(_Tex3Blend+1));
			
			start = _Tex4Height * _MapHeight;
			c = clamp((IN.localPos.y - start), 0, 1+_Tex4Blend);
			transcol = lerp(transcol, texture4, c/(_Tex4Blend+1));
		
			transcol = lerp(transcol, col, faderate*faderate*faderate*faderate); //Degradate
			
			col = transcol;
		}
		
		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			uv = IN.localPos.zx;
            uv.x *= _Scale;
            uv.y *= _Scale;

			fixed start;
			fixed4 texture1, texture2, texture3, texture4;
			texture1 = tex2D(_Texture0, uv); texture2 = tex2D(_Texture1, uv); 
			texture3 = tex2D(_Texture2, uv); texture4 = tex2D(_Texture3, uv);

			col = texture1;											// · Initialize with first texture 

			start = _Tex2Height * _MapHeight;						// · Normalize height start value
			c = clamp((IN.localPos.y - start), 0, 1+_Tex2Blend);	// · Calcule clamp value in current position Y
			col = lerp(col, texture2, c/(_Tex2Blend+1));			// · Calcule interpolated value with current texture and 
																	//   clamp value. Gets smoothed transition in limit values
			start = _Tex3Height * _MapHeight;
			c = clamp((IN.localPos.y - start), 0, 1+_Tex3Blend);
			col = lerp(col, texture3, c/(_Tex3Blend+1));
			
			start = _Tex4Height * _MapHeight;
			c = clamp((IN.localPos.y - start), 0, 1+_Tex4Blend);
			col = lerp(col, texture4, c/(_Tex4Blend+1));
			

			//**********************************************************************************************************************************
			//**********************************************************************************************************************************
			
			startPos = _Transition1+1;
			
			SetTransitionPosition(IN, _TransitionPos1);
			
			fixed transitionlimit = ResolveTransitionType(_TransitionType1, 1);
			
			bool condition;
			fixed faderate;
			if (_TransitionType1 == 3)
			{
				condition = (startPos >= transitionlimit);
				faderate = transitionlimit/startPos;
			}
			else
			{
				condition = (startPos <= transitionlimit);
				faderate = startPos/transitionlimit;
			}
			
			if (condition && limit >= _LimitMin && limit <= _LimitMax)
				DrawTransition(IN, faderate, 1);
			
			
			//**********************************************************************************************************************************
			//**********************************************************************************************************************************
			
			startPos = _Transition2+1;
			
			SetTransitionPosition(IN, _TransitionPos2);
			
			transitionlimit = ResolveTransitionType(_TransitionType2, 2);
			
			if (_TransitionType2 == 3)
			{
				condition = (startPos >= transitionlimit);
				faderate = transitionlimit/startPos;
			}
			else
			{
				condition = (startPos <= transitionlimit);
				faderate = startPos/transitionlimit;
			}
			
			if (condition && limit >= _LimitMin && limit <= _LimitMax)
				DrawTransition(IN, faderate, 2);
			
			//**********************************************************************************************************************************
			//**********************************************************************************************************************************

			DrawCliff(IN);

			o.Albedo = col.rgb * _Color;
			o.Alpha = col.a;

			//fixed splatSum = dot(col, fixed4(1,1,1,1));
			//fixed4 flatNormal = fixed4(0.5,0.5,1,0.5); // this is "flat normal" in both DXT5nm and xyz*2-1 cases
			//nrm = lerp(flatNormal, nrm, splatSum);
			//o.Normal = UnpackNormal(nrm);
			
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	} 

	

	Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Standard-AddPass"
	Dependency "BaseMapShader" = "Hidden/TerrainEngine/Splatmap/Standard-Base"

	Fallback "Nature/Terrain/Diffuse"
}
