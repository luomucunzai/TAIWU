using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000714 RID: 1812
	public static class AiConditionFactory
	{
		// Token: 0x06006855 RID: 26709 RVA: 0x003B4A58 File Offset: 0x003B2C58
		public static void Register(Assembly assembly)
		{
			int count = assembly.GetTypes().Sum((Type type) => AiConditionFactory.TryRegister(type) ? 1 : 0);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
			defaultInterpolatedStringHandler.AppendLiteral("AiConditionFactory.Register on ");
			defaultInterpolatedStringHandler.AppendFormatted(assembly.FullName);
			defaultInterpolatedStringHandler.AppendLiteral(" added ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(count);
			defaultInterpolatedStringHandler.AppendLiteral(" types");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06006856 RID: 26710 RVA: 0x003B4AE8 File Offset: 0x003B2CE8
		public static bool TryRegister(Type type)
		{
			bool flag = type.GetInterfaces().All((Type x) => x != typeof(IAiCondition));
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Attribute customAttribute = type.GetCustomAttribute(typeof(AiConditionAttribute));
				AiConditionAttribute attribute = customAttribute as AiConditionAttribute;
				result = (attribute != null && AiConditionFactory.Mapping.TryAdd(attribute.Type, type));
			}
			return result;
		}

		// Token: 0x06006857 RID: 26711 RVA: 0x003B4B5C File Offset: 0x003B2D5C
		public static IAiCondition Create(EAiConditionType type, int runtimeId, IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			IAiCondition result = null;
			Type conditionType;
			bool flag = AiConditionFactory.Mapping.TryGetValue(type, out conditionType);
			if (flag)
			{
				result = AiFactory.CreateInstance<IAiCondition>(conditionType, strings, ints);
			}
			bool flag2 = result != null;
			if (flag2)
			{
				result.RuntimeId = runtimeId;
			}
			else
			{
				short predefinedLogId = 8;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot analysis condition ");
				defaultInterpolatedStringHandler.AppendFormatted<EAiConditionType>(type);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(runtimeId);
				PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return result;
		}

		// Token: 0x04001C87 RID: 7303
		private static readonly Dictionary<EAiConditionType, Type> Mapping = new Dictionary<EAiConditionType, Type>();
	}
}
