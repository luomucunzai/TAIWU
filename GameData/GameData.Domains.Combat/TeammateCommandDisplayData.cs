using GameData.Serializer;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public struct TeammateCommandDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public bool IsAlly;

	[SerializableGameDataField]
	public sbyte IndexCharacter;

	[SerializableGameDataField]
	public sbyte ValidIndexCharacter;

	[SerializableGameDataField]
	public sbyte IndexCommand;

	[SerializableGameDataField]
	public sbyte CmdType;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 5;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (IsAlly ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)IndexCharacter;
		ptr++;
		*ptr = (byte)ValidIndexCharacter;
		ptr++;
		*ptr = (byte)IndexCommand;
		ptr++;
		*ptr = (byte)CmdType;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		IsAlly = *ptr != 0;
		ptr++;
		IndexCharacter = (sbyte)(*ptr);
		ptr++;
		ValidIndexCharacter = (sbyte)(*ptr);
		ptr++;
		IndexCommand = (sbyte)(*ptr);
		ptr++;
		CmdType = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
