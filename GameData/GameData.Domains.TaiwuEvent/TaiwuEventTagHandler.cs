using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Config;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.World;

namespace GameData.Domains.TaiwuEvent;

internal class TaiwuEventTagHandler
{
	private static readonly Regex TagRegex = new Regex("<(?!(/)?color|(/)?size|(/)?link).*?/>");

	private static readonly Regex TagNameRegex = new Regex("<(?<TagName>(?!(/)?color|(/)?size|(/)?link)[a-z|A-Z]+)( +)?");

	private static readonly Regex PairRegex = new Regex("(?<Name>(?!(/)?color|(/)?size|(/)?link)[a-z|A-Z]+)( +)?=( +)?(?<Value>[\\w]+)");

	private static Dictionary<string, string> _pairInfos = new Dictionary<string, string>();

	private static EventArgBox _argBox;

	private static TaiwuEvent _handlingEvent;

	private static Dictionary<string, string[]> _basicSingleGenderMap = new Dictionary<string, string[]>
	{
		{
			"Gender",
			new string[2] { "LK_Common_She", "LK_Common_He" }
		},
		{
			"GenderSubject",
			new string[2] { "LK_Common_She", "LK_Common_He" }
		},
		{
			"GenderObject",
			new string[2] { "LK_Common_Her", "LK_Common_Him" }
		},
		{
			"GenderPossessive",
			new string[2] { "LK_Common_Hers", "LK_Common_His" }
		},
		{
			"UpperGenderSubject",
			new string[2] { "LK_Common_UpperShe", "LK_Common_UpperHe" }
		},
		{
			"UpperGenderObject",
			new string[2] { "LK_Common_UpperHer", "LK_Common_UpperHim" }
		},
		{
			"UpperGenderPossessive",
			new string[2] { "LK_Common_UpperHers", "LK_Common_UpperHis" }
		},
		{
			"ChildGender",
			new string[2] { "LK_Gender_Child_She", "LK_Gender_Child_He" }
		},
		{
			"AgedGender",
			new string[2] { "LK_Gender_Aged_She", "LK_Gender_Aged_He" }
		},
		{
			"AdultGender",
			new string[2] { "LK_Common_Woman", "LK_Common_Man" }
		},
		{
			"AdoptiveParent",
			new string[2] { "LK_Relation_StepParent_Mother", "LK_Relation_StepParent_Father" }
		},
		{
			"AdoptiveChild",
			new string[2] { "LK_Relation_StepChild_Daughter", "LK_Relation_StepChild_Son" }
		},
		{
			"HusbandOrWife",
			new string[2] { "LK_Relation_Bride", "LK_Relation_Groom" }
		},
		{
			"GenderRanXinduSpecial",
			new string[2] { "LK_Gender_RanXinduSpecial_She", "LK_Gender_RanXinduSpecial_He" }
		}
	};

	private static List<string[]> _autoAgeGender = new List<string[]>
	{
		new string[2] { "LK_Gender_Young_She", "LK_Gender_Young_He" },
		new string[2] { "LK_Gender_Adult_She", "LK_Gender_Adult_He" },
		new string[2] { "LK_Gender_Old_She", "LK_Gender_Old_He" }
	};

	public static List<string[]> _taiwuCrossArchiveAgeGender = new List<string[]>
	{
		new string[2] { "LK_Gender_Child_She", "LK_Gender_Child_He" },
		new string[2] { "LK_Common_Woman", "LK_Common_Man" },
		new string[2] { "LK_Gender_Old_She", "LK_Gender_Old_He" }
	};

	private static Dictionary<ushort, string[]> _relationGenderMap = new Dictionary<ushort, string[]>
	{
		{
			1,
			new string[2] { "LK_Relation_BloodParent_Mother", "LK_Relation_BloodParent_Father" }
		},
		{
			2,
			new string[2] { "LK_Relation_BloodChild_Daughter", "LK_Relation_BloodChild_Son" }
		},
		{
			8,
			new string[2] { "LK_Relation_StepParent_Mother", "LK_Relation_StepParent_Father" }
		},
		{
			16,
			new string[2] { "LK_Relation_StepChild_Daughter", "LK_Relation_StepChild_Son" }
		}
	};

	public static string[] FiveElementTypeName = new string[6] { "LK_FiveElements_Type_0", "LK_FiveElements_Type_1", "LK_FiveElements_Type_2", "LK_FiveElements_Type_3", "LK_FiveElements_Type_4", "LK_FiveElements_Type_5" };

	public static string[] PersonalityTypeName = new string[7] { "LK_Personality_Calm_Name", "LK_Personality_Clever_Name", "LK_Personality_Enthusiastic_Name", "LK_Personality_Brave_Name", "LK_Personality_Firm_Name", "LK_Personality_Lucky_Name", "LK_Personality_Perceptive_Name" };

	private const string BabyLanguageKey = "LK_Baby";

	private const string YoungMaleLanguageKey = "LK_Boy";

	private const string YoungFemaleLanguageKey = "LK_Girl";

