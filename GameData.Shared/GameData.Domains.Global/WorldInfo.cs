using System.Collections.Generic;
using System.Text;
using GameData.DLC;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Domains.Mod;
using GameData.Domains.World;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Global;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class WorldInfo : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort CurrDate = 0;

		public const ushort TaiwuGenerationsCount = 1;

		public const ushort SavingTimestamp = 2;

		public const ushort TaiwuSurname = 3;

		public const ushort TaiwuGivenName = 4;

		public const ushort Gender = 5;

		public const ushort AvatarRelatedData = 6;

		public const ushort MapStateName = 7;

		public const ushort MapAreaName = 8;

		public const ushort CharacterLifespanType = 9;

		public const ushort CombatDifficulty = 10;

		public const ushort ReadingDifficulty = 11;

		public const ushort BreakoutDifficulty = 12;

		public const ushort LoopingDifficulty = 13;

		public const ushort HereticsAmountType = 14;

		public const ushort BossInvasionSpeedType = 15;

		public const ushort WorldResourceAmountType = 16;

		public const ushort AllowRandomTaiwuHeir = 17;

		public const ushort RestrictOptionsBehaviorType = 18;

		public const ushort StateTaskStatuses = 19;

		public const ushort XiangshuAvatarTaskStatuses = 20;

		public const ushort MainStoryLineProgress = 21;

		public const ushort BeatRanChenZi = 22;

		public const ushort ModIds = 23;

		public const ushort WorldPopulationType = 24;

		public const ushort EnemyPracticeLevel = 25;

		public const ushort GameVersionInfo = 26;

		public const ushort FavorabilityChange = 27;

		public const ushort DlcIds = 28;

		public const ushort ProfessionUpgrade = 29;

		public const ushort AvatarExtraData = 30;

		public const ushort LootYield = 31;

		public const ushort Count = 32;

		public static readonly string[] FieldId2FieldName = new string[32]
		{
			"CurrDate", "TaiwuGenerationsCount", "SavingTimestamp", "TaiwuSurname", "TaiwuGivenName", "Gender", "AvatarRelatedData", "MapStateName", "MapAreaName", "CharacterLifespanType",
			"CombatDifficulty", "ReadingDifficulty", "BreakoutDifficulty", "LoopingDifficulty", "HereticsAmountType", "BossInvasionSpeedType", "WorldResourceAmountType", "AllowRandomTaiwuHeir", "RestrictOptionsBehaviorType", "StateTaskStatuses",
			"XiangshuAvatarTaskStatuses", "MainStoryLineProgress", "BeatRanChenZi", "ModIds", "WorldPopulationType", "EnemyPracticeLevel", "GameVersionInfo", "FavorabilityChange", "DlcIds", "ProfessionUpgrade",
			"AvatarExtraData", "LootYield"
		};
	}

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
	public AvatarExtraData AvatarExtraData = new AvatarExtraData();

	[SerializableGameDataField]
	public string MapStateName;

	[SerializableGameDataField]
	public string MapAreaName;

	[SerializableGameDataField]
	public byte CharacterLifespanType;

	[SerializableGameDataField]
	public byte CombatDifficulty;

	[SerializableGameDataField]
	public byte ReadingDifficulty;

	[SerializableGameDataField]
	public byte BreakoutDifficulty;

	[SerializableGameDataField]
	public byte LoopingDifficulty;

	[SerializableGameDataField]
	public byte EnemyPracticeLevel;

	[SerializableGameDataField]
	public byte FavorabilityChange;

	[SerializableGameDataField]
	public byte ProfessionUpgrade;

	[SerializableGameDataField]
	public short LootYield;

	[SerializableGameDataField]
	public byte HereticsAmountType;

	[SerializableGameDataField]
	public byte BossInvasionSpeedType;

	[SerializableGameDataField]
	public byte WorldResourceAmountType;

	[SerializableGameDataField]
	public byte WorldPopulationType;

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

	[SerializableGameDataField]
	public List<DlcId> DlcIds;

	[SerializableGameDataField]
	public GameVersionInfo GameVersionInfo;

	public WorldInfo(FixedWorldInfo other)
	{
		CurrDate = other.CurrDate;
		TaiwuGenerationsCount = other.TaiwuGenerationsCount;
		SavingTimestamp = other.SavingTimestamp;
		TaiwuSurname = other.TaiwuSurname;
		TaiwuGivenName = other.TaiwuGivenName;
		Gender = other.Gender;
		AvatarRelatedData = new AvatarRelatedData(other.AvatarRelatedData);
		AvatarExtraData = new AvatarExtraData();
		MapStateName = other.MapStateName;
		MapAreaName = other.MapAreaName;
		CharacterLifespanType = other.CharacterLifespanType;
		CombatDifficulty = other.CombatDifficulty;
		ReadingDifficulty = 1;
		BreakoutDifficulty = 1;
		LoopingDifficulty = 1;
		EnemyPracticeLevel = 1;
		FavorabilityChange = 1;
		ProfessionUpgrade = 1;
		LootYield = 1;
		HereticsAmountType = other.HereticsAmountType;
		BossInvasionSpeedType = other.BossInvasionSpeedType;
		WorldResourceAmountType = other.WorldResourceAmountType;
		WorldPopulationType = 1;
		AllowRandomTaiwuHeir = other.AllowRandomTaiwuHeir;
		RestrictOptionsBehaviorType = other.RestrictOptionsBehaviorType;
		sbyte[] stateTaskStatuses = other.StateTaskStatuses;
		int num = stateTaskStatuses.Length;
		StateTaskStatuses = new sbyte[num];
		for (int i = 0; i < num; i++)
		{
			StateTaskStatuses[i] = stateTaskStatuses[i];
		}
		XiangshuAvatarTaskStatus[] xiangshuAvatarTaskStatuses = other.XiangshuAvatarTaskStatuses;
		int num2 = xiangshuAvatarTaskStatuses.Length;
		XiangshuAvatarTaskStatuses = new XiangshuAvatarTaskStatus[num2];
		for (int j = 0; j < num2; j++)
		{
			XiangshuAvatarTaskStatuses[j] = xiangshuAvatarTaskStatuses[j];
		}
		MainStoryLineProgress = other.MainStoryLineProgress;
		BeatRanChenZi = other.BeatRanChenZi;
		if (other.ModIds != null)
		{
			ModIds = new List<ModId>(other.ModIds);
		}
		else
		{
			ModIds = new List<ModId>();
		}
		GameVersionInfo = null;
	}

	public WorldInfo()
	{
		ReadingDifficulty = 1;
		BreakoutDifficulty = 1;
		LoopingDifficulty = 1;
		EnemyPracticeLevel = 1;
		FavorabilityChange = 1;
		ProfessionUpgrade = 1;
		LootYield = 1;
		WorldPopulationType = 1;
		GameVersionInfo = null;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 124;
		num = ((TaiwuSurname == null) ? (num + 2) : (num + (2 + 2 * TaiwuSurname.Length)));
		num = ((TaiwuGivenName == null) ? (num + 2) : (num + (2 + 2 * TaiwuGivenName.Length)));
		num = ((MapStateName == null) ? (num + 2) : (num + (2 + 2 * MapStateName.Length)));
		num = ((MapAreaName == null) ? (num + 2) : (num + (2 + 2 * MapAreaName.Length)));
		num = ((StateTaskStatuses == null) ? (num + 2) : (num + (2 + StateTaskStatuses.Length)));
		num = ((XiangshuAvatarTaskStatuses == null) ? (num + 2) : (num + (2 + 8 * XiangshuAvatarTaskStatuses.Length)));
		num = ((ModIds == null) ? (num + 2) : (num + (2 + 20 * ModIds.Count)));
		num = ((GameVersionInfo == null) ? (num + 2) : (num + (2 + GameVersionInfo.GetSerializedSize())));
		num = ((DlcIds == null) ? (num + 2) : (num + (2 + 16 * DlcIds.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 32;
		ptr += 2;
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
		*ptr = ReadingDifficulty;
		ptr++;
		*ptr = BreakoutDifficulty;
		ptr++;
		*ptr = LoopingDifficulty;
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
		*ptr = WorldPopulationType;
		ptr++;
		*ptr = EnemyPracticeLevel;
		ptr++;
		if (GameVersionInfo != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num4 = GameVersionInfo.Serialize(ptr);
			ptr += num4;
			Tester.Assert(num4 <= 65535);
			*(ushort*)intPtr = (ushort)num4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = FavorabilityChange;
		ptr++;
		if (DlcIds != null)
		{
			int count2 = DlcIds.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int num5 = 0; num5 < count2; num5++)
			{
				ptr += DlcIds[num5].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = ProfessionUpgrade;
		ptr++;
		ptr += AvatarExtraData.Serialize(ptr);
		*(short*)ptr = LootYield;
		ptr += 2;
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			CurrDate = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			TaiwuGenerationsCount = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			SavingTimestamp = *(long*)ptr;
			ptr += 8;
		}
		if (num > 3)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				int num3 = 2 * num2;
				TaiwuSurname = Encoding.Unicode.GetString(ptr, num3);
				ptr += num3;
			}
			else
			{
				TaiwuSurname = null;
			}
		}
		if (num > 4)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				int num5 = 2 * num4;
				TaiwuGivenName = Encoding.Unicode.GetString(ptr, num5);
				ptr += num5;
			}
			else
			{
				TaiwuGivenName = null;
			}
		}
		if (num > 5)
		{
			Gender = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 6)
		{
			if (AvatarRelatedData == null)
			{
				AvatarRelatedData = new AvatarRelatedData();
			}
			ptr += AvatarRelatedData.Deserialize(ptr);
		}
		if (num > 7)
		{
			ushort num6 = *(ushort*)ptr;
			ptr += 2;
			if (num6 > 0)
			{
				int num7 = 2 * num6;
				MapStateName = Encoding.Unicode.GetString(ptr, num7);
				ptr += num7;
			}
			else
			{
				MapStateName = null;
			}
		}
		if (num > 8)
		{
			ushort num8 = *(ushort*)ptr;
			ptr += 2;
			if (num8 > 0)
			{
				int num9 = 2 * num8;
				MapAreaName = Encoding.Unicode.GetString(ptr, num9);
				ptr += num9;
			}
			else
			{
				MapAreaName = null;
			}
		}
		if (num > 9)
		{
			CharacterLifespanType = *ptr;
			ptr++;
		}
		if (num > 10)
		{
			CombatDifficulty = *ptr;
			ptr++;
		}
		if (num > 11)
		{
			ReadingDifficulty = *ptr;
			ptr++;
		}
		if (num > 12)
		{
			BreakoutDifficulty = *ptr;
			ptr++;
		}
		if (num > 13)
		{
			LoopingDifficulty = *ptr;
			ptr++;
		}
		if (num > 14)
		{
			HereticsAmountType = *ptr;
			ptr++;
		}
		if (num > 15)
		{
			BossInvasionSpeedType = *ptr;
			ptr++;
		}
		if (num > 16)
		{
			WorldResourceAmountType = *ptr;
			ptr++;
		}
		if (num > 17)
		{
			AllowRandomTaiwuHeir = *ptr != 0;
			ptr++;
		}
		if (num > 18)
		{
			RestrictOptionsBehaviorType = *ptr != 0;
			ptr++;
		}
		if (num > 19)
		{
			ushort num10 = *(ushort*)ptr;
			ptr += 2;
			if (num10 > 0)
			{
				if (StateTaskStatuses == null || StateTaskStatuses.Length != num10)
				{
					StateTaskStatuses = new sbyte[num10];
				}
				for (int i = 0; i < num10; i++)
				{
					StateTaskStatuses[i] = (sbyte)ptr[i];
				}
				ptr += (int)num10;
			}
			else
			{
				StateTaskStatuses = null;
			}
		}
		if (num > 20)
		{
			ushort num11 = *(ushort*)ptr;
			ptr += 2;
			if (num11 > 0)
			{
				if (XiangshuAvatarTaskStatuses == null || XiangshuAvatarTaskStatuses.Length != num11)
				{
					XiangshuAvatarTaskStatuses = new XiangshuAvatarTaskStatus[num11];
				}
				for (int j = 0; j < num11; j++)
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
		}
		if (num > 21)
		{
			MainStoryLineProgress = *(short*)ptr;
			ptr += 2;
		}
		if (num > 22)
		{
			BeatRanChenZi = *ptr != 0;
			ptr++;
		}
		if (num > 23)
		{
			ushort num12 = *(ushort*)ptr;
			ptr += 2;
			if (num12 > 0)
			{
				if (ModIds == null)
				{
					ModIds = new List<ModId>(num12);
				}
				else
				{
					ModIds.Clear();
				}
				for (int k = 0; k < num12; k++)
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
		}
		if (num > 24)
		{
			WorldPopulationType = *ptr;
			ptr++;
		}
		if (num > 25)
		{
			EnemyPracticeLevel = *ptr;
			ptr++;
		}
		if (num > 26)
		{
			ushort num13 = *(ushort*)ptr;
			ptr += 2;
			if (num13 > 0)
			{
				if (GameVersionInfo == null)
				{
					GameVersionInfo = new GameVersionInfo();
				}
				ptr += GameVersionInfo.Deserialize(ptr);
			}
			else
			{
				GameVersionInfo = null;
			}
		}
		if (num > 27)
		{
			FavorabilityChange = *ptr;
			ptr++;
		}
		if (num > 28)
		{
			ushort num14 = *(ushort*)ptr;
			ptr += 2;
			if (num14 > 0)
			{
				if (DlcIds == null)
				{
					DlcIds = new List<DlcId>(num14);
				}
				else
				{
					DlcIds.Clear();
				}
				for (int l = 0; l < num14; l++)
				{
					DlcId item2 = default(DlcId);
					ptr += item2.Deserialize(ptr);
					DlcIds.Add(item2);
				}
			}
			else
			{
				DlcIds?.Clear();
			}
		}
		if (num > 29)
		{
			ProfessionUpgrade = *ptr;
			ptr++;
		}
		if (num > 30)
		{
			if (AvatarExtraData == null)
			{
				AvatarExtraData = new AvatarExtraData();
			}
			ptr += AvatarExtraData.Deserialize(ptr);
		}
		if (num > 31)
		{
			LootYield = *(short*)ptr;
			ptr += 2;
		}
		int num15 = (int)(ptr - pData);
		if (num15 > 4)
		{
			return (num15 + 3) / 4 * 4;
		}
		return num15;
	}
}
