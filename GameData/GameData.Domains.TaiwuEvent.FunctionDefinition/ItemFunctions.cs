using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class ItemFunctions
{
	[EventFunction(62)]
	private static ItemKey CreateItem(EventScriptRuntime runtime, UnmanagedVariant<TemplateKey> itemTemplate)
	{
		return DomainManager.Item.CreateItem(runtime.Context, ((Variant<TemplateKey>)(object)itemTemplate).Value.ItemType, ((Variant<TemplateKey>)(object)itemTemplate).Value.TemplateId);
	}

	[EventFunction(166)]
	private static ItemKey CreateFixedSkillBook(EventScriptRuntime runtime, UnmanagedVariant<TemplateKey> itemTemplate)
	{
		sbyte behaviorType = DomainManager.Taiwu.GetTaiwu().GetBehaviorType();
		return DomainManager.Item.CreateSkillBook(runtime.Context, ((Variant<TemplateKey>)(object)itemTemplate).Value.TemplateId, 5, 0, behaviorType, 100);
	}

	[EventFunction(63)]
	private static ItemKey CreateCricket(EventScriptRuntime runtime, short colorId, short partId)
	{
		return DomainManager.Item.CreateCricket(runtime.Context, colorId, partId);
	}
}
