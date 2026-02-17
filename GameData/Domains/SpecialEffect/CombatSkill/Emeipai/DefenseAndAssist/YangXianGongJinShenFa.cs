using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist
{
	// Token: 0x02000567 RID: 1383
	public class YangXianGongJinShenFa : DefenseSkillBase
	{
		// Token: 0x060040D8 RID: 16600 RVA: 0x00260646 File Offset: 0x0025E846
		public YangXianGongJinShenFa()
		{
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x00260650 File Offset: 0x0025E850
		public YangXianGongJinShenFa(CombatSkillKey skillKey) : base(skillKey, 2606)
		{
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x00260660 File Offset: 0x0025E860
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.TotalPercent);
				Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
				Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 70, -1, -1, -1, -1), EDataModifyType.AddPercent);
				Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			}
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x00260704 File Offset: 0x0025E904
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
				Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			}
			else
			{
				Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			}
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x00260760 File Offset: 0x0025E960
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x0026079C File Offset: 0x0025E99C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || context.DefenderId != base.CharacterId;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060040DE RID: 16606 RVA: 0x002607DC File Offset: 0x0025E9DC
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x00260804 File Offset: 0x0025EA04
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
				int usableTrickCount = base.CombatChar.UsableTrickCount;
				int changeDamageCount = base.IsDirect ? (usableTrickCount / 3) : usableTrickCount;
				bool flag2 = changeDamageCount <= 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 102;
					if (flag3)
					{
						this._affected = true;
						result = -10 * changeDamageCount;
					}
					else
					{
						bool flag4 = dataKey.FieldId == 70;
						if (flag4)
						{
							this._affected = true;
							result = 10 * changeDamageCount;
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001313 RID: 4883
		private const sbyte TrickCountUnit = 3;

		// Token: 0x04001314 RID: 4884
		private const sbyte DirectChangeDamageUnit = -10;

		// Token: 0x04001315 RID: 4885
		private const sbyte ReverseChangeDamageUnit = 10;

		// Token: 0x04001316 RID: 4886
		private bool _affected;
	}
}
