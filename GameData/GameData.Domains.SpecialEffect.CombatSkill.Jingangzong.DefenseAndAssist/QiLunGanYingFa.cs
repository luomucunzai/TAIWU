using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist;

public class QiLunGanYingFa : AssistSkillBase
{
	private static readonly QiLunGanYingFaStateEffect StateEffect;

	private const sbyte ChangePowerPercentTheFirst = 100;

	private const sbyte ChangePowerPercentTheAfter = 50;

	private const sbyte StatePowerUnit = 50;

	static QiLunGanYingFa()
	{
		StateEffect = new QiLunGanYingFaStateEffect();
		SpecialEffectDomain.RegisterResetHandler(StateEffect.Reset);
	}

	public QiLunGanYingFa()
	{
	}

	public QiLunGanYingFa(CombatSkillKey skillKey)
		: base(skillKey, 11703)
	{
		SetConstAffectingOnCombatBegin = true;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		StateEffect.Setup();
	}

	public override void OnDataAdded(DataContext context)
	{
		base.OnDataAdded(context);
		if (base.IsDirect)
		{
			AppendAffectedData(context, base.CharacterId, 167, (EDataModifyType)2, -1);
		}
		else
		{
			AppendAffectedAllEnemyData(context, 167, (EDataModifyType)2, -1);
		}
	}

	public override void OnDisable(DataContext context)
	{
		StateEffect.Close();
		base.OnDisable(context);
	}

	protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
	{
		SetConstAffecting(context, base.CanAffect);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		if (!base.CanAffect)
		{
			return 0;
		}
		if (!base.IsDirect && !base.IsCurrent)
		{
			return 0;
		}
		if (dataKey.FieldId != 167)
		{
			return 0;
		}
		if (base.IsDirect ? (dataKey.CharId != base.CharacterId) : (dataKey.CharId == base.CharacterId))
		{
			return 0;
		}
		sbyte b = (sbyte)dataKey.CustomParam0;
		if (b != (base.IsDirect ? 1 : 2))
		{
			return 0;
		}
		BoolArray8 val = BoolArray8.op_Implicit((byte)dataKey.CustomParam2);
		bool flag = ((BoolArray8)(ref val))[0];
		bool flag2 = ((BoolArray8)(ref val))[1];
		if (flag)
		{
			return 0;
		}
		int customParam = dataKey.CustomParam1;
		if ((uint)(customParam - 166) > 1u)
		{
			DataContext dataContext = base.CombatChar.GetDataContext();
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId);
			DomainManager.Combat.AddCombatState(dataContext, element_CombatCharacterDict, b, (short)(base.IsDirect ? 166 : 167), 50);
			ShowEffectTips(dataContext);
		}
		ShowSpecialEffectTipsOnceInFrame(0);
		return flag2 ? 100 : 50;
	}
}
