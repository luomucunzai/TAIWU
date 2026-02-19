using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger;

public class DaGuangMingShanYiYuanZhi : PowerUpOnCast
{
	private static readonly CValuePercent AddPower = CValuePercent.op_Implicit(60);

	protected override EDataModifyType ModifyType => (EDataModifyType)1;

	public DaGuangMingShanYiYuanZhi()
	{
	}

	public DaGuangMingShanYiYuanZhi(CombatSkillKey skillKey)
		: base(skillKey, 2206)
	{
	}

	public override void OnEnable(DataContext context)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		PowerUpValue = (base.IsDirect ? ((int)CharObj.GetFame() * AddPower) : (-CharObj.GetFame() * AddPower));
		base.OnEnable(context);
		Events.RegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		base.OnDisable(context);
	}

	private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			sbyte fame = CharObj.GetFame();
			sbyte fame2 = base.CurrEnemyChar.GetCharacter().GetFame();
			if (base.IsDirect ? (fame >= fame2) : (fame <= fame2))
			{
				AppendAffectedData(context, base.CharacterId, 251, (EDataModifyType)3, base.SkillTemplateId);
			}
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 251)
		{
			return true;
		}
		return dataValue;
	}
}
