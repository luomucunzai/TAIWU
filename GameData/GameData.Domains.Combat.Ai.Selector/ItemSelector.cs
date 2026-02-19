using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai.Selector;

public class ItemSelector
{
	private readonly ItemSelectorPredicate _predicate;

	private readonly ItemSelectorComparisonIsPrefer _comparison;

	private CombatCharacter _checkingCharacter;

	private static bool IsEat(ItemKey itemKey)
	{
		return itemKey.ItemType != 8 || Config.Medicine.Instance[itemKey.TemplateId].Duration > 0;
	}

	private static bool CharacterCheck(CombatCharacter combatChar)
	{
		if (combatChar.IsAlly)
		{
			return false;
		}
		return combatChar.GetCanUseItem();
	}

	public ItemSelector(ItemSelectorPredicate predicate)
		: this(predicate, null)
	{
	}

	public ItemSelector(ItemSelectorPredicate predicate, ItemSelectorComparisonIsPrefer comparison)
	{
		_predicate = predicate;
		_comparison = comparison;
	}

	public bool AnyMatch(CombatCharacter combatChar)
	{
		if (!CharacterCheck(combatChar))
		{
			return false;
		}
		_checkingCharacter = combatChar;
		bool result = combatChar.GetValidItems().Where(PredicateCheck).Any(CommonCheck);
		_checkingCharacter = null;
		return result;
	}

	public ItemKey Select(IRandomSource random, CombatCharacter combatChar)
	{
		if (!CharacterCheck(combatChar))
		{
			return ItemKey.Invalid;
		}
		bool flag = false;
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		_checkingCharacter = combatChar;
		foreach (ItemKey validItem in combatChar.GetValidItems())
		{
			if (PredicateCheck(validItem) && CommonCheck(validItem))
			{
				bool flag2 = _comparison?.Invoke(combatChar, validItem) ?? false;
				if (flag2 && !flag)
				{
					flag = true;
					list.Clear();
				}
				if (flag2 == flag)
				{
					list.Add(validItem);
				}
			}
		}
		_checkingCharacter = null;
		ItemKey result = ((list.Count > 0) ? ItemSelectorHelper.SelectBestGradeItem(random, list) : ItemKey.Invalid);
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		return result;
	}

	private bool CommonCheck(ItemKey itemKey)
	{
		CombatType combatType = (CombatType)DomainManager.Combat.GetCombatType();
		bool flag = ((combatType == CombatType.Play || combatType == CombatType.Test) ? true : false);
		if (flag && !itemKey.GetConfigAs<ICombatItemConfig>().AllowUseInPlayAndTest)
		{
			return false;
		}
		int consumedFeatureMedals = itemKey.GetConsumedFeatureMedals();
		short num = (_checkingCharacter.IsAlly ? DomainManager.Combat.GetSelfTeamWisdomCount() : DomainManager.Combat.GetEnemyTeamWisdomCount());
		if (consumedFeatureMedals > num)
		{
			return false;
		}
		GameData.Domains.Character.Character character = _checkingCharacter.GetCharacter();
		if (itemKey.ItemType == 8)
		{
			MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
			if (medicineItem.RequiredMainAttributeType >= 0)
			{
				short currMainAttribute = character.GetCurrMainAttribute(medicineItem.RequiredMainAttributeType);
				sbyte requiredMainAttributeValue = medicineItem.RequiredMainAttributeValue;
				if (currMainAttribute < requiredMainAttributeValue)
				{
					return false;
				}
			}
		}
		if (!IsEat(itemKey))
		{
			return true;
		}
		sbyte currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
		return character.GetEatingItems().GetAvailableEatingSlot(currMaxEatingSlotsCount) >= 0;
	}

	private bool PredicateCheck(ItemKey itemKey)
	{
		return _predicate?.Invoke(_checkingCharacter, itemKey) ?? true;
	}
}
