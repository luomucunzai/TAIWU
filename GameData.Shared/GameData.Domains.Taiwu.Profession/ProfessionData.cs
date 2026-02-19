using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.Profession;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class ProfessionData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort TemplateId = 0;

		public const ushort Type = 1;

		public const ushort Seniority = 2;

		public const ushort SkillOffCooldownDates = 3;

		public const ushort HadBeenUnlocked = 4;

		public const ushort SkillsData = 5;

		public const ushort ExtraSeniority = 6;

		public const ushort Count = 7;

		public static readonly string[] FieldId2FieldName = new string[7] { "TemplateId", "Type", "Seniority", "SkillOffCooldownDates", "HadBeenUnlocked", "SkillsData", "ExtraSeniority" };
	}

	[SerializableGameDataField]
	public int TemplateId;

	[SerializableGameDataField]
	public sbyte Type;

	[SerializableGameDataField]
	public int Seniority;

	[SerializableGameDataField]
	public int ExtraSeniority;

	[SerializableGameDataField]
	public int[] SkillOffCooldownDates;

	[Obsolete]
	public int ProfessionOffCooldownDate;

	[SerializableGameDataField]
	public bool[] HadBeenUnlocked;

	[SerializableGameDataField]
	public IProfessionSkillsData SkillsData;

	private const int SkillCount = 4;

	public int GetSkillCount()
	{
		ProfessionItem professionItem = Config.Profession.Instance[TemplateId];
		int num = professionItem.ProfessionSkills.Length;
		if (professionItem.ExtraProfessionSkill >= 0)
		{
			num++;
		}
		return num;
	}

	public ProfessionData(int templateId, sbyte type)
	{
		TemplateId = templateId;
		SkillOffCooldownDates = new int[4];
		HadBeenUnlocked = new bool[4];
		SkillsData = CreateExtraData(TemplateId, type);
	}

	public ProfessionData(ObsoleteProfessionData obsoleteProfessionData)
	{
		TemplateId = obsoleteProfessionData.TemplateId;
		Seniority = 300 * obsoleteProfessionData.Seniority;
		ExtraSeniority = 0;
		SkillOffCooldownDates = new int[4];
		for (int i = 0; i < obsoleteProfessionData.SkillOffCooldownDates.Length; i++)
		{
			SkillOffCooldownDates[i] = obsoleteProfessionData.SkillOffCooldownDates[i];
		}
		HadBeenUnlocked = new bool[4];
		for (int j = 0; j < obsoleteProfessionData.HadBeenUnlocked.Length; j++)
		{
			HadBeenUnlocked[j] = obsoleteProfessionData.HadBeenUnlocked[j];
		}
		SkillsData = CreateExtraData(obsoleteProfessionData.TemplateId, 0);
		SkillsData?.InheritFrom(obsoleteProfessionData.SkillsData);
		OfflineUpdateHadBeenUnlocked(isInherit: true);
	}

	public ProfessionItem GetConfig()
	{
		return Config.Profession.Instance[TemplateId];
	}

	public ProfessionSkillItem GetSkillConfig(int index)
	{
		ProfessionItem config = GetConfig();
		if (index < config.ProfessionSkills.Length)
		{
			return ProfessionSkill.Instance[config.ProfessionSkills[index]];
		}
		return ProfessionSkill.Instance[config.ExtraProfessionSkill];
	}

	public int GetSkillIndex(int skillId)
	{
		ProfessionItem config = GetConfig();
		int num = config.ProfessionSkills.IndexOf(skillId);
		if (num > -1)
		{
			return num;
		}
		if (skillId == config.ExtraProfessionSkill)
		{
			return config.ProfessionSkills.Length;
		}
		return -1;
	}

	[Obsolete]
	public bool IsProfessionAvailable(int currDate)
	{
		return currDate >= ProfessionOffCooldownDate;
	}

	public bool IsSkillUnlocked(int skillIndex)
	{
		return Seniority >= SharedMethods.GetSkillUnlockSeniority(SharedMethods.GetSkillId(TemplateId, skillIndex));
	}

	public int GetUnlockedSkillCount()
	{
		for (int num = 3; num >= 0; num--)
		{
			if (IsSkillUnlocked(num))
			{
				return num + 1;
			}
		}
		return 0;
	}

	public int GetSeniorityPercent()
	{
		return SeniorityToPercentage(Seniority);
	}

	public void OfflineUpdateHadBeenUnlocked(bool isInherit = false)
	{
		ProfessionItem config = GetConfig();
		int num = config.ProfessionSkills.Length;
		for (int i = 0; i < config.ProfessionSkills.Length; i++)
		{
			if (isInherit)
			{
				HadBeenUnlocked[i] = HadBeenUnlocked[i] && IsSkillUnlocked(i);
			}
			else
			{
				HadBeenUnlocked[i] = HadBeenUnlocked[i] || IsSkillUnlocked(i);
			}
		}
		if (config.ExtraProfessionSkill >= 0)
		{
			if (isInherit)
			{
				HadBeenUnlocked[num] = HadBeenUnlocked[num] && IsSkillUnlocked(num);
			}
			else
			{
				HadBeenUnlocked[num] = HadBeenUnlocked[num] || IsSkillUnlocked(num);
			}
		}
	}

	public bool IsSkillCooldown(int currDate, int skillIndex)
	{
		if (ExternalDataBridge.Context.NoProfessionSkillCooldown)
		{
			return false;
		}
		return currDate < SkillOffCooldownDates[skillIndex];
	}

	public void OfflineSkillCooldown(int skillIndex)
	{
		if (!ExternalDataBridge.Context.NoProfessionSkillCooldown)
		{
			SkillOffCooldownDates[skillIndex] = ExternalDataBridge.Context.CurrDate + GetSkillConfig(skillIndex).SkillCoolDown;
		}
	}

	public void OfflineClearSkillCooldown(int skillIndex)
	{
		if (!ExternalDataBridge.Context.NoProfessionSkillCooldown)
		{
			SkillOffCooldownDates[skillIndex] = 0;
		}
	}

	public T GetSkillsData<T>() where T : IProfessionSkillsData
	{
		return (T)SkillsData;
	}

	private static IProfessionSkillsData CreateExtraData(int templateId, sbyte type)
	{
		if (type != 0)
		{
			return null;
		}
		return templateId switch
		{
			1 => new HunterSkillsData(), 
			5 => new TaoistMonkSkillsData(), 
			6 => new BuddhistMonkSkillsData(), 
			7 => new WineTasterSkillsData(), 
			9 => new BeggarSkillsData(), 
			12 => new TravelingBuddhistMonkSkillsData(), 
			17 => new DukeSkillsData(), 
			8 => new AristocratSkillsData(), 
			14 => new TravelingTaoistMonkSkillsData(), 
			16 => new TeaTasterSkillsData(), 
			11 => new TravelerSkillsData(), 
			_ => null, 
		};
	}

	public ProfessionData()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 15;
		num = ((SkillOffCooldownDates == null) ? (num + 2) : (num + (2 + 4 * SkillOffCooldownDates.Length)));
		num = ((HadBeenUnlocked == null) ? (num + 2) : (num + (2 + HadBeenUnlocked.Length)));
		num = ((SkillsData == null) ? (num + 2) : (num + (2 + ((ISerializableGameData)SkillsData).GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 7;
		ptr += 2;
		*(int*)ptr = TemplateId;
		ptr += 4;
		*ptr = (byte)Type;
		ptr++;
		*(int*)ptr = Seniority;
		ptr += 4;
		if (SkillOffCooldownDates != null)
		{
			int num = SkillOffCooldownDates.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				((int*)ptr)[i] = SkillOffCooldownDates[i];
			}
			ptr += 4 * num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (HadBeenUnlocked != null)
		{
			int num2 = HadBeenUnlocked.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int j = 0; j < num2; j++)
			{
				ptr[j] = (HadBeenUnlocked[j] ? ((byte)1) : ((byte)0));
			}
			ptr += num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SkillsData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num3 = ((ISerializableGameData)SkillsData).Serialize(ptr);
			ptr += num3;
			Tester.Assert(num3 <= 65535);
			*(ushort*)intPtr = (ushort)num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = ExtraSeniority;
		ptr += 4;
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
			TemplateId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			Type = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 2)
		{
			Seniority = *(int*)ptr;
			ptr += 4;
		}
		if (num > 3)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (SkillOffCooldownDates == null || SkillOffCooldownDates.Length != num2)
				{
					SkillOffCooldownDates = new int[num2];
				}
				for (int i = 0; i < num2; i++)
				{
					SkillOffCooldownDates[i] = ((int*)ptr)[i];
				}
				ptr += 4 * num2;
			}
			else
			{
				SkillOffCooldownDates = null;
			}
		}
		if (num > 4)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (HadBeenUnlocked == null || HadBeenUnlocked.Length != num3)
				{
					HadBeenUnlocked = new bool[num3];
				}
				for (int j = 0; j < num3; j++)
				{
					HadBeenUnlocked[j] = ptr[j] != 0;
				}
				ptr += (int)num3;
			}
			else
			{
				HadBeenUnlocked = null;
			}
		}
		if (num > 5)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (SkillsData == null)
				{
					SkillsData = CreateExtraData(TemplateId, Type);
				}
				ptr += ((ISerializableGameData)SkillsData).Deserialize(ptr);
			}
			else
			{
				SkillsData = CreateExtraData(TemplateId, Type);
			}
		}
		if (num > 6)
		{
			ExtraSeniority = *(int*)ptr;
			ptr += 4;
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}

	public int GetSeniorityOrgGrade()
	{
		return SeniorityToOrgGrade(Seniority);
	}

	public int GetSeniorityMainAttributeAdditional()
	{
		return SeniorityToMainAttributeAdditional(Seniority);
	}

	public int GetSeniorityVisionRangeBonus()
	{
		return SeniorityToVisionRangeBonus(Seniority);
	}

	public int SeniorityToTeleportDistance()
	{
		return SeniorityToTeleportDistance(Seniority);
	}

	public int GetSeniorityResourceRecoveryFactor()
	{
		return SeniorityToResourceRecoveryFactor(Seniority);
	}

	public int GetSeniorityAttainmentBonus()
	{
		return SeniorityToAttainmentBonus(Seniority);
	}

	public int GetSeniorityEmptyToolAttainmentBonus()
	{
		return SeniorityToEmptyToolAttainmentBonus(Seniority);
	}

	public int GetSeniorityAttainmentReqFactor()
	{
		return SeniorityToAttainmentReqFactor(Seniority);
	}

	public int GetSeniorityTreatmentCharge()
	{
		return SeniorityToTreatmentCharge(Seniority);
	}

	public int GetSeniorityTradeCostFactor()
	{
		return SeniorityToTradeCostFactor(Seniority);
	}

	public sbyte GetSeniorityCaravanGrade()
	{
		return SeniorityToCaravanGrade(Seniority);
	}

	public (int sell, int buy) SeniorityToCaravanPrice()
	{
		return SeniorityToCaravanPrice(Seniority);
	}

	public CValuePercentBonus GetSeniorityToWineTasterSolarTermBonus(int wineCount)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return SeniorityToWineTasterSolarTermBonus(Seniority, wineCount);
	}

	public int GetInfluencePowerBonusFactor()
	{
		return SeniorityToInfluencePowerBonusFactor(Seniority);
	}

	public sbyte GetSeniorityGrowingGrade(IRandomSource random)
	{
		return SeniorityToGrowingGrade(Seniority, random);
	}

	public sbyte GetSeniorityGrowingGrade()
	{
		return SeniorityToGrowingGrade(Seniority);
	}

	public sbyte GetSeniorityFeatureUpgradeCount(IRandomSource random)
	{
		return SeniorityToFeatureUpgradeCount(Seniority, random);
	}

	public int GetSeniorityAuthorityGain()
	{
		return SeniorityToAuthorityGain(Seniority);
	}

	public int GetSeniorityCultureGain()
	{
		return SeniorityToCultureGain(Seniority);
	}

	public int GetSenioritySafetyGain()
	{
		return SeniorityToSafetyGain(Seniority);
	}

	public sbyte GetSeniorityGiftLevelReduce()
	{
		return SeniorityToGiftLevelReduce(Seniority);
	}

	public sbyte GetSeniorityFavorAddPercent()
	{
		return GetSeniorityFavorAddPercent(Seniority);
	}

	public sbyte GetSeniorityAnimalCount()
	{
		return SeniorityToAnimalCount(Seniority);
	}

	public int GetSeniorityHunterAnimalBonus()
	{
		return SeniorityHunterAnimalBonus(Seniority);
	}

	public sbyte GetSeniorityCallAnimalGrade()
	{
		return SeniorityCallAnimalGrade(Seniority);
	}

	public int GetSeniorityBeggingMoneyBaseValue()
	{
		return SeniorityToBeggingMoneyBaseValue(Seniority);
	}

	public sbyte GetSeniorityDoctorMaxSettlementType()
	{
		return SeniorityToDoctorMaxSettlementType(Seniority);
	}

	public short GetSeniorityDoctorMedicinePricePercent()
	{
		return GetSeniorityDoctorMedicinePricePercent(Seniority);
	}

	public short GetSeniorityDoctorFavorAddPercent()
	{
		return GetSeniorityDoctorFavorAddPercent(Seniority);
	}

	public int GetTaoistMonkSkill3AuthorityPara()
	{
		return GetTaoistMonkSkill3AuthorityPara(Seniority);
	}

	public sbyte GetSeniorityBeggarMaxSettlementType()
	{
		return SeniorityToBeggarMaxSettlementType(Seniority);
	}

	public int GetFameChange()
	{
		return FameAction.Instance[(short)56].Fame;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetMainAttributesRecoveryBonusAppliedRate(sbyte mainAttributeType, int baseRecovery)
	{
		baseRecovery += baseRecovery * (100 + GetSeniorityPercent()) / 100;
		return baseRecovery;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToPercentage(int seniority)
	{
		return seniority * 100 / 3000000;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static sbyte SeniorityToOrgGrade(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		if (num >= 90)
		{
			return 8;
		}
		if (num >= 70)
		{
			return 7;
		}
		if (num >= 50)
		{
			return 6;
		}
		if (num >= 30)
		{
			return 4;
		}
		return 2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToMainAttributeAdditional(int seniority)
	{
		return 10 + 20 * SeniorityToPercentage(seniority) / 100;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToVisionRangeBonus(int seniority)
	{
		return 10 * SeniorityToPercentage(seniority) / 100;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToTeleportDistance(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return 10 + 10 * (num / 100);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToResourceRecoveryFactor(int seniority)
	{
		return 33 + 33 * seniority / 3000000;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToEmptyToolAttainmentBonus(int seniority)
	{
		return 50 * seniority / 3000000 - 50;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToAttainmentBonus(int seniority)
	{
		return 33 + 33 * seniority / 3000000;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToAttainmentReqFactor(int seniority)
	{
		return MathUtils.Clamp(300 - 200 * seniority / 3000000, 100, 300);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToTreatmentCharge(int seniority)
	{
		return 100 + 2900 * seniority / 3000000;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToTradeCostFactor(int seniority)
	{
		return 500;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static sbyte SeniorityToCaravanGrade(int seniority)
	{
		return (sbyte)(SeniorityToPercentage(seniority) / 15);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (int sell, int buy) SeniorityToCaravanPrice(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return (sell: 25 * num / 100, buy: -25 * num / 100);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static CValuePercentBonus SeniorityToWineTasterSolarTermBonus(int seniority, int wineCount)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return CValuePercentBonus.op_Implicit(wineCount * 20 * SeniorityToPercentage(seniority) / 100);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToInfluencePowerBonusFactor(int seniority)
	{
		return 50 + SeniorityToPercentage(seniority) / 2;
	}

	public static sbyte SeniorityToGrowingGrade(int seniority, IRandomSource random)
	{
		return (sbyte)(random.Next((int)ProfessionRelatedConstants.AristocratGradeRange[0], ProfessionRelatedConstants.AristocratGradeRange[1] + 1) + SeniorityToGrowingGrade(seniority));
	}

	public static sbyte SeniorityToGrowingGrade(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return (sbyte)(3 * num / 100);
	}

	public static sbyte SeniorityToFeatureUpgradeCount(int seniority, IRandomSource random)
	{
		int num = SeniorityToPercentage(seniority);
		float num2 = Math.Max(0, 100 - 3 * num / 4);
		float num3 = num / 2;
		float num4 = random.NextFloat() * 100f;
		if (num4 < num2)
		{
			return 1;
		}
		if (num4 < num2 + num3)
		{
			return 2;
		}
		return 3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToAuthorityGain(int seniority)
	{
		return seniority / 5;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToCultureGain(int seniority)
	{
		return seniority / 400;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToSafetyGain(int seniority)
	{
		return seniority / 400;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static sbyte SeniorityToGiftLevelReduce(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return (sbyte)(1 + 4 * num / 100);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static sbyte GetSeniorityFavorAddPercent(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return (sbyte)(33 + 33 * num / 100);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetSeniorityCivilianAddHatredLimit(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return 12 - 8 * num / 100;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetSeniorityCivilianSeverHatredLimit(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return 3 + 3 * num / 100;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static sbyte SeniorityToAnimalCount(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		for (sbyte b = 0; b < GlobalConfig.Instance.HunterSkill2_SeniorityPercentToAnimalCount.Length; b++)
		{
			if (num < GlobalConfig.Instance.HunterSkill2_SeniorityPercentToAnimalCount[b])
			{
				return (sbyte)(b + 1);
			}
		}
		return (sbyte)GlobalConfig.Instance.HunterSkill2_SeniorityPercentToAnimalCount.Length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityHunterAnimalBonus(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return 33 + 33 * num / 100;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static sbyte SeniorityCallAnimalGrade(int seniority)
	{
		if (SeniorityToPercentage(seniority) >= 90)
		{
			return 7;
		}
		if (SeniorityToPercentage(seniority) >= 75)
		{
			return 6;
		}
		if (SeniorityToPercentage(seniority) >= 60)
		{
			return 5;
		}
		if (SeniorityToPercentage(seniority) >= 45)
		{
			return 4;
		}
		return 3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToBeggingMoneyBaseValue(int seniority)
	{
		return 10 + SeniorityToPercentage(seniority);
	}

	public static sbyte SeniorityToDoctorMaxSettlementType(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		if (num >= 60)
		{
			return 3;
		}
		if (num >= 50)
		{
			return 2;
		}
		if (num >= 40)
		{
			return 1;
		}
		if (num >= 30)
		{
			return 0;
		}
		return -1;
	}

	public static short GetSeniorityDoctorMedicinePricePercent(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return (short)(150 + 150 * num / 100);
	}

	public static short GetSeniorityDoctorFavorAddPercent(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return (short)(150 + 150 * num / 100);
	}

	public static short GetTaoistMonkSkill3AuthorityPara(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		return (short)(30 + num * 60 / 100);
	}

	public static sbyte SeniorityToBeggarMaxSettlementType(int seniority)
	{
		int num = SeniorityToPercentage(seniority);
		if (num >= 30)
		{
			return 3;
		}
		if (num >= 20)
		{
			return 2;
		}
		if (num >= 10)
		{
			return 1;
		}
		return 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SeniorityToExtraReadingLoopingStrategyCount(int seniority)
	{
		return 1 + 2 * seniority / 3000000;
	}
}
