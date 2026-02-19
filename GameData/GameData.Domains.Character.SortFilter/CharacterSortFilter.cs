using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Config;
using GameData.Domains.Character.Relation;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.World.SectMainStory;
using GameData.Utilities;

namespace GameData.Domains.Character.SortFilter;

public class CharacterSortFilter
{
	private readonly List<CharacterSortingInfo> _characters;

	private readonly List<CharacterSortingInfo>[] _filteredCharacters;

	private readonly Encoding _currEncoding;

	private CharacterSortFilterSettings _settings;

	private List<int> _characterCounts;

	private List<int> _maxSortingTypeCharIds;

	private HashSet<int> _addedCharIdSet;

	private static readonly Dictionary<string, Encoding> LanguageEncodingDict = new Dictionary<string, Encoding>
	{
		{
			"CN",
			Encoding.GetEncoding("GBK")
		},
		{
			"CNH",
			Encoding.GetEncoding("GBK")
		},
		{
			"EN",
			Encoding.ASCII
		},
		{
			"JP",
			Encoding.Unicode
		}
	};

	public CharacterSortFilterSettings Settings => _settings;

	public CharacterSortFilter(CharacterSortFilterSettings settings, string language)
	{
		_characters = new List<CharacterSortingInfo>();
		_addedCharIdSet = new HashSet<int>();
		if (settings.FilterSubType >= 0)
		{
			_filteredCharacters = new List<CharacterSortingInfo>[CharacterFilterSubType.SubTypeToFilterCount[settings.FilterSubType]];
			for (int i = 0; i < _filteredCharacters.Length; i++)
			{
				_filteredCharacters[i] = new List<CharacterSortingInfo>();
			}
		}
		_currEncoding = LanguageEncodingDict[language];
		_settings = settings;
		FindByFilter();
	}

	public List<int> GetCounts()
	{
		return _characterCounts;
	}

	public void UpdateCounts()
	{
		if (_characterCounts == null)
		{
			_characterCounts = new List<int>();
		}
		_characterCounts.Clear();
		_characterCounts.Add(_characters.Count);
		if (_filteredCharacters != null)
		{
			for (int i = 0; i < _filteredCharacters.Length; i++)
			{
				_characterCounts.Add(_filteredCharacters[i].Count);
			}
		}
	}

	public List<int> GetMaxSortingTypeCharIds(List<int> sortingTypes, sbyte filterSubId)
	{
		if (_characterCounts == null)
		{
			_characterCounts = new List<int>();
		}
		_characterCounts.Clear();
		for (int i = 0; i < sortingTypes.Count; i++)
		{
			_characterCounts.Add(-1);
		}
		List<CharacterSortingInfo> characters = ((filterSubId < 0) ? _characters : _filteredCharacters[filterSubId]);
		if (characters.Count == 0)
		{
			return _characterCounts;
		}
		Parallel.For(0, sortingTypes.Count, delegate(int index)
		{
			int sortingType = sortingTypes[index];
			CharacterSortingInfo characterSortingInfo = characters.Max((CharacterSortingInfo a, CharacterSortingInfo b) => CompareValue(a, b, sortingType));
			_characterCounts[index] = characterSortingInfo.Character.GetId();
		});
		return _characterCounts;
	}

	public CharacterList FindByName(string name)
	{
		CharacterList result = default(CharacterList);
		List<CharacterSortingInfo> list = ((_settings.FilterSubId < 0) ? _characters : _filteredCharacters[_settings.FilterSubId]);
		foreach (CharacterSortingInfo item in list)
		{
			string text = _currEncoding.GetString(item.NameBytes);
			if (text.Contains(name))
			{
				result.Add(item.Character.GetId());
			}
		}
		return result;
	}

	public CharacterList GetCharacterList(int filterIndex)
	{
		CharacterList result = default(CharacterList);
		if (filterIndex < 0)
		{
			foreach (CharacterSortingInfo character in _characters)
			{
				result.Add(character.Character.GetId());
			}
		}
		else
		{
			List<CharacterSortingInfo> list = _filteredCharacters[filterIndex];
			foreach (CharacterSortingInfo item in list)
			{
				result.Add(item.Character.GetId());
			}
		}
		return result;
	}

