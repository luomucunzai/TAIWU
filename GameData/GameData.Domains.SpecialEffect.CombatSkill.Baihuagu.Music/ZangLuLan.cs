using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music;

public class ZangLuLan : CombatSkillEffectBase
{
	private const sbyte DirectAddPowerUnit = 10;

	private const sbyte ReverseAddPowerUnit = 5;

	private static readonly List<(int, short)> CountWeights = new List<(int, short)>
	{
		(2, 25),
		(3, 50),
		(4, 25)
	};

	private int _addPower;

	private static int GetRandomCount(IRandomSource random)
	{
		return RandomUtils.GetRandomResult(CountWeights, random);
	}

	public ZangLuLan()
	{
	}

	public ZangLuLan(CombatSkillKey skillKey)
		: base(skillKey, 3305, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_addPower = (base.IsDirect ? (10 * (base.CombatChar.GetTrickCount(20) + base.EnemyChar.GetTrickCount(20))) : (5 * (base.CombatChar.GetDefeatMarkCollection().MindMarkList.Count + base.EnemyChar.GetDefeatMarkCollection().MindMarkList.Count)));
		if (_addPower > 0)
		{
			AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId), (EDataModifyType)1);
			ShowSpecialEffectTips(0);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power))
		{
			bool flag = (base.IsDirect ? (base.CombatChar.GetTrickCount(20) > 0) : (base.CombatChar.GetDefeatMarkCollection().MindMarkList.Count > 0));
			if (base.IsDirect)
			{
				if (flag)
				{
					DomainManager.Combat.RemoveTrick(context, base.CombatChar, 20, (byte)Math.Min(GetRandomCount(context.Random), base.CombatChar.GetTrickCount(20)));
				}
				DomainManager.Combat.AddTrick(context, base.EnemyChar, 20, GetRandomCount(context.Random), addedByAlly: false);
			}
			else
			{
				if (flag)
				{
					DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, Math.Min(GetRandomCount(context.Random), base.CombatChar.GetDefeatMarkCollection().MindMarkList.Count), random: true);
				}
				DomainManager.Combat.AppendMindDefeatMark(context, base.EnemyChar, GetRandomCount(context.Random), -1);
			}
			if (flag)
			{
				ShowSpecialEffectTips(1);
			}
			ShowSpecialEffectTips(2);
		}
		RemoveSelf(context);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return _addPower;
		}
		return 0;
	}
}
