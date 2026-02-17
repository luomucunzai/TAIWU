using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Config;
using GameData.DLC;
using GameData.DLC.FiveLoong;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Relation;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.World;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x02000085 RID: 133
	internal class TaiwuEventTagHandler
	{
		// Token: 0x060018B8 RID: 6328 RVA: 0x001647E8 File Offset: 0x001629E8
		public static string DecodeTag(string targetString, EventArgBox box, TaiwuEvent handlingEvent)
		{
			bool flag = string.IsNullOrEmpty(targetString);
			string result2;
			if (flag)
			{
				result2 = targetString;
			}
			else
			{
				TaiwuEventTagHandler._argBox = box;
				TaiwuEventTagHandler._handlingEvent = handlingEvent;
				string result = TaiwuEventTagHandler.TagRegex.Replace(targetString, delegate(Match tagMatch)
				{
					TaiwuEventTagHandler._pairInfos.Clear();
					string tagString = tagMatch.Value;
					MatchCollection collections = TaiwuEventTagHandler.PairRegex.Matches(tagString);
					foreach (object obj in collections)
					{
						Match i = (Match)obj;
						bool flag2 = i.Groups.Count < 3;
						if (!flag2)
						{
							TaiwuEventTagHandler._pairInfos.Add(i.Groups["Name"].Value, i.Groups["Value"].Value);
						}
					}
					MatchCollection tagNameMatchCollection = TaiwuEventTagHandler.TagNameRegex.Matches(tagString);
					string tagName = string.Empty;
					for (int j = 0; j < tagNameMatchCollection.Count; j++)
					{
						Match tagNameMatchItem = tagNameMatchCollection[j];
						string matchValue = tagNameMatchItem.Groups["TagName"].Value;
						bool flag3 = matchValue != "color";
						if (flag3)
						{
							tagName = matchValue;
							break;
						}
					}
					bool flag4 = string.IsNullOrEmpty(tagName);
					string result3;
					if (flag4)
					{
						result3 = tagString;
					}
					else
					{
						string text = tagName;
						string text2 = text;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						string resultString;
						if (num <= 2565036635U)
						{
							if (num <= 1066741867U)
							{
								if (num <= 526680070U)
								{
									if (num <= 82733161U)
									{
										if (num != 78467396U)
										{
											if (num == 82733161U)
											{
												if (text2 == "SecretInfo")
												{
													resultString = TaiwuEventTagHandler.DecodeSecretInformation();
													goto IL_7A8;
												}
											}
										}
										else if (text2 == "OrganizationMember")
										{
											resultString = TaiwuEventTagHandler.DecodeOrganizationMemberName();
											goto IL_7A8;
										}
									}
									else if (num != 502671050U)
									{
										if (num == 526680070U)
										{
											if (text2 == "Item")
											{
												resultString = TaiwuEventTagHandler.DecodeItem();
												goto IL_7A8;
											}
										}
									}
									else if (text2 == "TemplateCharacter")
									{
										resultString = TaiwuEventTagHandler.DecodeTemplateCharacter();
										goto IL_7A8;
									}
								}
								else if (num <= 781135343U)
								{
									if (num != 598792213U)
									{
										if (num == 781135343U)
										{
											if (text2 == "SwordGrave")
											{
												resultString = TaiwuEventTagHandler.DecodeSwordGrave();
												goto IL_7A8;
											}
										}
									}
									else if (text2 == "Resource")
									{
										resultString = TaiwuEventTagHandler.DecodeResource();
										goto IL_7A8;
									}
								}
								else if (num != 889009852U)
								{
									if (num == 1066741867U)
									{
										if (text2 == "Adventure")
										{
											resultString = TaiwuEventTagHandler.DecodeAdventure();
											goto IL_7A8;
										}
									}
								}
								else if (text2 == "Chicken")
								{
									resultString = TaiwuEventTagHandler.DecodeChickenName();
									goto IL_7A8;
								}
							}
							else if (num <= 1782320988U)
							{
								if (num <= 1432485131U)
								{
									if (num != 1347445242U)
									{
										if (num == 1432485131U)
										{
											if (text2 == "General")
											{
												resultString = TaiwuEventTagHandler.DecodeGeneral();
												goto IL_7A8;
											}
										}
									}
									else if (text2 == "CombatSkill")
									{
										resultString = TaiwuEventTagHandler.DecodeCombatSkill();
										goto IL_7A8;
									}
								}
								else if (num != 1767596533U)
								{
									if (num == 1782320988U)
									{
										if (text2 == "LifeSkillType")
										{
											resultString = TaiwuEventTagHandler.DecodeLifeSkillType();
											goto IL_7A8;
										}
									}
								}
								else if (text2 == "Merchant")
								{
									resultString = TaiwuEventTagHandler.DecodeMerchant();
									goto IL_7A8;
								}
							}
							else if (num <= 2159415579U)
							{
								if (num != 1952591431U)
								{
									if (num == 2159415579U)
									{
										if (text2 == "JiaoNurturance")
										{
											resultString = TaiwuEventTagHandler.DecodeJiaoNurturance();
											goto IL_7A8;
										}
									}
								}
								else if (text2 == "FiveElement")
								{
									resultString = TaiwuEventTagHandler.DecodeFiveElementName();
									goto IL_7A8;
								}
							}
							else if (num != 2219685410U)
							{
								if (num == 2565036635U)
								{
									if (text2 == "SkillBook")
									{
										resultString = TaiwuEventTagHandler.DecodeSkillBook();
										goto IL_7A8;
									}
								}
							}
							else if (text2 == "MapState")
							{
								resultString = TaiwuEventTagHandler.DecodeMapState();
								goto IL_7A8;
							}
						}
						else if (num <= 3631485979U)
						{
							if (num <= 3024739418U)
							{
								if (num <= 2794591594U)
								{
									if (num != 2726860691U)
									{
										if (num == 2794591594U)
										{
											if (text2 == "Settlement")
											{
												resultString = TaiwuEventTagHandler.DecodeSettlement();
												goto IL_7A8;
											}
										}
									}
									else if (text2 == "CricketName")
									{
										resultString = TaiwuEventTagHandler.DecodeCricketName();
										goto IL_7A8;
									}
								}
								else if (num != 2804681632U)
								{
									if (num == 3024739418U)
									{
										if (text2 == "Actor")
										{
											resultString = TaiwuEventTagHandler.DecodeActor();
											goto IL_7A8;
										}
									}
								}
								else if (text2 == "Gender")
								{
									resultString = TaiwuEventTagHandler.DecodeGender();
									goto IL_7A8;
								}
							}
							else if (num <= 3090765637U)
							{
								if (num != 3081494584U)
								{
									if (num == 3090765637U)
									{
										if (text2 == "MapBlockName")
										{
											resultString = TaiwuEventTagHandler.DecodeMapBlockName();
											goto IL_7A8;
										}
									}
								}
								else if (text2 == "ArgBox")
								{
									resultString = TaiwuEventTagHandler.DecodeArgBox();
									goto IL_7A8;
								}
							}
							else if (num != 3163548490U)
							{
								if (num == 3631485979U)
								{
									if (text2 == "PersonalityType")
									{
										resultString = TaiwuEventTagHandler.DecodePersonalityTypeName();
										goto IL_7A8;
									}
								}
							}
							else if (text2 == "NormalInfo")
							{
								resultString = TaiwuEventTagHandler.DecodeNormalInformation();
								goto IL_7A8;
							}
						}
						else if (num <= 3966976176U)
						{
							if (num <= 3910857114U)
							{
								if (num != 3662170534U)
								{
									if (num == 3910857114U)
									{
										if (text2 == "CombatSkillType")
										{
											resultString = TaiwuEventTagHandler.DecodeCombatSkillType();
											goto IL_7A8;
										}
									}
								}
								else if (text2 == "CharacterTitle")
								{
									resultString = TaiwuEventTagHandler.DecodeCharacterTitle();
									goto IL_7A8;
								}
							}
							else if (num != 3947662441U)
							{
								if (num == 3966976176U)
								{
									if (text2 == "Character")
									{
										resultString = TaiwuEventTagHandler.DecodeCharacter();
										goto IL_7A8;
									}
								}
							}
							else if (text2 == "JiaoLoong")
							{
								resultString = TaiwuEventTagHandler.DecodeJiaoLoong();
								goto IL_7A8;
							}
						}
						else if (num <= 4214795775U)
						{
							if (num != 3992405204U)
							{
								if (num == 4214795775U)
								{
									if (text2 == "Profession")
									{
										resultString = TaiwuEventTagHandler.DecodeProfession();
										goto IL_7A8;
									}
								}
							}
							else if (text2 == "LifeSkill")
							{
								resultString = TaiwuEventTagHandler.DecodeLifeSkill();
								goto IL_7A8;
							}
						}
						else if (num != 4265498248U)
						{
							if (num == 4274339234U)
							{
								if (text2 == "CharacterOrActor")
								{
									resultString = TaiwuEventTagHandler.DecodeCharacterOrActor();
									goto IL_7A8;
								}
							}
						}
						else if (text2 == "MapArea")
						{
							resultString = TaiwuEventTagHandler.DecodeMapArea();
							goto IL_7A8;
						}
						resultString = tagString;
						IL_7A8:
						result3 = (resultString ?? TaiwuEventTagHandler.Error("DecodeTag", tagName));
					}
					return result3;
				});
				TaiwuEventTagHandler._argBox = null;
				TaiwuEventTagHandler._handlingEvent = null;
				TaiwuEventTagHandler._pairInfos.Clear();
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x00164858 File Offset: 0x00162A58
		private static string DecodeCricketName()
		{
			string itemKeyString;
			bool flag = TaiwuEventTagHandler._pairInfos.TryGetValue("key", out itemKeyString);
			string result;
			if (flag)
			{
				short colorId = 0;
				short partId = 0;
				bool flag2 = TaiwuEventTagHandler._argBox.Get(itemKeyString + "_colorId", ref colorId) && TaiwuEventTagHandler._argBox.Get(itemKeyString + "_partId", ref partId);
				if (flag2)
				{
					sbyte grade = ItemTemplateHelper.GetCricketGrade(colorId, partId);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 3);
					defaultInterpolatedStringHandler.AppendLiteral("<color=#GradeColor_");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(grade);
					defaultInterpolatedStringHandler.AppendLiteral("><Cricket part=");
					defaultInterpolatedStringHandler.AppendFormatted<short>(partId);
					defaultInterpolatedStringHandler.AppendLiteral(" color=");
					defaultInterpolatedStringHandler.AppendFormatted<short>(colorId);
					defaultInterpolatedStringHandler.AppendLiteral("/></color>");
					result = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					result = TaiwuEventTagHandler.ErrorNoFieldName("DecodeCricketName");
				}
			}
			else
			{
				result = TaiwuEventTagHandler.ErrorNoKey("DecodeCricketName");
			}
			return result;
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x00164950 File Offset: 0x00162B50
		private static string DecodeGender()
		{
			string languageKey;
			string strKey;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeGender", out languageKey, out strKey);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				sbyte gender = 1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(languageKey, ref gender);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeGender", languageKey);
				}
				else
				{
					string text = strKey;
					string a = text;
					if (!(a == "Gender"))
					{
						result = TaiwuEventTagHandler.Error("DecodeGender", strKey);
					}
					else
					{
						result = "<Language Key=" + TaiwuEventTagHandler._basicSingleGenderMap[strKey][(int)gender] + "/>";
					}
				}
			}
			return result;
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x001649F4 File Offset: 0x00162BF4
		private static string DecodeArgBox()
		{
			string argKey;
			bool flag = !TaiwuEventTagHandler._pairInfos.TryGetValue("key", out argKey);
			string result;
			if (flag)
			{
				result = TaiwuEventTagHandler.ErrorNoKey("DecodeArgBox");
			}
			else
			{
				string argType;
				bool flag2 = !TaiwuEventTagHandler._pairInfos.TryGetValue("type", out argType);
				if (flag2)
				{
					result = TaiwuEventTagHandler.ErrorNoFieldName("DecodeArgBox");
				}
				else
				{
					string text = argType;
					string a = text;
					if (!(a == "int"))
					{
						if (!(a == "float"))
						{
							if (a == "string")
							{
								string arg = string.Empty;
								bool flag3 = TaiwuEventTagHandler._argBox.Get(argKey, ref arg);
								if (flag3)
								{
									return arg;
								}
							}
						}
						else
						{
							float arg2 = 0f;
							bool flag4 = TaiwuEventTagHandler._argBox.Get(argKey, ref arg2);
							if (flag4)
							{
								return arg2.ToString(CultureInfo.CurrentCulture);
							}
						}
					}
					else
					{
						int arg3 = 0;
						bool flag5 = TaiwuEventTagHandler._argBox.Get(argKey, ref arg3);
						if (flag5)
						{
							return arg3.ToString();
						}
					}
					result = TaiwuEventTagHandler.Error("DecodeArgBox", argType);
				}
			}
			return result;
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00164B0C File Offset: 0x00162D0C
		private static string DecodeTemplateCharacter()
		{
			string characterKey = TaiwuEventTagHandler._pairInfos["key"];
			short templateId = -1;
			bool flag = !TaiwuEventTagHandler._argBox.Get(characterKey, ref templateId);
			string result;
			if (flag)
			{
				result = TaiwuEventTagHandler.Error("DecodeTemplateCharacter", characterKey);
			}
			else
			{
				CharacterItem config = Config.Character.Instance[templateId];
				bool flag2 = config == null;
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeTemplateCharacter", characterKey);
				}
				else
				{
					string attrKey = TaiwuEventTagHandler._pairInfos["str"];
					string text = attrKey;
					string a = text;
					if (!(a == "Name"))
					{
						result = TaiwuEventTagHandler.Error("DecodeTemplateCharacter", attrKey);
					}
					else
					{
						bool flag3 = GameData.Domains.World.SharedMethods.SmallVillageXiangshu((short)config.OrganizationInfo.OrgTemplateId, false);
						if (flag3)
						{
							result = config.AnonymousTitle;
						}
						else
						{
							result = config.Surname + config.GivenName;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x00164BE8 File Offset: 0x00162DE8
		private static string DecodeCharacter()
		{
			string characterKey = TaiwuEventTagHandler._pairInfos["key"];
			GameData.Domains.Character.Character character = TaiwuEventTagHandler._argBox.GetCharacter(characterKey);
			DeadCharacter deadCharacter = null;
			int charId = -1;
			bool flag = character == null;
			if (flag)
			{
				bool flag2 = TaiwuEventTagHandler._argBox.Get(characterKey, ref charId);
				if (flag2)
				{
					deadCharacter = DomainManager.Character.GetDeadCharacter(charId);
				}
			}
			else
			{
				charId = character.GetId();
			}
			bool flag3 = character == null && deadCharacter == null;
			string result2;
			if (flag3)
			{
				result2 = TaiwuEventTagHandler.Error("DecodeCharacter", characterKey);
			}
			else
			{
				sbyte gender = (character != null) ? character.GetGender() : deadCharacter.Gender;
				AvatarData avatarData = (character != null) ? character.GetAvatar() : deadCharacter.Avatar;
				string attrKey = TaiwuEventTagHandler._pairInfos["str"];
				string text = attrKey;
				string text2 = text;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
				if (num <= 2281658218U)
				{
					if (num <= 1746932104U)
					{
						if (num <= 828885775U)
						{
							if (num <= 266367750U)
							{
								if (num != 183056235U)
								{
									if (num != 266367750U)
									{
										goto IL_F1E;
									}
									if (!(text2 == "Name"))
									{
										goto IL_F1E;
									}
									ValueTuple<string, string> result = DomainManager.Character.GetNameRelatedData(charId).GetMonasticTitleOrDisplayName(charId == DomainManager.Taiwu.GetTaiwuCharId());
									return result.Item1 + result.Item2;
								}
								else
								{
									if (!(text2 == "FrontHairColor"))
									{
										goto IL_F1E;
									}
									AvatarHairColorsItem colorItem = AvatarHairColors.Instance.GetItem(avatarData.ColorFrontHairId);
									bool flag4 = colorItem != null;
									if (flag4)
									{
										return colorItem.DisplayDesc;
									}
									goto IL_F1E;
								}
							}
							else if (num != 374968990U)
							{
								if (num != 828885775U)
								{
									goto IL_F1E;
								}
								if (!(text2 == "SkinColor"))
								{
									goto IL_F1E;
								}
								AvatarSkinColorsItem colorItem2 = AvatarSkinColors.Instance.GetItem(avatarData.ColorSkinId);
								bool flag5 = colorItem2 != null;
								if (flag5)
								{
									return colorItem2.DisplayDesc;
								}
								goto IL_F1E;
							}
							else if (!(text2 == "GenderPossessive"))
							{
								goto IL_F1E;
							}
						}
						else if (num <= 1344390383U)
						{
							if (num != 882498618U)
							{
								if (num != 1344390383U)
								{
									goto IL_F1E;
								}
								if (!(text2 == "CurAreaName"))
								{
									goto IL_F1E;
								}
								bool flag6 = character != null;
								if (flag6)
								{
									Location locationCurrent = character.GetLocation();
									bool flag7 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId() && DomainManager.Map.IsTraveling;
									if (flag7)
									{
										locationCurrent = DomainManager.Map.GetTravelCurrLocation();
									}
									return DomainManager.Map.GetStateAndAreaNameByAreaId(locationCurrent.AreaId).Item2;
								}
								goto IL_F1E;
							}
							else if (!(text2 == "GenderRanXinduSpecial"))
							{
								goto IL_F1E;
							}
						}
						else if (num != 1345014289U)
						{
							if (num != 1568619950U)
							{
								if (num != 1746932104U)
								{
									goto IL_F1E;
								}
								if (!(text2 == "GenderSubject"))
								{
									goto IL_F1E;
								}
							}
							else if (!(text2 == "AdultGender"))
							{
								goto IL_F1E;
							}
						}
						else
						{
							if (!(text2 == "SettlementName"))
							{
								goto IL_F1E;
							}
							OrganizationInfo orgInfo = (character != null) ? character.GetOrganizationInfo() : deadCharacter.OrganizationInfo;
							bool flag8 = orgInfo.SettlementId < 0;
							if (flag8)
							{
								return Organization.Instance[0].Name;
							}
							Settlement settlement = DomainManager.Organization.GetSettlement(orgInfo.SettlementId);
							CivilianSettlement cs = settlement as CivilianSettlement;
							short randomNameId = (cs != null) ? cs.GetRandomNameId() : -1;
							MapBlockData block = DomainManager.Map.GetBlock(settlement.GetLocation()).GetRootBlock();
							return (randomNameId != -1) ? LocalTownNames.Instance.TownNameCore[(int)randomNameId].Name : ((block.TemplateId != -1) ? MapBlock.Instance[block.TemplateId].Name : Organization.Instance[0].Name);
						}
					}
					else if (num <= 2032319115U)
					{
						if (num <= 1935941362U)
						{
							if (num != 1837572920U)
							{
								if (num != 1935941362U)
								{
									goto IL_F1E;
								}
								if (!(text2 == "CurBlock"))
								{
									goto IL_F1E;
								}
								bool flag9 = character != null;
								if (flag9)
								{
									Location location = character.GetLocation();
									bool flag10 = !location.IsValid();
									if (flag10)
									{
										location = character.GetValidLocation();
									}
									MapBlockItem config = DomainManager.Map.GetBlock(location).GetConfig();
									return config.Name;
								}
								goto IL_F1E;
							}
							else
							{
								if (!(text2 == "Beard2Color"))
								{
									goto IL_F1E;
								}
								AvatarHairColorsItem colorItem3 = AvatarHairColors.Instance.GetItem(avatarData.ColorBeard2Id);
								bool flag11 = colorItem3 != null;
								if (flag11)
								{
									return colorItem3.DisplayDesc;
								}
								goto IL_F1E;
							}
						}
						else if (num != 1942824117U)
						{
							if (num != 1999562133U)
							{
								if (num != 2032319115U)
								{
									goto IL_F1E;
								}
								if (!(text2 == "Feature1Color"))
								{
									goto IL_F1E;
								}
								AvatarFeatureColorsItem colorItem4 = AvatarFeatureColors.Instance.GetItem(avatarData.ColorFeature1Id);
								bool flag12 = colorItem4 != null;
								if (flag12)
								{
									return colorItem4.DisplayDesc;
								}
								goto IL_F1E;
							}
							else
							{
								if (!(text2 == "LipColor"))
								{
									goto IL_F1E;
								}
								AvatarLipColorsItem colorItem5 = AvatarLipColors.Instance.GetItem(avatarData.ColorMouthId);
								bool flag13 = colorItem5 != null;
								if (flag13)
								{
									return colorItem5.DisplayDesc;
								}
								goto IL_F1E;
							}
						}
						else if (!(text2 == "UpperGenderObject"))
						{
							goto IL_F1E;
						}
					}
					else if (num <= 2090023219U)
					{
						if (num != 2072046152U)
						{
							if (num != 2090023219U)
							{
								goto IL_F1E;
							}
							if (!(text2 == "Beard1Color"))
							{
								goto IL_F1E;
							}
							AvatarHairColorsItem colorItem6 = AvatarHairColors.Instance.GetItem(avatarData.ColorBeard1Id);
							bool flag14 = colorItem6 != null;
							if (flag14)
							{
								return colorItem6.DisplayDesc;
							}
							goto IL_F1E;
						}
						else if (!(text2 == "ChildGender"))
						{
							goto IL_F1E;
						}
					}
					else if (num != 2138679154U)
					{
						if (num != 2182690966U)
						{
							if (num != 2281658218U)
							{
								goto IL_F1E;
							}
							if (!(text2 == "ClothColor"))
							{
								goto IL_F1E;
							}
							AvatarClothColorsItem clothColorItem = AvatarClothColors.Instance.GetItem(avatarData.ColorClothId);
							bool flag15 = clothColorItem != null;
							if (flag15)
							{
								return clothColorItem.DisplayDesc;
							}
							goto IL_F1E;
						}
						else
						{
							if (!(text2 == "NicknameOfTaiwu"))
							{
								goto IL_F1E;
							}
							LoveDataItem loveDataItem;
							bool flag16 = DomainManager.Extra.TryGetInteractOfLoveData(charId, out loveDataItem);
							if (flag16)
							{
								bool flag17 = !loveDataItem.EventTimeDict.ContainsKey(25);
								if (flag17)
								{
									return "<Language Key=LK_You/>";
								}
								string taiwuNickname;
								bool flag18 = DomainManager.World.GetCustomTexts().TryGetValue(loveDataItem.TaiwuNicknameId, out taiwuNickname) && !string.IsNullOrEmpty(taiwuNickname);
								if (flag18)
								{
									return taiwuNickname;
								}
							}
							int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
							TaiwuEventTagHandler._handlingEvent.NeedNameRelatedDataCharacterIdList.Add(taiwuCharId);
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
							defaultInterpolatedStringHandler.AppendLiteral("<CharNickname Id=");
							defaultInterpolatedStringHandler.AppendFormatted<int>(taiwuCharId);
							defaultInterpolatedStringHandler.AppendLiteral(" />");
							return defaultInterpolatedStringHandler.ToStringAndClear();
						}
					}
					else
					{
						if (!(text2 == "OrgName"))
						{
							goto IL_F1E;
						}
						OrganizationInfo orgInfo2 = (character != null) ? character.GetOrganizationInfo() : deadCharacter.OrganizationInfo;
						OrganizationItem configItem = Organization.Instance.GetItem(orgInfo2.OrgTemplateId);
						bool flag19 = configItem != null;
						if (flag19)
						{
							return configItem.Name;
						}
						goto IL_F1E;
					}
				}
				else if (num <= 3009150591U)
				{
					if (num <= 2403551152U)
					{
						if (num <= 2374596503U)
						{
							if (num != 2303352797U)
							{
								if (num != 2374596503U)
								{
									goto IL_F1E;
								}
								if (!(text2 == "GenderObject"))
								{
									goto IL_F1E;
								}
							}
							else
							{
								if (!(text2 == "BodyType"))
								{
									goto IL_F1E;
								}
								AvatarHeadItem headItem = AvatarManager.Instance.GetAsset((int)avatarData.AvatarId, EAvatarElementsType.Head, new short[]
								{
									(short)avatarData.HeadId
								}).HeadConfig;
								bool flag20 = headItem != null;
								if (flag20)
								{
									return headItem.DisplayDesc;
								}
								goto IL_F1E;
							}
						}
						else if (num != 2390522771U)
						{
							if (num != 2403551152U)
							{
								goto IL_F1E;
							}
							if (!(text2 == "Feature2Color"))
							{
								goto IL_F1E;
							}
							AvatarFeatureColorsItem colorItem7 = AvatarFeatureColors.Instance.GetItem(avatarData.ColorFeature2Id);
							bool flag21 = colorItem7 != null;
							if (flag21)
							{
								return colorItem7.DisplayDesc;
							}
							goto IL_F1E;
						}
						else
						{
							if (!(text2 == "EyeBrowColor"))
							{
								goto IL_F1E;
							}
							AvatarHairColorsItem colorItem8 = AvatarHairColors.Instance.GetItem(avatarData.ColorEyebrowId);
							bool flag22 = colorItem8 != null;
							if (flag22)
							{
								return colorItem8.DisplayDesc;
							}
							goto IL_F1E;
						}
					}
					else if (num <= 2852869357U)
					{
						if (num != 2804681632U)
						{
							if (num != 2852869357U)
							{
								goto IL_F1E;
							}
							if (!(text2 == "BackHairColor"))
							{
								goto IL_F1E;
							}
							AvatarHairColorsItem colorItem9 = AvatarHairColors.Instance.GetItem(avatarData.ColorBackHairId);
							bool flag23 = colorItem9 != null;
							if (flag23)
							{
								return colorItem9.DisplayDesc;
							}
							goto IL_F1E;
						}
						else if (!(text2 == "Gender"))
						{
							goto IL_F1E;
						}
					}
					else if (num != 2857989772U)
					{
						if (num != 2931897228U)
						{
							if (num != 3009150591U)
							{
								goto IL_F1E;
							}
							if (!(text2 == "AreaName"))
							{
								goto IL_F1E;
							}
							OrganizationInfo orgInfo3 = (character != null) ? character.GetOrganizationInfo() : deadCharacter.OrganizationInfo;
							Location location2 = DomainManager.Organization.GetSettlement(orgInfo3.SettlementId).GetLocation();
							return DomainManager.Map.GetStateAndAreaNameByAreaId(location2.AreaId).Item2;
						}
						else
						{
							if (!(text2 == "AutoAgeGender"))
							{
								goto IL_F1E;
							}
							short age = (character != null) ? character.GetPhysiologicalAge() : deadCharacter.GetActualAge();
							sbyte ageLevel = 0;
							bool flag24 = age >= 16;
							if (flag24)
							{
								ageLevel = 1;
							}
							bool flag25 = (int)age >= GlobalConfig.Instance.AgeShowWrinkle2;
							if (flag25)
							{
								ageLevel = 2;
							}
							return "<Language Key=" + TaiwuEventTagHandler._autoAgeGender[(int)ageLevel][(int)gender] + "/>";
						}
					}
					else if (!(text2 == "UpperGenderPossessive"))
					{
						goto IL_F1E;
					}
				}
				else if (num <= 3823569466U)
				{
					if (num <= 3065457840U)
					{
						if (num != 3031640513U)
						{
							if (num != 3065457840U)
							{
								goto IL_F1E;
							}
							if (!(text2 == "ClothName"))
							{
								goto IL_F1E;
							}
							bool flag26 = character != null;
							if (flag26)
							{
								short templateId = character.GetTemplateId();
								CharacterItem config2 = Config.Character.Instance[templateId];
								return Config.Clothing.Instance[config2.PresetEquipment[4].TemplateId].Name;
							}
							goto IL_F1E;
						}
						else
						{
							if (!(text2 == "Relation"))
							{
								goto IL_F1E;
							}
							string relationKey;
							bool flag27 = TaiwuEventTagHandler._pairInfos.TryGetValue("relationKey", out relationKey);
							if (flag27)
							{
								GameData.Domains.Character.Character targetChar = TaiwuEventTagHandler._argBox.GetCharacter(relationKey);
								RelatedCharacter relatedChar;
								string[] relationValueArray;
								bool flag28 = targetChar != null && character != null && DomainManager.Character.TryGetRelation(character.GetId(), targetChar.GetId(), out relatedChar) && TaiwuEventTagHandler._relationGenderMap.TryGetValue(relatedChar.RelationType, out relationValueArray);
								if (flag28)
								{
									return "<Language Key=" + relationValueArray[(int)targetChar.GetGender()] + "/>";
								}
							}
							goto IL_F1E;
						}
					}
					else if (num != 3344223925U)
					{
						if (num != 3754021051U)
						{
							if (num != 3823569466U)
							{
								goto IL_F1E;
							}
							if (!(text2 == "UpperGenderSubject"))
							{
								goto IL_F1E;
							}
						}
						else
						{
							if (!(text2 == "Identity"))
							{
								goto IL_F1E;
							}
							short age2 = (character != null) ? character.GetCurrAge() : deadCharacter.GetActualAge();
							OrganizationInfo orgInfo4 = (character != null) ? character.GetOrganizationInfo() : deadCharacter.OrganizationInfo;
							OrganizationMemberItem memberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo4);
							bool flag29 = memberConfig != null;
							if (!flag29)
							{
								goto IL_F1E;
							}
							bool flag30 = age2 >= memberConfig.IdentityActiveAge;
							if (flag30)
							{
								string identityString = orgInfo4.Principal ? memberConfig.GradeName : memberConfig.SpouseAnonymousTitles[(int)gender];
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
								defaultInterpolatedStringHandler.AppendLiteral("<color=#GradeColor_");
								defaultInterpolatedStringHandler.AppendFormatted<sbyte>(orgInfo4.Grade);
								defaultInterpolatedStringHandler.AppendLiteral(">");
								defaultInterpolatedStringHandler.AppendFormatted(identityString);
								defaultInterpolatedStringHandler.AppendLiteral("</color>");
								return defaultInterpolatedStringHandler.ToStringAndClear();
							}
							bool flag31 = AgeGroup.GetAgeGroup(age2) == 0;
							if (flag31)
							{
								return "<Language Key=LK_Baby/>";
							}
							bool flag32 = gender == 1;
							if (flag32)
							{
								return "<Language Key=LK_Boy/>";
							}
							return "<Language Key=LK_Girl/>";
						}
					}
					else if (!(text2 == "AdoptiveParent"))
					{
						goto IL_F1E;
					}
				}
				else if (num <= 3951280853U)
				{
					if (num != 3829011397U)
					{
						if (num != 3951280853U)
						{
							goto IL_F1E;
						}
						if (!(text2 == "AgedGender"))
						{
							goto IL_F1E;
						}
					}
					else if (!(text2 == "AdoptiveChild"))
					{
						goto IL_F1E;
					}
				}
				else if (num != 4079484394U)
				{
					if (num != 4160155236U)
					{
						if (num != 4214795775U)
						{
							goto IL_F1E;
						}
						if (!(text2 == "Profession"))
						{
							goto IL_F1E;
						}
						ProfessionData currProfession = DomainManager.Extra.GetCharacterCurrentProfession(charId);
						bool flag33 = currProfession != null;
						if (flag33)
						{
							return currProfession.GetConfig().Name;
						}
						goto IL_F1E;
					}
					else if (!(text2 == "HusbandOrWife"))
					{
						goto IL_F1E;
					}
				}
				else
				{
					if (!(text2 == "EyeballColor"))
					{
						goto IL_F1E;
					}
					AvatarEyeballColorsItem colorItem10 = AvatarEyeballColors.Instance.GetItem(avatarData.ColorEyeballId);
					bool flag34 = colorItem10 != null;
					if (flag34)
					{
						return colorItem10.DisplayDesc;
					}
					goto IL_F1E;
				}
				return "<Language Key=" + TaiwuEventTagHandler._basicSingleGenderMap[attrKey][(int)gender] + "/>";
				IL_F1E:
				result2 = TaiwuEventTagHandler.Error("DecodeCharacter", attrKey);
			}
			return result2;
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x00165B28 File Offset: 0x00163D28
		private static string DecodeItem()
		{
			sbyte itemType = 0;
			short templateId = 0;
			string itemKeyString;
			bool flag = TaiwuEventTagHandler._pairInfos.TryGetValue("key", out itemKeyString);
			if (flag)
			{
				ItemKey itemKey;
				bool flag2 = TaiwuEventTagHandler._argBox.Get<ItemKey>(itemKeyString, out itemKey);
				if (!flag2)
				{
					return TaiwuEventTagHandler.Error("DecodeItem", itemKeyString);
				}
				itemType = itemKey.ItemType;
				templateId = itemKey.TemplateId;
			}
			else
			{
				string itemTypeString;
				string itemId;
				bool flag3 = TaiwuEventTagHandler._pairInfos.TryGetValue("type", out itemTypeString) && TaiwuEventTagHandler._pairInfos.TryGetValue("id", out itemId);
				if (!flag3)
				{
					return TaiwuEventTagHandler.ErrorNoKey("DecodeItem");
				}
				bool flag4 = !ItemType.TypeName2TypeId.TryGetValue(itemTypeString, out itemType);
				if (flag4)
				{
					return TaiwuEventTagHandler.Error("DecodeItem", itemTypeString);
				}
				bool flag5 = !short.TryParse(itemId, out templateId);
				if (flag5)
				{
					return TaiwuEventTagHandler.Error("DecodeItem", itemId);
				}
			}
			string result = string.Empty;
			string strKey;
			bool flag6 = TaiwuEventTagHandler._pairInfos.TryGetValue("str", out strKey);
			if (flag6)
			{
				ItemKey itemKey2;
				bool flag7 = itemType == 11 && !string.IsNullOrEmpty(itemKeyString) && TaiwuEventTagHandler._argBox.Get<ItemKey>(itemKeyString, out itemKey2);
				if (flag7)
				{
					GameData.Domains.Item.Cricket cricket = DomainManager.Item.GetBaseItem(itemKey2) as GameData.Domains.Item.Cricket;
					bool flag8 = cricket == null;
					if (flag8)
					{
						return "{Cricket decode error}";
					}
					string text = strKey;
					string a = text;
					if (!(a == "Name"))
					{
						if (a == "ColorName")
						{
							sbyte grade = ItemTemplateHelper.GetGrade(itemType, templateId);
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 3);
							defaultInterpolatedStringHandler.AppendLiteral("<color=#GradeColor_");
							defaultInterpolatedStringHandler.AppendFormatted<sbyte>(grade);
							defaultInterpolatedStringHandler.AppendLiteral("><Cricket part=");
							defaultInterpolatedStringHandler.AppendFormatted<short>(cricket.GetPartId());
							defaultInterpolatedStringHandler.AppendLiteral(" color=");
							defaultInterpolatedStringHandler.AppendFormatted<short>(cricket.GetColorId());
							defaultInterpolatedStringHandler.AppendLiteral("/></color>");
							result = defaultInterpolatedStringHandler.ToStringAndClear();
						}
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 2);
						defaultInterpolatedStringHandler.AppendLiteral("<Cricket part=");
						defaultInterpolatedStringHandler.AppendFormatted<short>(cricket.GetPartId());
						defaultInterpolatedStringHandler.AppendLiteral(" color=");
						defaultInterpolatedStringHandler.AppendFormatted<short>(cricket.GetColorId());
						defaultInterpolatedStringHandler.AppendLiteral("/>");
						result = defaultInterpolatedStringHandler.ToStringAndClear();
					}
				}
				else
				{
					string text2 = strKey;
					string a2 = text2;
					if (!(a2 == "Name"))
					{
						if (a2 == "ColorName")
						{
							sbyte grade2 = ItemTemplateHelper.GetGrade(itemType, templateId);
							string name = ItemTemplateHelper.GetName(itemType, templateId);
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
							defaultInterpolatedStringHandler.AppendLiteral("<color=#GradeColor_");
							defaultInterpolatedStringHandler.AppendFormatted<sbyte>(grade2);
							defaultInterpolatedStringHandler.AppendLiteral(">");
							defaultInterpolatedStringHandler.AppendFormatted(name);
							defaultInterpolatedStringHandler.AppendLiteral("</color>");
							result = defaultInterpolatedStringHandler.ToStringAndClear();
						}
					}
					else
					{
						result = ItemTemplateHelper.GetName(itemType, templateId);
					}
				}
			}
			string itemSprite;
			bool flag9 = TaiwuEventTagHandler._pairInfos.TryGetValue("sp", out itemSprite);
			if (flag9)
			{
				string text3 = itemSprite;
				string a3 = text3;
				if (a3 == "Icon")
				{
					result = result + "<SpName=\"" + ItemTemplateHelper.GetIcon(itemType, templateId) + "\"";
				}
			}
			return result;
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x00165E7C File Offset: 0x0016407C
		private static string DecodeCombatSkill()
		{
			string skillIdKey = TaiwuEventTagHandler._pairInfos["key"];
			short combatSKillId = -1;
			bool flag = !TaiwuEventTagHandler._argBox.Get(skillIdKey, ref combatSKillId);
			string result;
			if (flag)
			{
				result = TaiwuEventTagHandler.Error("DecodeCombatSkill", skillIdKey);
			}
			else
			{
				CombatSkillItem configItem = CombatSkill.Instance.GetItem(combatSKillId);
				string attrKey = TaiwuEventTagHandler._pairInfos["str"];
				string text = attrKey;
				string a = text;
				if (!(a == "SkillName"))
				{
					if (!(a == "SkillDesc"))
					{
						if (!(a == "TypeName"))
						{
							if (a == "SkillBookName")
							{
								SkillBookItem bookConfig = Config.SkillBook.Instance[configItem.BookId];
								bool flag2 = bookConfig != null;
								if (flag2)
								{
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
									defaultInterpolatedStringHandler.AppendLiteral("<color=#GradeColor_");
									defaultInterpolatedStringHandler.AppendFormatted<sbyte>(bookConfig.Grade);
									defaultInterpolatedStringHandler.AppendLiteral(">");
									defaultInterpolatedStringHandler.AppendFormatted(bookConfig.Name);
									defaultInterpolatedStringHandler.AppendLiteral("</color>");
									return defaultInterpolatedStringHandler.ToStringAndClear();
								}
							}
						}
						else
						{
							CombatSkillTypeItem typeConfigItem = CombatSkillType.Instance.GetItem(configItem.Type);
							bool flag3 = typeConfigItem != null;
							if (flag3)
							{
								return typeConfigItem.Name;
							}
						}
					}
					else
					{
						bool flag4 = configItem != null;
						if (flag4)
						{
							return configItem.Desc;
						}
					}
				}
				else
				{
					bool flag5 = configItem != null;
					if (flag5)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
						defaultInterpolatedStringHandler.AppendLiteral("《<color=#GradeColor_");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(configItem.Grade);
						defaultInterpolatedStringHandler.AppendLiteral(">");
						defaultInterpolatedStringHandler.AppendFormatted(configItem.Name);
						defaultInterpolatedStringHandler.AppendLiteral("</color>》");
						return defaultInterpolatedStringHandler.ToStringAndClear();
					}
				}
				result = TaiwuEventTagHandler.Error("DecodeCombatSkill", attrKey);
			}
			return result;
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x00166078 File Offset: 0x00164278
		private static string DecodeCombatSkillType()
		{
			string skillTypeKey;
			bool flag = !TaiwuEventTagHandler._pairInfos.TryGetValue("key", out skillTypeKey);
			string result;
			if (flag)
			{
				result = "{CombatSKillType key not find}";
			}
			else
			{
				sbyte combatSKillTypeId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(skillTypeKey, ref combatSKillTypeId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeCombatSkillType", skillTypeKey);
				}
				else
				{
					CombatSkillTypeItem config = CombatSkillType.Instance.GetItem(combatSKillTypeId);
					string displayType = TaiwuEventTagHandler._pairInfos["str"];
					string text = displayType;
					string a = text;
					if (a == "Name")
					{
						bool flag3 = config != null;
						if (flag3)
						{
							return config.Name;
						}
					}
					result = TaiwuEventTagHandler.Error("DecodeCombatSkillType", displayType);
				}
			}
			return result;
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x00166130 File Offset: 0x00164330
		private static string DecodeLifeSkill()
		{
			string skillIdKey = TaiwuEventTagHandler._pairInfos["key"];
			short lifeSKillId = -1;
			bool flag = !TaiwuEventTagHandler._argBox.Get(skillIdKey, ref lifeSKillId);
			string result;
			if (flag)
			{
				result = TaiwuEventTagHandler.Error("DecodeLifeSkill", skillIdKey);
			}
			else
			{
				Config.LifeSkillItem configItem = LifeSkill.Instance.GetItem(lifeSKillId);
				string strKey = TaiwuEventTagHandler._pairInfos["str"];
				string text = strKey;
				string a = text;
				if (!(a == "SkillName"))
				{
					if (!(a == "SkillDesc"))
					{
						if (a == "TypeName")
						{
							LifeSkillTypeItem typeConfigItem = Config.LifeSkillType.Instance.GetItem(configItem.Type);
							bool flag2 = typeConfigItem != null;
							if (flag2)
							{
								return typeConfigItem.Name;
							}
						}
					}
					else
					{
						bool flag3 = configItem != null;
						if (flag3)
						{
							return configItem.Desc;
						}
					}
				}
				else
				{
					bool flag4 = configItem != null;
					if (flag4)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
						defaultInterpolatedStringHandler.AppendLiteral("《<color=#GradeColor_");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(configItem.Grade);
						defaultInterpolatedStringHandler.AppendLiteral(">");
						defaultInterpolatedStringHandler.AppendFormatted(configItem.Name);
						defaultInterpolatedStringHandler.AppendLiteral("</color>》");
						return defaultInterpolatedStringHandler.ToStringAndClear();
					}
				}
				result = TaiwuEventTagHandler.Error("DecodeLifeSkill", strKey);
			}
			return result;
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x00166290 File Offset: 0x00164490
		private static string DecodeLifeSkillType()
		{
			string skillTypeKey;
			bool flag = !TaiwuEventTagHandler._pairInfos.TryGetValue("key", out skillTypeKey);
			string result;
			if (flag)
			{
				result = "{LifeSkillType key not find}";
			}
			else
			{
				sbyte lifeSkillTypeId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(skillTypeKey, ref lifeSkillTypeId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeLifeSkillType", skillTypeKey);
				}
				else
				{
					LifeSkillTypeItem config = Config.LifeSkillType.Instance.GetItem(lifeSkillTypeId);
					string displayType = TaiwuEventTagHandler._pairInfos["str"];
					string text = displayType;
					string a = text;
					if (a == "Name")
					{
						bool flag3 = config != null;
						if (flag3)
						{
							return config.Name;
						}
					}
					result = TaiwuEventTagHandler.Error("DecodeLifeSkillType", displayType);
				}
			}
			return result;
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x00166348 File Offset: 0x00164548
		private static string DecodeAdventure()
		{
			AdventureMapPoint point;
			bool flag = !TaiwuEventTagHandler._argBox.Get<AdventureMapPoint>("AdventurePoint", out point);
			string result;
			if (flag)
			{
				result = TaiwuEventTagHandler.Error("DecodeAdventure", "AdventurePoint");
			}
			else
			{
				string key = TaiwuEventTagHandler._pairInfos["key"];
				string strKey = TaiwuEventTagHandler._pairInfos["str"];
				bool flag2 = key == "Point";
				if (flag2)
				{
					string text = strKey;
					string a = text;
					if (a == "Name")
					{
						AdventureTerrainItem terrainConfig = AdventureTerrain.Instance.GetItem((sbyte)point.TerrainId);
						bool flag3 = terrainConfig != null;
						if (flag3)
						{
							return terrainConfig.Name;
						}
					}
				}
				else
				{
					bool flag4 = key == "Param";
					if (flag4)
					{
						string paramKey = TaiwuEventTagHandler._pairInfos["paramKey"];
						string text2 = strKey;
						string a2 = text2;
						if (a2 == "Name")
						{
							return DomainManager.Adventure.GetAdvParameterConfig(paramKey).Item2;
						}
						if (a2 == "Value")
						{
							return DomainManager.Adventure.GetAdvParameter(paramKey).ToString();
						}
					}
				}
				result = TaiwuEventTagHandler.Error("DecodeAdventure", strKey);
			}
			return result;
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0016648C File Offset: 0x0016468C
		private static string DecodeCharacterOrActor()
		{
			string characterKey = TaiwuEventTagHandler._pairInfos["key"];
			bool flag = TaiwuEventTagHandler._argBox.Contains<int>(characterKey);
			string result;
			if (flag)
			{
				result = TaiwuEventTagHandler.DecodeCharacter();
			}
			else
			{
				bool flag2 = TaiwuEventTagHandler._argBox.Contains<EventActorData>(characterKey);
				if (flag2)
				{
					result = TaiwuEventTagHandler.DecodeActor();
				}
				else
				{
					result = "{Unable to decode character or actor with key " + characterKey;
				}
			}
			return result;
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x001664E8 File Offset: 0x001646E8
		private static string DecodeActor()
		{
			string actorKey;
			string strKey;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeActor", out actorKey, out strKey);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				EventActorData actor;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get<EventActorData>(actorKey, out actor);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeActor", actorKey);
				}
				else
				{
					string text = strKey;
					string a = text;
					if (!(a == "Name"))
					{
						if (!(a == "Age"))
						{
							if (!(a == "ChildGender") && !(a == "AdultGender"))
							{
								result = TaiwuEventTagHandler.Error("DecodeActor", strKey);
							}
							else
							{
								result = "<Language Key=" + TaiwuEventTagHandler._basicSingleGenderMap[strKey][(int)actor.Gender] + "/>";
							}
						}
						else
						{
							result = actor.Age.ToString();
						}
					}
					else
					{
						result = actor.DisplayName;
					}
				}
			}
			return result;
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x001665D8 File Offset: 0x001647D8
		private static string DecodeResource()
		{
			string resKey;
			string strKey;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeResource", out resKey, out strKey);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				int arg = 0;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(resKey, ref arg);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeResource", resKey);
				}
				else
				{
					ResourceTypeItem config = Config.ResourceType.Instance.GetItem((sbyte)arg);
					bool flag3 = config == null;
					if (flag3)
					{
						result = TaiwuEventTagHandler.Error("DecodeResource", resKey);
					}
					else
					{
						string text = strKey;
						string a = text;
						if (!(a == "Name"))
						{
							result = TaiwuEventTagHandler.Error("DecodeResource", strKey);
						}
						else
						{
							result = config.Name;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x00166690 File Offset: 0x00164890
		private static string DecodeNormalInformation()
		{
			string resKey;
			string strKey;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeNormalInformation", out resKey, out strKey);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				NormalInformation normalInformation;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get<NormalInformation>(resKey, out normalInformation);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeNormalInformation", resKey);
				}
				else
				{
					InformationItem config = Information.Instance.GetItem(normalInformation.TemplateId);
					InformationInfoItem infoConfig = InformationInfo.Instance.GetItem(config.InfoIds[(int)normalInformation.Level]);
					string text = strKey;
					string a = text;
					if (!(a == "Belong"))
					{
						if (!(a == "Desc"))
						{
							if (!(a == "AnsEffective"))
							{
								if (!(a == "AnsNormal"))
								{
									if (!(a == "AnsInvalid"))
									{
										result = TaiwuEventTagHandler.Error("DecodeNormalInformation", strKey);
									}
									else
									{
										result = infoConfig.InvalidAnswer;
									}
								}
								else
								{
									result = infoConfig.NormalAnswer;
								}
							}
							else
							{
								result = infoConfig.EffectiveAnswer;
							}
						}
						else
						{
							result = infoConfig.Desc;
						}
					}
					else
					{
						bool flag3 = config.Type == 0 || config.Type == 1;
						if (flag3)
						{
							OrganizationItem orgConfig = Organization.Instance.GetItem(infoConfig.Oraganization);
							result = orgConfig.Name;
						}
						else
						{
							bool flag4 = config.Type == 2;
							if (flag4)
							{
								LifeSkillTypeItem lifeSkillTypeConfig = Config.LifeSkillType.Instance.GetItem(infoConfig.LifeSkillType);
								result = lifeSkillTypeConfig.Name;
							}
							else
							{
								result = "<Language Key=LK_InformationType_West/>";
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x00166830 File Offset: 0x00164A30
		private static string DecodeSecretInformation()
		{
			return "{secret information decode error}";
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x00166848 File Offset: 0x00164A48
		private static string DecodeSettlement()
		{
			string settlementIdKey;
			string strKey;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeSettlement", out settlementIdKey, out strKey);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short settlementId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(settlementIdKey, ref settlementId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeSettlement", settlementIdKey);
				}
				else
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
					string text = strKey;
					string a = text;
					if (!(a == "Name"))
					{
						if (!(a == "PrisonName"))
						{
							if (!(a == "TreasuryName"))
							{
								if (!(a == "MapAreaName"))
								{
									if (!(a == "MapStateName"))
									{
										result = TaiwuEventTagHandler.Error("DecodeSettlement", strKey);
									}
									else
									{
										Location location = settlement.GetLocation();
										sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
										result = MapState.Instance[stateTemplateId].Name;
									}
								}
								else
								{
									Location location2 = settlement.GetLocation();
									MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)location2.AreaId);
									result = areaData.GetConfig().Name;
								}
							}
							else
							{
								sbyte orgTemplateId = settlement.GetOrgTemplateId();
								if (!true)
								{
								}
								short num;
								if (orgTemplateId >= 21)
								{
									if (orgTemplateId <= 35)
									{
										num = 284;
										goto IL_23A;
									}
									switch (orgTemplateId)
									{
									case 36:
										num = 286;
										goto IL_23A;
									case 37:
										num = 285;
										goto IL_23A;
									case 38:
										num = 287;
										goto IL_23A;
									}
								}
								else if (orgTemplateId >= 1)
								{
									if (orgTemplateId <= 15)
									{
										num = (short)((int)(settlement.GetOrgTemplateId() - 1) + 288);
										goto IL_23A;
									}
								}
								num = -1;
								IL_23A:
								if (!true)
								{
								}
								short buildingTemplateId = num;
								result = BuildingBlock.Instance[buildingTemplateId].Name;
							}
						}
						else
						{
							OrganizationItem config = Organization.Instance[settlement.GetOrgTemplateId()];
							result = BuildingBlock.Instance[config.PrisonBuilding].Name;
						}
					}
					else
					{
						CivilianSettlement cs = settlement as CivilianSettlement;
						short randomNameId = (cs != null) ? cs.GetRandomNameId() : -1;
						MapBlockData block = DomainManager.Map.GetBlock(settlement.GetLocation()).GetRootBlock();
						string settlementName = (randomNameId != -1) ? LocalTownNames.Instance.TownNameCore[(int)randomNameId].Name : ((block.TemplateId != -1) ? MapBlock.Instance[block.TemplateId].Name : Organization.Instance[0].Name);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
						defaultInterpolatedStringHandler.AppendLiteral("<color=#GradeColor_");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(7);
						defaultInterpolatedStringHandler.AppendLiteral(">");
						defaultInterpolatedStringHandler.AppendFormatted(settlementName);
						defaultInterpolatedStringHandler.AppendLiteral("</color>");
						result = defaultInterpolatedStringHandler.ToStringAndClear();
					}
				}
			}
			return result;
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x00166B1C File Offset: 0x00164D1C
		private static string DecodeMapArea()
		{
			string areaIdKey;
			string strKey;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeMapArea", out areaIdKey, out strKey);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short areaId = -1;
				MapAreaData mapAreaData;
				bool flag2 = TaiwuEventTagHandler._argBox.Get<MapAreaData>(areaIdKey, out mapAreaData);
				if (flag2)
				{
					areaId = mapAreaData.GetAreaId();
				}
				else
				{
					bool flag3 = !TaiwuEventTagHandler._argBox.Get(areaIdKey, ref areaId);
					if (flag3)
					{
						return TaiwuEventTagHandler.Error("DecodeMapArea", areaIdKey);
					}
				}
				string text = strKey;
				string a = text;
				if (!(a == "Name"))
				{
					if (!(a == "StateName"))
					{
						result = TaiwuEventTagHandler.Error("DecodeMapArea", strKey);
					}
					else
					{
						string stateName = DomainManager.Map.GetStateAndAreaNameByAreaId(areaId).Item1;
						result = stateName;
					}
				}
				else
				{
					string areaName = DomainManager.Map.GetStateAndAreaNameByAreaId(areaId).Item2;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
					defaultInterpolatedStringHandler.AppendLiteral("<color=#GradeColor_");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(7);
					defaultInterpolatedStringHandler.AppendLiteral(">");
					defaultInterpolatedStringHandler.AppendFormatted(areaName);
					defaultInterpolatedStringHandler.AppendLiteral("</color>");
					result = defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			return result;
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x00166C48 File Offset: 0x00164E48
		private static string DecodeMapState()
		{
			string areaIdKey;
			string strKey;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeMapState", out areaIdKey, out strKey);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				sbyte areaId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(areaIdKey, ref areaId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeMapState", areaIdKey);
				}
				else
				{
					string text = strKey;
					string a = text;
					if (!(a == "Name"))
					{
						result = TaiwuEventTagHandler.Error("DecodeMapState", strKey);
					}
					else
					{
						sbyte stateId = DomainManager.Map.GetStateTemplateIdByAreaId((short)areaId);
						result = MapState.Instance[stateId].Name;
					}
				}
			}
			return result;
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x00166CEC File Offset: 0x00164EEC
		public static string DecodeMapBlockName()
		{
			string locationKey;
			string strKey;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeMapBlockName", out locationKey, out strKey);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				Location location = Location.Invalid;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get<Location>(locationKey, out location);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeMapBlockName", locationKey);
				}
				else
				{
					string text = strKey;
					string a = text;
					if (!(a == "Name"))
					{
						result = TaiwuEventTagHandler.Error("DecodeMapBlockName", strKey);
					}
					else
					{
						bool flag3 = !location.IsValid();
						if (flag3)
						{
							result = MapBlock.Instance[117].Name;
						}
						else
						{
							result = DomainManager.Map.GetBlock(location).GetConfig().Name;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x00166DB4 File Offset: 0x00164FB4
		private static string DecodeGeneral()
		{
			string languageKey;
			string strKey;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeGeneral", out languageKey, out strKey);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				string text = strKey;
				string a = text;
				if (!(a == "ItemSubType"))
				{
					if (!(a == "BehaviorType"))
					{
						if (!(a == "UpperNum"))
						{
							if (!(a == "CombatSkillOutlineType"))
							{
								result = TaiwuEventTagHandler.Error("DecodeGeneral", strKey);
							}
							else
							{
								sbyte type = 0;
								TaiwuEventTagHandler._argBox.Get("CombatSkillOutlineType", ref type);
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
								defaultInterpolatedStringHandler.AppendLiteral("<Language Key=LK_CombatSkill_First_Page_Type_");
								defaultInterpolatedStringHandler.AppendFormatted<sbyte>(type);
								defaultInterpolatedStringHandler.AppendLiteral("/>");
								result = defaultInterpolatedStringHandler.ToStringAndClear();
							}
						}
						else
						{
							int grade = 1;
							TaiwuEventTagHandler._argBox.Get("Grade", ref grade);
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
							defaultInterpolatedStringHandler.AppendLiteral("<Language Key=LK_Num_");
							defaultInterpolatedStringHandler.AppendFormatted<int>(grade);
							defaultInterpolatedStringHandler.AppendLiteral("/>");
							result = defaultInterpolatedStringHandler.ToStringAndClear();
						}
					}
					else
					{
						result = "<Language Key=LK_Goodness_" + languageKey + "/>";
					}
				}
				else
				{
					result = "<Language Key=LK_ItemSubType_" + languageKey + "/>";
				}
			}
			return result;
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x00166F0C File Offset: 0x0016510C
		private static string DecodeJiaoLoong()
		{
			string jiaoLoongKey;
			string strKey;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeJiaoLoong", out jiaoLoongKey, out strKey);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				int jiaoLoongId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(jiaoLoongKey, ref jiaoLoongId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeJiaoLoong", jiaoLoongKey);
				}
				else
				{
					string text3 = strKey;
					string a = text3;
					if (!(a == "JiaoLoongName"))
					{
						if (!(a == "ChildrenOfLoongLoongName"))
						{
							result = TaiwuEventTagHandler.Error("DecodeJiaoLoong", strKey);
						}
						else
						{
							string jiaoLoongName = string.Empty;
							ChildrenOfLoong childrenOfLoong;
							bool flag3 = DomainManager.Extra.TryGetElement_ChildrenOfLoong(jiaoLoongId, out childrenOfLoong);
							if (flag3)
							{
								bool flag4 = childrenOfLoong.NameId < 0;
								if (flag4)
								{
									jiaoLoongName = ItemTemplateHelper.GetName(childrenOfLoong.Key.ItemType, childrenOfLoong.Key.TemplateId);
								}
								else
								{
									IReadOnlyDictionary<int, string> customTexts = DomainManager.World.GetCustomTexts();
									string text;
									bool flag5 = customTexts.TryGetValue(childrenOfLoong.NameId, out text);
									if (flag5)
									{
										jiaoLoongName = text;
									}
								}
							}
							result = jiaoLoongName;
						}
					}
					else
					{
						string jiaoLoongName2 = string.Empty;
						GameData.DLC.FiveLoong.Jiao jiao;
						bool flag6 = DomainManager.Extra.TryGetElement_Jiaos(jiaoLoongId, out jiao);
						if (flag6)
						{
							bool flag7 = jiao.NameId < 0;
							if (flag7)
							{
								jiaoLoongName2 = ItemTemplateHelper.GetName(jiao.Key.ItemType, jiao.Key.TemplateId);
							}
							else
							{
								IReadOnlyDictionary<int, string> customTexts2 = DomainManager.World.GetCustomTexts();
								string text2;
								bool flag8 = customTexts2.TryGetValue(jiao.NameId, out text2);
								if (flag8)
								{
									jiaoLoongName2 = text2;
								}
							}
						}
						result = jiaoLoongName2;
					}
				}
			}
			return result;
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x001670A8 File Offset: 0x001652A8
		private static string DecodeJiaoNurturance()
		{
			string key;
			string str;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeJiaoNurturance", out key, out str);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short configId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(key, ref configId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeJiaoNurturance", key);
				}
				else
				{
					JiaoNurturanceItem config = JiaoNurturance.Instance[configId];
					if (!true)
					{
					}
					string text;
					if (!(str == "Name"))
					{
						text = TaiwuEventTagHandler.Error("DecodeJiaoNurturance", str);
					}
					else
					{
						text = config.Name;
					}
					if (!true)
					{
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x00167148 File Offset: 0x00165348
		private static string DecodeSkillBook()
		{
			string key;
			string str;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeCharacterTitle", out key, out str);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short configId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(key, ref configId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeSkillBook", key);
				}
				else
				{
					SkillBookItem config = Config.SkillBook.Instance[configId];
					if (!true)
					{
					}
					string text;
					if (!(str == "Name"))
					{
						text = TaiwuEventTagHandler.Error("DecodeSkillBook", str);
					}
					else
					{
						text = config.Name;
					}
					if (!true)
					{
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x001671E8 File Offset: 0x001653E8
		private static string DecodeFiveElementName()
		{
			string key;
			string str;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeFiveElementName", out key, out str);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short fiveElementType = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(key, ref fiveElementType);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeFiveElementName", key);
				}
				else
				{
					if (!true)
					{
					}
					string text;
					if (!(str == "Name"))
					{
						text = TaiwuEventTagHandler.Error("DecodeFiveElementName", str);
					}
					else
					{
						text = "<Language Key=" + TaiwuEventTagHandler.FiveElementTypeName[(int)fiveElementType] + "/>";
					}
					if (!true)
					{
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x00167288 File Offset: 0x00165488
		private static string DecodePersonalityTypeName()
		{
			string key;
			string str;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodePersonalityTypeName", out key, out str);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short personalityType = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(key, ref personalityType);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodePersonalityTypeName", key);
				}
				else
				{
					if (!true)
					{
					}
					string text;
					if (!(str == "Name"))
					{
						text = TaiwuEventTagHandler.Error("DecodePersonalityTypeName", str);
					}
					else
					{
						text = "<Language Key=" + TaiwuEventTagHandler.PersonalityTypeName[(int)personalityType] + "/>";
					}
					if (!true)
					{
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00167328 File Offset: 0x00165528
		private static string DecodeChickenName()
		{
			string key;
			string str;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeChickenName", out key, out str);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short chickenTemplateId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(key, ref chickenTemplateId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeChickenName", key);
				}
				else
				{
					ChickenItem config = Chicken.Instance[chickenTemplateId];
					if (!true)
					{
					}
					string text;
					if (!(str == "Name"))
					{
						if (!(str == "EventDesc"))
						{
							text = TaiwuEventTagHandler.Error("DecodeChickenName", str);
						}
						else
						{
							text = config.EventDesc;
						}
					}
					else
					{
						text = config.Name;
					}
					if (!true)
					{
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x001673E4 File Offset: 0x001655E4
		private static string DecodeProfession()
		{
			string key;
			string str;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeProfession", out key, out str);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short professionTemplateId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(key, ref professionTemplateId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeProfession", key);
				}
				else
				{
					ProfessionItem config = Profession.Instance[(int)professionTemplateId];
					if (!true)
					{
					}
					string text;
					if (!(str == "Name"))
					{
						text = TaiwuEventTagHandler.Error("DecodeProfession", str);
					}
					else
					{
						text = config.Name;
					}
					if (!true)
					{
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00167484 File Offset: 0x00165684
		private static string DecodeOrganizationMemberName()
		{
			string key;
			string str;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeOrganizationMemberName", out key, out str);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short organizationMemberTemplateId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(key, ref organizationMemberTemplateId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeOrganizationMemberName", key);
				}
				else
				{
					OrganizationMemberItem memberConfig = OrganizationMember.Instance[organizationMemberTemplateId];
					string text = str;
					string a = text;
					if (a == "Name")
					{
						bool flag3 = memberConfig != null;
						if (flag3)
						{
							string identityString = memberConfig.GradeName;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
							defaultInterpolatedStringHandler.AppendLiteral("<color=#GradeColor_");
							defaultInterpolatedStringHandler.AppendFormatted<sbyte>(memberConfig.Grade);
							defaultInterpolatedStringHandler.AppendLiteral(">");
							defaultInterpolatedStringHandler.AppendFormatted(identityString);
							defaultInterpolatedStringHandler.AppendLiteral("</color>");
							return defaultInterpolatedStringHandler.ToStringAndClear();
						}
					}
					result = TaiwuEventTagHandler.Error("DecodeOrganizationMemberName", str);
				}
			}
			return result;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x00167584 File Offset: 0x00165784
		private static string DecodeMerchant()
		{
			string key;
			string str;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeMerchant", out key, out str);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short merchantTypeTemplateId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(key, ref merchantTypeTemplateId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeMerchant", key);
				}
				else
				{
					MerchantTypeItem config = MerchantType.Instance[(int)merchantTypeTemplateId];
					if (!true)
					{
					}
					string text;
					if (!(str == "MerchantTypeName"))
					{
						text = TaiwuEventTagHandler.Error("DecodeMerchant", str);
					}
					else
					{
						text = config.Name;
					}
					if (!true)
					{
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x00167624 File Offset: 0x00165824
		private static string DecodeSwordGrave()
		{
			string key;
			string str;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeSwordGrave", out key, out str);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				int adventureTemplateId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(key, ref adventureTemplateId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeSwordGrave", key);
				}
				else
				{
					AdventureItem config = Adventure.Instance[adventureTemplateId];
					if (!true)
					{
					}
					string text;
					if (!(str == "SwordGraveName"))
					{
						text = TaiwuEventTagHandler.Error("DecodeSwordGrave", str);
					}
					else
					{
						text = config.Name;
					}
					if (!true)
					{
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x001676C4 File Offset: 0x001658C4
		private static string DecodeCharacterTitle()
		{
			string key;
			string str;
			string error = TaiwuEventTagHandler.TryGetTagKeyAndValueStr("DecodeCharacterTitle", out key, out str);
			bool flag = !string.IsNullOrEmpty(error);
			string result;
			if (flag)
			{
				result = error;
			}
			else
			{
				short characterTitleTemplateId = -1;
				bool flag2 = !TaiwuEventTagHandler._argBox.Get(key, ref characterTitleTemplateId);
				if (flag2)
				{
					result = TaiwuEventTagHandler.Error("DecodeCharacterTitle", key);
				}
				else
				{
					CharacterTitleItem config = CharacterTitle.Instance[characterTitleTemplateId];
					if (!true)
					{
					}
					string text;
					if (!(str == "Name"))
					{
						text = TaiwuEventTagHandler.Error("DecodeCharacterTitle", str);
					}
					else
					{
						text = config.Name;
					}
					if (!true)
					{
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00167764 File Offset: 0x00165964
		private static string TryGetTagKeyAndValueStr(string methodName, out string key, out string str)
		{
			key = string.Empty;
			str = string.Empty;
			bool flag = !TaiwuEventTagHandler._pairInfos.TryGetValue("key", out key);
			string result;
			if (flag)
			{
				result = TaiwuEventTagHandler.ErrorNoKey(methodName);
			}
			else
			{
				bool flag2 = !TaiwuEventTagHandler._pairInfos.TryGetValue("str", out str);
				if (flag2)
				{
					result = TaiwuEventTagHandler.ErrorNoFieldName(methodName);
				}
				else
				{
					result = string.Empty;
				}
			}
			return result;
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x001677CC File Offset: 0x001659CC
		private static string ErrorNoFieldName(string methodName)
		{
			return "{ErrorNoFieldName: " + methodName + "}";
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x001677F0 File Offset: 0x001659F0
		private static string ErrorNoKey(string methodName)
		{
			return "{ErrorNoKey: " + methodName + "}";
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x00167814 File Offset: 0x00165A14
		private static string Error(string methodName, string fieldName)
		{
			string result;
			if (fieldName != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 2);
				defaultInterpolatedStringHandler.AppendLiteral("{Error: ");
				defaultInterpolatedStringHandler.AppendFormatted(methodName);
				defaultInterpolatedStringHandler.AppendLiteral(", Field: ");
				defaultInterpolatedStringHandler.AppendFormatted(fieldName);
				defaultInterpolatedStringHandler.AppendLiteral("}");
				result = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				result = "{Error: " + methodName + "}";
			}
			return result;
		}

		// Token: 0x0400057F RID: 1407
		private static readonly Regex TagRegex = new Regex("<(?!(/)?color|(/)?size|(/)?link).*?/>");

		// Token: 0x04000580 RID: 1408
		private static readonly Regex TagNameRegex = new Regex("<(?<TagName>(?!(/)?color|(/)?size|(/)?link)[a-z|A-Z]+)( +)?");

		// Token: 0x04000581 RID: 1409
		private static readonly Regex PairRegex = new Regex("(?<Name>(?!(/)?color|(/)?size|(/)?link)[a-z|A-Z]+)( +)?=( +)?(?<Value>[\\w]+)");

		// Token: 0x04000582 RID: 1410
		private static Dictionary<string, string> _pairInfos = new Dictionary<string, string>();

		// Token: 0x04000583 RID: 1411
		private static EventArgBox _argBox;

		// Token: 0x04000584 RID: 1412
		private static TaiwuEvent _handlingEvent;

		// Token: 0x04000585 RID: 1413
		private static Dictionary<string, string[]> _basicSingleGenderMap = new Dictionary<string, string[]>
		{
			{
				"Gender",
				new string[]
				{
					"LK_Common_She",
					"LK_Common_He"
				}
			},
			{
				"GenderSubject",
				new string[]
				{
					"LK_Common_She",
					"LK_Common_He"
				}
			},
			{
				"GenderObject",
				new string[]
				{
					"LK_Common_Her",
					"LK_Common_Him"
				}
			},
			{
				"GenderPossessive",
				new string[]
				{
					"LK_Common_Hers",
					"LK_Common_His"
				}
			},
			{
				"UpperGenderSubject",
				new string[]
				{
					"LK_Common_UpperShe",
					"LK_Common_UpperHe"
				}
			},
			{
				"UpperGenderObject",
				new string[]
				{
					"LK_Common_UpperHer",
					"LK_Common_UpperHim"
				}
			},
			{
				"UpperGenderPossessive",
				new string[]
				{
					"LK_Common_UpperHers",
					"LK_Common_UpperHis"
				}
			},
			{
				"ChildGender",
				new string[]
				{
					"LK_Gender_Child_She",
					"LK_Gender_Child_He"
				}
			},
			{
				"AgedGender",
				new string[]
				{
					"LK_Gender_Aged_She",
					"LK_Gender_Aged_He"
				}
			},
			{
				"AdultGender",
				new string[]
				{
					"LK_Common_Woman",
					"LK_Common_Man"
				}
			},
			{
				"AdoptiveParent",
				new string[]
				{
					"LK_Relation_StepParent_Mother",
					"LK_Relation_StepParent_Father"
				}
			},
			{
				"AdoptiveChild",
				new string[]
				{
					"LK_Relation_StepChild_Daughter",
					"LK_Relation_StepChild_Son"
				}
			},
			{
				"HusbandOrWife",
				new string[]
				{
					"LK_Relation_Bride",
					"LK_Relation_Groom"
				}
			},
			{
				"GenderRanXinduSpecial",
				new string[]
				{
					"LK_Gender_RanXinduSpecial_She",
					"LK_Gender_RanXinduSpecial_He"
				}
			}
		};

		// Token: 0x04000586 RID: 1414
		private static List<string[]> _autoAgeGender = new List<string[]>
		{
			new string[]
			{
				"LK_Gender_Young_She",
				"LK_Gender_Young_He"
			},
			new string[]
			{
				"LK_Gender_Adult_She",
				"LK_Gender_Adult_He"
			},
			new string[]
			{
				"LK_Gender_Old_She",
				"LK_Gender_Old_He"
			}
		};

		// Token: 0x04000587 RID: 1415
		public static List<string[]> _taiwuCrossArchiveAgeGender = new List<string[]>
		{
			new string[]
			{
				"LK_Gender_Child_She",
				"LK_Gender_Child_He"
			},
			new string[]
			{
				"LK_Common_Woman",
				"LK_Common_Man"
			},
			new string[]
			{
				"LK_Gender_Old_She",
				"LK_Gender_Old_He"
			}
		};

		// Token: 0x04000588 RID: 1416
		private static Dictionary<ushort, string[]> _relationGenderMap = new Dictionary<ushort, string[]>
		{
			{
				1,
				new string[]
				{
					"LK_Relation_BloodParent_Mother",
					"LK_Relation_BloodParent_Father"
				}
			},
			{
				2,
				new string[]
				{
					"LK_Relation_BloodChild_Daughter",
					"LK_Relation_BloodChild_Son"
				}
			},
			{
				8,
				new string[]
				{
					"LK_Relation_StepParent_Mother",
					"LK_Relation_StepParent_Father"
				}
			},
			{
				16,
				new string[]
				{
					"LK_Relation_StepChild_Daughter",
					"LK_Relation_StepChild_Son"
				}
			}
		};

		// Token: 0x04000589 RID: 1417
		public static string[] FiveElementTypeName = new string[]
		{
			"LK_FiveElements_Type_0",
			"LK_FiveElements_Type_1",
			"LK_FiveElements_Type_2",
			"LK_FiveElements_Type_3",
			"LK_FiveElements_Type_4",
			"LK_FiveElements_Type_5"
		};

		// Token: 0x0400058A RID: 1418
		public static string[] PersonalityTypeName = new string[]
		{
			"LK_Personality_Calm_Name",
			"LK_Personality_Clever_Name",
			"LK_Personality_Enthusiastic_Name",
			"LK_Personality_Brave_Name",
			"LK_Personality_Firm_Name",
			"LK_Personality_Lucky_Name",
			"LK_Personality_Perceptive_Name"
		};

		// Token: 0x0400058B RID: 1419
		private const string BabyLanguageKey = "LK_Baby";

		// Token: 0x0400058C RID: 1420
		private const string YoungMaleLanguageKey = "LK_Boy";

		// Token: 0x0400058D RID: 1421
		private const string YoungFemaleLanguageKey = "LK_Girl";
	}
}
