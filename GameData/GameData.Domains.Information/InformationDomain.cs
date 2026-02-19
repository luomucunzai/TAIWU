using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Common.Binary;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Relation;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.Information;

[GameDataDomain(18)]
public class InformationDomain : BaseGameDataDomain
{
	public static class NormalInformationUseResultType
	{
		public const sbyte Effective = 0;

		public const sbyte Normal = 1;

		public const sbyte Ineffective = 2;

		public const sbyte SwordTomb = 3;
	}

	public struct SecretInformationFavorChangeItem
	{
		public int CharacterId;

		public int TargetId;

		public int DeltaFavor;

		public sbyte Priority;

		public SecretInformationFavorChangeItem(int characterId, int targetId, int deltaFavor, sbyte priority = 0)
		{
			CharacterId = characterId;
			TargetId = targetId;
			DeltaFavor = deltaFavor;
			Priority = priority;
		}
	}

	public struct SecretInformationHappinessChangeItem
	{
		public int CharacterId;

		public int DeltaHappiness;

		public sbyte Priority;

		public SecretInformationHappinessChangeItem(int characterId, int deltaHappiness, sbyte priority = 0)
		{
			CharacterId = characterId;
			DeltaHappiness = deltaHappiness;
			Priority = priority;
		}
	}

	public struct SecretInformationStartEnemyRelationItem
	{
		public short SecretInformationTemplateId;

		public int CharacterId;

		public int TargetId;

		public byte Odds;

		public SecretInformationStartEnemyRelationItem(short secretInformationTemplateId, int characterId, int targetId, byte odds)
		{
			SecretInformationTemplateId = secretInformationTemplateId;
			CharacterId = characterId;
			TargetId = targetId;
			Odds = odds;
		}
	}

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private readonly Dictionary<int, NormalInformationCollection> _information;

	[DomainData(DomainDataType.Binary, true, false, true, false)]
	private SecretInformationCollection _secretInformationCollection;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, SecretInformationMetaData> _secretInformationMetaData;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private int _nextMetaDataId;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private readonly Dictionary<int, SecretInformationCharacterDataCollection> _characterSecretInformation;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private List<int> _broadcastSecretInformation;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private List<NormalInformation> _taiwuReceivedNormalInformationInMonth;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private List<int> _taiwuReceivedSecretInformationInMonth;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _taiwuReceivedInformation;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<NormalInformation> _taiwuTmpInformation;

	private static readonly RawDataBlock _bufferForRelationSnapshot = new RawDataBlock();

	public static bool _isOfflineUpdate;

	private static readonly HashSet<int> _offlineIndicesCharacterData = new HashSet<int>();

	private static readonly HashSet<int> _offlineIndicesMetaDataOffset = new HashSet<int>();

	private static readonly HashSet<int> _offlineIndicesMetaDataCharacterDisseminationData = new HashSet<int>();

	private static List<SecretInformationStartEnemyRelationItem> _StartEnemyRelationItem = new List<SecretInformationStartEnemyRelationItem>();

	private readonly List<CharacterDisplayDataWithInfo> _characterDisplayDataWithInfoList = new List<CharacterDisplayDataWithInfo>(128);

	private EventArgBox _eventArgBox = new EventArgBox();

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public readonly LocalObjectPool<SecretInformationProcessor> SecretInformationProcessorPool = new LocalObjectPool<SecretInformationProcessor>(2, 16);

	public static readonly int[] FameTypeForDiscoveryRates = new int[7] { -100, -75, -25, 0, 25, 75, 100 };

	private Stopwatch _swSecretInformation;

	private readonly SortedList<int, int> _secretInformationRemovingList = new SortedList<int, int>();

	private int _npcPlanCastCount = 0;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[10][];

	private SingleValueCollectionModificationCollection<int> _modificationsInformation = SingleValueCollectionModificationCollection<int>.Create();

	private BinaryModificationCollection _modificationsSecretInformationCollection = BinaryModificationCollection.Create();

	private static readonly DataInfluence[][] CacheInfluencesSecretInformationMetaData = new DataInfluence[6][];

	private readonly ObjectCollectionDataStates _dataStatesSecretInformationMetaData = new ObjectCollectionDataStates(6, 0);

	public readonly ObjectCollectionHelperData HelperDataSecretInformationMetaData;

	private SingleValueCollectionModificationCollection<int> _modificationsCharacterSecretInformation = SingleValueCollectionModificationCollection<int>.Create();

	private Queue<uint> _pendingLoadingOperationIds;

	private void OnInitializedDomainData()
	{
		_StartEnemyRelationItem.Clear();
		_isOfflineUpdate = false;
		_nextMetaDataId = 1;
	}

	private void InitializeOnInitializeGameDataModule()
	{
	}

	private void InitializeOnEnterNewWorld()
	{
	}

	private void OnLoadedArchiveData()
	{
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		Dictionary<sbyte, int> dictionary = new Dictionary<sbyte, int>();
		Dictionary<short, sbyte> dictionary2 = new Dictionary<short, sbyte>();
		foreach (var (num2, normalInformationCollection2) in _information)
		{
			dictionary.Clear();
			dictionary2.Clear();
			if (!(normalInformationCollection2.GetList() is List<NormalInformation> list))
			{
				continue;
			}
			for (int num3 = list.Count - 1; num3 >= 0; num3--)
			{
				NormalInformation normalInformation = list[num3];
				InformationItem informationItem = Config.Information.Instance[normalInformation.TemplateId];
				if (informationItem.Type == 2)
				{
					InformationInfoItem informationInfoItem = InformationInfo.Instance[informationItem.InfoIds[normalInformation.Level]];
					if (!dictionary.TryGetValue(informationInfoItem.LifeSkillType, out var value))
					{
						dictionary.Add(informationInfoItem.LifeSkillType, value = 0);
					}
					dictionary[informationInfoItem.LifeSkillType] = value + 1;
				}
				if (informationItem.InfoIds[normalInformation.Level] < 0 || dictionary2.ContainsKey(normalInformation.TemplateId))
				{
					if (!dictionary2.ContainsKey(normalInformation.TemplateId))
					{
						sbyte b = 0;
						while (b < informationItem.InfoIds.Length && informationItem.InfoIds[b] < 0)
						{
							b++;
						}
						dictionary2.Add(normalInformation.TemplateId, b);
					}
					list.RemoveAt(num3);
				}
			}
			if (dictionary.Count > 0)
			{
				foreach (sbyte lifeSkillType in dictionary.Keys)
				{
					if (dictionary[lifeSkillType] > 1)
					{
						list.RemoveAll((NormalInformation inf) => InformationInfo.Instance[Config.Information.Instance[inf.TemplateId].InfoIds[inf.Level]].LifeSkillType == lifeSkillType);
					}
				}
				if (DomainManager.Character.TryGetElement_Objects(num2, out var element))
				{
					foreach (GameData.Domains.Character.LifeSkillItem learnedLifeSkill in element.GetLearnedLifeSkills())
					{
						sbyte type = LifeSkill.Instance[learnedLifeSkill.SkillTemplateId].Type;
						if (learnedLifeSkill.IsAllPagesRead() && dictionary.TryGetValue(type, out var value2) && value2 > 1)
						{
							GainLifeSkillInformationToCharacter(context, num2, type);
						}
					}
				}
			}
			if (dictionary.Count > 0 || dictionary2.Count > 0)
			{
				SetElement_Information(num2, normalInformationCollection2, context);
			}
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		NormalInformationCollection normalInformationCollection3 = EnsureCharacterNormalInformationCollection(context, taiwuCharId);
		sbyte[] xiangshuAvatarTasksInOrder = DomainManager.World.GetXiangshuAvatarTasksInOrder();
		for (int num4 = 0; num4 < 9; num4++)
		{
			if (DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(num4).SwordTombStatus != 2)
			{
				continue;
			}
			bool flag = num4 != xiangshuAvatarTasksInOrder[0];
			NormalInformation information = CalcSwordTombInformation(EInformationInfoSwordInformationType.SwordTombReal, flag, num4);
			if (flag)
			{
				if (normalInformationCollection3.GetList().Any(delegate(NormalInformation u)
				{
					InformationItem item2 = Config.Information.Instance.GetItem(u.TemplateId);
					return item2 != null && item2.TransformId == information.TemplateId;
				}))
				{
					continue;
				}
			}
			else
			{
				InformationItem item = Config.Information.Instance.GetItem(information.TemplateId);
				NormalInformation[] array = normalInformationCollection3.GetList().ToArray();
				for (int num5 = 0; num5 < array.Length; num5++)
				{
					NormalInformation information2 = array[num5];
					if (item.TransformId == information2.TemplateId)
					{
						DiscardNormalInformation(context, taiwuCharId, information2);
					}
				}
			}
			CheckAddNormalInformationToCharacter(context, taiwuCharId, information);
		}
		bool flag2 = false;
		try
		{
			_swSecretInformation = Stopwatch.StartNew();
			CheckSecretInformationValidState(context, out var offsetRefMap, out var multiRefOffsets, out var danglingOffsetList, checkTemplateId: true);
			_swSecretInformation.Stop();
			AdaptableLog.TagWarning("CheckSecretInformationValidState", $"cost {_swSecretInformation.ElapsedMilliseconds} ms");
			foreach (int item3 in multiRefOffsets)
			{
				if (!offsetRefMap.TryGetValue(item3, out var value3))
				{
					continue;
				}
				for (int num6 = value3.Count - 1; num6 >= 1; num6--)
				{
					if (danglingOffsetList.Count > 0)
					{
						int num7 = value3[num6];
						int index = 0;
						int num8 = danglingOffsetList[index];
						AdaptableLog.TagWarning("FixAbnormalDomainArchiveData", $"secretinformation metaData[{num7}] redirection to offset[{num8}]");
						_secretInformationMetaData[num7].SetOffset(num8, context);
						danglingOffsetList.RemoveAt(index);
						value3.RemoveAt(num6);
					}
					else
					{
						int num9 = value3[0];
						value3.RemoveAt(0);
						foreach (KeyValuePair<int, SecretInformationCharacterDataCollection> item4 in _characterSecretInformation)
						{
							if (item4.Value.Collection.ContainsKey(num9))
							{
								DiscardSecretInformation(context, item4.Key, num9);
							}
						}
						_broadcastSecretInformation.Remove(num9);
						SetBroadcastSecretInformation(_broadcastSecretInformation, context);
						DomainManager.Extra.RemoveSecretInformationInBroadcastList(context, num9);
						AdaptableLog.TagWarning("FixAbnormalDomainArchiveData", $"secretinformation metaData[{num9}] cleared its all reference");
						RemoveElement_SecretInformationMetaData(num9);
						AdaptableLog.TagWarning("FixAbnormalDomainArchiveData", $"secretinformation metaData[{num9}] has been removed");
					}
				}
			}
			if (danglingOffsetList.Count > 100)
			{
				flag2 = true;
			}
			else if (danglingOffsetList.Count > 0)
			{
				foreach (int item5 in danglingOffsetList)
				{
					int andIncreaseNextMetaDataId = GetAndIncreaseNextMetaDataId(context);
					SecretInformationMetaData instance = new SecretInformationMetaData(andIncreaseNextMetaDataId, item5);
					AddElement_SecretInformationMetaData(andIncreaseNextMetaDataId, instance);
					AdaptableLog.TagWarning("FixAbnormalDomainArchiveData", $"secretinformation metaData[{andIncreaseNextMetaDataId}] has been appended to dangling offset[{item5}]");
				}
			}
		}
		catch (NullReferenceException)
		{
			_swSecretInformation?.Stop();
			flag2 = true;
		}
		if (flag2)
		{
			SecretInformationWipeAll(context);
			AdaptableLog.TagWarning("FixAbnormalDomainArchiveData", PredefinedLog.Instance[(short)0].Info, appendWarningMessage: true);
		}
		RemoveAllNotExistBroadcastSecretInformation(context);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal NormalInformationCollection EnsureCharacterNormalInformationCollection(DataContext context, int characterId)
	{
		if (!TryGetElement_Information(characterId, out var value))
		{
			AddElement_Information(characterId, value = new NormalInformationCollection(), context);
		}
		return value;
	}

	public void RemoveCharacterAllInformation(DataContext context, int characterId)
	{
		RemoveElement_Information(characterId, context);
		RemoveElement_CharacterSecretInformation(characterId, context);
		DomainManager.Extra.ClearCharacterAllSecretInformationUsedCount(context, characterId);
	}

	[DomainMethod]
	public NormalInformationCollection GetCharacterNormalInformation(int characterId)
	{
		if (TryGetElement_Information(characterId, out var value))
		{
			return value;
		}
		return new NormalInformationCollection();
	}

	[DomainMethod]
	public void AddNormalInformationToCharacter(DataContext context, int characterId, NormalInformation information)
	{
		CheckAddNormalInformationToCharacter(context, characterId, information);
	}

	public bool CheckAddNormalInformationToCharacter(DataContext context, int characterId, NormalInformation information)
	{
		NormalInformationCollection normalInformationCollection = EnsureCharacterNormalInformationCollection(context, characterId);
		IList<NormalInformation> list = normalInformationCollection.GetList();
		foreach (NormalInformation item in list)
		{
			if (item.TemplateId == information.TemplateId && item.Level == information.Level)
			{
				return false;
			}
		}
		list.Add(information);
		if (characterId == DomainManager.Taiwu.GetTaiwuCharId())
		{
			_taiwuReceivedNormalInformationInMonth.Add(information);
			DomainManager.Taiwu.AddLegacyPoint(context, 34);
			InformationItem informationItem = Config.Information.Instance[information.TemplateId];
			switch (informationItem.Type)
			{
			case 0:
			case 1:
			{
				ProfessionFormulaItem formulaCfg2 = ProfessionFormula.Instance[77];
				int baseDelta2 = formulaCfg2.Calculate(information.Level);
				DomainManager.Extra.ChangeProfessionSeniority(context, 12, baseDelta2);
				break;
			}
			case 3:
			{
				ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[78];
				int baseDelta = formulaCfg.Calculate(information.Level);
				DomainManager.Extra.ChangeProfessionSeniority(context, 12, baseDelta);
				break;
			}
			}
		}
		SetElement_Information(characterId, normalInformationCollection, context);
		return true;
	}

	[DomainMethod]
	public void DeleteTmpInformation(DataContext context)
	{
		_taiwuReceivedInformation.Clear();
		_taiwuTmpInformation.Clear();
	}

	public void DiscardNormalInformation(DataContext context, int characterId, NormalInformation information)
	{
		if (TryGetElement_Information(characterId, out var value))
		{
			value.GetList().Remove(information);
			value.ClearUsedCountData(information);
			SetElement_Information(characterId, value, context);
		}
	}

	[DomainMethod]
	public int GetNormalInformationUsedCount(int characterId, NormalInformation information)
	{
		if (TryGetElement_Information(characterId, out var value))
		{
			return value.GetUsedCount(information);
		}
		return 0;
	}

	[DomainMethod]
	public IntPair GetNormalInformationUsedCountAndMax(int characterId, NormalInformation information)
	{
		if (TryGetElement_Information(characterId, out var value))
		{
			return new IntPair(value.GetUsedCount(information), value.GetUsedCountMax(information));
		}
		return new IntPair(0, 0);
	}

	public void TransformNormalInformation(DataContext context, int charId, NormalInformation normalInformation)
	{
		short transformId = Config.Information.Instance[normalInformation.TemplateId].TransformId;
		NormalInformationCollection normalInformationCollection = EnsureCharacterNormalInformationCollection(context, charId);
		sbyte usedCount = normalInformationCollection.GetUsedCount(normalInformation);
		DiscardNormalInformation(context, charId, normalInformation);
		if (transformId < 0)
		{
			return;
		}
		NormalInformation normalInformation2 = new NormalInformation(transformId, normalInformation.Level);
		AddNormalInformationToCharacter(context, charId, normalInformation2);
		normalInformationCollection.SetUsedCount(normalInformation2, usedCount);
		SetElement_Information(charId, normalInformationCollection, context);
		List<int> list = _information.Keys.ToList();
		foreach (int item in list)
		{
			if (TryGetElement_Information(item, out var value) && value.ReceivedCounts.Remove(normalInformation.TemplateId, out var value2))
			{
				value.ReceivedCounts.TryAdd(transformId, value2);
				SetElement_Information(item, value, context);
			}
		}
	}

	public NormalInformation CalcSwordTombInformation(EInformationInfoSwordInformationType type, bool isConsume, int xiangshuAvatarId)
	{
		return type switch
		{
			EInformationInfoSwordInformationType.SwordTombFake => new NormalInformation((short)(xiangshuAvatarId + 80), 4), 
			EInformationInfoSwordInformationType.SwordTombReal => new NormalInformation((short)(xiangshuAvatarId + (isConsume ? 89 : 98)), 8), 
			EInformationInfoSwordInformationType.SwordTombIntell => new NormalInformation((short)(xiangshuAvatarId + 111), 8), 
			_ => throw new Exception($"unsupported information type: {type}"), 
		};
	}

	public void RegisterInformation(int charId, DataContext context)
	{
		CharacterDomain character = DomainManager.Character;
		GameData.Domains.Character.Character element_Objects = character.GetElement_Objects(charId);
		foreach (GameData.Domains.Character.LifeSkillItem learnedLifeSkill in element_Objects.GetLearnedLifeSkills())
		{
			if (learnedLifeSkill.IsAllPagesRead())
			{
				GainLifeSkillInformationToCharacter(context, charId, LifeSkill.Instance[learnedLifeSkill.SkillTemplateId].Type);
			}
		}
		Location location = element_Objects.GetLocation();
		if (location.IsValid())
		{
			MapBlockData block = DomainManager.Map.GetBlock(location);
			short informationTemplateId = block.GetConfig().InformationTemplateId;
			if (informationTemplateId >= 0)
			{
				OrganizationInfo organizationInfo = element_Objects.GetOrganizationInfo();
				if (organizationInfo.Grade > 0)
				{
					AddNormalInformationToCharacter(context, charId, new NormalInformation(informationTemplateId, organizationInfo.Grade));
				}
			}
		}
		MapDomain map = DomainManager.Map;
		OrganizationDomain organization = DomainManager.Organization;
		OrganizationInfo organizationInfo2 = element_Objects.GetOrganizationInfo();
		if (organizationInfo2.SettlementId >= 0)
		{
			Location location2 = organization.GetSettlement(organizationInfo2.SettlementId).GetLocation();
			if (location2.IsValid())
			{
				short informationTemplateId2 = map.GetBlock(location2).GetConfig().InformationTemplateId;
				if (informationTemplateId2 >= 0)
				{
					AddNormalInformationToCharacter(context, charId, new NormalInformation(informationTemplateId2, organizationInfo2.Grade));
				}
			}
		}
		AddElement_CharacterSecretInformation(charId, new SecretInformationCharacterDataCollection(), context);
	}

	public void TransferInformation(int sourceCharId, int targetCharId, DataContext context)
	{
		RemoveCharacterAllInformation(context, targetCharId);
		if (TryGetElement_Information(sourceCharId, out var value))
		{
			AddElement_Information(targetCharId, value, context);
			RemoveElement_Information(sourceCharId, context);
		}
		if (TryGetElement_CharacterSecretInformation(sourceCharId, out var value2))
		{
			AddElement_CharacterSecretInformation(targetCharId, value2, context);
			RemoveElement_CharacterSecretInformation(sourceCharId, context);
		}
		DomainManager.Extra.TransferCharacterAllSecretInformationUsedCount(context, sourceCharId, targetCharId);
	}

	public void ClearedTaiwuReceivedSecretInformationInMonth()
	{
		_taiwuReceivedSecretInformationInMonth.Clear();
	}

	public void ProcessAdvanceMonth(DataContext context)
	{
		_isOfflineUpdate = true;
		if (DomainManager.World.GetCurrMonthInYear() == 0)
		{
			DomainManager.Extra.ClearPackReceivedNormalInformationInLastMonth(context);
		}
		DomainManager.Extra.PackReceivedNormalInformationInLastMonth(context, _taiwuReceivedNormalInformationInMonth);
		_taiwuReceivedNormalInformationInMonth.Clear();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		_swSecretInformation = Stopwatch.StartNew();
		MakeSettlementsInformation(context);
		_swSecretInformation.Stop();
		Logger.Info($"{"MakeSettlementsInformation"}: {_swSecretInformation.ElapsedMilliseconds} ms");
		_swSecretInformation = Stopwatch.StartNew();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list.AddRange(_characterSecretInformation.Keys);
		list.Remove(taiwuCharId);
		foreach (int item in list)
		{
			PlanDisseminateSecretInformation(context, item);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		_swSecretInformation.Stop();
		Logger.Info($"{"PlanDisseminateSecretInformation"}: {_swSecretInformation.ElapsedMilliseconds} ms");
		foreach (int secretInformationShopCharacterKey in DomainManager.Extra.GetSecretInformationShopCharacterKeys())
		{
			SecretInformationShopCharacterData secretInformationShopCharacterData = DomainManager.Extra.AddOrGetSecretInformationShopCharacterData(context, secretInformationShopCharacterKey);
			secretInformationShopCharacterData.CollectedSecretInformationIds.Clear();
			DomainManager.Extra.SetSecretInformationShopCharacterData(context, secretInformationShopCharacterKey, secretInformationShopCharacterData);
		}
		RemoveSenselessSecretInformation(context);
		SetTaiwuReceivedNormalInformationInMonth(_taiwuReceivedNormalInformationInMonth, context);
		SetTaiwuReceivedSecretInformationInMonth(_taiwuReceivedSecretInformationInMonth, context);
		foreach (int offlineIndicesCharacterDatum in _offlineIndicesCharacterData)
		{
			if (TryGetElement_CharacterSecretInformation(offlineIndicesCharacterDatum, out var value))
			{
				SetElement_CharacterSecretInformation(offlineIndicesCharacterDatum, value, context);
			}
			else
			{
				RemoveElement_CharacterSecretInformation(offlineIndicesCharacterDatum, context);
			}
		}
		_offlineIndicesCharacterData.Clear();
		foreach (int item2 in _offlineIndicesMetaDataOffset)
		{
			if (TryGetElement_SecretInformationMetaData(item2, out var element))
			{
				element.SetOffset(element.GetOffset(), context);
			}
			else
			{
				RemoveElement_SecretInformationMetaData(item2);
			}
		}
		_offlineIndicesMetaDataOffset.Clear();
		foreach (int offlineIndicesMetaDataCharacterDisseminationDatum in _offlineIndicesMetaDataCharacterDisseminationData)
		{
			if (TryGetElement_SecretInformationMetaData(offlineIndicesMetaDataCharacterDisseminationDatum, out var element2))
			{
				element2.SetDisseminationData(element2.GetDisseminationData(), context);
			}
			else
			{
				RemoveElement_SecretInformationMetaData(offlineIndicesMetaDataCharacterDisseminationDatum);
			}
		}
		_offlineIndicesMetaDataCharacterDisseminationData.Clear();
		SetBroadcastSecretInformation(GetBroadcastSecretInformation(), context);
		foreach (SecretInformationStartEnemyRelationItem item3 in _StartEnemyRelationItem)
		{
			if (DomainManager.Character.TryGetElement_Objects(item3.CharacterId, out var element3) && DomainManager.Character.TryGetElement_Objects(item3.TargetId, out var element4) && Config.Character.Instance[element3.GetTemplateId()].CreatingType == 1 && Config.Character.Instance[element4.GetTemplateId()].CreatingType == 1)
			{
				GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, element3, element4, item3.CharacterId == DomainManager.Taiwu.GetTaiwuCharId(), 5, new CharacterBecomeEnemyInfo(element3)
				{
					SecretInformationTemplateId = item3.SecretInformationTemplateId,
					Location = element4.GetValidLocation()
				});
			}
		}
		_StartEnemyRelationItem.Clear();
		_isOfflineUpdate = false;
	}

	public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (_information.TryGetValue(taiwuCharId, out var value))
		{
			crossArchiveGameData.NormalInformation = value;
		}
	}

	public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		bool flag = false;
		try
		{
			CheckSecretInformationValidState(context, out var _, out var multiRefOffsets, out var danglingOffsetList, checkTemplateId: true);
			if (multiRefOffsets.Count > 0 || danglingOffsetList.Count > 0)
			{
				flag = true;
			}
		}
		catch (NullReferenceException)
		{
			flag = true;
		}
		if (flag)
		{
			SecretInformationWipeAll(context);
			AdaptableLog.TagWarning("UnpackCrossArchiveGameData", PredefinedLog.Instance[(short)0].Info, appendWarningMessage: true);
		}
	}

