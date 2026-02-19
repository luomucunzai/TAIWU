using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

[SerializableGameData(NotForDisplayModule = true)]
public class OperationList : List<OperationBase>, ISerializableGameData
{
	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += 2;
		int i = 0;
		for (int count = base.Count; i < count; i++)
		{
			num += OperationBase.GetSerializeSizeWithPolymorphism(base[i]);
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		Tester.Assert(base.Count <= 65535);
		*(ushort*)ptr = (ushort)base.Count;
		ptr += 2;
		int i = 0;
		for (int count = base.Count; i < count; i++)
		{
			ptr += OperationBase.SerializeWithPolymorphism(base[i], ptr);
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		Clear();
		int i = 0;
		for (int num2 = num; i < num2; i++)
		{
			OperationBase operation = null;
			ptr += OperationBase.DeserializeWithPolymorphism(ref operation, ptr);
			Add(operation);
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
