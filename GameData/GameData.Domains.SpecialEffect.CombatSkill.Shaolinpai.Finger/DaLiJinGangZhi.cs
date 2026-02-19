using System;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Finger;

public class DaLiJinGangZhi : AddWeaponEquipAttackOnAttack
{
	private const int ChangeCount = 3;

	private int _flawCount;

	private int _acupointCount;

	protected override short AddWeaponEquipAttack => 1000;

	public DaLiJinGangZhi()
	{
	}

	public DaLiJinGangZhi(CombatSkillKey skillKey)
		: base(skillKey, 1203)
	{
	}

	protected override void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			sbyte skillAttackBodyPart = base.CombatChar.SkillAttackBodyPart;
			CombatCharacter currEnemyChar = base.CurrEnemyChar;
			FlawOrAcupointCollection flawCollection = currEnemyChar.GetFlawCollection();
			FlawOrAcupointCollection acupointCollection = currEnemyChar.GetAcupointCollection();
			_flawCount = flawCollection.BodyPartDict[skillAttackBodyPart].Count;
			_acupointCount = acupointCollection.BodyPartDict[skillAttackBodyPart].Count;
			base.OnPrepareSkillEnd(context, charId, isAlly, skillId);
		}
	}

	protected override void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power))
		{
			sbyte skillAttackBodyPart = base.CombatChar.SkillAttackBodyPart;
			CombatCharacter currEnemyChar = base.CurrEnemyChar;
			Injuries injuries = currEnemyChar.GetInjuries();
			if (_flawCount > 0 || _acupointCount > 0)
			{
				if (_flawCount > 0)
				{
					int num = Math.Min(3, _flawCount);
					int num2 = Math.Min(num, 6 - injuries.Get(skillAttackBodyPart, !base.IsDirect));
					currEnemyChar.RemoveRandomFlawOrAcupoint(context, isFlaw: true, num);
					DomainManager.Combat.AddInjury(context, currEnemyChar, skillAttackBodyPart, !base.IsDirect, (sbyte)num2);
					DomainManager.Combat.AppendFatalDamageMark(context, currEnemyChar, num - num2, (!base.IsDirect) ? ((sbyte)1) : ((sbyte)0), skillAttackBodyPart);
				}
				if (_acupointCount > 0)
				{
					int num3 = Math.Min(3, _flawCount);
					int num4 = Math.Min(num3, 6 - injuries.Get(skillAttackBodyPart, !base.IsDirect));
					currEnemyChar.RemoveRandomFlawOrAcupoint(context, isFlaw: false, num3);
					DomainManager.Combat.AddInjury(context, currEnemyChar, skillAttackBodyPart, !base.IsDirect, (sbyte)num4);
					DomainManager.Combat.AppendFatalDamageMark(context, currEnemyChar, num3 - num4, (!base.IsDirect) ? ((sbyte)1) : ((sbyte)0), skillAttackBodyPart);
				}
				DomainManager.Combat.UpdateBodyDefeatMark(context, currEnemyChar, skillAttackBodyPart);
				ShowSpecialEffectTips(1);
			}
		}
		base.OnCastSkillEnd(context, charId, isAlly, skillId, power, interrupted);
	}
}
