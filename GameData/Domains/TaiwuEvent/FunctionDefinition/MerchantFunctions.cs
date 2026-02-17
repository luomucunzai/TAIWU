using System;
using GameData.Domains.Item;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000A8 RID: 168
	public class MerchantFunctions
	{
		// Token: 0x06001B1E RID: 6942 RVA: 0x0017B16E File Offset: 0x0017936E
		[EventFunction(61)]
		private static void ChangeMerchantFavorability(EventScriptRuntime runtime, sbyte merchantType, int delta)
		{
			DomainManager.Merchant.ChangeMerchantCumulativeMoney(runtime.Context, merchantType, delta);
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0017B184 File Offset: 0x00179384
		[EventFunction(153)]
		private static ItemKey CreateMerchantRandomItem(EventScriptRuntime runtime, short merchantTemplateId)
		{
			return DomainManager.Merchant.CreateMerchantRandomItem(runtime.Context, merchantTemplateId);
		}
	}
}
