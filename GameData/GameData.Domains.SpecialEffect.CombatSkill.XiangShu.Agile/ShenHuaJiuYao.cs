using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile;

public class ShenHuaJiuYao : AgileSkillBase
{
	private const sbyte RequireMoveCount = 3;

	private bool _affecting;

	private int _moveCounter;

	public ShenHuaJiuYao()
	{
	}

	public ShenHuaJiuYao(CombatSkillKey skillKey)
		: base(skillKey, 16213)
	{
		ListenCanAffectChange = true;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_DistanceChanged(OnDistanceChanged);
		_affecting = false;
		OnMoveSkillCanAffectChanged(context, default(DataUid));
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_DistanceChanged(OnDistanceChanged);
		DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
	}

	private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
	{
		if (!(mover != base.CombatChar || !isMove || isForced) && _affecting)
		{
			_moveCounter++;
			if (_moveCounter >= 3)
			{
				_moveCounter = 0;
				AppendAffectedData(context, base.CharacterId, 85, (EDataModifyType)3, -1);
				AppendAffectedData(context, base.CharacterId, 86, (EDataModifyType)3, -1);
				AppendAffectedData(context, base.CharacterId, 76, (EDataModifyType)3, -1);
				base.CombatChar.NormalAttackFree();
				Events.RegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd);
				ShowSpecialEffectTips(0);
			}
		}
	}

	private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
	{
		if (attacker == base.CombatChar)
		{
			ClearAffectedData(context);
			Events.UnRegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd);
		}
	}

	protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
	{
		bool canAffect = base.CanAffect;
		if (_affecting != canAffect)
		{
			_affecting = canAffect;
			if (canAffect)
			{
				DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId);
			}
			else
			{
				DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
			}
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId >= 0)
		{
			return dataValue;
		}
		return false;
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 76)
		{
			return 100;
		}
		return dataValue;
	}
}
