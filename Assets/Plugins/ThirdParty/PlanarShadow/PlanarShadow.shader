Shader "Unlit/PlanarShadow"
{
	Properties
	{
        Center("Center", Vector) = (0,0,0,0)
        Normal("Normal", Vector) = (0,1,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};            

            float4 Center, Normal;
			v2f vert (appdata v)
			{
				v2f o;
                float4 wPos = mul(unity_ObjectToWorld, v.vertex);
                // directional light
                float3 direction = normalize(_WorldSpaceLightPos0.xyz);
                // https://en.wikipedia.org/wiki/Line%E2%80%93plane_intersection
                float dist = dot(Center.xyz - wPos.xyz, Normal.xyz) / dot(direction, Normal.xyz);
                wPos.xyz = wPos.xyz + dist * direction;
				o.vertex = mul(unity_MatrixVP, wPos);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return fixed4(0,0,0,1);
			}
			ENDCG
		}
	}
}