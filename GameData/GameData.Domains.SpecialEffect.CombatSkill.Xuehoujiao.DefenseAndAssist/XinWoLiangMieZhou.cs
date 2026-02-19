using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist;

public class XinWoLiangMieZhou : AssistSkillBase
{
	private const sbyte ChangeNeiliAllocationPercent = 60;

	private const sbyte AffectedDefeatMarkThreshold = 50;

	private readonly List<DataUid> _teammateDefeatMarkUid = new List<DataUid>();

	private readonly List<int> _ignoreTeammateCharIds = new List<int>();

	public XinWoLiangMieZhou()
	{
	}

	public XinWoLiangMieZhou(CombatSkillKey skillKey)
		: base(skillKey, 15805)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		foreach (DataUid item in _teammateDefeatMarkUid)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(item, base.DataHandlerKey);
		}
		base.OnDisable(context);
	}

	private void OnCombatBegin(DataContext context)
	{
		if (!DomainManager.Combat.IsMainCharacter(base.CombatChar))
		{
			return;
		}
		int[] characterList = DomainManager.Combat.GetCharacterList(base.CombatChar.IsAlly);
		int[] array = characterList;
		foreach (int num in array)
		{
			if (num != base.CharacterId && num >= 0 && DomainManager.Combat.TryGetElement_CombatCharacterDict(num, out var element))
			{
				DataUid dataUid = new DataUid(8, 10, (ulong)num, 50u);
				GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, OnDefeatMarkChanged);
				_teammateDefeatMarkUid.Add(dataUid);
				if (TeammateReachAffectedMarkCount(element))
				{
					_ignoreTeammateCharIds.Add(num);
				}
			}
		}
	}

	private unsafe void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
	{
		int num = (int)dataUid.SubId0;
		if (!DomainManager.Combat.TryGetElement_CombatCharacterDict(num, out var element) || element.IsAlly != base.CombatChar.IsAlly || element == base.CombatChar || !base.CanAffect)
		{
			return;
		}
		if (!TeammateReachAffectedMarkCount(element))
		{
			_ignoreTeammateCharIds.Remove(num);
		}
		else
		{
			if (_ignoreTeammateCharIds.Contains(num))
			{
				return;
			}
			_ignoreTeammateCharIds.Add(num);
			NeiliAllocation neiliAllocation = element.GetNeiliAllocation();
			CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly));
			for (byte b = 0; b < 4; b++)
			{
				int num2 = neiliAllocation.Items[(int)b] * 60 / 100;
				if (num2 > 0)
				{
					combatCharacter.ChangeNeiliAllocation(context, b, num2 * (base.IsDirect ? 1 : (-1)));
				}
			}
			ShowEffectTips(context);
			ShowSpecialEffectTips(0);
		}
	}

	private bool TeammateReachAffectedMarkCount(CombatCharacter combatChar)
	{
		return combatChar.GetDefeatMarkCollection().GetTotalCount() >= GlobalConfig.NeedDefeatMarkCount[DomainManager.Combat.GetCombatType()] * 50 / 100;
	}
}
