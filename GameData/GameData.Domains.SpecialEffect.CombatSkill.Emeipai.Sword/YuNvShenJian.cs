using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Sword;

public class YuNvShenJian : CombatSkillEffectBase
{
	private const sbyte RequireRatio = 60;

	private CValuePercent _preparePercent;

	private int _addPower;

	private bool _affecting;

	private bool _allSkillHasPower;

	private static int CalcAddPercent(short skillId)
	{
		return 30 + (Config.CombatSkill.Instance[skillId].Grade + 1) * 3;
	}

	public YuNvShenJian()
	{
	}

	public YuNvShenJian(CombatSkillKey skillKey)
		: base(skillKey, 2306, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		ClearFields();
		CreateAffectedData(199, (EDataModifyType)1, base.SkillTemplateId);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_ChangePreparingSkillBegin(OnChangePreparingSkillBegin);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_ChangePreparingSkillBegin(OnChangePreparingSkillBegin);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		if (base.CharacterId != charId)
		{
			return;
		}
		if (skillId == base.SkillTemplateId)
		{
			_allSkillHasPower = true;
			_affecting = true;
			base.CombatChar.CanCastDuringPrepareSkills.Clear();
			foreach (short attackSkill in base.CombatChar.GetAttackSkillList())
			{
				if (attackSkill >= 0 && attackSkill != base.SkillTemplateId)
				{
					CombatSkillKey objectId = new CombatSkillKey(base.CharacterId, attackSkill);
					GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(objectId);
					sbyte currInnerRatio = element_CombatSkills.GetCurrInnerRatio();
					if (base.IsDirect ? (currInnerRatio <= 40) : (currInnerRatio >= 60))
					{
						base.CombatChar.CanCastDuringPrepareSkills.Add(attackSkill);
						DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, attackSkill);
					}
				}
			}
			if (base.CombatChar.CanCastDuringPrepareSkills.Count > 0)
			{
				ShowSpecialEffectTips(0);
			}
			if (_preparePercent > 0)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * _preparePercent);
			}
		}
		else if (_affecting)
		{
			base.CombatChar.CanCastDuringPrepareSkills.Clear();
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar);
			DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress);
		}
	}

	private void OnChangePreparingSkillBegin(DataContext context, int charId, short prevSkillId, short currSkillId)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (charId == base.CharacterId && prevSkillId == base.SkillTemplateId && currSkillId != base.CombatChar.NeedUseSkillFreeId)
		{
			_preparePercent = CValuePercent.Parse(base.CombatChar.SkillPrepareCurrProgress, base.CombatChar.SkillPrepareTotalProgress);
			_preparePercent += CValuePercent.op_Implicit(CalcAddPercent(currSkillId));
			_addPower += CalcAddPercent(currSkillId);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId)
		{
			return;
		}
		if (skillId == base.SkillTemplateId)
		{
			base.CombatChar.CanCastDuringPrepareSkills.Clear();
			ClearFieldsWithInvalidateCache(context);
		}
		else if (_affecting)
		{
			_allSkillHasPower = _allSkillHasPower && PowerMatchAffectRequire(power);
			if (_allSkillHasPower && DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, costFree: true))
			{
				DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.YuNvShenJian);
				return;
			}
			ClearFieldsWithInvalidateCache(context);
			DomainManager.Combat.RaiseCastSkillEndByInterrupt(context, charId, isAlly, base.SkillTemplateId);
		}
	}

	private void ClearFields()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_preparePercent = CValuePercent.op_Implicit(0);
		_addPower = 0;
		_affecting = false;
	}

	private void ClearFieldsWithInvalidateCache(DataContext context)
	{
		ClearFields();
		DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return _addPower;
		}
		return 0;
	}
}
