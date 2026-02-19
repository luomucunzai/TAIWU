using GameData.Serializer;

namespace GameData.Domains.Character;

[SerializableGameData(NotForDisplayModule = true, IsExtensible = true, NoCopyConstructors = true)]
public struct DarkAshCounterData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort ExpiredDate2 = 0;

		public const ushort ExpiredDate3 = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "ExpiredDate2", "ExpiredDate3" };
	}

	[SerializableGameDataField]
	public int ExpiredDate2;

	[SerializableGameDataField]
	public int ExpiredDate3;

	public DarkAshCounterData(int currDate, int consummateLevel, int faith = 0)
	{
		ExpiredDate2 = (ExpiredDate3 = currDate + faith) + consummateLevel;
	}

	public DarkAshCounterData OfflineApplyFaithChangeToExtraData(int currDate, int delta)
	{
		if (ExpiredDate2 - currDate < 0)
		{
			ExpiredDate2 = (ExpiredDate3 = currDate + delta);
		}
		else
		{
			ExpiredDate2 += delta;
			if (ExpiredDate3 - currDate < 0)
			{
				ExpiredDate3 = currDate + delta;
			}
			else
			{
				ExpiredDate3 += delta;
			}
		}
		return this;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 2;
		byte* num = pData + 2;
		*(int*)num = ExpiredDate2;
		byte* num2 = num + 4;
		*(int*)num2 = ExpiredDate3;
		int num3 = (int)(num2 + 4 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ExpiredDate2 = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			ExpiredDate3 = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
