using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class ObsoleteDukeSkillsData : IProfessionSkillsData, ISerializableGameData
{
	public const int NobodyCharacterId = -1;

	[SerializableGameDataField]
	private int[] _dukeTitleOwners;

	private static short DukeTitleTemplateOffset => 37;

	public void Initialize()
	{
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			_dukeTitleOwners[i] = -1;
		}
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
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

	public ObsoleteDukeSkillsData()
	{
		_dukeTitleOwners = new int[6];
		for (int i = 0; i < _dukeTitleOwners.Length; i++)
		{
			_dukeTitleOwners[i] = -1;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((_dukeTitleOwners == null) ? (num + 2) : (num + (2 + 4 * _dukeTitleOwners.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
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
			if (_dukeTitleOwners == null || _dukeTitleOwners.Length != num)
			{
				_dukeTitleOwners = new int[num];
			}
			for (int i = 0; i < num; i++)
			{
				_dukeTitleOwners[i] = ((int*)ptr)[i];
			}
			ptr += 4 * num;
		}
		else
		{
			_dukeTitleOwners = null;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
