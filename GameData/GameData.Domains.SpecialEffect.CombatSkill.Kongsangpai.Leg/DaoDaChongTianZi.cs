using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg;

public class DaoDaChongTianZi : CombatSkillEffectBase
{
	private bool _canAffect;

	private bool _disableSkills;

	public DaoDaChongTianZi()
	{
	}

	public DaoDaChongTianZi(CombatSkillKey skillKey)
		: base(skillKey, 10300, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		_canAffect = !combatCharacter.AiController.Memory.EnemyRecordDict[base.CharacterId].SkillRecord.ContainsKey(base.SkillTemplateId);
		CreateAffectedAllEnemyData(287, (EDataModifyType)3, -1);
		CreateAffectedAllEnemyData(285, (EDataModifyType)3, -1);
		if (_canAffect)
		{
			int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			for (int i = 0; i < characterList.Length; i++)
			{
				if (characterList[i] >= 0)
				{
					DomainManager.Combat.GetElement_CombatCharacterDict(characterList[i]).AiController.AllowDefense = false;
				}
			}
			Events.RegisterHandler_CompareDataCalcFinished(OnCompareDataCalcFinished);
			Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
			ShowSpecialEffectTips(0);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		if (_canAffect)
		{
			Events.UnRegisterHandler_CompareDataCalcFinished(OnCompareDataCalcFinished);
			Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		}
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
	{
		if (context.Attacker == base.CombatChar && context.SkillTemplateId == base.SkillTemplateId)
		{
			if (base.IsDirect)
			{
				compareData.OuterDefendValue /= 2;
			}
			else
			{
				compareData.InnerDefendValue /= 2;
			}
		}
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker == base.CombatChar && skillId == base.SkillTemplateId)
		{
			_disableSkills = true;
			InvalidateCache(context, base.CurrEnemyChar.GetId(), 287);
			InvalidateCache(context, base.CurrEnemyChar.GetId(), 285);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (_canAffect)
		{
			int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			for (int i = 0; i < characterList.Length; i++)
			{
				if (characterList[i] >= 0)
				{
					DomainManager.Combat.GetElement_CombatCharacterDict(characterList[i]).AiController.AllowDefense = true;
				}
			}
			if (PowerMatchAffectRequire(power))
			{
				DomainManager.Combat.AppendFatalDamageMark(context, base.CurrEnemyChar, 1, -1, -1);
			}
			_disableSkills = false;
		}
		RemoveSelf(context);
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		ushort fieldId = dataKey.FieldId;
		if ((fieldId != 285 && fieldId != 287) || 1 == 0)
		{
			return dataValue;
		}
		return dataValue && !_disableSkills;
	}
}
