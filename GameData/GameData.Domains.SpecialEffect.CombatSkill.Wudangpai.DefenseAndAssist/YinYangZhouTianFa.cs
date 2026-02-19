using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.DefenseAndAssist;

public class YinYangZhouTianFa : AssistSkillBase
{
	private const sbyte ChangeQiDisorderUnit = 50;

	private const int ChangeInnerRatioPercent = 20;

	private bool _affected;

	private readonly List<DataUid> _dataUids = new List<DataUid>();

	public YinYangZhouTianFa()
	{
	}

	public YinYangZhouTianFa(CombatSkillKey skillKey)
		: base(skillKey, 4603)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
		_dataUids.Add(ParseCharDataUid(21));
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		foreach (int num in characterList)
		{
			if (num >= 0)
			{
				_dataUids.Add(ParseCharDataUid(num, 21));
			}
		}
		foreach (DataUid dataUid in _dataUids)
		{
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, OnQiDisorderChanged);
		}
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		foreach (DataUid dataUid in _dataUids)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(dataUid, base.DataHandlerKey);
		}
	}

	public override void OnDataAdded(DataContext context)
	{
		AppendAffectedAllEnemyData(context, 203, (EDataModifyType)0, -1);
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		UpdateAffected(context);
	}

	private void OnQiDisorderChanged(DataContext context, DataUid uid)
	{
		UpdateAffected(context);
	}

	private void UpdateAffected(DataContext context)
	{
		bool flag = base.IsCurrent && CharObj.GetDisorderOfQi() < base.CurrEnemyChar.GetCharacter().GetDisorderOfQi();
		if (flag != _affected)
		{
			_affected = flag;
			SetConstAffecting(context, _affected);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CurrEnemyChar.GetId(), 203);
			if (_affected)
			{
				ShowSpecialEffectTips(1);
			}
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && power >= 100 && base.CanAffect)
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
			if (combatSkillItem.EquipType == 1)
			{
				DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.IsDirect ? base.CombatChar : base.CurrEnemyChar, 50 * combatSkillItem.GridCost * ((!base.IsDirect) ? 1 : (-1)));
				ShowSpecialEffectTips(0);
				ShowEffectTips(context);
			}
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (!base.CanAffect || dataKey.FieldId != 203)
		{
			return 0;
		}
		if (!DomainManager.Character.TryGetElement_Objects(dataKey.CharId, out var element))
		{
			return 0;
		}
		if (CharObj.GetDisorderOfQi() >= element.GetDisorderOfQi())
		{
			return 0;
		}
		sbyte currInnerRatio = base.SkillInstance.GetCurrInnerRatio();
		int num = (base.IsDirect ? currInnerRatio : (100 - currInnerRatio)) * 20 / 100;
		return base.IsDirect ? num : (-num);
	}
}
