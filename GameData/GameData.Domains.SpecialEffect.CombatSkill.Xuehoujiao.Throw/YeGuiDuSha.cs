using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Throw;

public class YeGuiDuSha : CombatSkillEffectBase
{
	private const sbyte ChangeTrickCount = 2;

	public YeGuiDuSha()
	{
	}

	public YeGuiDuSha(CombatSkillKey skillKey)
		: base(skillKey, 15400, -1)
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
			CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly));
			TrickCollection tricks = combatCharacter.GetTricks();
			IReadOnlyDictionary<int, sbyte> tricks2 = tricks.Tricks;
			List<int> list = ObjectPool<List<int>>.Instance.Get();
			list.Clear();
			foreach (KeyValuePair<int, sbyte> item in tricks2)
			{
				if (base.IsDirect == combatCharacter.IsTrickUseless(item.Value))
				{
					list.Add(item.Key);
				}
			}
			while (list.Count > 2)
			{
				list.RemoveAt(context.Random.Next(0, list.Count));
			}
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					tricks.ReplaceTrick(list[i], 14);
				}
				combatCharacter.SetTricks(tricks, context);
				ShowSpecialEffectTips(0);
			}
			ObjectPool<List<int>>.Instance.Return(list);
		}
		RemoveSelf(context);
	}
}
