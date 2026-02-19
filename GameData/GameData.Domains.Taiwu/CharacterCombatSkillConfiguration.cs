using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
public class CharacterCombatSkillConfiguration : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort CurrentPlanId = 0;

		public const ushort CombatSkillEquipPlans = 1;

		public const ushort CombatSkillMasterPlans = 2;

		public const ushort IsCombatSkillLocked = 3;

		public const ushort IsCombatSkillAttainmentLocked = 4;

		public const ushort IsNeiliAllocationLocked = 5;

		public const ushort NeiliAllocation = 6;

		public const ushort CombatSkillAttainmentPanels = 7;

		public const ushort Count = 8;

		public static readonly string[] FieldId2FieldName = new string[8] { "CurrentPlanId", "CombatSkillEquipPlans", "CombatSkillMasterPlans", "IsCombatSkillLocked", "IsCombatSkillAttainmentLocked", "IsNeiliAllocationLocked", "NeiliAllocation", "CombatSkillAttainmentPanels" };
	}

	[SerializableGameDataField]
	public int CurrentPlanId;

	[SerializableGameDataField]
	public bool IsCombatSkillLocked;

	[SerializableGameDataField]
	public List<CombatSkillPlan> CombatSkillEquipPlans;

	[SerializableGameDataField]
	public List<ShortList> CombatSkillMasterPlans;

	[SerializableGameDataField]
	public bool IsCombatSkillAttainmentLocked;

	[SerializableGameDataField]
	public bool IsNeiliAllocationLocked;

	[SerializableGameDataField]
	public NeiliAllocation NeiliAllocation;

	[SerializableGameDataField]
	public short[] CombatSkillAttainmentPanels;

	public int PlanCount => CombatSkillEquipPlans?.Count ?? 0;

	public CombatSkillPlan CurrentEquipPlan => CombatSkillEquipPlans[CurrentPlanId];

	public ShortList CurrentMasterPlan => CombatSkillMasterPlans[CurrentPlanId];

	public void OfflineRecordNeiliAllocation(GameData.Domains.Character.Character character)
	{
		NeiliAllocation = character.GetBaseNeiliAllocation();
	}

	public void OfflineRecordCombatSkillAttainmentPanels(GameData.Domains.Character.Character character)
	{
		short[] combatSkillAttainmentPanels = character.GetCombatSkillAttainmentPanels();
		if (CombatSkillAttainmentPanels == null)
		{
			CombatSkillAttainmentPanels = new short[combatSkillAttainmentPanels.Length];
		}
		CombatSkillAttainmentPanelsHelper.CopyAll(combatSkillAttainmentPanels, CombatSkillAttainmentPanels);
	}

	public void OfflineRecordCombatSkillPlan(GameData.Domains.Character.Character character, int targetPlanId)
	{
		ShortList characterMasteredCombatSkills = DomainManager.Extra.GetCharacterMasteredCombatSkills(character.GetId());
		if (targetPlanId >= PlanCount)
		{
			OfflineUpdateMaxPlanCount(targetPlanId + 1);
		}
		CombatSkillEquipPlans[targetPlanId].Record(character);
		CombatSkillMasterPlans[targetPlanId] = new ShortList(characterMasteredCombatSkills);
	}

	public void OfflineDeleteCombatSkillPlan(int planId)
	{
		int num = PlanCount - 1;
		if (num <= 0)
		{
			throw new InvalidOperationException("Deleting the only plan is not allowed.");
		}
		for (int i = planId; i < num; i++)
		{
			int index = planId + 1;
			CombatSkillEquipPlans[planId] = CombatSkillEquipPlans[index];
			CombatSkillMasterPlans[planId] = CombatSkillMasterPlans[index];
		}
		CombatSkillEquipPlans.RemoveAt(num);
		CombatSkillMasterPlans.RemoveAt(num);
		if (CurrentPlanId >= num)
		{
			CurrentPlanId = num - 1;
		}
	}

	public void OfflineUpdateMaxPlanCount(int count)
	{
		if (CombatSkillEquipPlans == null)
		{
			CombatSkillEquipPlans = new List<CombatSkillPlan>();
		}
		for (int i = CombatSkillEquipPlans.Count; i < count; i++)
		{
			CombatSkillEquipPlans.Add(new CombatSkillPlan());
		}
		if (CombatSkillMasterPlans == null)
		{
			CombatSkillMasterPlans = new List<ShortList>();
		}
		for (int j = CombatSkillMasterPlans.Count; j < count; j++)
		{
			CombatSkillMasterPlans.Add(ShortList.Create());
		}
	}

	public CharacterCombatSkillConfiguration(GameData.Domains.Character.Character character)
	{
		OfflineUpdateMaxPlanCount(1);
		OfflineRecordCombatSkillPlan(character, 0);
		OfflineRecordNeiliAllocation(character);
		OfflineRecordCombatSkillAttainmentPanels(character);
	}

	public CharacterCombatSkillConfiguration()
	{
	}

	public CharacterCombatSkillConfiguration(CharacterCombatSkillConfiguration other)
	{
		CurrentPlanId = other.CurrentPlanId;
		if (other.CombatSkillEquipPlans != null)
		{
			List<CombatSkillPlan> combatSkillEquipPlans = other.CombatSkillEquipPlans;
			int count = combatSkillEquipPlans.Count;
			CombatSkillEquipPlans = new List<CombatSkillPlan>(count);
			for (int i = 0; i < count; i++)
			{
				CombatSkillEquipPlans.Add(new CombatSkillPlan(combatSkillEquipPlans[i]));
			}
		}
		else
		{
			CombatSkillEquipPlans = null;
		}
		if (other.CombatSkillMasterPlans != null)
		{
			List<ShortList> combatSkillMasterPlans = other.CombatSkillMasterPlans;
			int count2 = combatSkillMasterPlans.Count;
			CombatSkillMasterPlans = new List<ShortList>(count2);
			for (int j = 0; j < count2; j++)
			{
				CombatSkillMasterPlans.Add(new ShortList(combatSkillMasterPlans[j]));
			}
		}
		else
		{
			CombatSkillMasterPlans = null;
		}
		IsCombatSkillLocked = other.IsCombatSkillLocked;
		IsCombatSkillAttainmentLocked = other.IsCombatSkillAttainmentLocked;
		IsNeiliAllocationLocked = other.IsNeiliAllocationLocked;
		NeiliAllocation = other.NeiliAllocation;
		short[] combatSkillAttainmentPanels = other.CombatSkillAttainmentPanels;
		int num = combatSkillAttainmentPanels.Length;
		CombatSkillAttainmentPanels = new short[num];
		for (int k = 0; k < num; k++)
		{
			CombatSkillAttainmentPanels[k] = combatSkillAttainmentPanels[k];
		}
	}

	public void Assign(CharacterCombatSkillConfiguration other)
	{
		CurrentPlanId = other.CurrentPlanId;
		if (other.CombatSkillEquipPlans != null)
		{
			List<CombatSkillPlan> combatSkillEquipPlans = other.CombatSkillEquipPlans;
			int count = combatSkillEquipPlans.Count;
			CombatSkillEquipPlans = new List<CombatSkillPlan>(count);
			for (int i = 0; i < count; i++)
			{
				CombatSkillEquipPlans.Add(new CombatSkillPlan(combatSkillEquipPlans[i]));
			}
		}
		else
		{
			CombatSkillEquipPlans = null;
		}
		if (other.CombatSkillMasterPlans != null)
		{
			List<ShortList> combatSkillMasterPlans = other.CombatSkillMasterPlans;
			int count2 = combatSkillMasterPlans.Count;
			CombatSkillMasterPlans = new List<ShortList>(count2);
			for (int j = 0; j < count2; j++)
			{
				CombatSkillMasterPlans.Add(new ShortList(combatSkillMasterPlans[j]));
			}
		}
		else
		{
			CombatSkillMasterPlans = null;
		}
		IsCombatSkillLocked = other.IsCombatSkillLocked;
		IsCombatSkillAttainmentLocked = other.IsCombatSkillAttainmentLocked;
		IsNeiliAllocationLocked = other.IsNeiliAllocationLocked;
		NeiliAllocation = other.NeiliAllocation;
		short[] combatSkillAttainmentPanels = other.CombatSkillAttainmentPanels;
		int num = combatSkillAttainmentPanels.Length;
		CombatSkillAttainmentPanels = new short[num];
		for (int k = 0; k < num; k++)
		{
			CombatSkillAttainmentPanels[k] = combatSkillAttainmentPanels[k];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 17;
		if (CombatSkillEquipPlans != null)
		{
			num += 2;
			int count = CombatSkillEquipPlans.Count;
			for (int i = 0; i < count; i++)
			{
				CombatSkillPlan combatSkillPlan = CombatSkillEquipPlans[i];
				num = ((combatSkillPlan == null) ? (num + 2) : (num + (2 + combatSkillPlan.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (CombatSkillMasterPlans != null)
		{
			num += 2;
			int count2 = CombatSkillMasterPlans.Count;
			for (int j = 0; j < count2; j++)
			{
				num += CombatSkillMasterPlans[j].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		num = ((CombatSkillAttainmentPanels == null) ? (num + 2) : (num + (2 + 2 * CombatSkillAttainmentPanels.Length)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 8;
		ptr += 2;
		*(int*)ptr = CurrentPlanId;
		ptr += 4;
		if (CombatSkillEquipPlans != null)
		{
			int count = CombatSkillEquipPlans.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				CombatSkillPlan combatSkillPlan = CombatSkillEquipPlans[i];
				if (combatSkillPlan != null)
				{
					byte* ptr2 = ptr;
					ptr += 2;
					int num = combatSkillPlan.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)ptr2 = (ushort)num;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CombatSkillMasterPlans != null)
		{
			int count2 = CombatSkillMasterPlans.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				int num2 = CombatSkillMasterPlans[j].Serialize(ptr);
				ptr += num2;
				Tester.Assert(num2 <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (IsCombatSkillLocked ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsCombatSkillAttainmentLocked ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsNeiliAllocationLocked ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += NeiliAllocation.Serialize(ptr);
		if (CombatSkillAttainmentPanels != null)
		{
			int num3 = CombatSkillAttainmentPanels.Length;
			Tester.Assert(num3 <= 65535);
			*(ushort*)ptr = (ushort)num3;
			ptr += 2;
			for (int k = 0; k < num3; k++)
			{
				((short*)ptr)[k] = CombatSkillAttainmentPanels[k];
			}
			ptr += 2 * num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			CurrentPlanId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (CombatSkillEquipPlans == null)
				{
					CombatSkillEquipPlans = new List<CombatSkillPlan>(num2);
				}
				else
				{
					CombatSkillEquipPlans.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					if (num3 > 0)
					{
						CombatSkillPlan combatSkillPlan = new CombatSkillPlan();
						ptr += combatSkillPlan.Deserialize(ptr);
						CombatSkillEquipPlans.Add(combatSkillPlan);
					}
					else
					{
						CombatSkillEquipPlans.Add(null);
					}
				}
			}
			else
			{
				CombatSkillEquipPlans?.Clear();
			}
		}
		if (num > 2)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (CombatSkillMasterPlans == null)
				{
					CombatSkillMasterPlans = new List<ShortList>(num4);
				}
				else
				{
					CombatSkillMasterPlans.Clear();
				}
				for (int j = 0; j < num4; j++)
				{
					ShortList item = default(ShortList);
					ptr += item.Deserialize(ptr);
					CombatSkillMasterPlans.Add(item);
				}
			}
			else
			{
				CombatSkillMasterPlans?.Clear();
			}
		}
		if (num > 3)
		{
			IsCombatSkillLocked = *ptr != 0;
			ptr++;
		}
		if (num > 4)
		{
			IsCombatSkillAttainmentLocked = *ptr != 0;
			ptr++;
		}
		if (num > 5)
		{
			IsNeiliAllocationLocked = *ptr != 0;
			ptr++;
		}
		if (num > 6)
		{
			ptr += NeiliAllocation.Deserialize(ptr);
		}
		if (num > 7)
		{
			ushort num5 = *(ushort*)ptr;
			ptr += 2;
			if (num5 > 0)
			{
				if (CombatSkillAttainmentPanels == null || CombatSkillAttainmentPanels.Length != num5)
				{
					CombatSkillAttainmentPanels = new short[num5];
				}
				for (int k = 0; k < num5; k++)
				{
					CombatSkillAttainmentPanels[k] = ((short*)ptr)[k];
				}
				ptr += 2 * num5;
			}
			else
			{
				CombatSkillAttainmentPanels = null;
			}
		}
		int num6 = (int)(ptr - pData);
		return (num6 <= 4) ? num6 : ((num6 + 3) / 4 * 4);
	}
}
