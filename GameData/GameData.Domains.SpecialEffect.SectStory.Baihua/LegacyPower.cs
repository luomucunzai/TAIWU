using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.SpecialEffect.Animal;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua;

public abstract class LegacyPower : CarrierEffectBase
{
	private const int AddPower = 220;

	protected abstract sbyte OrgTemplateId { get; }

	private bool IsOrgSkill(short skillId)
	{
		return Config.CombatSkill.Instance[skillId].SectId == OrgTemplateId;
	}

	protected LegacyPower(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
	}

	protected override void OnDisableSubClass(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
	}

	private void OnCombatBegin(DataContext context)
	{
		AppendAffectedData(context, 199, (EDataModifyType)0, -1);
		AppendAffectedAllEnemyData(context, 199, (EDataModifyType)0, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.FieldId != 199 || !IsOrgSkill(dataKey.CombatSkillId))
		{
			return 0;
		}
		return (dataKey.CharId == base.CharacterId) ? 220 : (-220);
	}
}
