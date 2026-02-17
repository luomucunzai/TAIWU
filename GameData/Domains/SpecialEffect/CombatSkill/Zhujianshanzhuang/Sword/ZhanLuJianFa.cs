using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword
{
	// Token: 0x020001BA RID: 442
	public class ZhanLuJianFa : SwordUnlockEffectBase
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06002C85 RID: 11397 RVA: 0x00207B04 File Offset: 0x00205D04
		private CValuePercent MaxGiveUnlockValuePercent
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 50 : 25;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06002C86 RID: 11398 RVA: 0x00207B1C File Offset: 0x00205D1C
		protected override IEnumerable<sbyte> RequirePersonalityTypes
		{
			get
			{
				yield return 4;
				yield break;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06002C87 RID: 11399 RVA: 0x00207B3B File Offset: 0x00205D3B
		protected override int RequirePersonalityValue
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x00207B3F File Offset: 0x00205D3F
		public ZhanLuJianFa()
		{
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x00207B49 File Offset: 0x00205D49
		public ZhanLuJianFa(CombatSkillKey skillKey) : base(skillKey, 9107)
		{
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x00207B59 File Offset: 0x00205D59
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(307, EDataModifyType.Custom, -1);
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x00207B74 File Offset: 0x00205D74
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
					bool flag3 = unlockValues[usingIndex] <= 0;
					if (!flag3)
					{
						int maxGiveValue = GlobalConfig.Instance.UnlockAttackUnit * this.MaxGiveUnlockValuePercent * power;
						bool flag4 = maxGiveValue <= 0;
						if (!flag4)
						{
							maxGiveValue = Math.Min(maxGiveValue, unlockValues[usingIndex]);
							List<int> otherWeaponIndexes = ObjectPool<List<int>>.Instance.Get();
							List<int> otherWeaponRequireValues = ObjectPool<List<int>>.Instance.Get();
							for (int i = 0; i < 3; i++)
							{
								bool flag5 = i == usingIndex;
								if (!flag5)
								{
									int lackValue = GlobalConfig.Instance.UnlockAttackUnit - unlockValues[i];
									bool flag6 = lackValue == 0;
									if (!flag6)
									{
										bool flag7 = !base.CombatChar.CanUnlockAttackByConfig(i);
										if (!flag7)
										{
											int requireValue = lackValue / 2 + ((lackValue % 2 > 0) ? 1 : 0);
											requireValue = Math.Min(requireValue, maxGiveValue);
											otherWeaponIndexes.Add(i);
											otherWeaponRequireValues.Add(requireValue);
										}
									}
								}
							}
							bool flag8 = otherWeaponIndexes.Count > 0;
							if (flag8)
							{
								int giveIndex = RandomUtils.GetRandomIndex(otherWeaponRequireValues, context.Random);
								int index = otherWeaponIndexes[giveIndex];
								int value = otherWeaponRequireValues[giveIndex];
								base.CombatChar.ChangeUnlockAttackValue(context, index, value * 2);
								base.CombatChar.ChangeUnlockAttackValue(context, usingIndex, -value);
								base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
							}
							ObjectPool<List<int>>.Instance.Return(otherWeaponIndexes);
							ObjectPool<List<int>>.Instance.Return(otherWeaponRequireValues);
						}
					}
				}
			}
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x00207D58 File Offset: 0x00205F58
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 307 || base.EffectCount <= 0;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				base.ReduceEffectCount(1);
				base.ShowSpecialEffectTips(base.IsDirect, 2, 1);
				result = true;
			}
			return result;
		}

		// Token: 0x04000D6D RID: 3437
		private const int GiveValueMultiplier = 2;
	}
}
