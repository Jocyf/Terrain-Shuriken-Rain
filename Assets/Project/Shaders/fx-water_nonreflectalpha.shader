// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

Shader "FX/WaterPlane (transparent)" {
Properties {
	_horizonColor ("Horizon color", COLOR)  = ( .172 , .463 , .435 , 0)
	_WaveScale ("Wave scale", Range (0.02,0.15)) = .07
	_ColorControl ("Reflective color (RGB) fresnel (A) ", 2D) = "" { }
	_ColorControlCube ("Reflective color cube (RGB) fresnel (A) ", Cube) = "" { TexGen CubeReflect }
	_BumpMap ("Waves Bumpmap (RGB) ", 2D) = "" { }
	WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,1)
	_MainTex ("Fallback texture", 2D) = "" { }
}

CGINCLUDE
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct appdata members vertex,normal)
#pragma exclude_renderers d3d11 xbox360
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct appdata members vertex,normal)
#pragma exclude_renderers xbox360
// -----------------------------------------------------------
// This section is included in all program sections below

#include "UnityCG.cginc"

uniform float4 _horizonColor;

// Wave speed (map1 x,y; map2 x,y)
uniform float4 WaveSpeed;
uniform float _WaveScale;
	
struct appdata {
	float4 vertex : POSITION; 
	float3 normal : MORMAL; 
	float3 tangent : TANGENT;
};

struct v2f {
	float4 pos : SV_POSITION;
	float2 bumpuv[2] : TEXCOORD0;
	float3 viewDir : TEXCOORD3;
};

v2f vert(appdata v)
{
	v2f o;
	float4 s;

	o.pos = UnityObjectToClipPos (v.vertex);

	// scroll bump waves
	float4 temp;
	temp.xyzw = (v.vertex.xzxz + _Time.x * WaveSpeed.xyzw) * _WaveScale;
	o.bumpuv[0] = temp.xy * float2(.4, .45);
	o.bumpuv[1] = temp.wz;

	// object space view direction
	o.viewDir.xzy = normalize( ObjSpaceViewDir(v.vertex) );

	return o;
}

ENDCG

// -----------------------------------------------------------
// ARB fragment program

Subshader {
	Tags { "Queue" = "Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	ColorMask RGB
	
	Pass {

CGPROGRAM
// profiles arbfp1
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#pragma fragmentoption ARB_fog_exp2

sampler2D _BumpMap : register(s0);
sampler2D _ColorControl : register(s1);

half4 frag( v2f i ) : COLOR
{
	half3 bump1 = tex2D( _BumpMap, i.bumpuv[0] ).rgb;
	half3 bump2 = tex2D( _BumpMap, i.bumpuv[1] ).rgb;
	half3 bump = bump1 + bump2 - 1;
	
	half fresnel = dot( i.viewDir, bump );
	half4 water = tex2D( _ColorControl, float2(fresnel,fresnel) );
	
	half4 col;
	col.rgb = lerp( water.rgb, _horizonColor.rgb, water.a );
	col.a = water.a;
	return col;
}
ENDCG
		SetTexture [_BumpMap] {}
		SetTexture [_ColorControl] {}
	}
}

// -----------------------------------------------------------
// Radeon 9000

#warning Upgrade NOTE: SubShader commented out because of manual shader assembly
/*Subshader {
	Tags { "Queue" = "Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	ColorMask RGB
	
	Pass {

CGPROGRAM
#pragma vertex vert
// just define 'vert' as a vertex shader, the code is included
// from the section on top
ENDCG

		Program "" {
			SubProgram {
				Local 0, [_horizonColor]

"!!ATIfs1.0
StartConstants;
	CONSTANT c0 = program.local[0];
EndConstants;

StartPrelimPass;
	SampleMap r0, t0.str;
	SampleMap r1, t1.str;
	PassTexCoord r2, t2.str;
	
	ADD r1, r0.bias, r1.bias;	# bump = bump1 + bump2 - 1
	DOT3 r2, r1, r2;			# fresnel: dot (bump, viewer-pos)
EndPass;

StartOutputPass;
 	SampleMap r2, r2.str;

	LERP r0.rgb, r2.a, c0, r2;	# fade in reflection
	MOV r0.a, r2.a;
EndPass;
" 
}
}
		SetTexture [_BumpMap] {}
		SetTexture [_BumpMap] {}
		SetTexture [_ColorControl] {}
	}
}*/

// -----------------------------------------------------------
//  Old cards

Subshader {
	Pass {
		SetTexture [_ColorControlCube] {
			constantColor (0.0,0.0,0.0,0.5)
			combine texture + constant
			Matrix [_Reflection]
		}
		SetTexture [_BumpMap] {
			constantColor [_horizonColor]
			combine constant lerp (previous) previous, constant
		}
	}
}

// -----------------------------------------------------------
//  Really old cards (no cubemapping)

Subshader {
	Pass {
		SetTexture [_MainTex] { combine texture }
	}
}

}
