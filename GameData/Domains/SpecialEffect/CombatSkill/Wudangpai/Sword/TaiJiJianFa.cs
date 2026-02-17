using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Sword
{
	// Token: 0x020003BE RID: 958
	public class TaiJiJianFa : CombatSkillEffectBase
	{
		// Token: 0x0600373A RID: 14138 RVA: 0x00234950 File Offset: 0x00232B50
		public TaiJiJianFa()
		{
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x0023495A File Offset: 0x00232B5A
		public TaiJiJianFa(CombatSkillKey skillKey) : base(skillKey, 4206, -1)
		{
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x0023496B File Offset: 0x00232B6B
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x002349A4 File Offset: 0x00232BA4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x002349E0 File Offset: 0x00232BE0
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
						base.AppendAffectedData(context, base.CharacterId, 107, EDataModifyType.TotalPercent, -1);
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

		// Token: 0x0600373F RID: 14143 RVA: 0x00234A74 File Offset: 0x00232C74
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this.IsSrcSkillPerformed || defender != base.CombatChar || pursueIndex > 0 || attacker.NormalAttackHitType == 3;
			if (!flag)
			{
				bool flag2 = !hit;
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 2, base.IsDirect ? 22 : 23);
					base.ShowSpecialEffectTips(0);
				}
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x00234AE8 File Offset: 0x00232CE8
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x00234B38 File Offset: 0x00232D38
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this.IsSrcSkillPerformed || dataKey.CharId != base.CharacterId || dataKey.CombatSkillId >= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 107;
				if (flag2)
				{
					result = -80;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001024 RID: 4132
		private const sbyte ReduceHitOdds = -80;
	}
}
