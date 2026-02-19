using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Finger;

public class XueChiGuiZhua : CombatSkillEffectBase
{
	public XueChiGuiZhua()
	{
	}

	public XueChiGuiZhua(CombatSkillKey skillKey)
		: base(skillKey, 15206, -1)
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
		if (PowerMatchAffectRequire(power))
		{
			Injuries injuries = base.CombatChar.GetInjuries();
			Injuries injuries2 = injuries.Subtract(base.CombatChar.GetOldInjuries());
			List<short> featureIds = base.CurrEnemyChar.GetCharacter().GetFeatureIds();
			bool flag = false;
			for (sbyte b = 0; b < 7; b++)
			{
				short item = (base.IsDirect ? BreakFeatureHelper.BodyPart2CrashFeature : BreakFeatureHelper.BodyPart2HurtFeature)[b];
				if (injuries2.Get(b, !base.IsDirect) > 0 && featureIds.Contains(item))
				{
					injuries.Change(b, !base.IsDirect, -1);
					DomainManager.Combat.AddInjury(context, base.CurrEnemyChar, b, !base.IsDirect, 1);
					flag = true;
				}
			}
			if (flag)
			{
				DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries);
				DomainManager.Combat.UpdateBodyDefeatMark(context, base.CurrEnemyChar);
				ShowSpecialEffectTips(0);
			}
		}
		RemoveSelf(context);
	}
}
