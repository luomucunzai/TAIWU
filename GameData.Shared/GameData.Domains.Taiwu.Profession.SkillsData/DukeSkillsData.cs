using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class DukeSkillsData : IProfessionSkillsData, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort DukeTitleOwners = 0;

		public const ushort DukeLuckPoints = 1;

		public const ushort DukeCricketGiven = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "DukeTitleOwners", "DukeLuckPoints", "DukeCricketGiven" };
	}

	public const int NobodyCharacterId = -1;

	public const int TitleCount = 6;

	[SerializableGameDataField]
	private int[] _dukeTitleOwners;

	[SerializableGameDataField]
	private int[] _dukeLuckPoints;

	[SerializableGameDataField]
	private bool[] _dukeCricketGiven;

	private static short DukeTitleTemplateOffset => 37;

	public DukeSkillsData()
	{
		_dukeTitleOwners = new int[6];
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			_dukeTitleOwners[i] = -1;
		}
		_dukeLuckPoints = new int[6];
		for (int j = 0; j < _dukeLuckPoints.Length; j++)
		{
			_dukeLuckPoints[j] = 0;
		}
		_dukeCricketGiven = new bool[6];
	}

	public void Initialize()
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			_dukeTitleOwners[i] = -1;
			_dukeLuckPoints[i] = 0;
			_dukeCricketGiven[i] = false;
		}
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
		if (!(sourceData is ObsoleteDukeSkillsData obsoleteDukeSkillsData))
		{
			return;
		}
		foreach (var (num, num2) in obsoleteDukeSkillsData.GetAllOwners())
		{
			_dukeTitleOwners[num2 - DukeTitleTemplateOffset] = num;
		}
	}

	public bool TitleHasOwner(short templateId)
	{
		return GetOwnerOfTitle(templateId) != -1;
	}

	public bool CharacterHasTitle(int charId)
	{
		return GetTitleFromOwner(charId) != -1;
	}

	public short GetTitleFromOwner(int charId)
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			if (_dukeTitleOwners[i] == charId)
			{
				return (short)(i + DukeTitleTemplateOffset);
			}
		}
		return -1;
	}

	public int GetOwnerOfTitle(short templateId)
	{
		return _dukeTitleOwners[templateId - DukeTitleTemplateOffset];
	}

	public int GetDukeLuckPointByTitle(short title)
	{
		return _dukeLuckPoints[title - DukeTitleTemplateOffset];
	}

	public IEnumerable<(int CharacterId, short TemplateId)> GetAllOwners()
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			if (_dukeTitleOwners[i] != -1)
			{
				yield return (CharacterId: _dukeTitleOwners[i], TemplateId: (short)(DukeTitleTemplateOffset + i));
			}
		}
	}

	public IEnumerable<short> GetAllTitles()
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			yield return (short)(i + DukeTitleTemplateOffset);
		}
	}

	public IEnumerable<short> GetNotGivenCricketTitles(Predicate<int> predicate)
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			if (_dukeTitleOwners[i] != -1 && !_dukeCricketGiven[i] && predicate(_dukeTitleOwners[i]))
			{
				yield return (short)(i + DukeTitleTemplateOffset);
			}
		}
	}

	public int GetNotGiveCricketCharId(Predicate<int> predicate)
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			if (_dukeTitleOwners[i] != -1 && !_dukeCricketGiven[i] && predicate(_dukeTitleOwners[i]))
			{
				return _dukeTitleOwners[i];
			}
		}
		return -1;
	}

	public void OfflineAssignTitleToCharacter(IRandomSource random, short templateId, int charId)
	{
		_dukeTitleOwners[templateId - DukeTitleTemplateOffset] = charId;
		_dukeLuckPoints[templateId - DukeTitleTemplateOffset] = random.Next(51);
	}

	public void OfflineRemoveTitleFromAnybody(short templateId)
	{
		OfflineRemoveTitle(templateId - DukeTitleTemplateOffset);
	}

	public short OfflineRemoveTitleFromCharacter(int charId)
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			if (_dukeTitleOwners[i] == charId)
			{
				OfflineRemoveTitle(i);
				return (short)(i + DukeTitleTemplateOffset);
			}
		}
		return -1;
	}

	public void OfflineClearAllTitles()
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			OfflineRemoveTitle(i);
		}
	}

	private void OfflineRemoveTitle(int i)
	{
		_dukeTitleOwners[i] = -1;
		_dukeLuckPoints[i] = 0;
	}

	public void OfflineSetDukeLuckPointByTitle(short title, int value)
	{
		_dukeLuckPoints[title - DukeTitleTemplateOffset] = value;
	}

	public void ResetAllCricketGivenData()
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			_dukeCricketGiven[i] = false;
		}
	}

	public void SetCharacterCricketGivenData(int charId, bool isGiven)
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			if (_dukeTitleOwners[i] == charId && !_dukeCricketGiven[i])
			{
				_dukeCricketGiven[i] = isGiven;
			}
		}
	}

	public DukeSkillsData(DukeSkillsData other)
	{
		int[] dukeTitleOwners = other._dukeTitleOwners;
		int num = dukeTitleOwners.Length;
		_dukeTitleOwners = new int[num];
		for (int i = 0; i < num; i++)
		{
			_dukeTitleOwners[i] = dukeTitleOwners[i];
		}
		int[] dukeLuckPoints = other._dukeLuckPoints;
		int num2 = dukeLuckPoints.Length;
		_dukeLuckPoints = new int[num2];
		for (int j = 0; j < num2; j++)
		{
			_dukeLuckPoints[j] = dukeLuckPoints[j];
		}
		bool[] dukeCricketGiven = other._dukeCricketGiven;
		int num3 = dukeCricketGiven.Length;
		_dukeCricketGiven = new bool[num3];
		for (int k = 0; k < num3; k++)
		{
			_dukeCricketGiven[k] = dukeCricketGiven[k];
		}
	}

	public void Assign(DukeSkillsData other)
	{
		int[] dukeTitleOwners = other._dukeTitleOwners;
		int num = dukeTitleOwners.Length;
		_dukeTitleOwners = new int[num];
		for (int i = 0; i < num; i++)
		{
			_dukeTitleOwners[i] = dukeTitleOwners[i];
		}
		int[] dukeLuckPoints = other._dukeLuckPoints;
		int num2 = dukeLuckPoints.Length;
		_dukeLuckPoints = new int[num2];
		for (int j = 0; j < num2; j++)
		{
			_dukeLuckPoints[j] = dukeLuckPoints[j];
		}
		bool[] dukeCricketGiven = other._dukeCricketGiven;
		int num3 = dukeCricketGiven.Length;
		_dukeCricketGiven = new bool[num3];
		for (int k = 0; k < num3; k++)
		{
			_dukeCricketGiven[k] = dukeCricketGiven[k];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((_dukeTitleOwners == null) ? (num + 2) : (num + (2 + 4 * _dukeTitleOwners.Length)));
		num = ((_dukeLuckPoints == null) ? (num + 2) : (num + (2 + 4 * _dukeLuckPoints.Length)));
		num = ((_dukeCricketGiven == null) ? (num + 2) : (num + (2 + _dukeCricketGiven.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 3;
		ptr += 2;
		if (_dukeTitleOwners != null)
		{
			int num = _dukeTitleOwners.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				((int*)ptr)[i] = _dukeTitleOwners[i];
			}
			ptr += 4 * num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_dukeLuckPoints != null)
		{
			int num2 = _dukeLuckPoints.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int j = 0; j < num2; j++)
			{
				((int*)ptr)[j] = _dukeLuckPoints[j];
			}
			ptr += 4 * num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_dukeCricketGiven != null)
		{
			int num3 = _dukeCricketGiven.Length;
			Tester.Assert(num3 <= 65535);
			*(ushort*)ptr = (ushort)num3;
			ptr += 2;
			for (int k = 0; k < num3; k++)
			{
				ptr[k] = (_dukeCricketGiven[k] ? ((byte)1) : ((byte)0));
			}
			ptr += num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
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
				if (_dukeTitleOwners == null || _dukeTitleOwners.Length != num2)
				{
					_dukeTitleOwners = new int[num2];
				}
				for (int i = 0; i < num2; i++)
				{
					_dukeTitleOwners[i] = ((int*)ptr)[i];
				}
				ptr += 4 * num2;
			}
			else
			{
				_dukeTitleOwners = null;
			}
		}
		if (num > 1)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (_dukeLuckPoints == null || _dukeLuckPoints.Length != num3)
				{
					_dukeLuckPoints = new int[num3];
				}
				for (int j = 0; j < num3; j++)
				{
					_dukeLuckPoints[j] = ((int*)ptr)[j];
				}
				ptr += 4 * num3;
			}
			else
			{
				_dukeLuckPoints = null;
			}
		}
		if (num > 2)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (_dukeCricketGiven == null || _dukeCricketGiven.Length != num4)
				{
					_dukeCricketGiven = new bool[num4];
				}
				for (int k = 0; k < num4; k++)
				{
					_dukeCricketGiven[k] = ptr[k] != 0;
				}
				ptr += (int)num4;
			}
			else
			{
				_dukeCricketGiven = null;
			}
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
