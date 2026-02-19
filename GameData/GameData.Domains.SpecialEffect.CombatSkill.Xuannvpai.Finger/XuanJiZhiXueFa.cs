using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Finger;

public class XuanJiZhiXueFa : CombatSkillEffectBase
{
	private sbyte[] _maxLevel = new sbyte[7];

	public XuanJiZhiXueFa()
	{
	}

	public XuanJiZhiXueFa(CombatSkillKey skillKey)
		: base(skillKey, 8206, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		FlawOrAcupointCollection flawOrAcupointCollection = (base.IsDirect ? base.CurrEnemyChar.GetFlawCollection() : base.CurrEnemyChar.GetAcupointCollection());
		for (sbyte b = 0; b < 7; b++)
		{
			_maxLevel[b] = -1;
			foreach (var item in flawOrAcupointCollection.BodyPartDict[b])
			{
				if (item.level > _maxLevel[b])
				{
					_maxLevel[b] = item.level;
				}
			}
		}
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
			for (sbyte b = 0; b < 7; b++)
			{
				sbyte b2 = _maxLevel[b];
				if (b2 >= 0)
				{
					if (base.IsDirect)
					{
						DomainManager.Combat.AddAcupoint(context, base.CurrEnemyChar, b2, SkillKey, b);
					}
					else
					{
						DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, b2, SkillKey, b);
					}
					flag = true;
				}
			}
			if (flag)
			{
				ShowSpecialEffectTips(0);
				DomainManager.Combat.AddToCheckFallenSet(base.CurrEnemyChar.GetId());
			}
		}
		RemoveSelf(context);
	}
}
