using System.Collections.Generic;
using GameData.Domains.Taiwu;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Extra;

[SerializableGameData(NotForArchive = true)]
public class SectStoryBonusDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public short CombatSkillId;

	[SerializableGameDataField]
	public List<short> BreakBonusTemplateIds;

	[SerializableGameDataField]
	public SkillBreakBonusCollection NormalBonus;

	[SerializableGameDataField]
	public SkillBreakBonusCollection ExtraBonus;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((BreakBonusTemplateIds == null) ? (num + 2) : (num + (2 + 2 * BreakBonusTemplateIds.Count)));
		num = ((NormalBonus == null) ? (num + 2) : (num + (2 + NormalBonus.GetSerializedSize())));
		num = ((ExtraBonus == null) ? (num + 2) : (num + (2 + ExtraBonus.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = CombatSkillId;
		ptr += 2;
		if (BreakBonusTemplateIds != null)
		{
			int count = BreakBonusTemplateIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = BreakBonusTemplateIds[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (NormalBonus != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = NormalBonus.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ExtraBonus != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = ExtraBonus.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CombatSkillId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (BreakBonusTemplateIds == null)
			{
				BreakBonusTemplateIds = new List<short>(num);
			}
			else
			{
				BreakBonusTemplateIds.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				BreakBonusTemplateIds.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			BreakBonusTemplateIds?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (NormalBonus == null)
			{
				NormalBonus = new SkillBreakBonusCollection();
			}
			ptr += NormalBonus.Deserialize(ptr);
		}
		else
		{
			NormalBonus = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (ExtraBonus == null)
			{
				ExtraBonus = new SkillBreakBonusCollection();
			}
			ptr += ExtraBonus.Deserialize(ptr);
		}
		else
		{
			ExtraBonus = null;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
