using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000374 RID: 884
	public class RedEyeBase : WugEffectBase
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600359D RID: 13725 RVA: 0x0022D308 File Offset: 0x0022B508
		private int ChangeHitOdds
		{
			get
			{
				return (!base.IsGrown && base.IsElite) ? 75 : 50;
			}
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x0022D320 File Offset: 0x0022B520
		protected RedEyeBase()
		{
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x0022D35C File Offset: 0x0022B55C
		protected RedEyeBase(int charId, int type, short wugTemplateId, short effectId) : base(charId, type, wugTemplateId, effectId)
		{
			this.CostWugCount = 1;
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x0022D3AC File Offset: 0x0022B5AC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			if (this.AffectDatas == null)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			}
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 260, -1, -1, -1, -1), EDataModifyType.Custom);
			}
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				Events.RegisterHandler_EatingItem(new Events.OnEatingItem(this.OnEatingItem));
			}
			bool isGrown2 = base.IsGrown;
			if (isGrown2)
			{
				Events.RegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x0022D438 File Offset: 0x0022B638
		public override void OnDisable(DataContext context)
		{
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				Events.UnRegisterHandler_EatingItem(new Events.OnEatingItem(this.OnEatingItem));
			}
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				Events.UnRegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
			base.OnDisable(context);
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x0022D488 File Offset: 0x0022B688
		protected override void AddAffectDataAndEvent(DataContext context)
		{
			this._affected = true;
			base.AppendAffectedData(context, base.CharacterId, 74, EDataModifyType.TotalPercent, -1);
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			bool flag = WugGrowthType.IsWugGrowthTypeCombatOnly(this.WugConfig.WugGrowthType);
			if (flag)
			{
				Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			}
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x0022D4E8 File Offset: 0x0022B6E8
		protected override void ClearAffectDataAndEvent(DataContext context)
		{
			base.RemoveAffectedData(context, base.CharacterId, 74);
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			bool flag = WugGrowthType.IsWugGrowthTypeCombatOnly(this.WugConfig.WugGrowthType);
			if (flag)
			{
				Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			}
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x0022D540 File Offset: 0x0022B740
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = attacker != base.CombatChar || pursueIndex > 0 || !DomainManager.Combat.InAttackRange(base.CombatChar) || !base.CanAffect;
			if (!flag)
			{
				base.ShowEffectTips(context, 1);
				bool isElite = base.IsElite;
				if (isElite)
				{
					base.ShowEffectTips(context, 2);
				}
			}
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x0022D59C File Offset: 0x0022B79C
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this._affected || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this._affected = false;
				base.CostWugInCombat(context);
			}
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x0022D5DC File Offset: 0x0022B7DC
		private void OnEatingItem(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey)
		{
			bool flag = character.GetId() != base.CharacterId || itemKey.ItemType != 9 || !base.CanAffect;
			if (!flag)
			{
				bool flag2 = Config.TeaWine.Instance[itemKey.TemplateId].ItemSubType == 901;
				if (flag2)
				{
					this.ChangeToGrown(context);
				}
			}
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x0022D63C File Offset: 0x0022B83C
		private void OnAdvanceMonthFinish(DataContext context)
		{
			bool flag = !base.IsGrown || !base.IsElite || !base.CanAffect;
			if (!flag)
			{
				bool isTaiwu = base.CharacterId == DomainManager.Taiwu.GetTaiwuCharId();
				Inventory inventory = this.CharObj.GetInventory();
				CharacterDomain.ClassifyItemsToLose(this._classifiedItems, inventory.Items, isTaiwu);
				CharacterDomain.GetLostItemsByAmount(context.Random, 3, this._classifiedItems, this._lostItems);
				Location location = this.CharObj.GetLocation();
				bool flag2 = !location.IsValid();
				if (flag2)
				{
					location = this.CharObj.GetValidLocation();
				}
				LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
				List<MapBlockData> nearbyBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, nearbyBlocks, 2, true);
				foreach (ValueTuple<ItemBase, int> valueTuple in this._lostItems)
				{
					ItemBase item = valueTuple.Item1;
					int amount = valueTuple.Item2;
					ItemKey itemKey = item.GetItemKey();
					this.CharObj.RemoveInventoryItem(context, itemKey, amount, false, false);
					MapBlockData block = nearbyBlocks.GetRandom(context.Random);
					DomainManager.Map.AddBlockItem(context, block, itemKey, amount);
					base.AddLifeRecord<Location, sbyte, short>(new WugEffectBase.LifeRecordAddTemplate<Location, sbyte, short>(lifeRecord.AddWugKingRedEyeLoseItem), block.GetLocation(), itemKey.ItemType, itemKey.TemplateId);
				}
				this._lostItems.Clear();
				ObjectPool<List<MapBlockData>>.Instance.Return(nearbyBlocks);
			}
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x0022D7EC File Offset: 0x0022B9EC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId >= 0 || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 74;
				if (flag2)
				{
					this._affected = true;
					result = (base.IsGood ? this.ChangeHitOdds : (-this.ChangeHitOdds));
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x0022D85C File Offset: 0x0022BA5C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId == base.CharacterId && dataKey.FieldId == 260 && base.CanAffect;
			return !flag && dataValue;
		}

		// Token: 0x04000FAA RID: 4010
		private const int LostItemCount = 3;

		// Token: 0x04000FAB RID: 4011
		private bool _affected;

		// Token: 0x04000FAC RID: 4012
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		private readonly List<ValueTuple<ItemBase, int>>[] _classifiedItems = new List<ValueTuple<ItemBase, int>>[]
		{
			new List<ValueTuple<ItemBase, int>>(),
			new List<ValueTuple<ItemBase, int>>(),
			new List<ValueTuple<ItemBase, int>>()
		};

		// Token: 0x04000FAD RID: 4013
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		private readonly List<ValueTuple<ItemBase, int>> _lostItems = new List<ValueTuple<ItemBase, int>>();
	}
}