	public void Sort(CharacterSortFilterSettings sortFilterSettings)
	{
		if (_settings.FilterType != sortFilterSettings.FilterType)
		{
			throw new Exception("Cannot change filter type when the current sort filter is already initialized.");
		}
		_settings = sortFilterSettings;
		if (sortFilterSettings.FilterSubId < 0)
		{
			_characters.Sort(Compare);
		}
		else
		{
			_filteredCharacters[sortFilterSettings.FilterSubId].Sort(Compare);
		}
	}

	public bool CheckFilterSettingsMatch(CharacterSortFilterSettings settings)
	{
		if (settings.FilterType != _settings.FilterType)
		{
			return false;
		}
		return settings.FilterType switch
		{
			1 => settings.TargetLocation.Equals(_settings.TargetLocation), 
			2 => settings.TargetCharId == _settings.TargetCharId, 
			4 => settings.VillagerNeededItem.Equals(_settings.VillagerNeededItem), 
			_ => true, 
		};
	}

	public void FindByFilter()
	{
		_characters.Clear();
		_addedCharIdSet.Clear();
		bool flag = false;
		if (_settings.FilterType == 1)
		{
			DomainManager.Adventure.GetCharactersInAdventure(_settings.TargetLocation, _addedCharIdSet);
			foreach (int item in _addedCharIdSet)
			{
				Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				_characters.Add(new CharacterSortingInfo(element_Objects, _currEncoding));
			}
		}
		else
		{
			switch (_settings.FilterSubType)
			{
			case 0:
				Parallel.For(0, _filteredCharacters.Length, FindInState);
				break;
			case 1:
				Parallel.For(0, RelationDisplayType.Instance.Count, FindInRelationDisplayType);
				flag = true;
				break;
			case 2:
				Parallel.For(0, _filteredCharacters.Length, FindInVillagerRole);
				break;
			}
		}
		if (_filteredCharacters != null)
		{
			if (flag)
			{
				List<CharacterSortingInfo>[] filteredCharacters = _filteredCharacters;
				foreach (List<CharacterSortingInfo> list in filteredCharacters)
				{
					for (int j = 0; j < list.Count; j++)
					{
						CharacterSortingInfo characterSortingInfo = list[j];
						int id = characterSortingInfo.GetId();
						if (_addedCharIdSet.Add(id))
						{
							_characters.Add(characterSortingInfo);
						}
					}
				}
			}
			else
			{
				List<CharacterSortingInfo>[] filteredCharacters2 = _filteredCharacters;
				foreach (List<CharacterSortingInfo> collection in filteredCharacters2)
				{
					_characters.AddRange(collection);
				}
			}
		}
		_characters.Sort(Compare);
		UpdateCounts();
	}

	private sbyte FilterSubIdToStateId(int filterSubId)
	{
		return (sbyte)(filterSubId - 1);
	}

