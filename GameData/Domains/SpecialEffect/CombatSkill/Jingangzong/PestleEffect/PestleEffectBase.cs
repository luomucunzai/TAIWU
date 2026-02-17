using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect
{
	// Token: 0x020004B2 RID: 1202
	public class PestleEffectBase : SpecialEffectBase
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x0024F296 File Offset: 0x0024D496
		protected bool IsDirect
		{
			get
			{
				return this._weaponData.GetPestleEffect().IsDirect;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06003CD2 RID: 15570 RVA: 0x0024F2A8 File Offset: 0x0024D4A8
		protected bool CanAffect
		{
			get
			{
				return DomainManager.Combat.GetUsingWeaponKey(base.CombatChar).Equals(this._weaponKey);
			}
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x0024F2D3 File Offset: 0x0024D4D3
		protected PestleEffectBase()
		{
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x0024F2DD File Offset: 0x0024D4DD
		protected PestleEffectBase(int charId, int type) : base(charId, type)
		{
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x0024F2EC File Offset: 0x0024D4EC
		public override void OnEnable(DataContext context)
		{
			this._weaponKey = DomainManager.Combat.GetUsingWeaponKey(base.CombatChar);
			this._weaponDurabilityUid = new DataUid(8, 30, (ulong)this._weaponKey, 3U);
			this._weaponData = DomainManager.Combat.GetElement_WeaponDataDict(this._weaponKey.Id);
			GameDataBridge.AddPostDataModificationHandler(this._weaponDurabilityUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDurabilityChanged));
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x0024F363 File Offset: 0x0024D563
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._weaponDurabilityUid, base.DataHandlerKey);
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x0024F378 File Offset: 0x0024D578
		private void OnDurabilityChanged(DataContext context, DataUid dataUid)
		{
			bool flag = this._weaponData.GetDurability() <= 0;
			if (flag)
			{
				this._weaponData.RemovePestleEffect(context);
			}
		}

		// Token: 0x040011E6 RID: 4582
		private ItemKey _weaponKey;

		// Token: 0x040011E7 RID: 4583
		private DataUid _weaponDurabilityUid;

		// Token: 0x040011E8 RID: 4584
		private CombatWeaponData _weaponData;
	}
}
