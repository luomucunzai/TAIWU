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

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand;

public class StealCombatSkillDemandAction : IGeneralAction
{
	public short BookTemplateId;

	public byte InternalIndex;

	public byte GeneratedPageTypes;

	public sbyte Phase;

	public sbyte ActionEnergyType => 2;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return !selfChar.GetLearnedCombatSkills().Contains(Config.SkillBook.Instance[BookTemplateId].CombatSkillTemplateId);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = targetChar.GetLocation();
		SkillBookItem skillBookItem = Config.SkillBook.Instance[BookTemplateId];
		short combatSkillTemplateId = skillBookItem.CombatSkillTemplateId;
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		if (Phase <= 3)
		{
			monthlyNotificationCollection.AddStealCombatSkillFailure(id, location, id2, combatSkillTemplateId);
			ApplyChanges(context, selfChar, targetChar);
			return;
		}
		monthlyNotificationCollection.AddStealCombatSkillSuccess(id, location, id2, combatSkillTemplateId);
		if (Phase == 4)
		{
			monthlyEventCollection.AddStealCombatSkillButBeCaught(id, location, id2, 10, BookTemplateId, CombatSkillStateHelper.GetPageId(InternalIndex) + 1, InternalIndex, GeneratedPageTypes);
		}
		else
		{
			monthlyEventCollection.AddStealCombatSkillAndEscape(id, location, id2, 10, BookTemplateId, CombatSkillStateHelper.GetPageId(InternalIndex) + 1, InternalIndex, GeneratedPageTypes);
			ApplyChanges(context, selfChar, targetChar);
		}
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		SkillBookItem skillBookItem = Config.SkillBook.Instance[BookTemplateId];
		short combatSkillTemplateId = skillBookItem.CombatSkillTemplateId;
		byte pageId = CombatSkillStateHelper.GetPageId(InternalIndex);
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (id != taiwuCharId)
		{
			selfChar.ChangeCurrMainAttribute(context, 5, -GlobalConfig.Instance.HarmfulActionCost);
		}
		if (Phase >= 4)
		{
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddStealCombatSkill(id, id2, combatSkillTemplateId);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		switch (Phase)
		{
		case 0:
			lifeRecordCollection.AddStealCombatSkillFail1(id, currDate, id2, location, 10, BookTemplateId, pageId + 1);
			break;
		case 1:
			lifeRecordCollection.AddStealCombatSkillFail2(id, currDate, id2, location, 10, BookTemplateId, pageId + 1);
			break;
		case 2:
			lifeRecordCollection.AddStealCombatSkillFail3(id, currDate, id2, location, 10, BookTemplateId, pageId + 1);
			break;
		case 3:
			lifeRecordCollection.AddStealCombatSkillFail4(id, currDate, id2, location, 10, BookTemplateId, pageId + 1);
			break;
		case 4:
			if (id == taiwuCharId)
			{
				ItemKey itemKey2 = DomainManager.Item.CreateDemandedSkillBook(context, BookTemplateId, InternalIndex, GeneratedPageTypes);
				selfChar.AddInventoryItem(context, itemKey2, 1);
			}
			selfChar.LearnNewCombatSkill(context, combatSkillTemplateId, (ushort)(1 << (int)InternalIndex));
			lifeRecordCollection.AddStealCombatSkillSucceed(id, currDate, id2, location, 10, BookTemplateId, pageId + 1);
			if (id2 != taiwuCharId)
			{
				AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateCharacterCombat(context, targetChar, selfChar, CombatType.Beat);
				if ((uint)(npcCombatResultType - 2) <= 1u)
				{
					DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, targetChar, -40, -20, 0);
				}
				else
				{
					DomainManager.Character.SimulateCharacterCombatResult(context, targetChar, selfChar, -40, -20, 0);
				}
			}
			break;
		default:
			if (id == taiwuCharId)
			{
				ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, BookTemplateId, InternalIndex, GeneratedPageTypes);
				selfChar.AddInventoryItem(context, itemKey, 1);
			}
			selfChar.LearnNewCombatSkill(context, combatSkillTemplateId, (ushort)(1 << (int)InternalIndex));
			lifeRecordCollection.AddStealCombatSkillSucceedAndEscaped(id, currDate, id2, location, 10, BookTemplateId, pageId + 1);
			break;
		}
	}
}
