using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong;

public class LoongEarthImplementSilenceColorful : LoongEarthImplementSilence, ILoongEarthExtra
{
	public delegate void OnSilenceAffect(DataContext context, CombatCharacter combatChar, short skillId);

	private const int MaxValuePercent = 50;

	private const int AddFlawOrAcupointLevel = 3;

	private const int AddFlawOrAcupointCount = 4;

	private readonly Dictionary<int, HashSet<short>> _silencingAttackSkills = new Dictionary<int, HashSet<short>>();

	private readonly Dictionary<int, HashSet<short>> _silencingAgileSkills = new Dictionary<int, HashSet<short>>();

	private readonly Dictionary<int, HashSet<short>> _silencingAssistSkills = new Dictionary<int, HashSet<short>>();

	private OnSilenceAffect GetAffectByCombatSkillId(short skillId)
	{
		sbyte equipType = Config.CombatSkill.Instance[skillId].EquipType;
		if (1 == 0)
		{
		}
		OnSilenceAffect result = equipType switch
		{
			1 => OnSilenceAttack, 
			2 => OnSilenceAgile, 
			3 => OnSilenceDefense, 
			4 => OnSilenceAssist, 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		base.EffectBase.CreateAffectedAllEnemyData(171, (EDataModifyType)3, -1);
		base.EffectBase.CreateAffectedAllEnemyData(172, (EDataModifyType)3, -1);
		base.EffectBase.CreateAffectedAllEnemyData(274, (EDataModifyType)3, -1);
		Events.RegisterHandler_SkillSilenceEnd(OnSkillSilenceEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_SkillSilenceEnd(OnSkillSilenceEnd);
		base.OnDisable(context);
	}

	private void OnSkillSilenceEnd(DataContext context, CombatSkillKey skillKey)
	{
		if (_silencingAttackSkills.TryGetValue(skillKey.CharId, out var value) && value.Remove(skillKey.SkillTemplateId) && value.Count == 0)
		{
			InvalidCacheAttack(context, skillKey.CharId);
		}
		if (_silencingAgileSkills.TryGetValue(skillKey.CharId, out var value2) && value2.Remove(skillKey.SkillTemplateId) && value2.Count == 0)
		{
			InvalidCacheAgile(context, skillKey.CharId);
		}
	}

	public void OnSilenced(DataContext context, CombatCharacter combatChar, short skillId)
	{
		GetAffectByCombatSkillId(skillId)?.Invoke(context, combatChar, skillId);
	}

	private void InvalidCacheAttack(DataContext context, int charId)
	{
		DomainManager.SpecialEffect.InvalidateCache(context, charId, 171);
		DomainManager.SpecialEffect.InvalidateCache(context, charId, 172);
		if (DomainManager.Combat.IsCharInCombat(charId))
		{
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
			if (element_CombatCharacterDict.GetBreathValue() > element_CombatCharacterDict.GetMaxBreathValue())
			{
				DomainManager.Combat.ChangeBreathValue(context, element_CombatCharacterDict, 0);
			}
			if (element_CombatCharacterDict.GetStanceValue() > element_CombatCharacterDict.GetMaxStanceValue())
			{
				DomainManager.Combat.ChangeStanceValue(context, element_CombatCharacterDict, 0);
			}
		}
	}

	private void InvalidCacheAgile(DataContext context, int charId)
	{
		DomainManager.SpecialEffect.InvalidateCache(context, charId, 274);
		if (DomainManager.Combat.IsCharInCombat(charId))
		{
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
			if (element_CombatCharacterDict.GetMobilityValue() > element_CombatCharacterDict.GetMaxMobility())
			{
				DomainManager.Combat.ChangeMobilityValue(context, element_CombatCharacterDict, 0);
			}
		}
	}

	private void OnSilenceAttack(DataContext context, CombatCharacter combatChar, short skillId)
	{
		HashSet<short> orNew = _silencingAttackSkills.GetOrNew(combatChar.GetId());
		bool flag = orNew.Count == 0;
		orNew.Add(skillId);
		if (flag)
		{
			InvalidCacheAttack(context, combatChar.GetId());
			base.EffectBase.ShowSpecialEffectTips(1);
		}
	}

	private void OnSilenceAgile(DataContext context, CombatCharacter combatChar, short skillId)
	{
		HashSet<short> orNew = _silencingAgileSkills.GetOrNew(combatChar.GetId());
		bool flag = orNew.Count == 0;
		orNew.Add(skillId);
		if (flag)
		{
			InvalidCacheAgile(context, combatChar.GetId());
			base.EffectBase.ShowSpecialEffectTips(2);
		}
	}

	private void OnSilenceDefense(DataContext context, CombatCharacter combatChar, short skillId)
	{
		for (int i = 0; i < 4; i++)
		{
			DomainManager.Combat.AddFlaw(context, combatChar, 3, base.EffectBase.SkillKey, -1);
		}
		base.EffectBase.ShowSpecialEffectTips(3);
	}

	private void OnSilenceAssist(DataContext context, CombatCharacter combatChar, short skillId)
	{
		for (int i = 0; i < 4; i++)
		{
			DomainManager.Combat.AddAcupoint(context, combatChar, 3, base.EffectBase.SkillKey, -1);
		}
		base.EffectBase.ShowSpecialEffectTips(4);
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.CharId == base.EffectBase.CharacterId)
		{
			return dataValue;
		}
		ushort fieldId = dataKey.FieldId;
		bool flag = (uint)(fieldId - 171) <= 1u;
		if (flag && _silencingAttackSkills.TryGetValue(dataKey.CharId, out var value) && value.Count > 0)
		{
			return Math.Min(50, dataValue);
		}
		if (dataKey.FieldId == 274 && _silencingAgileSkills.TryGetValue(dataKey.CharId, out var value2) && value2.Count > 0)
		{
			return Math.Min(50, dataValue);
		}
		return dataValue;
	}
}
