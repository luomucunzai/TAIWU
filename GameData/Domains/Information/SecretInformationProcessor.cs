using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Information
{
	// Token: 0x02000683 RID: 1667
	public class SecretInformationProcessor
	{
		// Token: 0x0600540C RID: 21516 RVA: 0x002DCD44 File Offset: 0x002DAF44
		public bool Initialize(int metaDataId)
		{
			this.Reset();
			SecretInformationProcessor._metaDataId = metaDataId;
			bool flag = SecretInformationProcessor._metaDataId == -1;
			bool result;
			if (flag)
			{
				AdaptableLog.Info("Invalid metaDataId!");
				result = false;
			}
			else
			{
				bool flag2 = DomainManager.Information.TryGetElement_SecretInformationMetaData(SecretInformationProcessor._metaDataId, out SecretInformationProcessor._metaDataRef);
				if (flag2)
				{
					DomainManager.Information.MakeSecretInformationEventArgBox(SecretInformationProcessor._metaDataRef, SecretInformationProcessor._secretInfoArgBox);
					bool flag3 = SecretInformationProcessor._secretInfoArgBox == null;
					if (flag3)
					{
						AdaptableLog.Info("Invalid secretInfoArgBox!");
						result = false;
					}
					else
					{
						int actorId = -1;
						int reactorId = -1;
						int secactorId = -1;
						int itemType = -1;
						SecretInformationProcessor._secretInfoArgBox.Get("templateId", ref SecretInformationProcessor._templateId);
						bool flag4 = SecretInformationProcessor._templateId == -1;
						if (flag4)
						{
							result = false;
						}
						else
						{
							SecretInformationProcessor._infoConfig = SecretInformation.Instance.GetItem(SecretInformationProcessor._templateId);
							bool flag5 = SecretInformationProcessor._infoConfig == null;
							if (flag5)
							{
								result = false;
							}
							else
							{
								SecretInformationProcessor._effectConfig = SecretInformationEffect.Instance.GetItem(SecretInformationProcessor._infoConfig.DefaultEffectId);
								bool flag6 = SecretInformationProcessor._effectConfig == null;
								if (flag6)
								{
									result = false;
								}
								else
								{
									SecretInformationProcessor._sectPunishConfig = SecretInformationSectPunish.Instance.GetItem(SecretInformationProcessor._templateId);
									bool flag7 = SecretInformationProcessor._sectPunishConfig == null;
									if (flag7)
									{
										result = false;
									}
									else
									{
										bool flag8 = SecretInformationProcessor._effectConfig.ActorIndex != -1;
										if (flag8)
										{
											EventArgBox secretInfoArgBox = SecretInformationProcessor._secretInfoArgBox;
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
											defaultInterpolatedStringHandler.AppendLiteral("arg");
											defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._effectConfig.ActorIndex);
											secretInfoArgBox.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref actorId);
										}
										bool flag9 = SecretInformationProcessor._effectConfig.ReactorIndex != -1;
										if (flag9)
										{
											EventArgBox secretInfoArgBox2 = SecretInformationProcessor._secretInfoArgBox;
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
											defaultInterpolatedStringHandler.AppendLiteral("arg");
											defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._effectConfig.ReactorIndex);
											secretInfoArgBox2.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref reactorId);
										}
										bool flag10 = SecretInformationProcessor._effectConfig.SecactorIndex != -1;
										if (flag10)
										{
											EventArgBox secretInfoArgBox3 = SecretInformationProcessor._secretInfoArgBox;
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
											defaultInterpolatedStringHandler.AppendLiteral("arg");
											defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._effectConfig.SecactorIndex);
											secretInfoArgBox3.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref secactorId);
										}
										bool flag11 = SecretInformationProcessor._effectConfig.Item != -1;
										if (flag11)
										{
											EventArgBox secretInfoArgBox4 = SecretInformationProcessor._secretInfoArgBox;
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
											defaultInterpolatedStringHandler.AppendLiteral("arg");
											defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._effectConfig.Item);
											ItemKey item;
											secretInfoArgBox4.Get<ItemKey>(defaultInterpolatedStringHandler.ToStringAndClear(), out item);
											itemType = (int)item.ItemType;
										}
										SecretInformationProcessor.ArgList.Add(actorId);
										SecretInformationProcessor.ArgList.Add(reactorId);
										SecretInformationProcessor.ArgList.Add(secactorId);
										for (int i = 0; i < SecretInformationProcessor.ArgList.Count; i++)
										{
											int id = SecretInformationProcessor.ArgList[i];
											bool flag12 = id != -1;
											if (flag12)
											{
												SecretInformationProcessor.ActorIdList.Add(id);
												GameData.Domains.Character.Character character;
												bool flag13 = InformationDomain.CheckTuringTest(id, out character);
												if (flag13)
												{
													SecretInformationProcessor.ActiveActorList.Add(i, character);
												}
											}
										}
										SecretInformationProcessor.ArgList.Add(-1);
										SecretInformationProcessor.ArgList.Add(itemType);
										SecretInformationProcessor.ArgList.Add(DomainManager.Taiwu.GetTaiwuCharId());
										result = true;
									}
								}
							}
						}
					}
				}
				else
				{
					AdaptableLog.Info("Invalid secretInfoMetaData!");
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600540D RID: 21517 RVA: 0x002DD0B8 File Offset: 0x002DB2B8
		public void Reset()
		{
			SecretInformationProcessor.ArgList.Clear();
			SecretInformationProcessor.ActorIdList.Clear();
			SecretInformationProcessor.ActiveActorList.Clear();
			SecretInformationProcessor._metaDataId = -1;
			SecretInformationProcessor._templateId = -1;
			SecretInformationProcessor._metaDataRef = null;
			SecretInformationProcessor._secretInfoArgBox.Clear();
			SecretInformationProcessor._infoConfig = null;
			SecretInformationProcessor._effectConfig = null;
			SecretInformationProcessor._sectPunishConfig = null;
			SecretInformationProcessor._baseFavorOdds = -1;
			SecretInformationProcessor.RelatedCharacterIndexList = null;
		}

		// Token: 0x0600540E RID: 21518 RVA: 0x002DD122 File Offset: 0x002DB322
		public void Initialize_ForBroadcastEffect(IRandomSource random)
		{
			this.CalBaseFavorabilityConditionOdds();
			this.CalALLActiveRelatedCharactorRelationIndex(random);
		}

		// Token: 0x0600540F RID: 21519 RVA: 0x002DD134 File Offset: 0x002DB334
		public short GetSecretInformationTemplateId()
		{
			return SecretInformationProcessor._templateId;
		}

		// Token: 0x06005410 RID: 21520 RVA: 0x002DD14C File Offset: 0x002DB34C
		public SecretInformationEffectItem GetSecretInformationEffectConfig()
		{
			return SecretInformationProcessor._effectConfig;
		}

		// Token: 0x06005411 RID: 21521 RVA: 0x002DD164 File Offset: 0x002DB364
		public List<int> GetSecretInformationArgList()
		{
			return SecretInformationProcessor.ArgList;
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x002DD17C File Offset: 0x002DB37C
		public bool IsCharacterSecretInformationActor(int charId, bool includeDead = true)
		{
			IEnumerable<int> enumerable;
			if (!includeDead)
			{
				IEnumerable<int> actorIdList = SecretInformationProcessor.ActorIdList;
				enumerable = actorIdList;
			}
			else
			{
				enumerable = from x in SecretInformationProcessor.ActiveActorList.Values
				select x.GetId();
			}
			IEnumerable<int> list = enumerable;
			return list.Contains(charId);
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x002DD1D0 File Offset: 0x002DB3D0
		public List<int> GetActiveActorIdList(IEnumerable<int> excludeId = null)
		{
			HashSet<int> result = new HashSet<int>(from x in SecretInformationProcessor.ActiveActorList.Values
			select x.GetId());
			bool flag = excludeId != null;
			if (flag)
			{
				result.ExceptWith(excludeId);
			}
			return result.ToList<int>();
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x002DD22C File Offset: 0x002DB42C
		public List<int> GetActorIdList(IEnumerable<int> excludeId = null)
		{
			HashSet<int> result = new HashSet<int>(SecretInformationProcessor.ActorIdList);
			bool flag = excludeId != null;
			if (flag)
			{
				result.ExceptWith(excludeId);
			}
			return result.ToList<int>();
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x002DD260 File Offset: 0x002DB460
		public int GetCharacterActorIndex(int charId)
		{
			int result = SecretInformationProcessor.ArgList.FindIndex((int x) => x == charId);
			bool flag = result > 2;
			if (flag)
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x002DD2A4 File Offset: 0x002DB4A4
		public bool CheckIfActorExist_WithActorIndex(int charIndex)
		{
			bool flag = charIndex < 0 || charIndex > 2;
			GameData.Domains.Character.Character character;
			return !flag && DomainManager.Character.TryGetElement_Objects(SecretInformationProcessor.ArgList[charIndex], out character);
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x002DD2E0 File Offset: 0x002DB4E0
		public Dictionary<int, GameData.Domains.Character.Character> GetActiveActorList_WithActorIndex()
		{
			return SecretInformationProcessor.ActiveActorList;
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x002DD2F8 File Offset: 0x002DB4F8
		public Dictionary<int, GameData.Domains.Character.Character> GetActiveActorList_WithRelationIndex()
		{
			List<int> relationIndex = new List<int>
			{
				1,
				4,
				7
			};
			Dictionary<int, GameData.Domains.Character.Character> result = new Dictionary<int, GameData.Domains.Character.Character>();
			foreach (KeyValuePair<int, GameData.Domains.Character.Character> item in SecretInformationProcessor.ActiveActorList)
			{
				result.Add(relationIndex[item.Key], item.Value);
			}
			return result;
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x002DD390 File Offset: 0x002DB590
		public byte GetSecretInformationActorBroadcastType()
		{
			byte result = 2;
			bool flag = this.IsTaiwuSecretInformationActor();
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = this.IsTaiwuSecretInformationDissemination();
				if (flag2)
				{
					result = 1;
				}
			}
			return result;
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x002DD3C0 File Offset: 0x002DB5C0
		public bool IsTaiwuSecretInformationActor()
		{
			return SecretInformationProcessor.ActorIdList.Contains(SecretInformationProcessor.ArgList[5]);
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x002DD3E8 File Offset: 0x002DB5E8
		private bool IsTaiwuSecretInformationDissemination()
		{
			return this.GetSecretInformationDisseminationBranchCharacterIds().Contains(SecretInformationProcessor.ArgList[5]);
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x002DD410 File Offset: 0x002DB610
		public sbyte GetFameTypeSafe(int charId)
		{
			int index = SecretInformationProcessor.ArgList.FindIndex((int x) => x == charId);
			sbyte fameType = -1;
			bool flag = SecretInformationProcessor._infoConfig.ExtraSnapshotParameterIndices == null || !SecretInformationProcessor._infoConfig.ExtraSnapshotParameterIndices.Contains(index);
			sbyte result;
			if (flag)
			{
				result = fameType;
			}
			else
			{
				SecretInformationCharacterExtraInfo extraInfo;
				bool flag2 = SecretInformationProcessor._metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out extraInfo);
				if (flag2)
				{
					fameType = extraInfo.FameType;
				}
				bool flag3 = fameType == -2;
				if (flag3)
				{
					fameType = 3;
				}
				result = fameType;
			}
			return result;
		}

		// Token: 0x0600541D RID: 21533 RVA: 0x002DD4B0 File Offset: 0x002DB6B0
		public OrganizationInfo GetSectInfoSafe(int charId)
		{
			int index = SecretInformationProcessor.ArgList.FindIndex((int x) => x == charId);
			OrganizationInfo orgInfo = new OrganizationInfo(-1, 0, true, -1);
			bool flag = SecretInformationProcessor._infoConfig.ExtraSnapshotParameterIndices == null || !SecretInformationProcessor._infoConfig.ExtraSnapshotParameterIndices.Contains(index);
			OrganizationInfo result;
			if (flag)
			{
				result = orgInfo;
			}
			else
			{
				SecretInformationCharacterExtraInfo extraInfo;
				bool flag2 = SecretInformationProcessor._metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out extraInfo);
				if (flag2)
				{
					orgInfo = extraInfo.OrgInfo;
				}
				result = orgInfo;
			}
			return result;
		}

		// Token: 0x0600541E RID: 21534 RVA: 0x002DD54C File Offset: 0x002DB74C
		public byte GetMonkTypeSafe(int charId)
		{
			byte monkType = 0;
			int index = SecretInformationProcessor.ArgList.FindIndex((int x) => x == charId);
			bool flag = SecretInformationProcessor._infoConfig.ExtraSnapshotParameterIndices == null || !SecretInformationProcessor._infoConfig.ExtraSnapshotParameterIndices.Contains(index);
			byte result;
			if (flag)
			{
				result = monkType;
			}
			else
			{
				SecretInformationCharacterExtraInfo extraInfo;
				bool flag2 = SecretInformationProcessor._metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out extraInfo);
				if (flag2)
				{
					monkType = extraInfo.MonkType;
				}
				result = monkType;
			}
			return result;
		}

		// Token: 0x0600541F RID: 21535 RVA: 0x002DD5E0 File Offset: 0x002DB7E0
		public HashSet<int> GetSecretInformationRelatedCharactersOfSpecialRelation(int characterId, IEnumerable<SecretInformationRelationshipType> relations, bool includeAlive = true, bool includeDead = true, bool includeActors = false)
		{
			HashSet<int> result = new HashSet<int>();
			SecretInformationRelationshipType[] relationArray = (relations as SecretInformationRelationshipType[]) ?? relations.ToArray<SecretInformationRelationshipType>();
			bool flag = !relationArray.Any<SecretInformationRelationshipType>();
			HashSet<int> result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				SecretInformationCharacterRelationshipSnapshot relationshipSnapshot;
				bool flag2 = SecretInformationProcessor._metaDataRef.CharacterRelationshipSnapshotCollection.Collection.TryGetValue(characterId, out relationshipSnapshot);
				if (flag2)
				{
					SecretInformationRelationshipType[] array = relationArray;
					for (int i = 0; i < array.Length; i++)
					{
						switch (array[i])
						{
						case SecretInformationRelationshipType.Relative:
							result.UnionWith(relationshipSnapshot.RelatedCharacters.BloodParents.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.BloodChildren.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.BloodBrothersAndSisters.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.StepParents.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.StepChildren.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.StepBrothersAndSisters.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.AdoptiveParents.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.AdoptiveChildren.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.AdoptiveBrothersAndSisters.GetCollection());
							break;
						case SecretInformationRelationshipType.Friend:
							result.UnionWith(relationshipSnapshot.RelatedCharacters.Friends.GetCollection());
							break;
						case SecretInformationRelationshipType.SwornBrotherOrSister:
							result.UnionWith(relationshipSnapshot.RelatedCharacters.SwornBrothersAndSisters.GetCollection());
							break;
						case SecretInformationRelationshipType.HusbandOrWife:
							result.UnionWith(relationshipSnapshot.RelatedCharacters.HusbandsAndWives.GetCollection());
							break;
						case SecretInformationRelationshipType.Lover:
						{
							HashSet<int> temptResult = new HashSet<int>();
							foreach (int charId2 in relationshipSnapshot.RelatedCharacters.Adored.GetCollection())
							{
								SecretInformationCharacterRelationshipSnapshot targetRelationshipSnapshot;
								bool flag3 = SecretInformationProcessor._metaDataRef.CharacterRelationshipSnapshotCollection.Collection.TryGetValue(characterId, out targetRelationshipSnapshot);
								if (flag3)
								{
									bool flag4 = targetRelationshipSnapshot.RelatedCharacters.Adored.Contains(characterId);
									if (flag4)
									{
										temptResult.Add(charId2);
									}
								}
							}
							result.UnionWith(temptResult);
							break;
						}
						case SecretInformationRelationshipType.Adorer:
							result.UnionWith(relationshipSnapshot.RelatedCharacters.Adored.GetCollection());
							break;
						case SecretInformationRelationshipType.MentorAndMentee:
							result.UnionWith(relationshipSnapshot.RelatedCharacters.Mentors.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.Mentees.GetCollection());
							break;
						case SecretInformationRelationshipType.Allied:
							result.UnionWith(relationshipSnapshot.RelatedCharacters.BloodParents.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.BloodChildren.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.BloodBrothersAndSisters.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.StepParents.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.StepChildren.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.StepBrothersAndSisters.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.AdoptiveParents.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.AdoptiveChildren.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.AdoptiveBrothersAndSisters.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.SwornBrothersAndSisters.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.HusbandsAndWives.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.Adored.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.Mentors.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.Mentees.GetCollection());
							result.UnionWith(relationshipSnapshot.RelatedCharacters.Friends.GetCollection());
							break;
						case SecretInformationRelationshipType.Enemy:
							result.UnionWith(relationshipSnapshot.RelatedCharacters.Enemies.GetCollection());
							break;
						}
					}
					bool flag5 = !includeAlive;
					if (flag5)
					{
						result.RemoveWhere(delegate(int charId)
						{
							SecretInformationCharacterExtraInfo extraInfo;
							return SecretInformationProcessor._metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out extraInfo) && extraInfo.AliveState == 0;
						});
					}
					bool flag6 = !includeDead;
					if (flag6)
					{
						result.RemoveWhere(delegate(int charId)
						{
							SecretInformationCharacterExtraInfo extraInfo;
							return SecretInformationProcessor._metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out extraInfo) && extraInfo.AliveState != 0;
						});
					}
					bool flag7 = !includeActors;
					if (flag7)
					{
						result.ExceptWith(SecretInformationProcessor.ActorIdList);
					}
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06005420 RID: 21536 RVA: 0x002DDAB0 File Offset: 0x002DBCB0
		public HashSet<int> GetSecretInformationRelatedCharactersOfSpecialRelation_ForNoneActor(int characterId, IEnumerable<SecretInformationRelationshipType> relations, bool includeAlive = true, bool includeDead = true)
		{
			SecretInformationRelationshipType[] relationArray = (relations as SecretInformationRelationshipType[]) ?? relations.ToArray<SecretInformationRelationshipType>();
			SecretInformationRelationshipType[] single = relationArray.Intersect(SecretInformationProcessor.RelationIndex.Single).ToArray<SecretInformationRelationshipType>();
			SecretInformationRelationshipType[] twoway = relationArray.Except(SecretInformationProcessor.RelationIndex.Single).ToArray<SecretInformationRelationshipType>();
			HashSet<int> result = new HashSet<int>();
			foreach (int actorId in SecretInformationProcessor.ActorIdList)
			{
				bool flag = this.GetSecretInformationRelatedCharactersOfSpecialRelation(actorId, twoway, true, true, false).Contains(characterId) || this.GetSecretInformationRelatedCharactersOfSpecialRelation(characterId, single, true, true, false).Contains(actorId);
				if (flag)
				{
					result.Add(actorId);
				}
			}
			bool flag2 = !includeAlive;
			if (flag2)
			{
				result.RemoveWhere(delegate(int charId)
				{
					SecretInformationCharacterExtraInfo extraInfo;
					return SecretInformationProcessor._metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out extraInfo) && extraInfo.AliveState == 0;
				});
			}
			bool flag3 = !includeDead;
			if (flag3)
			{
				result.RemoveWhere(delegate(int charId)
				{
					SecretInformationCharacterExtraInfo extraInfo;
					return SecretInformationProcessor._metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out extraInfo) && extraInfo.AliveState != 0;
				});
			}
			return result;
		}

		// Token: 0x06005421 RID: 21537 RVA: 0x002DDBDC File Offset: 0x002DBDDC
		public bool Check_HasSpecialRelations(int charId, int targetId, IEnumerable<SecretInformationRelationshipType> relations)
		{
			SecretInformationRelationshipType[] relationArray = (relations as SecretInformationRelationshipType[]) ?? relations.ToArray<SecretInformationRelationshipType>();
			IEnumerable<SecretInformationRelationshipType> single = relationArray.Intersect(SecretInformationProcessor.RelationIndex.Single);
			SecretInformationRelationshipType[] twoway = relationArray.Except(SecretInformationProcessor.RelationIndex.Single).ToArray<SecretInformationRelationshipType>();
			return this.GetSecretInformationRelatedCharactersOfSpecialRelation(charId, single, true, true, true).Contains(targetId) || this.GetSecretInformationRelatedCharactersOfSpecialRelation(charId, twoway, true, true, true).Contains(targetId) || this.GetSecretInformationRelatedCharactersOfSpecialRelation(targetId, twoway, true, true, true).Contains(charId);
		}

		// Token: 0x06005422 RID: 21538 RVA: 0x002DDC58 File Offset: 0x002DBE58
		public bool Check_IsSectLeaderOfCharacter(int charId, int targetId)
		{
			OrganizationInfo oriActorOrgInfo = this.GetSectInfoSafe(targetId);
			bool flag = oriActorOrgInfo.OrgTemplateId < 1 || oriActorOrgInfo.OrgTemplateId > 15;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character character;
				bool flag2 = !InformationDomain.CheckTuringTest(charId, out character);
				if (flag2)
				{
					result = false;
				}
				else
				{
					OrganizationInfo curActorOrgInfo = character.GetOrganizationInfo();
					bool flag3 = curActorOrgInfo.OrgTemplateId < 0;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = curActorOrgInfo.OrgTemplateId != oriActorOrgInfo.OrgTemplateId;
						result = (!flag4 && curActorOrgInfo.Grade == 8);
					}
				}
			}
			return result;
		}

		// Token: 0x06005423 RID: 21539 RVA: 0x002DDCEC File Offset: 0x002DBEEC
		public List<int> GetAllSecretInformationRelationsOfCharacter(int charId, bool includeLoveRelation, bool includeLeadership)
		{
			List<int> result = new List<int>();
			bool flag = charId == -1;
			List<int> result2;
			if (flag)
			{
				result2 = new List<int>
				{
					0
				};
			}
			else
			{
				int actorId = SecretInformationProcessor.ArgList[0];
				int reactorId = SecretInformationProcessor.ArgList[1];
				int secactorId = SecretInformationProcessor.ArgList[2];
				bool flag2 = actorId != -1;
				if (flag2)
				{
					bool flag3 = actorId == charId;
					if (flag3)
					{
						result.Add(1);
					}
					bool flag4 = this.Check_HasSpecialRelations(charId, actorId, SecretInformationProcessor.RelationIndex.Allied);
					if (flag4)
					{
						result.Add(2);
					}
					bool flag5 = this.Check_HasSpecialRelations(charId, actorId, SecretInformationProcessor.RelationIndex.Enemy);
					if (flag5)
					{
						result.Add(3);
					}
					bool flag6 = includeLoveRelation && !SecretInformationProcessor.ActorIdList.Contains(charId);
					if (flag6)
					{
						bool flag7 = this.Check_HasSpecialRelations(charId, actorId, SecretInformationProcessor.RelationIndex.Love);
						if (flag7)
						{
							result.Add(10);
						}
						else
						{
							bool flag8 = this.Check_HasSpecialRelations(charId, actorId, SecretInformationProcessor.RelationIndex.Adorer);
							if (flag8)
							{
								result.Add(13);
							}
						}
					}
					bool flag9 = includeLeadership && this.Check_IsSectLeaderOfCharacter(charId, actorId);
					if (flag9)
					{
						result.Add(16);
					}
				}
				bool flag10 = reactorId != -1;
				if (flag10)
				{
					bool flag11 = reactorId == charId;
					if (flag11)
					{
						result.Add(4);
					}
					bool flag12 = this.Check_HasSpecialRelations(charId, reactorId, SecretInformationProcessor.RelationIndex.Allied);
					if (flag12)
					{
						result.Add(5);
					}
					bool flag13 = this.Check_HasSpecialRelations(charId, reactorId, SecretInformationProcessor.RelationIndex.Enemy);
					if (flag13)
					{
						result.Add(6);
					}
					bool flag14 = includeLoveRelation && !SecretInformationProcessor.ActorIdList.Contains(charId);
					if (flag14)
					{
						bool flag15 = this.Check_HasSpecialRelations(charId, reactorId, SecretInformationProcessor.RelationIndex.Love);
						if (flag15)
						{
							result.Add(11);
						}
						else
						{
							bool flag16 = this.Check_HasSpecialRelations(charId, reactorId, SecretInformationProcessor.RelationIndex.Adorer);
							if (flag16)
							{
								result.Add(14);
							}
						}
					}
					bool flag17 = includeLeadership && this.Check_IsSectLeaderOfCharacter(charId, reactorId);
					if (flag17)
					{
						result.Add(17);
					}
				}
				bool flag18 = secactorId != -1;
				if (flag18)
				{
					bool flag19 = secactorId == charId;
					if (flag19)
					{
						result.Add(7);
					}
					bool flag20 = this.Check_HasSpecialRelations(charId, secactorId, SecretInformationProcessor.RelationIndex.Allied);
					if (flag20)
					{
						result.Add(8);
					}
					bool flag21 = this.Check_HasSpecialRelations(charId, secactorId, SecretInformationProcessor.RelationIndex.Enemy);
					if (flag21)
					{
						result.Add(9);
					}
					bool flag22 = includeLoveRelation && !SecretInformationProcessor.ActorIdList.Contains(charId);
					if (flag22)
					{
						bool flag23 = this.Check_HasSpecialRelations(charId, secactorId, SecretInformationProcessor.RelationIndex.Love);
						if (flag23)
						{
							result.Add(12);
						}
						else
						{
							bool flag24 = this.Check_HasSpecialRelations(charId, secactorId, SecretInformationProcessor.RelationIndex.Adorer);
							if (flag24)
							{
								result.Add(15);
							}
						}
					}
					bool flag25 = includeLeadership && this.Check_IsSectLeaderOfCharacter(charId, secactorId);
					if (flag25)
					{
						result.Add(18);
					}
				}
				bool flag26 = result.Count == 0;
				if (flag26)
				{
					result.Add(0);
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x002DDFC8 File Offset: 0x002DC1C8
		public HashSet<int> GetAllSecretInformationRelatedCharacters(bool includeGeneral = true, bool includeAlive = true, bool includeDead = true, bool includeActors = false)
		{
			HashSet<int> result = new HashSet<int>();
			foreach (KeyValuePair<int, SecretInformationCharacterRelationshipSnapshot> item in SecretInformationProcessor._metaDataRef.CharacterRelationshipSnapshotCollection.Collection)
			{
				SecretInformationCharacterRelationshipSnapshot relationshipSnapshot = item.Value;
				relationshipSnapshot.RelatedCharacters.GetAllRelatedCharIds(result, includeGeneral);
			}
			bool flag = !includeAlive;
			if (flag)
			{
				result.RemoveWhere(delegate(int charId)
				{
					SecretInformationCharacterExtraInfo extraInfo;
					return SecretInformationProcessor._metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out extraInfo) && extraInfo.AliveState == 0;
				});
			}
			bool flag2 = !includeDead;
			if (flag2)
			{
				result.RemoveWhere(delegate(int charId)
				{
					SecretInformationCharacterExtraInfo extraInfo;
					return SecretInformationProcessor._metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out extraInfo) && extraInfo.AliveState != 0;
				});
			}
			bool flag3 = !includeActors;
			if (flag3)
			{
				result.ExceptWith(SecretInformationProcessor.ActorIdList);
			}
			return result;
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x002DE0B8 File Offset: 0x002DC2B8
		public List<GameData.Domains.Character.Character> GetAllSecretInformationRelatedCharacters_WithTuringTest(bool includeActors = false)
		{
			bool flag = SecretInformationProcessor.RelatedCharacterIndexList != null;
			List<GameData.Domains.Character.Character> result;
			if (flag)
			{
				Dictionary<GameData.Domains.Character.Character, int>.KeyCollection related = SecretInformationProcessor.RelatedCharacterIndexList.Keys;
				result = (includeActors ? related.ToList<GameData.Domains.Character.Character>() : related.Except(SecretInformationProcessor.ActiveActorList.Values).ToList<GameData.Domains.Character.Character>());
			}
			else
			{
				result = InformationDomain.GetTuringTestPassedCharacters(this.GetAllSecretInformationRelatedCharacters(true, true, true, includeActors));
			}
			return result;
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x002DE114 File Offset: 0x002DC314
		public Dictionary<GameData.Domains.Character.Character, int> GetAllSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex(bool includeActors = false)
		{
			bool flag = SecretInformationProcessor.RelatedCharacterIndexList == null;
			Dictionary<GameData.Domains.Character.Character, int> result2;
			if (flag)
			{
				result2 = new Dictionary<GameData.Domains.Character.Character, int>();
			}
			else
			{
				Dictionary<GameData.Domains.Character.Character, int> result = new Dictionary<GameData.Domains.Character.Character, int>(SecretInformationProcessor.RelatedCharacterIndexList);
				bool flag2 = !includeActors;
				if (flag2)
				{
					foreach (GameData.Domains.Character.Character character in SecretInformationProcessor.ActiveActorList.Values)
					{
						bool flag3 = result.ContainsKey(character);
						if (flag3)
						{
							result.Remove(character);
						}
					}
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x002DE1B0 File Offset: 0x002DC3B0
		public HashSet<int> GetSecretInformationDisseminationBranchCharacterIds()
		{
			return SecretInformationProcessor._metaDataRef.GetDisseminationBranchCharacterIds().ToHashSet<int>();
		}

		// Token: 0x06005428 RID: 21544 RVA: 0x002DE1C1 File Offset: 0x002DC3C1
		public List<GameData.Domains.Character.Character> GetSecretInformationDisseminationBranchCharacterIds_WithTuringTest()
		{
			return InformationDomain.GetTuringTestPassedCharacters(SecretInformationProcessor._metaDataRef.GetDisseminationBranchCharacterIds());
		}

		// Token: 0x06005429 RID: 21545 RVA: 0x002DE1D4 File Offset: 0x002DC3D4
		private int GetRandonRelationIndex(IRandomSource random, int charId, bool includeLoveRelation = false, bool includeLeadership = false)
		{
			List<int> relation = this.GetAllSecretInformationRelationsOfCharacter(charId, includeLoveRelation, includeLeadership);
			return relation.ElementAt(random.Next(0, relation.Count));
		}

		// Token: 0x0600542A RID: 21546 RVA: 0x002DE204 File Offset: 0x002DC404
		private void CalALLActiveRelatedCharactorRelationIndex(IRandomSource random)
		{
			SecretInformationProcessor.RelatedCharacterIndexList = this.GetAllSecretInformationRelatedCharacters_WithTuringTest(true).ToDictionary((GameData.Domains.Character.Character x) => x, (GameData.Domains.Character.Character x) => this.GetRandonRelationIndex(random, x.GetId(), false, false));
		}

		// Token: 0x0600542B RID: 21547 RVA: 0x002DE264 File Offset: 0x002DC464
		private int CalBaseFavorabilityConditionOdds()
		{
			int result = 0;
			SecretInformationProcessor._baseFavorOdds = 0;
			List<Config.ShortList> conditionItemList = SecretInformationProcessor._effectConfig.SpecialConditionFavorabilities;
			bool flag = conditionItemList.Count == 0;
			int result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				for (int i = 0; i < conditionItemList.Count; i++)
				{
					List<short> conditionItem = conditionItemList[i].DataList;
					bool flag2 = conditionItem.Count != 2;
					if (!flag2)
					{
						bool flag3 = SecretInformationProcessor._effectConfig.SpecialConditionIndices.Count <= i;
						if (flag3)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(132, 1);
							defaultInterpolatedStringHandler.AppendLiteral("TemplateId:");
							defaultInterpolatedStringHandler.AppendFormatted<short>(SecretInformationProcessor._templateId);
							defaultInterpolatedStringHandler.AppendLiteral(" Error in EffectConfig.SpecialConditionIndices ! Must None Less Than Count of EffectConfig.SpecialConditionFavorabilities");
							AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
							SecretInformationProcessor._baseFavorOdds = result;
							return result;
						}
						List<short> indicesItem = SecretInformationProcessor._effectConfig.SpecialConditionIndices[i].DataList;
						bool flag4 = indicesItem.Contains(3);
						if (!flag4)
						{
							bool flag5 = this.ConditionBox_FavorabilityConditionOddsEntrance(conditionItem[0], indicesItem, -1);
							if (flag5)
							{
								result += (int)conditionItem[1];
							}
						}
					}
				}
				SecretInformationProcessor._baseFavorOdds = result;
				result2 = result;
			}
			return result2;
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x002DE3A0 File Offset: 0x002DC5A0
		private int CalPersonalFavorabilityConditionOdds(int charId)
		{
			int result = 0;
			List<Config.ShortList> conditionItemList = SecretInformationProcessor._effectConfig.SpecialConditionFavorabilities;
			bool flag = conditionItemList.Count == 0;
			int result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				for (int i = 0; i < conditionItemList.Count; i++)
				{
					List<short> conditionItem = conditionItemList[i].DataList;
					bool flag2 = conditionItem.Count != 2;
					if (!flag2)
					{
						bool flag3 = SecretInformationProcessor._effectConfig.SpecialConditionIndices.Count <= i;
						if (flag3)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(132, 1);
							defaultInterpolatedStringHandler.AppendLiteral("TemplateId:");
							defaultInterpolatedStringHandler.AppendFormatted<short>(SecretInformationProcessor._templateId);
							defaultInterpolatedStringHandler.AppendLiteral(" Error in EffectConfig.SpecialConditionIndices ! Must None Less Than Count of EffectConfig.SpecialConditionFavorabilities");
							AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
							return result;
						}
						List<short> indicesItem = SecretInformationProcessor._effectConfig.SpecialConditionIndices[i].DataList;
						bool flag4 = !indicesItem.Contains(3);
						if (!flag4)
						{
							bool flag5 = this.ConditionBox_FavorabilityConditionOddsEntrance(conditionItem[0], indicesItem, charId);
							if (flag5)
							{
								result += (int)conditionItem[1];
							}
						}
					}
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x0600542D RID: 21549 RVA: 0x002DE4D0 File Offset: 0x002DC6D0
		private bool ConditionBox_FavorabilityConditionOddsEntrance(short conditionKey, List<short> argIndexList, int extraId = -1)
		{
			bool flag = conditionKey == 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				List<int> curList = new List<int>(SecretInformationProcessor.ArgList);
				bool flag2 = curList.Count > 3;
				if (flag2)
				{
					curList[3] = extraId;
				}
				bool flag3 = argIndexList != null;
				if (flag3)
				{
					argIndexList = (from t in argIndexList
					select (short)Math.Clamp((t < 0 || (int)t >= curList.Count<int>()) ? ((int)t) : curList[(int)t], -32768, 32767)).ToList<short>();
				}
				else
				{
					argIndexList = new List<short>();
				}
				result = this.ConditionBox((short)((sbyte)conditionKey), (int)((argIndexList.Count > 0) ? argIndexList[0] : -1), (int)((argIndexList.Count > 1) ? argIndexList[1] : -1), (int)((argIndexList.Count > 2) ? argIndexList[2] : -1), (int)((argIndexList.Count > 3) ? argIndexList[3] : -1));
			}
			return result;
		}

		// Token: 0x0600542E RID: 21550 RVA: 0x002DE5A8 File Offset: 0x002DC7A8
		private List<int> CalFinalDeltaFavorabilityOfRelatedCharacters_WithRelationIndex(GameData.Domains.Character.Character character, int relationIndex)
		{
			List<int> result = new List<int>
			{
				0,
				0,
				0,
				0
			};
			bool flag = SecretInformationProcessor.ActorIdList.Contains(character.GetId());
			List<int> result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				sbyte behaviorType = character.GetBehaviorType();
				int conditionOdds = SecretInformationProcessor._baseFavorOdds + this.CalPersonalFavorabilityConditionOdds(character.GetId());
				bool isSpecial = conditionOdds != 0;
				switch (relationIndex)
				{
				case 0:
				case 16:
				case 17:
				case 18:
				{
					IReadOnlyList<Config.ShortList> favorabilityDiffs = isSpecial ? SecretInformationProcessor._effectConfig.OtherFavorabilityDiffsWhenSpecial : SecretInformationProcessor._effectConfig.OtherFavorabilityDiffs;
					goto IL_19A;
				}
				case 2:
				case 10:
				case 13:
				{
					IReadOnlyList<Config.ShortList> favorabilityDiffs = isSpecial ? SecretInformationProcessor._effectConfig.ActorFriendFavorabilityDiffsWhenSpecial : SecretInformationProcessor._effectConfig.ActorFriendFavorabilityDiffs;
					goto IL_19A;
				}
				case 3:
				{
					IReadOnlyList<Config.ShortList> favorabilityDiffs = isSpecial ? SecretInformationProcessor._effectConfig.ActorEnemyFavorabilityDiffsWhenSpecial : SecretInformationProcessor._effectConfig.ActorEnemyFavorabilityDiffs;
					goto IL_19A;
				}
				case 5:
				case 11:
				case 14:
				{
					IReadOnlyList<Config.ShortList> favorabilityDiffs = isSpecial ? SecretInformationProcessor._effectConfig.ReactorFriendFavorabilityDiffsWhenSpecial : SecretInformationProcessor._effectConfig.ReactorFriendFavorabilityDiffs;
					goto IL_19A;
				}
				case 6:
				{
					IReadOnlyList<Config.ShortList> favorabilityDiffs = isSpecial ? SecretInformationProcessor._effectConfig.ReactorEnemyFavorabilityDiffsWhenSpecial : SecretInformationProcessor._effectConfig.ReactorEnemyFavorabilityDiffs;
					goto IL_19A;
				}
				case 8:
				case 12:
				case 15:
				{
					IReadOnlyList<Config.ShortList> favorabilityDiffs = isSpecial ? SecretInformationProcessor._effectConfig.SecactorFriendFavorabilityDiffsWhenSpecial : SecretInformationProcessor._effectConfig.SecactorFriendFavorabilityDiffs;
					goto IL_19A;
				}
				case 9:
				{
					IReadOnlyList<Config.ShortList> favorabilityDiffs = isSpecial ? SecretInformationProcessor._effectConfig.SecactorEnemyFavorabilityDiffsWhenSpecial : SecretInformationProcessor._effectConfig.SecactorEnemyFavorabilityDiffs;
					goto IL_19A;
				}
				}
				return result;
				IL_19A:
				result = new List<int>();
				for (int i = 0; i < 4; i++)
				{
					IReadOnlyList<Config.ShortList> favorabilityDiffs;
					int favor = (int)favorabilityDiffs[i].DataList[(int)behaviorType];
					bool flag2 = isSpecial;
					if (flag2)
					{
						favor = favor * conditionOdds / 100;
					}
					result.Add(favor);
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x0600542F RID: 21551 RVA: 0x002DE7A4 File Offset: 0x002DC9A4
		private List<int> CalFinalDeltaFavorabilityOfActors_WithActorIndex(int actorIndex)
		{
			List<int> result = new List<int>
			{
				0,
				0,
				0,
				0
			};
			GameData.Domains.Character.Character character;
			bool flag = !SecretInformationProcessor.ActiveActorList.TryGetValue(actorIndex, out character);
			List<int> result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				sbyte behaviorType = character.GetBehaviorType();
				bool isSpecial = SecretInformationProcessor._baseFavorOdds != 0;
				IReadOnlyList<Config.ShortList> favorabilityDiffs;
				switch (actorIndex)
				{
				case 0:
					favorabilityDiffs = (isSpecial ? SecretInformationProcessor._effectConfig.ActorFavorabilityDiffsWhenSpecial : SecretInformationProcessor._effectConfig.ActorFavorabilityDiffs);
					break;
				case 1:
					favorabilityDiffs = (isSpecial ? SecretInformationProcessor._effectConfig.ReactorFavorabilityDiffsWhenSpecial : SecretInformationProcessor._effectConfig.ReactorFavorabilityDiffs);
					break;
				case 2:
					favorabilityDiffs = (isSpecial ? SecretInformationProcessor._effectConfig.SecactorFavorabilityDiffsWhenSpecial : SecretInformationProcessor._effectConfig.SecactorFavorabilityDiffs);
					break;
				default:
					return result;
				}
				result = new List<int>();
				int curIndex = 0;
				for (int i = 0; i < 4; i++)
				{
					bool flag2 = i == actorIndex;
					if (flag2)
					{
						result.Add(0);
					}
					else
					{
						int favor = (int)favorabilityDiffs[curIndex].DataList[(int)behaviorType];
						bool flag3 = isSpecial;
						if (flag3)
						{
							favor = favor * SecretInformationProcessor._baseFavorOdds / 100;
						}
						result.Add(favor);
						curIndex++;
					}
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06005430 RID: 21552 RVA: 0x002DE8F8 File Offset: 0x002DCAF8
		public List<InformationDomain.SecretInformationFavorChangeItem> GetAllSecretInformationFavorabilityChangeWithSource(int sourceCharId)
		{
			Dictionary<int, Dictionary<int, int>> result = new Dictionary<int, Dictionary<int, int>>();
			Dictionary<GameData.Domains.Character.Character, int> relatedCharList = this.GetAllSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex(false);
			List<GameData.Domains.Character.Character> disseminationCharList = this.GetSecretInformationDisseminationBranchCharacterIds_WithTuringTest();
			foreach (KeyValuePair<int, GameData.Domains.Character.Character> actor in SecretInformationProcessor.ActiveActorList)
			{
				int charId = actor.Value.GetId();
				List<int> deltaFavorList = this.CalFinalDeltaFavorabilityOfActors_WithActorIndex(actor.Key);
				foreach (KeyValuePair<int, GameData.Domains.Character.Character> targetActor in SecretInformationProcessor.ActiveActorList)
				{
					bool flag = targetActor.Key == actor.Key;
					if (!flag)
					{
						int deltaFavor = deltaFavorList[targetActor.Key];
						bool flag2 = deltaFavor == 0;
						if (!flag2)
						{
							int targetId = targetActor.Value.GetId();
							bool flag3 = charId == targetId;
							if (!flag3)
							{
								bool flag4 = !result.ContainsKey(charId);
								if (flag4)
								{
									result.Add(charId, new Dictionary<int, int>());
								}
								bool flag5 = !result[charId].ContainsKey(targetId);
								if (flag5)
								{
									result[charId].Add(targetId, 0);
								}
								Dictionary<int, int> dictionary = result[charId];
								int key = targetId;
								dictionary[key] += deltaFavorList[3];
							}
						}
					}
				}
				bool flag6 = deltaFavorList[3] != 0;
				if (flag6)
				{
					bool flag7 = charId == sourceCharId || sourceCharId == -1;
					if (!flag7)
					{
						bool flag8 = !result.ContainsKey(charId);
						if (flag8)
						{
							result.Add(charId, new Dictionary<int, int>());
						}
						bool flag9 = !result[charId].ContainsKey(sourceCharId);
						if (flag9)
						{
							result[charId].Add(sourceCharId, 0);
						}
						Dictionary<int, int> dictionary = result[charId];
						dictionary[sourceCharId] += deltaFavorList[3];
					}
				}
			}
			foreach (KeyValuePair<GameData.Domains.Character.Character, int> item in relatedCharList)
			{
				int charId2 = item.Key.GetId();
				List<int> deltaFavorList2 = this.CalFinalDeltaFavorabilityOfRelatedCharacters_WithRelationIndex(item.Key, item.Value);
				foreach (KeyValuePair<int, GameData.Domains.Character.Character> actor2 in SecretInformationProcessor.ActiveActorList)
				{
					int targerId = actor2.Value.GetId();
					bool flag10 = charId2 == targerId;
					if (!flag10)
					{
						int deltaFavor2 = deltaFavorList2[actor2.Key];
						bool flag11 = deltaFavor2 != 0;
						if (flag11)
						{
							bool flag12 = !result.ContainsKey(charId2);
							if (flag12)
							{
								result.Add(charId2, new Dictionary<int, int>());
							}
							bool flag13 = !result[charId2].ContainsKey(targerId);
							if (flag13)
							{
								result[charId2].Add(targerId, 0);
							}
							Dictionary<int, int> dictionary = result[charId2];
							int key = targerId;
							dictionary[key] += deltaFavor2;
						}
					}
				}
				bool flag14 = deltaFavorList2[3] != 0;
				if (flag14)
				{
					bool flag15 = charId2 == sourceCharId || sourceCharId == -1;
					if (!flag15)
					{
						bool flag16 = !result.ContainsKey(charId2);
						if (flag16)
						{
							result.Add(charId2, new Dictionary<int, int>());
						}
						bool flag17 = !result[charId2].ContainsKey(sourceCharId);
						if (flag17)
						{
							result[charId2].Add(sourceCharId, 0);
						}
						Dictionary<int, int> dictionary = result[charId2];
						dictionary[sourceCharId] += deltaFavorList2[3];
					}
				}
			}
			List<InformationDomain.SecretInformationFavorChangeItem> finalResult = new List<InformationDomain.SecretInformationFavorChangeItem>();
			Dictionary<int, int> relatedIdList = relatedCharList.ToDictionary((KeyValuePair<GameData.Domains.Character.Character, int> x) => x.Key.GetId(), (KeyValuePair<GameData.Domains.Character.Character, int> x) => x.Value);
			foreach (KeyValuePair<int, Dictionary<int, int>> item2 in result)
			{
				sbyte basePriority = 0;
				bool flag18 = SecretInformationProcessor.ActorIdList.Contains(item2.Key);
				if (flag18)
				{
					basePriority += 10;
				}
				bool flag19 = item2.Key == SecretInformationProcessor.ArgList[5];
				if (flag19)
				{
					basePriority += 10;
				}
				bool flag20 = DomainManager.Taiwu.IsInGroup(item2.Key);
				if (flag20)
				{
					basePriority += 5;
				}
				int value;
				bool flag21 = relatedIdList.TryGetValue(item2.Key, out value) && value != 0;
				if (flag21)
				{
					basePriority += 4;
				}
				foreach (KeyValuePair<int, int> item3 in item2.Value)
				{
					sbyte priority = 0;
					bool flag22 = item3.Key == SecretInformationProcessor.ArgList[5];
					if (flag22)
					{
						priority += 15;
					}
					finalResult.Add(new InformationDomain.SecretInformationFavorChangeItem(item2.Key, item3.Key, item3.Value, basePriority + priority));
				}
			}
			return finalResult;
		}

		// Token: 0x06005431 RID: 21553 RVA: 0x002DEF0C File Offset: 0x002DD10C
		public List<InformationDomain.SecretInformationStartEnemyRelationItem> GetAllSecretInformationStartEnemyRelationItems(int sourceCharId)
		{
			Dictionary<GameData.Domains.Character.Character, int> relatedCharList = this.GetAllSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex(true);
			List<InformationDomain.SecretInformationStartEnemyRelationItem> result = new List<InformationDomain.SecretInformationStartEnemyRelationItem>();
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			foreach (KeyValuePair<GameData.Domains.Character.Character, int> related in relatedCharList)
			{
				int charId = related.Key.GetId();
				foreach (KeyValuePair<int, GameData.Domains.Character.Character> actor in SecretInformationProcessor.ActiveActorList)
				{
					int targetId = actor.Value.GetId();
					bool flag = charId == targetId || charId == taiwuId;
					if (!flag)
					{
						byte odds = this.GetStartEnemyRelationOdds(actor.Key, related.Value);
						InformationDomain.SecretInformationStartEnemyRelationItem item = new InformationDomain.SecretInformationStartEnemyRelationItem(this.GetSecretInformationTemplateId(), charId, targetId, odds);
						result.Add(item);
					}
				}
				bool flag2 = charId == sourceCharId || sourceCharId == -1 || charId == taiwuId;
				if (!flag2)
				{
					byte odds2 = this.GetStartEnemyRelationOdds(3, related.Value);
					InformationDomain.SecretInformationStartEnemyRelationItem item2 = new InformationDomain.SecretInformationStartEnemyRelationItem(this.GetSecretInformationTemplateId(), charId, sourceCharId, odds2);
					result.Add(item2);
				}
			}
			return result;
		}

		// Token: 0x06005432 RID: 21554 RVA: 0x002DF06C File Offset: 0x002DD26C
		private byte GetStartEnemyRelationOdds(int actorKey, int relatedValue)
		{
			bool flag = actorKey == 0;
			List<byte> odds;
			if (flag)
			{
				odds = SecretInformationProcessor._effectConfig.StartEnemyRelationOddsToActor;
			}
			else
			{
				bool flag2 = actorKey == 1;
				if (flag2)
				{
					odds = SecretInformationProcessor._effectConfig.StartEnemyRelationOddsToReactor;
				}
				else
				{
					bool flag3 = actorKey == 2;
					if (flag3)
					{
						odds = SecretInformationProcessor._effectConfig.StartEnemyRelationOddsToSecactor;
					}
					else
					{
						bool flag4 = actorKey == 3;
						if (flag4)
						{
							odds = SecretInformationProcessor._effectConfig.StartEnemyRelationOddsToSource;
						}
						else
						{
							odds = new List<byte>();
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
							defaultInterpolatedStringHandler.AppendLiteral("wrong actorKey: ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(actorKey);
							AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
				}
			}
			byte result;
			if (relatedValue - 1 > 8)
			{
				result = 0;
			}
			else
			{
				result = odds[relatedValue - 1];
			}
			return result;
		}

		// Token: 0x06005433 RID: 21555 RVA: 0x002DF134 File Offset: 0x002DD334
		public List<InformationDomain.SecretInformationHappinessChangeItem> GetAllSecretInformationHappinessChange()
		{
			Dictionary<int, int> result = new Dictionary<int, int>();
			Dictionary<GameData.Domains.Character.Character, int> relatedCharList = this.GetAllSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex(false);
			bool flag = relatedCharList == null;
			List<InformationDomain.SecretInformationHappinessChangeItem> result2;
			if (flag)
			{
				result2 = new List<InformationDomain.SecretInformationHappinessChangeItem>();
			}
			else
			{
				Dictionary<int, GameData.Domains.Character.Character> actorList = this.GetActiveActorList_WithRelationIndex();
				foreach (KeyValuePair<int, GameData.Domains.Character.Character> actor in actorList)
				{
					sbyte deltaHappiness = this.CalDeltaHappinessOfRelatedCharacters_WithRelationIndex(actor.Key);
					bool flag2 = deltaHappiness == 0;
					if (!flag2)
					{
						int charId = actor.Value.GetId();
						bool flag3 = !result.ContainsKey(charId);
						if (flag3)
						{
							result.Add(charId, 0);
						}
						Dictionary<int, int> dictionary = result;
						int key = charId;
						dictionary[key] += (int)deltaHappiness;
					}
				}
				foreach (KeyValuePair<GameData.Domains.Character.Character, int> actor2 in relatedCharList)
				{
					sbyte deltaHappiness2 = this.CalDeltaHappinessOfRelatedCharacters_WithRelationIndex(actor2.Value);
					bool flag4 = deltaHappiness2 == 0;
					if (!flag4)
					{
						int charId2 = actor2.Key.GetId();
						bool flag5 = !result.ContainsKey(charId2);
						if (flag5)
						{
							result.Add(charId2, 0);
						}
						Dictionary<int, int> dictionary = result;
						int key = charId2;
						dictionary[key] += (int)deltaHappiness2;
					}
				}
				List<InformationDomain.SecretInformationHappinessChangeItem> finalResult = new List<InformationDomain.SecretInformationHappinessChangeItem>();
				Dictionary<int, int> relatedIdList = relatedCharList.ToDictionary((KeyValuePair<GameData.Domains.Character.Character, int> x) => x.Key.GetId(), (KeyValuePair<GameData.Domains.Character.Character, int> x) => x.Value);
				foreach (KeyValuePair<int, int> item in result)
				{
					sbyte basePriority = 0;
					bool flag6 = SecretInformationProcessor.ActorIdList.Contains(item.Key);
					if (flag6)
					{
						basePriority += 10;
					}
					bool flag7 = DomainManager.Taiwu.IsInGroup(item.Key);
					if (flag7)
					{
						basePriority += 5;
					}
					bool flag8 = item.Key == SecretInformationProcessor.ArgList[5];
					if (flag8)
					{
						basePriority += 10;
					}
					int value;
					bool flag9 = relatedIdList.TryGetValue(item.Key, out value) && value != 0;
					if (flag9)
					{
						basePriority += 4;
					}
					finalResult.Add(new InformationDomain.SecretInformationHappinessChangeItem(item.Key, item.Value, basePriority));
				}
				result2 = finalResult;
			}
			return result2;
		}

		// Token: 0x06005434 RID: 21556 RVA: 0x002DF3E8 File Offset: 0x002DD5E8
		public sbyte CalDeltaHappinessOfRelatedCharacters_WithRelationIndex(int relationIndex)
		{
			sbyte result;
			switch (relationIndex)
			{
			case 1:
				result = SecretInformationProcessor._effectConfig.ActorHappinessDiffs[0];
				break;
			case 2:
			case 10:
			case 13:
				result = SecretInformationProcessor._effectConfig.ActorHappinessDiffs[1];
				break;
			case 3:
				result = SecretInformationProcessor._effectConfig.ActorHappinessDiffs[2];
				break;
			case 4:
				result = SecretInformationProcessor._effectConfig.ReactorHappinessDiffs[0];
				break;
			case 5:
			case 11:
			case 14:
				result = SecretInformationProcessor._effectConfig.ReactorHappinessDiffs[1];
				break;
			case 6:
				result = SecretInformationProcessor._effectConfig.ReactorHappinessDiffs[2];
				break;
			case 7:
				result = SecretInformationProcessor._effectConfig.SecactorHappinessDiffs[0];
				break;
			case 8:
			case 12:
			case 15:
				result = SecretInformationProcessor._effectConfig.SecactorHappinessDiffs[1];
				break;
			case 9:
				result = SecretInformationProcessor._effectConfig.SecactorHappinessDiffs[2];
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x002DF4D0 File Offset: 0x002DD6D0
		public List<short> GetActorFameRecord_WithActorIndex(int actorIndex, bool includeDeath = false)
		{
			bool flag = actorIndex < 0 || actorIndex > 2;
			bool flag2 = flag;
			List<short> result2;
			if (flag2)
			{
				result2 = new List<short>();
			}
			else
			{
				GameData.Domains.Character.Character character;
				bool flag3 = !includeDeath && !SecretInformationProcessor.ActiveActorList.TryGetValue(actorIndex, out character);
				if (flag3)
				{
					result2 = new List<short>();
				}
				else
				{
					List<ValueTuple<short, sbyte>> result = new List<ValueTuple<short, sbyte>>();
					if (!true)
					{
					}
					List<Config.ShortList> list;
					switch (actorIndex)
					{
					case 0:
						list = SecretInformationProcessor._effectConfig.ActorFameApplyCondition;
						break;
					case 1:
						list = SecretInformationProcessor._effectConfig.ReactorFameApplyCondition;
						break;
					case 2:
						list = SecretInformationProcessor._effectConfig.SeactorFameApplyCondition;
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
						defaultInterpolatedStringHandler.AppendLiteral("actorIndex impossible to other case ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(actorIndex);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
					if (!true)
					{
					}
					IReadOnlyList<Config.ShortList> fameItemCondition = list;
					if (!true)
					{
					}
					switch (actorIndex)
					{
					case 0:
						list = SecretInformationProcessor._effectConfig.ActorFameApplyContent;
						break;
					case 1:
						list = SecretInformationProcessor._effectConfig.ReactorFameApplyContent;
						break;
					case 2:
						list = SecretInformationProcessor._effectConfig.SecactorFameApplyContent;
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
						defaultInterpolatedStringHandler.AppendLiteral("actorIndex impossible to other case ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(actorIndex);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
					if (!true)
					{
					}
					IReadOnlyList<Config.ShortList> fameItemContent = list;
					foreach (List<short> conditionItem in from item in fameItemCondition
					select item.DataList)
					{
						bool flag4 = conditionItem.Count < 2;
						if (!flag4)
						{
							short fameKey = conditionItem[0];
							bool flag5 = fameKey < 0;
							if (!flag5)
							{
								bool flag6 = conditionItem[1] == 54;
								bool conditionMet;
								if (flag6)
								{
									int checkTargetIndex = this.GetArgIndexByRelationConfigDefKey(conditionItem[2]);
									short num;
									conditionMet = (this.CalSectPunishLevel_WithActorIndex(checkTargetIndex, out num, false) >= 0);
								}
								else
								{
									conditionMet = this.ConditionBoxEntrance(conditionItem, 1, -1, true);
								}
								bool flag7 = conditionMet;
								if (flag7)
								{
									sbyte fameType = -1;
									int fameLevel = 1;
									foreach (List<short> contentItem in fameItemContent.Select((Config.ShortList item) => item.DataList))
									{
										bool flag8 = fameKey != contentItem[0];
										if (!flag8)
										{
											bool flag9 = contentItem.Count > 2;
											if (flag9)
											{
												fameLevel = (int)contentItem[2];
												bool flag10 = fameKey == 82;
												if (flag10)
												{
													short num;
													PunishmentSeverityItem punishmentSeverity = PunishmentSeverity.Instance.GetItem(this.CalSectPunishLevel_WithActorIndex(actorIndex, out num, false));
													bool flag11 = punishmentSeverity != null;
													if (flag11)
													{
														fameLevel *= punishmentSeverity.FameActionFactorInPunish;
													}
												}
											}
											int index = this.GetArgIndexByRelationConfigDefKey(contentItem[1]);
											bool flag12 = index > 0 && index < SecretInformationProcessor.ArgList.Count;
											if (flag12)
											{
												fameType = this.GetFameTypeSafe(SecretInformationProcessor.ArgList[index]);
											}
										}
									}
									for (int c = 0; c < fameLevel; c++)
									{
										result.Add(new ValueTuple<short, sbyte>(fameKey, fameType));
									}
								}
							}
						}
					}
					List<short> finalResult = new List<short>();
					foreach (ValueTuple<short, sbyte> item2 in result)
					{
						short fameKey2 = item2.Item1;
						FameActionItem fameConfig = FameAction.Instance.GetItem(fameKey2);
						bool flag13 = fameConfig == null;
						if (!flag13)
						{
							bool flag14 = fameConfig.HasJump && item2.Item2 != -1;
							if (flag14)
							{
								bool flag15 = item2.Item2 == 3 || item2.Item2 == -2;
								if (flag15)
								{
									fameKey2 = fameConfig.NormalJumpId;
								}
								else
								{
									bool flag16 = item2.Item2 < 3;
									if (flag16)
									{
										fameKey2 = fameConfig.BadJumpId;
									}
									else
									{
										fameKey2 = fameConfig.GoodJumpId;
									}
								}
								fameConfig = FameAction.Instance.GetItem(fameKey2);
								bool flag17 = fameConfig == null;
								if (flag17)
								{
									continue;
								}
							}
							finalResult.Add(fameKey2);
						}
					}
					result2 = finalResult;
				}
			}
			return result2;
		}

		// Token: 0x06005436 RID: 21558 RVA: 0x002DF978 File Offset: 0x002DDB78
		public List<short> GetActorFameRecord_WithCharId(int charId, bool includeDeath = false)
		{
			bool flag = !SecretInformationProcessor.ActorIdList.Contains(charId);
			List<short> result;
			if (flag)
			{
				result = new List<short>();
			}
			else
			{
				int index = SecretInformationProcessor.ArgList.FindIndex((int x) => x == charId);
				result = this.GetActorFameRecord_WithActorIndex(index, includeDeath);
			}
			return result;
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x002DF9D8 File Offset: 0x002DDBD8
		public int IsActorFameRecordPositive_WithActorIndex(int actorIndex)
		{
			List<short> fameKeyList = this.GetActorFameRecord_WithActorIndex(actorIndex, true);
			short totalFame = 0;
			foreach (short fameKey in fameKeyList)
			{
				FameActionItem fameConfig = FameAction.Instance.GetItem(fameKey);
				bool flag = fameConfig == null;
				if (!flag)
				{
					totalFame += (short)fameConfig.Fame;
				}
			}
			return (int)totalFame;
		}

		// Token: 0x06005438 RID: 21560 RVA: 0x002DFA5C File Offset: 0x002DDC5C
		public int IsActorFameRecordPositive_WithCharId(int charId)
		{
			bool flag = !SecretInformationProcessor.ActorIdList.Contains(charId);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int index = SecretInformationProcessor.ArgList.FindIndex((int x) => x == charId);
				result = this.IsActorFameRecordPositive_WithActorIndex(index);
			}
			return result;
		}

		// Token: 0x06005439 RID: 21561 RVA: 0x002DFAB4 File Offset: 0x002DDCB4
		public short GetSecretInformationAppliedStructs(IRandomSource random, GameData.Domains.Character.Character character, GameData.Domains.Character.Character taiwu)
		{
			short structId = -1;
			bool flag = SecretInformationProcessor._infoConfig.StructGroupId == -1;
			short result;
			if (flag)
			{
				result = structId;
			}
			else
			{
				List<int> taiwuIndexList = this.GetAllSecretInformationRelationsOfCharacter(taiwu.GetId(), true, false);
				List<int> charIndexList = this.GetAllSecretInformationRelationsOfCharacter(character.GetId(), true, false);
				List<ValueTuple<short, short, short[]>> structIdList = (from s in SecretInformationAppliedStruct.Instance
				where s != null && s.GroupTemplateId == SecretInformationProcessor._infoConfig.StructGroupId && taiwuIndexList.Contains((int)s.TaiwuIndex) && charIndexList.Contains((int)s.CharIndex)
				select new ValueTuple<short, short, short[]>(s.TemplateId, s.RelationValue, s.BehaviorTypeValue)).ToList<ValueTuple<short, short, short[]>>();
				sbyte behaviorType = character.GetBehaviorType();
				bool flag2 = structIdList.Count != 0;
				if (flag2)
				{
					bool flag3 = structIdList.Count == 1;
					if (flag3)
					{
						return structIdList[0].Item1;
					}
					structId = structIdList.ElementAt(random.Next(0, structIdList.Count)).Item1;
					Dictionary<short, List<ValueTuple<short, short, short[]>>> weightGroup = new Dictionary<short, List<ValueTuple<short, short, short[]>>>();
					foreach (ValueTuple<short, short, short[]> item in structIdList)
					{
						bool flag4 = !weightGroup.ContainsKey(item.Item2);
						if (flag4)
						{
							weightGroup.Add(item.Item2, new List<ValueTuple<short, short, short[]>>());
						}
						weightGroup[item.Item2].Add(item);
					}
					short maxWeightId = weightGroup.Keys.Max<short>();
					bool flag5 = weightGroup[maxWeightId].Count != 0;
					if (flag5)
					{
						bool flag6 = weightGroup[maxWeightId].Count == 1;
						if (flag6)
						{
							return weightGroup[maxWeightId][0].Item1;
						}
						structId = weightGroup[maxWeightId].ElementAt(random.Next(0, weightGroup[maxWeightId].Count)).Item1;
						List<short> structPool = new List<short>();
						short defaultWeight = (short)(100 / weightGroup[maxWeightId].Count);
						foreach (ValueTuple<short, short, short[]> item2 in weightGroup[maxWeightId])
						{
							short weight = item2.Item3[(int)behaviorType];
							bool flag7 = weight == -1;
							if (flag7)
							{
								weight = defaultWeight;
							}
							for (int i = 0; i < (int)weight; i++)
							{
								structPool.Add(item2.Item1);
							}
						}
						bool flag8 = structPool.Count != 0;
						if (flag8)
						{
							structId = structPool.ElementAt(EventHelper.GetRandom(0, structPool.Count));
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 2);
							defaultInterpolatedStringHandler.AppendLiteral("No Available Structs In BehaviorTypeValue Check! metaDataId:");
							defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._metaDataId);
							defaultInterpolatedStringHandler.AppendLiteral(" TemplateId:");
							defaultInterpolatedStringHandler.AppendFormatted<short>(SecretInformationProcessor._templateId);
							AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 2);
						defaultInterpolatedStringHandler.AppendLiteral("No Available Structs In RelationValue Check! metaDataId:");
						defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._metaDataId);
						defaultInterpolatedStringHandler.AppendLiteral(" TemplateId:");
						defaultInterpolatedStringHandler.AppendFormatted<short>(SecretInformationProcessor._templateId);
						AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 2);
					defaultInterpolatedStringHandler.AppendLiteral("No Available SecretInformationAppliedStructs! metaDataId:");
					defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._metaDataId);
					defaultInterpolatedStringHandler.AppendLiteral(" TemplateId:");
					defaultInterpolatedStringHandler.AppendFormatted<short>(SecretInformationProcessor._templateId);
					AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = structId;
			}
			return result;
		}

		// Token: 0x0600543A RID: 21562 RVA: 0x002DFE64 File Offset: 0x002DE064
		public short GetContentIdAndSelections(IRandomSource random, short structId, GameData.Domains.Character.Character character, GameData.Domains.Character.Character taiwu, out List<short> selectionList, out short contentIndex)
		{
			short contentId = -1;
			contentIndex = -1;
			selectionList = new List<short>();
			SecretInformationAppliedStructItem structConfig = SecretInformationAppliedStruct.Instance.GetItem(structId);
			bool flag = structConfig == null;
			short result;
			if (flag)
			{
				result = contentId;
			}
			else
			{
				List<Config.ShortList> extraContentIds = structConfig.ExtraContentIds;
				List<Config.ShortList> keepCondition = structConfig.ActorSectPunishSpecialCondition;
				bool flag2 = structConfig.ContentId2 != -1 && !this.ConditionIsPublished() && !DomainManager.Information.IsSecretInformationInBroadcast(SecretInformationProcessor._metaDataId);
				if (flag2)
				{
					sbyte behaviorType = character.GetBehaviorType();
					short keepRateBase = this.CalBaseKeepRateBase(character.GetId(), behaviorType, keepCondition);
					sbyte favorLevel = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(character.GetId(), taiwu.GetId()));
					int keepRate = (int)(keepRateBase * (short)(100 - (favorLevel - 20) * 20) / 100);
					bool flag3 = random.Next(0, 100) < keepRate;
					if (flag3)
					{
						contentIndex = 2;
						contentId = structConfig.ContentId2;
					}
				}
				bool flag4 = contentIndex == -1 && extraContentIds.Count > 0;
				if (flag4)
				{
					for (int i = 0; i < extraContentIds.Count; i++)
					{
						List<short> conditionItem = extraContentIds[i].DataList;
						bool flag5 = conditionItem.Count < 2;
						if (!flag5)
						{
							bool flag6 = this.ConditionBoxEntrance(conditionItem, 1, character.GetId(), false);
							if (flag6)
							{
								contentIndex = (short)(i + 3);
								contentId = conditionItem[0];
							}
						}
					}
				}
				bool flag7 = contentIndex == -1;
				if (flag7)
				{
					contentIndex = 1;
					contentId = structConfig.ContentId1;
				}
				List<short> selectionIdList = new List<short>();
				bool flag8 = contentIndex > 2;
				if (flag8)
				{
					bool flag9 = structConfig.ExtraSelections.Count<Config.ShortList>() > (int)(contentIndex - 3);
					if (flag9)
					{
						selectionIdList = structConfig.ExtraSelections[(int)(contentIndex - 3)].DataList;
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 3);
						defaultInterpolatedStringHandler.AppendLiteral("No Available ExtraSelections ! metaDataId:");
						defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._metaDataId);
						defaultInterpolatedStringHandler.AppendLiteral(" TemplateId:");
						defaultInterpolatedStringHandler.AppendFormatted<short>(SecretInformationProcessor._templateId);
						defaultInterpolatedStringHandler.AppendLiteral(" StructId:");
						defaultInterpolatedStringHandler.AppendFormatted<short>(structId);
						AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				else
				{
					bool flag10 = contentIndex == 2;
					if (flag10)
					{
						bool flag11 = structConfig.Selection2 != null;
						if (flag11)
						{
							selectionIdList = structConfig.Selection2.ToList<short>();
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 3);
							defaultInterpolatedStringHandler.AppendLiteral("No Available Selection For Keep ! metaDataId:");
							defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._metaDataId);
							defaultInterpolatedStringHandler.AppendLiteral(" TemplateId:");
							defaultInterpolatedStringHandler.AppendFormatted<short>(SecretInformationProcessor._templateId);
							defaultInterpolatedStringHandler.AppendLiteral(" StructId:");
							defaultInterpolatedStringHandler.AppendFormatted<short>(structId);
							AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					else
					{
						bool flag12 = structConfig.Selection1 != null;
						if (flag12)
						{
							selectionIdList = structConfig.Selection1.ToList<short>();
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 3);
							defaultInterpolatedStringHandler.AppendLiteral("No Available Selection For Default! metaDataId:");
							defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._metaDataId);
							defaultInterpolatedStringHandler.AppendLiteral(" TemplateId:");
							defaultInterpolatedStringHandler.AppendFormatted<short>(SecretInformationProcessor._templateId);
							defaultInterpolatedStringHandler.AppendLiteral(" StructId:");
							defaultInterpolatedStringHandler.AppendFormatted<short>(structId);
							AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
				}
				bool flag13 = selectionIdList == null;
				if (flag13)
				{
					selectionIdList = new List<short>();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Fail To Get Selection! metaDataId:");
					defaultInterpolatedStringHandler.AppendFormatted<int>(SecretInformationProcessor._metaDataId);
					defaultInterpolatedStringHandler.AppendLiteral(" TemplateId:");
					defaultInterpolatedStringHandler.AppendFormatted<short>(SecretInformationProcessor._templateId);
					defaultInterpolatedStringHandler.AppendLiteral(" StructId:");
					defaultInterpolatedStringHandler.AppendFormatted<short>(structId);
					AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				selectionIdList.RemoveAll((short x) => x < 0);
				selectionList = selectionIdList;
				result = contentId;
			}
			return result;
		}

		// Token: 0x0600543B RID: 21563 RVA: 0x002E024C File Offset: 0x002DE44C
		private short CalBaseKeepRateBase(int charId, sbyte behaviorType, List<Config.ShortList> keepCondition)
		{
			short keepRate = 0;
			foreach (Config.ShortList item in keepCondition)
			{
				List<short> conditionItem = item.DataList;
				bool flag = conditionItem.Count == 0;
				if (!flag)
				{
					short conditionKey = conditionItem[0];
					bool flag2 = conditionKey == 0;
					if (flag2)
					{
						keepRate += SecretInformationEffect.Instance[SecretInformationProcessor._templateId].BaseSecretRate[(int)behaviorType];
					}
					else
					{
						bool flag3 = this.ConditionBoxEntrance(conditionItem, 0, charId, false);
						if (flag3)
						{
							keepRate += SecretInformationSpecialCondition.Instance[conditionKey].RequestKeepSecretRate[(int)behaviorType];
						}
					}
				}
			}
			return keepRate;
		}

		// Token: 0x0600543C RID: 21564 RVA: 0x002E0318 File Offset: 0x002DE518
		public List<short> GetVisibleSelection(IEnumerable<short> selectionKeys, GameData.Domains.Character.Character character, GameData.Domains.Character.Character taiwu)
		{
			List<ValueTuple<short, short>> result = new List<ValueTuple<short, short>>();
			foreach (short id in selectionKeys)
			{
				bool flag = id < 0;
				if (!flag)
				{
					SecretInformationAppliedSelectionItem selectionConfig = SecretInformationAppliedSelection.Instance.GetItem(id);
					bool flag2 = selectionConfig == null;
					if (!flag2)
					{
						bool flag3 = selectionConfig.SpecialConditionId != null;
						if (flag3)
						{
							bool conditionCheck = true;
							foreach (Config.ShortList item in selectionConfig.SpecialConditionId)
							{
								List<short> conditionItem = item.DataList;
								bool flag4 = conditionItem.Count == 0;
								if (!flag4)
								{
									short conditionKey = conditionItem[0];
									bool flag5 = conditionKey <= 0;
									if (!flag5)
									{
										bool flag6 = !this.ConditionBoxEntrance(conditionItem, 0, character.GetId(), false);
										if (flag6)
										{
											conditionCheck = false;
											break;
										}
									}
								}
							}
							bool flag7 = !conditionCheck;
							if (flag7)
							{
								continue;
							}
						}
						bool flag8 = selectionConfig.SpecialConditionId2 != null && selectionConfig.SpecialConditionId2.Count != 0;
						if (flag8)
						{
							bool conditionCheck2 = false;
							foreach (Config.ShortList item2 in selectionConfig.SpecialConditionId2)
							{
								List<short> conditionItem2 = item2.DataList;
								bool flag9 = conditionItem2.Count == 0;
								if (flag9)
								{
									conditionCheck2 = true;
									break;
								}
								short conditionKey2 = conditionItem2[0];
								bool flag10 = conditionKey2 <= 0;
								if (flag10)
								{
									conditionCheck2 = true;
									break;
								}
								bool flag11 = this.ConditionBoxEntrance(conditionItem2, 0, character.GetId(), false);
								if (flag11)
								{
									conditionCheck2 = true;
									break;
								}
							}
							bool flag12 = !conditionCheck2;
							if (flag12)
							{
								continue;
							}
						}
						sbyte taiwuFame = taiwu.GetFameType();
						sbyte charFame = character.GetFameType();
						sbyte[] fameConditions = selectionConfig.FameConditions;
						bool flag13 = fameConditions != null;
						if (flag13)
						{
							bool flag14 = !this.SelectionFameCheck(new List<sbyte>
							{
								taiwuFame,
								charFame,
								(fameConditions[2] == -1) ? -1 : this.GetFameTypeSafe(SecretInformationProcessor.ArgList[1]),
								(fameConditions[3] == -1) ? -1 : this.GetFameTypeSafe(SecretInformationProcessor.ArgList[3])
							}, fameConditions);
							if (flag14)
							{
								continue;
							}
						}
						short priority = selectionConfig.Priority;
						bool flag15 = priority < 0;
						if (flag15)
						{
							priority = short.MaxValue;
						}
						result.Add(new ValueTuple<short, short>(id, priority));
					}
				}
			}
			return (from x in result
			orderby x.Item2, x.Item1
			select x.Item1).ToList<short>();
		}

		// Token: 0x0600543D RID: 21565 RVA: 0x002E067C File Offset: 0x002DE87C
		private bool SelectionFameCheck(List<sbyte> fameTypes, sbyte[] fameCondition)
		{
			for (int i = 0; i < fameCondition.Length; i++)
			{
				bool flag = fameCondition[i] == -1;
				if (!flag)
				{
					bool flag2 = fameTypes[i] == -1;
					bool result;
					if (flag2)
					{
						result = false;
					}
					else
					{
						sbyte b = fameCondition[i];
						sbyte b2 = b;
						if (b2 != 0)
						{
							if (b2 != 1)
							{
								goto IL_71;
							}
							bool flag3 = FameType.IsNonNegative(fameTypes[i], true);
							if (flag3)
							{
								goto IL_71;
							}
							result = false;
						}
						else
						{
							bool flag4 = !FameType.IsNonNegative(fameTypes[i], true);
							if (flag4)
							{
								goto IL_71;
							}
							result = false;
						}
					}
					return result;
				}
				IL_71:;
			}
			return true;
		}

		// Token: 0x0600543E RID: 21566 RVA: 0x002E0710 File Offset: 0x002DE910
		private short GetResultIdOfSelection(short selectionId, GameData.Domains.Character.Character character, GameData.Domains.Character.Character taiwu)
		{
			SecretInformationAppliedSelectionItem selectionConfig = SecretInformationAppliedSelection.Instance.GetItem(selectionId);
			sbyte behaviorType = EventHelper.GetRoleBehavior(character);
			sbyte requiredFavor = selectionConfig.Result2FavorabilityTypeCondition[(int)behaviorType];
			bool flag = EventHelper.GetFavorabilityType(character, taiwu) < requiredFavor;
			short result;
			if (flag)
			{
				result = selectionConfig.ResultId2;
			}
			else
			{
				result = selectionConfig.ResultId1;
			}
			return result;
		}

		// Token: 0x0600543F RID: 21567 RVA: 0x002E0760 File Offset: 0x002DE960
		public sbyte CalSectPunishLevel_WithCharId(int charId, bool calRealLevel = false)
		{
			bool flag = !SecretInformationProcessor.ActorIdList.Contains(charId);
			sbyte result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				int charIndex = SecretInformationProcessor.ArgList.FindIndex((int a) => a == charId);
				short num;
				result = this.CalSectPunishLevel_WithActorIndex(charIndex, out num, calRealLevel);
			}
			return result;
		}

		// Token: 0x06005440 RID: 21568 RVA: 0x002E07BC File Offset: 0x002DE9BC
		internal sbyte CalcSectPunishLevelWithSpecificOrganization(int charIndex, out short reasonKey, OrganizationInfo organizationInfo)
		{
			reasonKey = -1;
			OrganizationItem orgConfig = Organization.Instance.GetItem(organizationInfo.OrgTemplateId);
			bool flag = orgConfig == null;
			sbyte result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool orgIsSect = orgConfig.IsSect;
				List<Config.ShortList> freeCondition;
				List<Config.ShortList> baseLevel;
				List<Config.ShortList> baseCondition;
				List<Config.ShortList> specialCondition;
				List<Config.ShortList> specialLevel;
				switch (charIndex)
				{
				case 0:
					freeCondition = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.ActorSectPunishFreeCondition : SecretInformationProcessor._sectPunishConfig.ActorCityPunishFreeCondition);
					baseLevel = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.ActorSectPunishBase : SecretInformationProcessor._sectPunishConfig.ActorCityPunishBase);
					baseCondition = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.ActorSectPunishCondition : SecretInformationProcessor._sectPunishConfig.ActorCityPunishCondition);
					specialCondition = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.ActorSectPunishSpecialCondition : SecretInformationProcessor._sectPunishConfig.ActorCityPunishSpecialCondition);
					specialLevel = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.ActorSectPunishSpecial : SecretInformationProcessor._sectPunishConfig.ActorCityPunishSpecial);
					break;
				case 1:
					freeCondition = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.ReactorSectPunishFreeCondition : SecretInformationProcessor._sectPunishConfig.ReactorCityPunishFreeCondition);
					baseLevel = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.ReactorSectPunishBase : SecretInformationProcessor._sectPunishConfig.ReactorCityPunishBase);
					baseCondition = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.ReactorSectPunishCondition : SecretInformationProcessor._sectPunishConfig.ReactorCityPunishCondition);
					specialCondition = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.ReactorSectPunishSpecialCondition : SecretInformationProcessor._sectPunishConfig.ReactorCityPunishSpecialCondition);
					specialLevel = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.ReactorSectPunishSpecial : SecretInformationProcessor._sectPunishConfig.ReactorCityPunishSpecial);
					break;
				case 2:
					freeCondition = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.SecactorSectPunishFreeCondition : SecretInformationProcessor._sectPunishConfig.SecactorCityPunishFreeCondition);
					baseLevel = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.SecactorSectPunishBase : SecretInformationProcessor._sectPunishConfig.SecactorCityPunishBase);
					baseCondition = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.SecactorSectPunishCondition : SecretInformationProcessor._sectPunishConfig.SecactorCityPunishCondition);
					specialCondition = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.SecactorSectPunishSpecialCondition : SecretInformationProcessor._sectPunishConfig.SecactorCityPunishSpecialCondition);
					specialLevel = (orgIsSect ? SecretInformationProcessor._sectPunishConfig.SecactorSectPunishSpecial : SecretInformationProcessor._sectPunishConfig.SecactorCityPunishSpecial);
					break;
				default:
					return -1;
				}
				foreach (Config.ShortList freeItem in freeCondition)
				{
					List<short> condition = freeItem.DataList;
					bool flag2 = condition.Count < 1;
					if (!flag2)
					{
						bool flag3 = this.ConditionBoxEntrance(condition, 0, -1, false);
						if (flag3)
						{
							return -1;
						}
					}
				}
				bool flag4 = specialCondition != null && specialCondition.Count > 0;
				if (flag4)
				{
					for (int i = 0; i < specialCondition.Count; i++)
					{
						List<short> condition2 = specialCondition[i].DataList;
						bool flag5 = condition2.Count < 1;
						if (!flag5)
						{
							bool flag6 = this.ConditionBoxEntrance(condition2, 0, -1, false);
							if (flag6)
							{
								bool flag7 = specialLevel.Count > i;
								if (flag7)
								{
									List<short> punish = specialLevel[i].DataList;
									bool flag8 = !punish.CheckIndex(0);
									if (flag8)
									{
										AdaptableLog.Warning("Error in Punish SpecialLevel!", false);
										return -1;
									}
									sbyte curLevel = this.CalcPunishLevelByPunishmentType(punish[0], organizationInfo.SettlementId);
									bool flag9 = curLevel < 0;
									if (!flag9)
									{
										reasonKey = specialLevel[i].DataList[0];
										return this.GetCustomizedPunishmentSeverity(reasonKey, curLevel, organizationInfo.OrgTemplateId);
									}
								}
								else
								{
									AdaptableLog.Warning("Error in Punish SpecialLevel Count!", false);
								}
							}
						}
					}
				}
				bool flag10 = baseCondition != null && baseCondition.Count > 0 && baseLevel != null;
				if (flag10)
				{
					for (int j = 0; j < baseCondition.Count; j++)
					{
						List<short> condition3 = baseCondition[j].DataList;
						bool flag11 = condition3.Count < 1;
						if (!flag11)
						{
							bool flag12 = this.ConditionBoxEntrance(condition3, 0, -1, false);
							if (flag12)
							{
								bool flag13 = baseLevel.Count > j;
								if (flag13)
								{
									List<short> punish2 = baseLevel[j].DataList;
									bool flag14 = !punish2.CheckIndex(0);
									if (flag14)
									{
										AdaptableLog.Warning("Error in Punish BaseLevel!", false);
										return -1;
									}
									sbyte curLevel2 = this.CalcPunishLevelByPunishmentType(punish2[0], organizationInfo.SettlementId);
									bool flag15 = curLevel2 < 0;
									if (flag15)
									{
										return curLevel2;
									}
									bool flag16 = baseLevel.CheckIndex(j);
									if (flag16)
									{
										reasonKey = baseLevel[j].DataList[0];
									}
									return this.GetCustomizedPunishmentSeverity(reasonKey, curLevel2, organizationInfo.OrgTemplateId);
								}
								else
								{
									AdaptableLog.Warning("Error in Base SpecialLevel Count!", false);
								}
							}
						}
					}
				}
				result = -1;
			}
			return result;
		}

		// Token: 0x06005441 RID: 21569 RVA: 0x002E0C94 File Offset: 0x002DEE94
		internal sbyte GetCustomizedPunishmentSeverity(short punishmentTypeTemplateId, sbyte punishmentSeverity, sbyte orgTemplateId)
		{
			bool flag = punishmentTypeTemplateId < 0;
			sbyte result;
			if (flag)
			{
				result = punishmentSeverity;
			}
			else
			{
				Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId);
				bool flag2 = settlement != null;
				if (flag2)
				{
					PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[punishmentTypeTemplateId];
					punishmentSeverity = settlement.GetPunishmentTypeSeverity(punishmentTypeCfg, false);
				}
				result = punishmentSeverity;
			}
			return result;
		}

		// Token: 0x06005442 RID: 21570 RVA: 0x002E0CE4 File Offset: 0x002DEEE4
		internal sbyte CalcPunishLevelByPunishmentType(short punishmentTypeTemplateId, short settlementId)
		{
			PunishmentTypeItem punishment = PunishmentType.Instance.GetItem(punishmentTypeTemplateId);
			bool flag = punishment != null && settlementId >= 0;
			sbyte result;
			if (flag)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				OrganizationItem orgConfig = Organization.Instance.GetItem(settlement.GetOrgTemplateId());
				result = punishment.GetSeverity(DomainManager.Map.GetStateTemplateIdByAreaId(settlement.GetLocation().AreaId), orgConfig != null && orgConfig.IsSect, false);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x06005443 RID: 21571 RVA: 0x002E0D60 File Offset: 0x002DEF60
		public sbyte CalSectPunishLevel_WithActorIndex(int charIndex, out short reasonKey, bool calRealLevel = false)
		{
			reasonKey = -1;
			bool flag = charIndex < 0 || charIndex > 2 || charIndex >= SecretInformationProcessor.ArgList.Count;
			sbyte result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				int actorId = SecretInformationProcessor.ArgList[charIndex];
				bool flag2 = actorId == -1;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					OrganizationInfo organization = this.GetSectInfoSafe(actorId);
					if (!calRealLevel)
					{
						int orgIndex = -1;
						bool flag4;
						bool flag3 = !this.CheckCanBePunished(actorId, ref orgIndex, out flag4);
						if (flag3)
						{
							return -1;
						}
					}
					result = this.CalcSectPunishLevelWithSpecificOrganization(charIndex, out reasonKey, organization);
				}
			}
			return result;
		}

		// Token: 0x06005444 RID: 21572 RVA: 0x002E0DEC File Offset: 0x002DEFEC
		public sbyte CalcTaiwuPunishLevel(int taiwuCharId, out short reasonKey, out OrganizationInfo organizationInfo)
		{
			reasonKey = -1;
			organizationInfo = OrganizationInfo.None;
			int taiwuCharIndex = -1;
			for (int i = 0; i <= 2; i++)
			{
				bool flag = SecretInformationProcessor.ArgList.CheckIndex(i);
				if (flag)
				{
					int charId = SecretInformationProcessor.ArgList[i];
					bool flag2 = taiwuCharId == charId;
					if (flag2)
					{
						taiwuCharIndex = i;
					}
				}
			}
			bool flag3 = taiwuCharIndex < 0;
			sbyte result;
			if (flag3)
			{
				result = -1;
			}
			else
			{
				List<Config.ShortList> freeCondition;
				List<Config.ShortList> baseLevel;
				List<Config.ShortList> baseCondition;
				List<Config.ShortList> specialCondition;
				List<Config.ShortList> specialLevel;
				switch (taiwuCharIndex)
				{
				case 0:
					freeCondition = SecretInformationProcessor._sectPunishConfig.ActorTaiwuPunishFreeCondition;
					baseLevel = SecretInformationProcessor._sectPunishConfig.ActorTaiwuPunishBase;
					baseCondition = SecretInformationProcessor._sectPunishConfig.ActorTaiwuPunishCondition;
					specialCondition = SecretInformationProcessor._sectPunishConfig.ActorTaiwuPunishSpecialCondition;
					specialLevel = SecretInformationProcessor._sectPunishConfig.ActorTaiwuPunishSpecial;
					break;
				case 1:
					freeCondition = SecretInformationProcessor._sectPunishConfig.ReactorTaiwuPunishFreeCondition;
					baseLevel = SecretInformationProcessor._sectPunishConfig.ReactorTaiwuPunishBase;
					baseCondition = SecretInformationProcessor._sectPunishConfig.ReactorTaiwuPunishCondition;
					specialCondition = SecretInformationProcessor._sectPunishConfig.ReactorTaiwuPunishSpecialCondition;
					specialLevel = SecretInformationProcessor._sectPunishConfig.ReactorTaiwuPunishSpecial;
					break;
				case 2:
					freeCondition = SecretInformationProcessor._sectPunishConfig.SecactorTaiwuPunishFreeCondition;
					baseLevel = SecretInformationProcessor._sectPunishConfig.SecactorTaiwuPunishBase;
					baseCondition = SecretInformationProcessor._sectPunishConfig.SecactorTaiwuPunishCondition;
					specialCondition = SecretInformationProcessor._sectPunishConfig.SecactorTaiwuPunishSpecialCondition;
					specialLevel = SecretInformationProcessor._sectPunishConfig.SecactorTaiwuPunishSpecial;
					break;
				default:
					return -1;
				}
				for (int charIndex = 0; charIndex < SecretInformationProcessor.ArgList.Count; charIndex++)
				{
					int charId2 = SecretInformationProcessor.ArgList[charIndex];
					bool flag4 = charId2 == taiwuCharId;
					if (!flag4)
					{
						organizationInfo = this.GetSectInfoSafe(charId2);
						bool flag5 = organizationInfo.OrgTemplateId < 0;
						if (flag5)
						{
							DeadCharacter deadCharacter;
							bool flag6 = DomainManager.Character.TryGetDeadCharacter(charId2, out deadCharacter);
							if (flag6)
							{
								organizationInfo = deadCharacter.OrganizationInfo;
							}
							else
							{
								GameData.Domains.Character.Character character;
								bool flag7 = DomainManager.Character.TryGetElement_Objects(charId2, out character);
								if (flag7)
								{
									organizationInfo = character.GetOrganizationInfo();
								}
							}
						}
						OrganizationItem orgConfig = Organization.Instance.GetItem(organizationInfo.OrgTemplateId);
						bool flag8 = orgConfig == null;
						if (!flag8)
						{
							bool isSect = orgConfig.IsSect;
							if (isSect)
							{
								Location settlementLocation = DomainManager.Organization.GetSettlement(organizationInfo.SettlementId).GetLocation();
								bool flag9 = settlementLocation.IsValid();
								if (flag9)
								{
									sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(settlementLocation.AreaId);
									MapStateItem stateCfg = MapState.Instance[stateTemplateId];
									MapAreaItem mainAreaCfg = MapArea.Instance.GetItem((short)stateCfg.MainAreaID);
									Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(mainAreaCfg.OrganizationId[0]);
									bool flag10 = settlement != null;
									if (flag10)
									{
										orgConfig = Organization.Instance.GetItem(settlement.GetOrgTemplateId());
									}
								}
							}
							bool orgIsSect = orgConfig.IsSect;
							foreach (Config.ShortList freeItem in freeCondition)
							{
								List<short> condition = freeItem.DataList;
								bool flag11 = condition.Count < 1;
								if (!flag11)
								{
									bool flag12 = this.ConditionBoxEntrance(condition, 0, -1, false);
									if (flag12)
									{
										return -1;
									}
								}
							}
							bool flag13 = orgIsSect;
							if (flag13)
							{
								bool flag14 = specialCondition.Count > 0;
								if (flag14)
								{
									for (int j = 0; j < specialCondition.Count; j++)
									{
										List<short> condition2 = specialCondition[j].DataList;
										bool flag15 = condition2.Count < 1;
										if (!flag15)
										{
											bool flag16 = this.ConditionBoxEntrance(condition2, 0, -1, false);
											if (flag16)
											{
												bool flag17 = specialLevel.Count > j;
												if (flag17)
												{
													List<short> punish = specialLevel[j].DataList;
													bool flag18 = !punish.CheckIndex(0);
													if (flag18)
													{
														AdaptableLog.Warning("Error in Punish SpecialLevel!", false);
														return -1;
													}
													sbyte curLevel = this.CalcPunishLevelByPunishmentType(punish[0], organizationInfo.SettlementId);
													bool flag19 = curLevel < 0;
													if (flag19)
													{
														return curLevel;
													}
													reasonKey = specialLevel[j].DataList[0];
													return this.GetCustomizedPunishmentSeverity(reasonKey, curLevel, organizationInfo.OrgTemplateId);
												}
												else
												{
													AdaptableLog.Warning("Error in Punish SpecialLevel Count!", false);
												}
											}
										}
									}
								}
							}
							bool flag20 = baseCondition != null && baseCondition.Count > 0;
							if (flag20)
							{
								for (int k = 0; k < baseCondition.Count; k++)
								{
									List<short> condition3 = baseCondition[k].DataList;
									bool flag21 = condition3.Count < 1;
									if (!flag21)
									{
										bool flag22 = this.ConditionBoxEntrance(condition3, 0, -1, false);
										if (flag22)
										{
											bool flag23 = baseLevel.Count > k;
											if (flag23)
											{
												List<short> punish2 = baseLevel[k].DataList;
												bool flag24 = !punish2.CheckIndex(0);
												if (flag24)
												{
													AdaptableLog.Warning("Error in Punish BaseLevel!", false);
													return -1;
												}
												sbyte curLevel2 = this.CalcPunishLevelByPunishmentType(punish2[0], organizationInfo.SettlementId);
												bool flag25 = curLevel2 < 0;
												if (flag25)
												{
													return curLevel2;
												}
												bool flag26 = baseLevel.CheckIndex(k);
												if (flag26)
												{
													reasonKey = baseLevel[k].DataList[0];
												}
												return this.GetCustomizedPunishmentSeverity(reasonKey, curLevel2, organizationInfo.OrgTemplateId);
											}
										}
									}
								}
							}
						}
					}
				}
				result = -1;
			}
			return result;
		}

		// Token: 0x06005445 RID: 21573 RVA: 0x002E1388 File Offset: 0x002DF588
		public bool CheckCanBePunished(int actorId, ref int orgIndex, out bool isSect)
		{
			isSect = false;
			GameData.Domains.Character.Character actorChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(actorId, out actorChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !InformationDomain.CheckTuringTest(actorChar);
				if (flag2)
				{
					result = false;
				}
				else
				{
					OrganizationInfo curActorOrgInfo = actorChar.GetOrganizationInfo();
					bool flag3 = curActorOrgInfo.OrgTemplateId < 0;
					if (flag3)
					{
						result = false;
					}
					else
					{
						OrganizationItem curActorOrgConfig = Organization.Instance[curActorOrgInfo.OrgTemplateId];
						OrganizationInfo oriActorOrgInfo = this.GetSectInfoSafe(actorId);
						bool flag4 = oriActorOrgInfo.OrgTemplateId == -1;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool isSect2 = curActorOrgConfig.IsSect;
							if (isSect2)
							{
								isSect = true;
								orgIndex = (int)(oriActorOrgInfo.OrgTemplateId - 1);
							}
							else
							{
								orgIndex = (int)(oriActorOrgInfo.OrgTemplateId - 21);
							}
							bool flag5 = curActorOrgInfo.OrgTemplateId == oriActorOrgInfo.OrgTemplateId;
							result = flag5;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005446 RID: 21574 RVA: 0x002E1460 File Offset: 0x002DF660
		internal int GetArgIndexByRelationConfigDefKey(short relationDefKey)
		{
			if (!true)
			{
			}
			int result;
			switch (relationDefKey)
			{
			case -1:
				result = -1;
				goto IL_7F;
			case 0:
				result = -1;
				goto IL_7F;
			case 1:
				result = 0;
				goto IL_7F;
			case 2:
			case 3:
			case 5:
			case 6:
				break;
			case 4:
				result = 1;
				goto IL_7F;
			case 7:
				result = 2;
				goto IL_7F;
			default:
				if (relationDefKey == 19)
				{
					result = 4;
					goto IL_7F;
				}
				break;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 1);
			defaultInterpolatedStringHandler.AppendLiteral("unsupported relation reference in secret fame condition: ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(relationDefKey);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			IL_7F:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06005447 RID: 21575 RVA: 0x002E14F8 File Offset: 0x002DF6F8
		public bool ConditionBoxEntrance(List<short> conditionItem, int conditionKeyIndex = 0, int extraId = -1, bool convertRelationDefKeyToArgIndex = false)
		{
			bool flag = conditionItem.Count <= conditionKeyIndex;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short conditionKey = conditionItem[conditionKeyIndex];
				short num = conditionKey;
				short num2 = num;
				if (num2 >= 0)
				{
					if (num2 != 0)
					{
						List<int> charIdList = new List<int>();
						List<int> curArgList = new List<int>(SecretInformationProcessor.ArgList);
						bool flag2 = curArgList.Count > 3;
						if (flag2)
						{
							curArgList[3] = extraId;
						}
						for (int i = conditionKeyIndex + 1; i < conditionItem.Count; i++)
						{
							int currentCharIndex = (int)conditionItem[i];
							if (convertRelationDefKeyToArgIndex)
							{
								currentCharIndex = this.GetArgIndexByRelationConfigDefKey((short)currentCharIndex);
							}
							charIdList.Add((currentCharIndex >= 0 && currentCharIndex < curArgList.Count) ? curArgList[currentCharIndex] : currentCharIndex);
						}
						result = this.ConditionBox(conditionKey, (charIdList.Count > 0) ? charIdList[0] : -1, (charIdList.Count > 1) ? charIdList[1] : -1, (charIdList.Count > 2) ? charIdList[2] : -1, (charIdList.Count > 3) ? charIdList[3] : -1);
					}
					else
					{
						result = true;
					}
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x002E1628 File Offset: 0x002DF828
		public bool ConditionBox(short conditionKey, int actorId = -1, int reactorId = -1, int secactorId = -1, int extraId = -1)
		{
			SecretInformationSpecialConditionItem condition = SecretInformationSpecialCondition.Instance.GetItem(conditionKey);
			bool flag = condition == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ESecretInformationSpecialConditionCalculate calculate = condition.Calculate;
				if (!true)
				{
				}
				bool flag2;
				switch (calculate)
				{
				case ESecretInformationSpecialConditionCalculate.SameSect:
					flag2 = this.ConditionSameSect(actorId, reactorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.SectJustice:
					flag2 = this.ConditionSectJustice(actorId, reactorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.SectBecomeEnemy:
					flag2 = this.ConditionSectBecomeEnemy(actorId, reactorId, secactorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.ForbidMarriage:
					flag2 = this.ConditionForbidMarriage(actorId, reactorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.IsPublished:
					flag2 = this.ConditionIsPublished();
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.IsRevealed:
					flag2 = this.ConditionIsRevealed(actorId, reactorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.IsRevealedSingle:
					flag2 = this.ConditionIsRevealedSingle(actorId, reactorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.HasCouple:
					flag2 = this.ConditionHasCouple(actorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.NotFame:
					flag2 = this.ConditionNotFame(actorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.IsMonk:
					flag2 = this.ConditionIsMonk(actorId, reactorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.ForbidWine:
					flag2 = this.ConditionForbidWine(actorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.ForbidPoison:
					flag2 = this.ConditionForbidPoison(actorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.ForbidItem:
					flag2 = this.ConditionForbidItem(actorId, reactorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.HasLove:
					flag2 = this.ConditionHasLove(actorId, reactorId, secactorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.HasRelation:
					flag2 = this.ConditionHasRelation(actorId, reactorId, secactorId, extraId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.IsKidnapped:
					flag2 = this.ConditionIsKidnapped(actorId, reactorId, secactorId);
					goto IL_2A9;
				case ESecretInformationSpecialConditionCalculate.TaiwuFight:
				case ESecretInformationSpecialConditionCalculate.TaiwuSteal:
				case ESecretInformationSpecialConditionCalculate.TaiwuScam:
				case ESecretInformationSpecialConditionCalculate.TaiwuRob:
				case ESecretInformationSpecialConditionCalculate.TaiwuRescueEscape:
				case ESecretInformationSpecialConditionCalculate.CharRescueEscape:
					break;
				case ESecretInformationSpecialConditionCalculate.CompareCombatPoint:
					flag2 = this.ConditionCompareCombatPoint(actorId, reactorId);
					goto IL_2A9;
				default:
					switch (calculate)
					{
					case ESecretInformationSpecialConditionCalculate.ActorAlive:
						flag2 = this.ConditionActorAlive(actorId, reactorId);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.CasualtyInSect:
						flag2 = this.ConditionCasualtyInSect(actorId);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.KillFameLine:
						flag2 = this.ConditionKillFameLine(actorId, condition.CalcFameLine);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.KidnapFameLine:
						flag2 = this.ConditionKidnapFameLine(actorId, condition.CalcFameLine);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.AlliedSectMember:
						flag2 = this.ConditionAlliedSectMember(actorId, condition.CalcOrganization);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.BeLoverSectMember:
						flag2 = this.ConditionBeLoverSectMember(actorId, condition.CalcOrganization);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.BeKyodaiSectMember:
						flag2 = this.ConditionBeKyodaiSectMember(actorId, condition.CalcOrganization);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.GainParentSectMember:
						flag2 = this.ConditionGainParentSectMember(actorId, condition.CalcOrganization);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.GainChildSectMember:
						flag2 = this.ConditionGainChildSectMember(actorId, condition.CalcOrganization);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.DateSectMember:
						flag2 = this.ConditionDateSectMember(actorId, condition.CalcOrganization);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.ReFoundChildSectMember:
						flag2 = this.ConditionReFoundChildSectMember(actorId, condition.CalcOrganization);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.BanSexualMate:
						flag2 = this.ConditionBanSexualMate(actorId, condition.CalcSexualMateCase, condition.CalcSexualMateRule, condition.CalcSectRule);
						goto IL_2A9;
					case ESecretInformationSpecialConditionCalculate.BanEating:
						flag2 = this.ConditionBanEating(condition.CalcSectRule);
						goto IL_2A9;
					}
					break;
				}
				flag2 = false;
				IL_2A9:
				if (!true)
				{
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x06005449 RID: 21577 RVA: 0x002E18E8 File Offset: 0x002DFAE8
		private bool ConditionSameSect(int actorId, int reactorId)
		{
			bool flag = actorId == -1 || reactorId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				OrganizationInfo actorSectInfo = this.GetSectInfoSafe(actorId);
				bool flag2 = actorSectInfo.OrgTemplateId == -1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					OrganizationItem actorOrgConfig = Organization.Instance[actorSectInfo.OrgTemplateId];
					bool flag3 = !actorOrgConfig.IsSect;
					if (flag3)
					{
						result = false;
					}
					else
					{
						OrganizationInfo reactorSectInfo = this.GetSectInfoSafe(reactorId);
						result = (reactorSectInfo.OrgTemplateId == actorSectInfo.OrgTemplateId);
					}
				}
			}
			return result;
		}

		// Token: 0x0600544A RID: 21578 RVA: 0x002E1968 File Offset: 0x002DFB68
		private bool ConditionSectJustice(int actorId, int reactorId)
		{
			bool flag = actorId == -1 || reactorId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				OrganizationInfo oriActorOrgInfo = this.GetSectInfoSafe(actorId);
				bool flag2 = oriActorOrgInfo.OrgTemplateId == -1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = this.GetSectInfoSafe(reactorId).SettlementId == oriActorOrgInfo.SettlementId;
					if (flag3)
					{
						result = false;
					}
					else
					{
						OrganizationItem oriActorOrgConfig = Organization.Instance[oriActorOrgInfo.OrgTemplateId];
						bool flag4 = !oriActorOrgConfig.IsSect;
						if (flag4)
						{
							result = false;
						}
						else
						{
							sbyte fameTypeOfReactor = this.GetFameTypeSafe(reactorId);
							sbyte goodness = oriActorOrgConfig.Goodness;
							bool flag5 = (goodness == -1 && fameTypeOfReactor > 3) || (goodness == 1 && fameTypeOfReactor < 3 && fameTypeOfReactor >= 0);
							bool flag6 = flag5;
							if (!flag6)
							{
								bool flag7 = goodness == 0;
								bool flag8 = flag7;
								if (flag8)
								{
									bool flag9 = fameTypeOfReactor == -2 || fameTypeOfReactor == 3;
									flag8 = flag9;
								}
								flag6 = flag8;
							}
							result = flag6;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600544B RID: 21579 RVA: 0x002E1A64 File Offset: 0x002DFC64
		private bool ConditionSectBecomeEnemy(int actorId, int reactorId, int seactorId)
		{
			bool actorResult = false;
			bool reactorResult = false;
			bool flag = actorId != -1 && reactorId != -1;
			if (flag)
			{
				OrganizationInfo actorSectInfo = this.GetSectInfoSafe(actorId);
				OrganizationInfo reactorSectInfo = this.GetSectInfoSafe(reactorId);
				bool flag2 = actorSectInfo.OrgTemplateId == -1 || reactorSectInfo.OrgTemplateId == -1;
				actorResult = (!flag2 && DomainManager.Organization.GetSectFavorability(actorSectInfo.OrgTemplateId, reactorSectInfo.OrgTemplateId) == -1);
			}
			bool flag3 = seactorId != -1 && reactorId != -1;
			if (flag3)
			{
				OrganizationInfo secactorSectInfo = this.GetSectInfoSafe(seactorId);
				OrganizationInfo reactorSectInfo2 = this.GetSectInfoSafe(reactorId);
				bool flag4 = secactorSectInfo.OrgTemplateId == -1 || reactorSectInfo2.OrgTemplateId == -1;
				reactorResult = (!flag4 && DomainManager.Organization.GetSectFavorability(reactorSectInfo2.OrgTemplateId, secactorSectInfo.OrgTemplateId) == -1);
			}
			return actorResult || reactorResult;
		}

		// Token: 0x0600544C RID: 21580 RVA: 0x002E1B44 File Offset: 0x002DFD44
		private bool ConditionForbidMarriage(int actorId, int reactorId = -1)
		{
			bool actorResult = false;
			bool reactorResult = false;
			bool flag = actorId != -1;
			if (flag)
			{
				OrganizationInfo actorSectInfo = this.GetSectInfoSafe(actorId);
				bool flag2 = actorSectInfo.OrgTemplateId != -1;
				if (flag2)
				{
					actorResult = (OrganizationMember.Instance[Organization.Instance[actorSectInfo.OrgTemplateId].Members[(int)actorSectInfo.Grade]].ChildGrade < 0 || this.ConditionIsMonk(actorId, -1));
				}
			}
			bool flag3 = reactorId != -1 && reactorId != actorId;
			if (flag3)
			{
				OrganizationInfo reactorSectInfo = this.GetSectInfoSafe(reactorId);
				bool flag4 = reactorSectInfo.OrgTemplateId != -1;
				if (flag4)
				{
					reactorResult = (OrganizationMember.Instance[Organization.Instance[reactorSectInfo.OrgTemplateId].Members[(int)reactorSectInfo.Grade]].ChildGrade < 0 || this.ConditionIsMonk(reactorId, -1));
				}
			}
			return actorResult || reactorResult;
		}

		// Token: 0x0600544D RID: 21581 RVA: 0x002E1C30 File Offset: 0x002DFE30
		private bool ConditionIsPublished()
		{
			int relatedId = SecretInformationProcessor._metaDataRef.GetRelevanceSecretInformationMetaDataId();
			bool flag = relatedId == -1;
			return !flag && DomainManager.Information.IsSecretInformationInBroadcast(relatedId);
		}

		// Token: 0x0600544E RID: 21582 RVA: 0x002E1C64 File Offset: 0x002DFE64
		private bool ConditionIsRevealed(int actorId, int reactorId = -1)
		{
			bool flag = actorId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = reactorId == -1;
				if (flag2)
				{
					result = !this.HasAnySpouse(actorId, false, true);
				}
				else
				{
					sbyte actorGender = -1;
					sbyte reactorGender = -1;
					GameData.Domains.Character.Character actorChar;
					bool flag3 = DomainManager.Character.TryGetElement_Objects(actorId, out actorChar);
					if (flag3)
					{
						actorGender = actorChar.GetGender();
					}
					else
					{
						DeadCharacter actorCharDead = DomainManager.Character.TryGetDeadCharacter(actorId);
						bool flag4 = actorCharDead != null;
						if (flag4)
						{
							actorGender = actorCharDead.Gender;
						}
					}
					GameData.Domains.Character.Character reactorChar;
					bool flag5 = DomainManager.Character.TryGetElement_Objects(reactorId, out reactorChar);
					if (flag5)
					{
						reactorGender = reactorChar.GetGender();
					}
					else
					{
						DeadCharacter reactorCharDead = DomainManager.Character.TryGetDeadCharacter(reactorId);
						bool flag6 = reactorCharDead != null;
						if (flag6)
						{
							actorGender = reactorCharDead.Gender;
						}
					}
					HashSet<int> relation = this.GetSecretInformationRelatedCharactersOfSpecialRelation(actorId, SecretInformationProcessor.RelationIndex.Revel, true, true, true);
					HashSet<int> relation2 = this.GetSecretInformationRelatedCharactersOfSpecialRelation(reactorId, SecretInformationProcessor.RelationIndex.Revel, true, true, true);
					result = (actorGender == reactorGender || relation.Contains(reactorId) || relation2.Contains(actorId));
				}
			}
			return result;
		}

		// Token: 0x0600544F RID: 21583 RVA: 0x002E1D68 File Offset: 0x002DFF68
		private bool ConditionIsRevealedSingle(int actorId, int reactorId = -1)
		{
			return !this.HasAnySpouse(actorId, false, true) || !this.HasAnySpouse(reactorId, false, true);
		}

		// Token: 0x06005450 RID: 21584 RVA: 0x002E1D94 File Offset: 0x002DFF94
		private bool ConditionHasCouple(int actorId)
		{
			return this.HasAnySpouse(actorId, true, false);
		}

		// Token: 0x06005451 RID: 21585 RVA: 0x002E1DB0 File Offset: 0x002DFFB0
		private bool ConditionHasLove(int actorId, int reactorId, int secactorId = -1)
		{
			bool flag = actorId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				HashSet<int> relation = this.GetSecretInformationRelatedCharactersOfSpecialRelation(actorId, SecretInformationProcessor.RelationIndex.Love, true, true, true);
				bool reactorResult = relation.Contains(reactorId);
				bool secactorResult = relation.Contains(secactorId);
				result = (reactorResult || secactorResult);
			}
			return result;
		}

		// Token: 0x06005452 RID: 21586 RVA: 0x002E1DF4 File Offset: 0x002DFFF4
		private bool HasAnySpouse(int actorId, bool includeLover = false, bool includeActors = false)
		{
			bool flag = actorId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<SecretInformationRelationshipType> relation = includeLover ? SecretInformationProcessor.RelationIndex.Love : SecretInformationProcessor.RelationIndex.Spouse;
				bool flag2 = SecretInformationProcessor.ActorIdList.Contains(actorId);
				if (flag2)
				{
					HashSet<int> relatedCharId = this.GetSecretInformationRelatedCharactersOfSpecialRelation(actorId, relation, true, false, includeActors);
					result = (relatedCharId.Count != 0);
				}
				else
				{
					bool flag3 = !includeActors;
					if (flag3)
					{
						result = false;
					}
					else
					{
						foreach (int character in SecretInformationProcessor.ActorIdList)
						{
							bool flag4 = this.Check_HasSpecialRelations(character, actorId, relation);
							if (flag4)
							{
								return true;
							}
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06005453 RID: 21587 RVA: 0x002E1EB8 File Offset: 0x002E00B8
		private bool ConditionNotFame(int actorId)
		{
			bool flag = actorId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte fameType = this.GetFameTypeSafe(actorId);
				bool flag2 = fameType == -1;
				result = (!flag2 && fameType < 3 && fameType >= 0);
			}
			return result;
		}

		// Token: 0x06005454 RID: 21588 RVA: 0x002E1EF8 File Offset: 0x002E00F8
		private bool ConditionIsMonk(int actorId, int reactorId = -1)
		{
			bool actorResult = false;
			bool reactorResult = false;
			bool flag = actorId != -1;
			if (flag)
			{
				actorResult = (this.GetMonkTypeSafe(actorId) > 0);
			}
			bool flag2 = reactorId != -1;
			if (flag2)
			{
				reactorResult = (this.GetMonkTypeSafe(reactorId) > 0);
			}
			return actorResult || reactorResult;
		}

		// Token: 0x06005455 RID: 21589 RVA: 0x002E1F40 File Offset: 0x002E0140
		private bool ConditionForbidWine(int actorId)
		{
			bool flag = actorId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				OrganizationInfo oriActorOrgInfo = this.GetSectInfoSafe(actorId);
				bool flag2 = oriActorOrgInfo.OrgTemplateId == -1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					OrganizationItem oriActorOrgConfig = Organization.Instance[oriActorOrgInfo.OrgTemplateId];
					result = oriActorOrgConfig.NoDrinking;
				}
			}
			return result;
		}

		// Token: 0x06005456 RID: 21590 RVA: 0x002E1F90 File Offset: 0x002E0190
		private bool ConditionForbidPoison(int actorId)
		{
			bool flag = actorId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				OrganizationInfo oriActorOrgInfo = this.GetSectInfoSafe(actorId);
				bool flag2 = oriActorOrgInfo.OrgTemplateId == -1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					OrganizationItem oriActorOrgConfig = Organization.Instance[oriActorOrgInfo.OrgTemplateId];
					result = !oriActorOrgConfig.AllowPoisoning;
				}
			}
			return result;
		}

		// Token: 0x06005457 RID: 21591 RVA: 0x002E1FE4 File Offset: 0x002E01E4
		private bool ConditionForbidItem(int actorId, int itemType)
		{
			bool flag = actorId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				OrganizationInfo oriActorOrgInfo = this.GetSectInfoSafe(actorId);
				bool flag2 = oriActorOrgInfo.OrgTemplateId == -1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					OrganizationItem oriActorOrgConfig = Organization.Instance[oriActorOrgInfo.OrgTemplateId];
					bool flag3 = itemType == 9;
					if (flag3)
					{
						result = oriActorOrgConfig.NoDrinking;
					}
					else
					{
						bool flag4 = itemType == 7;
						result = (flag4 && oriActorOrgConfig.NoMeatEating);
					}
				}
			}
			return result;
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x002E2058 File Offset: 0x002E0258
		private bool ConditionIsKidnapped(int actorId, int reactorId, int extra = -1)
		{
			bool flag = actorId == -1 || reactorId == -1;
			bool result2;
			if (flag)
			{
				result2 = false;
			}
			else
			{
				GameData.Domains.Character.Character actor;
				GameData.Domains.Character.Character reactor;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(actorId, out actor) || !DomainManager.Character.TryGetElement_Objects(reactorId, out reactor);
				if (flag2)
				{
					result2 = (extra != -1);
				}
				else
				{
					bool result = actor.GetKidnapperId() == reactorId;
					result2 = ((extra == -1) ? result : (!result));
				}
			}
			return result2;
		}

		// Token: 0x06005459 RID: 21593 RVA: 0x002E20C8 File Offset: 0x002E02C8
		private bool ConditionCompareCombatPoint(int actorId, int reactorId)
		{
			bool flag = actorId == -1 || reactorId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character actor;
				GameData.Domains.Character.Character reactor;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(actorId, out actor) || !DomainManager.Character.TryGetElement_Objects(reactorId, out reactor);
				result = (!flag2 && actor.GetCombatPower() >= reactor.GetCombatPower());
			}
			return result;
		}

		// Token: 0x0600545A RID: 21594 RVA: 0x002E212C File Offset: 0x002E032C
		private bool ConditionActorAlive(int actorId, int extraId = -1)
		{
			bool flag = actorId == extraId;
			GameData.Domains.Character.Character character;
			return !flag && DomainManager.Character.TryGetElement_Objects(actorId, out character);
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x002E2158 File Offset: 0x002E0358
		private bool ConditionCasualtyInSect(int charId)
		{
			GameData.Domains.Character.Character character;
			bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character);
			bool result;
			if (flag)
			{
				OrganizationInfo orgInfo = character.GetOrganizationInfo();
				result = Organization.Instance.GetItem(orgInfo.OrgTemplateId).IsSect;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x002E219C File Offset: 0x002E039C
		private bool ConditionHasRelation(int actorId, int reactorId, int relationType, int extra = -1)
		{
			bool flag = actorId == -1 || reactorId == -1;
			bool result2;
			if (flag)
			{
				result2 = false;
			}
			else
			{
				HashSet<SecretInformationRelationshipType> relation = DomainManager.Information.CheckSecretInformationRelationship(reactorId, -1, actorId, -1);
				SecretInformationRelationshipType type;
				switch (relationType)
				{
				case -19:
					return (extra == -1) ? (relation.Count != 0) : (relation.Count == 0);
				case -18:
					type = SecretInformationRelationshipType.Allied;
					break;
				case -17:
					type = SecretInformationRelationshipType.Comrade;
					break;
				case -16:
					type = SecretInformationRelationshipType.Enemy;
					break;
				case -15:
					type = SecretInformationRelationshipType.Adorer;
					break;
				case -14:
					type = SecretInformationRelationshipType.Friend;
					break;
				case -13:
					type = SecretInformationRelationshipType.MentorAndMentee;
					break;
				case -12:
					type = SecretInformationRelationshipType.Lover;
					break;
				case -11:
					type = SecretInformationRelationshipType.HusbandOrWife;
					break;
				case -10:
				{
					bool result0 = relation.Contains(SecretInformationRelationshipType.HusbandOrWife) || relation.Contains(SecretInformationRelationshipType.Lover) || relation.Contains(SecretInformationRelationshipType.Adorer);
					return (extra == -1) ? result0 : (!result0);
				}
				case -9:
					type = SecretInformationRelationshipType.SwornBrotherOrSister;
					break;
				case -8:
					type = SecretInformationRelationshipType.Relative;
					break;
				case -7:
					type = SecretInformationRelationshipType.ActualBloodFather;
					break;
				case -6:
					type = SecretInformationRelationshipType.RevealedIncest;
					break;
				default:
					return extra != -1;
				}
				bool result = relation.Contains(type);
				result2 = ((extra == -1) ? result : (!result));
			}
			return result2;
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x002E22C8 File Offset: 0x002E04C8
		private ESecretInformationSpecialConditionCalcFameLine CalcFameLineByFame(sbyte fameType)
		{
			if (!true)
			{
			}
			ESecretInformationSpecialConditionCalcFameLine result;
			if (fameType < 3)
			{
				if (fameType >= 0)
				{
					result = ESecretInformationSpecialConditionCalcFameLine.Badboy;
					goto IL_21;
				}
			}
			else if (fameType > 3)
			{
				result = ESecretInformationSpecialConditionCalcFameLine.SuperStar;
				goto IL_21;
			}
			result = ESecretInformationSpecialConditionCalcFameLine.Renowned;
			IL_21:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x002E2300 File Offset: 0x002E0500
		private bool ConditionKillFameLine(int actorId, ESecretInformationSpecialConditionCalcFameLine conditionCalcFameLine)
		{
			return this.CalcFameLineByFame(this.GetFameTypeSafe(actorId)) == conditionCalcFameLine;
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x002E2324 File Offset: 0x002E0524
		private bool ConditionKidnapFameLine(int actorId, ESecretInformationSpecialConditionCalcFameLine conditionCalcFameLine)
		{
			return this.CalcFameLineByFame(this.GetFameTypeSafe(actorId)) == conditionCalcFameLine;
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x002E2348 File Offset: 0x002E0548
		private bool ConditionAlliedSectMember(int actorId, short conditionCalcOrganization)
		{
			return (short)this.GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x002E236C File Offset: 0x002E056C
		private bool ConditionBeLoverSectMember(int actorId, short conditionCalcOrganization)
		{
			return (short)this.GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
		}

		// Token: 0x06005462 RID: 21602 RVA: 0x002E2390 File Offset: 0x002E0590
		private bool ConditionBeKyodaiSectMember(int actorId, short conditionCalcOrganization)
		{
			return (short)this.GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x002E23B4 File Offset: 0x002E05B4
		private bool ConditionGainParentSectMember(int actorId, short conditionCalcOrganization)
		{
			return (short)this.GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x002E23D8 File Offset: 0x002E05D8
		private bool ConditionGainChildSectMember(int actorId, short conditionCalcOrganization)
		{
			return (short)this.GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x002E23FC File Offset: 0x002E05FC
		private bool ConditionDateSectMember(int actorId, short conditionCalcOrganization)
		{
			return (short)this.GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x002E2420 File Offset: 0x002E0620
		private bool ConditionReFoundChildSectMember(int actorId, short conditionCalcOrganization)
		{
			return (short)this.GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x002E2444 File Offset: 0x002E0644
		private bool ConditionBanSexualMate(int actorId, ESecretInformationSpecialConditionCalcSexualMateCase conditionCalcSexualMateCase, ESecretInformationSpecialConditionCalcSexualMateRule conditionCalcSexualMateRule, short conditionCalcSectRule)
		{
			short templateId = SecretInformationProcessor._templateId;
			if (!true)
			{
			}
			ESecretInformationSpecialConditionCalcSexualMateCase esecretInformationSpecialConditionCalcSexualMateCase;
			if (templateId <= 34)
			{
				if (templateId - 19 > 4)
				{
					if (templateId != 34)
					{
						goto IL_6C;
					}
					esecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.BeLover;
					goto IL_71;
				}
			}
			else
			{
				if (templateId == 36)
				{
					esecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.BeHusband;
					goto IL_71;
				}
				switch (templateId)
				{
				case 105:
					esecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.SexInvalid;
					goto IL_71;
				case 106:
					esecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.SexNotAllow;
					goto IL_71;
				case 107:
				case 108:
					break;
				case 109:
					esecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.Date;
					goto IL_71;
				default:
					goto IL_6C;
				}
			}
			esecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.GainChild;
			goto IL_71;
			IL_6C:
			esecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.Invalid;
			IL_71:
			if (!true)
			{
			}
			bool flag = conditionCalcSexualMateCase != esecretInformationSpecialConditionCalcSexualMateCase;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				OrganizationInfo orgInfo = this.GetSectInfoSafe(actorId);
				if (!true)
				{
				}
				bool flag2;
				switch (conditionCalcSexualMateRule)
				{
				case ESecretInformationSpecialConditionCalcSexualMateRule.AllMember:
					flag2 = true;
					break;
				case ESecretInformationSpecialConditionCalcSexualMateRule.NoCommonHuman:
					flag2 = this.ConditionIsMonk(actorId, -1);
					break;
				case ESecretInformationSpecialConditionCalcSexualMateRule.GradeHigh:
					flag2 = (orgInfo.Grade >= 6);
					break;
				default:
					flag2 = false;
					break;
				}
				if (!true)
				{
				}
				bool shouldCheck = flag2;
				bool flag3 = !shouldCheck;
				if (flag3)
				{
					result = false;
				}
				else
				{
					Settlement org = DomainManager.Organization.GetSettlementByOrgTemplateId(orgInfo.OrgTemplateId);
					result = (org != null && PunishmentType.Instance[conditionCalcSectRule].GetSeverity(DomainManager.Map.GetStateTemplateIdByAreaId(org.GetLocation().AreaId), Organization.Instance[orgInfo.OrgTemplateId].IsSect, false) >= 0);
				}
			}
			return result;
		}

		// Token: 0x06005468 RID: 21608 RVA: 0x002E2598 File Offset: 0x002E0798
		private bool ConditionBanEating(short conditionCalcSectRule)
		{
			int actorId = SecretInformationProcessor.ArgList.CheckIndex(0) ? SecretInformationProcessor.ArgList[0] : -1;
			int itemType = SecretInformationProcessor.ArgList.CheckIndex(4) ? SecretInformationProcessor.ArgList[4] : -1;
			bool flag = !this.ConditionForbidItem(actorId, itemType);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				OrganizationInfo orgInfo = this.GetSectInfoSafe(actorId);
				Settlement org = DomainManager.Organization.GetSettlementByOrgTemplateId(orgInfo.OrgTemplateId);
				result = (org != null && PunishmentType.Instance[conditionCalcSectRule].GetSeverity(DomainManager.Map.GetStateTemplateIdByAreaId(org.GetLocation().AreaId), Organization.Instance[orgInfo.OrgTemplateId].IsSect, false) >= 0);
			}
			return result;
		}

		// Token: 0x0400167D RID: 5757
		private static readonly List<int> ArgList = new List<int>();

		// Token: 0x0400167E RID: 5758
		private static readonly List<int> ActorIdList = new List<int>();

		// Token: 0x0400167F RID: 5759
		private static readonly Dictionary<int, GameData.Domains.Character.Character> ActiveActorList = new Dictionary<int, GameData.Domains.Character.Character>();

		// Token: 0x04001680 RID: 5760
		private static int _metaDataId = -1;

		// Token: 0x04001681 RID: 5761
		private static short _templateId = -1;

		// Token: 0x04001682 RID: 5762
		private static int _baseFavorOdds = -1;

		// Token: 0x04001683 RID: 5763
		private static Dictionary<GameData.Domains.Character.Character, int> RelatedCharacterIndexList = null;

		// Token: 0x04001684 RID: 5764
		private static SecretInformationMetaData _metaDataRef;

		// Token: 0x04001685 RID: 5765
		private static EventArgBox _secretInfoArgBox = new EventArgBox();

		// Token: 0x04001686 RID: 5766
		private static SecretInformationItem _infoConfig;

		// Token: 0x04001687 RID: 5767
		private static SecretInformationEffectItem _effectConfig;

		// Token: 0x04001688 RID: 5768
		private static SecretInformationSectPunishItem _sectPunishConfig;

		// Token: 0x02000AE4 RID: 2788
		public static class RelationIndex
		{
			// Token: 0x04002D1A RID: 11546
			public static readonly List<SecretInformationRelationshipType> Allied = new List<SecretInformationRelationshipType>
			{
				SecretInformationRelationshipType.Allied
			};

			// Token: 0x04002D1B RID: 11547
			public static readonly List<SecretInformationRelationshipType> Enemy = new List<SecretInformationRelationshipType>
			{
				SecretInformationRelationshipType.Enemy
			};

			// Token: 0x04002D1C RID: 11548
			public static readonly List<SecretInformationRelationshipType> Adorer = new List<SecretInformationRelationshipType>
			{
				SecretInformationRelationshipType.Adorer
			};

			// Token: 0x04002D1D RID: 11549
			public static readonly List<SecretInformationRelationshipType> Revel = new List<SecretInformationRelationshipType>
			{
				SecretInformationRelationshipType.Relative,
				SecretInformationRelationshipType.MentorAndMentee,
				SecretInformationRelationshipType.SwornBrotherOrSister
			};

			// Token: 0x04002D1E RID: 11550
			public static readonly List<SecretInformationRelationshipType> Love = new List<SecretInformationRelationshipType>
			{
				SecretInformationRelationshipType.HusbandOrWife,
				SecretInformationRelationshipType.Lover
			};

			// Token: 0x04002D1F RID: 11551
			public static readonly List<SecretInformationRelationshipType> Spouse = new List<SecretInformationRelationshipType>
			{
				SecretInformationRelationshipType.HusbandOrWife
			};

			// Token: 0x04002D20 RID: 11552
			public static readonly List<SecretInformationRelationshipType> Single = new List<SecretInformationRelationshipType>
			{
				SecretInformationRelationshipType.Adorer,
				SecretInformationRelationshipType.Enemy
			};
		}
	}
}
