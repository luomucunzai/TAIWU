using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot
{
	// Token: 0x020001BC RID: 444
	public class HuangLongMuYuan : CombatSkillEffectBase
	{
		// Token: 0x06002C92 RID: 11410 RVA: 0x00207EA6 File Offset: 0x002060A6
		public HuangLongMuYuan()
		{
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x00207EB0 File Offset: 0x002060B0
		public HuangLongMuYuan(CombatSkillKey skillKey) : base(skillKey, 9406, -1)
		{
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x00207EC1 File Offset: 0x002060C1
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x00207EE8 File Offset: 0x002060E8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x00207F10 File Offset: 0x00206110
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId) && base.PowerMatchAffectRequire((int)power, 0);
			if (flag)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x00207F48 File Offset: 0x00206148
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || (base.IsDirect ? outerMarkCount : innerMarkCount) <= 0 || base.CombatChar.GetTrickCount(12) <= 0 || base.EffectCount <= 0;
			if (!flag)
			{
				DomainManager.Combat.RemoveTrick(context, base.CombatChar, 12, 1, true, -1);
				for (int i = 0; i < 2; i++)
				{
					DomainManager.Combat.AddRandomInjury(context, base.CurrEnemyChar, !base.IsDirect, 1, 1, true, -1);
				}
				base.ReduceEffectCount(1);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04000D6F RID: 3439
		private const int AddInjuryCount = 2;
	}
}
