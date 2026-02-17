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
	// Token: 0x02000883 RID: 2179
	public class StealCombatSkillDemandAction : IGeneralAction
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060077D0 RID: 30672 RVA: 0x0045FF40 File Offset: 0x0045E140
		public sbyte ActionEnergyType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060077D1 RID: 30673 RVA: 0x0045FF44 File Offset: 0x0045E144
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return !selfChar.GetLearnedCombatSkills().Contains(Config.SkillBook.Instance[this.BookTemplateId].CombatSkillTemplateId);
		}

		// Token: 0x060077D2 RID: 30674 RVA: 0x0045FF7C File Offset: 0x0045E17C
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = targetChar.GetLocation();
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookTemplateId];
			short combatSkillTemplateId = bookCfg.CombatSkillTemplateId;
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			bool flag = this.Phase <= 3;
			if (flag)
			{
				monthlyNotificationCollection.AddStealCombatSkillFailure(selfCharId, location, targetCharId, combatSkillTemplateId);
				this.ApplyChanges(context, selfChar, targetChar);
			}
			else
			{
				monthlyNotificationCollection.AddStealCombatSkillSuccess(selfCharId, location, targetCharId, combatSkillTemplateId);
				bool flag2 = this.Phase == 4;
				if (flag2)
				{
					monthlyEventCollection.AddStealCombatSkillButBeCaught(selfCharId, location, targetCharId, 10, this.BookTemplateId, (int)(CombatSkillStateHelper.GetPageId(this.InternalIndex) + 1), (int)this.InternalIndex, (int)this.GeneratedPageTypes);
				}
				else
				{
					monthlyEventCollection.AddStealCombatSkillAndEscape(selfCharId, location, targetCharId, 10, this.BookTemplateId, (int)(CombatSkillStateHelper.GetPageId(this.InternalIndex) + 1), (int)this.InternalIndex, (int)this.GeneratedPageTypes);
					this.ApplyChanges(context, selfChar, targetChar);
				}
				CharacterDomain.AddLockMovementCharSet(selfCharId);
			}
		}

		// Token: 0x060077D3 RID: 30675 RVA: 0x00460090 File Offset: 0x0045E290
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			SkillBookItem bookCfg = Config.SkillBook.Instance[this.BookTemplateId];
			short combatSkillTemplateId = bookCfg.CombatSkillTemplateId;
			byte pageId = CombatSkillStateHelper.GetPageId(this.InternalIndex);
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = selfCharId != taiwuCharId;
			if (flag)
			{
				selfChar.ChangeCurrMainAttribute(context, 5, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = this.Phase >= 4;
			if (flag2)
			{
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddStealCombatSkill(selfCharId, targetCharId, combatSkillTemplateId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
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
				lifeRecordCollection.AddStealCombatSkillSucceed(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
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
				lifeRecordCollection.AddStealCombatSkillSucceedAndEscaped(selfCharId, currDate, targetCharId, location, 10, this.BookTemplateId, (int)(pageId + 1));
				break;
			}
			}
		}

		// Token: 0x04002121 RID: 8481
		public short BookTemplateId;

		// Token: 0x04002122 RID: 8482
		public byte InternalIndex;

		// Token: 0x04002123 RID: 8483
		public byte GeneratedPageTypes;

		// Token: 0x04002124 RID: 8484
		public sbyte Phase;
	}
}
