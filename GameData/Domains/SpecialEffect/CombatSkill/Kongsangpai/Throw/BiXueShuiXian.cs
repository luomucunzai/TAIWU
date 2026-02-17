using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw
{
	// Token: 0x02000478 RID: 1144
	public class BiXueShuiXian : CombatSkillEffectBase
	{
		// Token: 0x06003B73 RID: 15219 RVA: 0x00247F19 File Offset: 0x00246119
		public BiXueShuiXian()
		{
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x00247F23 File Offset: 0x00246123
		public BiXueShuiXian(CombatSkillKey skillKey) : base(skillKey, 10403, -1)
		{
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x00247F34 File Offset: 0x00246134
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x00247F5B File Offset: 0x0024615B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x00247F84 File Offset: 0x00246184
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						this._addPower = 0;
						this._addRange = 0;
						base.AppendAffectedData(context, base.CharacterId, 199, EDataModifyType.Add, base.SkillTemplateId);
						base.AppendAffectedData(context, base.CharacterId, 145, EDataModifyType.Add, base.SkillTemplateId);
						base.AppendAffectedData(context, base.CharacterId, 146, EDataModifyType.Add, base.SkillTemplateId);
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x00248068 File Offset: 0x00246268
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = !this.IsSrcSkillPerformed || base.EffectCount <= 0 || (base.IsDirect && combatSkillId == base.SkillTemplateId);
			if (!flag)
			{
				bool flag2 = ((outerMarkCount > 0 || innerMarkCount > 0) && base.IsDirect) ? (attackerId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(defenderId).IsAlly != isAlly) : (defenderId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(attackerId).IsAlly != base.CombatChar.IsAlly);
				if (flag2)
				{
					this._addPower += (int)(10 * (outerMarkCount + innerMarkCount));
					this._addRange += (int)(10 * (outerMarkCount + innerMarkCount));
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
					base.ShowSpecialEffectTips(0);
					base.ReduceEffectCount(1);
				}
			}
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x00248194 File Offset: 0x00246394
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this.IsSrcSkillPerformed || dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = this._addPower;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 145 || dataKey.FieldId == 146;
					if (flag3)
					{
						result = this._addRange;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001168 RID: 4456
		private const sbyte AddPowerUnit = 10;

		// Token: 0x04001169 RID: 4457
		private const sbyte AddRangeUnit = 10;

		// Token: 0x0400116A RID: 4458
		private int _addPower;

		// Token: 0x0400116B RID: 4459
		private int _addRange;
	}
}
