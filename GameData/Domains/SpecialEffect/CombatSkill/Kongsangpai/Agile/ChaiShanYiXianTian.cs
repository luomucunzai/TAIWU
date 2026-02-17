using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Agile
{
	// Token: 0x020004A2 RID: 1186
	public class ChaiShanYiXianTian : AgileSkillBase
	{
		// Token: 0x06003C86 RID: 15494 RVA: 0x0024DF49 File Offset: 0x0024C149
		public ChaiShanYiXianTian()
		{
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x0024DF53 File Offset: 0x0024C153
		public ChaiShanYiXianTian(CombatSkillKey skillKey) : base(skillKey, 10503)
		{
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x0024DF64 File Offset: 0x0024C164
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x0024DFB0 File Offset: 0x0024C1B0
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x0024DFFC File Offset: 0x0024C1FC
		private void OnCastLegSkillWithAgile(DataContext context, CombatCharacter combatChar, short legSkillId)
		{
			bool flag = combatChar != base.CombatChar || combatChar.GetAutoCastingSkill();
			if (!flag)
			{
				this.AutoRemove = false;
				this._castingLegSkill = legSkillId;
				base.AppendAffectedData(context, base.CharacterId, 102, EDataModifyType.AddPercent, -1);
				Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			}
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x0024E054 File Offset: 0x0024C254
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || base.CombatChar != defender;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x0024E090 File Offset: 0x0024C290
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || base.CombatChar != context.Defender;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x0024E0D0 File Offset: 0x0024C2D0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != this._castingLegSkill;
			if (!flag)
			{
				base.ClearAffectedData(context);
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

		// Token: 0x06003C8E RID: 15502 RVA: 0x0024E134 File Offset: 0x0024C334
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
				bool flag2 = dataKey.FieldId == 102 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					this._affected = true;
					result = -60;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040011D2 RID: 4562
		private const sbyte ReduceDamagePercent = -60;

		// Token: 0x040011D3 RID: 4563
		private short _castingLegSkill;

		// Token: 0x040011D4 RID: 4564
		private bool _affected;
	}
}
