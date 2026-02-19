using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai;

public static class AiConditionFactory
{
	private static readonly Dictionary<EAiConditionType, Type> Mapping = new Dictionary<EAiConditionType, Type>();

	public static void Register(Assembly assembly)
	{
		int value = assembly.GetTypes().Sum((Type type) => TryRegister(type) ? 1 : 0);
		AdaptableLog.Info($"AiConditionFactory.Register on {assembly.FullName} added {value} types");
	}

	public static bool TryRegister(Type type)
	{
		if (type.GetInterfaces().All((Type x) => x != typeof(IAiCondition)))
		{
			return false;
		}
		Attribute customAttribute = type.GetCustomAttribute(typeof(AiConditionAttribute));
		return customAttribute is AiConditionAttribute aiConditionAttribute && Mapping.TryAdd(aiConditionAttribute.Type, type);
	}

	public static IAiCondition Create(EAiConditionType type, int runtimeId, IReadOnlyList<string> strings, IReadOnlyList<int> ints)
	{
		IAiCondition aiCondition = null;
		if (Mapping.TryGetValue(type, out var value))
		{
			aiCondition = AiFactory.CreateInstance<IAiCondition>(value, strings, ints);
		}
		if (aiCondition != null)
		{
			aiCondition.RuntimeId = runtimeId;
		}
		else
		{
			PredefinedLog.Show(8, $"Cannot analysis condition {type} {runtimeId}");
		}
		return aiCondition;
	}
}
