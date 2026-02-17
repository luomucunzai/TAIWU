using System;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000854 RID: 2132
	[SerializableGameData(NotForDisplayModule = true)]
	public abstract class BasePrioritizedAction : ISerializableGameData
	{
		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060076B1 RID: 30385
		public abstract short ActionType { get; }

		// Token: 0x060076B2 RID: 30386 RVA: 0x0045815C File Offset: 0x0045635C
		public virtual bool CanJointActionWith(BasePrioritizedAction other)
		{
			return this.ActionType == other.ActionType && this.Target.IsSameTargetWith(other.Target);
		}

		// Token: 0x060076B3 RID: 30387 RVA: 0x00458190 File Offset: 0x00456390
		public virtual bool CheckValid(Character selfChar)
		{
			return this.Target.RemainingMonth > 0;
		}

		// Token: 0x060076B4 RID: 30388
		public abstract void OnStart(DataContext context, Character selfChar);

		// Token: 0x060076B5 RID: 30389
		public abstract void OnInterrupt(DataContext context, Character selfChar);

		// Token: 0x060076B6 RID: 30390 RVA: 0x004581B0 File Offset: 0x004563B0
		public virtual void OnArrival(DataContext context, Character selfChar)
		{
			this.HasArrived = true;
		}

		// Token: 0x060076B7 RID: 30391
		public abstract bool Execute(DataContext context, Character selfChar);

		// Token: 0x060076B8 RID: 30392 RVA: 0x004581BA File Offset: 0x004563BA
		public virtual void OnCharacterDead(DataContext context, Character selfChar)
		{
		}

		// Token: 0x060076B9 RID: 30393 RVA: 0x004581C0 File Offset: 0x004563C0
		public virtual bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060076BA RID: 30394 RVA: 0x004581D4 File Offset: 0x004563D4
		public virtual int GetSerializedSize()
		{
			int totalSize = 17;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060076BB RID: 30395 RVA: 0x004581FC File Offset: 0x004563FC
		public unsafe virtual int Serialize(byte* pData)
		{
			byte* pCurrData = pData + this.Target.Serialize(pData);
			*pCurrData = (this.HasArrived ? 1 : 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060076BC RID: 30396 RVA: 0x00458244 File Offset: 0x00456444
		public unsafe virtual int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + this.Target.Deserialize(pData);
			this.HasArrived = (*pCurrData != 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040020C8 RID: 8392
		[SerializableGameDataField]
		public NpcTravelTarget Target;

		// Token: 0x040020C9 RID: 8393
		[SerializableGameDataField]
		public bool HasArrived = false;
	}
}
