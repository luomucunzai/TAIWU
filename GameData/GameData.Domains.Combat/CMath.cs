using GameData.Utilities;

namespace GameData.Domains.Combat;

public static class CMath
{
	public static int ClampFatalMarkCount(int markCount)
	{
		return MathUtils.Clamp(markCount, 0, GlobalConfig.Instance.MaxFatalMarkCount);
	}
}
