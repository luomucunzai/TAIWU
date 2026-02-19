using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi;

public class SheShenShi : CombatSkillEffectBase
{
	private const sbyte NeiliAllocationValue = 18;

	private DataUid _enemyInjuriesUid;

	public SheShenShi()
	{
	}

	public SheShenShi(CombatSkillKey skillKey)
		: base(skillKey, 17053, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
		if (IsSrcSkillPerformed)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyInjuriesUid, base.DataHandlerKey);
		}
	}

	private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId || interrupted)
		{
			return;
		}
		NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
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
			if (list.Count > 0)
			{
				byte type = list[context.Random.Next(list.Count)];
				base.CombatChar.ChangeNeiliAllocation(context, type, -18);
				if (CheckAndAddNeiliAllocation(context))
				{
					RemoveSelf(context);
				}
				else
				{
					IsSrcSkillPerformed = true;
					AddMaxEffectCount();
					UpdateEnemyUid(init: true);
				}
			}
			else
			{
				RemoveSelf(context);
			}
		}
		else if (list.Count > 0)
		{
			RemoveSelf(context);
		}
		ObjectPool<List<byte>>.Instance.Return(list);
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		if (IsSrcSkillPerformed && isAlly != base.CombatChar.IsAlly)
		{
			UpdateEnemyUid(init: false);
		}
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			RemoveSelf(context);
		}
	}

	private void OnEnemyInjuriesChanged(DataContext context, DataUid dataUid)
	{
		if (CheckAndAddNeiliAllocation(context))
		{
			ReduceEffectCount();
		}
	}

	private void UpdateEnemyUid(bool init)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		if (!init)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyInjuriesUid, base.DataHandlerKey);
		}
		_enemyInjuriesUid = new DataUid(8, 10, (ulong)combatCharacter.GetId(), 29u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_enemyInjuriesUid, base.DataHandlerKey, OnEnemyInjuriesChanged);
	}

	private bool CheckAndAddNeiliAllocation(DataContext context)
	{
		Injuries injuries = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly).GetInjuries();
		bool flag = false;
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			if (tuple.Item1 >= 6 || tuple.Item2 >= 6)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			for (byte b2 = 0; b2 < 4; b2++)
			{
				base.CombatChar.ChangeNeiliAllocation(context, b2, 18);
			}
			ShowSpecialEffectTips(0);
		}
		return flag;
	}
}
