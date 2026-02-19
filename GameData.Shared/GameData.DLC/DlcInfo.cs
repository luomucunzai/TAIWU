using System;
using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.DLC;

public class DlcInfo : ISerializableGameData, IEquatable<DlcInfo>
{
	[SerializableGameDataField]
	public DlcId DlcId;

	[SerializableGameDataField]
	public bool IsInstalled;

	[SerializableGameDataField]
	public string EventDirectory;

	public DlcInfo(ulong appId, ulong version, bool isInstalled, string eventDirectory)
	{
		DlcId = new DlcId(appId, version);
		IsInstalled = isInstalled;
		EventDirectory = eventDirectory;
	}

	public bool Equals(DlcInfo other)
	{
		return DlcId.Equals(other?.DlcId);
	}

	public override int GetHashCode()
	{
		return DlcId.GetHashCode();
	}

	public string GetVersionString()
	{
		var (major, minor, build, revision) = BitOperation.UnpackVersion(DlcId.Version);
		return new Version(major, minor, build, revision).ToString();
	}

	public DlcInfo()
	{
	}

	public DlcInfo(DlcInfo other)
	{
		DlcId = other.DlcId;
		IsInstalled = other.IsInstalled;
		EventDirectory = other.EventDirectory;
	}

	public void Assign(DlcInfo other)
	{
		DlcId = other.DlcId;
		IsInstalled = other.IsInstalled;
		EventDirectory = other.EventDirectory;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 17;
		num = ((EventDirectory == null) ? (num + 2) : (num + (2 + 2 * EventDirectory.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DlcId.Serialize(ptr);
		*ptr = (IsInstalled ? ((byte)1) : ((byte)0));
		ptr++;
		if (EventDirectory != null)
		{
			int length = EventDirectory.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* eventDirectory = EventDirectory)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)eventDirectory[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
		ptr += DlcId.Deserialize(ptr);
		IsInstalled = *ptr != 0;
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			EventDirectory = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			EventDirectory = null;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
