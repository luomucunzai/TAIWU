using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao;

public class JiuJiuZaoMing : CombatSkillEffectBase
{
	private DataUid _selfMarkUid;

	private readonly byte[] _selfLastPoisonMark = new byte[6];

	private DataUid _enemyInjuriesUid;

	private Injuries _enemyLastInjuries;

	public JiuJiuZaoMing()
	{
	}

	public JiuJiuZaoMing(CombatSkillKey skillKey)
		: base(skillKey, 17112, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Array.Copy(base.CombatChar.GetDefeatMarkCollection().PoisonMarkList, _selfLastPoisonMark, 6);
		_selfMarkUid = new DataUid(8, 10, (ulong)base.CharacterId, 50u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_selfMarkUid, base.DataHandlerKey, OnSelfMarkChanged);
		UpdateEnemyUid(init: true);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_selfMarkUid, base.DataHandlerKey);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyInjuriesUid, base.DataHandlerKey);
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

	private void OnSelfMarkChanged(DataContext context, DataUid dataUid)
	{
		byte[] poisonMarkList = base.CombatChar.GetDefeatMarkCollection().PoisonMarkList;
		for (sbyte b = 0; b < 6; b++)
		{
			byte b2 = poisonMarkList[b];
			if (b2 > _selfLastPoisonMark[b])
			{
				ReduceEffectCount(b2 - _selfLastPoisonMark[b]);
				if (base.EffectCount <= 0)
				{
					break;
				}
			}
			_selfLastPoisonMark[b] = b2;
		}
	}

	private void OnEnemyInjuriesChanged(DataContext context, DataUid dataUid)
	{
		Injuries injuries = base.CombatChar.GetInjuries();
		Injuries injuries2 = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly).GetInjuries();
		bool flag = false;
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			if (tuple.Item1 != 0 || tuple.Item2 != 0)
			{
				(sbyte, sbyte) tuple2 = injuries2.Get(b);
				(sbyte, sbyte) tuple3 = _enemyLastInjuries.Get(b);
				int num = 0;
				if (tuple2.Item1 != tuple3.Item1)
				{
					num += Math.Abs(tuple2.Item1 - tuple3.Item1);
				}
				if (tuple2.Item2 != tuple3.Item2)
				{
					num += Math.Abs(tuple2.Item2 - tuple3.Item2);
				}
				if (num != 0)
				{
					while (tuple.Item1 + tuple.Item2 > num)
					{
						if (tuple.Item1 > 0 && (tuple.Item2 == 0 || context.Random.CheckPercentProb(50)))
						{
							tuple.Item1--;
						}
						else
						{
							tuple.Item2--;
						}
					}
					if (tuple.Item1 > 0)
					{
						DomainManager.Combat.RemoveInjury(context, base.CombatChar, b, isInner: false, tuple.Item1);
					}
					if (tuple.Item2 > 0)
					{
						DomainManager.Combat.RemoveInjury(context, base.CombatChar, b, isInner: true, tuple.Item2);
					}
					flag = true;
				}
			}
		}
		_enemyLastInjuries = injuries2;
		if (flag)
		{
			DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
			ShowSpecialEffectTips(0);
		}
	}

	private void UpdateEnemyUid(bool init)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		_enemyLastInjuries = combatCharacter.GetInjuries();
		if (!init)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyInjuriesUid, base.DataHandlerKey);
		}
		_enemyInjuriesUid = new DataUid(8, 10, (ulong)combatCharacter.GetId(), 29u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_enemyInjuriesUid, base.DataHandlerKey, OnEnemyInjuriesChanged);
	}
}