	public static string DecodeTag(string targetString, EventArgBox box, TaiwuEvent handlingEvent)
	{
		if (string.IsNullOrEmpty(targetString))
		{
			return targetString;
		}
		_argBox = box;
		_handlingEvent = handlingEvent;
		string result = TagRegex.Replace(targetString, delegate(Match tagMatch)
		{
			_pairInfos.Clear();
			string value = tagMatch.Value;
			MatchCollection matchCollection = PairRegex.Matches(value);
			foreach (Match item in matchCollection)
			{
				if (item.Groups.Count >= 3)
				{
					_pairInfos.Add(item.Groups["Name"].Value, item.Groups["Value"].Value);
				}
			}
			MatchCollection matchCollection2 = TagNameRegex.Matches(value);
			string text = string.Empty;
			for (int i = 0; i < matchCollection2.Count; i++)
			{
				Match match2 = matchCollection2[i];
				string value2 = match2.Groups["TagName"].Value;
				if (value2 != "color")
				{
					text = value2;
					break;
				}
			}
			return (string)(string.IsNullOrEmpty(text) ? value : ((text switch
			{
				"Character" => DecodeCharacter(), 
				"TemplateCharacter" => DecodeTemplateCharacter(), 
				"CharacterOrActor" => DecodeCharacterOrActor(), 
				"Item" => DecodeItem(), 
				"CombatSkillType" => DecodeCombatSkillType(), 
				"CombatSkill" => DecodeCombatSkill(), 
				"LifeSkillType" => DecodeLifeSkillType(), 
				"LifeSkill" => DecodeLifeSkill(), 
				"ArgBox" => DecodeArgBox(), 
				"Adventure" => DecodeAdventure(), 
				"Actor" => DecodeActor(), 
				"Resource" => DecodeResource(), 
				"NormalInfo" => DecodeNormalInformation(), 
				"SecretInfo" => DecodeSecretInformation(), 
				"Settlement" => DecodeSettlement(), 
				"MapArea" => DecodeMapArea(), 
				"MapState" => DecodeMapState(), 
				"MapBlockName" => DecodeMapBlockName(), 
				"General" => DecodeGeneral(), 
				"Gender" => DecodeGender(), 
				"JiaoLoong" => DecodeJiaoLoong(), 
				"JiaoNurturance" => DecodeJiaoNurturance(), 
				"SkillBook" => DecodeSkillBook(), 
				"CricketName" => DecodeCricketName(), 
				"FiveElement" => DecodeFiveElementName(), 
				"PersonalityType" => DecodePersonalityTypeName(), 
				"Chicken" => DecodeChickenName(), 
				"OrganizationMember" => DecodeOrganizationMemberName(), 
				"Profession" => DecodeProfession(), 
				"Merchant" => DecodeMerchant(), 
				"SwordGrave" => DecodeSwordGrave(), 
				"CharacterTitle" => DecodeCharacterTitle(), 
				_ => value, 
			}) ?? Error("DecodeTag", text)));
		});
		_argBox = null;
		_handlingEvent = null;
		_pairInfos.Clear();
		return result;
	}

	private static string DecodeCricketName()
	{
		if (_pairInfos.TryGetValue("key", out var value))
		{
			short arg = 0;
			short arg2 = 0;
			if (_argBox.Get(value + "_colorId", ref arg) && _argBox.Get(value + "_partId", ref arg2))
			{
				sbyte cricketGrade = ItemTemplateHelper.GetCricketGrade(arg, arg2);
				return $"<color=#GradeColor_{cricketGrade}><Cricket part={arg2} color={arg}/></color>";
			}
			return ErrorNoFieldName("DecodeCricketName");
		}
		return ErrorNoKey("DecodeCricketName");
	}

