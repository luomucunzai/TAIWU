using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x0200087E RID: 2174
	public class RobGraveResourceAction : IGeneralAction
	{
		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x060077B7 RID: 30647 RVA: 0x0045F431 File Offset: 0x0045D631
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060077B8 RID: 30648 RVA: 0x0045F434 File Offset: 0x0045D634
		public unsafe bool CheckValid(Character selfChar, Character targetChar)
		{
			Grave grave;
			bool flag = !DomainManager.Character.TryGetElement_Graves(this.TargetGraveId, out grave);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ResourceInts resources = *grave.GetResources();
				result = (*(ref resources.Items.FixedElementField + (IntPtr)this.ResourceType * 4) >= this.Amount);
			}
			return result;
		}

		// Token: 0x060077B9 RID: 30649 RVA: 0x0045F491 File Offset: 0x0045D691
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("cannot be digging the current Taiwu's grave when he or she is still alive.");
		}

		// Token: 0x060077BA RID: 30650 RVA: 0x0045F4A0 File Offset: 0x0045D6A0
		public unsafe void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = selfCharId != taiwuCharId;
			if (flag)
			{
				selfChar.ChangeCurrMainAttribute(context, 3, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = DomainManager.Character.IsTaiwuPeople(this.TargetGraveId);
			if (flag2)
			{
				monthlyNotificationCollection.AddDigResource(selfCharId, location, this.TargetGraveId, this.ResourceType);
			}
			bool succeed = this.Succeed;
			if (succeed)
			{
				Grave grave = DomainManager.Character.GetElement_Graves(this.TargetGraveId);
				selfChar.ChangeResource(context, this.ResourceType, this.Amount);
				ResourceInts resources = *grave.GetResources();
				*(ref resources.Items.FixedElementField + (IntPtr)this.ResourceType * 4) -= this.Amount;
				grave.SetResources(ref resources, context);
				lifeRecordCollection.AddRobResourceFromGraveSucceed(selfCharId, currDate, this.TargetGraveId, location, this.ResourceType, this.Amount);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddRobGraveResource(selfCharId, this.TargetGraveId, this.ResourceType);
				DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				lifeRecordCollection.AddRobResourceFromGraveFail(selfCharId, currDate, this.TargetGraveId, location, this.ResourceType);
			}
		}

		// Token: 0x0400210F RID: 8463
		public sbyte ResourceType;

		// Token: 0x04002110 RID: 8464
		public int Amount;

		// Token: 0x04002111 RID: 8465
		public bool Succeed;

		// Token: 0x04002112 RID: 8466
		public int TargetGraveId;
	}
}
