using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.DefenseAndAssist;

public class HeBoQuQi : DefenseSkillBase
{
	public HeBoQuQi()
	{
	}

	public HeBoQuQi(CombatSkillKey skillKey)
		: base(skillKey, 13501)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (!isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect)
		{
			return;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		IReadOnlyDictionary<int, sbyte> tricks = (base.IsDirect ? combatCharacter : base.CombatChar).GetTricks().Tricks;
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		list.AddRange(tricks.Values);
		list.RemoveAll((sbyte type) => base.IsDirect != base.CombatChar.IsTrickUsable(type));
		if (list.Count > 0)
		{
			sbyte trickType2 = list[context.Random.Next(0, list.Count)];
			if (base.IsDirect)
			{
				if (DomainManager.Combat.RemoveTrick(context, combatCharacter, trickType2, 1, removedByAlly: false))
				{
					DomainManager.Combat.AddTrick(context, base.CombatChar, trickType2);
				}
			}
			else
			{
				DomainManager.Combat.RemoveTrick(context, base.CombatChar, trickType2, 1);
				DomainManager.Combat.AddTrick(context, combatCharacter, trickType2, addedByAlly: false);
			}
			ShowSpecialEffectTips(0);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}
}
