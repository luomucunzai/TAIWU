using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SolarTermItem : ConfigItem<SolarTermItem, sbyte>
{
	public readonly sbyte TemplateId;

	public readonly string Name;

	public readonly sbyte Type;

	public readonly string Desc;

	public readonly string Poem;

	public readonly string Image;

	public readonly string Sound;

	public readonly sbyte Month;

	public readonly List<byte> FiveElementsTypesOfCombatSkillBuff;

	public readonly List<short> MaterialIdsOfFoodBuff;

	public readonly sbyte PoisonBuffType;

	public readonly sbyte DetoxBuffType;

	public readonly bool OuterHealingBuff;

	public readonly bool InnerHealingBuff;

	public readonly bool QiDisorderRecoveringBuff;

	public readonly bool HealthBuff;

	public SolarTermItem(sbyte templateId, int name, sbyte type, int desc, int poem, string image, string sound, sbyte month, List<byte> fiveElementsTypesOfCombatSkillBuff, List<short> materialIdsOfFoodBuff, sbyte poisonBuffType, sbyte detoxBuffType, bool outerHealingBuff, bool innerHealingBuff, bool qiDisorderRecoveringBuff, bool healthBuff)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("SolarTerm_language", name);
		Type = type;
		Desc = LocalStringManager.GetConfig("SolarTerm_language", desc);
		Poem = LocalStringManager.GetConfig("SolarTerm_language", poem);
		Image = image;
		Sound = sound;
		Month = month;
		FiveElementsTypesOfCombatSkillBuff = fiveElementsTypesOfCombatSkillBuff;
		MaterialIdsOfFoodBuff = materialIdsOfFoodBuff;
		PoisonBuffType = poisonBuffType;
		DetoxBuffType = detoxBuffType;
		OuterHealingBuff = outerHealingBuff;
		InnerHealingBuff = innerHealingBuff;
		QiDisorderRecoveringBuff = qiDisorderRecoveringBuff;
		HealthBuff = healthBuff;
	}

	public SolarTermItem()
	{
		TemplateId = 0;
		Name = null;
		Type = 0;
		Desc = null;
		Poem = null;
		Image = null;
		Sound = null;
		Month = 0;
		FiveElementsTypesOfCombatSkillBuff = null;
		MaterialIdsOfFoodBuff = new List<short>();
		PoisonBuffType = 0;
		DetoxBuffType = 0;
		OuterHealingBuff = false;
		InnerHealingBuff = false;
		QiDisorderRecoveringBuff = false;
		HealthBuff = false;
	}

	public SolarTermItem(sbyte templateId, SolarTermItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Type = other.Type;
		Desc = other.Desc;
		Poem = other.Poem;
		Image = other.Image;
		Sound = other.Sound;
		Month = other.Month;
		FiveElementsTypesOfCombatSkillBuff = other.FiveElementsTypesOfCombatSkillBuff;
		MaterialIdsOfFoodBuff = other.MaterialIdsOfFoodBuff;
		PoisonBuffType = other.PoisonBuffType;
		DetoxBuffType = other.DetoxBuffType;
		OuterHealingBuff = other.OuterHealingBuff;
		InnerHealingBuff = other.InnerHealingBuff;
		QiDisorderRecoveringBuff = other.QiDisorderRecoveringBuff;
		HealthBuff = other.HealthBuff;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override SolarTermItem Duplicate(int templateId)
	{
		return new SolarTermItem((sbyte)templateId, this);
	}
}
