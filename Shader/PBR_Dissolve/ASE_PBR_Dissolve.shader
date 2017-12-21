// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/PBR_Dissolve"
{
	Properties
	{
		_AlbedoColor("Albedo Color", Color) = (0.5,0.5,0.5,1)
		_Albedo("Albedo", 2D) = "white" {}
		_MetallicRSmoothnessA("Metallic(R)Smoothness(A)", 2D) = "black" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Normal("Normal", 2D) = "bump" {}
		_AmbientOcclusion("Ambient Occlusion", 2D) = "white" {}
		_EmissionIntensity("EmissionIntensity", Range( 0 , 3)) = 2
		_Emission("Emission", 2D) = "black" {}
		_DissolveIntensity("DissolveIntensity", Range( 0 , 1)) = 1
		_Noise("Noise", 2D) = "white" {}
		_Dissolve("Dissolve", Range( 0 , 1)) = 0
		_DissolveColor("DissolveColor", Color) = (1,1,1,1)
		_Ramp("Ramp", 2D) = "white" {}
		_DissolveRes("DissolveRes", Range( 0.0001 , 0.1)) = 0.5411765
		_Cutoff( "Mask Clip Value", Float ) = 0.51
		[HideInInspector]_Float0("Float 0", Range( -1 , 0)) = 0.494
		[HideInInspector]_Float1("Float 1", Range( 0 , 1)) = 1.4
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float2 uv2_texcoord2;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _AlbedoColor;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _DissolveColor;
		uniform float _Dissolve;
		uniform sampler2D _Noise;
		uniform float4 _Noise_ST;
		uniform float _DissolveRes;
		uniform float _Float0;
		uniform float _Float1;
		uniform sampler2D _Ramp;
		uniform float _DissolveIntensity;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform float _EmissionIntensity;
		uniform sampler2D _MetallicRSmoothnessA;
		uniform float4 _MetallicRSmoothnessA_ST;
		uniform float _Smoothness;
		uniform sampler2D _AmbientOcclusion;
		uniform float4 _AmbientOcclusion_ST;
		uniform float _Cutoff = 0.51;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = ( _AlbedoColor * tex2D( _Albedo, uv_Albedo ) ).rgb;
			float2 uv_Noise = i.uv2_texcoord2 * _Noise_ST.xy + _Noise_ST.zw;
			float Noise35 = tex2D( _Noise, ( ceil( ( uv_Noise / _DissolveRes ) ) * _DissolveRes ) ).r;
			float temp_output_31_0 = ( _Dissolve + Noise35 );
			float clampResult132 = clamp( (0.0 + (( 1.0 - temp_output_31_0 ) - _Float0) * (1.0 - 0.0) / (_Float1 - _Float0)) , 0.0 , 1.0 );
			float temp_output_267_0 = ( 1.0 - clampResult132 );
			float2 appendResult66 = (float2(temp_output_267_0 , 1.0));
			float4 tex2DNode22 = tex2D( _Ramp, appendResult66 );
			float4 Burn28 = ( temp_output_267_0 * tex2DNode22 );
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float4 Emission53 = tex2D( _Emission, uv_Emission );
			float Cutout41 = clampResult132;
			float4 lerpResult58 = lerp( ( _DissolveColor * Burn28 * (1.0 + (_DissolveIntensity - 0.0) * (40.0 - 1.0) / (1.0 - 0.0)) ) , ( Emission53 * _EmissionIntensity ) , Cutout41);
			o.Emission = lerpResult58.rgb;
			float2 uv_MetallicRSmoothnessA = i.uv_texcoord * _MetallicRSmoothnessA_ST.xy + _MetallicRSmoothnessA_ST.zw;
			float4 tex2DNode67 = tex2D( _MetallicRSmoothnessA, uv_MetallicRSmoothnessA );
			float Metallic51 = tex2DNode67.r;
			o.Metallic = Metallic51;
			float Smoothness52 = tex2DNode67.a;
			o.Smoothness = ( Smoothness52 * _Smoothness );
			float2 uv_AmbientOcclusion = i.uv_texcoord * _AmbientOcclusion_ST.xy + _AmbientOcclusion_ST.zw;
			o.Occlusion = tex2D( _AmbientOcclusion, uv_AmbientOcclusion ).r;
			o.Alpha = 1;
			clip( Cutout41 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14001
1927;29;1906;1124;2683.095;506.5199;2.112203;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;283;-3811.788,-321.6638;Float;True;Property;_Noise;Noise;12;0;None;False;white;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.CommentaryNode;276;-3524.759,-370.569;Float;False;1096.788;442.9051;Pixelate;5;239;244;245;237;246;;1,0.7947261,0.6617647,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-3470.521,-51.74947;Float;False;Property;_DissolveRes;DissolveRes;16;0;0.5411765;0.0001;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;239;-3432.759,-317.569;Float;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;245;-3105.633,-229.0683;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CeilOpNode;237;-2886.75,-229.7114;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;287;-3510.586,-463.6638;Float;False;1;0;SAMPLER2D;0.0;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.CommentaryNode;64;-2384.802,-568.2442;Float;False;918.7616;705.7342;Metallic Emission Noise;7;51;52;53;67;69;35;70;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WireNode;286;-2443.886,-403.9637;Float;False;1;0;SAMPLER2D;0.0;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;246;-2582.671,-71.85341;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;49;-5010.854,151.283;Float;False;823.7187;450.3019;Noise Gradient;2;31;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;70;-2268.344,-98.34;Float;True;Property;_noise;noise;10;0;None;True;1;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;36;-4892.283,465.7841;Float;False;35;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-5698.404,342.4004;Float;False;Property;_Dissolve;Dissolve;13;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-1768.445,-64.24525;Float;False;Noise;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-4608.405,337.668;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;262;-3951.793,446.8049;Float;False;303;229.0001;Black & White Field;2;226;225;;0.4411765,0.8150101,1,1;0;0
Node;AmplifyShaderEditor.RelayNode;221;-4109.08,340.132;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;264;-3823.189,340.5178;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;226;-3925.793,579.8047;Float;False;Property;_Float1;Float 1;19;1;[HideInInspector];1.4;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;127;-3584.309,270.3063;Float;False;678.8417;380.5731;CutOut;3;34;132;41;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;225;-3928.793,493.8046;Float;False;Property;_Float0;Float 0;18;1;[HideInInspector];0.494;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;34;-3555.93,337.8242;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;2.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;50;-2872.096,267.5846;Float;False;1076.635;444.1685;Burn;4;22;66;47;267;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;132;-3332.924,338.0403;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;267;-2818.303,336.562;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;66;-2618.555,533.3632;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;22;-2433.222,505.5453;Float;True;Property;_Ramp;Ramp;15;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;65;-1291.042,-656.9462;Float;False;855.0999;789.7008;Emission;10;289;58;189;63;57;231;60;56;43;190;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;69;-2257.344,-321.3401;Float;True;Property;_Emission;Emission;7;0;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;190;-1261.685,-339.9033;Float;False;Property;_DissolveIntensity;DissolveIntensity;9;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-2099.187,336.8765;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-1185.842,-132.1447;Float;False;53;0;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-1246.138,-18.1451;Float;False;Property;_EmissionIntensity;EmissionIntensity;6;0;2;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;-1768.443,-213.6455;Float;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;28;-1714.643,328.2638;Float;False;Burn;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;231;-1228.181,-607.8739;Float;False;Property;_DissolveColor;DissolveColor;14;0;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;67;-2254.249,-520.8398;Float;True;Property;_MetallicRSmoothnessA;Metallic(R)Smoothness(A);2;0;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;43;-1185.943,-434.9466;Float;False;28;0;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;289;-952.9663,-334.6705;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.0;False;4;FLOAT;40.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;192;-5031.876,843.0922;Float;False;939.882;638.227;Vertical Sweep;7;209;193;197;198;195;201;199;;1,0.6323529,0.6323529,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;-936.7394,32.955;Float;False;41;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;116;-5017.722,1592.585;Float;False;835.7007;431.2001;World Normal Gradient;5;176;186;114;171;166;;0.2827098,0.4340764,0.6102941,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;-1772.344,-356.1449;Float;False;Smoothness;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;71;-287.6343,-891.739;Float;False;Property;_AlbedoColor;Albedo Color;0;0;0.5,0.5,0.5,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-916.0383,-128.3455;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;-378.9423,37.6543;Float;False;52;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-3147.827,558.6139;Float;False;Cutout;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;279;-1817.037,888.0204;Float;False;1833.247;177.5524;DEBUG;2;255;271;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;1;-345.6997,-635.9;Float;True;Property;_Albedo;Albedo;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;-918.3436,-516.0203;Float;False;3;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-444.7997,153.4998;Float;False;Property;_Smoothness;Smoothness;3;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;197;-4751.387,1091.648;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;4.365719,-736.7382;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;58;-623.7392,-152.6452;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;193;-5004.989,1382.423;Float;False;Property;_DissolveBottom;_DissolveBottom;11;1;[HideInInspector];1.35;-5;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;139;-3790.559,917.0485;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;255;-246.4447,940.3813;Float;False;1;0;COLOR;0.0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;209;-4264.922,941.7028;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-1772.143,-491.7449;Float;False;Metallic;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;201;-4509.122,940.9393;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT;0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;-4533.058,1770.11;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;199;-4781.518,896.5405;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;176;-4359.566,1769.712;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;278;-5131.563,1037.324;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-294.7005,-395.7992;Float;True;Property;_Normal;Normal;4;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;114;-4720.225,1769.684;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;-160.6417,-46.4399;Float;False;51;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;233;-5203.121,1594.603;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;280;-2454.96,931.0967;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;271;-1767.037,938.0204;Float;False;Property;_ToggleSwitch0;Toggle Switch0;20;0;1;2;0;COLOR;0.0;False;1;COLOR;0.0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;171;-4962.575,1644.512;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-1.0;False;4;FLOAT;4.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-114.0647,85.1394;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-4051.363,1210.129;Float;False;Property;_NoiseOrVertical;NoiseOrVertical;8;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;198;-5009.906,896.4865;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;195;-5004.989,1299.423;Float;False;Property;_DissolveTop;_DissolveTop;10;1;[HideInInspector];-1.2;-5;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;166;-4966.873,1843.412;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;228;-226.5541,247.7078;Float;True;Property;_AmbientOcclusion;Ambient Occlusion;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;42;-121.0102,476.3462;Float;False;41;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;380.12,-101.2171;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Amplify Shader/PBR_Dissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.51;True;True;0;True;Opaque;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;17;-1;-1;-1;0;0;0;False;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;239;2;283;0
WireConnection;245;0;239;0
WireConnection;245;1;244;0
WireConnection;237;0;245;0
WireConnection;287;0;283;0
WireConnection;286;0;287;0
WireConnection;246;0;237;0
WireConnection;246;1;244;0
WireConnection;70;0;286;0
WireConnection;70;1;246;0
WireConnection;35;0;70;1
WireConnection;31;0;33;0
WireConnection;31;1;36;0
WireConnection;221;0;31;0
WireConnection;264;0;221;0
WireConnection;34;0;264;0
WireConnection;34;1;225;0
WireConnection;34;2;226;0
WireConnection;132;0;34;0
WireConnection;267;0;132;0
WireConnection;66;0;267;0
WireConnection;22;1;66;0
WireConnection;47;0;267;0
WireConnection;47;1;22;0
WireConnection;53;0;69;0
WireConnection;28;0;47;0
WireConnection;289;0;190;0
WireConnection;52;0;67;4
WireConnection;63;0;56;0
WireConnection;63;1;60;0
WireConnection;41;0;132;0
WireConnection;189;0;231;0
WireConnection;189;1;43;0
WireConnection;189;2;289;0
WireConnection;197;0;278;0
WireConnection;197;3;195;0
WireConnection;197;4;193;0
WireConnection;72;0;71;0
WireConnection;72;1;1;0
WireConnection;58;0;189;0
WireConnection;58;1;63;0
WireConnection;58;2;57;0
WireConnection;139;0;31;0
WireConnection;139;1;209;0
WireConnection;139;2;138;0
WireConnection;255;0;271;0
WireConnection;209;0;201;0
WireConnection;51;0;67;1
WireConnection;201;0;199;2
WireConnection;201;2;197;0
WireConnection;186;0;114;0
WireConnection;199;0;198;0
WireConnection;176;0;186;0
WireConnection;278;0;33;0
WireConnection;114;0;171;0
WireConnection;114;1;166;2
WireConnection;233;0;33;0
WireConnection;280;0;41;0
WireConnection;271;0;22;0
WireConnection;271;1;280;0
WireConnection;171;0;233;0
WireConnection;7;0;55;0
WireConnection;7;1;6;0
WireConnection;0;0;72;0
WireConnection;0;1;2;0
WireConnection;0;2;58;0
WireConnection;0;3;68;0
WireConnection;0;4;7;0
WireConnection;0;5;228;0
WireConnection;0;10;42;0
ASEEND*/
//CHKSM=2E69943B402D9ECAB76EDC74E9895ACF515BF474