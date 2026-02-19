using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public class AttackBodyPartLegacy : CombatSkillEffectBase
{
	private const sbyte ReverseMissOdds = 50;

	private static readonly sbyte[] ReverseSecondRandOdds = new sbyte[7] { 20, 20, 1, 20, 20, 20, 20 };

	protected sbyte[] BodyParts;

	protected sbyte ReverseAddDamagePercent;

	private bool _affected;

	protected AttackBodyPartLegacy()
	{
	}

	protected AttackBodyPartLegacy(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_affected = false;
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		if (base.IsDirect)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 77, base.SkillTemplateId), (EDataModifyType)3);
		}
		else
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 140, base.SkillTemplateId), (EDataModifyType)3);
		}
		if (!base.IsDirect)
		{
			Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		if (!base.IsDirect)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		}
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker.GetId() == base.CharacterId && skillId == base.SkillTemplateId)
		{
			AppendAffectedData(context, base.CharacterId, 69, (EDataModifyType)1, base.SkillTemplateId);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if (_affected)
			{
				ShowSpecialEffectTips(0);
			}
			RemoveSelf(context);
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 77)
		{
			_affected = true;
			return true;
		}
		return dataValue;
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		DataContext context = DomainManager.Combat.Context;
		if (dataKey.FieldId == 140 && dataKey.CombatSkillId == base.SkillTemplateId && BodyParts.Exist((sbyte)dataValue) && context.Random.CheckPercentProb(50))
		{
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			list.Clear();
			list.AddRange(ReverseSecondRandOdds);
			for (int i = 0; i < BodyParts.Length; i++)
			{
				list[BodyParts[i]] = 0;
			}
			int num = list.ToArray().Sum();
			int num2 = context.Random.Next(num);
			int num3 = 0;
			for (sbyte b = 0; b < 7; b++)
			{
				num3 += list[b];
				if (num3 > num2)
				{
					dataValue = b;
					break;
				}
			}
			ObjectPool<List<sbyte>>.Instance.Return(list);
		}
		return dataValue;
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.FieldId == 69 && dataKey.CombatSkillId == base.SkillTemplateId && BodyParts.Exist((sbyte)dataKey.CustomParam1))
		{
			_affected = true;
			return ReverseAddDamagePercent;
		}
		return 0;
	}
}
