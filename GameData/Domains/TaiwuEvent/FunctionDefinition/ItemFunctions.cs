using System;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000A6 RID: 166
	public class ItemFunctions
	{
		// Token: 0x06001B10 RID: 6928 RVA: 0x0017AEDC File Offset: 0x001790DC
		[EventFunction(62)]
		private static ItemKey CreateItem(EventScriptRuntime runtime, UnmanagedVariant<TemplateKey> itemTemplate)
		{
			return DomainManager.Item.CreateItem(runtime.Context, itemTemplate.Value.ItemType, itemTemplate.Value.TemplateId);
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x0017AF14 File Offset: 0x00179114
		[EventFunction(166)]
		private static ItemKey CreateFixedSkillBook(EventScriptRuntime runtime, UnmanagedVariant<TemplateKey> itemTemplate)
		{
			sbyte outlinePageType = DomainManager.Taiwu.GetTaiwu().GetBehaviorType();
			return DomainManager.Item.CreateSkillBook(runtime.Context, itemTemplate.Value.TemplateId, 5, 0, outlinePageType, 100, true);
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x0017AF58 File Offset: 0x00179158
		[EventFunction(63)]
		private static ItemKey CreateCricket(EventScriptRuntime runtime, short colorId, short partId)
		{
			return DomainManager.Item.CreateCricket(runtime.Context, colorId, partId);
		}
	}
}
