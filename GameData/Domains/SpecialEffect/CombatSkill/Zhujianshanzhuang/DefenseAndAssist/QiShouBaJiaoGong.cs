using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.DefenseAndAssist
{
	// Token: 0x020001D5 RID: 469
	public class QiShouBaJiaoGong : AssistSkillBase
	{
		// Token: 0x06002D4E RID: 11598 RVA: 0x0020B246 File Offset: 0x00209446
		public QiShouBaJiaoGong()
		{
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x0020B250 File Offset: 0x00209450
		public QiShouBaJiaoGong(CombatSkillKey skillKey) : base(skillKey, 9700)
		{
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x0020B260 File Offset: 0x00209460
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 11 : 9, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x0020B2C0 File Offset: 0x002094C0
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool flag = this._weaponDurabilityUids != null;
			if (flag)
			{
				for (int i = 0; i < this._weaponDurabilityUids.Count; i++)
				{
					GameDataBridge.RemovePostDataModificationHandler(this._weaponDurabilityUids[i], base.DataHandlerKey);
				}
			}
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x0020B32C File Offset: 0x0020952C
		private void OnCombatBegin(DataContext context)
		{
			this._affecting = false;
			this.UpdateCanAffect(context, default(DataUid));
			this._weaponDurabilityUids = new List<DataUid>();
			ItemKey[] weapons = base.CombatChar.GetWeapons();
			for (int i = 0; i < 3; i++)
			{
				ItemKey weaponKey = weapons[i];
				bool flag = !weaponKey.IsValid();
				if (!flag)
				{
					DataUid dataUid = new DataUid(8, 30, (ulong)weaponKey, 3U);
					GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateCanAffect));
					this._weaponDurabilityUids.Add(dataUid);
				}
			}
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x0020B3D0 File Offset: 0x002095D0
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context, default(DataUid));
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x0020B3F0 File Offset: 0x002095F0
		private void UpdateCanAffect(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = canAffect;
			if (flag)
			{
				ItemKey[] weapons = base.CombatChar.GetWeapons();
				List<short> subTypeList = ObjectPool<List<short>>.Instance.Get();
				subTypeList.Clear();
				for (int i = 0; i < 3; i++)
				{
					ItemKey weaponKey = weapons[i];
					bool flag2 = !weaponKey.IsValid();
					if (!flag2)
					{
						short subType = ItemTemplateHelper.GetItemSubType(weaponKey.ItemType, weaponKey.TemplateId);
						bool flag3 = !subTypeList.Contains(subType);
						if (flag3)
						{
							subTypeList.Add(subType);
						}
					}
				}
				canAffect = (subTypeList.Count >= 3);
				ObjectPool<List<short>>.Instance.Return(subTypeList);
			}
			bool flag4 = this._affecting == canAffect;
			if (!flag4)
			{
				this._affecting = canAffect;
				base.SetConstAffecting(context, canAffect);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 11 : 9);
			}
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x0020B4EC File Offset: 0x002096EC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 30;
			}
			return result;
		}

		// Token: 0x04000D9C RID: 3484
		private const sbyte RequireWeaponTypeCount = 3;

		// Token: 0x04000D9D RID: 3485
		private const sbyte AddSpeed = 30;

		// Token: 0x04000D9E RID: 3486
		private List<DataUid> _weaponDurabilityUids;

		// Token: 0x04000D9F RID: 3487
		private bool _affecting;
	}
}
