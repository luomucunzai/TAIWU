using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect;

public class DaWeiDeJinGangChu : PestleEffectBase
{
	private const sbyte AddPowerUnit = 10;

	private DataUid _selfNeiliAllocationUid;

	private DataUid _enemyNeiliAllocationUid;

	private int _addPower;

	public DaWeiDeJinGangChu()
	{
	}

	public DaWeiDeJinGangChu(int charId)
		: base(charId, 11405)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		_selfNeiliAllocationUid = ParseNeiliAllocationDataUid();
		_enemyNeiliAllocationUid = ParseNeiliAllocationDataUid(combatCharacter.GetId());
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_selfNeiliAllocationUid, base.DataHandlerKey, OnNeiliAllocationChanged);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_enemyNeiliAllocationUid, base.DataHandlerKey, OnNeiliAllocationChanged);
		UpdateAddPower(context);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1), (EDataModifyType)1);
		Events.RegisterHandler_ChangeWeapon(OnChangeWeapon);
		Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_selfNeiliAllocationUid, base.DataHandlerKey);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyNeiliAllocationUid, base.DataHandlerKey);
		Events.UnRegisterHandler_ChangeWeapon(OnChangeWeapon);
		Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		base.OnDisable(context);
	}

	private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
	{
		if (charId == base.CharacterId)
		{
			UpdateAddPower(context);
		}
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		if (isAlly != base.CombatChar.IsAlly)
		{
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyNeiliAllocationUid, base.DataHandlerKey);
			_enemyNeiliAllocationUid = ParseNeiliAllocationDataUid(combatCharacter.GetId());
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_enemyNeiliAllocationUid, base.DataHandlerKey, OnNeiliAllocationChanged);
			UpdateAddPower(context);
		}
	}

	private void OnNeiliAllocationChanged(DataContext context, DataUid dataUid)
	{
		UpdateAddPower(context);
	}

	private unsafe void UpdateAddPower(DataContext context)
	{
		_addPower = 0;
		if (DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) && base.CanAffect)
		{
			NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
			NeiliAllocation neiliAllocation2 = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly).GetNeiliAllocation();
			for (byte b = 0; b < 4; b++)
			{
				if (base.IsDirect ? (neiliAllocation.Items[(int)b] > neiliAllocation2.Items[(int)b]) : (neiliAllocation.Items[(int)b] < neiliAllocation2.Items[(int)b]))
				{
					_addPower += 10;
				}
			}
		}
		DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !base.CanAffect)
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
