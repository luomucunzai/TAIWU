using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy;

public class BaiXie : MinionBase
{
	private const sbyte ChangeInjury = 2;

	public BaiXie()
	{
	}

	public BaiXie(CombatSkillKey skillKey)
		: base(skillKey, 16004)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		if (base.CombatChar.IsAlly == isAlly || !base.IsCurrent || !MinionBase.CanAffect)
		{
			return;
		}
		sbyte direction = DomainManager.CombatSkill.GetElement_CombatSkills((charId: charId, skillId: skillId)).GetDirection();
		if (direction != -1)
		{
			bool flag = direction == 0;
			CombatCharacter combatCharacter = (flag ? DomainManager.Combat.GetElement_CombatCharacterDict(charId) : base.CombatChar);
			if (flag ? combatCharacter.WorsenRandomInjury(context, WorsenConstants.SpecialPercentBaiXie) : combatCharacter.RemoveRandomInjury(context, 2))
			{
				ShowSpecialEffectTips(flag, 0, 1);
			}
		}
	}
}
