using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot;

public class TaiShanSuo : CombatSkillEffectBase
{
	private const sbyte AddAcupointLevel = 1;

	private readonly sbyte[] _directBodyParts = new sbyte[2] { 5, 6 };

	private readonly sbyte[] _reverseBodyParts = new sbyte[2] { 3, 4 };

	public TaiShanSuo()
	{
	}

	public TaiShanSuo(CombatSkillKey skillKey)
		: base(skillKey, 9405, -1)
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
		byte trickCount = base.CombatChar.GetTrickCount(12);
		if (PowerMatchAffectRequire(power) && trickCount > 0)
		{
			CombatCharacter currEnemyChar = base.CurrEnemyChar;
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			sbyte[] array = (base.IsDirect ? _directBodyParts : _reverseBodyParts);
			byte[] acupointCount = currEnemyChar.GetAcupointCount();
			int maxAcupointCount = currEnemyChar.GetMaxAcupointCount();
			list.Clear();
			foreach (sbyte b in array)
			{
				int num = maxAcupointCount - acupointCount[b];
				for (int j = 0; j < num; j++)
				{
					list.Add(b);
				}
			}
			if (list.Count > 0)
			{
				int num2 = Math.Min(trickCount, list.Count);
				int num3 = GlobalConfig.Instance.FlawBaseKeepTime[1];
				for (int k = 0; k < num2; k++)
				{
					sbyte b2 = list[context.Random.Next(0, list.Count)];
					list.Remove(b2);
					currEnemyChar.AddOrUpdateFlawOrAcupoint(context, b2, isFlaw: false, 1, raiseEvent: true, num3, num3);
				}
				DomainManager.Combat.AddToCheckFallenSet(currEnemyChar.GetId());
			}
			DomainManager.Combat.RemoveTrick(context, base.CombatChar, 12, trickCount);
			ShowSpecialEffectTips(0);
			ObjectPool<List<sbyte>>.Instance.Return(list);
		}
		RemoveSelf(context);
	}
}
