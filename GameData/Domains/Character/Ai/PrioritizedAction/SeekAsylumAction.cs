using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000869 RID: 2153
	[SerializableGameData(NotForDisplayModule = true, IsExtensible = true)]
	public class SeekAsylumAction : ExtensiblePrioritizedAction
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06007742 RID: 30530 RVA: 0x0045C074 File Offset: 0x0045A274
		public override short ActionType
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x06007743 RID: 30531 RVA: 0x0045C078 File Offset: 0x0045A278
		public override bool CheckValid(Character selfChar)
		{
			bool flag = selfChar.GetOrganizationInfo().OrgTemplateId != 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte bountySectTemplateId = DomainManager.Organization.GetFugitiveBountySect(selfChar.GetId());
				bool flag2 = bountySectTemplateId < 0;
				result = (!flag2 && base.CheckValid(selfChar));
			}
			return result;
		}

		// Token: 0x06007744 RID: 30532 RVA: 0x0045C0C4 File Offset: 0x0045A2C4
		public override void OnStart(DataContext context, Character selfChar)
		{
			bool flag = selfChar.GetLeaderId() >= 0;
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
			string targetName = DomainManager.Organization.GetSettlement(this.SettlementId).GetNameRelatedData().GetName();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 2);
			defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
			defaultInterpolatedStringHandler.AppendLiteral(" 开始前往 ");
			defaultInterpolatedStringHandler.AppendFormatted(targetName);
			defaultInterpolatedStringHandler.AppendLiteral(" 寻求庇护.");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			int selfCharId = selfChar.GetId();
			sbyte sectOrgTemplateId = DomainManager.Organization.GetFugitiveBountySect(selfCharId);
			Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(sectOrgTemplateId);
			SettlementBounty bounty = sect.Prison.GetBounty(selfCharId);
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecords.AddDecideToSeekAsylum(selfCharId, currDate, location, bounty.PunishmentType, sect.GetId(), this.SettlementId);
		}

		// Token: 0x06007745 RID: 30533 RVA: 0x0045C1C8 File Offset: 0x0045A3C8
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
			defaultInterpolatedStringHandler.AppendLiteral(" 终止寻求庇护.");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			int selfCharId = selfChar.GetId();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecords.AddFinishSeekAsylum(selfCharId, currDate, location, this.SettlementId);
		}

		// Token: 0x06007746 RID: 30534 RVA: 0x0045C238 File Offset: 0x0045A438
		public override bool Execute(DataContext context, Character selfChar)
		{
			OrganizationInfo selfOrgInfo = selfChar.GetOrganizationInfo();
			bool isSect = Organization.Instance[selfOrgInfo.OrgTemplateId].IsSect;
			bool result;
			if (isSect)
			{
				result = true;
			}
			else
			{
				bool flag = !context.Random.CheckPercentProb((int)this.CurrChance);
				if (flag)
				{
					this.CurrChance += 5;
					result = false;
				}
				else
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(this.SettlementId);
					sbyte orgTemplateId = settlement.GetOrgTemplateId();
					OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgTemplateId, selfOrgInfo.Grade);
					sbyte rejoinGrade = orgMemberCfg.GetRejoinGrade();
					OrganizationInfo targetOrgInfo = new OrganizationInfo(orgTemplateId, rejoinGrade, true, this.SettlementId);
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					int currDate = DomainManager.World.GetCurrDate();
					int selfCharId = selfChar.GetId();
					Location currLocation = selfChar.GetLocation();
					sbyte gender = selfChar.GetGender();
					lifeRecordCollection.AddSeekAsylumSuccess(selfCharId, currDate, currLocation, this.SettlementId, targetOrgInfo.OrgTemplateId, targetOrgInfo.Grade, true, gender);
					DomainManager.Organization.ChangeOrganization(context, selfChar, targetOrgInfo);
					bool flag2 = DomainManager.Character.IsTaiwuPeople(selfCharId);
					if (flag2)
					{
						MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
						monthlyNotifications.AddJoinOrganization(selfCharId, this.SettlementId);
					}
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 2);
					defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
					defaultInterpolatedStringHandler.AppendLiteral(" 成功获得 ");
					defaultInterpolatedStringHandler.AppendFormatted<Settlement>(settlement);
					defaultInterpolatedStringHandler.AppendLiteral(" 的庇护.");
					AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06007747 RID: 30535 RVA: 0x0045C3C0 File Offset: 0x0045A5C0
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06007748 RID: 30536 RVA: 0x0045C3D4 File Offset: 0x0045A5D4
		public override int GetSerializedSize()
		{
			int totalSize = 22;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007749 RID: 30537 RVA: 0x0045C3FC File Offset: 0x0045A5FC
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = 4;
			byte* pCurrData = pData + 2;
			pCurrData += this.Target.Serialize(pCurrData);
			*pCurrData = (this.HasArrived ? 1 : 0);
			pCurrData++;
			*(short*)pCurrData = this.SettlementId;
			pCurrData += 2;
			*pCurrData = (byte)this.CurrChance;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600774A RID: 30538 RVA: 0x0045C464 File Offset: 0x0045A664
		public unsafe override int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				pCurrData += this.Target.Deserialize(pCurrData);
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				this.HasArrived = (*pCurrData != 0);
				pCurrData++;
			}
			bool flag3 = fieldCount > 2;
			if (flag3)
			{
				this.SettlementId = *(short*)pCurrData;
				pCurrData += 2;
			}
			bool flag4 = fieldCount > 3;
			if (flag4)
			{
				this.CurrChance = *(sbyte*)pCurrData;
				pCurrData++;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040020D8 RID: 8408
		[SerializableGameDataField]
		public short SettlementId;

		// Token: 0x040020D9 RID: 8409
		[SerializableGameDataField]
		public sbyte CurrChance = 5;

		// Token: 0x040020DA RID: 8410
		private const int BonusChancePerMonth = 5;

		// Token: 0x02000C3E RID: 3134
		private static class FieldIds
		{
			// Token: 0x04003570 RID: 13680
			public const ushort Target = 0;

			// Token: 0x04003571 RID: 13681
			public const ushort HasArrived = 1;

			// Token: 0x04003572 RID: 13682
			public const ushort SettlementId = 2;

			// Token: 0x04003573 RID: 13683
			public const ushort CurrChance = 3;

			// Token: 0x04003574 RID: 13684
			public const ushort Count = 4;

			// Token: 0x04003575 RID: 13685
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"Target",
				"HasArrived",
				"SettlementId",
				"CurrChance"
			};
		}
	}
}
