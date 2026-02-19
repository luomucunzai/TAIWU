using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.DefenseAndAssist;

public class TianBingPiJiaShu : DefenseSkillBase
{
	private const sbyte ArmorExtraGrade = 2;

	private const sbyte ReduceDamagePercent = -60;

	private bool _affecting;

	public TianBingPiJiaShu()
	{
	}

	public TianBingPiJiaShu(CombatSkillKey skillKey)
		: base(skillKey, 9704)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(102, (EDataModifyType)1, -1);
		Events.RegisterHandler_NormalAttackBegin(OnNormalAttackBegin);
		Events.RegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd);
		Events.RegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_NormalAttackBegin(OnNormalAttackBegin);
		Events.UnRegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd);
		Events.UnRegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
	{
		if (defender == base.CombatChar && pursueIndex <= 0 && DomainManager.Combat.InAttackRange(attacker) && base.CanAffect)
		{
			UpdateAffecting(attacker.NormalAttackBodyPart);
		}
	}

	private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
	{
		if (_affecting && defender == base.CombatChar)
		{
			_affecting = false;
		}
	}

	private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (isAlly != base.CombatChar.IsAlly && base.CanAffect && Config.CombatSkill.Instance[skillId].EquipType == 1 && DomainManager.Combat.InAttackRange(DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly)) && DomainManager.Combat.GetCombatCharacter(base.CombatChar.IsAlly, tryGetCoverCharacter: true) == base.CombatChar)
		{
			UpdateAffecting(DomainManager.Combat.GetElement_CombatCharacterDict(charId).SkillAttackBodyPart);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (_affecting && isAlly != base.CombatChar.IsAlly && Config.CombatSkill.Instance[skillId].EquipType == 1)
		{
			_affecting = false;
		}
	}

	private void UpdateAffecting(sbyte attackBodyPart)
	{
		_affecting = CheckCanAffect(attackBodyPart);
		if (_affecting)
		{
			ShowSpecialEffectTips(0);
		}
	}

	private bool CheckCanAffect(sbyte attackBodyPart)
	{
		if (attackBodyPart < 0)
		{
			return false;
		}
		if (base.CurrEnemyChar.GetCharacter().GetConsummateLevel() <= CharObj.GetConsummateLevel())
		{
			return true;
		}
		sbyte grade = DomainManager.Combat.GetUsingWeapon(base.CurrEnemyChar).GetGrade();
		int num = -1;
		ItemKey itemKey = base.CombatChar.Armors[attackBodyPart];
		if (itemKey.IsValid())
		{
			GameData.Domains.Item.Armor element_Armors = DomainManager.Item.GetElement_Armors(itemKey.Id);
			if (element_Armors.GetCurrDurability() > 0)
			{
				num = element_Armors.GetGrade();
			}
		}
		return num >= 0 && num + 2 > grade;
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !_affecting || dataKey.CustomParam0 != ((!base.IsDirect) ? 1 : 0))
		{
			return 0;
		}
		if (dataKey.FieldId == 102)
		{
			return -60;
		}
		return 0;
	}
}
