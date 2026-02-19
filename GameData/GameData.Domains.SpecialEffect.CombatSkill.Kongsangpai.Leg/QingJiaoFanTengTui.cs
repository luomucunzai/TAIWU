using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg;

public class QingJiaoFanTengTui : CombatSkillEffectBase
{
	private const sbyte MoveDistInPrepare = 30;

	private const int AffectDistanceUnit = 2;

	private const int ReduceFlawOrAcupointKeepTimePercent = 10;

	private short _movedDist;

	private Injuries _lastInjuries;

	private Injuries _lastOldInjuries;

	private Injuries _addedInjuries;

	private Injuries _addedOldInjuries;

	private DataUid _injuryUid;

	private DataUid _oldInjuryUid;

	private readonly List<(sbyte part, bool inner)> _injuryRandomPool = new List<(sbyte, bool)>();

	private readonly Dictionary<short, (short power, bool reverse)> _lastDebuffState = new Dictionary<short, (short, bool)>();

	private readonly List<short> _addedDebuffState = new List<short>();

	private DataUid _debuffStateUid;

	public QingJiaoFanTengTui()
	{
	}

	public QingJiaoFanTengTui(CombatSkillKey skillKey)
		: base(skillKey, 10305, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 30, base.IsDirect);
		_lastInjuries = base.CombatChar.GetInjuries();
		_lastOldInjuries = base.CombatChar.GetOldInjuries();
		Dictionary<short, (short, bool, int)> stateDict = base.CombatChar.GetDebuffCombatStateCollection().StateDict;
		_lastDebuffState.Clear();
		foreach (KeyValuePair<short, (short, bool, int)> item in stateDict)
		{
			_lastDebuffState.Add(item.Key, (item.Value.Item1, item.Value.Item2));
		}
		_injuryUid = new DataUid(8, 10, (ulong)base.CharacterId, 29u);
		_oldInjuryUid = new DataUid(8, 10, (ulong)base.CharacterId, 30u);
		_debuffStateUid = new DataUid(8, 10, (ulong)base.CharacterId, 77u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_injuryUid, base.DataHandlerKey, OnInjuryChanged);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_oldInjuryUid, base.DataHandlerKey, OnOldInjuryChanged);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_debuffStateUid, base.DataHandlerKey, OnDebuffStateChanged);
		Events.RegisterHandler_DistanceChanged(OnDistanceChanged);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_injuryUid, base.DataHandlerKey);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_oldInjuryUid, base.DataHandlerKey);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_debuffStateUid, base.DataHandlerKey);
		Events.UnRegisterHandler_DistanceChanged(OnDistanceChanged);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnInjuryChanged(DataContext context, DataUid dataUid)
	{
		Injuries injuries = base.CombatChar.GetInjuries();
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			(sbyte, sbyte) tuple2 = _lastInjuries.Get(b);
			if (tuple.Item1 > tuple2.Item1)
			{
				_addedInjuries.Change(b, isInnerInjury: false, (sbyte)(tuple.Item1 - tuple2.Item1));
			}
			else if (_addedInjuries.Get(b, isInnerInjury: false) > tuple.Item1)
			{
				_addedInjuries.Change(b, isInnerInjury: false, (sbyte)(tuple.Item1 - _addedInjuries.Get(b, isInnerInjury: false)));
			}
			if (tuple.Item1 > tuple2.Item2)
			{
				_addedInjuries.Change(b, isInnerInjury: true, (sbyte)(tuple.Item2 - tuple2.Item2));
			}
			else if (_addedInjuries.Get(b, isInnerInjury: true) > tuple.Item2)
			{
				_addedInjuries.Change(b, isInnerInjury: true, (sbyte)(tuple.Item2 - _addedInjuries.Get(b, isInnerInjury: true)));
			}
		}
		_lastInjuries = injuries;
	}

	private void OnOldInjuryChanged(DataContext context, DataUid dataUid)
	{
		Injuries injuries = base.CombatChar.GetInjuries();
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			(sbyte, sbyte) tuple2 = _lastOldInjuries.Get(b);
			if (tuple.Item1 > tuple2.Item1)
			{
				_addedInjuries.Change(b, isInnerInjury: false, (sbyte)(tuple.Item1 - tuple2.Item1));
			}
			else if (_addedInjuries.Get(b, isInnerInjury: false) > tuple.Item1)
			{
				_addedInjuries.Change(b, isInnerInjury: false, (sbyte)(tuple.Item1 - _addedInjuries.Get(b, isInnerInjury: false)));
			}
			if (tuple.Item1 > tuple2.Item2)
			{
				_addedInjuries.Change(b, isInnerInjury: true, (sbyte)(tuple.Item2 - tuple2.Item2));
			}
			else if (_addedInjuries.Get(b, isInnerInjury: true) > tuple.Item2)
			{
				_addedInjuries.Change(b, isInnerInjury: true, (sbyte)(tuple.Item2 - _addedInjuries.Get(b, isInnerInjury: true)));
			}
		}
		_lastOldInjuries = injuries;
	}

	private void OnDebuffStateChanged(DataContext context, DataUid dataUid)
	{
		Dictionary<short, (short, bool, int)> stateDict = base.CombatChar.GetDebuffCombatStateCollection().StateDict;
		List<short> stateIdList = ObjectPool<List<short>>.Instance.Get();
		stateIdList.Clear();
		stateIdList.AddRange(stateDict.Keys);
		_addedDebuffState.RemoveAll((short id) => !stateIdList.Contains(id));
		foreach (short item in stateIdList)
		{
			if (!_lastDebuffState.ContainsKey(item) || Math.Abs(_lastDebuffState[item].power) < Math.Abs(stateDict[item].Item1))
			{
				_addedDebuffState.Add(item);
			}
		}
		_lastDebuffState.Clear();
		foreach (KeyValuePair<short, (short, bool, int)> item2 in stateDict)
		{
			_lastDebuffState.Add(item2.Key, (item2.Value.Item1, item2.Value.Item2));
		}
		ObjectPool<List<short>>.Instance.Return(stateIdList);
	}

	private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
	{
		if (mover.GetId() != base.CharacterId || !isMove)
		{
			return;
		}
		_movedDist += Math.Abs(distance);
		while (_movedDist >= 2)
		{
			_injuryRandomPool.Clear();
			for (sbyte b = 0; b < 7; b++)
			{
				(sbyte, sbyte) tuple = _addedInjuries.Get(b);
				(sbyte, sbyte) tuple2 = _addedOldInjuries.Get(b);
				for (int i = 0; i < tuple.Item1 - tuple2.Item1; i++)
				{
					_injuryRandomPool.Add((b, false));
				}
				for (int j = 0; j < tuple.Item2 - tuple2.Item2; j++)
				{
					_injuryRandomPool.Add((b, true));
				}
			}
			if (_injuryRandomPool.Count > 0)
			{
				Injuries injuries = base.CombatChar.GetInjuries();
				(sbyte, bool) tuple3 = _injuryRandomPool[context.Random.Next(0, _injuryRandomPool.Count)];
				injuries.Change(tuple3.Item1, tuple3.Item2, -1);
				DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries);
				if (_addedInjuries.Get(tuple3.Item1, tuple3.Item2) > 0)
				{
					_addedInjuries.Change(tuple3.Item1, tuple3.Item2, -1);
				}
				ShowSpecialEffectTips(0);
			}
			if (_addedDebuffState.Count > 0)
			{
				DomainManager.Combat.RemoveCombatState(context, base.CombatChar, 2, _addedDebuffState[context.Random.Next(0, _addedDebuffState.Count)]);
				ShowSpecialEffectTips(1);
			}
			if (base.CombatChar.GetFlawCollection().GetTotalCount() > 0 || base.CombatChar.GetAcupointCollection().GetTotalCount() > 0)
			{
				DomainManager.Combat.ReduceFlawKeepTimePercent(context, base.CombatChar, 10);
				DomainManager.Combat.ReduceAcupointKeepTimePercent(context, base.CombatChar, 10);
				ShowSpecialEffectTips(2);
			}
			_movedDist -= 2;
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			RemoveSelf(context);
		}
	}
}
