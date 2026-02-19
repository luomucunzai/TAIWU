using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GameData.Domains.Information;

public class SecretInformationProcessor
{
	public static class RelationIndex
	{
		public static readonly List<SecretInformationRelationshipType> Allied = new List<SecretInformationRelationshipType> { SecretInformationRelationshipType.Allied };

		public static readonly List<SecretInformationRelationshipType> Enemy = new List<SecretInformationRelationshipType> { SecretInformationRelationshipType.Enemy };

		public static readonly List<SecretInformationRelationshipType> Adorer = new List<SecretInformationRelationshipType> { SecretInformationRelationshipType.Adorer };

		public static readonly List<SecretInformationRelationshipType> Revel = new List<SecretInformationRelationshipType>
		{
			SecretInformationRelationshipType.Relative,
			SecretInformationRelationshipType.MentorAndMentee,
			SecretInformationRelationshipType.SwornBrotherOrSister
		};

		public static readonly List<SecretInformationRelationshipType> Love = new List<SecretInformationRelationshipType>
		{
			SecretInformationRelationshipType.HusbandOrWife,
			SecretInformationRelationshipType.Lover
		};

		public static readonly List<SecretInformationRelationshipType> Spouse = new List<SecretInformationRelationshipType> { SecretInformationRelationshipType.HusbandOrWife };

		public static readonly List<SecretInformationRelationshipType> Single = new List<SecretInformationRelationshipType>
		{
			SecretInformationRelationshipType.Adorer,
			SecretInformationRelationshipType.Enemy
		};
	}

	private static readonly List<int> ArgList = new List<int>();

	private static readonly List<int> ActorIdList = new List<int>();

	private static readonly Dictionary<int, GameData.Domains.Character.Character> ActiveActorList = new Dictionary<int, GameData.Domains.Character.Character>();

	private static int _metaDataId = -1;

	private static short _templateId = -1;

	private static int _baseFavorOdds = -1;

	private static Dictionary<GameData.Domains.Character.Character, int> RelatedCharacterIndexList = null;

	private static SecretInformationMetaData _metaDataRef;

	private static EventArgBox _secretInfoArgBox = new EventArgBox();

	private static SecretInformationItem _infoConfig;

	private static SecretInformationEffectItem _effectConfig;

	private static SecretInformationSectPunishItem _sectPunishConfig;

	public bool Initialize(int metaDataId)
	{
		Reset();
		_metaDataId = metaDataId;
		if (_metaDataId == -1)
		{
			AdaptableLog.Info("Invalid metaDataId!");
			return false;
		}
		if (DomainManager.Information.TryGetElement_SecretInformationMetaData(_metaDataId, out _metaDataRef))
		{
			DomainManager.Information.MakeSecretInformationEventArgBox(_metaDataRef, _secretInfoArgBox);
			if (_secretInfoArgBox == null)
			{
				AdaptableLog.Info("Invalid secretInfoArgBox!");
				return false;
			}
			int arg = -1;
			int arg2 = -1;
			int arg3 = -1;
			int item = -1;
			_secretInfoArgBox.Get("templateId", ref _templateId);
			if (_templateId == -1)
			{
				return false;
			}
			_infoConfig = SecretInformation.Instance.GetItem(_templateId);
			if (_infoConfig == null)
			{
				return false;
			}
			_effectConfig = SecretInformationEffect.Instance.GetItem(_infoConfig.DefaultEffectId);
			if (_effectConfig == null)
			{
				return false;
			}
			_sectPunishConfig = SecretInformationSectPunish.Instance.GetItem(_templateId);
			if (_sectPunishConfig == null)
			{
				return false;
			}
			if (_effectConfig.ActorIndex != -1)
			{
				_secretInfoArgBox.Get($"arg{_effectConfig.ActorIndex}", ref arg);
			}
			if (_effectConfig.ReactorIndex != -1)
			{
				_secretInfoArgBox.Get($"arg{_effectConfig.ReactorIndex}", ref arg2);
			}
			if (_effectConfig.SecactorIndex != -1)
			{
				_secretInfoArgBox.Get($"arg{_effectConfig.SecactorIndex}", ref arg3);
			}
			if (_effectConfig.Item != -1)
			{
				_secretInfoArgBox.Get<ItemKey>($"arg{_effectConfig.Item}", out ItemKey arg4);
				item = arg4.ItemType;
			}
			ArgList.Add(arg);
			ArgList.Add(arg2);
			ArgList.Add(arg3);
			for (int i = 0; i < ArgList.Count; i++)
			{
				int num = ArgList[i];
				if (num != -1)
				{
					ActorIdList.Add(num);
					if (InformationDomain.CheckTuringTest(num, out var character))
					{
						ActiveActorList.Add(i, character);
					}
				}
			}
			ArgList.Add(-1);
			ArgList.Add(item);
			ArgList.Add(DomainManager.Taiwu.GetTaiwuCharId());
			return true;
		}
		AdaptableLog.Info("Invalid secretInfoMetaData!");
		return false;
	}

	public void Reset()
	{
		ArgList.Clear();
		ActorIdList.Clear();
		ActiveActorList.Clear();
		_metaDataId = -1;
		_templateId = -1;
		_metaDataRef = null;
		_secretInfoArgBox.Clear();
		_infoConfig = null;
		_effectConfig = null;
		_sectPunishConfig = null;
		_baseFavorOdds = -1;
		RelatedCharacterIndexList = null;
	}

	public void Initialize_ForBroadcastEffect(IRandomSource random)
	{
		CalBaseFavorabilityConditionOdds();
		CalALLActiveRelatedCharactorRelationIndex(random);
	}

	public short GetSecretInformationTemplateId()
	{
		return _templateId;
	}

	public SecretInformationEffectItem GetSecretInformationEffectConfig()
	{
		return _effectConfig;
	}

	public List<int> GetSecretInformationArgList()
	{
		return ArgList;
	}

	public bool IsCharacterSecretInformationActor(int charId, bool includeDead = true)
	{
		IEnumerable<int> enumerable;
		if (!includeDead)
		{
			IEnumerable<int> actorIdList = ActorIdList;
			enumerable = actorIdList;
		}
		else
		{
			enumerable = ActiveActorList.Values.Select((GameData.Domains.Character.Character x) => x.GetId());
		}
		IEnumerable<int> source = enumerable;
		return source.Contains(charId);
	}

	public List<int> GetActiveActorIdList(IEnumerable<int> excludeId = null)
	{
		HashSet<int> hashSet = new HashSet<int>(ActiveActorList.Values.Select((GameData.Domains.Character.Character x) => x.GetId()));
		if (excludeId != null)
		{
			hashSet.ExceptWith(excludeId);
		}
		return hashSet.ToList();
	}

	public List<int> GetActorIdList(IEnumerable<int> excludeId = null)
	{
		HashSet<int> hashSet = new HashSet<int>(ActorIdList);
		if (excludeId != null)
		{
			hashSet.ExceptWith(excludeId);
		}
		return hashSet.ToList();
	}

	public int GetCharacterActorIndex(int charId)
	{
		int num = ArgList.FindIndex((int x) => x == charId);
		if (num > 2)
		{
			num = -1;
		}
		return num;
	}

	public bool CheckIfActorExist_WithActorIndex(int charIndex)
	{
		if (charIndex < 0 || charIndex > 2)
		{
			return false;
		}
		GameData.Domains.Character.Character element;
		return DomainManager.Character.TryGetElement_Objects(ArgList[charIndex], out element);
	}

	public Dictionary<int, GameData.Domains.Character.Character> GetActiveActorList_WithActorIndex()
	{
		return ActiveActorList;
	}

	public Dictionary<int, GameData.Domains.Character.Character> GetActiveActorList_WithRelationIndex()
	{
		List<int> list = new List<int> { 1, 4, 7 };
		Dictionary<int, GameData.Domains.Character.Character> dictionary = new Dictionary<int, GameData.Domains.Character.Character>();
		foreach (KeyValuePair<int, GameData.Domains.Character.Character> activeActor in ActiveActorList)
		{
			dictionary.Add(list[activeActor.Key], activeActor.Value);
		}
		return dictionary;
	}

	public byte GetSecretInformationActorBroadcastType()
	{
		byte result = 2;
		if (IsTaiwuSecretInformationActor())
		{
			result = 0;
		}
		else if (IsTaiwuSecretInformationDissemination())
		{
			result = 1;
		}
		return result;
	}

	public bool IsTaiwuSecretInformationActor()
	{
		return ActorIdList.Contains(ArgList[5]);
	}

	private bool IsTaiwuSecretInformationDissemination()
	{
		return GetSecretInformationDisseminationBranchCharacterIds().Contains(ArgList[5]);
	}

