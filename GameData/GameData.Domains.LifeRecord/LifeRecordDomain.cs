using System;
using System.Collections.Generic;
using System.Reflection;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Character;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.LifeRecord;

[GameDataDomain(13, CustomArchiveModuleCode = true)]
public class LifeRecordDomain : BaseGameDataDomain
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.SingleValue, false, false, false, false)]
	private int _lifeRecord;

	private readonly LifeRecordCollection _currLifeRecords = new LifeRecordCollection();

	private static Action<ReadonlyLifeRecords> _onReceiveCharacterLifeRecords;

	private readonly List<short> _sourceRecordTemplateIds = new List<short>();

	private readonly Dictionary<short, string> _templateId2Name = new Dictionary<short, string>();

	private Type _lifeRecordCollectionType;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[1][];

	private Queue<uint> _pendingLoadingOperationIds;

	private void OnInitializedDomainData()
	{
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

	private unsafe void ProcessLifeRecordArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			ResponseProcessor.LifeRecord_Get(pResult, out var records2);
			GameData.GameDataBridge.GameDataBridge.TryReturnPassthroughMethod(operation.Id, records2);
			break;
		}
		case 2:
		{
			ResponseProcessor.LifeRecord_GetByDate(pResult, out var records3);
			GameData.GameDataBridge.GameDataBridge.TryReturnPassthroughMethod(operation.Id, records3);
			break;
		}
		case 3:
		{
			ResponseProcessor.LifeRecord_GetLast(pResult, out var records5);
			GameData.GameDataBridge.GameDataBridge.TryReturnPassthroughMethod(operation.Id, records5);
			break;
		}
		case 4:
		{
			ResponseProcessor.LifeRecord_Search(pResult, out var indexes);
			GameData.GameDataBridge.GameDataBridge.TryReturnPassthroughMethod(operation.Id, indexes);
			break;
		}
		case 5:
			break;
		case 6:
			break;
		case 7:
		{
			ResponseProcessor.LifeRecord_GetDead(pResult, out var records4);
			GameData.GameDataBridge.GameDataBridge.TryReturnPassthroughMethod(operation.Id, records4);
			break;
		}
		case 8:
			break;
		case 11:
		{
			ResponseProcessor.LifeRecord_GetAllByChar(pResult, out var records);
			_onReceiveCharacterLifeRecords?.Invoke(records);
			_onReceiveCharacterLifeRecords = null;
			break;
		}
		default:
			throw new Exception($"Unsupported LifeRecordMethodId: {operation.MethodId}");
		}
	}

	public void Add(LifeRecordCollection collection)
	{
		OperationAdder.LifeRecord_Add(collection);
	}

	[DomainMethod(IsPassthrough = true)]
	public void Get(uint operationId, int charId, int beginIndex, int count)
	{
		OperationAdder.LifeRecord_Get(charId, beginIndex, count, isPassthrough: true, operationId);
	}

	[DomainMethod(IsPassthrough = true)]
	public void GetByDate(uint operationId, int charId, int startDate, int monthCount)
	{
		int currDate = DomainManager.World.GetCurrDate();
		OperationAdder.LifeRecord_GetByDate(charId, startDate, monthCount, currDate, isPassthrough: true, operationId);
	}

	[DomainMethod(IsPassthrough = true)]
	public void GetLast(uint operationId, int charId, int count)
	{
		OperationAdder.LifeRecord_GetLast(charId, count, isPassthrough: true, operationId);
	}

	[DomainMethod(IsPassthrough = true)]
	public void GetRelated(uint operationId, int charId, int date, short recordType, int relatedCharId)
	{
		List<short> relatedIds = Config.LifeRecord.Instance[recordType].RelatedIds;
		if (relatedIds.Count <= 0)
		{
			throw new Exception($"Record type {recordType} does not have related records.");
		}
		OperationAdder.LifeRecord_Search(relatedCharId, date, relatedIds, charId, isPassthrough: true, operationId);
	}

	public void Remove(int charId)
	{
		OperationAdder.LifeRecord_Remove(charId);
	}

	public void GenerateDead(int charId)
	{
		OperationAdder.LifeRecord_GenerateDead(charId);
	}

	[DomainMethod(IsPassthrough = true)]
	public void GetDead(uint operationId, int charId)
	{
		OperationAdder.LifeRecord_GetDead(charId, isPassthrough: true, operationId);
	}

	public void RemoveDead(int charId)
	{
		OperationAdder.LifeRecord_RemoveDead(charId);
	}

	public void GetTaiwuLifeRecords()
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		OperationAdder.LifeRecord_GetAllByChar(taiwuCharId, isPassthrough: false);
	}

	public LifeRecordCollection GetLifeRecordCollection()
	{
		return _currLifeRecords;
	}

	public void CommitCurrLifeRecords()
	{
		if (_currLifeRecords.Count > 0)
		{
			DomainManager.LifeRecord.Add(_currLifeRecords);
			_currLifeRecords.Clear();
		}
	}

	[DomainMethod]
	public ArgumentCollectionRenderArguments GetRecordRenderInfoArguments(DataContext context, string key, RecordArgumentsRequest request, bool isDreamBack = false)
	{
		ArgumentCollectionRenderArguments argumentCollectionRenderArguments = new ArgumentCollectionRenderArguments();
		argumentCollectionRenderArguments.Key = key;
		List<int> characters = request.Characters;
		if (characters != null && characters.Count > 0)
		{
			argumentCollectionRenderArguments.CharNameAndLifeDataList = (isDreamBack ? DomainManager.Extra.GetNameAndLifeRelatedDataListForDreamBack(request.Characters) : DomainManager.Character.GetNameAndLifeRelatedDataList(request.Characters));
		}
		List<Location> locations = request.Locations;
		if (locations != null && locations.Count > 0)
		{
			argumentCollectionRenderArguments.LocationNames = DomainManager.Map.GetLocationNameRelatedDataList(request.Locations);
		}
		List<short> settlements = request.Settlements;
		if (settlements != null && settlements.Count > 0)
		{
			argumentCollectionRenderArguments.SettlementNames = DomainManager.Organization.GetSettlementNameRelatedData(request.Settlements);
		}
		characters = request.JiaoLoongs;
		if (characters != null && characters.Count > 0)
		{
			argumentCollectionRenderArguments.JiaoLoongNames = DomainManager.Extra.GetJiaoLoongNameRelatedDataList(request.JiaoLoongs);
		}
		return argumentCollectionRenderArguments;
	}

	public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
	{
		_onReceiveCharacterLifeRecords = (Action<ReadonlyLifeRecords>)Delegate.Combine(_onReceiveCharacterLifeRecords, (Action<ReadonlyLifeRecords>)delegate(ReadonlyLifeRecords lifeRecords)
		{
			crossArchiveGameData.LifeRecords = lifeRecords;
		});
		GetTaiwuLifeRecords();
	}

	public void InitializeTestRelatedData()
	{
		_sourceRecordTemplateIds.Clear();
		foreach (LifeRecordItem item in (IEnumerable<LifeRecordItem>)Config.LifeRecord.Instance)
		{
			if (item.IsSourceRecord)
			{
				_sourceRecordTemplateIds.Add(item.TemplateId);
			}
		}
		_templateId2Name.Clear();
		Type type = Type.GetType("Config.LifeRecord+DefKey");
		Tester.Assert(type != null);
		FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			string name = fieldInfo.Name;
			short key = (short)fieldInfo.GetValue(null);
			_templateId2Name.Add(key, name);
		}
		_lifeRecordCollectionType = Type.GetType("GameData.Domains.LifeRecord.LifeRecordCollection");
	}

	public void AddRandomLifeRecord(DataContext context, LifeRecordCollection lifeRecords, GameData.Domains.Character.Character character, MapBlockData mapBlockData)
	{
		for (int i = 0; i < 3; i++)
		{
			if (AddRandomLifeRecordInternal(context, lifeRecords, character, mapBlockData))
			{
				break;
			}
		}
	}

	public void ShowLifeRecords(int charId, int beginIndex, int desiredCount, ReadonlyLifeRecordsWithTotalCount lifeRecords)
	{
		if (_lifeRecordCollectionType == null)
		{
			InitializeTestRelatedData();
		}
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		var (value, value2) = CharacterDomain.GetRealName(element_Objects);
		Logger.Info($"{value}{value2}: {lifeRecords.Records.Count}/{lifeRecords.TotalCount}");
		List<LifeRecordRenderInfo> list = new List<LifeRecordRenderInfo>();
		ArgumentCollection argumentCollection = new ArgumentCollection();
		lifeRecords.Records.GetRenderInfos(list, argumentCollection);
		foreach (LifeRecordRenderInfo item in list)
		{
			int num = item.Date / 12;
			int num2 = item.Date % 12;
			Logger.Info($"  {num + 1}年{num2 + 1}月: {item.Text}");
		}
	}

	public void ShowLifeRecords(int charId, int desiredCount, ReadonlyLifeRecords lifeRecords)
	{
		if (_lifeRecordCollectionType == null)
		{
			InitializeTestRelatedData();
		}
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		var (value, value2) = CharacterDomain.GetRealName(element_Objects);
		Logger.Info($"{value}{value2}: {lifeRecords.Count}/{desiredCount}");
		List<LifeRecordRenderInfo> list = new List<LifeRecordRenderInfo>();
		ArgumentCollection argumentCollection = new ArgumentCollection();
		lifeRecords.GetRenderInfos(list, argumentCollection);
		foreach (LifeRecordRenderInfo item in list)
		{
			int num = item.Date / 12;
			int num2 = item.Date % 12;
			Logger.Info($"  {num + 1}年{num2 + 1}月: {item.Text}");
		}
	}

	private bool AddRandomLifeRecordInternal(DataContext context, LifeRecordCollection lifeRecords, GameData.Domains.Character.Character character, MapBlockData mapBlockData)
	{
		int index = context.Random.Next(_sourceRecordTemplateIds.Count);
		short index2 = _sourceRecordTemplateIds[index];
		LifeRecordItem lifeRecordItem = Config.LifeRecord.Instance[index2];
		string text = _templateId2Name[lifeRecordItem.TemplateId];
		MethodInfo method = _lifeRecordCollectionType.GetMethod("Add" + text);
		Tester.Assert(method != null);
		List<object> list = new List<object>
		{
			character.GetId(),
			DomainManager.World.GetCurrDate()
		};
		int i = 0;
		for (int num = lifeRecordItem.Parameters.Length; i < num; i++)
		{
			string text2 = lifeRecordItem.Parameters[i];
			if (string.IsNullOrEmpty(text2))
			{
				break;
			}
			sbyte paramType = ParameterType.Parse(text2);
			if (!AddArguments(context.Random, list, paramType, character, mapBlockData))
			{
				return false;
			}
		}
		if (lifeRecordItem.RelatedIds.Count > 1)
		{
			index = context.Random.Next(lifeRecordItem.RelatedIds.Count);
			short num2 = lifeRecordItem.RelatedIds[index];
			list.Add(num2);
		}
		method.Invoke(lifeRecords, list.ToArray());
		return true;
	}

	private static bool AddArguments(IRandomSource random, List<object> arguments, sbyte paramType, GameData.Domains.Character.Character character, MapBlockData mapBlockData)
	{
		switch (paramType)
		{
		case 0:
		{
			int otherCharacter2 = GetOtherCharacter(mapBlockData.CharacterSet, character.GetId());
			if (otherCharacter2 < 0)
			{
				return false;
			}
			arguments.Add(otherCharacter2);
			return true;
		}
		case 1:
		{
			Location location = character.GetLocation();
			if (!location.IsValid())
			{
				location = character.GetValidLocation();
			}
			arguments.Add(location);
			return true;
		}
		case 2:
			var (b9, num14) = GetItem(character);
			if (b9 < 0)
			{
				return false;
			}
			arguments.Add(b9);
			arguments.Add(num14);
			return true;
		case 3:
		{
			short num13 = (short)random.Next(0, Config.CombatSkill.Instance.Count);
			arguments.Add(num13);
			return true;
		}
		case 4:
		{
			sbyte b10 = (sbyte)random.Next(0, 8);
			arguments.Add(b10);
			return true;
		}
		case 5:
		{
			short settlementId = character.GetOrganizationInfo().SettlementId;
			if (settlementId < 0)
			{
				return false;
			}
			arguments.Add(settlementId);
			return true;
		}
		case 6:
		{
			OrganizationInfo organizationInfo = character.GetOrganizationInfo();
			arguments.Add(organizationInfo.OrgTemplateId);
			arguments.Add(organizationInfo.Grade);
			arguments.Add(organizationInfo.Principal);
			arguments.Add(character.GetGender());
			return true;
		}
		case 20:
		{
			if (mapBlockData.TemplateEnemyList == null || mapBlockData.TemplateEnemyList.Count == 0)
			{
				return false;
			}
			short templateId = mapBlockData.TemplateEnemyList[0].TemplateId;
			arguments.Add(templateId);
			return true;
		}
		case 21:
		{
			short num12 = (short)random.Next(CharacterFeature.Instance.Count);
			arguments.Add(num12);
			return true;
		}
		case 22:
		{
			int num11 = random.Next(100);
			arguments.Add(num11);
			return true;
		}
		case 23:
		{
			short num10 = (short)random.Next(LifeSkill.Instance.Count);
			arguments.Add(num10);
			return true;
		}
		case 24:
		{
			sbyte b8 = (sbyte)random.Next(7);
			arguments.Add(b8);
			return true;
		}
		case 26:
		{
			sbyte b7 = (sbyte)random.Next(4);
			arguments.Add(b7);
			return true;
		}
		case 27:
		{
			sbyte b6 = (sbyte)random.Next(16);
			arguments.Add(b6);
			return true;
		}
		case 28:
		{
			sbyte b5 = (sbyte)random.Next(14);
			arguments.Add(b5);
			return true;
		}
		case 29:
		{
			short num9 = (short)random.Next(Config.Information.Instance.Count);
			arguments.Add(num9);
			return true;
		}
		case 30:
		{
			short num8 = (short)random.Next(SecretInformation.Instance.Count);
			arguments.Add(num8);
			return true;
		}
		case 31:
		{
			short num7 = (short)random.Next(PunishmentType.Instance.Count);
			arguments.Add(num7);
			return true;
		}
		case 32:
		{
			short num6 = (short)random.Next(CharacterTitle.Instance.Count);
			arguments.Add(num6);
			return true;
		}
		case 33:
		{
			float num5 = random.NextFloat();
			arguments.Add(num5);
			return true;
		}
		case 34:
		{
			int otherCharacter = GetOtherCharacter(mapBlockData.CharacterSet, character.GetId());
			if (otherCharacter < 0)
			{
				return false;
			}
			arguments.Add(otherCharacter);
			return true;
		}
		case 35:
		{
			sbyte b4 = (sbyte)random.Next(Month.Instance.Count);
			arguments.Add(b4);
			return true;
		}
		case 36:
		{
			int num4 = random.Next(Profession.Instance.Count);
			arguments.Add(num4);
			return true;
		}
		case 37:
		{
			int num3 = random.Next(ProfessionSkill.Instance.Count);
			arguments.Add(num3);
			return true;
		}
		case 38:
		{
			sbyte b3 = (sbyte)random.Next(9);
			arguments.Add(b3);
			return true;
		}
		case 39:
			return false;
		case 40:
		{
			short num2 = (short)random.Next(Music.Instance.Count);
			arguments.Add(num2);
			return true;
		}
		case 41:
		{
			sbyte b2 = (sbyte)random.Next(MapState.Instance.Count);
			arguments.Add(b2);
			return true;
		}
		case 42:
			return false;
		case 43:
		{
			short num = (short)random.Next(JiaoProperty.Instance.Count);
			arguments.Add(num);
			return false;
		}
		case 44:
		{
			sbyte b = (sbyte)random.Next(DestinyType.Instance.Count);
			arguments.Add(b);
			return true;
		}
		case 45:
		{
			short item = (short)random.Next(SecretInformation.Instance.Count);
			arguments.Add((item, -1));
			return true;
		}
		default:
			return false;
		}
	}

	private static int GetOtherCharacter(HashSet<int> charIds, int selfCharId)
	{
		if (charIds == null)
		{
			return -1;
		}
		foreach (int charId in charIds)
		{
			if (charId != selfCharId)
			{
				return charId;
			}
		}
		return -1;
	}

	private static (sbyte itemType, short itemTemplateId) GetItem(GameData.Domains.Character.Character character)
	{
		Inventory inventory = character.GetInventory();
		if (inventory.Items.Count > 0)
		{
			using (Dictionary<ItemKey, int>.Enumerator enumerator = inventory.Items.GetEnumerator())
			{
				enumerator.MoveNext();
				ItemKey key = enumerator.Current.Key;
				return (itemType: key.ItemType, itemTemplateId: key.TemplateId);
			}
		}
		ItemKey[] equipment = character.GetEquipment();
		for (int i = 0; i < 12; i++)
		{
			ItemKey itemKey = equipment[i];
			if (itemKey.IsValid())
			{
				return (itemType: itemKey.ItemType, itemTemplateId: itemKey.TemplateId);
			}
		}
		return (itemType: -1, itemTemplateId: -1);
	}

	public LifeRecordDomain()
		: base(1)
	{
		_lifeRecord = 0;
		OnInitializedDomainData();
	}

	private int GetLifeRecord()
	{
		return _lifeRecord;
	}

	private void SetLifeRecord(int value, DataContext context)
	{
		_lifeRecord = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
	}

	public override void OnLoadWorld()
	{
		InitializeInternalDataOfCollections();
		OnLoadedArchiveData();
		DomainManager.Global.CompleteLoading(13);
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		if (dataId == 0)
		{
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (dataId == 0)
		{
			throw new Exception($"Not allow to set value of dataId {dataId}");
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
	{
		int argsOffset = operation.ArgsOffset;
		switch (operation.MethodId)
		{
		case 0:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 3)
			{
				int item7 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				int item8 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				int item9 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				uint operationId2 = GameData.GameDataBridge.GameDataBridge.RecordPassthroughMethod(operation);
				Get(operationId2, item7, item8, item9);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 3)
			{
				int item16 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				int item17 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				int item18 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				uint operationId5 = GameData.GameDataBridge.GameDataBridge.RecordPassthroughMethod(operation);
				GetByDate(operationId5, item16, item17, item18);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 2)
			{
				int item10 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				int item11 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				uint operationId3 = GameData.GameDataBridge.GameDataBridge.RecordPassthroughMethod(operation);
				GetLast(operationId3, item10, item11);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 4)
			{
				int item12 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				int item13 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				short item14 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				int item15 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				uint operationId4 = GameData.GameDataBridge.GameDataBridge.RecordPassthroughMethod(operation);
				GetRelated(operationId4, item12, item13, item14, item15);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 4:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 1)
			{
				int item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				uint operationId = GameData.GameDataBridge.GameDataBridge.RecordPassthroughMethod(operation);
				GetDead(operationId, item6);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				string item4 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				RecordArgumentsRequest item5 = default(RecordArgumentsRequest);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				ArgumentCollectionRenderArguments recordRenderInfoArguments2 = GetRecordRenderInfoArguments(context, item4, item5);
				return GameData.Serializer.Serializer.Serialize(recordRenderInfoArguments2, returnDataPool);
			}
			case 3:
			{
				string item = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				RecordArgumentsRequest item2 = default(RecordArgumentsRequest);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				bool item3 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				ArgumentCollectionRenderArguments recordRenderInfoArguments = GetRecordRenderInfoArguments(context, item, item2, item3);
				return GameData.Serializer.Serializer.Serialize(recordRenderInfoArguments, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
	{
		if (dataId == 0)
		{
			return;
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		if (dataId == 0)
		{
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		if (dataId == 0)
		{
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		if (dataId == 0)
		{
			throw new Exception($"Not allow to verify modification state of dataId {dataId}");
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		if (influence.TargetIndicator.DataId != 0)
		{
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		}
		throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		if (operation.DataId == 0)
		{
			ProcessLifeRecordArchiveResponse(operation, pResult);
			if (_pendingLoadingOperationIds == null)
			{
				return;
			}
			uint num = _pendingLoadingOperationIds.Peek();
			if (num == operation.Id)
			{
				_pendingLoadingOperationIds.Dequeue();
				if (_pendingLoadingOperationIds.Count <= 0)
				{
					_pendingLoadingOperationIds = null;
					InitializeInternalDataOfCollections();
					OnLoadedArchiveData();
					DomainManager.Global.CompleteLoading(13);
				}
			}
			return;
		}
		throw new Exception($"Unsupported dataId {operation.DataId}");
	}

	private void InitializeInternalDataOfCollections()
	{
	}
}
