using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x0200071D RID: 1821
	public static class AiFactory
	{
		// Token: 0x06006899 RID: 26777 RVA: 0x003B75BC File Offset: 0x003B57BC
		public static T CreateInstance<T>(Type type, IReadOnlyList<string> strings, IReadOnlyList<int> ints) where T : class
		{
			ConstructorInfo[] constructors = type.GetConstructors();
			bool flag = constructors.Any(new Func<ConstructorInfo, bool>(AiFactory.IsStandard));
			T result;
			if (flag)
			{
				result = (T)((object)Activator.CreateInstance(type, new object[]
				{
					strings,
					ints
				}));
			}
			else
			{
				bool flag2 = constructors.Any(new Func<ConstructorInfo, bool>(AiFactory.IsOnlyStrings));
				if (flag2)
				{
					result = (T)((object)Activator.CreateInstance(type, new object[]
					{
						strings
					}));
				}
				else
				{
					bool flag3 = constructors.Any(new Func<ConstructorInfo, bool>(AiFactory.IsOnlyInts));
					if (flag3)
					{
						result = (T)((object)Activator.CreateInstance(type, new object[]
						{
							ints
						}));
					}
					else
					{
						bool flag4 = constructors.Any(new Func<ConstructorInfo, bool>(AiFactory.IsEmpty));
						if (flag4)
						{
							result = (T)((object)Activator.CreateInstance(type));
						}
						else
						{
							result = default(T);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600689A RID: 26778 RVA: 0x003B7698 File Offset: 0x003B5898
		public static bool IsStandard(ConstructorInfo constructor)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			bool flag = parameters == null || parameters.Length != 2;
			return !flag && parameters[0].ParameterType == typeof(IReadOnlyList<string>) && parameters[1].ParameterType == typeof(IReadOnlyList<int>);
		}

		// Token: 0x0600689B RID: 26779 RVA: 0x003B76FC File Offset: 0x003B58FC
		public static bool IsOnlyStrings(ConstructorInfo constructor)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			bool flag = parameters == null || parameters.Length != 1;
			return !flag && parameters[0].ParameterType == typeof(IReadOnlyList<string>);
		}

		// Token: 0x0600689C RID: 26780 RVA: 0x003B7744 File Offset: 0x003B5944
		public static bool IsOnlyInts(ConstructorInfo constructor)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			bool flag = parameters == null || parameters.Length != 1;
			return !flag && parameters[0].ParameterType == typeof(IReadOnlyList<int>);
		}

		// Token: 0x0600689D RID: 26781 RVA: 0x003B778C File Offset: 0x003B598C
		public static bool IsEmpty(ConstructorInfo constructor)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			return parameters == null || parameters.Length <= 0;
		}
	}
}
