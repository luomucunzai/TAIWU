using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm;

public class FuLongZuiBaXianQuan : CombatSkillEffectBase
{
	private const sbyte HitAvoidChangePercent = 20;

	public FuLongZuiBaXianQuan()
	{
	}

	public FuLongZuiBaXianQuan(CombatSkillKey skillKey)
		: base(skillKey, 14101, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(base.IsDirect ? 60 : 90), -1), (EDataModifyType)1);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(base.IsDirect ? 61 : 91), -1), (EDataModifyType)1);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(base.IsDirect ? 62 : 92), -1), (EDataModifyType)1);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(base.IsDirect ? 63 : 93), -1), (EDataModifyType)1);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_NormalAttackBegin(OnNormalAttackAllBegin);
		Events.RegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd);
		Events.RegisterHandler_CompareDataCalcFinished(OnCompareDataCalcFinished);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		for (int i = 0; i < characterList.Length; i++)
		{
			if (characterList[i] >= 0)
			{
				CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(characterList[i]);
				if (base.IsDirect)
				{
					element_CombatCharacterDict.ChangeAvoidTypeEffectCount--;
				}
				else
				{
					element_CombatCharacterDict.ChangeHitTypeEffectCount--;
				}
			}
		}
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_NormalAttackBegin(OnNormalAttackAllBegin);
		Events.UnRegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd);
		Events.UnRegisterHandler_CompareDataCalcFinished(OnCompareDataCalcFinished);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (!IsSrcSkillPerformed)
		{
			if (PowerMatchAffectRequire(power))
			{
				int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < characterList.Length; i++)
				{
					if (characterList[i] >= 0)
					{
						CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(characterList[i]);
						if (base.IsDirect)
						{
							element_CombatCharacterDict.ChangeAvoidTypeEffectCount++;
						}
						else
						{
							element_CombatCharacterDict.ChangeHitTypeEffectCount++;
						}
					}
				}
				IsSrcSkillPerformed = true;
				AddMaxEffectCount();
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

	private void OnNormalAttackAllBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
	{
		if (base.CombatChar == (base.IsDirect ? attacker : defender) && IsSrcSkillPerformed && pursueIndex <= 0)
		{
			ShowSpecialEffectTips(0);
		}
	}

	private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
	{
		if (base.CombatChar == (base.IsDirect ? attacker : defender))
		{
			ReduceEffectCount();
		}
	}

	private void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
	{
		CombatCharacter attacker = context.Attacker;
		CombatCharacter defender = context.Defender;
		if (!IsSrcSkillPerformed || context.SkillTemplateId >= 0 || base.CombatChar != (base.IsDirect ? attacker : defender))
		{
			return;
		}
		sbyte bodyPart = context.BodyPart;
		Weapon weapon = context.Weapon;
		sbyte b = compareData.HitType[0];
		int num = (base.IsDirect ? compareData.AvoidValue : compareData.HitValue)[0];
		for (sbyte b2 = 0; b2 < 4; b2++)
		{
			if (b2 != b)
			{
				num = Math.Min(num, base.IsDirect ? defender.GetAvoidValue(b2, bodyPart, -1) : attacker.GetHitValue(weapon, b2, bodyPart, 0, -1));
			}
		}
		(base.IsDirect ? compareData.AvoidValue : compareData.HitValue)[0] = num;
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			RemoveSelf(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CustomParam0 >= 0 || !CharObj.GetEatingItems().ContainsWine())
		{
			return 0;
		}
		return -20;
	}
}
