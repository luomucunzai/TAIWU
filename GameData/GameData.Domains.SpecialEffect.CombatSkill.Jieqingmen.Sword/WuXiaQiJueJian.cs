using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword;

public class WuXiaQiJueJian : CombatSkillEffectBase
{
	private const int CastNoTricksRequireShaTrickCount = 7;

	private const sbyte MinSkillGrade = 3;

	private const sbyte MaxSkillGrade = 5;

	private const short CdFrame = 180;

	private static readonly CValuePercent ProgressPercent = CValuePercent.op_Implicit(100);

	private short _autoCastSkillId;

	private DataUid _tricksUid;

	private bool _castNoTricks;

	private CombatCharacter TrickChar => base.IsDirect ? base.CombatChar : base.EnemyChar;

	public WuXiaQiJueJian()
	{
	}

	public WuXiaQiJueJian(CombatSkillKey skillKey)
		: base(skillKey, 13206, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(208, (EDataModifyType)3, base.SkillTemplateId);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_GetShaTrick(OnGetShaTrick);
		Events.RegisterHandler_RemoveShaTrick(OnRemoveShaTrick);
		_autoCastSkillId = -1;
		_castNoTricks = false;
		_tricksUid = new DataUid(0, 0, ulong.MaxValue);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_GetShaTrick(OnGetShaTrick);
		Events.UnRegisterHandler_RemoveShaTrick(OnRemoveShaTrick);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_tricksUid, base.DataHandlerKey);
	}

	private void OnCombatBegin(DataContext context)
	{
		UpdateTrickChar(context);
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		UpdateTrickChar(context);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (charId == base.CharacterId && skillId == _autoCastSkillId && base.CombatChar.GetAutoCastingSkill())
		{
			DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * ProgressPercent);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId)
		{
			if (skillId == _autoCastSkillId && base.CombatChar.GetAutoCastingSkill())
			{
				_autoCastSkillId = -1;
			}
			else if (skillId == base.SkillTemplateId && PowerMatchAffectRequire(power))
			{
				AddMaxEffectCount();
			}
		}
	}

	private void OnGetShaTrick(DataContext context, int charId, bool isAlly, bool real)
	{
		if (charId == TrickChar.GetId())
		{
			DoAutoCast(context);
		}
	}

	private void OnRemoveShaTrick(DataContext context, int charId)
	{
		if (charId == TrickChar.GetId())
		{
			DoAutoCast(context);
		}
	}

	private void UpdateTrickChar(DataContext context)
	{
		int id = TrickChar.GetId();
		if (id != (int)_tricksUid.SubId0)
		{
			if (_tricksUid.SubId0 != ulong.MaxValue)
			{
				GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_tricksUid, base.DataHandlerKey);
			}
			_tricksUid = ParseCombatCharacterDataUid(id, 28);
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_tricksUid, base.DataHandlerKey, OnTrickChanged);
			OnTrickChanged(context, _tricksUid);
		}
	}

	private void OnTrickChanged(DataContext context, DataUid dataUid)
	{
		bool flag = TrickChar.GetTrickCount(19) >= 7;
		if (flag != _castNoTricks)
		{
			_castNoTricks = flag;
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
		}
	}

	private void DoAutoCast(DataContext context)
	{
		if (base.EffectCount > 0 && !base.SkillData.GetSilencing() && _autoCastSkillId < 0)
		{
			int continueTricksAtStart = TrickChar.GetContinueTricksAtStart(19);
			int num = Math.Min(3 + continueTricksAtStart, 5);
			short randomAttackSkill = DomainManager.Combat.GetRandomAttackSkill(base.CombatChar, 7, (sbyte)num, context.Random, descSearch: true, -1);
			if (randomAttackSkill >= 0 && Config.CombatSkill.Instance[randomAttackSkill].Grade >= 3)
			{
				_autoCastSkillId = randomAttackSkill;
				DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, 180, -1);
				DomainManager.Combat.CastSkillFree(context, base.CombatChar, _autoCastSkillId, ECombatCastFreePriority.WuXiaQiJueJian);
				ShowSpecialEffectTips(0);
				ShowSpecialEffectTips(1);
				ReduceEffectCount();
			}
		}
	}

	public override List<NeedTrick> GetModifiedValue(AffectedDataKey dataKey, List<NeedTrick> dataValue)
	{
		if (dataKey.SkillKey == SkillKey && dataKey.FieldId == 208 && _castNoTricks)
		{
			dataValue.Clear();
		}
		return base.GetModifiedValue(dataKey, dataValue);
	}
}
