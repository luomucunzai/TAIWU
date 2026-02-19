using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music;

public class QingPingDiao : CombatSkillEffectBase
{
	private const sbyte AddPower = 40;

	private const int ExtraCount = 3;

	private bool _prevMatchAffect;

	public QingPingDiao()
	{
	}

	public QingPingDiao(CombatSkillKey skillKey)
		: base(skillKey, 8301, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_prevMatchAffect = (base.IsDirect ? base.CurrEnemyChar.GetTrickCount(20) : base.CurrEnemyChar.GetDefeatMarkCollection().MindMarkList.Count) == 0;
		if (_prevMatchAffect)
		{
			CreateAffectedData(199, (EDataModifyType)1, base.SkillTemplateId);
			ShowSpecialEffectTips(0);
		}
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
			int count = ((!_prevMatchAffect) ? 1 : 4);
			if (base.IsDirect)
			{
				DomainManager.Combat.AddTrick(context, base.CurrEnemyChar, 20, count, addedByAlly: false);
			}
			else
			{
				DomainManager.Combat.AppendMindDefeatMark(context, base.CurrEnemyChar, count, -1);
			}
			ShowSpecialEffectTips(1);
		}
		RemoveSelf(context);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return 40;
		}
		return 0;
	}
}
