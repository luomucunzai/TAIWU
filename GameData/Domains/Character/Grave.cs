using System;
using System.Runtime.CompilerServices;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character
{
	// Token: 0x02000813 RID: 2067
	[SerializableGameData(NotForDisplayModule = true)]
	public class Grave : BaseGameDataObject, ISerializableGameData
	{
		// Token: 0x06007483 RID: 29827 RVA: 0x0044482C File Offset: 0x00442A2C
		public unsafe Grave(DataContext context, Character character, Location location, sbyte level)
		{
			this._id = character.GetId();
			this._location = location;
			this._level = level;
			this._durability = GlobalConfig.Instance.GraveDurabilities[(int)this._level];
			bool flag = character.GetCreatingType() == 1;
			if (flag)
			{
				this._inventory = character.GetInventory();
				foreach (ItemKey itemKey in this._inventory.Items.Keys)
				{
					ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
					item.RemoveOwner(ItemOwnerType.CharacterInventory, this._id);
					item.SetOwner(ItemOwnerType.Grave, this._id);
				}
				this.PutEquipmentIntoInventory(character.GetEquipment(), this._inventory);
			}
			else
			{
				this._inventory = character.GetInventory();
				foreach (ItemKey itemKey2 in this._inventory.Items.Keys)
				{
					ItemBase item2 = DomainManager.Item.GetBaseItem(itemKey2);
					item2.RemoveOwner(ItemOwnerType.CharacterInventory, this._id);
					item2.SetOwner(ItemOwnerType.Grave, this._id);
				}
				character.SetInventory(new Inventory(), context);
			}
			Grave.RemoveEatingItems(context, character.GetEatingItems());
			this._resources = *character.GetResources();
			this._skeletonCharId = -1;
		}

		// Token: 0x06007484 RID: 29828 RVA: 0x004449D8 File Offset: 0x00442BD8
		public void RemoveInventoryItem(DataContext context, ItemKey itemKey, int amount, bool deleteItem = false)
		{
			bool flag = amount <= 0;
			if (!flag)
			{
				DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Grave, this._id);
				this._inventory.OfflineRemove(itemKey, amount);
				this.SetInventory(this._inventory, context);
				if (deleteItem)
				{
					DomainManager.Item.RemoveItem(context, itemKey);
				}
			}
		}

		// Token: 0x06007485 RID: 29829 RVA: 0x00444A38 File Offset: 0x00442C38
		public static sbyte CalcGraveLevel(int moneyAvailable)
		{
			bool flag = moneyAvailable < (int)GlobalConfig.Instance.GraveLevelMoneyCosts[1];
			sbyte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = moneyAvailable < (int)GlobalConfig.Instance.GraveLevelMoneyCosts[2];
				if (flag2)
				{
					result = 1;
				}
				else
				{
					bool flag3 = moneyAvailable < (int)GlobalConfig.Instance.GraveLevelMoneyCosts[3];
					if (flag3)
					{
						result = 2;
					}
					else
					{
						result = 3;
					}
				}
			}
			return result;
		}

		// Token: 0x06007486 RID: 29830 RVA: 0x00444A90 File Offset: 0x00442C90
		private void PutEquipmentIntoInventory(ItemKey[] equipment, Inventory inventory)
		{
			for (int i = 0; i < 12; i++)
			{
				ItemKey itemKey = equipment[i];
				bool flag = itemKey.IsValid();
				if (flag)
				{
					EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
					baseEquipment.RemoveOwner(ItemOwnerType.CharacterEquipment, this._id);
					baseEquipment.SetOwner(ItemOwnerType.Grave, this._id);
					inventory.Items.Add(itemKey, 1);
				}
			}
		}

		// Token: 0x06007487 RID: 29831 RVA: 0x00444B00 File Offset: 0x00442D00
		private unsafe static void RemoveEatingItems(DataContext context, ref EatingItems eatingItems)
		{
			for (int i = 0; i < 9; i++)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)i * 8));
				bool flag = itemKey.IsValid();
				if (flag)
				{
					DomainManager.Item.RemoveItem(context, itemKey);
				}
			}
		}

		// Token: 0x06007488 RID: 29832 RVA: 0x00444B50 File Offset: 0x00442D50
		public int GetId()
		{
			return this._id;
		}

		// Token: 0x06007489 RID: 29833 RVA: 0x00444B68 File Offset: 0x00442D68
		public Location GetLocation()
		{
			return this._location;
		}

		// Token: 0x0600748A RID: 29834 RVA: 0x00444B80 File Offset: 0x00442D80
		public unsafe void SetLocation(Location location, DataContext context)
		{
			this._location = location;
			base.SetModifiedAndInvalidateInfluencedCache(1, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 4U, 4);
				pData += this._location.Serialize(pData);
			}
		}

		// Token: 0x0600748B RID: 29835 RVA: 0x00444BE4 File Offset: 0x00442DE4
		public sbyte GetLevel()
		{
			return this._level;
		}

		// Token: 0x0600748C RID: 29836 RVA: 0x00444BFC File Offset: 0x00442DFC
		public unsafe void SetLevel(sbyte level, DataContext context)
		{
			this._level = level;
			base.SetModifiedAndInvalidateInfluencedCache(2, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 8U, 1);
				*pData = (byte)this._level;
				pData++;
			}
		}

		// Token: 0x0600748D RID: 29837 RVA: 0x00444C5C File Offset: 0x00442E5C
		public short GetDurability()
		{
			return this._durability;
		}

		// Token: 0x0600748E RID: 29838 RVA: 0x00444C74 File Offset: 0x00442E74
		public unsafe void SetDurability(short durability, DataContext context)
		{
			this._durability = durability;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 9U, 2);
				*(short*)pData = this._durability;
				pData += 2;
			}
		}

		// Token: 0x0600748F RID: 29839 RVA: 0x00444CD4 File Offset: 0x00442ED4
		public Inventory GetInventory()
		{
			return this._inventory;
		}

		// Token: 0x06007490 RID: 29840 RVA: 0x00444CEC File Offset: 0x00442EEC
		public unsafe void SetInventory(Inventory inventory, DataContext context)
		{
			this._inventory = inventory;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._inventory.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 0, dataSize);
				pData += this._inventory.Serialize(pData);
			}
		}

		// Token: 0x06007491 RID: 29841 RVA: 0x00444D5C File Offset: 0x00442F5C
		public ref ResourceInts GetResources()
		{
			return ref this._resources;
		}

		// Token: 0x06007492 RID: 29842 RVA: 0x00444D74 File Offset: 0x00442F74
		public unsafe void SetResources(ref ResourceInts resources, DataContext context)
		{
			this._resources = resources;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 11U, 32);
				pData += this._resources.Serialize(pData);
			}
		}

		// Token: 0x06007493 RID: 29843 RVA: 0x00444DE0 File Offset: 0x00442FE0
		public int GetSkeletonCharId()
		{
			return this._skeletonCharId;
		}

		// Token: 0x06007494 RID: 29844 RVA: 0x00444DF8 File Offset: 0x00442FF8
		public unsafe void SetSkeletonCharId(int skeletonCharId, DataContext context)
		{
			this._skeletonCharId = skeletonCharId;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 43U, 4);
				*(int*)pData = this._skeletonCharId;
				pData += 4;
			}
		}

		// Token: 0x06007495 RID: 29845 RVA: 0x00444E58 File Offset: 0x00443058
		public Grave()
		{
			this._inventory = new Inventory();
		}

		// Token: 0x06007496 RID: 29846 RVA: 0x00444E70 File Offset: 0x00443070
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x00444E84 File Offset: 0x00443084
		public int GetSerializedSize()
		{
			int totalSize = 51;
			int dataSize = this._inventory.GetSerializedSize();
			return totalSize + dataSize;
		}

		// Token: 0x06007498 RID: 29848 RVA: 0x00444EAC File Offset: 0x004430AC
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this._id;
			byte* pCurrData = pData + 4;
			pCurrData += this._location.Serialize(pCurrData);
			*pCurrData = (byte)this._level;
			pCurrData++;
			*(short*)pCurrData = this._durability;
			pCurrData += 2;
			pCurrData += this._resources.Serialize(pCurrData);
			*(int*)pCurrData = this._skeletonCharId;
			pCurrData += 4;
			byte* pBegin = pCurrData;
			pCurrData += 4;
			pCurrData += this._inventory.Serialize(pCurrData);
			int fieldSize = (int)((long)(pCurrData - pBegin) - 4L);
			bool flag = fieldSize > 4194304;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_inventory");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin = fieldSize;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06007499 RID: 29849 RVA: 0x00444FA0 File Offset: 0x004431A0
		public unsafe int Deserialize(byte* pData)
		{
			this._id = *(int*)pData;
			byte* pCurrData = pData + 4;
			pCurrData += this._location.Deserialize(pCurrData);
			this._level = *(sbyte*)pCurrData;
			pCurrData++;
			this._durability = *(short*)pCurrData;
			pCurrData += 2;
			pCurrData += this._resources.Deserialize(pCurrData);
			this._skeletonCharId = *(int*)pCurrData;
			pCurrData += 4;
			pCurrData += 4;
			pCurrData += this._inventory.Deserialize(pCurrData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04001ED8 RID: 7896
		[CollectionObjectField(false, true, false, true, false)]
		private int _id;

		// Token: 0x04001ED9 RID: 7897
		[CollectionObjectField(false, true, false, false, false)]
		private Location _location;

		// Token: 0x04001EDA RID: 7898
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _level;

		// Token: 0x04001EDB RID: 7899
		[CollectionObjectField(false, true, false, false, false)]
		private short _durability;

		// Token: 0x04001EDC RID: 7900
		[CollectionObjectField(false, true, false, false, false)]
		private Inventory _inventory;

		// Token: 0x04001EDD RID: 7901
		[CollectionObjectField(false, true, false, false, false)]
		private ResourceInts _resources;

		// Token: 0x04001EDE RID: 7902
		[CollectionObjectField(false, true, false, false, false)]
		private int _skeletonCharId;

		// Token: 0x04001EDF RID: 7903
		public const int FixedSize = 47;

		// Token: 0x04001EE0 RID: 7904
		public const int DynamicCount = 1;

		// Token: 0x02000C06 RID: 3078
		internal class FixedFieldInfos
		{
			// Token: 0x04003408 RID: 13320
			public const uint Id_Offset = 0U;

			// Token: 0x04003409 RID: 13321
			public const int Id_Size = 4;

			// Token: 0x0400340A RID: 13322
			public const uint Location_Offset = 4U;

			// Token: 0x0400340B RID: 13323
			public const int Location_Size = 4;

			// Token: 0x0400340C RID: 13324
			public const uint Level_Offset = 8U;

			// Token: 0x0400340D RID: 13325
			public const int Level_Size = 1;

			// Token: 0x0400340E RID: 13326
			public const uint Durability_Offset = 9U;

			// Token: 0x0400340F RID: 13327
			public const int Durability_Size = 2;

			// Token: 0x04003410 RID: 13328
			public const uint Resources_Offset = 11U;

			// Token: 0x04003411 RID: 13329
			public const int Resources_Size = 32;

			// Token: 0x04003412 RID: 13330
			public const uint SkeletonCharId_Offset = 43U;

			// Token: 0x04003413 RID: 13331
			public const int SkeletonCharId_Size = 4;
		}
	}
}
