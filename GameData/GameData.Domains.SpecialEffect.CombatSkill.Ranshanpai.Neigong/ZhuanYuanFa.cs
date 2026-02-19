using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong;

public class ZhuanYuanFa : CombatSkillEffectBase
{
	private const sbyte EvenAddPower = 10;

	private const sbyte EnemyAddPower = 10;

	private sbyte _currAddPower;

	private DataUid _selfBehaviorUid;

	private DataUid _enemyBehaviorUid;

	public ZhuanYuanFa()
	{
	}

	public ZhuanYuanFa(CombatSkillKey skillKey)
		: base(skillKey, 7001, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(199, (EDataModifyType)0, -1);
		UpdateAddPower(context);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_CombatSettlement(OnCombatSettlement);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_CombatSettlement(OnCombatSettlement);
	}

	private void OnCombatBegin(DataContext context)
	{
		if (DomainManager.Combat.IsCharInCombat(base.CharacterId))
		{
			_selfBehaviorUid = new DataUid(4, 0, (ulong)base.CharacterId, 78u);
			_enemyBehaviorUid = new DataUid(4, 0, (ulong)base.CurrEnemyChar.GetId(), 78u);
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_selfBehaviorUid, base.DataHandlerKey, UpdateAddPower);
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_enemyBehaviorUid, base.DataHandlerKey, UpdateAddPower);
			UpdateAddPower(context);
			Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
		}
	}

	private void OnCombatSettlement(DataContext context, sbyte combatStatus)
	{
		if (DomainManager.Combat.IsCharInCombat(base.CharacterId, checkCombatStatus: false))
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_selfBehaviorUid, base.DataHandlerKey);
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyBehaviorUid, base.DataHandlerKey);
			UpdateAddPower(context);
			Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		}
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		if (base.CombatChar.IsAlly != isAlly)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_enemyBehaviorUid, base.DataHandlerKey);
			_enemyBehaviorUid = new DataUid(4, 0, (ulong)base.CurrEnemyChar.GetId(), 78u);
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_enemyBehaviorUid, base.DataHandlerKey, UpdateAddPower);
			UpdateAddPower(context);
		}
	}

	private void UpdateAddPower(DataContext context, DataUid dataUid = default(DataUid))
	{
		sbyte behaviorType = CharObj.GetBehaviorType();
		_currAddPower = 0;
		if (behaviorType == 2)
		{
			_currAddPower += 10;
		}
		else if (DomainManager.Combat.IsInCombat())
		{
			sbyte behaviorType2 = base.CurrEnemyChar.GetCharacter().GetBehaviorType();
			bool flag = Math.Sign(behaviorType - 2) == Math.Sign(behaviorType2 - 2);
			if (behaviorType2 != 2 && base.IsDirect == flag)
			{
				_currAddPower += 10;
			}
		}
		DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return _currAddPower;
		}
		return 0;
	}
}
