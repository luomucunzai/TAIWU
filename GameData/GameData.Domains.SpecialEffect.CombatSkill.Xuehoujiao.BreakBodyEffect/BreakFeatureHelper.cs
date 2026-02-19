using System.Collections.Generic;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;

public static class BreakFeatureHelper
{
	public static readonly short[] AllCrashFeature = new short[5] { 249, 251, 247, 253, 255 };

	public static readonly short[] AllHurtFeature = new short[5] { 248, 250, 246, 252, 254 };

	public static readonly Dictionary<sbyte, short> BodyPart2CrashFeature = new Dictionary<sbyte, short>
	{
		[0] = 249,
		[1] = 251,
		[2] = 247,
		[3] = 253,
		[4] = 253,
		[5] = 255,
		[6] = 255
	};

	public static readonly Dictionary<sbyte, short> BodyPart2HurtFeature = new Dictionary<sbyte, short>
	{
		[0] = 248,
		[1] = 250,
		[2] = 246,
		[3] = 252,
		[4] = 252,
		[5] = 254,
		[6] = 254
	};

	public static readonly Dictionary<short, sbyte[]> Feature2BodyPart = new Dictionary<short, sbyte[]>
	{
		[249] = new sbyte[1],
		[251] = new sbyte[1] { 1 },
		[247] = new sbyte[1] { 2 },
		[253] = new sbyte[2] { 3, 4 },
		[255] = new sbyte[2] { 5, 6 },
		[248] = new sbyte[1],
		[250] = new sbyte[1] { 1 },
		[246] = new sbyte[1] { 2 },
		[252] = new sbyte[2] { 3, 4 },
		[254] = new sbyte[2] { 5, 6 }
	};
}
