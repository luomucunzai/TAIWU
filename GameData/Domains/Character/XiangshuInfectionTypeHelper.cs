using System;

namespace GameData.Domains.Character
{
	// Token: 0x02000818 RID: 2072
	public class XiangshuInfectionTypeHelper
	{
		// Token: 0x060074C8 RID: 29896 RVA: 0x00446D98 File Offset: 0x00444F98
		public static short GetInfectionFeatureIdThatShouldBe(byte xiangshuInfection)
		{
			bool flag = xiangshuInfection < 100;
			short result;
			if (flag)
			{
				result = 216;
			}
			else
			{
				bool flag2 = xiangshuInfection < 200;
				if (flag2)
				{
					result = 217;
				}
				else
				{
					result = 218;
				}
			}
			return result;
		}
	}
}
