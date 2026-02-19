using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip;

public class GouHunSuiGuBian : AttackBodyPart
{
	private const int RecoverFlawKeepTimePercent = 100;

	public GouHunSuiGuBian()
	{
	}

	public GouHunSuiGuBian(CombatSkillKey skillKey)
		: base(skillKey, 12403)
	{
		BodyParts = new sbyte[2] { 5, 6 };
		ReverseAddDamagePercent = 45;
	}

	protected override void OnCastAffectPower(DataContext context)
	{
		CombatCharacter currEnemyChar = base.CurrEnemyChar;
		FlawOrAcupointCollection flawCollection = currEnemyChar.GetFlawCollection();
		flawCollection.OfflineRecoverKeepTimePercent(100);
		currEnemyChar.SetFlawCollection(flawCollection, context);
		ShowSpecialEffectTips(1);
	}
}
