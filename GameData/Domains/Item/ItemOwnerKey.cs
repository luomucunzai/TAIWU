using System;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Item
{
	// Token: 0x02000674 RID: 1652
	public struct ItemOwnerKey : IEquatable<ItemOwnerKey>
	{
		// Token: 0x06005315 RID: 21269 RVA: 0x002D07A9 File Offset: 0x002CE9A9
		public ItemOwnerKey(ItemOwnerType ownerType, int ownerId)
		{
			this.OwnerType = ownerType;
			this.OwnerId = ownerId;
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x002D07BC File Offset: 0x002CE9BC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
			defaultInterpolatedStringHandler.AppendLiteral("{");
			defaultInterpolatedStringHandler.AppendFormatted<ItemOwnerType>(this.OwnerType);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.OwnerId);
			defaultInterpolatedStringHandler.AppendLiteral("}");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x002D0824 File Offset: 0x002CEA24
		public bool Equals(ItemOwnerKey other)
		{
			return this.OwnerType == other.OwnerType && this.OwnerId == other.OwnerId;
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x002D0858 File Offset: 0x002CEA58
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is ItemOwnerKey)
			{
				ItemOwnerKey other = (ItemOwnerKey)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x002D0884 File Offset: 0x002CEA84
		public override int GetHashCode()
		{
			return (int)this.OwnerType * 397 ^ this.OwnerId;
		}

		// Token: 0x0400161E RID: 5662
		public static readonly ItemOwnerKey None = default(ItemOwnerKey);

		// Token: 0x0400161F RID: 5663
		public ItemOwnerType OwnerType;

		// Token: 0x04001620 RID: 5664
		public int OwnerId;
	}
}
