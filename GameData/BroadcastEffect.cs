using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

// Token: 0x0200000E RID: 14
public static class BroadcastEffect
{
	// Token: 0x0600002E RID: 46 RVA: 0x0004E0BC File Offset: 0x0004C2BC
	public static void OpenEffectEntrance(DataContext context, int metaDataId, bool isOffline, int sourceCharId, ref List<InformationDomain.SecretInformationStartEnemyRelationItem> offlineStartEnemyRelationItem)
	{
		SecretInformationProcessor processor = DomainManager.Information.SecretInformationProcessorPool.Get();
		bool flag = !processor.Initialize(metaDataId);
		if (flag)
		{
			DomainManager.Information.SecretInformationProcessorPool.Return(processor);
		}
		else
		{
			processor.Initialize_ForBroadcastEffect(context.Random);
			bool flag2 = DomainManager.Taiwu.GetTaiwuCharId() == sourceCharId;
			if (flag2)
			{
				int cost = DomainManager.Information.CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(processor.GetSecretInformationTemplateId(), DomainManager.Taiwu.GetTaiwu().GetFameType(), DomainManager.Information.GetSecretInformationDisseminatingCountOfBranch(metaDataId, 0));
				ProfessionFormulaItem formula = ProfessionFormula.Instance[34];
				int addSeniority = formula.Calculate(cost, (int)SecretInformation.Instance[processor.GetSecretInformationTemplateId()].SortValue);
				DomainManager.Extra.ChangeProfessionSeniority(context, 4, addSeniority, true, false);
			}
			List<InformationDomain.SecretInformationHappinessChangeItem> happinessList = processor.GetAllSecretInformationHappinessChange();
			foreach (InformationDomain.SecretInformationHappinessChangeItem item in happinessList)
			{
				BroadcastEffect.ChangeRoleHappiness(context, item.CharacterId, item.DeltaHappiness);
			}
			List<InformationDomain.SecretInformationFavorChangeItem> favorList = processor.GetAllSecretInformationFavorabilityChangeWithSource(sourceCharId);
			foreach (InformationDomain.SecretInformationFavorChangeItem item2 in favorList)
			{
				BroadcastEffect.ChangeFavorability(context, item2.CharacterId, item2.TargetId, item2.DeltaFavor);
			}
			List<InformationDomain.SecretInformationStartEnemyRelationItem> startEnemyRelationItem = processor.GetAllSecretInformationStartEnemyRelationItems(sourceCharId);
			for (int i = startEnemyRelationItem.Count - 1; i >= 0; i--)
			{
				bool flag3 = context.Random.CheckPercentProb((int)startEnemyRelationItem[i].Odds);
				if (flag3)
				{
					bool flag4 = !isOffline;
					if (flag4)
					{
						GameData.Domains.Character.Character character;
						GameData.Domains.Character.Character targetChar;
						bool flag5 = DomainManager.Character.TryGetElement_Objects(startEnemyRelationItem[i].CharacterId, out character) && DomainManager.Character.TryGetElement_Objects(startEnemyRelationItem[i].TargetId, out targetChar) && Config.Character.Instance[character.GetTemplateId()].CreatingType == 1 && Config.Character.Instance[targetChar.GetTemplateId()].CreatingType == 1;
						if (flag5)
						{
							GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, character, targetChar, startEnemyRelationItem[i].CharacterId == DomainManager.Taiwu.GetTaiwuCharId(), 5, new CharacterBecomeEnemyInfo(character)
							{
								SecretInformationTemplateId = processor.GetSecretInformationTemplateId(),
								Location = targetChar.GetValidLocation()
							});
						}
					}
				}
				else
				{
					startEnemyRelationItem.RemoveAt(i);
				}
			}
			offlineStartEnemyRelationItem.AddRange(startEnemyRelationItem);
			Dictionary<int, GameData.Domains.Character.Character> activeActorList = processor.GetActiveActorList_WithActorIndex();
			Dictionary<int, List<short>> allActorFameList = new Dictionary<int, List<short>>();
			Dictionary<short, short> collectedFameWithLevel = new Dictionary<short, short>();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			foreach (KeyValuePair<int, GameData.Domains.Character.Character> item3 in activeActorList)
			{
				int charId = item3.Value.GetId();
				List<short> fameList = processor.GetActorFameRecord_WithActorIndex(item3.Key, false);
				collectedFameWithLevel.Clear();
				foreach (short fameKey in fameList)
				{
					short level;
					collectedFameWithLevel.TryGetValue(fameKey, out level);
					collectedFameWithLevel[fameKey] = level + 1;
				}
				foreach (KeyValuePair<short, short> famePair in collectedFameWithLevel)
				{
					item3.Value.RecordFameAction(context, famePair.Key, -1, famePair.Value, true);
					bool flag6 = item3.Value.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
					if (flag6)
					{
						InstantNotificationCollection collection = DomainManager.World.GetInstantNotificationCollection();
						sbyte changeValue = FameAction.Instance[famePair.Key].Fame;
						bool flag7 = changeValue > 0;
						if (flag7)
						{
							collection.AddFameIncreased(charId);
						}
						else
						{
							bool flag8 = changeValue < 0;
							if (flag8)
							{
								collection.AddFameDecreased(charId);
							}
						}
					}
				}
				allActorFameList.Add(item3.Key, fameList);
				bool flag9 = DomainManager.World.GetMainStoryLineProgress() < 16;
				if (!flag9)
				{
					bool flag10 = DomainManager.World.GetMainStoryLineProgress() >= 27;
					if (!flag10)
					{
						bool hasBeenFree = EventHelper.DukeSkill_CheckCharacterHasTitle(charId);
						bool flag11 = hasBeenFree;
						if (!flag11)
						{
							bool flag12 = item3.Value != null && item3.Value.GetId() == taiwuCharId;
							if (flag12)
							{
								short reasonKey;
								BroadcastEffect.<>c__DisplayClass0_1 CS$<>8__locals2;
								sbyte punishLevel = processor.CalcTaiwuPunishLevel(taiwuCharId, out reasonKey, out CS$<>8__locals2.orgInfo);
								bool flag13 = punishLevel >= 0;
								if (flag13)
								{
									Sect sect = BroadcastEffect.<OpenEffectEntrance>g__GetBountyHome|0_30(ref CS$<>8__locals2);
									if (sect != null)
									{
										sect.AddBounty(context, item3.Value, punishLevel, reasonKey, -1);
									}
								}
							}
							else
							{
								bool flag14 = item3.Value != null && item3.Value.GetOrganizationInfo().SettlementId == DomainManager.Taiwu.GetTaiwuVillageSettlementId();
								if (flag14)
								{
									BroadcastEffect.<>c__DisplayClass0_2 CS$<>8__locals3;
									CS$<>8__locals3.stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId);
									Sect home = BroadcastEffect.<OpenEffectEntrance>g__GetBountyHome|0_31(ref CS$<>8__locals3);
									bool flag15 = home != null;
									if (flag15)
									{
										OrganizationInfo orgInfo = item3.Value.GetOrganizationInfo();
										orgInfo.OrgTemplateId = EventHelper.GetStateMainCityOrgTemplateId(CS$<>8__locals3.stateTemplateId);
										short reasonKey2;
										sbyte punishLevel2 = processor.CalcSectPunishLevelWithSpecificOrganization(item3.Key, out reasonKey2, orgInfo);
										bool flag16 = punishLevel2 >= 0;
										if (flag16)
										{
											home.AddBounty(context, item3.Value, punishLevel2, reasonKey2, -1);
										}
									}
								}
								else
								{
									short reasonKey3;
									sbyte punishLevel3 = processor.CalSectPunishLevel_WithActorIndex(item3.Key, out reasonKey3, false);
									OrganizationInfo orgInfo2 = processor.GetSectInfoSafe(charId);
									DomainManager.Organization.OnSectMemberCrimeMadePublic(context, item3.Value, orgInfo2, punishLevel3, reasonKey3);
								}
							}
						}
					}
				}
			}
			byte broadcastType = processor.GetSecretInformationActorBroadcastType();
			bool flag17 = broadcastType == 2;
			if (flag17)
			{
				DomainManager.Information.SecretInformationProcessorPool.Return(processor);
			}
			else
			{
				SecretInformationBroadcastTipsData data = new SecretInformationBroadcastTipsData();
				data.MetaDataId = metaDataId;
				data.BroadcastType = broadcastType;
				List<short> fameActionsOfMain;
				bool flag18 = !allActorFameList.TryGetValue(0, out fameActionsOfMain);
				if (flag18)
				{
					fameActionsOfMain = new List<short>();
				}
				List<short> fameActionsOfTarget;
				bool flag19 = !allActorFameList.TryGetValue(1, out fameActionsOfTarget);
				if (flag19)
				{
					fameActionsOfTarget = new List<short>();
				}
				List<short> fameActionsOfTarget2;
				bool flag20 = !allActorFameList.TryGetValue(2, out fameActionsOfTarget2);
				if (flag20)
				{
					fameActionsOfTarget2 = new List<short>();
				}
				data.FameActionsOfMain = new List<int>();
				foreach (short fameKey2 in fameActionsOfMain)
				{
					FameActionItem fameConfig = FameAction.Instance.GetItem(fameKey2);
					bool flag21 = fameConfig == null;
					if (!flag21)
					{
						data.FameActionsOfMain.Add((int)fameKey2);
						data.FameActionsOfMain.Add((int)fameConfig.Fame);
						data.FameActionsOfMain.Add((int)fameConfig.Duration);
					}
				}
				data.FameActionsOfTarget1 = new List<int>();
				foreach (short fameKey3 in fameActionsOfTarget)
				{
					FameActionItem fameConfig2 = FameAction.Instance.GetItem(fameKey3);
					bool flag22 = fameConfig2 == null;
					if (!flag22)
					{
						data.FameActionsOfTarget1.Add((int)fameKey3);
						data.FameActionsOfTarget1.Add((int)fameConfig2.Fame);
						data.FameActionsOfTarget1.Add((int)fameConfig2.Duration);
					}
				}
				data.FameActionsOfTarget2 = new List<int>();
				foreach (short fameKey4 in fameActionsOfTarget2)
				{
					FameActionItem fameConfig3 = FameAction.Instance.GetItem(fameKey4);
					bool flag23 = fameConfig3 == null;
					if (!flag23)
					{
						data.FameActionsOfTarget2.Add((int)fameKey4);
						data.FameActionsOfTarget2.Add((int)fameConfig3.Fame);
						data.FameActionsOfTarget2.Add((int)fameConfig3.Duration);
					}
				}
				int showLimit = 4;
				data.HappinessUpCharacters = (from x in happinessList
				where x.DeltaHappiness > 0
				select x.CharacterId).Take(showLimit).ToList<int>();
				data.HappinessDownCharacters = (from x in happinessList
				where x.DeltaHappiness < 0
				select x.CharacterId).Take(showLimit).ToList<int>();
				List<int> actorList = processor.GetSecretInformationArgList();
				SecretInformationBroadcastTipsData secretInformationBroadcastTipsData = data;
				GameData.Domains.Character.Character character2;
				List<int> favorToMainUpCharacters;
				if (!activeActorList.TryGetValue(0, out character2))
				{
					favorToMainUpCharacters = (from x in favorList
					where x.TargetId == actorList[0] && x.DeltaFavor > 0
					orderby x.Priority
					select x.CharacterId).Take(showLimit).ToList<int>();
				}
				else
				{
					favorToMainUpCharacters = new List<int>();
				}
				secretInformationBroadcastTipsData.FavorToMainUpCharacters = favorToMainUpCharacters;
				SecretInformationBroadcastTipsData secretInformationBroadcastTipsData2 = data;
				List<int> favorToMainDownCharacters;
				if (!activeActorList.TryGetValue(0, out character2))
				{
					favorToMainDownCharacters = (from x in favorList
					where x.TargetId == actorList[0] && x.DeltaFavor < 0
					orderby x.Priority
					select x.CharacterId).Take(showLimit).ToList<int>();
				}
				else
				{
					favorToMainDownCharacters = new List<int>();
				}
				secretInformationBroadcastTipsData2.FavorToMainDownCharacters = favorToMainDownCharacters;
				SecretInformationBroadcastTipsData secretInformationBroadcastTipsData3 = data;
				List<int> favorToTarget1UpCharacters;
				if (!activeActorList.TryGetValue(1, out character2))
				{
					favorToTarget1UpCharacters = (from x in favorList
					where x.TargetId == actorList[1] && x.DeltaFavor > 0
					orderby x.Priority
					select x.CharacterId).Take(showLimit).ToList<int>();
				}
				else
				{
					favorToTarget1UpCharacters = new List<int>();
				}
				secretInformationBroadcastTipsData3.FavorToTarget1UpCharacters = favorToTarget1UpCharacters;
				SecretInformationBroadcastTipsData secretInformationBroadcastTipsData4 = data;
				List<int> favorToTarget1DownCharacters;
				if (!activeActorList.TryGetValue(1, out character2))
				{
					favorToTarget1DownCharacters = (from x in favorList
					where x.TargetId == actorList[1] && x.DeltaFavor < 0
					orderby x.Priority
					select x.CharacterId).Take(showLimit).ToList<int>();
				}
				else
				{
					favorToTarget1DownCharacters = new List<int>();
				}
				secretInformationBroadcastTipsData4.FavorToTarget1DownCharacters = favorToTarget1DownCharacters;
				SecretInformationBroadcastTipsData secretInformationBroadcastTipsData5 = data;
				List<int> favorToTarget2UpCharacters;
				if (!activeActorList.TryGetValue(2, out character2))
				{
					favorToTarget2UpCharacters = (from x in favorList
					where x.TargetId == actorList[2] && x.DeltaFavor > 0
					orderby x.Priority
					select x.CharacterId).Take(showLimit).ToList<int>();
				}
				else
				{
					favorToTarget2UpCharacters = new List<int>();
				}
				secretInformationBroadcastTipsData5.FavorToTarget2UpCharacters = favorToTarget2UpCharacters;
				SecretInformationBroadcastTipsData secretInformationBroadcastTipsData6 = data;
				List<int> favorToTarget2DownCharacters;
				if (!activeActorList.TryGetValue(2, out character2))
				{
					favorToTarget2DownCharacters = (from x in favorList
					where x.TargetId == actorList[2] && x.DeltaFavor < 0
					orderby x.Priority
					select x.CharacterId).Take(showLimit).ToList<int>();
				}
				else
				{
					favorToTarget2DownCharacters = new List<int>();
				}
				secretInformationBroadcastTipsData6.FavorToTarget2DownCharacters = favorToTarget2DownCharacters;
				SecretInformationBroadcastTipsExtraData extraData = new SecretInformationBroadcastTipsExtraData();
				extraData.MetaDataId = metaDataId;
				extraData.StartEnemyRelationCharactersToActor = (from x in startEnemyRelationItem
				where x.TargetId == actorList[0]
				select x.CharacterId).Take(showLimit).ToList<int>();
				extraData.StartEnemyRelationCharactersToReactor = (from x in startEnemyRelationItem
				where x.TargetId == actorList[1]
				select x.CharacterId).Take(showLimit).ToList<int>();
				extraData.StartEnemyRelationCharactersToSecactor = (from x in startEnemyRelationItem
				where x.TargetId == actorList[2]
				select x.CharacterId).Take(showLimit).ToList<int>();
				bool flag24 = sourceCharId != -1;
				if (flag24)
				{
					extraData.StartEnemyRelationCharactersToSource = (from x in startEnemyRelationItem
					where x.TargetId == sourceCharId
					select x.CharacterId).Take(showLimit).ToList<int>();
					extraData.StartEnemyRelationCharactersToSource.Insert(0, sourceCharId);
				}
				DomainManager.Extra.AddSecretInformationBroadcastNotify(data, context);
				DomainManager.Extra.AddSecretInformationBroadcastNotifyExtra(extraData, context);
				DomainManager.Information.SecretInformationProcessorPool.Return(processor);
			}
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x0004EF34 File Offset: 0x0004D134
	private static void ChangeRoleHappiness(DataContext context, int characterId, int deltaValue)
	{
		GameData.Domains.Character.Character character;
		bool flag = !DomainManager.Character.TryGetElement_Objects(characterId, out character);
		if (!flag)
		{
			character.ChangeHappiness(context, deltaValue);
			bool flag2 = DomainManager.Taiwu.IsInGroup(characterId);
			if (flag2)
			{
				InstantNotificationCollection collection = DomainManager.World.GetInstantNotificationCollection();
				bool flag3 = deltaValue > 0;
				if (flag3)
				{
					collection.AddHappinessIncreased(character.GetId());
				}
				else
				{
					bool flag4 = deltaValue < 0;
					if (flag4)
					{
						collection.AddHappinessDecreased(character.GetId());
					}
				}
			}
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x0004EFB0 File Offset: 0x0004D1B0
	private static void ChangeFavorability(DataContext context, int charAId, int charBId, int deltaFavor)
	{
		bool flag = charAId == charBId;
		if (!flag)
		{
			GameData.Domains.Character.Character characterA;
			bool flag2 = !DomainManager.Character.TryGetElement_Objects(charAId, out characterA);
			if (!flag2)
			{
				GameData.Domains.Character.Character characterB;
				bool flag3 = !DomainManager.Character.TryGetElement_Objects(charBId, out characterB);
				if (!flag3)
				{
					int curDelta;
					for (int totalDelta = deltaFavor; totalDelta != 0; totalDelta -= curDelta)
					{
						curDelta = Math.Clamp(totalDelta, -30000, 30000);
						EventHelper.ChangeFavorabilityOptional(characterA, characterB, (short)curDelta, 3);
					}
				}
			}
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0004F02C File Offset: 0x0004D22C
	[CompilerGenerated]
	internal static Sect <OpenEffectEntrance>g__GetBountyHome|0_30(ref BroadcastEffect.<>c__DisplayClass0_1 A_0)
	{
		Sect sect;
		bool flag = !DomainManager.Organization.TryGetElement_Sects(A_0.orgInfo.SettlementId, out sect);
		if (flag)
		{
			bool flag2 = A_0.orgInfo.SettlementId < 0;
			if (flag2)
			{
				return null;
			}
			Location settlementLocation = DomainManager.Organization.GetSettlement(A_0.orgInfo.SettlementId).GetLocation();
			bool flag3 = !settlementLocation.IsValid();
			if (flag3)
			{
				return null;
			}
			sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(settlementLocation.AreaId);
			MapStateItem stateCfg = MapState.Instance[stateTemplateId];
			bool flag4 = stateCfg.SectID < 0;
			if (flag4)
			{
				return null;
			}
			sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(stateCfg.SectID);
		}
		return sect;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x0004F0FC File Offset: 0x0004D2FC
	[CompilerGenerated]
	internal static Sect <OpenEffectEntrance>g__GetBountyHome|0_31(ref BroadcastEffect.<>c__DisplayClass0_2 A_0)
	{
		MapStateItem stateCfg = MapState.Instance[A_0.stateTemplateId];
		bool flag = stateCfg.SectID < 0;
		Sect result;
		if (flag)
		{
			result = null;
		}
		else
		{
			Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(stateCfg.SectID);
			result = sect;
		}
		return result;
	}
}
