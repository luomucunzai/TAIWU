using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.DefenseAndAssist
{
	// Token: 0x020004FB RID: 1275
	public class HanBingBaMai : DefenseSkillBase
	{
		// Token: 0x06003E5E RID: 15966 RVA: 0x00255915 File Offset: 0x00253B15
		public HanBingBaMai()
		{
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x0025591F File Offset: 0x00253B1F
		public HanBingBaMai(CombatSkillKey skillKey) : base(skillKey, 13503)
		{
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x00255930 File Offset: 0x00253B30
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
				Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
				Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 70, -1, -1, -1, -1), EDataModifyType.AddPercent);
				Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			}
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x002559D4 File Offset: 0x00253BD4
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

		// Token: 0x06003E62 RID: 15970 RVA: 0x00255A30 File Offset: 0x00253C30
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x00255A6C File Offset: 0x00253C6C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || context.Defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x00255AAC File Offset: 0x00253CAC
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = !this._affected || attackerId != base.CharacterId;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x00255AE8 File Offset: 0x00253CE8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect || dataKey.CustomParam0 != 1;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102;
				if (flag2)
				{
					this._affected = true;
					result = -45;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 70;
					if (flag3)
					{
						this._affected = true;
						result = 30;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001266 RID: 4710
		private const sbyte DirectReduceDamagePercent = -45;

		// Token: 0x04001267 RID: 4711
		private const sbyte DirectAddDamagePercent = 30;

		// Token: 0x04001268 RID: 4712
		private bool _affected;
	}
}
