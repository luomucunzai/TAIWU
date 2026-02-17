using System;
using Config;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.TeachRandom
{
	// Token: 0x0200087F RID: 2175
	public class TeachCombatSkillAction : IGeneralAction
	{
		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x060077BC RID: 30652 RVA: 0x0045F61B File Offset: 0x0045D81B
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060077BD RID: 30653 RVA: 0x0045F620 File Offset: 0x0045D820
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return !targetChar.GetLearnedCombatSkills().Contains(this.SkillTemplateId);
		}

		// Token: 0x060077BE RID: 30654 RVA: 0x0045F648 File Offset: 0x0045D848
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			bool flag = DomainManager.Taiwu.IsTaiwuAbleToGetTaught(selfChar);
			if (flag)
			{
				DomainManager.World.GetMonthlyEventCollection().AddTeachCombatSkill(selfChar.GetId(), selfChar.GetLocation(), targetChar.GetId(), this.SkillTemplateId);
			}
		}

		// Token: 0x060077BF RID: 30655 RVA: 0x0045F690 File Offset: 0x0045D890
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			int targetCharId = targetChar.GetId();
			int selfCharId = selfChar.GetId();
			short bookId = Config.CombatSkill.Instance[this.SkillTemplateId].BookId;
			SkillBookItem bookCfg = Config.SkillBook.Instance[bookId];
			byte pageId = CombatSkillStateHelper.GetPageId(this.InternalIndex);
			bool succeed = this.Succeed;
			if (succeed)
			{
				lifeRecordCollection.AddLearnCombatSkillWithInstructionSucceed(targetCharId, currDate, selfCharId, location, 10, bookId, (int)(pageId + 1));
				bool flag = targetCharId == DomainManager.Taiwu.GetTaiwuCharId();
				if (flag)
				{
					ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, bookId, this.InternalIndex, this.GeneratedPageTypes);
					targetChar.AddInventoryItem(context, itemKey, 1, false);
				}
				targetChar.LearnNewCombatSkill(context, this.SkillTemplateId, (ushort)(1 << (int)this.InternalIndex));
				targetChar.ChangeHappiness(context, (int)bookCfg.BaseHappinessChange);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, bookCfg.BaseFavorabilityChange);
			}
			else
			{
				lifeRecordCollection.AddLearnCombatSkillWithInstructionFail(targetCharId, currDate, selfCharId, location, 10, bookId, (int)(pageId + 1));
			}
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int secretInfoOffset = secretInformationCollection.AddInstructOnCombatSkill(selfCharId, targetCharId, this.SkillTemplateId);
			int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
		}

		// Token: 0x04002113 RID: 8467
		public short SkillTemplateId;

		// Token: 0x04002114 RID: 8468
		public byte InternalIndex;

		// Token: 0x04002115 RID: 8469
		public byte GeneratedPageTypes;

		// Token: 0x04002116 RID: 8470
		public bool Succeed;
	}
}
