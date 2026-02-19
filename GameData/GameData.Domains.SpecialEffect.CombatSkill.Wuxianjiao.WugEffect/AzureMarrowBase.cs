using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

public class AzureMarrowBase : WugEffectBase
{
	private const int MakeLoveRateAddPercent = 100;

	private const int AddPoisonTypeCount = 3;

	private const int AddPoisonLevel = 3;

	private const int AddPoisonValue = 3600;

	private const int ChangeWugCount = 3;

	private bool _affected;

	protected AzureMarrowBase()
	{
	}

	protected AzureMarrowBase(int charId, int type, short wugTemplateId, short effectId)
		: base(charId, type, wugTemplateId, effectId)
	{
		CostWugCount = 28;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		if (base.IsGrown)
		{
			CreateAffectedData(268, (EDataModifyType)2, -1);
			CreateAffectedData(299, (EDataModifyType)3, -1);
		}
		else
		{
			CreateAffectedData(293, (EDataModifyType)0, -1);
		}
		if (base.IsGrown)
		{
			Events.RegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish);
		}
		Events.RegisterHandler_MakeLove(OnMakeLove);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		if (base.IsGrown)
		{
			Events.UnRegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish);
		}
		Events.UnRegisterHandler_MakeLove(OnMakeLove);
	}

	protected override void AddAffectDataAndEvent(DataContext context)
	{
		Events.RegisterHandler_AddFatalDamageMark(OnAddFatalDamageMark);
	}

	protected override void ClearAffectDataAndEvent(DataContext context)
	{
		Events.UnRegisterHandler_AddFatalDamageMark(OnAddFatalDamageMark);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !base.CanAffect)
		{
			return 0;
		}
		ushort fieldId = dataKey.FieldId;
		if (1 == 0)
		{
		}
		int result = fieldId switch
		{
			268 => 100, 
			293 => base.IsElite ? (base.IsGood ? 1 : (-1)) : 0, 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 299 || !base.CanAffect)
		{
			return dataValue;
		}
		return true;
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
		DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, list, 1, includeCenter: true);
		IEnumerable<GameData.Domains.Character.Character> enumerable = list.Where((MapBlockData x) => x.CharacterSet != null).SelectMany((MapBlockData x) => x.CharacterSet).Select(DomainManager.Character.GetElement_Objects);
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		if (collection.Contains(CharObj.GetId()))
		{
			enumerable = enumerable.Union(collection.Select(DomainManager.Character.GetElement_Objects));
		}
		if (enumerable.All((GameData.Domains.Character.Character x) => x.GetGender() == CharObj.GetGender()))
		{
			List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
			list2.Clear();
			for (sbyte b = 0; b < 6; b++)
			{
				list2.Add(b);
			}
			CollectionUtils.Shuffle(context.Random, list2);
			for (int num = 0; num < 3; num++)
			{
				sbyte poisonType = list2[num];
				CharObj.ChangePoisoned(context, poisonType, 3, 3600);
			}
			ObjectPool<List<sbyte>>.Instance.Return(list2);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			AddLifeRecord(lifeRecordCollection.AddWugAzureMarrowAddPoison);
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	private void OnMakeLove(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character target, sbyte makeLoveState)
	{
		if (!base.CanAffect || !CheckValid() || (character.GetId() != base.CharacterId && target.GetId() != base.CharacterId) || makeLoveState == 4 || character.GetGender() == target.GetGender())
		{
			return;
		}
		GameData.Domains.Character.Character character2 = ((character.GetId() == base.CharacterId) ? target : character);
		int id = character2.GetId();
		EatingItems eatingItems = character2.GetEatingItems();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int num = eatingItems.IndexOfWug(WugConfig.WugType);
		MedicineItem medicineItem = ((num < 0) ? null : Config.Medicine.Instance[eatingItems.Get(num).TemplateId]);
		if (base.IsGrown && base.IsElite && medicineItem?.WugGrowthType != 4)
		{
			short wugTemplateId = ItemDomain.GetWugTemplateId(WugConfig.WugType, 3);
			character2.AddWug(context, wugTemplateId);
			AddLifeRecord(lifeRecordCollection.AddWugAzureMarrowAddWug, id, wugTemplateId);
		}
		else if (base.CanChangeToGrown)
		{
			short wugTemplateId2 = ItemDomain.GetWugTemplateId(WugConfig.WugType, 4);
			if (WugGrowthType.CanChangeToGrown(medicineItem?.WugGrowthType ?? (-1)))
			{
				CharObj.AddWug(context, wugTemplateId2);
				AddLifeRecord(lifeRecordCollection.AddWugAzureMarrowChangeToGrown, id, base.CharacterId, wugTemplateId2);
			}
			else
			{
				CharObj.RemoveWug(context, WugConfig.TemplateId);
			}
			character2.AddWug(context, wugTemplateId2);
			AddLifeRecord(lifeRecordCollection.AddWugAzureMarrowChangeToGrown, character2.GetId(), wugTemplateId2);
		}
	}

	private void OnAddFatalDamageMark(DataContext context, CombatCharacter combatChar, int count)
	{
		if (combatChar.GetId() == base.CharacterId && !_affected && base.CanAffect)
		{
			_affected = true;
			Events.RegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd);
		}
	}

	private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		if (combatChar.GetId() != base.CharacterId)
		{
			return;
		}
		Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd);
		_affected = false;
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list2.Clear();
		EatingItems eatingItems = CharObj.GetEatingItems();
		for (sbyte b = 0; b < 8; b++)
		{
			if (b != WugConfig.WugType)
			{
				int num = eatingItems.IndexOfWug(b);
				if (num >= 0)
				{
					sbyte wugGrowthType = Config.Medicine.Instance[eatingItems.Get(num).TemplateId].WugGrowthType;
					sbyte changedWugGrowthType = GetChangedWugGrowthType(wugGrowthType);
					if (changedWugGrowthType >= 0)
					{
						short wugTemplateId = ItemDomain.GetWugTemplateId(b, changedWugGrowthType);
						if (WugGrowthType.IsWugGrowthTypeCombatOnly(wugGrowthType))
						{
							list.Add(wugTemplateId);
						}
						else
						{
							list2.Add(wugTemplateId);
						}
					}
				}
			}
		}
		bool flag = false;
		foreach (short item in RandomUtils.GetRandomUnrepeated(context.Random, 3, list, list2))
		{
			DomainManager.Combat.AddWug(context, combatChar, item, base.CharacterId, EWugReplaceType.All);
			ShowEffectTips(context, (byte)((!base.IsGood || !list2.Contains(item)) ? 1u : 2u));
			flag = true;
		}
		if (flag)
		{
			CostWugInCombat(context);
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
	}

	private sbyte GetChangedWugGrowthType(sbyte oldGrowthType)
	{
		if (WugGrowthType.IsWugGrowthTypeCombatOnly(oldGrowthType))
		{
			return (sbyte)(base.IsGood ? 1 : 3);
		}
		if (oldGrowthType == 4)
		{
			return (sbyte)(base.IsGood ? 1 : (-1));
		}
		return (sbyte)(base.IsGood ? (-1) : 4);
	}
}
