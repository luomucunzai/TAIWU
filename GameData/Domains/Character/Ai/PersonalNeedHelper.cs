using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x0200084F RID: 2127
	public static class PersonalNeedHelper
	{
		// Token: 0x06007680 RID: 30336 RVA: 0x0045558C File Offset: 0x0045378C
		public unsafe static bool CheckValid(this PersonalNeed need, Character character)
		{
			bool result;
			switch (need.TemplateId)
			{
			case 0:
				result = (character.GetHappiness() < HappinessType.Ranges[3].Item1);
				break;
			case 1:
				result = (character.GetHealth() < character.GetLeftMaxHealth(false));
				break;
			case 2:
				result = (DisorderLevelOfQi.GetDisorderLevelOfQi(character.GetDisorderOfQi()) > 0);
				break;
			case 3:
				result = (character.GetCurrNeili() < character.GetMaxNeili());
				break;
			case 4:
				result = character.GetInjuries().HasAnyInjury(need.InjuryType == 1);
				break;
			case 5:
			{
				PoisonInts poisoned = *character.GetPoisoned();
				result = (*(ref poisoned.Items.FixedElementField + (IntPtr)need.PoisonType * 4) > 0 && !character.HasPoisonImmunity(need.PoisonType));
				break;
			}
			case 6:
			{
				MainAttributes currMainAttributes = character.GetCurrMainAttributes();
				MainAttributes maxMainAttributes = character.GetMaxMainAttributes();
				result = (*(ref currMainAttributes.Items.FixedElementField + (IntPtr)need.MainAttributeType * 2) < *(ref maxMainAttributes.Items.FixedElementField + (IntPtr)need.MainAttributeType * 2));
				break;
			}
			case 7:
				result = ((*character.GetEatingItems()).IndexOfWug(need.WugType, false) >= 0);
				break;
			case 8:
				result = (character.GetResource(need.ResourceType) < need.Amount && need.Amount >= 0);
				break;
			case 9:
				result = (character.GetResource(need.ResourceType) > need.Amount && need.Amount >= 0);
				break;
			case 10:
			{
				sbyte itemType = need.ItemType;
				short itemTemplateId = need.ItemTemplateId;
				List<ItemKey> itemList = ObjectPool<List<ItemKey>>.Instance.Get();
				bool hasItemInSameGroup = character.GetInventory().HasItemInSameGroup(itemType, itemTemplateId, -2, itemList);
				bool flag = hasItemInSameGroup;
				if (flag)
				{
					bool hasItem = itemList.Exists(delegate(ItemKey k)
					{
						ItemBase itemBase = DomainManager.Item.GetBaseItem(k);
						bool flag14 = itemBase.GetMaxDurability() <= 0;
						bool result2;
						if (flag14)
						{
							result2 = true;
						}
						else
						{
							bool flag15 = itemBase.GetCurrDurability() > 0;
							result2 = flag15;
						}
						return result2;
					});
					bool flag2 = hasItem;
					if (flag2)
					{
						ObjectPool<List<ItemKey>>.Instance.Return(itemList);
						result = false;
						break;
					}
				}
				ObjectPool<List<ItemKey>>.Instance.Return(itemList);
				bool flag3 = ItemTemplateHelper.GetEquipmentType(itemType, itemTemplateId) < 0;
				if (flag3)
				{
					result = true;
				}
				else
				{
					short groupId = ItemTemplateHelper.GetGroupId(itemType, itemTemplateId);
					sbyte expectedGrade = ItemTemplateHelper.GetGrade(itemType, itemTemplateId);
					sbyte requiredMinGrade = expectedGrade - -2;
					foreach (ItemKey equippedKey in character.GetEquipment())
					{
						bool flag4 = equippedKey.ItemType != itemType;
						if (!flag4)
						{
							bool flag5 = equippedKey.TemplateId == itemTemplateId;
							if (flag5)
							{
								return false;
							}
							bool flag6 = groupId < 0;
							if (!flag6)
							{
								bool flag7 = ItemTemplateHelper.GetGroupId(equippedKey.ItemType, equippedKey.TemplateId) != groupId;
								if (!flag7)
								{
									bool flag8 = ItemTemplateHelper.GetGrade(equippedKey.ItemType, equippedKey.TemplateId) < requiredMinGrade;
									if (!flag8)
									{
										return false;
									}
								}
							}
						}
					}
					result = true;
				}
				break;
			}
			case 11:
			{
				ItemKey itemKey = new ItemKey(need.ItemType, 0, -1, need.ItemId);
				bool flag9 = !DomainManager.Item.ItemExists(itemKey);
				if (flag9)
				{
					result = false;
				}
				else
				{
					ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
					itemKey = baseItem.GetItemKey();
					result = (character.GetInventory().Items.ContainsKey(itemKey) && baseItem.GetCurrDurability() < baseItem.GetMaxDurability());
				}
				break;
			}
			case 12:
			{
				bool flag10 = !Organization.Instance[character.GetOrganizationInfo().OrgTemplateId].AllowPoisoning;
				result = !flag10;
				break;
			}
			case 13:
				result = (character.GetInventoryTotalValue() > need.Amount);
				break;
			case 14:
				result = ((int)character.GetCombatSkillAttainment(need.CombatSkillType) < need.Amount);
				break;
			case 15:
				result = ((int)character.GetLifeSkillAttainment(need.LifeSkillType) < need.Amount);
				break;
			case 16:
				result = (character.GetExp() < need.Amount);
				break;
			case 17:
			{
				GameData.Domains.Item.SkillBook item;
				bool flag11 = !DomainManager.Item.TryGetElement_SkillBooks(need.ItemId, out item);
				if (flag11)
				{
					result = false;
				}
				else
				{
					bool flag12 = !character.GetInventory().Items.ContainsKey(item.GetItemKey());
					if (flag12)
					{
						result = false;
					}
					else
					{
						bool flag13 = item.IsCombatSkillBook();
						if (flag13)
						{
							ValueTuple<int, byte> readingInfo = character.GetCombatSkillBookCurrReadingInfo(item);
							result = (readingInfo.Item2 < 6);
						}
						else
						{
							ValueTuple<int, byte> readingInfo2 = character.GetLifeSkillBookCurrReadingInfo(item);
							result = (readingInfo2.Item2 < 5);
						}
					}
				}
				break;
			}
			case 18:
			{
				GameData.Domains.CombatSkill.CombatSkill combatSkill;
				result = (DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(character.GetId(), need.CombatSkillTemplateId), out combatSkill) && !CombatSkillStateHelper.IsBrokenOut(combatSkill.GetActivationState()) && combatSkill.CanBreakout());
				break;
			}
			case 19:
				result = DomainManager.Character.IsCharacterAlive(need.CharId);
				break;
			case 20:
				result = (DomainManager.Character.IsCharacterAlive(need.CharId) && !DomainManager.Character.IsInTheSameGroup(character.GetId(), need.CharId));
				break;
			case 21:
				result = DomainManager.Character.HasRelation(character.GetId(), need.CharId, 32768);
				break;
			case 22:
			{
				Grave grave;
				result = DomainManager.Character.TryGetElement_Graves(need.CharId, out grave);
				break;
			}
			case 23:
				result = DomainManager.Character.IsCharacterAlive(need.CharId);
				break;
			default:
				result = true;
				break;
			}
			return result;
		}
	}
}
