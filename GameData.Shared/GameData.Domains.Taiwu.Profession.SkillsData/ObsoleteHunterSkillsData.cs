using System;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class ObsoleteHunterSkillsData : IProfessionSkillsData, ISerializableGameData
{
	public const sbyte CarrierAnimalAttackCountPerMonth = 3;

	[SerializableGameDataField]
	public sbyte UsedCarrierAnimalAttackCount;

	public sbyte RemainCount => (sbyte)MathUtils.Clamp(3 - UsedCarrierAnimalAttackCount, 0, 3);

	public void Initialize()
	{
		UsedCarrierAnimalAttackCount = 0;
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
	}

	public ObsoleteHunterSkillsData()
	{
		UsedCarrierAnimalAttackCount = 0;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 1;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)UsedCarrierAnimalAttackCount;
		int num = (int)(pData + 1 - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		UsedCarrierAnimalAttackCount = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
