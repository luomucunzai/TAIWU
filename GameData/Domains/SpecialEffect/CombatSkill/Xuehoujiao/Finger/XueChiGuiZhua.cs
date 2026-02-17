using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Finger
{
	// Token: 0x0200023E RID: 574
	public class XueChiGuiZhua : CombatSkillEffectBase
	{
		// Token: 0x06002FAF RID: 12207 RVA: 0x00213FA2 File Offset: 0x002121A2
		public XueChiGuiZhua()
		{
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x00213FAC File Offset: 0x002121AC
		public XueChiGuiZhua(CombatSkillKey skillKey) : base(skillKey, 15206, -1)
		{
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x00213FBD File Offset: 0x002121BD
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x00213FD2 File Offset: 0x002121D2
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x00213FE8 File Offset: 0x002121E8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					Injuries injuries = base.CombatChar.GetInjuries();
					Injuries newInjuries = injuries.Subtract(base.CombatChar.GetOldInjuries());
					List<short> enemyFeatures = base.CurrEnemyChar.GetCharacter().GetFeatureIds();
					bool anyInjuryTransferred = false;
					for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
					{
						short featureId = (base.IsDirect ? BreakFeatureHelper.BodyPart2CrashFeature : BreakFeatureHelper.BodyPart2HurtFeature)[bodyPart];
						bool flag3 = newInjuries.Get(bodyPart, !base.IsDirect) > 0 && enemyFeatures.Contains(featureId);
						if (flag3)
						{
							injuries.Change(bodyPart, !base.IsDirect, -1);
							DomainManager.Combat.AddInjury(context, base.CurrEnemyChar, bodyPart, !base.IsDirect, 1, false, false);
							anyInjuryTransferred = true;
						}
					}
					bool flag4 = anyInjuryTransferred;
					if (flag4)
					{
						DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries, true, true);
						DomainManager.Combat.UpdateBodyDefeatMark(context, base.CurrEnemyChar);
						base.ShowSpecialEffectTips(0);
					}
				}
				base.RemoveSelf(context);
			}
		}
	}
}
