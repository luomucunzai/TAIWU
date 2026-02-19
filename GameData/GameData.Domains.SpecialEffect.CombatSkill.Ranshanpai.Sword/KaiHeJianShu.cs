using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword;

public class KaiHeJianShu : CombatSkillEffectBase
{
	private const sbyte StatePowerUnit = 25;

	public KaiHeJianShu()
	{
	}

	public KaiHeJianShu(CombatSkillKey skillKey)
		: base(skillKey, 7203, -1)
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

	private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (power > 0)
		{
			short[] array = ((!base.IsDirect) ? new short[4] { 30, 31, 32, 33 } : new short[4] { 26, 27, 28, 29 });
			HitOrAvoidInts hitOrAvoidInts = (base.IsDirect ? CharObj.GetAvoidValues() : CharObj.GetHitValues());
			HitOrAvoidInts hitOrAvoidInts2 = (base.IsDirect ? base.CurrEnemyChar.GetCharacter().GetAvoidValues() : base.CurrEnemyChar.GetCharacter().GetHitValues());
			bool flag = false;
			for (sbyte b = 0; b < 4; b++)
			{
				if (hitOrAvoidInts.Items[b] >= hitOrAvoidInts2.Items[b])
				{
					flag = true;
					DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 2, array[b], 25 * power / 10);
				}
			}
			if (flag)
			{
				ShowSpecialEffectTips(0);
			}
		}
		RemoveSelf(context);
	}
}
