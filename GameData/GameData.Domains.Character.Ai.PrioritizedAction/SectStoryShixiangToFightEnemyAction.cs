using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class SectStoryShixiangToFightEnemyAction : BasePrioritizedAction
{
	private static int _killedLimitInMonth;

	public override short ActionType => 12;

	public static void ResetKilledLimit(DataContext context)
	{
		_killedLimitInMonth = context.Random.Next(6, 13);
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		int leaderId = selfChar.GetLeaderId();
		if (leaderId >= 0 && leaderId != id)
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		if (_killedLimitInMonth <= 0)
		{
			return false;
		}
		MapBlockData block = DomainManager.Map.GetBlock(Target.GetRealTargetLocation());
		HashSet<int> enemyCharacterSet = block.EnemyCharacterSet;
		if (enemyCharacterSet != null && enemyCharacterSet.Count > 0)
		{
			Character character = null;
			foreach (int item in block.EnemyCharacterSet)
			{
				if (DomainManager.Character.TryGetElement_Objects(item, out var element))
				{
					short templateId = element.GetTemplateId();
					if ((templateId >= 608 && templateId <= 617) || 1 == 0)
					{
						character = element;
						break;
					}
				}
			}
			if (character == null)
			{
				return false;
			}
			int simulateCharacterCombatWinRate = DomainManager.Character.GetSimulateCharacterCombatWinRate(context, selfChar, character);
			bool flag = context.Random.CheckPercentProb(simulateCharacterCombatWinRate);
			DomainManager.Character.SimulateRandomEnemyAttackNpc(context, character, selfChar, 2, 2, CombatType.Die);
			if (flag)
			{
				DomainManager.Character.RemoveNonIntelligentCharacter(context, character);
				_killedLimitInMonth--;
				short templateId = character.GetTemplateId();
				if (templateId >= 613 && templateId <= 617)
				{
					EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(6);
					sectMainStoryEventArgBox.Set("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount", sectMainStoryEventArgBox.GetInt("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount") + 1);
					sectMainStoryEventArgBox.Set("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount2", sectMainStoryEventArgBox.GetInt("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount2") + 1);
					DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 6);
				}
			}
		}
		else
		{
			DomainManager.World.ShixiangQueryEnemyLocations(context, out var areaId, out var blockIds);
			if (blockIds.Count == 0)
			{
				return false;
			}
			Target = new NpcTravelTarget(new Location(areaId, blockIds.GetRandom(context.Random)), int.MaxValue);
		}
		return false;
	}

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		if (selfChar.GetOrganizationInfo().OrgTemplateId != 6)
		{
			return false;
		}
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(6);
		return sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_ShixiangToFightEnemy");
	}
}
