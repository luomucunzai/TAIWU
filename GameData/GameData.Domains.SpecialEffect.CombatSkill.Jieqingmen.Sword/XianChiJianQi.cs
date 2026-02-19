using System.Collections.Generic;
using System.Linq;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword;

public class XianChiJianQi : AttackBodyPart
{
	private const int ReducePowerValue = -20;

	public XianChiJianQi()
	{
	}

	public XianChiJianQi(CombatSkillKey skillKey)
		: base(skillKey, 13202)
	{
		BodyParts = new sbyte[1] { 2 };
		ReverseAddDamagePercent = 30;
	}

	protected override void OnCastAffectPower(DataContext context)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		int enemyCharId = combatCharacter.GetId();
		List<short> defenceSkillList = combatCharacter.GetDefenceSkillList();
		SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list.AddRange(defenceSkillList.Where((short x) => x >= 0 && DomainManager.Combat.GetReduceSkillPowerInCombat(new CombatSkillKey(enemyCharId, x), effectKey) == 0));
		short num = (short)((list.Count > 0) ? list.GetRandom(context.Random) : (-1));
		ObjectPool<List<short>>.Instance.Return(list);
		if (num >= 0)
		{
			DomainManager.Combat.ReduceSkillPowerInCombat(context, new CombatSkillKey(enemyCharId, num), effectKey, -20);
			ShowSpecialEffectTips(1);
		}
	}
}
