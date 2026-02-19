using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Map;

public class AnimalAreaData : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<short, List<short>> BlockAnimalCharacterTemplateIdList = new Dictionary<short, List<short>>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		if (BlockAnimalCharacterTemplateIdList != null)
		{
			num += 4;
			foreach (KeyValuePair<short, List<short>> blockAnimalCharacterTemplateId in BlockAnimalCharacterTemplateIdList)
			{
				num += 2;
				List<short> value = blockAnimalCharacterTemplateId.Value;
				if (value != null)
				{
					num += 4;
					foreach (short item in value)
					{
						_ = item;
						num += 2;
					}
				}
				else
				{
					num += 4;
				}
			}
		}
		else
		{
			num += 4;
		}
		int num2 = num;
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (BlockAnimalCharacterTemplateIdList != null)
		{
			int count = BlockAnimalCharacterTemplateIdList.Count;
			*(int*)ptr = count;
			ptr += 4;
			foreach (KeyValuePair<short, List<short>> blockAnimalCharacterTemplateId in BlockAnimalCharacterTemplateIdList)
			{
				*(short*)ptr = blockAnimalCharacterTemplateId.Key;
				ptr += 2;
				List<short> value = blockAnimalCharacterTemplateId.Value;
				if (value != null)
				{
					*(int*)ptr = value.Count;
					ptr += 4;
					foreach (short item in value)
					{
						*(short*)ptr = item;
						ptr += 2;
					}
				}
				else
				{
					*(int*)ptr = 0;
					ptr += 4;
				}
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
			if (BlockAnimalCharacterTemplateIdList == null)
			{
				BlockAnimalCharacterTemplateIdList = new Dictionary<short, List<short>>();
			}
			else
			{
				BlockAnimalCharacterTemplateIdList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				short key = *(short*)ptr;
				ptr += 2;
				int num2 = *(int*)ptr;
				ptr += 4;
				if (num2 != 0)
				{
					List<short> list = new List<short>();
					for (int j = 0; j < num2; j++)
					{
						short item = *(short*)ptr;
						ptr += 2;
						list.Add(item);
					}
					BlockAnimalCharacterTemplateIdList.Add(key, list);
				}
			}
		}
		else
		{
			BlockAnimalCharacterTemplateIdList?.Clear();
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
