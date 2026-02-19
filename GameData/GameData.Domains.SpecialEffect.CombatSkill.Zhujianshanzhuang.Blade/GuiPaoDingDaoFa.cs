using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade;

public class GuiPaoDingDaoFa : BladeUnlockEffectBase
{
	private const int MaxTransferCount = 5;

	private int AddDamagePercentPerMark => base.IsDirectOrReverseEffectDoubling ? 6 : 3;

	protected override IEnumerable<short> RequireWeaponTypes
	{
		get
		{
			yield return 14;
			yield return 15;
		}
	}

	public GuiPaoDingDaoFa()
	{
	}

	public GuiPaoDingDaoFa(CombatSkillKey skillKey)
		: base(skillKey, 9204)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(69, (EDataModifyType)1, -1);
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		base.OnDisable(context);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (SkillKey.IsMatch(attacker.GetId(), skillId) && base.IsReverseOrUsingDirectWeapon)
		{
			ShowSpecialEffectTips(base.IsDirect, 1, 0);
		}
	}

	protected override bool CanDoAffect()
	{
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		return defeatMarkCollection.MindMarkList.Count > 0 || defeatMarkCollection.GetTotalFlawCount() > 0 || defeatMarkCollection.GetTotalAcupointCount() > 0;
	}

	public override void DoAffectAfterCost(DataContext context, int weaponIndex)
	{
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Add(defeatMarkCollection.GetTotalFlawCount());
		list.Add(defeatMarkCollection.GetTotalAcupointCount());
		list.Add(defeatMarkCollection.MindMarkList.Count);
		if (list.Sum() > 0)
		{
			ShowSpecialEffectTips(base.IsDirect, 2, 1);
		}
		for (int i = 0; i < 5; i++)
		{
			if (list.Sum() == 0)
			{
				break;
			}
			int randomIndex = RandomUtils.GetRandomIndex(list, context.Random);
			if (randomIndex == 0)
			{
				DomainManager.Combat.TransferRandomFlaw(context, base.CombatChar, base.EnemyChar);
			}
			if (randomIndex == 1)
			{
				DomainManager.Combat.TransferRandomAcupoint(context, base.CombatChar, base.EnemyChar);
			}
			if (randomIndex == 2)
			{
				DomainManager.Combat.TransferRandomMindDefeatMark(context, base.CombatChar, base.EnemyChar);
			}
			list[randomIndex]--;
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.SkillKey != SkillKey || !base.IsReverseOrUsingDirectWeapon)
		{
			return 0;
		}
		if (dataKey.FieldId == 69)
		{
			return base.CombatChar.GetDefeatMarkCollection().GetTotalCount() * AddDamagePercentPerMark;
		}
		return 0;
	}
}
