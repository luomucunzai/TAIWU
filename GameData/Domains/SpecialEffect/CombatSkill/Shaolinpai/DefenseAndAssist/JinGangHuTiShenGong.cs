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
	// Token: 0x02000434 RID: 1076
	public class JinGangHuTiShenGong : DefenseSkillBase
	{
		// Token: 0x060039C9 RID: 14793 RVA: 0x002406D9 File Offset: 0x0023E8D9
		public JinGangHuTiShenGong()
		{
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x002406E3 File Offset: 0x0023E8E3
		public JinGangHuTiShenGong(CombatSkillKey skillKey) : base(skillKey, 1508)
		{
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x002406F4 File Offset: 0x0023E8F4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 242, -1, -1, -1, -1), EDataModifyType.Custom);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 126, -1, -1, -1, -1), EDataModifyType.Custom);
				Events.RegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 70, -1, -1, -1, -1), EDataModifyType.AddPercent);
				Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			}
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x002407CC File Offset: 0x0023E9CC
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

		// Token: 0x060039CD RID: 14797 RVA: 0x0024083C File Offset: 0x0023EA3C
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = defender != base.CombatChar || attacker.NormalAttackBodyPart < 0;
			if (!flag)
			{
				this._canAffectCurrAttack = (base.CombatChar.GetFlawCount()[(int)attacker.NormalAttackBodyPart] == 0);
			}
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x00240880 File Offset: 0x0023EA80
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = defender != base.CombatChar || Config.CombatSkill.Instance[skillId].EquipType != 1 || attacker.SkillAttackBodyPart < 0;
			if (!flag)
			{
				this._canAffectCurrAttack = (base.CombatChar.GetFlawCount()[(int)attacker.SkillAttackBodyPart] == 0);
			}
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x002408D8 File Offset: 0x0023EAD8
		private void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
		{
			bool flag = base.CombatChar != context.Defender || !base.CanAffect;
			if (!flag)
			{
				int transferValue = compareData.OuterDefendValue / 2;
				compareData.InnerDefendValue += transferValue;
				compareData.OuterDefendValue -= transferValue;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x00240934 File Offset: 0x0023EB34
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x00240960 File Offset: 0x0023EB60
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
				bool flag2 = dataKey.FieldId == 126;
				if (flag2)
				{
					result = false;
				}
				else
				{
					EDamageType damageType = (EDamageType)dataKey.CustomParam1;
					bool flag3 = dataKey.FieldId == 242 && damageType == EDamageType.Direct;
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

		// Token: 0x060039D2 RID: 14802 RVA: 0x002409E0 File Offset: 0x0023EBE0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._canAffectCurrAttack || dataKey.CustomParam0 != 0 || !base.CanAffect;
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

		// Token: 0x040010E5 RID: 4325
		private const sbyte AddBounceDamage = 100;

		// Token: 0x040010E6 RID: 4326
		private bool _canAffectCurrAttack;

		// Token: 0x040010E7 RID: 4327
		private bool _affected;
	}
}
