Shader "Infinite Runner/Diffuse CurveNormal" {
	Properties {
		_MainTex ("Main Texture", 2D) = "black" {}
		_FadeOutColor ("Fade Out Color", Color) = (0, 0, 0, 0)
		_NearCurve ("Near Curve", Vector) = (0, 0, 0, 0)
		_FarCurve ("Far Curve", Vector) = (0, 0, 0, 0)
		_Dist ("Distance Mod", Float) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }   // diese Tags werden auf alle Passes innerhalb dieses SubShaders angewendet
		
		
		LOD 200
		
        Pass { // pass 0

			Tags { "LightMode" = "ForwardBase" }   // diese Tags werden nur auf diesen Pass angewendet

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#pragma multi_compile_fwdbase LIGHTMAP_OFF LIGHTMAP_ON
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" // for _LightColor0
			#ifndef SHADOWS_OFF		
			#include "AutoLight.cginc"	
			#endif
						
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform half4 	_MainTex_TexelSize;
			// uniform sampler2D unity_Lightmap;
			// uniform half4 unity_LightmapST;
			uniform float4 _FadeOutColor;
			uniform float4 _NearCurve;
			uniform float4 _FarCurve;
			uniform float _Dist;

			//uniform float4 _LightColor0;
			
			struct fragmentInput
			{
				fixed4 diff : COLOR0;
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half2 uvLM : TEXCOORD1;
				float distanceSquared : TEXCOORD2;
				#ifndef SHADOWS_OFF
		        LIGHTING_COORDS(3,4)
				#endif
			};
						
			fragmentInput vert(appdata_full v)  //appdata_full ist vorgefüllt von Unity. Es enthält alle Sachen über die Mesh, an die dieser Shader klebt(z.B. UV, Normal, Texture1 und Texture2 Daten)
			{
				fragmentInput o;

				// Apply the curve
                float4 pos = mul(UNITY_MATRIX_MV, v.vertex); 					// P von MVP ausgelassen
                o.distanceSquared = pos.z * pos.z * _Dist;						
                pos.x += (_NearCurve.x - max(1.0 - o.distanceSquared / _FarCurve.x, 0.0) * _NearCurve.x);   //x² Funktion (pos.z * pos.z). Die Kurve biegt sich exponentiell. Evtl. 
                pos.y += (_NearCurve.y - max(1.0 - o.distanceSquared / _FarCurve.y, 0.0) * _NearCurve.y);
                o.pos = mul(UNITY_MATRIX_P, pos); 								// P hier benutzt
				o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uvLM = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				
				
				
				//neu: für Normal
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = nl*_LightColor0;
				//neu: End

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1.0-o.uv.y;
				#endif
				
				#ifndef SHADOWS_OFF			  	
      			TRANSFER_VERTEX_TO_FRAGMENT(o);
				#endif

				return o;
			}
			
			fixed4 frag(fragmentInput i) : COLOR
			{
				fixed4 color = tex2D(_MainTex, i.uv);

				#ifdef LIGHTMAP_ON
				fixed3 lm = DecodeLightmap (UNITY_SAMPLE_TEX2D (unity_Lightmap, i.uvLM));
				color.rgb *= lm;
				#endif
				
				#ifndef SHADOWS_OFF			  	
				fixed atten = LIGHT_ATTENUATION(i);
				color.rgb *= atten;
				#endif
				
				//neu
				color *= i.diff;
				
				return lerp(color, _FadeOutColor, max(i.distanceSquared  / _FarCurve.z, 0.0));  //***Gradient Verlauf soll früher beginnen. max(a,b)  returns the larger Number. 
			}									//Je niedriger _FarCurve.z -> desto früher der Gradient Übergang																																						
			ENDCG
        } // end pass
	} 
	FallBack "Diffuse"
}