	public void UnpackCrossArchiveGameData_NormalInformation(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		if (crossArchiveGameData.NormalInformation == null)
		{
			return;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		foreach (NormalInformation item2 in crossArchiveGameData.NormalInformation.GetList())
		{
			InformationItem informationItem = Config.Information.Instance[item2.TemplateId];
			if (informationItem.Type == 2)
			{
				if (!informationItem.InfoIds.CheckIndex(item2.Level))
				{
					continue;
				}
				InformationInfoItem item = InformationInfo.Instance.GetItem(informationItem.InfoIds[item2.Level]);
				if (item != null)
				{
					for (int i = 0; i < item2.Level + 1; i++)
					{
						GainLifeSkillInformationToCharacter(context, taiwuCharId, item.LifeSkillType);
					}
				}
			}
			else if (informationItem.Type != 5)
			{
				AddNormalInformationToCharacter(context, taiwuCharId, item2);
			}
		}
	}

	public void MakeSettlementsInformation(DataContext context)
	{
		MapDomain map = DomainManager.Map;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location taiwuLocation = taiwu.GetLocation();
		HashSet<int> taiwuGroups = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		for (short num = 0; num < 45; num++)
		{
			short mainSettlementMainBlockId = map.GetMainSettlementMainBlockId(num);
			if (mainSettlementMainBlockId >= 0)
			{
				MapBlockData block = map.GetBlock(num, mainSettlementMainBlockId);
				ProcessMapBlock(block);
			}
		}
		void ProcessCharacter(int characterId, MapBlockItem blockConfig)
		{
			if (blockConfig.InformationTemplateId >= 0)
			{
				NormalInformationCollection normalInformationCollection = EnsureCharacterNormalInformationCollection(context, characterId);
				IList<NormalInformation> list = normalInformationCollection.GetList();
				InformationItem informationItem = Config.Information.Instance[blockConfig.InformationTemplateId];
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					NormalInformation normalInformation = list[i];
					if (normalInformation.TemplateId == informationItem.TemplateId)
					{
						if (normalInformation.Level < 8 && context.Random.Next(0, 100) < informationItem.ExtraGainRate[normalInformation.Level])
						{
							AddNormalInformationToCharacter(context, characterId, new NormalInformation(normalInformation.TemplateId, normalInformation.Level));
						}
						break;
					}
				}
				int j = 0;
				for (int num2 = informationItem.BaseGainRate.Length; j < num2; j++)
				{
					int num3 = informationItem.BaseGainRate[j] * 2;
					if (context.Random.Next(0, 100) < num3)
					{
						AddNormalInformationToCharacter(context, characterId, new NormalInformation(informationItem.TemplateId, (sbyte)j));
						break;
					}
				}
			}
		}
		void ProcessCharacterSet(HashSet<int> characterSet, MapBlockItem blockConfig)
		{
			if (characterSet == null || blockConfig.InformationTemplateId < 0)
			{
				return;
			}
			foreach (int item in characterSet)
			{
				ProcessCharacter(item, blockConfig);
			}
		}
		void ProcessMapBlock(MapBlockData mapBlockData)
		{
			MapBlockItem config = mapBlockData.GetConfig();
			if (config.InformationTemplateId >= 0)
			{
				ProcessCharacterSet(mapBlockData.CharacterSet, config);
				if (mapBlockData.BlockId == taiwuLocation.BlockId && mapBlockData.AreaId == taiwuLocation.AreaId)
				{
					ProcessCharacterSet(taiwuGroups, config);
				}
				if (mapBlockData.GroupBlockList != null)
				{
					foreach (MapBlockData groupBlock in mapBlockData.GroupBlockList)
					{
						ProcessCharacterSet(groupBlock.CharacterSet, config);
						if (groupBlock.BlockId == taiwuLocation.BlockId && groupBlock.AreaId == taiwuLocation.AreaId)
						{
							ProcessCharacterSet(taiwuGroups, config);
						}
					}
				}
			}
		}
	}

	internal void GiveOrUpgradeWesternRegionInformation(DataContext context, int charId, short westernRegionId)
	{
		NormalInformationCollection normalInformationCollection = EnsureCharacterNormalInformationCollection(context, charId);
		IList<NormalInformation> list = normalInformationCollection.GetList();
		for (int i = 0; i < list.Count; i++)
		{
			NormalInformation normalInformation = list[i];
			InformationItem item = Config.Information.Instance.GetItem(normalInformation.TemplateId);
			InformationInfoItem item2 = InformationInfo.Instance.GetItem(item.InfoIds[normalInformation.Level]);
			sbyte b = (sbyte)(normalInformation.Level + 1);
			if (item2.WesternRegionId == westernRegionId && b <= 8 && item.InfoIds.CheckIndex(b) && item.InfoIds[b] >= 0)
			{
				AddNormalInformationToCharacter(context, charId, new NormalInformation(item.TemplateId, b));
				return;
			}
		}
		foreach (InformationItem item4 in (IEnumerable<InformationItem>)Config.Information.Instance)
		{
			for (sbyte b2 = 0; b2 <= 8; b2++)
			{
				InformationInfoItem item3 = InformationInfo.Instance.GetItem(item4.InfoIds[b2]);
				if (item3 != null && item3.WesternRegionId == westernRegionId)
				{
					AddNormalInformationToCharacter(context, charId, new NormalInformation(item4.TemplateId, b2));
					return;
				}
			}
		}
	}

	internal void GiveProfessionInformation(DataContext context, int charId, int professionId)
	{
		if (CalcProfessionInformation(professionId, out var information) && !CheckAddNormalInformationToCharacter(context, charId, information))
		{
			NormalInformationCollection normalInformationCollection = EnsureCharacterNormalInformationCollection(context, charId);
			normalInformationCollection.SetRemainUsableCount(information, (sbyte)(normalInformationCollection.GetRemainUsableCount(information) + GlobalConfig.Instance.NormalInformationDefaultCostableMaxUseCount));
			DomainManager.Information.SetNormalInformationCharacterDataModified(context, charId);
		}
	}

	internal void FixOldProfessionInformation(DataContext context, ProfessionData professionData, int charId)
	{
		ProfessionItem config = professionData.GetConfig();
		for (int i = 0; i <= config.ProfessionSkills.Length; i++)
		{
			if (professionData.IsSkillUnlocked(i))
			{
				GiveProfessionInformation(context, charId, professionData.TemplateId);
			}
		}
	}

	internal void GiveRemainUsedCountInformation(DataContext context, int charId, NormalInformation normalInformation)
	{
		InformationItem informationItem = Config.Information.Instance[normalInformation.TemplateId];
		Tester.Assert(!informationItem.UsedCountWithMax);
		NormalInformationCollection normalInformationCollection = EnsureCharacterNormalInformationCollection(context, charId);
		if (CheckAddNormalInformationToCharacter(context, charId, normalInformation))
		{
			normalInformationCollection.SetRemainUsableCount(normalInformation, 1);
		}
		else
		{
			normalInformationCollection.SetRemainUsableCount(normalInformation, (sbyte)(normalInformationCollection.GetRemainUsableCount(normalInformation) + 1));
		}
		DomainManager.Information.SetNormalInformationCharacterDataModified(context, charId);
	}

	internal void GiveProfessionInformationRemainUsableCount(DataContext context, int charId, int professionId, int oldExtraSeniority, int nowExtraSeniority)
	{
		ProfessionItem item = Profession.Instance.GetItem(professionId);
		for (int i = 0; i < item.ProfessionSkills.Length + 1; i++)
		{
			int num = (item.ProfessionSkills.CheckIndex(i) ? item.ProfessionSkills[i] : item.ExtraProfessionSkill);
			ProfessionSkillItem item2 = ProfessionSkill.Instance.GetItem(num);
			if (item2 != null)
			{
				int num2 = GlobalConfig.Instance.GiveProfessionInformationFactorWithExtraSeniority * GameData.Domains.Taiwu.Profession.SharedMethods.GetSkillUnlockSeniority(num) / 100;
				bool flag = oldExtraSeniority < num2 && nowExtraSeniority >= num2;
				bool flag2 = num2 == 1500000 && nowExtraSeniority < oldExtraSeniority;
				if ((flag || flag2) && DomainManager.Information.CalcProfessionInformation(professionId, out var information))
				{
					GiveRemainUsedCountInformation(context, charId, information);
				}
			}
		}
	}

	internal bool CalcProfessionInformation(int professionId, out NormalInformation information)
	{
		foreach (InformationItem item2 in (IEnumerable<InformationItem>)Config.Information.Instance)
		{
			for (sbyte b = 0; b <= 8; b++)
			{
				InformationInfoItem item = InformationInfo.Instance.GetItem(item2.InfoIds[b]);
				if (item != null && item.Profession == professionId)
				{
					information = new NormalInformation(item2.TemplateId, b);
					return true;
				}
			}
		}
		information = default(NormalInformation);
		return false;
	}

	public bool GainRandomSettlementInformationByStateIdToCharacter(DataContext context, sbyte grade, int characterId, sbyte stateId)
	{
		List<short> list = new List<short>();
		DomainManager.Map.GetAllAreaInState(stateId, list);
		List<short> list2 = new List<short>();
		foreach (short item in list)
		{
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(item);
			SettlementInfo[] settlementInfos = element_Areas.SettlementInfos;
			for (int i = 0; i < settlementInfos.Length; i++)
			{
				SettlementInfo settlementInfo = settlementInfos[i];
				list2.AddRange(from informationItem in Config.Information.Instance
					where informationItem.IsGeneral && informationItem.InfoIds.Any((short infoId) => InformationInfo.Instance[infoId]?.Oraganization == settlementInfo.OrgTemplateId && settlementInfo.OrgTemplateId >= 0)
					select informationItem.TemplateId);
			}
		}
		if (list2.Count <= 0)
		{
			return false;
		}
		return CheckAddNormalInformationToCharacter(context, characterId, new NormalInformation(list2.GetRandom(context.Random), grade));
	}

