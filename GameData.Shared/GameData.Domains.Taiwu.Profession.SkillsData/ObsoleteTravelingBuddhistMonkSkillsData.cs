using System;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class ObsoleteTravelingBuddhistMonkSkillsData : IProfessionSkillsData, ISerializableGameData
{
	[SerializableGameDataField]
	public bool[] _stateTempleVisited;

	[SerializableGameDataField]
	public Location[] _stateTempleLocation;

	public int _visitedCount;

	public bool HasVisitedAllTemple => _visitedCount >= 15;

	public void Initialize()
	{
		for (int i = 0; i < _stateTempleLocation.Length; i++)
		{
			_stateTempleLocation[i] = Location.Invalid;
		}
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
	}

	public int GetVisitedTempleCount()
	{
		return _visitedCount;
	}

	public bool StateHasTemple(sbyte stateId)
	{
		if (_stateTempleLocation.CheckIndex(stateId))
		{
			return _stateTempleLocation[stateId].IsValid();
		}
		return false;
	}

	public Location GetStateTempleLocation(sbyte stateId)
	{
		return _stateTempleLocation[stateId];
	}

	public bool IsStateTempleVisited(sbyte stateId)
	{
		if (_stateTempleVisited.CheckIndex(stateId))
		{
			return _stateTempleVisited[stateId];
		}
		return false;
	}

	private void CalcVisitedTempleCount()
	{
		int num = 0;
		bool[] stateTempleVisited = _stateTempleVisited;
		for (int i = 0; i < stateTempleVisited.Length; i++)
		{
			if (stateTempleVisited[i])
			{
				num++;
			}
		}
		_visitedCount = num;
	}

	public ObsoleteTravelingBuddhistMonkSkillsData()
	{
		_stateTempleLocation = new Location[15];
		_stateTempleVisited = new bool[15];
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((_stateTempleVisited == null) ? (num + 2) : (num + (2 + _stateTempleVisited.Length)));
		num = ((_stateTempleLocation == null) ? (num + 2) : (num + (2 + 4 * _stateTempleLocation.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (_stateTempleVisited != null)
		{
			int num = _stateTempleVisited.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				ptr[i] = (_stateTempleVisited[i] ? ((byte)1) : ((byte)0));
			}
			ptr += num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_stateTempleLocation != null)
		{
			int num2 = _stateTempleLocation.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int j = 0; j < num2; j++)
			{
				ptr += _stateTempleLocation[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_stateTempleVisited == null || _stateTempleVisited.Length != num)
			{
				_stateTempleVisited = new bool[num];
			}
			for (int i = 0; i < num; i++)
			{
				_stateTempleVisited[i] = ptr[i] != 0;
			}
			ptr += (int)num;
		}
		else
		{
			_stateTempleVisited = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (_stateTempleLocation == null || _stateTempleLocation.Length != num2)
			{
				_stateTempleLocation = new Location[num2];
			}
			for (int j = 0; j < num2; j++)
			{
				Location location = default(Location);
				ptr += location.Deserialize(ptr);
				_stateTempleLocation[j] = location;
			}
		}
		else
		{
			_stateTempleLocation = null;
		}
		CalcVisitedTempleCount();
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
