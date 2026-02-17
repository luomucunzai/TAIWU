using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x02000092 RID: 146
	[SerializableGameData(NotForDisplayModule = true)]
	public abstract class MonthlyActionBase : ISerializableGameData
	{
		// Token: 0x06001959 RID: 6489 RVA: 0x0016D7A4 File Offset: 0x0016B9A4
		public virtual bool IsMonthMatch()
		{
			return false;
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0016D7B7 File Offset: 0x0016B9B7
		public virtual void TriggerAction()
		{
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0016D7BA File Offset: 0x0016B9BA
		public virtual void MonthlyHandler()
		{
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x0016D7BD File Offset: 0x0016B9BD
		public virtual void Activate()
		{
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0016D7C0 File Offset: 0x0016B9C0
		public virtual void Deactivate(bool isComplete)
		{
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0016D7C3 File Offset: 0x0016B9C3
		public virtual void InheritNonArchiveData(MonthlyActionBase action)
		{
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0016D7C6 File Offset: 0x0016B9C6
		public virtual void FillEventArgBox(EventArgBox eventArgBox)
		{
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0016D7C9 File Offset: 0x0016B9C9
		public virtual void EnsurePrerequisites()
		{
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0016D7CC File Offset: 0x0016B9CC
		public virtual void ValidationHandler()
		{
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0016D7CF File Offset: 0x0016B9CF
		public virtual void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
		}

		// Token: 0x06001963 RID: 6499
		public abstract MonthlyActionBase CreateCopy();

		// Token: 0x06001964 RID: 6500 RVA: 0x0016D7D2 File Offset: 0x0016B9D2
		public virtual bool IsSerializedSizeFixed()
		{
			throw new Exception("Can't call ISerializableGameData method directly from abstract class MonthlyActionBase.");
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0016D7DF File Offset: 0x0016B9DF
		public virtual int GetSerializedSize()
		{
			throw new Exception("Can't call ISerializableGameData method directly from abstract class MonthlyActionBase.");
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0016D7EC File Offset: 0x0016B9EC
		public unsafe virtual int Serialize(byte* pData)
		{
			throw new Exception("Can't call ISerializableGameData method directly from abstract class MonthlyActionBase.");
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0016D7F9 File Offset: 0x0016B9F9
		public unsafe virtual int Deserialize(byte* pData)
		{
			throw new Exception("Can't call ISerializableGameData method directly from abstract class MonthlyActionBase.");
		}

		// Token: 0x040005AE RID: 1454
		[SerializableGameDataField]
		public MonthlyActionKey Key = MonthlyActionKey.Invalid;

		// Token: 0x040005AF RID: 1455
		[SerializableGameDataField]
		public sbyte State;

		// Token: 0x040005B0 RID: 1456
		[SerializableGameDataField]
		public int Month;

		// Token: 0x040005B1 RID: 1457
		[SerializableGameDataField]
		public int LastFinishDate;
	}
}
