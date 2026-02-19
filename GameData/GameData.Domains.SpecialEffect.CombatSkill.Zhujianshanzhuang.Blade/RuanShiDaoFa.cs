using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade;

public class RuanShiDaoFa : BladeUnlockEffectBase
{
	private int StealTrickCount => (!base.IsDirectOrReverseEffectDoubling) ? 1 : 2;

	protected override IEnumerable<short> RequireWeaponTypes
	{
		get
		{
			yield return 6;
			yield return 7;
		}
	}

	public RuanShiDaoFa()
	{
	}

	public RuanShiDaoFa(CombatSkillKey skillKey)
		: base(skillKey, 9201)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		base.OnDisable(context);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (SkillKey.IsMatch(charId, skillId) && PowerMatchAffectRequire(power) && base.IsReverseOrUsingDirectWeapon)
		{
			DoStealTrick(context);
		}
	}

	private void DoStealTrick(DataContext context)
	{
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		foreach (sbyte value in base.EnemyChar.GetTricks().Tricks.Values)
		{
			if (base.CombatChar.IsTrickUsable(value))
			{
				list.Add(value);
			}
		}
		if (list.Count > 0)
		{
			List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
			list2.AddRange(RandomUtils.GetRandomUnrepeated(context.Random, StealTrickCount, list));
			DomainManager.Combat.StealTrick(context, base.CombatChar, base.EnemyChar, list2);
			ObjectPool<List<sbyte>>.Instance.Return(list2);
			ShowSpecialEffectTips(base.IsDirect, 1, 0);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	public override void DoAffectAfterCost(DataContext context, int weaponIndex)
	{
		CombatWeaponData weaponData = base.CombatChar.GetWeaponData(weaponIndex);
		sbyte[] weaponTricks = weaponData.GetWeaponTricks();
		DomainManager.Combat.AddTrick(context, base.CombatChar, weaponTricks);
		ShowSpecialEffectTips(base.IsDirect, 2, 1);
	}
}
