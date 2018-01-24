// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:False,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:1,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:6,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:1,qpre:4,rntp:5,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.1670588,fgcg:0.2791611,fgcb:0.284,fgca:1,fgde:0.05,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33810,y:33602,varname:node_2865,prsc:2|emission-8922-OUT;n:type:ShaderForge.SFN_Relay,id:8397,x:28816,y:33156,varname:node_8397,prsc:2|IN-5459-U;n:type:ShaderForge.SFN_Tex2dAsset,id:4430,x:31553,y:33217,ptovrint:False,ptlb:MainTex,ptin:_MainTex,cmnt:MainTex contains the color of the scene,varname:_MainTex,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7542,x:31769,y:33245,varname:node_1672,prsc:2,ntxv:0,isnm:False|TEX-4430-TEX;n:type:ShaderForge.SFN_TexCoord,id:5459,x:28488,y:33165,varname:node_5459,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_OneMinus,id:8648,x:28787,y:33012,varname:node_8648,prsc:2|IN-5459-U;n:type:ShaderForge.SFN_Code,id:8806,x:29102,y:33160,varname:node_8806,prsc:2,code:DQAKAGkAZgAoAGoAdQBkAGcAZQAgAD4APQAgADEAIAAmACYAIABqAHUAZABnAGUAIAA8ACAAMgApAHsADQAKAAkAcgBlAHQAdQByAG4AIABsAGUAZgB0ADsADQAKAH0AZQBsAHMAZQAgAGkAZgAoAGoAdQBkAGcAZQAgAD4APQAgADIAIAAmACYAIABqAHUAZABnAGUAIAA8ACAAMwApAHsADQAKAAkAcgBlAHQAdQByAG4AIAByAGkAZwBoAHQAOwANAAoAfQBlAGwAcwBlACAAaQBmACgAagB1AGQAZwBlACAAPgA9ACAAMwAgACYAJgAgAGoAdQBkAGcAZQAgADwAIAA0ACkAewANAAoACQByAGUAdAB1AHIAbgAgAHUAcAA7AA0ACgB9AGUAbABzAGUAIABpAGYAKABqAHUAZABnAGUAIAA+AD0AIAA0ACAAJgAmACAAagB1AGQAZwBlACAAPAAgADUAKQB7AA0ACgAJAHIAZQB0AHUAcgBuACAAZABvAHcAbgA7AA0ACgB9AGUAbABzAGUAewANAAoACQByAGUAdAB1AHIAbgAgADAAOwANAAoAfQANAAoA,output:8,fname:Custom_Orient,width:247,height:153,input:8,input:8,input:8,input:8,input:8,input_1_label:left,input_2_label:right,input_3_label:up,input_4_label:down,input_5_label:judge|A-8648-OUT,B-8397-OUT,C-8853-OUT,D-5462-OUT,E-5402-OUT;n:type:ShaderForge.SFN_Relay,id:8853,x:28816,y:33231,varname:node_8853,prsc:2|IN-5459-V;n:type:ShaderForge.SFN_OneMinus,id:5462,x:28787,y:33325,varname:node_5462,prsc:2|IN-5459-V;n:type:ShaderForge.SFN_Slider,id:5402,x:28655,y:33500,ptovrint:False,ptlb:Orient,ptin:_Orient,cmnt:Direction,varname:_Orient,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.140617,max:5;n:type:ShaderForge.SFN_Lerp,id:4781,x:32116,y:33430,cmnt:Attack,varname:node_4781,prsc:2|A-7542-RGB,B-9334-OUT,T-2553-OUT;n:type:ShaderForge.SFN_Color,id:8505,x:30080,y:32926,ptovrint:False,ptlb:EdgeColor,ptin:_EdgeColor,varname:_EdgeColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.766,c2:0.1424759,c3:0.1424759,c4:1;n:type:ShaderForge.SFN_Power,id:7801,x:29654,y:33159,varname:node_7801,prsc:2|VAL-8703-OUT,EXP-8686-OUT;n:type:ShaderForge.SFN_Vector1,id:8686,x:29654,y:33326,cmnt:Contrast,varname:node_8686,prsc:2,v1:4;n:type:ShaderForge.SFN_RemapRange,id:8703,x:29452,y:33159,cmnt:Scope,varname:node_8703,prsc:2,frmn:0,frmx:1,tomn:0,tomx:1.5|IN-8806-OUT;n:type:ShaderForge.SFN_Tex2d,id:551,x:30274,y:33350,ptovrint:False,ptlb:BlendTex,ptin:_BlendTex,cmnt:Blend Texture,varname:_BlendTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:8ddab103f7c763443af11c12ee83ce91,ntxv:2,isnm:False|UVIN-3741-OUT;n:type:ShaderForge.SFN_Clamp01,id:316,x:29862,y:33159,varname:node_316,prsc:2|IN-7801-OUT;n:type:ShaderForge.SFN_TexCoord,id:1347,x:29818,y:32225,varname:node_1347,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Slider,id:7522,x:29465,y:32348,ptovrint:False,ptlb:OffsetX,ptin:_OffsetX,varname:_OffsetX,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.75,max:5;n:type:ShaderForge.SFN_Append,id:5406,x:29818,y:32397,cmnt:Offset Stereo,varname:node_5406,prsc:2|A-7522-OUT,B-1704-OUT;n:type:ShaderForge.SFN_Code,id:6913,x:30082,y:32317,varname:node_6913,prsc:2,code:IwBpAGYAIABVAE4ASQBUAFkAXwBTAEkATgBHAEwARQBfAFAAQQBTAFMAXwBTAFQARQBSAEUATwANAAoALwAvACAASQBmACAAUwBpAG4AZwBsAGUALQBQAGEAcwBzACAAUwB0AGUAcgBlAG8AIABtAG8AZABlACAAaQBzACAAYQBjAHQAaQB2AGUALAAgAHQAcgBhAG4AcwBmAG8AcgBtACAAdABoAGUADQAKAC8ALwAgAGMAbwBvAHIAZABpAG4AYQB0AGUAcwAgAHQAbwAgAGcAZQB0ACAAdABoAGUAIABjAG8AcgByAGUAYwB0ACAAbwB1AHQAcAB1AHQAIABVAFYAIABmAG8AcgAgAHQAaABlACAAYwB1AHIAcgBlAG4AdAAgAGUAeQBlAC4ADQAKAGYAbABvAGEAdAA0ACAAcwBjAGEAbABlAE8AZgBmAHMAZQB0ACAAPQAgAHUAbgBpAHQAeQBfAFMAdABlAHIAZQBvAFMAYwBhAGwAZQBPAGYAZgBzAGUAdABbAHUAbgBpAHQAeQBfAFMAdABlAHIAZQBvAEUAeQBlAEkAbgBkAGUAeABdADsADQAKAHUAdgAgAD0AIAAoAHUAdgAgAC0AIABzAGMAYQBsAGUATwBmAGYAcwBlAHQALgB6AHcAKQAgAC8AIABzAGMAYQBsAGUATwBmAGYAcwBlAHQALgB4AHkAOwANAAoAIwBlAG4AZABpAGYADQAKAHIAZQB0AHUAcgBuACAAdQB2ACAAKgA9ACAAbwBmAGYAcwBlAHQAOwA=,output:1,fname:Func_Stereo,width:519,height:266,input:1,input:1,input_1_label:uv,input_2_label:offset|A-1347-UVOUT,B-5406-OUT;n:type:ShaderForge.SFN_Slider,id:1704,x:29465,y:32479,ptovrint:False,ptlb:OffsetY,ptin:_OffsetY,varname:_OffsetY,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.9,max:5;n:type:ShaderForge.SFN_Set,id:5461,x:30668,y:32316,cmnt:Single Pass Stereo UV,varname:StereoUV,prsc:2|IN-6913-OUT;n:type:ShaderForge.SFN_Get,id:3741,x:30067,y:33350,varname:node_3741,prsc:2|IN-5461-OUT;n:type:ShaderForge.SFN_Multiply,id:5775,x:30287,y:33011,varname:node_5775,prsc:2|A-8505-RGB,B-4908-OUT;n:type:ShaderForge.SFN_Set,id:4410,x:33320,y:33706,varname:Emission,prsc:2|IN-7633-OUT;n:type:ShaderForge.SFN_Get,id:8922,x:33601,y:33701,varname:node_8922,prsc:2|IN-4410-OUT;n:type:ShaderForge.SFN_Add,id:887,x:30485,y:33162,varname:node_887,prsc:2|A-5775-OUT,B-551-RGB;n:type:ShaderForge.SFN_ComponentMask,id:5313,x:31267,y:33664,varname:node_5313,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-8435-OUT;n:type:ShaderForge.SFN_Slider,id:3255,x:31528,y:33867,ptovrint:False,ptlb:Weight,ptin:_Weight,cmnt:Blend Weight,varname:_Weight,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_ConstantClamp,id:845,x:31666,y:33664,varname:node_845,prsc:2,min:0,max:1|IN-9241-OUT;n:type:ShaderForge.SFN_Multiply,id:2553,x:31899,y:33737,varname:node_2553,prsc:2|A-845-OUT,B-3255-OUT;n:type:ShaderForge.SFN_Relay,id:9334,x:31601,y:33446,varname:node_9334,prsc:2|IN-8435-OUT;n:type:ShaderForge.SFN_RemapRange,id:9241,x:31467,y:33664,cmnt:Intensity,varname:node_9241,prsc:2,frmn:0,frmx:1,tomn:0,tomx:0.6|IN-5313-OUT;n:type:ShaderForge.SFN_Set,id:2908,x:30857,y:34756,cmnt:Flow Liquid,varname:FlowLiquid,prsc:2|IN-1451-OUT;n:type:ShaderForge.SFN_Set,id:5819,x:30681,y:33162,cmnt:Encounter Attack,varname:Attacked,prsc:2|IN-887-OUT;n:type:ShaderForge.SFN_Tex2d,id:6185,x:29754,y:34619,varname:node_2114,prsc:2,tex:f9732a1b08d64a7438b7c5197ab185e8,ntxv:0,isnm:False|UVIN-3849-UVOUT,TEX-8799-TEX;n:type:ShaderForge.SFN_Tex2d,id:998,x:30387,y:34775,ptovrint:False,ptlb:FlowTex,ptin:_FlowTex,varname:_FlowTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:41421d4d3bc8f3c4eabcde13e9698dc0,ntxv:0,isnm:False|UVIN-7151-OUT;n:type:ShaderForge.SFN_Color,id:8076,x:30387,y:34605,ptovrint:False,ptlb:LiquidColor,ptin:_LiquidColor,varname:_LiquidColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:1451,x:30619,y:34756,varname:node_1451,prsc:2|A-8076-RGB,B-998-RGB,C-89-OUT;n:type:ShaderForge.SFN_Panner,id:3849,x:29541,y:34619,varname:node_3849,prsc:2,spu:0,spv:0.3|UVIN-2687-OUT;n:type:ShaderForge.SFN_Lerp,id:3397,x:30016,y:34775,varname:node_3397,prsc:2|A-6185-R,B-2687-OUT,T-7037-OUT;n:type:ShaderForge.SFN_Clamp01,id:7151,x:30207,y:34775,varname:node_7151,prsc:2|IN-3397-OUT;n:type:ShaderForge.SFN_Vector1,id:7037,x:29769,y:34905,varname:node_7037,prsc:2,v1:0.9;n:type:ShaderForge.SFN_Relay,id:2687,x:29412,y:34799,varname:node_2687,prsc:2|IN-8705-OUT;n:type:ShaderForge.SFN_TexCoord,id:8609,x:29013,y:34482,cmnt:Problem  StereoUV?,varname:node_8609,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Get,id:8435,x:31252,y:33447,varname:node_8435,prsc:2|IN-5819-OUT;n:type:ShaderForge.SFN_Lerp,id:4006,x:32454,y:33567,cmnt:Death,varname:node_4006,prsc:2|A-4781-OUT,B-3884-OUT,T-4796-OUT;n:type:ShaderForge.SFN_Get,id:3884,x:32116,y:33587,varname:node_3884,prsc:2|IN-2908-OUT;n:type:ShaderForge.SFN_Slider,id:4796,x:32072,y:33696,ptovrint:False,ptlb:FlowLiquidBlend,ptin:_FlowLiquidBlend,varname:_FlowLiquidBlend,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:8705,x:29222,y:34799,cmnt:Scale,varname:node_8705,prsc:2|A-370-OUT,B-1253-OUT;n:type:ShaderForge.SFN_Vector1,id:1253,x:29013,y:34898,varname:node_1253,prsc:2,v1:0.6;n:type:ShaderForge.SFN_Tex2d,id:7338,x:29601,y:35330,varname:_NoiseTex_copy,prsc:2,tex:f9732a1b08d64a7438b7c5197ab185e8,ntxv:0,isnm:False|UVIN-1599-UVOUT,TEX-8799-TEX;n:type:ShaderForge.SFN_Tex2d,id:4917,x:30234,y:35486,ptovrint:False,ptlb:BurnTex,ptin:_BurnTex,varname:_BurnTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c111203c8bc0e6f459f79993b2e82d1d,ntxv:0,isnm:False|UVIN-5580-OUT;n:type:ShaderForge.SFN_Color,id:5269,x:30714,y:35638,ptovrint:False,ptlb:FireOuter,ptin:_FireOuter,varname:_FireOuter,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Panner,id:1599,x:29422,y:35330,varname:node_1599,prsc:2,spu:0.2,spv:-0.5|UVIN-8040-OUT;n:type:ShaderForge.SFN_Lerp,id:7564,x:29868,y:35486,varname:node_7564,prsc:2|A-7338-R,B-8040-OUT,T-3171-OUT;n:type:ShaderForge.SFN_Clamp01,id:5580,x:30042,y:35486,varname:node_5580,prsc:2|IN-7564-OUT;n:type:ShaderForge.SFN_Vector1,id:3171,x:29633,y:35605,varname:node_3171,prsc:2,v1:0.8;n:type:ShaderForge.SFN_Relay,id:8040,x:29259,y:35510,varname:node_8040,prsc:2|IN-9230-OUT;n:type:ShaderForge.SFN_Color,id:8498,x:30714,y:35859,ptovrint:False,ptlb:FireInner,ptin:_FireInner,varname:_FireInner,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.7364098,c3:0.1691176,c4:1;n:type:ShaderForge.SFN_Lerp,id:2010,x:30977,y:35609,varname:node_2010,prsc:2|A-5269-RGB,B-8498-RGB,T-1529-OUT;n:type:ShaderForge.SFN_ChannelBlend,id:7657,x:31183,y:35501,varname:node_7657,prsc:2,chbt:0|M-1529-OUT,R-2010-OUT;n:type:ShaderForge.SFN_Relay,id:1529,x:30743,y:35502,varname:node_1529,prsc:2|IN-2596-OUT;n:type:ShaderForge.SFN_RemapRange,id:2596,x:30462,y:35502,varname:node_2596,prsc:2,frmn:0,frmx:1,tomn:0,tomx:2|IN-4917-R;n:type:ShaderForge.SFN_Tex2dAsset,id:8799,x:29294,y:35055,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:_Noise,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f9732a1b08d64a7438b7c5197ab185e8,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Set,id:7131,x:31377,y:35501,cmnt:Burn Down,varname:Burn,prsc:2|IN-7657-OUT;n:type:ShaderForge.SFN_Get,id:6510,x:32433,y:33725,varname:node_6510,prsc:2|IN-7131-OUT;n:type:ShaderForge.SFN_Lerp,id:4312,x:32718,y:33706,cmnt:Burn,varname:node_4312,prsc:2|A-4006-OUT,B-6510-OUT,T-2154-OUT;n:type:ShaderForge.SFN_Slider,id:6450,x:32276,y:34068,ptovrint:False,ptlb:BurnBlend,ptin:_BurnBlend,varname:_BurnBlend,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ComponentMask,id:8709,x:32433,y:33883,varname:node_8709,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-6510-OUT;n:type:ShaderForge.SFN_Multiply,id:2154,x:32633,y:33946,varname:node_2154,prsc:2|A-8709-OUT,B-6450-OUT;n:type:ShaderForge.SFN_Get,id:4871,x:28176,y:35532,varname:node_4871,prsc:2|IN-5461-OUT;n:type:ShaderForge.SFN_Get,id:370,x:28992,y:34677,varname:node_370,prsc:2|IN-5461-OUT;n:type:ShaderForge.SFN_ComponentMask,id:4624,x:28770,y:33820,varname:node_4624,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-5050-OUT;n:type:ShaderForge.SFN_Add,id:2610,x:29150,y:33822,varname:node_2610,prsc:2|A-4331-OUT,B-8967-OUT;n:type:ShaderForge.SFN_Slider,id:1852,x:28602,y:34016,ptovrint:False,ptlb:VerticalSweep,ptin:_VerticalSweep,cmnt:Vertical Sweep,varname:node_1852,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Get,id:5050,x:28589,y:33820,varname:node_5050,prsc:2|IN-5461-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:7603,x:29345,y:33822,varname:node_7603,prsc:2,min:0,max:1|IN-2610-OUT;n:type:ShaderForge.SFN_RemapRange,id:8967,x:28963,y:34015,varname:node_8967,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-1852-OUT;n:type:ShaderForge.SFN_OneMinus,id:4331,x:28963,y:33822,varname:node_4331,prsc:2|IN-4624-G;n:type:ShaderForge.SFN_Set,id:1055,x:29540,y:33822,varname:VerticalSweep,prsc:2|IN-7603-OUT;n:type:ShaderForge.SFN_Get,id:89,x:30387,y:34964,varname:node_89,prsc:2|IN-1055-OUT;n:type:ShaderForge.SFN_Add,id:4548,x:28710,y:35313,varname:node_4548,prsc:2|A-5401-OUT,B-2666-OUT;n:type:ShaderForge.SFN_Slider,id:5401,x:28302,y:35251,ptovrint:False,ptlb:OffsetU,ptin:_OffsetU,varname:node_5401,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_ComponentMask,id:2666,x:28459,y:35384,varname:node_2666,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-4871-OUT;n:type:ShaderForge.SFN_Append,id:9230,x:29040,y:35510,varname:node_9230,prsc:2|A-429-OUT,B-26-OUT;n:type:ShaderForge.SFN_ComponentMask,id:26,x:28748,y:35530,varname:node_26,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-4871-OUT;n:type:ShaderForge.SFN_Multiply,id:429,x:28998,y:35291,varname:node_429,prsc:2|A-2099-OUT,B-4548-OUT;n:type:ShaderForge.SFN_Slider,id:2099,x:28568,y:35137,ptovrint:False,ptlb:TileU,ptin:_TileU,varname:node_2099,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:5;n:type:ShaderForge.SFN_Color,id:6253,x:32937,y:33896,ptovrint:False,ptlb:OverlayColor,ptin:_OverlayColor,varname:node_6253,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:0;n:type:ShaderForge.SFN_Multiply,id:3940,x:32937,y:33706,cmnt: Overlap Color,varname:node_3940,prsc:2|A-4312-OUT,B-6253-RGB;n:type:ShaderForge.SFN_Add,id:7633,x:33152,y:33706,cmnt:Overlap Intensity,varname:node_7633,prsc:2|A-3940-OUT,B-1206-OUT;n:type:ShaderForge.SFN_Multiply,id:1206,x:33152,y:33896,varname:node_1206,prsc:2|A-6253-RGB,B-6253-A;n:type:ShaderForge.SFN_Smoothstep,id:4908,x:30088,y:33120,varname:node_4908,prsc:2|A-1399-OUT,B-4317-OUT,V-316-OUT;n:type:ShaderForge.SFN_Vector1,id:1399,x:29842,y:32992,varname:node_1399,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:4317,x:29862,y:33079,varname:node_4317,prsc:2,v1:1;proporder:4430-5402-8505-551-3255-7522-1704-998-8076-4796-1852-8799-4917-5401-2099-5269-8498-6450-6253;pass:END;sub:END;*/

