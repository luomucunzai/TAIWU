using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000342 RID: 834
	public class AddWug : CombatSkillEffectBase
	{
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060034DC RID: 13532 RVA: 0x0022A445 File Offset: 0x00228645
		protected virtual int AddWugCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x0022A448 File Offset: 0x00228648
		protected AddWug()
		{
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x0022A45D File Offset: 0x0022865D
		protected AddWug(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x0022A478 File Offset: 0x00228678
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			base.CreateAffectedData(235, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCostNeiliConfirm(new Events.OnCombatCostNeiliConfirm(this.OnCombatCostNeiliConfirm));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x0022A4E2 File Offset: 0x002286E2
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatCostNeiliConfirm(new Events.OnCombatCostNeiliConfirm(this.OnCombatCostNeiliConfirm));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x0022A51C File Offset: 0x0022871C
		private void OnCombatBegin(DataContext context)
		{
			bool flag = this.AddWugCount <= 0;
			if (!flag)
			{
				base.CombatChar.ChangeWugCount(context, this.AddWugCount);
			}
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x0022A550 File Offset: 0x00228750
		private void OnCombatCostNeiliConfirm(DataContext context, int charId, short skillId, short effectId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || (int)effectId != base.EffectId;
			if (!flag)
			{
				ItemKey itemKey = this.CharObj.GetInventory().GetInventoryItemKey(8, this.GetWugKingTemplateId());
				bool flag2 = !itemKey.IsValid();
				if (!flag2)
				{
					this.CharObj.RemoveInventoryItem(context, itemKey, 1, false, false);
					this._usingWugKing = itemKey;
					base.InvalidateCache(context, 199);
				}
			}
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x0022A5D0 File Offset: 0x002287D0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter affectChar = base.IsDirect ? base.CurrEnemyChar : base.CombatChar;
					bool flag3 = DomainManager.Combat.AddWug(context, affectChar, this.WugType, base.IsDirect, base.CharacterId);
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
					}
					bool flag4 = this._usingWugKing.IsValid();
					if (flag4)
					{
						DomainManager.Combat.AddWugIrresistibly(context, affectChar, this._usingWugKing);
						DomainManager.Combat.ShowWugKingEffectTips(context, base.CombatChar.GetId(), affectChar.GetId());
						this._usingWugKing = ItemKey.Invalid;
					}
				}
				bool flag5 = this._usingWugKing.IsValid();
				if (flag5)
				{
					this.CharObj.AddInventoryItem(context, this._usingWugKing, 1, false);
					this._usingWugKing = ItemKey.Invalid;
				}
				base.InvalidateCache(context, 199);
			}
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x0022A6E4 File Offset: 0x002288E4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 199 || !this._usingWugKing.IsValid();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 40;
			}
			return result;
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x0022A734 File Offset: 0x00228934
		public override List<CastBoostEffectDisplayData> GetModifiedValue(AffectedDataKey dataKey, List<CastBoostEffectDisplayData> dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 235 || this._usingWugKing.IsValid();
			List<CastBoostEffectDisplayData> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				Inventory inventory = this.CharObj.GetInventory();
				int count = inventory.GetInventoryItemCount(8, this.GetWugKingTemplateId());
				bool flag2 = count > 0;
				if (flag2)
				{
					dataValue.Add(CastBoostEffectDisplayData.CostWugKing(this.SkillKey, this.GetWugKingTemplateId(), count));
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x0022A7BC File Offset: 0x002289BC
		private short GetWugKingTemplateId()
		{
			foreach (WugKingItem wugKing in ((IEnumerable<WugKingItem>)WugKing.Instance))
			{
				bool flag = wugKing.WugFinger == base.SkillTemplateId;
				if (flag)
				{
					return wugKing.WugMedicine;
				}
			}
			short predefinedLogId = 7;
			object arg = this.Id;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
			defaultInterpolatedStringHandler.AppendLiteral("get wug king by unexpected skill ");
			defaultInterpolatedStringHandler.AppendFormatted<CombatSkillKey>(this.SkillKey);
			PredefinedLog.Show(predefinedLogId, arg, defaultInterpolatedStringHandler.ToStringAndClear());
			return -1;
		}

		// Token: 0x04000F8F RID: 3983
		private const int AddPowerValue = 40;

		// Token: 0x04000F90 RID: 3984
		protected sbyte WugType;

		// Token: 0x04000F91 RID: 3985
		private ItemKey _usingWugKing = ItemKey.Invalid;
	}
}
