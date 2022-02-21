// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Triplanar Simple RMA"
{
	Properties
	{
		_TriplanarAlbedo("Triplanar Albedo", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.5
		_CoverageFalloff("Coverage Falloff", Range( 0.01 , 5)) = 0.5
		_Tiling("Tiling", Float) = 1
		_Offset("Offset", Vector) = (0,0,0,0)
		_Tint("Tint", Color) = (1,1,1,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZTest LEqual
		CGPROGRAM
		#pragma target 3.0
		#pragma exclude_renderers xboxseries playstation switch nomrt 
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _TriplanarAlbedo;
		uniform float _CoverageFalloff;
		uniform float _Tiling;
		uniform float3 _Offset;
		uniform float4 _Tint;
		uniform float _Smoothness;


		inline float4 TriplanarSampling343( sampler2D topTexMap, sampler2D midTexMap, sampler2D botTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			float negProjNormalY = max( 0, projNormal.y * -nsign.y );
			projNormal.y = max( 0, projNormal.y * nsign.y );
			half4 xNorm; half4 yNorm; half4 yNormN; half4 zNorm;
			xNorm  = tex2D( midTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm  = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			yNormN = tex2D( botTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm  = tex2D( midTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + yNormN * negProjNormalY + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 triplanar343 = TriplanarSampling343( _TriplanarAlbedo, _TriplanarAlbedo, _TriplanarAlbedo, ( ( _Tiling * ase_worldPos ) + _Offset ), ase_worldNormal, _CoverageFalloff, float2( 1,1 ), float3( 1,1,1 ), float3(0,0,0) );
			o.Albedo = ( triplanar343 * _Tint ).xyz;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
6.4;11.2;1523.2;791.8;-963.9005;310.4632;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;358;446.8327,121.6339;Inherit;False;Property;_Tiling;Tiling;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;344;426.6263,217.2297;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;359;680.1137,236.5411;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;361;592.9006,433.3368;Inherit;False;Property;_Offset;Offset;4;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;339;672,-48;Float;True;Property;_TriplanarAlbedo;Triplanar Albedo;0;0;Create;True;0;0;0;False;0;False;None;b297077dae62c1944ba14cad801cddf5;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;352;731.2036,542.9332;Float;False;Property;_CoverageFalloff;Coverage Falloff;2;0;Create;True;0;0;0;False;0;False;0.5;2;0.01;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;360;867.9006,340.3368;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TriplanarNode;343;1088,64;Inherit;True;Cylindrical;World;False;Top Texture 0;_TopTexture0;white;-1;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT3;1,1,1;False;3;FLOAT2;1,1;False;4;FLOAT;2;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;356;1555.564,243.1017;Inherit;False;Property;_Tint;Tint;5;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;357;2006.295,159.0238;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;213;1249.363,645.4126;Float;False;Property;_Smoothness;Smoothness;1;0;Create;True;0;0;0;False;0;False;0.5;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2200.728,173.2577;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;Triplanar Simple RMA;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;d3d9;d3d11_9x;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;ps4;psp2;n3ds;wiiu;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;359;0;358;0
WireConnection;359;1;344;0
WireConnection;360;0;359;0
WireConnection;360;1;361;0
WireConnection;343;0;339;0
WireConnection;343;1;339;0
WireConnection;343;2;339;0
WireConnection;343;9;360;0
WireConnection;343;4;352;0
WireConnection;357;0;343;0
WireConnection;357;1;356;0
WireConnection;0;0;357;0
WireConnection;0;4;213;0
ASEEND*/
//CHKSM=3425CD727AC8D5BE2E34D776BCA6B042DF2FD4E0