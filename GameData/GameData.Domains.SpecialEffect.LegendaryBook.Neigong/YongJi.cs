using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Neigong;

public class YongJi : CombatSkillEffectBase
{
	private const sbyte SameAddPower = 10;

	private const sbyte ProduceAddPower = 5;

	private const sbyte CounterReducePower = -5;

	private DataUid _equipSkillUid;

	private sbyte SelfType => Config.CombatSkill.Instance[base.SkillTemplateId].FiveElements;

	public YongJi()
	{
		IsLegendaryBookEffect = true;
	}

	public YongJi(CombatSkillKey skillKey)
		: base(skillKey, 40002, -1)
	{
		IsLegendaryBookEffect = true;
	}

	public override void OnEnable(DataContext context)
	{
		_equipSkillUid = ParseCharDataUid(117);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_equipSkillUid, base.DataHandlerKey, OnDataChanged);
		CreateAffectedData(199, (EDataModifyType)0, -1);
	}

	public override void OnDisable(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_equipSkillUid, base.DataHandlerKey);
	}

	private void OnDataChanged(DataContext context, DataUid dataUid)
	{
		DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
	}

	private bool IsProduce(sbyte fiveElements)
	{
		return SelfType == FiveElementsType.Producing[fiveElements] || SelfType == FiveElementsType.Produced[fiveElements];
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId == base.SkillTemplateId || dataKey.FieldId != 199)
		{
			return 0;
		}
		if (!CharObj.IsCombatSkillEquipped(base.SkillTemplateId) || !CharObj.GetCombatSkillCanAffect(base.SkillTemplateId))
		{
			return 0;
		}
		sbyte fiveElements = Config.CombatSkill.Instance[dataKey.CombatSkillId].FiveElements;
		if (SelfType == fiveElements)
		{
			return 10;
		}
		if (SelfType == 5 || fiveElements == 5)
		{
			return 0;
		}
		return IsProduce(fiveElements) ? 5 : (-5);
	}
}
