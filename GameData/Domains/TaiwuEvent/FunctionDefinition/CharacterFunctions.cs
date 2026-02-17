using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World.Notification;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000A2 RID: 162
	public class CharacterFunctions
	{
		// Token: 0x06001A7B RID: 6779 RVA: 0x00178130 File Offset: 0x00176330
		[EventFunction(18)]
		private unsafe static void SpecifyCurrMainAttribute(EventScriptRuntime runtime, Character character, sbyte mainAttributeType, int value)
		{
			MainAttributes mainAttributes = character.GetCurrMainAttributes();
			*(ref mainAttributes.Items.FixedElementField + (IntPtr)mainAttributeType * 2) = (short)value;
			character.SetCurrMainAttributes(mainAttributes, runtime.Context);
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x00178167 File Offset: 0x00176367
		[EventFunction(19)]
		private static void ChangeCurrMainAttribute(EventScriptRuntime runtime, Character character, sbyte mainAttributeType, int delta)
		{
			character.ChangeCurrMainAttribute(runtime.Context, mainAttributeType, delta);
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x0017817C File Offset: 0x0017637C
		[EventFunction(20)]
		private static void SpecifyInjury(EventScriptRuntime runtime, Character character, sbyte bodyPartType, bool isInner, int value)
		{
			value = Math.Clamp(value, 0, 6);
			Injuries injuries = character.GetInjuries();
			injuries.Set(bodyPartType, isInner, (sbyte)value);
			character.SetInjuries(injuries, runtime.Context);
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x001781B7 File Offset: 0x001763B7
		[EventFunction(21)]
		private static void ChangeInjury(EventScriptRuntime runtime, Character character, sbyte bodyPartType, bool isInner, int delta)
		{
			delta = Math.Clamp(delta, 0, 6);
			character.ChangeInjury(runtime.Context, bodyPartType, isInner, (sbyte)delta);
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x001781D8 File Offset: 0x001763D8
		[EventFunction(22)]
		private static void ClearInjuries(EventScriptRuntime runtime, Character character)
		{
			Injuries injuries = character.GetInjuries();
			injuries.Initialize();
			character.SetInjuries(injuries, runtime.Context);
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00178204 File Offset: 0x00176404
		[EventFunction(23)]
		private unsafe static void SpecifyPoisoned(EventScriptRuntime runtime, Character character, sbyte poisonType, int value)
		{
			ref PoisonInts poisoned = ref character.GetPoisoned();
			*poisoned[(int)poisonType] = value;
			character.SetPoisoned(ref poisoned, runtime.Context);
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x00178230 File Offset: 0x00176430
		[EventFunction(24)]
		private static void ChangePoisoned(EventScriptRuntime runtime, Character character, sbyte poisonType, int level, int delta)
		{
			level = Math.Clamp(level, 0, 3);
			character.ChangePoisoned(runtime.Context, poisonType, (sbyte)level, delta);
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00178250 File Offset: 0x00176450
		[EventFunction(25)]
		private static void ClearPoisons(EventScriptRuntime runtime, Character character)
		{
			ref PoisonInts poisoned = ref character.GetPoisoned();
			poisoned.Initialize();
			character.SetPoisoned(ref poisoned, runtime.Context);
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x0017827A File Offset: 0x0017647A
		[EventFunction(26)]
		private static void SpecifyDisorderOfQi(EventScriptRuntime runtime, Character character, int value)
		{
			value = Math.Clamp(value, (int)DisorderLevelOfQi.MinValue, (int)DisorderLevelOfQi.MaxValue);
			character.SetDisorderOfQi((short)value, runtime.Context);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x0017829E File Offset: 0x0017649E
		[EventFunction(27)]
		private static void ChangeDisorderOfQi(EventScriptRuntime runtime, Character character, int delta)
		{
			character.ChangeDisorderOfQi(runtime.Context, delta);
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x001782AF File Offset: 0x001764AF
		[EventFunction(28)]
		private static void SpecifyHealth(EventScriptRuntime runtime, Character character, int value)
		{
			value = Math.Clamp(value, 0, (int)character.GetLeftMaxHealth(false));
			character.SetHealth((short)value, runtime.Context);
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x001782D1 File Offset: 0x001764D1
		[EventFunction(29)]
		private static void ChangeHealth(EventScriptRuntime runtime, Character character, int delta)
		{
			character.ChangeHealth(runtime.Context, delta);
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x001782E2 File Offset: 0x001764E2
		[EventFunction(30)]
		private static void SpecifyHappiness(EventScriptRuntime runtime, Character character, int value)
		{
			value = Math.Clamp(value, -119, 119);
			character.SetHappiness((sbyte)value, runtime.Context);
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x00178300 File Offset: 0x00176500
		[EventFunction(31)]
		private static void ChangeHappiness(EventScriptRuntime runtime, Character character, int delta)
		{
			character.ChangeHappiness(runtime.Context, delta);
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x00178311 File Offset: 0x00176511
		[EventFunction(32)]
		private static void SpecifyFavorabilities(EventScriptRuntime runtime, Character self, Character target, int selfToTarget, int targetToSelf)
		{
			DomainManager.Character.DirectlySetFavorabilities(runtime.Context, self.GetId(), target.GetId(), (short)selfToTarget, (short)targetToSelf);
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x00178338 File Offset: 0x00176538
		[EventFunction(33)]
		private static void ChangeFavorability(EventScriptRuntime runtime, Character self, Character target, int delta, short type = -1)
		{
			int selfCharId = self.GetId();
			int targetCharId = target.GetId();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			byte selfCreatingType = self.GetCreatingType();
			byte targetCreatingType = target.GetCreatingType();
			bool flag = selfCreatingType == 0 || targetCreatingType == 0;
			if (flag)
			{
				bool flag2 = selfCharId != taiwuCharId && targetCharId != taiwuCharId;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(93, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to create relation between ");
					defaultInterpolatedStringHandler.AppendFormatted<Character>(self);
					defaultInterpolatedStringHandler.AppendLiteral(" and ");
					defaultInterpolatedStringHandler.AppendFormatted<Character>(target);
					defaultInterpolatedStringHandler.AppendLiteral(": fixed character can only create relation with taiwu.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int actualDelta = DomainManager.Character.CalcFavorabilityDelta(selfCharId, targetCharId, delta, type);
				DomainManager.Character.DirectlyChangeFavorabilityOptional(runtime.Context, self, target, actualDelta, type);
			}
			else
			{
				DomainManager.Character.ChangeFavorabilityOptional(runtime.Context, self, target, delta, type);
			}
			DomainManager.Character.AddFavorabilityChangeInstantNotification(self, target, delta > 0);
			bool flag3 = target.IsTaiwu();
			if (flag3)
			{
				DomainManager.TaiwuEvent.RecordFavorabilityToTaiwuChanged(selfCharId, (short)delta);
			}
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x00178453 File Offset: 0x00176653
		[EventFunction(34)]
		private static void AddFeature(EventScriptRuntime runtime, Character character, short featureId, bool removeMutexFeature)
		{
			character.AddFeature(runtime.Context, featureId, removeMutexFeature);
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x00178465 File Offset: 0x00176665
		[EventFunction(35)]
		private static void RemoveFeature(EventScriptRuntime runtime, Character character, short featureId)
		{
			character.RemoveFeature(runtime.Context, featureId);
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x00178478 File Offset: 0x00176678
		[EventFunction(36)]
		private static void AddKidnappedCharacter(EventScriptRuntime runtime, Character character, Character kidnappedChar, ItemKey ropeItemKey)
		{
			bool flag = character.GetKidnapperId() >= 0;
			if (!flag)
			{
				int kidnapperId = character.GetId();
				int kidnappedCharId = kidnappedChar.GetId();
				int srcKidnapperId = kidnappedChar.GetKidnapperId();
				bool flag2 = srcKidnapperId == kidnapperId;
				if (flag2)
				{
					DomainManager.Character.ChangeKidnappedCharacterRope(runtime.Context, kidnapperId, kidnappedCharId, ropeItemKey);
				}
				else
				{
					bool flag3 = srcKidnapperId >= 0;
					if (flag3)
					{
						KidnappedCharacterList kidnappedCharacterList = DomainManager.Character.GetKidnappedCharacters(srcKidnapperId);
						int index = kidnappedCharacterList.IndexOf(kidnappedCharId);
						KidnappedCharacter kidnappedCharacter = kidnappedCharacterList.Get(index);
						DomainManager.Character.TransferKidnappedCharacter(runtime.Context, kidnapperId, srcKidnapperId, kidnappedCharacter);
					}
					else
					{
						DomainManager.Character.AddKidnappedCharacter(runtime.Context, character, kidnappedChar, ropeItemKey);
					}
				}
			}
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x0017852C File Offset: 0x0017672C
		[EventFunction(37)]
		private static void RemoveKidnappedCharacter(EventScriptRuntime runtime, Character character, Character kidnappedChar, bool isEscape)
		{
			DomainManager.Character.RemoveKidnappedCharacter(runtime.Context, character.GetId(), kidnappedChar.GetId(), isEscape);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x00178550 File Offset: 0x00176750
		[EventFunction(38)]
		private static void JoinGroup(EventScriptRuntime runtime, Character character, Character leader)
		{
			int charId = character.GetId();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = charId == taiwuCharId;
			if (!flag)
			{
				bool flag2 = character.GetKidnapperId() >= 0 || leader.GetKidnapperId() >= 0;
				if (!flag2)
				{
					bool flag3 = character.GetLeaderId() >= 0;
					if (flag3)
					{
						DomainManager.Character.LeaveGroup(runtime.Context, character, true);
					}
					bool flag4 = leader.GetId() == taiwuCharId || leader.GetLeaderId() == taiwuCharId;
					if (flag4)
					{
						DomainManager.Taiwu.JoinGroup(runtime.Context, charId, true);
					}
					else
					{
						DomainManager.Character.JoinGroup(runtime.Context, character, leader);
					}
				}
			}
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x00178603 File Offset: 0x00176803
		[EventFunction(39)]
		private static void LeaveGroup(EventScriptRuntime runtime, Character character, bool bringWard)
		{
			DomainManager.Character.LeaveGroup(runtime.Context, character, bringWard);
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x0017861C File Offset: 0x0017681C
		[EventFunction(40)]
		private static void KillCharacter(EventScriptRuntime runtime, Character victim, Character killer, short deathType)
		{
			bool flag = victim.GetCreatingType() == 1;
			if (flag)
			{
				EventHelper.KillAddTianJieFuLu(killer.GetId(), victim, true);
				DomainManager.Character.MakeCharacterDead(runtime.Context, victim, deathType, new CharacterDeathInfo(victim.GetValidLocation())
				{
					KillerId = ((killer != null) ? killer.GetId() : -1)
				});
			}
			else
			{
				EventHelper.RemoveNonIntelligentCharacter(victim);
			}
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x00178684 File Offset: 0x00176884
		[EventFunction(41)]
		private static void AddInventoryItem(EventScriptRuntime runtime, Character character, ItemKey itemKey, int amount)
		{
			Tester.Assert(itemKey.IsValid(), "");
			character.AddInventoryItem(runtime.Context, itemKey, amount, false);
			bool flag = character == DomainManager.Taiwu.GetTaiwu();
			if (flag)
			{
				InstantNotificationCollection instantNotification = DomainManager.World.GetInstantNotifications();
				instantNotification.AddGetItem(character.GetId(), itemKey.ItemType, itemKey.TemplateId);
				runtime.Current.RegisterToShowGetItem(itemKey, amount);
			}
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x001786FC File Offset: 0x001768FC
		[EventFunction(65)]
		private static void RemoveInventoryItem(EventScriptRuntime runtime, Character character, ItemKey itemKey, int amount, bool deleteItem)
		{
			Tester.Assert(itemKey.IsValid(), "");
			character.RemoveInventoryItem(runtime.Context, itemKey, amount, deleteItem, false);
			bool flag = character == DomainManager.Taiwu.GetTaiwu();
			if (flag)
			{
				InstantNotificationCollection instantNotification = DomainManager.World.GetInstantNotifications();
				instantNotification.AddLoseItem(character.GetId(), itemKey.ItemType, itemKey.TemplateId);
			}
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00178768 File Offset: 0x00176968
		[EventFunction(42)]
		private static void TransferInventoryItem(EventScriptRuntime runtime, Character character, Character destChar, ItemKey itemKey, int amount, bool favorAndDebt)
		{
			Tester.Assert(itemKey.HasTemplate, "");
			sbyte resourceType = ItemTemplateHelper.GetMiscResourceType(itemKey.ItemType, itemKey.TemplateId);
			bool flag = resourceType == -1;
			if (flag)
			{
				bool flag2 = !itemKey.IsValid();
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Creating item by template on transfer: ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					itemKey = DomainManager.Item.CreateItem(runtime.Context, itemKey.ItemType, itemKey.TemplateId);
					character.AddInventoryItem(runtime.Context, itemKey, amount, false);
				}
				Character taiwu = DomainManager.Taiwu.GetTaiwu();
				DomainManager.Character.TransferInventoryItem(runtime.Context, character, destChar, itemKey, amount);
				if (favorAndDebt)
				{
					bool includeTaiwu = character == taiwu || destChar == taiwu;
					bool bothIntelligent = character.GetCreatingType() == 1 && destChar.GetCreatingType() == 1;
					int itemFavor = DomainManager.Item.GetBaseItem(itemKey).GetFavorabilityChange();
					bool flag3 = !bothIntelligent;
					if (flag3)
					{
						int actualDelta = CharacterDomain.CalcFavorabilityDelta(destChar, character, itemFavor, -1);
						DomainManager.Character.DirectlyChangeFavorabilityOptional(runtime.Context, destChar, character, actualDelta, 1);
					}
					else
					{
						bool flag4 = !includeTaiwu;
						if (flag4)
						{
							DomainManager.Character.ChangeFavorabilityOptional(runtime.Context, destChar, character, itemFavor, 1);
						}
						else
						{
							DomainManager.Character.UpdateDebtByItemTransfer(runtime.Context, character, destChar, itemKey, amount, true);
						}
					}
				}
				bool flag5 = destChar == taiwu;
				if (flag5)
				{
					InstantNotificationCollection instantNotification = DomainManager.World.GetInstantNotifications();
					instantNotification.AddGetItem(destChar.GetId(), itemKey.ItemType, itemKey.TemplateId);
					runtime.Current.RegisterToShowGetItem(itemKey, amount);
				}
			}
			else
			{
				CharacterFunctions.TransferCharacterResource(runtime, character, destChar, resourceType, amount, favorAndDebt);
			}
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x00178940 File Offset: 0x00176B40
		[EventFunction(129)]
		private static void SpecifyCharacterResource(EventScriptRuntime runtime, Character character, sbyte resourceType, int value)
		{
			character.SpecifyResource(runtime.Context, resourceType, value);
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x00178954 File Offset: 0x00176B54
		[EventFunction(66)]
		private static void ChangeCharacterResource(EventScriptRuntime runtime, Character character, sbyte resourceType, int delta)
		{
			character.ChangeResource(runtime.Context, resourceType, delta);
			bool flag = character == DomainManager.Taiwu.GetTaiwu();
			if (flag)
			{
				InstantNotificationCollection instantNotification = DomainManager.World.GetInstantNotifications();
				bool flag2 = delta > 0;
				if (flag2)
				{
					instantNotification.AddResourceIncreased(character.GetId(), resourceType, delta);
					runtime.Current.RegisterToShowGetResource(resourceType, delta);
				}
				else
				{
					instantNotification.AddResourceDecreased(character.GetId(), resourceType, -delta);
				}
			}
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x001789C8 File Offset: 0x00176BC8
		[EventFunction(130)]
		private unsafe static void TransferCharacterResource(EventScriptRuntime runtime, Character character, Character destChar, sbyte resourceType, int amount, bool favorAndDebt)
		{
			DomainManager.Character.TransferResource(runtime.Context, character, destChar, resourceType, amount);
			Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = destChar == taiwuChar;
			if (flag)
			{
				InstantNotificationCollection instantNotification = DomainManager.World.GetInstantNotifications();
				instantNotification.AddResourceIncreased(character.GetId(), resourceType, amount);
				runtime.Current.RegisterToShowGetResource(resourceType, amount);
			}
			else
			{
				bool flag2 = character == taiwuChar;
				if (flag2)
				{
					InstantNotificationCollection instantNotification2 = DomainManager.World.GetInstantNotifications();
					instantNotification2.AddResourceDecreased(character.GetId(), resourceType, amount);
				}
			}
			bool flag3 = !favorAndDebt;
			if (!flag3)
			{
				bool includeTaiwu = character == taiwuChar || destChar == taiwuChar;
				bool bothIntelligent = character.GetCreatingType() == 1 && destChar.GetCreatingType() == 1;
				short resourceFavor = AiHelper.GeneralActionConstants.GetResourceFavorabilityChange(resourceType, amount);
				bool flag4 = !bothIntelligent;
				if (flag4)
				{
					DomainManager.Character.DirectlyChangeFavorabilityOptional(runtime.Context, destChar, character, (int)resourceFavor, 1);
				}
				else
				{
					bool flag5 = !includeTaiwu;
					if (flag5)
					{
						DomainManager.Character.ChangeFavorabilityOptional(runtime.Context, destChar, character, (int)resourceFavor, 1);
					}
					else
					{
						ResourceInts resources = default(ResourceInts);
						resources.Initialize();
						*resources[(int)resourceType] = amount;
						DomainManager.Character.UpdateDebtByResourceTransfer(runtime.Context, character, destChar, resources, true);
					}
				}
			}
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x00178B0A File Offset: 0x00176D0A
		[EventFunction(43)]
		private static void ChangeCharBaseCombatSkillQualification(EventScriptRuntime runtime, Character character, sbyte combatSkillType, int delta)
		{
			character.ChangeBaseCombatSkillQualification(runtime.Context, combatSkillType, delta);
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00178B1C File Offset: 0x00176D1C
		[EventFunction(44)]
		private static void ChangeCharBaseLifeSkillQualification(EventScriptRuntime runtime, Character character, sbyte lifeSkillType, int delta)
		{
			character.ChangeBaseLifeSkillQualification(runtime.Context, lifeSkillType, delta);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x00178B2E File Offset: 0x00176D2E
		[EventFunction(45)]
		private static void LearnCombatSkill(EventScriptRuntime runtime, Character character, short templateId)
		{
			DomainManager.Character.LearnCombatSkill(runtime.Context, character.GetId(), templateId, 0);
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00178B4A File Offset: 0x00176D4A
		[EventFunction(46)]
		private static void LearnLifeSkill(EventScriptRuntime runtime, Character character, short templateId)
		{
			DomainManager.Character.LearnLifeSkill(runtime.Context, character.GetId(), templateId, 0);
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00178B68 File Offset: 0x00176D68
		[EventFunction(111)]
		private static int GetCharacterFavorability(EventScriptRuntime runtime, Character selfChar, Character targetChar)
		{
			return (int)DomainManager.Character.GetFavorability(selfChar.GetId(), targetChar.GetId());
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00178B90 File Offset: 0x00176D90
		[EventFunction(117)]
		private static void SetCharacterFollowTaiwu(EventScriptRuntime runtime, Character character, int distance)
		{
			bool flag = character.GetCreatingType() > 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" is is not a fixed character thus cannot follow taiwu.");
				AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
			}
			else
			{
				DomainManager.Extra.SetCharacterFollowTaiwu(runtime.Context, character.GetId(), distance);
			}
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00178BF4 File Offset: 0x00176DF4
		[EventFunction(120)]
		private static void CancelCharacterFollowTaiwu(EventScriptRuntime runtime, Character character)
		{
			bool flag = character.GetCreatingType() > 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" is is not a fixed character thus cannot follow taiwu.");
				AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
			}
			else
			{
				DomainManager.Extra.RemoveCharacterFollowTaiwu(runtime.Context, character.GetId());
			}
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00178C57 File Offset: 0x00176E57
		[EventFunction(127)]
		private static void FilterCharacterItem(EventScriptRuntime runtime, Character character, string selectItemNameKey, sbyte itemType = -1, short itemSubType = -1, bool includeTransferable = false)
		{
			EventHelper.FilterItemForCharacterByType(character.GetId(), selectItemNameKey, runtime.Current.ArgBox, itemType, itemSubType, includeTransferable, null);
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00178C78 File Offset: 0x00176E78
		[EventFunction(132)]
		private static void RegisterToSelectItemSubTypes(EventScriptRuntime runtime, short itemSubType)
		{
			runtime.Current.RegisterToSelectItemSubTypes(itemSubType);
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00178C88 File Offset: 0x00176E88
		[EventFunction(133)]
		private static void RegisterToSelectItemTemplateIds(EventScriptRuntime runtime, UnmanagedVariant<TemplateKey> itemTemplate)
		{
			runtime.Current.RegisterToSelectItemTemplateIds(itemTemplate.Value.ItemType, itemTemplate.Value.TemplateId);
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x00178CAD File Offset: 0x00176EAD
		[EventFunction(134)]
		private static void RegisterToExcludeItemTemplateIds(EventScriptRuntime runtime, UnmanagedVariant<TemplateKey> itemTemplate)
		{
			runtime.Current.RegisterToExcludeItemTemplateIds(itemTemplate.Value.ItemType, itemTemplate.Value.TemplateId);
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00178CD2 File Offset: 0x00176ED2
		[EventFunction(135)]
		private static void FilterCharacterItemByRegister(EventScriptRuntime runtime, Character character, string selectItemNameKey, bool includeTransferable = false)
		{
			runtime.Current.FilterItemForCharacter(character, selectItemNameKey, includeTransferable);
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x00178CE4 File Offset: 0x00176EE4
		[EventFunction(164)]
		private static void StartSetCharacterGivenName(EventScriptRuntime runtime, Character character, int maxStrCount)
		{
			Tester.Assert(character.IsCreatedWithFixedTemplate(), "");
			runtime.Current.ArgBox.Remove<string>("InputResult");
			EventInputRequestData inputData = new EventInputRequestData();
			inputData.DataKey = "InputResult";
			inputData.InputDataType = ((character.GetTemplateId() == 836) ? 2 : 3);
			inputData.FullName = runtime.Current.ArgBox.GetCharacter("CharacterId").GetFullName();
			inputData.NumberRange = new int[]
			{
				1,
				maxStrCount
			};
			runtime.Current.ArgBox.Set("InputRequestData", inputData);
			EventHelper.AddEventInListenWithActionName(runtime.Current.ScriptId.Guid, runtime.Current.ArgBox, "InputActionComplete");
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00178DB4 File Offset: 0x00176FB4
		[EventFunction(165)]
		private static void FinishSetCharacterGivenName(EventScriptRuntime runtime)
		{
			EventHelper.RemoveEventInListenWithActionName(runtime.Current.ScriptId.Guid, "InputActionComplete");
			string input = runtime.Current.ArgBox.GetString("InputResult");
			bool flag = !string.IsNullOrEmpty(input);
			if (flag)
			{
				Character character = runtime.Current.ArgBox.GetCharacter("CharacterId");
				Tester.Assert(character.IsCreatedWithFixedTemplate(), "");
				EventHelper.SetCharacterGivenName(character, runtime.Current.ArgBox.GetString("InputResult"));
				MapBlockData mapBlockData = DomainManager.Map.GetBlock(character.GetLocation());
				DomainManager.Map.SetBlockData(runtime.Context, mapBlockData);
			}
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x00178E68 File Offset: 0x00177068
		[EventFunction(189)]
		private static void TakeRandomDamage(EventScriptRuntime runtime, Character character, int damage)
		{
			character.TakeRandomDamage(runtime.Context, damage);
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00178E7C File Offset: 0x0017707C
		[EventFunction(191)]
		private static int GetCharacterConsummateLevel(EventScriptRuntime runtime, Character character)
		{
			return (int)character.GetConsummateLevel();
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x00178E94 File Offset: 0x00177094
		[EventFunction(200)]
		private static void SpecifyXiangshuInfectionValue(EventScriptRuntime runtime, Character character, int value)
		{
			value = (int)((byte)Math.Clamp(value, 0, 200));
			character.SetXiangshuInfection((byte)value, runtime.Context);
			character.UpdateXiangshuInfectionState(runtime.Context);
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x00178EC4 File Offset: 0x001770C4
		[EventFunction(201)]
		private static void ChangeXiangshuInfectionValue(EventScriptRuntime runtime, Character character, int delta)
		{
			byte value = character.GetXiangshuInfection();
			value = (byte)Math.Clamp((int)value + delta, 0, 200);
			character.SetXiangshuInfection(value, runtime.Context);
			character.UpdateXiangshuInfectionState(runtime.Context);
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x00178F04 File Offset: 0x00177104
		[EventFunction(205)]
		private static void SetCharacterMarriageStyleOne(EventScriptRuntime runtime, Character character, bool set)
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

		// Token: 0x06001AAB RID: 6827 RVA: 0x00178F3C File Offset: 0x0017713C
		[EventFunction(206)]
		private static void SetCharacterMarriageStyleTwo(EventScriptRuntime runtime, Character character, bool set)
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

		// Token: 0x06001AAC RID: 6828 RVA: 0x00178F74 File Offset: 0x00177174
		[EventFunction(210)]
		private static int GetCharacterBySettlementGradeAndAge(EventScriptRuntime runtime, Settlement settlement, sbyte gradeLower, sbyte gradeUpper, sbyte ageGroupLower, sbyte ageGroupUpper)
		{
			List<int> characters = new List<int>();
			settlement.GetMembers().GetAllMembers(characters);
			for (int index = characters.Count - 1; index >= 0; index--)
			{
				int charId = characters[index];
				Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag)
				{
					characters.RemoveAt(index);
				}
				else
				{
					OrganizationInfo orgInfo = character.GetOrganizationInfo();
					bool flag2 = orgInfo.Grade < gradeLower || orgInfo.Grade > gradeUpper;
					if (flag2)
					{
						characters.RemoveAt(index);
					}
					else
					{
						sbyte ageGroup = character.GetAgeGroup();
						bool flag3 = ageGroup < ageGroupLower || ageGroup > ageGroupUpper;
						if (flag3)
						{
							characters.RemoveAt(index);
						}
					}
				}
			}
			return characters.GetRandomOrDefault(runtime.Context.Random, -1);
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00179052 File Offset: 0x00177252
		[EventFunction(238)]
		private static void AddJieqingMaskCharId(EventScriptRuntime runtime, Character character)
		{
			DomainManager.TaiwuEvent.AddJieqingMaskCharId(character.GetId());
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x00179066 File Offset: 0x00177266
		[EventFunction(239)]
		private static void RemoveJieqingMaskCharId(EventScriptRuntime runtime, Character character)
		{
			DomainManager.TaiwuEvent.RemoveJieqingMaskCharId(character.GetId());
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x0017907C File Offset: 0x0017727C
		[EventFunction(242)]
		private static void DestroyEnemyNest(EventScriptRuntime runtime, short enemyNestId, sbyte behaviorType)
		{
			bool flag = behaviorType < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("behaviorType", behaviorType, "Parameter '{nameof(behaviorType)}' must be >= 0, but was {behaviorType}.");
			}
			DomainManager.Adventure.DestroyEnemyNest(runtime.Context, DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId, enemyNestId, behaviorType);
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x001790D0 File Offset: 0x001772D0
		[EventFunction(249)]
		private static int GetCurrentFaith(EventScriptRuntime runtime)
		{
			return DomainManager.Extra.GetFuyuFaith();
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x001790DC File Offset: 0x001772DC
		[EventFunction(250)]
		private static void GetFaithLevel(EventScriptRuntime runtime, Character character, int value, string faithLv = null, string giftLv = null, string debtVal = null, string favorVal = null)
		{
			int faithLevel = EventHelper.GetFuyuFaithLevel(character, value);
			bool flag = !string.IsNullOrEmpty(faithLv);
			if (flag)
			{
				runtime.Current.ArgBox.Set(faithLv, faithLevel);
			}
			bool flag2 = !string.IsNullOrEmpty(giftLv);
			if (flag2)
			{
				runtime.Current.ArgBox.Set(giftLv, EventHelper.GetFuyuFaithGiftLevelByLevel(character, faithLevel));
			}
			bool flag3 = !string.IsNullOrEmpty(debtVal);
			if (flag3)
			{
				runtime.Current.ArgBox.Set(debtVal, EventHelper.GetFuyuFaithDebtByLevel(character, faithLevel));
			}
			bool flag4 = !string.IsNullOrEmpty(favorVal);
			if (flag4)
			{
				runtime.Current.ArgBox.Set(favorVal, EventHelper.GetFuyuFaithFavorByLevel(character, faithLevel));
			}
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x0017918C File Offset: 0x0017738C
		[EventFunction(251)]
		private static int GetFuyuFaithTime(EventScriptRuntime runtime, Character character)
		{
			return character.GetDarkAshCounter().Tips3;
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x00179199 File Offset: 0x00177399
		[EventFunction(252)]
		private static void OpenFuyuFaithPanel(EventScriptRuntime runtime, Character character)
		{
			EventHelper.StartSelectFuyuFaithCount(runtime.Current.ArgBox, character);
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x001791B0 File Offset: 0x001773B0
		[EventFunction(253)]
		private unsafe static void OpenFuyuGiftPanel(EventScriptRuntime runtime, Character character, int grade, string itemKey)
		{
			EventHelper.FilterItemForCharacterByType(character.GetId(), itemKey2, runtime.Current.ArgBox, -1, -1, false, new List<Predicate<ItemKey>>
			{
				(ItemKey itemKey) => (int)EventHelper.GetItemGrade(itemKey) <= grade
			});
			EventSelectItemData selectItemData;
			runtime.Current.ArgBox.Get<EventSelectItemData>("SelectItemInfo", out selectItemData);
			short itemTemplateId = ItemTemplateHelper.GetTemplateIdInGroup(12, 385, (sbyte)grade);
			selectItemData.CanSelectItemList.Add(new ItemDisplayData(12, itemTemplateId));
			ResourceInts resources = *character.GetResources();
			for (int type = 0; type < 7; type++)
			{
				int amount = *resources[type];
				bool flag = amount <= 0;
				if (!flag)
				{
					short templateId = Convert.ToInt16(321 + type);
					ItemKey tmpItemKey = new ItemKey(12, 0, templateId, 0);
					ItemDisplayData itemData = new ItemDisplayData
					{
						Key = tmpItemKey,
						Amount = amount
					};
					selectItemData.CanSelectItemList.Add(itemData);
				}
			}
			selectItemData.ResourceMaxValue = GlobalConfig.FuyuResourceValueMax[Math.Clamp(grade, 0, GlobalConfig.FuyuResourceValueMax.Length)];
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x001792DF File Offset: 0x001774DF
		[EventFunction(254)]
		private static void ApplyFuyuFaith(EventScriptRuntime runtime, Character character, int duration)
		{
			character.ExtendDarkAshWithFuyuFaith(runtime.Context, duration, DomainManager.LifeRecord.GetLifeRecordCollection());
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x001792F9 File Offset: 0x001774F9
		[EventFunction(255)]
		private static int ReadSelectResultCount(EventScriptRuntime runtime)
		{
			return runtime.Current.ArgBox.GetInt("SelectCountResult");
		}
	}
}
