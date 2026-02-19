using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Memory;

public class AiMemory
{
	private readonly CombatCharacter _combatCharacter;

	public readonly RecordCollection SelfRecord = new RecordCollection();

	public readonly Dictionary<int, RecordCollection> EnemyRecordDict = new Dictionary<int, RecordCollection>();

	private CombatCharacter _attacker;

	private CombatCharacter _defender;

	private int _normalAttackWeaponId;

	private OuterAndInnerInts _singleAttackDamage;

	private int _singleAttackMindDamage;

	private bool _gotSkillNeedTrick;

	public AiMemory(CombatCharacter combatChar)
	{
		_combatCharacter = combatChar;
		int[] array = (_combatCharacter.IsAlly ? DomainManager.Combat.GetEnemyTeam() : DomainManager.Combat.GetSelfTeam());
		foreach (int num in array)
		{
			if (num >= 0)
			{
				EnemyRecordDict.Add(num, new RecordCollection());
			}
		}
	}

	public void ClearMemories()
	{
		SelfRecord.ClearAll();
		foreach (RecordCollection value in EnemyRecordDict.Values)
		{
			value.ClearAll();
		}
	}

	public void RegisterCallbacks()
	{
		Events.RegisterHandler_NormalAttackBegin(OnNormalAttackBegin);
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_AddDirectDamageValue(OnAddDirectDamageValue);
		Events.RegisterHandler_AddMindDamage(OnAddMindDamageValue);
		Events.RegisterHandler_GetTrick(OnGetTrick);
	}

	public void UnregisterCallbacks()
	{
		Events.UnRegisterHandler_NormalAttackBegin(OnNormalAttackBegin);
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_AddDirectDamageValue(OnAddDirectDamageValue);
		Events.UnRegisterHandler_AddMindDamage(OnAddMindDamageValue);
		Events.UnRegisterHandler_GetTrick(OnGetTrick);
	}

