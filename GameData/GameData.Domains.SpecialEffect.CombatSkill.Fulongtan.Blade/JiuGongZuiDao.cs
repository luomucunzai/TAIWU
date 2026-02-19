using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade;

public class JiuGongZuiDao : CombatSkillEffectBase
{
	private const sbyte NormalRemoveTrickCount = 2;

	private const sbyte DrunkRemoveTrickCount = 3;

	private const sbyte DrunkAddPowerUnit = 10;

	private int _addPower;

	public JiuGongZuiDao()
	{
	}

	public JiuGongZuiDao(CombatSkillKey skillKey)
		: base(skillKey, 14201, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CombatCharacter affectChar = (base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true));
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		list.AddRange(affectChar.GetTricks().Tricks.Values);
		list.RemoveAll((sbyte type) => affectChar.IsTrickUsable(type) == base.IsDirect);
		if (list.Count > 0)
		{
			bool flag = CharObj.GetEatingItems().ContainsWine();
			int num = Math.Min(flag ? 3 : 2, list.Count);
			List<NeedTrick> list2 = ObjectPool<List<NeedTrick>>.Instance.Get();
			list2.Clear();
			for (int num2 = 0; num2 < num; num2++)
			{
				sbyte b = list[context.Random.Next(0, list.Count)];
				list.Remove(b);
				list2.Add(new NeedTrick(b, 1));
			}
			if (flag)
			{
				_addPower = num * 10;
				AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId), (EDataModifyType)1);
			}
			DomainManager.Combat.RemoveTrick(context, affectChar, list2, base.IsDirect);
			ShowSpecialEffectTips(0);
			ObjectPool<List<NeedTrick>>.Instance.Return(list2);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
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
			RemoveSelf(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return _addPower;
		}
		return 0;
	}
}
