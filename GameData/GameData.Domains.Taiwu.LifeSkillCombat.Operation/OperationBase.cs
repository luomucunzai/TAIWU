using System;
using System.Collections.Generic;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

[SerializableGameData(NotForDisplayModule = true)]
public abstract class OperationBase : ISerializableGameData
{
	protected int Stamp;

	private static IDictionary<Type, byte> _typeIdStableMap;

	public sbyte PlayerId { get; protected set; }

	public OperationBase()
	{
	}

	public abstract string Inspect();

	public override string ToString()
	{
		return $"{GetType().Name} {Inspect()} by {Player.PredefinedId.GetName(PlayerId)}";
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public virtual int GetSerializedSize()
	{
		return 5;
	}

	public unsafe virtual int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)PlayerId;
		ptr++;
		*(int*)ptr = Stamp;
		ptr += 4;
		return (int)(ptr - pData);
	}

	public unsafe virtual int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		PlayerId = (sbyte)(*ptr);
		ptr++;
		Stamp = *(int*)ptr;
		ptr += 4;
		return (int)(ptr - pData);
	}

	private static IDictionary<Type, byte> CheckTypeIdMap()
	{
		if (_typeIdStableMap == null)
		{
			List<Type> list = new List<Type>();
			Type[] types = typeof(OperationBase).Assembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeof(OperationBase).IsAssignableFrom(type))
				{
					list.Add(type);
				}
			}
			list.Sort((Type a, Type b) => string.Compare(a.AssemblyQualifiedName, b.AssemblyQualifiedName, StringComparison.Ordinal));
			_typeIdStableMap = new Dictionary<Type, byte>();
			int num = 0;
			for (int count = list.Count; num < count; num++)
			{
				Tester.Assert(num < 255);
				_typeIdStableMap.Add(list[num], (byte)num);
			}
		}
		return _typeIdStableMap;
	}

	public static int GetSerializeSizeWithPolymorphism(OperationBase operation)
	{
		int num = 0;
		num++;
		return num + operation.GetSerializedSize();
	}

	public unsafe static int SerializeWithPolymorphism(OperationBase operation, byte* pData)
	{
		byte* ptr = pData;
		IDictionary<Type, byte> dictionary = CheckTypeIdMap();
		*ptr = dictionary[operation.GetType()];
		ptr++;
		ptr += operation.Serialize(ptr);
		return (int)(ptr - pData);
	}

	public unsafe static int DeserializeWithPolymorphism(ref OperationBase operation, byte* pData)
	{
		byte* ptr = pData;
		IDictionary<Type, byte> dictionary = CheckTypeIdMap();
		byte b = *ptr;
		ptr++;
		operation = null;
		foreach (KeyValuePair<Type, byte> item in dictionary)
		{
			if (item.Value == b)
			{
				operation = Activator.CreateInstance(item.Key) as OperationBase;
				break;
			}
		}
		Tester.Assert(operation != null);
		ptr += operation.Deserialize(ptr);
		return (int)(ptr - pData);
	}

	unsafe static OperationBase()
	{
		SerializerHolder<OperationBase>.SerializeFunc = delegate(OperationBase item, RawDataPool pool)
		{
			byte* pData = default(byte*);
			int result = pool.Allocate(GetSerializeSizeWithPolymorphism(item), &pData);
			SerializeWithPolymorphism(item, pData);
			return result;
		};
		SerializerHolder<OperationBase>.DeserializeFunc = delegate(RawDataPool pool, int offset, ref OperationBase item)
		{
			return DeserializeWithPolymorphism(ref item, pool.GetPointer(offset));
		};
	}

	protected OperationBase(sbyte playerId, int stamp)
	{
		PlayerId = playerId;
		Stamp = stamp;
	}

	public int GetStamp()
	{
		return Stamp;
	}
}
