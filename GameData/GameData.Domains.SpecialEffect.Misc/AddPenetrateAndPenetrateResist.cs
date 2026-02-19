using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;

namespace GameData.Domains.SpecialEffect.Misc;

public class AddPenetrateAndPenetrateResist : SpecialEffectBase
{
	private OuterAndInnerInts _addPenetrate;

	private OuterAndInnerInts _addPenetrateResist;

	public AddPenetrateAndPenetrateResist()
	{
	}

	public AddPenetrateAndPenetrateResist(int charId, OuterAndInnerInts addPenetrate, OuterAndInnerInts addPenetrateResist)
		: base(charId, 1000000)
	{
		_addPenetrate = addPenetrate;
		_addPenetrateResist = addPenetrateResist;
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 44, -1), (EDataModifyType)0);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 45, -1), (EDataModifyType)0);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 46, -1), (EDataModifyType)0);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 47, -1), (EDataModifyType)0);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		if (dataKey.FieldId == 44)
		{
			return _addPenetrate.Outer;
		}
		if (dataKey.FieldId == 45)
		{
			return _addPenetrate.Inner;
		}
		if (dataKey.FieldId == 46)
		{
			return _addPenetrateResist.Outer;
		}
		if (dataKey.FieldId == 47)
		{
			return _addPenetrateResist.Inner;
		}
		return 0;
	}

	protected override int GetSubClassSerializedSize()
	{
		return _addPenetrate.GetSerializedSize() + _addPenetrateResist.GetSerializedSize();
	}

	protected unsafe override int SerializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _addPenetrate.Outer;
		ptr += 4;
		*(int*)ptr = _addPenetrate.Inner;
		ptr += 4;
		*(int*)ptr = _addPenetrateResist.Outer;
		ptr += 4;
		*(int*)ptr = _addPenetrateResist.Inner;
		ptr += 4;
		return (int)(ptr - pData);
	}

	protected unsafe override int DeserializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		_addPenetrate.Outer = *(int*)ptr;
		ptr += 4;
		_addPenetrate.Inner = *(int*)ptr;
		ptr += 4;
		_addPenetrateResist.Outer = *(int*)ptr;
		ptr += 4;
		_addPenetrateResist.Inner = *(int*)ptr;
		ptr += 4;
		return (int)(ptr - pData);
	}
}
