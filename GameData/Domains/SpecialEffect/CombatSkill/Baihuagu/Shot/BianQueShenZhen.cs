using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot
{
	// Token: 0x020005B7 RID: 1463
	public class BianQueShenZhen : CombatSkillEffectBase
	{
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600437D RID: 17277 RVA: 0x0026B9E9 File Offset: 0x00269BE9
		private int AddPowerUnit
		{
			get
			{
				return base.IsDirect ? 5 : 10;
			}
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x0026B9F8 File Offset: 0x00269BF8
		public BianQueShenZhen()
		{
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x0026BA02 File Offset: 0x00269C02
		public BianQueShenZhen(CombatSkillKey skillKey) : base(skillKey, 3203, -1)
		{
		}

		// Token: 0x06004380 RID: 17280 RVA: 0x0026BA13 File Offset: 0x00269C13
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0026BA3A File Offset: 0x00269C3A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x0026BA64 File Offset: 0x00269C64
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.IsDirect ? (base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries()).GetSum() > 0) : base.CombatChar.GetPoison().Subtract(base.CombatChar.GetOldPoison()).IsNonZero();
				if (flag2)
				{
					this._addPower = this.AddPowerUnit * (int)(base.IsDirect ? DomainManager.Combat.HealInjuryInCombat(context, base.CombatChar, base.CombatChar, false) : DomainManager.Combat.HealPoisonInCombat(context, base.CombatChar, base.CombatChar, false));
					base.AppendAffectedData(context, base.CharacterId, 199, EDataModifyType.AddPercent, base.SkillTemplateId);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					base.ShowSpecialEffectTips(0);
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0026BB84 File Offset: 0x00269D84
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x0026BBBC File Offset: 0x00269DBC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
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
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400140A RID: 5130
		private int _addPower;
	}
}
