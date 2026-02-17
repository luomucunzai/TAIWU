using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008A5 RID: 2213
	public class GainExpByCricketBattleAction : IGeneralAction
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x0600787A RID: 30842 RVA: 0x00464998 File Offset: 0x00462B98
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600787B RID: 30843 RVA: 0x0046499C File Offset: 0x00462B9C
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return DomainManager.Item.CheckCharacterHasWager(this.Succeed ? targetChar : selfChar, this.Wager);
		}

		// Token: 0x0600787C RID: 30844 RVA: 0x004649CC File Offset: 0x00462BCC
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddRequestCricketBattle(selfCharId, location, targetChar.GetId());
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x0600787D RID: 30845 RVA: 0x00464A0C File Offset: 0x00462C0C
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
				selfChar.ChangeExp(context, this.ExpGain);
				lifeRecordCollection.AddCricketBattleWin(selfCharId, currDate, targetCharId, location);
				DomainManager.Item.TransferWager(context, targetChar, selfChar, this.Wager);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddCricketBattleWin(selfCharId, targetCharId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				targetChar.ChangeExp(context, this.ExpGain);
				lifeRecordCollection.AddCricketBattleLose(selfCharId, currDate, targetCharId, location);
				DomainManager.Item.TransferWager(context, selfChar, targetChar, this.Wager);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddCricketBattleWin(targetCharId, selfCharId);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x04002178 RID: 8568
		public Wager Wager;

		// Token: 0x04002179 RID: 8569
		public bool Succeed;

		// Token: 0x0400217A RID: 8570
		public int ExpGain;
	}
}
