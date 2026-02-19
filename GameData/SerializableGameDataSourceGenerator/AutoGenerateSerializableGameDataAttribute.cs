using System;

namespace SerializableGameDataSourceGenerator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class AutoGenerateSerializableGameDataAttribute : Attribute
{
	public bool NotForDisplayModule;

	public bool NotForArchive;

	public bool NotRestrictCollectionSerializedSize;

	public bool IsExtensible;

	public bool NoCopyConstructors;

	public AutoGenerateSerializableGameDataAttribute()
	{
		NotForDisplayModule = false;
		NotForArchive = false;
		NotRestrictCollectionSerializedSize = false;
		IsExtensible = false;
		NoCopyConstructors = false;
	}
}
