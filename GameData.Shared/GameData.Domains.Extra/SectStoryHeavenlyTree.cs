using System;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Extra;

[Obsolete("use SectStoryHeavenlyTreeExtendable instead.")]
public struct SectStoryHeavenlyTree : ISerializableGameData
{
	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public ushort GrowPoint;

	public SectStoryHeavenlyTree(int id, short templateId, Location location)
	{
		Id = id;
		TemplateId = templateId;
		Location = location;
		GrowPoint = 0;
	}

	public SectStoryHeavenlyTree(SectStoryHeavenlyTree tree, ushort growPoint)
	{
		Id = tree.Id;
		TemplateId = tree.TemplateId;
		Location = tree.Location;
		GrowPoint = growPoint;
	}

	public SectStoryHeavenlyTree(SectStoryHeavenlyTree tree, ushort growPoint, ushort triggerRandomEnemyCount)
	{
		Id = tree.Id;
		TemplateId = tree.TemplateId;
		Location = tree.Location;
		GrowPoint = growPoint;
	}

	public SectStoryHeavenlyTree(SectStoryHeavenlyTree tree, int id)
	{
		Id = id;
		TemplateId = tree.TemplateId;
		Location = tree.Location;
		GrowPoint = tree.GrowPoint;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		ptr += Location.Serialize(ptr);
		*(ushort*)ptr = GrowPoint;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		ptr += Location.Deserialize(ptr);
		GrowPoint = *(ushort*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
