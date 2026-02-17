using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Neigong
{
	// Token: 0x02000221 RID: 545
	public class SanDuWuMingZhou : CombatSkillEffectBase
	{
		// Token: 0x06002F36 RID: 12086 RVA: 0x002122FB File Offset: 0x002104FB
		public SanDuWuMingZhou()
		{
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x00212305 File Offset: 0x00210505
		public SanDuWuMingZhou(CombatSkillKey skillKey) : base(skillKey, 15003, -1)
		{
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x00212318 File Offset: 0x00210518
		public override void OnEnable(DataContext context)
		{
			this._happinessUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 6U);
			GameDataBridge.AddPostDataModificationHandler(this._happinessUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnHappinessChange));
			this.UpdateAddPower();
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.Add);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x002123B0 File Offset: 0x002105B0
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._happinessUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x002123EC File Offset: 0x002105EC
		private void OnCombatBegin(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (!flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._happinessUid, base.DataHandlerKey);
				this._happinessUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 136U);
				GameDataBridge.AddPostDataModificationHandler(this._happinessUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnHappinessChange));
			}
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x00212460 File Offset: 0x00210660
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			bool flag = this._happinessUid.DomainId != 8;
			if (!flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._happinessUid, base.DataHandlerKey);
				this._happinessUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 6U);
				GameDataBridge.AddPostDataModificationHandler(this._happinessUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnHappinessChange));
				this.OnHappinessChange(context, this._happinessUid);
			}
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x002124D8 File Offset: 0x002106D8
		private void OnHappinessChange(DataContext context, DataUid dataUid)
		{
			this.UpdateAddPower();
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x002124FC File Offset: 0x002106FC
		private void UpdateAddPower()
		{
			sbyte happinessType = DomainManager.Combat.IsCharInCombat(base.CharacterId, true) ? HappinessType.GetHappinessType(base.CombatChar.GetHappiness()) : this.CharObj.GetHappinessType();
			sbyte happinessThreshold = base.IsDirect ? 4 : 2;
			bool flag = happinessType >= 0 && (base.IsDirect ? (happinessType >= happinessThreshold) : (happinessType <= happinessThreshold));
			if (flag)
			{
				this._addPower = SanDuWuMingZhou.AddPower[(int)(base.IsDirect ? (happinessType - happinessThreshold) : (happinessThreshold - happinessType))];
			}
			else
			{
				this._addPower = 0;
			}
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x00212590 File Offset: 0x00210790
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
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
					result = (int)this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x002125D8 File Offset: 0x002107D8
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1;
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x002125F4 File Offset: 0x002107F4
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this._addPower;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x00212620 File Offset: 0x00210820
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this._addPower = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04000E03 RID: 3587
		private static readonly sbyte[] AddPower = new sbyte[]
		{
			5,
			10,
			15
		};

		// Token: 0x04000E04 RID: 3588
		private sbyte _addPower;

		// Token: 0x04000E05 RID: 3589
		private DataUid _happinessUid;
	}
}
