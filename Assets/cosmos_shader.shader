Shader "Unlit/cosmos_shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float2 screenpos:TEXCOORD2;
				float3 cameradir: TEXCOORD3;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.screenpos = ComputeScreenPos(o.vertex);
				o.cameradir = WorldSpaceViewDir(o.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			#define PI 3.141592653589793

			inline float2 RadialCoords(float3 a_coords)
			{
				float3 a_coords_n = normalize(a_coords);
				float lon = atan2(a_coords_n.z, a_coords_n.x);
				float lat = acos(a_coords_n.y);
				float2 sphereCoords = float2(lon, lat) * (1.0 / PI);
				return float2(sphereCoords.x * 0.5 + 0.5, 1 - sphereCoords.y);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				// fixed4 col = tex2D(_MainTex, i.uv);
				float2 uv = RadialCoords(i.cameradir);
				fixed4 col = tex2D(_MainTex, uv + i.screenpos);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
