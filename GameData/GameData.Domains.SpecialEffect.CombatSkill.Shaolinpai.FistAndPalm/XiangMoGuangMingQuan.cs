using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.FistAndPalm;

public class XiangMoGuangMingQuan : CombatSkillEffectBase
{
	private const sbyte AddPowerUnit = 40;

	private const sbyte AddRangeUnit = 10;

	private const sbyte ChangeMorality = 125;

	private int _addPower;

	private int _addRange;

	public XiangMoGuangMingQuan()
	{
	}

	public XiangMoGuangMingQuan(CombatSkillKey skillKey)
		: base(skillKey, 1107, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		sbyte b = (sbyte)(base.IsDirect ? 1 : 3);
		sbyte behaviorType = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly).GetCharacter().GetBehaviorType();
		_addPower = 40 * Math.Abs(b - behaviorType);
		_addRange = 10 * Math.Abs(b - behaviorType);
		if (_addPower > 0)
		{
			AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, base.SkillTemplateId), (EDataModifyType)0);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, base.SkillTemplateId), (EDataModifyType)0);
			ShowSpecialEffectTips(0);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		sbyte b = (sbyte)(base.IsDirect ? 1 : 3);
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		GameData.Domains.Character.Character character = combatCharacter.GetCharacter();
		HitOrAvoidInts hitValues = CharObj.GetHitValues();
		HitOrAvoidInts avoidValues = character.GetAvoidValues();
		sbyte behaviorType = character.GetBehaviorType();
		if (PowerMatchAffectRequire(power) && hitValues.Items[3] > avoidValues.Items[3] && b != behaviorType)
		{
			character.ChangeBaseMorality(context, (behaviorType > b) ? 125 : (-125));
			ShowSpecialEffectTips(1);
			if (character.GetBehaviorType() == b && combatCharacter.AiController.CanFlee())
			{
				combatCharacter.SetNeedUseOtherAction(context, 2);
				AppendAffectedData(context, combatCharacter.GetId(), 124, (EDataModifyType)1, -1);
			}
			else
			{
				RemoveSelf(context);
			}
		}
		else
		{
			RemoveSelf(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.FieldId == 124)
		{
			RemoveSelf(DomainManager.Combat.Context);
			return -67;
		}
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return _addPower;
		}
		if (dataKey.FieldId == 145 || dataKey.FieldId == 146)
		{
			return _addRange;
		}
		return 0;
	}
}
