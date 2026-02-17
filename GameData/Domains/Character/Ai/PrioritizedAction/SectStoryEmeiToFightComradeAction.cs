using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000866 RID: 2150
	[SerializableGameData(NotForDisplayModule = true)]
	public class SectStoryEmeiToFightComradeAction : BasePrioritizedAction
	{
		// Token: 0x0600772E RID: 30510 RVA: 0x0045BAA4 File Offset: 0x00459CA4
		private bool CheckAndUpdateTarget(DataContext context, Character selfChar, out Character target)
		{
			bool flag = !DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out target);
			bool result;
			if (flag)
			{
				List<int> charIds;
				DomainManager.World.GetEmeiPotentialVictims(selfChar, out charIds);
				bool flag2 = charIds.Count != 0;
				if (flag2)
				{
					this.Target = new NpcTravelTarget(charIds.GetRandom(context.Random), int.MaxValue);
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600772F RID: 30511 RVA: 0x0045BB10 File Offset: 0x00459D10
		public override short ActionType
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x06007730 RID: 30512 RVA: 0x0045BB14 File Offset: 0x00459D14
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			int groupLeader = selfChar.GetLeaderId();
			bool flag = groupLeader >= 0 && groupLeader != selfCharId;
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
			DomainManager.Extra.SectEmeiAddInsaneCharacterId(selfChar.GetId());
		}

		// Token: 0x06007731 RID: 30513 RVA: 0x0045BB61 File Offset: 0x00459D61
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			DomainManager.Extra.SectEmeiRemoveInsaneCharacterId(selfChar.GetId());
		}

		// Token: 0x06007732 RID: 30514 RVA: 0x0045BB78 File Offset: 0x00459D78
		public override bool Execute(DataContext context, Character selfChar)
		{
			Character target;
			bool flag = !this.CheckAndUpdateTarget(context, selfChar, out target);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.Character.SimulateCharacterCombat(context, selfChar, target, CombatType.Die, true, 1);
				DomainManager.Extra.SectEmeiRemoveInsaneCharacterId(selfChar.GetId());
				MonthlyNotificationCollection monthlyNotification = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotification.AddSectMainStoryEmeiInfighting(selfChar.GetId(), selfChar.GetLocation());
				result = true;
			}
			return result;
		}

		// Token: 0x06007733 RID: 30515 RVA: 0x0045BBE0 File Offset: 0x00459DE0
		public override bool CheckValid(Character selfChar)
		{
			bool flag = !base.CheckValid(selfChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Character target;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out target);
				if (flag2)
				{
					result = false;
				}
				else
				{
					OrganizationInfo selfOrgInfo = selfChar.GetOrganizationInfo();
					OrganizationInfo targetOrgInfo = target.GetOrganizationInfo();
					bool flag3 = selfOrgInfo.OrgTemplateId != 2;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = targetOrgInfo.OrgTemplateId != 2;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = selfChar.GetLocation().AreaId != target.GetLocation().AreaId;
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = selfChar.GetLocation().AreaId != DomainManager.Organization.GetSettlementByOrgTemplateId(2).GetLocation().AreaId;
								if (flag6)
								{
									result = false;
								}
								else
								{
									bool flag7 = target.GetAgeGroup() != 2;
									if (flag7)
									{
										result = false;
									}
									else
									{
										bool flag8 = targetOrgInfo.Grade > selfOrgInfo.Grade;
										if (flag8)
										{
											result = false;
										}
										else
										{
											EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
											result = sectArgBox.Contains<int>("ConchShip_PresetKey_EmeiKillEachOtherStage");
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}
	}
}
