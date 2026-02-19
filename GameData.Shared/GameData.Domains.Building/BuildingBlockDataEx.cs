using System;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Building;

[AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class BuildingBlockDataEx : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort AutoGiveMemberPresetId = 0;

		public const ushort LevelUnlockedFlags = 1;

		public const ushort Key = 2;

		public const ushort CumulatedScore = 3;

		public const ushort ArrangementSetting = 4;

		public const ushort SoldItemSetting = 5;

		public const ushort Count = 6;

		public static readonly string[] FieldId2FieldName = new string[6] { "AutoGiveMemberPresetId", "LevelUnlockedFlags", "Key", "CumulatedScore", "ArrangementSetting", "SoldItemSetting" };
	}

	[Obsolete]
	[SerializableGameDataField(FieldIndex = 0)]
	public int AutoGiveMemberPresetId;

	[SerializableGameDataField(FieldIndex = 1)]
	public ulong LevelUnlockedFlags = 1uL;

	[SerializableGameDataField(FieldIndex = 2)]
	public BuildingBlockKey Key;

	[SerializableGameDataField(FieldIndex = 3)]
	public int CumulatedScore;

	[SerializableGameDataField(FieldIndex = 4)]
	public BuildingOptionAutoGiveMemberPreset ArrangementSetting;

	[SerializableGameDataField(FieldIndex = 5)]
	public BuildingOptionAutoAddSoldItemPreset SoldItemSetting;

	public sbyte CalcUnlockedLevelCount()
	{
		sbyte b = 0;
		ulong num = LevelUnlockedFlags;
		while (num != 0L)
		{
			num &= num - 1;
			b++;
		}
		return b;
	}

	public void ResetInitialUnlockedSlot(int index)
	{
		LevelUnlockedFlags = (uint)(1 << index);
	}

	public void UnlockLevelSlot(int index)
	{
		ulong num = (uint)(1 << index);
		LevelUnlockedFlags |= num;
	}

	public bool SlotIsUnlocked(int index)
	{
		ulong num = (uint)(1 << index);
		return (LevelUnlockedFlags & num) != 0;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 18;
		num += Key.GetSerializedSize();
		num = ((ArrangementSetting == null) ? (num + 2) : (num + (2 + ArrangementSetting.GetSerializedSize())));
		num = ((SoldItemSetting == null) ? (num + 2) : (num + (2 + SoldItemSetting.GetSerializedSize())));
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
		*(int*)ptr = AutoGiveMemberPresetId;
		ptr += 4;
		*(ulong*)ptr = LevelUnlockedFlags;
		ptr += 8;
		ptr += Key.Serialize(ptr);
		*(int*)ptr = CumulatedScore;
		ptr += 4;
		if (ArrangementSetting != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = ArrangementSetting.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SoldItemSetting != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = SoldItemSetting.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			AutoGiveMemberPresetId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			LevelUnlockedFlags = *(ulong*)ptr;
			ptr += 8;
		}
		if (num > 2)
		{
			ptr += Key.Deserialize(ptr);
		}
		if (num > 3)
		{
			CumulatedScore = *(int*)ptr;
			ptr += 4;
		}
		if (num > 4)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				ArrangementSetting = new BuildingOptionAutoGiveMemberPreset();
				ptr += ArrangementSetting.Deserialize(ptr);
			}
			else
			{
				ArrangementSetting = null;
			}
		}
		if (num > 5)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				SoldItemSetting = new BuildingOptionAutoAddSoldItemPreset();
				ptr += SoldItemSetting.Deserialize(ptr);
			}
			else
			{
				SoldItemSetting = null;
			}
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
