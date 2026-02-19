using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special;

public class SuiShiChu : CombatSkillEffectBase
{
	private const short AddEquipAttack = 800;

	private const sbyte ExtraReduceDurability = -5;

	private bool _affected;

	private bool _anyHit;

	public SuiShiChu()
	{
	}

	public SuiShiChu(CombatSkillKey skillKey)
		: base(skillKey, 11300, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData((ushort)(base.IsDirect ? 141 : 143), (EDataModifyType)0, -1);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_NormalAttackCalcHitEnd(OnNormalAttackCalcHitEnd);
		Events.RegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd);
		Events.RegisterHandler_AttackSkillAttackHit(OnAttackSkillAttackHit);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_NormalAttackCalcHitEnd(OnNormalAttackCalcHitEnd);
		Events.UnRegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd);
		Events.UnRegisterHandler_AttackSkillAttackHit(OnAttackSkillAttackHit);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCombatBegin(DataContext context)
	{
		AppendAffectedAllEnemyData(context, 309, (EDataModifyType)0, -1);
	}

	private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
	{
		if ((base.IsDirect ? attacker : defender) == base.CombatChar && !isFightback && hit)
		{
			_anyHit = true;
		}
	}

	private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
	{
		if (_affected && _anyHit && (base.IsDirect ? attacker : defender) == base.CombatChar)
		{
			ReduceEffectCount();
		}
		_affected = (_anyHit = false);
	}

	private void OnAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical)
	{
		if ((base.IsDirect ? attacker : defender) == base.CombatChar)
		{
			_anyHit = true;
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
	{
		if (_affected && _anyHit && (base.IsDirect ? (charId == base.CharacterId) : (isAlly != base.CombatChar.IsAlly)))
		{
			ReduceEffectCount();
		}
		_affected = (_anyHit = false);
		if (SkillKey.IsMatch(charId, skillId) && PowerMatchAffectRequire(power))
		{
			AddMaxEffectCount();
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (base.EffectCount <= 0)
		{
			return 0;
		}
		ushort fieldId = dataKey.FieldId;
		bool flag = ((fieldId == 141 || fieldId == 143) ? true : false);
		if (flag && dataKey.CharId == base.CharacterId)
		{
			_affected = true;
			return 800;
		}
		if (dataKey.FieldId == 309 && dataKey.CustomParam0 == (base.IsDirect ? 1 : 0) && dataKey.CustomParam1 == 0 && dataKey.CharId == base.CurrEnemyChar.GetId() && DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar))
		{
			ShowSpecialEffectTips(0);
			return -5;
		}
		return 0;
	}
}
