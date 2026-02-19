using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Combat;

[AutoGenerateSerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public struct CombatSkillDamageStepBonusDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int OuterInjuryStepBonus;

	[SerializableGameDataField]
	public int InnerInjuryStepBonus;

	[SerializableGameDataField]
	public int FatalStepBonus;

	[SerializableGameDataField]
	public int MindStepBonus;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 16;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = OuterInjuryStepBonus;
		byte* num = pData + 4;
		*(int*)num = InnerInjuryStepBonus;
		byte* num2 = num + 4;
		*(int*)num2 = FatalStepBonus;
		byte* num3 = num2 + 4;
		*(int*)num3 = MindStepBonus;
		int num4 = (int)(num3 + 4 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		OuterInjuryStepBonus = *(int*)ptr;
		ptr += 4;
		InnerInjuryStepBonus = *(int*)ptr;
		ptr += 4;
		FatalStepBonus = *(int*)ptr;
		ptr += 4;
		MindStepBonus = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
