using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi;

public class LongTunShi : CombatSkillEffectBase
{
	private const sbyte CostNeiliAllocation = 18;

	private DataUid _injuriesUid;

	public LongTunShi()
	{
	}

	public LongTunShi(CombatSkillKey skillKey)
		: base(skillKey, 17050, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
		if (IsSrcSkillPerformed)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_injuriesUid, base.DataHandlerKey);
		}
	}

	private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		NeiliAllocation neiliAllocation = combatCharacter.GetNeiliAllocation();
		List<byte> list = ObjectPool<List<byte>>.Instance.Get();
		list.Clear();
		for (byte b = 0; b < 4; b++)
		{
			if (neiliAllocation.Items[(int)b] >= 18)
			{
				list.Add(b);
			}
		}
		if (!IsSrcSkillPerformed)
		{
			if (list.Count > 0 && PowerMatchAffectRequire(power))
			{
				byte type = list[context.Random.Next(list.Count)];
				combatCharacter.ChangeNeiliAllocation(context, type, -18);
				if (CheckAndHealInjury(context))
				{
					RemoveSelf(context);
				}
				else
				{
					IsSrcSkillPerformed = true;
					AddMaxEffectCount();
					_injuriesUid = new DataUid(8, 10, (ulong)base.CharacterId, 29u);
					GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_injuriesUid, base.DataHandlerKey, OnInjuriesChanged);
				}
			}
			else
			{
				RemoveSelf(context);
			}
		}
		else if (list.Count > 0 && PowerMatchAffectRequire(power))
		{
			RemoveSelf(context);
		}
		ObjectPool<List<byte>>.Instance.Return(list);
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			RemoveSelf(context);
		}
	}

	private void OnInjuriesChanged(DataContext context, DataUid dataUid)
	{
		if (CheckAndHealInjury(context))
		{
			ReduceEffectCount();
		}
	}

	private bool CheckAndHealInjury(DataContext context)
	{
		Injuries injuries = base.CombatChar.GetInjuries();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			if (tuple.Item1 >= 6 || tuple.Item2 >= 6)
			{
				list.Add(b);
			}
		}
		bool flag = list.Count > 0;
		if (flag)
		{
			sbyte b2 = list[context.Random.Next(list.Count)];
			(sbyte, sbyte) tuple2 = injuries.Get(b2);
			if (tuple2.Item1 > 0)
			{
				DomainManager.Combat.RemoveInjury(context, base.CombatChar, b2, isInner: false, tuple2.Item1);
			}
			if (tuple2.Item2 > 0)
			{
				DomainManager.Combat.RemoveInjury(context, base.CombatChar, b2, isInner: true, tuple2.Item2, updateDefeatMark: true);
			}
			ShowSpecialEffectTips(0);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return flag;
	}
}
