using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai;

public static class AiNodeFactory
{
	private static readonly Dictionary<EAiNodeType, Type> Mapping = new Dictionary<EAiNodeType, Type>();

	public static void Register(Assembly assembly)
	{
		int value = assembly.GetTypes().Sum((Type type) => TryRegister(type) ? 1 : 0);
		AdaptableLog.Info($"AiNodeFactory.Register on {assembly.FullName} added {value} types");
	}

	public static bool TryRegister(Type type)
	{
		if (type.GetInterfaces().All((Type x) => x != typeof(IAiNode)))
		{
			return false;
		}
		Attribute customAttribute = type.GetCustomAttribute(typeof(AiNodeAttribute));
		return customAttribute is AiNodeAttribute aiNodeAttribute && Mapping.TryAdd(aiNodeAttribute.Type, type);
	}

	public static IAiNode Create(EAiNodeType type, int runtimeId, IReadOnlyList<int> nodeOrActionIds)
	{
		IAiNode aiNode = null;
		if (Mapping.TryGetValue(type, out var value))
		{
			aiNode = (IAiNode)Activator.CreateInstance(value, nodeOrActionIds);
		}
		if (aiNode != null)
		{
			aiNode.RuntimeId = runtimeId;
		}
		else
		{
			PredefinedLog.Show(8, $"Cannot analysis node {type} {runtimeId}");
		}
		return aiNode;
	}
}