	private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
	{
		if (attacker == _combatCharacter)
		{
			UpdateMaxValues(isAttacker: true);
		}
		else if (defender == _combatCharacter)
		{
			UpdateMaxValues(isAttacker: false);
		}
		_attacker = attacker;
		_defender = defender;
		_normalAttackWeaponId = attacker.GetWeapons()[attacker.GetUsingWeaponIndex()].Id;
		_singleAttackDamage.Outer = 0;
		_singleAttackDamage.Inner = 0;
		_singleAttackMindDamage = 0;
		_gotSkillNeedTrick = false;
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (attacker == _combatCharacter || EnemyRecordDict.ContainsKey(attacker.GetId()))
		{
			int num = GetAttackScore();
			if (_gotSkillNeedTrick)
			{
				num += 400;
			}
			Dictionary<int, (int, int)> weaponRecord = ((attacker == _combatCharacter) ? SelfRecord.WeaponRecord : EnemyRecordDict[attacker.GetId()].WeaponRecord);
			UpdateWeaponScore(weaponRecord, _normalAttackWeaponId, num);
		}
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker == _combatCharacter)
		{
			UpdateMaxValues(isAttacker: true);
		}
		else if (defender == _combatCharacter)
		{
			UpdateMaxValues(isAttacker: false);
		}
		_attacker = attacker;
		_defender = defender;
		_singleAttackDamage.Outer = 0;
		_singleAttackDamage.Inner = 0;
		_singleAttackMindDamage = 0;
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		CombatSkillKey objectId = new CombatSkillKey(charId, skillId);
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		if (combatSkillItem.EquipType != 1 || interrupted || (charId != _combatCharacter.GetId() && !EnemyRecordDict.ContainsKey(charId)) || !DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var element))
		{
			return;
		}
		int attackScore = GetAttackScore();
		int num = DomainManager.SpecialEffect.ModifyData(charId, skillId, (ushort)306, (int)power, -1, -1, -1) * 2;
		if (num > 0)
		{
			sbyte direction = element.GetDirection();
			if (direction != -1)
			{
				short index = (short)((direction == 0) ? combatSkillItem.DirectEffectID : combatSkillItem.ReverseEffectID);
				SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[index];
				if (specialEffectItem.RequireAttackPower < 0 || power >= specialEffectItem.RequireAttackPower)
				{
					num *= 2;
				}
			}
		}
		attackScore += num;
		Dictionary<short, (int, int)> skillRecord = ((charId == _combatCharacter.GetId()) ? SelfRecord.SkillRecord : EnemyRecordDict[charId].SkillRecord);
		UpdateSkillScore(skillRecord, skillId, attackScore);
	}

	private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
	{
		if (defenderId == _combatCharacter.GetId() && attackerId != _combatCharacter.GetId())
		{
			int indexByDistance = RecordCollection.GetIndexByDistance(DomainManager.Combat.GetCurrentDistance());
			if (isInner)
			{
				SelfRecord.MaxDamages[indexByDistance].Inner = Math.Max(SelfRecord.MaxDamages[indexByDistance].Inner, damageValue);
			}
			else
			{
				SelfRecord.MaxDamages[indexByDistance].Outer = Math.Max(SelfRecord.MaxDamages[indexByDistance].Outer, damageValue);
			}
		}
		if (isInner)
		{
			_singleAttackDamage.Inner += damageValue;
		}
		else
		{
			_singleAttackDamage.Outer += damageValue;
		}
	}

	private void OnAddMindDamageValue(DataContext context, int attackerId, int defenderId, int damageValue, int markCount, short combatSkillId)
	{
		if (defenderId == _combatCharacter.GetId() && attackerId != _combatCharacter.GetId())
		{
			int indexByDistance = RecordCollection.GetIndexByDistance(DomainManager.Combat.GetCurrentDistance());
			SelfRecord.MaxMindDamages[indexByDistance] = Math.Max(SelfRecord.MaxMindDamages[indexByDistance], damageValue);
		}
		_singleAttackMindDamage += damageValue;
	}

	private void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
	{
		if (_attacker != null && charId == _attacker.GetId())
		{
			_gotSkillNeedTrick = true;
		}
	}

	private unsafe void UpdateMaxValues(bool isAttacker)
	{
		DamageCompareData damageCompareData = DomainManager.Combat.GetDamageCompareData();
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		int indexByDistance = RecordCollection.GetIndexByDistance(currentDistance);
		if (isAttacker)
		{
			for (int i = 0; i < damageCompareData.HitType.Length; i++)
			{
				sbyte b = damageCompareData.HitType[i];
				if (b >= 0)
				{
					HitOrAvoidInts hitOrAvoidInts = SelfRecord.MaxHits[indexByDistance];
					hitOrAvoidInts.Items[b] = Math.Max(hitOrAvoidInts.Items[b], damageCompareData.HitValue[i]);
				}
			}
			if (damageCompareData.OuterAttackValue >= 0)
			{
				SelfRecord.MaxPenetrates[indexByDistance].Outer = Math.Max(SelfRecord.MaxPenetrates[indexByDistance].Outer, damageCompareData.OuterAttackValue);
			}
			if (damageCompareData.InnerAttackValue >= 0)
			{
				SelfRecord.MaxPenetrates[indexByDistance].Inner = Math.Max(SelfRecord.MaxPenetrates[indexByDistance].Inner, damageCompareData.InnerAttackValue);
			}
			return;
		}
		for (int j = 0; j < damageCompareData.HitType.Length; j++)
		{
			sbyte b2 = damageCompareData.HitType[j];
			if (b2 >= 0)
			{
				SelfRecord.MaxAvoid.Items[b2] = Math.Max(SelfRecord.MaxAvoid.Items[b2], damageCompareData.AvoidValue[j]);
			}
		}
		if (damageCompareData.OuterDefendValue >= 0)
		{
			SelfRecord.MaxPenetrateResist.Outer = Math.Max(SelfRecord.MaxPenetrateResist.Outer, damageCompareData.OuterDefendValue);
		}
		if (damageCompareData.InnerDefendValue >= 0)
		{
			SelfRecord.MaxPenetrateResist.Inner = Math.Max(SelfRecord.MaxPenetrateResist.Inner, damageCompareData.InnerDefendValue);
		}
	}

	private int GetAttackScore()
	{
		DamageStepCollection damageStepCollection = _defender.GetDamageStepCollection();
		int num = 0;
		num += _singleAttackDamage.Outer * 100 / (damageStepCollection.OuterDamageSteps.Sum() / 7);
		num += _singleAttackDamage.Inner * 100 / (damageStepCollection.InnerDamageSteps.Sum() / 7);
		return num + _singleAttackMindDamage * 100 / damageStepCollection.MindDamageStep;
	}

	private void UpdateWeaponScore(Dictionary<int, (int score, int zeroScoreCount)> weaponRecord, int weaponId, int newScore)
	{
		if (weaponRecord.ContainsKey(weaponId))
		{
			(int, int) value = weaponRecord[weaponId];
			if (newScore > 0)
			{
				value.Item1 = newScore;
				value.Item2 = 0;
			}
			else
			{
				value.Item2++;
			}
			weaponRecord[weaponId] = value;
		}
		else
		{
			weaponRecord.Add(weaponId, (newScore, (newScore <= 0) ? 1 : 0));
		}
	}

	private void UpdateSkillScore(Dictionary<short, (int score, int zeroScoreCount)> skillRecord, short skillId, int newScore)
	{
		if (skillRecord.ContainsKey(skillId))
		{
			(int, int) value = skillRecord[skillId];
			if (newScore > 0)
			{
				value.Item1 = newScore;
				value.Item2 = 0;
			}
			else
			{
				value.Item2++;
			}
			skillRecord[skillId] = value;
		}
		else
		{
			skillRecord.Add(skillId, (newScore, (newScore <= 0) ? 1 : 0));
		}
	}
}