	public sbyte GetFameTypeSafe(int charId)
	{
		int value = ArgList.FindIndex((int x) => x == charId);
		sbyte b = -1;
		if (_infoConfig.ExtraSnapshotParameterIndices == null || !_infoConfig.ExtraSnapshotParameterIndices.Contains(value))
		{
			return b;
		}
		if (_metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out var value2))
		{
			b = value2.FameType;
		}
		if (b == -2)
		{
			b = 3;
		}
		return b;
	}

	public OrganizationInfo GetSectInfoSafe(int charId)
	{
		int value = ArgList.FindIndex((int x) => x == charId);
		OrganizationInfo result = new OrganizationInfo(-1, 0, principal: true, -1);
		if (_infoConfig.ExtraSnapshotParameterIndices == null || !_infoConfig.ExtraSnapshotParameterIndices.Contains(value))
		{
			return result;
		}
		if (_metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out var value2))
		{
			result = value2.OrgInfo;
		}
		return result;
	}

	public byte GetMonkTypeSafe(int charId)
	{
		byte result = 0;
		int value = ArgList.FindIndex((int x) => x == charId);
		if (_infoConfig.ExtraSnapshotParameterIndices == null || !_infoConfig.ExtraSnapshotParameterIndices.Contains(value))
		{
			return result;
		}
		if (_metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out var value2))
		{
			result = value2.MonkType;
		}
		return result;
	}

	public HashSet<int> GetSecretInformationRelatedCharactersOfSpecialRelation(int characterId, IEnumerable<SecretInformationRelationshipType> relations, bool includeAlive = true, bool includeDead = true, bool includeActors = false)
	{
		HashSet<int> hashSet = new HashSet<int>();
		SecretInformationRelationshipType[] array = (relations as SecretInformationRelationshipType[]) ?? relations.ToArray();
		if (!array.Any())
		{
			return hashSet;
		}
		if (_metaDataRef.CharacterRelationshipSnapshotCollection.Collection.TryGetValue(characterId, out var value))
		{
			SecretInformationRelationshipType[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				switch (array2[i])
				{
				case SecretInformationRelationshipType.Relative:
					hashSet.UnionWith(value.RelatedCharacters.BloodParents.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.BloodChildren.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.BloodBrothersAndSisters.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.StepParents.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.StepChildren.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.StepBrothersAndSisters.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.AdoptiveParents.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.AdoptiveChildren.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.AdoptiveBrothersAndSisters.GetCollection());
					break;
				case SecretInformationRelationshipType.SwornBrotherOrSister:
					hashSet.UnionWith(value.RelatedCharacters.SwornBrothersAndSisters.GetCollection());
					break;
				case SecretInformationRelationshipType.HusbandOrWife:
					hashSet.UnionWith(value.RelatedCharacters.HusbandsAndWives.GetCollection());
					break;
				case SecretInformationRelationshipType.Adorer:
					hashSet.UnionWith(value.RelatedCharacters.Adored.GetCollection());
					break;
				case SecretInformationRelationshipType.MentorAndMentee:
					hashSet.UnionWith(value.RelatedCharacters.Mentors.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.Mentees.GetCollection());
					break;
				case SecretInformationRelationshipType.Friend:
					hashSet.UnionWith(value.RelatedCharacters.Friends.GetCollection());
					break;
				case SecretInformationRelationshipType.Enemy:
					hashSet.UnionWith(value.RelatedCharacters.Enemies.GetCollection());
					break;
				case SecretInformationRelationshipType.Lover:
				{
					HashSet<int> hashSet2 = new HashSet<int>();
					foreach (int item in value.RelatedCharacters.Adored.GetCollection())
					{
						if (_metaDataRef.CharacterRelationshipSnapshotCollection.Collection.TryGetValue(characterId, out var value2) && value2.RelatedCharacters.Adored.Contains(characterId))
						{
							hashSet2.Add(item);
						}
					}
					hashSet.UnionWith(hashSet2);
					break;
				}
				case SecretInformationRelationshipType.Allied:
					hashSet.UnionWith(value.RelatedCharacters.BloodParents.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.BloodChildren.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.BloodBrothersAndSisters.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.StepParents.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.StepChildren.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.StepBrothersAndSisters.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.AdoptiveParents.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.AdoptiveChildren.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.AdoptiveBrothersAndSisters.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.SwornBrothersAndSisters.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.HusbandsAndWives.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.Adored.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.Mentors.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.Mentees.GetCollection());
					hashSet.UnionWith(value.RelatedCharacters.Friends.GetCollection());
					break;
				}
			}
			if (!includeAlive)
			{
				hashSet.RemoveWhere((int charId) => _metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out var value3) && value3.AliveState == 0);
			}
			if (!includeDead)
			{
				hashSet.RemoveWhere((int charId) => _metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out var value3) && value3.AliveState != 0);
			}
			if (!includeActors)
			{
				hashSet.ExceptWith(ActorIdList);
			}
		}
		return hashSet;
	}

	public HashSet<int> GetSecretInformationRelatedCharactersOfSpecialRelation_ForNoneActor(int characterId, IEnumerable<SecretInformationRelationshipType> relations, bool includeAlive = true, bool includeDead = true)
	{
		SecretInformationRelationshipType[] first = (relations as SecretInformationRelationshipType[]) ?? relations.ToArray();
		SecretInformationRelationshipType[] relations2 = first.Intersect(RelationIndex.Single).ToArray();
		SecretInformationRelationshipType[] relations3 = first.Except(RelationIndex.Single).ToArray();
		HashSet<int> hashSet = new HashSet<int>();
		foreach (int actorId in ActorIdList)
		{
			if (GetSecretInformationRelatedCharactersOfSpecialRelation(actorId, relations3).Contains(characterId) || GetSecretInformationRelatedCharactersOfSpecialRelation(characterId, relations2).Contains(actorId))
			{
				hashSet.Add(actorId);
			}
		}
		if (!includeAlive)
		{
			hashSet.RemoveWhere((int charId) => _metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out var value) && value.AliveState == 0);
		}
		if (!includeDead)
		{
			hashSet.RemoveWhere((int charId) => _metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out var value) && value.AliveState != 0);
		}
		return hashSet;
	}

	public bool Check_HasSpecialRelations(int charId, int targetId, IEnumerable<SecretInformationRelationshipType> relations)
	{
		SecretInformationRelationshipType[] first = (relations as SecretInformationRelationshipType[]) ?? relations.ToArray();
		IEnumerable<SecretInformationRelationshipType> relations2 = first.Intersect(RelationIndex.Single);
		SecretInformationRelationshipType[] relations3 = first.Except(RelationIndex.Single).ToArray();
		return GetSecretInformationRelatedCharactersOfSpecialRelation(charId, relations2, includeAlive: true, includeDead: true, includeActors: true).Contains(targetId) || GetSecretInformationRelatedCharactersOfSpecialRelation(charId, relations3, includeAlive: true, includeDead: true, includeActors: true).Contains(targetId) || GetSecretInformationRelatedCharactersOfSpecialRelation(targetId, relations3, includeAlive: true, includeDead: true, includeActors: true).Contains(charId);
	}

	public bool Check_IsSectLeaderOfCharacter(int charId, int targetId)
	{
		OrganizationInfo sectInfoSafe = GetSectInfoSafe(targetId);
		if (sectInfoSafe.OrgTemplateId < 1 || sectInfoSafe.OrgTemplateId > 15)
		{
			return false;
		}
		if (!InformationDomain.CheckTuringTest(charId, out var character))
		{
			return false;
		}
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		if (organizationInfo.OrgTemplateId < 0)
		{
			return false;
		}
		if (organizationInfo.OrgTemplateId != sectInfoSafe.OrgTemplateId)
		{
			return false;
		}
		return organizationInfo.Grade == 8;
	}

	public List<int> GetAllSecretInformationRelationsOfCharacter(int charId, bool includeLoveRelation, bool includeLeadership)
	{
		List<int> list = new List<int>();
		if (charId == -1)
		{
			return new List<int> { 0 };
		}
		int num = ArgList[0];
		int num2 = ArgList[1];
		int num3 = ArgList[2];
		if (num != -1)
		{
			if (num == charId)
			{
				list.Add(1);
			}
			if (Check_HasSpecialRelations(charId, num, RelationIndex.Allied))
			{
				list.Add(2);
			}
			if (Check_HasSpecialRelations(charId, num, RelationIndex.Enemy))
			{
				list.Add(3);
			}
			if (includeLoveRelation && !ActorIdList.Contains(charId))
			{
				if (Check_HasSpecialRelations(charId, num, RelationIndex.Love))
				{
					list.Add(10);
				}
				else if (Check_HasSpecialRelations(charId, num, RelationIndex.Adorer))
				{
					list.Add(13);
				}
			}
			if (includeLeadership && Check_IsSectLeaderOfCharacter(charId, num))
			{
				list.Add(16);
			}
		}
		if (num2 != -1)
		{
			if (num2 == charId)
			{
				list.Add(4);
			}
			if (Check_HasSpecialRelations(charId, num2, RelationIndex.Allied))
			{
				list.Add(5);
			}
			if (Check_HasSpecialRelations(charId, num2, RelationIndex.Enemy))
			{
				list.Add(6);
			}
			if (includeLoveRelation && !ActorIdList.Contains(charId))
			{
				if (Check_HasSpecialRelations(charId, num2, RelationIndex.Love))
				{
					list.Add(11);
				}
				else if (Check_HasSpecialRelations(charId, num2, RelationIndex.Adorer))
				{
					list.Add(14);
				}
			}
			if (includeLeadership && Check_IsSectLeaderOfCharacter(charId, num2))
			{
				list.Add(17);
			}
		}
		if (num3 != -1)
		{
			if (num3 == charId)
			{
				list.Add(7);
			}
			if (Check_HasSpecialRelations(charId, num3, RelationIndex.Allied))
			{
				list.Add(8);
			}
			if (Check_HasSpecialRelations(charId, num3, RelationIndex.Enemy))
			{
				list.Add(9);
			}
			if (includeLoveRelation && !ActorIdList.Contains(charId))
			{
				if (Check_HasSpecialRelations(charId, num3, RelationIndex.Love))
				{
					list.Add(12);
				}
				else if (Check_HasSpecialRelations(charId, num3, RelationIndex.Adorer))
				{
					list.Add(15);
				}
			}
			if (includeLeadership && Check_IsSectLeaderOfCharacter(charId, num3))
			{
				list.Add(18);
			}
		}
		if (list.Count == 0)
		{
			list.Add(0);
		}
		return list;
	}

	public HashSet<int> GetAllSecretInformationRelatedCharacters(bool includeGeneral = true, bool includeAlive = true, bool includeDead = true, bool includeActors = false)
	{
		HashSet<int> hashSet = new HashSet<int>();
		foreach (KeyValuePair<int, SecretInformationCharacterRelationshipSnapshot> item in _metaDataRef.CharacterRelationshipSnapshotCollection.Collection)
		{
			SecretInformationCharacterRelationshipSnapshot value = item.Value;
			value.RelatedCharacters.GetAllRelatedCharIds(hashSet, includeGeneral);
		}
		if (!includeAlive)
		{
			hashSet.RemoveWhere((int charId) => _metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out var value2) && value2.AliveState == 0);
		}
		if (!includeDead)
		{
			hashSet.RemoveWhere((int charId) => _metaDataRef.CharacterExtraInfoCollection.Collection.TryGetValue(charId, out var value2) && value2.AliveState != 0);
		}
		if (!includeActors)
		{
			hashSet.ExceptWith(ActorIdList);
		}
		return hashSet;
	}

	public List<GameData.Domains.Character.Character> GetAllSecretInformationRelatedCharacters_WithTuringTest(bool includeActors = false)
	{
		if (RelatedCharacterIndexList != null)
		{
			Dictionary<GameData.Domains.Character.Character, int>.KeyCollection keys = RelatedCharacterIndexList.Keys;
			return includeActors ? keys.ToList() : keys.Except(ActiveActorList.Values).ToList();
		}
		return InformationDomain.GetTuringTestPassedCharacters(GetAllSecretInformationRelatedCharacters(includeGeneral: true, includeAlive: true, includeDead: true, includeActors));
	}

	public Dictionary<GameData.Domains.Character.Character, int> GetAllSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex(bool includeActors = false)
	{
		if (RelatedCharacterIndexList == null)
		{
			return new Dictionary<GameData.Domains.Character.Character, int>();
		}
		Dictionary<GameData.Domains.Character.Character, int> dictionary = new Dictionary<GameData.Domains.Character.Character, int>(RelatedCharacterIndexList);
		if (!includeActors)
		{
			foreach (GameData.Domains.Character.Character value in ActiveActorList.Values)
			{
				if (dictionary.ContainsKey(value))
				{
					dictionary.Remove(value);
				}
			}
		}
		return dictionary;
	}

	public HashSet<int> GetSecretInformationDisseminationBranchCharacterIds()
	{
		return _metaDataRef.GetDisseminationBranchCharacterIds().ToHashSet();
	}

	public List<GameData.Domains.Character.Character> GetSecretInformationDisseminationBranchCharacterIds_WithTuringTest()
	{
		return InformationDomain.GetTuringTestPassedCharacters(_metaDataRef.GetDisseminationBranchCharacterIds());
	}

	private int GetRandonRelationIndex(IRandomSource random, int charId, bool includeLoveRelation = false, bool includeLeadership = false)
	{
		List<int> allSecretInformationRelationsOfCharacter = GetAllSecretInformationRelationsOfCharacter(charId, includeLoveRelation, includeLeadership);
		return allSecretInformationRelationsOfCharacter.ElementAt(random.Next(0, allSecretInformationRelationsOfCharacter.Count));
	}

	private void CalALLActiveRelatedCharactorRelationIndex(IRandomSource random)
	{
		RelatedCharacterIndexList = GetAllSecretInformationRelatedCharacters_WithTuringTest(includeActors: true).ToDictionary((GameData.Domains.Character.Character x) => x, (GameData.Domains.Character.Character x) => GetRandonRelationIndex(random, x.GetId()));
	}

	private int CalBaseFavorabilityConditionOdds()
	{
		int num = 0;
		_baseFavorOdds = 0;
		List<Config.ShortList> specialConditionFavorabilities = _effectConfig.SpecialConditionFavorabilities;
		if (specialConditionFavorabilities.Count == 0)
		{
			return num;
		}
		for (int i = 0; i < specialConditionFavorabilities.Count; i++)
		{
			List<short> dataList = specialConditionFavorabilities[i].DataList;
			if (dataList.Count == 2)
			{
				if (_effectConfig.SpecialConditionIndices.Count <= i)
				{
					AdaptableLog.Info($"TemplateId:{_templateId} Error in EffectConfig.SpecialConditionIndices ! Must None Less Than Count of EffectConfig.SpecialConditionFavorabilities");
					_baseFavorOdds = num;
					return num;
				}
				List<short> dataList2 = _effectConfig.SpecialConditionIndices[i].DataList;
				if (!dataList2.Contains(3) && ConditionBox_FavorabilityConditionOddsEntrance(dataList[0], dataList2))
				{
					num += dataList[1];
				}
			}
		}
		_baseFavorOdds = num;
		return num;
	}

	private int CalPersonalFavorabilityConditionOdds(int charId)
	{
		int num = 0;
		List<Config.ShortList> specialConditionFavorabilities = _effectConfig.SpecialConditionFavorabilities;
		if (specialConditionFavorabilities.Count == 0)
		{
			return num;
		}
		for (int i = 0; i < specialConditionFavorabilities.Count; i++)
		{
			List<short> dataList = specialConditionFavorabilities[i].DataList;
			if (dataList.Count == 2)
			{
				if (_effectConfig.SpecialConditionIndices.Count <= i)
				{
					AdaptableLog.Info($"TemplateId:{_templateId} Error in EffectConfig.SpecialConditionIndices ! Must None Less Than Count of EffectConfig.SpecialConditionFavorabilities");
					return num;
				}
				List<short> dataList2 = _effectConfig.SpecialConditionIndices[i].DataList;
				if (dataList2.Contains(3) && ConditionBox_FavorabilityConditionOddsEntrance(dataList[0], dataList2, charId))
				{
					num += dataList[1];
				}
			}
		}
		return num;
	}

	private bool ConditionBox_FavorabilityConditionOddsEntrance(short conditionKey, List<short> argIndexList, int extraId = -1)
	{
		if (conditionKey == 0)
		{
			return true;
		}
		List<int> curList = new List<int>(ArgList);
		if (curList.Count > 3)
		{
			curList[3] = extraId;
		}
		argIndexList = ((argIndexList == null) ? new List<short>() : argIndexList.Select((short t) => (short)Math.Clamp((t < 0 || t >= curList.Count()) ? t : curList[t], -32768, 32767)).ToList());
		return ConditionBox((sbyte)conditionKey, (argIndexList.Count > 0) ? argIndexList[0] : (-1), (argIndexList.Count > 1) ? argIndexList[1] : (-1), (argIndexList.Count > 2) ? argIndexList[2] : (-1), (argIndexList.Count > 3) ? argIndexList[3] : (-1));
	}

	private List<int> CalFinalDeltaFavorabilityOfRelatedCharacters_WithRelationIndex(GameData.Domains.Character.Character character, int relationIndex)
	{
		List<int> result = new List<int> { 0, 0, 0, 0 };
		if (ActorIdList.Contains(character.GetId()))
		{
			return result;
		}
		sbyte behaviorType = character.GetBehaviorType();
		int num = _baseFavorOdds + CalPersonalFavorabilityConditionOdds(character.GetId());
		bool flag = num != 0;
		IReadOnlyList<Config.ShortList> readOnlyList;
		switch (relationIndex)
		{
		case 0:
		case 16:
		case 17:
		case 18:
			readOnlyList = (flag ? _effectConfig.OtherFavorabilityDiffsWhenSpecial : _effectConfig.OtherFavorabilityDiffs);
			break;
		case 2:
		case 10:
		case 13:
			readOnlyList = (flag ? _effectConfig.ActorFriendFavorabilityDiffsWhenSpecial : _effectConfig.ActorFriendFavorabilityDiffs);
			break;
		case 3:
			readOnlyList = (flag ? _effectConfig.ActorEnemyFavorabilityDiffsWhenSpecial : _effectConfig.ActorEnemyFavorabilityDiffs);
			break;
		case 5:
		case 11:
		case 14:
			readOnlyList = (flag ? _effectConfig.ReactorFriendFavorabilityDiffsWhenSpecial : _effectConfig.ReactorFriendFavorabilityDiffs);
			break;
		case 6:
			readOnlyList = (flag ? _effectConfig.ReactorEnemyFavorabilityDiffsWhenSpecial : _effectConfig.ReactorEnemyFavorabilityDiffs);
			break;
		case 8:
		case 12:
		case 15:
			readOnlyList = (flag ? _effectConfig.SecactorFriendFavorabilityDiffsWhenSpecial : _effectConfig.SecactorFriendFavorabilityDiffs);
			break;
		case 9:
			readOnlyList = (flag ? _effectConfig.SecactorEnemyFavorabilityDiffsWhenSpecial : _effectConfig.SecactorEnemyFavorabilityDiffs);
			break;
		default:
			return result;
		}
		result = new List<int>();
		for (int i = 0; i < 4; i++)
		{
			int num2 = readOnlyList[i].DataList[behaviorType];
			if (flag)
			{
				num2 = num2 * num / 100;
			}
			result.Add(num2);
		}
		return result;
	}

	private List<int> CalFinalDeltaFavorabilityOfActors_WithActorIndex(int actorIndex)
	{
		List<int> result = new List<int> { 0, 0, 0, 0 };
		if (!ActiveActorList.TryGetValue(actorIndex, out var value))
		{
			return result;
		}
		sbyte behaviorType = value.GetBehaviorType();
		bool flag = _baseFavorOdds != 0;
		IReadOnlyList<Config.ShortList> readOnlyList;
		switch (actorIndex)
		{
		case 0:
			readOnlyList = (flag ? _effectConfig.ActorFavorabilityDiffsWhenSpecial : _effectConfig.ActorFavorabilityDiffs);
			break;
		case 1:
			readOnlyList = (flag ? _effectConfig.ReactorFavorabilityDiffsWhenSpecial : _effectConfig.ReactorFavorabilityDiffs);
			break;
		case 2:
			readOnlyList = (flag ? _effectConfig.SecactorFavorabilityDiffsWhenSpecial : _effectConfig.SecactorFavorabilityDiffs);
			break;
		default:
			return result;
		}
		result = new List<int>();
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (i == actorIndex)
			{
				result.Add(0);
				continue;
			}
			int num2 = readOnlyList[num].DataList[behaviorType];
			if (flag)
			{
				num2 = num2 * _baseFavorOdds / 100;
			}
			result.Add(num2);
			num++;
		}
		return result;
	}

	public List<InformationDomain.SecretInformationFavorChangeItem> GetAllSecretInformationFavorabilityChangeWithSource(int sourceCharId)
	{
		Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();
		Dictionary<GameData.Domains.Character.Character, int> allSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex = GetAllSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex();
		List<GameData.Domains.Character.Character> secretInformationDisseminationBranchCharacterIds_WithTuringTest = GetSecretInformationDisseminationBranchCharacterIds_WithTuringTest();
		foreach (KeyValuePair<int, GameData.Domains.Character.Character> activeActor in ActiveActorList)
		{
			int id = activeActor.Value.GetId();
			List<int> list = CalFinalDeltaFavorabilityOfActors_WithActorIndex(activeActor.Key);
			foreach (KeyValuePair<int, GameData.Domains.Character.Character> activeActor2 in ActiveActorList)
			{
				if (activeActor2.Key == activeActor.Key || list[activeActor2.Key] == 0)
				{
					continue;
				}
				int id2 = activeActor2.Value.GetId();
				if (id != id2)
				{
					if (!dictionary.ContainsKey(id))
					{
						dictionary.Add(id, new Dictionary<int, int>());
					}
					if (!dictionary[id].ContainsKey(id2))
					{
						dictionary[id].Add(id2, 0);
					}
					dictionary[id][id2] += list[3];
				}
			}
			if (list[3] != 0 && id != sourceCharId && sourceCharId != -1)
			{
				if (!dictionary.ContainsKey(id))
				{
					dictionary.Add(id, new Dictionary<int, int>());
				}
				if (!dictionary[id].ContainsKey(sourceCharId))
				{
					dictionary[id].Add(sourceCharId, 0);
				}
				dictionary[id][sourceCharId] += list[3];
			}
		}
		foreach (KeyValuePair<GameData.Domains.Character.Character, int> item in allSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex)
		{
			int id3 = item.Key.GetId();
			List<int> list2 = CalFinalDeltaFavorabilityOfRelatedCharacters_WithRelationIndex(item.Key, item.Value);
			foreach (KeyValuePair<int, GameData.Domains.Character.Character> activeActor3 in ActiveActorList)
			{
				int id4 = activeActor3.Value.GetId();
				if (id3 == id4)
				{
					continue;
				}
				int num = list2[activeActor3.Key];
				if (num != 0)
				{
					if (!dictionary.ContainsKey(id3))
					{
						dictionary.Add(id3, new Dictionary<int, int>());
					}
					if (!dictionary[id3].ContainsKey(id4))
					{
						dictionary[id3].Add(id4, 0);
					}
					dictionary[id3][id4] += num;
				}
			}
			if (list2[3] != 0 && id3 != sourceCharId && sourceCharId != -1)
			{
				if (!dictionary.ContainsKey(id3))
				{
					dictionary.Add(id3, new Dictionary<int, int>());
				}
				if (!dictionary[id3].ContainsKey(sourceCharId))
				{
					dictionary[id3].Add(sourceCharId, 0);
				}
				dictionary[id3][sourceCharId] += list2[3];
			}
		}
		List<InformationDomain.SecretInformationFavorChangeItem> list3 = new List<InformationDomain.SecretInformationFavorChangeItem>();
		Dictionary<int, int> dictionary2 = allSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex.ToDictionary((KeyValuePair<GameData.Domains.Character.Character, int> x) => x.Key.GetId(), (KeyValuePair<GameData.Domains.Character.Character, int> x) => x.Value);
		foreach (KeyValuePair<int, Dictionary<int, int>> item2 in dictionary)
		{
			sbyte b = 0;
			if (ActorIdList.Contains(item2.Key))
			{
				b += 10;
			}
			if (item2.Key == ArgList[5])
			{
				b += 10;
			}
			if (DomainManager.Taiwu.IsInGroup(item2.Key))
			{
				b += 5;
			}
			if (dictionary2.TryGetValue(item2.Key, out var value) && value != 0)
			{
				b += 4;
			}
			foreach (KeyValuePair<int, int> item3 in item2.Value)
			{
				sbyte b2 = 0;
				if (item3.Key == ArgList[5])
				{
					b2 += 15;
				}
				list3.Add(new InformationDomain.SecretInformationFavorChangeItem(item2.Key, item3.Key, item3.Value, (sbyte)(b + b2)));
			}
		}
		return list3;
	}

	public List<InformationDomain.SecretInformationStartEnemyRelationItem> GetAllSecretInformationStartEnemyRelationItems(int sourceCharId)
	{
		Dictionary<GameData.Domains.Character.Character, int> allSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex = GetAllSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex(includeActors: true);
		List<InformationDomain.SecretInformationStartEnemyRelationItem> list = new List<InformationDomain.SecretInformationStartEnemyRelationItem>();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		foreach (KeyValuePair<GameData.Domains.Character.Character, int> item3 in allSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex)
		{
			int id = item3.Key.GetId();
			foreach (KeyValuePair<int, GameData.Domains.Character.Character> activeActor in ActiveActorList)
			{
				int id2 = activeActor.Value.GetId();
				if (id != id2 && id != taiwuCharId)
				{
					byte startEnemyRelationOdds = GetStartEnemyRelationOdds(activeActor.Key, item3.Value);
					InformationDomain.SecretInformationStartEnemyRelationItem item = new InformationDomain.SecretInformationStartEnemyRelationItem(GetSecretInformationTemplateId(), id, id2, startEnemyRelationOdds);
					list.Add(item);
				}
			}
			if (id != sourceCharId && sourceCharId != -1 && id != taiwuCharId)
			{
				byte startEnemyRelationOdds2 = GetStartEnemyRelationOdds(3, item3.Value);
				InformationDomain.SecretInformationStartEnemyRelationItem item2 = new InformationDomain.SecretInformationStartEnemyRelationItem(GetSecretInformationTemplateId(), id, sourceCharId, startEnemyRelationOdds2);
				list.Add(item2);
			}
		}
		return list;
	}

	private byte GetStartEnemyRelationOdds(int actorKey, int relatedValue)
	{
		List<byte> list;
		switch (actorKey)
		{
		case 0:
			list = _effectConfig.StartEnemyRelationOddsToActor;
			break;
		case 1:
			list = _effectConfig.StartEnemyRelationOddsToReactor;
			break;
		case 2:
			list = _effectConfig.StartEnemyRelationOddsToSecactor;
			break;
		case 3:
			list = _effectConfig.StartEnemyRelationOddsToSource;
			break;
		default:
			list = new List<byte>();
			AdaptableLog.Info($"wrong actorKey: {actorKey}");
			break;
		}
		if ((uint)(relatedValue - 1) <= 8u)
		{
			return list[relatedValue - 1];
		}
		return 0;
	}

	public List<InformationDomain.SecretInformationHappinessChangeItem> GetAllSecretInformationHappinessChange()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		Dictionary<GameData.Domains.Character.Character, int> allSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex = GetAllSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex();
		if (allSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex == null)
		{
			return new List<InformationDomain.SecretInformationHappinessChangeItem>();
		}
		Dictionary<int, GameData.Domains.Character.Character> activeActorList_WithRelationIndex = GetActiveActorList_WithRelationIndex();
		foreach (KeyValuePair<int, GameData.Domains.Character.Character> item in activeActorList_WithRelationIndex)
		{
			sbyte b = CalDeltaHappinessOfRelatedCharacters_WithRelationIndex(item.Key);
			if (b != 0)
			{
				int id = item.Value.GetId();
				if (!dictionary.ContainsKey(id))
				{
					dictionary.Add(id, 0);
				}
				dictionary[id] += b;
			}
		}
		foreach (KeyValuePair<GameData.Domains.Character.Character, int> item2 in allSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex)
		{
			sbyte b2 = CalDeltaHappinessOfRelatedCharacters_WithRelationIndex(item2.Value);
			if (b2 != 0)
			{
				int id2 = item2.Key.GetId();
				if (!dictionary.ContainsKey(id2))
				{
					dictionary.Add(id2, 0);
				}
				dictionary[id2] += b2;
			}
		}
		List<InformationDomain.SecretInformationHappinessChangeItem> list = new List<InformationDomain.SecretInformationHappinessChangeItem>();
		Dictionary<int, int> dictionary2 = allSecretInformationRelatedCharacters_WithTuringTestAndRelationIndex.ToDictionary((KeyValuePair<GameData.Domains.Character.Character, int> x) => x.Key.GetId(), (KeyValuePair<GameData.Domains.Character.Character, int> x) => x.Value);
		foreach (KeyValuePair<int, int> item3 in dictionary)
		{
			sbyte b3 = 0;
			if (ActorIdList.Contains(item3.Key))
			{
				b3 += 10;
			}
			if (DomainManager.Taiwu.IsInGroup(item3.Key))
			{
				b3 += 5;
			}
			if (item3.Key == ArgList[5])
			{
				b3 += 10;
			}
			if (dictionary2.TryGetValue(item3.Key, out var value) && value != 0)
			{
				b3 += 4;
			}
			list.Add(new InformationDomain.SecretInformationHappinessChangeItem(item3.Key, item3.Value, b3));
		}
		return list;
	}

	public sbyte CalDeltaHappinessOfRelatedCharacters_WithRelationIndex(int relationIndex)
	{
		switch (relationIndex)
		{
		case 1:
			return _effectConfig.ActorHappinessDiffs[0];
		case 2:
		case 10:
		case 13:
			return _effectConfig.ActorHappinessDiffs[1];
		case 3:
			return _effectConfig.ActorHappinessDiffs[2];
		case 4:
			return _effectConfig.ReactorHappinessDiffs[0];
		case 5:
		case 11:
		case 14:
			return _effectConfig.ReactorHappinessDiffs[1];
		case 6:
			return _effectConfig.ReactorHappinessDiffs[2];
		case 7:
			return _effectConfig.SecactorHappinessDiffs[0];
		case 8:
		case 12:
		case 15:
			return _effectConfig.SecactorHappinessDiffs[1];
		case 9:
			return _effectConfig.SecactorHappinessDiffs[2];
		default:
			return 0;
		}
	}

	public List<short> GetActorFameRecord_WithActorIndex(int actorIndex, bool includeDeath = false)
	{
		if ((actorIndex < 0 || actorIndex > 2) ? true : false)
		{
			return new List<short>();
		}
		if (!includeDeath && !ActiveActorList.TryGetValue(actorIndex, out var _))
		{
			return new List<short>();
		}
		List<(short, sbyte)> list = new List<(short, sbyte)>();
		if (1 == 0)
		{
		}
		List<Config.ShortList> list2 = actorIndex switch
		{
			0 => _effectConfig.ActorFameApplyCondition, 
			1 => _effectConfig.ReactorFameApplyCondition, 
			2 => _effectConfig.SeactorFameApplyCondition, 
			_ => throw new Exception($"actorIndex impossible to other case {actorIndex}"), 
		};
		if (1 == 0)
		{
		}
		IReadOnlyList<Config.ShortList> source = list2;
		if (1 == 0)
		{
		}
		list2 = actorIndex switch
		{
			0 => _effectConfig.ActorFameApplyContent, 
			1 => _effectConfig.ReactorFameApplyContent, 
			2 => _effectConfig.SecactorFameApplyContent, 
			_ => throw new Exception($"actorIndex impossible to other case {actorIndex}"), 
		};
		if (1 == 0)
		{
		}
		IReadOnlyList<Config.ShortList> source2 = list2;
		foreach (List<short> item4 in source.Select((Config.ShortList shortList) => shortList.DataList))
		{
			if (item4.Count < 2)
			{
				continue;
			}
			short num = item4[0];
			if (num < 0)
			{
				continue;
			}
			bool flag;
			short reasonKey;
			if (item4[1] == 54)
			{
				int argIndexByRelationConfigDefKey = GetArgIndexByRelationConfigDefKey(item4[2]);
				flag = CalSectPunishLevel_WithActorIndex(argIndexByRelationConfigDefKey, out reasonKey) >= 0;
			}
			else
			{
				flag = ConditionBoxEntrance(item4, 1, -1, convertRelationDefKeyToArgIndex: true);
			}
			if (!flag)
			{
				continue;
			}
			sbyte item = -1;
			int num2 = 1;
			foreach (List<short> item5 in source2.Select((Config.ShortList shortList) => shortList.DataList))
			{
				if (num != item5[0])
				{
					continue;
				}
				if (item5.Count > 2)
				{
					num2 = item5[2];
					if (num == 82)
					{
						PunishmentSeverityItem item2 = PunishmentSeverity.Instance.GetItem(CalSectPunishLevel_WithActorIndex(actorIndex, out reasonKey));
						if (item2 != null)
						{
							num2 *= item2.FameActionFactorInPunish;
						}
					}
				}
				int argIndexByRelationConfigDefKey2 = GetArgIndexByRelationConfigDefKey(item5[1]);
				if (argIndexByRelationConfigDefKey2 > 0 && argIndexByRelationConfigDefKey2 < ArgList.Count)
				{
					item = GetFameTypeSafe(ArgList[argIndexByRelationConfigDefKey2]);
				}
			}
			for (int num3 = 0; num3 < num2; num3++)
			{
				list.Add((num, item));
			}
		}
		List<short> list3 = new List<short>();
		foreach (var item6 in list)
		{
			short num4 = item6.Item1;
			FameActionItem item3 = FameAction.Instance.GetItem(num4);
			if (item3 == null)
			{
				continue;
			}
			if (item3.HasJump && item6.Item2 != -1)
			{
				num4 = ((item6.Item2 == 3 || item6.Item2 == -2) ? item3.NormalJumpId : ((item6.Item2 >= 3) ? item3.GoodJumpId : item3.BadJumpId));
				item3 = FameAction.Instance.GetItem(num4);
				if (item3 == null)
				{
					continue;
				}
			}
			list3.Add(num4);
		}
		return list3;
	}

	public List<short> GetActorFameRecord_WithCharId(int charId, bool includeDeath = false)
	{
		if (!ActorIdList.Contains(charId))
		{
			return new List<short>();
		}
		int actorIndex = ArgList.FindIndex((int x) => x == charId);
		return GetActorFameRecord_WithActorIndex(actorIndex, includeDeath);
	}

	public int IsActorFameRecordPositive_WithActorIndex(int actorIndex)
	{
		List<short> actorFameRecord_WithActorIndex = GetActorFameRecord_WithActorIndex(actorIndex, includeDeath: true);
		short num = 0;
		foreach (short item2 in actorFameRecord_WithActorIndex)
		{
			FameActionItem item = FameAction.Instance.GetItem(item2);
			if (item != null)
			{
				num += item.Fame;
			}
		}
		return num;
	}

	public int IsActorFameRecordPositive_WithCharId(int charId)
	{
		if (!ActorIdList.Contains(charId))
		{
			return 0;
		}
		int actorIndex = ArgList.FindIndex((int x) => x == charId);
		return IsActorFameRecordPositive_WithActorIndex(actorIndex);
	}

	public short GetSecretInformationAppliedStructs(IRandomSource random, GameData.Domains.Character.Character character, GameData.Domains.Character.Character taiwu)
	{
		short result = -1;
		if (_infoConfig.StructGroupId == -1)
		{
			return result;
		}
		List<int> taiwuIndexList = GetAllSecretInformationRelationsOfCharacter(taiwu.GetId(), includeLoveRelation: true, includeLeadership: false);
		List<int> charIndexList = GetAllSecretInformationRelationsOfCharacter(character.GetId(), includeLoveRelation: true, includeLeadership: false);
		List<(short, short, short[])> list = (from s in SecretInformationAppliedStruct.Instance
			where s != null && s.GroupTemplateId == _infoConfig.StructGroupId && taiwuIndexList.Contains(s.TaiwuIndex) && charIndexList.Contains(s.CharIndex)
			select (TemplateId: s.TemplateId, RelationValue: s.RelationValue, BehaviorTypeValue: s.BehaviorTypeValue)).ToList();
		sbyte behaviorType = character.GetBehaviorType();
		if (list.Count != 0)
		{
			if (list.Count == 1)
			{
				return list[0].Item1;
			}
			result = list.ElementAt(random.Next(0, list.Count)).Item1;
			Dictionary<short, List<(short, short, short[])>> dictionary = new Dictionary<short, List<(short, short, short[])>>();
			foreach (var item in list)
			{
				if (!dictionary.ContainsKey(item.Item2))
				{
					dictionary.Add(item.Item2, new List<(short, short, short[])>());
				}
				dictionary[item.Item2].Add(item);
			}
			short key = dictionary.Keys.Max();
			if (dictionary[key].Count != 0)
			{
				if (dictionary[key].Count == 1)
				{
					return dictionary[key][0].Item1;
				}
				result = dictionary[key].ElementAt(random.Next(0, dictionary[key].Count)).Item1;
				List<short> list2 = new List<short>();
				short num = (short)(100 / dictionary[key].Count);
				foreach (var item2 in dictionary[key])
				{
					short num2 = item2.Item3[behaviorType];
					if (num2 == -1)
					{
						num2 = num;
					}
					for (int num3 = 0; num3 < num2; num3++)
					{
						list2.Add(item2.Item1);
					}
				}
				if (list2.Count != 0)
				{
					result = list2.ElementAt(EventHelper.GetRandom(0, list2.Count));
				}
				else
				{
					AdaptableLog.Info($"No Available Structs In BehaviorTypeValue Check! metaDataId:{_metaDataId} TemplateId:{_templateId}");
				}
			}
			else
			{
				AdaptableLog.Info($"No Available Structs In RelationValue Check! metaDataId:{_metaDataId} TemplateId:{_templateId}");
			}
		}
		else
		{
			AdaptableLog.Info($"No Available SecretInformationAppliedStructs! metaDataId:{_metaDataId} TemplateId:{_templateId}");
		}
		return result;
	}

	public short GetContentIdAndSelections(IRandomSource random, short structId, GameData.Domains.Character.Character character, GameData.Domains.Character.Character taiwu, out List<short> selectionList, out short contentIndex)
	{
		short result = -1;
		contentIndex = -1;
		selectionList = new List<short>();
		SecretInformationAppliedStructItem item = SecretInformationAppliedStruct.Instance.GetItem(structId);
		if (item == null)
		{
			return result;
		}
		List<Config.ShortList> extraContentIds = item.ExtraContentIds;
		List<Config.ShortList> actorSectPunishSpecialCondition = item.ActorSectPunishSpecialCondition;
		if (item.ContentId2 != -1 && !ConditionIsPublished() && !DomainManager.Information.IsSecretInformationInBroadcast(_metaDataId))
		{
			sbyte behaviorType = character.GetBehaviorType();
			short num = CalBaseKeepRateBase(character.GetId(), behaviorType, actorSectPunishSpecialCondition);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(character.GetId(), taiwu.GetId()));
			int num2 = num * (100 - (favorabilityType - 20) * 20) / 100;
			if (random.Next(0, 100) < num2)
			{
				contentIndex = 2;
				result = item.ContentId2;
			}
		}
		if (contentIndex == -1 && extraContentIds.Count > 0)
		{
			for (int i = 0; i < extraContentIds.Count; i++)
			{
				List<short> dataList = extraContentIds[i].DataList;
				if (dataList.Count >= 2 && ConditionBoxEntrance(dataList, 1, character.GetId()))
				{
					contentIndex = (short)(i + 3);
					result = dataList[0];
				}
			}
		}
		if (contentIndex == -1)
		{
			contentIndex = 1;
			result = item.ContentId1;
		}
		List<short> list = new List<short>();
		if (contentIndex > 2)
		{
			if (item.ExtraSelections.Count() > contentIndex - 3)
			{
				list = item.ExtraSelections[contentIndex - 3].DataList;
			}
			else
			{
				AdaptableLog.Info($"No Available ExtraSelections ! metaDataId:{_metaDataId} TemplateId:{_templateId} StructId:{structId}");
			}
		}
		else if (contentIndex == 2)
		{
			if (item.Selection2 != null)
			{
				list = item.Selection2.ToList();
			}
			else
			{
				AdaptableLog.Info($"No Available Selection For Keep ! metaDataId:{_metaDataId} TemplateId:{_templateId} StructId:{structId}");
			}
		}
		else if (item.Selection1 != null)
		{
			list = item.Selection1.ToList();
		}
		else
		{
			AdaptableLog.Info($"No Available Selection For Default! metaDataId:{_metaDataId} TemplateId:{_templateId} StructId:{structId}");
		}
		if (list == null)
		{
			list = new List<short>();
			AdaptableLog.Info($"Fail To Get Selection! metaDataId:{_metaDataId} TemplateId:{_templateId} StructId:{structId}");
		}
		list.RemoveAll((short x) => x < 0);
		selectionList = list;
		return result;
	}

	private short CalBaseKeepRateBase(int charId, sbyte behaviorType, List<Config.ShortList> keepCondition)
	{
		short num = 0;
		foreach (Config.ShortList item in keepCondition)
		{
			List<short> dataList = item.DataList;
			if (dataList.Count != 0)
			{
				short num2 = dataList[0];
				if (num2 == 0)
				{
					num += SecretInformationEffect.Instance[_templateId].BaseSecretRate[behaviorType];
				}
				else if (ConditionBoxEntrance(dataList, 0, charId))
				{
					num += SecretInformationSpecialCondition.Instance[num2].RequestKeepSecretRate[behaviorType];
				}
			}
		}
		return num;
	}

	public List<short> GetVisibleSelection(IEnumerable<short> selectionKeys, GameData.Domains.Character.Character character, GameData.Domains.Character.Character taiwu)
	{
		List<(short, short)> list = new List<(short, short)>();
		foreach (short selectionKey in selectionKeys)
		{
			if (selectionKey < 0)
			{
				continue;
			}
			SecretInformationAppliedSelectionItem item = SecretInformationAppliedSelection.Instance.GetItem(selectionKey);
			if (item == null)
			{
				continue;
			}
			if (item.SpecialConditionId != null)
			{
				bool flag = true;
				foreach (Config.ShortList item2 in item.SpecialConditionId)
				{
					List<short> dataList = item2.DataList;
					if (dataList.Count != 0)
					{
						short num = dataList[0];
						if (num > 0 && !ConditionBoxEntrance(dataList, 0, character.GetId()))
						{
							flag = false;
							break;
						}
					}
				}
				if (!flag)
				{
					continue;
				}
			}
			if (item.SpecialConditionId2 != null && item.SpecialConditionId2.Count != 0)
			{
				bool flag2 = false;
				foreach (Config.ShortList item3 in item.SpecialConditionId2)
				{
					List<short> dataList2 = item3.DataList;
					if (dataList2.Count == 0)
					{
						flag2 = true;
						break;
					}
					short num2 = dataList2[0];
					if (num2 <= 0)
					{
						flag2 = true;
						break;
					}
					if (ConditionBoxEntrance(dataList2, 0, character.GetId()))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					continue;
				}
			}
			sbyte fameType = taiwu.GetFameType();
			sbyte fameType2 = character.GetFameType();
			sbyte[] fameConditions = item.FameConditions;
			if (fameConditions == null || SelectionFameCheck(new List<sbyte>
			{
				fameType,
				fameType2,
				(sbyte)((fameConditions[2] == -1) ? (-1) : GetFameTypeSafe(ArgList[1])),
				(sbyte)((fameConditions[3] == -1) ? (-1) : GetFameTypeSafe(ArgList[3]))
			}, fameConditions))
			{
				short num3 = item.Priority;
				if (num3 < 0)
				{
					num3 = short.MaxValue;
				}
				list.Add((selectionKey, num3));
			}
		}
		return (from x in list
			orderby x.Priority, x.templateId
			select x.templateId).ToList();
	}

	private bool SelectionFameCheck(List<sbyte> fameTypes, sbyte[] fameCondition)
	{
		for (int i = 0; i < fameCondition.Length; i++)
		{
			if (fameCondition[i] == -1)
			{
				continue;
			}
			if (fameTypes[i] == -1)
			{
				return false;
			}
			switch (fameCondition[i])
			{
			case 0:
				if (!FameType.IsNonNegative(fameTypes[i]))
				{
					break;
				}
				return false;
			case 1:
				if (FameType.IsNonNegative(fameTypes[i]))
				{
					break;
				}
				return false;
			}
		}
		return true;
	}

	private short GetResultIdOfSelection(short selectionId, GameData.Domains.Character.Character character, GameData.Domains.Character.Character taiwu)
	{
		SecretInformationAppliedSelectionItem item = SecretInformationAppliedSelection.Instance.GetItem(selectionId);
		sbyte roleBehavior = EventHelper.GetRoleBehavior(character);
		sbyte b = item.Result2FavorabilityTypeCondition[roleBehavior];
		if (EventHelper.GetFavorabilityType(character, taiwu) < b)
		{
			return item.ResultId2;
		}
		return item.ResultId1;
	}

	public sbyte CalSectPunishLevel_WithCharId(int charId, bool calRealLevel = false)
	{
		if (!ActorIdList.Contains(charId))
		{
			return -1;
		}
		int charIndex = ArgList.FindIndex((int a) => a == charId);
		short reasonKey;
		return CalSectPunishLevel_WithActorIndex(charIndex, out reasonKey, calRealLevel);
	}

	internal sbyte CalcSectPunishLevelWithSpecificOrganization(int charIndex, out short reasonKey, OrganizationInfo organizationInfo)
	{
		reasonKey = -1;
		OrganizationItem item = Config.Organization.Instance.GetItem(organizationInfo.OrgTemplateId);
		if (item == null)
		{
			return -1;
		}
		bool isSect = item.IsSect;
		List<Config.ShortList> list;
		List<Config.ShortList> list2;
		List<Config.ShortList> list3;
		List<Config.ShortList> list4;
		List<Config.ShortList> list5;
		switch (charIndex)
		{
		case 0:
			list = (isSect ? _sectPunishConfig.ActorSectPunishFreeCondition : _sectPunishConfig.ActorCityPunishFreeCondition);
			list2 = (isSect ? _sectPunishConfig.ActorSectPunishBase : _sectPunishConfig.ActorCityPunishBase);
			list3 = (isSect ? _sectPunishConfig.ActorSectPunishCondition : _sectPunishConfig.ActorCityPunishCondition);
			list4 = (isSect ? _sectPunishConfig.ActorSectPunishSpecialCondition : _sectPunishConfig.ActorCityPunishSpecialCondition);
			list5 = (isSect ? _sectPunishConfig.ActorSectPunishSpecial : _sectPunishConfig.ActorCityPunishSpecial);
			break;
		case 1:
			list = (isSect ? _sectPunishConfig.ReactorSectPunishFreeCondition : _sectPunishConfig.ReactorCityPunishFreeCondition);
			list2 = (isSect ? _sectPunishConfig.ReactorSectPunishBase : _sectPunishConfig.ReactorCityPunishBase);
			list3 = (isSect ? _sectPunishConfig.ReactorSectPunishCondition : _sectPunishConfig.ReactorCityPunishCondition);
			list4 = (isSect ? _sectPunishConfig.ReactorSectPunishSpecialCondition : _sectPunishConfig.ReactorCityPunishSpecialCondition);
			list5 = (isSect ? _sectPunishConfig.ReactorSectPunishSpecial : _sectPunishConfig.ReactorCityPunishSpecial);
			break;
		case 2:
			list = (isSect ? _sectPunishConfig.SecactorSectPunishFreeCondition : _sectPunishConfig.SecactorCityPunishFreeCondition);
			list2 = (isSect ? _sectPunishConfig.SecactorSectPunishBase : _sectPunishConfig.SecactorCityPunishBase);
			list3 = (isSect ? _sectPunishConfig.SecactorSectPunishCondition : _sectPunishConfig.SecactorCityPunishCondition);
			list4 = (isSect ? _sectPunishConfig.SecactorSectPunishSpecialCondition : _sectPunishConfig.SecactorCityPunishSpecialCondition);
			list5 = (isSect ? _sectPunishConfig.SecactorSectPunishSpecial : _sectPunishConfig.SecactorCityPunishSpecial);
			break;
		default:
			return -1;
		}
		foreach (Config.ShortList item2 in list)
		{
			List<short> dataList = item2.DataList;
			if (dataList.Count < 1 || !ConditionBoxEntrance(dataList))
			{
				continue;
			}
			return -1;
		}
		if (list4 != null && list4.Count > 0)
		{
			for (int i = 0; i < list4.Count; i++)
			{
				List<short> dataList2 = list4[i].DataList;
				if (dataList2.Count < 1 || !ConditionBoxEntrance(dataList2))
				{
					continue;
				}
				if (list5.Count > i)
				{
					List<short> dataList3 = list5[i].DataList;
					if (!dataList3.CheckIndex(0))
					{
						AdaptableLog.Warning("Error in Punish SpecialLevel!");
						return -1;
					}
					sbyte b = CalcPunishLevelByPunishmentType(dataList3[0], organizationInfo.SettlementId);
					if (b >= 0)
					{
						reasonKey = list5[i].DataList[0];
						return GetCustomizedPunishmentSeverity(reasonKey, b, organizationInfo.OrgTemplateId);
					}
				}
				else
				{
					AdaptableLog.Warning("Error in Punish SpecialLevel Count!");
				}
			}
		}
		if (list3 != null && list3.Count > 0 && list2 != null)
		{
			for (int j = 0; j < list3.Count; j++)
			{
				List<short> dataList4 = list3[j].DataList;
				if (dataList4.Count < 1 || !ConditionBoxEntrance(dataList4))
				{
					continue;
				}
				if (list2.Count > j)
				{
					List<short> dataList5 = list2[j].DataList;
					if (!dataList5.CheckIndex(0))
					{
						AdaptableLog.Warning("Error in Punish BaseLevel!");
						return -1;
					}
					sbyte b2 = CalcPunishLevelByPunishmentType(dataList5[0], organizationInfo.SettlementId);
					if (b2 < 0)
					{
						return b2;
					}
					if (list2.CheckIndex(j))
					{
						reasonKey = list2[j].DataList[0];
					}
					return GetCustomizedPunishmentSeverity(reasonKey, b2, organizationInfo.OrgTemplateId);
				}
				AdaptableLog.Warning("Error in Base SpecialLevel Count!");
			}
		}
		return -1;
	}

	internal sbyte GetCustomizedPunishmentSeverity(short punishmentTypeTemplateId, sbyte punishmentSeverity, sbyte orgTemplateId)
	{
		if (punishmentTypeTemplateId < 0)
		{
			return punishmentSeverity;
		}
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId);
		if (settlementByOrgTemplateId != null)
		{
			PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[punishmentTypeTemplateId];
			punishmentSeverity = settlementByOrgTemplateId.GetPunishmentTypeSeverity(punishmentTypeCfg);
		}
		return punishmentSeverity;
	}

	internal sbyte CalcPunishLevelByPunishmentType(short punishmentTypeTemplateId, short settlementId)
	{
		PunishmentTypeItem item = PunishmentType.Instance.GetItem(punishmentTypeTemplateId);
		if (item != null && settlementId >= 0)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			OrganizationItem item2 = Config.Organization.Instance.GetItem(settlement.GetOrgTemplateId());
			return item.GetSeverity(DomainManager.Map.GetStateTemplateIdByAreaId(settlement.GetLocation().AreaId), item2?.IsSect ?? false);
		}
		return -1;
	}

	public sbyte CalSectPunishLevel_WithActorIndex(int charIndex, out short reasonKey, bool calRealLevel = false)
	{
		reasonKey = -1;
		if (charIndex < 0 || charIndex > 2 || charIndex >= ArgList.Count)
		{
			return -1;
		}
		int num = ArgList[charIndex];
		if (num == -1)
		{
			return -1;
		}
		OrganizationInfo sectInfoSafe = GetSectInfoSafe(num);
		if (!calRealLevel)
		{
			int orgIndex = -1;
			if (!CheckCanBePunished(num, ref orgIndex, out var _))
			{
				return -1;
			}
		}
		return CalcSectPunishLevelWithSpecificOrganization(charIndex, out reasonKey, sectInfoSafe);
	}

	public sbyte CalcTaiwuPunishLevel(int taiwuCharId, out short reasonKey, out OrganizationInfo organizationInfo)
	{
		reasonKey = -1;
		organizationInfo = OrganizationInfo.None;
		int num = -1;
		for (int i = 0; i <= 2; i++)
		{
			if (ArgList.CheckIndex(i))
			{
				int num2 = ArgList[i];
				if (taiwuCharId == num2)
				{
					num = i;
				}
			}
		}
		if (num < 0)
		{
			return -1;
		}
		List<Config.ShortList> list;
		List<Config.ShortList> list2;
		List<Config.ShortList> list3;
		List<Config.ShortList> list4;
		List<Config.ShortList> list5;
		switch (num)
		{
		case 0:
			list = _sectPunishConfig.ActorTaiwuPunishFreeCondition;
			list2 = _sectPunishConfig.ActorTaiwuPunishBase;
			list3 = _sectPunishConfig.ActorTaiwuPunishCondition;
			list4 = _sectPunishConfig.ActorTaiwuPunishSpecialCondition;
			list5 = _sectPunishConfig.ActorTaiwuPunishSpecial;
			break;
		case 1:
			list = _sectPunishConfig.ReactorTaiwuPunishFreeCondition;
			list2 = _sectPunishConfig.ReactorTaiwuPunishBase;
			list3 = _sectPunishConfig.ReactorTaiwuPunishCondition;
			list4 = _sectPunishConfig.ReactorTaiwuPunishSpecialCondition;
			list5 = _sectPunishConfig.ReactorTaiwuPunishSpecial;
			break;
		case 2:
			list = _sectPunishConfig.SecactorTaiwuPunishFreeCondition;
			list2 = _sectPunishConfig.SecactorTaiwuPunishBase;
			list3 = _sectPunishConfig.SecactorTaiwuPunishCondition;
			list4 = _sectPunishConfig.SecactorTaiwuPunishSpecialCondition;
			list5 = _sectPunishConfig.SecactorTaiwuPunishSpecial;
			break;
		default:
			return -1;
		}
		for (int j = 0; j < ArgList.Count; j++)
		{
			int num3 = ArgList[j];
			if (num3 == taiwuCharId)
			{
				continue;
			}
			organizationInfo = GetSectInfoSafe(num3);
			if (organizationInfo.OrgTemplateId < 0)
			{
				GameData.Domains.Character.Character element;
				if (DomainManager.Character.TryGetDeadCharacter(num3, out var character))
				{
					organizationInfo = character.OrganizationInfo;
				}
				else if (DomainManager.Character.TryGetElement_Objects(num3, out element))
				{
					organizationInfo = element.GetOrganizationInfo();
				}
			}
			OrganizationItem item = Config.Organization.Instance.GetItem(organizationInfo.OrgTemplateId);
			if (item == null)
			{
				continue;
			}
			if (item.IsSect)
			{
				Location location = DomainManager.Organization.GetSettlement(organizationInfo.SettlementId).GetLocation();
				if (location.IsValid())
				{
					sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
					MapStateItem mapStateItem = MapState.Instance[stateTemplateIdByAreaId];
					MapAreaItem item2 = MapArea.Instance.GetItem(mapStateItem.MainAreaID);
					Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(item2.OrganizationId[0]);
					if (settlementByOrgTemplateId != null)
					{
						item = Config.Organization.Instance.GetItem(settlementByOrgTemplateId.GetOrgTemplateId());
					}
				}
			}
			bool isSect = item.IsSect;
			foreach (Config.ShortList item3 in list)
			{
				List<short> dataList = item3.DataList;
				if (dataList.Count < 1 || !ConditionBoxEntrance(dataList))
				{
					continue;
				}
				return -1;
			}
			if (isSect && list4.Count > 0)
			{
				for (int k = 0; k < list4.Count; k++)
				{
					List<short> dataList2 = list4[k].DataList;
					if (dataList2.Count < 1 || !ConditionBoxEntrance(dataList2))
					{
						continue;
					}
					if (list5.Count > k)
					{
						List<short> dataList3 = list5[k].DataList;
						if (!dataList3.CheckIndex(0))
						{
							AdaptableLog.Warning("Error in Punish SpecialLevel!");
							return -1;
						}
						sbyte b = CalcPunishLevelByPunishmentType(dataList3[0], organizationInfo.SettlementId);
						if (b < 0)
						{
							return b;
						}
						reasonKey = list5[k].DataList[0];
						return GetCustomizedPunishmentSeverity(reasonKey, b, organizationInfo.OrgTemplateId);
					}
					AdaptableLog.Warning("Error in Punish SpecialLevel Count!");
				}
			}
			if (list3 == null || list3.Count <= 0)
			{
				continue;
			}
			for (int l = 0; l < list3.Count; l++)
			{
				List<short> dataList4 = list3[l].DataList;
				if (dataList4.Count >= 1 && ConditionBoxEntrance(dataList4) && list2.Count > l)
				{
					List<short> dataList5 = list2[l].DataList;
					if (!dataList5.CheckIndex(0))
					{
						AdaptableLog.Warning("Error in Punish BaseLevel!");
						return -1;
					}
					sbyte b2 = CalcPunishLevelByPunishmentType(dataList5[0], organizationInfo.SettlementId);
					if (b2 < 0)
					{
						return b2;
					}
					if (list2.CheckIndex(l))
					{
						reasonKey = list2[l].DataList[0];
					}
					return GetCustomizedPunishmentSeverity(reasonKey, b2, organizationInfo.OrgTemplateId);
				}
			}
		}
		return -1;
	}

	public bool CheckCanBePunished(int actorId, ref int orgIndex, out bool isSect)
	{
		isSect = false;
		if (!DomainManager.Character.TryGetElement_Objects(actorId, out var element))
		{
			return false;
		}
		if (!InformationDomain.CheckTuringTest(element))
		{
			return false;
		}
		OrganizationInfo organizationInfo = element.GetOrganizationInfo();
		if (organizationInfo.OrgTemplateId < 0)
		{
			return false;
		}
		OrganizationItem organizationItem = Config.Organization.Instance[organizationInfo.OrgTemplateId];
		OrganizationInfo sectInfoSafe = GetSectInfoSafe(actorId);
		if (sectInfoSafe.OrgTemplateId == -1)
		{
			return false;
		}
		if (organizationItem.IsSect)
		{
			isSect = true;
			orgIndex = sectInfoSafe.OrgTemplateId - 1;
		}
		else
		{
			orgIndex = sectInfoSafe.OrgTemplateId - 21;
		}
		if (organizationInfo.OrgTemplateId == sectInfoSafe.OrgTemplateId)
		{
			return true;
		}
		return false;
	}

	internal int GetArgIndexByRelationConfigDefKey(short relationDefKey)
	{
		if (1 == 0)
		{
		}
		int result = relationDefKey switch
		{
			0 => -1, 
			1 => 0, 
			4 => 1, 
			7 => 2, 
			19 => 4, 
			-1 => -1, 
			_ => throw new Exception($"unsupported relation reference in secret fame condition: {relationDefKey}"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public bool ConditionBoxEntrance(List<short> conditionItem, int conditionKeyIndex = 0, int extraId = -1, bool convertRelationDefKeyToArgIndex = false)
	{
		if (conditionItem.Count <= conditionKeyIndex)
		{
			return false;
		}
		short num = conditionItem[conditionKeyIndex];
		short num2 = num;
		short num3 = num2;
		if (num3 >= 0)
		{
			if (num3 == 0)
			{
				return true;
			}
			List<int> list = new List<int>();
			List<int> list2 = new List<int>(ArgList);
			if (list2.Count > 3)
			{
				list2[3] = extraId;
			}
			for (int i = conditionKeyIndex + 1; i < conditionItem.Count; i++)
			{
				int num4 = conditionItem[i];
				if (convertRelationDefKeyToArgIndex)
				{
					num4 = GetArgIndexByRelationConfigDefKey((short)num4);
				}
				list.Add((num4 >= 0 && num4 < list2.Count) ? list2[num4] : num4);
			}
			return ConditionBox(num, (list.Count > 0) ? list[0] : (-1), (list.Count > 1) ? list[1] : (-1), (list.Count > 2) ? list[2] : (-1), (list.Count > 3) ? list[3] : (-1));
		}
		return false;
	}

	public bool ConditionBox(short conditionKey, int actorId = -1, int reactorId = -1, int secactorId = -1, int extraId = -1)
	{
		SecretInformationSpecialConditionItem item = SecretInformationSpecialCondition.Instance.GetItem(conditionKey);
		if (item == null)
		{
			return false;
		}
		ESecretInformationSpecialConditionCalculate calculate = item.Calculate;
		if (1 == 0)
		{
		}
		bool result = calculate switch
		{
			ESecretInformationSpecialConditionCalculate.SameSect => ConditionSameSect(actorId, reactorId), 
			ESecretInformationSpecialConditionCalculate.SectJustice => ConditionSectJustice(actorId, reactorId), 
			ESecretInformationSpecialConditionCalculate.SectBecomeEnemy => ConditionSectBecomeEnemy(actorId, reactorId, secactorId), 
			ESecretInformationSpecialConditionCalculate.ForbidMarriage => ConditionForbidMarriage(actorId, reactorId), 
			ESecretInformationSpecialConditionCalculate.IsPublished => ConditionIsPublished(), 
			ESecretInformationSpecialConditionCalculate.IsRevealed => ConditionIsRevealed(actorId, reactorId), 
			ESecretInformationSpecialConditionCalculate.IsRevealedSingle => ConditionIsRevealedSingle(actorId, reactorId), 
			ESecretInformationSpecialConditionCalculate.HasCouple => ConditionHasCouple(actorId), 
			ESecretInformationSpecialConditionCalculate.NotFame => ConditionNotFame(actorId), 
			ESecretInformationSpecialConditionCalculate.IsMonk => ConditionIsMonk(actorId, reactorId), 
			ESecretInformationSpecialConditionCalculate.ForbidWine => ConditionForbidWine(actorId), 
			ESecretInformationSpecialConditionCalculate.ForbidPoison => ConditionForbidPoison(actorId), 
			ESecretInformationSpecialConditionCalculate.ForbidItem => ConditionForbidItem(actorId, reactorId), 
			ESecretInformationSpecialConditionCalculate.HasLove => ConditionHasLove(actorId, reactorId, secactorId), 
			ESecretInformationSpecialConditionCalculate.IsKidnapped => ConditionIsKidnapped(actorId, reactorId, secactorId), 
			ESecretInformationSpecialConditionCalculate.CompareCombatPoint => ConditionCompareCombatPoint(actorId, reactorId), 
			ESecretInformationSpecialConditionCalculate.HasRelation => ConditionHasRelation(actorId, reactorId, secactorId, extraId), 
			ESecretInformationSpecialConditionCalculate.ActorAlive => ConditionActorAlive(actorId, reactorId), 
			ESecretInformationSpecialConditionCalculate.CasualtyInSect => ConditionCasualtyInSect(actorId), 
			ESecretInformationSpecialConditionCalculate.KillFameLine => ConditionKillFameLine(actorId, item.CalcFameLine), 
			ESecretInformationSpecialConditionCalculate.KidnapFameLine => ConditionKidnapFameLine(actorId, item.CalcFameLine), 
			ESecretInformationSpecialConditionCalculate.AlliedSectMember => ConditionAlliedSectMember(actorId, item.CalcOrganization), 
			ESecretInformationSpecialConditionCalculate.BeLoverSectMember => ConditionBeLoverSectMember(actorId, item.CalcOrganization), 
			ESecretInformationSpecialConditionCalculate.BeKyodaiSectMember => ConditionBeKyodaiSectMember(actorId, item.CalcOrganization), 
			ESecretInformationSpecialConditionCalculate.GainParentSectMember => ConditionGainParentSectMember(actorId, item.CalcOrganization), 
			ESecretInformationSpecialConditionCalculate.GainChildSectMember => ConditionGainChildSectMember(actorId, item.CalcOrganization), 
			ESecretInformationSpecialConditionCalculate.DateSectMember => ConditionDateSectMember(actorId, item.CalcOrganization), 
			ESecretInformationSpecialConditionCalculate.ReFoundChildSectMember => ConditionReFoundChildSectMember(actorId, item.CalcOrganization), 
			ESecretInformationSpecialConditionCalculate.BanSexualMate => ConditionBanSexualMate(actorId, item.CalcSexualMateCase, item.CalcSexualMateRule, item.CalcSectRule), 
			ESecretInformationSpecialConditionCalculate.BanEating => ConditionBanEating(item.CalcSectRule), 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private bool ConditionSameSect(int actorId, int reactorId)
	{
		if (actorId == -1 || reactorId == -1)
		{
			return false;
		}
		OrganizationInfo sectInfoSafe = GetSectInfoSafe(actorId);
		if (sectInfoSafe.OrgTemplateId == -1)
		{
			return false;
		}
		OrganizationItem organizationItem = Config.Organization.Instance[sectInfoSafe.OrgTemplateId];
		if (!organizationItem.IsSect)
		{
			return false;
		}
		return GetSectInfoSafe(reactorId).OrgTemplateId == sectInfoSafe.OrgTemplateId;
	}

	private bool ConditionSectJustice(int actorId, int reactorId)
	{
		if (actorId == -1 || reactorId == -1)
		{
			return false;
		}
		OrganizationInfo sectInfoSafe = GetSectInfoSafe(actorId);
		if (sectInfoSafe.OrgTemplateId == -1)
		{
			return false;
		}
		if (GetSectInfoSafe(reactorId).SettlementId == sectInfoSafe.SettlementId)
		{
			return false;
		}
		OrganizationItem organizationItem = Config.Organization.Instance[sectInfoSafe.OrgTemplateId];
		if (!organizationItem.IsSect)
		{
			return false;
		}
		sbyte fameTypeSafe = GetFameTypeSafe(reactorId);
		sbyte goodness = organizationItem.Goodness;
		bool flag = (goodness == -1 && fameTypeSafe > 3) || (goodness == 1 && fameTypeSafe < 3 && fameTypeSafe >= 0);
		bool flag2 = flag;
		if (!flag2)
		{
			bool flag3 = goodness == 0;
			bool flag4 = flag3;
			if (flag4)
			{
				bool flag5 = ((fameTypeSafe == -2 || fameTypeSafe == 3) ? true : false);
				flag4 = flag5;
			}
			flag2 = flag4;
		}
		return flag2;
	}

	private bool ConditionSectBecomeEnemy(int actorId, int reactorId, int seactorId)
	{
		bool flag = false;
		bool flag2 = false;
		if (actorId != -1 && reactorId != -1)
		{
			OrganizationInfo sectInfoSafe = GetSectInfoSafe(actorId);
			OrganizationInfo sectInfoSafe2 = GetSectInfoSafe(reactorId);
			flag = sectInfoSafe.OrgTemplateId != -1 && sectInfoSafe2.OrgTemplateId != -1 && DomainManager.Organization.GetSectFavorability(sectInfoSafe.OrgTemplateId, sectInfoSafe2.OrgTemplateId) == -1;
		}
		if (seactorId != -1 && reactorId != -1)
		{
			OrganizationInfo sectInfoSafe3 = GetSectInfoSafe(seactorId);
			OrganizationInfo sectInfoSafe4 = GetSectInfoSafe(reactorId);
			flag2 = sectInfoSafe3.OrgTemplateId != -1 && sectInfoSafe4.OrgTemplateId != -1 && DomainManager.Organization.GetSectFavorability(sectInfoSafe4.OrgTemplateId, sectInfoSafe3.OrgTemplateId) == -1;
		}
		return flag || flag2;
	}

	private bool ConditionForbidMarriage(int actorId, int reactorId = -1)
	{
		bool flag = false;
		bool flag2 = false;
		if (actorId != -1)
		{
			OrganizationInfo sectInfoSafe = GetSectInfoSafe(actorId);
			if (sectInfoSafe.OrgTemplateId != -1)
			{
				flag = OrganizationMember.Instance[Config.Organization.Instance[sectInfoSafe.OrgTemplateId].Members[sectInfoSafe.Grade]].ChildGrade < 0 || ConditionIsMonk(actorId);
			}
		}
		if (reactorId != -1 && reactorId != actorId)
		{
			OrganizationInfo sectInfoSafe2 = GetSectInfoSafe(reactorId);
			if (sectInfoSafe2.OrgTemplateId != -1)
			{
				flag2 = OrganizationMember.Instance[Config.Organization.Instance[sectInfoSafe2.OrgTemplateId].Members[sectInfoSafe2.Grade]].ChildGrade < 0 || ConditionIsMonk(reactorId);
			}
		}
		return flag || flag2;
	}

	private bool ConditionIsPublished()
	{
		int relevanceSecretInformationMetaDataId = _metaDataRef.GetRelevanceSecretInformationMetaDataId();
		if (relevanceSecretInformationMetaDataId == -1)
		{
			return false;
		}
		return DomainManager.Information.IsSecretInformationInBroadcast(relevanceSecretInformationMetaDataId);
	}

	private bool ConditionIsRevealed(int actorId, int reactorId = -1)
	{
		if (actorId == -1)
		{
			return false;
		}
		if (reactorId == -1)
		{
			return !HasAnySpouse(actorId, includeLover: false, includeActors: true);
		}
		sbyte b = -1;
		sbyte b2 = -1;
		if (DomainManager.Character.TryGetElement_Objects(actorId, out var element))
		{
			b = element.GetGender();
		}
		else
		{
			DeadCharacter deadCharacter = DomainManager.Character.TryGetDeadCharacter(actorId);
			if (deadCharacter != null)
			{
				b = deadCharacter.Gender;
			}
		}
		if (DomainManager.Character.TryGetElement_Objects(reactorId, out var element2))
		{
			b2 = element2.GetGender();
		}
		else
		{
			DeadCharacter deadCharacter2 = DomainManager.Character.TryGetDeadCharacter(reactorId);
			if (deadCharacter2 != null)
			{
				b = deadCharacter2.Gender;
			}
		}
		HashSet<int> secretInformationRelatedCharactersOfSpecialRelation = GetSecretInformationRelatedCharactersOfSpecialRelation(actorId, RelationIndex.Revel, includeAlive: true, includeDead: true, includeActors: true);
		HashSet<int> secretInformationRelatedCharactersOfSpecialRelation2 = GetSecretInformationRelatedCharactersOfSpecialRelation(reactorId, RelationIndex.Revel, includeAlive: true, includeDead: true, includeActors: true);
		return b == b2 || secretInformationRelatedCharactersOfSpecialRelation.Contains(reactorId) || secretInformationRelatedCharactersOfSpecialRelation2.Contains(actorId);
	}

	private bool ConditionIsRevealedSingle(int actorId, int reactorId = -1)
	{
		return !HasAnySpouse(actorId, includeLover: false, includeActors: true) || !HasAnySpouse(reactorId, includeLover: false, includeActors: true);
	}

	private bool ConditionHasCouple(int actorId)
	{
		return HasAnySpouse(actorId, includeLover: true);
	}

	private bool ConditionHasLove(int actorId, int reactorId, int secactorId = -1)
	{
		if (actorId == -1)
		{
			return false;
		}
		HashSet<int> secretInformationRelatedCharactersOfSpecialRelation = GetSecretInformationRelatedCharactersOfSpecialRelation(actorId, RelationIndex.Love, includeAlive: true, includeDead: true, includeActors: true);
		bool flag = secretInformationRelatedCharactersOfSpecialRelation.Contains(reactorId);
		bool flag2 = secretInformationRelatedCharactersOfSpecialRelation.Contains(secactorId);
		return flag || flag2;
	}

	private bool HasAnySpouse(int actorId, bool includeLover = false, bool includeActors = false)
	{
		if (actorId == -1)
		{
			return false;
		}
		List<SecretInformationRelationshipType> relations = (includeLover ? RelationIndex.Love : RelationIndex.Spouse);
		if (ActorIdList.Contains(actorId))
		{
			HashSet<int> secretInformationRelatedCharactersOfSpecialRelation = GetSecretInformationRelatedCharactersOfSpecialRelation(actorId, relations, includeAlive: true, includeDead: false, includeActors);
			return secretInformationRelatedCharactersOfSpecialRelation.Count != 0;
		}
		if (!includeActors)
		{
			return false;
		}
		foreach (int actorId2 in ActorIdList)
		{
			if (Check_HasSpecialRelations(actorId2, actorId, relations))
			{
				return true;
			}
		}
		return false;
	}

	private bool ConditionNotFame(int actorId)
	{
		if (actorId == -1)
		{
			return false;
		}
		sbyte fameTypeSafe = GetFameTypeSafe(actorId);
		if (fameTypeSafe == -1)
		{
			return false;
		}
		return fameTypeSafe < 3 && fameTypeSafe >= 0;
	}

	private bool ConditionIsMonk(int actorId, int reactorId = -1)
	{
		bool flag = false;
		bool flag2 = false;
		if (actorId != -1)
		{
			flag = GetMonkTypeSafe(actorId) != 0;
		}
		if (reactorId != -1)
		{
			flag2 = GetMonkTypeSafe(reactorId) != 0;
		}
		return flag || flag2;
	}

	private bool ConditionForbidWine(int actorId)
	{
		if (actorId == -1)
		{
			return false;
		}
		OrganizationInfo sectInfoSafe = GetSectInfoSafe(actorId);
		if (sectInfoSafe.OrgTemplateId == -1)
		{
			return false;
		}
		OrganizationItem organizationItem = Config.Organization.Instance[sectInfoSafe.OrgTemplateId];
		return organizationItem.NoDrinking;
	}

	private bool ConditionForbidPoison(int actorId)
	{
		if (actorId == -1)
		{
			return false;
		}
		OrganizationInfo sectInfoSafe = GetSectInfoSafe(actorId);
		if (sectInfoSafe.OrgTemplateId == -1)
		{
			return false;
		}
		OrganizationItem organizationItem = Config.Organization.Instance[sectInfoSafe.OrgTemplateId];
		return !organizationItem.AllowPoisoning;
	}

	private bool ConditionForbidItem(int actorId, int itemType)
	{
		if (actorId == -1)
		{
			return false;
		}
		OrganizationInfo sectInfoSafe = GetSectInfoSafe(actorId);
		if (sectInfoSafe.OrgTemplateId == -1)
		{
			return false;
		}
		OrganizationItem organizationItem = Config.Organization.Instance[sectInfoSafe.OrgTemplateId];
		return itemType switch
		{
			9 => organizationItem.NoDrinking, 
			7 => organizationItem.NoMeatEating, 
			_ => false, 
		};
	}

	private bool ConditionIsKidnapped(int actorId, int reactorId, int extra = -1)
	{
		if (actorId == -1 || reactorId == -1)
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(actorId, out var element) || !DomainManager.Character.TryGetElement_Objects(reactorId, out var _))
		{
			return extra != -1;
		}
		bool flag = element.GetKidnapperId() == reactorId;
		return (extra == -1) ? flag : (!flag);
	}

	private bool ConditionCompareCombatPoint(int actorId, int reactorId)
	{
		if (actorId == -1 || reactorId == -1)
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(actorId, out var element) || !DomainManager.Character.TryGetElement_Objects(reactorId, out var element2))
		{
			return false;
		}
		return element.GetCombatPower() >= element2.GetCombatPower();
	}

	private bool ConditionActorAlive(int actorId, int extraId = -1)
	{
		if (actorId == extraId)
		{
			return false;
		}
		GameData.Domains.Character.Character element;
		return DomainManager.Character.TryGetElement_Objects(actorId, out element);
	}

	private bool ConditionCasualtyInSect(int charId)
	{
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			OrganizationInfo organizationInfo = element.GetOrganizationInfo();
			return Config.Organization.Instance.GetItem(organizationInfo.OrgTemplateId).IsSect;
		}
		return false;
	}

	private bool ConditionHasRelation(int actorId, int reactorId, int relationType, int extra = -1)
	{
		if (actorId == -1 || reactorId == -1)
		{
			return false;
		}
		HashSet<SecretInformationRelationshipType> hashSet = DomainManager.Information.CheckSecretInformationRelationship(reactorId, -1, actorId, -1);
		SecretInformationRelationshipType item;
		switch (relationType)
		{
		case -19:
			return (extra == -1) ? (hashSet.Count != 0) : (hashSet.Count == 0);
		case -18:
			item = SecretInformationRelationshipType.Allied;
			break;
		case -17:
			item = SecretInformationRelationshipType.Comrade;
			break;
		case -16:
			item = SecretInformationRelationshipType.Enemy;
			break;
		case -15:
			item = SecretInformationRelationshipType.Adorer;
			break;
		case -14:
			item = SecretInformationRelationshipType.Friend;
			break;
		case -13:
			item = SecretInformationRelationshipType.MentorAndMentee;
			break;
		case -12:
			item = SecretInformationRelationshipType.Lover;
			break;
		case -11:
			item = SecretInformationRelationshipType.HusbandOrWife;
			break;
		case -10:
		{
			bool flag = hashSet.Contains(SecretInformationRelationshipType.HusbandOrWife) || hashSet.Contains(SecretInformationRelationshipType.Lover) || hashSet.Contains(SecretInformationRelationshipType.Adorer);
			return (extra == -1) ? flag : (!flag);
		}
		case -9:
			item = SecretInformationRelationshipType.SwornBrotherOrSister;
			break;
		case -8:
			item = SecretInformationRelationshipType.Relative;
			break;
		case -7:
			item = SecretInformationRelationshipType.ActualBloodFather;
			break;
		case -6:
			item = SecretInformationRelationshipType.RevealedIncest;
			break;
		default:
			return extra != -1;
		}
		bool flag2 = hashSet.Contains(item);
		return (extra == -1) ? flag2 : (!flag2);
	}

	private ESecretInformationSpecialConditionCalcFameLine CalcFameLineByFame(sbyte fameType)
	{
		if (1 == 0)
		{
		}
		ESecretInformationSpecialConditionCalcFameLine result;
		if (fameType < 3)
		{
			if (fameType < 0)
			{
				goto IL_001d;
			}
			result = ESecretInformationSpecialConditionCalcFameLine.Badboy;
		}
		else
		{
			if (fameType <= 3)
			{
				goto IL_001d;
			}
			result = ESecretInformationSpecialConditionCalcFameLine.SuperStar;
		}
		goto IL_0021;
		IL_0021:
		if (1 == 0)
		{
		}
		return result;
		IL_001d:
		result = ESecretInformationSpecialConditionCalcFameLine.Renowned;
		goto IL_0021;
	}

	private bool ConditionKillFameLine(int actorId, ESecretInformationSpecialConditionCalcFameLine conditionCalcFameLine)
	{
		return CalcFameLineByFame(GetFameTypeSafe(actorId)) == conditionCalcFameLine;
	}

	private bool ConditionKidnapFameLine(int actorId, ESecretInformationSpecialConditionCalcFameLine conditionCalcFameLine)
	{
		return CalcFameLineByFame(GetFameTypeSafe(actorId)) == conditionCalcFameLine;
	}

	private bool ConditionAlliedSectMember(int actorId, short conditionCalcOrganization)
	{
		return GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
	}

	private bool ConditionBeLoverSectMember(int actorId, short conditionCalcOrganization)
	{
		return GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
	}

	private bool ConditionBeKyodaiSectMember(int actorId, short conditionCalcOrganization)
	{
		return GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
	}

	private bool ConditionGainParentSectMember(int actorId, short conditionCalcOrganization)
	{
		return GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
	}

	private bool ConditionGainChildSectMember(int actorId, short conditionCalcOrganization)
	{
		return GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
	}

	private bool ConditionDateSectMember(int actorId, short conditionCalcOrganization)
	{
		return GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
	}

	private bool ConditionReFoundChildSectMember(int actorId, short conditionCalcOrganization)
	{
		return GetSectInfoSafe(actorId).OrgTemplateId == conditionCalcOrganization;
	}

	private bool ConditionBanSexualMate(int actorId, ESecretInformationSpecialConditionCalcSexualMateCase conditionCalcSexualMateCase, ESecretInformationSpecialConditionCalcSexualMateRule conditionCalcSexualMateRule, short conditionCalcSectRule)
	{
		short templateId = _templateId;
		if (1 == 0)
		{
		}
		ESecretInformationSpecialConditionCalcSexualMateCase eSecretInformationSpecialConditionCalcSexualMateCase;
		switch (templateId)
		{
		case 19:
		case 20:
		case 21:
		case 22:
		case 23:
		case 107:
		case 108:
			eSecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.GainChild;
			break;
		case 36:
			eSecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.BeHusband;
			break;
		case 109:
			eSecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.Date;
			break;
		case 34:
			eSecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.BeLover;
			break;
		case 105:
			eSecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.SexInvalid;
			break;
		case 106:
			eSecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.SexNotAllow;
			break;
		default:
			eSecretInformationSpecialConditionCalcSexualMateCase = ESecretInformationSpecialConditionCalcSexualMateCase.Invalid;
			break;
		}
		if (1 == 0)
		{
		}
		if (conditionCalcSexualMateCase != eSecretInformationSpecialConditionCalcSexualMateCase)
		{
			return false;
		}
		OrganizationInfo sectInfoSafe = GetSectInfoSafe(actorId);
		if (1 == 0)
		{
		}
		bool flag = conditionCalcSexualMateRule switch
		{
			ESecretInformationSpecialConditionCalcSexualMateRule.AllMember => true, 
			ESecretInformationSpecialConditionCalcSexualMateRule.NoCommonHuman => ConditionIsMonk(actorId), 
			ESecretInformationSpecialConditionCalcSexualMateRule.GradeHigh => sectInfoSafe.Grade >= 6, 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		if (!flag)
		{
			return false;
		}
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(sectInfoSafe.OrgTemplateId);
		return settlementByOrgTemplateId != null && PunishmentType.Instance[conditionCalcSectRule].GetSeverity(DomainManager.Map.GetStateTemplateIdByAreaId(settlementByOrgTemplateId.GetLocation().AreaId), Config.Organization.Instance[sectInfoSafe.OrgTemplateId].IsSect) >= 0;
	}

	private bool ConditionBanEating(short conditionCalcSectRule)
	{
		int num = (ArgList.CheckIndex(0) ? ArgList[0] : (-1));
		int itemType = (ArgList.CheckIndex(4) ? ArgList[4] : (-1));
		if (!ConditionForbidItem(num, itemType))
		{
			return false;
		}
		OrganizationInfo sectInfoSafe = GetSectInfoSafe(num);
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(sectInfoSafe.OrgTemplateId);
		return settlementByOrgTemplateId != null && PunishmentType.Instance[conditionCalcSectRule].GetSeverity(DomainManager.Map.GetStateTemplateIdByAreaId(settlementByOrgTemplateId.GetLocation().AreaId), Config.Organization.Instance[sectInfoSafe.OrgTemplateId].IsSect) >= 0;
	}
}
