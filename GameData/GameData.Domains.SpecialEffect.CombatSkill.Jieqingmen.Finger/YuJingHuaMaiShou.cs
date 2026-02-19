using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger;

public class YuJingHuaMaiShou : CombatSkillEffectBase
{
	private const sbyte BaseNeiliAllocation = 3;

	private const sbyte ExtraNeiliAllocation = 3;

	private const sbyte ExtraCostNeiliAllocation = 3;

	public YuJingHuaMaiShou()
	{
	}

	public YuJingHuaMaiShou(CombatSkillKey skillKey)
		: base(skillKey, 13102, -1)
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
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if (PowerMatchAffectRequire(power))
			{
				DoAffect(context);
			}
			RemoveSelf(context);
		}
	}

	private void DoAffect(DataContext context)
	{
		byte b = RandomNeiliAllocationType(context);
		if (b != byte.MaxValue && base.CombatChar.AbsorbNeiliAllocation(context, base.CurrEnemyChar, b, 3))
		{
			ShowSpecialEffectTips(0);
			NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
			NeiliAllocation neiliAllocation2 = base.CurrEnemyChar.GetNeiliAllocation();
			NeiliAllocation originNeiliAllocation = base.CombatChar.GetOriginNeiliAllocation();
			NeiliAllocation originNeiliAllocation2 = base.CurrEnemyChar.GetOriginNeiliAllocation();
			bool flag = neiliAllocation[b] < originNeiliAllocation[b];
			bool flag2 = neiliAllocation2[b] > originNeiliAllocation2[b];
			if (!(base.IsDirect ? (!flag) : (!flag2)))
			{
				base.CombatChar.AbsorbNeiliAllocation(context, base.CurrEnemyChar, b, 3);
				base.CurrEnemyChar.ChangeNeiliAllocation(context, b, -3);
				ShowSpecialEffectTips(1);
			}
		}
	}

	private byte RandomNeiliAllocationType(DataContext context)
	{
		NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
		NeiliAllocation neiliAllocation2 = base.CurrEnemyChar.GetNeiliAllocation();
		List<byte> list = ObjectPool<List<byte>>.Instance.Get();
		short num = (short)(base.IsDirect ? short.MaxValue : 0);
		list.Clear();
		for (byte b = 0; b < 4; b++)
		{
			if (base.IsDirect ? (neiliAllocation[b] < num) : (neiliAllocation2[b] > num))
			{
				list.Clear();
				list.Add(b);
				num = (base.IsDirect ? neiliAllocation[b] : neiliAllocation2[b]);
			}
			else if (base.IsDirect ? (neiliAllocation[b] == num) : (neiliAllocation2[b] == num))
			{
				list.Add(b);
			}
		}
		byte result = ((list.Count > 0) ? list[context.Random.Next(0, list.Count)] : byte.MaxValue);
		ObjectPool<List<byte>>.Instance.Return(list);
		return result;
	}
}
