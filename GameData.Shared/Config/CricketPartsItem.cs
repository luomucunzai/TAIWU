using System;
using Config.Common;

namespace Config;

[Serializable]
public class CricketPartsItem : ConfigItem<CricketPartsItem, short>
{
	public readonly short TemplateId;

	public readonly string[] Name;

	public readonly ECricketPartsType Type;

	public readonly string Desc;

	public readonly sbyte Level;

	public readonly sbyte NameOrder;

	public readonly sbyte Life;

	public readonly int FavorabilityChange;

	public readonly sbyte HappinessChange;

	public readonly int Value;

	public readonly int Price;

	public readonly sbyte SingPitch;

	public readonly short SingSize;

	public readonly sbyte Rate;

	public readonly bool MustSuccessLoud;

	public readonly sbyte NpcSpecialRate;

	public readonly sbyte AdvanceRate;

	public readonly short CatchInfluence;

	public readonly string Color;

	public readonly short HP;

	public readonly short SP;

	public readonly sbyte Vigor;

	public readonly sbyte Strength;

	public readonly sbyte Bite;

	public readonly sbyte Deadliness;

	public readonly sbyte Damage;

	public readonly sbyte Cripple;

	public readonly sbyte Defence;

	public readonly sbyte DamageReduce;

	public readonly short Counter;

	public readonly sbyte Taste;

	public readonly byte AvailableOnLoading;

	public CricketPartsItem(short templateId, int[] name, ECricketPartsType type, int desc, sbyte level, sbyte nameOrder, sbyte life, int favorabilityChange, sbyte happinessChange, int value, int price, sbyte singPitch, short singSize, sbyte rate, bool mustSuccessLoud, sbyte npcSpecialRate, sbyte advanceRate, short catchInfluence, string color, short hP, short sP, sbyte vigor, sbyte strength, sbyte bite, sbyte deadliness, sbyte damage, sbyte cripple, sbyte defence, sbyte damageReduce, short counter, sbyte taste, byte availableOnLoading)
	{
		TemplateId = templateId;
		Name = LocalStringManager.ConvertConfigList("CricketParts_language", name);
		Type = type;
		Desc = LocalStringManager.GetConfig("CricketParts_language", desc);
		Level = level;
		NameOrder = nameOrder;
		Life = life;
		FavorabilityChange = favorabilityChange;
		HappinessChange = happinessChange;
		Value = value;
		Price = price;
		SingPitch = singPitch;
		SingSize = singSize;
		Rate = rate;
		MustSuccessLoud = mustSuccessLoud;
		NpcSpecialRate = npcSpecialRate;
		AdvanceRate = advanceRate;
		CatchInfluence = catchInfluence;
		Color = color;
		HP = hP;
		SP = sP;
		Vigor = vigor;
		Strength = strength;
		Bite = bite;
		Deadliness = deadliness;
		Damage = damage;
		Cripple = cripple;
		Defence = defence;
		DamageReduce = damageReduce;
		Counter = counter;
		Taste = taste;
		AvailableOnLoading = availableOnLoading;
	}

	public CricketPartsItem()
	{
		TemplateId = 0;
		Name = null;
		Type = ECricketPartsType.Trash;
		Desc = null;
		Level = 0;
		NameOrder = 0;
		Life = 0;
		FavorabilityChange = 0;
		HappinessChange = 0;
		Value = 0;
		Price = 0;
		SingPitch = 0;
		SingSize = 0;
		Rate = 0;
		MustSuccessLoud = false;
		NpcSpecialRate = 0;
		AdvanceRate = 0;
		CatchInfluence = 0;
		Color = null;
		HP = 0;
		SP = 0;
		Vigor = 0;
		Strength = 0;
		Bite = 0;
		Deadliness = 0;
		Damage = 0;
		Cripple = 0;
		Defence = 0;
		DamageReduce = 0;
		Counter = 0;
		Taste = 0;
		AvailableOnLoading = 0;
	}

	public CricketPartsItem(short templateId, CricketPartsItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Type = other.Type;
		Desc = other.Desc;
		Level = other.Level;
		NameOrder = other.NameOrder;
		Life = other.Life;
		FavorabilityChange = other.FavorabilityChange;
		HappinessChange = other.HappinessChange;
		Value = other.Value;
		Price = other.Price;
		SingPitch = other.SingPitch;
		SingSize = other.SingSize;
		Rate = other.Rate;
		MustSuccessLoud = other.MustSuccessLoud;
		NpcSpecialRate = other.NpcSpecialRate;
		AdvanceRate = other.AdvanceRate;
		CatchInfluence = other.CatchInfluence;
		Color = other.Color;
		HP = other.HP;
		SP = other.SP;
		Vigor = other.Vigor;
		Strength = other.Strength;
		Bite = other.Bite;
		Deadliness = other.Deadliness;
		Damage = other.Damage;
		Cripple = other.Cripple;
		Defence = other.Defence;
		DamageReduce = other.DamageReduce;
		Counter = other.Counter;
		Taste = other.Taste;
		AvailableOnLoading = other.AvailableOnLoading;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override CricketPartsItem Duplicate(int templateId)
	{
		return new CricketPartsItem((short)templateId, this);
	}
}
