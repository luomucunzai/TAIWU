namespace GameData.Domains.Combat;

public static class TeammateCommandImplementExtensions
{
	public static bool IsPushOrPull(this ETeammateCommandImplement implement)
	{
		if ((uint)(implement - 2) <= 1u || implement == ETeammateCommandImplement.PushOrPullIntoDanger)
		{
			return true;
		}
		return false;
	}

	public static bool IsAttack(this ETeammateCommandImplement implement)
	{
		if (implement == ETeammateCommandImplement.Attack || implement == ETeammateCommandImplement.GearMateA)
		{
			return true;
		}
		return false;
	}

	public static bool IsDefend(this ETeammateCommandImplement implement)
	{
		if (implement == ETeammateCommandImplement.Defend || implement == ETeammateCommandImplement.GearMateB)
		{
			return true;
		}
		return false;
	}
}
