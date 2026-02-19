using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Extra;

[SerializableGameData(IsExtensible = true)]
public class SectStoryHeavenlyTreeExtendable : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort TemplateId = 1;

		public const ushort Location = 2;

		public const ushort GrowPoint = 3;

		public const ushort TriggerRandomEnemyCount = 4;

		public const ushort MetInDream = 5;

		public const ushort FindFairyland = 6;

		public const ushort FightWithSnake = 7;

		public const ushort SnakeTemplateId = 8;

		public const ushort Count = 9;

		public static readonly string[] FieldId2FieldName = new string[9] { "Id", "TemplateId", "Location", "GrowPoint", "TriggerRandomEnemyCount", "MetInDream", "FindFairyland", "FightWithSnake", "SnakeTemplateId" };
	}

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public ushort GrowPoint;

	[SerializableGameDataField]
	public ushort TriggerRandomEnemyCount;

	[SerializableGameDataField]
	public bool MetInDream;

	[SerializableGameDataField]
	public bool FindFairyland;

	[SerializableGameDataField]
	public bool FightWithSnake;

	[SerializableGameDataField]
	public short SnakeTemplateId;

	public SectStoryHeavenlyTreeExtendable(int id, short templateId, Location location)
	{
		Id = id;
		TemplateId = templateId;
		Location = location;
		GrowPoint = 0;
		TriggerRandomEnemyCount = 0;
	}

	public SectStoryHeavenlyTreeExtendable(SectStoryHeavenlyTreeExtendable tree, ushort growPoint)
	{
		Id = tree.Id;
		TemplateId = tree.TemplateId;
		Location = tree.Location;
		GrowPoint = growPoint;
		TriggerRandomEnemyCount = tree.TriggerRandomEnemyCount;
	}

	public SectStoryHeavenlyTreeExtendable(SectStoryHeavenlyTreeExtendable tree, ushort growPoint, ushort triggerRandomEnemyCount)
	{
		Id = tree.Id;
		TemplateId = tree.TemplateId;
		Location = tree.Location;
		GrowPoint = growPoint;
		TriggerRandomEnemyCount = triggerRandomEnemyCount;
	}

	public SectStoryHeavenlyTreeExtendable(SectStoryHeavenlyTreeExtendable tree, int id)
	{
		Id = id;
		TemplateId = tree.TemplateId;
		Location = tree.Location;
		GrowPoint = tree.GrowPoint;
		TriggerRandomEnemyCount = tree.TriggerRandomEnemyCount;
	}

	public SectStoryHeavenlyTreeExtendable()
	{
	}

	public SectStoryHeavenlyTreeExtendable(SectStoryHeavenlyTreeExtendable other)
	{
		Id = other.Id;
		TemplateId = other.TemplateId;
		Location = other.Location;
		GrowPoint = other.GrowPoint;
		TriggerRandomEnemyCount = other.TriggerRandomEnemyCount;
		MetInDream = other.MetInDream;
		FindFairyland = other.FindFairyland;
		FightWithSnake = other.FightWithSnake;
		SnakeTemplateId = other.SnakeTemplateId;
	}

	public void Assign(SectStoryHeavenlyTreeExtendable other)
	{
		Id = other.Id;
		TemplateId = other.TemplateId;
		Location = other.Location;
		GrowPoint = other.GrowPoint;
		TriggerRandomEnemyCount = other.TriggerRandomEnemyCount;
		MetInDream = other.MetInDream;
		FindFairyland = other.FindFairyland;
		FightWithSnake = other.FightWithSnake;
		SnakeTemplateId = other.SnakeTemplateId;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 21;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 9;
		ptr += 2;
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		ptr += Location.Serialize(ptr);
		*(ushort*)ptr = GrowPoint;
		ptr += 2;
		*(ushort*)ptr = TriggerRandomEnemyCount;
		ptr += 2;
		*ptr = (MetInDream ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (FindFairyland ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (FightWithSnake ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = SnakeTemplateId;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			Id = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			TemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 2)
		{
			ptr += Location.Deserialize(ptr);
		}
		if (num > 3)
		{
			GrowPoint = *(ushort*)ptr;
			ptr += 2;
		}
		if (num > 4)
		{
			TriggerRandomEnemyCount = *(ushort*)ptr;
			ptr += 2;
		}
		if (num > 5)
		{
			MetInDream = *ptr != 0;
			ptr++;
		}
		if (num > 6)
		{
			FindFairyland = *ptr != 0;
			ptr++;
		}
		if (num > 7)
		{
			FightWithSnake = *ptr != 0;
			ptr++;
		}
		if (num > 8)
		{
			SnakeTemplateId = *(short*)ptr;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
