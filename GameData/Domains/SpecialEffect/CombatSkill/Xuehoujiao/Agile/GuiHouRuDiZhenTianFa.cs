using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Agile
{
	// Token: 0x0200025C RID: 604
	public class GuiHouRuDiZhenTianFa : AgileSkillBase
	{
		// Token: 0x0600303F RID: 12351 RVA: 0x002167CB File Offset: 0x002149CB
		public GuiHouRuDiZhenTianFa()
		{
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x002167D5 File Offset: 0x002149D5
		public GuiHouRuDiZhenTianFa(CombatSkillKey skillKey) : base(skillKey, 15603)
		{
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x002167E5 File Offset: 0x002149E5
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x00216802 File Offset: 0x00214A02
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x00216820 File Offset: 0x00214A20
		private void OnCastLegSkillWithAgile(DataContext context, CombatCharacter combatChar, short legSkillId)
		{
			bool flag = !base.CanAffect || combatChar != base.CombatChar;
			if (!flag)
			{
				this.AutoRemove = false;
				this._affectingLegSkill = legSkillId;
				bool flag2 = this.AffectDatas == null || this.AffectDatas.Count == 0;
				if (flag2)
				{
					base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, legSkillId);
				}
				base.ShowSpecialEffectTips(0);
				Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			}
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x002168A4 File Offset: 0x00214AA4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != this._affectingLegSkill;
			if (!flag)
			{
				Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
				bool agileSkillChanged = this.AgileSkillChanged;
				if (agileSkillChanged)
				{
					base.RemoveSelf(context);
				}
				else
				{
					this.AutoRemove = true;
				}
			}
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x00216900 File Offset: 0x00214B00
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CombatSkillId != this._affectingLegSkill || dataKey.CustomParam0 != (base.IsDirect ? 0 : 1);
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
					result = 30;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E53 RID: 3667
		private const sbyte AddDamagePercent = 30;

		// Token: 0x04000E54 RID: 3668
		private short _affectingLegSkill;
	}
}
