using Config;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true, IsExtensible = true)]
public class SeekAsylumAction : ExtensiblePrioritizedAction
{
	private static class FieldIds
	{
		public const ushort Target = 0;

		public const ushort HasArrived = 1;

		public const ushort SettlementId = 2;

		public const ushort CurrChance = 3;

		public const ushort Count = 4;

		public static readonly string[] FieldId2FieldName = new string[4] { "Target", "HasArrived", "SettlementId", "CurrChance" };
	}

	[SerializableGameDataField]
	public short SettlementId;

	[SerializableGameDataField]
	public sbyte CurrChance = 5;

	private const int BonusChancePerMonth = 5;

	public override short ActionType => 19;

	public override bool CheckValid(Character selfChar)
	{
		if (selfChar.GetOrganizationInfo().OrgTemplateId != 0)
		{
			return false;
		}
		sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(selfChar.GetId());
		if (fugitiveBountySect < 0)
		{
			return false;
		}
		return base.CheckValid(selfChar);
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		if (selfChar.GetLeaderId() >= 0)
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
		string name = DomainManager.Organization.GetSettlement(SettlementId).GetNameRelatedData().GetName();
		AdaptableLog.Info($"{selfChar} 开始前往 {name} 寻求庇护.");
		int id = selfChar.GetId();
		sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(id);
		Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(fugitiveBountySect);
		SettlementBounty bounty = sect.Prison.GetBounty(id);
		Location location = selfChar.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddDecideToSeekAsylum(id, currDate, location, bounty.PunishmentType, sect.GetId(), SettlementId);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		AdaptableLog.Info($"{selfChar} 终止寻求庇护.");
		int id = selfChar.GetId();
		Location location = selfChar.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddFinishSeekAsylum(id, currDate, location, SettlementId);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		if (Config.Organization.Instance[organizationInfo.OrgTemplateId].IsSect)
		{
			return true;
		}
		if (!context.Random.CheckPercentProb(CurrChance))
		{
			CurrChance += 5;
			return false;
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(SettlementId);
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgTemplateId, organizationInfo.Grade);
		sbyte rejoinGrade = orgMemberConfig.GetRejoinGrade();
		OrganizationInfo destOrgInfo = new OrganizationInfo(orgTemplateId, rejoinGrade, principal: true, SettlementId);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		int id = selfChar.GetId();
		Location location = selfChar.GetLocation();
		sbyte gender = selfChar.GetGender();
		lifeRecordCollection.AddSeekAsylumSuccess(id, currDate, location, SettlementId, destOrgInfo.OrgTemplateId, destOrgInfo.Grade, orgPrincipal: true, gender);
		DomainManager.Organization.ChangeOrganization(context, selfChar, destOrgInfo);
		if (DomainManager.Character.IsTaiwuPeople(id))
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddJoinOrganization(id, SettlementId);
		}
		AdaptableLog.Info($"{selfChar} 成功获得 {settlement} 的庇护.");
		return true;
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 22;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 4;
		ptr += 2;
		ptr += Target.Serialize(ptr);
		*ptr = (HasArrived ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = SettlementId;
		ptr += 2;
		*ptr = (byte)CurrChance;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ptr += Target.Deserialize(ptr);
		}
		if (num > 1)
		{
			HasArrived = *ptr != 0;
			ptr++;
		}
		if (num > 2)
		{
			SettlementId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 3)
		{
			CurrChance = (sbyte)(*ptr);
			ptr++;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
