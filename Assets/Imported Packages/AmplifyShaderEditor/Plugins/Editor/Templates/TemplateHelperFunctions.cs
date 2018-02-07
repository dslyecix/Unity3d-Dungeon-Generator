// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace AmplifyShaderEditor
{
	public enum TemplateSemantics
	{
		NONE,
		POSITION,
		SV_POSITION,
		COLOR,
		COLOR0,
		COLOR1,
		TEXCOORD0,
		TEXCOORD1,
		TEXCOORD2,
		TEXCOORD3,
		TEXCOORD4,
		TEXCOORD5,
		TEXCOORD6,
		TEXCOORD7,
		TEXCOORD8,
		TEXCOORD9,
		NORMAL,
		TANGENT,
		VFACE
	}

	public enum TemplateInfoOnSematics
	{
		NONE,
		POSITION,
		SCREEN_POSITION,
		COLOR,
		TEXTURE_COORDINATES0,
		TEXTURE_COORDINATES1,
		TEXTURE_COORDINATES2,
		TEXTURE_COORDINATES3,
		NORMAL,
		TANGENT,
		OTHER
	}

	public enum TemplateShaderPropertiesIdx
	{
		Name = 1,
		InspectorName,
		Type
	}

	public enum TemplateShaderGlobalsIdx
	{
		Type = 1,
		Name = 2
	}

	[Serializable]
	public class TemplateStencilData
	{
		public bool ValidStencilData = false;

		public int Reference;
		public int ReadMask;
		public int WriteMask;

		public string ComparisonFront;
		public string PassFront;
		public string FailFront;
		public string ZFailFront;

		public string ComparisonBack;
		public string PassBack;
		public string FailBack;
		public string ZFailBack;
	}
	
	[Serializable]
	public class TemplateBlendData
	{
		public bool ValidBlendData = false;

		public bool ValidBlendMode = false;
		public string BlendModeId;
		public bool SeparateBlendFactors = false;
		public AvailableBlendFactor SourceFactorRGB = AvailableBlendFactor.One;
		public AvailableBlendFactor DestFactorRGB = AvailableBlendFactor.Zero;

		public AvailableBlendFactor SourceFactorAlpha = AvailableBlendFactor.One;
		public AvailableBlendFactor DestFactorAlpha = AvailableBlendFactor.Zero;

		public bool ValidBlendOp = false;
		public string BlendOpId;
		public bool SeparateBlendOps = false;
		public AvailableBlendOps BlendOpRGB = AvailableBlendOps.OFF;
		public AvailableBlendOps BlendOpAlpha = AvailableBlendOps.OFF;
	}

	[Serializable]
	public class TemplateCullModeData
	{
		public bool ValidCullData = false;
		public string CullModeId;
		public CullMode CullModeData = CullMode.Front;
	}

	[Serializable]
	public class TemplateColorMaskData
	{
		public bool ValidColorMaskData = false;
		public string ColorMaskId;
		public bool[] ColorMaskData = { true, true, true, true };
	}

	public class TemplateHelperFunctions
	{
		public static string[] VectorSwizzle = { "x", "y", "z", "w" };
		public static string[] ColorSwizzle = { "r", "g", "b", "a" };

		public static readonly Dictionary<string, WirePortDataType> PropertyToWireType = new Dictionary<string, WirePortDataType>
		{
			{"Float",WirePortDataType.FLOAT},
			{"Range",WirePortDataType.FLOAT},
			{"Int",WirePortDataType.INT},
			{"Color",WirePortDataType.COLOR},
			{"Vector",WirePortDataType.FLOAT4},
			{"2D",WirePortDataType.SAMPLER2D},
			{"3D",WirePortDataType.SAMPLER3D},
			{"Cube",WirePortDataType.SAMPLERCUBE}
		};

		public static readonly Dictionary<WirePortDataType, int> DataTypeChannelUsage = new Dictionary<WirePortDataType, int>
		{
			{WirePortDataType.OBJECT,0 },
			{WirePortDataType.FLOAT,1 },
			{WirePortDataType.FLOAT2,2 },
			{WirePortDataType.FLOAT3,3 },
			{WirePortDataType.FLOAT4,4 },
			{WirePortDataType.FLOAT3x3,0 },
			{WirePortDataType.FLOAT4x4,0 },
			{WirePortDataType.COLOR,4 },
			{WirePortDataType.INT,1 },
			{WirePortDataType.SAMPLER1D,0 },
			{WirePortDataType.SAMPLER2D,0 },
			{WirePortDataType.SAMPLER3D,0 },
			{WirePortDataType.SAMPLERCUBE,0 }
		};

		public static readonly Dictionary<TemplateSemantics, string> SemanticsDefaultName = new Dictionary<TemplateSemantics, string>
		{
			{TemplateSemantics.COLOR        ,"ase_color"},
			{TemplateSemantics.NORMAL       ,"ase_normal"},
			{TemplateSemantics.POSITION     ,"ase_position"},
			{TemplateSemantics.SV_POSITION  ,"ase_sv_position"},
			{TemplateSemantics.TANGENT      ,"ase_tangent"},
			{TemplateSemantics.VFACE        ,"ase_vface"},
		};

		public static readonly Dictionary<int, TemplateInfoOnSematics> IntToInfo = new Dictionary<int, TemplateInfoOnSematics>
		{
			{0,TemplateInfoOnSematics.TEXTURE_COORDINATES0 },
			{1,TemplateInfoOnSematics.TEXTURE_COORDINATES1 },
			{2,TemplateInfoOnSematics.TEXTURE_COORDINATES2 },
			{3,TemplateInfoOnSematics.TEXTURE_COORDINATES3 },
		};

		public static readonly Dictionary<string, TemplateInfoOnSematics> ShortcutToInfo = new Dictionary<string, TemplateInfoOnSematics>
		{
			{"p"    ,TemplateInfoOnSematics.POSITION },
			{"sp"   ,TemplateInfoOnSematics.SCREEN_POSITION },
			{"c"    ,TemplateInfoOnSematics.COLOR },
			{"uv0"  ,TemplateInfoOnSematics.TEXTURE_COORDINATES0 },
			{"uv1"  ,TemplateInfoOnSematics.TEXTURE_COORDINATES1 },
			{"uv2"  ,TemplateInfoOnSematics.TEXTURE_COORDINATES2 },
			{"uv3"  ,TemplateInfoOnSematics.TEXTURE_COORDINATES3 },
			{"n"    ,TemplateInfoOnSematics.NORMAL },
			{"t"    ,TemplateInfoOnSematics.TANGENT }
		};

		public static readonly Dictionary<int, TemplateInfoOnSematics> IntToUVChannelInfo = new Dictionary<int, TemplateInfoOnSematics>
		{
			{0,TemplateInfoOnSematics.TEXTURE_COORDINATES0 },
			{1,TemplateInfoOnSematics.TEXTURE_COORDINATES1 },
			{2,TemplateInfoOnSematics.TEXTURE_COORDINATES2 },
			{3,TemplateInfoOnSematics.TEXTURE_COORDINATES3 }
		};

		public static readonly Dictionary<int, TemplateSemantics> IntToSemantic = new Dictionary<int, TemplateSemantics>
		{
			{ 0,TemplateSemantics.TEXCOORD0 },
			{ 1,TemplateSemantics.TEXCOORD1 },
			{ 2,TemplateSemantics.TEXCOORD2 },
			{ 3,TemplateSemantics.TEXCOORD3 },
			{ 4,TemplateSemantics.TEXCOORD4 },
			{ 5,TemplateSemantics.TEXCOORD5 },
			{ 6,TemplateSemantics.TEXCOORD6 },
			{ 7,TemplateSemantics.TEXCOORD7 },
			{ 8,TemplateSemantics.TEXCOORD8 },
			{ 9,TemplateSemantics.TEXCOORD9 },
		};

		public static readonly Dictionary<TemplateSemantics, int> SemanticToInt = new Dictionary<TemplateSemantics, int>
		{
			{ TemplateSemantics.TEXCOORD0,0 },
			{ TemplateSemantics.TEXCOORD1,1 },
			{ TemplateSemantics.TEXCOORD2,2 },
			{ TemplateSemantics.TEXCOORD3,3 },
			{ TemplateSemantics.TEXCOORD4,4 },
			{ TemplateSemantics.TEXCOORD5,5 },
			{ TemplateSemantics.TEXCOORD6,6 },
			{ TemplateSemantics.TEXCOORD7,7 },
			{ TemplateSemantics.TEXCOORD8,8 },
			{ TemplateSemantics.TEXCOORD9,9 },
		};

		public static readonly Dictionary<string, TemplateSemantics> ShortcutToSemantic = new Dictionary<string, TemplateSemantics>
		{
			{ "p"   ,TemplateSemantics.POSITION },
			{ "sp"  ,TemplateSemantics.SV_POSITION },
			{ "c"   ,TemplateSemantics.COLOR },
			{ "n"   ,TemplateSemantics.NORMAL },
			{ "t"   ,TemplateSemantics.TANGENT },
			{ "tc0" ,TemplateSemantics.TEXCOORD0 },
			{ "tc1" ,TemplateSemantics.TEXCOORD1 },
			{ "tc2" ,TemplateSemantics.TEXCOORD2 },
			{ "tc3" ,TemplateSemantics.TEXCOORD3 },
			{ "tc4" ,TemplateSemantics.TEXCOORD4 },
			{ "tc5" ,TemplateSemantics.TEXCOORD5 },
			{ "tc6" ,TemplateSemantics.TEXCOORD6 },
			{ "tc7" ,TemplateSemantics.TEXCOORD7 },
			{ "tc8" ,TemplateSemantics.TEXCOORD8 },
			{ "tc9" ,TemplateSemantics.TEXCOORD9 },
		};

		public static readonly Dictionary<string, WirePortDataType> CgToWirePortType = new Dictionary<string, WirePortDataType>()
		{
			{"float"            ,WirePortDataType.FLOAT},
			{"float2"           ,WirePortDataType.FLOAT2},
			{"float3"           ,WirePortDataType.FLOAT3},
			{"float4"           ,WirePortDataType.FLOAT4},
			{"float3x3"         ,WirePortDataType.FLOAT3x3},
			{"float4x4"         ,WirePortDataType.FLOAT4x4},
			{"half"             ,WirePortDataType.FLOAT},
			{"half2"            ,WirePortDataType.FLOAT2},
			{"half3"            ,WirePortDataType.FLOAT3},
			{"half4"            ,WirePortDataType.FLOAT4},
			{"half3x3"          ,WirePortDataType.FLOAT3x3},
			{"half4x4"          ,WirePortDataType.FLOAT4x4},
			{"fixed"            ,WirePortDataType.FLOAT},
			{"fixed2"           ,WirePortDataType.FLOAT2},
			{"fixed3"           ,WirePortDataType.FLOAT3},
			{"fixed4"           ,WirePortDataType.FLOAT4},
			{"fixed3x3"         ,WirePortDataType.FLOAT3x3},
			{"fixed4x4"         ,WirePortDataType.FLOAT4x4},
			{"int"              ,WirePortDataType.INT},
			{"sampler1D"        ,WirePortDataType.SAMPLER1D},
			{"sampler2D"        ,WirePortDataType.SAMPLER2D},
			{"sampler2D_float"  ,WirePortDataType.SAMPLER2D},
			{"sampler3D"        ,WirePortDataType.SAMPLER3D},
			{"samplerCUBE"      ,WirePortDataType.SAMPLERCUBE}
		};

		//public static readonly string VertexDataPattern = @"(\w+)[ \t](\w+)[ \t]:[ \t]([A-Z0-9_]+);";
		public static readonly string VertexDataPattern = @"(\w+)\s*(\w+)\s*:\s*([A-Z0-9_]+);";
		public static readonly string InterpRangePattern = @"ase_interp\((\d\.{0,1}\w{0,4}),(\d)\)";
		//public static readonly string PropertiesPattern = @"(\w*)\s*\(\s*\""([\w ] *)\""\s*\,\s*(\w*)\s*.*\)";
		public static readonly string PropertiesPatternB = "(\\w*)\\s*\\(\\s*\"([\\w ]*)\"\\s*\\,\\s*(\\w*)\\s*.*\\)";

		public static readonly string CullModePattern = @"\s*Cull\s+(\w+)";
		public static readonly string ColorMaskPattern = @"\s*ColorMask\s+(\w+)";
		public static readonly string BlendModePattern = @"\s*Blend\s+(\w+)\s+(\w+)(?:[\s,]+(\w+)\s+(\w+)|)";
		public static readonly string BlendOpPattern = @"\s*BlendOp\s+(\w+)[\s,]*(?:(\w+)|)";

		//public static readonly string StencilOpGlobalPattern = @"Stencil\s{([\w\W\s]*)}\/\*ase_stencil\*\/";
		public static readonly string StencilOpGlobalPattern = @"Stencil\s*{([\w\W\s]*)}";
		public static readonly string StencilOpLinePattern = @"(\w+)\s*(\w+)";

		public static readonly string ShaderGlobalsOverallPattern = @"[\}\#][\w\s\;\/\*]*\/\*ase_globals\*\/";
		public static readonly string ShaderGlobalsMultilinePattern = @"^\s*(?:uniform\s*)*(\w*)\s*(\w*);$";

		public static readonly string TexSemantic = "float4 {0} : TEXCOORD{1};";
		public static readonly string TexFullSemantic = "float4 {0} : {1};";
		public static readonly string InterpFullSemantic = "{0} {1} : {2};";
		public static readonly string BaseInterpolatorName = "ase_texcoord";

		public static readonly string InterpolatorDecl = Constants.VertexShaderOutputStr + ".{0} = " + Constants.VertexShaderInputStr + ".{0};";
		public static readonly string TemplateVariableDecl = "{0} = {1};";
		public static readonly string TemplateVarFormat = "{0}.{1}";

		static public string GenerateTextureSemantic( ref MasterNodeDataCollector dataCollector, int uv )
		{
			string texCoordName = BaseInterpolatorName;
			if( uv > 0 )
			{
				texCoordName += uv.ToString();
			}

			string texCoordData = string.Format( TexSemantic, texCoordName, uv );
			dataCollector.AddToVertexInput( texCoordData );
			dataCollector.AddToInterpolators( texCoordData );
			dataCollector.AddToVertexInterpolatorsDecl( string.Format( InterpolatorDecl, texCoordName ) );
			return texCoordName;
		}

		public static void CreateShaderPropertiesList( string propertyData, ref List<TemplateShaderPropertyData> propertiesList, ref Dictionary<string, TemplateShaderPropertyData> duplicatesHelper )
		{
			int nameIdx = (int)TemplateShaderPropertiesIdx.Name;
			int typeIdx = (int)TemplateShaderPropertiesIdx.Type;
			foreach( Match match in Regex.Matches( propertyData, PropertiesPatternB ) )
			{
				if( match.Groups.Count > 1 )
				{
					if( !duplicatesHelper.ContainsKey( match.Groups[ nameIdx ].Value ) && PropertyToWireType.ContainsKey( match.Groups[ typeIdx ].Value ) )
					{
						TemplateShaderPropertyData newData = new TemplateShaderPropertyData( match.Groups[ (int)TemplateShaderPropertiesIdx.InspectorName ].Value,
																								match.Groups[ nameIdx ].Value,
																								PropertyToWireType[ match.Groups[ typeIdx ].Value ],
																								PropertyType.Property );
						propertiesList.Add( newData );
						duplicatesHelper.Add( newData.PropertyName, newData );
					}
				}
			}
		}

		public static void CreateShaderGlobalsList( string propertyData, ref List<TemplateShaderPropertyData> propertiesList, ref Dictionary<string, TemplateShaderPropertyData> duplicatesHelper )
		{
			int typeIdx = (int)TemplateShaderGlobalsIdx.Type;
			int nameIdx = (int)TemplateShaderGlobalsIdx.Name;
			foreach( Match globalMatch in Regex.Matches( propertyData, ShaderGlobalsOverallPattern ) )
			{
				if( globalMatch.Groups.Count > 0 )
				{
					foreach( Match lineMatch in Regex.Matches( globalMatch.Groups[ 0 ].Value, ShaderGlobalsMultilinePattern, RegexOptions.Multiline ) )
					{
						if( lineMatch.Groups.Count > 1 )
						{
							if( !duplicatesHelper.ContainsKey( lineMatch.Groups[ nameIdx ].Value ) && CgToWirePortType.ContainsKey( lineMatch.Groups[ typeIdx ].Value ) )
							{
								TemplateShaderPropertyData newData = new TemplateShaderPropertyData( string.Empty, lineMatch.Groups[ nameIdx ].Value,
																										CgToWirePortType[ lineMatch.Groups[ typeIdx ].Value ],
																										PropertyType.Global );
								duplicatesHelper.Add( newData.PropertyName, newData );
								propertiesList.Add( newData );
							}
						}
					}
				}
			}
		}

		public static void CreateStencilOps( string stencilData, ref TemplateStencilData stencilDataObj )
		{
			MatchCollection overallGlobalMatch = Regex.Matches( stencilData, StencilOpGlobalPattern );
			if( overallGlobalMatch.Count == 1 && overallGlobalMatch[ 0 ].Groups.Count == 2 )
			{
				string value = overallGlobalMatch[ 0 ].Groups[ 1 ].Value;
				foreach( Match match in Regex.Matches( value, StencilOpLinePattern ) )
				{
					if( match.Groups.Count == 3 )
					{
						switch( match.Groups[ 1 ].Value )
						{
							case "Ref":
							{
								try
								{
									stencilDataObj.Reference = Convert.ToInt32( match.Groups[ 2 ].Value );
								}
								catch( Exception e )
								{
									Debug.LogException( e );
								}
							}
							break;
							case "ReadMask":
							{
								try
								{
								stencilDataObj.ReadMask =  Convert.ToInt32( match.Groups[ 2 ].Value );
								}
								catch( Exception e )
								{
									Debug.LogException( e );
								}
							}
							break;
							case "WriteMask":
							{
								try
								{
								stencilDataObj.WriteMask = Convert.ToInt32( match.Groups[ 2 ].Value );
								}
								catch( Exception e )
								{
									Debug.LogException( e );
								}
							}
							break;
							case "CompFront":
							case "Comp":
							{
								stencilDataObj.ComparisonFront = match.Groups[ 2 ].Value;
							}
							break;
							case "PassFront":
							case "Pass":
							{
								stencilDataObj.PassFront = match.Groups[ 2 ].Value;
							}
							break;
							case "FailFront":
							case "Fail":
							{
								stencilDataObj.FailFront = match.Groups[ 2 ].Value;
							}
							break;
							case "ZFail":
							case "ZFailFront":
							{
								stencilDataObj.ZFailFront = match.Groups[ 2 ].Value;
							}
							break;
							case "CompBack":
							{
								stencilDataObj.ComparisonBack = match.Groups[ 2 ].Value;
							}
							break;
							case "PassBack":
							{
								stencilDataObj.PassBack = match.Groups[ 2 ].Value;
							}
							break;
							case "FailBack":
							{
								stencilDataObj.ZFailBack = match.Groups[ 2 ].Value;
							}
							break;
							case "ZFailBack":
							{
								stencilDataObj.ZFailBack = match.Groups[ 2 ].Value;
							}
							break;
						}
					}
				}
			}
		}

		public static void CreateColorMask( string colorMaskData, ref TemplateColorMaskData colorMaskObj )
		{
			colorMaskObj.ValidColorMaskData = true;
			foreach( Match match in Regex.Matches( colorMaskData, ColorMaskPattern ) )
			{
				if( match.Groups.Count == 2 )
				{
					for( int i = 0; i < 4; i++ )
					{
						colorMaskObj.ColorMaskData[ i ] = false;
					}

					try
					{
						bool breakCycle = false;
						for( int i = 0; i < match.Groups[ 1 ].Value.Length; i++ )
						{
							if( breakCycle )
								break;
							
							switch( Char.ToLower( match.Groups[ 1 ].Value[ i ] ) )
							{
								case'0':
								{
									breakCycle = true;
									for( int j = 0; j < 4; j++ )
									{
										colorMaskObj.ColorMaskData[ j ] = false;
									}
								}break;
								case 'r': colorMaskObj.ColorMaskData[ 0 ] = true;break;
								case 'g': colorMaskObj.ColorMaskData[ 1 ] = true;break;
								case 'b': colorMaskObj.ColorMaskData[ 2 ] = true;break;
								case 'a': colorMaskObj.ColorMaskData[ 3 ] = true;break;
							}
						}
					}
					catch( Exception e )
					{
						Debug.LogException( e );
					}
				}
			}
		}

		public static void CreateCullMode( string cullModeData, ref TemplateCullModeData cullDataObj )
		{
			cullDataObj.ValidCullData = true;
			foreach( Match match in Regex.Matches( cullModeData, CullModePattern ) )
			{
				if( match.Groups.Count == 2 )
				{
					try
					{
						cullDataObj.CullModeData = (CullMode)Enum.Parse( typeof( CullMode ), match.Groups[1].Value );
					}
					catch( Exception e )
					{
						Debug.LogException( e );
					}
				}
			}
		}

		public static void CreateBlendMode( string blendModeData, ref TemplateBlendData blendDataObj )
		{
			blendDataObj.ValidBlendMode = true;
			// TODO: OPTIMIZE REGEX EXPRESSIONS TO NOT CATCH EMPTY GROUPS 
			foreach( Match match in Regex.Matches( blendModeData, BlendModePattern ) )
			{
				if( match.Groups.Count == 3 )
				{
					try
					{
						AvailableBlendFactor sourceAll = (AvailableBlendFactor)Enum.Parse( typeof( AvailableBlendFactor ), match.Groups[ 1 ].Value );
						AvailableBlendFactor destAll = (AvailableBlendFactor)Enum.Parse( typeof( AvailableBlendFactor ), match.Groups[ 2 ].Value );
						blendDataObj.SeparateBlendFactors = false;
						blendDataObj.SourceFactorRGB = sourceAll;
						blendDataObj.DestFactorRGB = destAll;
					}
					catch( Exception e )
					{
						Debug.LogException( e );
					}
					break;
				}
				else if( match.Groups.Count == 5 )
				{
					try
					{
						AvailableBlendFactor sourceRGB = (AvailableBlendFactor)Enum.Parse( typeof( AvailableBlendFactor ), match.Groups[ 1 ].Value );
						blendDataObj.SourceFactorRGB = sourceRGB;
						AvailableBlendFactor destRGB = (AvailableBlendFactor)Enum.Parse( typeof( AvailableBlendFactor ), match.Groups[ 2 ].Value );
						blendDataObj.DestFactorRGB = destRGB;

						if( match.Groups[ 3 ].Success && match.Groups[ 4 ].Success )
						{
							AvailableBlendFactor sourceA = (AvailableBlendFactor)Enum.Parse( typeof( AvailableBlendFactor ), match.Groups[ 3 ].Value );
							blendDataObj.SourceFactorAlpha = sourceA;
							AvailableBlendFactor destA = (AvailableBlendFactor)Enum.Parse( typeof( AvailableBlendFactor ), match.Groups[ 4 ].Value );
							blendDataObj.DestFactorAlpha = destA;
							blendDataObj.SeparateBlendFactors = true;
						}
						else
						{
							blendDataObj.SeparateBlendFactors = false;
						}
					}
					catch( Exception e )
					{
						Debug.LogException( e );
					}
					break;
				}
			}
		}

		public static void CreateBlendOp( string blendOpData, ref TemplateBlendData blendDataObj )
		{
			blendDataObj.ValidBlendOp = true;
			// TODO: OPTIMIZE REGEX EXPRESSIONS TO NOT CATCH EMPTY GROUPS 
			foreach( Match match in Regex.Matches( blendOpData, BlendOpPattern,RegexOptions.None) )
			{
				if( match.Groups.Count == 2 )
				{
					try
					{
						AvailableBlendOps blendOpsAll = (AvailableBlendOps)Enum.Parse( typeof( AvailableBlendOps ), match.Groups[ 1 ].Value );
						blendDataObj.SeparateBlendOps = false;
						blendDataObj.BlendOpRGB = blendOpsAll;
					}
					catch( Exception e )
					{
						Debug.LogException( e );
					}
					break;
				}
				else if( match.Groups.Count == 3 )
				{
					try
					{
						AvailableBlendOps blendOpsRGB = (AvailableBlendOps)Enum.Parse( typeof( AvailableBlendOps ), match.Groups[ 1 ].Value );
						blendDataObj.BlendOpRGB = blendOpsRGB;
						if( match.Groups[ 2 ].Success )
						{
							AvailableBlendOps blendOpsA = (AvailableBlendOps)Enum.Parse( typeof( AvailableBlendOps ), match.Groups[ 2 ].Value );
							blendDataObj.BlendOpAlpha = blendOpsA;
							blendDataObj.SeparateBlendOps = true;
						}
						else
						{
							blendDataObj.SeparateBlendOps = false;
						}
						
					}
					catch( Exception e )
					{
						Debug.LogException( e );
					}
					break;
				}
			}
		}

		public static List<TemplateVertexData> CreateVertexDataList( string vertexData, string parametersBody )
		{
			List<TemplateVertexData> vertexDataList = null;
			Dictionary<TemplateSemantics, TemplateVertexData> vertexDataDict = null;

			foreach( Match match in Regex.Matches( vertexData, VertexDataPattern ) )
			{
				if( match.Groups.Count > 1 )
				{
					if( vertexDataList == null )
					{
						vertexDataList = new List<TemplateVertexData>();
						vertexDataDict = new Dictionary<TemplateSemantics, TemplateVertexData>();
					}

					WirePortDataType dataType = CgToWirePortType[ match.Groups[ 1 ].Value ];
					string varName = match.Groups[ 2 ].Value;
					TemplateSemantics semantics = (TemplateSemantics)Enum.Parse( typeof( TemplateSemantics ), match.Groups[ 3 ].Value );
					TemplateVertexData templateVertexData = new TemplateVertexData( semantics, dataType, varName );
					vertexDataList.Add( templateVertexData );
					vertexDataDict.Add( semantics, templateVertexData );
				}
			}

			string[] paramsArray = parametersBody.Split( IOUtils.FIELD_SEPARATOR );
			if( paramsArray.Length > 0 )
			{
				for( int i = 0; i < paramsArray.Length; i++ )
				{
					string[] paramDataArr = paramsArray[ i ].Split( IOUtils.VALUE_SEPARATOR );
					if( paramDataArr.Length == 2 )
					{
						string[] swizzleInfoArr = paramDataArr[ 1 ].Split( IOUtils.FLOAT_SEPARATOR );
						TemplateSemantics semantic = ShortcutToSemantic[ swizzleInfoArr[ 0 ] ];
						if( vertexDataDict.ContainsKey( semantic ) )
						{
							TemplateVertexData templateVertexData = vertexDataDict[ semantic ];
							if( templateVertexData != null )
							{
								if( swizzleInfoArr.Length > 1 )
								{
									templateVertexData.DataSwizzle = "." + swizzleInfoArr[ 1 ];
								}
								templateVertexData.DataInfo = ShortcutToInfo[ paramDataArr[ 0 ] ];
								templateVertexData.Available = true;
							}
						}
					}
				}
			}

			vertexDataDict.Clear();
			vertexDataDict = null;
			return vertexDataList;
		}

		public static TemplateInterpData CreateInterpDataList( string interpData, string fullLine )
		{
			TemplateInterpData interpDataObj = null;
			List<TemplateVertexData> interpDataList = null;
			Dictionary<TemplateSemantics, TemplateVertexData> interpDataDict = null;
			Match rangeMatch = Regex.Match( fullLine, InterpRangePattern );
			if( rangeMatch.Groups.Count > 0 )
			{
				interpDataObj = new TemplateInterpData();
				// Get range of available interpolators
				int minVal = 0;
				int maxVal = 0;
				try
				{
					string[] minValArgs = rangeMatch.Groups[ 1 ].Value.Split( IOUtils.FLOAT_SEPARATOR );
					minVal = Convert.ToInt32( minValArgs[ 0 ] );
					maxVal = Convert.ToInt32( rangeMatch.Groups[ 2 ].Value );
					if( minVal > maxVal )
					{
						int aux = minVal;
						minVal = maxVal;
						maxVal = aux;
					}
					for( int i = minVal; i <= maxVal; i++ )
					{
						interpDataObj.AvailableInterpolators.Add( new TemplateInterpElement( IntToSemantic[ i ] ) );
					}
					if( minValArgs.Length > 1 )
					{
						interpDataObj.AvailableInterpolators[ 0 ].SetAvailableChannelsFromString( minValArgs[ 1 ] );
					}
				}
				catch( Exception e )
				{
					Debug.LogException( e );
				}

				//Get Current interpolators
				int parametersBeginIdx = fullLine.IndexOf( ":" ) + 1;
				int parametersEnd = fullLine.IndexOf( TemplatesManager.TemplateEndOfLine );
				string parametersBody = fullLine.Substring( parametersBeginIdx, parametersEnd - parametersBeginIdx );

				foreach( Match match in Regex.Matches( interpData, VertexDataPattern ) )
				{
					if( match.Groups.Count > 1 )
					{
						if( interpDataList == null )
						{
							interpDataList = new List<TemplateVertexData>();
							interpDataDict = new Dictionary<TemplateSemantics, TemplateVertexData>();
						}

						WirePortDataType dataType = CgToWirePortType[ match.Groups[ 1 ].Value ];
						string varName = match.Groups[ 2 ].Value;
						TemplateSemantics semantics = (TemplateSemantics)Enum.Parse( typeof( TemplateSemantics ), match.Groups[ 3 ].Value );
						TemplateVertexData templateVertexData = new TemplateVertexData( semantics, dataType, varName );
						//interpDataList.Add( templateVertexData );
						interpDataDict.Add( semantics, templateVertexData );
						//Check if they are also on the free channels list and update their names
						interpDataObj.ReplaceNameOnInterpolator( semantics, varName );
					}
				}

				// Get info for available interpolators
				string[] paramsArray = parametersBody.Split( IOUtils.FIELD_SEPARATOR );
				if( paramsArray.Length > 0 )
				{
					for( int i = 0; i < paramsArray.Length; i++ )
					{
						string[] paramDataArr = paramsArray[ i ].Split( IOUtils.VALUE_SEPARATOR );
						if( paramDataArr.Length == 2 )
						{
							string[] swizzleInfoArr = paramDataArr[ 1 ].Split( IOUtils.FLOAT_SEPARATOR );
							TemplateSemantics semantic = ShortcutToSemantic[ swizzleInfoArr[ 0 ] ];
							if( interpDataDict.ContainsKey( semantic ) )
							{
								if( interpDataDict[ semantic ] != null )
								{
									TemplateVertexData templateVertexData = new TemplateVertexData( interpDataDict[ semantic ] );
									if( swizzleInfoArr.Length > 1 )
									{
										templateVertexData.DataSwizzle = "." + swizzleInfoArr[ 1 ];
									}
									templateVertexData.DataInfo = ShortcutToInfo[ paramDataArr[ 0 ] ];
									templateVertexData.Available = true;
									interpDataList.Add( templateVertexData );
								}
							}
						}
					}
				}
				/*TODO: 
				1) Remove interpDataList.Add( templateVertexData ); from initial foreach 
				2) When looping though each foreach array element, create a new TemplateVertexData
				from the one containted on the interpDataDict and add it to interpDataList
				*/

				interpDataObj.Interpolators = interpDataList;
				interpDataDict.Clear();
				interpDataDict = null;
			}
			return interpDataObj;
		}

		public static string AutoSwizzleData( string dataVar, WirePortDataType dataType, WirePortDataType swizzle )
		{
			switch( dataType )
			{
				case WirePortDataType.COLOR:
				case WirePortDataType.FLOAT4:
				{
					switch( swizzle )
					{
						case WirePortDataType.FLOAT3: dataVar += ".xyz"; break;
						case WirePortDataType.FLOAT2: dataVar += ".xy"; break;
						case WirePortDataType.INT:
						case WirePortDataType.FLOAT: dataVar += ".x"; break;
					}
				}
				break;
				case WirePortDataType.FLOAT3:
				{
					switch( swizzle )
					{
						case WirePortDataType.FLOAT4: dataVar = string.Format( "float4({0},0)", dataVar ); break;
						case WirePortDataType.FLOAT2: dataVar += ".xy"; break;
						case WirePortDataType.INT:
						case WirePortDataType.FLOAT: dataVar += ".x"; break;
					}
				}
				break;
				case WirePortDataType.FLOAT2:
				{
					switch( swizzle )
					{
						case WirePortDataType.FLOAT4: dataVar = string.Format( "float4({0},0,0)", dataVar ); break;
						case WirePortDataType.FLOAT3: dataVar = string.Format( "float3({0},0)", dataVar ); break;
						case WirePortDataType.INT:
						case WirePortDataType.FLOAT: dataVar += ".x"; break;
					}
				}
				break;
				case WirePortDataType.FLOAT:
				{
					switch( swizzle )
					{
						case WirePortDataType.FLOAT4: dataVar = string.Format( "float4({0},0,0,0)", dataVar ); break;
						case WirePortDataType.FLOAT3: dataVar = string.Format( "float3({0},0,0)", dataVar ); break;
						case WirePortDataType.FLOAT2: dataVar = string.Format( "float2({0},0)", dataVar ); break;
					}
				}
				break;
			}
			return dataVar;
		}

		public static bool CheckIfTemplate( string assetPath )
		{
			Shader shader = AssetDatabase.LoadAssetAtPath<Shader>( assetPath );
			if( shader != null )
			{
				string body = IOUtils.LoadTextFileFromDisk( assetPath );
				return ( body.IndexOf( TemplatesManager.TemplateShaderNameBeginTag ) > -1 );
			}
			return false;
		}

		public static bool CheckIfCompatibles( WirePortDataType first, WirePortDataType second )
		{
			switch( first )
			{
				case WirePortDataType.OBJECT:
				return true;
				case WirePortDataType.FLOAT:
				case WirePortDataType.FLOAT2:
				case WirePortDataType.FLOAT3:
				case WirePortDataType.FLOAT4:
				case WirePortDataType.COLOR:
				case WirePortDataType.INT:
				{
					switch( second )
					{
						case WirePortDataType.FLOAT3x3:
						case WirePortDataType.FLOAT4x4:
						case WirePortDataType.SAMPLER1D:
						case WirePortDataType.SAMPLER2D:
						case WirePortDataType.SAMPLER3D:
						case WirePortDataType.SAMPLERCUBE:
						return false;
					}
				}
				break;
				case WirePortDataType.FLOAT3x3:
				case WirePortDataType.FLOAT4x4:
				{
					switch( second )
					{
						case WirePortDataType.FLOAT:
						case WirePortDataType.FLOAT2:
						case WirePortDataType.FLOAT3:
						case WirePortDataType.FLOAT4:
						case WirePortDataType.COLOR:
						case WirePortDataType.INT:
						case WirePortDataType.SAMPLER1D:
						case WirePortDataType.SAMPLER2D:
						case WirePortDataType.SAMPLER3D:
						case WirePortDataType.SAMPLERCUBE:
						return false;
					}
				}
				break;
				case WirePortDataType.SAMPLER1D:
				case WirePortDataType.SAMPLER2D:
				case WirePortDataType.SAMPLER3D:
				case WirePortDataType.SAMPLERCUBE:
				{
					switch( second )
					{
						case WirePortDataType.FLOAT:
						case WirePortDataType.FLOAT2:
						case WirePortDataType.FLOAT3:
						case WirePortDataType.FLOAT4:
						case WirePortDataType.FLOAT3x3:
						case WirePortDataType.FLOAT4x4:
						case WirePortDataType.COLOR:
						case WirePortDataType.INT:
						return false;
					}
				}
				break;
			}
			return true;
		}
	}
}