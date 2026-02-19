using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Status;

public struct Book : ISerializableGameData
{
	public int BasePoint;

	public LifeSkillItem LifeSkill;

	public int RemainingCd;

	public int CoveringCd;

	public int DisplayCd => (IsDisplayCd != IsCd) ? CoveringCd : RemainingCd;

	public bool IsCd => RemainingCd > 0;

	public bool IsDisplayCd
	{
		get
		{
			if (IsCd && CoveringCd == 0)
			{
				return false;
			}
			if (!IsCd && CoveringCd > 0)
			{
				return true;
			}
			return IsCd;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += 4;
		num += LifeSkill.GetSerializedSize();
		num += 4;
		num += 4;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = BasePoint;
		ptr += 4;
		ptr += LifeSkill.Serialize(ptr);
		*(int*)ptr = RemainingCd;
		ptr += 4;
		*(int*)ptr = CoveringCd;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		BasePoint = *(int*)ptr;
		ptr += 4;
		ptr += LifeSkill.Deserialize(ptr);
		RemainingCd = *(int*)ptr;
		ptr += 4;
		CoveringCd = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public Book(LifeSkillItem lifeSkill)
	{
		LifeSkill = lifeSkill;
		BasePoint = 0;
		RemainingCd = 0;
		CoveringCd = -1;
	}
}
