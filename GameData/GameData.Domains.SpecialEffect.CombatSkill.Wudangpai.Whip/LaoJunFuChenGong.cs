using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip;

public class LaoJunFuChenGong : CombatSkillEffectBase
{
	private const sbyte InitEffectCount = 6;

	private DataUid _defeatMarkUid;

	private readonly List<(sbyte, sbyte)> _markRandomPool = new List<(sbyte, sbyte)>();

	public LaoJunFuChenGong()
	{
	}

	public LaoJunFuChenGong(CombatSkillKey skillKey)
		: base(skillKey, 4303, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 6, base.MaxEffectCount, autoRemoveOnNoCount: false);
		_defeatMarkUid = new DataUid(8, 10, (ulong)base.CharacterId, 50u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_defeatMarkUid, base.DataHandlerKey, OnDefeatMarkChanged);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_defeatMarkUid, base.DataHandlerKey);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
	{
		if (base.EffectCount <= 0 || !DomainManager.Combat.IsCharacterHalfFallen(base.CombatChar))
		{
			return;
		}
		if (base.IsDirect)
		{
			Injuries injuries = base.CombatChar.GetInjuries();
			Injuries injuries2 = injuries.Subtract(base.CombatChar.GetOldInjuries());
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			list.Clear();
			for (sbyte b = 0; b < 7; b++)
			{
				(sbyte, sbyte) tuple = injuries2.Get(b);
				for (int i = 0; i < tuple.Item1 + tuple.Item2; i++)
				{
					list.Add(b);
				}
			}
			if (list.Count > 0)
			{
				int num = Math.Min(base.EffectCount, list.Count);
				for (int j = 0; j < num; j++)
				{
					sbyte bodyPartType = list[context.Random.Next(0, list.Count)];
					bool isInnerInjury = injuries2.Get(bodyPartType, isInnerInjury: true) > 0 && (injuries2.Get(bodyPartType, isInnerInjury: false) <= 0 || context.Random.CheckPercentProb(50));
					injuries.Change(bodyPartType, isInnerInjury, -1);
					injuries2.Change(bodyPartType, isInnerInjury, -1);
				}
				DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries);
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), (short)(-num));
				ShowSpecialEffectTips(0);
			}
			return;
		}
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		_markRandomPool.Clear();
		for (sbyte b2 = 0; b2 < 7; b2++)
		{
			for (int k = 0; k < defeatMarkCollection.FlawMarkList[b2].Count; k++)
			{
				_markRandomPool.Add((0, b2));
			}
			for (int l = 0; l < defeatMarkCollection.AcupointMarkList[b2].Count; l++)
			{
				_markRandomPool.Add((1, b2));
			}
		}
		for (int m = 0; m < defeatMarkCollection.MindMarkList.Count; m++)
		{
			_markRandomPool.Add((2, -1));
		}
		if (_markRandomPool.Count <= 0)
		{
			return;
		}
		int num2 = Math.Min(base.EffectCount, _markRandomPool.Count);
		for (int n = 0; n < num2; n++)
		{
			int index = context.Random.Next(0, _markRandomPool.Count);
			(sbyte, sbyte) tuple2 = _markRandomPool[index];
			if (tuple2.Item1 == 0)
			{
				DomainManager.Combat.RemoveFlaw(context, base.CombatChar, tuple2.Item2, context.Random.Next(0, (int)base.CombatChar.GetFlawCount()[tuple2.Item2]));
			}
			else if (tuple2.Item1 == 1)
			{
				DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, tuple2.Item2, context.Random.Next(0, (int)base.CombatChar.GetAcupointCount()[tuple2.Item2]));
			}
			else
			{
				DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, random: true);
			}
			_markRandomPool.RemoveAt(index);
		}
		DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), (short)(-num2));
		ShowSpecialEffectTips(0);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId && PowerMatchAffectRequire(power) && base.EffectCount < base.MaxEffectCount)
		{
			DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1);
		}
	}
}
