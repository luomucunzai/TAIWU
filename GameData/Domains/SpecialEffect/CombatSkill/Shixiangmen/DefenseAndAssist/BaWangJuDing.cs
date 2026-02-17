using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.DefenseAndAssist
{
	// Token: 0x020003FF RID: 1023
	public class BaWangJuDing : AssistSkillBase
	{
		// Token: 0x060038AA RID: 14506 RVA: 0x0023B6D8 File Offset: 0x002398D8
		public BaWangJuDing()
		{
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x0023B6E2 File Offset: 0x002398E2
		public BaWangJuDing(CombatSkillKey skillKey) : base(skillKey, 6603)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x0023B6FC File Offset: 0x002398FC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 69 : 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x0023B76B File Offset: 0x0023996B
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x0023B788 File Offset: 0x00239988
		private void OnCombatBegin(DataContext context)
		{
			this._changeDamage = ((base.CombatChar.GetUsingWeaponIndex() < 3) ? (base.CombatChar.GetWeaponData(-1).Item.GetWeight() / 100) : 0);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x0023B7D8 File Offset: 0x002399D8
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				this._changeDamage = ((base.CombatChar.GetUsingWeaponIndex() < 3) ? (newWeapon.Item.GetWeight() / 100) : 0);
			}
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x0023B81E File Offset: 0x00239A1E
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x0023B830 File Offset: 0x00239A30
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
				result = (base.IsDirect ? this._changeDamage : (-this._changeDamage));
			}
			return result;
		}

		// Token: 0x0400109A RID: 4250
		private int _changeDamage;
	}
}
