using System;
using Config;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class ObsoleteProfessionData : ISerializableGameData
{
	[SerializableGameDataField]
	public int TemplateId;

	[SerializableGameDataField]
	public int Seniority;

	[SerializableGameDataField]
	public int ProfessionOffCooldownDate;

	[SerializableGameDataField]
	public int[] SkillOffCooldownDates;

	[SerializableGameDataField]
	public bool[] HadBeenUnlocked;

	[SerializableGameDataField]
	public IProfessionSkillsData SkillsData;

	public int GetSkillCount()
	{
		ProfessionItem professionItem = Config.Profession.Instance[TemplateId];
		int num = professionItem.ProfessionSkills.Length;
		if (professionItem.ExtraProfessionSkill >= 0)
		{
			num++;
		}
		return num;
	}

	public ObsoleteProfessionData(int templateId)
	{
		TemplateId = templateId;
		int skillCount = GetSkillCount();
		SkillOffCooldownDates = new int[skillCount];
		HadBeenUnlocked = new bool[skillCount];
		SkillsData = CreateExtraData(TemplateId);
	}

	public ProfessionItem GetConfig()
	{
		return Config.Profession.Instance[TemplateId];
	}

	public ProfessionSkillItem GetSkillConfig(int index)
	{
		ProfessionItem config = GetConfig();
		if (index < config.ProfessionSkills.Length)
		{
			return ProfessionSkill.Instance[config.ProfessionSkills[index]];
		}
		return ProfessionSkill.Instance[config.ExtraProfessionSkill];
	}

	public int GetSkillIndex(int skillId)
	{
		ProfessionItem config = GetConfig();
		int num = config.ProfessionSkills.IndexOf(skillId);
		if (num > -1)
		{
			return num;
		}
		if (skillId == config.ExtraProfessionSkill)
		{
			return config.ProfessionSkills.Length;
		}
		return -1;
	}

	public bool IsSkillUnlocked(int skillIndex)
	{
		return Seniority >= ProfessionRelatedConstants.SkillUnlockSeniority[skillIndex];
	}

	public T GetSkillsData<T>() where T : IProfessionSkillsData
	{
		return (T)SkillsData;
	}

	private static IProfessionSkillsData CreateExtraData(int templateId)
	{
		return templateId switch
		{
			1 => new ObsoleteHunterSkillsData(), 
			5 => new ObsoleteTaoistMonkSkillsData(), 
			6 => new ObsoleteBuddhistMonkSkillsData(), 
			7 => new WineTasterSkillsData(), 
			9 => new ObsoleteBeggarSkillsData(), 
			12 => new ObsoleteTravelingBuddhistMonkSkillsData(), 
			17 => new ObsoleteDukeSkillsData(), 
			8 => new ObsoleteAristocratSkillsData(), 
			14 => new ObsoleteTravelingTaoistMonkSkillsData(), 
			16 => new TeaTasterSkillsData(), 
			_ => null, 
		};
	}

	public ObsoleteProfessionData()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		num = ((SkillOffCooldownDates == null) ? (num + 2) : (num + (2 + 4 * SkillOffCooldownDates.Length)));
		num = ((HadBeenUnlocked == null) ? (num + 2) : (num + (2 + HadBeenUnlocked.Length)));
		num = ((SkillsData == null) ? (num + 2) : (num + (2 + ((ISerializableGameData)SkillsData).GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = TemplateId;
		ptr += 4;
		*(int*)ptr = Seniority;
		ptr += 4;
		*(int*)ptr = ProfessionOffCooldownDate;
		ptr += 4;
		if (SkillOffCooldownDates != null)
		{
			int num = SkillOffCooldownDates.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				((int*)ptr)[i] = SkillOffCooldownDates[i];
			}
			ptr += 4 * num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (HadBeenUnlocked != null)
		{
			int num2 = HadBeenUnlocked.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int j = 0; j < num2; j++)
			{
				ptr[j] = (HadBeenUnlocked[j] ? ((byte)1) : ((byte)0));
			}
			ptr += num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SkillsData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num3 = ((ISerializableGameData)SkillsData).Serialize(ptr);
			ptr += num3;
			Tester.Assert(num3 <= 65535);
			*(ushort*)intPtr = (ushort)num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TemplateId = *(int*)ptr;
		ptr += 4;
		Seniority = *(int*)ptr;
		ptr += 4;
		ProfessionOffCooldownDate = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (SkillOffCooldownDates == null || SkillOffCooldownDates.Length != num)
			{
				SkillOffCooldownDates = new int[num];
			}
			for (int i = 0; i < num; i++)
			{
				SkillOffCooldownDates[i] = ((int*)ptr)[i];
			}
			ptr += 4 * num;
		}
		else
		{
			SkillOffCooldownDates = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (HadBeenUnlocked == null || HadBeenUnlocked.Length != num2)
			{
				HadBeenUnlocked = new bool[num2];
			}
			for (int j = 0; j < num2; j++)
			{
				HadBeenUnlocked[j] = ptr[j] != 0;
			}
			ptr += (int)num2;
		}
		else
		{
			HadBeenUnlocked = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (SkillsData == null)
			{
				SkillsData = CreateExtraData(TemplateId);
			}
			ptr += ((ISerializableGameData)SkillsData).Deserialize(ptr);
		}
		else
		{
			SkillsData = CreateExtraData(TemplateId);
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
