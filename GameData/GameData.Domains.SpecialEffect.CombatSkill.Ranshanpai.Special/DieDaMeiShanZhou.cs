using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special;

public class DieDaMeiShanZhou : CombatSkillEffectBase
{
	private static readonly CValuePercent DirectChangeMobility = CValuePercent.op_Implicit(-60);

	private static readonly CValuePercent ReverseChangeMobility = CValuePercent.op_Implicit(30);

	public DieDaMeiShanZhou()
	{
	}

	public DieDaMeiShanZhou(CombatSkillKey skillKey)
		: base(skillKey, 7300, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if (PowerMatchAffectRequire(power))
			{
				CombatCharacter character = (base.IsDirect ? base.CurrEnemyChar : base.CombatChar);
				CValuePercent val = (base.IsDirect ? DirectChangeMobility : ReverseChangeMobility);
				ChangeMobilityValue(context, character, MoveSpecialConstants.MaxMobility * val);
				ShowSpecialEffectTips(0);
			}
			RemoveSelf(context);
		}
	}
}
