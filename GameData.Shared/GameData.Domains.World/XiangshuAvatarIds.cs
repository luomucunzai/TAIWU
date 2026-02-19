using Config;
using GameData.Utilities;

namespace GameData.Domains.World;

public static class XiangshuAvatarIds
{
	public const sbyte Monv = 0;

	public const sbyte DayueYaochang = 1;

	public const sbyte Jiuhan = 2;

	public const sbyte JinHuanger = 3;

	public const sbyte YiYihou = 4;

	public const sbyte WeiQi = 5;

	public const sbyte Yixiang = 6;

	public const sbyte Xuefeng = 7;

	public const sbyte ShuFang = 8;

	public const int Count = 9;

	public const int TombsCount = 7;

	public static short[] JuniorXiangshuTemplateIds = new short[9] { 39, 40, 41, 42, 43, 44, 45, 46, 47 };

	public static short[] XiangshuPuppetTemplateIds = new short[9] { 490, 491, 492, 493, 494, 495, 496, 497, 498 };

	public static short[] XiangshuBossBeginIds = new short[9] { 48, 57, 66, 75, 84, 93, 102, 111, 120 };

	public static short[] XiangshuBossEndIds = new short[9] { 56, 65, 74, 83, 92, 101, 110, 119, 128 };

	public static short[] WeakenedXiangshuBossBeginIds = new short[9] { 129, 138, 147, 156, 165, 174, 183, 192, 201 };

	public static readonly short[] WeakenedXiangshuBossEndIds = new short[9] { 137, 146, 155, 164, 173, 182, 191, 200, 209 };

	public static readonly short[] SwordTombAdventureTemplateIds = new short[9] { 114, 111, 108, 110, 109, 113, 116, 115, 112 };

	public static readonly short[] SwordTombBlockTemplateIds = new short[9] { 128, 129, 130, 131, 132, 133, 134, 135, 136 };

	public static string GetJuniorXiangshuAvatarName(sbyte xiangshuAvatarId)
	{
		short index = JuniorXiangshuTemplateIds[xiangshuAvatarId];
		return Config.Character.Instance[index].FixedAvatarName;
	}

	public static string GetXiangshuPuppetAvatarName(sbyte xiangshuAvatarId)
	{
		short index = XiangshuPuppetTemplateIds[xiangshuAvatarId];
		return Config.Character.Instance[index].FixedAvatarName;
	}

	public static short GetCurrentLevelXiangshuTemplateId(sbyte xiangshuAvatarId, sbyte xiangshuLevel, bool isWeakened = false)
	{
		short num = (isWeakened ? WeakenedXiangshuBossBeginIds[xiangshuAvatarId] : XiangshuBossBeginIds[xiangshuAvatarId]);
		short max = (isWeakened ? WeakenedXiangshuBossEndIds[xiangshuAvatarId] : XiangshuBossEndIds[xiangshuAvatarId]);
		return MathUtils.Clamp((short)(num + xiangshuLevel), num, max);
	}

	public static sbyte GetXiangshuAvatarIdByCharacterTemplateId(short characterTemplateId)
	{
		for (sbyte b = 0; b < 9; b++)
		{
			if (characterTemplateId == JuniorXiangshuTemplateIds[b])
			{
				return b;
			}
			if (characterTemplateId >= XiangshuBossBeginIds[b] && characterTemplateId <= XiangshuBossEndIds[b])
			{
				return b;
			}
			if (characterTemplateId >= WeakenedXiangshuBossBeginIds[b] && characterTemplateId <= WeakenedXiangshuBossEndIds[b])
			{
				return b;
			}
		}
		return -1;
	}

	public static sbyte GetXiangshuAvatarIdBySwordTomb(short adventureId)
	{
		return (sbyte)SwordTombAdventureTemplateIds.IndexOf(adventureId);
	}
}
