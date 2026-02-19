using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg;

public class JiuGongLuanBaBu : CombatSkillEffectBase
{
	private const sbyte MoveDistInPrepare = 30;

	private const int AffectDistanceUnit = 2;

	private short _distanceAccumulator;

	private DefeatMarkCollection _oldMarks;

	private DefeatMarkCollection _lastMarks;

	private DefeatMarkCollection _newMarks;

	private DataUid _defeatMarkUid;

	private readonly List<(sbyte, sbyte)> _typeRandomPool = new List<(sbyte, sbyte)>();

	public JiuGongLuanBaBu()
	{
	}

	public JiuGongLuanBaBu(CombatSkillKey skillKey)
		: base(skillKey, 5103, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		_oldMarks = new DefeatMarkCollection(defeatMarkCollection);
		_lastMarks = new DefeatMarkCollection(defeatMarkCollection);
		_newMarks = new DefeatMarkCollection();
		_defeatMarkUid = new DataUid(8, 10, (ulong)base.CharacterId, 50u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_defeatMarkUid, base.DataHandlerKey, OnDefeatMarkChanged);
		DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 30, base.IsDirect);
		Events.RegisterHandler_DistanceChanged(OnDistanceChanged);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_defeatMarkUid, base.DataHandlerKey);
		Events.UnRegisterHandler_DistanceChanged(OnDistanceChanged);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
	{
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		for (sbyte b = 0; b < 7; b++)
		{
			int count = defeatMarkCollection.FlawMarkList[b].Count;
			int count2 = _lastMarks.FlawMarkList[b].Count;
			int count3 = defeatMarkCollection.AcupointMarkList[b].Count;
			int count4 = _lastMarks.AcupointMarkList[b].Count;
			if (count > count2)
			{
				for (int i = 0; i < count - count2; i++)
				{
					_newMarks.FlawMarkList[b].Add(0);
				}
			}
			else if (count < count2)
			{
				for (int j = 0; j < count2 - count; j++)
				{
					if (_oldMarks.FlawMarkList[b].Count > 0)
					{
						_oldMarks.FlawMarkList[b].RemoveAt(0);
					}
				}
				while (_newMarks.FlawMarkList[b].Count > count)
				{
					_newMarks.FlawMarkList[b].RemoveAt(0);
				}
			}
			_lastMarks.FlawMarkList[b].Clear();
			_lastMarks.FlawMarkList[b].AddRange(defeatMarkCollection.FlawMarkList[b]);
			if (count3 > count4)
			{
				for (int k = 0; k < count3 - count4; k++)
				{
					_newMarks.AcupointMarkList[b].Add(0);
				}
			}
			else if (count3 < count4)
			{
				for (int l = 0; l < count4 - count3; l++)
				{
					if (_oldMarks.AcupointMarkList[b].Count > 0)
					{
						_oldMarks.AcupointMarkList[b].RemoveAt(0);
					}
				}
				while (_newMarks.AcupointMarkList[b].Count > count3)
				{
					_newMarks.AcupointMarkList[b].RemoveAt(0);
				}
			}
			_lastMarks.AcupointMarkList[b].Clear();
			_lastMarks.AcupointMarkList[b].AddRange(defeatMarkCollection.AcupointMarkList[b]);
		}
		int count5 = defeatMarkCollection.MindMarkList.Count;
		int count6 = _lastMarks.MindMarkList.Count;
		if (count5 > count6)
		{
			for (int m = count6; m < count5; m++)
			{
				_newMarks.MindMarkList.Add(defeatMarkCollection.MindMarkList[m]);
			}
		}
		else if (count5 < count6)
		{
			for (int n = 0; n < count6 - count5; n++)
			{
				if (_oldMarks.MindMarkList.Count <= 0)
				{
					break;
				}
				_oldMarks.MindMarkList.RemoveAt(0);
			}
			while (_newMarks.MindMarkList.Count > count5)
			{
				_newMarks.MindMarkList.RemoveAt(0);
			}
		}
		_lastMarks.MindMarkList.Clear();
		_lastMarks.MindMarkList.AddRange(defeatMarkCollection.MindMarkList);
	}

	private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
	{
		if (mover.GetId() != base.CharacterId || !isMove || isForced)
		{
			return;
		}
		if (base.IsDirect ? (distance < 0) : (distance > 0))
		{
			_distanceAccumulator += Math.Abs(distance);
		}
		while (_distanceAccumulator >= 2)
		{
			_distanceAccumulator -= 2;
			if (_newMarks.GetTotalFlawCount() + _newMarks.GetTotalAcupointCount() + _newMarks.MindMarkList.Count <= 0)
			{
				continue;
			}
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
			_typeRandomPool.Clear();
			for (sbyte b = 0; b < 7; b++)
			{
				for (int i = 0; i < _newMarks.FlawMarkList[b].Count; i++)
				{
					_typeRandomPool.Add((0, b));
				}
				for (int j = 0; j < _newMarks.AcupointMarkList[b].Count; j++)
				{
					_typeRandomPool.Add((1, b));
				}
			}
			for (int k = 0; k < _newMarks.MindMarkList.Count; k++)
			{
				_typeRandomPool.Add((2, -1));
			}
			(sbyte, sbyte) tuple = _typeRandomPool[context.Random.Next(0, _typeRandomPool.Count)];
			if (tuple.Item1 == 0)
			{
				DomainManager.Combat.TransferFlaw(context, base.CombatChar, combatCharacter, tuple.Item2, context.Random.Next(_oldMarks.FlawMarkList[tuple.Item2].Count, _lastMarks.FlawMarkList[tuple.Item2].Count));
			}
			else if (tuple.Item1 == 1)
			{
				DomainManager.Combat.TransferAcupoint(context, base.CombatChar, combatCharacter, tuple.Item2, context.Random.Next(_oldMarks.AcupointMarkList[tuple.Item2].Count, _lastMarks.AcupointMarkList[tuple.Item2].Count));
			}
			else
			{
				DomainManager.Combat.TransferMindDefeatMark(context, base.CombatChar, combatCharacter, context.Random.Next(_oldMarks.MindMarkList.Count, _lastMarks.MindMarkList.Count));
			}
			DomainManager.Combat.AddToCheckFallenSet(combatCharacter.GetId());
			ShowSpecialEffectTips(0);
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
