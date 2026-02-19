using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.FistAndPalm;

public class DanShaShenZhang : CombatSkillEffectBase
{
	private const sbyte AddPoisonPercent = 100;

	private sbyte _poisonType;

	private sbyte _poisonLevel;

	private int _poisonValue;

	public DanShaShenZhang()
	{
	}

	public DanShaShenZhang(CombatSkillKey skillKey)
		: base(skillKey, 10104, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (IsSrcSkillPerformed && attacker.GetId() == base.CharacterId && hit)
		{
			AddPoison(context, attacker, defender);
			ShowSpecialEffectTips(1);
			ReduceEffectCount();
		}
	}

	private unsafe void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		CombatCharacter combatCharacter = (base.IsDirect ? base.CurrEnemyChar : base.CombatChar);
		PoisonInts poison = combatCharacter.GetPoison();
		PoisonInts oldPoison = combatCharacter.GetOldPoison();
		_poisonValue = 0;
		for (sbyte b = 0; b < 6; b++)
		{
			int num = poison.Items[b] - oldPoison.Items[b];
			if (num > _poisonValue)
			{
				_poisonType = b;
				_poisonLevel = PoisonsAndLevels.CalcPoisonedLevel(poison.Items[b]);
				_poisonValue = num;
			}
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId)
		{
			return;
		}
		if (!IsSrcSkillPerformed)
		{
			if (skillId != base.SkillTemplateId)
			{
				return;
			}
			IsSrcSkillPerformed = true;
			if (PowerMatchAffectRequire(power) && _poisonValue > 0)
			{
				CombatCharacter character = (base.IsDirect ? base.CurrEnemyChar : base.CombatChar);
				_poisonValue = DomainManager.Combat.ReducePoison(context, character, _poisonType, _poisonValue);
				if (_poisonValue <= 0)
				{
					RemoveSelf(context);
					return;
				}
				int num = PoisonsAndLevels.CalcPoisonedLevel(_poisonValue);
				if (num > 0)
				{
					bool inner = _poisonType == 1 || _poisonType == 2 || _poisonType == 5;
					DomainManager.Combat.AddRandomInjury(context, character, inner, num, 1, changeToOld: false, -1);
				}
				ShowSpecialEffectTips(0);
				AddMaxEffectCount();
			}
			else
			{
				RemoveSelf(context);
			}
		}
		else if (Config.CombatSkill.Instance[skillId].EquipType == 1 && power > 0)
		{
			AddPoison(context, base.CombatChar, base.CurrEnemyChar);
			ShowSpecialEffectTips(1);
			ReduceEffectCount();
			if (skillId == base.SkillTemplateId && PowerMatchAffectRequire(power))
			{
				RemoveSelf(context);
			}
		}
	}

	private void AddPoison(DataContext context, CombatCharacter attacker, CombatCharacter defender)
	{
		DomainManager.Combat.AddPoison(context, attacker, defender, _poisonType, _poisonLevel, _poisonValue * 100 / 100, base.SkillTemplateId, applySpecialEffect: false, canBounce: true, default(ItemKey), isDirectPoison: false, ignorePositiveResist: true);
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			RemoveSelf(context);
		}
	}
}
