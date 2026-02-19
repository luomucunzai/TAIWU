using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist;

public class SheHunDaFa : DefenseSkillBase
{
	private const int SilenceFrame = 3000;

	private static bool _affectingTrick;

	private static bool _affectingMindMark;

	private readonly Dictionary<int, List<short>> _silencedSkills = new Dictionary<int, List<short>>();

	public SheHunDaFa()
	{
	}

	public SheHunDaFa(CombatSkillKey skillKey)
		: base(skillKey, 7503)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		AutoRemove = false;
		Events.RegisterHandler_GetTrick(OnGetTrick);
		Events.RegisterHandler_AddMindMark(OnAddMindMark);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_GetTrick(OnGetTrick);
		Events.UnRegisterHandler_AddMindMark(OnAddMindMark);
		base.OnDisable(context);
	}

	private void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
	{
		if (charId == base.CharacterId && trickType == 20 && !_affectingTrick && base.CanAffect && base.IsDirect)
		{
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!isAlly);
			_affectingTrick = true;
			DomainManager.Combat.AddTrick(context, combatCharacter, 20, 1, addedByAlly: false);
			_affectingTrick = false;
			ShowSpecialEffectTips(0);
			DoSilence(context, combatCharacter);
		}
	}

	private void OnAddMindMark(DataContext context, CombatCharacter character, int count)
	{
		if (character.GetId() == base.CharacterId && count > 0 && !_affectingMindMark && base.CanAffect && !base.IsDirect)
		{
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!character.IsAlly);
			_affectingMindMark = true;
			DomainManager.Combat.AppendMindDefeatMark(context, combatCharacter, 1, -1);
			_affectingMindMark = false;
			ShowSpecialEffectTips(0);
			DoSilence(context, combatCharacter);
		}
	}

	private void DoSilence(DataContext context, CombatCharacter enemyChar)
	{
		if (base.CombatChar.GetCharacter().GetHitValues()[3] > enemyChar.GetCharacter().GetAvoidValues()[3])
		{
			List<short> value;
			short randomBanableSkillId = enemyChar.GetRandomBanableSkillId(context.Random, (short x) => !_silencedSkills.TryGetValue(enemyChar.GetId(), out value) || !value.Contains(x), -1);
			if (randomBanableSkillId >= 0)
			{
				DomainManager.Combat.SilenceSkill(context, enemyChar, randomBanableSkillId, 3000);
				_silencedSkills.GetOrNew(enemyChar.GetId()).Add(randomBanableSkillId);
				ShowSpecialEffectTips(1);
			}
		}
	}
}
