using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000591 RID: 1425
	public class AttackSkillFiveElementsType : CombatSkillEffectBase
	{
		// Token: 0x06004237 RID: 16951 RVA: 0x00265D65 File Offset: 0x00263F65
		protected AttackSkillFiveElementsType()
		{
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x00265D6F File Offset: 0x00263F6F
		protected AttackSkillFiveElementsType(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x00265D7C File Offset: 0x00263F7C
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x00265D91 File Offset: 0x00263F91
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x00265DA8 File Offset: 0x00263FA8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.DoAffecting(context, this.AffectFiveElementsType, 3);
					bool flag3 = this.AffectFiveElementsType != 5;
					if (flag3)
					{
						this.DoAffecting(context, FiveElementsType.Countered[(int)this.AffectFiveElementsType], 1);
						this.DoAffecting(context, FiveElementsType.Countering[(int)this.AffectFiveElementsType], 2);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x00265E38 File Offset: 0x00264038
		private void DoAffecting(DataContext context, sbyte fiveElementsType, int maxAffectCount)
		{
			AttackSkillFiveElementsType.<>c__DisplayClass11_0 CS$<>8__locals1 = new AttackSkillFiveElementsType.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.fiveElementsType = fiveElementsType;
			CS$<>8__locals1.enemyChar = base.CurrEnemyChar;
			CS$<>8__locals1.powerReduceDict = DomainManager.Combat.GetAllSkillPowerReduceInCombat();
			SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			foreach (short skillId in CS$<>8__locals1.enemyChar.GetRandomUnrepeatedBanableSkillIds(context.Random, maxAffectCount, new Func<short, bool>(CS$<>8__locals1.<DoAffecting>g__Predicate|0), -1, -1))
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.Combat.SilenceSkill(context, CS$<>8__locals1.enemyChar, skillId, 1200, 100);
				}
				else
				{
					DomainManager.Combat.ReduceSkillPowerInCombat(context, new CombatSkillKey(CS$<>8__locals1.enemyChar.GetId(), skillId), effectKey, -30);
				}
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
		}

		// Token: 0x0400138C RID: 5004
		private const sbyte AffectSkillCount = 3;

		// Token: 0x0400138D RID: 5005
		private const sbyte AffectSkillCounteringCount = 2;

		// Token: 0x0400138E RID: 5006
		private const sbyte AffectSkillCounteredCount = 1;

		// Token: 0x0400138F RID: 5007
		private const short SkillCdFrame = 1200;

		// Token: 0x04001390 RID: 5008
		private const sbyte ReducePower = -30;

		// Token: 0x04001391 RID: 5009
		protected sbyte AffectFiveElementsType;
	}
}
