using Config;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public struct ShowSpecialEffectDisplayData : ISerializableGameData
{
	public static readonly ShowSpecialEffectDisplayData Invalid = new ShowSpecialEffectDisplayData
	{
		Index = -1,
		ItemData = ItemKey.Invalid,
		EffectDescription = CombatSkillEffectDescriptionDisplayData.Invalid
	};

	[SerializableGameDataField]
	public int Index;

	[SerializableGameDataField]
	public int EffectId;

	[SerializableGameDataField]
	public ItemKey ItemData;

	[SerializableGameDataField]
	public CombatSkillEffectDescriptionDisplayData EffectDescription;

	public static int CheckIndex(int effectId, byte index)
	{
		SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[effectId];
		if (specialEffectItem.ShortDesc.Length <= index)
		{
			return -1;
		}
		return index;
	}

	public ShowSpecialEffectDisplayData(int charId, int effectId, int index, ItemKey itemData)
	{
		Index = index;
		EffectId = effectId;
		ItemData = itemData;
		short skillTemplateId = Config.SpecialEffect.Instance[effectId].SkillTemplateId;
		EffectDescription = DomainManager.CombatSkill.GetEffectDisplayData(charId, skillTemplateId);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 16;
		num += EffectDescription.GetSerializedSize();
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Index;
		ptr += 4;
		*(int*)ptr = EffectId;
		ptr += 4;
		ptr += ItemData.Serialize(ptr);
		int num = EffectDescription.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Index = *(int*)ptr;
		ptr += 4;
		EffectId = *(int*)ptr;
		ptr += 4;
		ptr += ItemData.Deserialize(ptr);
		ptr += EffectDescription.Deserialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
