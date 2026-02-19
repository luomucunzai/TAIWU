using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist;

public class JiShenChengFo : DefenseSkillBase
{
	private const sbyte RequireNeiliAllocation = 3;

	private const short AddQiDisorder = 200;

	private DataUid _defeatMarkUid;

	private DefeatMarkCollection _lastMarks;

	private readonly List<(sbyte, sbyte)> _newMarkList = new List<(sbyte, sbyte)>();

	public JiShenChengFo()
	{
	}

	public JiShenChengFo(CombatSkillKey skillKey)
		: base(skillKey, 11607)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(191, (EDataModifyType)3, -1);
		CreateAffectedData(192, (EDataModifyType)3, -1);
		_lastMarks = new DefeatMarkCollection(base.CombatChar.GetDefeatMarkCollection());
		_defeatMarkUid = ParseCombatCharacterDataUid(50);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_defeatMarkUid, base.DataHandlerKey, OnDefeatMarkChanged);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_defeatMarkUid, base.DataHandlerKey);
	}

	private unsafe void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
	{
		if (!base.CanAffect)
		{
			return;
		}
		NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
		byte b = 2;
		int num = neiliAllocation.Items[(int)b] / 3;
		if (num <= 0)
		{
			UpdateLastDefeatMarks();
			return;
		}
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		_newMarkList.Clear();
		if (base.IsDirect)
		{
			Injuries injuries = base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries());
			for (sbyte b2 = 0; b2 < 7; b2++)
			{
				int num2 = Math.Min(injuries.Get(b2, isInnerInjury: false), defeatMarkCollection.OuterInjuryMarkList[b2] - _lastMarks.OuterInjuryMarkList[b2]);
				for (int i = 0; i < num2; i++)
				{
					_newMarkList.Add((0, b2));
				}
				int num3 = Math.Min(injuries.Get(b2, isInnerInjury: true), defeatMarkCollection.InnerInjuryMarkList[b2] - _lastMarks.InnerInjuryMarkList[b2]);
				for (int j = 0; j < num3; j++)
				{
					_newMarkList.Add((1, b2));
				}
			}
		}
		else
		{
			for (sbyte b3 = 0; b3 < 7; b3++)
			{
				int num4 = defeatMarkCollection.FlawMarkList[b3].Count - _lastMarks.FlawMarkList[b3].Count;
				for (int k = 0; k < num4; k++)
				{
					_newMarkList.Add((2, b3));
				}
				int num5 = defeatMarkCollection.AcupointMarkList[b3].Count - _lastMarks.AcupointMarkList[b3].Count;
				for (int l = 0; l < num5; l++)
				{
					_newMarkList.Add((3, b3));
				}
			}
			int num6 = defeatMarkCollection.MindMarkList.Count - _lastMarks.MindMarkList.Count;
			for (int m = 0; m < num6; m++)
			{
				_newMarkList.Add((4, -1));
			}
		}
		UpdateLastDefeatMarks();
		if (_newMarkList.Count == 0)
		{
			return;
		}
		for (int n = 0; n < num; n++)
		{
			if (_newMarkList.Count <= 0)
			{
				break;
			}
			int index = context.Random.Next(0, _newMarkList.Count);
			(sbyte, sbyte) tuple = _newMarkList[index];
			_newMarkList.RemoveAt(index);
			base.CombatChar.ChangeNeiliAllocation(context, b, -3);
			DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.CombatChar, 200);
			if (tuple.Item1 == 0)
			{
				DomainManager.Combat.RemoveInjury(context, base.CombatChar, tuple.Item2, isInner: false, 1, updateDefeatMark: true);
			}
			else if (tuple.Item1 == 1)
			{
				DomainManager.Combat.RemoveInjury(context, base.CombatChar, tuple.Item2, isInner: true, 1, updateDefeatMark: true);
			}
			else if (tuple.Item1 == 2)
			{
				DomainManager.Combat.RemoveFlaw(context, base.CombatChar, tuple.Item2, base.CombatChar.GetFlawCount()[tuple.Item2] - 1);
			}
			else if (tuple.Item1 == 3)
			{
				DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, tuple.Item2, base.CombatChar.GetAcupointCount()[tuple.Item2] - 1);
			}
			else if (tuple.Item1 == 4)
			{
				DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, random: false, base.CombatChar.GetMindMarkTime().MarkList.Count - 1);
			}
		}
		ShowSpecialEffectTips(0);
	}

	private void UpdateLastDefeatMarks()
	{
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		if (base.IsDirect)
		{
			for (sbyte b = 0; b < 7; b++)
			{
				_lastMarks.OuterInjuryMarkList[b] = defeatMarkCollection.OuterInjuryMarkList[b];
				_lastMarks.InnerInjuryMarkList[b] = defeatMarkCollection.InnerInjuryMarkList[b];
			}
			return;
		}
		for (sbyte b2 = 0; b2 < 7; b2++)
		{
			_lastMarks.FlawMarkList[b2].Clear();
			_lastMarks.FlawMarkList[b2].AddRange(defeatMarkCollection.FlawMarkList[b2]);
			_lastMarks.AcupointMarkList[b2].Clear();
			_lastMarks.AcupointMarkList[b2].AddRange(defeatMarkCollection.AcupointMarkList[b2]);
		}
		_lastMarks.MindMarkList.Clear();
		_lastMarks.MindMarkList.AddRange(defeatMarkCollection.MindMarkList);
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.CharId != base.CharacterId || !base.CanAffect || base.CombatChar.BeCriticalDuringCalcAddInjury)
		{
			return dataValue;
		}
		ushort fieldId = dataKey.FieldId;
		if ((uint)(fieldId - 191) <= 1u)
		{
			return 0;
		}
		return dataValue;
	}
}
