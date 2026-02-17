using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip
{
	// Token: 0x02000380 RID: 896
	public class PoYuSuo : CombatSkillEffectBase
	{
		// Token: 0x060035EA RID: 13802 RVA: 0x0022E619 File Offset: 0x0022C819
		public PoYuSuo()
		{
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x0022E623 File Offset: 0x0022C823
		public PoYuSuo(CombatSkillKey skillKey) : base(skillKey, 12405, -1)
		{
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x0022E634 File Offset: 0x0022C834
		public override void OnEnable(DataContext context)
		{
			this._castingWithMobility = false;
			this._addPower = 0;
			Events.RegisterHandler_CastSkillUseMobilityAsBreathOrStance(new Events.OnCastSkillUseMobilityAsBreathOrStance(this.OnCastSkillUseMobilityAsBreathOrStance));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x0022E698 File Offset: 0x0022C898
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillUseMobilityAsBreathOrStance(new Events.OnCastSkillUseMobilityAsBreathOrStance(this.OnCastSkillUseMobilityAsBreathOrStance));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x0022E6EE File Offset: 0x0022C8EE
		public override void OnDataAdded(DataContext context)
		{
			this.UpdateAffectData(context);
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x0022E6F9 File Offset: 0x0022C8F9
		protected override void OnDirectionChanged(DataContext context)
		{
			this.UpdateAffectData(context);
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x0022E71C File Offset: 0x0022C91C
		private void UpdateAffectData(DataContext context)
		{
			base.ClearAffectedData(context);
			base.AppendAffectedData(context, base.CharacterId, 199, EDataModifyType.AddPercent, base.SkillTemplateId);
			base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 230 : 229, EDataModifyType.Custom, -1);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x0022E770 File Offset: 0x0022C970
		private void OnCastSkillUseMobilityAsBreathOrStance(DataContext context, int charId, short skillId, bool asBreath)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || asBreath == base.IsDirect;
			if (!flag)
			{
				this._castingWithMobility = true;
				base.CombatChar.CanNormalAttackInPrepareSkill = true;
				DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 1000, true);
				DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 1000, false);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x0022E7EC File Offset: 0x0022C9EC
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._castingWithMobility || !hit || attacker != base.CombatChar;
			if (!flag)
			{
				this._addPower += 5;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x0022E848 File Offset: 0x0022CA48
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = this._castingWithMobility && charId == base.CharacterId;
			if (flag)
			{
				DomainManager.Combat.InterruptSkill(context, base.CombatChar, -1);
			}
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x0022E884 File Offset: 0x0022CA84
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this._castingWithMobility || charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = this._addPower > 0;
				if (flag2)
				{
					this._addPower = 0;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				}
				this._castingWithMobility = false;
				base.CombatChar.CanNormalAttackInPrepareSkill = false;
			}
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x0022E8FC File Offset: 0x0022CAFC
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

		// Token: 0x060035F6 RID: 13814 RVA: 0x0022E954 File Offset: 0x0022CB54
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 230 || dataKey.FieldId == 229;
				result = (flag2 || dataValue);
			}
			return result;
		}

		// Token: 0x04000FB7 RID: 4023
		private const sbyte AddPowerUnit = 5;

		// Token: 0x04000FB8 RID: 4024
		private bool _castingWithMobility;

		// Token: 0x04000FB9 RID: 4025
		private int _addPower;
	}
}
