using System.Collections.Generic;
using System.Linq;
using GameData.Domains.World;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Information;

[SerializableGameData(IsExtensible = true)]
public class TaiwuInformationReceivedHistories : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort ReceivedNormalInformation = 0;

		public const ushort PackedNormalInformationIndices = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "ReceivedNormalInformation", "PackedNormalInformationIndices" };
	}

	[SerializableGameDataField]
	private List<NormalInformation> _receivedNormalInformation = new List<NormalInformation>();

	[SerializableGameDataField]
	private List<int> _packedNormalInformationIndices = new List<int>();

	public void PackReceivedNormalInformationInLastMonth(List<NormalInformation> taiwuReceivedNormalInformationInMonth)
	{
		Dictionary<int, List<NormalInformation>> dictionary = new Dictionary<int, List<NormalInformation>>();
		for (int i = 0; i < _packedNormalInformationIndices.Count; i++)
		{
			int key = _packedNormalInformationIndices[i++];
			List<NormalInformation> list = new List<NormalInformation>();
			for (; _packedNormalInformationIndices[i] >= 0; i++)
			{
				list.Add(_receivedNormalInformation[_packedNormalInformationIndices[i]]);
			}
			dictionary[key] = list;
		}
		sbyte b = SharedMethods.CalcMonthInYear(ExternalDataBridge.Context.CurrDate);
		if (b != 0)
		{
			dictionary[b - 1] = taiwuReceivedNormalInformationInMonth;
		}
		_receivedNormalInformation.Clear();
		_packedNormalInformationIndices.Clear();
		foreach (int item in dictionary.Keys.OrderByDescending((int d) => d))
		{
			_packedNormalInformationIndices.Add(item);
			foreach (NormalInformation item2 in dictionary[item])
			{
				_packedNormalInformationIndices.Add(_receivedNormalInformation.Count);
				_receivedNormalInformation.Add(item2);
			}
			_packedNormalInformationIndices.Add(-1);
		}
	}

	public void ClearReceivedInformation()
	{
		_receivedNormalInformation.Clear();
		_packedNormalInformationIndices.Clear();
	}

	public bool TryUnpackReceivedNormalInformationInMonth(int date, out List<NormalInformation> result)
	{
		for (int i = 0; i < _packedNormalInformationIndices.Count; i++)
		{
			if (_packedNormalInformationIndices[i++] == date)
			{
				result = new List<NormalInformation>();
				for (; _packedNormalInformationIndices[i] >= 0; i++)
				{
					result.Add(_receivedNormalInformation[_packedNormalInformationIndices[i]]);
				}
				return true;
			}
			for (; _packedNormalInformationIndices[i] >= 0; i++)
			{
			}
		}
		result = null;
		return false;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((_receivedNormalInformation == null) ? (num + 2) : (num + (2 + 3 * _receivedNormalInformation.Count)));
		num = ((_packedNormalInformationIndices == null) ? (num + 2) : (num + (2 + 4 * _packedNormalInformationIndices.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 2;
		ptr += 2;
		if (_receivedNormalInformation != null)
		{
			int count = _receivedNormalInformation.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += _receivedNormalInformation[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_packedNormalInformationIndices != null)
		{
			int count2 = _packedNormalInformationIndices.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((int*)ptr)[j] = _packedNormalInformationIndices[j];
			}
			ptr += 4 * count2;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (_receivedNormalInformation == null)
				{
					_receivedNormalInformation = new List<NormalInformation>(num2);
				}
				else
				{
					_receivedNormalInformation.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					NormalInformation item = default(NormalInformation);
					ptr += item.Deserialize(ptr);
					_receivedNormalInformation.Add(item);
				}
			}
			else
			{
				_receivedNormalInformation?.Clear();
			}
		}
		if (num > 1)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (_packedNormalInformationIndices == null)
				{
					_packedNormalInformationIndices = new List<int>(num3);
				}
				else
				{
					_packedNormalInformationIndices.Clear();
				}
				for (int j = 0; j < num3; j++)
				{
					_packedNormalInformationIndices.Add(((int*)ptr)[j]);
				}
				ptr += 4 * num3;
			}
			else
			{
				_packedNormalInformationIndices?.Clear();
			}
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
