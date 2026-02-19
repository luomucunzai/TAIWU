namespace GameData.Domains.Combat;

public class CombatStatusType
{
	public static sbyte NotInCombat = 0;

	public static sbyte InCombat = 1;

	public static sbyte SelfFail = 2;

	public static sbyte EnemyFail = 3;

	public static sbyte SelfFlee = 4;

	public static sbyte EnemyFlee = 5;

	public static bool IsCombatOver(sbyte statusType)
	{
		return statusType > InCombat;
	}
}
