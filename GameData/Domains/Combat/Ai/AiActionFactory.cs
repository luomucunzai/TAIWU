using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000711 RID: 1809
	public static class AiActionFactory
	{
		// Token: 0x0600684C RID: 26700 RVA: 0x003B48B0 File Offset: 0x003B2AB0
		public static void Register(Assembly assembly)
		{
			int count = assembly.GetTypes().Sum((Type type) => AiActionFactory.TryRegister(type) ? 1 : 0);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 2);
			defaultInterpolatedStringHandler.AppendLiteral("AiActionFactory.Register on ");
			defaultInterpolatedStringHandler.AppendFormatted(assembly.FullName);
			defaultInterpolatedStringHandler.AppendLiteral(" added ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(count);
			defaultInterpolatedStringHandler.AppendLiteral(" types");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600684D RID: 26701 RVA: 0x003B4940 File Offset: 0x003B2B40
		public static bool TryRegister(Type type)
		{
			bool flag = type.GetInterfaces().All((Type x) => x != typeof(IAiAction));
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Attribute customAttribute = type.GetCustomAttribute(typeof(AiActionAttribute));
				AiActionAttribute attribute = customAttribute as AiActionAttribute;
				result = (attribute != null && AiActionFactory.Mapping.TryAdd(attribute.Type, type));
			}
			return result;
		}

		// Token: 0x0600684E RID: 26702 RVA: 0x003B49B4 File Offset: 0x003B2BB4
		public static IAiAction Create(EAiActionType type, int runtimeId, IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			IAiAction result = null;
			Type conditionType;
			bool flag = AiActionFactory.Mapping.TryGetValue(type, out conditionType);
			if (flag)
			{
				result = AiFactory.CreateInstance<IAiAction>(conditionType, strings, ints);
			}
			bool flag2 = result != null;
			if (flag2)
			{
				result.RuntimeId = runtimeId;
			}
			else
			{
				short predefinedLogId = 8;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot analysis action ");
				defaultInterpolatedStringHandler.AppendFormatted<EAiActionType>(type);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(runtimeId);
				PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return result;
		}

		// Token: 0x04001C85 RID: 7301
		private static readonly Dictionary<EAiActionType, Type> Mapping = new Dictionary<EAiActionType, Type>();
	}
}