	private void FindInState(int filterSubId)
	{
		List<CharacterSortingInfo> list = _filteredCharacters[filterSubId];
		if (filterSubId == 0)
		{
			HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int item in collection)
			{
				Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				if (Match(element_Objects, _settings.FilterType))
				{
					list.Add(new CharacterSortingInfo(element_Objects, _currEncoding));
				}
			}
			list.Sort(Compare);
			return;
		}
		sbyte stateId = FilterSubIdToStateId(filterSubId);
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		DomainManager.Map.GetAllAreaInState(stateId, list2);
		bool flag = false;
		foreach (short item2 in list2)
		{
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(item2);
			Span<MapBlockData> span = areaBlocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData mapBlockData = span[i];
				if (mapBlockData.CharacterSet == null)
				{
					continue;
				}
				foreach (int item3 in mapBlockData.CharacterSet)
				{
					Character element_Objects2 = DomainManager.Character.GetElement_Objects(item3);
					if (Match(element_Objects2, _settings.FilterType))
					{
						list.Add(new CharacterSortingInfo(element_Objects2, _currEncoding));
					}
				}
				if (!flag || mapBlockData.InfectedCharacterSet == null)
				{
					continue;
				}
				foreach (int item4 in mapBlockData.InfectedCharacterSet)
				{
					Character element_Objects3 = DomainManager.Character.GetElement_Objects(item4);
					if (Match(element_Objects3, _settings.FilterType))
					{
						list.Add(new CharacterSortingInfo(element_Objects3, _currEncoding));
					}
				}
			}
		}
		list.Sort(Compare);
	}

	private void FindInRelationDisplayType(int filterSubId)
	{
		RelationDisplayTypeItem relationDisplayTypeItem = RelationDisplayType.Instance[filterSubId];
		RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(_settings.TargetCharId);
		if (relatedCharacters == null || !_filteredCharacters.CheckIndex(filterSubId))
		{
			return;
		}
		List<CharacterSortingInfo> list = _filteredCharacters[filterSubId];
		if (relationDisplayTypeItem.RelationTypeIds != null)
		{
			sbyte[] relationTypeIds = relationDisplayTypeItem.RelationTypeIds;
			foreach (sbyte typeId in relationTypeIds)
			{
				ushort relationType = RelationType.GetRelationType(typeId);
				HashSet<int> collection = relatedCharacters.GetCharacterSet(relationType).GetCollection();
				foreach (int item in collection)
				{
					if (DomainManager.Character.TryGetElement_Objects(item, out var element) && Match(element, _settings.FilterType))
					{
						list.Add(new CharacterSortingInfo(element, _currEncoding));
					}
				}
				if (!RelationType.IsOneWayRelation(relationType))
				{
					continue;
				}
				HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
				hashSet.Clear();
				relatedCharacters.GetAllRelatedCharIds(hashSet);
				foreach (int item2 in hashSet)
				{
					if (DomainManager.Character.HasRelation(item2, _settings.TargetCharId, relationType) && DomainManager.Character.TryGetElement_Objects(item2, out var element2) && Match(element2, _settings.FilterType) && !collection.Contains(item2))
					{
						list.Add(new CharacterSortingInfo(element2, _currEncoding));
					}
				}
				ObjectPool<HashSet<int>>.Instance.Return(hashSet);
			}
		}
		else if (relationDisplayTypeItem.TemplateId == 4)
		{
			Character element_Objects = DomainManager.Character.GetElement_Objects(_settings.TargetCharId);
			int factionId = element_Objects.GetFactionId();
			if (factionId >= 0)
			{
				HashSet<int> collection2 = DomainManager.Organization.GetElement_Factions(factionId).GetCollection();
				foreach (int item3 in collection2)
				{
					if (DomainManager.Character.TryGetElement_Objects(item3, out var element3))
					{
						list.Add(new CharacterSortingInfo(element3, _currEncoding));
					}
				}
			}
		}
		list.Sort(Compare);
	}

	private void FindInVillagerRole(int filterSubId)
	{
		List<CharacterSortingInfo> list = _filteredCharacters[filterSubId];
		Dictionary<int, sbyte> treasuryItemNeededCharDict = DomainManager.Taiwu.GetTreasuryItemNeededCharDict(_settings.VillagerNeededItem);
		if (filterSubId == 0)
		{
			foreach (int key in treasuryItemNeededCharDict.Keys)
			{
				if (DomainManager.Character.TryGetElement_Objects(key, out var element) && Match(element, _settings.FilterType) && element.GetOrganizationInfo().Grade == 0)
				{
					list.Add(new CharacterSortingInfo(element, _currEncoding, this));
				}
			}
		}
		else
		{
			int num = filterSubId - 1;
			IReadOnlySet<int> villagerRoleSet = DomainManager.Taiwu.GetVillagerRoleSet((short)num);
			foreach (int key2 in treasuryItemNeededCharDict.Keys)
			{
				if (villagerRoleSet.Contains(key2) && DomainManager.Character.TryGetElement_Objects(key2, out var element2) && Match(element2, _settings.FilterType))
				{
					list.Add(new CharacterSortingInfo(element2, _currEncoding, this));
				}
			}
		}
		list.Sort(Compare);
	}

	private static bool Match(Character character, int filterType)
	{
		if (1 == 0)
		{
		}
		bool result = filterType switch
		{
			0 => MatchDirectedSamsaraMother(character), 
			3 => MatchCanLinkInLifeDeathGate(character), 
			_ => true, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static bool MatchDirectedSamsaraMother(Character character)
	{
		if (character.GetGender() != 0)
		{
			return false;
		}
		if (character.GetAgeGroup() != 2)
		{
			return false;
		}
		if (character.GetLegendaryBookOwnerState() > 1)
		{
			return false;
		}
		if (character.GetFertility() <= 0)
		{
			return false;
		}
		int id = character.GetId();
		if (id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			return false;
		}
		if (ProfessionSkillHandle.BuddhistMonkSkill_IsDirectedSamsaraMother(id))
		{
			return false;
		}
		return true;
	}

	private static bool MatchCanLinkInLifeDeathGate(Character character)
	{
		int id = character.GetId();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (!DomainManager.Character.TryGetRelation(id, taiwuCharId, out var _) && id != taiwuCharId)
		{
			return false;
		}
		if (character.IsCompletelyInfected())
		{
			return false;
		}
		if (character.GetLegendaryBookOwnerState() >= 2)
		{
			return false;
		}
		if (character.GetAgeGroup() != 2)
		{
			return false;
		}
		SectBaihuaLifeLinkData sectBaihuaLifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
		if (sectBaihuaLifeLinkData.LifeGateCharIds.IndexOf(id) >= 0)
		{
			return false;
		}
		if (sectBaihuaLifeLinkData.DeathGateCharIds.IndexOf(id) >= 0)
		{
			return false;
		}
		return true;
	}

	private int Compare(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		for (int i = 0; i < _settings.SortOrder.Count - 1; i++)
		{
			(int, bool) tuple = _settings.SortOrder[i];
			int num = (tuple.Item2 ? Compare(charB, charA, tuple.Item1) : Compare(charA, charB, tuple.Item1));
			if (num != 0)
			{
				return num;
			}
		}
		if (_settings.SortOrder.Count > 0)
		{
			List<(int type, bool isDescending)> sortOrder = _settings.SortOrder;
			(int, bool) tuple2 = sortOrder[sortOrder.Count - 1];
			int num2 = (tuple2.Item2 ? CompareValue(charB, charA, tuple2.Item1) : CompareValue(charA, charB, tuple2.Item1));
			if (num2 != 0)
			{
				return num2;
			}
		}
		if (_settings.FilterType == 4 && charA.VillagerNeedWaitTime != charB.VillagerNeedWaitTime)
		{
			return charA.VillagerNeedWaitTime.CompareTo(charB.VillagerNeedWaitTime);
		}
		return charA.Character.GetId().CompareTo(charB.Character.GetId());
	}

	private static int CompareValue(CharacterSortingInfo charA, CharacterSortingInfo charB, int sortingType)
	{
		if (1 == 0)
		{
		}
		int result = sortingType switch
		{
			5 => CompareMorality(charA, charB), 
			6 => CompareHappiness(charA, charB), 
			7 => CompareFavorability(charA, charB), 
			8 => CompareFame(charA, charB), 
			201 => CompareAttraction(charA, charB), 
			_ => Compare(charA, charB, sortingType), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static int Compare(CharacterSortingInfo charA, CharacterSortingInfo charB, int sortingType)
	{
		if (1 == 0)
		{
		}
		int result;
		if (sortingType <= 33)
		{
			if (sortingType < 26)
			{
				switch (sortingType)
				{
				case 0:
					break;
				case 1:
					goto IL_00a3;
				case 2:
					goto IL_00b0;
				case 3:
					goto IL_00bd;
				case 4:
					goto IL_00ca;
				case 5:
					goto IL_00d7;
				case 6:
					goto IL_00e4;
				case 7:
					goto IL_00f1;
				case 8:
					goto IL_00fe;
				case 9:
					goto IL_010b;
				case 10:
					goto IL_0118;
				case 11:
					goto IL_0125;
				case 12:
					goto IL_0132;
				case 13:
					goto IL_013f;
				case 17:
					goto IL_0149;
				case 18:
					goto IL_0153;
				case 19:
					goto IL_015d;
				case 20:
					goto IL_0167;
				case 21:
					goto IL_0171;
				case 22:
					goto IL_017b;
				case 23:
					goto IL_0185;
				case 24:
					goto IL_018f;
				default:
					goto IL_01c0;
				}
				result = CompareName(charA, charB);
			}
			else
			{
				result = CompareResource(charA, charB, (sbyte)(sortingType - 26));
			}
		}
		else if (sortingType < 100)
		{
			if (sortingType != 34)
			{
				goto IL_01c0;
			}
			result = CompareConsummateLevel(charA, charB);
		}
		else
		{
			result = CompareByReferencedType(charA, charB, (ECharacterPropertyReferencedType)(sortingType - 100));
		}
		goto IL_01c4;
		IL_0153:
		result = CompareLifeSkillAgeAdjust(charA, charB);
		goto IL_01c4;
		IL_0167:
		result = CompareDisorderOfQi(charA, charB);
		goto IL_01c4;
		IL_01c0:
		result = 0;
		goto IL_01c4;
		IL_018f:
		result = CompareOrganization(charA, charB);
		goto IL_01c4;
		IL_01c4:
		if (1 == 0)
		{
		}
		return result;
		IL_015d:
		result = CompareDefeatMarkCount(charA, charB);
		goto IL_01c4;
		IL_017b:
		result = CompareCurrInventoryLoad(charA, charB);
		goto IL_01c4;
		IL_0171:
		result = ComparePreexistenceCharCount(charA, charB);
		goto IL_01c4;
		IL_0185:
		result = CompareKidnappedCharacterCount(charA, charB);
		goto IL_01c4;
		IL_00a3:
		result = CompareGrade(charA, charB);
		goto IL_01c4;
		IL_00b0:
		result = CompareAge(charA, charB);
		goto IL_01c4;
		IL_00bd:
		result = CompareHealth(charA, charB);
		goto IL_01c4;
		IL_00ca:
		result = CompareGender(charA, charB);
		goto IL_01c4;
		IL_00d7:
		result = CompareBehaviorType(charA, charB);
		goto IL_01c4;
		IL_00e4:
		result = CompareHappinessType(charA, charB);
		goto IL_01c4;
		IL_00f1:
		result = CompareFavorType(charA, charB);
		goto IL_01c4;
		IL_00fe:
		result = CompareFameType(charA, charB);
		goto IL_01c4;
		IL_010b:
		result = CompareLivingStatus(charA, charB);
		goto IL_01c4;
		IL_0118:
		result = CompareWorkStatus(charA, charB);
		goto IL_01c4;
		IL_0125:
		result = CompareAttackMedal(charA, charB);
		goto IL_01c4;
		IL_0132:
		result = CompareDefenseMedal(charA, charB);
		goto IL_01c4;
		IL_013f:
		result = CompareWisdomMedal(charA, charB);
		goto IL_01c4;
		IL_0149:
		result = CompareCombatSkillAgeAdjust(charA, charB);
		goto IL_01c4;
	}

	private static int CompareByReferencedType(CharacterSortingInfo charA, CharacterSortingInfo charB, ECharacterPropertyReferencedType referencedType)
	{
		if (1 == 0)
		{
		}
		int result;
		switch (referencedType)
		{
		case ECharacterPropertyReferencedType.Strength:
		case ECharacterPropertyReferencedType.Dexterity:
		case ECharacterPropertyReferencedType.Concentration:
		case ECharacterPropertyReferencedType.Vitality:
		case ECharacterPropertyReferencedType.Energy:
		case ECharacterPropertyReferencedType.Intelligence:
			result = CompareMaxMainAttribute(charA, charB, (sbyte)(referencedType - 0));
			break;
		case ECharacterPropertyReferencedType.HitRateStrength:
		case ECharacterPropertyReferencedType.HitRateTechnique:
		case ECharacterPropertyReferencedType.HitRateSpeed:
		case ECharacterPropertyReferencedType.HitRateMind:
			result = CompareHitValue(charA, charB, (sbyte)(referencedType - 6));
			break;
		case ECharacterPropertyReferencedType.PenetrateOfOuter:
			result = CompareOuterPenetration(charA, charB);
			break;
		case ECharacterPropertyReferencedType.PenetrateOfInner:
			result = CompareInnerPenetration(charA, charB);
			break;
		case ECharacterPropertyReferencedType.AvoidRateStrength:
		case ECharacterPropertyReferencedType.AvoidRateTechnique:
		case ECharacterPropertyReferencedType.AvoidRateSpeed:
		case ECharacterPropertyReferencedType.AvoidRateMind:
			result = CompareAvoidValue(charA, charB, (sbyte)(referencedType - 12));
			break;
		case ECharacterPropertyReferencedType.PenetrateResistOfOuter:
			result = CompareOuterPenetrationResist(charA, charB);
			break;
		case ECharacterPropertyReferencedType.PenetrateResistOfInner:
			result = CompareInnerPenetrationResist(charA, charB);
			break;
		case ECharacterPropertyReferencedType.QualificationMusic:
		case ECharacterPropertyReferencedType.QualificationChess:
		case ECharacterPropertyReferencedType.QualificationPoem:
		case ECharacterPropertyReferencedType.QualificationPainting:
		case ECharacterPropertyReferencedType.QualificationMath:
		case ECharacterPropertyReferencedType.QualificationAppraisal:
		case ECharacterPropertyReferencedType.QualificationForging:
		case ECharacterPropertyReferencedType.QualificationWoodworking:
		case ECharacterPropertyReferencedType.QualificationMedicine:
		case ECharacterPropertyReferencedType.QualificationToxicology:
		case ECharacterPropertyReferencedType.QualificationWeaving:
		case ECharacterPropertyReferencedType.QualificationJade:
		case ECharacterPropertyReferencedType.QualificationTaoism:
		case ECharacterPropertyReferencedType.QualificationBuddhism:
		case ECharacterPropertyReferencedType.QualificationCooking:
		case ECharacterPropertyReferencedType.QualificationEclectic:
			result = CompareLifeSkillQualification(charA, charB, (sbyte)(referencedType - 34));
			break;
		case ECharacterPropertyReferencedType.AttainmentMusic:
		case ECharacterPropertyReferencedType.AttainmentChess:
		case ECharacterPropertyReferencedType.AttainmentPoem:
		case ECharacterPropertyReferencedType.AttainmentPainting:
		case ECharacterPropertyReferencedType.AttainmentMath:
		case ECharacterPropertyReferencedType.AttainmentAppraisal:
		case ECharacterPropertyReferencedType.AttainmentForging:
		case ECharacterPropertyReferencedType.AttainmentWoodworking:
		case ECharacterPropertyReferencedType.AttainmentMedicine:
		case ECharacterPropertyReferencedType.AttainmentToxicology:
		case ECharacterPropertyReferencedType.AttainmentWeaving:
		case ECharacterPropertyReferencedType.AttainmentJade:
		case ECharacterPropertyReferencedType.AttainmentTaoism:
		case ECharacterPropertyReferencedType.AttainmentBuddhism:
		case ECharacterPropertyReferencedType.AttainmentCooking:
		case ECharacterPropertyReferencedType.AttainmentEclectic:
			result = CompareLifeSkillAttainment(charA, charB, (sbyte)(referencedType - 50));
			break;
		case ECharacterPropertyReferencedType.QualificationNeigong:
		case ECharacterPropertyReferencedType.QualificationPosing:
		case ECharacterPropertyReferencedType.QualificationStunt:
		case ECharacterPropertyReferencedType.QualificationFistAndPalm:
		case ECharacterPropertyReferencedType.QualificationFinger:
		case ECharacterPropertyReferencedType.QualificationLeg:
		case ECharacterPropertyReferencedType.QualificationThrow:
		case ECharacterPropertyReferencedType.QualificationSword:
		case ECharacterPropertyReferencedType.QualificationBlade:
		case ECharacterPropertyReferencedType.QualificationPolearm:
		case ECharacterPropertyReferencedType.QualificationSpecial:
		case ECharacterPropertyReferencedType.QualificationWhip:
		case ECharacterPropertyReferencedType.QualificationControllableShot:
		case ECharacterPropertyReferencedType.QualificationCombatMusic:
			result = CompareCombatSkillQualification(charA, charB, (sbyte)(referencedType - 66));
			break;
		case ECharacterPropertyReferencedType.AttainmentNeigong:
		case ECharacterPropertyReferencedType.AttainmentPosing:
		case ECharacterPropertyReferencedType.AttainmentStunt:
		case ECharacterPropertyReferencedType.AttainmentFistAndPalm:
		case ECharacterPropertyReferencedType.AttainmentFinger:
		case ECharacterPropertyReferencedType.AttainmentLeg:
		case ECharacterPropertyReferencedType.AttainmentThrow:
		case ECharacterPropertyReferencedType.AttainmentSword:
		case ECharacterPropertyReferencedType.AttainmentBlade:
		case ECharacterPropertyReferencedType.AttainmentPolearm:
		case ECharacterPropertyReferencedType.AttainmentSpecial:
		case ECharacterPropertyReferencedType.AttainmentWhip:
		case ECharacterPropertyReferencedType.AttainmentControllableShot:
		case ECharacterPropertyReferencedType.AttainmentCombatMusic:
			result = CompareCombatSkillAttainment(charA, charB, (sbyte)(referencedType - 80));
			break;
		case ECharacterPropertyReferencedType.PersonalityCalm:
		case ECharacterPropertyReferencedType.PersonalityClever:
		case ECharacterPropertyReferencedType.PersonalityEnthusiastic:
		case ECharacterPropertyReferencedType.PersonalityBrave:
		case ECharacterPropertyReferencedType.PersonalityFirm:
		case ECharacterPropertyReferencedType.PersonalityLucky:
		case ECharacterPropertyReferencedType.PersonalityPerceptive:
			result = ComparePersonality(charA, charB, (sbyte)(referencedType - 94));
			break;
		case ECharacterPropertyReferencedType.Attraction:
			result = CompareAttractionType(charA, charB);
			break;
		default:
			throw new Exception($"Sorting for {referencedType} is not implemented");
		}
		if (1 == 0)
		{
		}
		return result;
	}

	private static int CompareName(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		for (int i = 0; i < charB.NameBytes.Length; i++)
		{
			if (charA.NameBytes.Length <= i)
			{
				return -1;
			}
			int num = charA.NameBytes[i].CompareTo(charB.NameBytes[i]);
			if (num != 0)
			{
				return num;
			}
		}
		return 1;
	}

	private static int CompareGrade(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetOrganizationInfo().Grade.CompareTo(charB.Character.GetOrganizationInfo().Grade);
	}

	private static int CompareAge(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetCurrAge().CompareTo(charB.Character.GetCurrAge());
	}

	private static int CompareHealth(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.HealthPercentage.CompareTo(charB.HealthPercentage);
	}

	private static int CompareGender(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetGender().CompareTo(charB.Character.GetGender());
	}

	private static int CompareMorality(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Morality.CompareTo(charB.Morality);
	}

	private static int CompareBehaviorType(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.BehaviorType.CompareTo(charB.BehaviorType);
	}

	private static int CompareHappiness(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetHappiness().CompareTo(charB.Character.GetHappiness());
	}

	private static int CompareHappinessType(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.HappinessType.CompareTo(charB.HappinessType);
	}

	private static int CompareFavorability(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.FavorabilityToTaiwu.CompareTo(charB.FavorabilityToTaiwu);
	}

	private static int CompareFavorType(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.FavorType.CompareTo(charB.FavorType);
	}

	private static int CompareFame(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Fame.CompareTo(charB.Fame);
	}

	private static int CompareFameType(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.FameType.CompareTo(charB.FameType);
	}

	private static int CompareAttraction(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Attraction.CompareTo(charB.Attraction);
	}

	private static int CompareAttractionType(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.AttractionType.CompareTo(charB.AttractionType);
	}

	private static int CompareLivingStatus(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.LivingStatus.CompareTo(charB.LivingStatus);
	}

	private static int CompareWorkStatus(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.WorkStatus.CompareTo(charB.WorkStatus);
	}

	private static int CompareAttackMedal(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.AttackMedal.CompareTo(charB.AttackMedal);
	}

	private static int CompareDefenseMedal(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.DefenseMedal.CompareTo(charB.DefenseMedal);
	}

	private static int CompareWisdomMedal(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.WisdomMedal.CompareTo(charB.WisdomMedal);
	}

	private static int CompareDefeatMarkCount(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.DefeatMarkCount.CompareTo(charB.DefeatMarkCount);
	}

	private static int CompareCombatSkillAgeAdjust(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.CombatSkillAgeAdjust.CompareTo(charB.CombatSkillAgeAdjust);
	}

	private static int CompareLifeSkillAgeAdjust(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.LifeSkillAgeAdjust.CompareTo(charB.LifeSkillAgeAdjust);
	}

	private static int ComparePersonality(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte personalityType)
	{
		return charA.Character.GetPersonality(personalityType).CompareTo(charB.Character.GetPersonality(personalityType));
	}

	private static int CompareCombatSkillQualification(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte combatSkillType)
	{
		return charA.Character.GetCombatSkillQualification(combatSkillType).CompareTo(charB.Character.GetCombatSkillQualification(combatSkillType));
	}

	private static int CompareCombatSkillAttainment(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte combatSkillType)
	{
		return charA.Character.GetCombatSkillAttainment(combatSkillType).CompareTo(charB.Character.GetCombatSkillAttainment(combatSkillType));
	}

	private static int CompareLifeSkillQualification(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte lifeSkillType)
	{
		return charA.Character.GetLifeSkillQualification(lifeSkillType).CompareTo(charB.Character.GetLifeSkillQualification(lifeSkillType));
	}

	private static int CompareLifeSkillAttainment(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte lifeSkillType)
	{
		return charA.Character.GetLifeSkillAttainment(lifeSkillType).CompareTo(charB.Character.GetLifeSkillAttainment(lifeSkillType));
	}

	private static int CompareMaxMainAttribute(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte mainAttributeType)
	{
		return charA.Character.GetMaxMainAttribute(mainAttributeType).CompareTo(charB.Character.GetMaxMainAttribute(mainAttributeType));
	}

	private static int CompareResource(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte resourceType)
	{
		return charA.Character.GetResource(resourceType).CompareTo(charB.Character.GetResource(resourceType));
	}

	private static int CompareInnerPenetration(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetPenetrations().Inner.CompareTo(charB.Character.GetPenetrations().Inner);
	}

	private static int CompareOuterPenetration(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetPenetrations().Outer.CompareTo(charB.Character.GetPenetrations().Outer);
	}

	private static int CompareInnerPenetrationResist(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetPenetrations().Inner.CompareTo(charB.Character.GetPenetrations().Inner);
	}

	private static int CompareOuterPenetrationResist(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetPenetrations().Outer.CompareTo(charB.Character.GetPenetrations().Outer);
	}

	private unsafe static int CompareHitValue(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte hitValueType)
	{
		return charA.HitValues.Items[hitValueType].CompareTo(charB.HitValues.Items[hitValueType]);
	}

	private unsafe static int CompareAvoidValue(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte hitValueType)
	{
		return charA.AvoidValues.Items[hitValueType].CompareTo(charB.AvoidValues.Items[hitValueType]);
	}

	private static int CompareDisorderOfQi(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetDisorderOfQi().CompareTo(charB.Character.GetDisorderOfQi());
	}

	private static int ComparePreexistenceCharCount(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetPreexistenceCharIds().Count.CompareTo(charB.Character.GetPreexistenceCharIds().Count);
	}

	private static int CompareCurrInventoryLoad(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetCurrInventoryLoad().CompareTo(charB.Character.GetCurrInventoryLoad());
	}

	private static int CompareKidnappedCharacterCount(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.KidnappedCharCount.CompareTo(charB.KidnappedCharCount);
	}

	private static int CompareOrganization(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		OrganizationInfo organizationInfo = charA.Character.GetOrganizationInfo();
		OrganizationInfo organizationInfo2 = charB.Character.GetOrganizationInfo();
		int num = organizationInfo.OrgTemplateId.CompareTo(organizationInfo2.OrgTemplateId);
		return (num != 0) ? num : organizationInfo.SettlementId.CompareTo(organizationInfo2.SettlementId);
	}

	private static int CompareConsummateLevel(CharacterSortingInfo charA, CharacterSortingInfo charB)
	{
		return charA.Character.GetConsummateLevel().CompareTo(charB.Character.GetConsummateLevel());
	}
}
