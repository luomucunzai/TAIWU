using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public class UnlockSimulateResult : ISerializableGameData
{
	[SerializableGameDataField]
	private List<int> _rawCreateEffects;

	[SerializableGameDataField]
	private List<int> _blockedRawCreateEffects;

	public bool AllBlocked
	{
		get
		{
			List<int> blockedRawCreateEffects = _blockedRawCreateEffects;
			int result;
			if (blockedRawCreateEffects != null && blockedRawCreateEffects.Count > 0)
			{
				blockedRawCreateEffects = _rawCreateEffects;
				result = ((blockedRawCreateEffects == null || blockedRawCreateEffects.Count <= 0) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	public IEnumerable<int> AllRawCreateEffects
	{
		get
		{
			if (_rawCreateEffects != null)
			{
				foreach (int rawCreateEffect in _rawCreateEffects)
				{
					yield return rawCreateEffect;
				}
			}
			if (_blockedRawCreateEffects == null)
			{
				yield break;
			}
			foreach (int blockedRawCreateEffect in _blockedRawCreateEffects)
			{
				yield return blockedRawCreateEffect;
			}
		}
	}

	public IReadOnlyList<int> BlockedRawCreateEffects => _blockedRawCreateEffects;

	public int AllRawCreateEffectsCount
	{
		get
		{
			List<int> rawCreateEffects = _rawCreateEffects;
			int result;
			if (rawCreateEffects == null)
			{
				int? num = _blockedRawCreateEffects?.Count;
				int? num2 = num;
				result = num2.GetValueOrDefault();
			}
			else
			{
				result = rawCreateEffects.Count;
			}
			return result;
		}
	}

	public UnlockSimulateResult(IEnumerable<int> rawCreateEffects, Func<int, bool> blockedChecker)
	{
		_rawCreateEffects = new List<int>(rawCreateEffects);
		_blockedRawCreateEffects = new List<int>(_rawCreateEffects.Where(blockedChecker));
		_rawCreateEffects.RemoveAll(_blockedRawCreateEffects.Contains);
	}

	public UnlockSimulateResult()
	{
	}

	public UnlockSimulateResult(UnlockSimulateResult other)
	{
		_rawCreateEffects = ((other._rawCreateEffects == null) ? null : new List<int>(other._rawCreateEffects));
		_blockedRawCreateEffects = ((other._blockedRawCreateEffects == null) ? null : new List<int>(other._blockedRawCreateEffects));
	}

	public void Assign(UnlockSimulateResult other)
	{
		_rawCreateEffects = ((other._rawCreateEffects == null) ? null : new List<int>(other._rawCreateEffects));
		_blockedRawCreateEffects = ((other._blockedRawCreateEffects == null) ? null : new List<int>(other._blockedRawCreateEffects));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((_rawCreateEffects == null) ? (num + 2) : (num + (2 + 4 * _rawCreateEffects.Count)));
		num = ((_blockedRawCreateEffects == null) ? (num + 2) : (num + (2 + 4 * _blockedRawCreateEffects.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (_rawCreateEffects != null)
		{
			int count = _rawCreateEffects.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = _rawCreateEffects[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_blockedRawCreateEffects != null)
		{
			int count2 = _blockedRawCreateEffects.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((int*)ptr)[j] = _blockedRawCreateEffects[j];
			}
			ptr += 4 * count2;
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
			if (_rawCreateEffects == null)
			{
				_rawCreateEffects = new List<int>(num);
			}
			else
			{
				_rawCreateEffects.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				_rawCreateEffects.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			_rawCreateEffects?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (_blockedRawCreateEffects == null)
			{
				_blockedRawCreateEffects = new List<int>(num2);
			}
			else
			{
				_blockedRawCreateEffects.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				_blockedRawCreateEffects.Add(((int*)ptr)[j]);
			}
			ptr += 4 * num2;
		}
		else
		{
			_blockedRawCreateEffects?.Clear();
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
