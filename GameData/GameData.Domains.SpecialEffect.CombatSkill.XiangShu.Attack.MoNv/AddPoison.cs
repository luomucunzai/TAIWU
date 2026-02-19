using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.MoNv;

public class AddPoison : CombatSkillEffectBase
{
	private const short AffectFrameCount = 30;

	private const short PoisonPercent = 20;

	protected sbyte PoisonTypeCount;

	private int _frameCounter;

	protected AddPoison()
	{
	}

	protected AddPoison(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private unsafe void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		if (base.CombatChar != combatChar || DomainManager.Combat.Pause)
		{
			return;
		}
		_frameCounter++;
		if (_frameCounter >= 30 && DomainManager.Combat.InAttackRange(base.CombatChar))
		{
			_frameCounter = 0;
			PoisonsAndLevels poisons = base.SkillInstance.GetPoisons();
			int power = base.SkillInstance.GetPower();
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			list.Clear();
			for (sbyte b = 0; b < 6; b++)
			{
				list.Add(b);
			}
			while (list.Count > PoisonTypeCount)
			{
				list.RemoveAt(context.Random.Next(list.Count));
			}
			for (int i = 0; i < list.Count; i++)
			{
				sbyte b2 = list[i];
				DomainManager.Combat.AddPoison(context, base.CombatChar, combatCharacter, b2, poisons.Levels[b2], poisons.Values[b2] * power / 100 * 20 / 100, base.SkillTemplateId);
			}
			ObjectPool<List<sbyte>>.Instance.Return(list);
			DomainManager.Combat.AddToCheckFallenSet(combatCharacter.GetId());
			ShowSpecialEffectTips(0);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			RemoveSelf(context);
		}
	}
}
