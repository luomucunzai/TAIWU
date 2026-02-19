using System;

namespace Config;

[Serializable]
public struct ResourceInfo
{
	public sbyte ResourceType;

	public int ResourceCount;

	public ResourceInfo(sbyte type, int count)
	{
		ResourceType = type;
		ResourceCount = count;
	}
}
