using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace TaiwuEventCompiler;

internal class Compiler
{
	private CSharpCompilation _compilation;

	private string[] _scriptsContentArray;

	private MetadataReference _gameDataReference;

	private List<MetadataReference> _backendDllReferences;

	private HashSet<string> _loadedReferenceDisplays;

	private UTF8Encoding _encoding;

	public int Start()
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		_encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
		InitGameDataReference();
		for (int i = 0; i < CompileConfig.ExportEventGroupConfig.Length; i += 2)
		{
			string text = CompileConfig.ExportEventGroupConfig[i];
			string text2 = CompileConfig.ExportEventGroupConfig[i + 1];
			string text3 = "EventPackage_" + text;
			string text4 = Path.Combine(CompileConfig.ExportFileRootFolder, CompileConfig.NameSpace + "_" + text3 + ".dll");
			_compilation = CSharpCompilation.Create(Path.GetFileNameWithoutExtension(text4), (IEnumerable<SyntaxTree>)null, (IEnumerable<MetadataReference>)null, (CSharpCompilationOptions)null).WithOptions(new CSharpCompilationOptions((OutputKind)2, false, (string)null, (string)null, (string)null, (IEnumerable<string>)null, (OptimizationLevel)0, false, false, (string)null, (string)null, default(ImmutableArray<byte>), (bool?)null, (Platform)0, (ReportDiagnostic)0, 4, (IEnumerable<KeyValuePair<string, ReportDiagnostic>>)null, true, false, (XmlReferenceResolver)null, (SourceReferenceResolver)null, (MetadataReferenceResolver)null, (AssemblyIdentityComparer)null, (StrongNameProvider)null, false, (MetadataImportOptions)0, (NullableContextOptions)0));
			ReadScriptFiles(text);
			EmitResult val = Emit(text4);
			if (!val.Success)
			{
				ConsoleColor foregroundColor = Console.ForegroundColor;
				for (int j = 0; j < val.Diagnostics.Length; j++)
				{
					Diagnostic val2 = val.Diagnostics[j];
					if (val2.WarningLevel == 0)
					{
						Console.WriteLine(text2 + " Codes below has errors:" + (object)val2);
						FileLinePositionSpan lineSpan = val.Diagnostics[j].Location.GetLineSpan();
						LinePosition startLinePosition = ((FileLinePositionSpan)(ref lineSpan)).StartLinePosition;
						int line = ((LinePosition)(ref startLinePosition)).Line;
						string[] array = ((object)val.Diagnostics[j].Location.SourceTree).ToString().Split('\n');
						for (int k = Math.Max(0, line - 5); k < Math.Min(array.Length, line + 5); k++)
						{
							Console.ForegroundColor = ((k == line) ? ConsoleColor.Red : foregroundColor);
							Console.WriteLine(array[k]);
						}
					}
				}
				Console.ReadKey();
				return -1;
			}
			Console.WriteLine(text2 + " Compile Success!");
		}
		return 0;
	}

	private void InitGameDataReference()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		_gameDataReference = (MetadataReference)(object)MetadataReference.CreateFromFile(CompileConfig.GameDataFilePath, default(MetadataReferenceProperties), (DocumentationProvider)null);
		string fullName = Directory.GetParent(CompileConfig.GameDataFilePath).FullName;
		_backendDllReferences = new List<MetadataReference>();
		AddDllReference(Path.Combine(fullName, "CompDevLib.Interpreter.dll"));
		AddDllReference(Path.Combine(fullName, "netstandard.dll"));
		AssemblyName[] referencedAssemblies = Assembly.LoadFile(CompileConfig.GameDataFilePath).GetReferencedAssemblies();
		foreach (AssemblyName assemblyName in referencedAssemblies)
		{
			string? name = assemblyName.Name;
			if (name != null && name.StartsWith("GameData"))
			{
				AddDllReference(Path.Combine(fullName, assemblyName.Name + ".dll"));
			}
		}
		if (!string.IsNullOrEmpty(CompileConfig.BackEndDllFolder) && Directory.Exists(CompileConfig.BackEndDllFolder))
		{
			string[] files = Directory.GetFiles(CompileConfig.BackEndDllFolder, "*.dll", SearchOption.AllDirectories);
			for (int j = 0; j < files.Length; j++)
			{
				AddDllReference(files[j]);
			}
		}
	}

	private void AddDllReference(string path)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (_backendDllReferences == null)
		{
			_backendDllReferences = new List<MetadataReference>();
		}
		if (_loadedReferenceDisplays == null)
		{
			_loadedReferenceDisplays = new HashSet<string>();
		}
		PortableExecutableReference val = MetadataReference.CreateFromFile(path, default(MetadataReferenceProperties), (DocumentationProvider)null);
		if (_loadedReferenceDisplays.Add(((MetadataReference)val).Display))
		{
			Console.WriteLine($"Loaded {((MetadataReference)val).Display} from {path}.");
			_backendDllReferences.Add((MetadataReference)(object)val);
		}
		else
		{
			Console.WriteLine($"duplicate reference for {((MetadataReference)val).Display} at {path}.");
		}
	}

	private void ReadScriptFiles(string eventGroupKey)
	{
		string[] files = Directory.GetFiles(Path.Combine(CompileConfig.ScriptsRootFolder, eventGroupKey), "*.cs", SearchOption.AllDirectories);
		_scriptsContentArray = new string[files.Length];
		for (int i = 0; i < files.Length; i++)
		{
			_scriptsContentArray[i] = File.ReadAllText(files[i]);
		}
	}

	private EmitResult Emit(string exportDllPath)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		PortableExecutableReference val = MetadataReference.CreateFromFile(typeof(object).Assembly.Location, default(MetadataReferenceProperties), (DocumentationProvider)null);
		List<MetadataReference> backendDllReferences = _backendDllReferences;
		if (backendDllReferences != null && backendDllReferences.Count > 0)
		{
			_compilation = _compilation.AddReferences((IEnumerable<MetadataReference>)_backendDllReferences);
		}
		_compilation = _compilation.AddReferences((MetadataReference[])(object)new MetadataReference[2]
		{
			_gameDataReference,
			(MetadataReference)val
		});
		_compilation = _compilation.AddReferences((IEnumerable<MetadataReference>)(from x in AppDomain.CurrentDomain.GetAssemblies()
			select MetadataReference.CreateFromFile(x.Location, default(MetadataReferenceProperties), (DocumentationProvider)null)));
		CSharpParseOptions val2 = new CSharpParseOptions((LanguageVersion)int.MaxValue, (DocumentationMode)1, (SourceCodeKind)0, (IEnumerable<string>)null);
		List<SyntaxTree> list = new List<SyntaxTree>();
		for (int num = 0; num < _scriptsContentArray.Length; num++)
		{
			string text = _scriptsContentArray[num];
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(CSharpSyntaxTree.ParseText(text, val2, "", (Encoding)_encoding, default(CancellationToken)));
			}
		}
		_compilation = _compilation.AddSyntaxTrees((IEnumerable<SyntaxTree>)list);
		using MemoryStream memoryStream = new MemoryStream();
		EmitResult result = ((Compilation)_compilation).Emit((Stream)memoryStream, (Stream)null, (Stream)null, (Stream)null, (IEnumerable<ResourceDescription>)null, (EmitOptions)null, (IMethodSymbol)null, (Stream)null, (IEnumerable<EmbeddedText>)null, (Stream)null, default(CancellationToken));
		File.WriteAllBytes(exportDllPath, memoryStream.ToArray());
		return result;
	}
}
