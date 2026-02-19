using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw;

public class MingLongZhi : CombatSkillEffectBase
{
	private const sbyte AddPowerUnit = 20;

	private const int MaxCostTrick = 3;

	private const sbyte AddDamagePercentUnit = 20;

	private int _addPower;

	private int _addDamagePercent;

	public MingLongZhi()
	{
	}

	public MingLongZhi(CombatSkillKey skillKey)
		: base(skillKey, 13306, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : base.EnemyChar);
		_addPower = 20 * combatCharacter.GetContinueTricksAtStart(19);
		if (_addPower > 0)
		{
			CreateAffectedData(199, (EDataModifyType)1, base.SkillTemplateId);
			ShowSpecialEffectTips(0);
		}
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar))
		{
			return;
		}
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : base.EnemyChar);
		int num = Math.Min(3, (int)combatCharacter.GetTrickCount(19));
		if (num <= 0)
		{
			return;
		}
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list2.Clear();
		TrickCollection tricks = combatCharacter.GetTricks();
		foreach (var (item, b2) in tricks.Tricks)
		{
			if (b2 != 19)
			{
				if (combatCharacter.IsTrickUsable(b2))
				{
					(base.IsDirect ? list2 : list).Add(item);
				}
				else
				{
					(base.IsDirect ? list : list2).Add(item);
				}
			}
		}
		int num3 = Math.Min(num, list.Count + list2.Count);
		for (int i = 0; i < num3; i++)
		{
			int trickIndex = ((i < list.Count) ? list[i] : list2[i - list.Count]);
			tricks.RemoveTrick(trickIndex);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
		if (num3 > 0)
		{
			combatCharacter.SetTricks(tricks, context);
			_addDamagePercent = 20 * num3;
			AppendAffectedData(context, base.CharacterId, 69, (EDataModifyType)1, base.SkillTemplateId);
			ShowSpecialEffectTips(1);
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
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		ushort fieldId = dataKey.FieldId;
		if (1 == 0)
		{
		}
		int result = fieldId switch
		{
			69 => _addDamagePercent, 
			199 => _addPower, 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		return result;
	}
}
