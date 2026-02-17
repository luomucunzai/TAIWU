using System;
using Config;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand
{
	// Token: 0x02000882 RID: 2178
	public class RequestCombatSkillDemandAction : IGeneralAction
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060077CB RID: 30667 RVA: 0x0045FC21 File Offset: 0x0045DE21
		public sbyte ActionEnergyType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060077CC RID: 30668 RVA: 0x0045FC24 File Offset: 0x0045DE24
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return !selfChar.GetLearnedCombatSkills().Contains(Config.SkillBook.Instance[this.BookTemplateId].CombatSkillTemplateId);
		}

		// Token: 0x060077CD RID: 30669 RVA: 0x0045FC5C File Offset: 0x0045DE5C
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddRequestInstructionOnCombatSkill(selfCharId, location, targetChar.GetId(), 10, this.BookTemplateId, (int)(CombatSkillStateHelper.GetPageId(this.InternalIndex) + 1), (int)this.InternalIndex, (int)this.GeneratedPageTypes);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x060077CE RID: 30670 RVA: 0x0045FCBC File Offset: 0x0045DEBC
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookTemplateId];
			short combatSkillTemplateId = bookCfg.CombatSkillTemplateId;
			byte pageId = CombatSkillStateHelper.GetPageId(this.InternalIndex);
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
						ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, this.BookTemplateId, this.InternalIndex, this.GeneratedPageTypes);
						selfChar.AddInventoryItem(context, itemKey, 1, false);
						ProfessionFormulaItem seniorityFormula = ProfessionFormula.Instance[50];
						int addSeniority = seniorityFormula.Calculate((int)Config.CombatSkill.Instance[combatSkillTemplateId].Grade);
						DomainManager.Extra.ChangeProfessionSeniority(context, 7, addSeniority, true, false);
					}
					else
					{
						bool flag2 = targetCharId == taiwuCharId;
						if (flag2)
						{
							ProfessionFormulaItem seniorityFormula2 = ProfessionFormula.Instance[53];
							int addSeniority2 = seniorityFormula2.Calculate((int)Config.CombatSkill.Instance[combatSkillTemplateId].Grade);
							DomainManager.Extra.ChangeProfessionSeniority(context, 7, addSeniority2, true, false);
						}
					}
					selfChar.LearnNewCombatSkill(context, combatSkillTemplateId, (ushort)(1 << (int)this.InternalIndex));
					selfChar.ChangeHappiness(context, (int)(ItemTemplateHelper.GetBaseHappinessChange(10, this.BookTemplateId) / 2));
					DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, ItemTemplateHelper.GetBaseFavorabilityChange(10, this.BookTemplateId));
					lifeRecordCollection.AddRequestInstructionOnCombatSkillSucceed(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
				}
				else
				{
					lifeRecordCollection.AddRequestInstructionOnCombatSkillFailToLearn(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
				}
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddInstructOnCombatSkill(targetCharId, selfCharId, bookCfg.CombatSkillTemplateId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
				secretInfoOffset = secretInformationCollection.AddAcceptRequestInstructionOnCombatSkill(targetCharId, selfCharId, bookCfg.CombatSkillTemplateId);
				secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				lifeRecordCollection.AddRequestInstructionOnCombatSkillFail(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestInstructionOnCombatSkill(targetCharId, selfCharId, bookCfg.CombatSkillTemplateId);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x0400211C RID: 8476
		public short BookTemplateId;

		// Token: 0x0400211D RID: 8477
		public byte InternalIndex;

		// Token: 0x0400211E RID: 8478
		public byte GeneratedPageTypes;

		// Token: 0x0400211F RID: 8479
		public bool AgreeToRequest;

		// Token: 0x04002120 RID: 8480
		public bool Succeed;
	}
}
