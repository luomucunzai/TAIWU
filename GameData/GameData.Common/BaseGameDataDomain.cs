using System;
using System.Collections.Generic;
using GameData.ArchiveData;
using GameData.Dependencies;
using GameData.Domains;
using GameData.Domains.Global;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Common;

public abstract class BaseGameDataDomain
{
	protected readonly byte[] DataStates;

	protected readonly Dictionary<DataUid, DataModificationHandlerGroup> PostModificationDataHandlers = new Dictionary<DataUid, DataModificationHandlerGroup>();

	protected BaseGameDataDomain(int fieldsCount)
	{
		DataStates = new byte[(fieldsCount + 3) / 4];
	}

	public abstract void OnInitializeGameDataModule();

	public abstract void OnEnterNewWorld();

	public abstract void OnLoadWorld();

	public virtual void OnUpdate(DataContext context)
	{
	}

	public virtual void FixAbnormalDomainArchiveData(DataContext context)
	{
	}

	public virtual void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
	{
	}

	public virtual void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
	{
	}

	public virtual void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
	}

	public abstract int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified);

	public abstract void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context);

	public abstract int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context);

	public abstract void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring);

	public abstract int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool);

	public abstract void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1);

	public abstract bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1);

	public abstract void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll);

	public unsafe abstract void ProcessArchiveResponse(OperationWrapper operation, byte* pResult);

	public static bool IsCached(byte[] dataStates, int fieldId)
	{
		return (dataStates[fieldId / 4] & (1 << fieldId % 4 * 2)) != 0;
	}

	public static void SetCached(byte[] dataStates, int fieldId)
	{
		dataStates[fieldId / 4] |= (byte)(1 << fieldId % 4 * 2);
	}

	public static void ResetCached(byte[] dataStates, int fieldId)
	{
		dataStates[fieldId / 4] &= (byte)(~(1 << fieldId % 4 * 2));
	}

	public static bool IsModified(byte[] dataStates, int fieldId)
	{
		return (dataStates[fieldId / 4] & (2 << fieldId % 4 * 2)) != 0;
	}

	public static void SetModified(byte[] dataStates, int fieldId)
	{
		dataStates[fieldId / 4] |= (byte)(2 << fieldId % 4 * 2);
	}

	public static void ResetModified(byte[] dataStates, int fieldId)
	{
		dataStates[fieldId / 4] &= (byte)(~(2 << fieldId % 4 * 2));
	}

	public void ExecutePostModificationHandlers(DataContext context, DataUid uid)
	{
		if (PostModificationDataHandlers.TryGetValue(uid, out var value))
		{
			value.ExecuteAll(context, uid);
		}
	}

	public void AddPostModificationHandler(DataUid uid, string handlerKey, Action<DataContext, DataUid> handler)
	{
		if (!PostModificationDataHandlers.TryGetValue(uid, out var value))
		{
			value = new DataModificationHandlerGroup();
			PostModificationDataHandlers.Add(uid, value);
		}
		value.RegisterHandler(handlerKey, handler);
	}

	public bool RemovePostModificationHandler(DataUid uid, string handlerKey)
	{
		if (PostModificationDataHandlers.TryGetValue(uid, out var value))
		{
			value.UnregisterHandler(handlerKey);
			return value.Count == 0;
		}
		return true;
	}

	public void RemovePostModificationHandlers(DataUid uid)
	{
		PostModificationDataHandlers.Remove(uid);
	}

	protected static void SetModifiedAndInvalidateInfluencedCache(int dataId, byte[] dataStates, DataInfluence[][] cacheInfluences, DataContext context)
	{
		SetModified(dataStates, dataId);
		DataInfluence[] array = cacheInfluences[dataId];
		if (array != null)
		{
			Tester.Assert(context != null);
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				DataInfluence dataInfluence = array[i];
				BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[dataInfluence.TargetIndicator.DomainId];
				baseGameDataDomain.InvalidateCache(null, dataInfluence, context, unconditionallyInfluenceAll: false);
			}
		}
	}

	protected static void InvalidateSelfAndInfluencedCache(int dataId, byte[] dataStates, DataInfluence[][] cacheInfluences, DataContext context)
	{
		int num = dataId / 4;
		int num2 = dataId % 4 * 2;
		int num3 = (dataStates[num] & ~(3 << num2)) | (2 << num2);
		dataStates[num] = (byte)num3;
		DataInfluence[] array = cacheInfluences[dataId];
		if (array != null)
		{
			Tester.Assert(context != null);
			int num4 = array.Length;
			for (int i = 0; i < num4; i++)
			{
				DataInfluence dataInfluence = array[i];
				BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[dataInfluence.TargetIndicator.DomainId];
				baseGameDataDomain.InvalidateCache(null, dataInfluence, context, unconditionallyInfluenceAll: false);
			}
		}
	}

	protected static void InvalidateAllAndInfluencedCaches(DataInfluence[][] cacheInfluences, ObjectCollectionDataStates dataStates, DataInfluence influence, DataContext context)
	{
		Tester.Assert(context != null);
		dataStates.InvalidateAll(influence);
		List<DataUid> targetUids = influence.TargetUids;
		int count = targetUids.Count;
		for (int i = 0; i < count; i++)
		{
			ushort num = (ushort)targetUids[i].SubId1;
			DataInfluence[] array = cacheInfluences[num];
			if (array != null)
			{
				int num2 = array.Length;
				for (int j = 0; j < num2; j++)
				{
					DataInfluence dataInfluence = array[j];
					BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[dataInfluence.TargetIndicator.DomainId];
					baseGameDataDomain.InvalidateCache(null, dataInfluence, context, unconditionallyInfluenceAll: true);
				}
			}
		}
	}
}
