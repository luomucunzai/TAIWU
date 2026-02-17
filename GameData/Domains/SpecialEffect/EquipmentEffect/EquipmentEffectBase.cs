using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect
{
	// Token: 0x02000180 RID: 384
	public class EquipmentEffectBase : SpecialEffectBase
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06002B75 RID: 11125 RVA: 0x00205594 File Offset: 0x00203794
		protected short Durability
		{
			get
			{
				return DomainManager.Item.GetBaseItem(this.EquipItemKey).GetCurrDurability();
			}
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x002055AB File Offset: 0x002037AB
		protected EquipmentEffectBase()
		{
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x002055B5 File Offset: 0x002037B5
		protected EquipmentEffectBase(int charId, ItemKey itemKey, int type) : base(charId, type)
		{
			this.EquipItemKey = itemKey;
			this.AutoRemoveAfterCombat = true;
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x002055CF File Offset: 0x002037CF
		protected EquipmentEffectBase(int charId, ItemKey itemKey, int type, bool autoRemoveAfterCombat) : base(charId, type)
		{
			this.EquipItemKey = itemKey;
			this.AutoRemoveAfterCombat = autoRemoveAfterCombat;
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x002055EC File Offset: 0x002037EC
		protected bool IsCurrWeapon()
		{
			return DomainManager.Combat.GetUsingWeaponKey(base.CombatChar).Equals(this.EquipItemKey);
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x0020561C File Offset: 0x0020381C
		protected bool IsCurrArmor(sbyte bodyPart)
		{
			bool flag = bodyPart < 0;
			return !flag && base.CombatChar.Armors[(int)bodyPart].Equals(this.EquipItemKey) && this.Durability > 0;
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x00205664 File Offset: 0x00203864
		protected override int GetSubClassSerializedSize()
		{
			return this.EquipItemKey.GetSerializedSize();
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x00205684 File Offset: 0x00203884
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + this.EquipItemKey.Serialize(pData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x002056B0 File Offset: 0x002038B0
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + this.EquipItemKey.Deserialize(pData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04000D42 RID: 3394
		public ItemKey EquipItemKey;

		// Token: 0x04000D43 RID: 3395
		public readonly bool AutoRemoveAfterCombat;
	}
}