	public void GainLifeSkillInformationToCharacter(DataContext context, int characterId, sbyte lifeSkillType)
	{
		NormalInformationCollection normalInformationCollection = EnsureCharacterNormalInformationCollection(context, characterId);
		IList<NormalInformation> list = normalInformationCollection.GetList();
		short informationTemplateId = Config.LifeSkillType.Instance[lifeSkillType].InformationTemplateId;
		if (informationTemplateId < 0)
		{
			return;
		}
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			NormalInformation value = list[i];
			if (value.TemplateId == informationTemplateId)
			{
				value.UpdateLevel((sbyte)(value.Level + 1));
				list[i] = value;
				SetElement_Information(characterId, normalInformationCollection, context);
				return;
			}
		}
		AddNormalInformationToCharacter(context, characterId, new NormalInformation(informationTemplateId, 0));
	}

	[DomainMethod]
	public int GmCmd_CreateSecretInformationByCharacterIds(DataContext context, string templateDefKeyName, List<int> charIds)
	{
		SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
		Type type = secretInformationCollection.GetType();
		MethodInfo method = type.GetMethod("Add" + templateDefKeyName);
		if (method != null)
		{
			short index = (short)typeof(SecretInformation.DefKey).GetField(templateDefKeyName, BindingFlags.Static | BindingFlags.Public).GetValue(null);
			SecretInformationItem secretInformationItem = SecretInformation.Instance[index];
			if (secretInformationItem.Parameters != null)
			{
				object[] array = new object[secretInformationItem.Parameters.Count((string p) => !string.IsNullOrEmpty(p))];
				int num = 0;
				for (int num2 = array.Length; num < num2; num++)
				{
					switch (secretInformationItem.Parameters[num])
					{
					case "Character":
						if (charIds.Count > 0)
						{
							array[num] = charIds[0];
							charIds.RemoveAt(0);
							break;
						}
						return -1;
					case "Location":
					{
						List<Settlement> list = new List<Settlement>();
						DomainManager.Organization.GetAllCivilianSettlements(list);
						array[num] = list.GetRandom(context.Random).GetLocation();
						break;
					}
					case "ItemKey":
						array[num] = (ulong)new ItemKey(10, 0, (short)context.Random.Next(Config.SkillBook.Instance.Count), -1);
						break;
					case "Resource":
						array[num] = (sbyte)context.Random.Next(8);
						break;
					case "LifeSkill":
						array[num] = (short)context.Random.Next(LifeSkill.Instance.Count);
						break;
					case "CombatSkill":
						array[num] = (short)context.Random.Next(Config.CombatSkill.Instance.Count);
						break;
					case "Integer":
						array[num] = context.Random.Next();
						break;
					}
				}
				int dataOffset = (int)method.Invoke(secretInformationCollection, array);
				return AddSecretInformationMetaData(context, dataOffset, withInitialDistribute: false);
			}
		}
		return -1;
	}

	[DomainMethod]
	public bool GmCmd_MakeCharacterReceiveSecretInformation(DataContext context, int characterId, int metaDataId)
	{
		if (TryGetElement_SecretInformationMetaData(metaDataId, out var _))
		{
			return ReceiveSecretInformation(context, metaDataId, characterId);
		}
		return false;
	}

	[DomainMethod]
	public void GmCmd_MakeSecretInformationBroadcast(DataContext context, int metaDataId, int sourceCharId = -1)
	{
		MakeSecretInformationBroadcastEffect(context, metaDataId, sourceCharId);
	}

	[DomainMethod]
	public int GmCmd_DisseminationSecretInformationToRandomCharacters(DataContext context, int secretId, int sourceCharId, int amount)
	{
		CharacterDomain character = DomainManager.Character;
		InformationDomain information = DomainManager.Information;
		List<GameData.Domains.Character.Character> list = new List<GameData.Domains.Character.Character>();
		int num = amount;
		character.FindIntelligentCharacters((GameData.Domains.Character.Character _) => true, list);
		CollectionUtils.Shuffle(context.Random, list);
		foreach (GameData.Domains.Character.Character item in list)
		{
			if (amount <= 0)
			{
				break;
			}
			if (information.ReceiveSecretInformation(context, secretId, item.GetId(), sourceCharId))
			{
				amount--;
			}
		}
		return num - amount;
	}

	[DomainMethod]
	public List<CharacterDisplayDataWithInfo> GetCharacterDisplayDataWithInfoList(List<int> charList)
	{
		_characterDisplayDataWithInfoList.Clear();
		foreach (int @char in charList)
		{
			if (DomainManager.Character.TryGetElement_Objects(@char, out var _))
			{
				CharacterInfoCountData characterInfoCountData = GetCharacterInfoCountData(@char);
				if (characterInfoCountData != null)
				{
					CharacterDisplayDataWithInfo item = new CharacterDisplayDataWithInfo
					{
						CharacterDisplayData = DomainManager.Character.GetCharacterDisplayData(@char),
						CharacterInfoCountData = characterInfoCountData
					};
					_characterDisplayDataWithInfoList.Add(item);
				}
			}
		}
		return _characterDisplayDataWithInfoList;
	}

	private CharacterInfoCountData GetCharacterInfoCountData(int characterId)
	{
		CharacterInfoCountData result = null;
		if (TryGetElement_CharacterSecretInformation(characterId, out var value))
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			int count = value.Collection.Count;
			int num = value.Collection.Count((KeyValuePair<int, SecretInformationCharacterData> p) => CheckSecretIsRelated(p.Key, taiwuCharId));
			if (count > 0 || num > 0)
			{
				result = new CharacterInfoCountData
				{
					HoldInfoCount = count,
					HoldInfoTaiwuRelatedCount = num
				};
			}
		}
		return result;
	}

	private bool CheckSecretIsRelated(int secretId, int charId)
	{
		if (TryGetElement_SecretInformationMetaData(secretId, out var element))
		{
			_eventArgBox.Clear();
			DomainManager.Information.MakeSecretInformationEventArgBox(element, _eventArgBox);
			int num = _eventArgBox.GetInt("arg0");
			int num2 = _eventArgBox.GetInt("arg1");
			if (num == charId || num2 == charId)
			{
				return true;
			}
		}
		return false;
	}

	[DomainMethod]
	public void PerformProfessionLiteratiSkill2(DataContext ctx, int secretInformationId)
	{
		ProfessionSkillHandle.LiteratiSkill_BroadcastModifiedSecretInformation(ctx, secretInformationId);
	}

	[DomainMethod]
	public void PerformProfessionLiteratiSkill3(DataContext ctx, NormalInformation normalInformation)
	{
		ProfessionSkillHandle.LiteratiSkill_AreaBroadcastNormalInformation(ctx, normalInformation);
	}

	public unsafe bool TryGetSecretInformationCharacter(int secretId, int index, out int characterId)
	{
		characterId = -1;
		if (TryGetElement_SecretInformationMetaData(secretId, out var element))
		{
			SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
			int offset = element.GetOffset();
			fixed (byte* rawData = secretInformationCollection.GetRawData())
			{
				byte* ptr = rawData + offset;
				ptr++;
				int num = *(int*)ptr;
				ptr += 4;
				short index2 = *(short*)ptr;
				ptr += 2;
				SecretInformationItem secretInformationItem = SecretInformation.Instance[index2];
				if (secretInformationItem.Parameters != null)
				{
					int i = 0;
					for (int num2 = secretInformationItem.Parameters.Length; i < num2; i++)
					{
						string text = secretInformationItem.Parameters[i];
						if (string.IsNullOrEmpty(text))
						{
							continue;
						}
						switch (text)
						{
						case "Character":
						{
							int num3 = *(int*)ptr;
							ptr += 4;
							if (i == index)
							{
								characterId = num3;
								return true;
							}
							break;
						}
						case "Location":
							ptr += 2;
							ptr += 2;
							break;
						case "ItemKey":
							ptr += 8;
							break;
						case "Resource":
							ptr++;
							break;
						case "LifeSkill":
							ptr += 2;
							break;
						case "CombatSkill":
							ptr += 2;
							break;
						case "Integer":
							ptr += 4;
							break;
						default:
							throw new NotImplementedException(text);
						}
					}
				}
			}
		}
		return false;
	}

	public void SetSecretInformationCollectionModified(DataContext context)
	{
	}

	public IEnumerable<int> GetBroadcastSecretInformationIds()
	{
		return GetBroadcastSecretInformation().Concat(DomainManager.Extra.GetSecretInformationInBroadcastList());
	}

	public bool IsSecretInformationInBroadcast(int metaDataId)
	{
		if (GetBroadcastSecretInformation().Contains(metaDataId))
		{
			return true;
		}
		return DomainManager.Extra.IsSecretInformationInBroadcastList(metaDataId);
	}

	public short CalcSecretInformationTemplateId(SecretInformationMetaData secretInformationMetaData)
	{
		CalcSecretInformationRemainingLifeTime(secretInformationMetaData, out var templateId);
		return templateId;
	}

	public short CalcSecretInformationTemplateId(int metaDataId)
	{
		if (TryGetElement_SecretInformationMetaData(metaDataId, out var element))
		{
			return CalcSecretInformationTemplateId(element);
		}
		return -1;
	}

	public SecretInformationItem GetSecretInformationConfig(int metaDataId)
	{
		SecretInformationMetaData element_SecretInformationMetaData = GetElement_SecretInformationMetaData(metaDataId);
		short index = CalcSecretInformationTemplateId(element_SecretInformationMetaData);
		return SecretInformation.Instance[index];
	}

	public bool CharacterHasSecretInformationByTemplateId(int charId, short templateId)
	{
		if (DomainManager.Information.TryGetElement_CharacterSecretInformation(charId, out var value))
		{
			foreach (int key in value.Collection.Keys)
			{
				if (MatchMetaDataIdAndTemplateId(key, templateId))
				{
					return true;
				}
			}
		}
		IEnumerable<int> secretInformationInBroadcastList = DomainManager.Extra.GetSecretInformationInBroadcastList();
		foreach (int item in secretInformationInBroadcastList)
		{
			if (MatchMetaDataIdAndTemplateId(item, templateId))
			{
				return true;
			}
		}
		return false;
	}

	private bool MatchMetaDataIdAndTemplateId(int metaDataId, short templateId)
	{
		SecretInformationMetaData element;
		short templateId2;
		return DomainManager.Information.TryGetElement_SecretInformationMetaData(metaDataId, out element) && DomainManager.Information.CalcSecretInformationRemainingLifeTime(element, out templateId2) > 0 && templateId2 == templateId;
	}

	public void GetSecretInformationOfCharacter(ICollection<int> result, int charId, bool includeBroadcast = true)
	{
		if (DomainManager.Information.TryGetElement_CharacterSecretInformation(charId, out var value))
		{
			foreach (int key in value.Collection.Keys)
			{
				if (DomainManager.Information.TryGetElement_SecretInformationMetaData(key, out var element) && DomainManager.Information.CalcSecretInformationRemainingLifeTime(element, out var _) > 0)
				{
					result.Add(key);
				}
			}
		}
		if (!includeBroadcast)
		{
			return;
		}
		IEnumerable<int> secretInformationInBroadcastList = DomainManager.Extra.GetSecretInformationInBroadcastList();
		foreach (int item in secretInformationInBroadcastList)
		{
			result.Add(item);
		}
	}

	public List<int> GetSecretInformationOfCharacter(int charId, bool includeBroadcast = true)
	{
		List<int> result = new List<int>();
		GetSecretInformationOfCharacter(result, charId, includeBroadcast);
		return result;
	}

	public sbyte CalcSecretInformationDisplaySize(SecretInformationMetaData metaData, IReadOnlyList<sbyte> informationSettings)
	{
		int num = 0;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		SecretInformationItem secretInformationItem = SecretInformation.Instance[CalcSecretInformationTemplateId(metaData)];
		sbyte b = secretInformationItem.BlockSizeArgs[0];
		sbyte b2 = secretInformationItem.BlockSizeArgs[1 + informationSettings[1]];
		num += b * b2;
		int characterIndex = 0;
		if (secretInformationItem.Parameters != null)
		{
			EventArgBox eventArgBox = MakeSecretInformationEventArgBox(metaData, new EventArgBox());
			for (int i = 0; i < secretInformationItem.Parameters.Length; i++)
			{
				if (secretInformationItem.Parameters[i].Equals("Character"))
				{
					int num2 = eventArgBox.GetInt($"arg{i}");
					bool isSect = false;
					sbyte b3 = 0;
					if (DomainManager.Character.TryGetElement_Objects(num2, out var element))
					{
						sbyte orgTemplateId = element.GetOrganizationInfo().OrgTemplateId;
						isSect = orgTemplateId >= 0 && Config.Organization.Instance[orgTemplateId].IsSect;
						b3 = element.GetOrganizationInfo().Grade;
					}
					else
					{
						DeadCharacter deadCharacter = DomainManager.Character.TryGetDeadCharacter(num2);
						if (deadCharacter != null)
						{
							sbyte orgTemplateId2 = deadCharacter.OrganizationInfo.OrgTemplateId;
							isSect = orgTemplateId2 >= 0 && Config.Organization.Instance[orgTemplateId2].IsSect;
							b3 = deadCharacter.OrganizationInfo.Grade;
						}
					}
					short[] array = CharRelationTypeToValue(isSect);
					sbyte b4 = RelationType.GetTypeId(0);
					RelatedCharacter relation;
					if (num2 == taiwuCharId)
					{
						b4 = (sbyte)(array.Length - 1);
					}
					else if (DomainManager.Character.TryGetRelation(taiwuCharId, num2, out relation) && relation.RelationType != ushort.MaxValue)
					{
						for (ushort num3 = relation.RelationType; num3 < 17; num3++)
						{
							ushort num4 = (ushort)(1 << (int)num3);
							if ((num4 & relation.RelationType) != 0)
							{
								b4 = RelationType.GetTypeId(num4);
								break;
							}
						}
					}
					num += CharGradeToValue(isSect)[b3] * secretInformationItem.BlockSizeArgs[4 + informationSettings[2]];
					num += array[b4] * secretInformationItem.BlockSizeArgs[7 + informationSettings[3]];
					characterIndex++;
				}
				else if (secretInformationItem.Parameters[i].Equals("ItemKey"))
				{
					eventArgBox.Get<ItemKey>($"arg{i}", out ItemKey arg);
					if (arg.ItemType >= 0 && arg.TemplateId >= 0)
					{
						num += GlobalConfig.SecretInformationDisplay_ItemGradeToValue[ItemTemplateHelper.GetGrade(arg.ItemType, arg.TemplateId)] * secretInformationItem.BlockSizeArgs[10 + informationSettings[4]];
					}
				}
			}
		}
		for (int j = 0; j < GlobalConfig.SecretInformationDisplay_SizeThresholds.Length; j++)
		{
			if (num < GlobalConfig.SecretInformationDisplay_SizeThresholds[j])
			{
				return (sbyte)j;
			}
		}
		return (sbyte)GlobalConfig.SecretInformationDisplay_SizeThresholds.Length;
		short[] CharGradeToValue(bool flag)
		{
			return characterIndex switch
			{
				0 => flag ? GlobalConfig.SecretInformationDisplay_PosASectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosANotSectCharGradeToValue, 
				1 => flag ? GlobalConfig.SecretInformationDisplay_PosBSectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosBNotSectCharGradeToValue, 
				2 => flag ? GlobalConfig.SecretInformationDisplay_PosCSectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosCNotSectCharGradeToValue, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		short[] CharRelationTypeToValue(bool flag)
		{
			return characterIndex switch
			{
				0 => flag ? GlobalConfig.SecretInformationDisplay_PosASectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosANotSectCharRelationTypeToValue, 
				1 => flag ? GlobalConfig.SecretInformationDisplay_PosBSectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosBNotSectCharRelationTypeToValue, 
				2 => flag ? GlobalConfig.SecretInformationDisplay_PosCSectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosCNotSectCharRelationTypeToValue, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
	}

	public int CalcSecretInformationShopValue(SecretInformationMetaData metaData)
	{
		SecretInformationItem secretInformationItem = SecretInformation.Instance[CalcSecretInformationTemplateId(metaData)];
		int num = 0;
		EventArgBox eventArgBox = MakeSecretInformationEventArgBox(metaData, new EventArgBox());
		for (int i = 0; i < secretInformationItem.Parameters.Length; i++)
		{
			if (secretInformationItem.Parameters[i].Equals("Character"))
			{
				int num2 = eventArgBox.GetInt($"arg{i}");
				int num3 = 0;
				DeadCharacter character;
				if (DomainManager.Character.TryGetElement_Objects(num2, out var element))
				{
					num3 = element.GetOrganizationInfo().Grade + 1;
				}
				else if (DomainManager.Character.TryGetDeadCharacter(num2, out character))
				{
					num3 = character.OrganizationInfo.Grade + 1;
				}
				num += num3;
				if (i == 1 || i == 2)
				{
					num += num3;
				}
			}
		}
		return secretInformationItem.SortValue * num * CalcSecretInformationDisplaySize(metaData, new sbyte[5] { 0, 1, 1, 1, 1 }) * 100;
	}

	public unsafe int CalcSecretInformationRemainingLifeTime(SecretInformationMetaData secretInformationMetaData, out short templateId)
	{
		SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
		int offset = secretInformationMetaData.GetOffset();
		int result;
		fixed (byte* rawData = secretInformationCollection.GetRawData())
		{
			byte* ptr = rawData + offset;
			ptr++;
			int num = *(int*)ptr;
			ptr += 4;
			templateId = *(short*)ptr;
			ptr += 2;
			short duration = SecretInformation.Instance[templateId].Duration;
			result = ((duration < 0) ? 1 : (duration - (DomainManager.World.GetCurrDate() - num)));
		}
		return result;
	}

	public unsafe EventArgBox MakeSecretInformationEventArgBox(SecretInformationMetaData secretInformationMetaData, EventArgBox target)
	{
		SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
		int offset = secretInformationMetaData.GetOffset();
		fixed (byte* rawData = secretInformationCollection.GetRawData())
		{
			byte* ptr = rawData + offset;
			ptr++;
			int num = *(int*)ptr;
			ptr += 4;
			short num2 = *(short*)ptr;
			ptr += 2;
			SecretInformationItem secretInformationItem = SecretInformation.Instance[num2];
			if (secretInformationItem.Parameters != null)
			{
				int i = 0;
				for (int num3 = secretInformationItem.Parameters.Length; i < num3; i++)
				{
					string text = secretInformationItem.Parameters[i];
					if (!string.IsNullOrEmpty(text))
					{
						string key = $"arg{i}";
						switch (text)
						{
						case "Character":
						{
							int arg4 = *(int*)ptr;
							ptr += 4;
							target.Set(key, arg4);
							break;
						}
						case "Location":
						{
							short areaId = *(short*)ptr;
							ptr += 2;
							short blockId = *(short*)ptr;
							ptr += 2;
							target.Set(key, (ISerializableGameData)(object)new Location(areaId, blockId));
							break;
						}
						case "ItemKey":
						{
							ulong num4 = *(ulong*)ptr;
							ItemKey itemKey = (ItemKey)num4;
							ptr += 8;
							target.Set(key, (ISerializableGameData)(object)new ItemKey(itemKey.ItemType, 0, itemKey.TemplateId, -1));
							break;
						}
						case "Resource":
						{
							sbyte arg3 = (sbyte)(*ptr);
							ptr++;
							target.Set(key, arg3);
							break;
						}
						case "LifeSkill":
						{
							short arg2 = *(short*)ptr;
							ptr += 2;
							target.Set(key, arg2);
							break;
						}
						case "CombatSkill":
						{
							short arg = *(short*)ptr;
							ptr += 2;
							target.Set(key, arg);
							break;
						}
						case "Integer":
							target.Set(key, *(int*)ptr);
							ptr += 4;
							break;
						default:
							throw new NotImplementedException(text);
						}
					}
				}
			}
			target.Set("templateId", num2);
			target.Set("metaDataId", secretInformationMetaData.GetId());
		}
		return target;
	}

	public unsafe bool SecretInformationIsRelateWithCharacter(int secretInformationMetaDataId, int characterId)
	{
		if (TryGetElement_SecretInformationMetaData(secretInformationMetaDataId, out var element))
		{
			bool result = false;
			SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
			int offset = element.GetOffset();
			fixed (byte* rawData = secretInformationCollection.GetRawData())
			{
				byte* ptr = rawData + offset;
				ptr++;
				int num = *(int*)ptr;
				ptr += 4;
				short index = *(short*)ptr;
				ptr += 2;
				SecretInformationItem secretInformationItem = SecretInformation.Instance[index];
				if (secretInformationItem.Parameters != null)
				{
					int i = 0;
					for (int num2 = secretInformationItem.Parameters.Length; i < num2; i++)
					{
						string text = secretInformationItem.Parameters[i];
						if (string.IsNullOrEmpty(text))
						{
							continue;
						}
						switch (text)
						{
						case "Character":
						{
							int num3 = *(int*)ptr;
							ptr += 4;
							if (num3 == characterId)
							{
								result = true;
							}
							break;
						}
						case "Location":
							ptr += 2;
							ptr += 2;
							break;
						case "ItemKey":
							ptr += 8;
							break;
						case "Resource":
							ptr++;
							break;
						case "LifeSkill":
							ptr += 2;
							break;
						case "CombatSkill":
							ptr += 2;
							break;
						case "Integer":
							ptr += 4;
							break;
						default:
							throw new NotImplementedException(text);
						}
					}
				}
			}
			return result;
		}
		return false;
	}

	public unsafe sbyte CalcSecretInformationDisplaySize(SecretInformationMetaData metaData)
	{
		SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
		int offset = metaData.GetOffset();
		int num = 0;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		fixed (byte* rawData = secretInformationCollection.GetRawData())
		{
			byte* ptr = rawData + offset;
			ptr++;
			int num2 = *(int*)ptr;
			ptr += 4;
			SecretInformationItem secretInformationItem = SecretInformation.Instance[*(short*)ptr];
			ptr += 2;
			sbyte b = secretInformationItem.BlockSizeArgs[0];
			sbyte b2 = secretInformationItem.BlockSizeArgs[1];
			num += b * b2;
			int characterIndex = 0;
			if (secretInformationItem.Parameters != null)
			{
				int i = 0;
				for (int num3 = secretInformationItem.Parameters.Length; i < num3; i++)
				{
					string text = secretInformationItem.Parameters[i];
					if (string.IsNullOrEmpty(text))
					{
						continue;
					}
					switch (text)
					{
					case "Character":
					{
						int num5 = *(int*)ptr;
						ptr += 4;
						bool isSect = false;
						sbyte b3 = 0;
						if (DomainManager.Character.TryGetElement_Objects(num5, out var element))
						{
							sbyte orgTemplateId = element.GetOrganizationInfo().OrgTemplateId;
							isSect = orgTemplateId >= 0 && Config.Organization.Instance[orgTemplateId].IsSect;
							b3 = element.GetOrganizationInfo().Grade;
						}
						else
						{
							DeadCharacter deadCharacter = DomainManager.Character.TryGetDeadCharacter(num5);
							if (deadCharacter != null)
							{
								sbyte orgTemplateId2 = deadCharacter.OrganizationInfo.OrgTemplateId;
								isSect = orgTemplateId2 >= 0 && Config.Organization.Instance[orgTemplateId2].IsSect;
								b3 = deadCharacter.OrganizationInfo.Grade;
							}
						}
						short[] array = CharRelationTypeToValue(isSect);
						sbyte b4 = RelationType.GetTypeId(0);
						RelatedCharacter relation;
						if (num5 == taiwuCharId)
						{
							b4 = (sbyte)(array.Length - 1);
						}
						else if (DomainManager.Character.TryGetRelation(taiwuCharId, num5, out relation) && relation.RelationType != ushort.MaxValue)
						{
							for (ushort num6 = relation.RelationType; num6 < 17; num6++)
							{
								ushort num7 = (ushort)(1 << (int)num6);
								if ((num7 & relation.RelationType) != 0)
								{
									b4 = RelationType.GetTypeId(num7);
									break;
								}
							}
						}
						num += CharGradeToValue(isSect)[b3] * secretInformationItem.BlockSizeArgs[2];
						num += array[b4] * secretInformationItem.BlockSizeArgs[3];
						characterIndex++;
						break;
					}
					case "Location":
						ptr += 2;
						ptr += 2;
						break;
					case "ItemKey":
					{
						ulong num4 = *(ulong*)ptr;
						ItemKey itemKey = (ItemKey)num4;
						ptr += 8;
						if (itemKey.ItemType >= 0 && itemKey.TemplateId >= 0)
						{
							num += GlobalConfig.SecretInformationDisplay_ItemGradeToValue[ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId)] * secretInformationItem.BlockSizeArgs[4];
						}
						break;
					}
					case "Resource":
						ptr++;
						break;
					case "LifeSkill":
						ptr += 2;
						break;
					case "CombatSkill":
						ptr += 2;
						break;
					case "Integer":
						ptr += 4;
						break;
					default:
						throw new NotImplementedException(text);
					}
				}
			}
			short[] CharGradeToValue(bool flag)
			{
				return characterIndex switch
				{
					0 => flag ? GlobalConfig.SecretInformationDisplay_PosASectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosANotSectCharGradeToValue, 
					1 => flag ? GlobalConfig.SecretInformationDisplay_PosBSectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosBNotSectCharGradeToValue, 
					2 => flag ? GlobalConfig.SecretInformationDisplay_PosCSectCharGradeToValue : GlobalConfig.SecretInformationDisplay_PosCNotSectCharGradeToValue, 
					_ => throw new IndexOutOfRangeException(), 
				};
			}
			short[] CharRelationTypeToValue(bool flag)
			{
				return characterIndex switch
				{
					0 => flag ? GlobalConfig.SecretInformationDisplay_PosASectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosANotSectCharRelationTypeToValue, 
					1 => flag ? GlobalConfig.SecretInformationDisplay_PosBSectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosBNotSectCharRelationTypeToValue, 
					2 => flag ? GlobalConfig.SecretInformationDisplay_PosCSectCharRelationTypeToValue : GlobalConfig.SecretInformationDisplay_PosCNotSectCharRelationTypeToValue, 
					_ => throw new IndexOutOfRangeException(), 
				};
			}
		}
		for (int j = 0; j < GlobalConfig.SecretInformationDisplay_SizeThresholds.Length; j++)
		{
			if (num < GlobalConfig.SecretInformationDisplay_SizeThresholds[j])
			{
				return (sbyte)j;
			}
		}
		return (sbyte)GlobalConfig.SecretInformationDisplay_SizeThresholds.Length;
	}

	public unsafe SecretInformationDisplayData GetSecretInformationDisplayData(int secretInformationMetaDataId, ISet<int> characterSet)
	{
		if (TryGetElement_SecretInformationMetaData(secretInformationMetaDataId, out var element))
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			SecretInformationDisplayData secretInformationDisplayData = new SecretInformationDisplayData();
			SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
			int offset = element.GetOffset();
			secretInformationDisplayData.SourceCharacterId = -1;
			fixed (byte* rawData = secretInformationCollection.GetRawData())
			{
				HashSet<SecretInformationRelationshipType> hashSet = ObjectPool<HashSet<SecretInformationRelationshipType>>.Instance.Get();
				byte* ptr = rawData + offset;
				ptr++;
				int num = *(int*)ptr;
				ptr += 4;
				secretInformationDisplayData.SecretInformationTemplateId = *(short*)ptr;
				ptr += 2;
				SecretInformationItem secretInformationItem = SecretInformation.Instance[secretInformationDisplayData.SecretInformationTemplateId];
				if (secretInformationItem.Parameters != null)
				{
					int i = 0;
					for (int num2 = secretInformationItem.Parameters.Length; i < num2; i++)
					{
						string text = secretInformationItem.Parameters[i];
						if (string.IsNullOrEmpty(text))
						{
							continue;
						}
						switch (text)
						{
						case "Character":
						{
							int num3 = *(int*)ptr;
							ptr += 4;
							characterSet.Add(num3);
							if (num3 == taiwuCharId)
							{
								flag = true;
							}
							hashSet.Clear();
							CheckSecretInformationRelationship(taiwuCharId, -1, num3, -1, hashSet);
							if (hashSet.Contains(SecretInformationRelationshipType.Friend) || hashSet.Contains(SecretInformationRelationshipType.Relative))
							{
								flag2 = true;
							}
							else if (hashSet.Contains(SecretInformationRelationshipType.Enemy))
							{
								flag3 = true;
							}
							break;
						}
						case "Location":
							ptr += 2;
							ptr += 2;
							break;
						case "ItemKey":
							ptr += 8;
							break;
						case "Resource":
							ptr++;
							break;
						case "LifeSkill":
							ptr += 2;
							break;
						case "CombatSkill":
							ptr += 2;
							break;
						case "Integer":
							ptr += 4;
							break;
						default:
							throw new NotImplementedException(text);
						}
					}
				}
				ObjectPool<HashSet<SecretInformationRelationshipType>>.Instance.Return(hashSet);
			}
			secretInformationDisplayData.HolderCount = GetSecretInformationHolderCount(secretInformationMetaDataId);
			secretInformationDisplayData.UsedCount = 0;
			secretInformationDisplayData.AuthorityCostWhenDisseminating = 0;
			secretInformationDisplayData.Location = DomainManager.Map.GetBlockFullName(DomainManager.Extra.GetSecretInformationOccurredLocation(secretInformationMetaDataId));
			secretInformationDisplayData.SecretInformationMetaDataId = secretInformationMetaDataId;
			SecretInformationCollection secretInformationCollection2 = new SecretInformationCollection();
			int recordSize = secretInformationCollection.GetRecordSize(offset);
			secretInformationCollection2.EnsureCapacity(recordSize);
			secretInformationCollection2.Count = 1;
			Array.Copy(secretInformationCollection.RawData, offset, secretInformationCollection2.RawData, 0, recordSize);
			secretInformationDisplayData.RawData = secretInformationCollection2;
			if (flag)
			{
				secretInformationDisplayData.FilterMask |= 1;
			}
			else
			{
				secretInformationDisplayData.FilterMask |= 2;
			}
			if (flag2)
			{
				secretInformationDisplayData.FilterMask |= 4;
			}
			if (flag3)
			{
				secretInformationDisplayData.FilterMask |= 8;
			}
			secretInformationDisplayData.DisplaySize = CalcSecretInformationDisplaySize(element);
			secretInformationDisplayData.ShopValue = CalcSecretInformationShopValue(element);
			return secretInformationDisplayData;
		}
		return null;
	}

	public int GetSecretInformationHolderCount(int secretInformationMetaDataId)
	{
		return _characterSecretInformation.Values.Count((SecretInformationCharacterDataCollection v) => v.Collection.ContainsKey(secretInformationMetaDataId));
	}

	[DomainMethod]
	public SecretInformationDisplayPackage GetSecretInformationDisplayPackage(List<int> secretInformationMetaDataIds)
	{
		SecretInformationDisplayPackage secretInformationDisplayPackage = new SecretInformationDisplayPackage();
		HashSet<int> characterSet = new HashSet<int>();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = taiwu.GetId();
		SecretInformationCharacterDataCollection value;
		bool flag = TryGetElement_CharacterSecretInformation(id, out value);
		if (secretInformationMetaDataIds == null)
		{
			secretInformationMetaDataIds = new List<int>();
		}
		foreach (SecretInformationDisplayData item in secretInformationMetaDataIds.Select((int metaDataId) => GetSecretInformationDisplayData(metaDataId, characterSet)))
		{
			if (item != null)
			{
				item.UsedCount = DomainManager.Extra.GetCharacterSecretInformationUsedCount(id, item.SecretInformationMetaDataId);
				item.AuthorityCostWhenDisseminating = CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(item.SecretInformationTemplateId, taiwu.GetFameType(), GetSecretInformationDisseminatingCountOfBranch(item.SecretInformationMetaDataId, (!flag || !value.Collection.TryGetValue(item.SecretInformationMetaDataId, out var value2)) ? 1 : value2.SecretInformationDisseminationBranch));
				secretInformationDisplayPackage.SecretInformationDisplayDataList.Add(item);
			}
		}
		foreach (int item2 in characterSet)
		{
			secretInformationDisplayPackage.CharacterData.Add(item2, DomainManager.Character.GetCharacterDisplayData(item2));
		}
		return secretInformationDisplayPackage;
	}

	[DomainMethod]
	public SecretInformationDisplayPackage GetSecretInformationDisplayPackageFromCharacter(int characterId)
	{
		SecretInformationDisplayPackage secretInformationDisplayPackage = new SecretInformationDisplayPackage();
		HashSet<int> hashSet = new HashSet<int>();
		if (TryGetElement_CharacterSecretInformation(characterId, out var value) && DomainManager.Character.TryGetElement_Objects(characterId, out var element))
		{
			foreach (KeyValuePair<int, SecretInformationCharacterData> item in value.Collection)
			{
				SecretInformationDisplayData secretInformationDisplayData = GetSecretInformationDisplayData(item.Key, hashSet);
				secretInformationDisplayData.SourceCharacterId = item.Value.SecretInformationDisseminationBranch;
				secretInformationDisplayData.UsedCount = DomainManager.Extra.GetCharacterSecretInformationUsedCount(characterId, item.Key);
				secretInformationDisplayData.AuthorityCostWhenDisseminating = CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(secretInformationDisplayData.SecretInformationTemplateId, element.GetFameType(), GetSecretInformationDisseminatingCountOfBranch(secretInformationDisplayData.SecretInformationMetaDataId, item.Value.SecretInformationDisseminationBranch));
				secretInformationDisplayPackage.SecretInformationDisplayDataList.Add(secretInformationDisplayData);
				if (secretInformationDisplayData.SourceCharacterId > 0)
				{
					hashSet.Add(secretInformationDisplayData.SourceCharacterId);
				}
			}
		}
		foreach (int item2 in hashSet)
		{
			secretInformationDisplayPackage.CharacterData.Add(item2, DomainManager.Character.GetCharacterDisplayData(item2));
		}
		return secretInformationDisplayPackage;
	}

	[DomainMethod]
	public SecretInformationDisplayPackage GetSecretInformationDisplayPackageFromBroadcast(int characterId)
	{
		SecretInformationDisplayPackage secretInformationDisplayPackage = new SecretInformationDisplayPackage();
		HashSet<int> hashSet = new HashSet<int>();
		foreach (int broadcastSecretInformationId in GetBroadcastSecretInformationIds())
		{
			SecretInformationDisplayData secretInformationDisplayData = GetSecretInformationDisplayData(broadcastSecretInformationId, hashSet);
			secretInformationDisplayData.UsedCount = DomainManager.Extra.GetCharacterSecretInformationUsedCount(characterId, broadcastSecretInformationId);
			secretInformationDisplayPackage.SecretInformationDisplayDataList.Add(secretInformationDisplayData);
		}
		foreach (int item in hashSet)
		{
			secretInformationDisplayPackage.CharacterData.Add(item, DomainManager.Character.GetCharacterDisplayData(item));
		}
		return secretInformationDisplayPackage;
	}

	[DomainMethod]
	public SecretInformationDisplayPackage GetSecretInformationDisplayPackageForSelections(int characterId)
	{
		SecretInformationDisplayPackage secretInformationDisplayPackage = new SecretInformationDisplayPackage();
		HashSet<int> hashSet = new HashSet<int>();
		if (TryGetElement_CharacterSecretInformation(characterId, out var value) && DomainManager.Character.TryGetElement_Objects(characterId, out var element))
		{
			foreach (KeyValuePair<int, SecretInformationCharacterData> item in value.Collection)
			{
				SecretInformationDisplayData secretInformationDisplayData = GetSecretInformationDisplayData(item.Key, hashSet);
				secretInformationDisplayData.SourceCharacterId = item.Value.SecretInformationDisseminationBranch;
				secretInformationDisplayData.UsedCount = DomainManager.Extra.GetCharacterSecretInformationUsedCount(characterId, item.Key);
				secretInformationDisplayData.AuthorityCostWhenDisseminating = CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(secretInformationDisplayData.SecretInformationTemplateId, element.GetFameType(), GetSecretInformationDisseminatingCountOfBranch(secretInformationDisplayData.SecretInformationMetaDataId, item.Value.SecretInformationDisseminationBranch));
				secretInformationDisplayPackage.SecretInformationDisplayDataList.Add(secretInformationDisplayData);
				if (secretInformationDisplayData.SourceCharacterId > 0)
				{
					hashSet.Add(secretInformationDisplayData.SourceCharacterId);
				}
			}
		}
		foreach (int broadcastSecretInformationId in GetBroadcastSecretInformationIds())
		{
			SecretInformationDisplayData secretInformationDisplayData2 = GetSecretInformationDisplayData(broadcastSecretInformationId, hashSet);
			secretInformationDisplayData2.UsedCount = DomainManager.Extra.GetCharacterSecretInformationUsedCount(characterId, broadcastSecretInformationId);
			secretInformationDisplayPackage.SecretInformationDisplayDataList.Add(secretInformationDisplayData2);
		}
		foreach (int item2 in hashSet)
		{
			secretInformationDisplayPackage.CharacterData.Add(item2, DomainManager.Character.GetCharacterDisplayData(item2));
		}
		return secretInformationDisplayPackage;
	}

	[DomainMethod]
	public bool DisseminateSecretInformation(DataContext context, int metaDataId, int sourceCharId, int targetCharId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(sourceCharId);
		int val = 0;
		if (DomainManager.Information.TryGetElement_CharacterSecretInformation(sourceCharId, out var value) && value.Collection.TryGetValue(metaDataId, out var value2))
		{
			short secretInformationTemplateId = DomainManager.Information.CalcSecretInformationTemplateId(metaDataId);
			val = DomainManager.Information.CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(secretInformationTemplateId, element_Objects.GetFameType(), GetSecretInformationDisseminatingCountOfBranch(metaDataId, value2.SecretInformationDisseminationBranch));
		}
		CostSecretInformationUsedCount(context, sourceCharId, metaDataId);
		if (DistributeSecretInformationToCharacter(context, metaDataId, targetCharId, sourceCharId))
		{
			element_Objects.ChangeResource(context, 7, -Math.Min(element_Objects.GetResource(7), val));
			return true;
		}
		return false;
	}

	public SecretInformationDisplayPackage GetRelateBSecretInformationDisplayPackageForSelections(int charA, int charB)
	{
		SecretInformationDisplayPackage secretInformationDisplayPackage = new SecretInformationDisplayPackage();
		HashSet<int> hashSet = new HashSet<int>();
		if (TryGetElement_CharacterSecretInformation(charA, out var value) && DomainManager.Character.TryGetElement_Objects(charA, out var element))
		{
			foreach (KeyValuePair<int, SecretInformationCharacterData> item in value.Collection)
			{
				if (SecretInformationIsRelateWithCharacter(item.Key, charB))
				{
					SecretInformationDisplayData secretInformationDisplayData = GetSecretInformationDisplayData(item.Key, hashSet);
					secretInformationDisplayData.SourceCharacterId = item.Value.SecretInformationDisseminationBranch;
					secretInformationDisplayData.UsedCount = DomainManager.Extra.GetCharacterSecretInformationUsedCount(charA, item.Key);
					secretInformationDisplayData.AuthorityCostWhenDisseminating = CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(secretInformationDisplayData.SecretInformationTemplateId, element.GetFameType(), GetSecretInformationDisseminatingCountOfBranch(secretInformationDisplayData.SecretInformationMetaDataId, item.Value.SecretInformationDisseminationBranch));
					secretInformationDisplayPackage.SecretInformationDisplayDataList.Add(secretInformationDisplayData);
					if (secretInformationDisplayData.SourceCharacterId > 0)
					{
						hashSet.Add(secretInformationDisplayData.SourceCharacterId);
					}
				}
			}
		}
		foreach (int broadcastSecretInformationId in GetBroadcastSecretInformationIds())
		{
			if (SecretInformationIsRelateWithCharacter(broadcastSecretInformationId, charB))
			{
				SecretInformationDisplayData secretInformationDisplayData2 = GetSecretInformationDisplayData(broadcastSecretInformationId, hashSet);
				secretInformationDisplayData2.UsedCount = DomainManager.Extra.GetCharacterSecretInformationUsedCount(charA, broadcastSecretInformationId);
				secretInformationDisplayPackage.SecretInformationDisplayDataList.Add(secretInformationDisplayData2);
			}
		}
		foreach (int item2 in hashSet)
		{
			secretInformationDisplayPackage.CharacterData.Add(item2, DomainManager.Character.GetCharacterDisplayData(item2));
		}
		return secretInformationDisplayPackage;
	}

	public void PrepareSecretInformationAdvanceMonth()
	{
	}

	public void SetNormalInformationCharacterDataModified(DataContext context, int characterId)
	{
		if (TryGetElement_Information(characterId, out var value))
		{
			SetElement_Information(characterId, value, context);
		}
	}

	public void SetSecretInformationCharacterDataModified(DataContext context, int characterId)
	{
		if (TryGetElement_CharacterSecretInformation(characterId, out var value))
		{
			SetElement_CharacterSecretInformation(characterId, value, context);
		}
	}

	public int CalcSecretInformationAuthorityCostWhenDisseminating(short secretInformationTemplateId, int disseminationCountOfBranch)
	{
		SecretInformationItem secretInformationItem = SecretInformation.Instance[secretInformationTemplateId];
		return secretInformationItem.CostAuthority * Math.Max(0, 100 - Math.Max(disseminationCountOfBranch - 1, 0) / 10) / 100;
	}

	public int CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(short secretInformationTemplateId, sbyte characterFameType, int disseminationCountOfBranch)
	{
		if (characterFameType == -2)
		{
			characterFameType = 3;
		}
		int num = CalcSecretInformationAuthorityCostWhenDisseminating(secretInformationTemplateId, disseminationCountOfBranch);
		SecretInformationItem secretInformationItem = SecretInformation.Instance[secretInformationTemplateId];
		int num2 = secretInformationItem.FameThreshold - characterFameType;
		if (num2 > 0)
		{
			return num * (1 + num2 * 2);
		}
		return num;
	}

	public int GetSecretInformationDisseminatingCountOfBranch(int metaDataId, int disseminationBranch)
	{
		int result = 0;
		if (TryGetElement_SecretInformationMetaData(metaDataId, out var element))
		{
			result = element.GetCharacterDisseminationCount(disseminationBranch);
		}
		return result;
	}

	private int GetAndIncreaseNextMetaDataId(DataContext context)
	{
		int nextMetaDataId = _nextMetaDataId;
		_nextMetaDataId++;
		if ((uint)_nextMetaDataId > 2147483647u)
		{
			_nextMetaDataId = 1;
		}
		SetNextMetaDataId(_nextMetaDataId, context);
		return nextMetaDataId;
	}

	public int AddSecretInformationMetaData(DataContext context, int dataOffset, bool withInitialDistribute = true)
	{
		return AddSecretInformationMetaDataWithNecessity(context, dataOffset, withInitialDistribute, necessarily: false, null);
	}

	public unsafe int AddSecretInformationMetaDataWithNecessity(DataContext context, int dataOffset, bool withInitialDistribute, bool necessarily, Action<ICollection<int>> distributeCallback)
	{
		int andIncreaseNextMetaDataId = GetAndIncreaseNextMetaDataId(context);
		bool flag = false;
		bool flag2 = false;
		SecretInformationMetaData secretInformationMetaData = new SecretInformationMetaData(andIncreaseNextMetaDataId, dataOffset);
		CalcSecretInformationRemainingLifeTime(secretInformationMetaData, out var templateId);
		SecretInformationItem secretInformationItem = SecretInformation.Instance[templateId];
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		CommitInsert_SecretInformationCollection(context, dataOffset, _secretInformationCollection.GetRecordSize(dataOffset));
		CommitSetMetadata_SecretInformationCollection(context);
		int discoveryRate = secretInformationItem.DiscoveryRate;
		sbyte arg = -1;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		ObjectPool<HashSet<int>> instance = ObjectPool<HashSet<int>>.Instance;
		EventArgBox eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
		HashSet<int> hashSet = instance.Get();
		DomainManager.Information.MakeSecretInformationEventArgBox(secretInformationMetaData, eventArgBox);
		if (secretInformationItem.Parameters != null)
		{
			for (int i = 0; i < secretInformationItem.Parameters.Length; i++)
			{
				int arg2 = -1;
				if (secretInformationItem.Parameters[i] == "ItemKey")
				{
					if (eventArgBox.Get<ItemKey>($"arg{i}", out ItemKey arg3) && arg3.IsValid())
					{
						num += ItemTemplateHelper.GetGrade(arg3.ItemType, arg3.TemplateId);
						num2++;
					}
				}
				else if (secretInformationItem.Parameters[i] == "CombatSkill")
				{
					short arg4 = -1;
					if (eventArgBox.Get($"arg{i}", ref arg4))
					{
						num += Config.CombatSkill.Instance[arg4].Grade;
						num2++;
					}
				}
				else if (secretInformationItem.Parameters[i] == "LifeSkill")
				{
					short arg5 = -1;
					if (eventArgBox.Get($"arg{i}", ref arg5))
					{
						num += LifeSkill.Instance[arg5].Grade;
						num2++;
					}
				}
				else if (secretInformationItem.Parameters[i] == "Resource")
				{
					eventArgBox.Get($"arg{i}", ref arg);
				}
				else if (secretInformationItem.Parameters[i] == "Integer")
				{
					int arg6 = -1;
					if (arg >= 0 && eventArgBox.Get($"arg{i}", ref arg6))
					{
						sbyte b = ResourceTypeHelper.ResourceAmountToGrade(arg, arg6);
						if (b >= 0)
						{
							num += b;
							num2++;
						}
					}
				}
				else
				{
					if (secretInformationItem.Parameters[i] != "Character" || !eventArgBox.Get($"arg{i}", ref arg2))
					{
						continue;
					}
					int num5 = -1;
					DeadCharacter deadCharacter = DomainManager.Character.TryGetDeadCharacter(arg2);
					if (DomainManager.Character.TryGetElement_Objects(arg2, out var element))
					{
						num5 = element.GetFameType();
						num += element.GetOrganizationInfo().Grade;
					}
					else if (deadCharacter != null)
					{
						num5 = deadCharacter.FameType;
						num += deadCharacter.OrganizationInfo.Grade;
					}
					num4++;
					num2++;
					if (num5 == -2)
					{
						num3 = num3;
					}
					else if (FameTypeForDiscoveryRates.CheckIndex(num5))
					{
						num3 += Math.Abs(FameTypeForDiscoveryRates[num5]);
					}
					RelatedCharacter relation;
					if (arg2 == taiwuCharId)
					{
						flag = true;
					}
					else if (DomainManager.Character.TryGetRelation(taiwuCharId, arg2, out relation))
					{
						flag2 = true;
					}
					if (DomainManager.Character.TryGetElement_Objects(arg2, out var element2) && i == 0)
					{
						DomainManager.Extra.SetSecretInformationOccurredLocation(context, andIncreaseNextMetaDataId, element2.GetLocation());
					}
					if (secretInformationItem.RelationshipSnapshotParameterIndices != null && secretInformationItem.RelationshipSnapshotParameterIndices.Contains(i))
					{
						RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(arg2);
						if (relatedCharacters != null)
						{
							_bufferForRelationSnapshot.EnsureCapacity(relatedCharacters.GetSerializedSize());
							fixed (byte* rawData = _bufferForRelationSnapshot.RawData)
							{
								relatedCharacters.Serialize(rawData);
								relatedCharacters = new RelatedCharacters();
								relatedCharacters.Deserialize(rawData);
							}
							relatedCharacters.General.Clear();
							HashSet<int> collection = DomainManager.Character.GetReversedRelatedCharIds(arg2, 16384).GetCollection();
							if (collection != null)
							{
								foreach (int item in collection)
								{
									if (secretInformationMetaData.CharacterRelationshipSnapshotCollection.Collection.ContainsKey(item))
									{
										continue;
									}
									RelatedCharacters relatedCharacters2 = DomainManager.Character.GetRelatedCharacters(item);
									_bufferForRelationSnapshot.EnsureCapacity(relatedCharacters2.GetSerializedSize());
									fixed (byte* rawData2 = _bufferForRelationSnapshot.RawData)
									{
										relatedCharacters2.Serialize(rawData2);
										relatedCharacters2 = new RelatedCharacters();
										relatedCharacters2.Deserialize(rawData2);
									}
									relatedCharacters2.Friends.Clear();
									relatedCharacters2.General.Clear();
									relatedCharacters2.Mentees.Clear();
									relatedCharacters2.Mentors.Clear();
									relatedCharacters2.AdoptiveChildren.Clear();
									relatedCharacters2.AdoptiveParents.Clear();
									relatedCharacters2.BloodChildren.Clear();
									relatedCharacters2.BloodParents.Clear();
									relatedCharacters2.StepChildren.Clear();
									relatedCharacters2.StepParents.Clear();
									relatedCharacters2.HusbandsAndWives.Clear();
									relatedCharacters2.AdoptiveBrothersAndSisters.Clear();
									relatedCharacters2.BloodBrothersAndSisters.Clear();
									relatedCharacters2.StepBrothersAndSisters.Clear();
									relatedCharacters2.SwornBrothersAndSisters.Clear();
									secretInformationMetaData.CharacterRelationshipSnapshotCollection.Collection[item] = new SecretInformationCharacterRelationshipSnapshot
									{
										RelatedCharacters = relatedCharacters2
									};
								}
							}
							HashSet<int> collection2 = DomainManager.Character.GetReversedRelatedCharIds(arg2, 32768).GetCollection();
							if (collection2 != null)
							{
								foreach (int item2 in collection2)
								{
									if (secretInformationMetaData.CharacterRelationshipSnapshotCollection.Collection.ContainsKey(item2))
									{
										continue;
									}
									RelatedCharacters relatedCharacters3 = DomainManager.Character.GetRelatedCharacters(item2);
									if (relatedCharacters3 == null || _bufferForRelationSnapshot == null)
									{
									}
									_bufferForRelationSnapshot.EnsureCapacity(relatedCharacters3.GetSerializedSize());
									fixed (byte* rawData3 = _bufferForRelationSnapshot.RawData)
									{
										relatedCharacters3.Serialize(rawData3);
										relatedCharacters3 = new RelatedCharacters();
										relatedCharacters3.Deserialize(rawData3);
									}
									relatedCharacters3.Friends.Clear();
									relatedCharacters3.General.Clear();
									relatedCharacters3.Mentees.Clear();
									relatedCharacters3.Mentors.Clear();
									relatedCharacters3.AdoptiveChildren.Clear();
									relatedCharacters3.AdoptiveParents.Clear();
									relatedCharacters3.BloodChildren.Clear();
									relatedCharacters3.BloodParents.Clear();
									relatedCharacters3.StepChildren.Clear();
									relatedCharacters3.StepParents.Clear();
									relatedCharacters3.HusbandsAndWives.Clear();
									relatedCharacters3.AdoptiveBrothersAndSisters.Clear();
									relatedCharacters3.BloodBrothersAndSisters.Clear();
									relatedCharacters3.StepBrothersAndSisters.Clear();
									relatedCharacters3.SwornBrothersAndSisters.Clear();
									secretInformationMetaData.CharacterRelationshipSnapshotCollection.Collection[item2] = new SecretInformationCharacterRelationshipSnapshot
									{
										RelatedCharacters = relatedCharacters3
									};
								}
							}
							hashSet.Clear();
							relatedCharacters.GetAllRelatedCharIds(hashSet, secretInformationItem.IsGeneralRelationCharactersNeedSnapshot);
							foreach (int item3 in hashSet)
							{
								secretInformationMetaData.CharacterExtraInfoCollection.Collection.TryGetValue(item3, out var value);
								value.AliveState = ((!DomainManager.Character.TryGetElement_Objects(arg2, out var _)) ? ((sbyte)1) : ((sbyte)0));
								secretInformationMetaData.CharacterExtraInfoCollection.Collection[item3] = value;
							}
							secretInformationMetaData.CharacterRelationshipSnapshotCollection.Collection[arg2] = new SecretInformationCharacterRelationshipSnapshot
							{
								RelatedCharacters = relatedCharacters
							};
						}
					}
					if (secretInformationItem.IsRelationCharactersAliveStateNeedSnapshot || (secretInformationItem.ExtraSnapshotParameterIndices != null && secretInformationItem.ExtraSnapshotParameterIndices.Contains(i)))
					{
						secretInformationMetaData.CharacterExtraInfoCollection.Collection.TryGetValue(arg2, out var value2);
						if (deadCharacter != null)
						{
							value2.FameType = deadCharacter.FameType;
							value2.MonkType = deadCharacter.MonkType;
							value2.OrgInfo = deadCharacter.OrganizationInfo;
						}
						else if (element != null)
						{
							value2.FameType = element.GetFameType();
							value2.MonkType = element.GetMonkType();
							value2.OrgInfo = element.GetOrganizationInfo();
						}
						secretInformationMetaData.CharacterExtraInfoCollection.Collection[arg2] = value2;
					}
				}
			}
		}
		AddElement_SecretInformationMetaData(andIncreaseNextMetaDataId, secretInformationMetaData);
		if (flag)
		{
			discoveryRate = secretInformationItem.DiscoveryRateTaiwu;
		}
		else
		{
			int num6 = ((num4 > 0) ? (num3 / num4) : 0);
			num6 = Math.Max(num6 * secretInformationItem.DiscoveryRateFactorA, 0);
			int num7 = ((num2 > 0) ? (num / num2) : 0);
			num7 = Math.Max(num7 * secretInformationItem.DiscoveryRateFactorB, 0);
			int val = (flag2 ? secretInformationItem.DiscoveryRateFactorC : 0);
			discoveryRate = discoveryRate * Math.Max(100 + num6 + num7, 100) / 100;
			discoveryRate = Math.Max(val, discoveryRate);
		}
		if (context.Random.CheckProb(discoveryRate, 10000) || necessarily)
		{
			if (secretInformationItem.AutoBroadCast)
			{
				MakeSecretInformationBroadcastEffect(context, andIncreaseNextMetaDataId, -1);
			}
			else if (withInitialDistribute)
			{
				int[] initialTargetParameterIndices = secretInformationItem.InitialTargetParameterIndices;
				foreach (int num8 in initialTargetParameterIndices)
				{
					int arg7 = -1;
					if (secretInformationItem.Parameters[num8] != "Character" || !eventArgBox.Get($"arg{num8}", ref arg7))
					{
						continue;
					}
					hashSet.Clear();
					if (!DomainManager.Character.TryGetElement_Objects(arg7, out var element4) || element4 == null)
					{
						continue;
					}
					Personalities personalities = element4.GetPersonalities();
					sbyte b2 = personalities.Items[2];
					if (element4.GetLocation().IsValid())
					{
						switch (secretInformationItem.InitialTarget)
						{
						case ESecretInformationInitialTarget.Area:
							DomainManager.Character.GetAreaPeople(context.Random, element4, hashSet, (sbyte)Math.Clamp((15 + b2 / 2) / 5, 0, 100));
							break;
						case ESecretInformationInitialTarget.Nearest:
							DomainManager.Character.GetClosePeople(context.Random, element4, hashSet, (sbyte)Math.Clamp((30 + b2) / 5, 0, 100));
							break;
						case ESecretInformationInitialTarget.Local:
							DomainManager.Character.GetSameBlockPeople(context.Random, element4, hashSet, (sbyte)Math.Clamp((60 + b2 * 2) / 5, 0, 100));
							break;
						}
					}
					else
					{
						hashSet.Add(arg7);
					}
					foreach (int item4 in hashSet)
					{
						ReceiveSecretInformation(context, andIncreaseNextMetaDataId, item4);
					}
					distributeCallback?.Invoke(hashSet);
				}
			}
		}
		DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		instance.Return(hashSet);
		return andIncreaseNextMetaDataId;
	}

	public bool ReceiveSecretInformation(DataContext context, int metaDataId, int charId, int sourceCharId = -1)
	{
		if (IsSecretInformationInBroadcast(metaDataId))
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element) || element.GetAgeGroup() == 0)
		{
			return false;
		}
		if (!TryGetElement_CharacterSecretInformation(charId, out var value))
		{
			AddElement_CharacterSecretInformation(charId, value = new SecretInformationCharacterDataCollection(), context);
		}
		if (!value.Collection.ContainsKey(metaDataId) && TryGetElement_SecretInformationMetaData(metaDataId, out var element2))
		{
			int num = -1;
			if (sourceCharId >= 0)
			{
				num = ((!TryGetElement_CharacterSecretInformation(sourceCharId, out var value2) || !value2.Collection.TryGetValue(metaDataId, out var value3) || value3.SecretInformationDisseminationBranch < 0) ? sourceCharId : value3.SecretInformationDisseminationBranch);
				element2.IncreaseCharacterDisseminationCount(num);
				if (DomainManager.Character.TryGetElement_Objects(num, out var element3))
				{
					short index = CalcSecretInformationTemplateId(element2);
					element3.ChangeResource(context, 7, SecretInformation.Instance[index].CostAuthority / 5);
				}
				if (_isOfflineUpdate)
				{
					_offlineIndicesMetaDataCharacterDisseminationData.Add(metaDataId);
				}
				else
				{
					element2.SetDisseminationData(element2.GetDisseminationData(), context);
				}
			}
			if (charId == DomainManager.Taiwu.GetTaiwuCharId())
			{
				_taiwuReceivedSecretInformationInMonth.Add(metaDataId);
				DomainManager.Taiwu.AddLegacyPoint(context, 34);
				short index2 = CalcSecretInformationTemplateId(element2);
				ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[90];
				int baseDelta = formulaCfg.Calculate(SecretInformation.Instance[index2].SortValue);
				DomainManager.Extra.ChangeProfessionSeniority(context, 14, baseDelta);
			}
			value.Collection.Add(metaDataId, new SecretInformationCharacterData(metaDataId, num));
			DomainManager.Extra.ClearCharacterSecretInformationUsedCount(context, charId, metaDataId);
			if (_isOfflineUpdate)
			{
				_characterSecretInformation[charId] = value;
				_offlineIndicesCharacterData.Add(charId);
			}
			else
			{
				SetElement_CharacterSecretInformation(charId, value, context);
			}
			return true;
		}
		return false;
	}

	public bool DistributeSecretInformationToCharacter(DataContext context, int metaDataId, int charId, int sourceCharId = -1)
	{
		if (IsSecretInformationInBroadcast(metaDataId))
		{
			return true;
		}
		return DomainManager.Information.ReceiveSecretInformation(context, metaDataId, charId, sourceCharId);
	}

	public void CostSecretInformationUsedCount(DataContext context, int charId, int metaDataId)
	{
		sbyte characterSecretInformationUsedCount = DomainManager.Extra.GetCharacterSecretInformationUsedCount(charId, metaDataId);
		int num = (IsSecretInformationInBroadcast(metaDataId) ? GlobalConfig.Instance.SecretInformationInBroadcastMaxUseCount : GlobalConfig.Instance.SecretInformationInPrivateMaxUseCount);
		characterSecretInformationUsedCount++;
		DomainManager.Extra.SetCharacterSecretInformationUsedCount(context, charId, metaDataId, characterSecretInformationUsedCount);
		if (characterSecretInformationUsedCount > num)
		{
			AdaptableLog.Error($"taiwu({charId}) used secret information {metaDataId} in {characterSecretInformationUsedCount} times(max: {num})");
		}
	}

	public void MakeSecretInformationBroadcastEffect(DataContext context, int metaDataId, int sourceCharId)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list.AddRange(_characterSecretInformation.Keys);
		foreach (int item in list)
		{
			SecretInformationCharacterDataCollection secretInformationCharacterDataCollection = _characterSecretInformation[item];
			if (secretInformationCharacterDataCollection.Collection.Remove(metaDataId))
			{
				if (_isOfflineUpdate)
				{
					_characterSecretInformation[item] = secretInformationCharacterDataCollection;
					_offlineIndicesCharacterData.Add(item);
				}
				else
				{
					SetElement_CharacterSecretInformation(item, secretInformationCharacterDataCollection, context);
				}
				DomainManager.Extra.ClearCharacterSecretInformationUsedCount(context, item, metaDataId);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
		if (IsSecretInformationInBroadcast(metaDataId))
		{
			return;
		}
		TaiwuEventDomain taiwuEvent = DomainManager.TaiwuEvent;
		DomainManager.Extra.AddSecretInformationInBroadcastList(context, metaDataId);
		if (TryGetElement_SecretInformationMetaData(metaDataId, out var _))
		{
			BroadcastEffect.OpenEffectEntrance(context, metaDataId, _isOfflineUpdate, sourceCharId, ref _StartEnemyRelationItem);
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		StackTrace stackTrace = new StackTrace();
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder3 = stringBuilder2;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(57, 1, stringBuilder2);
		handler.AppendLiteral("secret_information: ");
		handler.AppendFormatted(metaDataId);
		handler.AppendLiteral(" not found when require for broadcast");
		stringBuilder3.AppendLine(ref handler);
		StackFrame[] frames = stackTrace.GetFrames();
		foreach (StackFrame stackFrame in frames)
		{
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder4 = stringBuilder2;
			handler = new StringBuilder.AppendInterpolatedStringHandler(14, 3, stringBuilder2);
			handler.AppendLiteral("at ");
			handler.AppendFormatted(stackFrame.GetMethod()?.Name);
			handler.AppendLiteral(" in ");
			handler.AppendFormatted(stackFrame.GetFileName());
			handler.AppendLiteral(" Line: ");
			handler.AppendFormatted(stackFrame.GetFileLineNumber());
			stringBuilder4.AppendLine(ref handler);
		}
		AdaptableLog.Warning(stringBuilder.ToString());
	}

	[DomainMethod]
	public void DiscardSecretInformation(DataContext context, int charId, int metaDataId)
	{
		if (TryGetElement_CharacterSecretInformation(charId, out var value) && value.Collection.ContainsKey(metaDataId))
		{
			value.Collection.Remove(metaDataId);
			if (_isOfflineUpdate)
			{
				_characterSecretInformation[charId] = value;
				_offlineIndicesCharacterData.Add(charId);
			}
			else
			{
				SetElement_CharacterSecretInformation(charId, value, context);
			}
		}
		DomainManager.Extra.ClearCharacterSecretInformationUsedCount(context, charId, metaDataId);
	}

	[DomainMethod]
	public SecretInformationDisplayData GetSecretInformationDisplayData(int metaDataId)
	{
		return GetSecretInformationDisplayData(metaDataId, new HashSet<int>());
	}

	public unsafe void CheckSecretInformationValidState(DataContext context, out Dictionary<int, List<int>> offsetRefMap, out HashSet<int> multiRefOffsets, out List<int> danglingOffsetList, bool checkTemplateId = false)
	{
		offsetRefMap = new Dictionary<int, List<int>>();
		multiRefOffsets = new HashSet<int>();
		foreach (SecretInformationMetaData value2 in _secretInformationMetaData.Values)
		{
			int offset = value2.GetOffset();
			if (offsetRefMap.TryGetValue(offset, out var value))
			{
				multiRefOffsets.Add(offset);
			}
			else
			{
				value = new List<int>();
			}
			value.Add(value2.GetId());
			offsetRefMap[offset] = value;
		}
		danglingOffsetList = new List<int>();
		int num = 0;
		int i = 0;
		for (int count = _secretInformationCollection.Count; i < count; i++)
		{
			if (num >= _secretInformationCollection.RawData.Length)
			{
				AdaptableLog.TagWarning("CheckSecretInformationValidState", $"wrong secretInformationCollection.Count {_secretInformationCollection.Count}, recount to {i}");
				_secretInformationCollection.Count = i;
				CommitSetMetadata_SecretInformationCollection(context);
				break;
			}
			short num2 = -1;
			if (checkTemplateId)
			{
				fixed (byte* rawData = _secretInformationCollection.GetRawData())
				{
					byte* ptr = rawData + num;
					ptr++;
					int num3 = *(int*)ptr;
					ptr += 4;
					num2 = *(short*)ptr;
				}
			}
			if (!offsetRefMap.ContainsKey(num))
			{
				if (checkTemplateId && num2 >= 0)
				{
					AdaptableLog.TagWarning("CheckSecretInformationValidState", $"secretinformation data offset {num}[{SecretInformation.Instance[num2].Name}] is dangling");
				}
				else
				{
					AdaptableLog.TagWarning("CheckSecretInformationValidState", $"secretinformation data offset {num} is dangling");
				}
				danglingOffsetList.Add(num);
			}
			num += _secretInformationCollection.GetRecordSize(num);
		}
	}

	public void RemoveSenselessSecretInformation(DataContext context)
	{
		ObjectPool<Dictionary<int, int>> instance = ObjectPool<Dictionary<int, int>>.Instance;
		Dictionary<int, int> dictionary = instance.Get();
		dictionary.Clear();
		Dictionary<int, IntPair> dictionary2 = ObjectPool<Dictionary<int, IntPair>>.Instance.Get();
		dictionary2.Clear();
		foreach (KeyValuePair<int, SecretInformationMetaData> secretInformationMetaDatum in _secretInformationMetaData)
		{
			dictionary.Add(secretInformationMetaDatum.Key, 0);
			dictionary2.Add(secretInformationMetaDatum.Key, GetMaxDisseminationCounts(secretInformationMetaDatum.Value.GetDisseminationData().DisseminationCounts));
		}
		_swSecretInformation.Restart();
		foreach (SecretInformationCharacterDataCollection value4 in _characterSecretInformation.Values)
		{
			foreach (int key in value4.Collection.Keys)
			{
				if (dictionary.TryGetValue(key, out var value))
				{
					dictionary[key] = value + 1;
				}
			}
		}
		_swSecretInformation.Stop();
		Logger.Info($"{"RemoveSenselessSecretInformation"}(calculate character refCount): {_swSecretInformation.ElapsedMilliseconds} ms");
		_swSecretInformation.Restart();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list.AddRange(_secretInformationMetaData.Keys);
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		list2.Clear();
		list2.AddRange(_characterSecretInformation.Keys);
		foreach (int item in list)
		{
			IntPair intPair = dictionary2[item];
			SecretInformationMetaData element_SecretInformationMetaData = GetElement_SecretInformationMetaData(item);
			short templateId;
			int num = CalcSecretInformationRemainingLifeTime(element_SecretInformationMetaData, out templateId);
			SecretInformationItem secretInformationItem = SecretInformation.Instance[templateId];
			if (intPair.Second >= secretInformationItem.MaxPersonAmount)
			{
				MakeSecretInformationBroadcastEffect(context, item, intPair.First);
				if (IsSecretInformationInBroadcast(item))
				{
					dictionary[item] = 0;
				}
			}
			if (num > 0)
			{
				continue;
			}
			List<int> broadcastSecretInformation = GetBroadcastSecretInformation();
			if (broadcastSecretInformation.Remove(item) && !_isOfflineUpdate)
			{
				SetBroadcastSecretInformation(broadcastSecretInformation, context);
			}
			DomainManager.Extra.RemoveSecretInformationInBroadcastList(context, item);
			foreach (int item2 in list2)
			{
				if (!TryGetElement_CharacterSecretInformation(item2, out var value2) || !value2.Collection.Remove(item))
				{
					continue;
				}
				if (value2.Collection.Count <= 0)
				{
					if (_isOfflineUpdate)
					{
						if (_characterSecretInformation.Remove(item2))
						{
							_offlineIndicesCharacterData.Add(item2);
						}
					}
					else
					{
						RemoveElement_CharacterSecretInformation(item2, context);
					}
				}
				else if (_isOfflineUpdate)
				{
					_offlineIndicesCharacterData.Add(item2);
				}
				else
				{
					SetElement_CharacterSecretInformation(item2, value2, context);
				}
				dictionary[item]--;
			}
			Tester.Assert(dictionary[item] == 0, $"SecretInformation {item} refCount({dictionary[item]}) must be 0 when it will be deleted.");
		}
		ObjectPool<List<int>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
		_swSecretInformation.Stop();
		Logger.Info($"{"RemoveSenselessSecretInformation"}(calculate object status): {_swSecretInformation.ElapsedMilliseconds} ms");
		foreach (int broadcastSecretInformationId in GetBroadcastSecretInformationIds())
		{
			if (dictionary.TryGetValue(broadcastSecretInformationId, out var value3))
			{
				dictionary[broadcastSecretInformationId] = value3 + 1;
			}
		}
		_swSecretInformation.Restart();
		SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
		_secretInformationRemovingList.Clear();
		foreach (SecretInformationMetaData value5 in _secretInformationMetaData.Values)
		{
			_secretInformationRemovingList.Add(value5.GetOffset(), value5.GetId());
		}
		ObjectPool<List<int>> instance2 = ObjectPool<List<int>>.Instance;
		List<int> list3 = instance2.Get();
		List<int> list4 = instance2.Get();
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < _secretInformationRemovingList.Count; i++)
		{
			int num4 = _secretInformationRemovingList.Values[i];
			if (dictionary[num4] >= 1)
			{
				continue;
			}
			int offset = GetElement_SecretInformationMetaData(num4).GetOffset();
			int recordSize = secretInformationCollection.GetRecordSize(offset);
			RemoveElement_SecretInformationMetaData(num4);
			DomainManager.Extra.RemoveSecretInformationOccurredLocation(context, num4);
			num3++;
			num2 += recordSize;
			if (list3.Count > 0)
			{
				if (list4[list4.Count - 1] + list3[list3.Count - 1] == offset)
				{
					List<int> list5 = list3;
					list5[list5.Count - 1] += recordSize;
					continue;
				}
			}
			list3.Add(recordSize);
			list4.Add(offset);
		}
		int num5 = -1;
		for (int j = 0; j < list4.Count; j++)
		{
			int num6 = list3[j];
			int num7 = list4[j];
			if (num5 < 0)
			{
				num5 = num7;
			}
			int num8 = num5;
			int num9 = num7 + num6;
			int num10 = ((j + 1 == list4.Count) ? secretInformationCollection.Size : list4[j + 1]);
			int num11 = num10 - num9;
			secretInformationCollection.Move(num9, num8, num11);
			CommitWrite_SecretInformationCollection(context, num8, num11);
			num5 += num11;
		}
		if (num5 >= 0)
		{
			secretInformationCollection.Remove(num5, num2);
			CommitRemove_SecretInformationCollection(context, num5, num2);
			secretInformationCollection.Count -= num3;
			CommitSetMetadata_SecretInformationCollection(context);
		}
		if (list3.Count > 0)
		{
			int num12 = 0;
			int num13 = list3[0];
			for (int k = 0; k < _secretInformationRemovingList.Count; k++)
			{
				int num14 = _secretInformationRemovingList.Values[k];
				if (!TryGetElement_SecretInformationMetaData(num14, out var element))
				{
					continue;
				}
				int offset2 = element.GetOffset();
				if (offset2 >= list4[num12])
				{
					if (num12 < list4.Count - 1 && offset2 > list4[num12 + 1])
					{
						num12++;
						num13 += list3[num12];
					}
					element.UpdateOffset(-num13);
					if (_isOfflineUpdate)
					{
						_offlineIndicesMetaDataOffset.Add(num14);
					}
					else
					{
						element.SetOffset(element.GetOffset(), context);
					}
				}
			}
		}
		_npcPlanCastCount = 0;
		instance2.Return(list3);
		instance2.Return(list4);
		_swSecretInformation.Stop();
		Logger.Info($"{"RemoveSenselessSecretInformation"}(remove objects {num3}): {_swSecretInformation.ElapsedMilliseconds} ms");
		instance.Return(dictionary);
		ObjectPool<Dictionary<int, IntPair>>.Instance.Return(dictionary2);
		CheckSecretInformationValidState(context, out var _, out var multiRefOffsets, out var danglingOffsetList, checkTemplateId: true);
		Tester.Assert(multiRefOffsets.Count == 0, "multiRefOffsets.Count == 0");
		Tester.Assert(danglingOffsetList.Count == 0, "danglingOffsetList.Count == 0");
	}

	private void SecretInformationWipeAll(DataContext context)
	{
		SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
		int size = secretInformationCollection.Size;
		secretInformationCollection.Clear();
		secretInformationCollection.Count = 0;
		CommitRemove_SecretInformationCollection(context, 0, size);
		CommitSetMetadata_SecretInformationCollection(context);
		ClearSecretInformationMetaData();
		ClearCharacterSecretInformation(context);
		List<int> broadcastSecretInformation = GetBroadcastSecretInformation();
		broadcastSecretInformation?.Clear();
		SetBroadcastSecretInformation(broadcastSecretInformation, context);
		DomainManager.Extra.ClearCharacterAllSecretInformationUsedCount(context);
		DomainManager.Extra.ClearAllSecretInformationOccurredLocation(context);
		DomainManager.Extra.ClearSecretInformationInBroadcastList(context);
		ClearedTaiwuReceivedSecretInformationInMonth();
		DomainManager.Extra.ClearSecretInformationBroadcastNotifyList(context);
	}

	private IntPair GetMaxDisseminationCounts(IDictionary<int, int> disseminationCounts)
	{
		int num = 0;
		int first = -1;
		foreach (KeyValuePair<int, int> disseminationCount in disseminationCounts)
		{
			if (disseminationCount.Value > num)
			{
				num = disseminationCount.Value;
				first = disseminationCount.Key;
			}
		}
		return new IntPair(first, num);
	}

	public unsafe void PlanDisseminateSecretInformation(DataContext context, int charId)
	{
		CharacterDomain character = DomainManager.Character;
		if (!TryGetElement_CharacterSecretInformation(charId, out var value) || !character.TryGetElement_Objects(charId, out var element))
		{
			return;
		}
		Location location = element.GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		List<SecretInformationMetaData> list = ObjectPool<List<SecretInformationMetaData>>.Instance.Get();
		list.Clear();
		foreach (KeyValuePair<int, SecretInformationCharacterData> item2 in value.Collection)
		{
			if (!IsSecretInformationInBroadcast(item2.Key) && TryGetElement_SecretInformationMetaData(item2.Key, out var element2) && CalcSecretInformationRemainingLifeTime(element2, out var templateId) > 0)
			{
				SecretInformationItem item = SecretInformation.Instance.GetItem(templateId);
				if ((item == null || item.AutoDissemination) && CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(templateId, element.GetFameType(), GetSecretInformationDisseminatingCountOfBranch(item2.Key, item2.Value.SecretInformationDisseminationBranch)) <= element.GetResource(7))
				{
					list.Add(element2);
				}
			}
		}
		Dictionary<int, int> relatives;
		SecretInformationDisseminationItem dissemination;
		SecretInformationEffectItem effect;
		bool isSelfIntroduction;
		HashSet<SecretInformationRelationshipType> r;
		if (list.Count > 0)
		{
			SecretInformationMetaData random = list.GetRandom(context.Random);
			int id = random.GetId();
			int num = 0;
			int num2 = (IsSecretInformationInBroadcast(id) ? GlobalConfig.Instance.SecretInformationInBroadcastMaxUseCount : GlobalConfig.Instance.SecretInformationInPrivateMaxUseCount);
			num = DomainManager.Extra.GetCharacterSecretInformationUsedCount(element.GetId(), id);
			if (num < num2)
			{
				relatives = ObjectPool<Dictionary<int, int>>.Instance.Get();
				relatives.Clear();
				SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
				int offset = random.GetOffset();
				SecretInformationItem secretInformationItem;
				fixed (byte* rawData = secretInformationCollection.GetRawData())
				{
					byte* ptr = rawData + offset;
					ptr++;
					int num3 = *(int*)ptr;
					ptr += 4;
					short index = *(short*)ptr;
					ptr += 2;
					secretInformationItem = SecretInformation.Instance[index];
					if (secretInformationItem.Parameters != null)
					{
						int i = 0;
						for (int num4 = secretInformationItem.Parameters.Length; i < num4; i++)
						{
							string text = secretInformationItem.Parameters[i];
							if (!string.IsNullOrEmpty(text))
							{
								string text2 = $"arg{i}";
								switch (text)
								{
								case "Character":
									relatives.Add(i, *(int*)ptr);
									ptr += 4;
									break;
								case "Location":
									ptr += 2;
									ptr += 2;
									break;
								case "ItemKey":
									ptr += 8;
									break;
								case "Resource":
									ptr++;
									break;
								case "LifeSkill":
									ptr += 2;
									break;
								case "CombatSkill":
									ptr += 2;
									break;
								case "Integer":
									ptr += 4;
									break;
								default:
									throw new NotImplementedException(text);
								}
							}
						}
					}
				}
				int num5 = CalcSecretInformationAuthorityCostWhenDisseminatingByCharacter(secretInformationItem.TemplateId, element.GetFameType(), GetSecretInformationDisseminatingCountOfBranch(id, value.Collection[id].SecretInformationDisseminationBranch));
				dissemination = SecretInformationDissemination.Instance[secretInformationItem.DisseminationId];
				effect = SecretInformationEffect.Instance[secretInformationItem.DefaultEffectId];
				Personalities personalities = element.GetPersonalities();
				isSelfIntroduction = relatives.ContainsValue(charId);
				r = new HashSet<SecretInformationRelationshipType>();
				int num6 = 0;
				if (isSelfIntroduction)
				{
					num6 += dissemination.SfBehaviorTypeDiff[element.GetBehaviorType()];
					for (int j = 0; j < 5; j++)
					{
						num6 += dissemination.SfPersonalityDiff[j] * personalities.Items[j];
					}
				}
				else
				{
					num6 += dissemination.TfBehaviorTypeDiff[element.GetBehaviorType()];
					for (int k = 0; k < 5; k++)
					{
						num6 += dissemination.TfPersonalityDiff[k] * personalities.Items[k];
					}
					if (relatives.TryGetValue(effect.ActorIndex, out var value2))
					{
						r.Clear();
						CheckSecretInformationRelationship(charId, -1, value2, -1, r);
						if (r.Contains(SecretInformationRelationshipType.Relative) || r.Contains(SecretInformationRelationshipType.Friend))
						{
							num6 += dissemination.TfRateDiffWhenActFri;
						}
						else if (r.Contains(SecretInformationRelationshipType.Enemy))
						{
							num6 += dissemination.TfRateDiffWhenActEnm;
						}
					}
					if (relatives.TryGetValue(effect.ActorIndex, out var value3))
					{
						r.Clear();
						CheckSecretInformationRelationship(charId, -1, value3, -1, r);
						if (r.Contains(SecretInformationRelationshipType.Relative) || r.Contains(SecretInformationRelationshipType.Friend))
						{
							num6 += dissemination.TfRateDiffWhenUnaFri;
						}
						else if (r.Contains(SecretInformationRelationshipType.Enemy))
						{
							num6 += dissemination.TfRateDiffWhenUnaEnm;
						}
					}
				}
				MapBlockData block = DomainManager.Map.GetBlock(location);
				ByteCoordinate blockPos = block.GetBlockPos();
				int num7 = 0;
				if (secretInformationItem.DiffusionRange >= 0)
				{
					Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(location.AreaId);
					for (int l = 0; l < areaBlocks.Length; l++)
					{
						MapBlockData mapBlockData = areaBlocks[l];
						if (mapBlockData.GetManhattanDistanceToPos(blockPos.X, blockPos.Y) != secretInformationItem.DiffusionRange)
						{
							continue;
						}
						if (mapBlockData.CharacterSet != null)
						{
							foreach (int item3 in mapBlockData.CharacterSet)
							{
								int num8 = GetRateByRelation(item3) + num6;
								if (context.Random.Next(100) < num8)
								{
									ReceiveSecretInformation(context, id, item3, charId);
									num++;
									num7++;
								}
								if (num7 >= secretInformationItem.DiffusionSpeed || num >= num2)
								{
									break;
								}
							}
						}
						if (num7 >= secretInformationItem.DiffusionSpeed || num >= num2)
						{
							break;
						}
					}
				}
				else
				{
					int num9 = 0;
					for (; num6 > 0; num6 += secretInformationItem.DiffusionRange * 10)
					{
						Span<MapBlockData> areaBlocks2 = DomainManager.Map.GetAreaBlocks(location.AreaId);
						for (int m = 0; m < areaBlocks2.Length; m++)
						{
							MapBlockData mapBlockData2 = areaBlocks2[m];
							if (mapBlockData2.GetManhattanDistanceToPos(blockPos.X, blockPos.Y) != num9)
							{
								continue;
							}
							if (mapBlockData2.CharacterSet != null)
							{
								foreach (int item4 in mapBlockData2.CharacterSet)
								{
									int num10 = GetRateByRelation(item4) + num6;
									if (context.Random.Next(100) < num10)
									{
										ReceiveSecretInformation(context, id, item4, charId);
										num++;
										num7++;
									}
									if (num7 >= secretInformationItem.DiffusionSpeed || num >= num2)
									{
										break;
									}
								}
							}
							if (num7 >= secretInformationItem.DiffusionSpeed || num >= num2)
							{
								break;
							}
						}
						if (num7 >= secretInformationItem.DiffusionSpeed || num >= num2)
						{
							break;
						}
						num9++;
					}
				}
				_npcPlanCastCount += num7;
				num5 *= num7;
				element.ChangeResource(context, 7, -Math.Min(element.GetResource(7), num5));
				DomainManager.Extra.SetCharacterSecretInformationUsedCount(context, charId, id, (sbyte)Math.Clamp(num, 0, num2));
				ObjectPool<Dictionary<int, int>>.Instance.Return(relatives);
			}
		}
		ObjectPool<List<SecretInformationMetaData>>.Instance.Return(list);
		int GetRateByRelation(int targetCharId)
		{
			int num11 = 0;
			if (relatives.TryGetValue(effect.ActorIndex, out var value4) && relatives.TryGetValue(effect.ReactorIndex, out var value5))
			{
				if (!isSelfIntroduction)
				{
					r.Clear();
					CheckSecretInformationRelationship(charId, -1, targetCharId, -1, r);
					if (r.Contains(SecretInformationRelationshipType.Relative) || r.Contains(SecretInformationRelationshipType.Friend))
					{
						num11 += dissemination.TfRateItsFri;
					}
					else if (r.Contains(SecretInformationRelationshipType.Enemy))
					{
						num11 += dissemination.TfRateItsEnm;
					}
				}
				if (num11 == 0)
				{
					r.Clear();
					CheckSecretInformationRelationship(value4, -1, targetCharId, -1, r);
					if (r.Contains(SecretInformationRelationshipType.Relative) || r.Contains(SecretInformationRelationshipType.Friend))
					{
						num11 += (isSelfIntroduction ? dissemination.SfRateActFri : dissemination.TfRateActFri);
					}
					else if (r.Contains(SecretInformationRelationshipType.Enemy))
					{
						num11 += (isSelfIntroduction ? dissemination.SfRateActEnm : dissemination.TfRateActEnm);
					}
					else
					{
						r.Clear();
						CheckSecretInformationRelationship(value5, -1, targetCharId, -1, r);
						num11 = ((r.Contains(SecretInformationRelationshipType.Relative) || r.Contains(SecretInformationRelationshipType.Friend)) ? (num11 + (isSelfIntroduction ? dissemination.SfRateUnaFri : dissemination.TfRateUnaFri)) : (r.Contains(SecretInformationRelationshipType.Enemy) ? (num11 + (isSelfIntroduction ? dissemination.SfRateUnaEnm : dissemination.TfRateUnaEnm)) : ((!DomainManager.Character.TryGetRelation(charId, targetCharId, out var _)) ? (num11 + (isSelfIntroduction ? dissemination.SfRateNStr : dissemination.TfRateNStr)) : (num11 + (isSelfIntroduction ? dissemination.SfRateStr : dissemination.TfRateStr)))));
					}
				}
				return num11;
			}
			return -10000;
		}
	}

	private RelatedCharacters GetSecretInformationCharacterRelationSnapshot(int metaDataId, int characterId)
	{
		SecretInformationCharacterRelationshipSnapshot value;
		return GetElement_SecretInformationMetaData(metaDataId).CharacterRelationshipSnapshotCollection.Collection.TryGetValue(characterId, out value) ? value.RelatedCharacters : null;
	}

	public HashSet<SecretInformationRelationshipType> CheckSecretInformationRelationship(int characterId, int selfMetaDataIdForSnapshot, int targetCharacterId, int targetMetaDataIdForSnapshot)
	{
		return CheckSecretInformationRelationship(characterId, selfMetaDataIdForSnapshot, targetCharacterId, targetMetaDataIdForSnapshot, new HashSet<SecretInformationRelationshipType>());
	}

	public HashSet<SecretInformationRelationshipType> CheckSecretInformationRelationship(int characterId, int selfMetaDataIdForSnapshot, int targetCharacterId, int targetMetaDataIdForSnapshot, HashSet<SecretInformationRelationshipType> container)
	{
		CharacterDomain character = DomainManager.Character;
		OrganizationDomain organization = DomainManager.Organization;
		RelatedCharacters relatedCharacters = ((selfMetaDataIdForSnapshot < 0) ? character.GetRelatedCharacters(characterId) : GetSecretInformationCharacterRelationSnapshot(selfMetaDataIdForSnapshot, characterId));
		RelatedCharacters relatedCharacters2 = ((targetMetaDataIdForSnapshot < 0) ? character.GetRelatedCharacters(targetCharacterId) : GetSecretInformationCharacterRelationSnapshot(targetMetaDataIdForSnapshot, targetCharacterId));
		if (relatedCharacters != null && (relatedCharacters.BloodParents.Contains(targetCharacterId) || relatedCharacters.StepParents.Contains(targetCharacterId) || relatedCharacters.AdoptiveParents.Contains(targetCharacterId) || relatedCharacters.BloodChildren.Contains(targetCharacterId) || relatedCharacters.StepChildren.Contains(targetCharacterId) || relatedCharacters.AdoptiveChildren.Contains(targetCharacterId) || relatedCharacters.BloodBrothersAndSisters.Contains(targetCharacterId) || relatedCharacters.StepBrothersAndSisters.Contains(targetCharacterId) || relatedCharacters.AdoptiveBrothersAndSisters.Contains(targetCharacterId)))
		{
			container.Add(SecretInformationRelationshipType.Relative);
			container.Add(SecretInformationRelationshipType.Allied);
		}
		if ((relatedCharacters != null && relatedCharacters.Friends.Contains(targetCharacterId)) || (relatedCharacters2 != null && relatedCharacters2.Friends.Contains(characterId)))
		{
			container.Add(SecretInformationRelationshipType.Friend);
			container.Add(SecretInformationRelationshipType.Allied);
		}
		if ((relatedCharacters != null && relatedCharacters.SwornBrothersAndSisters.Contains(targetCharacterId)) || (relatedCharacters2 != null && relatedCharacters2.SwornBrothersAndSisters.Contains(characterId)))
		{
			container.Add(SecretInformationRelationshipType.SwornBrotherOrSister);
			container.Add(SecretInformationRelationshipType.Allied);
		}
		if ((relatedCharacters != null && relatedCharacters.HusbandsAndWives.Contains(targetCharacterId)) || (relatedCharacters2 != null && relatedCharacters2.HusbandsAndWives.Contains(characterId)))
		{
			container.Add(SecretInformationRelationshipType.HusbandOrWife);
			container.Add(SecretInformationRelationshipType.Allied);
		}
		if (relatedCharacters2 != null && relatedCharacters2.Adored.Contains(characterId))
		{
			if (relatedCharacters != null && relatedCharacters.Adored.Contains(targetCharacterId))
			{
				container.Add(SecretInformationRelationshipType.Lover);
			}
			container.Add(SecretInformationRelationshipType.Adorer);
			container.Add(SecretInformationRelationshipType.Allied);
		}
		if (relatedCharacters2 != null && relatedCharacters2.Enemies.Contains(characterId))
		{
			container.Add(SecretInformationRelationshipType.Enemy);
		}
		OrganizationInfo organizationInfo = new OrganizationInfo(-1, -1, principal: true, -1);
		OrganizationInfo organizationInfo2 = new OrganizationInfo(-1, -1, principal: true, -1);
		DeadCharacter deadCharacter;
		if (character.TryGetElement_Objects(characterId, out var element))
		{
			organizationInfo = element.GetOrganizationInfo();
		}
		else if ((deadCharacter = character.TryGetDeadCharacter(characterId)) != null)
		{
			organizationInfo = deadCharacter.OrganizationInfo;
		}
		if (character.TryGetElement_Objects(targetCharacterId, out element))
		{
			organizationInfo2 = element.GetOrganizationInfo();
		}
		else if ((deadCharacter = character.TryGetDeadCharacter(targetCharacterId)) != null)
		{
			organizationInfo2 = deadCharacter.OrganizationInfo;
		}
		if (organizationInfo.OrgTemplateId >= 0 && Config.Organization.Instance[organizationInfo.OrgTemplateId].IsSect && organizationInfo.OrgTemplateId == organizationInfo2.OrgTemplateId)
		{
			container.Add(SecretInformationRelationshipType.Comrade);
		}
		if ((relatedCharacters != null && (relatedCharacters.Mentors.Contains(targetCharacterId) || relatedCharacters.Mentees.Contains(targetCharacterId))) || (relatedCharacters2 != null && (relatedCharacters2.Mentors.Contains(characterId) || relatedCharacters2.Mentees.Contains(characterId))))
		{
			container.Add(SecretInformationRelationshipType.MentorAndMentee);
			container.Add(SecretInformationRelationshipType.Allied);
		}
		int bloodParent = character.GetBloodParent(characterId, 1);
		if (bloodParent >= 0)
		{
			if (!character.TryGetActualBloodParent(bloodParent, characterId, out var actualParentId))
			{
				actualParentId = bloodParent;
			}
			if (actualParentId == targetCharacterId)
			{
				container.Add(SecretInformationRelationshipType.ActualBloodFather);
			}
		}
		return container;
	}

	internal bool RequestShopSecretInformationIdShouldExclude(int secretId, EventArgBox argBox, int charId)
	{
		if (!SecretInformationDisplayData.IsSecretInformationValid(secretId))
		{
			return true;
		}
		if (IsSecretInformationInBroadcast(secretId))
		{
			return true;
		}
		if (IsCharacterIsOwningSecretInformation(DomainManager.Taiwu.GetTaiwuCharId(), secretId))
		{
			return true;
		}
		if (TryGetElement_SecretInformationMetaData(secretId, out var element))
		{
			SecretInformationItem secretInformationItem = SecretInformation.Instance[CalcSecretInformationTemplateId(element)];
			argBox.Clear();
			argBox = MakeSecretInformationEventArgBox(element, argBox);
			if (secretInformationItem.Parameters.Length != 0 && secretInformationItem.Parameters[0] == "Character" && argBox.GetInt("arg0") == charId)
			{
				return true;
			}
			if (secretInformationItem.Parameters.Length > 1 && secretInformationItem.Parameters[1] == "Character" && argBox.GetInt("arg1") == charId)
			{
				return true;
			}
		}
		return false;
	}

	public ICollection<int> RequestShopSecretInformationIdList(DataContext dataContext, int charId)
	{
		HashSet<int> hashSet = new HashSet<int>();
		SecretInformationShopCharacterData secretInformationShopCharacterData = DomainManager.Extra.AddOrGetSecretInformationShopCharacterData(dataContext, charId);
		hashSet.UnionWith(secretInformationShopCharacterData.CollectedSecretInformationIds);
		return hashSet;
	}

	[DomainMethod]
	public void SettleSecretInformationShopTrade(DataContext context, List<IntPair> secretList, int shopCharId)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(shopCharId);
		SecretInformationShopCharacterData secretInformationShopCharacterData = DomainManager.Extra.AddOrGetSecretInformationShopCharacterData(context, shopCharId);
		bool flag = false;
		foreach (IntPair secret in secretList)
		{
			int first = secret.First;
			int second = secret.Second;
			ReceiveSecretInformation(context, first, taiwu.GetId(), taiwu.GetId());
			flag = true;
			secretInformationShopCharacterData.CollectedSecretInformationIds.Remove(first);
			taiwu.ChangeResource(context, 6, -second);
			element_Objects.ChangeResource(context, 6, second);
		}
		if (flag)
		{
			DomainManager.TaiwuEvent.SetListenerEventActionBoolArg("ShopActionComplete", "ConchShip_PresetKey_ShopHasAnyTrade", value: true);
		}
		if (secretInformationShopCharacterData.CollectedSecretInformationIds.Count == 0)
		{
			secretInformationShopCharacterData.CollectedSecretInformationIds.Add(-1);
		}
		DomainManager.Extra.SetSecretInformationShopCharacterData(context, shopCharId, secretInformationShopCharacterData);
	}

	internal void ReleaseAllJingangInformation(DataContext ctx)
	{
		int[] array = _characterSecretInformation.Keys.ToArray();
		foreach (int charId in array)
		{
			foreach (int item in GetSecretInformationOfCharacter(charId, includeBroadcast: false))
			{
				if (EventHelper.JingangSecretInformationIsSolveScripture(item))
				{
					DiscardSecretInformation(ctx, charId, item);
				}
			}
		}
		int[] array2 = GetBroadcastSecretInformationIds().ToArray();
		foreach (int metaDataId in array2)
		{
			if (EventHelper.JingangSecretInformationIsSolveScripture(metaDataId))
			{
				DomainManager.Extra.RemoveSecretInformationInBroadcastList(ctx, metaDataId);
			}
		}
	}

	internal void RemoveAllNotExistBroadcastSecretInformation(DataContext ctx)
	{
		ExtraDomain extra = DomainManager.Extra;
		int[] array = extra.GetSecretInformationInBroadcastList().ToArray();
		foreach (int num in array)
		{
			if (!TryGetElement_SecretInformationMetaData(num, out var _))
			{
				extra.RemoveSecretInformationInBroadcastList(ctx, num);
			}
		}
	}

	internal IReadOnlyCollection<int> SecretInformationAllHandledCharacterIds()
	{
		return _characterSecretInformation.Keys;
	}

	internal IReadOnlyCollection<int> QuerySecretInformationIdsHandledByCharacter(int characterId)
	{
		if (TryGetElement_CharacterSecretInformation(characterId, out var value) && value.Collection != null)
		{
			return value.Collection.Keys as IReadOnlyCollection<int>;
		}
		return (IReadOnlyCollection<int>)(object)Array.Empty<int>();
	}

	internal bool IsCharacterIsOwningSecretInformation(int characterId, int secretId)
	{
		if (TryGetElement_CharacterSecretInformation(characterId, out var value) && value.Collection != null)
		{
			return value.Collection.ContainsKey(secretId);
		}
		return false;
	}

	internal unsafe int QuerySecretInformationOccurrenceDate(int secretId)
	{
		if (TryGetElement_SecretInformationMetaData(secretId, out var element))
		{
			SecretInformationCollection secretInformationCollection = GetSecretInformationCollection();
			int offset = element.GetOffset();
			fixed (byte* rawData = secretInformationCollection.GetRawData())
			{
				byte* ptr = rawData + offset;
				ptr++;
				int num = *(int*)ptr;
				ptr += 4;
			}
		}
		return -1;
	}

	public InformationDomain()
		: base(10)
	{
		_information = new Dictionary<int, NormalInformationCollection>(0);
		_secretInformationCollection = new SecretInformationCollection();
		_secretInformationMetaData = new Dictionary<int, SecretInformationMetaData>(0);
		_nextMetaDataId = 0;
		_characterSecretInformation = new Dictionary<int, SecretInformationCharacterDataCollection>(0);
		_broadcastSecretInformation = new List<int>();
		_taiwuReceivedNormalInformationInMonth = new List<NormalInformation>();
		_taiwuReceivedSecretInformationInMonth = new List<int>();
		_taiwuReceivedInformation = new List<int>();
		_taiwuTmpInformation = new List<NormalInformation>();
		HelperDataSecretInformationMetaData = new ObjectCollectionHelperData(18, 2, CacheInfluencesSecretInformationMetaData, _dataStatesSecretInformationMetaData, isArchive: true);
		OnInitializedDomainData();
	}

	public NormalInformationCollection GetElement_Information(int elementId)
	{
		return _information[elementId];
	}

	public bool TryGetElement_Information(int elementId, out NormalInformationCollection value)
	{
		return _information.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_Information(int elementId, NormalInformationCollection value, DataContext context)
	{
		_information.Add(elementId, value);
		_modificationsInformation.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(18, 0, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(18, 0, elementId, 0);
		}
	}

	private unsafe void SetElement_Information(int elementId, NormalInformationCollection value, DataContext context)
	{
		_information[elementId] = value;
		_modificationsInformation.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(18, 0, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(18, 0, elementId, 0);
		}
	}

	private void RemoveElement_Information(int elementId, DataContext context)
	{
		_information.Remove(elementId);
		_modificationsInformation.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(18, 0, elementId);
	}

	private void ClearInformation(DataContext context)
	{
		_information.Clear();
		_modificationsInformation.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(18, 0);
	}

	public SecretInformationCollection GetSecretInformationCollection()
	{
		return _secretInformationCollection;
	}

	private unsafe void CommitInsert_SecretInformationCollection(DataContext context, int offset, int size)
	{
		_modificationsSecretInformationCollection.RecordInserting(offset, size);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		byte* pDest = OperationAdder.Binary_Insert(18, 1, offset, size);
		_secretInformationCollection.CopyTo(offset, size, pDest);
	}

	private unsafe void CommitWrite_SecretInformationCollection(DataContext context, int offset, int size)
	{
		_modificationsSecretInformationCollection.RecordWriting(offset, size);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		byte* pDest = OperationAdder.Binary_Write(18, 1, offset, size);
		_secretInformationCollection.CopyTo(offset, size, pDest);
	}

	private void CommitRemove_SecretInformationCollection(DataContext context, int offset, int size)
	{
		_modificationsSecretInformationCollection.RecordRemoving(offset, size);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		OperationAdder.Binary_Remove(18, 1, offset, size);
	}

	private unsafe void CommitSetMetadata_SecretInformationCollection(DataContext context)
	{
		_modificationsSecretInformationCollection.RecordSettingMetadata();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		ushort serializedFixedSizeOfMetadata = _secretInformationCollection.GetSerializedFixedSizeOfMetadata();
		byte* pData = OperationAdder.Binary_SetMetadata(18, 1, serializedFixedSizeOfMetadata);
		_secretInformationCollection.SerializeMetadata(pData);
	}

	public SecretInformationMetaData GetElement_SecretInformationMetaData(int objectId)
	{
		return _secretInformationMetaData[objectId];
	}

	public bool TryGetElement_SecretInformationMetaData(int objectId, out SecretInformationMetaData element)
	{
		return _secretInformationMetaData.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_SecretInformationMetaData(int objectId, SecretInformationMetaData instance)
	{
		instance.CollectionHelperData = HelperDataSecretInformationMetaData;
		instance.DataStatesOffset = _dataStatesSecretInformationMetaData.Create();
		_secretInformationMetaData.Add(objectId, instance);
		byte* pData = OperationAdder.DynamicObjectCollection_Add(18, 2, objectId, instance.GetSerializedSize());
		instance.Serialize(pData);
	}

	private void RemoveElement_SecretInformationMetaData(int objectId)
	{
		if (_secretInformationMetaData.TryGetValue(objectId, out var value))
		{
			_dataStatesSecretInformationMetaData.Remove(value.DataStatesOffset);
			_secretInformationMetaData.Remove(objectId);
			OperationAdder.DynamicObjectCollection_Remove(18, 2, objectId);
		}
	}

	private void ClearSecretInformationMetaData()
	{
		_dataStatesSecretInformationMetaData.Clear();
		_secretInformationMetaData.Clear();
		OperationAdder.DynamicObjectCollection_Clear(18, 2);
	}

	public int GetElementField_SecretInformationMetaData(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_secretInformationMetaData.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_SecretInformationMetaData", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesSecretInformationMetaData.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetOffset(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetDisseminationData(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetRelevanceSecretInformationMetaDataId(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetSecretInformationCharacterRelationshipSnapshotCollection(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetSecretInformationCharacterExtraInfoCollection(), dataPool);
		default:
			if (fieldId >= 6)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_SecretInformationMetaData(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_secretInformationMetaData.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
		{
			int item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetOffset(item3, context);
			return;
		}
		case 2:
		{
			SecretInformationDisseminationData item2 = value.GetDisseminationData();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetDisseminationData(item2, context);
			return;
		}
		case 3:
		{
			int item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetRelevanceSecretInformationMetaDataId(item, context);
			return;
		}
		case 4:
		{
			SecretInformationCharacterRelationshipSnapshotCollection item5 = value.GetSecretInformationCharacterRelationshipSnapshotCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetSecretInformationCharacterRelationshipSnapshotCollection(item5, context);
			return;
		}
		case 5:
		{
			SecretInformationCharacterExtraInfoCollection item4 = value.GetSecretInformationCharacterExtraInfoCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetSecretInformationCharacterExtraInfoCollection(item4, context);
			return;
		}
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_SecretInformationMetaData(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_secretInformationMetaData.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesSecretInformationMetaData.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesSecretInformationMetaData.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetOffset(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetDisseminationData(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetRelevanceSecretInformationMetaDataId(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetSecretInformationCharacterRelationshipSnapshotCollection(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetSecretInformationCharacterExtraInfoCollection(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_SecretInformationMetaData(int objectId, ushort fieldId)
	{
		if (_secretInformationMetaData.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 6)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesSecretInformationMetaData.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesSecretInformationMetaData.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_SecretInformationMetaData(int objectId, ushort fieldId)
	{
		if (!_secretInformationMetaData.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 6)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesSecretInformationMetaData.IsModified(value.DataStatesOffset, fieldId);
	}

	private int GetNextMetaDataId()
	{
		return _nextMetaDataId;
	}

	private unsafe void SetNextMetaDataId(int value, DataContext context)
	{
		_nextMetaDataId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(18, 3, 4);
		*(int*)ptr = _nextMetaDataId;
		ptr += 4;
	}

	public SecretInformationCharacterDataCollection GetElement_CharacterSecretInformation(int elementId)
	{
		return _characterSecretInformation[elementId];
	}

	public bool TryGetElement_CharacterSecretInformation(int elementId, out SecretInformationCharacterDataCollection value)
	{
		return _characterSecretInformation.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_CharacterSecretInformation(int elementId, SecretInformationCharacterDataCollection value, DataContext context)
	{
		_characterSecretInformation.Add(elementId, value);
		_modificationsCharacterSecretInformation.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(18, 4, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(18, 4, elementId, 0);
		}
	}

	private unsafe void SetElement_CharacterSecretInformation(int elementId, SecretInformationCharacterDataCollection value, DataContext context)
	{
		_characterSecretInformation[elementId] = value;
		_modificationsCharacterSecretInformation.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(18, 4, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(18, 4, elementId, 0);
		}
	}

	private void RemoveElement_CharacterSecretInformation(int elementId, DataContext context)
	{
		_characterSecretInformation.Remove(elementId);
		_modificationsCharacterSecretInformation.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(18, 4, elementId);
	}

	private void ClearCharacterSecretInformation(DataContext context)
	{
		_characterSecretInformation.Clear();
		_modificationsCharacterSecretInformation.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(18, 4);
	}

	public List<int> GetBroadcastSecretInformation()
	{
		return _broadcastSecretInformation;
	}

	public unsafe void SetBroadcastSecretInformation(List<int> value, DataContext context)
	{
		_broadcastSecretInformation = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		int count = _broadcastSecretInformation.Count;
		int num = 4 * count;
		int valueSize = 2 + num;
		byte* ptr = OperationAdder.DynamicSingleValue_Set(18, 5, valueSize);
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			((int*)ptr)[i] = _broadcastSecretInformation[i];
		}
		ptr += num;
	}

	public List<NormalInformation> GetTaiwuReceivedNormalInformationInMonth()
	{
		return _taiwuReceivedNormalInformationInMonth;
	}

	public unsafe void SetTaiwuReceivedNormalInformationInMonth(List<NormalInformation> value, DataContext context)
	{
		_taiwuReceivedNormalInformationInMonth = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		int count = _taiwuReceivedNormalInformationInMonth.Count;
		int num = 3 * count;
		int valueSize = 2 + num;
		byte* ptr = OperationAdder.DynamicSingleValue_Set(18, 6, valueSize);
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			ptr += _taiwuReceivedNormalInformationInMonth[i].Serialize(ptr);
		}
	}

	public List<int> GetTaiwuReceivedSecretInformationInMonth()
	{
		return _taiwuReceivedSecretInformationInMonth;
	}

	public unsafe void SetTaiwuReceivedSecretInformationInMonth(List<int> value, DataContext context)
	{
		_taiwuReceivedSecretInformationInMonth = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		int count = _taiwuReceivedSecretInformationInMonth.Count;
		int num = 4 * count;
		int valueSize = 2 + num;
		byte* ptr = OperationAdder.DynamicSingleValue_Set(18, 7, valueSize);
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			((int*)ptr)[i] = _taiwuReceivedSecretInformationInMonth[i];
		}
		ptr += num;
	}

	public List<int> GetTaiwuReceivedInformation()
	{
		return _taiwuReceivedInformation;
	}

	public void SetTaiwuReceivedInformation(List<int> value, DataContext context)
	{
		_taiwuReceivedInformation = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
	}

	public List<NormalInformation> GetTaiwuTmpInformation()
	{
		return _taiwuTmpInformation;
	}

	public void SetTaiwuTmpInformation(List<NormalInformation> value, DataContext context)
	{
		_taiwuTmpInformation = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		foreach (KeyValuePair<int, NormalInformationCollection> item in _information)
		{
			int key = item.Key;
			NormalInformationCollection value = item.Value;
			if (value != null)
			{
				int serializedSize = value.GetSerializedSize();
				byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(18, 0, key, serializedSize);
				ptr += value.Serialize(ptr);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(18, 0, key, 0);
			}
		}
		int size = _secretInformationCollection.GetSize();
		byte* pDest = OperationAdder.Binary_Write(18, 1, 0, size);
		_secretInformationCollection.CopyTo(0, size, pDest);
		ushort serializedFixedSizeOfMetadata = _secretInformationCollection.GetSerializedFixedSizeOfMetadata();
		byte* pData = OperationAdder.Binary_SetMetadata(18, 1, serializedFixedSizeOfMetadata);
		_secretInformationCollection.SerializeMetadata(pData);
		foreach (KeyValuePair<int, SecretInformationMetaData> secretInformationMetaDatum in _secretInformationMetaData)
		{
			int key2 = secretInformationMetaDatum.Key;
			SecretInformationMetaData value2 = secretInformationMetaDatum.Value;
			byte* pData2 = OperationAdder.DynamicObjectCollection_Add(18, 2, key2, value2.GetSerializedSize());
			value2.Serialize(pData2);
		}
		byte* ptr2 = OperationAdder.FixedSingleValue_Set(18, 3, 4);
		*(int*)ptr2 = _nextMetaDataId;
		ptr2 += 4;
		foreach (KeyValuePair<int, SecretInformationCharacterDataCollection> item2 in _characterSecretInformation)
		{
			int key3 = item2.Key;
			SecretInformationCharacterDataCollection value3 = item2.Value;
			if (value3 != null)
			{
				int serializedSize2 = value3.GetSerializedSize();
				byte* ptr3 = OperationAdder.DynamicSingleValueCollection_Add(18, 4, key3, serializedSize2);
				ptr3 += value3.Serialize(ptr3);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(18, 4, key3, 0);
			}
		}
		int count = _broadcastSecretInformation.Count;
		int num = 4 * count;
		int valueSize = 2 + num;
		byte* ptr4 = OperationAdder.DynamicSingleValue_Set(18, 5, valueSize);
		*(ushort*)ptr4 = (ushort)count;
		ptr4 += 2;
		for (int i = 0; i < count; i++)
		{
			((int*)ptr4)[i] = _broadcastSecretInformation[i];
		}
		ptr4 += num;
		int count2 = _taiwuReceivedNormalInformationInMonth.Count;
		int num2 = 3 * count2;
		int valueSize2 = 2 + num2;
		byte* ptr5 = OperationAdder.DynamicSingleValue_Set(18, 6, valueSize2);
		*(ushort*)ptr5 = (ushort)count2;
		ptr5 += 2;
		for (int j = 0; j < count2; j++)
		{
			ptr5 += _taiwuReceivedNormalInformationInMonth[j].Serialize(ptr5);
		}
		int count3 = _taiwuReceivedSecretInformationInMonth.Count;
		int num3 = 4 * count3;
		int valueSize3 = 2 + num3;
		byte* ptr6 = OperationAdder.DynamicSingleValue_Set(18, 7, valueSize3);
		*(ushort*)ptr6 = (ushort)count3;
		ptr6 += 2;
		for (int k = 0; k < count3; k++)
		{
			((int*)ptr6)[k] = _taiwuReceivedSecretInformationInMonth[k];
		}
		ptr6 += num3;
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(18, 0));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.Binary_Get(18, 1));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicObjectCollection_GetAllObjects(18, 2));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(18, 3));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(18, 4));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(18, 5));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(18, 6));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(18, 7));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 0);
				_modificationsInformation.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, NormalInformationCollection>)_information, dataPool);
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
				_modificationsSecretInformationCollection.Reset(_secretInformationCollection.GetSize());
			}
			return GameData.Serializer.Serializer.SerializeModifications((IBinary)(object)_secretInformationCollection, dataPool);
		case 2:
			return GetElementField_SecretInformationMetaData((int)subId0, (ushort)subId1, dataPool, resetModified);
		case 3:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 4:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
				_modificationsCharacterSecretInformation.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, SecretInformationCharacterDataCollection>)_characterSecretInformation, dataPool);
		case 5:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			return GameData.Serializer.Serializer.Serialize(_broadcastSecretInformation, dataPool);
		case 6:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuReceivedNormalInformationInMonth, dataPool);
		case 7:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuReceivedSecretInformationInMonth, dataPool);
		case 8:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuReceivedInformation, dataPool);
		case 9:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuTmpInformation, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 2:
			SetElementField_SecretInformationMetaData((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 3:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 4:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 5:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _broadcastSecretInformation);
			SetBroadcastSecretInformation(_broadcastSecretInformation, context);
			break;
		case 6:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuReceivedNormalInformationInMonth);
			SetTaiwuReceivedNormalInformationInMonth(_taiwuReceivedNormalInformationInMonth, context);
			break;
		case 7:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuReceivedSecretInformationInMonth);
			SetTaiwuReceivedSecretInformationInMonth(_taiwuReceivedSecretInformationInMonth, context);
			break;
		case 8:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuReceivedInformation);
			SetTaiwuReceivedInformation(_taiwuReceivedInformation, context);
			break;
		case 9:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuTmpInformation);
			SetTaiwuTmpInformation(_taiwuTmpInformation, context);
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
	{
		int argsOffset = operation.ArgsOffset;
		switch (operation.MethodId)
		{
		case 0:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 1)
			{
				int item26 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				NormalInformationCollection characterNormalInformation = GetCharacterNormalInformation(item26);
				return GameData.Serializer.Serializer.Serialize(characterNormalInformation, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 2)
			{
				int item16 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				NormalInformation item17 = default(NormalInformation);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				AddNormalInformationToCharacter(context, item16, item17);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
			if (operation.ArgsCount == 0)
			{
				DeleteTmpInformation(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 3:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 2)
			{
				int item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				NormalInformation item7 = default(NormalInformation);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				int normalInformationUsedCount = GetNormalInformationUsedCount(item6, item7);
				return GameData.Serializer.Serializer.Serialize(normalInformationUsedCount, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 4:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 1)
			{
				List<int> item22 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				SecretInformationDisplayPackage secretInformationDisplayPackage = GetSecretInformationDisplayPackage(item22);
				return GameData.Serializer.Serializer.Serialize(secretInformationDisplayPackage, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 1)
			{
				int item5 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				SecretInformationDisplayPackage secretInformationDisplayPackageFromCharacter = GetSecretInformationDisplayPackageFromCharacter(item5);
				return GameData.Serializer.Serializer.Serialize(secretInformationDisplayPackageFromCharacter, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 6:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 1)
			{
				int item23 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				SecretInformationDisplayPackage secretInformationDisplayPackageFromBroadcast = GetSecretInformationDisplayPackageFromBroadcast(item23);
				return GameData.Serializer.Serializer.Serialize(secretInformationDisplayPackageFromBroadcast, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 7:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 1)
			{
				int item13 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				SecretInformationDisplayPackage secretInformationDisplayPackageForSelections = GetSecretInformationDisplayPackageForSelections(item13);
				return GameData.Serializer.Serializer.Serialize(secretInformationDisplayPackageForSelections, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 8:
		{
			int argsCount17 = operation.ArgsCount;
			int num17 = argsCount17;
			if (num17 == 2)
			{
				int item31 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item31);
				int item32 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				DiscardSecretInformation(context, item31, item32);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 9:
		{
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 1)
			{
				int item25 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item25);
				SecretInformationDisplayData secretInformationDisplayData = GetSecretInformationDisplayData(item25);
				return GameData.Serializer.Serializer.Serialize(secretInformationDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 2)
			{
				string item18 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				List<int> item19 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item19);
				int item20 = GmCmd_CreateSecretInformationByCharacterIds(context, item18, item19);
				return GameData.Serializer.Serializer.Serialize(item20, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 11:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 2)
			{
				int item10 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				int item11 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				bool item12 = GmCmd_MakeCharacterReceiveSecretInformation(context, item10, item11);
				return GameData.Serializer.Serializer.Serialize(item12, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 12:
		{
			int argsCount18 = operation.ArgsCount;
			int num18 = argsCount18;
			if (num18 == 3)
			{
				int item33 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item33);
				int item34 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item34);
				int item35 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item35);
				bool item36 = DisseminateSecretInformation(context, item33, item34, item35);
				return GameData.Serializer.Serializer.Serialize(item36, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 13:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 1)
			{
				List<int> item30 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item30);
				List<CharacterDisplayDataWithInfo> characterDisplayDataWithInfoList = GetCharacterDisplayDataWithInfoList(item30);
				return GameData.Serializer.Serializer.Serialize(characterDisplayDataWithInfoList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 14:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				int item29 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item29);
				GmCmd_MakeSecretInformationBroadcast(context, item29);
				return -1;
			}
			case 2:
			{
				int item27 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item27);
				int item28 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item28);
				GmCmd_MakeSecretInformationBroadcast(context, item27, item28);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 15:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 1)
			{
				NormalInformation item24 = default(NormalInformation);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				PerformProfessionLiteratiSkill3(context, item24);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 16:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 1)
			{
				int item21 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				PerformProfessionLiteratiSkill2(context, item21);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 17:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 2)
			{
				int item14 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				NormalInformation item15 = default(NormalInformation);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				IntPair normalInformationUsedCountAndMax = GetNormalInformationUsedCountAndMax(item14, item15);
				return GameData.Serializer.Serializer.Serialize(normalInformationUsedCountAndMax, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 18:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 2)
			{
				List<IntPair> item8 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				int item9 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				SettleSecretInformationShopTrade(context, item8, item9);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 19:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 3)
			{
				int item = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				int item2 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				int item3 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				int item4 = GmCmd_DisseminationSecretInformationToRandomCharacters(context, item, item2, item3);
				return GameData.Serializer.Serializer.Serialize(item4, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
	{
		switch (dataId)
		{
		case 0:
			_modificationsInformation.ChangeRecording(monitoring);
			break;
		case 1:
			_modificationsSecretInformationCollection.ChangeRecording(monitoring, _secretInformationCollection.GetSize());
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			_modificationsCharacterSecretInformation.ChangeRecording(monitoring);
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		case 9:
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 0);
			int result2 = GameData.Serializer.Serializer.SerializeModifications(_information, dataPool, _modificationsInformation);
			_modificationsInformation.Reset();
			return result2;
		}
		case 1:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			int result = GameData.Serializer.Serializer.SerializeModifications((IBinary)(object)_secretInformationCollection, dataPool, _modificationsSecretInformationCollection);
			_modificationsSecretInformationCollection.Reset(_secretInformationCollection.GetSize());
			return result;
		}
		case 2:
			return CheckModified_SecretInformationMetaData((int)subId0, (ushort)subId1, dataPool);
		case 3:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 4:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 4))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 4);
			int result3 = GameData.Serializer.Serializer.SerializeModifications(_characterSecretInformation, dataPool, _modificationsCharacterSecretInformation);
			_modificationsCharacterSecretInformation.Reset();
			return result3;
		}
		case 5:
			if (!BaseGameDataDomain.IsModified(DataStates, 5))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 5);
			return GameData.Serializer.Serializer.Serialize(_broadcastSecretInformation, dataPool);
		case 6:
			if (!BaseGameDataDomain.IsModified(DataStates, 6))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 6);
			return GameData.Serializer.Serializer.Serialize(_taiwuReceivedNormalInformationInMonth, dataPool);
		case 7:
			if (!BaseGameDataDomain.IsModified(DataStates, 7))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 7);
			return GameData.Serializer.Serializer.Serialize(_taiwuReceivedSecretInformationInMonth, dataPool);
		case 8:
			if (!BaseGameDataDomain.IsModified(DataStates, 8))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 8);
			return GameData.Serializer.Serializer.Serialize(_taiwuReceivedInformation, dataPool);
		case 9:
			if (!BaseGameDataDomain.IsModified(DataStates, 9))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 9);
			return GameData.Serializer.Serializer.Serialize(_taiwuTmpInformation, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			if (BaseGameDataDomain.IsModified(DataStates, 0))
			{
				BaseGameDataDomain.ResetModified(DataStates, 0);
				_modificationsInformation.Reset();
			}
			break;
		case 1:
			if (BaseGameDataDomain.IsModified(DataStates, 1))
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			break;
		case 2:
			ResetModifiedWrapper_SecretInformationMetaData((int)subId0, (ushort)subId1);
			break;
		case 3:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 4:
			if (BaseGameDataDomain.IsModified(DataStates, 4))
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
				_modificationsCharacterSecretInformation.Reset();
			}
			break;
		case 5:
			if (BaseGameDataDomain.IsModified(DataStates, 5))
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			break;
		case 6:
			if (BaseGameDataDomain.IsModified(DataStates, 6))
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
			}
			break;
		case 7:
			if (BaseGameDataDomain.IsModified(DataStates, 7))
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			break;
		case 8:
			if (BaseGameDataDomain.IsModified(DataStates, 8))
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
			}
			break;
		case 9:
			if (BaseGameDataDomain.IsModified(DataStates, 9))
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
			}
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		return dataId switch
		{
			0 => BaseGameDataDomain.IsModified(DataStates, 0), 
			1 => BaseGameDataDomain.IsModified(DataStates, 1), 
			2 => IsModifiedWrapper_SecretInformationMetaData((int)subId0, (ushort)subId1), 
			3 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			4 => BaseGameDataDomain.IsModified(DataStates, 4), 
			5 => BaseGameDataDomain.IsModified(DataStates, 5), 
			6 => BaseGameDataDomain.IsModified(DataStates, 6), 
			7 => BaseGameDataDomain.IsModified(DataStates, 7), 
			8 => BaseGameDataDomain.IsModified(DataStates, 8), 
			9 => BaseGameDataDomain.IsModified(DataStates, 9), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		case 2:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _secretInformationMetaData, list))
				{
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						BaseGameDataObject baseGameDataObject = list[i];
						List<DataUid> targetUids = influence.TargetUids;
						int count2 = targetUids.Count;
						for (int j = 0; j < count2; j++)
						{
							baseGameDataObject.InvalidateSelfAndInfluencedCache((ushort)targetUids[j].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesSecretInformationMetaData, _dataStatesSecretInformationMetaData, influence, context);
				}
				list.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesSecretInformationMetaData, _dataStatesSecretInformationMetaData, influence, context);
			}
			break;
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 0:
		case 1:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		uint num;
		switch (operation.DataId)
		{
		case 0:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _information);
			goto IL_0105;
		case 1:
			ResponseProcessor.ProcessBinary(operation, pResult, (IBinary)(object)_secretInformationCollection);
			goto IL_0105;
		case 2:
			ResponseProcessor.ProcessDynamicObjectCollection(operation, pResult, _secretInformationMetaData);
			goto IL_0105;
		case 3:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _nextMetaDataId);
			goto IL_0105;
		case 4:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _characterSecretInformation);
			goto IL_0105;
		case 5:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List(operation, pResult, _broadcastSecretInformation);
			goto IL_0105;
		case 6:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_List<NormalInformation>(operation, pResult, _taiwuReceivedNormalInformationInMonth, 3);
			goto IL_0105;
		case 7:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List(operation, pResult, _taiwuReceivedSecretInformationInMonth);
			goto IL_0105;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 8:
		case 9:
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_0105:
			if (_pendingLoadingOperationIds == null)
			{
				break;
			}
			num = _pendingLoadingOperationIds.Peek();
			if (num == operation.Id)
			{
				_pendingLoadingOperationIds.Dequeue();
				if (_pendingLoadingOperationIds.Count <= 0)
				{
					_pendingLoadingOperationIds = null;
					InitializeInternalDataOfCollections();
					OnLoadedArchiveData();
					DomainManager.Global.CompleteLoading(18);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
		foreach (KeyValuePair<int, SecretInformationMetaData> secretInformationMetaDatum in _secretInformationMetaData)
		{
			SecretInformationMetaData value = secretInformationMetaDatum.Value;
			value.CollectionHelperData = HelperDataSecretInformationMetaData;
			value.DataStatesOffset = _dataStatesSecretInformationMetaData.Create();
		}
	}

	public static bool CheckTuringTest(GameData.Domains.Character.Character character)
	{
		if (character == null)
		{
			return false;
		}
		return character.GetCreatingType() == 1 && character.GetAgeGroup() > 0;
	}

	public static bool CheckTuringTest(int charId, out GameData.Domains.Character.Character character)
	{
		return DomainManager.Character.TryGetElement_Objects(charId, out character) && CheckTuringTest(character);
	}

	public static List<GameData.Domains.Character.Character> GetTuringTestPassedCharacters(IEnumerable collection)
	{
		List<GameData.Domains.Character.Character> list = new List<GameData.Domains.Character.Character>();
		foreach (int item in collection)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && CheckTuringTest(element))
			{
				list.Add(element);
			}
		}
		return list;
	}
}
