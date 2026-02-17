using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000722 RID: 1826
	public static class AiNodeFactory
	{
		// Token: 0x060068A7 RID: 26791 RVA: 0x003B7884 File Offset: 0x003B5A84
		public static void Register(Assembly assembly)
		{
			int count = assembly.GetTypes().Sum((Type type) => AiNodeFactory.TryRegister(type) ? 1 : 0);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
			defaultInterpolatedStringHandler.AppendLiteral("AiNodeFactory.Register on ");
			defaultInterpolatedStringHandler.AppendFormatted(assembly.FullName);
			defaultInterpolatedStringHandler.AppendLiteral(" added ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(count);
			defaultInterpolatedStringHandler.AppendLiteral(" types");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060068A8 RID: 26792 RVA: 0x003B7914 File Offset: 0x003B5B14
		public static bool TryRegister(Type type)
		{
			bool flag = type.GetInterfaces().All((Type x) => x != typeof(IAiNode));
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Attribute customAttribute = type.GetCustomAttribute(typeof(AiNodeAttribute));
				AiNodeAttribute attribute = customAttribute as AiNodeAttribute;
				result = (attribute != null && AiNodeFactory.Mapping.TryAdd(attribute.Type, type));
			}
			return result;
		}

		// Token: 0x060068A9 RID: 26793 RVA: 0x003B7988 File Offset: 0x003B5B88
		public static IAiNode Create(EAiNodeType type, int runtimeId, IReadOnlyList<int> nodeOrActionIds)
		{
			IAiNode result = null;
			Type conditionType;
			bool flag = AiNodeFactory.Mapping.TryGetValue(type, out conditionType);
			if (flag)
			{
				result = (IAiNode)Activator.CreateInstance(conditionType, new object[]
				{
					nodeOrActionIds
				});
			}
			bool flag2 = result != null;
			if (flag2)
			{
				result.RuntimeId = runtimeId;
			}
			else
			{
				short predefinedLogId = 8;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot analysis node ");
				defaultInterpolatedStringHandler.AppendFormatted<EAiNodeType>(type);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(runtimeId);
				PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return result;
		}

		// Token: 0x04001CB2 RID: 7346
		private static readonly Dictionary<EAiNodeType, Type> Mapping = new Dictionary<EAiNodeType, Type>();
	}
}
