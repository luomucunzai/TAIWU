using System.Collections.Generic;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class TaiwuProfessionSkillSlots : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Slots = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "Slots" };
	}

	[SerializableGameDataField]
	public IntList[] Slots;

	public const int LevelCount = 4;

	public void Initialize()
	{
		Slots = new IntList[4];
		for (int i = 0; i < 4; i++)
		{
			IntList intList = IntList.Create();
			int num = 4 - i;
			for (int j = 0; j < num; j++)
			{
				intList.Items.Add(-1);
			}
			Slots[i] = intList;
		}
	}

	public bool IsEquipped(int professionSkillId)
	{
		ProfessionSkillItem professionSkillItem = ProfessionSkill.Instance[professionSkillId];
		return Slots[professionSkillItem.Level - 1].Items.Contains(professionSkillId);
	}

	public bool IsEquipped(ProfessionSkillItem skillCfg)
	{
		return Slots[skillCfg.Level - 1].Items.Contains(skillCfg.TemplateId);
	}

	public IEnumerable<int> GetNewlyEquippedSkills(TaiwuProfessionSkillSlots newSlots)
	{
		for (int i = 0; i < 4; i++)
		{
			IntList oldLevelSlots = Slots[i];
			IntList newLevelSlots = newSlots.Slots[i];
			for (int j = 0; j < oldLevelSlots.Items.Count; j++)
			{
				if (oldLevelSlots.Items[j] == -1 && newLevelSlots.Items[j] != -1)
				{
					yield return newLevelSlots.Items[j];
				}
			}
		}
	}

	public IEnumerable<int> GetNewlyRemovedSkills(TaiwuProfessionSkillSlots newSlots)
	{
		for (int i = 0; i < 4; i++)
		{
			IntList oldLevelSlots = Slots[i];
			IntList newLevelSlots = newSlots.Slots[i];
			for (int j = 0; j < oldLevelSlots.Items.Count; j++)
			{
				if (oldLevelSlots.Items[j] != -1 && newLevelSlots.Items[j] == -1)
				{
					yield return oldLevelSlots.Items[j];
				}
			}
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		if (Slots != null)
		{
			num += 2;
			int num2 = Slots.Length;
			for (int i = 0; i < num2; i++)
			{
				num += Slots[i].GetSerializedSize();
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
		*(short*)ptr = 1;
		ptr += 2;
		if (Slots != null)
		{
			int num = Slots.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				int num2 = Slots[i].Serialize(ptr);
				ptr += num2;
				Tester.Assert(num2 <= 65535);
			}
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (Slots == null || Slots.Length != num2)
				{
					Slots = new IntList[num2];
				}
				for (int i = 0; i < num2; i++)
				{
					IntList intList = default(IntList);
					ptr += intList.Deserialize(ptr);
					Slots[i] = intList;
				}
			}
			else
			{
				Slots = null;
			}
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
