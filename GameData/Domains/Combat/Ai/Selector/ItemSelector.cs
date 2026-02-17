using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai.Selector
{
	// Token: 0x02000728 RID: 1832
	public class ItemSelector
	{
		// Token: 0x060068BF RID: 26815 RVA: 0x003B7CF6 File Offset: 0x003B5EF6
		private static bool IsEat(ItemKey itemKey)
		{
			return itemKey.ItemType != 8 || Config.Medicine.Instance[itemKey.TemplateId].Duration > 0;
		}

		// Token: 0x060068C0 RID: 26816 RVA: 0x003B7D1C File Offset: 0x003B5F1C
		private static bool CharacterCheck(CombatCharacter combatChar)
		{
			bool isAlly = combatChar.IsAlly;
			return !isAlly && combatChar.GetCanUseItem();
		}

		// Token: 0x060068C1 RID: 26817 RVA: 0x003B7D42 File Offset: 0x003B5F42
		public ItemSelector(ItemSelectorPredicate predicate) : this(predicate, null)
		{
		}

		// Token: 0x060068C2 RID: 26818 RVA: 0x003B7D4E File Offset: 0x003B5F4E
		public ItemSelector(ItemSelectorPredicate predicate, ItemSelectorComparisonIsPrefer comparison)
		{
			this._predicate = predicate;
			this._comparison = comparison;
		}

		// Token: 0x060068C3 RID: 26819 RVA: 0x003B7D68 File Offset: 0x003B5F68
		public bool AnyMatch(CombatCharacter combatChar)
		{
			bool flag = !ItemSelector.CharacterCheck(combatChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._checkingCharacter = combatChar;
				bool pass = combatChar.GetValidItems().Where(new Func<ItemKey, bool>(this.PredicateCheck)).Any(new Func<ItemKey, bool>(this.CommonCheck));
				this._checkingCharacter = null;
				result = pass;
			}
			return result;
		}

		// Token: 0x060068C4 RID: 26820 RVA: 0x003B7DC4 File Offset: 0x003B5FC4
		public ItemKey Select(IRandomSource random, CombatCharacter combatChar)
		{
			bool flag = !ItemSelector.CharacterCheck(combatChar);
			ItemKey result2;
			if (flag)
			{
				result2 = ItemKey.Invalid;
			}
			else
			{
				bool anyPrefer = false;
				List<ItemKey> pool = ObjectPool<List<ItemKey>>.Instance.Get();
				this._checkingCharacter = combatChar;
				foreach (ItemKey itemKey in combatChar.GetValidItems())
				{
					bool flag2 = !this.PredicateCheck(itemKey) || !this.CommonCheck(itemKey);
					if (!flag2)
					{
						ItemSelectorComparisonIsPrefer comparison = this._comparison;
						bool prefer = comparison != null && comparison(combatChar, itemKey);
						bool flag3 = prefer && !anyPrefer;
						if (flag3)
						{
							anyPrefer = true;
							pool.Clear();
						}
						bool flag4 = prefer == anyPrefer;
						if (flag4)
						{
							pool.Add(itemKey);
						}
					}
				}
				this._checkingCharacter = null;
				ItemKey result = (pool.Count > 0) ? ItemSelectorHelper.SelectBestGradeItem(random, pool) : ItemKey.Invalid;
				ObjectPool<List<ItemKey>>.Instance.Return(pool);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060068C5 RID: 26821 RVA: 0x003B7EE0 File Offset: 0x003B60E0
		private bool CommonCheck(ItemKey itemKey)
		{
			CombatType combatType = (CombatType)DomainManager.Combat.GetCombatType();
			bool flag = combatType == CombatType.Play || combatType == CombatType.Test;
			bool flag2 = flag && !itemKey.GetConfigAs<ICombatItemConfig>().AllowUseInPlayAndTest;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				int costWisdom = itemKey.GetConsumedFeatureMedals();
				short wisdom = this._checkingCharacter.IsAlly ? DomainManager.Combat.GetSelfTeamWisdomCount() : DomainManager.Combat.GetEnemyTeamWisdomCount();
				bool flag3 = costWisdom > (int)wisdom;
				if (flag3)
				{
					result = false;
				}
				else
				{
					bool flag4 = !ItemSelector.IsEat(itemKey);
					if (flag4)
					{
						result = true;
					}
					else
					{
						GameData.Domains.Character.Character character = this._checkingCharacter.GetCharacter();
						sbyte maxSlotCount = character.GetCurrMaxEatingSlotsCount();
						result = (character.GetEatingItems().GetAvailableEatingSlot(maxSlotCount) >= 0);
					}
				}
			}
			return result;
		}

		// Token: 0x060068C6 RID: 26822 RVA: 0x003B7FA8 File Offset: 0x003B61A8
		private bool PredicateCheck(ItemKey itemKey)
		{
			ItemSelectorPredicate predicate = this._predicate;
			return predicate == null || predicate(this._checkingCharacter, itemKey);
		}

		// Token: 0x04001CBA RID: 7354
		private readonly ItemSelectorPredicate _predicate;

		// Token: 0x04001CBB RID: 7355
		private readonly ItemSelectorComparisonIsPrefer _comparison;

		// Token: 0x04001CBC RID: 7356
		private CombatCharacter _checkingCharacter;
	}
}
