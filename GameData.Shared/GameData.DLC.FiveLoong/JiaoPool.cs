using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.DLC.FiveLoong;

[SerializableGameData(IsExtensible = true)]
public class JiaoPool : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Jiaos = 0;

		public const ushort NextPeriod = 1;

		public const ushort IsDisabled = 2;

		public const ushort BlockStyle = 3;

		public const ushort IsBabysitting = 4;

		public const ushort Count = 5;

		public static readonly string[] FieldId2FieldName = new string[5] { "Jiaos", "NextPeriod", "IsDisabled", "BlockStyle", "IsBabysitting" };
	}

	[SerializableGameDataField]
	public List<int> Jiaos;

	[SerializableGameDataField]
	public int NextPeriod;

	[SerializableGameDataField]
	public bool IsDisabled;

	[SerializableGameDataField]
	public short BlockStyle;

	[SerializableGameDataField]
	public bool isBabysitting;

	public JiaoPool()
	{
		Jiaos = new List<int>();
		NextPeriod = -1;
		IsDisabled = false;
		BlockStyle = -1;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		num = ((Jiaos == null) ? (num + 2) : (num + (2 + 4 * Jiaos.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 5;
		ptr += 2;
		if (Jiaos != null)
		{
			int count = Jiaos.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = Jiaos[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = NextPeriod;
		ptr += 4;
		*ptr = (IsDisabled ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = BlockStyle;
		ptr += 2;
		*ptr = (isBabysitting ? ((byte)1) : ((byte)0));
		ptr++;
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
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (Jiaos == null)
				{
					Jiaos = new List<int>(num2);
				}
				else
				{
					Jiaos.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					Jiaos.Add(((int*)ptr)[i]);
				}
				ptr += 4 * num2;
			}
			else
			{
				Jiaos?.Clear();
			}
		}
		if (num > 1)
		{
			NextPeriod = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			IsDisabled = *ptr != 0;
			ptr++;
		}
		if (num > 3)
		{
			BlockStyle = *(short*)ptr;
			ptr += 2;
		}
		if (num > 4)
		{
			isBabysitting = *ptr != 0;
			ptr++;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
