using System;
using System.Collections.Generic;
using GameData.ArchiveData;
using GameData.Dependencies;
using GameData.Domains;
using GameData.Domains.Global;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Common
{
	// Token: 0x020008ED RID: 2285
	public abstract class BaseGameDataDomain
	{
		// Token: 0x060081F5 RID: 33269 RVA: 0x004D9432 File Offset: 0x004D7632
		protected BaseGameDataDomain(int fieldsCount)
		{
			this.DataStates = new byte[(fieldsCount + 3) / 4];
		}

		// Token: 0x060081F6 RID: 33270
		public abstract void OnInitializeGameDataModule();

		// Token: 0x060081F7 RID: 33271
		public abstract void OnEnterNewWorld();

		// Token: 0x060081F8 RID: 33272
		public abstract void OnLoadWorld();

		// Token: 0x060081F9 RID: 33273 RVA: 0x004D9457 File Offset: 0x004D7657
		public virtual void OnUpdate(DataContext context)
		{
		}

		// Token: 0x060081FA RID: 33274 RVA: 0x004D945A File Offset: 0x004D765A
		public virtual void FixAbnormalDomainArchiveData(DataContext context)
		{
		}

		// Token: 0x060081FB RID: 33275 RVA: 0x004D945D File Offset: 0x004D765D
		public virtual void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
		{
		}

		// Token: 0x060081FC RID: 33276 RVA: 0x004D9460 File Offset: 0x004D7660
		public virtual void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
		{
		}

		// Token: 0x060081FD RID: 33277 RVA: 0x004D9463 File Offset: 0x004D7663
		public virtual void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
		}

		// Token: 0x060081FE RID: 33278
		public abstract int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified);

		// Token: 0x060081FF RID: 33279
		public abstract void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context);

		// Token: 0x06008200 RID: 33280
		public abstract int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context);

		// Token: 0x06008201 RID: 33281
		public abstract void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring);

		// Token: 0x06008202 RID: 33282
		public abstract int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool);

		// Token: 0x06008203 RID: 33283
		public abstract void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1);

		// Token: 0x06008204 RID: 33284
		public abstract bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1);

		// Token: 0x06008205 RID: 33285
		public abstract void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll);

		// Token: 0x06008206 RID: 33286
		public unsafe abstract void ProcessArchiveResponse(OperationWrapper operation, byte* pResult);

		// Token: 0x06008207 RID: 33287 RVA: 0x004D9468 File Offset: 0x004D7668
		public static bool IsCached(byte[] dataStates, int fieldId)
		{
			return ((int)dataStates[fieldId / 4] & 1 << fieldId % 4 * 2) != 0;
		}

		// Token: 0x06008208 RID: 33288 RVA: 0x004D948D File Offset: 0x004D768D
		public static void SetCached(byte[] dataStates, int fieldId)
		{
			int num = fieldId / 4;
			dataStates[num] |= (byte)(1 << fieldId % 4 * 2);
		}

		// Token: 0x06008209 RID: 33289 RVA: 0x004D94A9 File Offset: 0x004D76A9
		public static void ResetCached(byte[] dataStates, int fieldId)
		{
			int num = fieldId / 4;
			dataStates[num] &= (byte)(~(byte)(1 << fieldId % 4 * 2));
		}

		// Token: 0x0600820A RID: 33290 RVA: 0x004D94C8 File Offset: 0x004D76C8
		public static bool IsModified(byte[] dataStates, int fieldId)
		{
			return ((int)dataStates[fieldId / 4] & 2 << fieldId % 4 * 2) != 0;
		}

		// Token: 0x0600820B RID: 33291 RVA: 0x004D94ED File Offset: 0x004D76ED
		public static void SetModified(byte[] dataStates, int fieldId)
		{
			int num = fieldId / 4;
			dataStates[num] |= (byte)(2 << fieldId % 4 * 2);
		}

		// Token: 0x0600820C RID: 33292 RVA: 0x004D9509 File Offset: 0x004D7709
		public static void ResetModified(byte[] dataStates, int fieldId)
		{
			int num = fieldId / 4;
			dataStates[num] &= (byte)(~(byte)(2 << fieldId % 4 * 2));
		}

		// Token: 0x0600820D RID: 33293 RVA: 0x004D9528 File Offset: 0x004D7728
		public void ExecutePostModificationHandlers(DataContext context, DataUid uid)
		{
			DataModificationHandlerGroup handlerGroup;
			bool flag = !this.PostModificationDataHandlers.TryGetValue(uid, out handlerGroup);
			if (!flag)
			{
				handlerGroup.ExecuteAll(context, uid);
			}
		}

		// Token: 0x0600820E RID: 33294 RVA: 0x004D9558 File Offset: 0x004D7758
		public void AddPostModificationHandler(DataUid uid, string handlerKey, Action<DataContext, DataUid> handler)
		{
			DataModificationHandlerGroup handlerGroup;
			bool flag = !this.PostModificationDataHandlers.TryGetValue(uid, out handlerGroup);
			if (flag)
			{
				handlerGroup = new DataModificationHandlerGroup();
				this.PostModificationDataHandlers.Add(uid, handlerGroup);
			}
			handlerGroup.RegisterHandler(handlerKey, handler);
		}

		// Token: 0x0600820F RID: 33295 RVA: 0x004D959C File Offset: 0x004D779C
		public bool RemovePostModificationHandler(DataUid uid, string handlerKey)
		{
			DataModificationHandlerGroup handlerGroup;
			bool flag = this.PostModificationDataHandlers.TryGetValue(uid, out handlerGroup);
			bool result;
			if (flag)
			{
				handlerGroup.UnregisterHandler(handlerKey);
				result = (handlerGroup.Count == 0);
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06008210 RID: 33296 RVA: 0x004D95D6 File Offset: 0x004D77D6
		public void RemovePostModificationHandlers(DataUid uid)
		{
			this.PostModificationDataHandlers.Remove(uid);
		}

		// Token: 0x06008211 RID: 33297 RVA: 0x004D95E8 File Offset: 0x004D77E8
		protected static void SetModifiedAndInvalidateInfluencedCache(int dataId, byte[] dataStates, DataInfluence[][] cacheInfluences, DataContext context)
		{
			BaseGameDataDomain.SetModified(dataStates, dataId);
			DataInfluence[] influences = cacheInfluences[dataId];
			bool flag = influences == null;
			if (!flag)
			{
				Tester.Assert(context != null, "");
				int influencesCount = influences.Length;
				for (int i = 0; i < influencesCount; i++)
				{
					DataInfluence influence = influences[i];
					BaseGameDataDomain domain = DomainManager.Domains[(int)influence.TargetIndicator.DomainId];
					domain.InvalidateCache(null, influence, context, false);
				}
			}
		}

		// Token: 0x06008212 RID: 33298 RVA: 0x004D965C File Offset: 0x004D785C
		protected static void InvalidateSelfAndInfluencedCache(int dataId, byte[] dataStates, DataInfluence[][] cacheInfluences, DataContext context)
		{
			int outerIndex = dataId / 4;
			int innerIndex = dataId % 4 * 2;
			int state = ((int)dataStates[outerIndex] & ~(3 << innerIndex)) | 2 << innerIndex;
			dataStates[outerIndex] = (byte)state;
			DataInfluence[] influences = cacheInfluences[dataId];
			bool flag = influences == null;
			if (!flag)
			{
				Tester.Assert(context != null, "");
				int influencesCount = influences.Length;
				for (int i = 0; i < influencesCount; i++)
				{
					DataInfluence influence = influences[i];
					BaseGameDataDomain domain = DomainManager.Domains[(int)influence.TargetIndicator.DomainId];
					domain.InvalidateCache(null, influence, context, false);
				}
			}
		}

		// Token: 0x06008213 RID: 33299 RVA: 0x004D96F0 File Offset: 0x004D78F0
		protected static void InvalidateAllAndInfluencedCaches(DataInfluence[][] cacheInfluences, ObjectCollectionDataStates dataStates, DataInfluence influence, DataContext context)
		{
			Tester.Assert(context != null, "");
			dataStates.InvalidateAll(influence);
			List<DataUid> targetUids = influence.TargetUids;
			int targetUidsCount = targetUids.Count;
			for (int i = 0; i < targetUidsCount; i++)
			{
				ushort fieldId = (ushort)targetUids[i].SubId1;
				DataInfluence[] currInfluences = cacheInfluences[(int)fieldId];
				bool flag = currInfluences == null;
				if (!flag)
				{
					int currInfluencesCount = currInfluences.Length;
					for (int j = 0; j < currInfluencesCount; j++)
					{
						DataInfluence currInfluence = currInfluences[j];
						BaseGameDataDomain domain = DomainManager.Domains[(int)currInfluence.TargetIndicator.DomainId];
						domain.InvalidateCache(null, currInfluence, context, true);
					}
				}
			}
		}

		// Token: 0x04002416 RID: 9238
		protected readonly byte[] DataStates;

		// Token: 0x04002417 RID: 9239
		protected readonly Dictionary<DataUid, DataModificationHandlerGroup> PostModificationDataHandlers = new Dictionary<DataUid, DataModificationHandlerGroup>();
	}
}
