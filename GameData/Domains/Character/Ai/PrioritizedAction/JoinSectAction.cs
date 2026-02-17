using System;
using Config;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x0200085F RID: 2143
	[SerializableGameData(NotForDisplayModule = true)]
	public class JoinSectAction : BasePrioritizedAction
	{
		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06007701 RID: 30465 RVA: 0x0045A403 File Offset: 0x00458603
		public override short ActionType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06007702 RID: 30466 RVA: 0x0045A408 File Offset: 0x00458608
		public override void OnStart(DataContext context, Character selfChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			int selfCharId = selfChar.GetId();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddDecideToJoinSect(selfCharId, currDate, currLocation, this.SettlementId);
			bool flag = DomainManager.Character.IsTaiwuPeople(selfCharId);
			if (flag)
			{
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotifications.AddGoToJoinOrganization(selfCharId, this.SettlementId);
			}
		}

		// Token: 0x06007703 RID: 30467 RVA: 0x0045A478 File Offset: 0x00458678
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			int selfCharId = selfChar.GetId();
			lifeRecordCollection.AddJoinSectFail(selfCharId, currDate, this.SettlementId);
		}

		// Token: 0x06007704 RID: 30468 RVA: 0x0045A4B4 File Offset: 0x004586B4
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
				Settlement settlement = DomainManager.Organization.GetSettlement(this.SettlementId);
				OrganizationInfo targetOrgInfo = new OrganizationInfo(settlement.GetOrgTemplateId(), 0, true, this.SettlementId);
				OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(targetOrgInfo);
				sbyte selfGender = selfChar.GetGender();
				bool flag = !OrganizationDomain.MeetGenderRestriction(targetOrgInfo.OrgTemplateId, selfGender);
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = orgMemberCfg.Gender >= 0 && orgMemberCfg.Gender != selfGender;
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool flag3 = orgMemberCfg.ChildGrade < 0 && (DomainManager.Character.GetAliveSpouse(selfChar.GetId()) >= 0 || DomainManager.Character.GetAliveChild(selfChar.GetId()) >= 0);
						if (flag3)
						{
							result = false;
						}
						else
						{
							int successRate = this.GetJoinOrgSuccessRate(context.Random, selfChar, targetOrgInfo);
							bool flag4 = !context.Random.CheckPercentProb(successRate);
							if (flag4)
							{
								result = false;
							}
							else
							{
								DomainManager.Organization.JoinSect(context, selfChar, targetOrgInfo);
								int selfCharId = selfChar.GetId();
								bool flag5 = DomainManager.Character.IsTaiwuPeople(selfCharId);
								if (flag5)
								{
									MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
									monthlyNotifications.AddJoinOrganization(selfCharId, this.SettlementId);
								}
								result = true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007705 RID: 30469 RVA: 0x0045A624 File Offset: 0x00458824
		private unsafe int GetJoinOrgSuccessRate(IRandomSource random, Character selfChar, OrganizationInfo targetOrgInfo)
		{
			int score = 0;
			OrganizationItem orgCfg = Organization.Instance[targetOrgInfo.OrgTemplateId];
			OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(targetOrgInfo);
			int combatSkillScore = 0;
			int scoredCombatSkillTypeCount = 0;
			CombatSkillShorts combatSkillQualifications = *selfChar.GetCombatSkillQualifications();
			for (sbyte combatSkillType = 0; combatSkillType < 14; combatSkillType += 1)
			{
				short adjust = orgMemberCfg.CombatSkillsAdjust[(int)combatSkillType];
				bool flag = adjust < 6;
				if (!flag)
				{
					combatSkillScore += (int)(*(ref combatSkillQualifications.Items.FixedElementField + (IntPtr)combatSkillType * 2) * adjust / 12);
					scoredCombatSkillTypeCount++;
				}
			}
			bool flag2 = scoredCombatSkillTypeCount > 0;
			if (flag2)
			{
				score += combatSkillScore / scoredCombatSkillTypeCount;
			}
			int lifeSkillScore = 0;
			int scoredLifeSkillTypeCount = 0;
			LifeSkillShorts lifeSkillQualifications = *selfChar.GetLifeSkillQualifications();
			for (sbyte lifeSkillType = 0; lifeSkillType < 16; lifeSkillType += 1)
			{
				short adjust2 = orgMemberCfg.LifeSkillsAdjust[(int)lifeSkillType];
				bool flag3 = adjust2 < 6;
				if (!flag3)
				{
					lifeSkillScore += (int)(*(ref lifeSkillQualifications.Items.FixedElementField + (IntPtr)lifeSkillType * 2) * adjust2 / 12);
					scoredLifeSkillTypeCount++;
				}
			}
			bool flag4 = scoredLifeSkillTypeCount > 0;
			if (flag4)
			{
				score += lifeSkillScore / scoredLifeSkillTypeCount;
			}
			int mainAttrScore = 0;
			int scoredMainAttributeTypeCount = 0;
			MainAttributes mainAttributes = selfChar.GetMaxMainAttributes();
			for (sbyte mainAttrType = 0; mainAttrType < 6; mainAttrType += 1)
			{
				short adjust3 = orgMemberCfg.MainAttributesAdjust[(int)mainAttrType];
				bool flag5 = adjust3 < 6;
				if (!flag5)
				{
					mainAttrScore += (int)(*(ref mainAttributes.Items.FixedElementField + (IntPtr)mainAttrType * 2) * adjust3 / 12);
					scoredMainAttributeTypeCount++;
				}
			}
			bool flag6 = scoredMainAttributeTypeCount > 0;
			if (flag6)
			{
				score += mainAttrScore / scoredMainAttributeTypeCount;
			}
			sbyte charFameType = selfChar.GetFameType();
			bool flag7 = charFameType == -2;
			if (flag7)
			{
				charFameType = (random.NextBool() ? 4 : 2);
			}
			int num = score;
			sbyte goodness = orgCfg.Goodness;
			if (!true)
			{
			}
			int num2;
			switch (goodness)
			{
			case -1:
				num2 = (int)(25 * (3 - charFameType));
				break;
			case 0:
				num2 = 75 - 25 * Math.Abs((int)(charFameType - 3));
				break;
			case 1:
				num2 = (int)(25 * (charFameType - 3));
				break;
			default:
				num2 = 0;
				break;
			}
			if (!true)
			{
			}
			score = num + num2;
			sbyte charBehaviorType = selfChar.GetBehaviorType();
			sbyte orgBehaviorType = BehaviorType.GetBehaviorType((short)orgCfg.Goodness);
			bool flag8 = charBehaviorType == orgBehaviorType;
			if (flag8)
			{
				score += 50;
			}
			else
			{
				bool flag9 = BehaviorType.IsContradictory(charBehaviorType, orgBehaviorType);
				if (flag9)
				{
					score -= 50;
				}
			}
			return score / 5;
		}

		// Token: 0x06007707 RID: 30471 RVA: 0x0045A888 File Offset: 0x00458A88
		public override bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06007708 RID: 30472 RVA: 0x0045A89C File Offset: 0x00458A9C
		public override int GetSerializedSize()
		{
			int totalSize = 19;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007709 RID: 30473 RVA: 0x0045A8C4 File Offset: 0x00458AC4
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = this.SettlementId;
			byte* pCurrData = pData + 2;
			pCurrData += this.Target.Serialize(pCurrData);
			*pCurrData = (this.HasArrived ? 1 : 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600770A RID: 30474 RVA: 0x0045A918 File Offset: 0x00458B18
		public unsafe override int Deserialize(byte* pData)
		{
			this.SettlementId = *(short*)pData;
			byte* pCurrData = pData + 2;
			pCurrData += this.Target.Deserialize(pCurrData);
			this.HasArrived = (*pCurrData != 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040020CB RID: 8395
		[SerializableGameDataField]
		public short SettlementId;
	}
}
