using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi;

public class ErJianMoNvYi : CombatSkillEffectBase
{
	private DataUid _defeatMarkUid;

	private DefeatMarkCollection _lastMarks;

	private readonly List<(sbyte part, bool inner)> _injuryMarkRandomPool = new List<(sbyte, bool)>();

	private readonly List<(sbyte type, sbyte part)> _notInjuryMarkRandomPool = new List<(sbyte, sbyte)>();

	public ErJianMoNvYi()
	{
	}

	public ErJianMoNvYi(CombatSkillKey skillKey)
		: base(skillKey, 17131, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_lastMarks = new DefeatMarkCollection(base.CombatChar.GetDefeatMarkCollection());
		_defeatMarkUid = new DataUid(8, 10, (ulong)base.CharacterId, 50u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_defeatMarkUid, base.DataHandlerKey, OnDefeatMarkChanged);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_defeatMarkUid, base.DataHandlerKey);
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (!IsSrcSkillPerformed)
		{
			IsSrcSkillPerformed = true;
			AddMaxEffectCount();
			sbyte juniorXiangshuTaskStatus = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(0).JuniorXiangshuTaskStatus;
			if (juniorXiangshuTaskStatus <= 4)
			{
				return;
			}
			bool flag = juniorXiangshuTaskStatus == 6;
			Injuries injuries = base.CurrEnemyChar.GetInjuries();
			if (flag)
			{
				Injuries oldInjuries = base.CurrEnemyChar.GetOldInjuries();
				List<(sbyte, bool)> list = ObjectPool<List<(sbyte, bool)>>.Instance.Get();
				list.Clear();
				for (sbyte b = 0; b < 7; b++)
				{
					for (int i = 0; i < injuries.Get(b, isInnerInjury: false); i++)
					{
						list.Add((b, false));
					}
					for (int j = 0; j < injuries.Get(b, isInnerInjury: true); j++)
					{
						list.Add((b, true));
					}
				}
				if (list.Count > 0)
				{
					(sbyte, bool) tuple = list[context.Random.Next(list.Count)];
					DomainManager.Combat.RemoveInjury(context, base.CurrEnemyChar, tuple.Item1, tuple.Item2, 1, updateDefeatMark: true, oldInjuries.Get(tuple.Item1, tuple.Item2) > 0);
				}
				ObjectPool<List<(sbyte, bool)>>.Instance.Return(list);
			}
			else
			{
				bool flag2 = true;
				bool flag3 = true;
				for (sbyte b2 = 0; b2 < 7; b2++)
				{
					if (injuries.Get(b2, isInnerInjury: false) < 6)
					{
						flag2 = false;
					}
					if (injuries.Get(b2, isInnerInjury: true) < 6)
					{
						flag3 = false;
					}
					if (!flag2 && !flag3)
					{
						break;
					}
				}
				bool flag4 = !base.CurrEnemyChar.GetOuterInjuryImmunity() && !flag2;
				bool flag5 = !base.CurrEnemyChar.GetInnerInjuryImmunity() && !flag3;
				if (flag4 || flag5)
				{
					bool inner = flag5 && (!flag4 || context.Random.CheckPercentProb(50));
					DomainManager.Combat.AddRandomInjury(context, base.CurrEnemyChar, inner, 1, 1, changeToOld: true, -1);
				}
			}
			ShowSpecialEffectTips(flag, 1, 2);
		}
		else
		{
			RemoveSelf(context);
		}
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			RemoveSelf(context);
		}
	}

	private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
	{
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		int num = 0;
		int num2 = 0;
		_injuryMarkRandomPool.Clear();
		_notInjuryMarkRandomPool.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			byte b2 = defeatMarkCollection.OuterInjuryMarkList[b];
			byte b3 = defeatMarkCollection.InnerInjuryMarkList[b];
			int count = defeatMarkCollection.FlawMarkList[b].Count;
			int count2 = defeatMarkCollection.AcupointMarkList[b].Count;
			byte b4 = _lastMarks.OuterInjuryMarkList[b];
			byte b5 = _lastMarks.InnerInjuryMarkList[b];
			int count3 = _lastMarks.FlawMarkList[b].Count;
			int count4 = _lastMarks.AcupointMarkList[b].Count;
			if (b2 > b4)
			{
				num += b2 - b4;
			}
			if (b3 > b5)
			{
				num += b3 - b5;
			}
			if (count > count3)
			{
				num2 += count - count3;
			}
			if (count2 > count4)
			{
				num2 += count2 - count4;
			}
			for (int i = 0; i < b2; i++)
			{
				_injuryMarkRandomPool.Add((b, false));
			}
			for (int j = 0; j < b3; j++)
			{
				_injuryMarkRandomPool.Add((b, true));
			}
			for (int k = 0; k < count; k++)
			{
				_notInjuryMarkRandomPool.Add((0, b));
			}
			for (int l = 0; l < count2; l++)
			{
				_notInjuryMarkRandomPool.Add((1, b));
			}
		}
		int count5 = defeatMarkCollection.MindMarkList.Count;
		int count6 = _lastMarks.MindMarkList.Count;
		if (count5 > count6)
		{
			num2 += count5 - count6;
		}
		for (int m = 0; m < count5; m++)
		{
			_notInjuryMarkRandomPool.Add((2, -1));
		}
		num = Math.Min(num, _notInjuryMarkRandomPool.Count);
		num2 = Math.Min(num2, _injuryMarkRandomPool.Count);
		int num3 = Math.Min(num + num2, base.EffectCount);
		if (num3 <= 0)
		{
			return;
		}
		while (num + num2 > num3)
		{
			if (num > 0 && (num2 == 0 || context.Random.CheckPercentProb(50)))
			{
				num--;
			}
			else
			{
				num2--;
			}
		}
		for (int n = 0; n < num2; n++)
		{
			int index = context.Random.Next(_injuryMarkRandomPool.Count);
			(sbyte, bool) tuple = _injuryMarkRandomPool[index];
			DomainManager.Combat.RemoveInjury(context, base.CombatChar, tuple.Item1, tuple.Item2, 1);
			_injuryMarkRandomPool.RemoveAt(index);
		}
		for (int num4 = 0; num4 < num; num4++)
		{
			int index2 = context.Random.Next(_notInjuryMarkRandomPool.Count);
			(sbyte, sbyte) tuple2 = _notInjuryMarkRandomPool[index2];
			if (tuple2.Item1 == 0)
			{
				int index3 = context.Random.Next(defeatMarkCollection.FlawMarkList[tuple2.Item2].Count);
				defeatMarkCollection.FlawMarkList[tuple2.Item2].RemoveAt(index3);
				DomainManager.Combat.RemoveFlaw(context, base.CombatChar, tuple2.Item2, index3, raiseEvent: false, updateMark: false);
			}
			else if (tuple2.Item1 == 1)
			{
				int index4 = context.Random.Next(defeatMarkCollection.AcupointMarkList[tuple2.Item2].Count);
				defeatMarkCollection.AcupointMarkList[tuple2.Item2].RemoveAt(index4);
				DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, tuple2.Item2, index4, raiseEvent: false, updateMark: false);
			}
			else if (tuple2.Item1 == 2)
			{
				DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, random: true);
			}
			_notInjuryMarkRandomPool.RemoveAt(index2);
		}
		_lastMarks = new DefeatMarkCollection(base.CombatChar.GetDefeatMarkCollection());
		ReduceEffectCount(num3);
		DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
		ShowSpecialEffectTips(0);
	}
}
