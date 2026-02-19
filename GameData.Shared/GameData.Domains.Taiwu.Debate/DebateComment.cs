using GameData.Serializer;

namespace GameData.Domains.Taiwu.Debate;

public class DebateComment : ISerializableGameData
{
	[SerializableGameDataField]
	public int SpectatorId;

	[SerializableGameDataField]
	public int PlayerId;

	[SerializableGameDataField]
	public short TemplateId;

	public DebateComment(int spectatorId, int playerId, short templateId)
	{
		SpectatorId = spectatorId;
		PlayerId = playerId;
		TemplateId = templateId;
	}

	public DebateComment()
	{
	}

	public DebateComment(DebateComment other)
	{
		SpectatorId = other.SpectatorId;
		PlayerId = other.PlayerId;
		TemplateId = other.TemplateId;
	}

	public void Assign(DebateComment other)
	{
		SpectatorId = other.SpectatorId;
		PlayerId = other.PlayerId;
		TemplateId = other.TemplateId;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = SpectatorId;
		byte* num = pData + 4;
		*(int*)num = PlayerId;
		byte* num2 = num + 4;
		*(short*)num2 = TemplateId;
		int num3 = (int)(num2 + 2 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		SpectatorId = *(int*)ptr;
		ptr += 4;
		PlayerId = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
