using System;
using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x0200008C RID: 140
	[SerializableGameData(NotForDisplayModule = true)]
	public class ConfigWrapperAction : MonthlyActionBase, ISerializableGameData
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06001915 RID: 6421 RVA: 0x0016A48A File Offset: 0x0016868A
		// (set) Token: 0x06001916 RID: 6422 RVA: 0x0016A492 File Offset: 0x00168692
		public ConfigMonthlyAction CurrConfigMonthlyAction { get; private set; }

		// Token: 0x06001917 RID: 6423 RVA: 0x0016A49B File Offset: 0x0016869B
		public ConfigWrapperAction(MonthlyActionKey key)
		{
			this.Key = key;
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x0016A4AC File Offset: 0x001686AC
		public void CreateWrappedAction(short templateId, short assignedAreaId = -1)
		{
			bool flag = this.CurrConfigMonthlyAction != null;
			if (!flag)
			{
				this.CurrConfigMonthlyAction = new ConfigMonthlyAction(templateId, assignedAreaId);
				this.CurrConfigMonthlyAction.Key = this.Key;
				this.CurrConfigMonthlyAction.SelectLocation();
				this.CurrConfigMonthlyAction.TriggerAction();
				bool flag2 = this.CurrConfigMonthlyAction.State == 0;
				if (flag2)
				{
					this.CurrConfigMonthlyAction = null;
				}
			}
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0016A51A File Offset: 0x0016871A
		public override void MonthlyHandler()
		{
			ConfigMonthlyAction currConfigMonthlyAction = this.CurrConfigMonthlyAction;
			if (currConfigMonthlyAction != null)
			{
				currConfigMonthlyAction.MonthlyHandler();
			}
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0016A52F File Offset: 0x0016872F
		public override void ValidationHandler()
		{
			ConfigMonthlyAction currConfigMonthlyAction = this.CurrConfigMonthlyAction;
			if (currConfigMonthlyAction != null)
			{
				currConfigMonthlyAction.ValidationHandler();
			}
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0016A544 File Offset: 0x00168744
		public override void Deactivate(bool isComplete)
		{
			this.CurrConfigMonthlyAction.Deactivate(isComplete);
			this.CurrConfigMonthlyAction = null;
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0016A55C File Offset: 0x0016875C
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			bool flag = this.CurrConfigMonthlyAction == null;
			if (!flag)
			{
				Location location;
				eventArgBox.Get<Location>("AdventureLocation", out location);
				bool flag2 = !location.IsValid();
				if (!flag2)
				{
					this.CurrConfigMonthlyAction.EnsurePrerequisites();
					this.CurrConfigMonthlyAction.FillEventArgBox(eventArgBox);
				}
			}
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0016A5B0 File Offset: 0x001687B0
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			ConfigMonthlyAction currConfigMonthlyAction = this.CurrConfigMonthlyAction;
			if (currConfigMonthlyAction != null)
			{
				currConfigMonthlyAction.CollectCalledCharacters(calledCharacters);
			}
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x0016A5C8 File Offset: 0x001687C8
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<ConfigWrapperAction>(this);
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x0016A5E0 File Offset: 0x001687E0
		public ConfigWrapperAction()
		{
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0016A5EC File Offset: 0x001687EC
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0016A600 File Offset: 0x00168800
		public override int GetSerializedSize()
		{
			int totalSize = 12;
			bool flag = this.CurrConfigMonthlyAction != null;
			if (flag)
			{
				totalSize += 2 + this.CurrConfigMonthlyAction.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0016A648 File Offset: 0x00168848
		public unsafe override int Serialize(byte* pData)
		{
			bool flag = this.CurrConfigMonthlyAction != null;
			byte* pCurrData;
			if (flag)
			{
				pCurrData = pData + 2;
				int fieldSize = this.CurrConfigMonthlyAction.Serialize(pCurrData);
				pCurrData += fieldSize;
				Tester.Assert(fieldSize <= 65535, "");
				*(short*)pData = (short)((ushort)fieldSize);
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			pCurrData += this.Key.Serialize(pCurrData);
			*pCurrData = (byte)this.State;
			pCurrData++;
			*(int*)pCurrData = this.Month;
			pCurrData += 4;
			*(int*)pCurrData = this.LastFinishDate;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x0016A6FC File Offset: 0x001688FC
		public unsafe override int Deserialize(byte* pData)
		{
			ushort fieldSize = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldSize > 0;
			if (flag)
			{
				bool flag2 = this.CurrConfigMonthlyAction == null;
				if (flag2)
				{
					this.CurrConfigMonthlyAction = new ConfigMonthlyAction();
				}
				pCurrData += this.CurrConfigMonthlyAction.Deserialize(pCurrData);
			}
			else
			{
				this.CurrConfigMonthlyAction = null;
			}
			pCurrData += this.Key.Deserialize(pCurrData);
			this.State = *(sbyte*)pCurrData;
			pCurrData++;
			this.Month = *(int*)pCurrData;
			pCurrData += 4;
			this.LastFinishDate = *(int*)pCurrData;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}
	}
}
