using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.DefenseAndAssist;

public class LiuZhuanTianHuang : DefenseSkillBase
{
	private const int AbsorbMainAttributeValue = 3;

	private static readonly sbyte[] DirectAbsorbMainAttributeTypes = new sbyte[3] { 0, 3, 2 };

	private static readonly sbyte[] ReverseAbsorbMainAttributeTypes = new sbyte[3] { 1, 4, 5 };

	public LiuZhuanTianHuang()
	{
	}

	public LiuZhuanTianHuang(CombatSkillKey skillKey)
		: base(skillKey, 13502)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		Events.RegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		Events.UnRegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (!hit && defender == base.CombatChar && base.CanAffect && attacker.NormalAttackHitType == 2)
		{
			DoEffect(context);
		}
	}

	private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
	{
		if (!(context.Defender != base.CombatChar || hit) && base.CanAffect && index <= 2 && DomainManager.Combat.GetDamageCompareData().HitType[index] == 2)
		{
			DoEffect(context);
		}
	}

	private void DoEffect(DataContext context)
	{
		sbyte[] array = (base.IsDirect ? DirectAbsorbMainAttributeTypes : ReverseAbsorbMainAttributeTypes);
		GameData.Domains.Character.Character character = base.EnemyChar.GetCharacter();
		GameData.Domains.Character.Character charObj = CharObj;
		sbyte[] array2 = array;
		foreach (sbyte mainAttributeType in array2)
		{
			int num = Math.Min(3, (int)character.GetCurrMainAttribute(mainAttributeType));
			if (num > 0)
			{
				character.ChangeCurrMainAttribute(context, mainAttributeType, -num);
				charObj.ChangeCurrMainAttribute(context, mainAttributeType, num);
				ShowSpecialEffectTipsOnceInFrame(0);
			}
		}
	}
}
