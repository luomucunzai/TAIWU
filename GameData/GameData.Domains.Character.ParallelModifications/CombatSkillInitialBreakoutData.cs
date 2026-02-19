using GameData.Domains.CombatSkill;

namespace GameData.Domains.Character.ParallelModifications;

public struct CombatSkillInitialBreakoutData
{
	public readonly GameData.Domains.CombatSkill.CombatSkill CombatSkill;

	public readonly ushort ActivationState;

	public readonly sbyte BreakoutStepsCount;

	public readonly sbyte ForceBreakoutStepsCount;

	public short ObtainedNeili;

	public CombatSkillInitialBreakoutData(GameData.Domains.CombatSkill.CombatSkill combatSkill, ushort activationState, sbyte breakoutStepsCount, sbyte forceBreakoutStepsCount)
	{
		CombatSkill = combatSkill;
		ActivationState = activationState;
		BreakoutStepsCount = breakoutStepsCount;
		ForceBreakoutStepsCount = forceBreakoutStepsCount;
		ObtainedNeili = 0;
	}
}
