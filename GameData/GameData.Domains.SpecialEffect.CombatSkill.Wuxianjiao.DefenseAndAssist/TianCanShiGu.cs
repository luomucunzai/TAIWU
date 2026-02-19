using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist;

public class TianCanShiGu : AssistSkillBase
{
	private const short EffectRequireFrame = 300;

	private const short ChangeNeiliAllocationValue = 3;

	private bool _isCurrCombatChar;

	private DataUid _eatingItemsUid;

	private readonly Dictionary<short, int> _wugFrameDict = new Dictionary<short, int>();

	public TianCanShiGu()
	{
	}

	public TianCanShiGu(CombatSkillKey skillKey)
		: base(skillKey, 12806)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_eatingItemsUid, base.DataHandlerKey);
		Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		if (_isCurrCombatChar)
		{
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
		}
	}

	private void OnCombatBegin(DataContext context)
	{
		_isCurrCombatChar = DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
		if (_isCurrCombatChar)
		{
			Events.RegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
		}
		_eatingItemsUid = new DataUid(4, 0, (ulong)(base.IsDirect ? base.CurrEnemyChar.GetId() : base.CharacterId), 59u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_eatingItemsUid, base.DataHandlerKey, OnUpdateEatingItems);
		OnUpdateEatingItems(context, default(DataUid));
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		if (isAlly == base.CombatChar.IsAlly)
		{
			bool flag = DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
			if (_isCurrCombatChar != flag)
			{
				_isCurrCombatChar = flag;
				if (flag)
				{
					Events.RegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
				}
				else
				{
					Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
				}
			}
		}
		else if (base.IsDirect)
		{
			UpdateEnemyUid(context);
		}
	}

	private unsafe void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		if (base.CombatChar != combatChar || DomainManager.Combat.Pause)
		{
			return;
		}
		bool flag = false;
		foreach (short key in _wugFrameDict.Keys)
		{
			int num = _wugFrameDict[key] + 1;
			if (num >= 300)
			{
				num = 0;
				if (base.CanAffect)
				{
					CombatCharacter combatCharacter = (base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly) : base.CombatChar);
					if (base.IsDirect)
					{
						NeiliAllocation neiliAllocation = combatCharacter.GetNeiliAllocation();
						List<byte> list = ObjectPool<List<byte>>.Instance.Get();
						list.Clear();
						for (byte b = 0; b < 4; b++)
						{
							if (neiliAllocation.Items[(int)b] > 0)
							{
								list.Add(b);
							}
						}
						if (list.Count > 0)
						{
							combatCharacter.ChangeNeiliAllocation(context, list[context.Random.Next(0, list.Count)], -3);
							flag = true;
						}
						ObjectPool<List<byte>>.Instance.Return(list);
					}
					else
					{
						combatCharacter.ChangeNeiliAllocation(context, (byte)context.Random.Next(0, 4), 3);
						flag = true;
					}
				}
			}
			_wugFrameDict[key] = num;
		}
		if (flag)
		{
			ShowEffectTips(context);
			ShowSpecialEffectTips(0);
		}
	}

	private unsafe void OnUpdateEatingItems(DataContext context, DataUid dataUid)
	{
		GameData.Domains.Character.Character character = (base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly).GetCharacter() : CharObj);
		EatingItems eatingItems = character.GetEatingItems();
		List<short> wugList = ObjectPool<List<short>>.Instance.Get();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		wugList.Clear();
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[i];
			if (EatingItems.IsWug(itemKey))
			{
				wugList.Add(itemKey.TemplateId);
			}
		}
		list.Clear();
		list.AddRange(_wugFrameDict.Keys);
		list.RemoveAll((short id) => wugList.Contains(id));
		for (int num = 0; num < list.Count; num++)
		{
			_wugFrameDict.Remove(list[num]);
		}
		for (int num2 = 0; num2 < wugList.Count; num2++)
		{
			short key = wugList[num2];
			if (!_wugFrameDict.ContainsKey(key))
			{
				_wugFrameDict.Add(key, 0);
			}
		}
		ObjectPool<List<short>>.Instance.Return(wugList);
		ObjectPool<List<short>>.Instance.Return(list);
	}

	private void UpdateEnemyUid(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_eatingItemsUid, base.DataHandlerKey);
		_eatingItemsUid = new DataUid(4, 0, (ulong)DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly).GetId(), 59u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_eatingItemsUid, base.DataHandlerKey, OnUpdateEatingItems);
		_wugFrameDict.Clear();
		OnUpdateEatingItems(context, default(DataUid));
	}
}
