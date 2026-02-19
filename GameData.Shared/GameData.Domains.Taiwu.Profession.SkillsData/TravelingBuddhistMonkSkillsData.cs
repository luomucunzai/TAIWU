using System.Collections.Generic;
using System.Linq;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class TravelingBuddhistMonkSkillsData : IProfessionSkillsData, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort StateTempleVisited = 0;

		public const ushort StateTempleLocation = 1;

		public const ushort GeneratedTempleIndices = 2;

		public const ushort SelectedSkill3FeatureId = 3;

		public const ushort Count = 4;

		public static readonly string[] FieldId2FieldName = new string[4] { "StateTempleVisited", "StateTempleLocation", "GeneratedTempleIndices", "SelectedSkill3FeatureId" };
	}

	public const int GenerateTempleCount = 5;

	public const short DefaultSelectedSkill3FeatureId = 616;

	[SerializableGameDataField]
	private bool[] _stateTempleVisited;

	[SerializableGameDataField]
	private Location[] _stateTempleLocation;

	[SerializableGameDataField]
	private List<int> _generatedTempleIndices;

	[SerializableGameDataField]
	private short _selectedSkill3FeatureId;

	public bool HasVisitedAllTemple => GetVisitedTempleCount() >= 15;

	public void Initialize()
	{
		_generatedTempleIndices?.Clear();
		if (_stateTempleVisited != null)
		{
			for (int i = 0; i < _stateTempleVisited.Length; i++)
			{
				_stateTempleVisited[i] = false;
				_stateTempleLocation[i] = Location.Invalid;
			}
		}
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
		if (sourceData is ObsoleteTravelingBuddhistMonkSkillsData)
		{
			OfflineClearAllTampleState();
			_selectedSkill3FeatureId = -1;
		}
	}

	public void OfflineCreateTemple(sbyte stateId, Location location)
	{
		_stateTempleLocation[stateId] = location;
		if (_generatedTempleIndices == null)
		{
			_generatedTempleIndices = new List<int>();
		}
		if (!_generatedTempleIndices.Contains(stateId))
		{
			_generatedTempleIndices.Add(stateId);
		}
	}

	public void OfflineSetStateTempleVisited(sbyte stateId)
	{
		if (!_stateTempleVisited[stateId])
		{
			_stateTempleVisited[stateId] = true;
		}
	}

	public void OfflineClearAllTampleState()
	{
		if (_generatedTempleIndices == null)
		{
			_generatedTempleIndices = new List<int>();
		}
		_generatedTempleIndices.Clear();
		for (int i = 0; i < _stateTempleVisited.Length; i++)
		{
			_stateTempleVisited[i] = false;
			_stateTempleLocation[i] = Location.Invalid;
		}
	}

	public int GetVisitedTempleCount()
	{
		return _stateTempleVisited.Count((bool x) => x);
	}

	public bool IsGeneratedTempleIndex(int index)
	{
		return _generatedTempleIndices?.Contains(index) ?? false;
	}

	public bool StateHasTemple(sbyte stateId)
	{
		if (IsGeneratedTempleIndex(stateId) && _stateTempleLocation.CheckIndex(stateId))
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

	public TravelingBuddhistMonkSkillsData()
	{
		_stateTempleLocation = new Location[15];
		_stateTempleVisited = new bool[15];
	}

	public short GetSelectedSkill3FeatureId()
	{
		return _selectedSkill3FeatureId;
	}

	public void OfflineSetSelectedSkill3FeatureId(short featureId)
	{
		_selectedSkill3FeatureId = featureId;
	}

	public TravelingBuddhistMonkSkillsData(TravelingBuddhistMonkSkillsData other)
	{
		bool[] stateTempleVisited = other._stateTempleVisited;
		int num = stateTempleVisited.Length;
		_stateTempleVisited = new bool[num];
		for (int i = 0; i < num; i++)
		{
			_stateTempleVisited[i] = stateTempleVisited[i];
		}
		Location[] stateTempleLocation = other._stateTempleLocation;
		int num2 = stateTempleLocation.Length;
		_stateTempleLocation = new Location[num2];
		for (int j = 0; j < num2; j++)
		{
			_stateTempleLocation[j] = stateTempleLocation[j];
		}
		_generatedTempleIndices = ((other._generatedTempleIndices == null) ? null : new List<int>(other._generatedTempleIndices));
		_selectedSkill3FeatureId = other._selectedSkill3FeatureId;
	}

	public void Assign(TravelingBuddhistMonkSkillsData other)
	{
		bool[] stateTempleVisited = other._stateTempleVisited;
		int num = stateTempleVisited.Length;
		_stateTempleVisited = new bool[num];
		for (int i = 0; i < num; i++)
		{
			_stateTempleVisited[i] = stateTempleVisited[i];
		}
		Location[] stateTempleLocation = other._stateTempleLocation;
		int num2 = stateTempleLocation.Length;
		_stateTempleLocation = new Location[num2];
		for (int j = 0; j < num2; j++)
		{
			_stateTempleLocation[j] = stateTempleLocation[j];
		}
		_generatedTempleIndices = ((other._generatedTempleIndices == null) ? null : new List<int>(other._generatedTempleIndices));
		_selectedSkill3FeatureId = other._selectedSkill3FeatureId;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((_stateTempleVisited == null) ? (num + 2) : (num + (2 + _stateTempleVisited.Length)));
		num = ((_stateTempleLocation == null) ? (num + 2) : (num + (2 + 4 * _stateTempleLocation.Length)));
		num = ((_generatedTempleIndices == null) ? (num + 2) : (num + (2 + 4 * _generatedTempleIndices.Count)));
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
		if (_generatedTempleIndices != null)
		{
			int count = _generatedTempleIndices.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int k = 0; k < count; k++)
			{
				((int*)ptr)[k] = _generatedTempleIndices[k];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = _selectedSkill3FeatureId;
		ptr += 2;
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
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (_stateTempleVisited == null || _stateTempleVisited.Length != num2)
				{
					_stateTempleVisited = new bool[num2];
				}
				for (int i = 0; i < num2; i++)
				{
					_stateTempleVisited[i] = ptr[i] != 0;
				}
				ptr += (int)num2;
			}
			else
			{
				_stateTempleVisited = null;
			}
		}
		if (num > 1)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (_stateTempleLocation == null || _stateTempleLocation.Length != num3)
				{
					_stateTempleLocation = new Location[num3];
				}
				for (int j = 0; j < num3; j++)
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
		}
		if (num > 2)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (_generatedTempleIndices == null)
				{
					_generatedTempleIndices = new List<int>(num4);
				}
				else
				{
					_generatedTempleIndices.Clear();
				}
				for (int k = 0; k < num4; k++)
				{
					_generatedTempleIndices.Add(((int*)ptr)[k]);
				}
				ptr += 4 * num4;
			}
			else
			{
				_generatedTempleIndices?.Clear();
			}
		}
		if (num > 3)
		{
			_selectedSkill3FeatureId = *(short*)ptr;
			ptr += 2;
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
