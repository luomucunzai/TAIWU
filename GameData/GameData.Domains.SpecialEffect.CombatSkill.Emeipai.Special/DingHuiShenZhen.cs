using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.Combat.Ai.Memory;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Special;

public class DingHuiShenZhen : CombatSkillEffectBase
{
	private const sbyte ChangePower = 40;

	public DingHuiShenZhen()
	{
	}

	public DingHuiShenZhen(CombatSkillKey skillKey)
		: base(skillKey, 2405, -1)
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
		AiMemory memory = base.CurrEnemyChar.AiController.Memory;
		if (PowerMatchAffectRequire(power) && memory != null)
		{
			SkillEffectKey skillEffectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			SkillPowerChangeCollection value;
			if (base.IsDirect)
			{
				foreach (short key in memory.EnemyRecordDict[base.CharacterId].SkillRecord.Keys)
				{
					CombatSkillKey combatSkillKey = new CombatSkillKey(base.CharacterId, key);
					if (DomainManager.Combat.CombatSkillDataExist(combatSkillKey) && (!DomainManager.Combat.TryGetElement_SkillPowerAddInCombat(combatSkillKey, out value) || !DomainManager.Combat.GetElement_SkillPowerAddInCombat(combatSkillKey).EffectDict.ContainsKey(skillEffectKey)))
					{
						DomainManager.Combat.AddSkillPowerInCombat(context, combatSkillKey, skillEffectKey, 40);
					}
				}
			}
			else
			{
				foreach (short key2 in memory.SelfRecord.SkillRecord.Keys)
				{
					CombatSkillKey combatSkillKey2 = new CombatSkillKey(base.CurrEnemyChar.GetId(), key2);
					if (DomainManager.Combat.CombatSkillDataExist(combatSkillKey2) && (!DomainManager.Combat.TryGetElement_SkillPowerReduceInCombat(combatSkillKey2, out value) || !DomainManager.Combat.GetElement_SkillPowerReduceInCombat(combatSkillKey2).EffectDict.ContainsKey(skillEffectKey)))
					{
						DomainManager.Combat.ReduceSkillPowerInCombat(context, combatSkillKey2, skillEffectKey, -40);
					}
				}
			}
			base.CurrEnemyChar.AiController.ClearMemories();
			ShowSpecialEffectTips(0);
		}
		RemoveSelf(context);
	}
}
