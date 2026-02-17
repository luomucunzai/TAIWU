using System;
using System.Runtime.CompilerServices;
using GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x0200008F RID: 143
	public static class DynamicActionType
	{
		// Token: 0x0600193E RID: 6462 RVA: 0x0016B9C8 File Offset: 0x00169BC8
		public static MonthlyActionBase CreateDynamicAction(short dynamicActionType)
		{
			MonthlyActionBase result;
			switch (dynamicActionType)
			{
			case 0:
				result = new MarriageTriggerAction();
				break;
			case 1:
				result = new LegendaryBookMonthlyAction();
				break;
			case 2:
				result = new ShixiangStoryAdventureTriggerAction();
				break;
			case 3:
				result = new WuxianStoryAdventureTriggerAction();
				break;
			case 4:
				result = new BaihuaStoryAdventureFourTriggerAction();
				break;
			case 5:
				result = new FulongStoryAdventureOneTriggerAction();
				break;
			case 6:
				result = new FulongStoryAdventureThreeTriggerAction();
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
				defaultInterpolatedStringHandler.AppendFormatted("dynamicActionType");
				defaultInterpolatedStringHandler.AppendLiteral(" has an invalid value of ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(dynamicActionType);
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x040005A0 RID: 1440
		public const short MarriageTriggerAction = 0;

		// Token: 0x040005A1 RID: 1441
		public const short LegendaryBookMonthlyAction = 1;

		// Token: 0x040005A2 RID: 1442
		public const short ShixiangStoryAdventureTriggerAction = 2;

		// Token: 0x040005A3 RID: 1443
		public const short WuxianStoryAdventureTriggerAction = 3;

		// Token: 0x040005A4 RID: 1444
		public const short BaihuaStoryAdventureFourTriggerAction = 4;

		// Token: 0x040005A5 RID: 1445
		public const short FulongStoryAdventureOneTriggerAction = 5;

		// Token: 0x040005A6 RID: 1446
		public const short FulongStoryAdventureThreeTriggerAction = 6;
	}
}
