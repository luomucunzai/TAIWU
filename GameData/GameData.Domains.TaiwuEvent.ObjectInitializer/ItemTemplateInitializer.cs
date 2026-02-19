using CompDevLib.Interpreter;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.ObjectInitializer;

public class ItemTemplateInitializer : IObjectInitializer
{
	public object CreateInstance()
	{
		return new UnmanagedVariant<TemplateKey>();
	}

	public void SetField(object instance, string fieldName, object value)
	{
		UnmanagedVariant<TemplateKey> val = (UnmanagedVariant<TemplateKey>)instance;
		if (!(fieldName == "ItemType"))
		{
			if (fieldName == "TemplateId")
			{
				((Variant<TemplateKey>)(object)val).Value = new TemplateKey(((Variant<TemplateKey>)(object)val).Value.ItemType, (short)value);
			}
		}
		else
		{
			((Variant<TemplateKey>)(object)val).Value = new TemplateKey((sbyte)value, ((Variant<TemplateKey>)(object)val).Value.TemplateId);
		}
	}
}
