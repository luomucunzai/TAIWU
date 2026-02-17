using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect
{
	// Token: 0x020004AF RID: 1199
	public class DaWeiDeJinGangChu : PestleEffectBase
	{
		// Token: 0x06003CC0 RID: 15552 RVA: 0x0024ED74 File Offset: 0x0024CF74
		public DaWeiDeJinGangChu()
		{
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x0024ED7E File Offset: 0x0024CF7E
		public DaWeiDeJinGangChu(int charId) : base(charId, 11405)
		{
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x0024ED90 File Offset: 0x0024CF90
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			this._selfNeiliAllocationUid = base.ParseNeiliAllocationDataUid();
			this._enemyNeiliAllocationUid = base.ParseNeiliAllocationDataUid(currEnemy.GetId());
			GameDataBridge.AddPostDataModificationHandler(this._selfNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnNeiliAllocationChanged));
			GameDataBridge.AddPostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnNeiliAllocationChanged));
			this.UpdateAddPower(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x0024EE74 File Offset: 0x0024D074
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._selfNeiliAllocationUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey);
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			base.OnDisable(context);
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x0024EED4 File Offset: 0x0024D0D4
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = charId == base.CharacterId;
			if (flag)
			{
				this.UpdateAddPower(context);
			}
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x0024EEF8 File Offset: 0x0024D0F8
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly != base.CombatChar.IsAlly;
			if (flag)
			{
				CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				GameDataBridge.RemovePostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey);
				this._enemyNeiliAllocationUid = base.ParseNeiliAllocationDataUid(currEnemy.GetId());
				GameDataBridge.AddPostDataModificationHandler(this._enemyNeiliAllocationUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnNeiliAllocationChanged));
				this.UpdateAddPower(context);
			}
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x0024EF81 File Offset: 0x0024D181
		private void OnNeiliAllocationChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateAddPower(context);
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x0024EF8C File Offset: 0x0024D18C
		private unsafe void UpdateAddPower(DataContext context)
		{
			this._addPower = 0;
			bool flag = DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) && base.CanAffect;
			if (flag)
			{
				NeiliAllocation selfNeiliAllocation = base.CombatChar.GetNeiliAllocation();
				NeiliAllocation enemyNeiliAllocation = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetNeiliAllocation();
				for (byte type = 0; type < 4; type += 1)
				{
					bool flag2 = base.IsDirect ? (*(ref selfNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2) > *(ref enemyNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2)) : (*(ref selfNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2) < *(ref enemyNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2));
					if (flag2)
					{
						this._addPower += 10;
					}
				}
			}
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x0024F088 File Offset: 0x0024D288
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040011E2 RID: 4578
		private const sbyte AddPowerUnit = 10;

		// Token: 0x040011E3 RID: 4579
		private DataUid _selfNeiliAllocationUid;

		// Token: 0x040011E4 RID: 4580
		private DataUid _enemyNeiliAllocationUid;

		// Token: 0x040011E5 RID: 4581
		private int _addPower;
	}
}
