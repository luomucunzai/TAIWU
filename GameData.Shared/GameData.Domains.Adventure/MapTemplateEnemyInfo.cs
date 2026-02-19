using System;
using Config;
using GameData.Serializer;

namespace GameData.Domains.Adventure;

[Serializable]
public struct MapTemplateEnemyInfo : ISerializableGameData, IEquatable<MapTemplateEnemyInfo>
{
	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public short BlockId;

	[SerializableGameDataField]
	public short SourceAdventureBlockId;

	[SerializableGameDataField]
	public sbyte Duration;

	public static readonly MapTemplateEnemyInfo Invalid = new MapTemplateEnemyInfo(-1, -1, -1);

	public static sbyte DefaultDuration(int taiwuConsummateLevel)
	{
		return (sbyte)Math.Clamp(3 + taiwuConsummateLevel * GlobalConfig.Instance.GeneratedXiangshuMinionDurationFactor / 100 / 2, 1, 12);
	}

	public bool IsValid()
	{
		if (TemplateId >= 0)
		{
			return BlockId >= 0;
		}
		return false;
	}

	public MapTemplateEnemyInfo(short templateId, short blockId, short sourceAdventureBlockId)
		: this(templateId, blockId, sourceAdventureBlockId, -1)
	{
	}

	public MapTemplateEnemyInfo(short templateId, short blockId, short sourceAdventureBlockId, sbyte duration)
	{
		TemplateId = templateId;
		BlockId = blockId;
		SourceAdventureBlockId = sourceAdventureBlockId;
		Duration = duration;
	}

	public override string ToString()
	{
		return $"{Config.Character.Instance[TemplateId].Surname}{Config.Character.Instance[TemplateId].GivenName}, Pos:{BlockId}, Source:{SourceAdventureBlockId}, Duration: {Duration}";
	}

	public bool Equals(MapTemplateEnemyInfo other)
	{
		if (TemplateId == other.TemplateId && BlockId == other.BlockId && SourceAdventureBlockId == other.SourceAdventureBlockId)
		{
			return Duration == other.Duration;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is MapTemplateEnemyInfo other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (TemplateId.GetHashCode() * 397) ^ BlockId.GetHashCode();
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
		*(short*)pData = TemplateId;
		byte* num = pData + 2;
		*(short*)num = BlockId;
		byte* num2 = num + 2;
		*(short*)num2 = SourceAdventureBlockId;
		byte* num3 = num2 + 2;
		*num3 = (byte)Duration;
		int num4 = (int)(num3 + 1 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TemplateId = *(short*)ptr;
		ptr += 2;
		BlockId = *(short*)ptr;
		ptr += 2;
		SourceAdventureBlockId = *(short*)ptr;
		ptr += 2;
		Duration = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
