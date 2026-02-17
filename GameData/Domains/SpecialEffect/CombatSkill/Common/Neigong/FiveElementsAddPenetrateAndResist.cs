using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x02000574 RID: 1396
	public class FiveElementsAddPenetrateAndResist : CombatSkillEffectBase
	{
		// Token: 0x06004137 RID: 16695 RVA: 0x00261DA6 File Offset: 0x0025FFA6
		protected FiveElementsAddPenetrateAndResist()
		{
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x00261DB0 File Offset: 0x0025FFB0
		protected FiveElementsAddPenetrateAndResist(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x00261DC0 File Offset: 0x0025FFC0
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 46, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 47, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 44, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 45, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			this._neiliPortionUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 111U);
			GameDataBridge.AddPostDataModificationHandler(this._neiliPortionUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnNeiliProportionOfFiveElementsChanged));
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x00261E95 File Offset: 0x00260095
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._neiliPortionUid, base.DataHandlerKey);
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x00261EAC File Offset: 0x002600AC
		private void OnNeiliProportionOfFiveElementsChanged(DataContext context, DataUid dataUid)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 46);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 47);
			}
			else
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 44);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 45);
			}
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x00261F1C File Offset: 0x0026011C
		public unsafe override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte neiliPercent = *(ref this.CharObj.GetNeiliProportionOfFiveElements().Items.FixedElementField + this.RequireFiveElementsType);
				result = (int)(neiliPercent / 5);
			}
			return result;
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x00261F6C File Offset: 0x0026016C
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1 + 1;
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x00261F88 File Offset: 0x00260188
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this.RequireFiveElementsType;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x00261FB4 File Offset: 0x002601B4
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this.RequireFiveElementsType = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001332 RID: 4914
		private const sbyte AddValueRatio = 5;

		// Token: 0x04001333 RID: 4915
		protected sbyte RequireFiveElementsType;

		// Token: 0x04001334 RID: 4916
		private DataUid _neiliPortionUid;
	}
}
