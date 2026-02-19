using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm;

public class BaiMoBaQiang : CombatSkillEffectBase
{
	private const int AddAttackRange = 20;

	private const int ChangeCdCount = int.MaxValue;

	private bool _castWithTeammate;

	private HitOrAvoidInts _addHits;

	private OuterAndInnerInts _addAttacks;

	public BaiMoBaQiang()
	{
	}

	public BaiMoBaQiang(CombatSkillKey skillKey)
		: base(skillKey, 6308, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		bool isAlly = (base.IsDirect ? base.CombatChar.IsAlly : (!base.CombatChar.IsAlly));
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(isAlly);
		foreach (CombatCharacter character in DomainManager.Combat.GetCharacters(isAlly))
		{
			if (mainCharacter != character)
			{
				_castWithTeammate = true;
				_addHits += character.GetCharacter().GetHitValues();
				_addAttacks += character.GetCharacter().GetPenetrations();
			}
		}
		if (_castWithTeammate)
		{
			ShowSpecialEffectTips(0);
			CreateAffectedData(44, (EDataModifyType)0, -1);
			CreateAffectedData(45, (EDataModifyType)0, -1);
			for (int i = 0; i < 4; i++)
			{
				CreateAffectedData((ushort)(32 + i), (EDataModifyType)0, -1);
			}
		}
		else
		{
			ShowSpecialEffectTips(2);
			CreateAffectedData(145, (EDataModifyType)0, -1);
			CreateAffectedData(146, (EDataModifyType)0, -1);
			CreateAffectedData(251, (EDataModifyType)3, -1);
			CreateAffectedData(248, (EDataModifyType)3, -1);
		}
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		base.OnDisable(context);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
	{
		if (SkillKey.IsMatch(charId, skillId))
		{
			if (PowerMatchAffectRequire(power) && _castWithTeammate)
			{
				DoChangeTeammateCommandCd(context);
			}
			RemoveSelf(context);
		}
	}

	private void DoChangeTeammateCommandCd(DataContext context)
	{
		if (!DomainManager.Combat.IsMainCharacter(base.CombatChar))
		{
			return;
		}
		bool isAlly = (base.IsDirect ? base.CombatChar.IsAlly : (!base.CombatChar.IsAlly));
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(isAlly);
		List<(CombatCharacter, int)> list = new List<(CombatCharacter, int)>();
		bool showTransferInjuryCommand = mainCharacter.GetShowTransferInjuryCommand();
		foreach (CombatCharacter character in DomainManager.Combat.GetCharacters(isAlly))
		{
			if (mainCharacter == character || base.CombatChar == character)
			{
				continue;
			}
			List<sbyte> currTeammateCommands = character.GetCurrTeammateCommands();
			List<byte> teammateCommandCdPercent = character.GetTeammateCommandCdPercent();
			for (int i = 0; i < currTeammateCommands.Count; i++)
			{
				if (currTeammateCommands[i] >= 0 && (!showTransferInjuryCommand || i <= 0) && i != character.ExecutingTeammateCommandIndex && (base.IsDirect ? ((teammateCommandCdPercent[i] == 0) ? 1 : 0) : ((int)teammateCommandCdPercent[i])) == 0)
				{
					list.Add((character, i));
				}
			}
		}
		foreach (var (combatCharacter, index) in RandomUtils.GetRandomUnrepeated(context.Random, int.MaxValue, list))
		{
			ShowSpecialEffectTipsOnceInFrame(1);
			if (base.IsDirect)
			{
				combatCharacter.ClearTeammateCommandCd(context, index);
			}
			else
			{
				combatCharacter.ResetTeammateCommandCd(context, index);
			}
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		ushort fieldId = dataKey.FieldId;
		if (1 == 0)
		{
		}
		int result;
		switch (fieldId)
		{
		case 32:
			result = _addHits[0];
			break;
		case 33:
			result = _addHits[1];
			break;
		case 34:
			result = _addHits[2];
			break;
		case 35:
			result = _addHits[3];
			break;
		case 44:
			result = _addAttacks.Outer;
			break;
		case 45:
			result = _addAttacks.Inner;
			break;
		case 145:
		case 146:
			result = 20;
			break;
		default:
			result = base.GetModifyValue(dataKey, currModifyValue);
			break;
		}
		if (1 == 0)
		{
		}
		return result;
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		bool flag = dataKey.SkillKey == SkillKey;
		bool flag2 = flag;
		if (flag2)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag3 = ((fieldId == 248 || fieldId == 251) ? true : false);
			flag2 = flag3;
		}
		if (flag2)
		{
			return true;
		}
		return base.GetModifiedValue(dataKey, dataValue);
	}
}
