using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.World.SectMainStory;

[SerializableGameData(IsExtensible = true)]
public class SectWuxianWugJugData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Poisons = 0;

		public const ushort LastRefiningDate = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "Poisons", "LastRefiningDate" };
	}

	[SerializableGameDataField]
	private List<int> _poisons = new List<int>(6);

	[SerializableGameDataField]
	private int _lastRefiningDate;

	public IReadOnlyList<int> Poisons => _poisons;

	public int TotalPoison => GetTotalPoison();

	public int LastRefiningDate => _lastRefiningDate;

	public int GetTotalPoison()
	{
		int num = 0;
		foreach (int poison in Poisons)
		{
			num += poison;
		}
		return num;
	}

	public void Reset()
	{
		_poisons.Clear();
		for (int i = 0; i < 6; i++)
		{
			_poisons.Add(0);
		}
		_lastRefiningDate = -1;
	}

	public void AddPoison(sbyte poisonType, int addValue)
	{
		if (_poisons.Count != 6)
		{
			Reset();
		}
		_poisons[poisonType] = MathUtils.Clamp(_poisons[poisonType] + addValue, 0, 357913941);
	}

	public void ReducePoison(sbyte poisonType, int reduceValue)
	{
		AddPoison(poisonType, -reduceValue);
	}

	public void UpdateRefiningDate()
	{
		_lastRefiningDate = ExternalDataBridge.Context.CurrDate;
	}

	public SectWuxianWugJugData()
	{
	}

	public SectWuxianWugJugData(SectWuxianWugJugData other)
	{
		_poisons = ((other._poisons == null) ? null : new List<int>(other._poisons));
		_lastRefiningDate = other._lastRefiningDate;
	}

	public void Assign(SectWuxianWugJugData other)
	{
		_poisons = ((other._poisons == null) ? null : new List<int>(other._poisons));
		_lastRefiningDate = other._lastRefiningDate;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		num = ((_poisons == null) ? (num + 2) : (num + (2 + 4 * _poisons.Count)));
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
		if (_poisons != null)
		{
			int count = _poisons.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = _poisons[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = _lastRefiningDate;
		ptr += 4;
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
				if (_poisons == null)
				{
					_poisons = new List<int>(num2);
				}
				else
				{
					_poisons.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					_poisons.Add(((int*)ptr)[i]);
				}
				ptr += 4 * num2;
			}
			else
			{
				_poisons?.Clear();
			}
		}
		if (num > 1)
		{
			_lastRefiningDate = *(int*)ptr;
			ptr += 4;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
