using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao;

public class QiQiShengHui : CombatSkillEffectBase
{
	private DataUid _enemyTricksUid;

	private int _enemyLastTrickCount;

	public QiQiShengHui()
	{
	}

	public QiQiShengHui(CombatSkillKey skillKey)
		: base(skillKey, 17111, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		UpdateEnemyUid(init: true);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.RegisterHandler_GetTrick(OnGetTrick);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyTricksUid, base.DataHandlerKey);
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.UnRegisterHandler_GetTrick(OnGetTrick);
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

	private void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
	{
		if (!(charId != base.CharacterId || usable))
		{
			ReduceEffectCount();
		}
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			RemoveSelf(context);
		}
	}

	private void OnEnemyTricksChanged(DataContext context, DataUid dataUid)
	{
		int count = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly).GetTricks().Tricks.Count;
		int num = Math.Abs(count - _enemyLastTrickCount);
		if (num > 0)
		{
			DomainManager.Combat.AddRandomTrick(context, base.CombatChar, num);
			ShowSpecialEffectTips(0);
		}
		_enemyLastTrickCount = count;
	}

	private void UpdateEnemyUid(bool init)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		_enemyLastTrickCount = combatCharacter.GetTricks().Tricks.Count;
		if (!init)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyTricksUid, base.DataHandlerKey);
		}
		_enemyTricksUid = new DataUid(8, 10, (ulong)combatCharacter.GetId(), 28u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_enemyTricksUid, base.DataHandlerKey, OnEnemyTricksChanged);
	}
}
