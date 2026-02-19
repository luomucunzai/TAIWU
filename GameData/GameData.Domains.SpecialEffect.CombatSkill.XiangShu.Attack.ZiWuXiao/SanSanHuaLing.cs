using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao;

public class SanSanHuaLing : CombatSkillEffectBase
{
	private DataUid _selfNeiliAllocationUid;

	private NeiliAllocation _selfLastNeiliAllocation;

	private DataUid _enemyNeiliAllocationUid;

	private NeiliAllocation _enemyLastNeiliAllocation;

	public SanSanHuaLing()
	{
	}

	public SanSanHuaLing(CombatSkillKey skillKey)
		: base(skillKey, 17110, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_selfLastNeiliAllocation = base.CombatChar.GetNeiliAllocation();
		_selfNeiliAllocationUid = new DataUid(8, 10, (ulong)base.CharacterId, 3u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_selfNeiliAllocationUid, base.DataHandlerKey, OnSelfNeiliAllocationChanged);
		UpdateEnemyUid(init: true);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_selfNeiliAllocationUid, base.DataHandlerKey);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyNeiliAllocationUid, base.DataHandlerKey);
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if (base.EffectCount <= 0)
			{
				IsSrcSkillPerformed = true;
				AddMaxEffectCount();
			}
			else
			{
				RemoveSelf(context);
			}
		}
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		if (isAlly != base.CombatChar.IsAlly)
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

	private unsafe void OnSelfNeiliAllocationChanged(DataContext context, DataUid dataUid)
	{
		NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
		for (byte b = 0; b < 4; b++)
		{
			if (neiliAllocation.Items[(int)b] < _selfLastNeiliAllocation.Items[(int)b])
			{
				ReduceEffectCount();
				break;
			}
		}
		_selfLastNeiliAllocation = neiliAllocation;
	}

	private unsafe void OnEnemyNeiliAllocationChanged(DataContext context, DataUid dataUid)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		NeiliAllocation neiliAllocation = combatCharacter.GetNeiliAllocation();
		bool flag = false;
		for (byte b = 0; b < 4; b++)
		{
			int num = Math.Abs(neiliAllocation.Items[(int)b] - _enemyLastNeiliAllocation.Items[(int)b]);
			if (num != 0)
			{
				base.CombatChar.ChangeNeiliAllocation(context, b, num * 2);
				flag = true;
			}
		}
		if (flag)
		{
			ShowSpecialEffectTips(0);
		}
		_enemyLastNeiliAllocation = neiliAllocation;
	}

	private void UpdateEnemyUid(bool init)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		_enemyLastNeiliAllocation = combatCharacter.GetNeiliAllocation();
		if (!init)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyNeiliAllocationUid, base.DataHandlerKey);
		}
		_enemyNeiliAllocationUid = new DataUid(8, 10, (ulong)combatCharacter.GetId(), 3u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_enemyNeiliAllocationUid, base.DataHandlerKey, OnEnemyNeiliAllocationChanged);
	}
}
