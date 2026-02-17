using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade
{
	// Token: 0x020001DD RID: 477
	public class KunWuDaoFa : BladeUnlockEffectBase
	{
		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06002D99 RID: 11673 RVA: 0x0020C5F7 File Offset: 0x0020A7F7
		private CValuePercent AddPrepareProgress
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 80 : 40;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06002D9A RID: 11674 RVA: 0x0020C60C File Offset: 0x0020A80C
		protected override IEnumerable<short> RequireWeaponTypes
		{
			get
			{
				yield return 3;
				yield return 11;
				yield break;
			}
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x0020C62B File Offset: 0x0020A82B
		public KunWuDaoFa()
		{
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x0020C635 File Offset: 0x0020A835
		public KunWuDaoFa(CombatSkillKey skillKey) : base(skillKey, 9203)
		{
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x0020C645 File Offset: 0x0020A845
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x0020C662 File Offset: 0x0020A862
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			base.OnDisable(context);
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x0020C680 File Offset: 0x0020A880
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.IsReverseOrUsingDirectWeapon;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * this.AddPrepareProgress);
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x0020C6E6 File Offset: 0x0020A8E6
		private IEnumerable<ItemKey> CanReduceDurabilityWeaponOrArmors()
		{
			ItemKey[] weapons = base.CurrEnemyChar.GetWeapons();
			foreach (ItemKey key in weapons)
			{
				bool flag = !key.IsValid();
				if (!flag)
				{
					ItemBase item = DomainManager.Item.GetBaseItem(key);
					bool flag2 = item.GetCurrDurability() <= 0;
					if (!flag2)
					{
						bool flag3 = !CombatDomain.IsWeaponCanBreak(item.GetItemSubType());
						if (!flag3)
						{
							yield return key;
							item = null;
							key = default(ItemKey);
						}
					}
				}
			}
			ItemKey[] array = null;
			ItemKey[] armors = base.CurrEnemyChar.Armors;
			foreach (ItemKey key2 in armors)
			{
				bool flag4 = !key2.IsValid();
				if (!flag4)
				{
					ItemBase item2 = DomainManager.Item.GetBaseItem(key2);
					bool flag5 = item2.GetCurrDurability() <= 0;
					if (!flag5)
					{
						yield return key2;
						item2 = null;
						key2 = default(ItemKey);
					}
				}
			}
			ItemKey[] array2 = null;
			yield break;
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x0020C6F6 File Offset: 0x0020A8F6
		protected override bool CanDoAffect()
		{
			return this.CanReduceDurabilityWeaponOrArmors().Any<ItemKey>();
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x0020C704 File Offset: 0x0020A904
		public override void DoAffectAfterCost(DataContext context, int weaponIndex)
		{
			List<ItemKey> pool = ObjectPool<List<ItemKey>>.Instance.Get();
			pool.Clear();
			foreach (ItemKey itemKey in this.CanReduceDurabilityWeaponOrArmors())
			{
				bool flag = !pool.Contains(itemKey);
				if (flag)
				{
					pool.Add(itemKey);
				}
			}
			foreach (ItemKey key in RandomUtils.GetRandomUnrepeated<ItemKey>(context.Random, 3, pool, null))
			{
				ItemBase item = DomainManager.Item.GetBaseItem(key);
				int reduceDurabilityValue = Math.Max((int)item.GetCurrDurability() * KunWuDaoFa.ReduceDurabilityPercent, 1);
				base.ChangeDurability(context, base.CurrEnemyChar, key, -reduceDurabilityValue);
			}
			ObjectPool<List<ItemKey>>.Instance.Return(pool);
			base.ShowSpecialEffectTips(base.IsDirect, 2, 1);
		}

		// Token: 0x04000DAF RID: 3503
		private const int MaxReduceDurabilityCount = 3;

		// Token: 0x04000DB0 RID: 3504
		private static readonly CValuePercent ReduceDurabilityPercent = 20;
	}
}
