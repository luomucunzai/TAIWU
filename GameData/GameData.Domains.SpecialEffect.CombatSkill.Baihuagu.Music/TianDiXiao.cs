using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music;

public class TianDiXiao : CombatSkillEffectBase
{
	private sbyte ChangePower = 40;

	public TianDiXiao()
	{
	}

	public TianDiXiao(CombatSkillKey skillKey)
		: base(skillKey, 3306, -1)
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
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power))
		{
			CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true));
			Dictionary<CombatSkillKey, SkillPowerChangeCollection> dictionary = (base.IsDirect ? DomainManager.Combat.GetAllSkillPowerAddInCombat() : DomainManager.Combat.GetAllSkillPowerReduceInCombat());
			SkillEffectKey skillEffectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			List<short> list = ObjectPool<List<short>>.Instance.Get();
			bool flag = false;
			sbyte equipType;
			for (equipType = 1; equipType < 5; equipType++)
			{
				list.Clear();
				if (combatCharacter.BossConfig == null)
				{
					list.AddRange(combatCharacter.GetCombatSkillList(equipType));
				}
				else
				{
					list.AddRange(combatCharacter.GetCharacter().GetLearnedCombatSkills().FindAll((short id) => Config.CombatSkill.Instance[id].EquipType == equipType));
				}
				for (int num = list.Count - 1; num >= 0; num--)
				{
					short num2 = list[num];
					if (num2 < 0)
					{
						list.RemoveAt(num);
					}
					else
					{
						CombatSkillKey key = new CombatSkillKey(combatCharacter.GetId(), num2);
						if (dictionary.ContainsKey(key) && dictionary[key].EffectDict != null && dictionary[key].EffectDict.ContainsKey(skillEffectKey))
						{
							list.RemoveAt(num);
						}
					}
				}
				if (list.Count > 0)
				{
					if (base.IsDirect)
					{
						DomainManager.Combat.AddSkillPowerInCombat(context, new CombatSkillKey(combatCharacter.GetId(), list[context.Random.Next(0, list.Count)]), skillEffectKey, ChangePower);
					}
					else
					{
						DomainManager.Combat.ReduceSkillPowerInCombat(context, new CombatSkillKey(combatCharacter.GetId(), list[context.Random.Next(0, list.Count)]), skillEffectKey, -ChangePower);
					}
					flag = true;
				}
			}
			if (flag)
			{
				ShowSpecialEffectTips(0);
			}
			ObjectPool<List<short>>.Instance.Return(list);
		}
		RemoveSelf(context);
	}
}
