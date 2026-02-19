using GameData.Serializer;

namespace GameData.Domains.World.SectMainStory;

[SerializableGameData(IsExtensible = true)]
public struct SectEmeiBreakBonusData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort TemplateId = 0;

		public const ushort BonusCount = 1;

		public const ushort BonusProgress = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "TemplateId", "BonusCount", "BonusProgress" };
	}

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public int BonusCount;

	[SerializableGameDataField]
	public int BonusProgress;

	public void OfflineAddProgress(int progress)
	{
		BonusProgress += progress;
		BonusCount += BonusProgress / GlobalConfig.Instance.SectStoryEmeiBonusProgressPerCount;
		BonusProgress %= GlobalConfig.Instance.SectStoryEmeiBonusProgressPerCount;
	}

	public void OfflineMerge(SectEmeiBreakBonusData data)
	{
		BonusCount += data.BonusCount;
		OfflineAddProgress(data.BonusProgress);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 3;
		byte* num = pData + 2;
		*(short*)num = TemplateId;
		byte* num2 = num + 2;
		*(int*)num2 = BonusCount;
		byte* num3 = num2 + 4;
		*(int*)num3 = BonusProgress;
		int num4 = (int)(num3 + 4 - pData);
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
			TemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 1)
		{
			BonusCount = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			BonusProgress = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
