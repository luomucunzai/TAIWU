using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger;

public class TaiYiLiuLiZhi : CombatSkillEffectBase
{
	private const sbyte MaxInjuryMark = 2;

	private int _selfTransferValue;

	private int _enemyTransferValue;

	public TaiYiLiuLiZhi()
	{
	}

	public TaiYiLiuLiZhi(CombatSkillKey skillKey)
		: base(skillKey, 13108, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		GameData.Domains.Character.Character character = base.CurrEnemyChar.GetCharacter();
		int id = character.GetId();
		OuterAndInnerInts penetrationResists = CharObj.GetPenetrationResists();
		OuterAndInnerInts penetrationResists2 = character.GetPenetrationResists();
		_selfTransferValue = (base.IsDirect ? penetrationResists.Outer : penetrationResists.Inner) / 2;
		_enemyTransferValue = (base.IsDirect ? penetrationResists2.Outer : penetrationResists2.Inner) / 2;
		CreateAffectedData(114, (EDataModifyType)3, -1);
		CreateAffectedData((ushort)(base.IsDirect ? 44 : 45), (EDataModifyType)0, -1);
		CreateAffectedData((ushort)(base.IsDirect ? 46 : 47), (EDataModifyType)0, -1);
		CreateAffectedData(id, (ushort)(base.IsDirect ? 44 : 45), (EDataModifyType)0, -1);
		CreateAffectedData(id, (ushort)(base.IsDirect ? 46 : 47), (EDataModifyType)0, -1);
		ShowSpecialEffectTips(0);
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

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 114)
		{
			bool flag = dataKey.CustomParam1 == 1;
			if (flag == base.IsDirect)
			{
				return dataValue;
			}
			sbyte bodyPart = (sbyte)dataKey.CustomParam2;
			return Math.Min(dataValue, base.CombatChar.MarkCountChangeToDamageValue(bodyPart, flag, 2));
		}
		return dataValue;
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		ushort fieldId = dataKey.FieldId;
		if ((uint)(fieldId - 44) <= 1u)
		{
			return (dataKey.CharId == base.CharacterId) ? _selfTransferValue : _enemyTransferValue;
		}
		fieldId = dataKey.FieldId;
		if ((uint)(fieldId - 46) <= 1u)
		{
			return (dataKey.CharId == base.CharacterId) ? (-_selfTransferValue) : (-_enemyTransferValue);
		}
		return 0;
	}
}
