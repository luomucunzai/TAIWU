using System;
using System.IO;

namespace TaiwuEventCompiler;

internal class Program
{
	private static int Main(string[] args)
	{
		if (args == null || args.Length < 6)
		{
			Console.WriteLine("args error:args count must more than 5!");
			Console.ReadKey();
			return 1;
		}
		CompileConfig.NameSpace = args[0];
		CompileConfig.ScriptsRootFolder = args[1];
		CompileConfig.GameDataFilePath = args[2];
		CompileConfig.ExportFileRootFolder = args[3];
		CompileConfig.BackEndDllFolder = args[4];
		CompileConfig.ExportEventGroupConfig = new string[args.Length - 5];
		Array.Copy(args, 5, CompileConfig.ExportEventGroupConfig, 0, args.Length - 5);
		if (string.IsNullOrEmpty(CompileConfig.ScriptsRootFolder))
		{
			Console.WriteLine("need to specify ScriptsRootFolder!");
			Console.ReadKey();
			return 2;
		}
		if (!Directory.Exists(CompileConfig.ScriptsRootFolder))
		{
			Console.WriteLine(CompileConfig.ScriptsRootFolder + " is not exist!");
			Console.ReadKey();
			return 3;
		}
		if (string.IsNullOrEmpty(CompileConfig.ExportFileRootFolder))
		{
			Console.WriteLine("need to specify ExportFileRootFolder!");
			Console.ReadKey();
			return 4;
		}
		if (string.IsNullOrEmpty(CompileConfig.GameDataFilePath))
		{
			Console.WriteLine("need to specify GameDataFilePath!");
			Console.ReadKey();
			return 5;
		}
		if (!File.Exists(CompileConfig.GameDataFilePath))
		{
			Console.WriteLine(CompileConfig.GameDataFilePath + " is not exist!");
			Console.ReadKey();
			return 6;
		}
		return new Compiler().Start();
	}
}
