using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade;

public class TaiYiJinDao : AttackBodyPart
{
	private const int RecoverAcupointKeepTimePercent = 100;

	public TaiYiJinDao()
	{
	}

	public TaiYiJinDao(CombatSkillKey skillKey)
		: base(skillKey, 5303)
	{
		BodyParts = new sbyte[2] { 5, 6 };
		ReverseAddDamagePercent = 45;
	}

	protected override void OnCastAffectPower(DataContext context)
	{
		CombatCharacter currEnemyChar = base.CurrEnemyChar;
		FlawOrAcupointCollection acupointCollection = currEnemyChar.GetAcupointCollection();
		acupointCollection.OfflineRecoverKeepTimePercent(100);
		currEnemyChar.SetAcupointCollection(acupointCollection, context);
		ShowSpecialEffectTips(1);
	}
}
