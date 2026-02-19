using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public abstract class CurseSilenceCombatSkill : CombatSkillEffectBase
{
	private const int DirectSilenceCount = 1;

	private const int DirectSilenceFrame = 900;

	private const int ReverseSilenceCount = 2;

	private const int ReverseSilenceFrame = 1800;

	private const int ReverseSilenceSelfFrame = 1800;

	private const int AddPowerPercent = 40;

	private readonly HashSet<CombatSkillKey> _silencingSkills = new HashSet<CombatSkillKey>();

	protected abstract sbyte TargetEquipType { get; }

	protected IReadOnlySet<CombatSkillKey> SilencingSkills => _silencingSkills;

	protected CurseSilenceCombatSkill()
	{
	}

	protected CurseSilenceCombatSkill(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		if (base.IsDirect)
		{
			CreateAffectedData(199, (EDataModifyType)1, base.SkillTemplateId);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_SkillSilenceEnd(OnSkillSilenceEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_SkillSilenceEnd(OnSkillSilenceEnd);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.SkillKey != SkillKey)
		{
			return 0;
		}
		return (dataKey.FieldId == 199) ? (_silencingSkills.Count * 40) : 0;
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power))
		{
			bool flag = false;
			CombatCharacter currEnemyChar = base.CurrEnemyChar;
			int maxCount = (base.IsDirect ? 1 : 2);
			foreach (short randomUnrepeatedBanableSkillId in currEnemyChar.GetRandomUnrepeatedBanableSkillIds(context.Random, maxCount, null, TargetEquipType, -1))
			{
				DomainManager.Combat.SilenceSkill(context, currEnemyChar, randomUnrepeatedBanableSkillId, base.IsDirect ? 900 : 1800);
				CombatSkillKey combatSkillKey = new CombatSkillKey(currEnemyChar.GetId(), randomUnrepeatedBanableSkillId);
				_silencingSkills.Add(combatSkillKey);
				OnSilenceBegin(context, combatSkillKey);
				flag = true;
			}
			if (flag)
			{
				ShowSpecialEffectTips(0);
			}
			if (flag && base.IsDirect)
			{
				InvalidateCache(context, 199);
				ShowSpecialEffectTips(1);
			}
		}
		if (!base.IsDirect && !interrupted)
		{
			DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, 1800, -1);
			ShowSpecialEffectTips(1);
		}
	}

	private void OnSkillSilenceEnd(DataContext context, CombatSkillKey skillKey)
	{
		if (_silencingSkills.Contains(skillKey))
		{
			_silencingSkills.Remove(skillKey);
			InvalidateCache(context, 199);
			OnSilenceEnd(context, skillKey);
		}
	}

	protected abstract void OnSilenceBegin(DataContext context, CombatSkillKey skillKey);

	protected abstract void OnSilenceEnd(DataContext context, CombatSkillKey skillKey);
}
