using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Sword
{
	// Token: 0x0200053D RID: 1341
	public class YueNvJianFa : CombatSkillEffectBase
	{
		// Token: 0x06003FDE RID: 16350 RVA: 0x0025BDB6 File Offset: 0x00259FB6
		public YueNvJianFa()
		{
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x0025BDC0 File Offset: 0x00259FC0
		public YueNvJianFa(CombatSkillKey skillKey) : base(skillKey, 2304, -1)
		{
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x0025BDD1 File Offset: 0x00259FD1
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x0025BE0A File Offset: 0x0025A00A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x0025BE44 File Offset: 0x0025A044
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
						base.AddMaxEffectCount(true);
						base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 76 : 109, EDataModifyType.TotalPercent, -1);
						base.ShowSpecialEffectTips(0);
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

		// Token: 0x06003FE3 RID: 16355 RVA: 0x0025BEEC File Offset: 0x0025A0EC
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this.IsSrcSkillPerformed || (base.IsDirect ? attacker.GetId() : defender.GetId()) != base.CharacterId;
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0025BF34 File Offset: 0x0025A134
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0025BF84 File Offset: 0x0025A184
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 76;
				if (flag2)
				{
					result = 50;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 109;
					if (flag3)
					{
						result = -50;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040012D0 RID: 4816
		private const sbyte ChangePursueOdds = 50;
	}
}
