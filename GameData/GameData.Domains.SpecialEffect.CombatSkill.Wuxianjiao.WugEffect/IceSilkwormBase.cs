using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.LifeRecord;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

public class IceSilkwormBase : WugEffectBase
{
	private static readonly CValuePercent CostNeiliPercent = CValuePercent.op_Implicit(50);

	private DataUid _disorderOfQiUid;

	private bool _affectedOnMonthChange;

	private int ChangeSilenceFramePercent => base.IsGood ? (-30) : 60;

	private int ChangeSilenceOddsPercent => base.IsElite ? (base.IsGood ? (-50) : 50) : 0;

	public static bool CanGrown(GameData.Domains.Character.Character character)
	{
		return character.GetDisorderOfQi() == DisorderLevelOfQi.MaxValue;
	}

	protected IceSilkwormBase()
	{
	}

	protected IceSilkwormBase(int charId, int type, short wugTemplateId, short effectId)
		: base(charId, type, wugTemplateId, effectId)
	{
		CostWugCount = 8;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		if (base.IsGrown)
		{
			CreateAffectedData(266, (EDataModifyType)3, -1);
			CreateAffectedData(298, (EDataModifyType)3, -1);
			Events.RegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish);
		}
		if (base.CanChangeToGrown)
		{
			_disorderOfQiUid = new DataUid(4, 0, (ulong)base.CharacterId, 21u);
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_disorderOfQiUid, base.DataHandlerKey, OnDisorderOfQiChanged);
			OnDisorderOfQiChanged(context, default(DataUid));
		}
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		if (base.IsGrown)
		{
			Events.UnRegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish);
		}
		if (base.CanChangeToGrown)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_disorderOfQiUid, base.DataHandlerKey);
		}
	}

	protected override void AddAffectDataAndEvent(DataContext context)
	{
		AppendAffectedData(context, base.CharacterId, 265, (EDataModifyType)1, -1);
		AppendAffectedData(context, base.CharacterId, 264, (EDataModifyType)1, -1);
		if (!base.IsGrown)
		{
			AppendAffectedData(context, base.CharacterId, 218, (EDataModifyType)2, -1);
		}
	}

	protected override void ClearAffectDataAndEvent(DataContext context)
	{
		RemoveAffectedData(context, base.CharacterId, 265);
		RemoveAffectedData(context, base.CharacterId, 264);
		if (!base.IsGrown)
		{
			RemoveAffectedData(context, base.CharacterId, 218);
		}
	}

	private void OnAdvanceMonthFinish(DataContext context)
	{
		if (_affectedOnMonthChange)
		{
			_affectedOnMonthChange = false;
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			AddLifeRecord(lifeRecordCollection.AddWugKingIceSilkwormLoseNeili);
		}
	}

	private void OnDisorderOfQiChanged(DataContext context, DataUid _)
	{
		if (CanGrown(CharObj))
		{
			ChangeToGrown(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !base.CanAffect)
		{
			return 0;
		}
		ushort fieldId = dataKey.FieldId;
		if ((uint)(fieldId - 264) <= 1u)
		{
			ShowEffectTips(DomainManager.Combat.Context, 1);
			CostWugInCombat(DomainManager.Combat.Context);
			return ChangeSilenceFramePercent;
		}
		if (dataKey.FieldId == 218)
		{
			ShowEffectTips(DomainManager.Combat.Context, 1);
			return ChangeSilenceOddsPercent;
		}
		return 0;
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 298 || !base.CanAffect || !base.IsElite)
		{
			return dataValue;
		}
		_affectedOnMonthChange = true;
		int maxNeili = CharObj.GetMaxNeili();
		return -maxNeili * CostNeiliPercent;
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 266 || !base.CanAffect)
		{
			return dataValue;
		}
		return true;
	}
}
