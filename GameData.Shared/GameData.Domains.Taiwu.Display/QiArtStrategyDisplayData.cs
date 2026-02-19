using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class QiArtStrategyDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte TemplateId;

	[SerializableGameDataField]
	public int ExpireTime;

	public QiArtStrategyDisplayData()
	{
	}

	public QiArtStrategyDisplayData(QiArtStrategyDisplayData other)
	{
		TemplateId = other.TemplateId;
		ExpireTime = other.ExpireTime;
	}

	public void Assign(QiArtStrategyDisplayData other)
	{
		TemplateId = other.TemplateId;
		ExpireTime = other.ExpireTime;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 5;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)TemplateId;
		byte* num = pData + 1;
		*(int*)num = ExpireTime;
		int num2 = (int)(num + 4 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TemplateId = (sbyte)(*ptr);
		ptr++;
		ExpireTime = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
