using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai;

public static class AiActionFactory
{
	private static readonly Dictionary<EAiActionType, Type> Mapping = new Dictionary<EAiActionType, Type>();

	public static void Register(Assembly assembly)
	{
		int value = assembly.GetTypes().Sum((Type type) => TryRegister(type) ? 1 : 0);
		AdaptableLog.Info($"AiActionFactory.Register on {assembly.FullName} added {value} types");
	}

	public static bool TryRegister(Type type)
	{
		if (type.GetInterfaces().All((Type x) => x != typeof(IAiAction)))
		{
			return false;
		}
		Attribute customAttribute = type.GetCustomAttribute(typeof(AiActionAttribute));
		return customAttribute is AiActionAttribute aiActionAttribute && Mapping.TryAdd(aiActionAttribute.Type, type);
	}

	public static IAiAction Create(EAiActionType type, int runtimeId, IReadOnlyList<string> strings, IReadOnlyList<int> ints)
	{
		IAiAction aiAction = null;
		if (Mapping.TryGetValue(type, out var value))
		{
			aiAction = AiFactory.CreateInstance<IAiAction>(value, strings, ints);
		}
		if (aiAction != null)
		{
			aiAction.RuntimeId = runtimeId;
		}
		else
		{
			PredefinedLog.Show(8, $"Cannot analysis action {type} {runtimeId}");
		}
		return aiAction;
	}
}
