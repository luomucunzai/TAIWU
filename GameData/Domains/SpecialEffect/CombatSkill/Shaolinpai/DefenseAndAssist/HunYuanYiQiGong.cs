using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist
{
	// Token: 0x02000433 RID: 1075
	public class HunYuanYiQiGong : DefenseSkillBase
	{
		// Token: 0x060039BF RID: 14783 RVA: 0x0024036C File Offset: 0x0023E56C
		public HunYuanYiQiGong()
		{
		}

		// Token: 0x060039C0 RID: 14784 RVA: 0x00240376 File Offset: 0x0023E576
		public HunYuanYiQiGong(CombatSkillKey skillKey) : base(skillKey, 1507)
		{
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x00240388 File Offset: 0x0023E588
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 241, -1, -1, -1, -1), EDataModifyType.Custom);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 131, -1, -1, -1, -1), EDataModifyType.Custom);
				Events.RegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 70, -1, -1, -1, -1), EDataModifyType.AddPercent);
				Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			}
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x00240464 File Offset: 0x0023E664
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.UnRegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
			}
			else
			{
				Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			}
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x002404D4 File Offset: 0x0023E6D4
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = defender != base.CombatChar || attacker.NormalAttackBodyPart < 0;
			if (!flag)
			{
				this._canAffectCurrAttack = (base.CombatChar.GetAcupointCount()[(int)attacker.NormalAttackBodyPart] == 0);
			}
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x00240518 File Offset: 0x0023E718
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = defender != base.CombatChar || Config.CombatSkill.Instance[skillId].EquipType != 1 || attacker.SkillAttackBodyPart < 0;
			if (!flag)
			{
				this._canAffectCurrAttack = (base.CombatChar.GetAcupointCount()[(int)attacker.SkillAttackBodyPart] == 0);
			}
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x00240570 File Offset: 0x0023E770
		private void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
		{
			bool flag = base.CombatChar != context.Defender || !base.CanAffect;
			if (!flag)
			{
				int transferValue = compareData.InnerDefendValue / 2;
				compareData.OuterDefendValue += transferValue;
				compareData.InnerDefendValue -= transferValue;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x002405CC File Offset: 0x0023E7CC
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x002405F8 File Offset: 0x0023E7F8
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._canAffectCurrAttack || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 131;
				if (flag2)
				{
					result = false;
				}
				else
				{
					EDamageType damageType = (EDamageType)dataKey.CustomParam1;
					bool flag3 = dataKey.FieldId == 241 && damageType == EDamageType.Direct;
					if (flag3)
					{
						base.ShowSpecialEffectTips(1);
						result = true;
					}
					else
					{
						result = dataValue;
					}
				}
			}
			return result;
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x00240678 File Offset: 0x0023E878
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._canAffectCurrAttack || dataKey.CustomParam0 != 1 || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 70;
				if (flag2)
				{
					this._affected = true;
					result = 100;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040010E2 RID: 4322
		private const sbyte AddBounceDamage = 100;

		// Token: 0x040010E3 RID: 4323
		private bool _canAffectCurrAttack;

		// Token: 0x040010E4 RID: 4324
		private bool _affected;
	}
}
