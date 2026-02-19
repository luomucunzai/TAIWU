using GameData.Serializer;

namespace GameData.Domains.World;

[SerializableGameData(IsExtensible = true)]
public class NotificationSortingGroup : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort Priority = 1;

		public const ushort IsHidden = 2;

		public const ushort IsOnTop = 3;

		public const ushort Count = 4;

		public static readonly string[] FieldId2FieldName = new string[4] { "Id", "Priority", "IsHidden", "IsOnTop" };
	}

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public int Priority;

	[SerializableGameDataField]
	public bool IsHidden;

	[SerializableGameDataField]
	public bool IsOnTop;

	public NotificationSortingGroup(int id, int priority, bool isHidden, bool isOnTop)
	{
		Id = id;
		Priority = priority;
		IsHidden = isHidden;
		IsOnTop = isOnTop;
	}

	public NotificationSortingGroup()
	{
	}

	public NotificationSortingGroup(NotificationSortingGroup other)
	{
		Id = other.Id;
		Priority = other.Priority;
		IsHidden = other.IsHidden;
		IsOnTop = other.IsOnTop;
	}

	public void Assign(NotificationSortingGroup other)
	{
		Id = other.Id;
		Priority = other.Priority;
		IsHidden = other.IsHidden;
		IsOnTop = other.IsOnTop;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
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
		*(short*)pData = 4;
		byte* num = pData + 2;
		*(int*)num = Id;
		byte* num2 = num + 4;
		*(int*)num2 = Priority;
		byte* num3 = num2 + 4;
		*num3 = (IsHidden ? ((byte)1) : ((byte)0));
		byte* num4 = num3 + 1;
		*num4 = (IsOnTop ? ((byte)1) : ((byte)0));
		int num5 = (int)(num4 + 1 - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
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
			Priority = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			IsHidden = *ptr != 0;
			ptr++;
		}
		if (num > 3)
		{
			IsOnTop = *ptr != 0;
			ptr++;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
