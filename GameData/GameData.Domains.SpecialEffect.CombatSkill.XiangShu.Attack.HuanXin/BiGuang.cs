using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.HuanXin;

public class BiGuang : CombatSkillEffectBase
{
	private const sbyte ChangeStateCount = 2;

	private const short SilenceFrame = 600;

	public BiGuang()
	{
	}

	public BiGuang(CombatSkillKey skillKey)
		: base(skillKey, 17104, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		Dictionary<short, (short, bool, int)> stateDict = base.CombatChar.GetDebuffCombatStateCollection().StateDict;
		Dictionary<short, (short, bool, int)> stateDict2 = combatCharacter.GetBuffCombatStateCollection().StateDict;
		if (stateDict.Count > 0)
		{
			List<short> list = ObjectPool<List<short>>.Instance.Get();
			list.Clear();
			list.AddRange(stateDict.Keys);
			int num = Math.Min(2, list.Count);
			for (int i = 0; i < num; i++)
			{
				int index = context.Random.Next(0, list.Count);
				short stateId = list[index];
				list.RemoveAt(index);
				DomainManager.Combat.ReverseCombatState(context, base.CombatChar, 2, stateId);
			}
			ObjectPool<List<short>>.Instance.Return(list);
		}
		if (stateDict2.Count > 0)
		{
			List<short> list2 = ObjectPool<List<short>>.Instance.Get();
			list2.Clear();
			list2.AddRange(stateDict2.Keys);
			int num2 = Math.Min(2, list2.Count);
			for (int j = 0; j < num2; j++)
			{
				int index2 = context.Random.Next(0, list2.Count);
				short stateId2 = list2[index2];
				list2.RemoveAt(index2);
				DomainManager.Combat.ReverseCombatState(context, combatCharacter, 1, stateId2);
			}
			ObjectPool<List<short>>.Instance.Return(list2);
		}
		if (stateDict.Count > 0 || stateDict2.Count > 0)
		{
			ShowSpecialEffectTips(0);
		}
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
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
			foreach (short banableSkillId in combatCharacter.GetBanableSkillIds(3, -1))
			{
				DomainManager.Combat.SilenceSkill(context, combatCharacter, banableSkillId, 600);
			}
			ShowSpecialEffectTips(1);
		}
		RemoveSelf(context);
	}
}
