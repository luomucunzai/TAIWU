using System;

namespace GameData.Utilities;

public class VersionUtils
{
	public static ulong VersionStringToUlong(string versionStr)
	{
		if (!Version.TryParse(versionStr, out Version result))
		{
			return 0uL;
		}
		return BitOperation.SetSubUlong(BitOperation.SetSubUlong(BitOperation.SetSubUlong(BitOperation.SetSubUlong(0uL, 0, 16, (ulong)ParseVersionNumber(result.Major)), 16, 16, (ulong)ParseVersionNumber(result.Minor)), 32, 16, (ulong)ParseVersionNumber(result.Build)), 48, 16, (ulong)ParseVersionNumber(result.Revision));
	}

	public static string VersionUlongToString(ulong versionUlong)
	{
		ulong subUlong = BitOperation.GetSubUlong(versionUlong, 0, 16);
		ulong subUlong2 = BitOperation.GetSubUlong(versionUlong, 16, 16);
		ulong subUlong3 = BitOperation.GetSubUlong(versionUlong, 32, 16);
		ulong subUlong4 = BitOperation.GetSubUlong(versionUlong, 48, 16);
		return new Version((ushort)subUlong, (ushort)subUlong2, (ushort)subUlong3, (ushort)subUlong4).ToString();
	}

	private static ushort ParseVersionNumber(int versionNumber)
	{
		if (versionNumber < 0 || versionNumber > 65535)
		{
			return 0;
		}
		return (ushort)versionNumber;
	}
}
