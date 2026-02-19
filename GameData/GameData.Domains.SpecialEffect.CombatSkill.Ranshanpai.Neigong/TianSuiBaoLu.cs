using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong;

public class TianSuiBaoLu : CombatSkillEffectBase
{
	private static readonly short[] DirectWords = new short[5] { 375, 376, 377, 378, 379 };

	private static readonly short[] ReverseWords = new short[5] { 380, 381, 382, 383, 384 };

	private const int RemoveTrickCount = 6;

	private const int WordCount = 3;

	private List<ItemKeyAndCount> _wordItemKeys = new List<ItemKeyAndCount>();

	public TianSuiBaoLu()
	{
	}

	public TianSuiBaoLu(CombatSkillKey skillKey)
		: base(skillKey, 7008, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_UsedCustomItem(OnUsedCustomItem);
		Events.RegisterHandler_CombatSettlement(OnCombatSettlement);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_UsedCustomItem(OnUsedCustomItem);
		Events.UnRegisterHandler_CombatSettlement(OnCombatSettlement);
		base.OnDisable(context);
	}

	private void OnCombatBegin(DataContext context)
	{
		if (_wordItemKeys == null)
		{
			_wordItemKeys = new List<ItemKeyAndCount>();
		}
		_wordItemKeys.Clear();
		short[] array = (base.IsDirect ? DirectWords : ReverseWords);
		short[] array2 = array;
		foreach (short templateId in array2)
		{
			_wordItemKeys.Add((itemKey: DomainManager.Item.CreateMisc(context, templateId), count: 3));
		}
		AppendAffectedData(context, 325, (EDataModifyType)3, -1);
		ShowSpecialEffectTips(0);
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.CombatShowTianSuiBaoLu, base.IsDirect, base.CharacterId);
	}

	private void OnUsedCustomItem(DataContext context, int charId, ItemKey itemKey)
	{
		if (charId != base.CharacterId || itemKey.ItemType != 12)
		{
			return;
		}
		int num = _wordItemKeys.FindIndex((ItemKeyAndCount x) => x.ItemKey == itemKey);
		if (num >= 0)
		{
			_wordItemKeys[num] = (itemKey: _wordItemKeys[num].ItemKey, count: _wordItemKeys[num].Count - 1);
			if (_wordItemKeys[num].Count <= 0)
			{
				DomainManager.Item.RemoveItem(context, itemKey);
				_wordItemKeys.RemoveAt(num);
			}
			InvalidateCache(context, 325);
			short templateId = itemKey.TemplateId;
			if ((templateId == 375 || templateId == 380) ? true : false)
			{
				DoAffectShenJian(context);
				return;
			}
			if ((templateId == 376 || templateId == 381) ? true : false)
			{
				DoAffectHuanShe(context);
				return;
			}
			if ((templateId == 377 || templateId == 382) ? true : false)
			{
				DoAffectFuCang(context);
				return;
			}
			if ((templateId == 378 || templateId == 383) ? true : false)
			{
				DoAffectYinShen(context);
				return;
			}
			if ((templateId == 379 || templateId == 384) ? true : false)
			{
				DoAffectQuLiuWu(context);
				return;
			}
			AdaptableLog.Warning($"Unexpected template id {templateId}");
		}
	}

	private void OnCombatSettlement(DataContext context, sbyte combatStatus)
	{
		foreach (var (itemKey2, _) in _wordItemKeys)
		{
			DomainManager.Item.RemoveItem(context, itemKey2);
		}
		_wordItemKeys.Clear();
	}

	private void DoAffectShenJian(DataContext context)
	{
		ShowSpecialEffectTips(1);
		if (base.IsDirect)
		{
			DomainManager.Combat.AddTrick(context, base.CombatChar, base.CombatChar.GetWeaponTricks());
			return;
		}
		CombatCharacter enemyChar = base.EnemyChar;
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.AddRange(enemyChar.GetTricks().Tricks.Values.Where(enemyChar.IsTrickUsable));
		foreach (sbyte item in RandomUtils.GetRandomUnrepeated(context.Random, 6, list))
		{
			DomainManager.Combat.RemoveTrick(context, enemyChar, item, 1, removedByAlly: false);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	private void DoAffectHuanShe(DataContext context)
	{
		ShowSpecialEffectTips(2);
		if (base.IsDirect)
		{
			ChangeBreathValue(context, base.CombatChar, base.CombatChar.GetMaxBreathValue());
			ChangeStanceValue(context, base.CombatChar, base.CombatChar.GetMaxStanceValue());
		}
		else
		{
			CombatCharacter enemyChar = base.EnemyChar;
			ChangeBreathValue(context, enemyChar, -enemyChar.GetMaxBreathValue());
			ChangeStanceValue(context, enemyChar, -enemyChar.GetMaxStanceValue());
		}
	}

	private void DoAffectFuCang(DataContext context)
	{
		ShowSpecialEffectTips(3);
		if (base.IsDirect)
		{
			ChangeMobilityValue(context, base.CombatChar, base.CombatChar.GetMaxMobility());
		}
		else
		{
			ChangeMobilityValue(context, base.EnemyChar, -base.EnemyChar.GetMaxMobility());
		}
	}

	private void DoAffectYinShen(DataContext context)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		ShowSpecialEffectTips(4);
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : base.EnemyChar);
		NeiliAllocation neiliAllocation = combatCharacter.GetNeiliAllocation();
		NeiliAllocation originNeiliAllocation = combatCharacter.GetOriginNeiliAllocation();
		for (byte b = 0; b < 4; b++)
		{
			short num = neiliAllocation[b];
			short num2 = originNeiliAllocation[b];
			if (!(base.IsDirect ? (num >= num2) : (num <= num2)))
			{
				int addValue = (base.IsDirect ? num : (-((int)num * CValueHalf.RoundDown)));
				combatCharacter.ChangeNeiliAllocation(context, b, addValue, applySpecialEffect: false);
			}
		}
	}

	private void DoAffectQuLiuWu(DataContext context)
	{
		ShowSpecialEffectTips(5);
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : base.EnemyChar);
		foreach (short bannedSkillId in combatCharacter.GetBannedSkillIds(requireNotInfinity: true))
		{
			if (base.IsDirect)
			{
				DomainManager.Combat.ClearSkillCd(context, combatCharacter, bannedSkillId);
			}
			else
			{
				DomainManager.Combat.DoubleSkillCd(context, combatCharacter, bannedSkillId);
			}
		}
	}

	public override List<ItemKeyAndCount> GetModifiedValue(AffectedDataKey dataKey, List<ItemKeyAndCount> dataValue)
	{
		if (dataKey.CharId == base.CharacterId && dataKey.FieldId == 325)
		{
			foreach (ItemKeyAndCount wordItemKey in _wordItemKeys)
			{
				dataValue.Add(wordItemKey);
			}
		}
		return base.GetModifiedValue(dataKey, dataValue);
	}
}
