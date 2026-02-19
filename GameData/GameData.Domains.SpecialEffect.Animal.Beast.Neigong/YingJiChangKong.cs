using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong;

public class YingJiChangKong : AnimalEffectBase
{
	private DataUid _hazardUid;

	private int _addedAttackRange;

	private int AddAttackRange => base.IsElite ? 40 : 20;

	private CValuePercent AddCriticalOddsPercent => CValuePercent.op_Implicit(base.IsElite ? 200 : 100);

	public YingJiChangKong()
	{
	}

	public YingJiChangKong(CombatSkillKey skillKey)
		: base(skillKey)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, -1), (EDataModifyType)0);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, -1), (EDataModifyType)0);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 254, -1), (EDataModifyType)1);
		_hazardUid = new DataUid(8, 10, (ulong)base.CharacterId, 81u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_hazardUid, base.DataHandlerKey, OnHazardChanged);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_hazardUid, base.DataHandlerKey);
	}

	private void OnHazardChanged(DataContext context, DataUid arg2)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		int num = AddAttackRange * base.CombatChar.AiController.GetHazardPercent();
		if (num != _addedAttackRange)
		{
			_addedAttackRange = num;
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		ushort fieldId = dataKey.FieldId;
		if ((uint)(fieldId - 145) <= 1u)
		{
			return _addedAttackRange;
		}
		if (dataKey.FieldId == 254)
		{
			ShowSpecialEffectTipsOnceInFrame(0);
			return (int)DomainManager.Combat.GetCurrentDistance() * AddCriticalOddsPercent;
		}
		return 0;
	}
}
