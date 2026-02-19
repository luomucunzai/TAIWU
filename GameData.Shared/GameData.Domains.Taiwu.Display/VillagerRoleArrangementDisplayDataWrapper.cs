using GameData.Domains.Taiwu.Display.VillagerRoleArrangement;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Display;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class VillagerRoleArrangementDisplayDataWrapper : ISerializableGameData
{
	[SerializableGameDataField]
	public int ArrangementTemplateId = -1;

	[SerializableGameDataField]
	public short AreaId = -1;

	[SerializableGameDataField]
	public int ArrangementDataId = -1;

	[SerializableGameDataField]
	public IVillagerRoleArrangementDisplayData ArrangementData;

	private static IVillagerRoleArrangementDisplayData CreateRealArrangementData(int arrangementTemplateId)
	{
		return arrangementTemplateId switch
		{
			0 => new HealingDisplayData(), 
			1 => new PeddlingDisplayData(), 
			2 => new EntertainingDisplayData(), 
			3 => new GuardingSwordTombDisplayData(), 
			4 => new TaiwuEnvoyDisplayData(), 
			_ => null, 
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		num = ((ArrangementData == null) ? (num + 2) : (num + (2 + ((ISerializableGameData)ArrangementData).GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = ArrangementTemplateId;
		ptr += 4;
		*(short*)ptr = AreaId;
		ptr += 2;
		*(int*)ptr = ArrangementDataId;
		ptr += 4;
		if (ArrangementData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = ((ISerializableGameData)ArrangementData).Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
		ArrangementTemplateId = *(int*)ptr;
		ptr += 4;
		AreaId = *(short*)ptr;
		ptr += 2;
		ArrangementDataId = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ArrangementData == null)
			{
				ArrangementData = CreateRealArrangementData(ArrangementDataId);
			}
			ptr += ((ISerializableGameData)ArrangementData).Deserialize(ptr);
		}
		else
		{
			ArrangementData = null;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
