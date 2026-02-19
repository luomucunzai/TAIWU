using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade;

public class JiuLongBaDao : CombatSkillEffectBase
{
	private const sbyte MaxTransferCount = 9;

	private const int SilenceFrame = 3000;

	private const int TransferPowerRatio = 2;

	private int _addedPower;

	public JiuLongBaDao()
	{
	}

	public JiuLongBaDao(CombatSkillKey skillKey)
		: base(skillKey, 14207, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId), (EDataModifyType)0);
		ChangePower(context);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, 3000, -1);
			ShowSpecialEffectTips(1);
			RemoveSelf(context);
		}
	}

	private void ChangePower(DataContext context)
	{
		CombatCharacter currEnemyChar = base.CurrEnemyChar;
		Dictionary<CombatSkillKey, SkillPowerChangeCollection> dictionary = (base.IsDirect ? DomainManager.Combat.GetAllSkillPowerAddInCombat() : DomainManager.Combat.GetAllSkillPowerReduceInCombat());
		List<CombatSkillKey> list = ObjectPool<List<CombatSkillKey>>.Instance.Get();
		List<CombatSkillKey> list2 = ObjectPool<List<CombatSkillKey>>.Instance.Get();
		int num = (base.IsDirect ? currEnemyChar.GetId() : base.CombatChar.GetId());
		foreach (CombatSkillKey key in dictionary.Keys)
		{
			if (key.CharId == num)
			{
				list.Add(key);
			}
			else
			{
				list2.Add(key);
			}
		}
		if (list.Count > 0 || list2.Count > 0)
		{
			foreach (CombatSkillKey item in RandomUtils.GetRandomUnrepeated(context.Random, 9, list, list2))
			{
				SkillPowerChangeCollection skillPowerChangeCollection = (base.IsDirect ? DomainManager.Combat.RemoveSkillPowerAddInCombat(context, item) : DomainManager.Combat.RemoveSkillPowerReduceInCombat(context, item));
				if (skillPowerChangeCollection == null)
				{
					continue;
				}
				foreach (int value in skillPowerChangeCollection.EffectDict.Values)
				{
					_addedPower += Math.Abs(value) * 2;
				}
			}
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			ShowSpecialEffectTips(0);
		}
		ObjectPool<List<CombatSkillKey>>.Instance.Return(list);
		ObjectPool<List<CombatSkillKey>>.Instance.Return(list2);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (!dataKey.IsMatch(SkillKey) || dataKey.FieldId != 199)
		{
			return 0;
		}
		return _addedPower;
	}
}
