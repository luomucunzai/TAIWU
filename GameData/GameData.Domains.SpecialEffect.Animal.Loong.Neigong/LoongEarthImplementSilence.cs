using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong;

public class LoongEarthImplementSilence : ISpecialEffectImplement, ISpecialEffectModifier
{
	public int SilenceFrame;

	public CombatSkillEffectBase EffectBase { get; set; }

	public virtual void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public virtual void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (isAlly != EffectBase.CombatChar.IsAlly && EffectBase.PowerMatchAffectRequire(power) && Config.CombatSkill.Instance[skillId].EquipType == 1 && DomainManager.Combat.IsCurrentCombatCharacter(EffectBase.CombatChar))
		{
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
			short randomBanableSkillId = element_CombatCharacterDict.GetRandomBanableSkillId(context.Random, null, -1);
			if (randomBanableSkillId >= 0)
			{
				DoSilence(context, element_CombatCharacterDict, randomBanableSkillId);
			}
		}
	}

	private void DoSilence(DataContext context, CombatCharacter combatChar, short skillId)
	{
		Tester.Assert(SilenceFrame > 0, "SilenceFrame > 0");
		if (DomainManager.Combat.SilenceSkill(context, combatChar, skillId, SilenceFrame))
		{
			EffectBase.ShowSpecialEffectTips(0);
			if (this is ILoongEarthExtra loongEarthExtra)
			{
				loongEarthExtra.OnSilenced(context, combatChar, skillId);
			}
		}
	}

	public virtual int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		return dataValue;
	}
}
