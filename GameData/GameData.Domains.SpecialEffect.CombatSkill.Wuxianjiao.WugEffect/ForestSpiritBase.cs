using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

public class ForestSpiritBase : WugEffectBase
{
	private const int ReduceFavorability = 4000;

	private const int ReduceFavorabilityCharCount = 3;

	private const int TakeRevengeRateAddPercent = 900;

	public static bool CanGrown(GameData.Domains.Character.Character character)
	{
		List<short> featureIds = character.GetFeatureIds();
		return featureIds.Contains(217) || featureIds.Contains(218);
	}

	protected ForestSpiritBase()
	{
	}

	protected ForestSpiritBase(int charId, int type, short wugTemplateId, short effectId)
		: base(charId, type, wugTemplateId, effectId)
	{
		CostWugCount = 4;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		if (base.IsGrown)
		{
			CreateAffectedData(296, (EDataModifyType)1, -1);
		}
		if (base.CanChangeToGrown)
		{
			Events.RegisterHandler_XiangshuInfectionFeatureChangedEnd(OnXiangshuInfectionFeatureChangedEnd);
		}
		if (base.IsGrown)
		{
			Events.RegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish);
		}
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		if (base.CanChangeToGrown)
		{
			Events.UnRegisterHandler_XiangshuInfectionFeatureChangedEnd(OnXiangshuInfectionFeatureChangedEnd);
		}
		if (base.IsGrown)
		{
			Events.UnRegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish);
		}
	}

	protected override void AddAffectDataAndEvent(DataContext context)
	{
		Events.RegisterHandler_PoisonAffected(OnPoisonAffected);
	}

	protected override void ClearAffectDataAndEvent(DataContext context)
	{
		Events.UnRegisterHandler_PoisonAffected(OnPoisonAffected);
	}

	private void OnXiangshuInfectionFeatureChangedEnd(DataContext context, GameData.Domains.Character.Character character, short featureId)
	{
		if (CanGrown(CharObj) && base.CanAffect)
		{
			ChangeToGrown(context);
		}
	}

	private void OnAdvanceMonthFinish(DataContext context)
	{
		if (!base.CanAffect)
		{
			return;
		}
		Location location = CharObj.GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, list, 1, includeCenter: true);
		list2.Clear();
		list2.AddRange(list.Where((MapBlockData x) => x.CharacterSet != null).SelectMany((MapBlockData x) => x.CharacterSet));
		list2.Remove(base.CharacterId);
		CollectionUtils.Shuffle(context.Random, list2);
		int num = Math.Min(list2.Count, 3);
		bool flag = CharObj.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		for (int num2 = 0; num2 < num; num2++)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(list2[num2]);
			if (flag)
			{
				DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, element_Objects, CharObj, -4000);
			}
			else
			{
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, element_Objects, CharObj, -4000);
			}
			AddLifeRecord(lifeRecordCollection.AddWugForestSpiritReduceFavorability, element_Objects.GetId());
			if (base.IsElite)
			{
				GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, CharObj, element_Objects, flag, 6, new CharacterBecomeEnemyInfo(CharObj)
				{
					WugTemplateId = WugConfig.TemplateId
				});
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
	}

	private void OnPoisonAffected(DataContext context, int charId, sbyte poisonType)
	{
		if (charId == base.CharacterId && base.CanAffect)
		{
			CombatCharacter character = (base.IsGood ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly) : base.CombatChar);
			DomainManager.Combat.AppendMindDefeatMark(context, character, (base.IsGrown || !base.IsElite) ? 1 : 2, -1);
			ShowEffectTips(context, 1);
			if (base.IsElite)
			{
				ShowEffectTips(context, 2);
			}
			CostWugInCombat(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 296 || !base.CanAffect || !base.IsElite)
		{
			return 0;
		}
		return 900;
	}
}