Shader "Shader Forge/SF_PPE_EdgeOverlay" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Orient ("Orient", Range(0, 5)) = 1.140617
        _EdgeColor ("EdgeColor", Color) = (0.766,0.1424759,0.1424759,1)
        _BlendTex ("BlendTex", 2D) = "black" {}
        _Weight ("Weight", Range(0, 1)) = 1
        [HideInInspector]_OffsetX ("OffsetX", Range(0, 5)) = 0.75
        [HideInInspector]_OffsetY ("OffsetY", Range(0, 5)) = 0.9
        _FlowTex ("FlowTex", 2D) = "white" {}
        _LiquidColor ("LiquidColor", Color) = (0.5,0.5,0.5,1)
        _FlowLiquidBlend ("FlowLiquidBlend", Range(0, 1)) = 0
        _VerticalSweep ("VerticalSweep", Range(0, 1)) = 1
        _Noise ("Noise", 2D) = "white" {}
        _BurnTex ("BurnTex", 2D) = "white" {}
        _OffsetU ("OffsetU", Range(-1, 1)) = 0
        _TileU ("TileU", Range(0, 5)) = 1
        _FireOuter ("FireOuter", Color) = (1,0,0,1)
        _FireInner ("FireInner", Color) = (1,0.7364098,0.1691176,1)
        _BurnBlend ("BurnBlend", Range(0, 1)) = 0
        _OverlayColor ("OverlayColor", Color) = (1,1,1,0)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay+1"
            "RenderType"="Overlay"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZTest Always
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            fixed Custom_Orient( fixed left , fixed right , fixed up , fixed down , fixed judge ){
            
            if(judge >= 1 && judge < 2){
            	return left;
            }else if(judge >= 2 && judge < 3){
            	return right;
            }else if(judge >= 3 && judge < 4){
            	return up;
            }else if(judge >= 4 && judge < 5){
            	return down;
            }else{
            	return 0;
            }
            
            }
            
            uniform float _Orient;
            uniform float4 _EdgeColor;
            uniform sampler2D _BlendTex; uniform float4 _BlendTex_ST;
            uniform float _OffsetX;
            float2 Func_Stereo( float2 uv , float2 offset ){
            #if UNITY_SINGLE_PASS_STEREO
            // If Single-Pass Stereo mode is active, transform the
            // coordinates to get the correct output UV for the current eye.
            float4 scaleOffset = unity_StereoScaleOffset[unity_StereoEyeIndex];
            uv = (uv - scaleOffset.zw) / scaleOffset.xy;
            #endif
            return uv *= offset;
            }
            
            uniform float _OffsetY;
            uniform float _Weight;
            uniform sampler2D _FlowTex; uniform float4 _FlowTex_ST;
            uniform float4 _LiquidColor;
            uniform float _FlowLiquidBlend;
            uniform sampler2D _BurnTex; uniform float4 _BurnTex_ST;
            uniform float4 _FireOuter;
            uniform float4 _FireInner;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _BurnBlend;
            uniform float _VerticalSweep;
            uniform float _OffsetU;
            uniform float _TileU;
            uniform float4 _OverlayColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_1672 = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float2 StereoUV = Func_Stereo( i.uv1 , float2(_OffsetX,_OffsetY) ); // Single Pass Stereo UV
                float2 node_3741 = StereoUV;
                float4 _BlendTex_var = tex2D(_BlendTex,TRANSFORM_TEX(node_3741, _BlendTex)); // Blend Texture
                float3 Attacked = ((_EdgeColor.rgb*smoothstep( 0.0, 1.0, saturate(pow((Custom_Orient( (1.0 - i.uv0.r) , i.uv0.r , i.uv0.g , (1.0 - i.uv0.g) , _Orient )*1.5+0.0),4.0)) ))+_BlendTex_var.rgb); // Encounter Attack
                float3 node_8435 = Attacked;
                float4 node_2923 = _Time;
                float2 node_2687 = (StereoUV*0.6);
                float2 node_3849 = (node_2687+node_2923.g*float2(0,0.3));
                float4 node_2114 = tex2D(_Noise,TRANSFORM_TEX(node_3849, _Noise));
                float2 node_7151 = saturate(lerp(float2(node_2114.r,node_2114.r),node_2687,0.9));
                float4 _FlowTex_var = tex2D(_FlowTex,TRANSFORM_TEX(node_7151, _FlowTex));
                float VerticalSweep = clamp(((1.0 - StereoUV.rg.g)+(_VerticalSweep*2.0+-1.0)),0,1);
                float3 FlowLiquid = (_LiquidColor.rgb*_FlowTex_var.rgb*VerticalSweep); // Flow Liquid
                float2 node_4871 = StereoUV;
                float2 node_8040 = float2((_TileU*(_OffsetU+node_4871.r)),node_4871.g);
                float2 node_1599 = (node_8040+node_2923.g*float2(0.2,-0.5));
                float4 _NoiseTex_copy = tex2D(_Noise,TRANSFORM_TEX(node_1599, _Noise));
                float2 node_5580 = saturate(lerp(float2(_NoiseTex_copy.r,_NoiseTex_copy.r),node_8040,0.8));
                float4 _BurnTex_var = tex2D(_BurnTex,TRANSFORM_TEX(node_5580, _BurnTex));
                float node_1529 = (_BurnTex_var.r*2.0+0.0);
                float3 Burn = (node_1529.r*lerp(_FireOuter.rgb,_FireInner.rgb,node_1529)); // Burn Down
                float3 node_6510 = Burn;
                float3 Emission = ((lerp(lerp(lerp(node_1672.rgb,node_8435,(clamp((node_8435.r*0.6+0.0),0,1)*_Weight)),FlowLiquid,_FlowLiquidBlend),node_6510,(node_6510.r*_BurnBlend))*_OverlayColor.rgb)+(_OverlayColor.rgb*_OverlayColor.a));
                float3 emissive = Emission;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
