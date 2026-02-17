using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200058F RID: 1423
	public class AttackHitType : CombatSkillEffectBase
	{
		// Token: 0x06004227 RID: 16935 RVA: 0x002658DD File Offset: 0x00263ADD
		protected AttackHitType()
		{
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x002658E7 File Offset: 0x00263AE7
		protected AttackHitType(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x002658F4 File Offset: 0x00263AF4
		public override void OnEnable(DataContext context)
		{
			this._affecting = false;
			base.CreateAffectedData(69, EDataModifyType.AddPercent, base.SkillTemplateId);
			base.CreateAffectedData(327, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x00265950 File Offset: 0x00263B50
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x00265978 File Offset: 0x00263B78
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || !hit || index > 2 || hitType != this.AffectHitType;
			if (!flag)
			{
				this._affecting = true;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x002659C8 File Offset: 0x00263BC8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x00265A00 File Offset: 0x00263C00
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this._affecting || dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || dataKey.CustomParam0 != (base.IsDirect ? 0 : 1);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = 40;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x00265A6C File Offset: 0x00263C6C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey && dataKey.FieldId == 327 && dataKey.CustomParam2 == 1;
			return !flag && base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x04001385 RID: 4997
		private const sbyte AddDamage = 40;

		// Token: 0x04001386 RID: 4998
		protected sbyte AffectHitType;

		// Token: 0x04001387 RID: 4999
		private bool _affecting;
	}
}
