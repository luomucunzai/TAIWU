using System;
using Config.Common;
using GameData.Domains.Character;

namespace Config;

[Serializable]
public class EquipmentEffectItem : ConfigItem<EquipmentEffectItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly sbyte Type;

	public readonly bool Special;

	public readonly bool IsTotalPercent;

	public readonly string Desc;

	public readonly short[] HitFactors;

	public readonly HitOrAvoidShorts AvoidFactors;

	public readonly OuterAndInnerShorts PenetrationResistFactors;

	public readonly OuterAndInnerShorts InjuryFactors;

	public readonly sbyte EquipmentAttackChange;

	public readonly sbyte EquipmentDefenseChange;

	public readonly sbyte WeightChange;

	public readonly sbyte MaxDurabilityChange;

	public readonly sbyte ValueChange;

	public readonly sbyte FavorChange;

	public readonly sbyte RequirementChange;

	public readonly string EffectClassName;

	public EquipmentEffectItem(short templateId, int name, sbyte type, bool special, bool isTotalPercent, int desc, short[] hitFactors, HitOrAvoidShorts avoidFactors, OuterAndInnerShorts penetrationResistFactors, OuterAndInnerShorts injuryFactors, sbyte equipmentAttackChange, sbyte equipmentDefenseChange, sbyte weightChange, sbyte maxDurabilityChange, sbyte valueChange, sbyte favorChange, sbyte requirementChange, string effectClassName)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("EquipmentEffect_language", name);
		Type = type;
		Special = special;
		IsTotalPercent = isTotalPercent;
		Desc = LocalStringManager.GetConfig("EquipmentEffect_language", desc);
		HitFactors = hitFactors;
		AvoidFactors = avoidFactors;
		PenetrationResistFactors = penetrationResistFactors;
		InjuryFactors = injuryFactors;
		EquipmentAttackChange = equipmentAttackChange;
		EquipmentDefenseChange = equipmentDefenseChange;
		WeightChange = weightChange;
		MaxDurabilityChange = maxDurabilityChange;
		ValueChange = valueChange;
		FavorChange = favorChange;
		RequirementChange = requirementChange;
		EffectClassName = effectClassName;
	}

	public EquipmentEffectItem()
	{
		TemplateId = 0;
		Name = null;
		Type = -1;
		Special = false;
		IsTotalPercent = false;
		Desc = null;
		HitFactors = new short[4];
		AvoidFactors = new HitOrAvoidShorts(default(short), default(short), default(short), default(short));
		PenetrationResistFactors = new OuterAndInnerShorts(0, 0);
		InjuryFactors = new OuterAndInnerShorts(0, 0);
		EquipmentAttackChange = 0;
		EquipmentDefenseChange = 0;
		WeightChange = 0;
		MaxDurabilityChange = 0;
		ValueChange = 0;
		FavorChange = 0;
		RequirementChange = 0;
		EffectClassName = null;
	}

	public EquipmentEffectItem(short templateId, EquipmentEffectItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Type = other.Type;
		Special = other.Special;
		IsTotalPercent = other.IsTotalPercent;
		Desc = other.Desc;
		HitFactors = other.HitFactors;
		AvoidFactors = other.AvoidFactors;
		PenetrationResistFactors = other.PenetrationResistFactors;
		InjuryFactors = other.InjuryFactors;
		EquipmentAttackChange = other.EquipmentAttackChange;
		EquipmentDefenseChange = other.EquipmentDefenseChange;
		WeightChange = other.WeightChange;
		MaxDurabilityChange = other.MaxDurabilityChange;
		ValueChange = other.ValueChange;
		FavorChange = other.FavorChange;
		RequirementChange = other.RequirementChange;
		EffectClassName = other.EffectClassName;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override EquipmentEffectItem Duplicate(int templateId)
	{
		return new EquipmentEffectItem((short)templateId, this);
	}
}
