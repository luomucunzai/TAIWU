using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace GameData.Domains.Organization
{
	// Token: 0x0200063F RID: 1599
	[SerializableGameData(NotForDisplayModule = true)]
	public class CivilianSettlement : Settlement, ISerializableGameData
	{
		// Token: 0x0600465A RID: 18010 RVA: 0x0027456F File Offset: 0x0027276F
		public CivilianSettlement(short id, Location location, sbyte orgTemplateId, SettlementCreatingInfo settlementCreatingInfo, IRandomSource random) : base(id, location, orgTemplateId, random)
		{
			this._randomNameId = settlementCreatingInfo.GenerateRandomName(orgTemplateId);
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x00274598 File Offset: 0x00272798
		public short UpdateMainMorality(DataContext context)
		{
			GameData.Domains.Character.Character leader = base.GetLeader();
			bool flag = leader == null;
			short result;
			if (flag)
			{
				result = this._mainMorality;
			}
			else
			{
				short morality = leader.GetMorality();
				this.SetMainMorality(morality, context);
				result = morality;
			}
			return result;
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x002745D4 File Offset: 0x002727D4
		protected override void RecruitOrCreateLackingMembers(DataContext context)
		{
			bool flag = this.Location.AreaId >= 45;
			if (!flag)
			{
				List<short> blockIds = ObjectPool<List<short>>.Instance.Get();
				blockIds.Clear();
				DomainManager.Map.GetSettlementBlocks(this.Location.AreaId, this.Location.BlockId, blockIds);
				List<short> nearbyBlockIds = ObjectPool<List<short>>.Instance.Get();
				nearbyBlockIds.Clear();
				DomainManager.Map.GetSettlementBlocksAndAffiliatedBlocks(this.Location.AreaId, this.Location.BlockId, nearbyBlockIds);
				sbyte mapStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(this.Location.AreaId);
				OrganizationItem organizationCfg = Organization.Instance[this.OrgTemplateId];
				for (sbyte grade = 8; grade >= 0; grade -= 1)
				{
					OrganizationMemberItem orgMemberCfg = OrganizationMember.Instance[organizationCfg.Members[(int)grade]];
					int principalAmount = base.GetPrincipalAmount(grade);
					int expectedAmount = base.GetExpectedCoreMemberAmount(orgMemberCfg);
					int recruitCount = expectedAmount - principalAmount;
					for (int i = 0; i < recruitCount; i++)
					{
						SettlementMembersCreationInfo info = new SettlementMembersCreationInfo(this.OrgTemplateId, this.Id, mapStateTemplateId, this.Location.AreaId, blockIds, nearbyBlockIds);
						info.CoreMemberConfig = orgMemberCfg;
						OrganizationDomain.CreateCoreCharacter(context, info);
						info.CompleteCreatingCharacters();
					}
					bool flag2 = recruitCount > 0;
					if (flag2)
					{
						string tag = "RecruitOrCreateLackingMembers";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Recruited Count for ");
						defaultInterpolatedStringHandler.AppendFormatted(organizationCfg.Name);
						defaultInterpolatedStringHandler.AppendLiteral(", grade ");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(grade);
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(recruitCount);
						AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				ObjectPool<List<short>>.Instance.Return(blockIds);
				ObjectPool<List<short>>.Instance.Return(nearbyBlockIds);
			}
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x002747BC File Offset: 0x002729BC
		protected override void OfflineUpdateTreasuryGuards(DataContext context, SettlementLayeredTreasuries treasuries)
		{
			foreach (SettlementTreasury treasury in treasuries.SettlementTreasuries.Reverse<SettlementTreasury>())
			{
				SettlementTreasury settlementTreasury = treasury;
				if (settlementTreasury.TemplateGuardIds == null)
				{
					settlementTreasury.TemplateGuardIds = new List<short>();
				}
				treasury.TemplateGuardIds.Clear();
				bool isSect = Organization.Instance[this.OrgTemplateId].IsSect;
				int count = GlobalConfig.Instance.TreasuryGuardCount;
				sbyte grade = GlobalConfig.Instance.TreasuryGuardMaxGrade[(int)treasury.LayerIndex];
				for (int i = 0; i < count; i++)
				{
					short templateId = isSect ? GameData.Domains.Character.Character.GetSectRandomEnemyTemplateIdByGrade(this.OrgTemplateId, grade) : CivilianSettlement.GetTreasuryGuardTemplateId(grade);
					treasury.TemplateGuardIds.Add(templateId);
				}
			}
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x002748B0 File Offset: 0x00272AB0
		public static short GetTreasuryGuardTemplateId(sbyte guardGrade)
		{
			return (short)(307 + (int)guardGrade);
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x002748CC File Offset: 0x00272ACC
		public override SettlementNameRelatedData GetNameRelatedData()
		{
			MapBlockData block = DomainManager.Map.GetBlock(this.Location).GetRootBlock();
			return new SettlementNameRelatedData(this._randomNameId, block.TemplateId);
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x00274908 File Offset: 0x00272B08
		public unsafe override void SetCulture(short culture, DataContext context)
		{
			this.Culture = culture;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 7U, 2);
				*(short*)pData = this.Culture;
				pData += 2;
			}
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x00274968 File Offset: 0x00272B68
		public unsafe override void SetMaxCulture(short maxCulture, DataContext context)
		{
			this.MaxCulture = maxCulture;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 9U, 2);
				*(short*)pData = this.MaxCulture;
				pData += 2;
			}
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x002749C8 File Offset: 0x00272BC8
		public unsafe override void SetSafety(short safety, DataContext context)
		{
			this.Safety = safety;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 11U, 2);
				*(short*)pData = this.Safety;
				pData += 2;
			}
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x00274A28 File Offset: 0x00272C28
		public unsafe override void SetMaxSafety(short maxSafety, DataContext context)
		{
			this.MaxSafety = maxSafety;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 13U, 2);
				*(short*)pData = this.MaxSafety;
				pData += 2;
			}
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x00274A88 File Offset: 0x00272C88
		public unsafe override void SetPopulation(int population, DataContext context)
		{
			this.Population = population;
			base.SetModifiedAndInvalidateInfluencedCache(7, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 15U, 4);
				*(int*)pData = this.Population;
				pData += 4;
			}
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x00274AE8 File Offset: 0x00272CE8
		public unsafe override void SetMaxPopulation(int maxPopulation, DataContext context)
		{
			this.MaxPopulation = maxPopulation;
			base.SetModifiedAndInvalidateInfluencedCache(8, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 19U, 4);
				*(int*)pData = this.MaxPopulation;
				pData += 4;
			}
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x00274B48 File Offset: 0x00272D48
		public unsafe override void SetStandardOnStagePopulation(int standardOnStagePopulation, DataContext context)
		{
			this.StandardOnStagePopulation = standardOnStagePopulation;
			base.SetModifiedAndInvalidateInfluencedCache(9, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 23U, 4);
				*(int*)pData = this.StandardOnStagePopulation;
				pData += 4;
			}
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x00274BAC File Offset: 0x00272DAC
		public unsafe override void SetMembers(OrgMemberCollection members, DataContext context)
		{
			this.Members = members;
			base.SetModifiedAndInvalidateInfluencedCache(10, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this.Members.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 0, dataSize);
				pData += this.Members.Serialize(pData);
			}
		}

		// Token: 0x06004668 RID: 18024 RVA: 0x00274C1C File Offset: 0x00272E1C
		public unsafe override void SetLackingCoreMembers(OrgMemberCollection lackingCoreMembers, DataContext context)
		{
			this.LackingCoreMembers = lackingCoreMembers;
			base.SetModifiedAndInvalidateInfluencedCache(11, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this.LackingCoreMembers.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 1, dataSize);
				pData += this.LackingCoreMembers.Serialize(pData);
			}
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x00274C8C File Offset: 0x00272E8C
		public unsafe override void SetApprovingRateUpperLimitBonus(short approvingRateUpperLimitBonus, DataContext context)
		{
			this.ApprovingRateUpperLimitBonus = approvingRateUpperLimitBonus;
			base.SetModifiedAndInvalidateInfluencedCache(12, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 27U, 2);
				*(short*)pData = this.ApprovingRateUpperLimitBonus;
				pData += 2;
			}
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x00274CF0 File Offset: 0x00272EF0
		public unsafe override void SetInfluencePowerUpdateDate(int influencePowerUpdateDate, DataContext context)
		{
			this.InfluencePowerUpdateDate = influencePowerUpdateDate;
			base.SetModifiedAndInvalidateInfluencedCache(13, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 29U, 4);
				*(int*)pData = this.InfluencePowerUpdateDate;
				pData += 4;
			}
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x00274D54 File Offset: 0x00272F54
		public short GetRandomNameId()
		{
			return this._randomNameId;
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x00274D6C File Offset: 0x00272F6C
		public short GetMainMorality()
		{
			return this._mainMorality;
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x00274D84 File Offset: 0x00272F84
		public unsafe void SetMainMorality(short mainMorality, DataContext context)
		{
			this._mainMorality = mainMorality;
			base.SetModifiedAndInvalidateInfluencedCache(15, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 35U, 2);
				*(short*)pData = this._mainMorality;
				pData += 2;
			}
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x00274DE8 File Offset: 0x00272FE8
		public override short GetApprovingRateUpperLimitTempBonus()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 16);
			short approvingRateUpperLimitTempBonus;
			if (flag)
			{
				approvingRateUpperLimitTempBonus = this.ApprovingRateUpperLimitTempBonus;
			}
			else
			{
				short value = base.CalcApprovingRateUpperLimitTempBonus();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this.ApprovingRateUpperLimitTempBonus = value;
					dataStates.SetCached(this.DataStatesOffset, 16);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				approvingRateUpperLimitTempBonus = this.ApprovingRateUpperLimitTempBonus;
			}
			return approvingRateUpperLimitTempBonus;
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x00274E90 File Offset: 0x00273090
		public CivilianSettlement()
		{
			this.Members = new OrgMemberCollection();
			this.LackingCoreMembers = new OrgMemberCollection();
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x00274EBC File Offset: 0x002730BC
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x00274ED0 File Offset: 0x002730D0
		public int GetSerializedSize()
		{
			int totalSize = 45;
			int dataSize = this.Members.GetSerializedSize();
			totalSize += dataSize;
			int dataSize2 = this.LackingCoreMembers.GetSerializedSize();
			return totalSize + dataSize2;
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x00274F0C File Offset: 0x0027310C
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = this.Id;
			byte* pCurrData = pData + 2;
			*pCurrData = (byte)this.OrgTemplateId;
			pCurrData++;
			pCurrData += this.Location.Serialize(pCurrData);
			*(short*)pCurrData = this.Culture;
			pCurrData += 2;
			*(short*)pCurrData = this.MaxCulture;
			pCurrData += 2;
			*(short*)pCurrData = this.Safety;
			pCurrData += 2;
			*(short*)pCurrData = this.MaxSafety;
			pCurrData += 2;
			*(int*)pCurrData = this.Population;
			pCurrData += 4;
			*(int*)pCurrData = this.MaxPopulation;
			pCurrData += 4;
			*(int*)pCurrData = this.StandardOnStagePopulation;
			pCurrData += 4;
			*(short*)pCurrData = this.ApprovingRateUpperLimitBonus;
			pCurrData += 2;
			*(int*)pCurrData = this.InfluencePowerUpdateDate;
			pCurrData += 4;
			*(short*)pCurrData = this._randomNameId;
			pCurrData += 2;
			*(short*)pCurrData = this._mainMorality;
			pCurrData += 2;
			byte* pBegin = pCurrData;
			pCurrData += 4;
			pCurrData += this.Members.Serialize(pCurrData);
			int fieldSize = (int)((long)(pCurrData - pBegin) - 4L);
			bool flag = fieldSize > 4194304;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("Members");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin = fieldSize;
			byte* pBegin2 = pCurrData;
			pCurrData += 4;
			pCurrData += this.LackingCoreMembers.Serialize(pCurrData);
			int fieldSize2 = (int)((long)(pCurrData - pBegin2) - 4L);
			bool flag2 = fieldSize2 > 4194304;
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("LackingCoreMembers");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin2 = fieldSize2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x002750F0 File Offset: 0x002732F0
		public unsafe int Deserialize(byte* pData)
		{
			this.Id = *(short*)pData;
			byte* pCurrData = pData + 2;
			this.OrgTemplateId = *(sbyte*)pCurrData;
			pCurrData++;
			pCurrData += this.Location.Deserialize(pCurrData);
			this.Culture = *(short*)pCurrData;
			pCurrData += 2;
			this.MaxCulture = *(short*)pCurrData;
			pCurrData += 2;
			this.Safety = *(short*)pCurrData;
			pCurrData += 2;
			this.MaxSafety = *(short*)pCurrData;
			pCurrData += 2;
			this.Population = *(int*)pCurrData;
			pCurrData += 4;
			this.MaxPopulation = *(int*)pCurrData;
			pCurrData += 4;
			this.StandardOnStagePopulation = *(int*)pCurrData;
			pCurrData += 4;
			this.ApprovingRateUpperLimitBonus = *(short*)pCurrData;
			pCurrData += 2;
			this.InfluencePowerUpdateDate = *(int*)pCurrData;
			pCurrData += 4;
			this._randomNameId = *(short*)pCurrData;
			pCurrData += 2;
			this._mainMorality = *(short*)pCurrData;
			pCurrData += 2;
			pCurrData += 4;
			pCurrData += this.Members.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this.LackingCoreMembers.Deserialize(pCurrData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x0400149D RID: 5277
		[CollectionObjectField(false, true, false, true, false)]
		private short _randomNameId;

		// Token: 0x0400149E RID: 5278
		[CollectionObjectField(false, true, false, false, false)]
		private short _mainMorality;

		// Token: 0x0400149F RID: 5279
		public const int FixedSize = 37;

		// Token: 0x040014A0 RID: 5280
		public const int DynamicCount = 2;

		// Token: 0x040014A1 RID: 5281
		private SpinLock _spinLock = new SpinLock(false);

		// Token: 0x02000A7B RID: 2683
		internal class FixedFieldInfos
		{
			// Token: 0x04002AD1 RID: 10961
			public const uint Id_Offset = 0U;

			// Token: 0x04002AD2 RID: 10962
			public const int Id_Size = 2;

			// Token: 0x04002AD3 RID: 10963
			public const uint OrgTemplateId_Offset = 2U;

			// Token: 0x04002AD4 RID: 10964
			public const int OrgTemplateId_Size = 1;

			// Token: 0x04002AD5 RID: 10965
			public const uint Location_Offset = 3U;

			// Token: 0x04002AD6 RID: 10966
			public const int Location_Size = 4;

			// Token: 0x04002AD7 RID: 10967
			public const uint Culture_Offset = 7U;

			// Token: 0x04002AD8 RID: 10968
			public const int Culture_Size = 2;

			// Token: 0x04002AD9 RID: 10969
			public const uint MaxCulture_Offset = 9U;

			// Token: 0x04002ADA RID: 10970
			public const int MaxCulture_Size = 2;

			// Token: 0x04002ADB RID: 10971
			public const uint Safety_Offset = 11U;

			// Token: 0x04002ADC RID: 10972
			public const int Safety_Size = 2;

			// Token: 0x04002ADD RID: 10973
			public const uint MaxSafety_Offset = 13U;

			// Token: 0x04002ADE RID: 10974
			public const int MaxSafety_Size = 2;

			// Token: 0x04002ADF RID: 10975
			public const uint Population_Offset = 15U;

			// Token: 0x04002AE0 RID: 10976
			public const int Population_Size = 4;

			// Token: 0x04002AE1 RID: 10977
			public const uint MaxPopulation_Offset = 19U;

			// Token: 0x04002AE2 RID: 10978
			public const int MaxPopulation_Size = 4;

			// Token: 0x04002AE3 RID: 10979
			public const uint StandardOnStagePopulation_Offset = 23U;

			// Token: 0x04002AE4 RID: 10980
			public const int StandardOnStagePopulation_Size = 4;

			// Token: 0x04002AE5 RID: 10981
			public const uint ApprovingRateUpperLimitBonus_Offset = 27U;

			// Token: 0x04002AE6 RID: 10982
			public const int ApprovingRateUpperLimitBonus_Size = 2;

			// Token: 0x04002AE7 RID: 10983
			public const uint InfluencePowerUpdateDate_Offset = 29U;

			// Token: 0x04002AE8 RID: 10984
			public const int InfluencePowerUpdateDate_Size = 4;

			// Token: 0x04002AE9 RID: 10985
			public const uint RandomNameId_Offset = 33U;

			// Token: 0x04002AEA RID: 10986
			public const int RandomNameId_Size = 2;

			// Token: 0x04002AEB RID: 10987
			public const uint MainMorality_Offset = 35U;

			// Token: 0x04002AEC RID: 10988
			public const int MainMorality_Size = 2;
		}
	}
}