	private static string DecodeGender()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeGender", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		sbyte arg = 1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeGender", key);
		}
		string text2 = str;
		string text3 = text2;
		if (text3 == "Gender")
		{
			return "<Language Key=" + _basicSingleGenderMap[str][arg] + "/>";
		}
		return Error("DecodeGender", str);
	}

	private static string DecodeArgBox()
	{
		if (!_pairInfos.TryGetValue("key", out var value))
		{
			return ErrorNoKey("DecodeArgBox");
		}
		if (!_pairInfos.TryGetValue("type", out var value2))
		{
			return ErrorNoFieldName("DecodeArgBox");
		}
		switch (value2)
		{
		case "int":
		{
			int arg2 = 0;
			if (_argBox.Get(value, ref arg2))
			{
				return arg2.ToString();
			}
			break;
		}
		case "float":
		{
			float arg3 = 0f;
			if (_argBox.Get(value, ref arg3))
			{
				return arg3.ToString(CultureInfo.CurrentCulture);
			}
			break;
		}
		case "string":
		{
			string arg = string.Empty;
			if (_argBox.Get(value, ref arg))
			{
				return arg;
			}
			break;
		}
		}
		return Error("DecodeArgBox", value2);
	}

	private static string DecodeTemplateCharacter()
	{
		string text = _pairInfos["key"];
		short arg = -1;
		if (!_argBox.Get(text, ref arg))
		{
			return Error("DecodeTemplateCharacter", text);
		}
		CharacterItem characterItem = Config.Character.Instance[arg];
		if (characterItem == null)
		{
			return Error("DecodeTemplateCharacter", text);
		}
		string text2 = _pairInfos["str"];
		string text3 = text2;
		string text4 = text3;
		if (text4 == "Name")
		{
			if (GameData.Domains.World.SharedMethods.SmallVillageXiangshu(characterItem.OrganizationInfo.OrgTemplateId, includeXiangshuInfected: false))
			{
				return characterItem.AnonymousTitle;
			}
			return characterItem.Surname + characterItem.GivenName;
		}
		return Error("DecodeTemplateCharacter", text2);
	}

	private static string DecodeCharacter()
	{
		string text = _pairInfos["key"];
		GameData.Domains.Character.Character character = _argBox.GetCharacter(text);
		DeadCharacter deadCharacter = null;
		int arg = -1;
		if (character == null)
		{
			if (_argBox.Get(text, ref arg))
			{
				deadCharacter = DomainManager.Character.GetDeadCharacter(arg);
			}
		}
		else
		{
			arg = character.GetId();
		}
		if (character == null && deadCharacter == null)
		{
			return Error("DecodeCharacter", text);
		}
		sbyte b = character?.GetGender() ?? deadCharacter.Gender;
		AvatarData avatarData = ((character != null) ? character.GetAvatar() : deadCharacter.Avatar);
		string text2 = _pairInfos["str"];
		switch (text2)
		{
		case "Name":
		{
			(string, string) monasticTitleOrDisplayName = DomainManager.Character.GetNameRelatedData(arg).GetMonasticTitleOrDisplayName(arg == DomainManager.Taiwu.GetTaiwuCharId());
			return monasticTitleOrDisplayName.Item1 + monasticTitleOrDisplayName.Item2;
		}
		case "Gender":
		case "ChildGender":
		case "AgedGender":
		case "AdultGender":
		case "AdoptiveParent":
		case "AdoptiveChild":
		case "HusbandOrWife":
		case "GenderSubject":
		case "GenderObject":
		case "GenderPossessive":
		case "UpperGenderSubject":
		case "UpperGenderObject":
		case "UpperGenderPossessive":
		case "GenderRanXinduSpecial":
			return "<Language Key=" + _basicSingleGenderMap[text2][b] + "/>";
		case "AutoAgeGender":
		{
			short num2 = character?.GetPhysiologicalAge() ?? deadCharacter.GetActualAge();
			sbyte index = 0;
			if (num2 >= 16)
			{
				index = 1;
			}
			if (num2 >= GlobalConfig.Instance.AgeShowWrinkle2)
			{
				index = 2;
			}
			return "<Language Key=" + _autoAgeGender[index][b] + "/>";
		}
		case "Relation":
		{
			if (_pairInfos.TryGetValue("relationKey", out var value2))
			{
				GameData.Domains.Character.Character character2 = _argBox.GetCharacter(value2);
				if (character2 != null && character != null && DomainManager.Character.TryGetRelation(character.GetId(), character2.GetId(), out var relation) && _relationGenderMap.TryGetValue(relation.RelationType, out var value3))
				{
					return "<Language Key=" + value3[character2.GetGender()] + "/>";
				}
			}
			break;
		}
		case "OrgName":
		{
			OrganizationInfo organizationInfo2 = character?.GetOrganizationInfo() ?? deadCharacter.OrganizationInfo;
			OrganizationItem item4 = Config.Organization.Instance.GetItem(organizationInfo2.OrgTemplateId);
			if (item4 != null)
			{
				return item4.Name;
			}
			break;
		}
		case "AreaName":
		{
			OrganizationInfo organizationInfo = character?.GetOrganizationInfo() ?? deadCharacter.OrganizationInfo;
			Location location = DomainManager.Organization.GetSettlement(organizationInfo.SettlementId).GetLocation();
			return DomainManager.Map.GetStateAndAreaNameByAreaId(location.AreaId).areaName;
		}
		case "CurAreaName":
			if (character != null)
			{
				Location location2 = character.GetLocation();
				if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId() && DomainManager.Map.IsTraveling)
				{
					location2 = DomainManager.Map.GetTravelCurrLocation();
				}
				return DomainManager.Map.GetStateAndAreaNameByAreaId(location2.AreaId).areaName;
			}
			break;
		case "SettlementName":
		{
			OrganizationInfo organizationInfo3 = character?.GetOrganizationInfo() ?? deadCharacter.OrganizationInfo;
			if (organizationInfo3.SettlementId < 0)
			{
				return Config.Organization.Instance[(sbyte)0].Name;
			}
			Settlement settlement = DomainManager.Organization.GetSettlement(organizationInfo3.SettlementId);
			short num = (short)((settlement is CivilianSettlement civilianSettlement) ? civilianSettlement.GetRandomNameId() : (-1));
			MapBlockData rootBlock = DomainManager.Map.GetBlock(settlement.GetLocation()).GetRootBlock();
			return (num != -1) ? LocalTownNames.Instance.TownNameCore[num].Name : ((rootBlock.TemplateId != -1) ? MapBlock.Instance[rootBlock.TemplateId].Name : Config.Organization.Instance[(sbyte)0].Name);
		}
		case "Identity":
		{
			short num3 = character?.GetCurrAge() ?? deadCharacter.GetActualAge();
			OrganizationInfo orgInfo = character?.GetOrganizationInfo() ?? deadCharacter.OrganizationInfo;
			OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			if (orgMemberConfig != null)
			{
				if (num3 >= orgMemberConfig.IdentityActiveAge)
				{
					string value4 = (orgInfo.Principal ? orgMemberConfig.GradeName : orgMemberConfig.SpouseAnonymousTitles[b]);
					return $"<color=#GradeColor_{orgInfo.Grade}>{value4}</color>";
				}
				if (AgeGroup.GetAgeGroup(num3) == 0)
				{
					return "<Language Key=LK_Baby/>";
				}
				if (b == 1)
				{
					return "<Language Key=LK_Boy/>";
				}
				return "<Language Key=LK_Girl/>";
			}
			break;
		}
		case "CurBlock":
			if (character != null)
			{
				Location key = character.GetLocation();
				if (!key.IsValid())
				{
					key = character.GetValidLocation();
				}
				MapBlockItem config = DomainManager.Map.GetBlock(key).GetConfig();
				return config.Name;
			}
			break;
		case "BodyType":
		{
			AvatarHeadItem headConfig = AvatarManager.Instance.GetAsset(avatarData.AvatarId, EAvatarElementsType.Head, avatarData.HeadId).HeadConfig;
			if (headConfig != null)
			{
				return headConfig.DisplayDesc;
			}
			break;
		}
		case "ClothColor":
		{
			AvatarClothColorsItem item3 = AvatarClothColors.Instance.GetItem(avatarData.ColorClothId);
			if (item3 != null)
			{
				return item3.DisplayDesc;
			}
			break;
		}
		case "ClothName":
			if (character != null)
			{
				short templateId = character.GetTemplateId();
				CharacterItem characterItem = Config.Character.Instance[templateId];
				return Config.Clothing.Instance[characterItem.PresetEquipment[4].TemplateId].Name;
			}
			break;
		case "SkinColor":
		{
			AvatarSkinColorsItem item2 = AvatarSkinColors.Instance.GetItem(avatarData.ColorSkinId);
			if (item2 != null)
			{
				return item2.DisplayDesc;
			}
			break;
		}
		case "LipColor":
		{
			AvatarLipColorsItem item = AvatarLipColors.Instance.GetItem(avatarData.ColorMouthId);
			if (item != null)
			{
				return item.DisplayDesc;
			}
			break;
		}
		case "FrontHairColor":
		{
			AvatarHairColorsItem item12 = AvatarHairColors.Instance.GetItem(avatarData.ColorFrontHairId);
			if (item12 != null)
			{
				return item12.DisplayDesc;
			}
			break;
		}
		case "BackHairColor":
		{
			AvatarHairColorsItem item11 = AvatarHairColors.Instance.GetItem(avatarData.ColorBackHairId);
			if (item11 != null)
			{
				return item11.DisplayDesc;
			}
			break;
		}
		case "Beard1Color":
		{
			AvatarHairColorsItem item10 = AvatarHairColors.Instance.GetItem(avatarData.ColorBeard1Id);
			if (item10 != null)
			{
				return item10.DisplayDesc;
			}
			break;
		}
		case "Beard2Color":
		{
			AvatarHairColorsItem item9 = AvatarHairColors.Instance.GetItem(avatarData.ColorBeard2Id);
			if (item9 != null)
			{
				return item9.DisplayDesc;
			}
			break;
		}
		case "EyeBrowColor":
		{
			AvatarHairColorsItem item8 = AvatarHairColors.Instance.GetItem(avatarData.ColorEyebrowId);
			if (item8 != null)
			{
				return item8.DisplayDesc;
			}
			break;
		}
		case "EyeballColor":
		{
			AvatarEyeballColorsItem item7 = AvatarEyeballColors.Instance.GetItem(avatarData.ColorEyeballId);
			if (item7 != null)
			{
				return item7.DisplayDesc;
			}
			break;
		}
		case "Feature1Color":
		{
			AvatarFeatureColorsItem item6 = AvatarFeatureColors.Instance.GetItem(avatarData.ColorFeature1Id);
			if (item6 != null)
			{
				return item6.DisplayDesc;
			}
			break;
		}
		case "Feature2Color":
		{
			AvatarFeatureColorsItem item5 = AvatarFeatureColors.Instance.GetItem(avatarData.ColorFeature2Id);
			if (item5 != null)
			{
				return item5.DisplayDesc;
			}
			break;
		}
		case "NicknameOfTaiwu":
		{
			if (DomainManager.Extra.TryGetInteractOfLoveData(arg, out var loveDataItem))
			{
				if (!loveDataItem.EventTimeDict.ContainsKey(25))
				{
					return "<Language Key=LK_You/>";
				}
				if (DomainManager.World.GetCustomTexts().TryGetValue(loveDataItem.TaiwuNicknameId, out var value) && !string.IsNullOrEmpty(value))
				{
					return value;
				}
			}
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			_handlingEvent.NeedNameRelatedDataCharacterIdList.Add(taiwuCharId);
			return $"<CharNickname Id={taiwuCharId} />";
		}
		case "Profession":
		{
			ProfessionData characterCurrentProfession = DomainManager.Extra.GetCharacterCurrentProfession(arg);
			if (characterCurrentProfession != null)
			{
				return characterCurrentProfession.GetConfig().Name;
			}
			break;
		}
		}
		return Error("DecodeCharacter", text2);
	}

	private static string DecodeItem()
	{
		sbyte value = 0;
		short result = 0;
		if (_pairInfos.TryGetValue("key", out var value2))
		{
			if (!_argBox.Get<ItemKey>(value2, out ItemKey arg))
			{
				return Error("DecodeItem", value2);
			}
			value = arg.ItemType;
			result = arg.TemplateId;
		}
		else
		{
			if (!_pairInfos.TryGetValue("type", out var value3) || !_pairInfos.TryGetValue("id", out var value4))
			{
				return ErrorNoKey("DecodeItem");
			}
			if (!ItemType.TypeName2TypeId.TryGetValue(value3, out value))
			{
				return Error("DecodeItem", value3);
			}
			if (!short.TryParse(value4, out result))
			{
				return Error("DecodeItem", value4);
			}
		}
		string text = string.Empty;
		if (_pairInfos.TryGetValue("str", out var value5))
		{
			if (value == 11 && !string.IsNullOrEmpty(value2) && _argBox.Get<ItemKey>(value2, out ItemKey arg2))
			{
				if (!(DomainManager.Item.GetBaseItem(arg2) is GameData.Domains.Item.Cricket cricket))
				{
					return "{Cricket decode error}";
				}
				string text2 = value5;
				string text3 = text2;
				if (!(text3 == "Name"))
				{
					if (text3 == "ColorName")
					{
						sbyte grade = ItemTemplateHelper.GetGrade(value, result);
						text = $"<color=#GradeColor_{grade}><Cricket part={cricket.GetPartId()} color={cricket.GetColorId()}/></color>";
					}
				}
				else
				{
					text = $"<Cricket part={cricket.GetPartId()} color={cricket.GetColorId()}/>";
				}
			}
			else
			{
				string text4 = value5;
				string text5 = text4;
				if (!(text5 == "Name"))
				{
					if (text5 == "ColorName")
					{
						sbyte grade2 = ItemTemplateHelper.GetGrade(value, result);
						string name = ItemTemplateHelper.GetName(value, result);
						text = $"<color=#GradeColor_{grade2}>{name}</color>";
					}
				}
				else
				{
					text = ItemTemplateHelper.GetName(value, result);
				}
			}
		}
		if (_pairInfos.TryGetValue("sp", out var value6))
		{
			string text6 = value6;
			string text7 = text6;
			if (text7 == "Icon")
			{
				text = text + "<SpName=\"" + ItemTemplateHelper.GetIcon(value, result) + "\"";
			}
		}
		return text;
	}

	private static string DecodeCombatSkill()
	{
		string text = _pairInfos["key"];
		short arg = -1;
		if (!_argBox.Get(text, ref arg))
		{
			return Error("DecodeCombatSkill", text);
		}
		CombatSkillItem item = Config.CombatSkill.Instance.GetItem(arg);
		string text2 = _pairInfos["str"];
		switch (text2)
		{
		case "SkillName":
			if (item != null)
			{
				return $"《<color=#GradeColor_{item.Grade}>{item.Name}</color>》";
			}
			break;
		case "SkillDesc":
			if (item != null)
			{
				return item.Desc;
			}
			break;
		case "TypeName":
		{
			CombatSkillTypeItem item2 = CombatSkillType.Instance.GetItem(item.Type);
			if (item2 != null)
			{
				return item2.Name;
			}
			break;
		}
		case "SkillBookName":
		{
			SkillBookItem skillBookItem = Config.SkillBook.Instance[item.BookId];
			if (skillBookItem != null)
			{
				return $"<color=#GradeColor_{skillBookItem.Grade}>{skillBookItem.Name}</color>";
			}
			break;
		}
		}
		return Error("DecodeCombatSkill", text2);
	}

	private static string DecodeCombatSkillType()
	{
		if (!_pairInfos.TryGetValue("key", out var value))
		{
			return "{CombatSKillType key not find}";
		}
		sbyte arg = -1;
		if (!_argBox.Get(value, ref arg))
		{
			return Error("DecodeCombatSkillType", value);
		}
		CombatSkillTypeItem item = CombatSkillType.Instance.GetItem(arg);
		string text = _pairInfos["str"];
		string text2 = text;
		string text3 = text2;
		if (text3 == "Name" && item != null)
		{
			return item.Name;
		}
		return Error("DecodeCombatSkillType", text);
	}

	private static string DecodeLifeSkill()
	{
		string text = _pairInfos["key"];
		short arg = -1;
		if (!_argBox.Get(text, ref arg))
		{
			return Error("DecodeLifeSkill", text);
		}
		Config.LifeSkillItem item = LifeSkill.Instance.GetItem(arg);
		string text2 = _pairInfos["str"];
		switch (text2)
		{
		case "SkillName":
			if (item != null)
			{
				return $"《<color=#GradeColor_{item.Grade}>{item.Name}</color>》";
			}
			break;
		case "SkillDesc":
			if (item != null)
			{
				return item.Desc;
			}
			break;
		case "TypeName":
		{
			LifeSkillTypeItem item2 = Config.LifeSkillType.Instance.GetItem(item.Type);
			if (item2 != null)
			{
				return item2.Name;
			}
			break;
		}
		}
		return Error("DecodeLifeSkill", text2);
	}

	private static string DecodeLifeSkillType()
	{
		if (!_pairInfos.TryGetValue("key", out var value))
		{
			return "{LifeSkillType key not find}";
		}
		sbyte arg = -1;
		if (!_argBox.Get(value, ref arg))
		{
			return Error("DecodeLifeSkillType", value);
		}
		LifeSkillTypeItem item = Config.LifeSkillType.Instance.GetItem(arg);
		string text = _pairInfos["str"];
		string text2 = text;
		string text3 = text2;
		if (text3 == "Name" && item != null)
		{
			return item.Name;
		}
		return Error("DecodeLifeSkillType", text);
	}

	private static string DecodeAdventure()
	{
		if (!_argBox.Get("AdventurePoint", out AdventureMapPoint arg))
		{
			return Error("DecodeAdventure", "AdventurePoint");
		}
		string text = _pairInfos["key"];
		string text2 = _pairInfos["str"];
		if (text == "Point")
		{
			string text3 = text2;
			string text4 = text3;
			if (text4 == "Name")
			{
				AdventureTerrainItem item = AdventureTerrain.Instance.GetItem((sbyte)arg.TerrainId);
				if (item != null)
				{
					return item.Name;
				}
			}
		}
		else if (text == "Param")
		{
			string key = _pairInfos["paramKey"];
			string text5 = text2;
			string text6 = text5;
			if (text6 == "Name")
			{
				return DomainManager.Adventure.GetAdvParameterConfig(key).Item2;
			}
			if (text6 == "Value")
			{
				return DomainManager.Adventure.GetAdvParameter(key).ToString();
			}
		}
		return Error("DecodeAdventure", text2);
	}

	private static string DecodeCharacterOrActor()
	{
		string text = _pairInfos["key"];
		if (_argBox.Contains<int>(text))
		{
			return DecodeCharacter();
		}
		if (_argBox.Contains<EventActorData>(text))
		{
			return DecodeActor();
		}
		return "{Unable to decode character or actor with key " + text;
	}

	private static string DecodeActor()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeActor", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		if (!_argBox.Get(key, out EventActorData arg))
		{
			return Error("DecodeActor", key);
		}
		switch (str)
		{
		case "Name":
			return arg.DisplayName;
		case "Age":
			return arg.Age.ToString();
		case "ChildGender":
		case "AdultGender":
			return "<Language Key=" + _basicSingleGenderMap[str][arg.Gender] + "/>";
		default:
			return Error("DecodeActor", str);
		}
	}

	private static string DecodeResource()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeResource", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		int arg = 0;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeResource", key);
		}
		ResourceTypeItem item = Config.ResourceType.Instance.GetItem((sbyte)arg);
		if (item == null)
		{
			return Error("DecodeResource", key);
		}
		string text2 = str;
		string text3 = text2;
		if (text3 == "Name")
		{
			return item.Name;
		}
		return Error("DecodeResource", str);
	}

	private static string DecodeNormalInformation()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeNormalInformation", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		if (!_argBox.Get<NormalInformation>(key, out NormalInformation arg))
		{
			return Error("DecodeNormalInformation", key);
		}
		InformationItem item = Config.Information.Instance.GetItem(arg.TemplateId);
		InformationInfoItem item2 = InformationInfo.Instance.GetItem(item.InfoIds[arg.Level]);
		switch (str)
		{
		case "Belong":
			if (item.Type == 0 || item.Type == 1)
			{
				OrganizationItem item3 = Config.Organization.Instance.GetItem(item2.Oraganization);
				return item3.Name;
			}
			if (item.Type == 2)
			{
				LifeSkillTypeItem item4 = Config.LifeSkillType.Instance.GetItem(item2.LifeSkillType);
				return item4.Name;
			}
			return "<Language Key=LK_InformationType_West/>";
		case "Desc":
			return item2.Desc;
		case "AnsEffective":
			return item2.EffectiveAnswer;
		case "AnsNormal":
			return item2.NormalAnswer;
		case "AnsInvalid":
			return item2.InvalidAnswer;
		default:
			return Error("DecodeNormalInformation", str);
		}
	}

	private static string DecodeSecretInformation()
	{
		return "{secret information decode error}";
	}

	private static string DecodeSettlement()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeSettlement", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeSettlement", key);
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(arg);
		switch (str)
		{
		case "Name":
		{
			short num2 = (short)((settlement is CivilianSettlement civilianSettlement) ? civilianSettlement.GetRandomNameId() : (-1));
			MapBlockData rootBlock = DomainManager.Map.GetBlock(settlement.GetLocation()).GetRootBlock();
			string value = ((num2 != -1) ? LocalTownNames.Instance.TownNameCore[num2].Name : ((rootBlock.TemplateId != -1) ? MapBlock.Instance[rootBlock.TemplateId].Name : Config.Organization.Instance[(sbyte)0].Name));
			return $"<color=#GradeColor_{7}>{value}</color>";
		}
		case "PrisonName":
		{
			OrganizationItem organizationItem = Config.Organization.Instance[settlement.GetOrgTemplateId()];
			return BuildingBlock.Instance[organizationItem.PrisonBuilding].Name;
		}
		case "TreasuryName":
		{
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			if (1 == 0)
			{
			}
			short num;
			switch (orgTemplateId)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
				num = (short)(settlement.GetOrgTemplateId() - 1 + 288);
				break;
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
				num = 284;
				break;
			case 36:
				num = 286;
				break;
			case 37:
				num = 285;
				break;
			case 38:
				num = 287;
				break;
			default:
				num = -1;
				break;
			}
			if (1 == 0)
			{
			}
			short index = num;
			return BuildingBlock.Instance[index].Name;
		}
		case "MapAreaName":
		{
			Location location2 = settlement.GetLocation();
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(location2.AreaId);
			return element_Areas.GetConfig().Name;
		}
		case "MapStateName":
		{
			Location location = settlement.GetLocation();
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
			return MapState.Instance[stateTemplateIdByAreaId].Name;
		}
		default:
			return Error("DecodeSettlement", str);
		}
	}

	private static string DecodeMapArea()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeMapArea", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (_argBox.Get(key, out MapAreaData arg2))
		{
			arg = arg2.GetAreaId();
		}
		else if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeMapArea", key);
		}
		string text2 = str;
		string text3 = text2;
		if (!(text3 == "Name"))
		{
			if (text3 == "StateName")
			{
				return DomainManager.Map.GetStateAndAreaNameByAreaId(arg).stateName;
			}
			return Error("DecodeMapArea", str);
		}
		string item = DomainManager.Map.GetStateAndAreaNameByAreaId(arg).areaName;
		return $"<color=#GradeColor_{7}>{item}</color>";
	}

	private static string DecodeMapState()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeMapState", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		sbyte arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeMapState", key);
		}
		string text2 = str;
		string text3 = text2;
		if (text3 == "Name")
		{
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(arg);
			return MapState.Instance[stateTemplateIdByAreaId].Name;
		}
		return Error("DecodeMapState", str);
	}

	public static string DecodeMapBlockName()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeMapBlockName", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		Location arg = Location.Invalid;
		if (!_argBox.Get<Location>(key, out arg))
		{
			return Error("DecodeMapBlockName", key);
		}
		string text2 = str;
		string text3 = text2;
		if (text3 == "Name")
		{
			if (!arg.IsValid())
			{
				return MapBlock.Instance[117].Name;
			}
			return DomainManager.Map.GetBlock(arg).GetConfig().Name;
		}
		return Error("DecodeMapBlockName", str);
	}

	private static string DecodeGeneral()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeGeneral", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		switch (str)
		{
		case "ItemSubType":
			return "<Language Key=LK_ItemSubType_" + key + "/>";
		case "BehaviorType":
			return "<Language Key=LK_Goodness_" + key + "/>";
		case "UpperNum":
		{
			int arg2 = 1;
			_argBox.Get("Grade", ref arg2);
			return $"<Language Key=LK_Num_{arg2}/>";
		}
		case "CombatSkillOutlineType":
		{
			sbyte arg = 0;
			_argBox.Get("CombatSkillOutlineType", ref arg);
			return $"<Language Key=LK_CombatSkill_First_Page_Type_{arg}/>";
		}
		default:
			return Error("DecodeGeneral", str);
		}
	}

	private static string DecodeJiaoLoong()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeJiaoLoong", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		int arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeJiaoLoong", key);
		}
		string text2 = str;
		string text3 = text2;
		if (!(text3 == "JiaoLoongName"))
		{
			if (text3 == "ChildrenOfLoongLoongName")
			{
				string result = string.Empty;
				if (DomainManager.Extra.TryGetElement_ChildrenOfLoong(arg, out var value))
				{
					if (value.NameId < 0)
					{
						result = ItemTemplateHelper.GetName(value.Key.ItemType, value.Key.TemplateId);
					}
					else
					{
						IReadOnlyDictionary<int, string> customTexts = DomainManager.World.GetCustomTexts();
						if (customTexts.TryGetValue(value.NameId, out var value2))
						{
							result = value2;
						}
					}
				}
				return result;
			}
			return Error("DecodeJiaoLoong", str);
		}
		string result2 = string.Empty;
		if (DomainManager.Extra.TryGetElement_Jiaos(arg, out var value3))
		{
			if (value3.NameId < 0)
			{
				result2 = ItemTemplateHelper.GetName(value3.Key.ItemType, value3.Key.TemplateId);
			}
			else
			{
				IReadOnlyDictionary<int, string> customTexts2 = DomainManager.World.GetCustomTexts();
				if (customTexts2.TryGetValue(value3.NameId, out var value4))
				{
					result2 = value4;
				}
			}
		}
		return result2;
	}

	private static string DecodeJiaoNurturance()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeJiaoNurturance", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeJiaoNurturance", key);
		}
		JiaoNurturanceItem jiaoNurturanceItem = JiaoNurturance.Instance[arg];
		if (1 == 0)
		{
		}
		string result = ((!(str == "Name")) ? Error("DecodeJiaoNurturance", str) : jiaoNurturanceItem.Name);
		if (1 == 0)
		{
		}
		return result;
	}

	private static string DecodeSkillBook()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeCharacterTitle", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeSkillBook", key);
		}
		SkillBookItem skillBookItem = Config.SkillBook.Instance[arg];
		if (1 == 0)
		{
		}
		string result = ((!(str == "Name")) ? Error("DecodeSkillBook", str) : skillBookItem.Name);
		if (1 == 0)
		{
		}
		return result;
	}

	private static string DecodeFiveElementName()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeFiveElementName", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeFiveElementName", key);
		}
		if (1 == 0)
		{
		}
		string result = ((!(str == "Name")) ? Error("DecodeFiveElementName", str) : ("<Language Key=" + FiveElementTypeName[arg] + "/>"));
		if (1 == 0)
		{
		}
		return result;
	}

	private static string DecodePersonalityTypeName()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodePersonalityTypeName", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodePersonalityTypeName", key);
		}
		if (1 == 0)
		{
		}
		string result = ((!(str == "Name")) ? Error("DecodePersonalityTypeName", str) : ("<Language Key=" + PersonalityTypeName[arg] + "/>"));
		if (1 == 0)
		{
		}
		return result;
	}

	private static string DecodeChickenName()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeChickenName", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeChickenName", key);
		}
		ChickenItem chickenItem = Chicken.Instance[arg];
		if (1 == 0)
		{
		}
		string result = ((str == "Name") ? chickenItem.Name : ((!(str == "EventDesc")) ? Error("DecodeChickenName", str) : chickenItem.EventDesc));
		if (1 == 0)
		{
		}
		return result;
	}

	private static string DecodeProfession()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeProfession", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeProfession", key);
		}
		ProfessionItem professionItem = Profession.Instance[arg];
		if (1 == 0)
		{
		}
		string result = ((!(str == "Name")) ? Error("DecodeProfession", str) : professionItem.Name);
		if (1 == 0)
		{
		}
		return result;
	}

	private static string DecodeOrganizationMemberName()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeOrganizationMemberName", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeOrganizationMemberName", key);
		}
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[arg];
		string text2 = str;
		string text3 = text2;
		if (text3 == "Name" && organizationMemberItem != null)
		{
			string gradeName = organizationMemberItem.GradeName;
			return $"<color=#GradeColor_{organizationMemberItem.Grade}>{gradeName}</color>";
		}
		return Error("DecodeOrganizationMemberName", str);
	}

	private static string DecodeMerchant()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeMerchant", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeMerchant", key);
		}
		MerchantTypeItem merchantTypeItem = MerchantType.Instance[arg];
		if (1 == 0)
		{
		}
		string result = ((!(str == "MerchantTypeName")) ? Error("DecodeMerchant", str) : merchantTypeItem.Name);
		if (1 == 0)
		{
		}
		return result;
	}

	private static string DecodeSwordGrave()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeSwordGrave", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		int arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeSwordGrave", key);
		}
		AdventureItem adventureItem = Config.Adventure.Instance[arg];
		if (1 == 0)
		{
		}
		string result = ((!(str == "SwordGraveName")) ? Error("DecodeSwordGrave", str) : adventureItem.Name);
		if (1 == 0)
		{
		}
		return result;
	}

	private static string DecodeCharacterTitle()
	{
		string key;
		string str;
		string text = TryGetTagKeyAndValueStr("DecodeCharacterTitle", out key, out str);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		short arg = -1;
		if (!_argBox.Get(key, ref arg))
		{
			return Error("DecodeCharacterTitle", key);
		}
		CharacterTitleItem characterTitleItem = CharacterTitle.Instance[arg];
		if (1 == 0)
		{
		}
		string result = ((!(str == "Name")) ? Error("DecodeCharacterTitle", str) : characterTitleItem.Name);
		if (1 == 0)
		{
		}
		return result;
	}

	private static string TryGetTagKeyAndValueStr(string methodName, out string key, out string str)
	{
		key = string.Empty;
		str = string.Empty;
		if (!_pairInfos.TryGetValue("key", out key))
		{
			return ErrorNoKey(methodName);
		}
		if (!_pairInfos.TryGetValue("str", out str))
		{
			return ErrorNoFieldName(methodName);
		}
		return string.Empty;
	}

	private static string ErrorNoFieldName(string methodName)
	{
		return "{ErrorNoFieldName: " + methodName + "}";
	}

	private static string ErrorNoKey(string methodName)
	{
		return "{ErrorNoKey: " + methodName + "}";
	}

	private static string Error(string methodName, string fieldName)
	{
		return (fieldName == null) ? ("{Error: " + methodName + "}") : $"{{Error: {methodName}, Field: {fieldName}}}";
	}
}
