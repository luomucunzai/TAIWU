using GameData.Serializer;

namespace GameData.Domains.Item;

[SerializableGameData(NotForArchive = true)]
public struct TemplateKeyAndCount : ISerializableGameData
{
	[SerializableGameDataField]
	public TemplateKey TemplateKey;

	[SerializableGameDataField]
	public int Count;

	public static implicit operator TemplateKeyAndCount((TemplateKey templateKey, int count) tuple)
	{
		TemplateKeyAndCount result = default(TemplateKeyAndCount);
		(result.TemplateKey, result.Count) = tuple;
		return result;
	}

	public static implicit operator TemplateKeyAndCount(TemplateKey templateKey)
	{
		return new TemplateKeyAndCount
		{
			TemplateKey = templateKey,
			Count = 1
		};
	}

	public void Deconstruct(out TemplateKey templateKey, out int count)
	{
		templateKey = TemplateKey;
		count = Count;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 7;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += TemplateKey.Serialize(ptr);
		*(int*)ptr = Count;
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
		ptr += TemplateKey.Deserialize(ptr);
		Count = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
