using System;
using GameData.Common;
using GameData.Domains.Information;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom
{
	// Token: 0x02000896 RID: 2198
	public class LifeSkillDivinationAction : IGeneralAction
	{
		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600782F RID: 30767 RVA: 0x00462C0F File Offset: 0x00460E0F
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06007830 RID: 30768 RVA: 0x00462C14 File Offset: 0x00460E14
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			bool flag = selfChar.GetHealth() < selfChar.GetLeftMaxHealth(false) || selfChar.GetHealth() <= 12;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				SecretInformationCharacterDataCollection collection;
				bool flag2 = DomainManager.Information.TryGetElement_CharacterSecretInformation(selfChar.GetId(), out collection) && collection.Collection.ContainsKey(this.SecretInfoMetaDataId);
				result = !flag2;
			}
			return result;
		}

		// Token: 0x06007831 RID: 30769 RVA: 0x00462C80 File Offset: 0x00460E80
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			bool succeed = this.Succeed;
			if (succeed)
			{
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				int selfCharId = selfChar.GetId();
				int targetCharId = targetChar.GetId();
				Location location = selfChar.GetLocation();
				monthlyNotifications.AddPractiseDivination(selfCharId, location, targetCharId);
			}
			this.ApplyChanges(context, selfChar, targetChar);
		}

		// Token: 0x06007832 RID: 30770 RVA: 0x00462CD0 File Offset: 0x00460ED0
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool succeed = this.Succeed;
			if (succeed)
			{
				lifeRecordCollection.AddDivinationSucceed(selfCharId, currDate, targetCharId, location);
				DomainManager.Information.ReceiveSecretInformation(context, this.SecretInfoMetaDataId, selfCharId, targetCharId);
			}
			else
			{
				lifeRecordCollection.AddDivinationFail(selfCharId, currDate, location);
				selfChar.ChangeHealth(context, -12);
			}
		}

		// Token: 0x04002158 RID: 8536
		public int SecretInfoMetaDataId;

		// Token: 0x04002159 RID: 8537
		public bool Succeed;
	}
}
