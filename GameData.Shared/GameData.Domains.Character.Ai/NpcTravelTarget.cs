using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai;

public struct NpcTravelTarget : ISerializableGameData
{
	[SerializableGameDataField]
	private bool _isTargetFixedLocation;

	[SerializableGameDataField]
	public int TargetCharId;

	[SerializableGameDataField]
	private Location _targetLocation;

	[SerializableGameDataField]
	public int RemainingMonth;

	public NpcTravelTarget(Location targetLocation, int maxDuration)
	{
		_isTargetFixedLocation = true;
		_targetLocation = targetLocation;
		TargetCharId = -1;
		RemainingMonth = maxDuration;
	}

	public NpcTravelTarget(int targetCharId, int maxDuration)
	{
		_isTargetFixedLocation = false;
		_targetLocation = Location.Invalid;
		TargetCharId = targetCharId;
		RemainingMonth = maxDuration;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 13;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (_isTargetFixedLocation ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = TargetCharId;
		ptr += 4;
		ptr += _targetLocation.Serialize(ptr);
		*(int*)ptr = RemainingMonth;
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
		_isTargetFixedLocation = *ptr != 0;
		ptr++;
		TargetCharId = *(int*)ptr;
		ptr += 4;
		ptr += _targetLocation.Deserialize(ptr);
		RemainingMonth = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public bool IsSameTargetWith(NpcTravelTarget other)
	{
		if (other._isTargetFixedLocation != _isTargetFixedLocation)
		{
			return false;
		}
		if (!_isTargetFixedLocation)
		{
			return other.TargetCharId == TargetCharId;
		}
		return other._targetLocation.Equals(_targetLocation);
	}

	public bool TryGetFixedLocation(out Location location)
	{
		location = _targetLocation;
		return _isTargetFixedLocation;
	}
}
