using System.Collections.Generic;
using Config;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class TravelerSkillsData : IProfessionSkillsData, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Palaces = 0;

		public const ushort MovementConsumedActionPoints = 1;

		public const ushort ExploredMapBlockCount = 2;

		public const ushort ExploredMapBlockActionPoints = 3;

		public const ushort Count = 4;

		public static readonly string[] FieldId2FieldName = new string[4] { "Palaces", "MovementConsumedActionPoints", "ExploredMapBlockCount", "ExploredMapBlockActionPoints" };
	}

	[SerializableGameDataField]
	private List<TravelerPalaceData> _palaces;

	[SerializableGameDataField]
	private int _movementConsumedActionPoints;

	[SerializableGameDataField]
	private int _exploredMapBlockCount;

	[SerializableGameDataField]
	private int _exploredMapBlockActionPoints;

	public int PalaceCount => _palaces?.Count ?? 0;

	public TravelerSkillsData()
	{
		Initialize();
	}

	public void Initialize()
	{
		_palaces?.Clear();
		_movementConsumedActionPoints = 0;
		_exploredMapBlockCount = 0;
		_exploredMapBlockActionPoints = 0;
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
	}

	public TravelerPalaceData TryGetPalaceData(int index)
	{
		if (_palaces == null)
		{
			return null;
		}
		if (!_palaces.CheckIndex(index))
		{
			return null;
		}
		return _palaces[index];
	}

	public int RecordMovementConsumedActionPoints(int actionPoints)
	{
		_movementConsumedActionPoints += actionPoints;
		if (_movementConsumedActionPoints < 300)
		{
			return 0;
		}
		int movementConsumedActionPoints = _movementConsumedActionPoints;
		_movementConsumedActionPoints %= 300;
		int arg = movementConsumedActionPoints - _movementConsumedActionPoints;
		return ProfessionFormula.Instance[71].Calculate(arg);
	}

	public int RecordExploredMapBlock(int actionPoints)
	{
		_exploredMapBlockCount++;
		_exploredMapBlockActionPoints += actionPoints;
		if (_exploredMapBlockCount < 12)
		{
			return 0;
		}
		int result = ProfessionFormula.Instance[72].Calculate(_exploredMapBlockActionPoints);
		_exploredMapBlockActionPoints = 0;
		_exploredMapBlockCount = 0;
		return result;
	}

	public void OfflineBuildPalace(Location location)
	{
		if (_palaces == null)
		{
			_palaces = new List<TravelerPalaceData>();
		}
		_palaces.Add(new TravelerPalaceData
		{
			Location = location
		});
	}

	public bool OfflineDestroyPalace(int index)
	{
		if (_palaces == null)
		{
			return false;
		}
		if (!_palaces.CheckIndex(index))
		{
			return false;
		}
		_palaces.RemoveAt(index);
		return true;
	}

	public TravelerSkillsData(TravelerSkillsData other)
	{
		if (other._palaces != null)
		{
			List<TravelerPalaceData> palaces = other._palaces;
			int count = palaces.Count;
			_palaces = new List<TravelerPalaceData>(count);
			for (int i = 0; i < count; i++)
			{
				_palaces.Add(new TravelerPalaceData(palaces[i]));
			}
		}
		else
		{
			_palaces = null;
		}
		_movementConsumedActionPoints = other._movementConsumedActionPoints;
		_exploredMapBlockCount = other._exploredMapBlockCount;
		_exploredMapBlockActionPoints = other._exploredMapBlockActionPoints;
	}

	public void Assign(TravelerSkillsData other)
	{
		if (other._palaces != null)
		{
			List<TravelerPalaceData> palaces = other._palaces;
			int count = palaces.Count;
			_palaces = new List<TravelerPalaceData>(count);
			for (int i = 0; i < count; i++)
			{
				_palaces.Add(new TravelerPalaceData(palaces[i]));
			}
		}
		else
		{
			_palaces = null;
		}
		_movementConsumedActionPoints = other._movementConsumedActionPoints;
		_exploredMapBlockCount = other._exploredMapBlockCount;
		_exploredMapBlockActionPoints = other._exploredMapBlockActionPoints;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 14;
		if (_palaces != null)
		{
			num += 2;
			int count = _palaces.Count;
			for (int i = 0; i < count; i++)
			{
				TravelerPalaceData travelerPalaceData = _palaces[i];
				num = ((travelerPalaceData == null) ? (num + 2) : (num + (2 + travelerPalaceData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 4;
		ptr += 2;
		if (_palaces != null)
		{
			int count = _palaces.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				TravelerPalaceData travelerPalaceData = _palaces[i];
				if (travelerPalaceData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = travelerPalaceData.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)intPtr = (ushort)num;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = _movementConsumedActionPoints;
		ptr += 4;
		*(int*)ptr = _exploredMapBlockCount;
		ptr += 4;
		*(int*)ptr = _exploredMapBlockActionPoints;
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
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (_palaces == null)
				{
					_palaces = new List<TravelerPalaceData>(num2);
				}
				else
				{
					_palaces.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					if (num3 > 0)
					{
						TravelerPalaceData travelerPalaceData = new TravelerPalaceData();
						ptr += travelerPalaceData.Deserialize(ptr);
						_palaces.Add(travelerPalaceData);
					}
					else
					{
						_palaces.Add(null);
					}
				}
			}
			else
			{
				_palaces?.Clear();
			}
		}
		if (num > 1)
		{
			_movementConsumedActionPoints = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			_exploredMapBlockCount = *(int*)ptr;
			ptr += 4;
		}
		if (num > 3)
		{
			_exploredMapBlockActionPoints = *(int*)ptr;
			ptr += 4;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
