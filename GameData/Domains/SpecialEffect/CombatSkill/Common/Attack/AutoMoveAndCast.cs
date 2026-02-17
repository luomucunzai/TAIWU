using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000592 RID: 1426
	public abstract class AutoMoveAndCast : CombatSkillEffectBase
	{
		// Token: 0x0600423D RID: 16957 RVA: 0x00265F34 File Offset: 0x00264134
		private static CombatSkillItem GetConfig(short skillId)
		{
			return Config.CombatSkill.Instance[skillId];
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x00265F41 File Offset: 0x00264141
		private bool IsAffectPower(int power)
		{
			return base.IsDirect ? (!base.PowerMatchAffectRequire(power, 0)) : base.PowerMatchAffectRequire(power, 0);
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x00265F60 File Offset: 0x00264160
		private bool IsAffectSkill(short skillId)
		{
			return base.IsDirect ? (skillId == base.SkillTemplateId) : (base.EffectCount > 0 && skillId != base.SkillTemplateId && AutoMoveAndCast.GetConfig(skillId).Type == this.RequireSkillType && AutoMoveAndCast.GetConfig(skillId).Grade < base.SkillConfig.Grade);
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x00265FC0 File Offset: 0x002641C0
		private bool IsRequireWeapon(CombatWeaponData weaponData)
		{
			return weaponData.Template.ItemSubType == this.RequireWeaponType;
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06004241 RID: 16961
		protected abstract bool MoveForward { get; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06004242 RID: 16962
		protected abstract short RequireWeaponType { get; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06004243 RID: 16963
		protected abstract sbyte RequireSkillType { get; }

		// Token: 0x06004244 RID: 16964 RVA: 0x00265FD5 File Offset: 0x002641D5
		protected AutoMoveAndCast()
		{
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x00265FDF File Offset: 0x002641DF
		protected AutoMoveAndCast(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x00265FEC File Offset: 0x002641EC
		public override void OnEnable(DataContext context)
		{
			this._autoCastSkillId = -1;
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x0026604C File Offset: 0x0026424C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x002660A4 File Offset: 0x002642A4
		private void OnCombatBegin(DataContext context)
		{
			bool flag = !base.IsDirect;
			if (flag)
			{
				base.AddMaxEffectCount(false);
			}
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x002660C8 File Offset: 0x002642C8
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = base.IsDirect || charId != base.CharacterId;
			if (!flag)
			{
				bool flag2 = this.IsRequireWeapon(oldWeapon) || !this.IsRequireWeapon(newWeapon);
				if (!flag2)
				{
					base.AddEffectCount(1);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x00266124 File Offset: 0x00264324
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				bool flag2 = skillId == this._autoCastSkillId;
				if (flag2)
				{
					DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
				}
			}
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x00266174 File Offset: 0x00264374
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId == base.CharacterId && skillId == this._autoCastSkillId;
			if (flag)
			{
				this._autoCastSkillId = -1;
			}
			bool flag2 = charId != base.CharacterId || !this.IsAffectPower((int)power) || !this.IsAffectSkill(skillId) || interrupted;
			if (!flag2)
			{
				DomainManager.Combat.ChangeDistance(context, base.CombatChar, this.MoveForward ? -20 : 20);
				bool flag3 = !DomainManager.Combat.InAttackRange(base.CombatChar);
				if (!flag3)
				{
					this._autoCastSkillId = (base.IsDirect ? DomainManager.Combat.GetRandomAttackSkill(base.CombatChar, this.RequireSkillType, Config.CombatSkill.Instance[base.SkillTemplateId].Grade, context.Random, true, -1) : base.SkillTemplateId);
					bool flag4 = this._autoCastSkillId < 0;
					if (!flag4)
					{
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, this._autoCastSkillId, ECombatCastFreePriority.AutoMoveAndCast);
						base.ShowSpecialEffectTips(0);
						bool flag5 = !base.IsDirect;
						if (flag5)
						{
							base.ReduceEffectCount(1);
						}
					}
				}
			}
		}

		// Token: 0x04001392 RID: 5010
		private const sbyte MoveDistance = 20;

		// Token: 0x04001393 RID: 5011
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x04001394 RID: 5012
		private short _autoCastSkillId;
	}
}
