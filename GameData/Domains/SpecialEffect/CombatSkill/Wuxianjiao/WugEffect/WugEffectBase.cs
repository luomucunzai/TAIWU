using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x0200037B RID: 891
	public class WugEffectBase : SpecialEffectBase
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060035B6 RID: 13750 RVA: 0x0022D97F File Offset: 0x0022BB7F
		protected unsafe bool CanAffect
		{
			get
			{
				return this.WugConfig.WugGrowthType != 5 || (*this.CharObj.GetEatingItems()).IndexOfWug(this.WugConfig.WugType, false) < 0;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060035B7 RID: 13751 RVA: 0x0022D9B6 File Offset: 0x0022BBB6
		protected bool IsElite
		{
			get
			{
				return this.WugConfig.WugGrowthType != 5 && this.CharObj.GetEatingItems().ContainsWugKing(this.WugConfig.WugType);
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060035B8 RID: 13752 RVA: 0x0022D9E4 File Offset: 0x0022BBE4
		protected bool IsGood
		{
			get
			{
				return WugGrowthType.IsGood(this.WugConfig.WugGrowthType);
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060035B9 RID: 13753 RVA: 0x0022D9F6 File Offset: 0x0022BBF6
		protected bool IsBad
		{
			get
			{
				return WugGrowthType.IsBad(this.WugConfig.WugGrowthType);
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060035BA RID: 13754 RVA: 0x0022DA08 File Offset: 0x0022BC08
		protected bool IsGrown
		{
			get
			{
				sbyte wugGrowthType = this.WugConfig.WugGrowthType;
				return wugGrowthType - 4 <= 1;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060035BB RID: 13755 RVA: 0x0022DA30 File Offset: 0x0022BC30
		protected bool CanChangeToGrown
		{
			get
			{
				return WugGrowthType.CanChangeToGrown(this.WugConfig.WugGrowthType);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060035BC RID: 13756 RVA: 0x0022DA42 File Offset: 0x0022BC42
		public short WugTemplateId
		{
			get
			{
				return this._wugTemplateId;
			}
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x0022DA4A File Offset: 0x0022BC4A
		protected WugEffectBase()
		{
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x0022DA54 File Offset: 0x0022BC54
		protected WugEffectBase(int charId, int type, short wugTemplateId, short effectId) : base(charId, type)
		{
			this._wugTemplateId = wugTemplateId;
			this._effectTemplateId = effectId;
			this.WugConfig = Config.Medicine.Instance[wugTemplateId];
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x0022DA80 File Offset: 0x0022BC80
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_RemoveWug(new Events.OnRemoveWug(this.OnRemoveWug));
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x0022DAA8 File Offset: 0x0022BCA8
		public override void OnDataAdded(DataContext context)
		{
			bool flag = DomainManager.Combat.Started && this.CheckValid();
			if (flag)
			{
				this.OnCombatBegin(context);
			}
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x0022DAD8 File Offset: 0x0022BCD8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_RemoveWug(new Events.OnRemoveWug(this.OnRemoveWug));
			bool flag = DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (flag)
			{
				Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
				this.ClearAffectDataAndEvent(context);
			}
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x0022DB3B File Offset: 0x0022BD3B
		public virtual void OnEffectAdded(DataContext context, short replacedWug)
		{
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x0022DB40 File Offset: 0x0022BD40
		private void OnCombatBegin(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (!flag)
			{
				Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
				this.AddAffectDataAndEvent(context);
			}
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x0022DB84 File Offset: 0x0022BD84
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			bool flag = WugGrowthType.IsWugGrowthTypeCombatOnly(this.WugConfig.WugGrowthType);
			if (flag)
			{
				this.CharObj.AddWug(context, ItemDomain.GetWugTemplateId(this.WugConfig.WugType, this.WugConfig.WugGrowthType + 1));
			}
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			this.ClearAffectDataAndEvent(context);
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x0022DBEC File Offset: 0x0022BDEC
		private void OnRemoveWug(DataContext context, int charId, short wugTemplateId)
		{
			bool flag = charId != base.CharacterId || wugTemplateId != this.WugConfig.TemplateId;
			if (!flag)
			{
				DomainManager.SpecialEffect.Remove(context, this.Id);
			}
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x0022DC2F File Offset: 0x0022BE2F
		protected virtual void AddAffectDataAndEvent(DataContext context)
		{
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x0022DC32 File Offset: 0x0022BE32
		protected virtual void ClearAffectDataAndEvent(DataContext context)
		{
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x0022DC38 File Offset: 0x0022BE38
		protected void CostWugInCombat(DataContext context)
		{
			bool flag = this.CostWugCount <= 0 || !WugGrowthType.IsWugGrowthTypeCombatOnly(this.WugConfig.WugGrowthType);
			if (!flag)
			{
				int costCount = (int)this.CostWugCount;
				int selfWugCount = (int)base.CombatChar.GetWugCount();
				int enemyWugCount = (int)base.CurrEnemyChar.GetWugCount();
				bool flag2 = costCount > 0 && selfWugCount > 0;
				if (flag2)
				{
					int cost = Math.Min(costCount, selfWugCount);
					costCount -= cost;
					base.CombatChar.ChangeWugCount(context, -cost);
				}
				bool flag3 = costCount > 0 && enemyWugCount > 0;
				if (flag3)
				{
					int cost2 = Math.Min(costCount, enemyWugCount);
					costCount -= cost2;
					base.CurrEnemyChar.ChangeWugCount(context, -cost2);
				}
				bool flag4 = costCount > 0;
				if (flag4)
				{
					this.CharObj.RemoveWug(context, this.WugConfig.TemplateId);
				}
			}
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x0022DD11 File Offset: 0x0022BF11
		protected void ShowEffectTips(DataContext context, byte index = 1)
		{
			DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, (int)this._effectTemplateId, new ItemKey(8, byte.MaxValue, this.WugConfig.TemplateId, -1), index);
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x0022DD44 File Offset: 0x0022BF44
		protected virtual void ChangeToGrown(DataContext context)
		{
			short grownTemplateId = ItemDomain.GetWugTemplateId(this.WugConfig.WugType, 4);
			this.CharObj.AddWug(context, grownTemplateId);
			LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
			sbyte wugType = this.WugConfig.WugType;
			if (!true)
			{
			}
			WugEffectBase.LifeRecordGrownAddTemplate lifeRecordGrownAddTemplate;
			switch (wugType)
			{
			case 0:
				lifeRecordGrownAddTemplate = new WugEffectBase.LifeRecordGrownAddTemplate(lifeRecord.AddWugRedEyeChangeToGrown);
				goto IL_C0;
			case 1:
				lifeRecordGrownAddTemplate = new WugEffectBase.LifeRecordGrownAddTemplate(lifeRecord.AddWugForestSpiritChangeToGrown);
				goto IL_C0;
			case 2:
				lifeRecordGrownAddTemplate = new WugEffectBase.LifeRecordGrownAddTemplate(lifeRecord.AddWugBlackBloodChangeToGrown);
				goto IL_C0;
			case 3:
				lifeRecordGrownAddTemplate = new WugEffectBase.LifeRecordGrownAddTemplate(lifeRecord.AddWugDevilInsideChangeToGrown);
				goto IL_C0;
			case 5:
				lifeRecordGrownAddTemplate = new WugEffectBase.LifeRecordGrownAddTemplate(lifeRecord.AddWugIceSilkwormChangeToGrown);
				goto IL_C0;
			case 6:
				lifeRecordGrownAddTemplate = new WugEffectBase.LifeRecordGrownAddTemplate(lifeRecord.AddWugGoldenSilkwormChangeToGrown);
				goto IL_C0;
			}
			lifeRecordGrownAddTemplate = null;
			IL_C0:
			if (!true)
			{
			}
			WugEffectBase.LifeRecordGrownAddTemplate template = lifeRecordGrownAddTemplate;
			bool flag = template != null;
			if (flag)
			{
				this.AddLifeRecord(template, grownTemplateId);
			}
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x0022DE2A File Offset: 0x0022C02A
		protected unsafe bool CheckValid()
		{
			return (*this.CharObj.GetEatingItems()).IndexOfWug(this.WugConfig.TemplateId) >= 0;
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x0022DE54 File Offset: 0x0022C054
		public static sbyte GetNpcCombatAddGrowthType(GameData.Domains.Character.Character addChar, sbyte wugType, bool direct)
		{
			if (!true)
			{
			}
			sbyte result;
			switch (wugType)
			{
			case 1:
				if (ForestSpiritBase.CanGrown(addChar))
				{
					result = 4;
					goto IL_59;
				}
				break;
			case 3:
				if (DevilInsideBase.CanGrown(addChar))
				{
					result = 4;
					goto IL_59;
				}
				break;
			case 5:
				if (IceSilkwormBase.CanGrown(addChar))
				{
					result = 4;
					goto IL_59;
				}
				break;
			}
			result = (direct ? 3 : 1);
			IL_59:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x0022DEC4 File Offset: 0x0022C0C4
		protected void AddLifeRecord(WugEffectBase.LifeRecordAddTemplate template)
		{
			int currDate = DomainManager.World.GetCurrDate();
			template(base.CharacterId, currDate, this.WugConfig.ItemType, this.WugConfig.TemplateId);
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x0022DF04 File Offset: 0x0022C104
		protected void AddLifeRecord<T0>(WugEffectBase.LifeRecordAddTemplate<T0> template, T0 arg0)
		{
			int currDate = DomainManager.World.GetCurrDate();
			template(base.CharacterId, currDate, this.WugConfig.ItemType, this.WugConfig.TemplateId, arg0);
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x0022DF44 File Offset: 0x0022C144
		protected void AddLifeRecord<T0, T1>(WugEffectBase.LifeRecordAddTemplate<T0, T1> template, T0 arg0, T1 arg1)
		{
			int currDate = DomainManager.World.GetCurrDate();
			template(base.CharacterId, currDate, this.WugConfig.ItemType, this.WugConfig.TemplateId, arg0, arg1);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x0022DF84 File Offset: 0x0022C184
		protected void AddLifeRecord<T0, T1, T2>(WugEffectBase.LifeRecordAddTemplate<T0, T1, T2> template, T0 arg0, T1 arg1, T2 arg2)
		{
			int currDate = DomainManager.World.GetCurrDate();
			template(base.CharacterId, currDate, this.WugConfig.ItemType, this.WugConfig.TemplateId, arg0, arg1, arg2);
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x0022DFC8 File Offset: 0x0022C1C8
		protected void AddLifeRecord(WugEffectBase.LifeRecordGrownAddTemplate template, short newTemplateId)
		{
			int currDate = DomainManager.World.GetCurrDate();
			template(base.CharacterId, currDate, this.WugConfig.ItemType, this.WugConfig.TemplateId, 8, newTemplateId);
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x0022E008 File Offset: 0x0022C208
		protected void AddLifeRecord(WugEffectBase.LifeRecordRelatedAddTemplate template, int charId, short newTemplateId)
		{
			int currDate = DomainManager.World.GetCurrDate();
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			Location location = this.CharObj.GetLocation().IsValid() ? this.CharObj.GetLocation() : character.GetLocation();
			template(base.CharacterId, currDate, charId, location, this.WugConfig.ItemType, this.WugConfig.TemplateId, 8, newTemplateId);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x0022E080 File Offset: 0x0022C280
		protected void AddLifeRecord(WugEffectBase.LifeRecordRelatedAddTemplate template, int srcCharId, int dstCharId, short newTemplateId)
		{
			int currDate = DomainManager.World.GetCurrDate();
			GameData.Domains.Character.Character srcChar = DomainManager.Character.GetElement_Objects(srcCharId);
			GameData.Domains.Character.Character dstChar = DomainManager.Character.GetElement_Objects(dstCharId);
			Location location = srcChar.GetLocation().IsValid() ? srcChar.GetLocation() : dstChar.GetLocation();
			template(srcCharId, currDate, dstCharId, location, this.WugConfig.ItemType, this.WugConfig.TemplateId, 8, newTemplateId);
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x0022E0F8 File Offset: 0x0022C2F8
		protected override int GetSubClassSerializedSize()
		{
			return 4;
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x0022E10C File Offset: 0x0022C30C
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			*(short*)pData = this._wugTemplateId;
			byte* pCurrData = pData + 2;
			*(short*)pCurrData = this._effectTemplateId;
			pCurrData += 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x0022E140 File Offset: 0x0022C340
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			this._wugTemplateId = *(short*)pData;
			byte* pCurrData = pData + 2;
			this._effectTemplateId = *(short*)pCurrData;
			pCurrData += 2;
			this.WugConfig = Config.Medicine.Instance[this._wugTemplateId];
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04000FAE RID: 4014
		private short _wugTemplateId;

		// Token: 0x04000FAF RID: 4015
		private short _effectTemplateId;

		// Token: 0x04000FB0 RID: 4016
		protected MedicineItem WugConfig;

		// Token: 0x04000FB1 RID: 4017
		protected sbyte CostWugCount;

		// Token: 0x02000A2B RID: 2603
		// (Invoke) Token: 0x060086FB RID: 34555
		public delegate void LifeRecordAddTemplate(int charId, int currDate, sbyte itemType, short templateId);

		// Token: 0x02000A2C RID: 2604
		// (Invoke) Token: 0x060086FF RID: 34559
		public delegate void LifeRecordAddTemplate<in T0>(int charId, int currDate, sbyte itemType, short templateId, T0 arg0);

		// Token: 0x02000A2D RID: 2605
		// (Invoke) Token: 0x06008703 RID: 34563
		public delegate void LifeRecordAddTemplate<in T0, in T1>(int charId, int currDate, sbyte itemType, short templateId, T0 arg0, T1 arg1);

		// Token: 0x02000A2E RID: 2606
		// (Invoke) Token: 0x06008707 RID: 34567
		public delegate void LifeRecordAddTemplate<in T0, in T1, in T2>(int charId, int currDate, sbyte itemType, short templateId, T0 arg0, T1 arg1, T2 arg2);

		// Token: 0x02000A2F RID: 2607
		// (Invoke) Token: 0x0600870B RID: 34571
		public delegate void LifeRecordGrownAddTemplate(int charId, int currDate, sbyte itemType, short templateId, sbyte newItemType, short newTemplateId);

		// Token: 0x02000A30 RID: 2608
		// (Invoke) Token: 0x0600870F RID: 34575
		public delegate void LifeRecordRelatedAddTemplate(int srcCharId, int currDate, int dstCharId, Location location, sbyte itemType, short templateId, sbyte newItemType, short newTemplateId);
	}
}
