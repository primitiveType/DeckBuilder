Shader "Projector/Caustic" {
	Properties{
		_Color("Main Color", Color) = (0,0,1,0.7)
		_ColorT("Tint", Color) = (0,1,1,0.6)
		_Mask("Mask", 2D) = "" {}
		_Noise("Noise", 2D) = "black" {}	
		_Tile("Tile", 2D) = "" {}	
		_FalloffTex("FallOff", 2D) = "white" {}
		_Scale("Scale", Range(0,1)) = 0.1
		_Speed("Speed", Range(0,10)) = 1
		_Intensity("Intensity", Range(0,10)) = 5
		_NoiseScale("Noise Scale", Range(0,1)) = 0.1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendOp ("Blend Op", Int) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendMode ("Blend Mode", Int) = 10
	}

	Subshader{
		Tags {"Queue" = "Transparent"}
		Pass {
			ZWrite Off
			Cull Off
			ColorMask RGB
			Blend [_BlendOp] [_BlendMode]
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag  
			#include "UnityCG.cginc"

			struct v2f {
				float4 uvMask : TEXCOORD0;
				float4 pos : SV_POSITION;
				float4 uvFalloff : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;					
				};

			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			
			v2f vert(appdata_full v)
			{
				v2f o;								
				o.uvFalloff = mul(unity_ProjectorClip, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uvMask = mul(unity_Projector, v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = normalize(mul(float4(v.normal, 0.0), unity_ObjectToWorld).xyz);
				return o;
			}

			float4 _Color, _ColorT;
			sampler2D _Tile, _Mask, _FalloffTex, _Noise;
			float _Scale, _Intensity, _Speed, _NoiseScale;
			
			fixed4 triplanar(float3 blendNormal, float4 texturex, float4 texturey, float4 texturez)
			{					
				float4 triplanartexture = texturez;
				triplanartexture = lerp(triplanartexture, texturex, blendNormal.x);
				triplanartexture = lerp(triplanartexture, texturey, blendNormal.y);
				return triplanartexture;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float speed = _Time.x * _Speed;
				float3 blendNormal = saturate(pow(i.worldNormal * 1.4,4));

				// distortion
				float4 distortx = tex2D(_Noise, float2(i.worldPos.zy * _NoiseScale) - (speed));
				float4 distorty = tex2D(_Noise, float2(i.worldPos.xz * _NoiseScale)  -(speed));
				float4 distortz = tex2D(_Noise, float2(i.worldPos.xy * _NoiseScale)  - (speed));
					
				float distort = triplanar(blendNormal, distortx , distorty, distortz);
					
				// moving caustics
				float4 xc = tex2D(_Tile, float2((i.worldPos.z +distort) * _Scale, (i.worldPos.y) * (_Scale/4)));
				float4 zc = tex2D(_Tile, float2((i.worldPos.x +distort) * _Scale, (i.worldPos.y ) * (_Scale/4)));
				float4 yc = tex2D(_Tile, (float2(i.worldPos.x + distort, i.worldPos.z + distort) )* _Scale );

				float4 causticsTex = triplanar(blendNormal, xc , yc, zc);
					
				// secondary moving caustics, smaller scale and moving opposite direction
				float secScale = _Scale * 0.6;
				float4 xc2 = tex2D(_Tile, float2((i.worldPos.z -distort) * secScale, (i.worldPos.y ) * (secScale/4)));
				float4 zc2 = tex2D(_Tile, float2((i.worldPos.x -distort) * secScale, (i.worldPos.y ) * (secScale/4)));
				float4 yc2 = tex2D(_Tile, float2(i.worldPos.x - distort, i.worldPos.z - distort)* secScale);

				float4 causticsTex2 = triplanar(blendNormal, xc2, yc2, zc2);
					
				// combining
				causticsTex *= causticsTex2;
				causticsTex *= _Intensity * _ColorT;
					
				// alpha
				float falloff = tex2Dproj(_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff)).a;
				float alphaMask = tex2Dproj(_Mask, UNITY_PROJ_COORD(i.uvMask)).a;
				float alpha = falloff * alphaMask;

				// texture and color times alpha
				_Color *= alpha * _Color.a;
				causticsTex *= alpha;

				return causticsTex + _Color;
			}
			ENDCG
		}
	}
}