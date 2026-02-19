using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotForDisplayModule = true)]
public class BuildingAreaEffectProgress : ISerializableGameData
{
	public static class EffectType
	{
		public const sbyte Animal = 0;

		public const sbyte Cricket = 1;

		public const sbyte Adventure = 2;

		public const int Count = 3;

		public static readonly int[] ProgressThreshold = new int[3] { 60, 180, 360 };

		public static readonly int[] MaxActiveCount = new int[3] { 6, 3, 1 };

		public static readonly EBuildingScaleEffect[] ToBuildingScaleEffect = new EBuildingScaleEffect[3]
		{
			EBuildingScaleEffect.AnimalProgressDelta,
			EBuildingScaleEffect.CricketProgressDelta,
			EBuildingScaleEffect.AdventureProgressDelta
		};
	}

	private static class FieldIds
	{
		public const ushort EffectType = 0;

		public const ushort Progress = 1;

		public const ushort CurrActiveLocations = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "EffectType", "Progress", "CurrActiveLocations" };
	}

	[SerializableGameDataField]
	private sbyte _effectType;

	[SerializableGameDataField]
	private int _progress;

	[SerializableGameDataField]
	private List<Location> _currActiveLocations;

	public EBuildingScaleEffect BuildingScaleEffect => EffectType.ToBuildingScaleEffect[_effectType];

	public int LocationCount => _currActiveLocations?.Count ?? 0;

	public int Progress => _progress;

	public BuildingAreaEffectProgress(sbyte effectType)
	{
		_effectType = effectType;
	}

	public BuildingAreaEffectProgress()
	{
	}

	public bool RemoveLocation(Location location)
	{
		if (location.IsValid() && _currActiveLocations != null)
		{
			return _currActiveLocations.Remove(location);
		}
		return false;
	}

	public void AddLocation(Location location)
	{
		if (_currActiveLocations == null)
		{
			_currActiveLocations = new List<Location>();
		}
		_currActiveLocations.Add(location);
	}

	public int OfflineSetProgress(int value)
	{
		return _progress = value;
	}

	public int OfflineChangeProgress(int delta)
	{
		int num = EffectType.MaxActiveCount[_effectType];
		int locationCount = LocationCount;
		if (locationCount >= num)
		{
			return 0;
		}
		_progress += delta;
		int num2 = EffectType.ProgressThreshold[_effectType];
		int num3 = _progress / num2;
		if (num3 <= 0)
		{
			return 0;
		}
		if (locationCount + num3 >= num)
		{
			num3 = num - locationCount;
		}
		_progress %= num2;
		return num3;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 7;
		num = ((_currActiveLocations == null) ? (num + 2) : (num + (2 + 4 * _currActiveLocations.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 3;
		ptr += 2;
		*ptr = (byte)_effectType;
		ptr++;
		*(int*)ptr = _progress;
		ptr += 4;
		if (_currActiveLocations != null)
		{
			int count = _currActiveLocations.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += _currActiveLocations[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			_effectType = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 1)
		{
			_progress = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (_currActiveLocations == null)
				{
					_currActiveLocations = new List<Location>(num2);
				}
				else
				{
					_currActiveLocations.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					Location item = default(Location);
					ptr += item.Deserialize(ptr);
					_currActiveLocations.Add(item);
				}
			}
			else
			{
				_currActiveLocations?.Clear();
			}
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
