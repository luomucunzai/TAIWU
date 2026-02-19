using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music;

public class YuFeiYin : CombatSkillEffectBase
{
	private const sbyte StartCastNeiliAllocation = 3;

	private const sbyte FullPowerNeiliAllocation = 6;

	public YuFeiYin()
	{
	}

	public YuFeiYin(CombatSkillKey skillKey)
		: base(skillKey, 3303, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		ChangeAllNeiliAllocation(context, 3);
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
			if (PowerMatchAffectRequire(power))
			{
				ChangeAllNeiliAllocation(context, 6);
			}
			RemoveSelf(context);
		}
	}

	private unsafe void ChangeAllNeiliAllocation(DataContext context, sbyte changeValue)
	{
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : base.CurrEnemyChar);
		NeiliAllocation neiliAllocation = combatCharacter.GetNeiliAllocation();
		NeiliAllocation originNeiliAllocation = combatCharacter.GetOriginNeiliAllocation();
		bool flag = false;
		for (byte b = 0; b < 4; b++)
		{
			if (base.IsDirect ? (neiliAllocation.Items[(int)b] < originNeiliAllocation.Items[(int)b]) : (neiliAllocation.Items[(int)b] > originNeiliAllocation.Items[(int)b]))
			{
				combatCharacter.ChangeNeiliAllocation(context, b, base.IsDirect ? changeValue : (-changeValue));
				flag = true;
			}
		}
		if (flag)
		{
			ShowSpecialEffectTips(0);
		}
	}
}
