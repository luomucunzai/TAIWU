using System;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib.Internal.Util;

internal class ReflectionTools
{
	internal struct TypeAndName
	{
		internal Type type;

		internal string name;
	}

	internal static TypeAndName TypColonName(string typeColonName)
	{
		if (typeColonName == null)
		{
			throw new ArgumentNullException("typeColonName");
		}
		string[] array = typeColonName.Split(new char[1] { ':' });
		if (array.Length != 2)
		{
			throw new ArgumentException(" must be specified as 'Namespace.Type1.Type2:MemberName", "typeColonName");
		}
		return new TypeAndName
		{
			type = AccessTools.TypeByName(array[0]),
			name = array[1]
		};
	}

	internal static void ValidateFieldType<F>(FieldInfo fieldInfo)
	{
		Type typeFromHandle = typeof(F);
		Type fieldType = fieldInfo.FieldType;
		if (typeFromHandle == fieldType)
		{
			return;
		}
		if (fieldType.IsEnum)
		{
			Type underlyingType = Enum.GetUnderlyingType(fieldType);
			if (!(typeFromHandle != underlyingType))
			{
				return;
			}
			throw new ArgumentException("FieldRefAccess return type must be the same as FieldType or " + $"FieldType's underlying integral type ({underlyingType}) for enum types");
		}
		if (fieldType.IsValueType)
		{
			throw new ArgumentException("FieldRefAccess return type must be the same as FieldType for value types");
		}
		if (typeFromHandle.IsAssignableFrom(fieldType))
		{
			return;
		}
		throw new ArgumentException("FieldRefAccess return type must be assignable from FieldType for reference types");
	}

	internal static AccessTools.FieldRef<T, F> FieldRefAccess<T, F>(FieldInfo fieldInfo, bool needCastclass)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		ValidateFieldType<F>(fieldInfo);
		Type typeFromHandle = typeof(T);
		Type declaringType = fieldInfo.DeclaringType;
		DynamicMethodDefinition val = new DynamicMethodDefinition("__refget_" + typeFromHandle.Name + "_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[1] { typeFromHandle });
		ILGenerator iLGenerator = val.GetILGenerator();
		if (fieldInfo.IsStatic)
		{
			iLGenerator.Emit(OpCodes.Ldsflda, fieldInfo);
		}
		else
		{
			iLGenerator.Emit(OpCodes.Ldarg_0);
			if (needCastclass)
			{
				iLGenerator.Emit(OpCodes.Castclass, declaringType);
			}
			iLGenerator.Emit(OpCodes.Ldflda, fieldInfo);
		}
		iLGenerator.Emit(OpCodes.Ret);
		return (AccessTools.FieldRef<T, F>)val.Generate().CreateDelegate(typeof(AccessTools.FieldRef<T, F>));
	}

	internal static AccessTools.StructFieldRef<T, F> StructFieldRefAccess<T, F>(FieldInfo fieldInfo) where T : struct
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		ValidateFieldType<F>(fieldInfo);
		DynamicMethodDefinition val = new DynamicMethodDefinition("__refget_" + typeof(T).Name + "_struct_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[1] { typeof(T).MakeByRefType() });
		ILGenerator iLGenerator = val.GetILGenerator();
		iLGenerator.Emit(OpCodes.Ldarg_0);
		iLGenerator.Emit(OpCodes.Ldflda, fieldInfo);
		iLGenerator.Emit(OpCodes.Ret);
		return (AccessTools.StructFieldRef<T, F>)val.Generate().CreateDelegate(typeof(AccessTools.StructFieldRef<T, F>));
	}

	internal static AccessTools.FieldRef<F> StaticFieldRefAccess<F>(FieldInfo fieldInfo)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (!fieldInfo.IsStatic)
		{
			throw new ArgumentException("Field must be static");
		}
		ValidateFieldType<F>(fieldInfo);
		DynamicMethodDefinition val = new DynamicMethodDefinition("__refget_" + (fieldInfo.DeclaringType?.Name ?? "null") + "_static_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[0]);
		ILGenerator iLGenerator = val.GetILGenerator();
		iLGenerator.Emit(OpCodes.Ldsflda, fieldInfo);
		iLGenerator.Emit(OpCodes.Ret);
		return (AccessTools.FieldRef<F>)val.Generate().CreateDelegate(typeof(AccessTools.FieldRef<F>));
	}

	internal static FieldInfo GetInstanceField(Type type, string fieldName)
	{
		FieldInfo obj = AccessTools.Field(type, fieldName) ?? throw new MissingFieldException(type.Name, fieldName);
		if (obj.IsStatic)
		{
			throw new ArgumentException("Field must not be static");
		}
		return obj;
	}

	internal static bool FieldRefNeedsClasscast(Type delegateInstanceType, Type declaringType)
	{
		bool flag = false;
		if (delegateInstanceType != declaringType)
		{
			flag = delegateInstanceType.IsAssignableFrom(declaringType);
			if (!flag && !declaringType.IsAssignableFrom(delegateInstanceType))
			{
				throw new ArgumentException("FieldDeclaringType must be assignable from or to T (FieldRefAccess instance type) - \"instanceOfT is FieldDeclaringType\" must be possible");
			}
		}
		return flag;
	}

	internal static void ValidateStructField<T, F>(FieldInfo fieldInfo) where T : struct
	{
		if (fieldInfo.IsStatic)
		{
			throw new ArgumentException("Field must not be static");
		}
		if (fieldInfo.DeclaringType != typeof(T))
		{
			throw new ArgumentException("FieldDeclaringType must be T (StructFieldRefAccess instance type)");
		}
	}
}
