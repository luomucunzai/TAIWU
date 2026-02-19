using System.Collections.Generic;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession;

public class ProfessionSkillArg : ISerializableGameData
{
	[SerializableGameDataField]
	public int ProfessionId;

	[SerializableGameDataField]
	public int SkillId;

	[SerializableGameDataField]
	public bool IsSuccess;

	[SerializableGameDataField]
	public ItemKey ItemKey;

	[SerializableGameDataField]
	public int CharId;

	[SerializableGameDataField]
	public sbyte CombatSkillType;

	[SerializableGameDataField]
	public sbyte LifeSkillType;

	[SerializableGameDataField]
	public short EffectId;

	[SerializableGameDataField]
	public bool IsExtraordinary;

	[SerializableGameDataField]
	public List<int> CharIds;

	[SerializableGameDataField]
	public List<int> BookIds;

	[SerializableGameDataField]
	public bool SkipConfirm;

	[SerializableGameDataField]
	public bool SkipAnimation;

	[SerializableGameDataField]
	public ItemDisplayData MakeMedicineCostMedicine;

	[SerializableGameDataField]
	public ItemDisplayData MakeMedicineCostTool;

	[SerializableGameDataField]
	public int MakeMedicineCount;

	[SerializableGameDataField]
	public Location ProfessionTravelerTargetLocation;

	[SerializableGameDataField]
	public List<short> EffectBlocks;

	[SerializableGameDataField]
	public CombatResultDisplayData CombatResultData;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 36;
		num = ((CharIds == null) ? (num + 2) : (num + (2 + 4 * CharIds.Count)));
		num = ((BookIds == null) ? (num + 2) : (num + (2 + 4 * BookIds.Count)));
		num = ((MakeMedicineCostMedicine == null) ? (num + 2) : (num + (2 + MakeMedicineCostMedicine.GetSerializedSize())));
		num = ((MakeMedicineCostTool == null) ? (num + 2) : (num + (2 + MakeMedicineCostTool.GetSerializedSize())));
		num = ((EffectBlocks == null) ? (num + 2) : (num + (2 + 2 * EffectBlocks.Count)));
		num = ((CombatResultData == null) ? (num + 2) : (num + (2 + CombatResultData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = ProfessionId;
		ptr += 4;
		*(int*)ptr = SkillId;
		ptr += 4;
		*ptr = (IsSuccess ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += ItemKey.Serialize(ptr);
		*(int*)ptr = CharId;
		ptr += 4;
		*ptr = (byte)CombatSkillType;
		ptr++;
		*ptr = (byte)LifeSkillType;
		ptr++;
		*(short*)ptr = EffectId;
		ptr += 2;
		*ptr = (IsExtraordinary ? ((byte)1) : ((byte)0));
		ptr++;
		if (CharIds != null)
		{
			int count = CharIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = CharIds[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BookIds != null)
		{
			int count2 = BookIds.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((int*)ptr)[j] = BookIds[j];
			}
			ptr += 4 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (SkipConfirm ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (SkipAnimation ? ((byte)1) : ((byte)0));
		ptr++;
		if (MakeMedicineCostMedicine != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = MakeMedicineCostMedicine.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (MakeMedicineCostTool != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = MakeMedicineCostTool.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = MakeMedicineCount;
		ptr += 4;
		ptr += ProfessionTravelerTargetLocation.Serialize(ptr);
		if (EffectBlocks != null)
		{
			int count3 = EffectBlocks.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((short*)ptr)[k] = EffectBlocks[k];
			}
			ptr += 2 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CombatResultData != null)
		{
			byte* intPtr3 = ptr;
			ptr += 2;
			int num3 = CombatResultData.Serialize(ptr);
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
		ProfessionId = *(int*)ptr;
		ptr += 4;
		SkillId = *(int*)ptr;
		ptr += 4;
		IsSuccess = *ptr != 0;
		ptr++;
		ptr += ItemKey.Deserialize(ptr);
		CharId = *(int*)ptr;
		ptr += 4;
		CombatSkillType = (sbyte)(*ptr);
		ptr++;
		LifeSkillType = (sbyte)(*ptr);
		ptr++;
		EffectId = *(short*)ptr;
		ptr += 2;
		IsExtraordinary = *ptr != 0;
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CharIds == null)
			{
				CharIds = new List<int>(num);
			}
			else
			{
				CharIds.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				CharIds.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			CharIds?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (BookIds == null)
			{
				BookIds = new List<int>(num2);
			}
			else
			{
				BookIds.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				BookIds.Add(((int*)ptr)[j]);
			}
			ptr += 4 * num2;
		}
		else
		{
			BookIds?.Clear();
		}
		SkipConfirm = *ptr != 0;
		ptr++;
		SkipAnimation = *ptr != 0;
		ptr++;
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (MakeMedicineCostMedicine == null)
			{
				MakeMedicineCostMedicine = new ItemDisplayData();
			}
			ptr += MakeMedicineCostMedicine.Deserialize(ptr);
		}
		else
		{
			MakeMedicineCostMedicine = null;
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (MakeMedicineCostTool == null)
			{
				MakeMedicineCostTool = new ItemDisplayData();
			}
			ptr += MakeMedicineCostTool.Deserialize(ptr);
		}
		else
		{
			MakeMedicineCostTool = null;
		}
		MakeMedicineCount = *(int*)ptr;
		ptr += 4;
		ptr += ProfessionTravelerTargetLocation.Deserialize(ptr);
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (EffectBlocks == null)
			{
				EffectBlocks = new List<short>(num5);
			}
			else
			{
				EffectBlocks.Clear();
			}
			for (int k = 0; k < num5; k++)
			{
				EffectBlocks.Add(((short*)ptr)[k]);
			}
			ptr += 2 * num5;
		}
		else
		{
			EffectBlocks?.Clear();
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (CombatResultData == null)
			{
				CombatResultData = new CombatResultDisplayData();
			}
			ptr += CombatResultData.Deserialize(ptr);
		}
		else
		{
			CombatResultData = null;
		}
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}
}
