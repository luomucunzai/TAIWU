using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip;

public class XuanKongShenFu : CombatSkillEffectBase
{
	private const sbyte AffectSkillCount = 3;

	private const int AddPowerPercent = 500;

	private const int ChangeDisorderOfQiUnit = 10;

	private int _addPower;

	public XuanKongShenFu()
	{
	}

	public XuanKongShenFu(CombatSkillKey skillKey)
		: base(skillKey, 4307, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		DoAffect(context);
		CreateAffectedData(199, (EDataModifyType)0, -1);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void DoAffect(DataContext context)
	{
		int num = (base.IsDirect ? base.CharacterId : base.CurrEnemyChar.GetId());
		List<CombatSkillKey> list = ObjectPool<List<CombatSkillKey>>.Instance.Get();
		Dictionary<CombatSkillKey, SkillPowerChangeCollection> dictionary = (base.IsDirect ? DomainManager.Combat.GetAllSkillPowerReduceInCombat() : DomainManager.Combat.GetAllSkillPowerAddInCombat());
		list.Clear();
		foreach (CombatSkillKey key in dictionary.Keys)
		{
			if (key.CharId == num)
			{
				list.Add(key);
			}
		}
		if (list.Count > 0)
		{
			DoAffectImplement(context, list);
		}
		ObjectPool<List<CombatSkillKey>>.Instance.Return(list);
	}

	private void DoAffectImplement(DataContext context, List<CombatSkillKey> pool)
	{
		int num = Math.Min(3, pool.Count);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			int index = context.Random.Next(0, pool.Count);
			SkillPowerChangeCollection skillPowerChangeCollection = (base.IsDirect ? DomainManager.Combat.RemoveSkillPowerReduceInCombat(context, pool[index]) : DomainManager.Combat.RemoveSkillPowerAddInCombat(context, pool[index]));
			pool.RemoveAt(index);
			if (skillPowerChangeCollection != null)
			{
				num2 += Math.Abs(skillPowerChangeCollection.GetTotalChangeValue());
			}
		}
		_addPower = num2 * 500 / 100;
		int num3 = 10 * num2;
		if (num3 > 0)
		{
			if (base.IsDirect)
			{
				DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.CombatChar, -num3);
			}
			else
			{
				DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.CurrEnemyChar, num3);
			}
		}
		if (!base.IsDirect)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CurrEnemyChar.GetId(), 199);
		}
		if (num2 > 0)
		{
			ShowSpecialEffectTips(0);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			RemoveSelf(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.SkillKey == SkillKey && dataKey.FieldId == 199)
		{
			return _addPower;
		}
		return base.GetModifyValue(dataKey, currModifyValue);
	}
}
