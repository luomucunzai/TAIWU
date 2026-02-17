using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x0200032F RID: 815
	public class SiJie : AssistSkillBase
	{
		// Token: 0x06003477 RID: 13431 RVA: 0x00228C30 File Offset: 0x00226E30
		public SiJie()
		{
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x00228C3A File Offset: 0x00226E3A
		public SiJie(CombatSkillKey skillKey) : base(skillKey, 16416)
		{
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x00228C4C File Offset: 0x00226E4C
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x00228CC5 File Offset: 0x00226EC5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x00228CEC File Offset: 0x00226EEC
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowEffectTips(context);
			}
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x00228D18 File Offset: 0x00226F18
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowEffectTips(context);
			}
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x00228D4C File Offset: 0x00226F4C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool hasGodTrick = base.CombatChar.GetTricks().ContainsTrick(21);
				this._affected = true;
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = (hasGodTrick ? 50 : -50);
				}
				else
				{
					bool flag3 = dataKey.FieldId == 102;
					if (flag3)
					{
						result = (hasGodTrick ? -50 : 50);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000F79 RID: 3961
		private const sbyte ChangeDamage = 50;

		// Token: 0x04000F7A RID: 3962
		private bool _affected;
	}
}
