using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Map;

[Obsolete]
public class AnimalPlaceData : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<short, short> BlockAnimalCharacterTemplateIds;

	public AnimalPlaceData()
	{
		BlockAnimalCharacterTemplateIds = new Dictionary<short, short>();
	}

	public AnimalPlaceData(AnimalPlaceData other)
	{
		BlockAnimalCharacterTemplateIds = new Dictionary<short, short>(other.BlockAnimalCharacterTemplateIds);
	}

	public void Assign(AnimalPlaceData other)
	{
		BlockAnimalCharacterTemplateIds = new Dictionary<short, short>(other.BlockAnimalCharacterTemplateIds);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((BlockAnimalCharacterTemplateIds == null) ? (num + 4) : (num + (4 + 4 * BlockAnimalCharacterTemplateIds.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (BlockAnimalCharacterTemplateIds != null)
		{
			int count = BlockAnimalCharacterTemplateIds.Count;
			*(int*)ptr = count;
			ptr += 4;
			foreach (KeyValuePair<short, short> blockAnimalCharacterTemplateId in BlockAnimalCharacterTemplateIds)
			{
				*(short*)ptr = blockAnimalCharacterTemplateId.Key;
				ptr += 2;
				*(short*)ptr = blockAnimalCharacterTemplateId.Value;
				ptr += 2;
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
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
		uint num = *(uint*)ptr;
		ptr += 4;
		if (num != 0)
		{
			if (BlockAnimalCharacterTemplateIds == null)
			{
				BlockAnimalCharacterTemplateIds = new Dictionary<short, short>();
			}
			else
			{
				BlockAnimalCharacterTemplateIds.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				short key = *(short*)ptr;
				ptr += 2;
				short value = *(short*)ptr;
				ptr += 2;
				BlockAnimalCharacterTemplateIds.Add(key, value);
			}
		}
		else
		{
			BlockAnimalCharacterTemplateIds?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
