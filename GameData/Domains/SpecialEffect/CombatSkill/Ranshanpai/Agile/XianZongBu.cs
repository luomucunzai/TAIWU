using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Agile
{
	// Token: 0x0200046E RID: 1134
	public class XianZongBu : AgileSkillBase
	{
		// Token: 0x06003B32 RID: 15154 RVA: 0x00246FE5 File Offset: 0x002451E5
		public XianZongBu()
		{
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x00246FEF File Offset: 0x002451EF
		public XianZongBu(CombatSkillKey skillKey) : base(skillKey, 7404)
		{
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x00247000 File Offset: 0x00245200
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AutoRemove = false;
			this._autoMoving = false;
			this._needChangeDistance = 0;
			this._attackSkillKey.CharId = -1;
			base.CreateAffectedData(157, EDataModifyType.Custom, -1);
			Events.RegisterHandler_NormalAttackPrepareEnd(new Events.OnNormalAttackPrepareEnd(this.OnNormalAttackPrepareEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillChangeDistance(new Events.OnPrepareSkillChangeDistance(this.OnPrepareSkillChangeDistance));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x00247090 File Offset: 0x00245290
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackPrepareEnd(new Events.OnNormalAttackPrepareEnd(this.OnNormalAttackPrepareEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillChangeDistance(new Events.OnPrepareSkillChangeDistance(this.OnPrepareSkillChangeDistance));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x002470F0 File Offset: 0x002452F0
		private void OnNormalAttackPrepareEnd(DataContext context, int charId, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly || !base.CanAffect || DomainManager.Combat.GetCombatCharacter(base.CombatChar.IsAlly, true) != base.CombatChar;
			if (!flag)
			{
				this.ChangeDistanceOnAttack(context);
			}
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x00247148 File Offset: 0x00245348
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = this._needChangeDistance == 0 || defender != base.CombatChar;
			if (!flag)
			{
				this.RestoreDistance(context);
			}
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x0024717C File Offset: 0x0024537C
		private void OnPrepareSkillChangeDistance(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = skillId < 0 || defender != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this._attackSkillKey = new CombatSkillKey(attacker.GetId(), skillId);
				this.ChangeDistanceOnAttack(context);
			}
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x002471C8 File Offset: 0x002453C8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this._needChangeDistance == 0 || charId != this._attackSkillKey.CharId || skillId != this._attackSkillKey.SkillTemplateId;
			if (!flag)
			{
				this.RestoreDistance(context);
			}
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x00247210 File Offset: 0x00245410
		protected override void OnMoveSkillChanged(DataContext context, DataUid dataUid)
		{
			base.OnMoveSkillChanged(context, dataUid);
			bool flag = this.AgileSkillChanged && this._needChangeDistance == 0;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x00247248 File Offset: 0x00245448
		private void ChangeDistanceOnAttack(DataContext context)
		{
			short distanceBefore = DomainManager.Combat.GetCurrentDistance();
			this._autoMoving = true;
			DomainManager.Combat.ChangeDistance(context, base.CombatChar, base.IsDirect ? -20 : 20);
			this._autoMoving = false;
			this._needChangeDistance = (int)(distanceBefore - DomainManager.Combat.GetCurrentDistance());
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x002472AC File Offset: 0x002454AC
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

		// Token: 0x06003B3D RID: 15165 RVA: 0x002472FC File Offset: 0x002454FC
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool result;
			if (flag)
			{
				result = dataValue;
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

		// Token: 0x04001155 RID: 4437
		private const sbyte ChangeDistance = 20;

		// Token: 0x04001156 RID: 4438
		private bool _autoMoving;

		// Token: 0x04001157 RID: 4439
		private int _needChangeDistance;

		// Token: 0x04001158 RID: 4440
		private CombatSkillKey _attackSkillKey;
	}
}
