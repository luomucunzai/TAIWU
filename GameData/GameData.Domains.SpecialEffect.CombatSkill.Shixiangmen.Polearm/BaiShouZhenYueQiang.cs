using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm;

public class BaiShouZhenYueQiang : CombatSkillEffectBase
{
	private const short WeaponSilenceFrame = 300;

	public BaiShouZhenYueQiang()
	{
	}

	public BaiShouZhenYueQiang(CombatSkillKey skillKey)
		: base(skillKey, 6305, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		if ((base.IsDirect ? combatCharacter.GetAffectingDefendSkillId() : combatCharacter.GetAffectingMoveSkillId()) < 0 && combatCharacter.ChangeToEmptyHandOrOther(context))
		{
			ItemKey[] weapons = combatCharacter.GetWeapons();
			for (int i = 0; i < 7; i++)
			{
				if (i != combatCharacter.GetUsingWeaponIndex() && weapons[i].IsValid())
				{
					DomainManager.Combat.SilenceWeapon(context, combatCharacter, i, 300);
				}
			}
			ShowSpecialEffectTips(0);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			RemoveSelf(context);
		}
	}
}
