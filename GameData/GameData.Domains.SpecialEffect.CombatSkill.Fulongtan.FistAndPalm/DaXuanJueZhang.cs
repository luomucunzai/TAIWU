using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm;

public class DaXuanJueZhang : CombatSkillEffectBase
{
	private const sbyte TransferPowerPercent = 10;

	public DaXuanJueZhang()
	{
	}

	public DaXuanJueZhang(CombatSkillKey skillKey)
		: base(skillKey, 14104, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		SkillEffectKey skillEffectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
		list.Clear();
		list.AddRange(base.CombatChar.GetCombatSkillList((sbyte)(base.IsDirect ? 3 : 2)));
		list.RemoveAll((short id) => id < 0);
		list2.Clear();
		list2.AddRange(combatCharacter.GetCombatSkillList((sbyte)(base.IsDirect ? 3 : 2)));
		list2.RemoveAll((short id) => id < 0);
		DomainManager.Combat.RemoveSkillPowerAddInCombat(context, SkillKey, skillEffectKey);
		if (list.Count > 0 || list2.Count > 0)
		{
			int num = 0;
			List<CombatSkillKey> list3 = ObjectPool<List<CombatSkillKey>>.Instance.Get();
			list3.Clear();
			if (list.Count > 0)
			{
				short num2 = 0;
				for (int num3 = 0; num3 < list.Count; num3++)
				{
					short skillTemplateId = list[num3];
					CombatSkillKey combatSkillKey = new CombatSkillKey(base.CharacterId, skillTemplateId);
					short power = DomainManager.CombatSkill.GetElement_CombatSkills(combatSkillKey).GetPower();
					if (power > num2)
					{
						list3.Clear();
						list3.Add(combatSkillKey);
						num2 = power;
					}
					else if (power == num2)
					{
						list3.Add(combatSkillKey);
					}
				}
				int num4 = num2 * 10 / 100;
				if (num4 > 0)
				{
					DomainManager.Combat.ReduceSkillPowerInCombat(context, list3[context.Random.Next(0, list3.Count)], skillEffectKey, -num4);
					num += num4;
				}
			}
			list3.Clear();
			if (list2.Count > 0)
			{
				short num2 = 0;
				for (int num5 = 0; num5 < list2.Count; num5++)
				{
					short skillTemplateId2 = list2[num5];
					CombatSkillKey combatSkillKey2 = new CombatSkillKey(combatCharacter.GetId(), skillTemplateId2);
					short power2 = DomainManager.CombatSkill.GetElement_CombatSkills(combatSkillKey2).GetPower();
					if (power2 > num2)
					{
						list3.Clear();
						list3.Add(combatSkillKey2);
						num2 = power2;
					}
					else if (power2 == num2)
					{
						list3.Add(combatSkillKey2);
					}
				}
				int num6 = num2 * 10 / 100;
				if (num6 > 0)
				{
					DomainManager.Combat.ReduceSkillPowerInCombat(context, list3[context.Random.Next(0, list3.Count)], skillEffectKey, -num6);
					num += num6;
				}
			}
			ObjectPool<List<CombatSkillKey>>.Instance.Return(list3);
			if (num > 0)
			{
				DomainManager.Combat.AddSkillPowerInCombat(context, SkillKey, skillEffectKey, num);
				ShowSpecialEffectTips(0);
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
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
			RemoveSelf(context);
		}
	}
}
