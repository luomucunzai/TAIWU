namespace GameData.Domains.World;

public static class AdvancingMonthState
{
	public const sbyte NotInProcess = 0;

	public const sbyte PreAdvancingLastMonthEnding = 1;

	public const sbyte PeriAdvancingUpdateCharacterStatus = 2;

	public const sbyte PeriAdvancingUpdateRandomEnemies = 3;

	public const sbyte PeriAdvancingCharacterSelfImprovement = 4;

	public const sbyte PeriAdvancingCharacterActivePreparation = 5;

	public const sbyte PeriAdvancingCharacterPassivePreparation = 6;

	public const sbyte PeriAdvancingCharacterRelationsUpdate = 7;

	public const sbyte PeriAdvancingCharacterPersonalNeedsProcessing = 8;

	public const sbyte PeriAdvancingCharacterPrioritizedAction = 9;

	public const sbyte PeriAdvancingCharacterGeneralAction = 10;

	public const sbyte PeriAdvancingCharacterFixedAction = 11;

	public const sbyte PeriAdvancingInformationSpreading = 12;

	public const sbyte PostAdvancingEnterNewMonth = 13;

	public const sbyte DisplayingMonthlyNotifications = 20;
}
