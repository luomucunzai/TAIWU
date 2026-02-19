using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib.Internal.Patching;
using HarmonyLib.Internal.Util;
using HarmonyLib.Tools;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace HarmonyLib.Public.Patching;

public class HarmonyManipulator
{
	private class PatchContext
	{
		public bool hasArgsArrayArg;

		public MethodInfo method;

		public bool wrapTryCatch;
	}

	private class ArgumentBoxInfo
	{
		public int index;

		public bool isByRef;

		public VariableDefinition tmpVar;

		public Type type;
	}

	private static readonly MethodInfo GetMethodFromHandle1 = typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[1] { typeof(RuntimeMethodHandle) });

	private static readonly MethodInfo GetMethodFromHandle2 = typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[2]
	{
		typeof(RuntimeMethodHandle),
		typeof(RuntimeTypeHandle)
	});

	private static readonly string InstanceParam = "__instance";

	private static readonly string OriginalMethodParam = "__originalMethod";

	private static readonly string RunOriginalParam = "__runOriginal";

	private static readonly string ResultVar = "__result";

	private static readonly string ArgsArrayVar = "__args";

	private static readonly string StateVar = "__state";

	private static readonly string ExceptionVar = "__exception";

	private static readonly string ParamIndexPrefix = "__";

	private static readonly string InstanceFieldPrefix = "___";

	private static readonly MethodInfo LogPatchExceptionMethod = AccessTools.Method(typeof(HarmonyManipulator), "LogPatchException");

	private readonly bool debug;

	private readonly string[] dumpPaths;

	private readonly PatchInfo patchInfo;

	private readonly Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>();

	private ILContext ctx;

	private List<PatchContext> finalizers;

	private ILEmitter il;

	private List<PatchContext> ilManipulators;

	private MethodBase original;

	private List<PatchContext> postfixes;

	private List<PatchContext> prefixes;

	private List<PatchContext> transpilers;

	public HarmonyManipulator(PatchInfo patchInfo)
	{
		this.patchInfo = patchInfo;
		debug = patchInfo.Debugging;
		dumpPaths = patchInfo.DebugEmitPaths;
	}

	public static void Manipulate(MethodBase original, PatchInfo patchInfo, ILContext ctx)
	{
		new HarmonyManipulator(patchInfo).Process(ctx, original);
	}

	public static void Manipulate(MethodBase original, ILContext ctx)
	{
		Manipulate(original, original.GetPatchInfo(), ctx);
	}

	private static void LogPatchException(object errorObject, string patch)
	{
		Logger.LogText(Logger.LogChannel.Error, $"Error while running {patch}. Error: {errorObject}");
	}

	public void Process(ILContext ilContext, MethodBase originalMethod)
	{
		ctx = ilContext;
		original = originalMethod;
		il = new ILEmitter(ctx.IL);
		SortPatches();
		Logger.Log(Logger.LogChannel.Info, delegate
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"Patching {original.FullDescription()} with {prefixes.Count} prefixes, {postfixes.Count} postfixes, {transpilers.Count} transpilers, {finalizers.Count} finalizers");
			Print(prefixes, "prefixes");
			Print(postfixes, "postfixes");
			Print(transpilers, "transpilers");
			Print(finalizers, "finalizers");
			Print(ilManipulators, "ilmanipulators");
			return sb.ToString();
			void Print(ICollection<PatchContext> list, string type)
			{
				if (list.Count == 0)
				{
					return;
				}
				sb.AppendLine($"{list.Count} {type}:");
				foreach (PatchContext item in list)
				{
					sb.AppendLine("* " + item.method.FullDescription());
				}
			}
		}, debug);
		WriteImpl();
		if (dumpPaths.Length != 0)
		{
			CecilEmitter.Dump(ctx.Method, dumpPaths, original);
		}
	}

	private bool WritePrefixes(ILEmitter.Label returnLabel)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		if (prefixes.Count == 0)
		{
			return false;
		}
		Logger.Log(Logger.LogChannel.Info, () => "Writing prefixes", debug);
		il.emitBefore = il.IL.Body.Instructions[0];
		VariableDefinition val;
		if (!variables.TryGetValue(ResultVar, out var value))
		{
			Type returnedType = AccessTools.GetReturnedType(original);
			val = (variables[ResultVar] = ((returnedType == typeof(void)) ? null : il.DeclareVariable(returnedType)));
			value = val;
		}
		bool flag = prefixes.Any((PatchContext p) => p.method.ReturnType == typeof(bool) || p.method.GetParameters().Any((ParameterInfo pp) => pp.Name == RunOriginalParam && pp.ParameterType.OpenRefType() == typeof(bool)));
		val = (variables[RunOriginalParam] = il.DeclareVariable(typeof(bool)));
		VariableDefinition varDef = val;
		il.Emit(OpCodes.Ldc_I4_1);
		il.Emit(OpCodes.Stloc, varDef);
		ILEmitter.Label label = ((value != null) ? il.DeclareLabel() : returnLabel);
		foreach (PatchContext prefix in prefixes)
		{
			MethodInfo method = prefix.method;
			ILEmitter.Label label2 = il.DeclareLabel();
			il.MarkLabel(label2);
			EmitCallParameter(method, allowFirsParamPassthrough: false, out var tmpObjectVar, out var tmpBoxVars);
			il.Emit(OpCodes.Call, method);
			if (prefix.hasArgsArrayArg)
			{
				EmitAssignRefsFromArgsArray(variables[ArgsArrayVar]);
			}
			EmitResultUnbox(tmpObjectVar, value);
			EmitArgUnbox(tmpBoxVars);
			if (!AccessTools.IsVoid(method.ReturnType))
			{
				if (method.ReturnType != typeof(bool))
				{
					throw new InvalidHarmonyPatchArgumentException($"Prefix patch {method.FullDescription()} has return type {method.ReturnType}, but only `bool` or `void` are permitted", original, method);
				}
				if (flag)
				{
					il.Emit(OpCodes.Ldloc, varDef);
					il.Emit(OpCodes.And);
					il.Emit(OpCodes.Stloc, varDef);
				}
			}
			if (prefix.wrapTryCatch)
			{
				EmitTryCatchWrapper(method, label2);
			}
		}
		if (!flag)
		{
			return false;
		}
		il.Emit(OpCodes.Ldloc, varDef);
		il.Emit(OpCodes.Brfalse, label);
		if (value == null)
		{
			return true;
		}
		il.emitBefore = il.IL.Body.Instructions[il.IL.Body.Instructions.Count - 1];
		il.MarkLabel(label);
		il.Emit(OpCodes.Ldloc, value);
		return true;
	}

	private void EmitTryCatchWrapper(MethodBase target, ILEmitter.Label start)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		ILEmitter.ExceptionBlock block = il.BeginExceptionBlock(start);
		il.BeginHandler(block, (ExceptionHandlerType)0, typeof(object));
		il.Emit(OpCodes.Ldstr, target.FullDescription());
		il.Emit(OpCodes.Call, LogPatchExceptionMethod);
		il.EndExceptionBlock(block);
		il.Emit(OpCodes.Nop);
	}

	private void EmitArgUnbox(List<ArgumentBoxInfo> boxInfo)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		if (boxInfo == null)
		{
			return;
		}
		foreach (ArgumentBoxInfo item in boxInfo)
		{
			if (item.isByRef)
			{
				il.Emit(OpCodes.Ldarg, item.index);
			}
			il.Emit(OpCodes.Ldloc, item.tmpVar);
			il.Emit(OpCodes.Unbox_Any, item.type);
			if (item.isByRef)
			{
				il.Emit(OpCodes.Stobj, item.type);
			}
			else
			{
				il.Emit(OpCodes.Starg, item.index);
			}
		}
	}

	private void EmitResultUnbox(VariableDefinition tmp, VariableDefinition result)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (tmp != null)
		{
			il.Emit(OpCodes.Ldloc, tmp);
			il.Emit(OpCodes.Unbox_Any, AccessTools.GetReturnedType(original));
			il.Emit(OpCodes.Stloc, result);
		}
	}

	private void WriteTranspilers()
	{
		if (transpilers.Count == 0)
		{
			return;
		}
		Logger.Log(Logger.LogChannel.Info, () => "Transpiling " + original.FullDescription(), debug);
		ILManipulator iLManipulator = new ILManipulator(ctx.Body, debug);
		foreach (PatchContext transpiler in transpilers)
		{
			iLManipulator.AddTranspiler(transpiler.method);
		}
		iLManipulator.WriteTo(ctx.Body, original);
	}

	private (ILEmitter.Label, bool) MakeReturnLabel()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		if (ctx.IL.Body.Instructions.Count == 0)
		{
			il.Emit(OpCodes.Nop);
		}
		ILEmitter.Label label = il.DeclareLabel();
		label.emitted = false;
		bool item = false;
		foreach (Instruction item2 in ((IEnumerable<Instruction>)il.IL.Body.Instructions).Where((Instruction ins) => ILPatternMatchingExt.MatchRet(ins)))
		{
			item = true;
			item2.OpCode = OpCodes.Br;
			item2.Operand = label.instruction;
			label.targets.Add(item2);
		}
		label.instruction = Instruction.Create(OpCodes.Ret);
		il.IL.Append(label.instruction);
		return (label, item);
	}

	private void WriteImpl()
	{
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (original == null)
			{
				throw new ArgumentException("original");
			}
			Logger.Log(Logger.LogChannel.Info, () => "Running ILHook manipulator on " + original.FullDescription(), debug);
			WriteTranspilers();
			if (prefixes.Count + postfixes.Count + finalizers.Count + ilManipulators.Count == 0)
			{
				Logger.Log(Logger.LogChannel.IL, () => "Generated patch (" + ((MemberReference)ctx.Method).FullName + "):\n" + ctx.Body.ToILDasmString(), debug);
				return;
			}
			var (label, emitResultStore) = MakeReturnLabel();
			foreach (PatchContext item in prefixes.Union(postfixes).Union(finalizers))
			{
				ParameterInfo[] parameters = item.method.GetParameters();
				ParameterInfo parameterInfo = parameters.FirstOrDefault((ParameterInfo p) => p.Name == ArgsArrayVar);
				if (parameterInfo != null)
				{
					if (parameterInfo.ParameterType != typeof(object[]))
					{
						throw new InvalidHarmonyPatchArgumentException("Patch " + item.method.FullDescription() + " defines __args list, but only type `object[]` is permitted", original, item.method);
					}
					variables[ArgsArrayVar] = il.DeclareVariable(typeof(object[]));
				}
				if (item.method.DeclaringType?.FullName == null || variables.ContainsKey(item.method.DeclaringType.FullName))
				{
					continue;
				}
				foreach (ParameterInfo item2 in parameters.Where((ParameterInfo patchParam) => patchParam.Name == StateVar))
				{
					variables[item.method.DeclaringType.FullName] = il.DeclareVariable(item2.ParameterType.OpenRefType());
				}
			}
			WritePrefixes(label);
			WritePostfixes(label, emitResultStore);
			WriteFinalizers(label);
			il.MarkLabel(label);
			il.SetOpenLabelsTo(ctx.Instrs[ctx.Instrs.Count - 1]);
			if (variables.TryGetValue(ArgsArrayVar, out var value))
			{
				il.emitBefore = ctx.Instrs[0];
				EmitInitArgsArray();
				il.Emit(OpCodes.Stloc, value);
			}
			ApplyManipulators(ctx, original, ilManipulators.Select((PatchContext m) => m.method).ToList(), label);
			Logger.Log(Logger.LogChannel.IL, () => "Generated patch (" + ((MemberReference)ctx.Method).FullName + "):\n" + ctx.Body.ToILDasmString(), debug);
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			Logger.Log(Logger.LogChannel.Error, () => $"Failed to patch {original.FullDescription()}: {e}", debug);
			throw HarmonyException.Create(e, ctx.Body);
		}
	}

	internal static void ApplyManipulators(ILContext ctx, MethodBase original, List<MethodInfo> ilManipulators, ILEmitter.Label retLabel)
	{
		ILLabel item = ctx.DefineLabel(retLabel?.instruction) ?? ctx.DefineLabel(((IEnumerable<Instruction>)ctx.Body.Instructions).Last());
		foreach (MethodInfo ilManipulator in ilManipulators)
		{
			List<object> list = new List<object>();
			foreach (Type item2 in from p in ilManipulator.GetParameters()
				select p.ParameterType)
			{
				if (item2.IsAssignableFrom(typeof(ILContext)))
				{
					list.Add(ctx);
				}
				if (item2.IsAssignableFrom(typeof(MethodBase)))
				{
					list.Add(original);
				}
				if (item2.IsAssignableFrom(typeof(ILLabel)))
				{
					list.Add(item);
				}
			}
			ilManipulator.Invoke(null, list.ToArray());
		}
	}

	private void EmitOutParameter(int arg, Type t)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		t = t.OpenRefType();
		il.Emit(OpCodes.Ldarg, arg);
		if (AccessTools.IsStruct(t))
		{
			il.Emit(OpCodes.Initobj, t);
		}
		else if (AccessTools.IsValue(t))
		{
			var (num, opcode, opcode2) = ((t == typeof(float)) ? (0.0, OpCodes.Ldc_R4, OpCodes.Stind_R4) : ((t == typeof(double)) ? (0.0, OpCodes.Ldc_R8, OpCodes.Stind_R8) : ((!(t == typeof(long))) ? (0.0, OpCodes.Ldc_I4, t.GetCecilStoreOpCode()) : (0.0, OpCodes.Ldc_I8, OpCodes.Stind_I8))));
			il.EmitUnsafe(opcode, num);
			il.Emit(opcode2);
		}
		else
		{
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stind_Ref);
		}
	}

	private void EmitInitArgsArray()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		ParameterInfo[] parameters = original.GetParameters();
		int num = ((!original.IsStatic) ? 1 : 0);
		for (int i = 0; i < parameters.Length; i++)
		{
			ParameterInfo parameterInfo = parameters[i];
			if (parameterInfo.IsOut || parameterInfo.IsRetval)
			{
				EmitOutParameter(i + num, parameterInfo.ParameterType.OpenRefType());
			}
		}
		il.Emit(OpCodes.Ldc_I4, parameters.Length);
		il.Emit(OpCodes.Newarr, typeof(object));
		for (int j = 0; j < parameters.Length; j++)
		{
			Type parameterType = parameters[j].ParameterType;
			Type type = parameterType.OpenRefType();
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Ldc_I4, j);
			il.Emit(OpCodes.Ldarg, j + num);
			if (parameterType.IsByRef)
			{
				if (AccessTools.IsStruct(type))
				{
					il.Emit(OpCodes.Ldobj, type);
				}
				else
				{
					il.Emit(type.GetCecilLoadOpCode());
				}
			}
			if (type.IsValueType)
			{
				il.Emit(OpCodes.Box, type);
			}
			il.Emit(OpCodes.Stelem_Ref);
		}
	}

	private bool WriteFinalizers(ILEmitter.Label returnLabel)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		if (finalizers.Count == 0)
		{
			return false;
		}
		Logger.Log(Logger.LogChannel.Info, () => "Writing finalizers", debug);
		variables[ExceptionVar] = il.DeclareVariable(typeof(Exception));
		VariableDefinition varDef = il.DeclareVariable(typeof(bool));
		il.emitBefore = il.IL.Body.Instructions[0];
		il.Emit(OpCodes.Ldc_I4_0);
		il.Emit(OpCodes.Stloc, varDef);
		il.emitBefore = il.IL.Body.Instructions[il.IL.Body.Instructions.Count - 1];
		il.MarkLabel(returnLabel);
		if (!variables.TryGetValue(ResultVar, out var returnValueVar))
		{
			Type returnedType = AccessTools.GetReturnedType(original);
			VariableDefinition val = (variables[ResultVar] = ((returnedType == typeof(void)) ? null : il.DeclareVariable(returnedType)));
			returnValueVar = val;
		}
		ILEmitter.ExceptionBlock block = il.BeginExceptionBlock(il.DeclareLabelFor(il.IL.Body.Instructions[0]));
		if (returnValueVar != null)
		{
			il.Emit(OpCodes.Stloc, returnValueVar);
		}
		WriteFinalizerCalls(suppressExceptions: false);
		il.Emit(OpCodes.Ldc_I4_1);
		il.Emit(OpCodes.Stloc, varDef);
		ILEmitter.Label label = il.DeclareLabel();
		il.Emit(OpCodes.Ldloc, variables[ExceptionVar]);
		il.Emit(OpCodes.Brfalse, label);
		il.Emit(OpCodes.Ldloc, variables[ExceptionVar]);
		il.Emit(OpCodes.Throw);
		il.MarkLabel(label);
		il.BeginHandler(block, (ExceptionHandlerType)0, typeof(Exception));
		il.Emit(OpCodes.Stloc, variables[ExceptionVar]);
		il.Emit(OpCodes.Ldloc, varDef);
		ILEmitter.Label label2 = il.DeclareLabel();
		il.Emit(OpCodes.Brtrue, label2);
		bool num = WriteFinalizerCalls(suppressExceptions: true);
		il.MarkLabel(label2);
		label = il.DeclareLabel();
		il.Emit(OpCodes.Ldloc, variables[ExceptionVar]);
		il.Emit(OpCodes.Brfalse, label);
		if (num)
		{
			il.Emit(OpCodes.Rethrow);
		}
		else
		{
			il.Emit(OpCodes.Ldloc, variables[ExceptionVar]);
			il.Emit(OpCodes.Throw);
		}
		il.MarkLabel(label);
		il.EndExceptionBlock(block);
		if (returnValueVar != null)
		{
			il.Emit(OpCodes.Ldloc, returnValueVar);
		}
		return true;
		bool WriteFinalizerCalls(bool suppressExceptions)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			bool result = true;
			foreach (PatchContext finalizer in finalizers)
			{
				MethodInfo method = finalizer.method;
				ILEmitter.Label label3 = il.DeclareLabel();
				il.MarkLabel(label3);
				EmitCallParameter(method, allowFirsParamPassthrough: false, out var tmpObjectVar, out var tmpBoxVars);
				il.Emit(OpCodes.Call, method);
				if (finalizer.hasArgsArrayArg)
				{
					EmitAssignRefsFromArgsArray(variables[ArgsArrayVar]);
				}
				EmitResultUnbox(tmpObjectVar, returnValueVar);
				EmitArgUnbox(tmpBoxVars);
				if (method.ReturnType != typeof(void))
				{
					il.Emit(OpCodes.Stloc, variables[ExceptionVar]);
					result = false;
				}
				if (suppressExceptions || finalizer.wrapTryCatch)
				{
					EmitTryCatchWrapper(method, label3);
				}
			}
			return result;
		}
	}

	private void WritePostfixes(ILEmitter.Label returnLabel, bool emitResultStore)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		if (postfixes.Count == 0)
		{
			return;
		}
		Logger.Log(Logger.LogChannel.Info, () => "Writing postfixes", debug);
		il.emitBefore = il.IL.Body.Instructions[il.IL.Body.Instructions.Count - 1];
		il.MarkLabel(returnLabel);
		if (!variables.TryGetValue(ResultVar, out var value))
		{
			Type returnedType = AccessTools.GetReturnedType(original);
			VariableDefinition val = (variables[ResultVar] = ((returnedType == typeof(void)) ? null : il.DeclareVariable(returnedType)));
			value = val;
		}
		if (value != null && emitResultStore)
		{
			il.Emit(OpCodes.Stloc, value);
		}
		if (!variables.ContainsKey(RunOriginalParam))
		{
			VariableDefinition val = (variables[RunOriginalParam] = il.DeclareVariable(typeof(bool)));
			VariableDefinition varDef = val;
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Stloc, varDef);
		}
		foreach (PatchContext item in postfixes.Where((PatchContext p) => p.method.ReturnType == typeof(void)))
		{
			MethodInfo method = item.method;
			ILEmitter.Label label = il.DeclareLabel();
			il.MarkLabel(label);
			EmitCallParameter(method, allowFirsParamPassthrough: true, out var tmpObjectVar, out var tmpBoxVars);
			il.Emit(OpCodes.Call, method);
			if (item.hasArgsArrayArg)
			{
				EmitAssignRefsFromArgsArray(variables[ArgsArrayVar]);
			}
			EmitResultUnbox(tmpObjectVar, value);
			EmitArgUnbox(tmpBoxVars);
			if (item.wrapTryCatch)
			{
				EmitTryCatchWrapper(method, label);
			}
		}
		if (value != null)
		{
			il.Emit(OpCodes.Ldloc, value);
		}
		foreach (PatchContext item2 in postfixes.Where((PatchContext p) => p.method.ReturnType != typeof(void)))
		{
			MethodInfo method2 = item2.method;
			if (item2.wrapTryCatch)
			{
				il.Emit(OpCodes.Stloc, value);
			}
			ILEmitter.Label label2 = il.DeclareLabel();
			il.MarkLabel(label2);
			if (item2.wrapTryCatch)
			{
				il.Emit(OpCodes.Ldloc, value);
			}
			EmitCallParameter(method2, allowFirsParamPassthrough: true, out var tmpObjectVar2, out var tmpBoxVars2);
			il.Emit(OpCodes.Call, method2);
			if (item2.hasArgsArrayArg)
			{
				EmitAssignRefsFromArgsArray(variables[ArgsArrayVar]);
			}
			EmitResultUnbox(tmpObjectVar2, value);
			EmitArgUnbox(tmpBoxVars2);
			ParameterInfo parameterInfo = method2.GetParameters().FirstOrDefault();
			if (parameterInfo == null || method2.ReturnType != parameterInfo.ParameterType)
			{
				if (parameterInfo != null)
				{
					throw new InvalidHarmonyPatchArgumentException("Return type of pass through postfix " + method2.FullDescription() + " does not match type of its first parameter", original, method2);
				}
				throw new InvalidHarmonyPatchArgumentException("Postfix patch " + method2.FullDescription() + " must have `void` as return type", original, method2);
			}
			if (item2.wrapTryCatch)
			{
				il.Emit(OpCodes.Stloc, value);
				EmitTryCatchWrapper(method2, label2);
				il.Emit(OpCodes.Ldloc, value);
			}
		}
	}

	private void SortPatches()
	{
		Patch[] patches;
		Patch[] patches2;
		Patch[] patches3;
		Patch[] patches4;
		Patch[] patches5;
		lock (patchInfo)
		{
			patches = patchInfo.prefixes.ToArray();
			patches2 = patchInfo.postfixes.ToArray();
			patches3 = patchInfo.transpilers.ToArray();
			patches4 = patchInfo.finalizers.ToArray();
			patches5 = patchInfo.ilmanipulators.ToArray();
		}
		prefixes = Sort(original, patches, debug);
		postfixes = Sort(original, patches2, debug);
		transpilers = Sort(original, patches3, debug);
		finalizers = Sort(original, patches4, debug);
		ilManipulators = Sort(original, patches5, debug);
		static List<PatchContext> Sort(MethodBase original, Patch[] patches6, bool debug)
		{
			return (from p in PatchFunctions.GetSortedPatchMethodsAsPatches(original, patches6, debug)
				select new
				{
					method = p.GetMethod(original),
					origP = p
				} into p
				select new PatchContext
				{
					method = p.method,
					wrapTryCatch = p.origP.wrapTryCatch,
					hasArgsArrayArg = p.method.GetParameters().Any((ParameterInfo par) => par.Name == ArgsArrayVar)
				}).ToList();
		}
	}

	private bool EmitOriginalBaseMethod()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (original is MethodInfo mInfo)
		{
			il.Emit(OpCodes.Ldtoken, mInfo);
		}
		else
		{
			if (!(original is ConstructorInfo cInfo))
			{
				return false;
			}
			il.Emit(OpCodes.Ldtoken, cInfo);
		}
		Type reflectedType = original.ReflectedType;
		if (reflectedType == null)
		{
			return false;
		}
		if (reflectedType.IsGenericType)
		{
			il.Emit(OpCodes.Ldtoken, reflectedType);
		}
		il.Emit(OpCodes.Call, reflectedType.IsGenericType ? GetMethodFromHandle2 : GetMethodFromHandle1);
		return true;
	}

	private void EmitAssignRefsFromArgsArray(VariableDefinition argsArrayVariable)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		ParameterInfo[] parameters = original.GetParameters();
		int num = ((!original.IsStatic) ? 1 : 0);
		for (int i = 0; i < parameters.Length; i++)
		{
			Type parameterType = parameters[i].ParameterType;
			if (!parameterType.IsByRef)
			{
				continue;
			}
			parameterType = parameterType.OpenRefType();
			il.Emit(OpCodes.Ldarg, i + num);
			il.Emit(OpCodes.Ldloc, argsArrayVariable);
			il.Emit(OpCodes.Ldc_I4, i);
			il.Emit(OpCodes.Ldelem_Ref);
			if (parameterType.IsValueType)
			{
				il.Emit(OpCodes.Unbox_Any, parameterType);
				if (AccessTools.IsStruct(parameterType))
				{
					il.Emit(OpCodes.Stobj, parameterType);
				}
				else
				{
					il.Emit(parameterType.GetCecilStoreOpCode());
				}
			}
			else
			{
				il.Emit(OpCodes.Castclass, parameterType);
				il.Emit(OpCodes.Stind_Ref);
			}
		}
	}

	private void EmitCallParameter(MethodInfo patch, bool allowFirsParamPassthrough, out VariableDefinition tmpObjectVar, out List<ArgumentBoxInfo> tmpBoxVars)
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		tmpObjectVar = null;
		tmpBoxVars = new List<ArgumentBoxInfo>();
		bool flag = !original.IsStatic;
		ParameterInfo[] parameters = original.GetParameters();
		string[] originalParameterNames = parameters.Select((ParameterInfo p) => p.Name).ToArray();
		List<ParameterInfo> list = patch.GetParameters().ToList();
		if (allowFirsParamPassthrough && patch.ReturnType != typeof(void) && list.Count > 0 && list[0].ParameterType == patch.ReturnType)
		{
			list.RemoveRange(0, 1);
		}
		foreach (ParameterInfo item in list)
		{
			if (item.Name == OriginalMethodParam)
			{
				if (!EmitOriginalBaseMethod())
				{
					il.Emit(OpCodes.Ldnull);
				}
				continue;
			}
			if (item.Name == InstanceParam)
			{
				if (original.IsStatic)
				{
					il.Emit(OpCodes.Ldnull);
					continue;
				}
				bool num = original.DeclaringType != null && AccessTools.IsStruct(original.DeclaringType);
				bool isByRef = item.ParameterType.IsByRef;
				if (num == isByRef)
				{
					il.Emit(OpCodes.Ldarg_0);
				}
				if (num && !isByRef)
				{
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Ldobj, original.DeclaringType);
				}
				if (!num && isByRef)
				{
					il.Emit(OpCodes.Ldarga, 0);
				}
				continue;
			}
			if (item.Name == ArgsArrayVar)
			{
				if (variables.TryGetValue(ArgsArrayVar, out var value))
				{
					il.Emit(OpCodes.Ldloc, value);
				}
				else
				{
					il.Emit(OpCodes.Ldnull);
				}
				continue;
			}
			if (item.Name.StartsWith(InstanceFieldPrefix, StringComparison.Ordinal))
			{
				string text = item.Name.Substring(InstanceFieldPrefix.Length);
				FieldInfo fieldInfo;
				if (text.All(char.IsDigit))
				{
					fieldInfo = AccessTools.DeclaredField(original.DeclaringType, int.Parse(text));
					if ((object)fieldInfo == null)
					{
						throw new ArgumentException("No field found at given index in class " + original.DeclaringType.FullName, text);
					}
				}
				else
				{
					fieldInfo = AccessTools.Field(original.DeclaringType, text);
					if ((object)fieldInfo == null)
					{
						throw new ArgumentException("No such field defined in class " + original.DeclaringType.FullName, text);
					}
				}
				if (fieldInfo.IsStatic)
				{
					il.Emit(item.ParameterType.IsByRef ? OpCodes.Ldsflda : OpCodes.Ldsfld, fieldInfo);
					continue;
				}
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(item.ParameterType.IsByRef ? OpCodes.Ldflda : OpCodes.Ldfld, fieldInfo);
				continue;
			}
			if (item.Name == StateVar)
			{
				OpCode opcode = (item.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc);
				if (variables.TryGetValue(patch.DeclaringType.FullName, out var value2))
				{
					il.Emit(opcode, value2);
				}
				else
				{
					il.Emit(OpCodes.Ldnull);
				}
				continue;
			}
			if (item.Name == ResultVar)
			{
				Type returnedType = AccessTools.GetReturnedType(original);
				if (returnedType == typeof(void))
				{
					throw new Exception("Cannot get result from void method " + original.FullDescription());
				}
				Type type = item.ParameterType;
				if (type.IsByRef && !returnedType.IsByRef)
				{
					type = type.GetElementType();
				}
				if (!type.IsAssignableFrom(returnedType))
				{
					throw new Exception("Cannot assign method return type " + returnedType.FullName + " to " + ResultVar + " type " + type.FullName + " for method " + original.FullDescription());
				}
				OpCode opcode2 = ((item.ParameterType.IsByRef && !returnedType.IsByRef) ? OpCodes.Ldloca : OpCodes.Ldloc);
				if (returnedType.IsValueType && item.ParameterType == typeof(object).MakeByRefType())
				{
					opcode2 = OpCodes.Ldloc;
				}
				il.Emit(opcode2, variables[ResultVar]);
				if (returnedType.IsValueType)
				{
					if (item.ParameterType == typeof(object))
					{
						il.Emit(OpCodes.Box, returnedType);
					}
					else if (item.ParameterType == typeof(object).MakeByRefType())
					{
						il.Emit(OpCodes.Box, returnedType);
						tmpObjectVar = il.DeclareVariable(typeof(object));
						il.Emit(OpCodes.Stloc, tmpObjectVar);
						il.Emit(OpCodes.Ldloca, tmpObjectVar);
					}
				}
				continue;
			}
			if (variables.TryGetValue(item.Name, out var value3))
			{
				OpCode opcode3 = (item.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc);
				il.Emit(opcode3, value3);
				continue;
			}
			int result;
			if (item.Name.StartsWith(ParamIndexPrefix, StringComparison.Ordinal))
			{
				if (!int.TryParse(item.Name.Substring(ParamIndexPrefix.Length), out result))
				{
					throw new Exception("Parameter " + item.Name + " does not contain a valid index");
				}
				if (result < 0 || result >= parameters.Length)
				{
					throw new Exception($"No parameter found at index {result}");
				}
			}
			else
			{
				result = patch.GetArgumentIndex(originalParameterNames, item);
				if (result == -1)
				{
					HarmonyMethod mergedFromType = HarmonyMethodExtensions.GetMergedFromType(item.ParameterType);
					MethodType? methodType = mergedFromType.methodType;
					if (!methodType.HasValue)
					{
						mergedFromType.methodType = MethodType.Normal;
					}
					if (mergedFromType.GetOriginalMethod() is MethodInfo methodInfo)
					{
						ConstructorInfo constructor = item.ParameterType.GetConstructor(new Type[2]
						{
							typeof(object),
							typeof(IntPtr)
						});
						if (constructor != null)
						{
							Type declaringType = original.DeclaringType;
							if (methodInfo.IsStatic)
							{
								il.Emit(OpCodes.Ldnull);
							}
							else
							{
								il.Emit(OpCodes.Ldarg_0);
								if (declaringType.IsValueType)
								{
									il.Emit(OpCodes.Ldobj, declaringType);
									il.Emit(OpCodes.Box, declaringType);
								}
							}
							if (!methodInfo.IsStatic && !mergedFromType.nonVirtualDelegate)
							{
								il.Emit(OpCodes.Dup);
								il.Emit(OpCodes.Ldvirtftn, methodInfo);
							}
							else
							{
								il.Emit(OpCodes.Ldftn, methodInfo);
							}
							il.Emit(OpCodes.Newobj, constructor);
							continue;
						}
					}
					throw new Exception("Parameter \"" + item.Name + "\" not found in method " + original.FullDescription());
				}
			}
			Type parameterType = parameters[result].ParameterType;
			Type type2 = (parameterType.IsByRef ? parameterType.GetElementType() : parameterType);
			Type parameterType2 = item.ParameterType;
			Type type3 = (parameterType2.IsByRef ? parameterType2.GetElementType() : parameterType2);
			bool flag2 = !parameters[result].IsOut && !parameterType.IsByRef;
			bool flag3 = !item.IsOut && !parameterType2.IsByRef;
			bool flag4 = type2.IsValueType && !type3.IsValueType;
			int num2 = result + (flag ? 1 : 0);
			if (flag2 == flag3)
			{
				il.Emit(OpCodes.Ldarg, num2);
				if (flag4)
				{
					if (flag3)
					{
						il.Emit(OpCodes.Box, type2);
						continue;
					}
					il.Emit(OpCodes.Ldobj, type2);
					il.Emit(OpCodes.Box, type2);
					VariableDefinition val = il.DeclareVariable(type3);
					il.Emit(OpCodes.Stloc, val);
					il.Emit(OpCodes.Ldloca_S, val);
					tmpBoxVars.Add(new ArgumentBoxInfo
					{
						index = num2,
						type = type2,
						tmpVar = val,
						isByRef = true
					});
				}
			}
			else if (flag2 && !flag3)
			{
				if (flag4)
				{
					il.Emit(OpCodes.Ldarg, num2);
					il.Emit(OpCodes.Box, type2);
					VariableDefinition val2 = il.DeclareVariable(type3);
					il.Emit(OpCodes.Stloc, val2);
					il.Emit(OpCodes.Ldloca_S, val2);
					tmpBoxVars.Add(new ArgumentBoxInfo
					{
						index = num2,
						type = type2,
						tmpVar = val2,
						isByRef = false
					});
				}
				else
				{
					il.Emit(OpCodes.Ldarga, num2);
				}
			}
			else
			{
				il.Emit(OpCodes.Ldarg, num2);
				if (flag4)
				{
					il.Emit(OpCodes.Ldobj, type2);
					il.Emit(OpCodes.Box, type2);
				}
				else if (type2.IsValueType)
				{
					il.Emit(OpCodes.Ldobj, type2);
				}
				else
				{
					il.Emit(parameters[result].ParameterType.GetCecilLoadOpCode());
				}
			}
		}
	}
}
