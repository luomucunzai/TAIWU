using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.DefenseAndAssist;

public class ZheTianBiRiGong : DefenseSkillBase
{
	private const short AddDamageUnit = 160;

	private DataUid _defendSkillUid;

	public ZheTianBiRiGong()
	{
	}

	public ZheTianBiRiGong(CombatSkillKey skillKey)
		: base(skillKey, 14507)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_defendSkillUid = new DataUid(8, 10, (ulong)base.CharacterId, 63u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_defendSkillUid, base.DataHandlerKey, OnDefendSkillChanged);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_defendSkillUid, base.DataHandlerKey);
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
	}

	private void OnDefendSkillChanged(DataContext context, DataUid dataUid)
	{
		if (base.CombatChar.GetAffectingDefendSkillId() == base.SkillTemplateId)
		{
			AppendAffectedAllEnemyData(context, 89, (EDataModifyType)3, -1);
			AppendAffectedAllEnemyData(context, 271, (EDataModifyType)3, -1);
			DomainManager.Combat.UpdateAllTeammateCommandUsable(context, !base.CombatChar.IsAlly, -1);
		}
		else
		{
			ClearAffectedData(context);
			DomainManager.Combat.UpdateAllTeammateCommandUsable(context, !base.CombatChar.IsAlly, -1);
		}
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!isAlly);
			if (combatCharacter.TeammateBeforeMainChar >= 0 || combatCharacter.TeammateAfterMainChar >= 0 || !DomainManager.Combat.IsMainCharacter(combatCharacter))
			{
				DomainManager.Combat.ForceAllTeammateLeaveCombatField(context, !isAlly);
				ShowSpecialEffectTips(0);
			}
		}
	}

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		EDamageType customParam = (EDamageType)dataKey.CustomParam0;
		if (customParam != EDamageType.Bounce || dataKey.CustomParam1 != ((!base.IsDirect) ? 1 : 0) || !base.CanAffect)
		{
			return dataValue;
		}
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list.AddRange(DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly));
		list.RemoveAll((int id) => id < 0);
		DataContext context = DomainManager.Combat.Context;
		sbyte bodyPart = (sbyte)dataKey.CustomParam2;
		int num = 100 + 160 * (list.Count - 1);
		int num2 = (int)(dataValue / list.Count * num / 100);
		for (int num3 = 1; num3 < list.Count; num3++)
		{
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(list[num3]);
			DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, element_CombatCharacterDict, bodyPart, base.IsDirect ? num2 : 0, (!base.IsDirect) ? num2 : 0, base.SkillTemplateId);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		ShowSpecialEffectTips(1);
		return num2;
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId == base.CharacterId || !base.CanAffect || dataKey.FieldId != 271)
		{
			return dataValue;
		}
		return false;
	}
}
