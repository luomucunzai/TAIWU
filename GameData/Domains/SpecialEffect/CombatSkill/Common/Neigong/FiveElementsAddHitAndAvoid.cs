using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x02000573 RID: 1395
	public class FiveElementsAddHitAndAvoid : CombatSkillEffectBase
	{
		// Token: 0x0600412E RID: 16686 RVA: 0x00261AA6 File Offset: 0x0025FCA6
		protected FiveElementsAddHitAndAvoid()
		{
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x00261AB0 File Offset: 0x0025FCB0
		protected FiveElementsAddHitAndAvoid(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x00261AC0 File Offset: 0x0025FCC0
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 38, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 39, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 40, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 41, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 32, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 33, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 34, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 35, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			this._neiliPortionUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 111U);
			GameDataBridge.AddPostDataModificationHandler(this._neiliPortionUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnNeiliProportionOfFiveElementsChanged));
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x00261C0D File Offset: 0x0025FE0D
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._neiliPortionUid, base.DataHandlerKey);
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x00261C24 File Offset: 0x0025FE24
		private void OnNeiliProportionOfFiveElementsChanged(DataContext context, DataUid dataUid)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 38);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 39);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 40);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 41);
			}
			else
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 32);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 33);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 34);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 35);
			}
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x00261CE4 File Offset: 0x0025FEE4
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

		// Token: 0x06004134 RID: 16692 RVA: 0x00261D34 File Offset: 0x0025FF34
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1 + 1;
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x00261D50 File Offset: 0x0025FF50
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this.RequireFiveElementsType;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x00261D7C File Offset: 0x0025FF7C
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this.RequireFiveElementsType = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x0400132F RID: 4911
		private const sbyte AddValueRatio = 5;

		// Token: 0x04001330 RID: 4912
		protected sbyte RequireFiveElementsType;

		// Token: 0x04001331 RID: 4913
		private DataUid _neiliPortionUid;
	}
}
