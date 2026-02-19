using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public class ChangePowerByEquipType : CombatSkillEffectBase
{
	protected sbyte AffectEquipType;

	protected virtual sbyte ChangePowerUnitDirect => 2;

	protected virtual sbyte ChangePowerUnitReverse => 2;

	protected ChangePowerByEquipType()
	{
	}

	protected ChangePowerByEquipType(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		sbyte b = (base.IsDirect ? ChangePowerUnitDirect : ChangePowerUnitReverse);
		int num = power / 10 * (base.IsDirect ? b : (-b));
		if (num != 0)
		{
			SkillEffectKey skillEffectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true));
			bool flag = false;
			List<short> list = ObjectPool<List<short>>.Instance.Get();
			list.Clear();
			if (combatCharacter.BossConfig == null)
			{
				list.AddRange(combatCharacter.GetCombatSkillList(AffectEquipType));
			}
			else
			{
				list.AddRange(combatCharacter.GetCharacter().GetLearnedCombatSkills().FindAll((short id) => Config.CombatSkill.Instance[id].EquipType == AffectEquipType));
			}
			foreach (short item in list)
			{
				if (item < 0)
				{
					continue;
				}
				CombatSkillKey combatSkillKey = new CombatSkillKey(combatCharacter.GetId(), item);
				SkillPowerChangeCollection value;
				if (base.IsDirect)
				{
					DomainManager.Combat.TryGetElement_SkillPowerAddInCombat(combatSkillKey, out value);
				}
				else
				{
					DomainManager.Combat.TryGetElement_SkillPowerReduceInCombat(combatSkillKey, out value);
				}
				int num2 = ((value != null && value.EffectDict.ContainsKey(skillEffectKey)) ? value.EffectDict[skillEffectKey] : 0);
				int num3 = num - num2;
				if (base.IsDirect ? (num3 > 0) : (num3 < 0))
				{
					if (base.IsDirect)
					{
						DomainManager.Combat.AddSkillPowerInCombat(context, combatSkillKey, skillEffectKey, num3);
					}
					else
					{
						DomainManager.Combat.ReduceSkillPowerInCombat(context, combatSkillKey, skillEffectKey, num3);
					}
					flag = true;
				}
			}
			if (flag)
			{
				ShowSpecialEffectTips(0);
			}
			ObjectPool<List<short>>.Instance.Return(list);
		}
		RemoveSelf(context);
	}
}
