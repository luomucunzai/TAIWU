using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu
{
	// Token: 0x0200003B RID: 59
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
	public class CharacterCombatSkillConfiguration : ISerializableGameData
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x000FD495 File Offset: 0x000FB695
		public int PlanCount
		{
			get
			{
				List<CombatSkillPlan> combatSkillEquipPlans = this.CombatSkillEquipPlans;
				return (combatSkillEquipPlans != null) ? combatSkillEquipPlans.Count : 0;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x000FD4A9 File Offset: 0x000FB6A9
		public CombatSkillPlan CurrentEquipPlan
		{
			get
			{
				return this.CombatSkillEquipPlans[this.CurrentPlanId];
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x000FD4BC File Offset: 0x000FB6BC
		public ShortList CurrentMasterPlan
		{
			get
			{
				return this.CombatSkillMasterPlans[this.CurrentPlanId];
			}
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x000FD4CF File Offset: 0x000FB6CF
		public void OfflineRecordNeiliAllocation(Character character)
		{
			this.NeiliAllocation = character.GetBaseNeiliAllocation();
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x000FD4E0 File Offset: 0x000FB6E0
		public void OfflineRecordCombatSkillAttainmentPanels(Character character)
		{
			short[] panels = character.GetCombatSkillAttainmentPanels();
			if (this.CombatSkillAttainmentPanels == null)
			{
				this.CombatSkillAttainmentPanels = new short[panels.Length];
			}
			CombatSkillAttainmentPanelsHelper.CopyAll(panels, this.CombatSkillAttainmentPanels);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x000FD518 File Offset: 0x000FB718
		public void OfflineRecordCombatSkillPlan(Character character, int targetPlanId)
		{
			ShortList masteredSkills = DomainManager.Extra.GetCharacterMasteredCombatSkills(character.GetId());
			bool flag = targetPlanId >= this.PlanCount;
			if (flag)
			{
				this.OfflineUpdateMaxPlanCount(targetPlanId + 1);
			}
			this.CombatSkillEquipPlans[targetPlanId].Record(character);
			this.CombatSkillMasterPlans[targetPlanId] = new ShortList(masteredSkills);
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x000FD578 File Offset: 0x000FB778
		public void OfflineDeleteCombatSkillPlan(int planId)
		{
			int lastPlanId = this.PlanCount - 1;
			bool flag = lastPlanId <= 0;
			if (flag)
			{
				throw new InvalidOperationException("Deleting the only plan is not allowed.");
			}
			for (int i = planId; i < lastPlanId; i++)
			{
				int nextPlanId = planId + 1;
				this.CombatSkillEquipPlans[planId] = this.CombatSkillEquipPlans[nextPlanId];
				this.CombatSkillMasterPlans[planId] = this.CombatSkillMasterPlans[nextPlanId];
			}
			this.CombatSkillEquipPlans.RemoveAt(lastPlanId);
			this.CombatSkillMasterPlans.RemoveAt(lastPlanId);
			bool flag2 = this.CurrentPlanId >= lastPlanId;
			if (flag2)
			{
				this.CurrentPlanId = lastPlanId - 1;
			}
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x000FD624 File Offset: 0x000FB824
		public void OfflineUpdateMaxPlanCount(int count)
		{
			if (this.CombatSkillEquipPlans == null)
			{
				this.CombatSkillEquipPlans = new List<CombatSkillPlan>();
			}
			for (int i = this.CombatSkillEquipPlans.Count; i < count; i++)
			{
				this.CombatSkillEquipPlans.Add(new CombatSkillPlan());
			}
			if (this.CombatSkillMasterPlans == null)
			{
				this.CombatSkillMasterPlans = new List<ShortList>();
			}
			for (int j = this.CombatSkillMasterPlans.Count; j < count; j++)
			{
				this.CombatSkillMasterPlans.Add(ShortList.Create());
			}
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x000FD6AE File Offset: 0x000FB8AE
		public CharacterCombatSkillConfiguration(Character character)
		{
			this.OfflineUpdateMaxPlanCount(1);
			this.OfflineRecordCombatSkillPlan(character, 0);
			this.OfflineRecordNeiliAllocation(character);
			this.OfflineRecordCombatSkillAttainmentPanels(character);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x000FD6D9 File Offset: 0x000FB8D9
		public CharacterCombatSkillConfiguration()
		{
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x000FD6E4 File Offset: 0x000FB8E4
		public CharacterCombatSkillConfiguration(CharacterCombatSkillConfiguration other)
		{
			this.CurrentPlanId = other.CurrentPlanId;
			bool flag = other.CombatSkillEquipPlans != null;
			if (flag)
			{
				List<CombatSkillPlan> item = other.CombatSkillEquipPlans;
				int elementsCount = item.Count;
				this.CombatSkillEquipPlans = new List<CombatSkillPlan>(elementsCount);
				for (int i = 0; i < elementsCount; i++)
				{
					this.CombatSkillEquipPlans.Add(new CombatSkillPlan(item[i]));
				}
			}
			else
			{
				this.CombatSkillEquipPlans = null;
			}
			bool flag2 = other.CombatSkillMasterPlans != null;
			if (flag2)
			{
				List<ShortList> item2 = other.CombatSkillMasterPlans;
				int elementsCount2 = item2.Count;
				this.CombatSkillMasterPlans = new List<ShortList>(elementsCount2);
				for (int j = 0; j < elementsCount2; j++)
				{
					this.CombatSkillMasterPlans.Add(new ShortList(item2[j]));
				}
			}
			else
			{
				this.CombatSkillMasterPlans = null;
			}
			this.IsCombatSkillLocked = other.IsCombatSkillLocked;
			this.IsCombatSkillAttainmentLocked = other.IsCombatSkillAttainmentLocked;
			this.IsNeiliAllocationLocked = other.IsNeiliAllocationLocked;
			this.NeiliAllocation = other.NeiliAllocation;
			short[] item3 = other.CombatSkillAttainmentPanels;
			int elementsCount3 = item3.Length;
			this.CombatSkillAttainmentPanels = new short[elementsCount3];
			for (int k = 0; k < elementsCount3; k++)
			{
				this.CombatSkillAttainmentPanels[k] = item3[k];
			}
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x000FD83C File Offset: 0x000FBA3C
		public void Assign(CharacterCombatSkillConfiguration other)
		{
			this.CurrentPlanId = other.CurrentPlanId;
			bool flag = other.CombatSkillEquipPlans != null;
			if (flag)
			{
				List<CombatSkillPlan> item = other.CombatSkillEquipPlans;
				int elementsCount = item.Count;
				this.CombatSkillEquipPlans = new List<CombatSkillPlan>(elementsCount);
				for (int i = 0; i < elementsCount; i++)
				{
					this.CombatSkillEquipPlans.Add(new CombatSkillPlan(item[i]));
				}
			}
			else
			{
				this.CombatSkillEquipPlans = null;
			}
			bool flag2 = other.CombatSkillMasterPlans != null;
			if (flag2)
			{
				List<ShortList> item2 = other.CombatSkillMasterPlans;
				int elementsCount2 = item2.Count;
				this.CombatSkillMasterPlans = new List<ShortList>(elementsCount2);
				for (int j = 0; j < elementsCount2; j++)
				{
					this.CombatSkillMasterPlans.Add(new ShortList(item2[j]));
				}
			}
			else
			{
				this.CombatSkillMasterPlans = null;
			}
			this.IsCombatSkillLocked = other.IsCombatSkillLocked;
			this.IsCombatSkillAttainmentLocked = other.IsCombatSkillAttainmentLocked;
			this.IsNeiliAllocationLocked = other.IsNeiliAllocationLocked;
			this.NeiliAllocation = other.NeiliAllocation;
			short[] item3 = other.CombatSkillAttainmentPanels;
			int elementsCount3 = item3.Length;
			this.CombatSkillAttainmentPanels = new short[elementsCount3];
			for (int k = 0; k < elementsCount3; k++)
			{
				this.CombatSkillAttainmentPanels[k] = item3[k];
			}
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x000FD990 File Offset: 0x000FBB90
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x000FD9A4 File Offset: 0x000FBBA4
		public int GetSerializedSize()
		{
			int totalSize = 17;
			bool flag = this.CombatSkillEquipPlans != null;
			if (flag)
			{
				totalSize += 2;
				int elementsCount = this.CombatSkillEquipPlans.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					CombatSkillPlan element = this.CombatSkillEquipPlans[i];
					bool flag2 = element != null;
					if (flag2)
					{
						totalSize += 2 + element.GetSerializedSize();
					}
					else
					{
						totalSize += 2;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			bool flag3 = this.CombatSkillMasterPlans != null;
			if (flag3)
			{
				totalSize += 2;
				int elementsCount2 = this.CombatSkillMasterPlans.Count;
				for (int j = 0; j < elementsCount2; j++)
				{
					totalSize += this.CombatSkillMasterPlans[j].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			bool flag4 = this.CombatSkillAttainmentPanels != null;
			if (flag4)
			{
				totalSize += 2 + 2 * this.CombatSkillAttainmentPanels.Length;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x000FDAB0 File Offset: 0x000FBCB0
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 8;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.CurrentPlanId;
			pCurrData += 4;
			bool flag = this.CombatSkillEquipPlans != null;
			if (flag)
			{
				int elementsCount = this.CombatSkillEquipPlans.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					CombatSkillPlan element = this.CombatSkillEquipPlans[i];
					bool flag2 = element != null;
					if (flag2)
					{
						byte* pSubDataCount = pCurrData;
						pCurrData += 2;
						int subDataSize = element.Serialize(pCurrData);
						pCurrData += subDataSize;
						Tester.Assert(subDataSize <= 65535, "");
						*(short*)pSubDataCount = (short)((ushort)subDataSize);
					}
					else
					{
						*(short*)pCurrData = 0;
						pCurrData += 2;
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag3 = this.CombatSkillMasterPlans != null;
			if (flag3)
			{
				int elementsCount2 = this.CombatSkillMasterPlans.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					int subDataSize2 = this.CombatSkillMasterPlans[j].Serialize(pCurrData);
					pCurrData += subDataSize2;
					Tester.Assert(subDataSize2 <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			*pCurrData = (this.IsCombatSkillLocked ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.IsCombatSkillAttainmentLocked ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.IsNeiliAllocationLocked ? 1 : 0);
			pCurrData++;
			pCurrData += this.NeiliAllocation.Serialize(pCurrData);
			bool flag4 = this.CombatSkillAttainmentPanels != null;
			if (flag4)
			{
				int elementsCount3 = this.CombatSkillAttainmentPanels.Length;
				Tester.Assert(elementsCount3 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount3);
				pCurrData += 2;
				for (int k = 0; k < elementsCount3; k++)
				{
					*(short*)(pCurrData + (IntPtr)k * 2) = this.CombatSkillAttainmentPanels[k];
				}
				pCurrData += 2 * elementsCount3;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x000FDCF0 File Offset: 0x000FBEF0
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.CurrentPlanId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				ushort elementsCount = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag3 = elementsCount > 0;
				if (flag3)
				{
					bool flag4 = this.CombatSkillEquipPlans == null;
					if (flag4)
					{
						this.CombatSkillEquipPlans = new List<CombatSkillPlan>((int)elementsCount);
					}
					else
					{
						this.CombatSkillEquipPlans.Clear();
					}
					for (int i = 0; i < (int)elementsCount; i++)
					{
						ushort subDataCount = *(ushort*)pCurrData;
						pCurrData += 2;
						bool flag5 = subDataCount > 0;
						if (flag5)
						{
							CombatSkillPlan element = new CombatSkillPlan();
							pCurrData += element.Deserialize(pCurrData);
							this.CombatSkillEquipPlans.Add(element);
						}
						else
						{
							this.CombatSkillEquipPlans.Add(null);
						}
					}
				}
				else
				{
					List<CombatSkillPlan> combatSkillEquipPlans = this.CombatSkillEquipPlans;
					if (combatSkillEquipPlans != null)
					{
						combatSkillEquipPlans.Clear();
					}
				}
			}
			bool flag6 = fieldCount > 2;
			if (flag6)
			{
				ushort elementsCount2 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag7 = elementsCount2 > 0;
				if (flag7)
				{
					bool flag8 = this.CombatSkillMasterPlans == null;
					if (flag8)
					{
						this.CombatSkillMasterPlans = new List<ShortList>((int)elementsCount2);
					}
					else
					{
						this.CombatSkillMasterPlans.Clear();
					}
					for (int j = 0; j < (int)elementsCount2; j++)
					{
						ShortList element2 = default(ShortList);
						pCurrData += element2.Deserialize(pCurrData);
						this.CombatSkillMasterPlans.Add(element2);
					}
				}
				else
				{
					List<ShortList> combatSkillMasterPlans = this.CombatSkillMasterPlans;
					if (combatSkillMasterPlans != null)
					{
						combatSkillMasterPlans.Clear();
					}
				}
			}
			bool flag9 = fieldCount > 3;
			if (flag9)
			{
				this.IsCombatSkillLocked = (*pCurrData != 0);
				pCurrData++;
			}
			bool flag10 = fieldCount > 4;
			if (flag10)
			{
				this.IsCombatSkillAttainmentLocked = (*pCurrData != 0);
				pCurrData++;
			}
			bool flag11 = fieldCount > 5;
			if (flag11)
			{
				this.IsNeiliAllocationLocked = (*pCurrData != 0);
				pCurrData++;
			}
			bool flag12 = fieldCount > 6;
			if (flag12)
			{
				pCurrData += this.NeiliAllocation.Deserialize(pCurrData);
			}
			bool flag13 = fieldCount > 7;
			if (flag13)
			{
				ushort elementsCount3 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag14 = elementsCount3 > 0;
				if (flag14)
				{
					bool flag15 = this.CombatSkillAttainmentPanels == null || this.CombatSkillAttainmentPanels.Length != (int)elementsCount3;
					if (flag15)
					{
						this.CombatSkillAttainmentPanels = new short[(int)elementsCount3];
					}
					for (int k = 0; k < (int)elementsCount3; k++)
					{
						this.CombatSkillAttainmentPanels[k] = *(short*)(pCurrData + (IntPtr)k * 2);
					}
					pCurrData += 2 * elementsCount3;
				}
				else
				{
					this.CombatSkillAttainmentPanels = null;
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400022A RID: 554
		[SerializableGameDataField]
		public int CurrentPlanId;

		// Token: 0x0400022B RID: 555
		[SerializableGameDataField]
		public bool IsCombatSkillLocked;

		// Token: 0x0400022C RID: 556
		[SerializableGameDataField]
		public List<CombatSkillPlan> CombatSkillEquipPlans;

		// Token: 0x0400022D RID: 557
		[SerializableGameDataField]
		public List<ShortList> CombatSkillMasterPlans;

		// Token: 0x0400022E RID: 558
		[SerializableGameDataField]
		public bool IsCombatSkillAttainmentLocked;

		// Token: 0x0400022F RID: 559
		[SerializableGameDataField]
		public bool IsNeiliAllocationLocked;

		// Token: 0x04000230 RID: 560
		[SerializableGameDataField]
		public NeiliAllocation NeiliAllocation;

		// Token: 0x04000231 RID: 561
		[SerializableGameDataField]
		public short[] CombatSkillAttainmentPanels;

		// Token: 0x02000923 RID: 2339
		private static class FieldIds
		{
			// Token: 0x040026AA RID: 9898
			public const ushort CurrentPlanId = 0;

			// Token: 0x040026AB RID: 9899
			public const ushort CombatSkillEquipPlans = 1;

			// Token: 0x040026AC RID: 9900
			public const ushort CombatSkillMasterPlans = 2;

			// Token: 0x040026AD RID: 9901
			public const ushort IsCombatSkillLocked = 3;

			// Token: 0x040026AE RID: 9902
			public const ushort IsCombatSkillAttainmentLocked = 4;

			// Token: 0x040026AF RID: 9903
			public const ushort IsNeiliAllocationLocked = 5;

			// Token: 0x040026B0 RID: 9904
			public const ushort NeiliAllocation = 6;

			// Token: 0x040026B1 RID: 9905
			public const ushort CombatSkillAttainmentPanels = 7;

			// Token: 0x040026B2 RID: 9906
			public const ushort Count = 8;

			// Token: 0x040026B3 RID: 9907
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"CurrentPlanId",
				"CombatSkillEquipPlans",
				"CombatSkillMasterPlans",
				"IsCombatSkillLocked",
				"IsCombatSkillAttainmentLocked",
				"IsNeiliAllocationLocked",
				"NeiliAllocation",
				"CombatSkillAttainmentPanels"
			};
		}
	}
}
