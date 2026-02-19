using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.Animal;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.SectStory.Fulong;

public class SoulWitheringBell : CarrierEffectBase
{
	private static readonly List<ushort> AllFieldIds = new List<ushort> { 8, 7, 9, 14, 11, 13, 10, 12, 16, 15 };

	private static readonly CValuePercent MinorAttributePercent = CValuePercent.op_Implicit(50);

	private DataUid _dataUid;

	private bool _transferred;

	protected override short CombatStateId => 241;

	public SoulWitheringBell(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		foreach (ushort allFieldId in AllFieldIds)
		{
			CreateAffectedData(allFieldId, (EDataModifyType)3, -1);
		}
		_dataUid = ParseCombatCharacterDataUid(base.EnemyChar.GetId(), 50);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_dataUid, base.DataHandlerKey, OnDefeatMarkChanged);
	}

	protected override void OnDisableSubClass(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_dataUid, base.DataHandlerKey);
	}

	private void OnDefeatMarkChanged(DataContext context, DataUid arg2)
	{
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(!base.CombatChar.IsAlly);
		if (_transferred || !DomainManager.Combat.IsCharacterHalfFallen(mainCharacter))
		{
			return;
		}
		_transferred = true;
		DomainManager.Combat.RemoveCombatState(context, base.CombatChar, 0, CombatStateId);
		DomainManager.Combat.AddCombatState(context, mainCharacter, 0, 242, 100, reverse: false, applyEffect: true, base.CharacterId);
		DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, 1717, 0);
		foreach (ushort allFieldId in AllFieldIds)
		{
			AppendAffectedData(context, mainCharacter.GetId(), allFieldId, (EDataModifyType)3, -1);
			RemoveAffectedData(context, base.CharacterId, allFieldId);
		}
		DomainManager.TaiwuEvent.OnEvent_SoulWitheringBellTransfer();
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (!AllFieldIds.Contains(dataKey.FieldId))
		{
			return dataValue;
		}
		if (_transferred ? (dataKey.CharId == base.CharacterId) : (dataKey.CharId != base.CharacterId))
		{
			return dataValue;
		}
		return dataValue * MinorAttributePercent;
	}
}
