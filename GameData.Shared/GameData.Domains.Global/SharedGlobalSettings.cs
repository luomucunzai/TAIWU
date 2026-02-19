using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Global;

[SerializableGameData(NoCopyConstructors = true)]
public class SharedGlobalSettings : ISerializableGameData
{
	[SerializableGameDataField]
	public string Language;

	[SerializableGameDataField]
	public bool AutoTriggerMapNormalPickup;

	[SerializableGameDataField]
	public MapPickupAutoTriggerSetting NormalMapPickupAutoTriggerSetting;

	[SerializableGameDataField]
	public int AutoWipeOut;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 5;
		num = ((Language == null) ? (num + 2) : (num + (2 + 2 * Language.Length)));
		num = ((NormalMapPickupAutoTriggerSetting == null) ? (num + 2) : (num + (2 + NormalMapPickupAutoTriggerSetting.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Language != null)
		{
			int length = Language.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* language = Language)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)language[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (AutoTriggerMapNormalPickup ? ((byte)1) : ((byte)0));
		ptr++;
		if (NormalMapPickupAutoTriggerSetting != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = NormalMapPickupAutoTriggerSetting.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = AutoWipeOut;
		ptr += 4;
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
			int num2 = 2 * num;
			Language = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			Language = null;
		}
		AutoTriggerMapNormalPickup = *ptr != 0;
		ptr++;
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (NormalMapPickupAutoTriggerSetting == null)
			{
				NormalMapPickupAutoTriggerSetting = new MapPickupAutoTriggerSetting();
			}
			ptr += NormalMapPickupAutoTriggerSetting.Deserialize(ptr);
		}
		else
		{
			NormalMapPickupAutoTriggerSetting = null;
		}
		AutoWipeOut = *(int*)ptr;
		ptr += 4;
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public bool IsTypeAutoWipeOutOn(int index)
	{
		return (AutoWipeOut & (1 << index)) != 0;
	}

	public bool IsTypeAutoWipeOutOn(WipeOutType type)
	{
		return IsTypeAutoWipeOutOn((int)type);
	}
}
