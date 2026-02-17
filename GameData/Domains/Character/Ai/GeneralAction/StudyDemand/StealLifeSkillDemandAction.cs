using System;
using Config;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand
{
	// Token: 0x02000886 RID: 2182
	public class StealLifeSkillDemandAction : IGeneralAction
	{
		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060077DF RID: 30687 RVA: 0x004609FC File Offset: 0x0045EBFC
		public sbyte ActionEnergyType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060077E0 RID: 30688 RVA: 0x00460A00 File Offset: 0x0045EC00
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.FindLearnedLifeSkillIndex(Config.SkillBook.Instance[this.BookTemplateId].LifeSkillTemplateId) < 0;
		}

		// Token: 0x060077E1 RID: 30689 RVA: 0x00460A30 File Offset: 0x0045EC30
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookTemplateId];
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			bool flag = this.Phase <= 3;
			if (flag)
			{
				monthlyNotificationCollection.AddStealLifeSkillFailure(selfCharId, location, targetCharId, bookCfg.LifeSkillTemplateId);
				this.ApplyChanges(context, selfChar, targetChar);
			}
			else
			{
				monthlyNotificationCollection.AddStealLifeSkillSuccess(selfCharId, location, targetCharId, bookCfg.LifeSkillTemplateId);
				bool flag2 = this.Phase == 4;
				if (flag2)
				{
					monthlyEventCollection.AddStealLifeSkillButBeCaught(selfCharId, location, targetCharId, 10, this.BookTemplateId, (int)(this.PageId + 1));
				}
				else
				{
					monthlyEventCollection.AddStealLifeSkillAndEscape(selfCharId, location, targetCharId, 10, this.BookTemplateId, (int)(this.PageId + 1));
				}
				CharacterDomain.AddLockMovementCharSet(selfCharId);
			}
		}

		// Token: 0x060077E2 RID: 30690 RVA: 0x00460B14 File Offset: 0x0045ED14
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookTemplateId];
			short lifeSkillTemplateId = bookCfg.LifeSkillTemplateId;
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool flag = selfCharId != taiwuCharId;
			if (flag)
			{
				selfChar.ChangeCurrMainAttribute(context, 5, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = this.Phase >= 4;
			if (flag2)
			{
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddStealLifeSkill(selfCharId, targetCharId, lifeSkillTemplateId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			switch (this.Phase)
			{
			case 0:
				lifeRecordCollection.AddStealLifeSkillFail1(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				break;
			case 1:
				lifeRecordCollection.AddStealLifeSkillFail2(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				break;
			case 2:
				lifeRecordCollection.AddStealLifeSkillFail3(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				break;
			case 3:
				lifeRecordCollection.AddStealLifeSkillFail4(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				break;
			case 4:
			{
				bool flag3 = selfCharId == taiwuCharId;
				if (flag3)
				{
					ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, this.BookTemplateId, this.PageId, 0);
					selfChar.AddInventoryItem(context, itemKey, 1, false);
				}
				selfChar.LearnNewLifeSkill(context, lifeSkillTemplateId, (byte)(1 << (int)this.PageId));
				lifeRecordCollection.AddStealLifeSkillSucceed(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
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
					ItemKey itemKey2 = DomainManager.Item.CreateDemandedSkillBook(context, this.BookTemplateId, this.PageId, 0);
					selfChar.AddInventoryItem(context, itemKey2, 1, false);
				}
				selfChar.LearnNewLifeSkill(context, lifeSkillTemplateId, (byte)(1 << (int)this.PageId));
				lifeRecordCollection.AddStealLifeSkillSucceedAndEscaped(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(this.PageId + 1));
				break;
			}
			}
		}

		// Token: 0x0400212D RID: 8493
		public short BookTemplateId;

		// Token: 0x0400212E RID: 8494
		public byte PageId;

		// Token: 0x0400212F RID: 8495
		public sbyte Phase;
	}
}
