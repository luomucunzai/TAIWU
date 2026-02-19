using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.LongYuFu;

public class DanXinSheYaoLong : CombatSkillEffectBase
{
	private const sbyte RequireInjury = 4;

	private const sbyte AffectSkillCount = 6;

	public DanXinSheYaoLong()
	{
	}

	public DanXinSheYaoLong(CombatSkillKey skillKey)
		: base(skillKey, 17125, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 219, base.SkillTemplateId), (EDataModifyType)3);
		for (int i = 0; i < characterList.Length; i++)
		{
			if (characterList[i] >= 0)
			{
				AffectDatas.Add(new AffectedDataKey(characterList[i], 169, -1), (EDataModifyType)3);
			}
		}
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		DomainManager.Combat.UpdateSkillCanUse(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly));
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		Dictionary<SkillEffectKey, short> effectDict = combatCharacter.GetSkillEffectCollection().EffectDict;
		if (base.CombatChar.GetInjuries().Get(2, isInnerInjury: false) >= 4 && effectDict != null && effectDict.Count > 0)
		{
			List<SkillEffectKey> list = ObjectPool<List<SkillEffectKey>>.Instance.Get();
			int num = Math.Min(6, effectDict.Count);
			list.Clear();
			list.AddRange(effectDict.Keys);
			for (int i = 0; i < num; i++)
			{
				int index = context.Random.Next(list.Count);
				SkillEffectKey key = list[index];
				list.RemoveAt(index);
				DomainManager.Combat.ChangeSkillEffectToMinCount(context, combatCharacter, key);
				DomainManager.Combat.AddGoneMadInjury(context, combatCharacter, key.SkillId);
			}
			ObjectPool<List<SkillEffectKey>>.Instance.Return(list);
			DomainManager.Combat.AddToCheckFallenSet(combatCharacter.GetId());
			ShowSpecialEffectTips(0);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			DomainManager.Combat.UpdateSkillCanUse(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly));
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.FieldId == 219)
		{
			return true;
		}
		if (dataKey.FieldId == 169 && !dataValue && dataKey.CustomParam0 == 2 && (base.CombatChar.GetPreparingSkillId() == base.SkillTemplateId || base.CombatChar.GetPerformingSkillId() == base.SkillTemplateId))
		{
			return true;
		}
		return dataValue;
	}
}
