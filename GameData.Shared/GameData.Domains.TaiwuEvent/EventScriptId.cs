using System;
using Config;

namespace GameData.Domains.TaiwuEvent;

public readonly struct EventScriptId : IEquatable<EventScriptId>
{
	public readonly sbyte Type;

	public readonly string Guid;

	public readonly string SubGuid;

	public static readonly EventScriptId Invalid = new EventScriptId(0, null);

	public EventScriptId(sbyte type, string guid, string subGuid = null)
	{
		Type = type;
		Guid = guid;
		SubGuid = subGuid;
	}

	public EventScriptId(sbyte type, Guid guid, Guid subGuid)
	{
		Type = type;
		Guid = guid.ToString();
		SubGuid = subGuid.ToString();
	}

	public EventScriptId(sbyte type, Guid guid)
	{
		Type = type;
		Guid = guid.ToString();
		SubGuid = null;
	}

	public static bool IsOptionType(sbyte type)
	{
		if (type != 3 && type != 4)
		{
			return type == 5;
		}
		return true;
	}

	public static bool IsConditionList(sbyte type)
	{
		if (type != 2 && type != 4)
		{
			return type == 5;
		}
		return true;
	}

	public bool Equals(EventScriptId other)
	{
		if (Type == other.Type && Guid == other.Guid)
		{
			return SubGuid == other.SubGuid;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is EventScriptId other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((Type * 397) ^ ((Guid != null) ? Guid.GetHashCode() : 0)) * 397) ^ ((SubGuid != null) ? SubGuid.GetHashCode() : 0);
	}

	public override string ToString()
	{
		EventScriptTypeItem eventScriptTypeItem = EventScriptType.Instance[Type];
		if (!string.IsNullOrEmpty(SubGuid))
		{
			return eventScriptTypeItem.Name + " " + Guid + " option " + SubGuid;
		}
		return eventScriptTypeItem.Name + " " + Guid;
	}

	public string GetFileName()
	{
		return Type switch
		{
			1 => Guid, 
			2 => Guid + "_condition", 
			3 => SubGuid, 
			4 => SubGuid + "_available", 
			5 => SubGuid + "_visible", 
			_ => null, 
		};
	}
}
