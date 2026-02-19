using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist;

public class SanBuJiuHouFa : AssistSkillBase
{
	private const sbyte MaxAffectDistance = 50;

	private const int DistancePerUnit = 10;

	private const int AddPercentValueBase = 5;

	private const int AddPercentValuePerUnit = 5;

	private int _currAddValue;

	public SanBuJiuHouFa()
	{
	}

	public SanBuJiuHouFa(CombatSkillKey skillKey)
		: base(skillKey, 10701)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_currAddValue = CalcAddValue();
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		if (base.IsDirect)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 32, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 33, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 34, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 35, -1), (EDataModifyType)1);
		}
		else
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 38, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 39, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 40, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 41, -1), (EDataModifyType)1);
		}
		Events.RegisterHandler_DistanceChanged(OnDistanceChanged);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_DistanceChanged(OnDistanceChanged);
	}

	private int CalcAddValue()
	{
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		if (currentDistance >= 50)
		{
			return 0;
		}
		int num = (50 - currentDistance) / 10;
		return 5 + num * 5;
	}

	private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
	{
		int num = CalcAddValue();
		if (_currAddValue != num)
		{
			_currAddValue = num;
			InvalidateCache(context);
			SetConstAffecting(context, _currAddValue > 0);
		}
	}

	protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
	{
		InvalidateCache(context);
	}

	private void InvalidateCache(DataContext context)
	{
		if (base.IsDirect)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 32);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 33);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 34);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 35);
		}
		else
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 38);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 39);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 40);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 41);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		return base.CanAffect ? _currAddValue : 0;
	}
}
