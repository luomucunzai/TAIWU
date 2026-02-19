using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Information;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World.Notification;
using GameData.Utilities;

public static class BroadcastEffect
{
	public static void OpenEffectEntrance(DataContext context, int metaDataId, bool isOffline, int sourceCharId, ref List<InformationDomain.SecretInformationStartEnemyRelationItem> offlineStartEnemyRelationItem)
	{
		SecretInformationProcessor secretInformationProcessor = DomainManager.Information.SecretInformationProcessorPool.Get();
		if (!secretInformationProcessor.Initialize(metaDataId))
		{
			DomainManager.Information.SecretInformationProcessorPool.Return(secretInformationProcessor);
			return;
		}
		secretInformationProcessor.Initialize_ForBroadcastEffect(context.Random);
		if (DomainManager.Taiwu.GetTaiwuCharId() == sourceCharId)
		{
			int arg = DomainManager.Information.CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(secretInformationProcessor.GetSecretInformationTemplateId(), DomainManager.Taiwu.GetTaiwu().GetFameType(), DomainManager.Information.GetSecretInformationDisseminatingCountOfBranch(metaDataId, 0));
			ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[34];
			int baseDelta = formulaCfg.Calculate(arg, SecretInformation.Instance[secretInformationProcessor.GetSecretInformationTemplateId()].SortValue);
			DomainManager.Extra.ChangeProfessionSeniority(context, 4, baseDelta);
		}
		List<InformationDomain.SecretInformationHappinessChangeItem> allSecretInformationHappinessChange = secretInformationProcessor.GetAllSecretInformationHappinessChange();
		foreach (InformationDomain.SecretInformationHappinessChangeItem item4 in allSecretInformationHappinessChange)
		{
			ChangeRoleHappiness(context, item4.CharacterId, item4.DeltaHappiness);
		}
		List<InformationDomain.SecretInformationFavorChangeItem> allSecretInformationFavorabilityChangeWithSource = secretInformationProcessor.GetAllSecretInformationFavorabilityChangeWithSource(sourceCharId);
		foreach (InformationDomain.SecretInformationFavorChangeItem item5 in allSecretInformationFavorabilityChangeWithSource)
		{
			ChangeFavorability(context, item5.CharacterId, item5.TargetId, item5.DeltaFavor);
		}
		List<InformationDomain.SecretInformationStartEnemyRelationItem> allSecretInformationStartEnemyRelationItems = secretInformationProcessor.GetAllSecretInformationStartEnemyRelationItems(sourceCharId);
		for (int num = allSecretInformationStartEnemyRelationItems.Count - 1; num >= 0; num--)
		{
			if (context.Random.CheckPercentProb(allSecretInformationStartEnemyRelationItems[num].Odds))
			{
				if (!isOffline && DomainManager.Character.TryGetElement_Objects(allSecretInformationStartEnemyRelationItems[num].CharacterId, out var element) && DomainManager.Character.TryGetElement_Objects(allSecretInformationStartEnemyRelationItems[num].TargetId, out var element2) && Config.Character.Instance[element.GetTemplateId()].CreatingType == 1 && Config.Character.Instance[element2.GetTemplateId()].CreatingType == 1)
				{
					GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, element, element2, allSecretInformationStartEnemyRelationItems[num].CharacterId == DomainManager.Taiwu.GetTaiwuCharId(), 5, new CharacterBecomeEnemyInfo(element)
					{
						SecretInformationTemplateId = secretInformationProcessor.GetSecretInformationTemplateId(),
						Location = element2.GetValidLocation()
					});
				}
			}
			else
			{
				allSecretInformationStartEnemyRelationItems.RemoveAt(num);
			}
		}
		offlineStartEnemyRelationItem.AddRange(allSecretInformationStartEnemyRelationItems);
		Dictionary<int, GameData.Domains.Character.Character> activeActorList_WithActorIndex = secretInformationProcessor.GetActiveActorList_WithActorIndex();
		Dictionary<int, List<short>> dictionary = new Dictionary<int, List<short>>();
		Dictionary<short, short> dictionary2 = new Dictionary<short, short>();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		foreach (KeyValuePair<int, GameData.Domains.Character.Character> item6 in activeActorList_WithActorIndex)
		{
			int id = item6.Value.GetId();
			List<short> actorFameRecord_WithActorIndex = secretInformationProcessor.GetActorFameRecord_WithActorIndex(item6.Key);
			dictionary2.Clear();
			foreach (short item7 in actorFameRecord_WithActorIndex)
			{
				dictionary2.TryGetValue(item7, out var value);
				dictionary2[item7] = (short)(value + 1);
			}
			foreach (KeyValuePair<short, short> item8 in dictionary2)
			{
				item6.Value.RecordFameAction(context, item8.Key, -1, item8.Value);
				if (item6.Value.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
				{
					InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
					sbyte fame = FameAction.Instance[item8.Key].Fame;
					if (fame > 0)
					{
						instantNotificationCollection.AddFameIncreased(id);
					}
					else if (fame < 0)
					{
						instantNotificationCollection.AddFameDecreased(id);
					}
				}
			}
			dictionary.Add(item6.Key, actorFameRecord_WithActorIndex);
			if (DomainManager.World.GetMainStoryLineProgress() < 16 || DomainManager.World.GetMainStoryLineProgress() >= 27 || EventHelper.DukeSkill_CheckCharacterHasTitle(id))
			{
				continue;
			}
			OrganizationInfo orgInfo;
			sbyte stateTemplateId;
			if (item6.Value != null && item6.Value.GetId() == taiwuCharId)
			{
				short reasonKey;
				sbyte b = secretInformationProcessor.CalcTaiwuPunishLevel(taiwuCharId, out reasonKey, out orgInfo);
				if (b >= 0)
				{
					GetBountyHome()?.AddBounty(context, item6.Value, b, reasonKey);
				}
			}
			else if (item6.Value != null && item6.Value.GetOrganizationInfo().SettlementId == DomainManager.Taiwu.GetTaiwuVillageSettlementId())
			{
				stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId);
				Sect sect = GetBountyHome2();
				if (sect != null)
				{
					OrganizationInfo organizationInfo = item6.Value.GetOrganizationInfo();
					organizationInfo.OrgTemplateId = EventHelper.GetStateMainCityOrgTemplateId(stateTemplateId);
					short reasonKey2;
					sbyte b2 = secretInformationProcessor.CalcSectPunishLevelWithSpecificOrganization(item6.Key, out reasonKey2, organizationInfo);
					if (b2 >= 0)
					{
						sect.AddBounty(context, item6.Value, b2, reasonKey2);
					}
				}
			}
			else
			{
				short reasonKey3;
				sbyte punishmentSeverity = secretInformationProcessor.CalSectPunishLevel_WithActorIndex(item6.Key, out reasonKey3);
				OrganizationInfo sectInfoSafe = secretInformationProcessor.GetSectInfoSafe(id);
				DomainManager.Organization.OnSectMemberCrimeMadePublic(context, item6.Value, sectInfoSafe, punishmentSeverity, reasonKey3);
			}
			Sect GetBountyHome()
			{
				if (!DomainManager.Organization.TryGetElement_Sects(orgInfo.SettlementId, out var element3))
				{
					if (orgInfo.SettlementId < 0)
					{
						return null;
					}
					Location location = DomainManager.Organization.GetSettlement(orgInfo.SettlementId).GetLocation();
					if (!location.IsValid())
					{
						return null;
					}
					sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
					MapStateItem mapStateItem = MapState.Instance[stateTemplateIdByAreaId];
					if (mapStateItem.SectID < 0)
					{
						return null;
					}
					element3 = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(mapStateItem.SectID);
				}
				return element3;
			}
			Sect GetBountyHome2()
			{
				MapStateItem mapStateItem = MapState.Instance[stateTemplateId];
				if (mapStateItem.SectID < 0)
				{
					return null;
				}
				return (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(mapStateItem.SectID);
			}
		}
		byte secretInformationActorBroadcastType = secretInformationProcessor.GetSecretInformationActorBroadcastType();
		if (secretInformationActorBroadcastType == 2)
		{
			DomainManager.Information.SecretInformationProcessorPool.Return(secretInformationProcessor);
			return;
		}
		SecretInformationBroadcastTipsData secretInformationBroadcastTipsData = new SecretInformationBroadcastTipsData();
		secretInformationBroadcastTipsData.MetaDataId = metaDataId;
		secretInformationBroadcastTipsData.BroadcastType = secretInformationActorBroadcastType;
		if (!dictionary.TryGetValue(0, out var value2))
		{
			value2 = new List<short>();
		}
		if (!dictionary.TryGetValue(1, out var value3))
		{
			value3 = new List<short>();
		}
		if (!dictionary.TryGetValue(2, out var value4))
		{
			value4 = new List<short>();
		}
		secretInformationBroadcastTipsData.FameActionsOfMain = new List<int>();
		foreach (short item9 in value2)
		{
			FameActionItem item = FameAction.Instance.GetItem(item9);
			if (item != null)
			{
				secretInformationBroadcastTipsData.FameActionsOfMain.Add(item9);
				secretInformationBroadcastTipsData.FameActionsOfMain.Add(item.Fame);
				secretInformationBroadcastTipsData.FameActionsOfMain.Add(item.Duration);
			}
		}
		secretInformationBroadcastTipsData.FameActionsOfTarget1 = new List<int>();
		foreach (short item10 in value3)
		{
			FameActionItem item2 = FameAction.Instance.GetItem(item10);
			if (item2 != null)
			{
				secretInformationBroadcastTipsData.FameActionsOfTarget1.Add(item10);
				secretInformationBroadcastTipsData.FameActionsOfTarget1.Add(item2.Fame);
				secretInformationBroadcastTipsData.FameActionsOfTarget1.Add(item2.Duration);
			}
		}
		secretInformationBroadcastTipsData.FameActionsOfTarget2 = new List<int>();
		foreach (short item11 in value4)
		{
			FameActionItem item3 = FameAction.Instance.GetItem(item11);
			if (item3 != null)
			{
				secretInformationBroadcastTipsData.FameActionsOfTarget2.Add(item11);
				secretInformationBroadcastTipsData.FameActionsOfTarget2.Add(item3.Fame);
				secretInformationBroadcastTipsData.FameActionsOfTarget2.Add(item3.Duration);
			}
		}
		int count = 4;
		secretInformationBroadcastTipsData.HappinessUpCharacters = (from x in allSecretInformationHappinessChange
			where x.DeltaHappiness > 0
			select x.CharacterId).Take(count).ToList();
		secretInformationBroadcastTipsData.HappinessDownCharacters = (from x in allSecretInformationHappinessChange
			where x.DeltaHappiness < 0
			select x.CharacterId).Take(count).ToList();
		List<int> actorList = secretInformationProcessor.GetSecretInformationArgList();
		secretInformationBroadcastTipsData.FavorToMainUpCharacters = (activeActorList_WithActorIndex.TryGetValue(0, out var value5) ? new List<int>() : (from x in allSecretInformationFavorabilityChangeWithSource
			where x.TargetId == actorList[0] && x.DeltaFavor > 0
			orderby x.Priority
			select x.CharacterId).Take(count).ToList());
		secretInformationBroadcastTipsData.FavorToMainDownCharacters = (activeActorList_WithActorIndex.TryGetValue(0, out value5) ? new List<int>() : (from x in allSecretInformationFavorabilityChangeWithSource
			where x.TargetId == actorList[0] && x.DeltaFavor < 0
			orderby x.Priority
			select x.CharacterId).Take(count).ToList());
		secretInformationBroadcastTipsData.FavorToTarget1UpCharacters = (activeActorList_WithActorIndex.TryGetValue(1, out value5) ? new List<int>() : (from x in allSecretInformationFavorabilityChangeWithSource
			where x.TargetId == actorList[1] && x.DeltaFavor > 0
			orderby x.Priority
			select x.CharacterId).Take(count).ToList());
		secretInformationBroadcastTipsData.FavorToTarget1DownCharacters = (activeActorList_WithActorIndex.TryGetValue(1, out value5) ? new List<int>() : (from x in allSecretInformationFavorabilityChangeWithSource
			where x.TargetId == actorList[1] && x.DeltaFavor < 0
			orderby x.Priority
			select x.CharacterId).Take(count).ToList());
		secretInformationBroadcastTipsData.FavorToTarget2UpCharacters = (activeActorList_WithActorIndex.TryGetValue(2, out value5) ? new List<int>() : (from x in allSecretInformationFavorabilityChangeWithSource
			where x.TargetId == actorList[2] && x.DeltaFavor > 0
			orderby x.Priority
			select x.CharacterId).Take(count).ToList());
		secretInformationBroadcastTipsData.FavorToTarget2DownCharacters = (activeActorList_WithActorIndex.TryGetValue(2, out value5) ? new List<int>() : (from x in allSecretInformationFavorabilityChangeWithSource
			where x.TargetId == actorList[2] && x.DeltaFavor < 0
			orderby x.Priority
			select x.CharacterId).Take(count).ToList());
		SecretInformationBroadcastTipsExtraData secretInformationBroadcastTipsExtraData = new SecretInformationBroadcastTipsExtraData();
		secretInformationBroadcastTipsExtraData.MetaDataId = metaDataId;
		secretInformationBroadcastTipsExtraData.StartEnemyRelationCharactersToActor = (from x in allSecretInformationStartEnemyRelationItems
			where x.TargetId == actorList[0]
			select x.CharacterId).Take(count).ToList();
		secretInformationBroadcastTipsExtraData.StartEnemyRelationCharactersToReactor = (from x in allSecretInformationStartEnemyRelationItems
			where x.TargetId == actorList[1]
			select x.CharacterId).Take(count).ToList();
		secretInformationBroadcastTipsExtraData.StartEnemyRelationCharactersToSecactor = (from x in allSecretInformationStartEnemyRelationItems
			where x.TargetId == actorList[2]
			select x.CharacterId).Take(count).ToList();
		if (sourceCharId != -1)
		{
			secretInformationBroadcastTipsExtraData.StartEnemyRelationCharactersToSource = (from x in allSecretInformationStartEnemyRelationItems
				where x.TargetId == sourceCharId
				select x.CharacterId).Take(count).ToList();
			secretInformationBroadcastTipsExtraData.StartEnemyRelationCharactersToSource.Insert(0, sourceCharId);
		}
		DomainManager.Extra.AddSecretInformationBroadcastNotify(secretInformationBroadcastTipsData, context);
		DomainManager.Extra.AddSecretInformationBroadcastNotifyExtra(secretInformationBroadcastTipsExtraData, context);
		DomainManager.Information.SecretInformationProcessorPool.Return(secretInformationProcessor);
	}

	private static void ChangeRoleHappiness(DataContext context, int characterId, int deltaValue)
	{
		if (!DomainManager.Character.TryGetElement_Objects(characterId, out var element))
		{
			return;
		}
		element.ChangeHappiness(context, deltaValue);
		if (DomainManager.Taiwu.IsInGroup(characterId))
		{
			InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
			if (deltaValue > 0)
			{
				instantNotificationCollection.AddHappinessIncreased(element.GetId());
			}
			else if (deltaValue < 0)
			{
				instantNotificationCollection.AddHappinessDecreased(element.GetId());
			}
		}
	}

	private static void ChangeFavorability(DataContext context, int charAId, int charBId, int deltaFavor)
	{
		if (charAId != charBId && DomainManager.Character.TryGetElement_Objects(charAId, out var element) && DomainManager.Character.TryGetElement_Objects(charBId, out var element2))
		{
			int num = deltaFavor;
			while (num != 0)
			{
				int num2 = Math.Clamp(num, -30000, 30000);
				EventHelper.ChangeFavorabilityOptional(element, element2, (short)num2, 3);
				num -= num2;
			}
		}
	}
}
