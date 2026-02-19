using Config;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Item.Display;

[SerializableGameData(NotForArchive = true)]
public class SkillBookModifyDisplayData : ISerializableGameData, IFilterableCombatSkill
{
	[SerializableGameDataField]
	public ItemDisplayData ItemDisplayData;

	[SerializableGameDataField]
	public int NormalPageCostExp;

	[SerializableGameDataField]
	public int OutlinePageCostExp;

	[SerializableGameDataField]
	public byte PageTypes;

	[SerializableGameDataField]
	public ushort PageIncompleteState;

	public sbyte Type => SkillConfig.Type;

	public sbyte SectId => SkillConfig.SectId;

	public short SkillTemplateId => SkillBook.Instance[ItemDisplayData.Key.TemplateId].CombatSkillTemplateId;

	public CombatSkillItem SkillConfig => Config.CombatSkill.Instance[SkillTemplateId];

	public SkillBookModifyDisplayData()
	{
	}

	public SkillBookModifyDisplayData(SkillBookModifyDisplayData other)
	{
		ItemDisplayData = new ItemDisplayData(other.ItemDisplayData);
		NormalPageCostExp = other.NormalPageCostExp;
		OutlinePageCostExp = other.OutlinePageCostExp;
		PageTypes = other.PageTypes;
		PageIncompleteState = other.PageIncompleteState;
	}

	public void Assign(SkillBookModifyDisplayData other)
	{
		ItemDisplayData = new ItemDisplayData(other.ItemDisplayData);
		NormalPageCostExp = other.NormalPageCostExp;
		OutlinePageCostExp = other.OutlinePageCostExp;
		PageTypes = other.PageTypes;
		PageIncompleteState = other.PageIncompleteState;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 11;
		num = ((ItemDisplayData == null) ? (num + 2) : (num + (2 + ItemDisplayData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (ItemDisplayData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = ItemDisplayData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = NormalPageCostExp;
		ptr += 4;
		*(int*)ptr = OutlinePageCostExp;
		ptr += 4;
		*ptr = PageTypes;
		ptr++;
		*(ushort*)ptr = PageIncompleteState;
		ptr += 2;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ItemDisplayData == null)
			{
				ItemDisplayData = new ItemDisplayData();
			}
			ptr += ItemDisplayData.Deserialize(ptr);
		}
		else
		{
			ItemDisplayData = null;
		}
		NormalPageCostExp = *(int*)ptr;
		ptr += 4;
		OutlinePageCostExp = *(int*)ptr;
		ptr += 4;
		PageTypes = *ptr;
		ptr++;
		PageIncompleteState = *(ushort*)ptr;
		ptr += 2;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
