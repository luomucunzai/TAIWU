using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Agile
{
	// Token: 0x02000506 RID: 1286
	public class ShenXianSuo : AgileSkillBase
	{
		// Token: 0x06003EA7 RID: 16039 RVA: 0x00256A7B File Offset: 0x00254C7B
		public ShenXianSuo()
		{
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x00256A85 File Offset: 0x00254C85
		public ShenXianSuo(CombatSkillKey skillKey) : base(skillKey, 13405)
		{
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x00256A98 File Offset: 0x00254C98
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AutoRemove = false;
			this._autoMoving = false;
			this._needChangeDistance = 0;
			base.CreateAffectedData(149, EDataModifyType.Custom, -1);
			base.CreateAffectedData(157, EDataModifyType.Custom, -1);
			Events.RegisterHandler_NormalAttackPrepareEnd(new Events.OnNormalAttackPrepareEnd(this.OnNormalAttackPrepareEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillChangeDistance(new Events.OnPrepareSkillChangeDistance(this.OnPrepareSkillChangeDistance));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x00256B28 File Offset: 0x00254D28
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackPrepareEnd(new Events.OnNormalAttackPrepareEnd(this.OnNormalAttackPrepareEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillChangeDistance(new Events.OnPrepareSkillChangeDistance(this.OnPrepareSkillChangeDistance));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x00256B88 File Offset: 0x00254D88
		private void OnNormalAttackPrepareEnd(DataContext context, int charId, bool isAlly)
		{
			bool flag = charId != base.CharacterId || !base.CanAffect;
			if (!flag)
			{
				this.ChangeDistanceOnAttack(context);
			}
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x00256BBC File Offset: 0x00254DBC
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = this._needChangeDistance == 0 || attacker != base.CombatChar || base.CombatChar.NormalAttackLeftRepeatTimes > 0;
			if (!flag)
			{
				this.RestoreDistance(context);
			}
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x00256BFC File Offset: 0x00254DFC
		private void OnPrepareSkillChangeDistance(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = skillId < 0 || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this.ChangeDistanceOnAttack(context);
			}
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x00256C34 File Offset: 0x00254E34
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this._needChangeDistance == 0 || charId != base.CharacterId;
			if (!flag)
			{
				this.RestoreDistance(context);
			}
		}

		// Token: 0x06003EAF RID: 16047 RVA: 0x00256C68 File Offset: 0x00254E68
		protected override void OnMoveSkillChanged(DataContext context, DataUid dataUid)
		{
			base.OnMoveSkillChanged(context, dataUid);
			bool flag = this.AgileSkillChanged && this._needChangeDistance == 0;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x00256CA0 File Offset: 0x00254EA0
		private void ChangeDistanceOnAttack(DataContext context)
		{
			short distanceBefore = DomainManager.Combat.GetCurrentDistance();
			this._autoMoving = true;
			DomainManager.Combat.ChangeDistance(context, base.CombatChar, base.IsDirect ? -40 : 40);
			this._autoMoving = false;
			this._needChangeDistance = (int)(distanceBefore - DomainManager.Combat.GetCurrentDistance());
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x00256D04 File Offset: 0x00254F04
		private void RestoreDistance(DataContext context)
		{
			this._autoMoving = true;
			DomainManager.Combat.ChangeDistance(context, base.CombatChar, this._needChangeDistance);
			this._autoMoving = false;
			this._needChangeDistance = 0;
			bool agileSkillChanged = this.AgileSkillChanged;
			if (agileSkillChanged)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x00256D54 File Offset: 0x00254F54
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.FieldId == 149 && dataKey.CustomParam0 >= 0 && DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CustomParam0).IsAlly != base.CombatChar.IsAlly && base.CanAffect;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 157;
				if (flag2)
				{
					result = !this._autoMoving;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x0400127E RID: 4734
		private const sbyte ChangeDistance = 40;

		// Token: 0x0400127F RID: 4735
		private bool _autoMoving;

		// Token: 0x04001280 RID: 4736
		private int _needChangeDistance;
	}
}
