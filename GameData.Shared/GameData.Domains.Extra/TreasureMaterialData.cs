using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Extra;

public class TreasureMaterialData : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<short, List<short>> BlockMaterialTemplateIds = new Dictionary<short, List<short>>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		if (BlockMaterialTemplateIds != null)
		{
			foreach (List<short> value in BlockMaterialTemplateIds.Values)
			{
				num += 6;
				num += 2 * (value?.Count ?? 0);
			}
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
		if (BlockMaterialTemplateIds != null)
		{
			int count = BlockMaterialTemplateIds.Count;
			*(int*)ptr = count;
			ptr += 4;
			foreach (KeyValuePair<short, List<short>> blockMaterialTemplateId in BlockMaterialTemplateIds)
			{
				*(short*)ptr = blockMaterialTemplateId.Key;
				ptr += 2;
				List<short> value = blockMaterialTemplateId.Value;
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
			if (BlockMaterialTemplateIds == null)
			{
				BlockMaterialTemplateIds = new Dictionary<short, List<short>>();
			}
			else
			{
				BlockMaterialTemplateIds.Clear();
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
					BlockMaterialTemplateIds.Add(key, list);
				}
			}
		}
		else
		{
			BlockMaterialTemplateIds?.Clear();
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
