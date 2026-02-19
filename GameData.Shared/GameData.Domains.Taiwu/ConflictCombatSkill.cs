using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class ConflictCombatSkill : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort TemplateId = 0;

		public const ushort BreakStepCount = 1;

		public const ushort ForcedBreakStepCount = 2;

		public const ushort BreakPlateObsolete = 3;

		public const ushort BonusCollection = 4;

		public const ushort BreakPlate = 5;

		public const ushort Count = 6;

		public static readonly string[] FieldId2FieldName = new string[6] { "TemplateId", "BreakStepCount", "ForcedBreakStepCount", "BreakPlateObsolete", "BonusCollection", "BreakPlate" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public short TemplateId;

	[SerializableGameDataField(FieldIndex = 1)]
	public sbyte BreakStepCount;

	[SerializableGameDataField(FieldIndex = 2)]
	public sbyte ForcedBreakStepCount;

	[SerializableGameDataField(FieldIndex = 5)]
	public SkillBreakPlate BreakPlate;

	[SerializableGameDataField(FieldIndex = 3)]
	public SkillBreakPlateObsolete BreakPlateObsolete;

	[SerializableGameDataField(FieldIndex = 4)]
	public SkillBreakBonusCollection BonusCollection;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		num = ((BreakPlateObsolete == null) ? (num + 2) : (num + (2 + BreakPlateObsolete.GetSerializedSize())));
		num = ((BonusCollection == null) ? (num + 2) : (num + (2 + BonusCollection.GetSerializedSize())));
		num = ((BreakPlate == null) ? (num + 2) : (num + (2 + BreakPlate.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 6;
		ptr += 2;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*ptr = (byte)BreakStepCount;
		ptr++;
		*ptr = (byte)ForcedBreakStepCount;
		ptr++;
		if (BreakPlateObsolete != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = BreakPlateObsolete.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BonusCollection != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = BonusCollection.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BreakPlate != null)
		{
			byte* intPtr3 = ptr;
			ptr += 2;
			int num3 = BreakPlate.Serialize(ptr);
			ptr += num3;
			Tester.Assert(num3 <= 65535);
			*(ushort*)intPtr3 = (ushort)num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num4 = (int)(ptr - pData);
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
			BreakStepCount = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 2)
		{
			ForcedBreakStepCount = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 3)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (BreakPlateObsolete == null)
				{
					BreakPlateObsolete = new SkillBreakPlateObsolete();
				}
				ptr += BreakPlateObsolete.Deserialize(ptr);
			}
			else
			{
				BreakPlateObsolete = null;
			}
		}
		if (num > 4)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (BonusCollection == null)
				{
					BonusCollection = new SkillBreakBonusCollection();
				}
				ptr += BonusCollection.Deserialize(ptr);
			}
			else
			{
				BonusCollection = null;
			}
		}
		if (num > 5)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (BreakPlate == null)
				{
					BreakPlate = new SkillBreakPlate();
				}
				ptr += BreakPlate.Deserialize(ptr);
			}
			else
			{
				BreakPlate = null;
			}
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
