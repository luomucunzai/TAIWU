using System;
using GameData.Serializer;

namespace GameData.Domains.World;

[SerializableGameData]
public class GameVersionInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public long TimestampCreating;

	[SerializableGameDataField]
	public string GameVersionCreating;

	[SerializableGameDataField]
	public string GameVersionLastSaving;

	public GameVersionInfo()
	{
		TimestampCreating = 0L;
		GameVersionCreating = string.Empty;
		GameVersionLastSaving = string.Empty;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += 8;
		num += SerializationHelper.GetSerializedSize(GameVersionCreating);
		num += SerializationHelper.GetSerializedSize(GameVersionLastSaving);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(long*)pData = TimestampCreating;
		byte* num = pData + 8;
		byte* num2 = num + SerializationHelper.Serialize(num, GameVersionCreating);
		int num3 = (int)(num2 + SerializationHelper.Serialize(num2, GameVersionLastSaving) - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TimestampCreating = *(long*)ptr;
		ptr += 8;
		ptr += SerializationHelper.Deserialize(ptr, ref GameVersionCreating);
		ptr += SerializationHelper.Deserialize(ptr, ref GameVersionLastSaving);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public static Version ParseGameVersion(string gameVersion)
	{
		if (string.IsNullOrEmpty(gameVersion))
		{
			return null;
		}
		if (gameVersion[0] == 'V')
		{
			gameVersion = gameVersion.Substring(1);
		}
		if (Version.TryParse(gameVersion, out Version result))
		{
			if (result.Major > 1)
			{
				return null;
			}
			return result;
		}
		int num = gameVersion.IndexOf('-');
		if (num < 0)
		{
			return null;
		}
		if (!Version.TryParse(gameVersion.Substring(0, num), out result))
		{
			return null;
		}
		if (result.Major > 1)
		{
			return null;
		}
		return result;
	}
}
