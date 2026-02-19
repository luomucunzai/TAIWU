using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;

public class BreakBodyEffectBase : SpecialEffectBase
{
	private const sbyte BreakNeedInjury = 4;

	protected sbyte[] AffectBodyParts;

	protected bool IsInner;

	protected short FeatureId;

	private DataUid _injuryUid;

	protected BreakBodyEffectBase()
	{
	}

	protected BreakBodyEffectBase(int charId, int type)
		: base(charId, type)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_injuryUid = new DataUid(4, 0, (ulong)base.CharacterId, 26u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_injuryUid, base.DataHandlerKey, OnInjuriesUpdate);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 168, -1), (EDataModifyType)3);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_injuryUid, base.DataHandlerKey);
	}

	private void OnInjuriesUpdate(DataContext context, DataUid dataUid)
	{
		Injuries injuries = CharObj.GetInjuries();
		bool flag = true;
		for (int i = 0; i < AffectBodyParts.Length; i++)
		{
			if (injuries.Get(AffectBodyParts[i], IsInner) > 0)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			CharObj.RemoveFeature(context, FeatureId);
			DomainManager.SpecialEffect.Remove(context, Id);
		}
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 168 && AffectBodyParts.Exist((sbyte)dataKey.CustomParam0) && dataKey.CustomParam1 == (IsInner ? 1 : 0))
		{
			return Math.Min(4, dataValue);
		}
		return dataValue;
	}

	protected override int GetSubClassSerializedSize()
	{
		return 1 + AffectBodyParts.Length + 1 + 2;
	}

	protected unsafe override int SerializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		sbyte b = (sbyte)(*ptr = (byte)(sbyte)AffectBodyParts.Length);
		ptr++;
		for (int i = 0; i < b; i++)
		{
			*ptr = (byte)AffectBodyParts[i];
			ptr++;
		}
		*ptr = (IsInner ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = FeatureId;
		ptr += 2;
		return (int)(ptr - pData);
	}

	protected unsafe override int DeserializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		sbyte b = (sbyte)(*ptr);
		ptr++;
		AffectBodyParts = new sbyte[b];
		for (int i = 0; i < b; i++)
		{
			AffectBodyParts[i] = (sbyte)(*ptr);
			ptr++;
		}
		IsInner = *ptr != 0;
		ptr++;
		FeatureId = *(short*)ptr;
		ptr += 2;
		return (int)(ptr - pData);
	}
}
