using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music;

public class HouRenXiYi : CombatSkillEffectBase
{
	private sbyte MaxCharCount = 10;

	private sbyte PowerChangeUnit = 12;

	private int _powerChangeValue;

	public HouRenXiYi()
	{
	}

	public HouRenXiYi(CombatSkillKey skillKey)
		: base(skillKey, 8307, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		int num = (base.IsDirect ? base.CurrEnemyChar.GetId() : base.CharacterId);
		HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(num, 16384);
		int num2 = 0;
		foreach (int item in relatedCharIds)
		{
			if (DomainManager.Character.GetRelatedCharIds(item, 16384).Contains(num))
			{
				num2++;
			}
		}
		_powerChangeValue = ((num2 <= MaxCharCount) ? (PowerChangeUnit * num2) : (-PowerChangeUnit * (num2 - MaxCharCount)));
		if (_powerChangeValue != 0)
		{
			AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId), (EDataModifyType)1);
			ShowSpecialEffectTips(_powerChangeValue > 0, 0, 1);
		}
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
			RemoveSelf(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return _powerChangeValue;
		}
		return 0;
	}
}
