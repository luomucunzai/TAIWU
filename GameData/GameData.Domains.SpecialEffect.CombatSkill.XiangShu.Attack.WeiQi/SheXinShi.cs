using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi;

public class SheXinShi : CombatSkillEffectBase
{
	private const sbyte AddPowerUnit = 60;

	private readonly Dictionary<short, int> _addPowerDict = new Dictionary<short, int>();

	public SheXinShi()
	{
	}

	public SheXinShi(CombatSkillKey skillKey)
		: base(skillKey, 17055, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(199, (EDataModifyType)1, -1);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId || interrupted)
		{
			return;
		}
		IReadOnlyDictionary<int, sbyte> tricks = base.CombatChar.GetTricks().Tricks;
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		short num = 0;
		list.Clear();
		list.AddRange(tricks.Values);
		int num2 = Math.Min(base.MaxEffectCount, list.Count);
		for (int i = 0; i < num2; i++)
		{
			int index = context.Random.Next(list.Count);
			if (DomainManager.Combat.RemoveTrick(context, base.CombatChar, list[index], 1))
			{
				num++;
			}
			list.RemoveAt(index);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		if (num > 0)
		{
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), num, base.MaxEffectCount, autoRemoveOnNoCount: true);
		}
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId != base.CharacterId || base.EffectCount <= 0)
		{
			return;
		}
		ReduceEffectCount();
		if (base.EffectCount > 0)
		{
			if (!_addPowerDict.TryAdd(skillId, 60))
			{
				_addPowerDict[skillId] += 60;
			}
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			ShowSpecialEffectTips(0);
		}
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			_addPowerDict.Clear();
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		return _addPowerDict.GetValueOrDefault(dataKey.CombatSkillId, 0);
	}
}
