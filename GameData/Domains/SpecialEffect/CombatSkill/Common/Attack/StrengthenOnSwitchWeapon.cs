using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x020005A6 RID: 1446
	public class StrengthenOnSwitchWeapon : CombatSkillEffectBase
	{
		// Token: 0x060042F8 RID: 17144 RVA: 0x0026931A File Offset: 0x0026751A
		protected StrengthenOnSwitchWeapon()
		{
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x00269324 File Offset: 0x00267524
		protected StrengthenOnSwitchWeapon(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x00269334 File Offset: 0x00267534
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			}
			else
			{
				this._addPower = 0;
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			}
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x002693C0 File Offset: 0x002675C0
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			}
			else
			{
				Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			}
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x00269410 File Offset: 0x00267610
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				short oldType = oldWeapon.Template.ItemSubType;
				short newType = newWeapon.Template.ItemSubType;
				bool flag2 = newType != this.RequireWeaponSubType || oldType == this.RequireWeaponSubType;
				if (!flag2)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						bool flag3 = !DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, true);
						if (!flag3)
						{
							DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
							base.ShowSpecialEffectTips(0);
						}
					}
					else
					{
						bool flag4 = this._addPower == 0;
						if (flag4)
						{
							this._addPower = 20;
							DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
							base.ShowSpecialEffectTips(0);
						}
					}
				}
			}
		}

		// Token: 0x060042FD RID: 17149 RVA: 0x002694F8 File Offset: 0x002676F8
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.CombatChar.GetAutoCastingSkill();
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
			}
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x00269554 File Offset: 0x00267754
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || this._addPower == 0;
			if (!flag)
			{
				this._addPower = 0;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x002695A8 File Offset: 0x002677A8
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

		// Token: 0x040013D3 RID: 5075
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x040013D4 RID: 5076
		private const sbyte AddPower = 20;

		// Token: 0x040013D5 RID: 5077
		protected short RequireWeaponSubType;

		// Token: 0x040013D6 RID: 5078
		private int _addPower;
	}
}
