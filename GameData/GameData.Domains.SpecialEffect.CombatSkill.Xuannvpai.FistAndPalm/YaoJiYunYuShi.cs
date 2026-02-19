using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.FistAndPalm;

public class YaoJiYunYuShi : PowerUpOnCast
{
	protected override EDataModifyType ModifyType => (EDataModifyType)1;

	public YaoJiYunYuShi()
	{
	}

	public YaoJiYunYuShi(CombatSkillKey skillKey)
		: base(skillKey, 8105)
	{
	}

	public override void OnEnable(DataContext context)
	{
		GameData.Domains.Character.Character character = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true).GetCharacter();
		GameData.Domains.Character.Character character2 = (base.IsDirect ? character : CharObj);
		GameData.Domains.Character.Character character3 = (base.IsDirect ? CharObj : character);
		if (character.GetCreatingType() == 1 && (character2.GetBisexual() || character3.GetAvatar().Gender == Gender.Flip(character2.GetGender())))
		{
			PowerUpValue = character3.GetAttraction() / 15;
		}
		base.OnEnable(context);
	}

	protected override void OnCastSelf(DataContext context, sbyte power, bool interrupted)
	{
		if (PowerMatchAffectRequire(power) && AllowAddAdore(base.CurrEnemyChar.GetId()))
		{
			GameData.Domains.Character.Character character = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true).GetCharacter();
			GameData.Domains.Character.Character character2 = (base.IsDirect ? character : CharObj);
			GameData.Domains.Character.Character character3 = (base.IsDirect ? CharObj : character);
			int percentProb = character3.GetAttraction() / 100;
			if (context.Random.CheckPercentProb(percentProb))
			{
				GameData.Domains.Character.Character.ApplyAddRelation_Adore(context, character2, character3, character2.GetBehaviorType(), targetLovesBack: false, selfIsTaiwuPeople: false, targetIsTaiwuPeople: false);
				ShowSpecialEffectTips(1);
			}
		}
	}

	private bool AllowAddAdore(int relatedCharId)
	{
		int characterId = base.CharacterId;
		if (!DomainManager.Character.TryGetElement_Objects(relatedCharId, out var element))
		{
			return false;
		}
		if (element.GetAgeGroup() != 2 || element.GetCreatingType() != 1)
		{
			return false;
		}
		if (!DomainManager.Character.TryGetRelation(characterId, relatedCharId, out var relation))
		{
			relation.RelationType = ushort.MaxValue;
		}
		if (relation.RelationType == ushort.MaxValue)
		{
			return true;
		}
		return !RelationType.HasRelation(relation.RelationType, 511);
	}
}
