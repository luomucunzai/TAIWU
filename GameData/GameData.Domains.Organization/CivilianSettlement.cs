using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Organization;

[SerializableGameData(NotForDisplayModule = true)]
public class CivilianSettlement : Settlement, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 2;

		public const uint OrgTemplateId_Offset = 2u;

		public const int OrgTemplateId_Size = 1;

		public const uint Location_Offset = 3u;

		public const int Location_Size = 4;

		public const uint Culture_Offset = 7u;

		public const int Culture_Size = 2;

		public const uint MaxCulture_Offset = 9u;

		public const int MaxCulture_Size = 2;

		public const uint Safety_Offset = 11u;

		public const int Safety_Size = 2;

		public const uint MaxSafety_Offset = 13u;

		public const int MaxSafety_Size = 2;

		public const uint Population_Offset = 15u;

		public const int Population_Size = 4;

		public const uint MaxPopulation_Offset = 19u;

		public const int MaxPopulation_Size = 4;

		public const uint StandardOnStagePopulation_Offset = 23u;

		public const int StandardOnStagePopulation_Size = 4;

		public const uint ApprovingRateUpperLimitBonus_Offset = 27u;

		public const int ApprovingRateUpperLimitBonus_Size = 2;

		public const uint InfluencePowerUpdateDate_Offset = 29u;

		public const int InfluencePowerUpdateDate_Size = 4;

		public const uint RandomNameId_Offset = 33u;

		public const int RandomNameId_Size = 2;

		public const uint MainMorality_Offset = 35u;

		public const int MainMorality_Size = 2;
	}

	[CollectionObjectField(false, true, false, true, false)]
	private short _randomNameId;

	[CollectionObjectField(false, true, false, false, false)]
	private short _mainMorality;

	public const int FixedSize = 37;

	public const int DynamicCount = 2;

	private SpinLock _spinLock = new SpinLock(enableThreadOwnerTracking: false);

	public CivilianSettlement(short id, Location location, sbyte orgTemplateId, SettlementCreatingInfo settlementCreatingInfo, IRandomSource random)
		: base(id, location, orgTemplateId, random)
	{
		_randomNameId = settlementCreatingInfo.GenerateRandomName(orgTemplateId);
	}

	public short UpdateMainMorality(DataContext context)
	{
		GameData.Domains.Character.Character leader = GetLeader();
		if (leader == null)
		{
			return _mainMorality;
		}
		short morality = leader.GetMorality();
		SetMainMorality(morality, context);
		return morality;
	}

	protected override void RecruitOrCreateLackingMembers(DataContext context)
	{
		if (Location.AreaId >= 45)
		{
			return;
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		DomainManager.Map.GetSettlementBlocks(Location.AreaId, Location.BlockId, list);
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		list2.Clear();
		DomainManager.Map.GetSettlementBlocksAndAffiliatedBlocks(Location.AreaId, Location.BlockId, list2);
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(Location.AreaId);
		OrganizationItem organizationItem = Config.Organization.Instance[OrgTemplateId];
		for (sbyte b = 8; b >= 0; b--)
		{
			OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[organizationItem.Members[b]];
			int principalAmount = GetPrincipalAmount(b);
			int expectedCoreMemberAmount = GetExpectedCoreMemberAmount(organizationMemberItem);
			int num = expectedCoreMemberAmount - principalAmount;
			for (int i = 0; i < num; i++)
			{
				SettlementMembersCreationInfo settlementMembersCreationInfo = new SettlementMembersCreationInfo(OrgTemplateId, Id, stateTemplateIdByAreaId, Location.AreaId, list, list2);
				settlementMembersCreationInfo.CoreMemberConfig = organizationMemberItem;
				OrganizationDomain.CreateCoreCharacter(context, settlementMembersCreationInfo);
				settlementMembersCreationInfo.CompleteCreatingCharacters();
			}
			if (num > 0)
			{
				AdaptableLog.TagInfo("RecruitOrCreateLackingMembers", $"Recruited Count for {organizationItem.Name}, grade {b}: {num}");
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
	}

	protected override void OfflineUpdateTreasuryGuards(DataContext context, SettlementLayeredTreasuries treasuries)
	{
		foreach (SettlementTreasury item2 in Enumerable.Reverse(treasuries.SettlementTreasuries))
		{
			SettlementTreasury settlementTreasury = item2;
			if (settlementTreasury.TemplateGuardIds == null)
			{
				settlementTreasury.TemplateGuardIds = new List<short>();
			}
			item2.TemplateGuardIds.Clear();
			bool isSect = Config.Organization.Instance[OrgTemplateId].IsSect;
			int treasuryGuardCount = GlobalConfig.Instance.TreasuryGuardCount;
			sbyte b = GlobalConfig.Instance.TreasuryGuardMaxGrade[item2.LayerIndex];
			for (int i = 0; i < treasuryGuardCount; i++)
			{
				short item = (isSect ? GameData.Domains.Character.Character.GetSectRandomEnemyTemplateIdByGrade(OrgTemplateId, b) : GetTreasuryGuardTemplateId(b));
				item2.TemplateGuardIds.Add(item);
			}
		}
	}

	public static short GetTreasuryGuardTemplateId(sbyte guardGrade)
	{
		return (short)(307 + guardGrade);
	}

	public override SettlementNameRelatedData GetNameRelatedData()
	{
		MapBlockData rootBlock = DomainManager.Map.GetBlock(Location).GetRootBlock();
		return new SettlementNameRelatedData(_randomNameId, rootBlock.TemplateId);
	}

	public unsafe override void SetCulture(short culture, DataContext context)
	{
		Culture = culture;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 7u, 2);
			*(short*)ptr = Culture;
			ptr += 2;
		}
	}

	public unsafe override void SetMaxCulture(short maxCulture, DataContext context)
	{
		MaxCulture = maxCulture;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 9u, 2);
			*(short*)ptr = MaxCulture;
			ptr += 2;
		}
	}

	public unsafe override void SetSafety(short safety, DataContext context)
	{
		Safety = safety;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 11u, 2);
			*(short*)ptr = Safety;
			ptr += 2;
		}
	}

	public unsafe override void SetMaxSafety(short maxSafety, DataContext context)
	{
		MaxSafety = maxSafety;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 13u, 2);
			*(short*)ptr = MaxSafety;
			ptr += 2;
		}
	}

	public unsafe override void SetPopulation(int population, DataContext context)
	{
		Population = population;
		SetModifiedAndInvalidateInfluencedCache(7, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 15u, 4);
			*(int*)ptr = Population;
			ptr += 4;
		}
	}

	public unsafe override void SetMaxPopulation(int maxPopulation, DataContext context)
	{
		MaxPopulation = maxPopulation;
		SetModifiedAndInvalidateInfluencedCache(8, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 19u, 4);
			*(int*)ptr = MaxPopulation;
			ptr += 4;
		}
	}

	public unsafe override void SetStandardOnStagePopulation(int standardOnStagePopulation, DataContext context)
	{
		StandardOnStagePopulation = standardOnStagePopulation;
		SetModifiedAndInvalidateInfluencedCache(9, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 23u, 4);
			*(int*)ptr = StandardOnStagePopulation;
			ptr += 4;
		}
	}

	public unsafe override void SetMembers(OrgMemberCollection members, DataContext context)
	{
		Members = members;
		SetModifiedAndInvalidateInfluencedCache(10, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = Members.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 0, serializedSize);
			ptr += Members.Serialize(ptr);
		}
	}

	public unsafe override void SetLackingCoreMembers(OrgMemberCollection lackingCoreMembers, DataContext context)
	{
		LackingCoreMembers = lackingCoreMembers;
		SetModifiedAndInvalidateInfluencedCache(11, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = LackingCoreMembers.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 1, serializedSize);
			ptr += LackingCoreMembers.Serialize(ptr);
		}
	}

	public unsafe override void SetApprovingRateUpperLimitBonus(short approvingRateUpperLimitBonus, DataContext context)
	{
		ApprovingRateUpperLimitBonus = approvingRateUpperLimitBonus;
		SetModifiedAndInvalidateInfluencedCache(12, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 27u, 2);
			*(short*)ptr = ApprovingRateUpperLimitBonus;
			ptr += 2;
		}
	}

	public unsafe override void SetInfluencePowerUpdateDate(int influencePowerUpdateDate, DataContext context)
	{
		InfluencePowerUpdateDate = influencePowerUpdateDate;
		SetModifiedAndInvalidateInfluencedCache(13, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 29u, 4);
			*(int*)ptr = InfluencePowerUpdateDate;
			ptr += 4;
		}
	}

	public short GetRandomNameId()
	{
		return _randomNameId;
	}

	public short GetMainMorality()
	{
		return _mainMorality;
	}

	public unsafe void SetMainMorality(short mainMorality, DataContext context)
	{
		_mainMorality = mainMorality;
		SetModifiedAndInvalidateInfluencedCache(15, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 35u, 2);
			*(short*)ptr = _mainMorality;
			ptr += 2;
		}
	}

	public override short GetApprovingRateUpperLimitTempBonus()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 16))
		{
			return ApprovingRateUpperLimitTempBonus;
		}
		short approvingRateUpperLimitTempBonus = CalcApprovingRateUpperLimitTempBonus();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			ApprovingRateUpperLimitTempBonus = approvingRateUpperLimitTempBonus;
			dataStates.SetCached(DataStatesOffset, 16);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return ApprovingRateUpperLimitTempBonus;
	}

	public CivilianSettlement()
	{
		Members = new OrgMemberCollection();
		LackingCoreMembers = new OrgMemberCollection();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 45;
		int serializedSize = Members.GetSerializedSize();
		num += serializedSize;
		int serializedSize2 = LackingCoreMembers.GetSerializedSize();
		return num + serializedSize2;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = Id;
		ptr += 2;
		*ptr = (byte)OrgTemplateId;
		ptr++;
		ptr += Location.Serialize(ptr);
		*(short*)ptr = Culture;
		ptr += 2;
		*(short*)ptr = MaxCulture;
		ptr += 2;
		*(short*)ptr = Safety;
		ptr += 2;
		*(short*)ptr = MaxSafety;
		ptr += 2;
		*(int*)ptr = Population;
		ptr += 4;
		*(int*)ptr = MaxPopulation;
		ptr += 4;
		*(int*)ptr = StandardOnStagePopulation;
		ptr += 4;
		*(short*)ptr = ApprovingRateUpperLimitBonus;
		ptr += 2;
		*(int*)ptr = InfluencePowerUpdateDate;
		ptr += 4;
		*(short*)ptr = _randomNameId;
		ptr += 2;
		*(short*)ptr = _mainMorality;
		ptr += 2;
		byte* ptr2 = ptr;
		ptr += 4;
		ptr += Members.Serialize(ptr);
		int num = (int)(ptr - ptr2 - 4);
		if (num > 4194304)
		{
			throw new Exception($"Size of field {"Members"} must be less than {4096}KB");
		}
		*(int*)ptr2 = num;
		byte* ptr3 = ptr;
		ptr += 4;
		ptr += LackingCoreMembers.Serialize(ptr);
		int num2 = (int)(ptr - ptr3 - 4);
		if (num2 > 4194304)
		{
			throw new Exception($"Size of field {"LackingCoreMembers"} must be less than {4096}KB");
		}
		*(int*)ptr3 = num2;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(short*)ptr;
		ptr += 2;
		OrgTemplateId = (sbyte)(*ptr);
		ptr++;
		ptr += Location.Deserialize(ptr);
		Culture = *(short*)ptr;
		ptr += 2;
		MaxCulture = *(short*)ptr;
		ptr += 2;
		Safety = *(short*)ptr;
		ptr += 2;
		MaxSafety = *(short*)ptr;
		ptr += 2;
		Population = *(int*)ptr;
		ptr += 4;
		MaxPopulation = *(int*)ptr;
		ptr += 4;
		StandardOnStagePopulation = *(int*)ptr;
		ptr += 4;
		ApprovingRateUpperLimitBonus = *(short*)ptr;
		ptr += 2;
		InfluencePowerUpdateDate = *(int*)ptr;
		ptr += 4;
		_randomNameId = *(short*)ptr;
		ptr += 2;
		_mainMorality = *(short*)ptr;
		ptr += 2;
		ptr += 4;
		ptr += Members.Deserialize(ptr);
		ptr += 4;
		ptr += LackingCoreMembers.Deserialize(ptr);
		return (int)(ptr - pData);
	}
}
