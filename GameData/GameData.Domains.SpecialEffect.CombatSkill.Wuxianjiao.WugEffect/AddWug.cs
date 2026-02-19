using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

public class AddWug : CombatSkillEffectBase
{
	private const int AddPowerValue = 40;

	protected sbyte WugType;

	private ItemKey _usingWugKing = ItemKey.Invalid;

	protected virtual int AddWugCount => 0;

	protected AddWug()
	{
	}

	protected AddWug(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(199, (EDataModifyType)1, base.SkillTemplateId);
		CreateAffectedData(235, (EDataModifyType)3, base.SkillTemplateId);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_CombatCostNeiliConfirm(OnCombatCostNeiliConfirm);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_CombatCostNeiliConfirm(OnCombatCostNeiliConfirm);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCombatBegin(DataContext context)
	{
		if (AddWugCount > 0)
		{
			base.CombatChar.ChangeWugCount(context, AddWugCount);
		}
	}

	private void OnCombatCostNeiliConfirm(DataContext context, int charId, short skillId, short effectId)
	{
		if (SkillKey.IsMatch(charId, skillId) && effectId == base.EffectId)
		{
			ItemKey inventoryItemKey = CharObj.GetInventory().GetInventoryItemKey(8, GetWugKingTemplateId());
			if (inventoryItemKey.IsValid())
			{
				CharObj.RemoveInventoryItem(context, inventoryItemKey, 1, deleteItem: false);
				_usingWugKing = inventoryItemKey;
				InvalidateCache(context, 199);
			}
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power))
		{
			CombatCharacter combatCharacter = (base.IsDirect ? base.CurrEnemyChar : base.CombatChar);
			if (DomainManager.Combat.AddWug(context, combatCharacter, WugType, base.IsDirect, base.CharacterId))
			{
				ShowSpecialEffectTips(0);
			}
			if (_usingWugKing.IsValid())
			{
				DomainManager.Combat.AddWugIrresistibly(context, combatCharacter, _usingWugKing);
				DomainManager.Combat.ShowWugKingEffectTips(context, base.CombatChar.GetId(), combatCharacter.GetId());
				_usingWugKing = ItemKey.Invalid;
			}
		}
		if (_usingWugKing.IsValid())
		{
			CharObj.AddInventoryItem(context, _usingWugKing, 1);
			_usingWugKing = ItemKey.Invalid;
		}
		InvalidateCache(context, 199);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.SkillKey != SkillKey || dataKey.FieldId != 199 || !_usingWugKing.IsValid())
		{
			return 0;
		}
		return 40;
	}

	public override List<CastBoostEffectDisplayData> GetModifiedValue(AffectedDataKey dataKey, List<CastBoostEffectDisplayData> dataValue)
	{
		if (dataKey.SkillKey != SkillKey || dataKey.FieldId != 235 || _usingWugKing.IsValid())
		{
			return dataValue;
		}
		Inventory inventory = CharObj.GetInventory();
		int inventoryItemCount = inventory.GetInventoryItemCount(8, GetWugKingTemplateId());
		if (inventoryItemCount > 0)
		{
			dataValue.Add(CastBoostEffectDisplayData.CostWugKing(SkillKey, GetWugKingTemplateId(), inventoryItemCount));
		}
		return dataValue;
	}

	private short GetWugKingTemplateId()
	{
		foreach (WugKingItem item in (IEnumerable<WugKingItem>)WugKing.Instance)
		{
			if (item.WugFinger == base.SkillTemplateId)
			{
				return item.WugMedicine;
			}
		}
		PredefinedLog.Show(7, Id, $"get wug king by unexpected skill {SkillKey}");
		return -1;
	}
}
