using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm;

public class DaShenWeiZhang : CombatSkillEffectBase
{
	private const int AddPowerPercent = 20;

	private const int SilenceFrame = 3000;

	private int _addPower;

	public DaShenWeiZhang()
	{
	}

	public DaShenWeiZhang(CombatSkillKey skillKey)
		: base(skillKey, 6107, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId && DomainManager.Combat.InAttackRange(base.CombatChar))
		{
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true);
			short skillTemplateId = (base.IsDirect ? combatCharacter.GetAffectingDefendSkillId() : combatCharacter.GetAffectingMoveSkillId());
			if (DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(combatCharacter.GetId(), skillTemplateId), out var element))
			{
				_addPower = element.GetPower() * 20 / 100;
				AppendAffectedData(context, base.CharacterId, 199, (EDataModifyType)1, base.SkillTemplateId);
				ShowSpecialEffectTips(0);
			}
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true);
		if (PowerMatchAffectRequire(power) && (base.IsDirect ? combatCharacter.GetAffectingDefendSkillId() : combatCharacter.GetAffectingMoveSkillId()) >= 0)
		{
			short num = (base.IsDirect ? combatCharacter.GetAffectingDefendSkillId() : combatCharacter.GetAffectingMoveSkillId());
			if (base.IsDirect)
			{
				DomainManager.Combat.ClearAffectingDefenseSkill(context, combatCharacter);
			}
			else
			{
				ClearAffectingAgileSkill(DomainManager.Combat.Context, combatCharacter);
			}
			if (num >= 0)
			{
				DomainManager.Combat.SilenceSkill(context, combatCharacter, num, 3000);
			}
			ShowSpecialEffectTips(1);
			ShowSpecialEffectTips(2);
		}
		RemoveSelf(context);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return _addPower;
		}
		return 0;
	}
}
