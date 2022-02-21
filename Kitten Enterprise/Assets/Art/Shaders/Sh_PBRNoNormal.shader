// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sh_PBRNoNormal"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_RMA("RMA", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_AOStrength("AOStrength", Range( 0 , 1)) = 1
		_EmissionMask("EmissionMask", 2D) = "white" {}
		[HDR]_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _EmissionMask;
		uniform float4 _EmissionMask_ST;
		uniform float4 _EmissionColor;
		uniform sampler2D _RMA;
		uniform float4 _RMA_ST;
		uniform float _AOStrength;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = ( _Color * tex2D( _Albedo, uv_Albedo ) ).rgb;
			float2 uv_EmissionMask = i.uv_texcoord * _EmissionMask_ST.xy + _EmissionMask_ST.zw;
			o.Emission = ( tex2D( _EmissionMask, uv_EmissionMask ).r * _EmissionColor ).rgb;
			float2 uv_RMA = i.uv_texcoord * _RMA_ST.xy + _RMA_ST.zw;
			float4 tex2DNode2 = tex2D( _RMA, uv_RMA );
			o.Metallic = tex2DNode2.g;
			o.Smoothness = ( 1.0 - tex2DNode2.r );
			float lerpResult7 = lerp( 1.0 , tex2DNode2.b , _AOStrength);
			o.Occlusion = lerpResult7;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
6.4;5.6;1523.2;797.4;987.3033;-536.0594;1;True;True
Node;AmplifyShaderEditor.SamplerNode;2;-622,209.3;Inherit;True;Property;_RMA;RMA;1;0;Create;True;0;0;0;False;0;False;-1;None;9e3baba5169c6ed42b9258c79f979cb9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-558,-16.50002;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-476.4025,426.7099;Inherit;False;Constant;_NoAOValue;NoAOValue;3;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-527.4025,552.7099;Inherit;False;Property;_AOStrength;AOStrength;3;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-531.6001,-237.1001;Inherit;False;Property;_Color;Color;2;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-560.1234,719.7825;Inherit;True;Property;_EmissionMask;EmissionMask;4;0;Create;True;0;0;0;False;0;False;-1;None;c41e5135a2ec0a7428c78dea9d6c25e9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;-488.4608,954.4812;Inherit;False;Property;_EmissionColor;EmissionColor;5;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;2,2,2,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;3;-240,155.3;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-165.6001,-30.10004;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;7;-186.4025,377.7099;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-161.3786,809.2159;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;176,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Sh_PBRNoNormal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;1
WireConnection;5;0;4;0
WireConnection;5;1;1;0
WireConnection;7;0;8;0
WireConnection;7;1;2;3
WireConnection;7;2;9;0
WireConnection;11;0;10;1
WireConnection;11;1;12;0
WireConnection;0;0;5;0
WireConnection;0;2;11;0
WireConnection;0;3;2;2
WireConnection;0;4;3;0
WireConnection;0;5;7;0
ASEEND*/
//CHKSM=624AA2560B39F38CE39C58FB949A9227A0B9D1D8