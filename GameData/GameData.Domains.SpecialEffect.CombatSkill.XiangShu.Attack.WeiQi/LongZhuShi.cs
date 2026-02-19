using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi;

public class LongZhuShi : CombatSkillEffectBase
{
	private const sbyte ReducePowerUnit = -30;

	private readonly Dictionary<CombatSkillKey, int> _reducePowerDict = new Dictionary<CombatSkillKey, int>();

	public LongZhuShi()
	{
	}

	public LongZhuShi(CombatSkillKey skillKey)
		: base(skillKey, 17052, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedAllEnemyData(199, (EDataModifyType)1, -1);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId || !PowerMatchAffectRequire(power))
		{
			return;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		IReadOnlyDictionary<int, sbyte> tricks = combatCharacter.GetTricks().Tricks;
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		int num = 0;
		list.Clear();
		list.AddRange(tricks.Values.Where(combatCharacter.IsTrickUsable));
		int maxCount = Math.Min(base.MaxEffectCount, list.Count);
		foreach (sbyte item in RandomUtils.GetRandomUnrepeated(context.Random, maxCount, list))
		{
			if (DomainManager.Combat.RemoveTrick(context, combatCharacter, item, 1))
			{
				num++;
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		if (num > 0)
		{
			AddEffectCount(num);
		}
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (SkillKey.IsMatch(charId, skillId))
		{
			ClearPowers(context);
		}
		if (base.CombatChar.IsAlly != isAlly && base.EffectCount > 0)
		{
			CombatSkillKey key = new CombatSkillKey(charId, skillId);
			_reducePowerDict[key] = _reducePowerDict.GetOrDefault(key) + -30;
			DomainManager.SpecialEffect.InvalidateCache(context, charId, 199);
			ReduceEffectCount();
			ShowSpecialEffectTips(0);
		}
	}

	private void ClearPowers(DataContext context)
	{
		_reducePowerDict.Clear();
		InvalidateAllEnemyCache(context, 199);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		return CollectionExtensions.GetValueOrDefault(key: new CombatSkillKey(dataKey.CharId, dataKey.CombatSkillId), dictionary: _reducePowerDict, defaultValue: 0);
	}
}
