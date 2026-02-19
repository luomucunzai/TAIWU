using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger;

public class BaiHuaSha : CombatSkillEffectBase
{
	private List<sbyte> _bodyPartList;

	public BaiHuaSha()
	{
	}

	public BaiHuaSha(CombatSkillKey skillKey)
		: base(skillKey, 3107, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		if (_bodyPartList != null)
		{
			ObjectPool<List<sbyte>>.Instance.Return(_bodyPartList);
		}
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		SortedDictionary<sbyte, List<(sbyte, int, int)>> bodyPartDict = (base.IsDirect ? base.CurrEnemyChar.GetAcupointCollection() : base.CurrEnemyChar.GetFlawCollection()).BodyPartDict;
		_bodyPartList = ObjectPool<List<sbyte>>.Instance.Get();
		_bodyPartList.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			if (bodyPartDict[b].Count > 0)
			{
				_bodyPartList.Add(b);
			}
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power))
		{
			Injuries injuries = base.CurrEnemyChar.GetInjuries();
			int currInnerRatio = base.SkillInstance.GetCurrInnerRatio();
			bool flag = false;
			foreach (sbyte bodyPart in _bodyPartList)
			{
				(sbyte, sbyte) tuple = injuries.Get(bodyPart);
				if (tuple.Item1 < 6 || tuple.Item2 < 6)
				{
					bool isInner = tuple.Item1 >= 6 || (tuple.Item2 < 6 && context.Random.CheckPercentProb(currInnerRatio));
					DomainManager.Combat.AddInjury(context, base.CurrEnemyChar, bodyPart, isInner, 1, updateDefeatMark: false, changeToOld: true);
					flag = true;
				}
			}
			if (flag)
			{
				DomainManager.Combat.UpdateBodyDefeatMark(context, base.CurrEnemyChar);
				ShowSpecialEffectTips(0);
			}
		}
		RemoveSelf(context);
	}
}
