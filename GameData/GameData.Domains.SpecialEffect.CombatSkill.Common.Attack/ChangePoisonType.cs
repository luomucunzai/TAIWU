using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public class ChangePoisonType : CombatSkillEffectBase
{
	private const sbyte AddPower = 40;

	protected sbyte[] CanChangePoisonType;

	protected sbyte AddPowerPoisonType;

	private sbyte _targetPoisonType;

	protected ChangePoisonType()
	{
	}

	protected ChangePoisonType(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public unsafe override void OnEnable(DataContext context)
	{
		PoisonInts poison = (base.IsDirect ? base.CurrEnemyChar : base.CombatChar).GetPoison();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		int num = int.MaxValue;
		list.Clear();
		for (int i = 0; i < CanChangePoisonType.Length; i++)
		{
			sbyte b = CanChangePoisonType[i];
			if (poison.Items[b] < num)
			{
				num = poison.Items[b];
				list.Clear();
				list.Add(b);
			}
			else if (poison.Items[b] == num)
			{
				list.Add(b);
			}
		}
		_targetPoisonType = (list.Contains(AddPowerPoisonType) ? AddPowerPoisonType : list[context.Random.Next(0, list.Count)]);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 81, base.SkillTemplateId), (EDataModifyType)3);
		if (_targetPoisonType == AddPowerPoisonType)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId), (EDataModifyType)1);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		ShowSpecialEffectTips(_targetPoisonType == AddPowerPoisonType, 1, 0);
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

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 81)
		{
			return _targetPoisonType;
		}
		return dataValue;
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
