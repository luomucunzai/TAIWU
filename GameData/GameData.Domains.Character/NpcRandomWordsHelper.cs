using System.Collections.Generic;
using Config;
using GameData.DLC;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.FunctionDefinition;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character;

public static class NpcRandomWordsHelper
{
	private static bool HasRelation(int charId, int relatedCharId, ushort targetRelationType)
	{
		return DomainManager.Character.HasRelation(charId, relatedCharId, targetRelationType);
	}

	private static bool HasBloodGrandParentRelations(int charId, int relatedCharId)
	{
		return DomainManager.Character.HasBloodGrandParentRelations(charId, relatedCharId);
	}

	public static string RandomContent(this ENpcRandomWordsType type, IRandomSource random, Character npcChar, ENpcRandomWordsType subType = ENpcRandomWordsType.Count)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list2.Clear();
		foreach (NpcRandomWordsItem item in (IEnumerable<NpcRandomWordsItem>)NpcRandomWords.Instance)
		{
			if (item.IsMatch(type, subType) && item.IsMatch(npcChar))
			{
				list.Add(item.TemplateId);
				list2.Add(item.Weight);
			}
		}
		string result = string.Empty;
		if (list.Count > 0)
		{
			int randomIndex = RandomUtils.GetRandomIndex(list2, random);
			short index = list[randomIndex];
			NpcRandomWordsItem word = NpcRandomWords.Instance[index];
			result = word.RandomContent(random);
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
		return result;
	}

	public static string RandomContent(this NpcRandomWordsItem word, IRandomSource random)
	{
		string text = word.Words;
		short templateId = word.TemplateId;
		if (templateId >= 695 && templateId <= 699)
		{
			short num = word.RandomMatchTitle(random);
			Tester.Assert(num >= 0, "Illegal selectedTaiwuTitle");
			CharacterTitleItem characterTitleItem = CharacterTitle.Instance[num];
			MiscItem miscItem = Misc.Instance[characterTitleItem.TemplateId + 208];
			text = text.GetFormat(miscItem.Name);
		}
		if (word.TemplateId == 701)
		{
			short num2 = word.RandomMatchTitle(random);
			Tester.Assert(num2 >= 0, "Illegal selectedTaiwuTitle");
			CharacterTitleItem characterTitleItem2 = CharacterTitle.Instance[num2];
			text = text.GetFormat(characterTitleItem2.Name);
		}
		return text;
	}

	private static short RandomMatchTitle(this NpcRandomWordsItem word, IRandomSource random = null)
	{
		List<short> taiwuTitleLimit = word.TaiwuTitleLimit;
		if (taiwuTitleLimit == null || taiwuTitleLimit.Count <= 0)
		{
			return -1;
		}
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		taiwu.GetTitles(list);
		list.RemoveAll((short x) => !word.TaiwuTitleLimit.Contains(x));
		short result = (short)((list.Count == 0) ? (-1) : ((random != null) ? list.GetRandom(random) : list[0]));
		ObjectPool<List<short>>.Instance.Return(list);
		return result;
	}

	private static bool IsMatch(this NpcRandomWordsItem word, ENpcRandomWordsType type, ENpcRandomWordsType subType)
	{
		return word.Type == type || word.Type == subType;
	}

	private static bool IsMatch(this NpcRandomWordsItem word, Character npcChar)
	{
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (word.SelfGender != -1 && !taiwu.CheckGenderMeetsRequirement(word.SelfGender))
		{
			return false;
		}
		if (word.TargetGender != -1 && !npcChar.CheckGenderMeetsRequirement(word.TargetGender))
		{
			return false;
		}
		if (word.SexualOrientation != -1 && (npcChar.GetCurrAge() < 16 || npcChar.GetGender() != word.TargetGender || !npcChar.CheckSexualOrientationMeetsRequirement(word.SexualOrientation)))
		{
			return false;
		}
		if (!word.CheckAge(taiwu, npcChar))
		{
			return false;
		}
		short morality = npcChar.GetMorality();
		if (morality < word.BehaviorLimit[0] || morality > word.BehaviorLimit[1])
		{
			return false;
		}
		if (!word.CheckFeature(npcChar))
		{
			return false;
		}
		if (!word.CheckProperty(npcChar))
		{
			return false;
		}
		short favorability = DomainManager.Character.GetFavorability(npcChar.GetId(), taiwu.GetId());
		if (favorability < word.FavorLimit[0] || favorability > word.FavorLimit[1])
		{
			return false;
		}
		if (!word.CheckOrganization(npcChar))
		{
			return false;
		}
		if (!word.CheckRelation(taiwu, npcChar))
		{
			return false;
		}
		if (!word.CheckSectStoryTaskStatus())
		{
			return false;
		}
		if (!word.CheckDlc())
		{
			return false;
		}
		if (!word.CheckTitle())
		{
			return false;
		}
		if (!word.CheckTaiwuMapState(taiwu) || !word.CheckTaiwuLocation(taiwu))
		{
			return false;
		}
		if (!word.CheckCharacterMapArea(npcChar) || !word.CheckCharacterSettlement(taiwu))
		{
			return false;
		}
		if (!word.CheckTaskInfos())
		{
			return false;
		}
		sbyte xiangshuProgress = DomainManager.World.GetXiangshuProgress();
		if (xiangshuProgress < word.XiangshuProgressLimit[0] || xiangshuProgress > word.XiangshuProgressLimit[1])
		{
			return false;
		}
		return true;
	}

