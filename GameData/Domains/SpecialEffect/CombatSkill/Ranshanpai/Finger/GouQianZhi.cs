using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Finger
{
	// Token: 0x0200045D RID: 1117
	public class GouQianZhi : CombatSkillEffectBase
	{
		// Token: 0x06003AD6 RID: 15062 RVA: 0x00245765 File Offset: 0x00243965
		public GouQianZhi()
		{
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x0024576F File Offset: 0x0024396F
		public GouQianZhi(CombatSkillKey skillKey) : base(skillKey, 7101, -1)
		{
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x00245780 File Offset: 0x00243980
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x002457A7 File Offset: 0x002439A7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x002457D0 File Offset: 0x002439D0
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index < 2 || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				bool flag2 = index == 2;
				if (flag2)
				{
					this._addDamagePercent = 15 * base.CurrEnemyChar.GetFlawCollection().BodyPartDict[base.CombatChar.SkillAttackBodyPart].Count;
					this._affected = false;
					base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x00245868 File Offset: 0x00243A68
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x002458A0 File Offset: 0x00243AA0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CombatSkillId != base.SkillTemplateId || dataKey.CustomParam0 != (base.IsDirect ? 0 : 1);
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
					bool flag3 = !this._affected;
					if (flag3)
					{
						this._affected = true;
						base.ShowSpecialEffectTips(0);
					}
					result = this._addDamagePercent;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400113A RID: 4410
		private const int AddDamageUnit = 15;

		// Token: 0x0400113B RID: 4411
		private int _addDamagePercent;

		// Token: 0x0400113C RID: 4412
		private bool _affected;
	}
}
