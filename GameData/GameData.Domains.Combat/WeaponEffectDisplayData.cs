using Config;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public struct WeaponEffectDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public SkillEffectKey EffectKey;

	[SerializableGameDataField]
	public CombatSkillEffectDescriptionDisplayData EffectDescription;

	public WeaponEffectDisplayData(SkillEffectKey effectKey, int charId)
	{
		EffectKey = effectKey;
		if (DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: charId, skillId: effectKey.SkillId), out var element))
		{
			EffectDescription = DomainManager.CombatSkill.GetEffectDisplayData(element);
		}
		else if (effectKey.SkillId >= 0)
		{
			EffectDescription = new CombatSkillEffectDescriptionDisplayData
			{
				EffectId = (effectKey.IsDirect ? Config.CombatSkill.Instance[effectKey.SkillId].DirectEffectID : Config.CombatSkill.Instance[effectKey.SkillId].ReverseEffectID)
			};
		}
		else
		{
			EffectDescription = CombatSkillEffectDescriptionDisplayData.Invalid;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		num += EffectDescription.GetSerializedSize();
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += EffectKey.Serialize(ptr);
		int num = EffectDescription.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += EffectKey.Deserialize(ptr);
		ptr += EffectDescription.Deserialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
