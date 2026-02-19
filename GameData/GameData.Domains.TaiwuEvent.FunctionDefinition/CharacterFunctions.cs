using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class CharacterFunctions
{
	[EventFunction(18)]
	private unsafe static void SpecifyCurrMainAttribute(EventScriptRuntime runtime, GameData.Domains.Character.Character character, sbyte mainAttributeType, int value)
	{
		MainAttributes currMainAttributes = character.GetCurrMainAttributes();
		currMainAttributes.Items[mainAttributeType] = (short)value;
		character.SetCurrMainAttributes(currMainAttributes, runtime.Context);
	}

	[EventFunction(19)]
	private static void ChangeCurrMainAttribute(EventScriptRuntime runtime, GameData.Domains.Character.Character character, sbyte mainAttributeType, int delta)
	{
		character.ChangeCurrMainAttribute(runtime.Context, mainAttributeType, delta);
	}

	[EventFunction(20)]
	private static void SpecifyInjury(EventScriptRuntime runtime, GameData.Domains.Character.Character character, sbyte bodyPartType, bool isInner, int value)
	{
		value = Math.Clamp(value, 0, 6);
		Injuries injuries = character.GetInjuries();
		injuries.Set(bodyPartType, isInner, (sbyte)value);
		character.SetInjuries(injuries, runtime.Context);
	}

	[EventFunction(21)]
	private static void ChangeInjury(EventScriptRuntime runtime, GameData.Domains.Character.Character character, sbyte bodyPartType, bool isInner, int delta)
	{
		delta = Math.Clamp(delta, 0, 6);
		character.ChangeInjury(runtime.Context, bodyPartType, isInner, (sbyte)delta);
	}

	[EventFunction(22)]
	private static void ClearInjuries(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		Injuries injuries = character.GetInjuries();
		injuries.Initialize();
		character.SetInjuries(injuries, runtime.Context);
	}

	[EventFunction(23)]
	private static void SpecifyPoisoned(EventScriptRuntime runtime, GameData.Domains.Character.Character character, sbyte poisonType, int value)
	{
		ref PoisonInts poisoned = ref character.GetPoisoned();
		poisoned[poisonType] = value;
		character.SetPoisoned(ref poisoned, runtime.Context);
	}

	[EventFunction(24)]
	private static void ChangePoisoned(EventScriptRuntime runtime, GameData.Domains.Character.Character character, sbyte poisonType, int level, int delta)
	{
		level = Math.Clamp(level, 0, 3);
		character.ChangePoisoned(runtime.Context, poisonType, (sbyte)level, delta);
	}

	[EventFunction(25)]
	private static void ClearPoisons(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		ref PoisonInts poisoned = ref character.GetPoisoned();
		poisoned.Initialize();
		character.SetPoisoned(ref poisoned, runtime.Context);
	}

	[EventFunction(26)]
	private static void SpecifyDisorderOfQi(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int value)
	{
		value = Math.Clamp(value, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue);
		character.SetDisorderOfQi((short)value, runtime.Context);
	}

	[EventFunction(27)]
	private static void ChangeDisorderOfQi(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int delta)
	{
		character.ChangeDisorderOfQi(runtime.Context, delta);
	}

	[EventFunction(28)]
	private static void SpecifyHealth(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int value)
	{
		value = Math.Clamp(value, 0, character.GetLeftMaxHealth());
		character.SetHealth((short)value, runtime.Context);
	}

	[EventFunction(29)]
	private static void ChangeHealth(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int delta)
	{
		character.ChangeHealth(runtime.Context, delta);
	}

	[EventFunction(30)]
	private static void SpecifyHappiness(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int value)
	{
		value = Math.Clamp(value, -119, 119);
		character.SetHappiness((sbyte)value, runtime.Context);
	}

	[EventFunction(31)]
	private static void ChangeHappiness(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int delta)
	{
		character.ChangeHappiness(runtime.Context, delta);
	}

	[EventFunction(32)]
	private static void SpecifyFavorabilities(EventScriptRuntime runtime, GameData.Domains.Character.Character self, GameData.Domains.Character.Character target, int selfToTarget, int targetToSelf)
	{
		DomainManager.Character.DirectlySetFavorabilities(runtime.Context, self.GetId(), target.GetId(), (short)selfToTarget, (short)targetToSelf);
	}

	[EventFunction(33)]
	private static void ChangeFavorability(EventScriptRuntime runtime, GameData.Domains.Character.Character self, GameData.Domains.Character.Character target, int delta, short type = -1)
	{
		int id = self.GetId();
		int id2 = target.GetId();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		byte creatingType = self.GetCreatingType();
		byte creatingType2 = target.GetCreatingType();
		if (creatingType == 0 || creatingType2 == 0)
		{
			if (id != taiwuCharId && id2 != taiwuCharId)
			{
				throw new InvalidOperationException($"Failed to create relation between {self} and {target}: fixed character can only create relation with taiwu.");
			}
			int delta2 = DomainManager.Character.CalcFavorabilityDelta(id, id2, delta, type);
			DomainManager.Character.DirectlyChangeFavorabilityOptional(runtime.Context, self, target, delta2, type);
		}
		else
		{
			DomainManager.Character.ChangeFavorabilityOptional(runtime.Context, self, target, delta, type);
		}
		DomainManager.Character.AddFavorabilityChangeInstantNotification(self, target, delta > 0);
		if (target.IsTaiwu())
		{
			DomainManager.TaiwuEvent.RecordFavorabilityToTaiwuChanged(id, (short)delta);
		}
	}

	[EventFunction(34)]
	private static void AddFeature(EventScriptRuntime runtime, GameData.Domains.Character.Character character, short featureId, bool removeMutexFeature)
	{
		character.AddFeature(runtime.Context, featureId, removeMutexFeature);
	}

	[EventFunction(35)]
	private static void RemoveFeature(EventScriptRuntime runtime, GameData.Domains.Character.Character character, short featureId)
	{
		character.RemoveFeature(runtime.Context, featureId);
	}

	[EventFunction(36)]
	private static void AddKidnappedCharacter(EventScriptRuntime runtime, GameData.Domains.Character.Character character, GameData.Domains.Character.Character kidnappedChar, ItemKey ropeItemKey)
	{
		if (character.GetKidnapperId() < 0)
		{
			int id = character.GetId();
			int id2 = kidnappedChar.GetId();
			int kidnapperId = kidnappedChar.GetKidnapperId();
			if (kidnapperId == id)
			{
				DomainManager.Character.ChangeKidnappedCharacterRope(runtime.Context, id, id2, ropeItemKey);
			}
			else if (kidnapperId >= 0)
			{
				KidnappedCharacterList kidnappedCharacters = DomainManager.Character.GetKidnappedCharacters(kidnapperId);
				int index = kidnappedCharacters.IndexOf(id2);
				KidnappedCharacter kidnappedCharData = kidnappedCharacters.Get(index);
				DomainManager.Character.TransferKidnappedCharacter(runtime.Context, id, kidnapperId, kidnappedCharData);
			}
			else
			{
				DomainManager.Character.AddKidnappedCharacter(runtime.Context, character, kidnappedChar, ropeItemKey);
			}
		}
	}

	[EventFunction(37)]
	private static void RemoveKidnappedCharacter(EventScriptRuntime runtime, GameData.Domains.Character.Character character, GameData.Domains.Character.Character kidnappedChar, bool isEscape)
	{
		DomainManager.Character.RemoveKidnappedCharacter(runtime.Context, character.GetId(), kidnappedChar.GetId(), isEscape);
	}

	[EventFunction(38)]
	private static void JoinGroup(EventScriptRuntime runtime, GameData.Domains.Character.Character character, GameData.Domains.Character.Character leader)
	{
		int id = character.GetId();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (id != taiwuCharId && character.GetKidnapperId() < 0 && leader.GetKidnapperId() < 0)
		{
			if (character.GetLeaderId() >= 0)
			{
				DomainManager.Character.LeaveGroup(runtime.Context, character);
			}
			if (leader.GetId() == taiwuCharId || leader.GetLeaderId() == taiwuCharId)
			{
				DomainManager.Taiwu.JoinGroup(runtime.Context, id);
			}
			else
			{
				DomainManager.Character.JoinGroup(runtime.Context, character, leader);
			}
		}
	}

	[EventFunction(39)]
	private static void LeaveGroup(EventScriptRuntime runtime, GameData.Domains.Character.Character character, bool bringWard)
	{
		DomainManager.Character.LeaveGroup(runtime.Context, character, bringWard);
	}

	[EventFunction(40)]
	private static void KillCharacter(EventScriptRuntime runtime, GameData.Domains.Character.Character victim, GameData.Domains.Character.Character killer, short deathType)
	{
		if (victim.GetCreatingType() == 1)
		{
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.KillAddTianJieFuLu(killer.GetId(), victim);
			DomainManager.Character.MakeCharacterDead(runtime.Context, victim, deathType, new CharacterDeathInfo(victim.GetValidLocation())
			{
				KillerId = (killer?.GetId() ?? (-1))
			});
		}
		else
		{
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.RemoveNonIntelligentCharacter(victim);
		}
	}

	[EventFunction(41)]
	private static void AddInventoryItem(EventScriptRuntime runtime, GameData.Domains.Character.Character character, ItemKey itemKey, int amount)
	{
		Tester.Assert(itemKey.IsValid());
		character.AddInventoryItem(runtime.Context, itemKey, amount);
		if (character == DomainManager.Taiwu.GetTaiwu())
		{
			InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
			instantNotifications.AddGetItem(character.GetId(), itemKey.ItemType, itemKey.TemplateId);
			runtime.Current.RegisterToShowGetItem(itemKey, amount);
		}
	}

	[EventFunction(65)]
	private static void RemoveInventoryItem(EventScriptRuntime runtime, GameData.Domains.Character.Character character, ItemKey itemKey, int amount, bool deleteItem)
	{
		Tester.Assert(itemKey.IsValid());
		character.RemoveInventoryItem(runtime.Context, itemKey, amount, deleteItem);
		if (character == DomainManager.Taiwu.GetTaiwu())
		{
			InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
			instantNotifications.AddLoseItem(character.GetId(), itemKey.ItemType, itemKey.TemplateId);
		}
	}

	[EventFunction(42)]
	private static void TransferInventoryItem(EventScriptRuntime runtime, GameData.Domains.Character.Character character, GameData.Domains.Character.Character destChar, ItemKey itemKey, int amount, bool favorAndDebt)
	{
		Tester.Assert(itemKey.HasTemplate);
		sbyte miscResourceType = ItemTemplateHelper.GetMiscResourceType(itemKey.ItemType, itemKey.TemplateId);
		if (miscResourceType == -1)
		{
			if (!itemKey.IsValid())
			{
				AdaptableLog.Info($"Creating item by template on transfer: {itemKey}.");
				itemKey = DomainManager.Item.CreateItem(runtime.Context, itemKey.ItemType, itemKey.TemplateId);
				character.AddInventoryItem(runtime.Context, itemKey, amount);
			}
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			DomainManager.Character.TransferInventoryItem(runtime.Context, character, destChar, itemKey, amount);
			if (favorAndDebt)
			{
				bool flag = character == taiwu || destChar == taiwu;
				bool flag2 = character.GetCreatingType() == 1 && destChar.GetCreatingType() == 1;
				int favorabilityChange = DomainManager.Item.GetBaseItem(itemKey).GetFavorabilityChange();
				if (!flag2)
				{
					int delta = CharacterDomain.CalcFavorabilityDelta(destChar, character, favorabilityChange, -1);
					DomainManager.Character.DirectlyChangeFavorabilityOptional(runtime.Context, destChar, character, delta, 1);
				}
				else if (!flag)
				{
					DomainManager.Character.ChangeFavorabilityOptional(runtime.Context, destChar, character, favorabilityChange, 1);
				}
				else
				{
					DomainManager.Character.UpdateDebtByItemTransfer(runtime.Context, character, destChar, itemKey, amount, changeFavor: true);
				}
			}
			if (destChar == taiwu)
			{
				InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
				instantNotifications.AddGetItem(destChar.GetId(), itemKey.ItemType, itemKey.TemplateId);
				runtime.Current.RegisterToShowGetItem(itemKey, amount);
			}
		}
		else
		{
			TransferCharacterResource(runtime, character, destChar, miscResourceType, amount, favorAndDebt);
		}
	}

	[EventFunction(129)]
	private static void SpecifyCharacterResource(EventScriptRuntime runtime, GameData.Domains.Character.Character character, sbyte resourceType, int value)
	{
		character.SpecifyResource(runtime.Context, resourceType, value);
	}

	[EventFunction(66)]
	private static void ChangeCharacterResource(EventScriptRuntime runtime, GameData.Domains.Character.Character character, sbyte resourceType, int delta)
	{
		character.ChangeResource(runtime.Context, resourceType, delta);
		if (character == DomainManager.Taiwu.GetTaiwu())
		{
			InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
			if (delta > 0)
			{
				instantNotifications.AddResourceIncreased(character.GetId(), resourceType, delta);
				runtime.Current.RegisterToShowGetResource(resourceType, delta);
			}
			else
			{
				instantNotifications.AddResourceDecreased(character.GetId(), resourceType, -delta);
			}
		}
	}

	[EventFunction(130)]
	private static void TransferCharacterResource(EventScriptRuntime runtime, GameData.Domains.Character.Character character, GameData.Domains.Character.Character destChar, sbyte resourceType, int amount, bool favorAndDebt)
	{
		DomainManager.Character.TransferResource(runtime.Context, character, destChar, resourceType, amount);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (destChar == taiwu)
		{
			InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
			instantNotifications.AddResourceIncreased(character.GetId(), resourceType, amount);
			runtime.Current.RegisterToShowGetResource(resourceType, amount);
		}
		else if (character == taiwu)
		{
			InstantNotificationCollection instantNotifications2 = DomainManager.World.GetInstantNotifications();
			instantNotifications2.AddResourceDecreased(character.GetId(), resourceType, amount);
		}
		if (favorAndDebt)
		{
			bool flag = character == taiwu || destChar == taiwu;
			bool flag2 = character.GetCreatingType() == 1 && destChar.GetCreatingType() == 1;
			short resourceFavorabilityChange = AiHelper.GeneralActionConstants.GetResourceFavorabilityChange(resourceType, amount);
			if (!flag2)
			{
				DomainManager.Character.DirectlyChangeFavorabilityOptional(runtime.Context, destChar, character, resourceFavorabilityChange, 1);
				return;
			}
			if (!flag)
			{
				DomainManager.Character.ChangeFavorabilityOptional(runtime.Context, destChar, character, resourceFavorabilityChange, 1);
				return;
			}
			ResourceInts resources = default(ResourceInts);
			resources.Initialize();
			resources[resourceType] = amount;
			DomainManager.Character.UpdateDebtByResourceTransfer(runtime.Context, character, destChar, resources, changeFavor: true);
		}
	}

	[EventFunction(43)]
	private static void ChangeCharBaseCombatSkillQualification(EventScriptRuntime runtime, GameData.Domains.Character.Character character, sbyte combatSkillType, int delta)
	{
		character.ChangeBaseCombatSkillQualification(runtime.Context, combatSkillType, delta);
	}

	[EventFunction(44)]
	private static void ChangeCharBaseLifeSkillQualification(EventScriptRuntime runtime, GameData.Domains.Character.Character character, sbyte lifeSkillType, int delta)
	{
		character.ChangeBaseLifeSkillQualification(runtime.Context, lifeSkillType, delta);
	}

	[EventFunction(45)]
	private static void LearnCombatSkill(EventScriptRuntime runtime, GameData.Domains.Character.Character character, short templateId)
	{
		DomainManager.Character.LearnCombatSkill(runtime.Context, character.GetId(), templateId, 0);
	}

	[EventFunction(46)]
	private static void LearnLifeSkill(EventScriptRuntime runtime, GameData.Domains.Character.Character character, short templateId)
	{
		DomainManager.Character.LearnLifeSkill(runtime.Context, character.GetId(), templateId, 0);
	}

	[EventFunction(111)]
	private static int GetCharacterFavorability(EventScriptRuntime runtime, GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar)
	{
		return DomainManager.Character.GetFavorability(selfChar.GetId(), targetChar.GetId());
	}

	[EventFunction(117)]
	private static void SetCharacterFollowTaiwu(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int distance)
	{
		if (character.GetCreatingType() != 0)
		{
			AdaptableLog.Warning($"{character} is is not a fixed character thus cannot follow taiwu.");
		}
		else
		{
			DomainManager.Extra.SetCharacterFollowTaiwu(runtime.Context, character.GetId(), distance);
		}
	}

	[EventFunction(120)]
	private static void CancelCharacterFollowTaiwu(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		if (character.GetCreatingType() != 0)
		{
			AdaptableLog.Warning($"{character} is is not a fixed character thus cannot follow taiwu.");
		}
		else
		{
			DomainManager.Extra.RemoveCharacterFollowTaiwu(runtime.Context, character.GetId());
		}
	}

	[EventFunction(127)]
	private static void FilterCharacterItem(EventScriptRuntime runtime, GameData.Domains.Character.Character character, string selectItemNameKey, sbyte itemType = -1, short itemSubType = -1, bool includeTransferable = false)
	{
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.FilterItemForCharacterByType(character.GetId(), selectItemNameKey, runtime.Current.ArgBox, itemType, itemSubType, includeTransferable);
	}

	[EventFunction(132)]
	private static void RegisterToSelectItemSubTypes(EventScriptRuntime runtime, short itemSubType)
	{
		runtime.Current.RegisterToSelectItemSubTypes(itemSubType);
	}

	[EventFunction(133)]
	private static void RegisterToSelectItemTemplateIds(EventScriptRuntime runtime, UnmanagedVariant<TemplateKey> itemTemplate)
	{
		runtime.Current.RegisterToSelectItemTemplateIds(((Variant<TemplateKey>)(object)itemTemplate).Value.ItemType, ((Variant<TemplateKey>)(object)itemTemplate).Value.TemplateId);
	}

	[EventFunction(134)]
	private static void RegisterToExcludeItemTemplateIds(EventScriptRuntime runtime, UnmanagedVariant<TemplateKey> itemTemplate)
	{
		runtime.Current.RegisterToExcludeItemTemplateIds(((Variant<TemplateKey>)(object)itemTemplate).Value.ItemType, ((Variant<TemplateKey>)(object)itemTemplate).Value.TemplateId);
	}

	[EventFunction(135)]
	private static void FilterCharacterItemByRegister(EventScriptRuntime runtime, GameData.Domains.Character.Character character, string selectItemNameKey, bool includeTransferable = false)
	{
		runtime.Current.FilterItemForCharacter(character, selectItemNameKey, includeTransferable);
	}

	[EventFunction(164)]
	private static void StartSetCharacterGivenName(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int maxStrCount)
	{
		Tester.Assert(character.IsCreatedWithFixedTemplate());
		runtime.Current.ArgBox.Remove<string>("InputResult");
		EventInputRequestData eventInputRequestData = new EventInputRequestData();
		eventInputRequestData.DataKey = "InputResult";
		eventInputRequestData.InputDataType = (sbyte)((character.GetTemplateId() == 836) ? 2 : 3);
		eventInputRequestData.FullName = runtime.Current.ArgBox.GetCharacter("CharacterId").GetFullName();
		eventInputRequestData.NumberRange = new int[2] { 1, maxStrCount };
		runtime.Current.ArgBox.Set("InputRequestData", (ISerializableGameData)(object)eventInputRequestData);
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.AddEventInListenWithActionName(runtime.Current.ScriptId.Guid, runtime.Current.ArgBox, "InputActionComplete");
	}

	[EventFunction(165)]
	private static void FinishSetCharacterGivenName(EventScriptRuntime runtime)
	{
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.RemoveEventInListenWithActionName(runtime.Current.ScriptId.Guid, "InputActionComplete");
		string value = runtime.Current.ArgBox.GetString("InputResult");
		if (!string.IsNullOrEmpty(value))
		{
			GameData.Domains.Character.Character character = runtime.Current.ArgBox.GetCharacter("CharacterId");
			Tester.Assert(character.IsCreatedWithFixedTemplate());
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SetCharacterGivenName(character, runtime.Current.ArgBox.GetString("InputResult"));
			MapBlockData block = DomainManager.Map.GetBlock(character.GetLocation());
			DomainManager.Map.SetBlockData(runtime.Context, block);
		}
	}

	[EventFunction(189)]
	private static void TakeRandomDamage(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int damage)
	{
		character.TakeRandomDamage(runtime.Context, damage);
	}

	[EventFunction(191)]
	private static int GetCharacterConsummateLevel(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		return character.GetConsummateLevel();
	}

	[EventFunction(200)]
	private static void SpecifyXiangshuInfectionValue(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int value)
	{
		value = (byte)Math.Clamp(value, 0, 200);
		character.SetXiangshuInfection((byte)value, runtime.Context);
		character.UpdateXiangshuInfectionState(runtime.Context);
	}

	[EventFunction(201)]
	private static void ChangeXiangshuInfectionValue(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int delta)
	{
		byte xiangshuInfection = character.GetXiangshuInfection();
		xiangshuInfection = (byte)Math.Clamp(xiangshuInfection + delta, 0, 200);
		character.SetXiangshuInfection(xiangshuInfection, runtime.Context);
		character.UpdateXiangshuInfectionState(runtime.Context);
	}

	[EventFunction(205)]
	private static void SetCharacterMarriageStyleOne(EventScriptRuntime runtime, GameData.Domains.Character.Character character, bool set)
	{
		if (set)
		{
			DomainManager.TaiwuEvent.AppendMarriageLook1CharId(character.GetId());
		}
		else
		{
			DomainManager.TaiwuEvent.RemoveMarriageLook1CharId(character.GetId());
		}
	}

	[EventFunction(206)]
	private static void SetCharacterMarriageStyleTwo(EventScriptRuntime runtime, GameData.Domains.Character.Character character, bool set)
	{
		if (set)
		{
			DomainManager.TaiwuEvent.AppendMarriageLook2CharId(character.GetId());
		}
		else
		{
			DomainManager.TaiwuEvent.RemoveMarriageLook2CharId(character.GetId());
		}
	}

	[EventFunction(210)]
	private static int GetCharacterBySettlementGradeAndAge(EventScriptRuntime runtime, Settlement settlement, sbyte gradeLower, sbyte gradeUpper, sbyte ageGroupLower, sbyte ageGroupUpper)
	{
		List<int> list = new List<int>();
		settlement.GetMembers().GetAllMembers(list);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			int objectId = list[num];
			if (!DomainManager.Character.TryGetElement_Objects(objectId, out var element))
			{
				list.RemoveAt(num);
			}
			else
			{
				OrganizationInfo organizationInfo = element.GetOrganizationInfo();
				if (organizationInfo.Grade < gradeLower || organizationInfo.Grade > gradeUpper)
				{
					list.RemoveAt(num);
				}
				else
				{
					sbyte ageGroup = element.GetAgeGroup();
					if (ageGroup < ageGroupLower || ageGroup > ageGroupUpper)
					{
						list.RemoveAt(num);
					}
				}
			}
		}
		return list.GetRandomOrDefault(runtime.Context.Random, -1);
	}

	[EventFunction(238)]
	private static void AddJieqingMaskCharId(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		DomainManager.TaiwuEvent.AddJieqingMaskCharId(character.GetId());
	}

	[EventFunction(239)]
	private static void RemoveJieqingMaskCharId(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		DomainManager.TaiwuEvent.RemoveJieqingMaskCharId(character.GetId());
	}

	[EventFunction(242)]
	private static void DestroyEnemyNest(EventScriptRuntime runtime, short enemyNestId, sbyte behaviorType)
	{
		if (behaviorType < 0)
		{
			throw new ArgumentOutOfRangeException("behaviorType", behaviorType, "Parameter '{nameof(behaviorType)}' must be >= 0, but was {behaviorType}.");
		}
		DomainManager.Adventure.DestroyEnemyNest(runtime.Context, DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId, enemyNestId, behaviorType);
	}

	[EventFunction(249)]
	private static int GetCurrentFaith(EventScriptRuntime runtime)
	{
		return DomainManager.Extra.GetFuyuFaith();
	}

	[EventFunction(250)]
	private static void GetFaithLevel(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int value, string faithLv = null, string giftLv = null, string debtVal = null, string favorVal = null)
	{
		int fuyuFaithLevel = GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetFuyuFaithLevel(character, value);
		if (!string.IsNullOrEmpty(faithLv))
		{
			runtime.Current.ArgBox.Set(faithLv, fuyuFaithLevel);
		}
		if (!string.IsNullOrEmpty(giftLv))
		{
			runtime.Current.ArgBox.Set(giftLv, GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetFuyuFaithGiftLevelByLevel(character, fuyuFaithLevel));
		}
		if (!string.IsNullOrEmpty(debtVal))
		{
			runtime.Current.ArgBox.Set(debtVal, GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetFuyuFaithDebtByLevel(character, fuyuFaithLevel));
		}
		if (!string.IsNullOrEmpty(favorVal))
		{
			runtime.Current.ArgBox.Set(favorVal, GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetFuyuFaithFavorByLevel(character, fuyuFaithLevel));
		}
	}

	[EventFunction(251)]
	private static int GetFuyuFaithTime(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		return character.GetDarkAshCounter().Tips3;
	}

	[EventFunction(252)]
	private static void OpenFuyuFaithPanel(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.StartSelectFuyuFaithCount(runtime.Current.ArgBox, character);
	}

	[EventFunction(253)]
	private static void OpenFuyuGiftPanel(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int grade, string itemKey)
	{
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.FilterItemForCharacterByType(character.GetId(), itemKey, runtime.Current.ArgBox, -1, -1, includeTransferable: false, new List<Predicate<ItemKey>>
		{
			(ItemKey itemKey2) => GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetItemGrade(itemKey2) <= grade
		});
		runtime.Current.ArgBox.Get("SelectItemInfo", out EventSelectItemData arg);
		short templateIdInGroup = ItemTemplateHelper.GetTemplateIdInGroup(12, 385, (sbyte)grade);
		arg.CanSelectItemList.Add(new ItemDisplayData(12, templateIdInGroup));
		ResourceInts resources = character.GetResources();
		for (int num = 0; num < 7; num++)
		{
			int num2 = resources[num];
			if (num2 > 0)
			{
				short templateId = Convert.ToInt16(321 + num);
				ItemKey key = new ItemKey(12, 0, templateId, 0);
				ItemDisplayData item = new ItemDisplayData
				{
					Key = key,
					Amount = num2
				};
				arg.CanSelectItemList.Add(item);
			}
		}
		arg.ResourceMaxValue = GlobalConfig.FuyuResourceValueMax[Math.Clamp(grade, 0, GlobalConfig.FuyuResourceValueMax.Length)];
	}

	[EventFunction(254)]
	private static void ApplyFuyuFaith(EventScriptRuntime runtime, GameData.Domains.Character.Character character, int duration)
	{
		character.ExtendDarkAshWithFuyuFaith(runtime.Context, duration, DomainManager.LifeRecord.GetLifeRecordCollection());
	}

	[EventFunction(255)]
	private static int ReadSelectResultCount(EventScriptRuntime runtime)
	{
		return runtime.Current.ArgBox.GetInt("SelectCountResult");
	}
}
