namespace GameData.Domains.Merchant;

public static class SharedMethods
{
	public static int GetFavorLevel(int favor)
	{
		if (favor < 60)
		{
			if (favor >= 20)
			{
				if (favor < 40)
				{
					return 1;
				}
				return 2;
			}
			return 0;
		}
		if (favor < 90)
		{
			if (favor < 80)
			{
				return 3;
			}
			return 4;
		}
		if (favor < 100)
		{
			return 5;
		}
		return 6;
	}

	public static int GetBuildingMerchantCaravanId(sbyte type, bool isHead)
	{
		int num = (isHead ? 1 : 2);
		return type - 7 * num - 1;
	}

	public static int GetCaravanRobbedRate(int originRate, bool isInBrokenArea)
	{
		if (!isInBrokenArea)
		{
			return originRate;
		}
		return originRate * 2;
	}

	public static int RealFavorabilityGain(int buyMoney)
	{
		if (buyMoney <= 0 || !ExternalDataBridge.Context.IsProfessionalSkillUnlockedAndEquipped(71))
		{
			return buyMoney;
		}
		return buyMoney * 3;
	}
}
