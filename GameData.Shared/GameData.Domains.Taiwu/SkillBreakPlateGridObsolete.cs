using GameData.Serializer;

namespace GameData.Domains.Taiwu;

[SerializableGameData]
public class SkillBreakPlateGridObsolete : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte TemplateId;

	[SerializableGameDataField]
	public sbyte SuccessRateFix;

	[SerializableGameDataField]
	private sbyte _internalState;

	[SerializableGameDataField]
	public short BonusType;

	[SerializableGameDataField]
	public short RecordedSuccessRate;

	public ESkillBreakGridState State => (ESkillBreakGridState)_internalState;

	public SkillBreakPlateGridObsolete()
	{
	}

	public SkillBreakPlateGridObsolete(SkillBreakPlateGridObsolete other)
	{
		TemplateId = other.TemplateId;
		SuccessRateFix = other.SuccessRateFix;
		_internalState = other._internalState;
		BonusType = other.BonusType;
		RecordedSuccessRate = other.RecordedSuccessRate;
	}

	public void Assign(SkillBreakPlateGridObsolete other)
	{
		TemplateId = other.TemplateId;
		SuccessRateFix = other.SuccessRateFix;
		_internalState = other._internalState;
		BonusType = other.BonusType;
		RecordedSuccessRate = other.RecordedSuccessRate;
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
		*pData = (byte)TemplateId;
		byte* num = pData + 1;
		*num = (byte)SuccessRateFix;
		byte* num2 = num + 1;
		*num2 = (byte)_internalState;
		byte* num3 = num2 + 1;
		*(short*)num3 = BonusType;
		byte* num4 = num3 + 2;
		*(short*)num4 = RecordedSuccessRate;
		int num5 = (int)(num4 + 2 - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TemplateId = (sbyte)(*ptr);
		ptr++;
		SuccessRateFix = (sbyte)(*ptr);
		ptr++;
		_internalState = (sbyte)(*ptr);
		ptr++;
		BonusType = *(short*)ptr;
		ptr += 2;
		RecordedSuccessRate = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
