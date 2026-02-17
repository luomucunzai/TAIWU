using System;
using System.Collections.Generic;
using Config;
using GameData.DLC;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.FunctionDefinition;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character
{
	// Token: 0x02000816 RID: 2070
	public static class NpcRandomWordsHelper
	{
		// Token: 0x060074AC RID: 29868 RVA: 0x00445A7F File Offset: 0x00443C7F
		private static bool HasRelation(int charId, int relatedCharId, ushort targetRelationType)
		{
			return DomainManager.Character.HasRelation(charId, relatedCharId, targetRelationType);
		}

		// Token: 0x060074AD RID: 29869 RVA: 0x00445A8E File Offset: 0x00443C8E
		private static bool HasBloodGrandParentRelations(int charId, int relatedCharId)
		{
			return DomainManager.Character.HasBloodGrandParentRelations(charId, relatedCharId);
		}

		// Token: 0x060074AE RID: 29870 RVA: 0x00445A9C File Offset: 0x00443C9C
		public static string RandomContent(this ENpcRandomWordsType type, IRandomSource random, Character npcChar, ENpcRandomWordsType subType = ENpcRandomWordsType.Count)
		{
			List<short> range = ObjectPool<List<short>>.Instance.Get();
			List<short> weight = ObjectPool<List<short>>.Instance.Get();
			range.Clear();
			weight.Clear();
			foreach (NpcRandomWordsItem word in ((IEnumerable<NpcRandomWordsItem>)NpcRandomWords.Instance))
			{
				bool flag = !word.IsMatch(type, subType) || !word.IsMatch(npcChar);
				if (!flag)
				{
					range.Add(word.TemplateId);
					weight.Add(word.Weight);
				}
			}
			string content = string.Empty;
			bool flag2 = range.Count > 0;
			if (flag2)
			{
				int index = RandomUtils.GetRandomIndex(weight, random);
				short templateId = range[index];
				NpcRandomWordsItem word2 = NpcRandomWords.Instance[templateId];
				content = word2.RandomContent(random);
			}
			ObjectPool<List<short>>.Instance.Return(range);
			ObjectPool<List<short>>.Instance.Return(weight);
			return content;
		}

		// Token: 0x060074AF RID: 29871 RVA: 0x00445BA8 File Offset: 0x00443DA8
		public static string RandomContent(this NpcRandomWordsItem word, IRandomSource random)
		{
			string words = word.Words;
			short templateId = word.TemplateId;
			bool flag = templateId >= 695 && templateId <= 699;
			if (flag)
			{
				short selectedTaiwuTitle = word.RandomMatchTitle(random);
				Tester.Assert(selectedTaiwuTitle >= 0, "Illegal selectedTaiwuTitle");
				CharacterTitleItem titleItem = CharacterTitle.Instance[selectedTaiwuTitle];
				MiscItem miscItem = Misc.Instance[(int)(titleItem.TemplateId + 208)];
				words = words.GetFormat(miscItem.Name);
			}
			bool flag2 = word.TemplateId == 701;
			if (flag2)
			{
				short selectedTaiwuTitle2 = word.RandomMatchTitle(random);
				Tester.Assert(selectedTaiwuTitle2 >= 0, "Illegal selectedTaiwuTitle");
				CharacterTitleItem titleItem2 = CharacterTitle.Instance[selectedTaiwuTitle2];
				words = words.GetFormat(titleItem2.Name);
			}
			return words;
		}

		// Token: 0x060074B0 RID: 29872 RVA: 0x00445C84 File Offset: 0x00443E84
		private static short RandomMatchTitle(this NpcRandomWordsItem word, IRandomSource random = null)
		{
			List<short> taiwuTitleLimit = word.TaiwuTitleLimit;
			bool flag = taiwuTitleLimit == null || taiwuTitleLimit.Count <= 0;
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
				List<short> titles = ObjectPool<List<short>>.Instance.Get();
				titles.Clear();
				taiwuChar.GetTitles(titles);
				titles.RemoveAll((short x) => !word.TaiwuTitleLimit.Contains(x));
				short titleId = (titles.Count == 0) ? -1 : ((random != null) ? titles.GetRandom(random) : titles[0]);
				ObjectPool<List<short>>.Instance.Return(titles);
				result = titleId;
			}
			return result;
		}

		// Token: 0x060074B1 RID: 29873 RVA: 0x00445D38 File Offset: 0x00443F38
		private static bool IsMatch(this NpcRandomWordsItem word, ENpcRandomWordsType type, ENpcRandomWordsType subType)
		{
			return word.Type == type || word.Type == subType;
		}

		// Token: 0x060074B2 RID: 29874 RVA: 0x00445D60 File Offset: 0x00443F60
		private static bool IsMatch(this NpcRandomWordsItem word, Character npcChar)
		{
			Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = word.SelfGender != -1 && !taiwuChar.CheckGenderMeetsRequirement(word.SelfGender);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = word.TargetGender != -1 && !npcChar.CheckGenderMeetsRequirement(word.TargetGender);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = word.SexualOrientation != -1;
					if (flag3)
					{
						bool flag4 = npcChar.GetCurrAge() < 16 || npcChar.GetGender() != word.TargetGender || !npcChar.CheckSexualOrientationMeetsRequirement(word.SexualOrientation);
						if (flag4)
						{
							return false;
						}
					}
					bool flag5 = !word.CheckAge(taiwuChar, npcChar);
					if (flag5)
					{
						result = false;
					}
					else
					{
						short morality = npcChar.GetMorality();
						bool flag6 = morality < word.BehaviorLimit[0] || morality > word.BehaviorLimit[1];
						if (flag6)
						{
							result = false;
						}
						else
						{
							bool flag7 = !word.CheckFeature(npcChar);
							if (flag7)
							{
								result = false;
							}
							else
							{
								bool flag8 = !word.CheckProperty(npcChar);
								if (flag8)
								{
									result = false;
								}
								else
								{
									short favorToTaiwu = DomainManager.Character.GetFavorability(npcChar.GetId(), taiwuChar.GetId());
									bool flag9 = favorToTaiwu < word.FavorLimit[0] || favorToTaiwu > word.FavorLimit[1];
									if (flag9)
									{
										result = false;
									}
									else
									{
										bool flag10 = !word.CheckOrganization(npcChar);
										if (flag10)
										{
											result = false;
										}
										else
										{
											bool flag11 = !word.CheckRelation(taiwuChar, npcChar);
											if (flag11)
											{
												result = false;
											}
											else
											{
												bool flag12 = !word.CheckSectStoryTaskStatus();
												if (flag12)
												{
													result = false;
												}
												else
												{
													bool flag13 = !word.CheckDlc();
													if (flag13)
													{
														result = false;
													}
													else
													{
														bool flag14 = !word.CheckTitle();
														if (flag14)
														{
															result = false;
														}
														else
														{
															bool flag15 = !word.CheckTaiwuMapState(taiwuChar) || !word.CheckTaiwuLocation(taiwuChar);
															if (flag15)
															{
																result = false;
															}
															else
															{
																bool flag16 = !word.CheckCharacterMapArea(npcChar) || !word.CheckCharacterSettlement(taiwuChar);
																if (flag16)
																{
																	result = false;
																}
																else
																{
																	bool flag17 = !word.CheckTaskInfos();
																	if (flag17)
																	{
																		result = false;
																	}
																	else
																	{
																		sbyte progress = DomainManager.World.GetXiangshuProgress();
																		bool flag18 = progress < word.XiangshuProgressLimit[0] || progress > word.XiangshuProgressLimit[1];
																		result = !flag18;
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060074B3 RID: 29875 RVA: 0x00445FD0 File Offset: 0x004441D0
		private static bool CheckFeature(this NpcRandomWordsItem word, Character npcChar)
		{
			bool flag = word.FeatureLimit.Count > 0;
			bool result;
			if (flag)
			{
				bool featureMatch = false;
				foreach (short featureId in word.FeatureLimit)
				{
					bool flag2 = npcChar.GetFeatureIds().Contains(featureId);
					if (flag2)
					{
						featureMatch = true;
					}
				}
				result = featureMatch;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060074B4 RID: 29876 RVA: 0x00446058 File Offset: 0x00444258
		private static bool CheckProperty(this NpcRandomWordsItem word, Character npcChar)
		{
			short[] propertyLimit = word.PropertyLimit;
			bool flag = propertyLimit != null && propertyLimit.Length > 0;
			bool result;
			if (flag)
			{
				int value = npcChar.GetPropertyValue((ECharacterPropertyReferencedType)word.PropertyLimit[0]);
				result = EventConditions.PerformOperation((int)word.PropertyLimit[1], value, (int)word.PropertyLimit[2]);
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060074B5 RID: 29877 RVA: 0x004460AC File Offset: 0x004442AC
		private static bool CheckAge(this NpcRandomWordsItem word, Character taiwuChar, Character npcChar)
		{
			bool flag = word.AgeLimit < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				short taiwuAge = taiwuChar.GetActualAge();
				short npcAge = npcChar.GetActualAge();
				short ageLimit = word.AgeLimit;
				if (!true)
				{
				}
				bool flag2;
				if (ageLimit != 0)
				{
					flag2 = (ageLimit == 1 && npcAge <= taiwuAge);
				}
				else
				{
					flag2 = (npcAge > taiwuAge);
				}
				if (!true)
				{
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x060074B6 RID: 29878 RVA: 0x00446114 File Offset: 0x00444314
		private static bool CheckOrganization(this NpcRandomWordsItem word, Character npcChar)
		{
			List<short> organizationGradeLimit = word.OrganizationGradeLimit;
			bool flag = organizationGradeLimit == null || organizationGradeLimit.Count <= 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				OrganizationMemberItem orgMember = OrganizationDomain.GetOrgMemberConfig(npcChar.GetOrganizationInfo());
				result = word.OrganizationGradeLimit.Contains(orgMember.TemplateId);
			}
			return result;
		}

		// Token: 0x060074B7 RID: 29879 RVA: 0x00446164 File Offset: 0x00444364
		private static bool CheckRelation(this NpcRandomWordsItem word, Character taiwuChar, Character npcChar)
		{
			sbyte relation = word.RelationLimit;
			bool flag = relation < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				int taiwuCharId = taiwuChar.GetId();
				int npcCharId = npcChar.GetId();
				bool loveDlcMatch = false;
				bool flag2 = DlcManager.IsDlcInstalled(2305890UL);
				bool flag4;
				if (flag2)
				{
					bool isPrevious = DomainManager.Extra.IsPreviousTaiwuLover(npcCharId);
					LoveDataItem loveDataItem;
					bool isNow = InteractOfLove.TryGetLoveDataItem(npcCharId, out loveDataItem);
					bool isFirstMeet = isNow && loveDataItem.InteractTime == 0;
					bool flag3 = isFirstMeet && relation != 14;
					if (flag3)
					{
						return false;
					}
					if (!true)
					{
					}
					switch (relation)
					{
					case 14:
						flag4 = (isFirstMeet && !isPrevious);
						break;
					case 15:
						flag4 = (!isFirstMeet && !isPrevious);
						break;
					case 16:
						flag4 = (!isNow && isPrevious);
						break;
					default:
						flag4 = false;
						break;
					}
					if (!true)
					{
					}
					loveDlcMatch = flag4;
				}
				bool adoredAToB = NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 16384);
				bool adoredBToA = NpcRandomWordsHelper.HasRelation(taiwuCharId, npcCharId, 16384);
				if (!true)
				{
				}
				switch (relation)
				{
				case 0:
					flag4 = NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 32768);
					break;
				case 1:
					flag4 = NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 8192);
					break;
				case 2:
					flag4 = NpcRandomWordsHelper.HasBloodGrandParentRelations(npcCharId, taiwuCharId);
					break;
				case 3:
					flag4 = NpcRandomWordsHelper.HasBloodGrandParentRelations(taiwuCharId, npcCharId);
					break;
				case 4:
					flag4 = (NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 128) || NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 16));
					break;
				case 5:
					flag4 = NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 2);
					break;
				case 6:
					flag4 = NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 1024);
					break;
				case 7:
					flag4 = NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 1);
					break;
				case 8:
					flag4 = NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 64);
					break;
				case 9:
					flag4 = NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 8);
					break;
				case 10:
					flag4 = (NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 4) || NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 256) || NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 32));
					break;
				case 11:
					flag4 = NpcRandomWordsHelper.HasRelation(npcCharId, taiwuCharId, 512);
					break;
				case 12:
					flag4 = (adoredAToB != adoredBToA);
					break;
				case 13:
					flag4 = (adoredAToB && adoredBToA);
					break;
				default:
					flag4 = false;
					break;
				}
				if (!true)
				{
				}
				bool normalMatch = flag4;
				result = (loveDlcMatch || normalMatch);
			}
			return result;
		}

		// Token: 0x060074B8 RID: 29880 RVA: 0x004463BC File Offset: 0x004445BC
		private static bool CheckSectStoryTaskStatus(this NpcRandomWordsItem word)
		{
			bool flag = word.SectStoryTaskStatus < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				sbyte sectStoryOrganizationTemplateId = word.SectStoryOrganizationTemplateId;
				bool flag2 = sectStoryOrganizationTemplateId < 1 || sectStoryOrganizationTemplateId >= 16;
				bool flag3 = flag2;
				result = (flag3 || DomainManager.World.GetSectMainStoryTaskStatus(word.SectStoryOrganizationTemplateId) == word.SectStoryTaskStatus);
			}
			return result;
		}

		// Token: 0x060074B9 RID: 29881 RVA: 0x0044641C File Offset: 0x0044461C
		private static bool CheckDlc(this NpcRandomWordsItem word)
		{
			bool flag = word.DlcAppId == 0U;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = !DlcManager.IsDlcInstalled((ulong)word.DlcAppId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = word.DlcAppId == 2764950U;
					result = (flag3 && DomainManager.Extra.GetFiveLoongDictCount(null) > 0);
				}
			}
			return result;
		}

		// Token: 0x060074BA RID: 29882 RVA: 0x0044647C File Offset: 0x0044467C
		private static bool CheckTitle(this NpcRandomWordsItem word)
		{
			List<short> taiwuTitleLimit = word.TaiwuTitleLimit;
			bool flag = taiwuTitleLimit == null || taiwuTitleLimit.Count <= 0;
			return flag || word.RandomMatchTitle(null) >= 0;
		}

		// Token: 0x060074BB RID: 29883 RVA: 0x004464BC File Offset: 0x004446BC
		private static bool CheckTaiwuMapState(this NpcRandomWordsItem word, Character taiwuChar)
		{
			bool flag = word.MapState < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				Location location = taiwuChar.GetLocation();
				bool flag2 = location.AreaId < 0;
				result = (!flag2 && word.MapState == DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId));
			}
			return result;
		}

		// Token: 0x060074BC RID: 29884 RVA: 0x00446510 File Offset: 0x00444710
		private static bool CheckTaiwuLocation(this NpcRandomWordsItem word, Character taiwuChar)
		{
			bool flag = word.Location == ENpcRandomWordsLocation.None;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				Location location = taiwuChar.GetLocation();
				ENpcRandomWordsLocation type = ENpcRandomWordsLocation.Other;
				bool flag2 = MapAreaData.IsBrokenArea(location.AreaId);
				if (flag2)
				{
					type = ENpcRandomWordsLocation.Broken;
				}
				else
				{
					bool flag3 = location == DomainManager.Taiwu.GetTaiwuVillageLocation();
					if (flag3)
					{
						type = ENpcRandomWordsLocation.TaiwuVillage;
					}
					else
					{
						bool flag4 = location.IsValid();
						if (flag4)
						{
							MapBlockData data = DomainManager.Map.GetBlock(location);
							bool flag5 = data.BelongBlockId >= 0;
							if (flag5)
							{
								data = DomainManager.Map.GetBlock(location.AreaId, data.BelongBlockId);
							}
							EMapBlockType blockType = data.BlockType;
							if (!true)
							{
							}
							ENpcRandomWordsLocation enpcRandomWordsLocation;
							switch (blockType)
							{
							case EMapBlockType.City:
								enpcRandomWordsLocation = ENpcRandomWordsLocation.City;
								break;
							case EMapBlockType.Sect:
								enpcRandomWordsLocation = ENpcRandomWordsLocation.None;
								break;
							case EMapBlockType.Town:
								enpcRandomWordsLocation = ENpcRandomWordsLocation.Town;
								break;
							default:
								enpcRandomWordsLocation = ENpcRandomWordsLocation.Other;
								break;
							}
							if (!true)
							{
							}
							type = enpcRandomWordsLocation;
						}
					}
				}
				result = (type == word.Location);
			}
			return result;
		}

		// Token: 0x060074BD RID: 29885 RVA: 0x00446604 File Offset: 0x00444804
		private static bool CheckCharacterMapArea(this NpcRandomWordsItem word, Character character)
		{
			bool flag = word.MapAreaTemplateId < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				Location location = character.GetLocation();
				bool flag2 = location.AreaId < 0;
				result = (!flag2 && DomainManager.Map.GetAreaIdByAreaTemplateId(word.MapAreaTemplateId) == location.AreaId);
			}
			return result;
		}

		// Token: 0x060074BE RID: 29886 RVA: 0x00446658 File Offset: 0x00444858
		private static bool CheckCharacterSettlement(this NpcRandomWordsItem word, Character character)
		{
			bool flag = word.MapAreaTemplateId < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				short areaId = DomainManager.Map.GetAreaIdByAreaTemplateId(word.MapAreaTemplateId);
				OrganizationInfo organizationInfo = character.GetOrganizationInfo();
				Settlement settlement = DomainManager.Organization.GetSettlement(organizationInfo.SettlementId);
				Location settlementLocation = settlement.GetLocation();
				bool flag2 = settlementLocation.AreaId != areaId;
				if (flag2)
				{
					result = false;
				}
				else
				{
					OrganizationItem orgConfig = Organization.Instance[organizationInfo.OrgTemplateId];
					bool flag3 = orgConfig.IsSect != word.IsSectSettlement;
					result = (!flag3 && DomainManager.Map.IsLocationInSettlementInfluenceRange(character.GetLocation(), organizationInfo.SettlementId));
				}
			}
			return result;
		}

		// Token: 0x060074BF RID: 29887 RVA: 0x00446710 File Offset: 0x00444910
		private static bool CheckTaskInfos(this NpcRandomWordsItem word)
		{
			bool flag = word.NeedTaskInfos.Count == 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				for (int i = 0; i < word.NeedTaskInfos.Count; i++)
				{
					bool flag2 = DomainManager.Extra.IsExtraTaskInProgress(word.NeedTaskInfos[i]);
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
