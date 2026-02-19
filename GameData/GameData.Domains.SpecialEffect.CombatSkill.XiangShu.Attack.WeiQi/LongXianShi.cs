using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi;

public class LongXianShi : CombatSkillEffectBase
{
	private const sbyte MaxReduceMark = 3;

	private DataUid _defeatMarkUid;

	private DefeatMarkCollection _lastMarks;

	private readonly List<(sbyte type, sbyte part)> _markRandomPool = new List<(sbyte, sbyte)>();

	public LongXianShi()
	{
	}

	public LongXianShi(CombatSkillKey skillKey)
		: base(skillKey, 17051, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		if (IsSrcSkillPerformed)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_defeatMarkUid, base.DataHandlerKey);
		}
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		DefeatMarkCollection defeatMarkCollection = combatCharacter.GetDefeatMarkCollection();
		_markRandomPool.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			int count = defeatMarkCollection.FlawMarkList[b].Count;
			int count2 = defeatMarkCollection.AcupointMarkList[b].Count;
			for (int i = 0; i < count; i++)
			{
				_markRandomPool.Add((0, b));
			}
			for (int j = 0; j < count2; j++)
			{
				_markRandomPool.Add((1, b));
			}
		}
		int count3 = defeatMarkCollection.MindMarkList.Count;
		for (int k = 0; k < count3; k++)
		{
			_markRandomPool.Add((2, -1));
		}
		if (!IsSrcSkillPerformed)
		{
			if (_markRandomPool.Count > 0 && PowerMatchAffectRequire(power))
			{
				while (_markRandomPool.Count > 3)
				{
					_markRandomPool.RemoveAt(context.Random.Next(_markRandomPool.Count));
				}
				for (int l = 0; l < _markRandomPool.Count; l++)
				{
					RemoveDefeatMark(context, combatCharacter, _markRandomPool[l], defeatMarkCollection, randomRemove: true);
				}
				combatCharacter.SetDefeatMarkCollection(defeatMarkCollection, context);
				IsSrcSkillPerformed = true;
				short num = (short)(_markRandomPool.Count * 2);
				DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), num, num, autoRemoveOnNoCount: true);
				_lastMarks = new DefeatMarkCollection(base.CombatChar.GetDefeatMarkCollection());
				_defeatMarkUid = new DataUid(8, 10, (ulong)base.CharacterId, 50u);
				GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_defeatMarkUid, base.DataHandlerKey, OnDefeatMarkChanged);
			}
			else
			{
				RemoveSelf(context);
			}
		}
		else if (_markRandomPool.Count > 0 && PowerMatchAffectRequire(power))
		{
			RemoveSelf(context);
		}
	}

	private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
	{
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		_markRandomPool.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			int num = defeatMarkCollection.FlawMarkList[b].Count - _lastMarks.FlawMarkList[b].Count;
			int num2 = defeatMarkCollection.AcupointMarkList[b].Count - _lastMarks.AcupointMarkList[b].Count;
			for (int i = 0; i < num; i++)
			{
				_markRandomPool.Add((0, b));
			}
			for (int j = 0; j < num2; j++)
			{
				_markRandomPool.Add((1, b));
			}
		}
		int num3 = defeatMarkCollection.MindMarkList.Count - defeatMarkCollection.MindMarkList.Count;
		for (int k = 0; k < num3; k++)
		{
			_markRandomPool.Add((2, -1));
		}
		_lastMarks = new DefeatMarkCollection(base.CombatChar.GetDefeatMarkCollection());
		if (_markRandomPool.Count > 0)
		{
			while (_markRandomPool.Count > base.EffectCount)
			{
				_markRandomPool.RemoveAt(context.Random.Next(_markRandomPool.Count));
			}
			for (int l = 0; l < _markRandomPool.Count; l++)
			{
				RemoveDefeatMark(context, base.CombatChar, _markRandomPool[l], defeatMarkCollection, randomRemove: false);
			}
			base.CombatChar.SetDefeatMarkCollection(defeatMarkCollection, context);
			ReduceEffectCount(_markRandomPool.Count);
			ShowSpecialEffectTips(0);
		}
	}

	private void RemoveDefeatMark(DataContext context, CombatCharacter combatChar, (sbyte type, sbyte part) markInfo, DefeatMarkCollection markCollection, bool randomRemove)
	{
		if (markInfo.type == 0)
		{
			int count = markCollection.FlawMarkList[markInfo.part].Count;
			int index = (randomRemove ? context.Random.Next(count) : (count - 1));
			markCollection.FlawMarkList[markInfo.part].RemoveAt(index);
			DomainManager.Combat.RemoveFlaw(context, combatChar, markInfo.part, index, raiseEvent: false, updateMark: false);
		}
		else if (markInfo.type == 1)
		{
			int count2 = markCollection.AcupointMarkList[markInfo.part].Count;
			int index2 = (randomRemove ? context.Random.Next(count2) : (count2 - 1));
			markCollection.AcupointMarkList[markInfo.part].RemoveAt(index2);
			DomainManager.Combat.RemoveAcupoint(context, combatChar, markInfo.part, index2, raiseEvent: false, updateMark: false);
		}
		else if (markInfo.type == 2)
		{
			DomainManager.Combat.RemoveMindDefeatMark(context, combatChar, 1, randomRemove, context.Random.Next(markCollection.MindMarkList.Count));
		}
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			RemoveSelf(context);
		}
	}
}
