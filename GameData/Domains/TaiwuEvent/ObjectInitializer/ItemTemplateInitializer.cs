using System;
using CompDevLib.Interpreter;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.ObjectInitializer
{
	// Token: 0x02000089 RID: 137
	public class ItemTemplateInitializer : IObjectInitializer
	{
		// Token: 0x060018E9 RID: 6377 RVA: 0x00167FA0 File Offset: 0x001661A0
		public object CreateInstance()
		{
			return new UnmanagedVariant<TemplateKey>();
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x00167FB8 File Offset: 0x001661B8
		public void SetField(object instance, string fieldName, object value)
		{
			UnmanagedVariant<TemplateKey> templateKey = (UnmanagedVariant<TemplateKey>)instance;
			if (!(fieldName == "ItemType"))
			{
				if (fieldName == "TemplateId")
				{
					templateKey.Value = new TemplateKey(templateKey.Value.ItemType, (short)value);
				}
			}
			else
			{
				templateKey.Value = new TemplateKey((sbyte)value, templateKey.Value.TemplateId);
			}
		}
	}
}
