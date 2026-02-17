using System;
using System.Collections.Generic;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x0200024D RID: 589
	public static class BreakFeatureHelper
	{
		// Token: 0x0600300D RID: 12301 RVA: 0x00215B2C File Offset: 0x00213D2C
		// Note: this type is marked as 'beforefieldinit'.
		static BreakFeatureHelper()
		{
			Dictionary<sbyte, short> dictionary = new Dictionary<sbyte, short>();
			dictionary[0] = 249;
			dictionary[1] = 251;
			dictionary[2] = 247;
			dictionary[3] = 253;
			dictionary[4] = 253;
			dictionary[5] = 255;
			dictionary[6] = 255;
			BreakFeatureHelper.BodyPart2CrashFeature = dictionary;
			Dictionary<sbyte, short> dictionary2 = new Dictionary<sbyte, short>();
			dictionary2[0] = 248;
			dictionary2[1] = 250;
			dictionary2[2] = 246;
			dictionary2[3] = 252;
			dictionary2[4] = 252;
			dictionary2[5] = 254;
			dictionary2[6] = 254;
			BreakFeatureHelper.BodyPart2HurtFeature = dictionary2;
			Dictionary<short, sbyte[]> dictionary3 = new Dictionary<short, sbyte[]>();
			dictionary3[249] = new sbyte[1];
			dictionary3[251] = new sbyte[]
			{
				1
			};
			dictionary3[247] = new sbyte[]
			{
				2
			};
			dictionary3[253] = new sbyte[]
			{
				3,
				4
			};
			dictionary3[255] = new sbyte[]
			{
				5,
				6
			};
			dictionary3[248] = new sbyte[1];
			dictionary3[250] = new sbyte[]
			{
				1
			};
			dictionary3[246] = new sbyte[]
			{
				2
			};
			dictionary3[252] = new sbyte[]
			{
				3,
				4
			};
			dictionary3[254] = new sbyte[]
			{
				5,
				6
			};
			BreakFeatureHelper.Feature2BodyPart = dictionary3;
		}

		// Token: 0x04000E43 RID: 3651
		public static readonly short[] AllCrashFeature = new short[]
		{
			249,
			251,
			247,
			253,
			255
		};

		// Token: 0x04000E44 RID: 3652
		public static readonly short[] AllHurtFeature = new short[]
		{
			248,
			250,
			246,
			252,
			254
		};

		// Token: 0x04000E45 RID: 3653
		public static readonly Dictionary<sbyte, short> BodyPart2CrashFeature;

		// Token: 0x04000E46 RID: 3654
		public static readonly Dictionary<sbyte, short> BodyPart2HurtFeature;

		// Token: 0x04000E47 RID: 3655
		public static readonly Dictionary<short, sbyte[]> Feature2BodyPart;
	}
}
