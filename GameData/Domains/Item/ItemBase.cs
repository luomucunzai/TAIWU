using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x0200065D RID: 1629
	public abstract class ItemBase : BaseGameDataObject
	{
		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06004E53 RID: 20051 RVA: 0x002B062E File Offset: 0x002AE82E
		public TemplateKey TemplateKey
		{
			get
			{
				return new TemplateKey(this.GetItemType(), this.TemplateId);
			}
		}

		// Token: 0x06004E54 RID: 20052
		[CollectionObjectField(true, false, false, false, false)]
		public abstract string GetName();

		// Token: 0x06004E55 RID: 20053
		[CollectionObjectField(true, false, false, false, false)]
		public abstract sbyte GetItemType();

		// Token: 0x06004E56 RID: 20054
		[CollectionObjectField(true, false, false, false, false)]
		public abstract short GetItemSubType();

		// Token: 0x06004E57 RID: 20055
		[CollectionObjectField(true, false, false, false, false)]
		public abstract sbyte GetGrade();

		// Token: 0x06004E58 RID: 20056
		[CollectionObjectField(true, false, false, false, false)]
		public abstract string GetIcon();

		// Token: 0x06004E59 RID: 20057
		[CollectionObjectField(true, false, false, false, false)]
		public abstract string GetDesc();

		// Token: 0x06004E5A RID: 20058
		[CollectionObjectField(true, false, false, false, false)]
		public abstract bool GetTransferable();

		// Token: 0x06004E5B RID: 20059
		[CollectionObjectField(true, false, false, false, false)]
		public abstract bool GetStackable();

		// Token: 0x06004E5C RID: 20060
		[CollectionObjectField(true, false, false, false, false)]
		public abstract bool GetWagerable();

		// Token: 0x06004E5D RID: 20061
		[CollectionObjectField(true, false, false, false, false)]
		public abstract bool GetRefinable();

		// Token: 0x06004E5E RID: 20062
		[CollectionObjectField(true, false, false, false, false)]
		public abstract bool GetPoisonable();

		// Token: 0x06004E5F RID: 20063
		[CollectionObjectField(true, false, false, false, false)]
		public abstract bool GetRepairable();

		// Token: 0x06004E60 RID: 20064
		[CollectionObjectField(true, false, false, false, false)]
		public abstract int GetBaseWeight();

		// Token: 0x06004E61 RID: 20065
		[CollectionObjectField(true, false, false, false, false)]
		public abstract int GetBaseValue();

		// Token: 0x06004E62 RID: 20066
		[CollectionObjectField(true, false, false, false, false)]
		public abstract sbyte GetBaseHappinessChange();

		// Token: 0x06004E63 RID: 20067
		[CollectionObjectField(true, false, false, false, false)]
		public abstract int GetBaseFavorabilityChange();

		// Token: 0x06004E64 RID: 20068
		[CollectionObjectField(true, false, false, false, false)]
		public abstract sbyte GetDropRate();

		// Token: 0x06004E65 RID: 20069
		[CollectionObjectField(true, false, false, false, false)]
		public abstract sbyte GetResourceType();

		// Token: 0x06004E66 RID: 20070
		[CollectionObjectField(true, false, false, false, false)]
		public abstract short GetPreservationDuration();

		// Token: 0x06004E67 RID: 20071
		public abstract int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type);

		// Token: 0x06004E68 RID: 20072 RVA: 0x002B0644 File Offset: 0x002AE844
		public virtual int GetWeight()
		{
			return this.GetBaseWeight();
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x002B065C File Offset: 0x002AE85C
		public virtual int GetValue()
		{
			int baseValue = this.GetBaseValue();
			return this.ApplyDurabilityEffect(baseValue);
		}

		// Token: 0x06004E6A RID: 20074 RVA: 0x002B067C File Offset: 0x002AE87C
		public virtual int GetFavorabilityChange()
		{
			return this.GetBaseFavorabilityChange();
		}

		// Token: 0x06004E6B RID: 20075 RVA: 0x002B0694 File Offset: 0x002AE894
		public virtual sbyte GetHappinessChange()
		{
			return this.GetBaseHappinessChange();
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x002B06AC File Offset: 0x002AE8AC
		public void OfflineSetId(int id)
		{
			this.Id = id;
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x002B06B8 File Offset: 0x002AE8B8
		public ItemKey GetItemKey()
		{
			return new ItemKey(this.GetItemType(), this.ModificationState, this.TemplateId, this.Id);
		}

		// Token: 0x06004E6E RID: 20078 RVA: 0x002B06E8 File Offset: 0x002AE8E8
		public static short GenerateMaxDurability(IRandomSource random, short value)
		{
			if (!true)
			{
			}
			short result;
			if (value <= 0)
			{
				if (value >= 0)
				{
					result = value;
				}
				else
				{
					result = -value;
				}
			}
			else
			{
				result = (short)Math.Max(random.Next((int)(value / 2), (int)(value + 1)), 1);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x002B0730 File Offset: 0x002AE930
		protected int ApplyDurabilityEffect(int value)
		{
			return value * ItemFormula.FormulaCalcDurabilityEffect((int)this.CurrDurability, (int)this.MaxDurability);
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x002B075C File Offset: 0x002AE95C
		public static void OfflineRepairItem(CraftTool craftTool, ItemBase targetItem, short targetDurability, short durabilityCost)
		{
			Tester.Assert(targetItem.GetRepairable(), "");
			Tester.Assert(targetItem.CurrDurability < targetItem.MaxDurability && targetItem.CurrDurability < targetDurability, "");
			Tester.Assert(targetDurability <= targetItem.MaxDurability, "");
			Tester.Assert(craftTool.CurrDurability >= durabilityCost, "");
			craftTool.CurrDurability -= durabilityCost;
			targetItem.CurrDurability = targetDurability;
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x002B07E4 File Offset: 0x002AE9E4
		public bool IsDurabilityRunningOut()
		{
			return this.GetMaxDurability() != 0 && this.GetCurrDurability() <= 0;
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06004E72 RID: 20082 RVA: 0x002B080D File Offset: 0x002AEA0D
		public ItemOwnerKey Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06004E73 RID: 20083 RVA: 0x002B0815 File Offset: 0x002AEA15
		public ItemOwnerKey PrevOwner
		{
			get
			{
				return this._prevOwner;
			}
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x002B081D File Offset: 0x002AEA1D
		public void ResetOwner()
		{
			this._prevOwner = this._owner;
			this._owner = ItemOwnerKey.None;
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x002B0838 File Offset: 0x002AEA38
		public void RemoveOwner(ItemOwnerType ownerType, int ownerId)
		{
			bool flag = ItemDomain.IsPureStackable(this);
			if (!flag)
			{
				bool flag2 = this._owner.OwnerType != ownerType || this._owner.OwnerId != ownerId;
				if (flag2)
				{
					string itemKeyStr = this.GetItemKey().ToString();
					ItemOwnerKey expectedKey = new ItemOwnerKey(ownerType, ownerId);
					PredefinedLog.Show(21, itemKeyStr, expectedKey.ToString());
					bool flag3 = this._owner.OwnerType > ItemOwnerType.None;
					if (flag3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 2);
						defaultInterpolatedStringHandler.AppendFormatted(itemKeyStr);
						defaultInterpolatedStringHandler.AppendLiteral(" actual owner: ");
						defaultInterpolatedStringHandler.AppendFormatted<ItemOwnerKey>(this._owner);
						AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
				}
				this._prevOwner = this._owner;
				this._owner = ItemOwnerKey.None;
			}
		}

		// Token: 0x06004E76 RID: 20086 RVA: 0x002B091C File Offset: 0x002AEB1C
		public void SetOwner(ItemOwnerType ownerType, int ownerId)
		{
			ItemOwnerKey newOwner = new ItemOwnerKey
			{
				OwnerType = ownerType,
				OwnerId = ownerId
			};
			bool flag = this._owner.OwnerType != ItemOwnerType.None && this._owner.OwnerType != ownerType && this._owner.OwnerId != ownerId;
			if (flag)
			{
				string itemKeyStr = this.GetItemKey().ToString();
				PredefinedLog.Show(20, itemKeyStr, newOwner.ToString(), this._owner.ToString());
				bool flag2 = this._prevOwner.OwnerType > ItemOwnerType.None;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 2);
					defaultInterpolatedStringHandler.AppendFormatted(itemKeyStr);
					defaultInterpolatedStringHandler.AppendLiteral(" previous owner: ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemOwnerKey>(this._prevOwner);
					AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
				}
				this._prevOwner = this._owner;
			}
			bool flag3 = ItemDomain.IsPureStackable(this);
			if (!flag3)
			{
				this._owner = newOwner;
			}
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x002B0A2C File Offset: 0x002AEC2C
		public int GetId()
		{
			return this.Id;
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x002B0A44 File Offset: 0x002AEC44
		public short GetTemplateId()
		{
			return this.TemplateId;
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x002B0A5C File Offset: 0x002AEC5C
		public short GetMaxDurability()
		{
			return this.MaxDurability;
		}

		// Token: 0x06004E7A RID: 20090
		public abstract void SetMaxDurability(short maxDurability, DataContext context);

		// Token: 0x06004E7B RID: 20091 RVA: 0x002B0A74 File Offset: 0x002AEC74
		public short GetCurrDurability()
		{
			return this.CurrDurability;
		}

		// Token: 0x06004E7C RID: 20092
		public abstract void SetCurrDurability(short currDurability, DataContext context);

		// Token: 0x06004E7D RID: 20093 RVA: 0x002B0A8C File Offset: 0x002AEC8C
		public byte GetModificationState()
		{
			return this.ModificationState;
		}

		// Token: 0x06004E7E RID: 20094
		public abstract void SetModificationState(byte modificationState, DataContext context);

		// Token: 0x0400157B RID: 5499
		[CollectionObjectField(false, true, false, true, false)]
		protected int Id;

		// Token: 0x0400157C RID: 5500
		[CollectionObjectField(true, true, false, true, false)]
		protected short TemplateId;

		// Token: 0x0400157D RID: 5501
		[CollectionObjectField(true, true, false, false, false)]
		protected short MaxDurability;

		// Token: 0x0400157E RID: 5502
		[CollectionObjectField(false, true, false, false, false)]
		protected short CurrDurability;

		// Token: 0x0400157F RID: 5503
		[CollectionObjectField(false, true, false, false, false)]
		protected byte ModificationState;

		// Token: 0x04001580 RID: 5504
		private ItemOwnerKey _owner = ItemOwnerKey.None;

		// Token: 0x04001581 RID: 5505
		private ItemOwnerKey _prevOwner = ItemOwnerKey.None;
	}
}
