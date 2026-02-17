using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x0200004C RID: 76
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public abstract class VillagerRoleBase : ISerializableGameData
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600137E RID: 4990
		public abstract short RoleTemplateId { get; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x00138C1A File Offset: 0x00136E1A
		public short OrgMemberTemplateId
		{
			get
			{
				return this.RoleConfig.OrganizationMember;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x00138C27 File Offset: 0x00136E27
		public bool HasChickenUpgradeEffect
		{
			get
			{
				return DomainManager.Building.HasFulongChicken(this.RoleConfig);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x00138C39 File Offset: 0x00136E39
		public VillagerRoleItem RoleConfig
		{
			get
			{
				return VillagerRole.Instance[this.RoleTemplateId];
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x00138C4B File Offset: 0x00136E4B
		public bool IsInTaiwuVillageInfluenceRange
		{
			get
			{
				return DomainManager.Map.IsLocationInSettlementInfluenceRange(this.Character.GetLocation(), DomainManager.Taiwu.GetTaiwuVillageSettlementId());
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06001383 RID: 4995 RVA: 0x00138C6C File Offset: 0x00136E6C
		public int Personality
		{
			get
			{
				return (int)this.Character.GetPersonality(this.RoleConfig.PersonalityType);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06001384 RID: 4996 RVA: 0x00138C84 File Offset: 0x00136E84
		public BoolArray64 AutoActionStates
		{
			get
			{
				return DomainManager.Extra.GetElement_VillagerRoleAutoActionStates(this.RoleTemplateId);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x00138C9B File Offset: 0x00136E9B
		public SettlementTreasury SettlementTreasury
		{
			get
			{
				return DomainManager.Taiwu.GetTaiwuTreasury();
			}
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00138CA7 File Offset: 0x00136EA7
		public virtual void OfflineSetArrangement(short arrangementTemplateId, Location location)
		{
			this.ArrangementTemplateId = (int)arrangementTemplateId;
			this.WorkData = new VillagerWorkData(this.Character.GetId(), 2, location.AreaId, location.BlockId);
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00138CD4 File Offset: 0x00136ED4
		public virtual void Initialize(int charId)
		{
			DomainManager.Character.TryGetElement_Objects(charId, out this.Character);
			DomainManager.Taiwu.TryGetElement_VillagerWork(charId, out this.WorkData);
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00138CFB File Offset: 0x00136EFB
		public virtual void ExecuteFixedAction(DataContext context)
		{
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00138D00 File Offset: 0x00136F00
		protected virtual void TryAddNextAutoTravelTarget(DataContext context, Predicate<MapBlockData> condition)
		{
			bool flag = this.Character.GetNpcTravelTargets().Count > 0;
			if (!flag)
			{
				Location location = this.Character.GetLocation();
				bool flag2 = location.IsValid() && condition(DomainManager.Map.GetBlock(location));
				if (!flag2)
				{
					Location villageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
					MapBlockData targetBlock = DomainManager.Map.SelectBlockInCurrentOrNeighborState(context.Random, villageLocation, condition, false);
					bool flag3 = targetBlock == null;
					if (!flag3)
					{
						NpcTravelTarget travelTarget = new NpcTravelTarget(targetBlock.GetLocation(), 12);
						this.Character.AddTravelTarget(context, travelTarget);
					}
				}
			}
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x00138DA4 File Offset: 0x00136FA4
		public virtual bool GetExecuteFixedActionFailReasons(DataContext context, sbyte fixedActionType, ref List<sbyte> failReasons)
		{
			return false;
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x00138DB8 File Offset: 0x00136FB8
		public virtual Func<DataContext, bool, bool> GetFixedActionByType(sbyte fixedActionType)
		{
			return null;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00138DCB File Offset: 0x00136FCB
		public virtual void AddLifeRecordByFailReasons(List<sbyte> failReasons, sbyte fixedActionType)
		{
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00138DD0 File Offset: 0x00136FD0
		public void AddFailReasonsDistinct(bool realAction, ref List<sbyte> failReasons, sbyte failType)
		{
			bool flag = !failReasons.Contains(failType);
			if (flag)
			{
				failReasons.Add(failType);
			}
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00138DF8 File Offset: 0x00136FF8
		public Settlement GetCurrentSettlement()
		{
			Location location = this.Character.GetLocation();
			bool flag = !location.IsValid();
			Settlement result;
			if (flag)
			{
				result = null;
			}
			else
			{
				MapBlockData settlementBlock = DomainManager.Map.GetBelongSettlementBlock(location);
				result = ((settlementBlock == null) ? null : DomainManager.Organization.GetSettlementByLocation(settlementBlock.GetLocation()));
			}
			return result;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00138E4C File Offset: 0x0013704C
		public virtual IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
		{
			return null;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00138E60 File Offset: 0x00137060
		public bool IsWorkingAtArrangementLocation()
		{
			Location charLocation = this.Character.GetLocation();
			BasePrioritizedAction action;
			if (charLocation.IsValid() && this.Character.IsInteractableAsIntelligentCharacter() && DomainManager.Character.TryGetCharacterPrioritizedAction(this.Character.GetId(), out action))
			{
				VillagerRoleArrangementAction arrangementAction = action as VillagerRoleArrangementAction;
				if (arrangementAction != null && arrangementAction.HasArrived && arrangementAction.CheckValid(this.Character))
				{
					return DomainManager.Map.CheckLocationsHasSameRoot(action.Target.GetRealTargetLocation(), charLocation);
				}
			}
			return false;
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00138EE8 File Offset: 0x001370E8
		public bool CheckResource(sbyte requiredResourceType, int requiredResourceAmount)
		{
			int storageResourceAmount = this.SettlementTreasury.Resources.Get((int)requiredResourceType);
			return storageResourceAmount >= requiredResourceAmount;
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00138F13 File Offset: 0x00137113
		public void CostResource(DataContext context, sbyte resourceType, int resourceAmount)
		{
			this.SettlementTreasury.Resources.Subtract(resourceType, resourceAmount);
			DomainManager.Taiwu.SetNeedCommitSettlementTreasury(true);
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00138F35 File Offset: 0x00137135
		public void GainResource(DataContext context, sbyte resourceType, int resourceAmount)
		{
			this.SettlementTreasury.Resources.Add(resourceType, resourceAmount);
			DomainManager.Taiwu.SetNeedCommitSettlementTreasury(true);
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00138F57 File Offset: 0x00137157
		public void CostItem(DataContext context, ItemKey itemKey, int amount, bool deleteItem = false)
		{
			DomainManager.Taiwu.RemoveItem(context, itemKey, amount, ItemSourceType.Treasury, false, true);
			DomainManager.Taiwu.SetNeedCommitSettlementTreasury(true);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00138F77 File Offset: 0x00137177
		public void GainItem(DataContext context, ItemKey itemKey, int amount)
		{
			DomainManager.Taiwu.AddItem(context, itemKey, amount, ItemSourceType.Treasury, true);
			DomainManager.Taiwu.SetNeedCommitSettlementTreasury(true);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00138F98 File Offset: 0x00137198
		public virtual bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00138FAC File Offset: 0x001371AC
		public virtual int GetSerializedSize()
		{
			int totalSize = 6;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00138FD0 File Offset: 0x001371D0
		public unsafe virtual int Serialize(byte* pData)
		{
			*(short*)pData = 1;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.ArrangementTemplateId;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00139010 File Offset: 0x00137210
		public unsafe virtual int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.ArrangementTemplateId = *(int*)pCurrData;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04000314 RID: 788
		[SerializableGameDataField]
		public int ArrangementTemplateId = -1;

		// Token: 0x04000315 RID: 789
		public GameData.Domains.Character.Character Character;

		// Token: 0x04000316 RID: 790
		public VillagerWorkData WorkData;

		// Token: 0x0200095C RID: 2396
		private static class FieldIds
		{
			// Token: 0x0400274E RID: 10062
			public const ushort ArrangementTemplateId = 0;

			// Token: 0x0400274F RID: 10063
			public const ushort Count = 1;

			// Token: 0x04002750 RID: 10064
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"ArrangementTemplateId"
			};
		}
	}
}
