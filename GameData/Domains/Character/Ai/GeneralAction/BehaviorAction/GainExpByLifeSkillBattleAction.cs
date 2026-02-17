using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008A1 RID: 2209
	public class GainExpByLifeSkillBattleAction : IGeneralAction
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06007866 RID: 30822 RVA: 0x0046460A File Offset: 0x0046280A
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06007867 RID: 30823 RVA: 0x00464610 File Offset: 0x00462810
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return true;
		}

		// Token: 0x06007868 RID: 30824 RVA: 0x00464624 File Offset: 0x00462824
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			MonthlyEventCollection monthlyEvents = DomainManager.World.GetMonthlyEventCollection();
			monthlyEvents.AddRequestLifeSkillBattle(selfCharId, location, targetChar.GetId());
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x06007869 RID: 30825 RVA: 0x00464664 File Offset: 0x00462864
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			TempHashsetContainer<int> closeCharIds = context.AdvanceMonthRelatedData.RelatedCharIds;
			bool succeed = this.Succeed;
			Character winner;
			if (succeed)
			{
				lifeRecordCollection.AddLifeSkillBattleWin(selfCharId, currDate, targetCharId, location);
				winner = selfChar;
			}
			else
			{
				lifeRecordCollection.AddLifeSkillBattleLose(selfCharId, currDate, targetCharId, location);
				winner = targetChar;
			}
			winner.ChangeExp(context, this.ExpGain);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int secretInfoOffset = secretInformationCollection.AddLifeSkillBattleWin(selfCharId, targetCharId);
			int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
		}

		// Token: 0x04002171 RID: 8561
		public bool Succeed;

		// Token: 0x04002172 RID: 8562
		public int ExpGain;
	}
}
