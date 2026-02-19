using System;
using System.Collections.Generic;
using Config;
using Config.ConfigCells;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.SpecialEffect.CombatSkill;

[SerializableGameData(NotForDisplayModule = true)]
public class CombatStateEffect : SpecialEffectBase
{
	private readonly sbyte _stateType;

	private readonly bool _reverse;

	private readonly Dictionary<ushort, (short value, EDataModifyType modifyType, int[] requireCustomParam)> _fieldDict = new Dictionary<ushort, (short, EDataModifyType, int[])>();

	private short _statePower;

	public CombatStateEffect()
	{
	}

	public CombatStateEffect(int charId, sbyte stateType, short stateId, short power, bool reverse)
		: base(charId, -1)
	{
		_stateType = stateType;
		_reverse = reverse;
		List<CombatStateProperty> propertyList = CombatState.Instance[stateId].PropertyList;
		foreach (CombatStateProperty item in propertyList)
		{
			SpecialEffectDataFieldItem specialEffectDataFieldItem = SpecialEffectDataField.Instance[item.SpecialEffectDataId];
			ushort key = AffectedDataHelper.FieldName2FieldId[specialEffectDataFieldItem.FieldName];
			_fieldDict.Add(key, (item.Value, (EDataModifyType)item.ModifyType, specialEffectDataFieldItem.RequireCustomParam));
		}
		_statePower = power;
	}

	public override void OnEnable(DataContext context)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		foreach (KeyValuePair<ushort, (short, EDataModifyType, int[])> item in _fieldDict)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, item.Key, -1), item.Value.Item2);
		}
	}

	public void ChangePower(DataContext context, short power)
	{
		_statePower = power;
		InvalidateCache(context);
	}

	public void InvalidateCache(DataContext context)
	{
		foreach (ushort key in _fieldDict.Keys)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, key);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		(short, EDataModifyType, int[]) tuple = _fieldDict[dataKey.FieldId];
		if ((tuple.Item3[0] >= 0 && tuple.Item3[0] != dataKey.CustomParam0) || (tuple.Item3[1] >= 0 && tuple.Item3[1] != dataKey.CustomParam1) || (tuple.Item3[2] >= 0 && tuple.Item3[2] != dataKey.CustomParam2))
		{
			return 0;
		}
		(int, int) totalPercentModifyValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(base.CharacterId, -1, 155, _stateType);
		int num = Math.Max(100 + totalPercentModifyValue.Item1 + totalPercentModifyValue.Item2, 0);
		return tuple.Item1 * _statePower / 100 * num / 100 * ((!_reverse) ? 1 : (-1));
	}
}
