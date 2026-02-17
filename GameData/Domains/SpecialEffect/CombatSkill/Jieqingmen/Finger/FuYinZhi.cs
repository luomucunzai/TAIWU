using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger
{
	// Token: 0x020004F3 RID: 1267
	public class FuYinZhi : CombatSkillEffectBase
	{
		// Token: 0x06003E36 RID: 15926 RVA: 0x00254E9F File Offset: 0x0025309F
		public FuYinZhi()
		{
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x00254EB1 File Offset: 0x002530B1
		public FuYinZhi(CombatSkillKey skillKey) : base(skillKey, 13103, -1)
		{
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x00254ECA File Offset: 0x002530CA
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_AddInjury(new Events.OnAddInjury(this.OnAddInjury));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x00254F03 File Offset: 0x00253103
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_AddInjury(new Events.OnAddInjury(this.OnAddInjury));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x00254F3C File Offset: 0x0025313C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						this._affectEnemyId = base.CurrEnemyChar.GetId();
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x00254FCC File Offset: 0x002531CC
		private void OnAddInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte value, bool changeToOld)
		{
			bool flag = character.GetId() != this._affectEnemyId || isInner == base.IsDirect || changeToOld;
			if (!flag)
			{
				int changeCount = 0;
				int i = 0;
				while (i < (int)value && changeCount < base.EffectCount)
				{
					bool flag2 = context.Random.CheckPercentProb((int)this.AffectOdds);
					if (flag2)
					{
						changeCount++;
					}
					i++;
				}
				bool flag3 = changeCount > 0;
				if (flag3)
				{
					DomainManager.Combat.ChangeToOldInjury(context, character, bodyPart, isInner, changeCount);
					base.ShowSpecialEffectTips(0);
					base.ReduceEffectCount(changeCount);
				}
			}
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x00255068 File Offset: 0x00253268
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0400125A RID: 4698
		private sbyte AffectOdds = 60;

		// Token: 0x0400125B RID: 4699
		private int _affectEnemyId;
	}
}
