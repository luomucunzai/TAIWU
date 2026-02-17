using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu
{
	// Token: 0x0200003D RID: 61
	[SerializableGameData(NotForDisplayModule = true)]
	public class EquipmentPlan : ISerializableGameData
	{
		// Token: 0x06000F6C RID: 3948 RVA: 0x000FDFCC File Offset: 0x000FC1CC
		public EquipmentPlan()
		{
			this.Slots = new ItemKey[12];
			this.WeaponInnerRatios = new sbyte[3];
			for (int i = 0; i < 12; i++)
			{
				this.Slots[i] = ItemKey.Invalid;
			}
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x000FE01C File Offset: 0x000FC21C
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x000FE030 File Offset: 0x000FC230
		public int GetSerializedSize()
		{
			return sizeof(ItemKey) * 12 + 3;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x000FE050 File Offset: 0x000FC250
		public unsafe int Serialize(byte* pData)
		{
			for (int i = 0; i < 12; i++)
			{
				*(ItemKey*)(pData + (IntPtr)i * (IntPtr)sizeof(ItemKey)) = this.Slots[i];
			}
			pData += sizeof(ItemKey) * 12;
			for (int j = 0; j < 3; j++)
			{
				pData[j] = (byte)this.WeaponInnerRatios[j];
			}
			return this.GetSerializedSize();
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x000FE0C0 File Offset: 0x000FC2C0
		public unsafe int Deserialize(byte* pData)
		{
			for (int i = 0; i < 12; i++)
			{
				this.Slots[i] = *(ItemKey*)(pData + (IntPtr)i * (IntPtr)sizeof(ItemKey));
			}
			pData += sizeof(ItemKey) * 12;
			for (int j = 0; j < 3; j++)
			{
				this.WeaponInnerRatios[j] = *(sbyte*)(pData + j);
			}
			return this.GetSerializedSize();
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x000FE130 File Offset: 0x000FC330
		public void Record(GameData.Domains.Character.Character character)
		{
			ItemKey[] equipment = character.GetEquipment();
			sbyte[] weaponInnerRatios = DomainManager.Taiwu.GetWeaponInnerRatios();
			for (int i = 0; i < 12; i++)
			{
				this.Slots[i] = equipment[i];
			}
			for (int j = 0; j < this.WeaponInnerRatios.Length; j++)
			{
				this.WeaponInnerRatios[j] = weaponInnerRatios[j];
			}
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x000FE1A0 File Offset: 0x000FC3A0
		public bool Apply(DataContext context, GameData.Domains.Character.Character character, bool skipInvalid)
		{
			EquipmentPlan.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.inventory = character.GetInventory();
			CS$<>8__locals1.equipment = character.GetEquipment();
			ItemKey[] newEquipments = new ItemKey[12];
			Array.Fill<ItemKey>(newEquipments, ItemKey.Invalid);
			for (int i = 0; i < 12; i++)
			{
				ItemKey itemKey = this.<Apply>g__GetEquipmentAtSlot|8_0(i, ref CS$<>8__locals1);
				ItemKey prevEquippedItemKey = CS$<>8__locals1.equipment[i];
				bool flag = prevEquippedItemKey.ItemType == 3 && Config.Clothing.Instance[prevEquippedItemKey.TemplateId].AgeGroup != 2;
				if (flag)
				{
					newEquipments[i] = prevEquippedItemKey;
				}
				else
				{
					bool flag2 = !itemKey.IsValid() && skipInvalid;
					if (flag2)
					{
						itemKey = prevEquippedItemKey;
					}
					bool flag3 = itemKey.IsValid() && newEquipments.Exist(itemKey);
					if (flag3)
					{
						itemKey = ItemKey.Invalid;
					}
					newEquipments[i] = itemKey;
				}
			}
			bool isModified = !CollectionUtils.Equals<ItemKey>(CS$<>8__locals1.equipment, newEquipments, 12);
			character.ChangeEquipment(context, newEquipments);
			bool flag4 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag4)
			{
				sbyte[] weaponInnerRatios = DomainManager.Taiwu.GetWeaponInnerRatios();
				for (int j = 0; j < this.WeaponInnerRatios.Length; j++)
				{
					weaponInnerRatios[j] = this.WeaponInnerRatios[j];
				}
				DomainManager.Taiwu.SetWeaponInnerRatios(weaponInnerRatios, context);
			}
			return isModified;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x000FE314 File Offset: 0x000FC514
		[CompilerGenerated]
		private ItemKey <Apply>g__GetEquipmentAtSlot|8_0(int index, ref EquipmentPlan.<>c__DisplayClass8_0 A_2)
		{
			ItemKey itemKeyInPlan = this.Slots[index];
			bool flag = !itemKeyInPlan.IsValid();
			ItemKey result;
			if (flag)
			{
				result = ItemKey.Invalid;
			}
			else
			{
				bool flag2 = !DomainManager.Item.ItemExists(itemKeyInPlan);
				if (flag2)
				{
					result = ItemKey.Invalid;
				}
				else
				{
					EquipmentBase equipmentBase = DomainManager.Item.GetBaseEquipment(itemKeyInPlan);
					ItemKey itemKey = equipmentBase.GetItemKey();
					bool flag3 = !A_2.inventory.Items.ContainsKey(itemKey) && !A_2.equipment.Exist(itemKey);
					if (flag3)
					{
						result = ItemKey.Invalid;
					}
					else
					{
						result = itemKey;
					}
				}
			}
			return result;
		}

		// Token: 0x04000232 RID: 562
		[SerializableGameDataField]
		public readonly ItemKey[] Slots;

		// Token: 0x04000233 RID: 563
		[SerializableGameDataField]
		public readonly sbyte[] WeaponInnerRatios;
	}
}
