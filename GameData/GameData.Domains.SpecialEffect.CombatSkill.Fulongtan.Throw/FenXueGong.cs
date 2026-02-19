using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Throw;

public class FenXueGong : CombatSkillEffectBase
{
	private const int ChangeNewInjuryCount = 3;

	private const int ChangeOldInjuryCount = 1;

	private const sbyte AddPower = 60;

	private const sbyte AddAttackRange = 20;

	public FenXueGong()
	{
	}

	public FenXueGong(CombatSkillKey skillKey)
		: base(skillKey, 14304, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		Injuries injuries = base.CombatChar.GetInjuries();
		Injuries oldInjuries = base.CombatChar.GetOldInjuries();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			int num = injuries.Get(b, !base.IsDirect) - oldInjuries.Get(b, !base.IsDirect);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					list.Add(b);
				}
			}
		}
		if (list.Count > 0)
		{
			int num2 = Math.Min(list.Count, 1);
			int num3 = Math.Min(list.Count, 3);
			CollectionUtils.Shuffle(context.Random, list);
			for (int j = 0; j < num2; j++)
			{
				sbyte bodyPart = list[j];
				DomainManager.Combat.ChangeToOldInjury(context, base.CombatChar, bodyPart, !base.IsDirect, 1);
			}
			for (int k = num2; k < num3; k++)
			{
				sbyte bodyPart2 = list[k];
				DomainManager.Combat.RemoveInjury(context, base.CombatChar, bodyPart2, !base.IsDirect, 1, updateDefeatMark: true);
			}
			AppendAffectedData(context, base.CharacterId, 199, (EDataModifyType)1, base.SkillTemplateId);
			AppendAffectedData(context, base.CharacterId, 145, (EDataModifyType)0, base.SkillTemplateId);
			AppendAffectedData(context, base.CharacterId, 146, (EDataModifyType)0, base.SkillTemplateId);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
			ShowSpecialEffectTips(0);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
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
		if (dataKey.FieldId == 145 || dataKey.FieldId == 146)
		{
			return 20;
		}
		if (dataKey.FieldId == 199)
		{
			return 60;
		}
		return 0;
	}
}
