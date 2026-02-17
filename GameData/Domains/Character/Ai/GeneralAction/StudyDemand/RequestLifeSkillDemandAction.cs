using System;
using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand
{
	// Token: 0x02000885 RID: 2181
	public class RequestLifeSkillDemandAction : IGeneralAction
	{
		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060077DA RID: 30682 RVA: 0x004606F5 File Offset: 0x0045E8F5
		public sbyte ActionEnergyType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060077DB RID: 30683 RVA: 0x004606F8 File Offset: 0x0045E8F8
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.FindLearnedLifeSkillIndex(Config.SkillBook.Instance[this.BookTemplateId].LifeSkillTemplateId) < 0;
		}

		// Token: 0x060077DC RID: 30684 RVA: 0x00460728 File Offset: 0x0045E928
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddRequestInstructionOnLifeSkill(selfCharId, location, targetCharId, 10, this.BookTemplateId, (int)(this.PageId + 1));
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x060077DD RID: 30685 RVA: 0x00460778 File Offset: 0x0045E978
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookTemplateId];
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				bool succeed = this.Succeed;
				if (succeed)
				{
					int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
					bool flag = selfCharId == taiwuCharId;
					if (flag)
					{
						ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, this.BookTemplateId, this.PageId, 0);
						selfChar.AddInventoryItem(context, itemKey, 1, false);
						ProfessionFormulaItem seniorityFormula = ProfessionFormula.Instance[101];
						int addSeniority = seniorityFormula.Calculate((int)LifeSkill.Instance[bookCfg.LifeSkillTemplateId].Grade);
						DomainManager.Extra.ChangeProfessionSeniority(context, 16, addSeniority, true, false);
					}
					else
					{
						bool flag2 = targetCharId == taiwuCharId;
						if (flag2)
						{
							ProfessionFormulaItem seniorityFormula2 = ProfessionFormula.Instance[104];
							int addSeniority2 = seniorityFormula2.Calculate((int)LifeSkill.Instance[bookCfg.LifeSkillTemplateId].Grade);
							DomainManager.Extra.ChangeProfessionSeniority(context, 16, addSeniority2, true, false);
						}
					}
					selfChar.LearnNewLifeSkill(context, bookCfg.LifeSkillTemplateId, (byte)(1 << (int)this.PageId));
					selfChar.ChangeHappiness(context, (int)(ItemTemplateHelper.GetBaseHappinessChange(10, this.BookTemplateId) / 2));
					DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, ItemTemplateHelper.GetBaseFavorabilityChange(10, this.BookTemplateId));
					lifeRecordCollection.AddRequestInstructionOnLifeSkillSucceed(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				}
				else
				{
					lifeRecordCollection.AddRequestInstructionOnLifeSkillFailToLearn(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				}
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddInstructOnLifeSkill(targetCharId, selfCharId, bookCfg.LifeSkillTemplateId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
				secretInfoOffset = secretInformationCollection.AddAcceptRequestInstructionOnLifeSkill(targetCharId, selfCharId, bookCfg.LifeSkillTemplateId);
				secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				lifeRecordCollection.AddRequestInstructionOnLifeSkillFail(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestInstructionOnLifeSkill(targetCharId, selfCharId, bookCfg.LifeSkillTemplateId);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x04002129 RID: 8489
		public short BookTemplateId;

		// Token: 0x0400212A RID: 8490
		public byte PageId;

		// Token: 0x0400212B RID: 8491
		public bool AgreeToRequest;

		// Token: 0x0400212C RID: 8492
		public bool Succeed;
	}
}
