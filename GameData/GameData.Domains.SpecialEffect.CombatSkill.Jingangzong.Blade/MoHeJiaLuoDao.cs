using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade;

public class MoHeJiaLuoDao : CombatSkillEffectBase
{
	private const short AddGoneMadInjury = 100;

	private const int DisorderOfQiPerEffectCount = 1000;

	private const sbyte BuffEnemyCount = 18;

	private const sbyte MaxCostOnce = 6;

	private const int ChangeNeiliAllocationValue = 5;

	private int _delayDisorderOfQi;

	private int _delayFatalDamages;

	private readonly OuterAndInnerInts[] _delayDamages = new OuterAndInnerInts[7];

	private bool _delaying;

	private int ChangeNeiliAllocationDirection => base.IsDirect ? 1 : (-1);

	private bool CanBuffEnemy => base.EffectCount >= 18;

	public MoHeJiaLuoDao()
	{
	}

	public MoHeJiaLuoDao(CombatSkillKey skillKey)
		: base(skillKey, 11208, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(322, (EDataModifyType)3, -1);
		CreateAffectedData(114, (EDataModifyType)3, -1);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			_delaying = true;
			_delayFatalDamages = 0;
			for (int i = 0; i < _delayDamages.Length; i++)
			{
				_delayDamages[i] = default(OuterAndInnerInts);
			}
			DomainManager.Combat.AddGoneMadInjury(context, base.CombatChar, skillId, 100);
		}
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker.GetId() != base.CharacterId && (!CanBuffEnemy || defender.GetId() != base.CharacterId))
		{
			return;
		}
		int num = Math.Min(6, base.EffectCount);
		if (num > 0)
		{
			ShowSpecialEffectTips(attacker.GetId() == base.CharacterId, 2, 3);
			ReduceEffectCount(num);
			int addValue = num * 5 * ChangeNeiliAllocationDirection;
			CombatCharacter combatCharacter = (base.IsDirect ? attacker : defender);
			for (byte b = 0; b < 4; b++)
			{
				combatCharacter.ChangeNeiliAllocation(context, b, addValue);
			}
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		_delaying = false;
		int num = base.EffectCount;
		bool flag = PowerMatchAffectRequire(power);
		if (flag)
		{
			ShowSpecialEffectTips(1);
		}
		if (flag)
		{
			num += _delayFatalDamages / base.CombatChar.GetDamageStepCollection().FatalDamageStep;
			num += _delayDisorderOfQi / 1000;
		}
		else
		{
			DomainManager.Combat.AddFatalDamageValue(context, base.CombatChar, _delayFatalDamages, -1, -1, -1);
			base.CombatChar.GetCharacter().ChangeDisorderOfQi(context, _delayDisorderOfQi);
		}
		for (sbyte b = 0; b < 7; b++)
		{
			OuterAndInnerInts outerAndInnerInts = _delayDamages[b];
			if (flag)
			{
				num += CalcEffectCount(outerAndInnerInts.Outer, inner: false, b);
				num += CalcEffectCount(outerAndInnerInts.Inner, inner: true, b);
			}
			else
			{
				DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, base.CombatChar, b, outerAndInnerInts.Outer, outerAndInnerInts.Inner, -1, updateDefeatMark: false);
			}
		}
		if (flag && num > 0)
		{
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, base.EffectKey, (short)num, (short)num, autoRemoveOnNoCount: true);
		}
		DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
	}

	private int CalcEffectCount(int damage, bool inner, sbyte bodyPart)
	{
		DamageStepCollection damageStepCollection = base.CombatChar.GetDamageStepCollection();
		Injuries injuries = base.CombatChar.GetInjuries();
		int[] array = (inner ? damageStepCollection.InnerDamageSteps : damageStepCollection.OuterDamageSteps);
		int num = damage / array[bodyPart];
		if (injuries.Get(bodyPart, inner) + num <= 6)
		{
			return num;
		}
		num = 6 - injuries.Get(bodyPart, inner);
		damage -= num * array[bodyPart];
		return num + damage / damageStepCollection.FatalDamageStep;
	}

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		if (dataKey.CharId != base.CharacterId || !_delaying || dataKey.FieldId != 114)
		{
			return dataValue;
		}
		EDamageType customParam = (EDamageType)dataKey.CustomParam0;
		if (customParam != EDamageType.Direct)
		{
			return dataValue;
		}
		sbyte b = (sbyte)dataKey.CustomParam2;
		if (dataKey.CustomParam1 == 1)
		{
			_delayDamages[b].Inner += (int)dataValue;
		}
		else
		{
			_delayDamages[b].Outer += (int)dataValue;
		}
		ShowSpecialEffectTipsOnceInFrame(0);
		return 0L;
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.CharId != base.CharacterId || !_delaying || dataKey.FieldId != 322)
		{
			return dataValue;
		}
		bool flag = dataKey.CustomParam0 == 1;
		sbyte b = (sbyte)dataKey.CustomParam1;
		if (dataKey.CustomParam2 == 1)
		{
			_delayDisorderOfQi += dataValue;
		}
		else if (b < 0)
		{
			_delayFatalDamages += dataValue;
		}
		else if (flag)
		{
			_delayDamages[b].Inner += dataValue;
		}
		else
		{
			_delayDamages[b].Outer += dataValue;
		}
		ShowSpecialEffectTipsOnceInFrame(0);
		return 0;
	}
}
