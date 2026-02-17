using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Neigong
{
	// Token: 0x020001CF RID: 463
	public class QianChuiBaiLianPian : CombatSkillEffectBase
	{
		// Token: 0x06002D25 RID: 11557 RVA: 0x0020A682 File Offset: 0x00208882
		public QianChuiBaiLianPian()
		{
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x0020A693 File Offset: 0x00208893
		public QianChuiBaiLianPian(CombatSkillKey skillKey) : base(skillKey, 9003, -1)
		{
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x0020A6AC File Offset: 0x002088AC
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 102 : 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x0020A713 File Offset: 0x00208913
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x0020A728 File Offset: 0x00208928
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateDamageChangePercent();
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x0020A744 File Offset: 0x00208944
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			this.UpdateDamageChangePercent();
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x0020A750 File Offset: 0x00208950
		private void UpdateDamageChangePercent()
		{
			Weapon currWeapon = DomainManager.Item.GetElement_Weapons(DomainManager.Combat.GetUsingWeaponKey(base.CombatChar).Id);
			int refineCount = 0;
			bool flag = ModificationStateHelper.IsActive(currWeapon.GetModificationState(), 2);
			if (flag)
			{
				refineCount = (int)DomainManager.Item.GetRefinedEffects(currWeapon.GetItemKey()).GetTotalRefiningCount();
			}
			this._damageChangePercent = (int)this.DamageChangeUnit * refineCount;
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x0020A7B8 File Offset: 0x002089B8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102;
				if (flag2)
				{
					result = -this._damageChangePercent;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 69;
					if (flag3)
					{
						result = this._damageChangePercent;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D90 RID: 3472
		private sbyte DamageChangeUnit = 4;

		// Token: 0x04000D91 RID: 3473
		private int _damageChangePercent;
	}
}
