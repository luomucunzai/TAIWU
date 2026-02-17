using System;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006B7 RID: 1719
	[SerializableGameData(NotForArchive = true)]
	public struct CombatReserveData : ISerializableGameData
	{
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x0600660D RID: 26125 RVA: 0x003AA2C0 File Offset: 0x003A84C0
		public static CombatReserveData Invalid
		{
			get
			{
				return new CombatReserveData
				{
					_internalType = 0
				};
			}
		}

		// Token: 0x0600660E RID: 26126 RVA: 0x003AA2E0 File Offset: 0x003A84E0
		public static CombatReserveData CreateSkill(short skillId)
		{
			bool flag = skillId < 0;
			CombatReserveData result;
			if (flag)
			{
				result = CombatReserveData.Invalid;
			}
			else
			{
				result = new CombatReserveData
				{
					_internalType = 1,
					_internalValue0 = (long)skillId
				};
			}
			return result;
		}

		// Token: 0x0600660F RID: 26127 RVA: 0x003AA31C File Offset: 0x003A851C
		public static CombatReserveData CreateChangeTrick(bool valid)
		{
			bool flag = !valid;
			CombatReserveData result;
			if (flag)
			{
				result = CombatReserveData.Invalid;
			}
			else
			{
				result = new CombatReserveData
				{
					_internalType = 2
				};
			}
			return result;
		}

		// Token: 0x06006610 RID: 26128 RVA: 0x003AA350 File Offset: 0x003A8550
		public static CombatReserveData CreateChangeWeapon(int weaponIndex)
		{
			bool flag = weaponIndex < 0;
			CombatReserveData result;
			if (flag)
			{
				result = CombatReserveData.Invalid;
			}
			else
			{
				result = new CombatReserveData
				{
					_internalType = 3,
					_internalValue0 = (long)weaponIndex
				};
			}
			return result;
		}

		// Token: 0x06006611 RID: 26129 RVA: 0x003AA38C File Offset: 0x003A858C
		public static CombatReserveData CreateUnlockAttack(int weaponIndex)
		{
			bool flag = weaponIndex < 0;
			CombatReserveData result;
			if (flag)
			{
				result = CombatReserveData.Invalid;
			}
			else
			{
				result = new CombatReserveData
				{
					_internalType = 4,
					_internalValue0 = (long)weaponIndex
				};
			}
			return result;
		}

		// Token: 0x06006612 RID: 26130 RVA: 0x003AA3C8 File Offset: 0x003A85C8
		public static CombatReserveData CreateUseItem(ItemKey itemKey)
		{
			bool flag = !itemKey.IsValid();
			CombatReserveData result;
			if (flag)
			{
				result = CombatReserveData.Invalid;
			}
			else
			{
				result = new CombatReserveData
				{
					_internalType = 5,
					_internalValue0 = (long)((ulong)itemKey)
				};
			}
			return result;
		}

		// Token: 0x06006613 RID: 26131 RVA: 0x003AA410 File Offset: 0x003A8610
		public static CombatReserveData CreateOtherAction(sbyte otherActionType)
		{
			bool flag = otherActionType < 0;
			CombatReserveData result;
			if (flag)
			{
				result = CombatReserveData.Invalid;
			}
			else
			{
				result = new CombatReserveData
				{
					_internalType = 6,
					_internalValue0 = (long)otherActionType
				};
			}
			return result;
		}

		// Token: 0x06006614 RID: 26132 RVA: 0x003AA44C File Offset: 0x003A864C
		public static CombatReserveData CreateTeammateCommand(int teammateCharId, int teammateCmdIndex)
		{
			bool flag = teammateCharId < 0 || teammateCmdIndex < 0;
			CombatReserveData result;
			if (flag)
			{
				result = CombatReserveData.Invalid;
			}
			else
			{
				result = new CombatReserveData
				{
					_internalType = 7,
					_internalValue0 = ((long)teammateCharId << 32 | (long)teammateCmdIndex)
				};
			}
			return result;
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06006615 RID: 26133 RVA: 0x003AA495 File Offset: 0x003A8695
		public ECombatReserveType Type
		{
			get
			{
				return (ECombatReserveType)this._internalType;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06006616 RID: 26134 RVA: 0x003AA49D File Offset: 0x003A869D
		public bool AnyReserve
		{
			get
			{
				return this.Type > ECombatReserveType.Invalid;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06006617 RID: 26135 RVA: 0x003AA4A8 File Offset: 0x003A86A8
		public short NeedUseSkillId
		{
			get
			{
				return (this.Type == ECombatReserveType.Skill) ? ((short)this._internalValue0) : -1;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06006618 RID: 26136 RVA: 0x003AA4BD File Offset: 0x003A86BD
		public bool NeedShowChangeTrick
		{
			get
			{
				return this.Type == ECombatReserveType.ChangeTrick;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06006619 RID: 26137 RVA: 0x003AA4C8 File Offset: 0x003A86C8
		public int NeedChangeWeaponIndex
		{
			get
			{
				return (this.Type == ECombatReserveType.ChangeWeapon) ? ((int)this._internalValue0) : -1;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x0600661A RID: 26138 RVA: 0x003AA4DD File Offset: 0x003A86DD
		public int NeedUnlockWeaponIndex
		{
			get
			{
				return (this.Type == ECombatReserveType.UnlockAttack) ? ((int)this._internalValue0) : -1;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600661B RID: 26139 RVA: 0x003AA4F2 File Offset: 0x003A86F2
		public ItemKey NeedUseItem
		{
			get
			{
				return (this.Type == ECombatReserveType.UseItem) ? ((ItemKey)((ulong)this._internalValue0)) : ItemKey.Invalid;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600661C RID: 26140 RVA: 0x003AA50F File Offset: 0x003A870F
		public sbyte NeedUseOtherAction
		{
			get
			{
				return (this.Type == ECombatReserveType.OtherAction) ? ((sbyte)this._internalValue0) : -1;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600661D RID: 26141 RVA: 0x003AA524 File Offset: 0x003A8724
		public int TeammateCharId
		{
			get
			{
				return (this.Type == ECombatReserveType.TeammateCommand) ? ((int)(this._internalValue0 >> 32)) : -1;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600661E RID: 26142 RVA: 0x003AA53C File Offset: 0x003A873C
		public int TeammateCmdIndex
		{
			get
			{
				return (this.Type == ECombatReserveType.TeammateCommand) ? ((int)this._internalValue0) : -1;
			}
		}

		// Token: 0x0600661F RID: 26143 RVA: 0x003AA551 File Offset: 0x003A8751
		public CombatReserveData(CombatReserveData other)
		{
			this._internalType = other._internalType;
			this._internalValue0 = other._internalValue0;
		}

		// Token: 0x06006620 RID: 26144 RVA: 0x003AA56C File Offset: 0x003A876C
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06006621 RID: 26145 RVA: 0x003AA580 File Offset: 0x003A8780
		public int GetSerializedSize()
		{
			int totalSize = 9;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006622 RID: 26146 RVA: 0x003AA5A8 File Offset: 0x003A87A8
		public unsafe int Serialize(byte* pData)
		{
			*pData = (byte)this._internalType;
			byte* pCurrData = pData + 1;
			*(long*)pCurrData = this._internalValue0;
			pCurrData += 8;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006623 RID: 26147 RVA: 0x003AA5EC File Offset: 0x003A87EC
		public unsafe int Deserialize(byte* pData)
		{
			this._internalType = *(sbyte*)pData;
			byte* pCurrData = pData + 1;
			this._internalValue0 = *(long*)pCurrData;
			pCurrData += 8;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001BA2 RID: 7074
		[SerializableGameDataField]
		private sbyte _internalType;

		// Token: 0x04001BA3 RID: 7075
		[SerializableGameDataField]
		private long _internalValue0;
	}
}
