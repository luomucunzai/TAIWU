using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi;

public class SheMingShi : CombatSkillEffectBase
{
	private const sbyte MaxReduceMark = 3;

	private Dictionary<int, (int mind, int die)> _enemyLastMarkCount;

	private readonly List<DataUid> _defeatMarkUids = new List<DataUid>();

	public SheMingShi()
	{
	}

	public SheMingShi(CombatSkillKey skillKey)
		: base(skillKey, 17054, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
		foreach (DataUid defeatMarkUid in _defeatMarkUids)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(defeatMarkUid, base.DataHandlerKey);
		}
		_defeatMarkUids.Clear();
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		List<(sbyte, sbyte)> list = new List<(sbyte, sbyte)>();
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			int count = defeatMarkCollection.FlawMarkList[b].Count;
			int count2 = defeatMarkCollection.AcupointMarkList[b].Count;
			for (int i = 0; i < count; i++)
			{
				list.Add((0, b));
			}
			for (int j = 0; j < count2; j++)
			{
				list.Add((1, b));
			}
		}
		int count3 = defeatMarkCollection.MindMarkList.Count;
		for (int k = 0; k < count3; k++)
		{
			list.Add((2, -1));
		}
		if (!IsSrcSkillPerformed)
		{
			if (list.Count > 0 && !interrupted)
			{
				while (list.Count > 3)
				{
					list.RemoveAt(context.Random.Next(list.Count));
				}
				for (int l = 0; l < list.Count; l++)
				{
					(sbyte, sbyte) tuple = list[l];
					if (tuple.Item1 == 0)
					{
						int count4 = defeatMarkCollection.FlawMarkList[tuple.Item2].Count;
						int index = context.Random.Next(count4);
						defeatMarkCollection.FlawMarkList[tuple.Item2].RemoveAt(index);
						DomainManager.Combat.RemoveFlaw(context, base.CombatChar, tuple.Item2, index, raiseEvent: false, updateMark: false);
					}
					else if (tuple.Item1 == 1)
					{
						int count5 = defeatMarkCollection.AcupointMarkList[tuple.Item2].Count;
						int index2 = context.Random.Next(count5);
						defeatMarkCollection.AcupointMarkList[tuple.Item2].RemoveAt(index2);
						DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, tuple.Item2, index2, raiseEvent: false, updateMark: false);
					}
					else if (tuple.Item1 == 2)
					{
						DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, random: false);
					}
				}
				base.CombatChar.SetDefeatMarkCollection(defeatMarkCollection, context);
				IsSrcSkillPerformed = true;
				short num = (short)(list.Count * 2);
				DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), num, num, autoRemoveOnNoCount: true);
				AppendAffectedAllEnemyData(context, 129, (EDataModifyType)0, -1);
				AppendAffectedAllEnemyData(context, 134, (EDataModifyType)0, -1);
				_enemyLastMarkCount = new Dictionary<int, (int, int)>();
				int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				foreach (int num2 in characterList)
				{
					if (num2 >= 0)
					{
						DataUid dataUid = new DataUid(8, 10, (ulong)num2, 50u);
						DefeatMarkCollection defeatMarkCollection2 = DomainManager.Combat.GetElement_CombatCharacterDict(num2).GetDefeatMarkCollection();
						_enemyLastMarkCount[num2] = (defeatMarkCollection2.MindMarkList?.Count ?? 0, defeatMarkCollection2.DieMarkList?.Count ?? 0);
						GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, OnDefeatMarkChanged);
						_defeatMarkUids.Add(dataUid);
					}
				}
			}
			else
			{
				RemoveSelf(context);
			}
		}
		else if (list.Count > 0 && !interrupted)
		{
			RemoveSelf(context);
		}
	}

	private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
	{
		int num = (int)dataUid.SubId0;
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(num);
		DefeatMarkCollection defeatMarkCollection = element_CombatCharacterDict.GetDefeatMarkCollection();
		bool flag = (defeatMarkCollection.MindMarkList?.Count ?? 0) > _enemyLastMarkCount[num].mind;
		if (flag && (base.EffectCount > 1 || context.Random.CheckPercentProb(50)))
		{
			DomainManager.Combat.AppendMindDefeatMark(context, element_CombatCharacterDict, 1, -1);
			ReduceEffectCount();
		}
		if (flag)
		{
			DomainManager.Combat.AddToCheckFallenSet(element_CombatCharacterDict.GetId());
			ShowSpecialEffectTips(0);
		}
		_enemyLastMarkCount[num] = (defeatMarkCollection.MindMarkList?.Count ?? 0, defeatMarkCollection.DieMarkList?.Count ?? 0);
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			RemoveSelf(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.FieldId == 134 || dataKey.FieldId == 129)
		{
			ReduceEffectCount();
			ShowSpecialEffectTips(0);
			return 1;
		}
		return 0;
	}
}
