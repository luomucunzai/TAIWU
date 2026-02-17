using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.DefenseAndAssist
{
	// Token: 0x02000207 RID: 519
	public class ShiJiaZhiXiang : AssistSkillBase
	{
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06002EBF RID: 11967 RVA: 0x002108B8 File Offset: 0x0020EAB8
		private int UsingWeaponWeight
		{
			get
			{
				return DomainManager.Combat.GetUsingWeapon(base.CombatChar).GetWeight();
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x002108CF File Offset: 0x0020EACF
		private bool Affecting
		{
			get
			{
				return base.CombatChar.GetUsingWeaponIndex() == this._checkedIndex;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06002EC1 RID: 11969 RVA: 0x002108E4 File Offset: 0x0020EAE4
		private int AffectingWeaponWeight
		{
			get
			{
				return this.Affecting ? this.UsingWeaponWeight : 0;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x002108F7 File Offset: 0x0020EAF7
		private int AffectingMakeDamageTotalPercent
		{
			get
			{
				return Math.Max(base.IsDirect ? (this.AffectingWeaponWeight / 5) : (300 - this.AffectingWeaponWeight / 5), 0);
			}
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x0021091F File Offset: 0x0020EB1F
		public ShiJiaZhiXiang()
		{
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x00210930 File Offset: 0x0020EB30
		public ShiJiaZhiXiang(CombatSkillKey skillKey) : base(skillKey, 5604)
		{
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x00210947 File Offset: 0x0020EB47
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(69, EDataModifyType.TotalPercent, -1);
			base.CreateAffectedData(275, EDataModifyType.TotalPercent, -1);
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x00210987 File Offset: 0x0020EB87
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x002109B0 File Offset: 0x0020EBB0
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				this._checkedIndex = -1;
				bool flag2 = !base.CanAffect || base.CombatChar.GetUsingWeaponIndex() >= 3;
				if (!flag2)
				{
					base.ShowSpecialEffectTips(0);
					this._checkedIndex = base.CombatChar.GetUsingWeaponIndex();
				}
			}
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x00210A14 File Offset: 0x0020EC14
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker.GetId() != base.CharacterId || attacker.GetIsFightBack();
			if (!flag)
			{
				this._checkedIndex = -1;
			}
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x00210A48 File Offset: 0x0020EC48
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !dataKey.IsNormalAttack || !base.CanAffect || this._checkedIndex < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool isAutoNormalAttackingSpecial = base.CombatChar.IsAutoNormalAttackingSpecial;
				if (isAutoNormalAttackingSpecial)
				{
					result = 0;
				}
				else
				{
					ushort fieldId = dataKey.FieldId;
					if (!true)
					{
					}
					int num;
					if (fieldId != 69 && fieldId != 275)
					{
						num = 0;
					}
					else
					{
						num = this.AffectingMakeDamageTotalPercent;
					}
					if (!true)
					{
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x04000DEC RID: 3564
		private int _checkedIndex = -1;
	}
}
