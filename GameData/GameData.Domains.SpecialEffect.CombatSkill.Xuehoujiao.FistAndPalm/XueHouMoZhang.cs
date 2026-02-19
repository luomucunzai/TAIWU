using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.FistAndPalm;

public class XueHouMoZhang : PoisonAddInjury
{
	public XueHouMoZhang()
	{
	}

	public XueHouMoZhang(CombatSkillKey skillKey)
		: base(skillKey, 15107)
	{
		RequirePoisonType = 3;
	}

	protected override void OnCastMaxPower(DataContext context)
	{
		CombatCharacter currEnemyChar = base.CurrEnemyChar;
		byte b = (base.IsDirect ? currEnemyChar : base.CombatChar).GetDefeatMarkCollection().PoisonMarkList[RequirePoisonType];
		if (b > 0)
		{
			DomainManager.Combat.AppendFatalDamageMark(context, currEnemyChar, b, -1, -1);
			for (int i = 0; i < b; i++)
			{
				Injuries injuries = currEnemyChar.GetInjuries();
				bool isInner = !injuries.AllPartsFully(isInnerInjury: true) && (injuries.AllPartsFully(isInnerInjury: false) || context.Random.CheckPercentProb(50));
				sbyte lightestPart = injuries.GetLightestPart(isInner);
				if (lightestPart >= 0)
				{
					DomainManager.Combat.AddInjury(context, currEnemyChar, lightestPart, isInner, 1, updateDefeatMark: true);
				}
			}
			DomainManager.Combat.AddToCheckFallenSet(currEnemyChar.GetId());
		}
		ShowSpecialEffectTips(1);
	}
}
