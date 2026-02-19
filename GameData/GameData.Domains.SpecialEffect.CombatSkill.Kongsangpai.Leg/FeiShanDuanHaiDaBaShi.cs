using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg;

public class FeiShanDuanHaiDaBaShi : CombatSkillEffectBase
{
	private const int AddDamageValueBasePercentDivisor = 8;

	private sbyte StatePowerUnit = 25;

	private int _affectingMoveCharId;

	private int _affectingCastCharId;

	private DataUid _selfAttributeUid;

	private DataUid _enemyAttributeUid;

	private DataUid _enemyAgileSkillUid;

	private CombatCharacter AffectingMoveChar => DomainManager.Combat.GetElement_CombatCharacterDict(_affectingMoveCharId);

	private CombatCharacter AffectingCastChar => DomainManager.Combat.GetElement_CombatCharacterDict(_affectingCastCharId);

	public FeiShanDuanHaiDaBaShi()
	{
	}

	public FeiShanDuanHaiDaBaShi(CombatSkillKey skillKey)
		: base(skillKey, 10306, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, base.SkillTemplateId), (EDataModifyType)1);
		_affectingMoveCharId = -1;
		_affectingCastCharId = -1;
		_selfAttributeUid = new DataUid(4, 0, (ulong)base.CharacterId, base.IsDirect ? 87u : 89u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_selfAttributeUid, base.DataHandlerKey, UpdateAffecting);
		Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDataAdded(DataContext context)
	{
		UpdateEnemyDataUid(context, init: true);
		UpdateAffecting(context, _selfAttributeUid);
	}

	public override void OnDisable(DataContext context)
	{
		if (_affectingMoveCharId >= 0)
		{
			AffectingMoveChar.SetMobilityLockEffectCount((short)(AffectingMoveChar.GetMobilityLockEffectCount() - 1), context);
		}
		if (_affectingCastCharId >= 0)
		{
			AffectingCastChar.PreventCastSkillEffectCount--;
			DomainManager.Combat.UpdateSkillCanUse(context, AffectingCastChar);
		}
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_selfAttributeUid, base.DataHandlerKey);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyAttributeUid, base.DataHandlerKey);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyAgileSkillUid, base.DataHandlerKey);
		Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		if (isAlly != base.CombatChar.IsAlly)
		{
			UpdateEnemyDataUid(context);
			UpdateAffecting(context, _selfAttributeUid);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if (power > 0)
			{
				DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 2, (short)(base.IsDirect ? 51 : 52), StatePowerUnit * power / 10);
				ShowSpecialEffectTips(1);
			}
			RemoveSelf(context);
		}
	}

	private void UpdateEnemyDataUid(DataContext context, bool init = false)
	{
		if (!init)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyAttributeUid, base.DataHandlerKey);
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyAgileSkillUid, base.DataHandlerKey);
			ClearAffectedData(context);
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		_enemyAttributeUid = new DataUid(4, 0, (ulong)combatCharacter.GetId(), base.IsDirect ? 87u : 89u);
		_enemyAgileSkillUid = new DataUid(8, 10, (ulong)combatCharacter.GetId(), 62u);
		AppendAffectedData(context, combatCharacter.GetId(), 151, (EDataModifyType)3, -1);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_enemyAttributeUid, base.DataHandlerKey, UpdateAffecting);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_enemyAgileSkillUid, base.DataHandlerKey, UpdateAffecting);
	}

	private void UpdateAffecting(DataContext context, DataUid dataUid)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		GameData.Domains.Character.Character character = combatCharacter.GetCharacter();
		short num = (base.IsDirect ? CharObj.GetRecoveryOfFlaw() : CharObj.GetRecoveryOfBlockedAcupoint());
		short num2 = (base.IsDirect ? character.GetRecoveryOfFlaw() : character.GetRecoveryOfBlockedAcupoint());
		bool flag = num > num2;
		bool flag2 = flag && DomainManager.SpecialEffect.ModifyData(combatCharacter.GetId(), -1, 147, dataValue: true);
		if (_affectingMoveCharId >= 0 != flag2)
		{
			CombatCharacter combatCharacter2 = (flag2 ? combatCharacter : AffectingMoveChar);
			if (flag2)
			{
				combatCharacter2.SetMobilityLockEffectCount((short)(combatCharacter2.GetMobilityLockEffectCount() + 1), context);
			}
			else
			{
				combatCharacter2.SetMobilityLockEffectCount((short)(combatCharacter2.GetMobilityLockEffectCount() - 1), context);
			}
			_affectingMoveCharId = (flag2 ? combatCharacter.GetId() : (-1));
		}
		if (_affectingCastCharId >= 0 != flag)
		{
			CombatCharacter combatCharacter3 = (flag ? combatCharacter : AffectingCastChar);
			if (flag)
			{
				combatCharacter3.PreventCastSkillEffectCount++;
			}
			else
			{
				combatCharacter3.PreventCastSkillEffectCount--;
			}
			DomainManager.Combat.UpdateSkillCanUse(context, combatCharacter3);
			_affectingCastCharId = (flag ? combatCharacter.GetId() : (-1));
		}
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.FieldId == 151 && _affectingMoveCharId >= 0)
		{
			return 0;
		}
		return dataValue;
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !CombatCharPowerMatchAffectRequire())
		{
			return 0;
		}
		if (dataKey.FieldId == 69 && dataKey.CustomParam0 == ((!base.IsDirect) ? 1 : 0))
		{
			int num = (base.IsDirect ? (base.CombatChar.GetCharacter().GetRecoveryOfFlaw() - base.CurrEnemyChar.GetCharacter().GetRecoveryOfFlaw()) : (base.CombatChar.GetCharacter().GetRecoveryOfBlockedAcupoint() - base.CurrEnemyChar.GetCharacter().GetRecoveryOfBlockedAcupoint()));
			if (num > 0)
			{
				ShowSpecialEffectTips(2);
				return num / 8;
			}
		}
		return 0;
	}
}