	private static bool CheckFeature(this NpcRandomWordsItem word, Character npcChar)
	{
		if (word.FeatureLimit.Count > 0)
		{
			bool result = false;
			foreach (short item in word.FeatureLimit)
			{
				if (npcChar.GetFeatureIds().Contains(item))
				{
					result = true;
				}
			}
			return result;
		}
		return true;
	}

	private static bool CheckProperty(this NpcRandomWordsItem word, Character npcChar)
	{
		short[] propertyLimit = word.PropertyLimit;
		if (propertyLimit != null && propertyLimit.Length > 0)
		{
			int propertyValue = npcChar.GetPropertyValue((ECharacterPropertyReferencedType)word.PropertyLimit[0]);
			return EventConditions.PerformOperation(word.PropertyLimit[1], propertyValue, word.PropertyLimit[2]);
		}
		return true;
	}

	private static bool CheckAge(this NpcRandomWordsItem word, Character taiwuChar, Character npcChar)
	{
		if (word.AgeLimit < 0)
		{
			return true;
		}
		short actualAge = taiwuChar.GetActualAge();
		short actualAge2 = npcChar.GetActualAge();
		short ageLimit = word.AgeLimit;
		if (1 == 0)
		{
		}
		bool result = ageLimit switch
		{
			0 => actualAge2 > actualAge, 
			1 => actualAge2 <= actualAge, 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static bool CheckOrganization(this NpcRandomWordsItem word, Character npcChar)
	{
		List<short> organizationGradeLimit = word.OrganizationGradeLimit;
		if (organizationGradeLimit == null || organizationGradeLimit.Count <= 0)
		{
			return true;
		}
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(npcChar.GetOrganizationInfo());
		return word.OrganizationGradeLimit.Contains(orgMemberConfig.TemplateId);
	}

	private static bool CheckRelation(this NpcRandomWordsItem word, Character taiwuChar, Character npcChar)
	{
		sbyte relationLimit = word.RelationLimit;
		if (relationLimit < 0)
		{
			return true;
		}
		int id = taiwuChar.GetId();
		int id2 = npcChar.GetId();
		bool flag = false;
		bool flag5;
		if (DlcManager.IsDlcInstalled(2305890uL))
		{
			bool flag2 = DomainManager.Extra.IsPreviousTaiwuLover(id2);
			LoveDataItem loveDataItem;
			bool flag3 = InteractOfLove.TryGetLoveDataItem(id2, out loveDataItem);
			bool flag4 = flag3 && loveDataItem.InteractTime == 0;
			if (flag4 && relationLimit != 14)
			{
				return false;
			}
			if (1 == 0)
			{
			}
			flag5 = relationLimit switch
			{
				14 => flag4 && !flag2, 
				15 => !flag4 && !flag2, 
				16 => !flag3 && flag2, 
				_ => false, 
			};
			if (1 == 0)
			{
			}
			flag = flag5;
		}
		bool flag6 = HasRelation(id2, id, 16384);
		bool flag7 = HasRelation(id, id2, 16384);
		if (1 == 0)
		{
		}
		flag5 = relationLimit switch
		{
			0 => HasRelation(id2, id, 32768), 
			1 => HasRelation(id2, id, 8192), 
			2 => HasBloodGrandParentRelations(id2, id), 
			3 => HasBloodGrandParentRelations(id, id2), 
			4 => HasRelation(id2, id, 128) || HasRelation(id2, id, 16), 
			5 => HasRelation(id2, id, 2), 
			6 => HasRelation(id2, id, 1024), 
			7 => HasRelation(id2, id, 1), 
			8 => HasRelation(id2, id, 64), 
			9 => HasRelation(id2, id, 8), 
			10 => HasRelation(id2, id, 4) || HasRelation(id2, id, 256) || HasRelation(id2, id, 32), 
			11 => HasRelation(id2, id, 512), 
			12 => flag6 != flag7, 
			13 => flag6 && flag7, 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		bool flag8 = flag5;
		return flag || flag8;
	}

	private static bool CheckSectStoryTaskStatus(this NpcRandomWordsItem word)
	{
		if (word.SectStoryTaskStatus < 0)
		{
			return true;
		}
		sbyte sectStoryOrganizationTemplateId = word.SectStoryOrganizationTemplateId;
		if ((sectStoryOrganizationTemplateId < 1 || sectStoryOrganizationTemplateId >= 16) ? true : false)
		{
			return true;
		}
		return DomainManager.World.GetSectMainStoryTaskStatus(word.SectStoryOrganizationTemplateId) == word.SectStoryTaskStatus;
	}

	private static bool CheckDlc(this NpcRandomWordsItem word)
	{
		if (word.DlcAppId == 0)
		{
			return true;
		}
		if (!DlcManager.IsDlcInstalled(word.DlcAppId))
		{
			return false;
		}
		if (word.DlcAppId == 2764950)
		{
			return DomainManager.Extra.GetFiveLoongDictCount(null) > 0;
		}
		return false;
	}

	private static bool CheckTitle(this NpcRandomWordsItem word)
	{
		List<short> taiwuTitleLimit = word.TaiwuTitleLimit;
		if (taiwuTitleLimit == null || taiwuTitleLimit.Count <= 0)
		{
			return true;
		}
		return word.RandomMatchTitle() >= 0;
	}

	private static bool CheckTaiwuMapState(this NpcRandomWordsItem word, Character taiwuChar)
	{
		if (word.MapState < 0)
		{
			return true;
		}
		Location location = taiwuChar.GetLocation();
		if (location.AreaId < 0)
		{
			return false;
		}
		return word.MapState == DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
	}

	private static bool CheckTaiwuLocation(this NpcRandomWordsItem word, Character taiwuChar)
	{
		if (word.Location == ENpcRandomWordsLocation.None)
		{
			return true;
		}
		Location location = taiwuChar.GetLocation();
		ENpcRandomWordsLocation eNpcRandomWordsLocation = ENpcRandomWordsLocation.Other;
		if (MapAreaData.IsBrokenArea(location.AreaId))
		{
			eNpcRandomWordsLocation = ENpcRandomWordsLocation.Broken;
		}
		else if (location == DomainManager.Taiwu.GetTaiwuVillageLocation())
		{
			eNpcRandomWordsLocation = ENpcRandomWordsLocation.TaiwuVillage;
		}
		else if (location.IsValid())
		{
			MapBlockData block = DomainManager.Map.GetBlock(location);
			if (block.BelongBlockId >= 0)
			{
				block = DomainManager.Map.GetBlock(location.AreaId, block.BelongBlockId);
			}
			EMapBlockType blockType = block.BlockType;
			if (1 == 0)
			{
			}
			ENpcRandomWordsLocation eNpcRandomWordsLocation2 = blockType switch
			{
				EMapBlockType.Sect => ENpcRandomWordsLocation.None, 
				EMapBlockType.City => ENpcRandomWordsLocation.City, 
				EMapBlockType.Town => ENpcRandomWordsLocation.Town, 
				_ => ENpcRandomWordsLocation.Other, 
			};
			if (1 == 0)
			{
			}
			eNpcRandomWordsLocation = eNpcRandomWordsLocation2;
		}
		return eNpcRandomWordsLocation == word.Location;
	}

	private static bool CheckCharacterMapArea(this NpcRandomWordsItem word, Character character)
	{
		if (word.MapAreaTemplateId < 0)
		{
			return true;
		}
		Location location = character.GetLocation();
		if (location.AreaId < 0)
		{
			return false;
		}
		return DomainManager.Map.GetAreaIdByAreaTemplateId(word.MapAreaTemplateId) == location.AreaId;
	}

	private static bool CheckCharacterSettlement(this NpcRandomWordsItem word, Character character)
	{
		if (word.MapAreaTemplateId < 0)
		{
			return true;
		}
		short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId(word.MapAreaTemplateId);
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		Settlement settlement = DomainManager.Organization.GetSettlement(organizationInfo.SettlementId);
		if (settlement.GetLocation().AreaId != areaIdByAreaTemplateId)
		{
			return false;
		}
		OrganizationItem organizationItem = Config.Organization.Instance[organizationInfo.OrgTemplateId];
		if (organizationItem.IsSect != word.IsSectSettlement)
		{
			return false;
		}
		return DomainManager.Map.IsLocationInSettlementInfluenceRange(character.GetLocation(), organizationInfo.SettlementId);
	}

	private static bool CheckTaskInfos(this NpcRandomWordsItem word)
	{
		if (word.NeedTaskInfos.Count == 0)
		{
			return true;
		}
		for (int i = 0; i < word.NeedTaskInfos.Count; i++)
		{
			if (DomainManager.Extra.IsExtraTaskInProgress(word.NeedTaskInfos[i]))
			{
				return true;
			}
		}
		return false;
	}
}
