using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

public abstract class ReduceMainAttributeCharacter : ReduceMainAttribute
{
	private bool _affecting;

	private readonly List<DataUid> _dataUids = new List<DataUid>();

	protected abstract bool DirectIsAlly { get; }

	protected abstract IReadOnlyList<ushort> FieldIds { get; }

	protected abstract EDataModifyType ModifyType { get; }

	protected abstract int CurrAddValue { get; }

	protected override bool IsAffect => _affecting;

	private bool TargetIsAlly => base.IsDirect == DirectIsAlly;

	protected abstract IEnumerable<DataUid> GetUpdateAffectingDataUids();

	protected ReduceMainAttributeCharacter()
	{
	}

	protected ReduceMainAttributeCharacter(CombatSkillKey skillKey, int type)
		: base(skillKey, type)
	{
	}

	protected abstract bool IsTargetMatchAffect(CombatCharacter target);

	protected virtual void OnAffected()
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
	}

	public override void OnDisable(DataContext context)
	{
		foreach (DataUid dataUid in _dataUids)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(dataUid, base.DataHandlerKey);
		}
		_dataUids.Clear();
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		base.OnDisable(context);
	}

	private void OnCombatBegin(DataContext context)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		foreach (ushort fieldId in FieldIds)
		{
			if (TargetIsAlly)
			{
				AppendAffectedData(context, fieldId, ModifyType, -1);
			}
			else
			{
				AppendAffectedAllEnemyData(context, fieldId, ModifyType, -1);
			}
		}
		_dataUids.AddRange(GetUpdateAffectingDataUids());
		foreach (DataUid dataUid in _dataUids)
		{
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(dataUid, base.DataHandlerKey, OnDataChanged);
		}
		UpdateAffecting(context);
	}

	public override void OnProcess(DataContext context, int counterType)
	{
		base.OnProcess(context, counterType);
		UpdateAffecting(context);
		if (_affecting)
		{
			InvalidAllCaches(context);
		}
	}

	private void OnDataChanged(DataContext context, DataUid arg2)
	{
		UpdateAffecting(context);
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		if (isAlly == base.CombatChar.IsAlly)
		{
			UpdateAffecting(context);
		}
	}

	private void UpdateAffecting(DataContext context)
	{
		bool flag = base.IsCurrent && CurrAddValue != 0 && IsTargetMatchAffect(TargetIsAlly ? base.CombatChar : base.EnemyChar);
		if (flag != _affecting)
		{
			_affecting = flag;
			InvalidAllCaches(context);
			if (_affecting)
			{
				OnAffected();
			}
		}
	}

	private void InvalidAllCaches(DataContext context)
	{
		foreach (ushort fieldId in FieldIds)
		{
			if (TargetIsAlly)
			{
				InvalidateCache(context, fieldId);
			}
			else
			{
				InvalidateAllEnemyCache(context, fieldId);
			}
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (!IsAffect || !FieldIds.Contains(dataKey.FieldId))
		{
			return base.GetModifyValue(dataKey, currModifyValue);
		}
		if (!DomainManager.Combat.TryGetElement_CombatCharacterDict(dataKey.CharId, out var element))
		{
			return base.GetModifyValue(dataKey, currModifyValue);
		}
		if (TargetIsAlly ? (element.GetId() != base.CharacterId) : (element.IsAlly == base.CombatChar.IsAlly))
		{
			return base.GetModifyValue(dataKey, currModifyValue);
		}
		return CurrAddValue;
	}
}
