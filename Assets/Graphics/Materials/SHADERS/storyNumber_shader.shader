Shader "Unlit/storyNumber_shader"
{
	Properties
	{
		_Color("first Color", Color) = (1,1,1,1)
		_Color2("second Color", Color) = (1,1,1,1)
		_speed("blink speed", range(0,5)) = 1
		_SineMag("Sine amplitude", range(0,1)) = 1
		_push("push value",Range(-1,1)) = 1
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
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;
			fixed4 _Color2;
			float _speed;
			float _SineMag;
			float _push;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				float pi =  3.1415926535;				

				fixed4 col = lerp(_Color, _Color2, sin(_Time.w*pi*_speed)*_SineMag + _push);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
