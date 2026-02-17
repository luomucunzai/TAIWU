using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Agile
{
	// Token: 0x0200025A RID: 602
	public class FeiShiDaNuoWu : AgileSkillBase
	{
		// Token: 0x06003036 RID: 12342 RVA: 0x002165CB File Offset: 0x002147CB
		public FeiShiDaNuoWu()
		{
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x002165D5 File Offset: 0x002147D5
		public FeiShiDaNuoWu(CombatSkillKey skillKey) : base(skillKey, 15604)
		{
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x002165E5 File Offset: 0x002147E5
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x00216602 File Offset: 0x00214802
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x00216620 File Offset: 0x00214820
		private void OnCastLegSkillWithAgile(DataContext context, CombatCharacter combatChar, short legSkillId)
		{
			bool flag = !base.CanAffect || combatChar.GetId() != base.CharacterId;
			if (!flag)
			{
				this.AutoRemove = false;
				this._affectingLegSkill = legSkillId;
				bool flag2 = this.AffectDatas == null || this.AffectDatas.Count == 0;
				if (flag2)
				{
					base.AppendAffectedData(context, 327, EDataModifyType.Custom, legSkillId);
				}
				base.ShowSpecialEffectTips(0);
				Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			}
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x002166A4 File Offset: 0x002148A4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
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
					bool flag2 = power != 100;
					if (!flag2)
					{
						int agileAdd = 20 * MoveSpecialConstants.MaxMobility / 100;
						base.ChangeMobilityValue(context, base.CombatChar, agileAdd);
						base.ShowSpecialEffectTips(1);
					}
				}
			}
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x00216734 File Offset: 0x00214934
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CombatSkillId != this._affectingLegSkill || dataKey.CustomParam0 != (base.IsDirect ? 0 : 1);
			bool result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				bool flag2 = dataKey.FieldId != 327 || dataKey.CustomParam2 != 1;
				result = (flag2 && base.GetModifiedValue(dataKey, dataValue));
			}
			return result;
		}

		// Token: 0x04000E51 RID: 3665
		private short _affectingLegSkill;

		// Token: 0x04000E52 RID: 3666
		private const int AgileSkillRemainingTimeAddPercent = 20;
	}
}
