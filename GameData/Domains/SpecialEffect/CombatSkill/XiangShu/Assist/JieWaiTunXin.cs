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
	// Token: 0x02000325 RID: 805
	public class JieWaiTunXin : AssistSkillBase
	{
		// Token: 0x06003448 RID: 13384 RVA: 0x002283AF File Offset: 0x002265AF
		public JieWaiTunXin()
		{
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x002283B9 File Offset: 0x002265B9
		public JieWaiTunXin(CombatSkillKey skillKey) : base(skillKey, 16409)
		{
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x002283CC File Offset: 0x002265CC
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x00228445 File Offset: 0x00226645
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x0022846C File Offset: 0x0022666C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowEffectTips(context);
			}
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x00228498 File Offset: 0x00226698
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowEffectTips(context);
			}
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x002284CC File Offset: 0x002266CC
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
				byte infection = base.CurrEnemyChar.GetCharacter().GetXiangshuInfection();
				bool flag2 = dataKey.FieldId == 69 && infection > 0;
				if (flag2)
				{
					this._affected = true;
					result = (int)(infection / 2);
				}
				else
				{
					bool flag3 = dataKey.FieldId == 102 && infection < 200;
					if (flag3)
					{
						this._affected = true;
						result = (int)(100 - infection / 2);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000F6E RID: 3950
		private bool _affected;
	}
}
