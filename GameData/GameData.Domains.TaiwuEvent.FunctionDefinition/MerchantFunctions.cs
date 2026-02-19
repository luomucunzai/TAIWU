using GameData.Domains.Item;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class MerchantFunctions
{
	[EventFunction(61)]
	private static void ChangeMerchantFavorability(EventScriptRuntime runtime, sbyte merchantType, int delta)
	{
		DomainManager.Merchant.ChangeMerchantCumulativeMoney(runtime.Context, merchantType, delta);
	}

	[EventFunction(153)]
	private static ItemKey CreateMerchantRandomItem(EventScriptRuntime runtime, short merchantTemplateId)
	{
		return DomainManager.Merchant.CreateMerchantRandomItem(runtime.Context, merchantTemplateId);
	}
}
