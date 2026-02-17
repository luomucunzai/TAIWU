using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom
{
	// Token: 0x02000895 RID: 2197
	public class LifeSkillCricketAction : IGeneralAction
	{
		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600782A RID: 30762 RVA: 0x00462AB6 File Offset: 0x00460CB6
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600782B RID: 30763 RVA: 0x00462ABC File Offset: 0x00460CBC
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return DomainManager.Item.CheckCharacterHasWager(this.Succeed ? targetChar : selfChar, this.Wager);
		}

		// Token: 0x0600782C RID: 30764 RVA: 0x00462AEC File Offset: 0x00460CEC
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddRequestCricketBattle(selfCharId, location, targetChar.GetId());
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x0600782D RID: 30765 RVA: 0x00462B2C File Offset: 0x00460D2C
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
				lifeRecordCollection.AddCricketBattleWin(selfCharId, currDate, targetCharId, location);
				DomainManager.Item.TransferWager(context, targetChar, selfChar, this.Wager);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddCricketBattleWin(selfCharId, targetCharId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				lifeRecordCollection.AddCricketBattleLose(selfCharId, currDate, targetCharId, location);
				DomainManager.Item.TransferWager(context, selfChar, targetChar, this.Wager);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddCricketBattleWin(targetCharId, selfCharId);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x04002156 RID: 8534
		public Wager Wager;

		// Token: 0x04002157 RID: 8535
		public bool Succeed;
	}
}
