using System;
using System.Collections.Generic;
using System.Diagnostics;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Organization;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class SettlementTreasury : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort StandardGuardCount = 0;

		public const ushort StandardGuardGrade = 1;

		public const ushort GuardIds = 2;

		public const ushort TemplateGuardIds = 3;

		public const ushort Resources = 4;

		public const ushort ObsoleteInventory = 5;

		public const ushort LovingItemSubTypes = 6;

		public const ushort HatingItemSubTypes = 7;

		public const ushort Contributions = 8;

		public const ushort AlertTime = 9;

		public const ushort ResourceStatus = 10;

		public const ushort Inventory = 11;

		public const ushort LayerIndex = 12;

		public const ushort Count = 13;

		public static readonly string[] FieldId2FieldName = new string[13]
		{
			"StandardGuardCount", "StandardGuardGrade", "GuardIds", "TemplateGuardIds", "Resources", "ObsoleteInventory", "LovingItemSubTypes", "HatingItemSubTypes", "Contributions", "AlertTime",
			"ResourceStatus", "Inventory", "LayerIndex"
		};
	}

	[Obsolete("库房守卫现在数量固定")]
	[SerializableGameDataField]
	public int StandardGuardCount;

	[Obsolete("库房守卫现在最大品级固定")]
	[SerializableGameDataField]
	public sbyte StandardGuardGrade;

	[SerializableGameDataField]
	public CharacterSet GuardIds;

	[SerializableGameDataField]
	public List<short> TemplateGuardIds = new List<short>();

	[SerializableGameDataField]
	public ResourceInts Resources;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public Inventory Inventory = new Inventory();

	[SerializableGameDataField]
	private Inventory _obsoleteInventory;

	[SerializableGameDataField]
	public List<short> LovingItemSubTypes = new List<short>();

	[SerializableGameDataField]
	public List<short> HatingItemSubTypes = new List<short>();

	[SerializableGameDataField]
	public Dictionary<int, int> Contributions = new Dictionary<int, int>();

	[Obsolete]
	[SerializableGameDataField]
	public byte AlertTime;

	[Obsolete]
	[SerializableGameDataField]
	public sbyte ResourceStatus;

	[SerializableGameDataField]
	public sbyte LayerIndex;

	private readonly Dictionary<int, int> _memberUsedPresetContributions = new Dictionary<int, int>();

	public bool UpgradeData()
	{
		if (Inventory == null)
		{
			Inventory = new Inventory();
		}
		if (_obsoleteInventory != null)
		{
			Dictionary<ItemKey, int> items = _obsoleteInventory.Items;
			if (items != null && items.Count > 0)
			{
				foreach (KeyValuePair<ItemKey, int> item in _obsoleteInventory.Items)
				{
					Inventory.OfflineAdd(item.Key, item.Value);
				}
				_obsoleteInventory.Items.Clear();
				return true;
			}
		}
		return false;
	}

	public int CalcBonusInfluencePower(int charId)
	{
		int valueOrDefault = Contributions.GetValueOrDefault(charId, 0);
		int baseValue = Accessory.Instance[(short)8].BaseValue;
		return 100 + MathUtils.Min(valueOrDefault * 10 / baseValue, 100);
	}

	public int CalcAdjustedWorth(short itemSubType, int worth)
	{
		if (LovingItemSubTypes.Contains(itemSubType))
		{
			return worth * 125 / 100;
		}
		if (HatingItemSubTypes.Contains(itemSubType))
		{
			return worth * 50 / 100;
		}
		return worth;
	}

	public int GetContribution(int charId)
	{
		if (!Contributions.TryGetValue(charId, out var value))
		{
			return 0;
		}
		return value;
	}

	public void DetectAndFixInvalidData()
	{
		for (sbyte b = 0; b < 8; b++)
		{
			if (Resources[b] < 0)
			{
				AdaptableLog.Warning($"Invalid resource amount for settlement treasury: type={b},value={Resources[b]}\n{new StackTrace()}", appendWarningMessage: true);
				Resources[b] = 0;
			}
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 42;
		num += GuardIds.GetSerializedSize();
		num = ((TemplateGuardIds == null) ? (num + 2) : (num + (2 + 2 * TemplateGuardIds.Count)));
		num = ((_obsoleteInventory == null) ? (num + 2) : (num + (2 + _obsoleteInventory.GetSerializedSize())));
		num = ((LovingItemSubTypes == null) ? (num + 2) : (num + (2 + 2 * LovingItemSubTypes.Count)));
		num = ((HatingItemSubTypes == null) ? (num + 2) : (num + (2 + 2 * HatingItemSubTypes.Count)));
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, int>((IReadOnlyDictionary<int, int>)Contributions);
		num = ((Inventory == null) ? (num + 4) : (num + (4 + Inventory.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 13;
		ptr += 2;
		*(int*)ptr = StandardGuardCount;
		ptr += 4;
		*ptr = (byte)StandardGuardGrade;
		ptr++;
		int num = GuardIds.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		if (TemplateGuardIds != null)
		{
			int count = TemplateGuardIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = TemplateGuardIds[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += Resources.Serialize(ptr);
		if (_obsoleteInventory != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num2 = _obsoleteInventory.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LovingItemSubTypes != null)
		{
			int count2 = LovingItemSubTypes.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((short*)ptr)[j] = LovingItemSubTypes[j];
			}
			ptr += 2 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (HatingItemSubTypes != null)
		{
			int count3 = HatingItemSubTypes.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((short*)ptr)[k] = HatingItemSubTypes[k];
			}
			ptr += 2 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypePair.Serialize<int, int>(ptr, ref Contributions);
		*ptr = AlertTime;
		ptr++;
		*ptr = (byte)ResourceStatus;
		ptr++;
		if (Inventory != null)
		{
			byte* intPtr2 = ptr;
			ptr += 4;
			int num3 = Inventory.Serialize(ptr);
			ptr += num3;
			Tester.Assert(num3 <= int.MaxValue);
			*(int*)intPtr2 = num3;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		*ptr = (byte)LayerIndex;
		ptr++;
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
			StandardGuardCount = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			StandardGuardGrade = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 2)
		{
			ptr += GuardIds.Deserialize(ptr);
		}
		if (num > 3)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (TemplateGuardIds == null)
				{
					TemplateGuardIds = new List<short>(num2);
				}
				else
				{
					TemplateGuardIds.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					TemplateGuardIds.Add(((short*)ptr)[i]);
				}
				ptr += 2 * num2;
			}
			else
			{
				TemplateGuardIds?.Clear();
			}
		}
		if (num > 4)
		{
			ptr += Resources.Deserialize(ptr);
		}
		if (num > 5)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (_obsoleteInventory == null)
				{
					_obsoleteInventory = new Inventory();
				}
				ptr += _obsoleteInventory.Deserialize(ptr);
			}
			else
			{
				_obsoleteInventory = null;
			}
		}
		if (num > 6)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (LovingItemSubTypes == null)
				{
					LovingItemSubTypes = new List<short>(num4);
				}
				else
				{
					LovingItemSubTypes.Clear();
				}
				for (int j = 0; j < num4; j++)
				{
					LovingItemSubTypes.Add(((short*)ptr)[j]);
				}
				ptr += 2 * num4;
			}
			else
			{
				LovingItemSubTypes?.Clear();
			}
		}
		if (num > 7)
		{
			ushort num5 = *(ushort*)ptr;
			ptr += 2;
			if (num5 > 0)
			{
				if (HatingItemSubTypes == null)
				{
					HatingItemSubTypes = new List<short>(num5);
				}
				else
				{
					HatingItemSubTypes.Clear();
				}
				for (int k = 0; k < num5; k++)
				{
					HatingItemSubTypes.Add(((short*)ptr)[k]);
				}
				ptr += 2 * num5;
			}
			else
			{
				HatingItemSubTypes?.Clear();
			}
		}
		if (num > 8)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, int>(ptr, ref Contributions);
		}
		if (num > 9)
		{
			AlertTime = *ptr;
			ptr++;
		}
		if (num > 10)
		{
			ResourceStatus = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 11)
		{
			int num6 = *(int*)ptr;
			ptr += 4;
			if (num6 > 0)
			{
				if (Inventory == null)
				{
					Inventory = new Inventory();
				}
				ptr += Inventory.Deserialize(ptr);
			}
			else
			{
				Inventory = null;
			}
		}
		if (num > 12)
		{
			LayerIndex = (sbyte)(*ptr);
			ptr++;
		}
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}

	public void ClearMemberUsedPresetContribution()
	{
		_memberUsedPresetContributions.Clear();
	}

	public int GetMemberContribution(int charId, OrganizationInfo orgInfo)
	{
		int contributionPerMonth = orgInfo.GetOrgMemberConfig().ContributionPerMonth;
		int valueOrDefault = _memberUsedPresetContributions.GetValueOrDefault(charId, 0);
		return Contributions.GetValueOrDefault(charId, 0) + contributionPerMonth - valueOrDefault;
	}

	public void OfflineChangeContribution(int charId, int presetContribution, int delta)
	{
		if (delta >= 0)
		{
			OfflineChanceActualContribution(charId, delta);
			return;
		}
		int valueOrDefault = _memberUsedPresetContributions.GetValueOrDefault(charId);
		int num = presetContribution - valueOrDefault;
		if (num + delta >= 0)
		{
			_memberUsedPresetContributions[charId] = valueOrDefault - delta;
			return;
		}
		OfflineChanceActualContribution(charId, num + delta);
		_memberUsedPresetContributions[charId] = presetContribution;
	}

	private void OfflineChanceActualContribution(int charId, int delta)
	{
		if (Contributions.TryGetValue(charId, out var value))
		{
			Contributions[charId] = value + delta;
		}
		else
		{
			Contributions.Add(charId, delta);
		}
	}
}
