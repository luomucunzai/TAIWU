using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class EnemyNestItem : ConfigItem<EnemyNestItem, short>
{
	public readonly short TemplateId;

	public readonly string TipTitle;

	public readonly string TipDesc;

	public readonly sbyte NestType;

	public readonly short WorldTotalCountLimit;

	public readonly List<short> Members;

	public readonly short Leader;

	public readonly List<short> SpawnAmountFactors;

	public readonly short MonthlyActionId;

	public readonly short AdventureId;

	public readonly short SpiritualDebtChange;

	public readonly int ExpReward;

	public readonly int AuthorityReward;

	public readonly int MoneyReward;

	public readonly short ConqueredDuration;

	public EnemyNestItem(short templateId, int tipTitle, int tipDesc, sbyte nestType, short worldTotalCountLimit, List<short> members, short leader, List<short> spawnAmountFactors, short monthlyActionId, short adventureId, short spiritualDebtChange, int expReward, int authorityReward, int moneyReward, short conqueredDuration)
	{
		TemplateId = templateId;
		TipTitle = LocalStringManager.GetConfig("EnemyNest_language", tipTitle);
		TipDesc = LocalStringManager.GetConfig("EnemyNest_language", tipDesc);
		NestType = nestType;
		WorldTotalCountLimit = worldTotalCountLimit;
		Members = members;
		Leader = leader;
		SpawnAmountFactors = spawnAmountFactors;
		MonthlyActionId = monthlyActionId;
		AdventureId = adventureId;
		SpiritualDebtChange = spiritualDebtChange;
		ExpReward = expReward;
		AuthorityReward = authorityReward;
		MoneyReward = moneyReward;
		ConqueredDuration = conqueredDuration;
	}

	public EnemyNestItem()
	{
		TemplateId = 0;
		TipTitle = null;
		TipDesc = null;
		NestType = 0;
		WorldTotalCountLimit = -1;
		Members = null;
		Leader = 0;
		SpawnAmountFactors = null;
		MonthlyActionId = 0;
		AdventureId = 0;
		SpiritualDebtChange = 0;
		ExpReward = 0;
		AuthorityReward = 0;
		MoneyReward = 0;
		ConqueredDuration = -1;
	}

	public EnemyNestItem(short templateId, EnemyNestItem other)
	{
		TemplateId = templateId;
		TipTitle = other.TipTitle;
		TipDesc = other.TipDesc;
		NestType = other.NestType;
		WorldTotalCountLimit = other.WorldTotalCountLimit;
		Members = other.Members;
		Leader = other.Leader;
		SpawnAmountFactors = other.SpawnAmountFactors;
		MonthlyActionId = other.MonthlyActionId;
		AdventureId = other.AdventureId;
		SpiritualDebtChange = other.SpiritualDebtChange;
		ExpReward = other.ExpReward;
		AuthorityReward = other.AuthorityReward;
		MoneyReward = other.MoneyReward;
		ConqueredDuration = other.ConqueredDuration;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override EnemyNestItem Duplicate(int templateId)
	{
		return new EnemyNestItem((short)templateId, this);
	}
}
