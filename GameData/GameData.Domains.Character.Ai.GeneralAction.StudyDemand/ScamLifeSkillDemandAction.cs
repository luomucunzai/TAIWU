using Config;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.StudyDemand;

public class ScamLifeSkillDemandAction : IGeneralAction
{
	public short BookTemplateId;

	public byte PageId;

	public sbyte Phase;

	public sbyte ActionEnergyType => 2;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.FindLearnedLifeSkillIndex(Config.SkillBook.Instance[BookTemplateId].LifeSkillTemplateId) < 0;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = selfChar.GetLocation();
		SkillBookItem skillBookItem = Config.SkillBook.Instance[BookTemplateId];
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		if (Phase <= 2)
		{
			monthlyNotificationCollection.AddCheatLifeSkillFailure(id, location, id2, skillBookItem.LifeSkillTemplateId);
			ApplyChanges(context, selfChar, targetChar);
		}
		else
		{
			monthlyEventCollection.AddScamLifeSkill(id, location, id2, 10, BookTemplateId, PageId + 1);
			CharacterDomain.AddLockMovementCharSet(id);
		}
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		SkillBookItem skillBookItem = Config.SkillBook.Instance[BookTemplateId];
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (id != taiwuCharId)
		{
			selfChar.ChangeCurrMainAttribute(context, 2, -GlobalConfig.Instance.HarmfulActionCost);
		}
		if (Phase >= 4)
		{
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddScamLifeSkill(id, id2, skillBookItem.LifeSkillTemplateId);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		switch (Phase)
		{
		case 0:
			lifeRecordCollection.AddScamLifeSkillFail1(id, currDate, id2, location, 10, BookTemplateId, PageId + 1);
			break;
		case 1:
			lifeRecordCollection.AddScamLifeSkillFail2(id, currDate, id2, location, 10, BookTemplateId, PageId + 1);
			break;
		case 2:
			lifeRecordCollection.AddScamLifeSkillFail3(id, currDate, id2, location, 10, BookTemplateId, PageId + 1);
			break;
		case 3:
			lifeRecordCollection.AddScamLifeSkillFail4(id, currDate, id2, location, 10, BookTemplateId, PageId + 1);
			break;
		case 4:
			if (id == taiwuCharId)
			{
				ItemKey itemKey2 = DomainManager.Item.CreateDemandedSkillBook(context, BookTemplateId, PageId, 0);
				selfChar.AddInventoryItem(context, itemKey2, 1);
			}
			selfChar.LearnNewLifeSkill(context, skillBookItem.LifeSkillTemplateId, (byte)(1 << (int)PageId));
			lifeRecordCollection.AddScamLifeSkillSucceed(id, currDate, id2, location, 10, BookTemplateId, PageId + 1);
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
				ItemKey itemKey = DomainManager.Item.CreateDemandedSkillBook(context, BookTemplateId, PageId, 0);
				selfChar.AddInventoryItem(context, itemKey, 1);
			}
			selfChar.LearnNewLifeSkill(context, skillBookItem.LifeSkillTemplateId, (byte)(1 << (int)PageId));
			lifeRecordCollection.AddScamLifeSkillSucceedAndEscaped(id, currDate, id2, location, 10, BookTemplateId, PageId + 1);
			break;
		}
	}
}
