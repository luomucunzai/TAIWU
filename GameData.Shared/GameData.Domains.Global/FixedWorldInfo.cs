using System;
using System.Collections.Generic;
using System.Text;
using GameData.Domains.Character.Display;
using GameData.Domains.Mod;
using GameData.Domains.World;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Global;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class FixedWorldInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public int CurrDate;

	[SerializableGameDataField]
	public int TaiwuGenerationsCount;

	[SerializableGameDataField]
	public long SavingTimestamp;

	[SerializableGameDataField]
	public string TaiwuSurname;

	[SerializableGameDataField]
	public string TaiwuGivenName;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public AvatarRelatedData AvatarRelatedData;

	[SerializableGameDataField]
	public string MapStateName;

	[SerializableGameDataField]
	public string MapAreaName;

	[SerializableGameDataField]
	public byte CharacterLifespanType;

	[SerializableGameDataField]
	public byte CombatDifficulty;

	[SerializableGameDataField]
	public byte HereticsAmountType;

	[SerializableGameDataField]
	public byte BossInvasionSpeedType;

	[SerializableGameDataField]
	public byte WorldResourceAmountType;

	[SerializableGameDataField]
	public bool AllowRandomTaiwuHeir;

	[SerializableGameDataField]
	public bool RestrictOptionsBehaviorType;

	[SerializableGameDataField]
	public sbyte[] StateTaskStatuses;

	[SerializableGameDataField]
	public XiangshuAvatarTaskStatus[] XiangshuAvatarTaskStatuses;

	[SerializableGameDataField]
	public short MainStoryLineProgress;

	[SerializableGameDataField]
	public bool BeatRanChenZi;

	[SerializableGameDataField]
	public List<ModId> ModIds;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 111;
		num = ((TaiwuSurname == null) ? (num + 2) : (num + (2 + 2 * TaiwuSurname.Length)));
		num = ((TaiwuGivenName == null) ? (num + 2) : (num + (2 + 2 * TaiwuGivenName.Length)));
		num = ((MapStateName == null) ? (num + 2) : (num + (2 + 2 * MapStateName.Length)));
		num = ((MapAreaName == null) ? (num + 2) : (num + (2 + 2 * MapAreaName.Length)));
		num = ((StateTaskStatuses == null) ? (num + 2) : (num + (2 + StateTaskStatuses.Length)));
		num = ((XiangshuAvatarTaskStatuses == null) ? (num + 2) : (num + (2 + 8 * XiangshuAvatarTaskStatuses.Length)));
		num = ((ModIds == null) ? (num + 2) : (num + (2 + 20 * ModIds.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CurrDate;
		ptr += 4;
		*(int*)ptr = TaiwuGenerationsCount;
		ptr += 4;
		*(long*)ptr = SavingTimestamp;
		ptr += 8;
		if (TaiwuSurname != null)
		{
			int length = TaiwuSurname.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* taiwuSurname = TaiwuSurname)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)taiwuSurname[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (TaiwuGivenName != null)
		{
			int length2 = TaiwuGivenName.Length;
			Tester.Assert(length2 <= 65535);
			*(ushort*)ptr = (ushort)length2;
			ptr += 2;
			fixed (char* taiwuGivenName = TaiwuGivenName)
			{
				for (int j = 0; j < length2; j++)
				{
					((short*)ptr)[j] = (short)taiwuGivenName[j];
				}
			}
			ptr += 2 * length2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)Gender;
		ptr++;
		ptr += AvatarRelatedData.Serialize(ptr);
		if (MapStateName != null)
		{
			int length3 = MapStateName.Length;
			Tester.Assert(length3 <= 65535);
			*(ushort*)ptr = (ushort)length3;
			ptr += 2;
			fixed (char* mapStateName = MapStateName)
			{
				for (int k = 0; k < length3; k++)
				{
					((short*)ptr)[k] = (short)mapStateName[k];
				}
			}
			ptr += 2 * length3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (MapAreaName != null)
		{
			int length4 = MapAreaName.Length;
			Tester.Assert(length4 <= 65535);
			*(ushort*)ptr = (ushort)length4;
			ptr += 2;
			fixed (char* mapAreaName = MapAreaName)
			{
				for (int l = 0; l < length4; l++)
				{
					((short*)ptr)[l] = (short)mapAreaName[l];
				}
			}
			ptr += 2 * length4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = CharacterLifespanType;
		ptr++;
		*ptr = CombatDifficulty;
		ptr++;
		*ptr = HereticsAmountType;
		ptr++;
		*ptr = BossInvasionSpeedType;
		ptr++;
		*ptr = WorldResourceAmountType;
		ptr++;
		*ptr = (AllowRandomTaiwuHeir ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (RestrictOptionsBehaviorType ? ((byte)1) : ((byte)0));
		ptr++;
		if (StateTaskStatuses != null)
		{
			int num = StateTaskStatuses.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int m = 0; m < num; m++)
			{
				ptr[m] = (byte)StateTaskStatuses[m];
			}
			ptr += num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (XiangshuAvatarTaskStatuses != null)
		{
			int num2 = XiangshuAvatarTaskStatuses.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int n = 0; n < num2; n++)
			{
				ptr += XiangshuAvatarTaskStatuses[n].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = MainStoryLineProgress;
		ptr += 2;
		*ptr = (BeatRanChenZi ? ((byte)1) : ((byte)0));
		ptr++;
		if (ModIds != null)
		{
			int count = ModIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int num3 = 0; num3 < count; num3++)
			{
				ptr += ModIds[num3].Serialize(ptr);
			}
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
		CurrDate = *(int*)ptr;
		ptr += 4;
		TaiwuGenerationsCount = *(int*)ptr;
		ptr += 4;
		SavingTimestamp = *(long*)ptr;
		ptr += 8;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			TaiwuSurname = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			TaiwuSurname = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			int num4 = 2 * num3;
			TaiwuGivenName = Encoding.Unicode.GetString(ptr, num4);
			ptr += num4;
		}
		else
		{
			TaiwuGivenName = null;
		}
		Gender = (sbyte)(*ptr);
		ptr++;
		if (AvatarRelatedData == null)
		{
			AvatarRelatedData = new AvatarRelatedData();
		}
		ptr += AvatarRelatedData.Deserialize(ptr);
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			int num6 = 2 * num5;
			MapStateName = Encoding.Unicode.GetString(ptr, num6);
			ptr += num6;
		}
		else
		{
			MapStateName = null;
		}
		ushort num7 = *(ushort*)ptr;
		ptr += 2;
		if (num7 > 0)
		{
			int num8 = 2 * num7;
			MapAreaName = Encoding.Unicode.GetString(ptr, num8);
			ptr += num8;
		}
		else
		{
			MapAreaName = null;
		}
		CharacterLifespanType = *ptr;
		ptr++;
		CombatDifficulty = *ptr;
		ptr++;
		HereticsAmountType = *ptr;
		ptr++;
		BossInvasionSpeedType = *ptr;
		ptr++;
		WorldResourceAmountType = *ptr;
		ptr++;
		AllowRandomTaiwuHeir = *ptr != 0;
		ptr++;
		RestrictOptionsBehaviorType = *ptr != 0;
		ptr++;
		ushort num9 = *(ushort*)ptr;
		ptr += 2;
		if (num9 > 0)
		{
			if (StateTaskStatuses == null || StateTaskStatuses.Length != num9)
			{
				StateTaskStatuses = new sbyte[num9];
			}
			for (int i = 0; i < num9; i++)
			{
				StateTaskStatuses[i] = (sbyte)ptr[i];
			}
			ptr += (int)num9;
		}
		else
		{
			StateTaskStatuses = null;
		}
		ushort num10 = *(ushort*)ptr;
		ptr += 2;
		if (num10 > 0)
		{
			if (XiangshuAvatarTaskStatuses == null || XiangshuAvatarTaskStatuses.Length != num10)
			{
				XiangshuAvatarTaskStatuses = new XiangshuAvatarTaskStatus[num10];
			}
			for (int j = 0; j < num10; j++)
			{
				XiangshuAvatarTaskStatus xiangshuAvatarTaskStatus = default(XiangshuAvatarTaskStatus);
				ptr += xiangshuAvatarTaskStatus.Deserialize(ptr);
				XiangshuAvatarTaskStatuses[j] = xiangshuAvatarTaskStatus;
			}
		}
		else
		{
			XiangshuAvatarTaskStatuses = null;
		}
		MainStoryLineProgress = *(short*)ptr;
		ptr += 2;
		BeatRanChenZi = *ptr != 0;
		ptr++;
		ushort num11 = *(ushort*)ptr;
		ptr += 2;
		if (num11 > 0)
		{
			if (ModIds == null)
			{
				ModIds = new List<ModId>(num11);
			}
			else
			{
				ModIds.Clear();
			}
			for (int k = 0; k < num11; k++)
			{
				ModId item = default(ModId);
				ptr += item.Deserialize(ptr);
				ModIds.Add(item);
			}
		}
		else
		{
			ModIds?.Clear();
		}
		int num12 = (int)(ptr - pData);
		if (num12 > 4)
		{
			return (num12 + 3) / 4 * 4;
		}
		return num12;
	}
}
