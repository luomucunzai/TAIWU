using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang;

public class SilenceSkillAndMinAttribute : CombatSkillEffectBase
{
	protected sbyte SilenceSkillCount;

	private const short SilenceFrame = 2400;

	protected SilenceSkillAndMinAttribute()
	{
	}

	protected SilenceSkillAndMinAttribute(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedAllEnemyData(18, (EDataModifyType)3, -1);
		ShowSpecialEffectTips(2);
		CombatCharacter currEnemyChar = base.CurrEnemyChar;
		if (currEnemyChar.GetAffectingMoveSkillId() >= 0)
		{
			ClearAffectingAgileSkill(context, currEnemyChar);
			ShowSpecialEffectTipsOnceInFrame(0);
		}
		if (currEnemyChar.GetAffectingDefendSkillId() >= 0)
		{
			DomainManager.Combat.ClearAffectingDefenseSkill(context, currEnemyChar);
			ShowSpecialEffectTipsOnceInFrame(0);
		}
		foreach (short randomUnrepeatedBanableSkillId in currEnemyChar.GetRandomUnrepeatedBanableSkillIds(context.Random, SilenceSkillCount, null, -1, 1))
		{
			DomainManager.Combat.SilenceSkill(context, currEnemyChar, randomUnrepeatedBanableSkillId, 2400);
			ShowSpecialEffectTipsOnceInFrame(1);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		short moveCd = combatCharacter.GetMoveCd();
		if (moveCd < combatCharacter.MoveData.MoveCd)
		{
			combatCharacter.MoveData.MoveCd = moveCd;
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			RemoveSelf(context);
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		return true;
	}
}
