namespace GameData.Domains.Character;

public class XiangshuInfectionTypeHelper
{
	public static short GetInfectionFeatureIdThatShouldBe(byte xiangshuInfection)
	{
		if (xiangshuInfection < 100)
		{
			return 216;
		}
		if (xiangshuInfection < 200)
		{
			return 217;
		}
		return 218;
	}
}
