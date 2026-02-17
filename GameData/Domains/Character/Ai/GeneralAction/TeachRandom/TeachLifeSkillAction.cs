using System;
using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.TeachRandom
{
	// Token: 0x02000880 RID: 2176
	public class TeachLifeSkillAction : IGeneralAction
	{
		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x060077C1 RID: 30657 RVA: 0x0045F7EA File Offset: 0x0045D9EA
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060077C2 RID: 30658 RVA: 0x0045F7F0 File Offset: 0x0045D9F0
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return targetChar.FindLearnedLifeSkillIndex(this.SkillTemplateId) < 0;
		}

		// Token: 0x060077C3 RID: 30659 RVA: 0x0045F814 File Offset: 0x0045DA14
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			bool succeed = this.Succeed;
			if (succeed)
			{
				DomainManager.World.GetMonthlyNotificationCollection().AddTeachLifeSkillSuccess(selfCharId, location, targetCharId, this.SkillTemplateId);
			}
			else
			{
				DomainManager.World.GetMonthlyNotificationCollection().AddTeachLifeSkillFailure(selfCharId, location, targetCharId, this.SkillTemplateId);
			}
			this.ApplyChanges(context, selfChar, targetChar);
		}

		// Token: 0x060077C4 RID: 30660 RVA: 0x0045F880 File Offset: 0x0045DA80
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			int targetCharId = targetChar.GetId();
			int selfCharId = selfChar.GetId();
			short bookId = LifeSkill.Instance[this.SkillTemplateId].SkillBookId;
			SkillBookItem bookCfg = Config.SkillBook.Instance[bookId];
			bool succeed = this.Succeed;
			if (succeed)
			{
				lifeRecordCollection.AddLearnLifeSkillWithInstructionSucceed(targetCharId, currDate, selfCharId, location, 10, bookId, (int)(this.PageId + 1));
				bool flag = targetCharId == DomainManager.Taiwu.GetTaiwuCharId();
				if (flag)
				{
					ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, bookId, this.PageId, 0);
					targetChar.AddInventoryItem(context, itemKey, 1, false);
				}
				LifeSkillItem lifeSkillItem = targetChar.LearnNewLifeSkill(context, this.SkillTemplateId, (byte)(1 << (int)this.PageId));
				targetChar.ChangeHappiness(context, (int)ItemTemplateHelper.GetBaseHappinessChange(10, this.SkillTemplateId));
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, ItemTemplateHelper.GetBaseFavorabilityChange(10, this.SkillTemplateId));
			}
			else
			{
				lifeRecordCollection.AddLearnLifeSkillWithInstructionFail(targetCharId, currDate, selfCharId, location, 10, bookId, (int)(this.PageId + 1));
			}
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int secretInfoOffset = secretInformationCollection.AddInstructOnLifeSkill(selfCharId, targetCharId, this.SkillTemplateId);
			int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
		}

		// Token: 0x04002117 RID: 8471
		public short SkillTemplateId;

		// Token: 0x04002118 RID: 8472
		public byte PageId;

		// Token: 0x04002119 RID: 8473
		public bool Succeed;
	}
}
