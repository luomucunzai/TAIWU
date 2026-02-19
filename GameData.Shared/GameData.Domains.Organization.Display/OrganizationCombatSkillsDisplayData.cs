using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Organization.Display;

public class OrganizationCombatSkillsDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte OrganizationTemplateId;

	[SerializableGameDataField]
	public short ApprovingRate;

	[SerializableGameDataField]
	public short ApprovingRateTotal;

	[SerializableGameDataField]
	public short ApprovingRateUpperLimit;

	[SerializableGameDataField]
	public short ApprovingRateUpperLimitBonus;

	[SerializableGameDataField]
	public List<CombatSkillDisplayData> LearnedSkills;

	public OrganizationCombatSkillsDisplayData()
	{
	}

	public OrganizationCombatSkillsDisplayData(OrganizationCombatSkillsDisplayData other)
	{
		OrganizationTemplateId = other.OrganizationTemplateId;
		ApprovingRate = other.ApprovingRate;
		ApprovingRateTotal = other.ApprovingRateTotal;
		ApprovingRateUpperLimit = other.ApprovingRateUpperLimit;
		ApprovingRateUpperLimitBonus = other.ApprovingRateUpperLimitBonus;
		List<CombatSkillDisplayData> learnedSkills = other.LearnedSkills;
		int count = learnedSkills.Count;
		LearnedSkills = new List<CombatSkillDisplayData>(count);
		for (int i = 0; i < count; i++)
		{
			LearnedSkills.Add(new CombatSkillDisplayData(learnedSkills[i]));
		}
	}

	public void Assign(OrganizationCombatSkillsDisplayData other)
	{
		OrganizationTemplateId = other.OrganizationTemplateId;
		ApprovingRate = other.ApprovingRate;
		ApprovingRateTotal = other.ApprovingRateTotal;
		ApprovingRateUpperLimit = other.ApprovingRateUpperLimit;
		ApprovingRateUpperLimitBonus = other.ApprovingRateUpperLimitBonus;
		List<CombatSkillDisplayData> learnedSkills = other.LearnedSkills;
		int count = learnedSkills.Count;
		LearnedSkills = new List<CombatSkillDisplayData>(count);
		for (int i = 0; i < count; i++)
		{
			LearnedSkills.Add(new CombatSkillDisplayData(learnedSkills[i]));
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 9;
		if (LearnedSkills != null)
		{
			num += 2;
			int count = LearnedSkills.Count;
			for (int i = 0; i < count; i++)
			{
				CombatSkillDisplayData combatSkillDisplayData = LearnedSkills[i];
				num = ((combatSkillDisplayData == null) ? (num + 2) : (num + (2 + combatSkillDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)OrganizationTemplateId;
		ptr++;
		*(short*)ptr = ApprovingRate;
		ptr += 2;
		*(short*)ptr = ApprovingRateTotal;
		ptr += 2;
		*(short*)ptr = ApprovingRateUpperLimit;
		ptr += 2;
		*(short*)ptr = ApprovingRateUpperLimitBonus;
		ptr += 2;
		if (LearnedSkills != null)
		{
			int count = LearnedSkills.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				CombatSkillDisplayData combatSkillDisplayData = LearnedSkills[i];
				if (combatSkillDisplayData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = combatSkillDisplayData.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)intPtr = (ushort)num;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		OrganizationTemplateId = (sbyte)(*ptr);
		ptr++;
		ApprovingRate = *(short*)ptr;
		ptr += 2;
		ApprovingRateTotal = *(short*)ptr;
		ptr += 2;
		ApprovingRateUpperLimit = *(short*)ptr;
		ptr += 2;
		ApprovingRateUpperLimitBonus = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (LearnedSkills == null)
			{
				LearnedSkills = new List<CombatSkillDisplayData>(num);
			}
			else
			{
				LearnedSkills.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					CombatSkillDisplayData combatSkillDisplayData = new CombatSkillDisplayData();
					ptr += combatSkillDisplayData.Deserialize(ptr);
					LearnedSkills.Add(combatSkillDisplayData);
				}
				else
				{
					LearnedSkills.Add(null);
				}
			}
		}
		else
		{
			LearnedSkills?.Clear();
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
