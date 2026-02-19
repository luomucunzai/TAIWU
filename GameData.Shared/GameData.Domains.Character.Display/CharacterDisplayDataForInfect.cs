using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Character.Display;

[AutoGenerateSerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class CharacterDisplayDataForInfect : ISerializableGameData
{
	[SerializableGameDataField]
	public CharacterDisplayDataForTooltip DataForTooltip;

	[SerializableGameDataField]
	public byte Infection;

	[SerializableGameDataField]
	public bool IsTaiwu;

	[SerializableGameDataField]
	public bool IsTeammate;

	[SerializableGameDataField]
	public bool IsKidnapped;

	public int TempInfection;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((DataForTooltip == null) ? (num + 2) : (num + (2 + DataForTooltip.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (DataForTooltip != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = DataForTooltip.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = Infection;
		ptr++;
		*ptr = (IsTaiwu ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsTeammate ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsKidnapped ? ((byte)1) : ((byte)0));
		ptr++;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			DataForTooltip = new CharacterDisplayDataForTooltip();
			ptr += DataForTooltip.Deserialize(ptr);
		}
		else
		{
			DataForTooltip = null;
		}
		Infection = *ptr;
		ptr++;
		IsTaiwu = *ptr != 0;
		ptr++;
		IsTeammate = *ptr != 0;
		ptr++;
		IsKidnapped = *ptr != 0;
		ptr++;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
