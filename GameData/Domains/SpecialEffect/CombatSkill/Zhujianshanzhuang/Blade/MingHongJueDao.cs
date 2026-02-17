using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade
{
	// Token: 0x020001DE RID: 478
	public class MingHongJueDao : BladeUnlockEffectBase
	{
		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06002DA4 RID: 11684 RVA: 0x0020C822 File Offset: 0x0020AA22
		private CValuePercent MaxStealUnlockValuePercent
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 40 : 20;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06002DA5 RID: 11685 RVA: 0x0020C838 File Offset: 0x0020AA38
		protected override IEnumerable<short> RequireWeaponTypes
		{
			get
			{
				yield return 8;
				yield break;
			}
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x0020C857 File Offset: 0x0020AA57
		public MingHongJueDao()
		{
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x0020C861 File Offset: 0x0020AA61
		public MingHongJueDao(CombatSkillKey skillKey) : base(skillKey, 9207)
		{
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x0020C871 File Offset: 0x0020AA71
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(69, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_UnlockAttackEnd(new Events.OnUnlockAttackEnd(this.OnUnlockAttackEnd));
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x0020C899 File Offset: 0x0020AA99
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_UnlockAttackEnd(new Events.OnUnlockAttackEnd(this.OnUnlockAttackEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x0020C8B8 File Offset: 0x0020AAB8
		protected override void OnCastAddUnlockAttackValue(DataContext context, CValuePercent power)
		{
			base.OnCastAddUnlockAttackValue(context, power);
			bool flag = !base.IsReverseOrUsingDirectWeapon;
			if (!flag)
			{
				int usingIndex = base.CombatChar.GetUsingWeaponIndex();
				bool flag2 = usingIndex >= 3 || base.CombatChar.GetUnlockEffect(usingIndex) == null;
				if (!flag2)
				{
					List<int> unlockValues = base.CombatChar.GetUnlockPrepareValue();
					int lackUnlockValue = GlobalConfig.Instance.UnlockAttackUnit - unlockValues[usingIndex];
					bool flag3 = lackUnlockValue == 0;
					if (!flag3)
					{
						int maxStealValue = GlobalConfig.Instance.UnlockAttackUnit * this.MaxStealUnlockValuePercent * power;
						bool flag4 = maxStealValue == 0;
						if (!flag4)
						{
							int avg = lackUnlockValue / 2;
							List<int> otherWeaponIndexes = ObjectPool<List<int>>.Instance.Get();
							List<int> otherWeaponUnlockValues = ObjectPool<List<int>>.Instance.Get();
							for (int i = 0; i < 3; i++)
							{
								bool flag5 = i == usingIndex;
								if (!flag5)
								{
									int canStealValue = Math.Min(unlockValues[i], maxStealValue);
									bool flag6 = canStealValue == 0;
									if (!flag6)
									{
										avg = Math.Min(avg, canStealValue);
										otherWeaponIndexes.Add(i);
										otherWeaponUnlockValues.Add(canStealValue);
									}
								}
							}
							int maxFillValue = Math.Min(maxStealValue * otherWeaponIndexes.Count, lackUnlockValue);
							int rem = maxFillValue - avg * otherWeaponIndexes.Count;
							for (int j = 0; j < otherWeaponIndexes.Count; j++)
							{
								int remFill = Math.Min(otherWeaponUnlockValues[j] - avg, rem);
								rem -= remFill;
								otherWeaponUnlockValues[j] = avg + remFill;
							}
							for (int k = 0; k < otherWeaponIndexes.Count; k++)
							{
								int stealIndex = otherWeaponIndexes[k];
								int stealValue = otherWeaponUnlockValues[k];
								base.CombatChar.ChangeUnlockAttackValue(context, stealIndex, -stealValue);
								base.CombatChar.ChangeUnlockAttackValue(context, base.CombatChar.GetUsingWeaponIndex(), stealValue);
							}
							bool flag7 = otherWeaponIndexes.Count > 0;
							if (flag7)
							{
								base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
							}
							ObjectPool<List<int>>.Instance.Return(otherWeaponIndexes);
							ObjectPool<List<int>>.Instance.Return(otherWeaponUnlockValues);
						}
					}
				}
			}
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x0020CAE9 File Offset: 0x0020ACE9
		public override void DoAffectAfterCost(DataContext context, int weaponIndex)
		{
			this._affected = true;
			base.ShowSpecialEffectTips(base.IsDirect, 2, 1);
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x0020CB04 File Offset: 0x0020AD04
		private void OnUnlockAttackEnd(DataContext context, CombatCharacter attacker)
		{
			bool flag = attacker.GetId() == base.CharacterId;
			if (flag)
			{
				this._affected = false;
			}
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x0020CB2C File Offset: 0x0020AD2C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 69 || !this._affected;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 300;
			}
			return result;
		}

		// Token: 0x04000DB1 RID: 3505
		private const int AddDirectDamagePercent = 300;

		// Token: 0x04000DB2 RID: 3506
		private bool _affected;
	}
}
