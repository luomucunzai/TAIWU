using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Combat;

[AutoGenerateSerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public struct DamageStepDisplayData : ISerializableGameData
{
	public static readonly DamageStepDisplayData Invalid = new DamageStepDisplayData
	{
		ActivateSkillTemplateId = -1
	};

	[SerializableGameDataField]
	public short ActivateSkillTemplateId;

	[SerializableGameDataField]
	public CombatSkillDamageStepBonusDisplayData ActivateSkillBonusData;

	[SerializableGameDataField]
	public int EatingBonusData;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 22;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = ActivateSkillTemplateId;
		ptr += 2;
		ptr += ActivateSkillBonusData.Serialize(ptr);
		*(int*)ptr = EatingBonusData;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ActivateSkillTemplateId = *(short*)ptr;
		ptr += 2;
		ptr += ActivateSkillBonusData.Deserialize(ptr);
		EatingBonusData = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
