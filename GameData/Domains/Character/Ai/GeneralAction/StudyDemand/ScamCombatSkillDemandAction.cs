using System;
using Config;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand
{
	// Token: 0x02000884 RID: 2180
	public class ScamCombatSkillDemandAction : IGeneralAction
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060077D5 RID: 30677 RVA: 0x00460349 File Offset: 0x0045E549
		public sbyte ActionEnergyType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060077D6 RID: 30678 RVA: 0x0046034C File Offset: 0x0045E54C
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return !selfChar.GetLearnedCombatSkills().Contains(Config.SkillBook.Instance[this.BookTemplateId].CombatSkillTemplateId);
		}

		// Token: 0x060077D7 RID: 30679 RVA: 0x00460384 File Offset: 0x0045E584
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = targetChar.GetLocation();
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookTemplateId];
			short combatSkillTemplateId = bookCfg.CombatSkillTemplateId;
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			bool flag = this.Phase <= 2;
			if (flag)
			{
				monthlyNotificationCollection.AddCheatCombatSkillFailure(selfCharId, location, targetCharId, combatSkillTemplateId);
				this.ApplyChanges(context, selfChar, targetChar);
			}
			else
			{
				monthlyEventCollection.AddScamCombatSkill(selfCharId, location, targetCharId, 10, this.BookTemplateId, (int)(CombatSkillStateHelper.GetPageId(this.InternalIndex) + 1), (int)this.InternalIndex, (int)this.GeneratedPageTypes);
				CharacterDomain.AddLockMovementCharSet(selfCharId);
			}
		}

		// Token: 0x060077D8 RID: 30680 RVA: 0x0046043C File Offset: 0x0045E63C
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookTemplateId];
			short combatSkillTemplateId = bookCfg.CombatSkillTemplateId;
			byte pageId = CombatSkillStateHelper.GetPageId(this.InternalIndex);
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = selfCharId != taiwuCharId;
			if (flag)
			{
				selfChar.ChangeCurrMainAttribute(context, 2, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = this.Phase >= 4;
			if (flag2)
			{
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddScamCombatSkill(selfCharId, targetCharId, combatSkillTemplateId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			switch (this.Phase)
			{
			case 0:
				lifeRecordCollection.AddStealCombatSkillFail1(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
				break;
			case 1:
				lifeRecordCollection.AddStealCombatSkillFail2(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
				break;
			case 2:
				lifeRecordCollection.AddStealCombatSkillFail3(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
				break;
			case 3:
				lifeRecordCollection.AddStealCombatSkillFail4(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
				break;
			case 4:
			{
				bool flag3 = selfCharId == taiwuCharId;
				if (flag3)
				{
					ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, this.BookTemplateId, this.InternalIndex, this.GeneratedPageTypes);
					selfChar.AddInventoryItem(context, itemKey, 1, false);
				}
				selfChar.LearnNewCombatSkill(context, combatSkillTemplateId, (ushort)(1 << (int)this.InternalIndex));
				lifeRecordCollection.AddScamCombatSkillSucceed(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
				bool flag4 = targetCharId == taiwuCharId;
				if (!flag4)
				{
					AiHelper.NpcCombatResultType combatResultType = DomainManager.Character.SimulateCharacterCombat(context, targetChar, selfChar, CombatType.Beat, true, 1);
					bool flag5 = combatResultType - AiHelper.NpcCombatResultType.MajorDefeat <= 1;
					bool flag6 = flag5;
					if (flag6)
					{
						DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, targetChar, -40, -20, 0);
					}
					else
					{
						DomainManager.Character.SimulateCharacterCombatResult(context, targetChar, selfChar, -40, -20, 0);
					}
				}
				break;
			}
			default:
			{
				bool flag7 = selfCharId == taiwuCharId;
				if (flag7)
				{
					ItemKey itemKey2 = DomainManager.Item.CreateDemandedSkillBook(context, this.BookTemplateId, this.InternalIndex, this.GeneratedPageTypes);
					selfChar.AddInventoryItem(context, itemKey2, 1, false);
				}
				selfChar.LearnNewCombatSkill(context, combatSkillTemplateId, (ushort)(1 << (int)this.InternalIndex));
				lifeRecordCollection.AddScamCombatSkillSucceedAndEscaped(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
				break;
			}
			}
		}

		// Token: 0x04002125 RID: 8485
		public short BookTemplateId;

		// Token: 0x04002126 RID: 8486
		public byte InternalIndex;

		// Token: 0x04002127 RID: 8487
		public byte GeneratedPageTypes;

		// Token: 0x04002128 RID: 8488
		public sbyte Phase;
	}
}
