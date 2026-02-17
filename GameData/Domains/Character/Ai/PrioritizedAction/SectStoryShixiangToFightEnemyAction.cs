using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000867 RID: 2151
	[SerializableGameData(NotForDisplayModule = true)]
	public class SectStoryShixiangToFightEnemyAction : BasePrioritizedAction
	{
		// Token: 0x06007735 RID: 30517 RVA: 0x0045BD1C File Offset: 0x00459F1C
		public static void ResetKilledLimit(DataContext context)
		{
			SectStoryShixiangToFightEnemyAction._killedLimitInMonth = context.Random.Next(6, 13);
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06007736 RID: 30518 RVA: 0x0045BD32 File Offset: 0x00459F32
		public override short ActionType
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x06007737 RID: 30519 RVA: 0x0045BD38 File Offset: 0x00459F38
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			int groupLeader = selfChar.GetLeaderId();
			bool flag = groupLeader >= 0 && groupLeader != selfCharId;
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
		}

		// Token: 0x06007738 RID: 30520 RVA: 0x0045BD74 File Offset: 0x00459F74
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
		}

		// Token: 0x06007739 RID: 30521 RVA: 0x0045BD78 File Offset: 0x00459F78
		public override bool Execute(DataContext context, Character selfChar)
		{
			bool flag = SectStoryShixiangToFightEnemyAction._killedLimitInMonth <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapBlockData block = DomainManager.Map.GetBlock(this.Target.GetRealTargetLocation());
				HashSet<int> enemyCharacterSet = block.EnemyCharacterSet;
				bool flag2 = enemyCharacterSet != null && enemyCharacterSet.Count > 0;
				if (flag2)
				{
					Character enemyChar = null;
					foreach (int enemyId in block.EnemyCharacterSet)
					{
						Character tempEnemyChar;
						bool flag3 = !DomainManager.Character.TryGetElement_Objects(enemyId, out tempEnemyChar);
						if (!flag3)
						{
							short templateId = tempEnemyChar.GetTemplateId();
							bool flag4 = templateId < 608 || templateId > 617;
							bool flag5 = flag4;
							if (!flag5)
							{
								enemyChar = tempEnemyChar;
								break;
							}
						}
					}
					bool flag6 = enemyChar == null;
					if (flag6)
					{
						return false;
					}
					int fightScore = DomainManager.Character.GetSimulateCharacterCombatWinRate(context, selfChar, enemyChar, 1, 1, true);
					bool win = context.Random.CheckPercentProb(fightScore);
					DomainManager.Character.SimulateRandomEnemyAttackNpc(context, enemyChar, selfChar, 2, 2, CombatType.Die);
					bool flag7 = win;
					if (flag7)
					{
						DomainManager.Character.RemoveNonIntelligentCharacter(context, enemyChar);
						SectStoryShixiangToFightEnemyAction._killedLimitInMonth--;
						short templateId = enemyChar.GetTemplateId();
						bool flag8 = templateId >= 613 && templateId <= 617;
						if (flag8)
						{
							EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(6);
							sectArgBox.Set("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount", sectArgBox.GetInt("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount") + 1);
							sectArgBox.Set("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount2", sectArgBox.GetInt("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount2") + 1);
							DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 6);
						}
					}
				}
				else
				{
					short areaId;
					List<short> blockIds;
					DomainManager.World.ShixiangQueryEnemyLocations(context, out areaId, out blockIds);
					bool flag9 = blockIds.Count == 0;
					if (flag9)
					{
						return false;
					}
					this.Target = new NpcTravelTarget(new Location(areaId, blockIds.GetRandom(context.Random)), int.MaxValue);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600773A RID: 30522 RVA: 0x0045BFA0 File Offset: 0x0045A1A0
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
				bool flag2 = selfChar.GetOrganizationInfo().OrgTemplateId != 6;
				if (flag2)
				{
					result = false;
				}
				else
				{
					EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(6);
					result = sectArgBox.GetBool("ConchShip_PresetKey_ShixiangToFightEnemy");
				}
			}
			return result;
		}

		// Token: 0x040020D7 RID: 8407
		private static int _killedLimitInMonth;
	}
}
