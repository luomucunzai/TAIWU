using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special;

public class JiuZiZhenYanZhou : CombatSkillEffectBase
{
	private const int DirectChangeDamagePercent = 10;

	private const int ReverseChangeDamagePercent = -5;

	private static readonly IReadOnlyDictionary<sbyte, ushort> Type2FieldIds = new Dictionary<sbyte, ushort>
	{
		{ 1, 102 },
		{ 2, 191 },
		{ 3, 276 }
	};

	private static readonly IReadOnlyDictionary<ushort, byte> FieldId2SpecialEffectTipsIndex = new Dictionary<ushort, byte>
	{
		{ 102, 0 },
		{ 191, 1 },
		{ 276, 2 }
	};

	private static readonly IReadOnlyDictionary<ushort, Func<AffectedDataKey, bool>> ExtraCheckers = new Dictionary<ushort, Func<AffectedDataKey, bool>> { { 191, FatalDamageValueExtraChecker } };

	private readonly Dictionary<ushort, int> _fieldId2ChangePercent = new Dictionary<ushort, int>();

	private CombatSkillKey _affectingSkillKey;

	private static bool FatalDamageValueExtraChecker(AffectedDataKey dataKey)
	{
		EDamageType customParam = (EDamageType)dataKey.CustomParam1;
		if (customParam == EDamageType.Direct || customParam == EDamageType.FightBack)
		{
			return true;
		}
		return false;
	}

	public JiuZiZhenYanZhou()
	{
	}

	public JiuZiZhenYanZhou(CombatSkillKey skillKey)
		: base(skillKey, 7304, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		ResetAffecting();
		if (base.IsDirect)
		{
			CreateAffectedAllEnemyData(102, (EDataModifyType)1, -1);
			CreateAffectedAllEnemyData(191, (EDataModifyType)1, -1);
			CreateAffectedAllEnemyData(276, (EDataModifyType)1, -1);
		}
		else
		{
			CreateAffectedData(102, (EDataModifyType)1, -1);
			CreateAffectedData(191, (EDataModifyType)1, -1);
			CreateAffectedData(276, (EDataModifyType)1, -1);
		}
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		base.OnDisable(context);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (!(base.IsDirect ? (attacker.GetId() == base.CharacterId) : (defender.GetId() == base.CharacterId)) || base.EffectCount <= 0 || _affectingSkillKey.IsValid)
		{
			return;
		}
		_affectingSkillKey = new CombatSkillKey(defender.GetId(), skillId);
		CombatCharacter combatCharacter = (base.IsDirect ? defender : attacker);
		int num = (base.IsDirect ? 10 : (-5));
		foreach (short bannedSkillId in combatCharacter.GetBannedSkillIds())
		{
			sbyte equipType = Config.CombatSkill.Instance[bannedSkillId].EquipType;
			if (Type2FieldIds.TryGetValue(equipType, out var value))
			{
				_fieldId2ChangePercent[value] = _fieldId2ChangePercent.GetOrDefault(value) + num;
			}
		}
		if (_fieldId2ChangePercent.Count > 0)
		{
			ReduceEffectCount();
			{
				foreach (ushort key in _fieldId2ChangePercent.Keys)
				{
					ShowSpecialEffectTips(FieldId2SpecialEffectTipsIndex[key]);
				}
				return;
			}
		}
		_affectingSkillKey = CombatSkillKey.Invalid;
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (SkillKey.IsMatch(charId, skillId) && PowerMatchAffectRequire(power))
		{
			AddMaxEffectCount();
		}
		int id = DomainManager.Combat.GetCombatCharacter(!isAlly, tryGetCoverCharacter: true).GetId();
		if (id == _affectingSkillKey.CharId && skillId == _affectingSkillKey.SkillTemplateId)
		{
			ResetAffecting();
		}
	}

	private void ResetAffecting()
	{
		_affectingSkillKey = CombatSkillKey.Invalid;
		_fieldId2ChangePercent.Clear();
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.SkillKey != _affectingSkillKey)
		{
			return 0;
		}
		if (ExtraCheckers.TryGetValue(dataKey.FieldId, out var value) && !value(dataKey))
		{
			return 0;
		}
		int value2;
		return _fieldId2ChangePercent.TryGetValue(dataKey.FieldId, out value2) ? value2 : 0;
	}
}
