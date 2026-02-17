using System;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000862 RID: 2146
	[SerializableGameData(NotForDisplayModule = true)]
	public class PrioritizedActionWrapper : ISerializableGameData
	{
		// Token: 0x06007712 RID: 30482 RVA: 0x0045AC33 File Offset: 0x00458E33
		public PrioritizedActionWrapper()
		{
			this.ActionType = -1;
			this.Action = null;
		}

		// Token: 0x06007713 RID: 30483 RVA: 0x0045AC4B File Offset: 0x00458E4B
		public PrioritizedActionWrapper(sbyte actionType, NpcTravelTarget target)
		{
			this.ActionType = (short)actionType;
			this.Action = PrioritizedActionTypeHelper.CreatePrioritizedAction((short)actionType);
			this.Action.Target = target;
		}

		// Token: 0x06007714 RID: 30484 RVA: 0x0045AC74 File Offset: 0x00458E74
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06007715 RID: 30485 RVA: 0x0045AC88 File Offset: 0x00458E88
		public int GetSerializedSize()
		{
			int totalSize = 3;
			bool flag = this.ActionType >= 0;
			if (flag)
			{
				totalSize += this.Action.GetSerializedSize();
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007716 RID: 30486 RVA: 0x0045ACCC File Offset: 0x00458ECC
		public unsafe int Serialize(byte* pData)
		{
			*pData = 15;
			byte* pCurrData = pData + 1;
			*(short*)pCurrData = this.ActionType;
			pCurrData += 2;
			bool flag = this.ActionType >= 0;
			if (flag)
			{
				pCurrData += this.Action.Serialize(pCurrData);
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007717 RID: 30487 RVA: 0x0045AD30 File Offset: 0x00458F30
		public unsafe int Deserialize(byte* pData)
		{
			this.ObsoleteActionType = *(sbyte*)pData;
			byte* pCurrData = pData + 1;
			bool flag = this.ObsoleteActionType > 14;
			if (flag)
			{
				this.ActionType = *(short*)pCurrData;
				pCurrData += 2;
			}
			else
			{
				this.ActionType = (short)this.ObsoleteActionType;
			}
			bool flag2 = this.ActionType >= 0;
			if (flag2)
			{
				this.Action = PrioritizedActionTypeHelper.CreatePrioritizedAction(this.ActionType);
				pCurrData += this.Action.Deserialize(pCurrData);
			}
			else
			{
				this.Action = null;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040020D4 RID: 8404
		[Obsolete]
		[SerializableGameDataField]
		public sbyte ObsoleteActionType;

		// Token: 0x040020D5 RID: 8405
		[SerializableGameDataField]
		public short ActionType;

		// Token: 0x040020D6 RID: 8406
		[SerializableGameDataField]
		public BasePrioritizedAction Action;
	}
}
