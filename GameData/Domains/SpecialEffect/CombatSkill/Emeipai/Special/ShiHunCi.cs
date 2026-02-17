using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Special
{
	// Token: 0x02000544 RID: 1348
	public class ShiHunCi : CombatSkillEffectBase
	{
		// Token: 0x06004003 RID: 16387 RVA: 0x0025C95D File Offset: 0x0025AB5D
		public ShiHunCi()
		{
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x0025C967 File Offset: 0x0025AB67
		public ShiHunCi(CombatSkillKey skillKey) : base(skillKey, 2401, -1)
		{
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0025C978 File Offset: 0x0025AB78
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0025C9B1 File Offset: 0x0025ABB1
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x0025C9EC File Offset: 0x0025ABEC
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
						base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 75 : 108, EDataModifyType.TotalPercent, -1);
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

		// Token: 0x06004008 RID: 16392 RVA: 0x0025CA8C File Offset: 0x0025AC8C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this.IsSrcSkillPerformed || !isFightBack || (base.IsDirect ? attacker : defender) != base.CombatChar;
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x0025CAD8 File Offset: 0x0025ACD8
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x0025CB28 File Offset: 0x0025AD28
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
				bool flag2 = dataKey.FieldId == 75;
				if (flag2)
				{
					result = 50;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 108;
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

		// Token: 0x040012D7 RID: 4823
		private const sbyte ChangeHitOdds = 50;
	}
}
