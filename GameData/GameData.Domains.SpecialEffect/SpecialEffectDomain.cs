using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;
using GameData.Domains.SpecialEffect.EquipmentEffect;
using GameData.Domains.SpecialEffect.Misc;
using GameData.Domains.Taiwu;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect;

[GameDataDomain(17)]
public class SpecialEffectDomain : BaseGameDataDomain
{
	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private Dictionary<long, SpecialEffectWrapper> _effectDict;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private long _nextEffectId;

	[DomainData(DomainDataType.ObjectCollection, false, false, false, false)]
	private readonly Dictionary<int, AffectedData> _affectedDatas;

	private readonly ConcurrentBag<(long effectId, int charId, short skillId)> _brokenEffectsChangedDuringAdvance = new ConcurrentBag<(long, int, short)>();

	private readonly Dictionary<long, long> _featureEffectDict = new Dictionary<long, long>();

	private readonly List<CastBoostEffectDisplayData> _costNeiliEffectDisplayDataCache = new List<CastBoostEffectDisplayData>();

	private bool _domainInitialized;

	public static readonly Dictionary<short, string> BreakBodyFeatureEffectClassName = new Dictionary<short, string>
	{
		[246] = "CombatSkill.Xuehoujiao.BreakBodyEffect.HeadHurt",
		[247] = "CombatSkill.Xuehoujiao.BreakBodyEffect.HeadCrash",
		[248] = "CombatSkill.Xuehoujiao.BreakBodyEffect.ChestHurt",
		[249] = "CombatSkill.Xuehoujiao.BreakBodyEffect.ChestCrash",
		[250] = "CombatSkill.Xuehoujiao.BreakBodyEffect.BellyHurt",
		[251] = "CombatSkill.Xuehoujiao.BreakBodyEffect.BellyCrash",
		[252] = "CombatSkill.Xuehoujiao.BreakBodyEffect.HandHurt",
		[253] = "CombatSkill.Xuehoujiao.BreakBodyEffect.HandCrash",
		[254] = "CombatSkill.Xuehoujiao.BreakBodyEffect.LegHurt",
		[255] = "CombatSkill.Xuehoujiao.BreakBodyEffect.LegCrash"
	};

	private bool _updatingErrorEffect;

	private readonly HashSet<int> _requestUpdateCombatSkillCharIds = new HashSet<int>();

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[3][];

	private static readonly DataInfluence[][] CacheInfluencesAffectedDatas = new DataInfluence[329][];

	private readonly ObjectCollectionDataStates _dataStatesAffectedDatas = new ObjectCollectionDataStates(329, 0);

	public readonly ObjectCollectionHelperData HelperDataAffectedDatas;

	private Queue<uint> _pendingLoadingOperationIds;

	private static readonly List<Action> ResetOnInitializeGameDataModuleEffects = new List<Action>();

	private void OnInitializedDomainData()
	{
	}

	private void InitializeOnInitializeGameDataModule()
	{
	}

	private void InitializeOnEnterNewWorld()
	{
		_domainInitialized = false;
		Events.RegisterHandler_AddWug(OnAddWug);
		InvokeResetHandlers();
	}

	private void OnLoadedArchiveData()
	{
		_domainInitialized = false;
		Events.RegisterHandler_AddWug(OnAddWug);
		InvokeResetHandlers();
	}

	public override void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
	{
		if (!isNewWorld)
		{
			OnLoadedAllArchiveData(context);
		}
	}

	private void OnLoadedAllArchiveData(DataContext context)
	{
		List<long> list = new List<long>();
		_updatingErrorEffect = true;
		foreach (KeyValuePair<long, SpecialEffectWrapper> item in _effectDict)
		{
			SpecialEffectBase effect = item.Value.Effect;
			if (IsErrorEffectOnLoad(effect))
			{
				list.Add(item.Key);
				continue;
			}
			effect.OnEnable(context);
			AddDataUid(context, effect);
			effect.OnDataAdded(context);
			if (effect is FeatureEffectBase featureEffectBase)
			{
				_featureEffectDict.Add(GetFeatureEffectKey(featureEffectBase.CharacterId, featureEffectBase.FeatureId), featureEffectBase.Id);
			}
		}
		foreach (long item2 in list)
		{
			RemoveElement_EffectDict(item2, context);
		}
		_updatingErrorEffect = false;
		_requestUpdateCombatSkillCharIds.Remove(DomainManager.Taiwu.GetTaiwuCharId());
		foreach (int requestUpdateCombatSkillCharId in _requestUpdateCombatSkillCharIds)
		{
			if (DomainManager.Character.TryGetElement_Objects(requestUpdateCombatSkillCharId, out var element))
			{
				UpdateEquippedSkillEffect(context, element);
			}
		}
		_requestUpdateCombatSkillCharIds.Clear();
		_domainInitialized = true;
	}

	private bool IsErrorEffect(SpecialEffectBase effect)
	{
		if (effect.Type < 0)
		{
			return true;
		}
		if (effect.CharObj == null)
		{
			return true;
		}
		if (effect is CombatSkillEffectBase { IsLegendaryBookEffect: false } combatSkillEffectBase)
		{
			sbyte effectActiveType = Config.SpecialEffect.Instance[combatSkillEffectBase.EffectId].EffectActiveType;
			if (effectActiveType == 1 || effectActiveType == 0)
			{
				return true;
			}
			if (effectActiveType == 2 && !effect.CharObj.IsCombatSkillEquipped(combatSkillEffectBase.SkillKey.SkillTemplateId))
			{
				return true;
			}
			if (!DomainManager.CombatSkill.TryGetElement_CombatSkills(combatSkillEffectBase.SkillKey, out var _))
			{
				return true;
			}
		}
		if (effect is WugEffectBase wugEffectBase && effect.CharObj.GetEatingItems().IndexOfWug(wugEffectBase.WugTemplateId) < 0)
		{
			return true;
		}
		return false;
	}

	private bool IsErrorEffectOnLoad(SpecialEffectBase effect)
	{
		if (IsErrorEffect(effect))
		{
			return true;
		}
		if (effect is CombatSkillEffectBase { IsLegendaryBookEffect: false } combatSkillEffectBase)
		{
			return combatSkillEffectBase.SkillInstance.GetSpecialEffectId() != effect.Id;
		}
		return false;
	}

	[DomainMethod]
	public List<CastBoostEffectDisplayData> GetAllCostNeiliEffectData(int charId, short skillId)
	{
		_costNeiliEffectDisplayDataCache.Clear();
		if (!DomainManager.Combat.IsCharInCombat(charId))
		{
			return _costNeiliEffectDisplayDataCache;
		}
		if (skillId >= 0)
		{
			ModifyData(charId, skillId, 235, _costNeiliEffectDisplayDataCache);
		}
		return _costNeiliEffectDisplayDataCache;
	}

	[DomainMethod]
	public void CostNeiliEffect(DataContext context, int charId, short skillId, short effectId)
	{
		Events.RaiseCombatCostNeiliConfirm(context, charId, skillId, effectId);
	}

	[DomainMethod]
	public bool CanCostTrickDuringPreparingSkill(int charId, short skillId)
	{
		return ModifyData(charId, skillId, 324, dataValue: false);
	}

	[DomainMethod]
	public bool CostTrickDuringPreparingSkill(DataContext context, int charId, int trickIndex)
	{
		if (!DomainManager.Combat.IsCharInCombat(charId))
		{
			return false;
		}
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
		short preparingSkillId = element_CombatCharacterDict.GetPreparingSkillId();
		if (preparingSkillId < 0 || !CanCostTrickDuringPreparingSkill(charId, preparingSkillId))
		{
			return false;
		}
		IReadOnlyDictionary<int, sbyte> tricks = element_CombatCharacterDict.GetTricks().Tricks;
		if (!tricks.TryGetValue(trickIndex, out var value))
		{
			return false;
		}
		DomainManager.Combat.RemoveTrick(context, element_CombatCharacterDict, value, 1, removedByAlly: true, trickIndex);
		Events.RaiseCostTrickDuringPreparingSkill(context, charId);
		return true;
	}

	public long Add(DataContext context, SpecialEffectBase effect)
	{
		if (_effectDict.ContainsKey(_nextEffectId))
		{
			throw new Exception($"SpecialEffectSystem: nextEffectId {_nextEffectId} already exists.");
		}
		if (effect == null)
		{
			throw new Exception("SpecialEffectSystem: effect can not be null.");
		}
		effect.Id = _nextEffectId;
		AddElement_EffectDict(effect.Id, new SpecialEffectWrapper
		{
			Effect = effect
		}, context);
		do
		{
			_nextEffectId++;
			if (_nextEffectId < 0)
			{
				_nextEffectId = 0L;
			}
		}
		while (_effectDict.ContainsKey(_nextEffectId));
		SetNextEffectId(_nextEffectId, context);
		if (_domainInitialized)
		{
			effect.OnEnable(context);
			AddDataUid(context, effect);
			effect.OnDataAdded(context);
		}
		return effect.Id;
	}

	public long Add(DataContext context, int charId, string effectName)
	{
		string text = "GameData.Domains.SpecialEffect." + effectName;
		Type type = Type.GetType(text);
		if (type == null)
		{
			throw new Exception("Cannot find type '" + text + "'.");
		}
		SpecialEffectBase specialEffectBase = (SpecialEffectBase)Activator.CreateInstance(type, charId);
		Add(context, specialEffectBase);
		return specialEffectBase.Id;
	}

	public void Add(DataContext context, int charId, short skillTemplateId, sbyte effectActiveType, sbyte direction = -1)
	{
		CombatSkillKey combatSkillKey = new CombatSkillKey(charId, skillTemplateId);
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(combatSkillKey);
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		if (direction < 0)
		{
			direction = element_CombatSkills.GetDirection();
		}
		short num = (short)(direction switch
		{
			1 => combatSkillItem.ReverseEffectID, 
			0 => combatSkillItem.DirectEffectID, 
			_ => -1, 
		});
		if (num < 0)
		{
			return;
		}
		SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[num];
		if (specialEffectItem.EffectActiveType == effectActiveType && !string.IsNullOrEmpty(specialEffectItem.ClassName))
		{
			string text = "GameData.Domains.SpecialEffect." + specialEffectItem.ClassName;
			Type type = Type.GetType(text);
			if (type == null)
			{
				throw new Exception("Cannot find type '" + text + "'.");
			}
			SpecialEffectBase specialEffectBase = ((effectActiveType == 3) ? ((SpecialEffectBase)Activator.CreateInstance(type, combatSkillKey, direction)) : ((SpecialEffectBase)Activator.CreateInstance(type, combatSkillKey)));
			Add(context, specialEffectBase);
			if (effectActiveType == 3 || effectActiveType == 2 || effectActiveType == 1)
			{
				element_CombatSkills.SetSpecialEffectId(specialEffectBase.Id, context);
			}
		}
	}

	public long AddCombatStateEffect(DataContext context, int charId, sbyte stateType, short stateId, short power, bool reverse)
	{
		CombatStateEffect combatStateEffect = new CombatStateEffect(charId, stateType, stateId, power, reverse);
		Add(context, combatStateEffect);
		return combatStateEffect.Id;
	}

	public void AddEquipmentEffect(DataContext context, int charId, ItemKey equipKey)
	{
		short equipmentEffectId = DomainManager.Item.GetBaseEquipment(equipKey).GetEquipmentEffectId();
		if (equipmentEffectId >= 0)
		{
			AddEquipmentEffect(context, charId, equipKey, equipmentEffectId);
		}
	}

	public long AddEquipmentEffect(DataContext context, int charId, ItemKey equipKey, short effectId)
	{
		string effectClassName = Config.EquipmentEffect.Instance[effectId].EffectClassName;
		if (string.IsNullOrEmpty(effectClassName))
		{
			return -1L;
		}
		string text = "GameData.Domains.SpecialEffect." + effectClassName;
		Type type = Type.GetType(text);
		if (type == null)
		{
			throw new Exception("Cannot find type '" + text + "'.");
		}
		SpecialEffectBase effect = (SpecialEffectBase)Activator.CreateInstance(type, charId, equipKey);
		return Add(context, effect);
	}

	public void AddFeatureEffect(DataContext context, int charId, short featureId)
	{
		string associatedSpecialEffect = CharacterFeature.Instance[featureId].AssociatedSpecialEffect;
		if (!string.IsNullOrEmpty(associatedSpecialEffect))
		{
			long featureEffectKey = GetFeatureEffectKey(charId, featureId);
			string text = "GameData.Domains.SpecialEffect." + associatedSpecialEffect;
			Type type = Type.GetType(text);
			if (type == null)
			{
				throw new Exception("Cannot find type '" + text + "'.");
			}
			SpecialEffectBase specialEffectBase = (SpecialEffectBase)Activator.CreateInstance(type, charId, featureId);
			Add(context, specialEffectBase);
			_featureEffectDict.Add(featureEffectKey, specialEffectBase.Id);
		}
	}

	public long AddAddPenetrateAndPenetrateResistEffect(DataContext context, int charId, OuterAndInnerInts addPenetrate, OuterAndInnerInts addPenetrateResist)
	{
		SpecialEffectBase specialEffectBase = new AddPenetrateAndPenetrateResist(charId, addPenetrate, addPenetrateResist);
		Add(context, specialEffectBase);
		return specialEffectBase.Id;
	}

	public long AddAddMaxHealthEffect(DataContext context, int charId, int addMaxHealth)
	{
		SpecialEffectBase specialEffectBase = new AddMaxHealth(charId, addMaxHealth);
		Add(context, specialEffectBase);
		return specialEffectBase.Id;
	}

	public SpecialEffectBase Get(long effectId)
	{
		return _effectDict.ContainsKey(effectId) ? _effectDict[effectId].Effect : null;
	}

	public void Remove(DataContext context, long effectId)
	{
		if (_effectDict.ContainsKey(effectId))
		{
			SpecialEffectBase effect = _effectDict[effectId].Effect;
			RemoveDataUid(context, effect);
			effect.OnDisable(context);
			RemoveElement_EffectDict(effectId, context);
		}
	}

	public void Remove(DataContext context, int charId, short skillTemplateId, sbyte effectActiveType)
	{
		CombatSkillKey objectId = new CombatSkillKey(charId, skillTemplateId);
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(objectId);
		if (element_CombatSkills.GetSpecialEffectId() >= 0)
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
			short index = (short)((element_CombatSkills.GetDirection() == 0) ? combatSkillItem.DirectEffectID : combatSkillItem.ReverseEffectID);
			if (Config.SpecialEffect.Instance[index].EffectActiveType == effectActiveType && (effectActiveType == 3 || effectActiveType == 2))
			{
				Remove(context, element_CombatSkills.GetSpecialEffectId());
				element_CombatSkills.SetSpecialEffectId(-1L, context);
			}
		}
	}

	private void Remove(DataContext context, List<long> removeIdList, bool removeAffectedData = true, bool clearCharObj = false)
	{
		foreach (long removeId in removeIdList)
		{
			if (_effectDict.TryGetValue(removeId, out var value))
			{
				SpecialEffectBase effect = value.Effect;
				if (clearCharObj)
				{
					effect.CharObj = null;
				}
				effect.OnDisable(context);
				if (removeAffectedData)
				{
					RemoveDataUid(context, effect);
				}
				if (effect is CombatSkillEffectBase && DomainManager.CombatSkill.TryGetElement_CombatSkills(((CombatSkillEffectBase)effect).SkillKey, out var element))
				{
					element.SetSpecialEffectId(-1L, context);
				}
				RemoveElement_EffectDict(removeId, context);
			}
		}
	}

	public void RemoveAllEffectsInCombat(DataContext context)
	{
		List<long> list = ObjectPool<List<long>>.Instance.Get();
		list.Clear();
		foreach (KeyValuePair<long, SpecialEffectWrapper> item in _effectDict)
		{
			SpecialEffectBase effect = item.Value.Effect;
			if (effect is CombatStateEffect combatStateEffect)
			{
				combatStateEffect.OnDisable(context);
				RemoveDataUid(context, combatStateEffect);
				list.Add(item.Key);
			}
			else if (effect is PestleEffectBase pestleEffectBase)
			{
				pestleEffectBase.OnDisable(context);
				RemoveDataUid(context, pestleEffectBase);
				list.Add(item.Key);
			}
			else if (effect is EquipmentEffectBase { AutoRemoveAfterCombat: not false } equipmentEffectBase)
			{
				equipmentEffectBase.OnDisable(context);
				RemoveDataUid(context, equipmentEffectBase);
				list.Add(item.Key);
			}
			else
			{
				if (!(effect is CombatSkillEffectBase { IsLegendaryBookEffect: false } combatSkillEffectBase))
				{
					continue;
				}
				sbyte effectActiveType = Config.SpecialEffect.Instance[combatSkillEffectBase.EffectId].EffectActiveType;
				if (effectActiveType == 1 || effectActiveType == 0)
				{
					combatSkillEffectBase.OnDisable(context);
					RemoveDataUid(context, combatSkillEffectBase);
					list.Add(item.Key);
					if (effectActiveType == 1)
					{
						DomainManager.CombatSkill.GetElement_CombatSkills(combatSkillEffectBase.SkillKey).SetSpecialEffectId(-1L, context);
					}
				}
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			RemoveElement_EffectDict(list[i], context);
		}
		ObjectPool<List<long>>.Instance.Return(list);
	}

	public void RemoveFeatureEffect(DataContext context, int charId, short featureId)
	{
		long featureEffectKey = GetFeatureEffectKey(charId, featureId);
		if (_featureEffectDict.ContainsKey(featureEffectKey))
		{
			Remove(context, _featureEffectDict[featureEffectKey]);
			_featureEffectDict.Remove(featureEffectKey);
		}
	}

	public void RemoveAllEquippedSkillEffects(DataContext context, GameData.Domains.Character.Character character)
	{
		foreach (short item in character.GetCombatSkillEquipment())
		{
			DomainManager.SpecialEffect.Remove(context, character.GetId(), item, 2);
		}
	}

	public void RemoveAllBrokenSkillEffects(DataContext context, GameData.Domains.Character.Character character)
	{
		List<short> learnedCombatSkills = character.GetLearnedCombatSkills();
		foreach (short item in learnedCombatSkills)
		{
			if (item >= 0)
			{
				DomainManager.SpecialEffect.Remove(context, character.GetId(), item, 3);
			}
		}
	}

	public void AddAllBrokenSkillEffects(DataContext context, GameData.Domains.Character.Character character)
	{
		List<short> learnedCombatSkills = character.GetLearnedCombatSkills();
		foreach (short item in learnedCombatSkills)
		{
			if (item >= 0)
			{
				DomainManager.SpecialEffect.Add(context, character.GetId(), item, 3, -1);
			}
		}
	}

	private long GetFeatureEffectKey(int charId, short featureId)
	{
		return (long)charId * 1000000L + featureId;
	}

	private void AddDataUid(DataContext context, SpecialEffectBase effect)
	{
		if (effect.AffectDatas == null)
		{
			return;
		}
		foreach (AffectedDataKey key in effect.AffectDatas.Keys)
		{
			AppendDataUid(context, effect, key);
		}
	}

	public void AppendDataUid(DataContext context, SpecialEffectBase effect, AffectedDataKey dataKey)
	{
		if (!_affectedDatas.ContainsKey(dataKey.CharId))
		{
			AddElement_AffectedDatas(dataKey.CharId, new AffectedData(dataKey.CharId));
		}
		AffectedData affectedData = _affectedDatas[dataKey.CharId];
		SpecialEffectList effectList = affectedData.GetEffectList(dataKey.FieldId, createIfNull: true);
		effectList.EffectList.Add(effect);
		affectedData.SetEffectList(context, dataKey.FieldId, effectList);
	}

	public void RemoveDataUid(DataContext context, SpecialEffectBase effect, AffectedDataKey dataKey)
	{
		if (effect.AffectDatas != null && effect.AffectDatas.ContainsKey(dataKey))
		{
			AffectedData affectedData = _affectedDatas[dataKey.CharId];
			SpecialEffectList effectList = affectedData.GetEffectList(dataKey.FieldId);
			effectList?.EffectList.Remove(effect);
			affectedData.SetEffectList(context, dataKey.FieldId, effectList);
		}
	}

	public void RemoveDataUid(DataContext context, SpecialEffectBase effect)
	{
		if (effect.AffectDatas == null)
		{
			return;
		}
		foreach (AffectedDataKey key in effect.AffectDatas.Keys)
		{
			AffectedData affectedData = _affectedDatas[key.CharId];
			SpecialEffectList effectList = affectedData.GetEffectList(key.FieldId);
			effectList?.EffectList.Remove(effect);
			affectedData.SetEffectList(context, key.FieldId, effectList);
		}
	}

	public void InvalidateCache(DataContext context, int charId, ushort fieldId)
	{
		if (_affectedDatas.ContainsKey(charId))
		{
			AffectedData affectedData = _affectedDatas[charId];
			affectedData.SetEffectList(context, fieldId, affectedData.GetEffectList(fieldId));
			return;
		}
		AddElement_AffectedDatas(charId, new AffectedData(charId));
		AffectedData affectedData2 = _affectedDatas[charId];
		affectedData2.SetEffectList(context, fieldId, null);
		RemoveElement_AffectedDatas(charId);
	}

	public void ChangeAffectedDataUids(DataContext context, SpecialEffectBase effect, Dictionary<AffectedDataKey, EDataModifyType> affectDataUids)
	{
		RemoveDataUid(context, effect);
		effect.AffectDatas = affectDataUids;
		AddDataUid(context, effect);
	}

	public int ModifyValue(int charId, ushort fieldId, int value, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, int extraAdd = 0, int extraAddPercent = 0, int extraTotalPercentAdd = 0, int extraTotalPercentReduce = 0)
	{
		return ModifyValue(charId, -1, fieldId, value, customParam0, customParam1, customParam2, extraAdd, extraAddPercent, extraTotalPercentAdd, extraTotalPercentReduce);
	}

	public int ModifyValue(int charId, short skillId, ushort fieldId, int value, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, int extraAdd = 0, int extraAddPercent = 0, int extraTotalPercentAdd = 0, int extraTotalPercentReduce = 0)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		CValueModify modify = GetModify(charId, skillId, fieldId, customParam0, customParam1, customParam2, (EDataSumType)0);
		modify += new CValueModify(extraAdd, CValuePercentBonus.op_Implicit(extraAddPercent), CValuePercentBonus.op_Implicit(extraTotalPercentAdd), CValuePercentBonus.op_Implicit(extraTotalPercentReduce));
		return value * modify;
	}

	public int ModifyValueCustom(int charId, ushort fieldId, int value, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, int extraAdd = 0, int extraAddPercent = 0, int extraTotalPercentAdd = 0, int extraTotalPercentReduce = 0)
	{
		return ModifyValueCustom(charId, -1, fieldId, value, customParam0, customParam1, customParam2, extraAdd, extraAddPercent, extraTotalPercentAdd, extraTotalPercentReduce);
	}

	public int ModifyValueCustom(int charId, short skillId, ushort fieldId, int value, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, int extraAdd = 0, int extraAddPercent = 0, int extraTotalPercentAdd = 0, int extraTotalPercentReduce = 0)
	{
		value = ModifyValue(charId, skillId, fieldId, value, customParam0, customParam1, customParam2, extraAdd, extraAddPercent, extraTotalPercentAdd, extraTotalPercentReduce);
		return ModifyData(charId, skillId, fieldId, value, customParam0, customParam1, customParam2);
	}

	public CValueModify GetModify(int charId, ushort fieldId, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, EDataSumType valueSumType = (EDataSumType)0)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return GetModify(charId, -1, fieldId, customParam0, customParam1, customParam2, valueSumType);
	}

	public CValueModify GetModify(int charId, short combatSkillId, ushort fieldId, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, EDataSumType valueSumType = (EDataSumType)0)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		if ((int)valueSumType == 3)
		{
			return CValueModify.Zero;
		}
		int modifyValue = GetModifyValue(charId, combatSkillId, fieldId, (EDataModifyType)0, customParam0, customParam1, customParam2, valueSumType);
		int modifyValue2 = GetModifyValue(charId, combatSkillId, fieldId, (EDataModifyType)1, customParam0, customParam1, customParam2, valueSumType);
		(int, int) totalPercentModifyValue = GetTotalPercentModifyValue(charId, combatSkillId, fieldId, customParam0, customParam1, customParam2);
		int num;
		if ((int)valueSumType != 2)
		{
			(num, _) = totalPercentModifyValue;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		int num3 = (((int)valueSumType != 1) ? totalPercentModifyValue.Item2 : 0);
		return new CValueModify(modifyValue, CValuePercentBonus.op_Implicit(modifyValue2), CValuePercentBonus.op_Implicit(num2), CValuePercentBonus.op_Implicit(num3));
	}

	public int GetModifyValue(int charId, ushort fieldId, EDataModifyType modifyType, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, EDataSumType valueSumType = (EDataSumType)0)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return GetModifyValue(charId, -1, fieldId, modifyType, customParam0, customParam1, customParam2, valueSumType);
	}

	public int GetModifyValue(int charId, short combatSkillId, ushort fieldId, EDataModifyType modifyType, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1, EDataSumType valueSumType = (EDataSumType)0)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		if ((int)modifyType != 0 && (int)modifyType != 1)
		{
			throw new Exception($"Invalid DataModifyType {modifyType}");
		}
		if (!_affectedDatas.ContainsKey(charId))
		{
			return 0;
		}
		SpecialEffectList effectList = GetElement_AffectedDatas(charId).GetEffectList(fieldId);
		int num = 0;
		if (effectList != null)
		{
			AffectedDataKey affectedDataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			for (int i = 0; i < effectList.EffectList.Count; i++)
			{
				SpecialEffectBase specialEffectBase = effectList.EffectList[i];
				if (specialEffectBase.AffectDatas.TryGetValue(affectedDataKey, out var value) && value == modifyType)
				{
					int modifyValue = specialEffectBase.GetModifyValue(affectedDataKey, num);
					num = DataSumTypeHelper.Sum(valueSumType, num, modifyValue);
				}
			}
		}
		return num;
	}

	public (int add, int reduce) GetTotalPercentModifyValue(int charId, short combatSkillId, ushort fieldId, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		(int, int) result = (0, 0);
		if (!_affectedDatas.ContainsKey(charId))
		{
			return result;
		}
		SpecialEffectList effectList = GetElement_AffectedDatas(charId).GetEffectList(fieldId);
		if (effectList != null)
		{
			AffectedDataKey affectedDataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
			for (int i = 0; i < effectList.EffectList.Count; i++)
			{
				SpecialEffectBase specialEffectBase = effectList.EffectList[i];
				if (specialEffectBase.AffectDatas.TryGetValue(affectedDataKey, out var value) && (int)value == 2)
				{
					int modifyValue = specialEffectBase.GetModifyValue(affectedDataKey, 0);
					if (modifyValue > result.Item1)
					{
						result.Item1 = modifyValue;
					}
					else if (modifyValue < result.Item2)
					{
						result.Item2 = modifyValue;
					}
				}
			}
		}
		return result;
	}

	private void CalcCustomModifyEffectList(AffectedDataKey dataKey, List<SpecialEffectBase> customEffectList)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		customEffectList.Clear();
		if (!_affectedDatas.ContainsKey(dataKey.CharId))
		{
			return;
		}
		SpecialEffectList effectList = GetElement_AffectedDatas(dataKey.CharId).GetEffectList(dataKey.FieldId);
		if (effectList == null)
		{
			return;
		}
		for (int i = 0; i < effectList.EffectList.Count; i++)
		{
			SpecialEffectBase specialEffectBase = effectList.EffectList[i];
			if (specialEffectBase.AffectDatas.TryGetValue(dataKey, out var value) && (int)value == 3)
			{
				customEffectList.Add(specialEffectBase);
			}
		}
	}

	public bool ModifyData(int charId, short combatSkillId, ushort fieldId, bool dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public int ModifyData(int charId, short combatSkillId, ushort fieldId, int dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public long ModifyData(int charId, short combatSkillId, ushort fieldId, long dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public HitOrAvoidInts ModifyData(int charId, short combatSkillId, ushort fieldId, HitOrAvoidInts dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public NeiliProportionOfFiveElements ModifyData(int charId, short combatSkillId, ushort fieldId, NeiliProportionOfFiveElements dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public OuterAndInnerInts ModifyData(int charId, short combatSkillId, ushort fieldId, OuterAndInnerInts dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public List<NeedTrick> ModifyData(int charId, short combatSkillId, ushort fieldId, List<NeedTrick> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public (sbyte, sbyte) ModifyData(int charId, short combatSkillId, ushort fieldId, (sbyte, sbyte) dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public List<ItemKeyAndCount> ModifyData(int charId, short combatSkillId, ushort fieldId, List<ItemKeyAndCount> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public List<CastBoostEffectDisplayData> ModifyData(int charId, short combatSkillId, ushort fieldId, List<CastBoostEffectDisplayData> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public List<CombatSkillEffectData> ModifyData(int charId, short combatSkillId, ushort fieldId, List<CombatSkillEffectData> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public BoolArray8 ModifyData(int charId, short combatSkillId, ushort fieldId, BoolArray8 dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public CombatCharacter ModifyData(int charId, short combatSkillId, ushort fieldId, CombatCharacter dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		CalcCustomModifyEffectList(dataKey, list);
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public List<int> ModifyData(int charId, short combatSkillId, ushort fieldId, List<int> dataValue, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
	{
		AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, customParam0, customParam1, customParam2);
		List<SpecialEffectBase> list = ObjectPool<List<SpecialEffectBase>>.Instance.Get();
		for (int i = 0; i < list.Count; i++)
		{
			dataValue = list[i].GetModifiedValue(dataKey, dataValue);
		}
		ObjectPool<List<SpecialEffectBase>>.Instance.Return(list);
		return dataValue;
	}

	public void OnCharacterCreated(DataContext context, GameData.Domains.Character.Character character)
	{
		if (!_affectedDatas.ContainsKey(character.GetId()))
		{
			AddElement_AffectedDatas(character.GetId(), new AffectedData(character.GetId()));
		}
	}

	public void OnCharacterRemoved(DataContext context, GameData.Domains.Character.Character character)
	{
		List<long> list = ObjectPool<List<long>>.Instance.Get();
		list.Clear();
		foreach (KeyValuePair<long, SpecialEffectWrapper> item in _effectDict)
		{
			if (item.Value.Effect.CharacterId == character.GetId())
			{
				list.Add(item.Key);
			}
		}
		Remove(context, list, removeAffectedData: false);
		RemoveElement_AffectedDatas(character.GetId());
		ObjectPool<List<long>>.Instance.Return(list);
	}

	public void AddCombatSkillSpecialEffects(DataContext context, int charId, short[] combatSkills, sbyte activeType)
	{
		foreach (short num in combatSkills)
		{
			if (num >= 0)
			{
				DomainManager.SpecialEffect.Add(context, charId, num, activeType, -1);
			}
		}
	}

	public void UpdateEquippedSkillEffect(DataContext context, GameData.Domains.Character.Character character)
	{
		if (_updatingErrorEffect)
		{
			_requestUpdateCombatSkillCharIds.Add(character.GetId());
			return;
		}
		foreach (short learnedCombatSkill in character.GetLearnedCombatSkills())
		{
			Remove(context, character.GetId(), learnedCombatSkill, 2);
		}
		foreach (short item in character.GetCombatSkillEquipment())
		{
			if (character.GetCombatSkillCanAffect(item))
			{
				Add(context, character.GetId(), item, 2, -1);
			}
		}
	}

	private void OnAddWug(DataContext context, int charId, short wugTemplateId, short replacedWug)
	{
		long key = Add(context, charId, Config.Medicine.Instance[wugTemplateId].SpecialEffectClass);
		if (_effectDict.TryGetValue(key, out var value))
		{
			((WugEffectBase)value.Effect).OnEffectAdded(context, replacedWug);
		}
	}

	public void AddBrokenEffectChangedDuringAdvance(long effectId, int charId, short skillId)
	{
		_brokenEffectsChangedDuringAdvance.Add((effectId, charId, skillId));
	}

	public void ApplyBrokenEffectChangedDuringAdvance(DataContext context)
	{
		foreach (var item in _brokenEffectsChangedDuringAdvance)
		{
			GameData.Domains.CombatSkill.CombatSkill element;
			sbyte b = (sbyte)(DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(item.charId, item.skillId), out element) ? element.GetDirection() : (-1));
			if (item.effectId >= 0)
			{
				DomainManager.SpecialEffect.Remove(context, item.effectId);
			}
			if (b >= 0)
			{
				DomainManager.SpecialEffect.Add(context, item.charId, item.skillId, 3, b);
			}
		}
		_brokenEffectsChangedDuringAdvance.Clear();
	}

	public void SaveEffect(DataContext context, long effectId)
	{
		SetElement_EffectDict(effectId, _effectDict[effectId], context);
	}

	public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		List<SpecialEffectWrapper> list = (crossArchiveGameData.TaiwuEffects = new List<SpecialEffectWrapper>());
		foreach (SpecialEffectWrapper value in _effectDict.Values)
		{
			if (value.Effect != null && value.Effect.CharacterId == taiwuCharId)
			{
				list.Add(value);
			}
		}
	}

	public void UnpackCrossArchiveGameData_Items(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		List<SpecialEffectWrapper> taiwuEffects = crossArchiveGameData.TaiwuEffects;
		if (taiwuEffects == null)
		{
			return;
		}
		Dictionary<long, sbyte> dictionary = new Dictionary<long, sbyte>();
		Dictionary<long, sbyte> dictionary2 = new Dictionary<long, sbyte>();
		for (sbyte b = 0; b < 14; b++)
		{
			if (DomainManager.Extra.TryGetElement_LegendaryBookWeaponEffectId(b, out var value))
			{
				dictionary[value] = b;
			}
			if (DomainManager.Extra.TryGetElement_LegendaryBookSkillEffectId(b, out var value2))
			{
				List<long> items = value2.Items;
				if (items != null && items.Count > 0)
				{
					foreach (long item in value2.Items)
					{
						if (item >= 0)
						{
							dictionary2[item] = b;
						}
					}
				}
			}
		}
		List<SpecialEffectWrapper> list = new List<SpecialEffectWrapper>();
		foreach (SpecialEffectWrapper item2 in taiwuEffects)
		{
			SpecialEffectBase effect = item2.Effect;
			bool flag;
			if (effect is EquipmentEffectBase equipmentEffectBase)
			{
				equipmentEffectBase.EquipItemKey = DomainManager.Item.UnpackCrossArchiveItem(context, crossArchiveGameData, equipmentEffectBase.EquipItemKey);
				flag = !equipmentEffectBase.EquipItemKey.IsValid();
			}
			else
			{
				if (!(effect is CombatSkillEffectBase { IsLegendaryBookEffect: not false } combatSkillEffectBase))
				{
					continue;
				}
				combatSkillEffectBase.SkillKey.CharId = taiwuCharId;
				flag = !DomainManager.CombatSkill.TryGetElement_CombatSkills(combatSkillEffectBase.SkillKey, out var _);
			}
			effect.OnDreamBack(context);
			effect.CharObj = taiwu;
			effect.CharacterId = taiwuCharId;
			list.Add(item2);
			long id = effect.Id;
			long currEffectId = ((flag || IsErrorEffect(effect)) ? (-1) : Add(context, effect));
			if (dictionary.TryGetValue(id, out var value3))
			{
				DomainManager.Extra.SetLegendaryBookWeaponEffectId(context, value3, currEffectId, id);
			}
			if (dictionary2.TryGetValue(id, out var value4))
			{
				DomainManager.Extra.SetLegendaryBookSkillEffectId(context, value4, currEffectId, id);
			}
		}
		foreach (SpecialEffectWrapper item3 in list)
		{
			taiwuEffects.Remove(item3);
		}
	}

	public void UnpackCrossArchiveGameData_CombatSkills(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		List<SpecialEffectWrapper> taiwuEffects = crossArchiveGameData.TaiwuEffects;
		if (taiwuEffects == null)
		{
			return;
		}
		List<SpecialEffectWrapper> list = new List<SpecialEffectWrapper>();
		Dictionary<int, CombatSkillEffectBase> dictionary = new Dictionary<int, CombatSkillEffectBase>();
		foreach (SpecialEffectWrapper value2 in _effectDict.Values)
		{
			SpecialEffectBase effect = value2.Effect;
			if (effect.CharacterId == taiwuCharId && effect is CombatSkillEffectBase { IsLegendaryBookEffect: false } combatSkillEffectBase)
			{
				dictionary.Add(combatSkillEffectBase.Type, combatSkillEffectBase);
			}
		}
		foreach (SpecialEffectWrapper item in taiwuEffects)
		{
			SpecialEffectBase effect2 = item.Effect;
			if (!(effect2 is CombatSkillEffectBase { IsLegendaryBookEffect: false } combatSkillEffectBase2))
			{
				continue;
			}
			combatSkillEffectBase2.CharObj = taiwu;
			combatSkillEffectBase2.CharacterId = taiwuCharId;
			combatSkillEffectBase2.SkillKey.CharId = taiwuCharId;
			list.Add(item);
			if (!IsErrorEffect(combatSkillEffectBase2))
			{
				if (dictionary.TryGetValue(combatSkillEffectBase2.Type, out var value))
				{
					combatSkillEffectBase2.Id = value.Id;
					DomainManager.Extra.AddConflictSpecialEffect(context, item);
				}
				else
				{
					combatSkillEffectBase2.OnDreamBack(context);
					Add(context, combatSkillEffectBase2);
					combatSkillEffectBase2.SkillInstance.SetSpecialEffectId(combatSkillEffectBase2.Id, context);
				}
			}
		}
		foreach (SpecialEffectWrapper item2 in list)
		{
			taiwuEffects.Remove(item2);
		}
	}

	public void OverwriteSpecialEffectWithConflictCombatSkill(DataContext context, ConflictCombatSkill conflictCombatSkill)
	{
		if (!DomainManager.Extra.TryGetConflictCombatSkillEffect(conflictCombatSkill.TemplateId, out var effect))
		{
			return;
		}
		effect.CharObj = DomainManager.Taiwu.GetTaiwu();
		effect.CharacterId = DomainManager.Taiwu.GetTaiwuCharId();
		effect.SkillKey.CharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (IsErrorEffect(effect))
		{
			return;
		}
		if (!_effectDict.ContainsKey(effect.Id))
		{
			foreach (var (id, specialEffectWrapper2) in _effectDict)
			{
				if (specialEffectWrapper2.Effect.CharacterId == effect.CharacterId && specialEffectWrapper2.Effect.Type == effect.Type)
				{
					effect.Id = id;
				}
			}
		}
		Remove(context, effect.Id);
		effect.OnDreamBack(context);
		Add(context, effect);
		effect.SkillInstance.SetSpecialEffectId(effect.Id, context);
	}

	public SpecialEffectDomain()
		: base(3)
	{
		_effectDict = new Dictionary<long, SpecialEffectWrapper>(0);
		_nextEffectId = 0L;
		_affectedDatas = new Dictionary<int, AffectedData>(0);
		HelperDataAffectedDatas = new ObjectCollectionHelperData(17, 2, CacheInfluencesAffectedDatas, _dataStatesAffectedDatas, isArchive: false);
		OnInitializedDomainData();
	}

	private SpecialEffectWrapper GetElement_EffectDict(long elementId)
	{
		return _effectDict[elementId];
	}

	private bool TryGetElement_EffectDict(long elementId, out SpecialEffectWrapper value)
	{
		return _effectDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_EffectDict(long elementId, SpecialEffectWrapper value, DataContext context)
	{
		_effectDict.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(17, 0, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(17, 0, elementId, 0);
		}
	}

	private unsafe void SetElement_EffectDict(long elementId, SpecialEffectWrapper value, DataContext context)
	{
		_effectDict[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(17, 0, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(17, 0, elementId, 0);
		}
	}

	private void RemoveElement_EffectDict(long elementId, DataContext context)
	{
		_effectDict.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(17, 0, elementId);
	}

	private void ClearEffectDict(DataContext context)
	{
		_effectDict.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(17, 0);
	}

	private long GetNextEffectId()
	{
		return _nextEffectId;
	}

	private unsafe void SetNextEffectId(long value, DataContext context)
	{
		_nextEffectId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(17, 1, 8);
		*(long*)ptr = _nextEffectId;
		ptr += 8;
	}

	private AffectedData GetElement_AffectedDatas(int objectId)
	{
		return _affectedDatas[objectId];
	}

	private bool TryGetElement_AffectedDatas(int objectId, out AffectedData element)
	{
		return _affectedDatas.TryGetValue(objectId, out element);
	}

	private void AddElement_AffectedDatas(int objectId, AffectedData instance)
	{
		instance.CollectionHelperData = HelperDataAffectedDatas;
		instance.DataStatesOffset = _dataStatesAffectedDatas.Create();
		_affectedDatas.Add(objectId, instance);
	}

	private void RemoveElement_AffectedDatas(int objectId)
	{
		if (_affectedDatas.TryGetValue(objectId, out var value))
		{
			_dataStatesAffectedDatas.Remove(value.DataStatesOffset);
			_affectedDatas.Remove(objectId);
		}
	}

	private void ClearAffectedDatas()
	{
		_dataStatesAffectedDatas.Clear();
		_affectedDatas.Clear();
	}

	private int GetElementField_AffectedDatas(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_affectedDatas.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_AffectedDatas", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesAffectedDatas.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxStrength(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDexterity(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxConcentration(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxVitality(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxEnergy(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxIntelligence(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetRecoveryOfStance(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetRecoveryOfBreath(), dataPool);
		case 9:
			return GameData.Serializer.Serializer.Serialize(value.GetMoveSpeed(), dataPool);
		case 10:
			return GameData.Serializer.Serializer.Serialize(value.GetRecoveryOfFlaw(), dataPool);
		case 11:
			return GameData.Serializer.Serializer.Serialize(value.GetCastSpeed(), dataPool);
		case 12:
			return GameData.Serializer.Serializer.Serialize(value.GetRecoveryOfBlockedAcupoint(), dataPool);
		case 13:
			return GameData.Serializer.Serializer.Serialize(value.GetWeaponSwitchSpeed(), dataPool);
		case 14:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackSpeed(), dataPool);
		case 15:
			return GameData.Serializer.Serializer.Serialize(value.GetInnerRatio(), dataPool);
		case 16:
			return GameData.Serializer.Serializer.Serialize(value.GetRecoveryOfQiDisorder(), dataPool);
		case 17:
			return GameData.Serializer.Serializer.Serialize(value.GetMinorAttributeFixMaxValue(), dataPool);
		case 18:
			return GameData.Serializer.Serializer.Serialize(value.GetMinorAttributeFixMinValue(), dataPool);
		case 19:
			return GameData.Serializer.Serializer.Serialize(value.GetResistOfHotPoison(), dataPool);
		case 20:
			return GameData.Serializer.Serializer.Serialize(value.GetResistOfGloomyPoison(), dataPool);
		case 21:
			return GameData.Serializer.Serializer.Serialize(value.GetResistOfColdPoison(), dataPool);
		case 22:
			return GameData.Serializer.Serializer.Serialize(value.GetResistOfRedPoison(), dataPool);
		case 23:
			return GameData.Serializer.Serializer.Serialize(value.GetResistOfRottenPoison(), dataPool);
		case 24:
			return GameData.Serializer.Serializer.Serialize(value.GetResistOfIllusoryPoison(), dataPool);
		case 25:
			return GameData.Serializer.Serializer.Serialize(value.GetDisplayAge(), dataPool);
		case 26:
			return GameData.Serializer.Serializer.Serialize(value.GetNeiliProportionOfFiveElements(), dataPool);
		case 27:
			return GameData.Serializer.Serializer.Serialize(value.GetWeaponMaxPower(), dataPool);
		case 28:
			return GameData.Serializer.Serializer.Serialize(value.GetWeaponUseRequirement(), dataPool);
		case 29:
			return GameData.Serializer.Serializer.Serialize(value.GetWeaponAttackRange(), dataPool);
		case 30:
			return GameData.Serializer.Serializer.Serialize(value.GetArmorMaxPower(), dataPool);
		case 31:
			return GameData.Serializer.Serializer.Serialize(value.GetArmorUseRequirement(), dataPool);
		case 32:
			return GameData.Serializer.Serializer.Serialize(value.GetHitStrength(), dataPool);
		case 33:
			return GameData.Serializer.Serializer.Serialize(value.GetHitTechnique(), dataPool);
		case 34:
			return GameData.Serializer.Serializer.Serialize(value.GetHitSpeed(), dataPool);
		case 35:
			return GameData.Serializer.Serializer.Serialize(value.GetHitMind(), dataPool);
		case 36:
			return GameData.Serializer.Serializer.Serialize(value.GetHitCanChange(), dataPool);
		case 37:
			return GameData.Serializer.Serializer.Serialize(value.GetHitChangeEffectPercent(), dataPool);
		case 38:
			return GameData.Serializer.Serializer.Serialize(value.GetAvoidStrength(), dataPool);
		case 39:
			return GameData.Serializer.Serializer.Serialize(value.GetAvoidTechnique(), dataPool);
		case 40:
			return GameData.Serializer.Serializer.Serialize(value.GetAvoidSpeed(), dataPool);
		case 41:
			return GameData.Serializer.Serializer.Serialize(value.GetAvoidMind(), dataPool);
		case 42:
			return GameData.Serializer.Serializer.Serialize(value.GetAvoidCanChange(), dataPool);
		case 43:
			return GameData.Serializer.Serializer.Serialize(value.GetAvoidChangeEffectPercent(), dataPool);
		case 44:
			return GameData.Serializer.Serializer.Serialize(value.GetPenetrateOuter(), dataPool);
		case 45:
			return GameData.Serializer.Serializer.Serialize(value.GetPenetrateInner(), dataPool);
		case 46:
			return GameData.Serializer.Serializer.Serialize(value.GetPenetrateResistOuter(), dataPool);
		case 47:
			return GameData.Serializer.Serializer.Serialize(value.GetPenetrateResistInner(), dataPool);
		case 48:
			return GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationAttack(), dataPool);
		case 49:
			return GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationAgile(), dataPool);
		case 50:
			return GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationDefense(), dataPool);
		case 51:
			return GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationAssist(), dataPool);
		case 52:
			return GameData.Serializer.Serializer.Serialize(value.GetHappiness(), dataPool);
		case 53:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxHealth(), dataPool);
		case 54:
			return GameData.Serializer.Serializer.Serialize(value.GetHealthCost(), dataPool);
		case 55:
			return GameData.Serializer.Serializer.Serialize(value.GetMoveSpeedCanChange(), dataPool);
		case 56:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerHitStrength(), dataPool);
		case 57:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerHitTechnique(), dataPool);
		case 58:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerHitSpeed(), dataPool);
		case 59:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerHitMind(), dataPool);
		case 60:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerAvoidStrength(), dataPool);
		case 61:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerAvoidTechnique(), dataPool);
		case 62:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerAvoidSpeed(), dataPool);
		case 63:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerAvoidMind(), dataPool);
		case 64:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerPenetrateOuter(), dataPool);
		case 65:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerPenetrateInner(), dataPool);
		case 66:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerPenetrateResistOuter(), dataPool);
		case 67:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerPenetrateResistInner(), dataPool);
		case 68:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackHitType(), dataPool);
		case 69:
			return GameData.Serializer.Serializer.Serialize(value.GetMakeDirectDamage(), dataPool);
		case 70:
			return GameData.Serializer.Serializer.Serialize(value.GetMakeBounceDamage(), dataPool);
		case 71:
			return GameData.Serializer.Serializer.Serialize(value.GetMakeFightBackDamage(), dataPool);
		case 72:
			return GameData.Serializer.Serializer.Serialize(value.GetMakePoisonLevel(), dataPool);
		case 73:
			return GameData.Serializer.Serializer.Serialize(value.GetMakePoisonValue(), dataPool);
		case 74:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerHitOdds(), dataPool);
		case 75:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerFightBackHitOdds(), dataPool);
		case 76:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerPursueOdds(), dataPool);
		case 77:
			return GameData.Serializer.Serializer.Serialize(value.GetMakedInjuryChangeToOld(), dataPool);
		case 78:
			return GameData.Serializer.Serializer.Serialize(value.GetMakedPoisonChangeToOld(), dataPool);
		case 79:
			return GameData.Serializer.Serializer.Serialize(value.GetMakeDamageType(), dataPool);
		case 80:
			return GameData.Serializer.Serializer.Serialize(value.GetCanMakeInjuryToNoInjuryPart(), dataPool);
		case 81:
			return GameData.Serializer.Serializer.Serialize(value.GetMakePoisonType(), dataPool);
		case 82:
			return GameData.Serializer.Serializer.Serialize(value.GetNormalAttackWeapon(), dataPool);
		case 83:
			return GameData.Serializer.Serializer.Serialize(value.GetNormalAttackTrick(), dataPool);
		case 84:
			return GameData.Serializer.Serializer.Serialize(value.GetExtraFlawCount(), dataPool);
		case 85:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackCanBounce(), dataPool);
		case 86:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackCanFightBack(), dataPool);
		case 87:
			return GameData.Serializer.Serializer.Serialize(value.GetMakeFightBackInjuryMark(), dataPool);
		case 88:
			return GameData.Serializer.Serializer.Serialize(value.GetLegSkillUseShoes(), dataPool);
		case 89:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerFinalDamageValue(), dataPool);
		case 90:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderHitStrength(), dataPool);
		case 91:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderHitTechnique(), dataPool);
		case 92:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderHitSpeed(), dataPool);
		case 93:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderHitMind(), dataPool);
		case 94:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderAvoidStrength(), dataPool);
		case 95:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderAvoidTechnique(), dataPool);
		case 96:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderAvoidSpeed(), dataPool);
		case 97:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderAvoidMind(), dataPool);
		case 98:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderPenetrateOuter(), dataPool);
		case 99:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderPenetrateInner(), dataPool);
		case 100:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderPenetrateResistOuter(), dataPool);
		case 101:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderPenetrateResistInner(), dataPool);
		case 102:
			return GameData.Serializer.Serializer.Serialize(value.GetAcceptDirectDamage(), dataPool);
		case 103:
			return GameData.Serializer.Serializer.Serialize(value.GetAcceptBounceDamage(), dataPool);
		case 104:
			return GameData.Serializer.Serializer.Serialize(value.GetAcceptFightBackDamage(), dataPool);
		case 105:
			return GameData.Serializer.Serializer.Serialize(value.GetAcceptPoisonLevel(), dataPool);
		case 106:
			return GameData.Serializer.Serializer.Serialize(value.GetAcceptPoisonValue(), dataPool);
		case 107:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderHitOdds(), dataPool);
		case 108:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderFightBackHitOdds(), dataPool);
		case 109:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderPursueOdds(), dataPool);
		case 110:
			return GameData.Serializer.Serializer.Serialize(value.GetAcceptMaxInjuryCount(), dataPool);
		case 111:
			return GameData.Serializer.Serializer.Serialize(value.GetBouncePower(), dataPool);
		case 112:
			return GameData.Serializer.Serializer.Serialize(value.GetFightBackPower(), dataPool);
		case 113:
			return GameData.Serializer.Serializer.Serialize(value.GetDirectDamageInnerRatio(), dataPool);
		case 114:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderFinalDamageValue(), dataPool);
		case 115:
			return GameData.Serializer.Serializer.Serialize(value.GetDirectDamageValue(), dataPool);
		case 116:
			return GameData.Serializer.Serializer.Serialize(value.GetDirectInjuryMark(), dataPool);
		case 117:
			return GameData.Serializer.Serializer.Serialize(value.GetGoneMadInjury(), dataPool);
		case 118:
			return GameData.Serializer.Serializer.Serialize(value.GetHealInjurySpeed(), dataPool);
		case 119:
			return GameData.Serializer.Serializer.Serialize(value.GetHealInjuryBuff(), dataPool);
		case 120:
			return GameData.Serializer.Serializer.Serialize(value.GetHealInjuryDebuff(), dataPool);
		case 121:
			return GameData.Serializer.Serializer.Serialize(value.GetHealPoisonSpeed(), dataPool);
		case 122:
			return GameData.Serializer.Serializer.Serialize(value.GetHealPoisonBuff(), dataPool);
		case 123:
			return GameData.Serializer.Serializer.Serialize(value.GetHealPoisonDebuff(), dataPool);
		case 124:
			return GameData.Serializer.Serializer.Serialize(value.GetFleeSpeed(), dataPool);
		case 125:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxFlawCount(), dataPool);
		case 126:
			return GameData.Serializer.Serializer.Serialize(value.GetCanAddFlaw(), dataPool);
		case 127:
			return GameData.Serializer.Serializer.Serialize(value.GetFlawLevel(), dataPool);
		case 128:
			return GameData.Serializer.Serializer.Serialize(value.GetFlawLevelCanReduce(), dataPool);
		case 129:
			return GameData.Serializer.Serializer.Serialize(value.GetFlawCount(), dataPool);
		case 130:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxAcupointCount(), dataPool);
		case 131:
			return GameData.Serializer.Serializer.Serialize(value.GetCanAddAcupoint(), dataPool);
		case 132:
			return GameData.Serializer.Serializer.Serialize(value.GetAcupointLevel(), dataPool);
		case 133:
			return GameData.Serializer.Serializer.Serialize(value.GetAcupointLevelCanReduce(), dataPool);
		case 134:
			return GameData.Serializer.Serializer.Serialize(value.GetAcupointCount(), dataPool);
		case 135:
			return GameData.Serializer.Serializer.Serialize(value.GetAddNeiliAllocation(), dataPool);
		case 136:
			return GameData.Serializer.Serializer.Serialize(value.GetCostNeiliAllocation(), dataPool);
		case 137:
			return GameData.Serializer.Serializer.Serialize(value.GetCanChangeNeiliAllocation(), dataPool);
		case 138:
			return GameData.Serializer.Serializer.Serialize(value.GetCanGetTrick(), dataPool);
		case 139:
			return GameData.Serializer.Serializer.Serialize(value.GetGetTrickType(), dataPool);
		case 140:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackBodyPart(), dataPool);
		case 141:
			return GameData.Serializer.Serializer.Serialize(value.GetWeaponEquipAttack(), dataPool);
		case 142:
			return GameData.Serializer.Serializer.Serialize(value.GetWeaponEquipDefense(), dataPool);
		case 143:
			return GameData.Serializer.Serializer.Serialize(value.GetArmorEquipAttack(), dataPool);
		case 144:
			return GameData.Serializer.Serializer.Serialize(value.GetArmorEquipDefense(), dataPool);
		case 145:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackRangeForward(), dataPool);
		case 146:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackRangeBackward(), dataPool);
		case 147:
			return GameData.Serializer.Serializer.Serialize(value.GetMoveCanBeStopped(), dataPool);
		case 148:
			return GameData.Serializer.Serializer.Serialize(value.GetCanForcedMove(), dataPool);
		case 149:
			return GameData.Serializer.Serializer.Serialize(value.GetMobilityCanBeRemoved(), dataPool);
		case 150:
			return GameData.Serializer.Serializer.Serialize(value.GetMobilityCostByEffect(), dataPool);
		case 151:
			return GameData.Serializer.Serializer.Serialize(value.GetMoveDistance(), dataPool);
		case 152:
			return GameData.Serializer.Serializer.Serialize(value.GetJumpPrepareFrame(), dataPool);
		case 153:
			return GameData.Serializer.Serializer.Serialize(value.GetBounceInjuryMark(), dataPool);
		case 154:
			return GameData.Serializer.Serializer.Serialize(value.GetSkillHasCost(), dataPool);
		case 155:
			return GameData.Serializer.Serializer.Serialize(value.GetCombatStateEffect(), dataPool);
		case 156:
			return GameData.Serializer.Serializer.Serialize(value.GetChangeNeedUseSkill(), dataPool);
		case 157:
			return GameData.Serializer.Serializer.Serialize(value.GetChangeDistanceIsMove(), dataPool);
		case 158:
			return GameData.Serializer.Serializer.Serialize(value.GetReplaceCharHit(), dataPool);
		case 159:
			return GameData.Serializer.Serializer.Serialize(value.GetCanAddPoison(), dataPool);
		case 160:
			return GameData.Serializer.Serializer.Serialize(value.GetCanReducePoison(), dataPool);
		case 161:
			return GameData.Serializer.Serializer.Serialize(value.GetReducePoisonValue(), dataPool);
		case 162:
			return GameData.Serializer.Serializer.Serialize(value.GetPoisonCanAffect(), dataPool);
		case 163:
			return GameData.Serializer.Serializer.Serialize(value.GetPoisonAffectCount(), dataPool);
		case 164:
			return GameData.Serializer.Serializer.Serialize(value.GetCostTricks(), dataPool);
		case 165:
			return GameData.Serializer.Serializer.Serialize(value.GetJumpMoveDistance(), dataPool);
		case 166:
			return GameData.Serializer.Serializer.Serialize(value.GetCombatStateToAdd(), dataPool);
		case 167:
			return GameData.Serializer.Serializer.Serialize(value.GetCombatStatePower(), dataPool);
		case 168:
			return GameData.Serializer.Serializer.Serialize(value.GetBreakBodyPartInjuryCount(), dataPool);
		case 169:
			return GameData.Serializer.Serializer.Serialize(value.GetBodyPartIsBroken(), dataPool);
		case 170:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxTrickCount(), dataPool);
		case 171:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxBreathPercent(), dataPool);
		case 172:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxStancePercent(), dataPool);
		case 173:
			return GameData.Serializer.Serializer.Serialize(value.GetExtraBreathPercent(), dataPool);
		case 174:
			return GameData.Serializer.Serializer.Serialize(value.GetExtraStancePercent(), dataPool);
		case 175:
			return GameData.Serializer.Serializer.Serialize(value.GetMoveCostMobility(), dataPool);
		case 176:
			return GameData.Serializer.Serializer.Serialize(value.GetDefendSkillKeepTime(), dataPool);
		case 177:
			return GameData.Serializer.Serializer.Serialize(value.GetBounceRange(), dataPool);
		case 178:
			return GameData.Serializer.Serializer.Serialize(value.GetMindMarkKeepTime(), dataPool);
		case 179:
			return GameData.Serializer.Serializer.Serialize(value.GetSkillMobilityCostPerFrame(), dataPool);
		case 180:
			return GameData.Serializer.Serializer.Serialize(value.GetCanAddWug(), dataPool);
		case 181:
			return GameData.Serializer.Serializer.Serialize(value.GetHasGodWeaponBuff(), dataPool);
		case 182:
			return GameData.Serializer.Serializer.Serialize(value.GetHasGodArmorBuff(), dataPool);
		case 183:
			return GameData.Serializer.Serializer.Serialize(value.GetTeammateCmdRequireGenerateValue(), dataPool);
		case 184:
			return GameData.Serializer.Serializer.Serialize(value.GetTeammateCmdEffect(), dataPool);
		case 185:
			return GameData.Serializer.Serializer.Serialize(value.GetFlawRecoverSpeed(), dataPool);
		case 186:
			return GameData.Serializer.Serializer.Serialize(value.GetAcupointRecoverSpeed(), dataPool);
		case 187:
			return GameData.Serializer.Serializer.Serialize(value.GetMindMarkRecoverSpeed(), dataPool);
		case 188:
			return GameData.Serializer.Serializer.Serialize(value.GetInjuryAutoHealSpeed(), dataPool);
		case 189:
			return GameData.Serializer.Serializer.Serialize(value.GetCanRecoverBreath(), dataPool);
		case 190:
			return GameData.Serializer.Serializer.Serialize(value.GetCanRecoverStance(), dataPool);
		case 191:
			return GameData.Serializer.Serializer.Serialize(value.GetFatalDamageValue(), dataPool);
		case 192:
			return GameData.Serializer.Serializer.Serialize(value.GetFatalDamageMarkCount(), dataPool);
		case 193:
			return GameData.Serializer.Serializer.Serialize(value.GetCanFightBackDuringPrepareSkill(), dataPool);
		case 194:
			return GameData.Serializer.Serializer.Serialize(value.GetSkillPrepareSpeed(), dataPool);
		case 195:
			return GameData.Serializer.Serializer.Serialize(value.GetBreathRecoverSpeed(), dataPool);
		case 196:
			return GameData.Serializer.Serializer.Serialize(value.GetStanceRecoverSpeed(), dataPool);
		case 197:
			return GameData.Serializer.Serializer.Serialize(value.GetMobilityRecoverSpeed(), dataPool);
		case 198:
			return GameData.Serializer.Serializer.Serialize(value.GetChangeTrickProgressAddValue(), dataPool);
		case 199:
			return GameData.Serializer.Serializer.Serialize(value.GetPower(), dataPool);
		case 200:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxPower(), dataPool);
		case 201:
			return GameData.Serializer.Serializer.Serialize(value.GetPowerCanReduce(), dataPool);
		case 202:
			return GameData.Serializer.Serializer.Serialize(value.GetUseRequirement(), dataPool);
		case 203:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrInnerRatio(), dataPool);
		case 204:
			return GameData.Serializer.Serializer.Serialize(value.GetCostBreathAndStance(), dataPool);
		case 205:
			return GameData.Serializer.Serializer.Serialize(value.GetCostBreath(), dataPool);
		case 206:
			return GameData.Serializer.Serializer.Serialize(value.GetCostStance(), dataPool);
		case 207:
			return GameData.Serializer.Serializer.Serialize(value.GetCostMobility(), dataPool);
		case 208:
			return GameData.Serializer.Serializer.Serialize(value.GetSkillCostTricks(), dataPool);
		case 209:
			return GameData.Serializer.Serializer.Serialize(value.GetEffectDirection(), dataPool);
		case 210:
			return GameData.Serializer.Serializer.Serialize(value.GetEffectDirectionCanChange(), dataPool);
		case 211:
			return GameData.Serializer.Serializer.Serialize(value.GetGridCost(), dataPool);
		case 212:
			return GameData.Serializer.Serializer.Serialize(value.GetPrepareTotalProgress(), dataPool);
		case 213:
			return GameData.Serializer.Serializer.Serialize(value.GetSpecificGridCount(), dataPool);
		case 214:
			return GameData.Serializer.Serializer.Serialize(value.GetGenericGridCount(), dataPool);
		case 215:
			return GameData.Serializer.Serializer.Serialize(value.GetCanInterrupt(), dataPool);
		case 216:
			return GameData.Serializer.Serializer.Serialize(value.GetInterruptOdds(), dataPool);
		case 217:
			return GameData.Serializer.Serializer.Serialize(value.GetCanSilence(), dataPool);
		case 218:
			return GameData.Serializer.Serializer.Serialize(value.GetSilenceOdds(), dataPool);
		case 219:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCastWithBrokenBodyPart(), dataPool);
		case 220:
			return GameData.Serializer.Serializer.Serialize(value.GetAddPowerCanBeRemoved(), dataPool);
		case 221:
			return GameData.Serializer.Serializer.Serialize(value.GetSkillType(), dataPool);
		case 222:
			return GameData.Serializer.Serializer.Serialize(value.GetEffectCountCanChange(), dataPool);
		case 223:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCastInDefend(), dataPool);
		case 224:
			return GameData.Serializer.Serializer.Serialize(value.GetHitDistribution(), dataPool);
		case 225:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCastOnLackBreath(), dataPool);
		case 226:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCastOnLackStance(), dataPool);
		case 227:
			return GameData.Serializer.Serializer.Serialize(value.GetCostBreathOnCast(), dataPool);
		case 228:
			return GameData.Serializer.Serializer.Serialize(value.GetCostStanceOnCast(), dataPool);
		case 229:
			return GameData.Serializer.Serializer.Serialize(value.GetCanUseMobilityAsBreath(), dataPool);
		case 230:
			return GameData.Serializer.Serializer.Serialize(value.GetCanUseMobilityAsStance(), dataPool);
		case 231:
			return GameData.Serializer.Serializer.Serialize(value.GetCastCostNeiliAllocation(), dataPool);
		case 232:
			return GameData.Serializer.Serializer.Serialize(value.GetAcceptPoisonResist(), dataPool);
		case 233:
			return GameData.Serializer.Serializer.Serialize(value.GetMakePoisonResist(), dataPool);
		case 234:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCriticalHit(), dataPool);
		case 235:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCostNeiliAllocationEffect(), dataPool);
		case 236:
			return GameData.Serializer.Serializer.Serialize(value.GetConsummateLevelRelatedMainAttributesHitValues(), dataPool);
		case 237:
			return GameData.Serializer.Serializer.Serialize(value.GetConsummateLevelRelatedMainAttributesAvoidValues(), dataPool);
		case 238:
			return GameData.Serializer.Serializer.Serialize(value.GetConsummateLevelRelatedMainAttributesPenetrations(), dataPool);
		case 239:
			return GameData.Serializer.Serializer.Serialize(value.GetConsummateLevelRelatedMainAttributesPenetrationResists(), dataPool);
		case 240:
			return GameData.Serializer.Serializer.Serialize(value.GetSkillAlsoAsFiveElements(), dataPool);
		case 241:
			return GameData.Serializer.Serializer.Serialize(value.GetInnerInjuryImmunity(), dataPool);
		case 242:
			return GameData.Serializer.Serializer.Serialize(value.GetOuterInjuryImmunity(), dataPool);
		case 243:
			return GameData.Serializer.Serializer.Serialize(value.GetPoisonAffectThreshold(), dataPool);
		case 244:
			return GameData.Serializer.Serializer.Serialize(value.GetLockDistance(), dataPool);
		case 245:
			return GameData.Serializer.Serializer.Serialize(value.GetResistOfAllPoison(), dataPool);
		case 246:
			return GameData.Serializer.Serializer.Serialize(value.GetMakePoisonTarget(), dataPool);
		case 247:
			return GameData.Serializer.Serializer.Serialize(value.GetAcceptPoisonTarget(), dataPool);
		case 248:
			return GameData.Serializer.Serializer.Serialize(value.GetCertainCriticalHit(), dataPool);
		case 249:
			return GameData.Serializer.Serializer.Serialize(value.GetMindMarkCount(), dataPool);
		case 250:
			return GameData.Serializer.Serializer.Serialize(value.GetCanFightBackWithHit(), dataPool);
		case 251:
			return GameData.Serializer.Serializer.Serialize(value.GetInevitableHit(), dataPool);
		case 252:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackCanPursue(), dataPool);
		case 253:
			return GameData.Serializer.Serializer.Serialize(value.GetCombatSkillDataEffectList(), dataPool);
		case 254:
			return GameData.Serializer.Serializer.Serialize(value.GetCriticalOdds(), dataPool);
		case 255:
			return GameData.Serializer.Serializer.Serialize(value.GetStanceCostByEffect(), dataPool);
		case 256:
			return GameData.Serializer.Serializer.Serialize(value.GetBreathCostByEffect(), dataPool);
		case 257:
			return GameData.Serializer.Serializer.Serialize(value.GetPowerAddRatio(), dataPool);
		case 258:
			return GameData.Serializer.Serializer.Serialize(value.GetPowerReduceRatio(), dataPool);
		case 259:
			return GameData.Serializer.Serializer.Serialize(value.GetPoisonAffectProduceValue(), dataPool);
		case 260:
			return GameData.Serializer.Serializer.Serialize(value.GetCanReadingOnMonthChange(), dataPool);
		case 261:
			return GameData.Serializer.Serializer.Serialize(value.GetMedicineEffect(), dataPool);
		case 262:
			return GameData.Serializer.Serializer.Serialize(value.GetXiangshuInfectionDelta(), dataPool);
		case 263:
			return GameData.Serializer.Serializer.Serialize(value.GetHealthDelta(), dataPool);
		case 264:
			return GameData.Serializer.Serializer.Serialize(value.GetWeaponSilenceFrame(), dataPool);
		case 265:
			return GameData.Serializer.Serializer.Serialize(value.GetSilenceFrame(), dataPool);
		case 266:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrAgeDelta(), dataPool);
		case 267:
			return GameData.Serializer.Serializer.Serialize(value.GetGoneMadInAllBreak(), dataPool);
		case 268:
			return GameData.Serializer.Serializer.Serialize(value.GetMakeLoveRateOnMonthChange(), dataPool);
		case 269:
			return GameData.Serializer.Serializer.Serialize(value.GetCanAutoHealOnMonthChange(), dataPool);
		case 270:
			return GameData.Serializer.Serializer.Serialize(value.GetHappinessDelta(), dataPool);
		case 271:
			return GameData.Serializer.Serializer.Serialize(value.GetTeammateCmdCanUse(), dataPool);
		case 272:
			return GameData.Serializer.Serializer.Serialize(value.GetMixPoisonInfinityAffect(), dataPool);
		case 273:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackRangeMaxAcupoint(), dataPool);
		case 274:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxMobilityPercent(), dataPool);
		case 275:
			return GameData.Serializer.Serializer.Serialize(value.GetMakeMindDamage(), dataPool);
		case 276:
			return GameData.Serializer.Serializer.Serialize(value.GetAcceptMindDamage(), dataPool);
		case 277:
			return GameData.Serializer.Serializer.Serialize(value.GetHitAddByTempValue(), dataPool);
		case 278:
			return GameData.Serializer.Serializer.Serialize(value.GetAvoidAddByTempValue(), dataPool);
		case 279:
			return GameData.Serializer.Serializer.Serialize(value.GetIgnoreEquipmentOverload(), dataPool);
		case 280:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCostEnemyUsableTricks(), dataPool);
		case 281:
			return GameData.Serializer.Serializer.Serialize(value.GetIgnoreArmor(), dataPool);
		case 282:
			return GameData.Serializer.Serializer.Serialize(value.GetUnyieldingFallen(), dataPool);
		case 283:
			return GameData.Serializer.Serializer.Serialize(value.GetNormalAttackPrepareFrame(), dataPool);
		case 284:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCostUselessTricks(), dataPool);
		case 285:
			return GameData.Serializer.Serializer.Serialize(value.GetDefendSkillCanAffect(), dataPool);
		case 286:
			return GameData.Serializer.Serializer.Serialize(value.GetAssistSkillCanAffect(), dataPool);
		case 287:
			return GameData.Serializer.Serializer.Serialize(value.GetAgileSkillCanAffect(), dataPool);
		case 288:
			return GameData.Serializer.Serializer.Serialize(value.GetAllMarkChangeToMind(), dataPool);
		case 289:
			return GameData.Serializer.Serializer.Serialize(value.GetMindMarkChangeToFatal(), dataPool);
		case 290:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCast(), dataPool);
		case 291:
			return GameData.Serializer.Serializer.Serialize(value.GetInevitableAvoid(), dataPool);
		case 292:
			return GameData.Serializer.Serializer.Serialize(value.GetPowerEffectReverse(), dataPool);
		case 293:
			return GameData.Serializer.Serializer.Serialize(value.GetFeatureBonusReverse(), dataPool);
		case 294:
			return GameData.Serializer.Serializer.Serialize(value.GetWugFatalDamageValue(), dataPool);
		case 295:
			return GameData.Serializer.Serializer.Serialize(value.GetCanRecoverHealthOnMonthChange(), dataPool);
		case 296:
			return GameData.Serializer.Serializer.Serialize(value.GetTakeRevengeRateOnMonthChange(), dataPool);
		case 297:
			return GameData.Serializer.Serializer.Serialize(value.GetConsummateLevelBonus(), dataPool);
		case 298:
			return GameData.Serializer.Serializer.Serialize(value.GetNeiliDelta(), dataPool);
		case 299:
			return GameData.Serializer.Serializer.Serialize(value.GetCanMakeLoveSpecialOnMonthChange(), dataPool);
		case 300:
			return GameData.Serializer.Serializer.Serialize(value.GetHealAcupointSpeed(), dataPool);
		case 301:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxChangeTrickCount(), dataPool);
		case 302:
			return GameData.Serializer.Serializer.Serialize(value.GetConvertCostBreathAndStance(), dataPool);
		case 303:
			return GameData.Serializer.Serializer.Serialize(value.GetPersonalitiesAll(), dataPool);
		case 304:
			return GameData.Serializer.Serializer.Serialize(value.GetFinalFatalDamageMarkCount(), dataPool);
		case 305:
			return GameData.Serializer.Serializer.Serialize(value.GetInfinityMindMarkProgress(), dataPool);
		case 306:
			return GameData.Serializer.Serializer.Serialize(value.GetCombatSkillAiScorePower(), dataPool);
		case 307:
			return GameData.Serializer.Serializer.Serialize(value.GetNormalAttackChangeToUnlockAttack(), dataPool);
		case 308:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackBodyPartOdds(), dataPool);
		case 309:
			return GameData.Serializer.Serializer.Serialize(value.GetChangeDurability(), dataPool);
		case 310:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentBonus(), dataPool);
		case 311:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentWeight(), dataPool);
		case 312:
			return GameData.Serializer.Serializer.Serialize(value.GetRawCreateEffectList(), dataPool);
		case 313:
			return GameData.Serializer.Serializer.Serialize(value.GetJiTrickAsWeaponTrickCount(), dataPool);
		case 314:
			return GameData.Serializer.Serializer.Serialize(value.GetUselessTrickAsJiTrickCount(), dataPool);
		case 315:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentPower(), dataPool);
		case 316:
			return GameData.Serializer.Serializer.Serialize(value.GetHealFlawSpeed(), dataPool);
		case 317:
			return GameData.Serializer.Serializer.Serialize(value.GetUnlockSpeed(), dataPool);
		case 318:
			return GameData.Serializer.Serializer.Serialize(value.GetFlawBonusFactor(), dataPool);
		case 319:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCostShaTricks(), dataPool);
		case 320:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenderDirectFinalDamageValue(), dataPool);
		case 321:
			return GameData.Serializer.Serializer.Serialize(value.GetNormalAttackRecoveryFrame(), dataPool);
		case 322:
			return GameData.Serializer.Serializer.Serialize(value.GetFinalGoneMadInjury(), dataPool);
		case 323:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackerDirectFinalDamageValue(), dataPool);
		case 324:
			return GameData.Serializer.Serializer.Serialize(value.GetCanCostTrickDuringPreparingSkill(), dataPool);
		case 325:
			return GameData.Serializer.Serializer.Serialize(value.GetValidItemList(), dataPool);
		case 326:
			return GameData.Serializer.Serializer.Serialize(value.GetAcceptDamageCanAdd(), dataPool);
		case 327:
			return GameData.Serializer.Serializer.Serialize(value.GetMakeDamageCanReduce(), dataPool);
		case 328:
			return GameData.Serializer.Serializer.Serialize(value.GetNormalAttackGetTrickCount(), dataPool);
		default:
			if (fieldId >= 329)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	private void SetElementField_AffectedDatas(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_affectedDatas.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
		{
			int item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetId(item4, context);
			return;
		}
		case 1:
		{
			SpecialEffectList item3 = value.GetMaxStrength();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetMaxStrength(item3, context);
			return;
		}
		case 2:
		{
			SpecialEffectList item2 = value.GetMaxDexterity();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDexterity(item2, context);
			return;
		}
		case 3:
		{
			SpecialEffectList item = value.GetMaxConcentration();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetMaxConcentration(item, context);
			return;
		}
		case 4:
		{
			SpecialEffectList item329 = value.GetMaxVitality();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item329);
			value.SetMaxVitality(item329, context);
			return;
		}
		case 5:
		{
			SpecialEffectList item328 = value.GetMaxEnergy();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item328);
			value.SetMaxEnergy(item328, context);
			return;
		}
		case 6:
		{
			SpecialEffectList item327 = value.GetMaxIntelligence();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item327);
			value.SetMaxIntelligence(item327, context);
			return;
		}
		case 7:
		{
			SpecialEffectList item326 = value.GetRecoveryOfStance();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item326);
			value.SetRecoveryOfStance(item326, context);
			return;
		}
		case 8:
		{
			SpecialEffectList item325 = value.GetRecoveryOfBreath();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item325);
			value.SetRecoveryOfBreath(item325, context);
			return;
		}
		case 9:
		{
			SpecialEffectList item324 = value.GetMoveSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item324);
			value.SetMoveSpeed(item324, context);
			return;
		}
		case 10:
		{
			SpecialEffectList item323 = value.GetRecoveryOfFlaw();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item323);
			value.SetRecoveryOfFlaw(item323, context);
			return;
		}
		case 11:
		{
			SpecialEffectList item322 = value.GetCastSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item322);
			value.SetCastSpeed(item322, context);
			return;
		}
		case 12:
		{
			SpecialEffectList item321 = value.GetRecoveryOfBlockedAcupoint();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item321);
			value.SetRecoveryOfBlockedAcupoint(item321, context);
			return;
		}
		case 13:
		{
			SpecialEffectList item320 = value.GetWeaponSwitchSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item320);
			value.SetWeaponSwitchSpeed(item320, context);
			return;
		}
		case 14:
		{
			SpecialEffectList item319 = value.GetAttackSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item319);
			value.SetAttackSpeed(item319, context);
			return;
		}
		case 15:
		{
			SpecialEffectList item318 = value.GetInnerRatio();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item318);
			value.SetInnerRatio(item318, context);
			return;
		}
		case 16:
		{
			SpecialEffectList item317 = value.GetRecoveryOfQiDisorder();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item317);
			value.SetRecoveryOfQiDisorder(item317, context);
			return;
		}
		case 17:
		{
			SpecialEffectList item316 = value.GetMinorAttributeFixMaxValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item316);
			value.SetMinorAttributeFixMaxValue(item316, context);
			return;
		}
		case 18:
		{
			SpecialEffectList item315 = value.GetMinorAttributeFixMinValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item315);
			value.SetMinorAttributeFixMinValue(item315, context);
			return;
		}
		case 19:
		{
			SpecialEffectList item314 = value.GetResistOfHotPoison();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item314);
			value.SetResistOfHotPoison(item314, context);
			return;
		}
		case 20:
		{
			SpecialEffectList item313 = value.GetResistOfGloomyPoison();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item313);
			value.SetResistOfGloomyPoison(item313, context);
			return;
		}
		case 21:
		{
			SpecialEffectList item312 = value.GetResistOfColdPoison();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item312);
			value.SetResistOfColdPoison(item312, context);
			return;
		}
		case 22:
		{
			SpecialEffectList item311 = value.GetResistOfRedPoison();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item311);
			value.SetResistOfRedPoison(item311, context);
			return;
		}
		case 23:
		{
			SpecialEffectList item310 = value.GetResistOfRottenPoison();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item310);
			value.SetResistOfRottenPoison(item310, context);
			return;
		}
		case 24:
		{
			SpecialEffectList item309 = value.GetResistOfIllusoryPoison();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item309);
			value.SetResistOfIllusoryPoison(item309, context);
			return;
		}
		case 25:
		{
			SpecialEffectList item308 = value.GetDisplayAge();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item308);
			value.SetDisplayAge(item308, context);
			return;
		}
		case 26:
		{
			SpecialEffectList item307 = value.GetNeiliProportionOfFiveElements();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item307);
			value.SetNeiliProportionOfFiveElements(item307, context);
			return;
		}
		case 27:
		{
			SpecialEffectList item306 = value.GetWeaponMaxPower();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item306);
			value.SetWeaponMaxPower(item306, context);
			return;
		}
		case 28:
		{
			SpecialEffectList item305 = value.GetWeaponUseRequirement();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item305);
			value.SetWeaponUseRequirement(item305, context);
			return;
		}
		case 29:
		{
			SpecialEffectList item304 = value.GetWeaponAttackRange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item304);
			value.SetWeaponAttackRange(item304, context);
			return;
		}
		case 30:
		{
			SpecialEffectList item303 = value.GetArmorMaxPower();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item303);
			value.SetArmorMaxPower(item303, context);
			return;
		}
		case 31:
		{
			SpecialEffectList item302 = value.GetArmorUseRequirement();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item302);
			value.SetArmorUseRequirement(item302, context);
			return;
		}
		case 32:
		{
			SpecialEffectList item301 = value.GetHitStrength();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item301);
			value.SetHitStrength(item301, context);
			return;
		}
		case 33:
		{
			SpecialEffectList item300 = value.GetHitTechnique();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item300);
			value.SetHitTechnique(item300, context);
			return;
		}
		case 34:
		{
			SpecialEffectList item299 = value.GetHitSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item299);
			value.SetHitSpeed(item299, context);
			return;
		}
		case 35:
		{
			SpecialEffectList item298 = value.GetHitMind();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item298);
			value.SetHitMind(item298, context);
			return;
		}
		case 36:
		{
			SpecialEffectList item297 = value.GetHitCanChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item297);
			value.SetHitCanChange(item297, context);
			return;
		}
		case 37:
		{
			SpecialEffectList item296 = value.GetHitChangeEffectPercent();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item296);
			value.SetHitChangeEffectPercent(item296, context);
			return;
		}
		case 38:
		{
			SpecialEffectList item295 = value.GetAvoidStrength();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item295);
			value.SetAvoidStrength(item295, context);
			return;
		}
		case 39:
		{
			SpecialEffectList item294 = value.GetAvoidTechnique();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item294);
			value.SetAvoidTechnique(item294, context);
			return;
		}
		case 40:
		{
			SpecialEffectList item293 = value.GetAvoidSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item293);
			value.SetAvoidSpeed(item293, context);
			return;
		}
		case 41:
		{
			SpecialEffectList item292 = value.GetAvoidMind();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item292);
			value.SetAvoidMind(item292, context);
			return;
		}
		case 42:
		{
			SpecialEffectList item291 = value.GetAvoidCanChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item291);
			value.SetAvoidCanChange(item291, context);
			return;
		}
		case 43:
		{
			SpecialEffectList item290 = value.GetAvoidChangeEffectPercent();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item290);
			value.SetAvoidChangeEffectPercent(item290, context);
			return;
		}
		case 44:
		{
			SpecialEffectList item289 = value.GetPenetrateOuter();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item289);
			value.SetPenetrateOuter(item289, context);
			return;
		}
		case 45:
		{
			SpecialEffectList item288 = value.GetPenetrateInner();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item288);
			value.SetPenetrateInner(item288, context);
			return;
		}
		case 46:
		{
			SpecialEffectList item287 = value.GetPenetrateResistOuter();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item287);
			value.SetPenetrateResistOuter(item287, context);
			return;
		}
		case 47:
		{
			SpecialEffectList item286 = value.GetPenetrateResistInner();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item286);
			value.SetPenetrateResistInner(item286, context);
			return;
		}
		case 48:
		{
			SpecialEffectList item285 = value.GetNeiliAllocationAttack();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item285);
			value.SetNeiliAllocationAttack(item285, context);
			return;
		}
		case 49:
		{
			SpecialEffectList item284 = value.GetNeiliAllocationAgile();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item284);
			value.SetNeiliAllocationAgile(item284, context);
			return;
		}
		case 50:
		{
			SpecialEffectList item283 = value.GetNeiliAllocationDefense();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item283);
			value.SetNeiliAllocationDefense(item283, context);
			return;
		}
		case 51:
		{
			SpecialEffectList item282 = value.GetNeiliAllocationAssist();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item282);
			value.SetNeiliAllocationAssist(item282, context);
			return;
		}
		case 52:
		{
			SpecialEffectList item281 = value.GetHappiness();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item281);
			value.SetHappiness(item281, context);
			return;
		}
		case 53:
		{
			SpecialEffectList item280 = value.GetMaxHealth();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item280);
			value.SetMaxHealth(item280, context);
			return;
		}
		case 54:
		{
			SpecialEffectList item279 = value.GetHealthCost();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item279);
			value.SetHealthCost(item279, context);
			return;
		}
		case 55:
		{
			SpecialEffectList item278 = value.GetMoveSpeedCanChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item278);
			value.SetMoveSpeedCanChange(item278, context);
			return;
		}
		case 56:
		{
			SpecialEffectList item277 = value.GetAttackerHitStrength();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item277);
			value.SetAttackerHitStrength(item277, context);
			return;
		}
		case 57:
		{
			SpecialEffectList item276 = value.GetAttackerHitTechnique();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item276);
			value.SetAttackerHitTechnique(item276, context);
			return;
		}
		case 58:
		{
			SpecialEffectList item275 = value.GetAttackerHitSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item275);
			value.SetAttackerHitSpeed(item275, context);
			return;
		}
		case 59:
		{
			SpecialEffectList item274 = value.GetAttackerHitMind();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item274);
			value.SetAttackerHitMind(item274, context);
			return;
		}
		case 60:
		{
			SpecialEffectList item273 = value.GetAttackerAvoidStrength();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item273);
			value.SetAttackerAvoidStrength(item273, context);
			return;
		}
		case 61:
		{
			SpecialEffectList item272 = value.GetAttackerAvoidTechnique();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item272);
			value.SetAttackerAvoidTechnique(item272, context);
			return;
		}
		case 62:
		{
			SpecialEffectList item271 = value.GetAttackerAvoidSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item271);
			value.SetAttackerAvoidSpeed(item271, context);
			return;
		}
		case 63:
		{
			SpecialEffectList item270 = value.GetAttackerAvoidMind();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item270);
			value.SetAttackerAvoidMind(item270, context);
			return;
		}
		case 64:
		{
			SpecialEffectList item269 = value.GetAttackerPenetrateOuter();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item269);
			value.SetAttackerPenetrateOuter(item269, context);
			return;
		}
		case 65:
		{
			SpecialEffectList item268 = value.GetAttackerPenetrateInner();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item268);
			value.SetAttackerPenetrateInner(item268, context);
			return;
		}
		case 66:
		{
			SpecialEffectList item267 = value.GetAttackerPenetrateResistOuter();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item267);
			value.SetAttackerPenetrateResistOuter(item267, context);
			return;
		}
		case 67:
		{
			SpecialEffectList item266 = value.GetAttackerPenetrateResistInner();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item266);
			value.SetAttackerPenetrateResistInner(item266, context);
			return;
		}
		case 68:
		{
			SpecialEffectList item265 = value.GetAttackHitType();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item265);
			value.SetAttackHitType(item265, context);
			return;
		}
		case 69:
		{
			SpecialEffectList item264 = value.GetMakeDirectDamage();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item264);
			value.SetMakeDirectDamage(item264, context);
			return;
		}
		case 70:
		{
			SpecialEffectList item263 = value.GetMakeBounceDamage();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item263);
			value.SetMakeBounceDamage(item263, context);
			return;
		}
		case 71:
		{
			SpecialEffectList item262 = value.GetMakeFightBackDamage();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item262);
			value.SetMakeFightBackDamage(item262, context);
			return;
		}
		case 72:
		{
			SpecialEffectList item261 = value.GetMakePoisonLevel();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item261);
			value.SetMakePoisonLevel(item261, context);
			return;
		}
		case 73:
		{
			SpecialEffectList item260 = value.GetMakePoisonValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item260);
			value.SetMakePoisonValue(item260, context);
			return;
		}
		case 74:
		{
			SpecialEffectList item259 = value.GetAttackerHitOdds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item259);
			value.SetAttackerHitOdds(item259, context);
			return;
		}
		case 75:
		{
			SpecialEffectList item258 = value.GetAttackerFightBackHitOdds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item258);
			value.SetAttackerFightBackHitOdds(item258, context);
			return;
		}
		case 76:
		{
			SpecialEffectList item257 = value.GetAttackerPursueOdds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item257);
			value.SetAttackerPursueOdds(item257, context);
			return;
		}
		case 77:
		{
			SpecialEffectList item256 = value.GetMakedInjuryChangeToOld();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item256);
			value.SetMakedInjuryChangeToOld(item256, context);
			return;
		}
		case 78:
		{
			SpecialEffectList item255 = value.GetMakedPoisonChangeToOld();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item255);
			value.SetMakedPoisonChangeToOld(item255, context);
			return;
		}
		case 79:
		{
			SpecialEffectList item254 = value.GetMakeDamageType();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item254);
			value.SetMakeDamageType(item254, context);
			return;
		}
		case 80:
		{
			SpecialEffectList item253 = value.GetCanMakeInjuryToNoInjuryPart();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item253);
			value.SetCanMakeInjuryToNoInjuryPart(item253, context);
			return;
		}
		case 81:
		{
			SpecialEffectList item252 = value.GetMakePoisonType();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item252);
			value.SetMakePoisonType(item252, context);
			return;
		}
		case 82:
		{
			SpecialEffectList item251 = value.GetNormalAttackWeapon();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item251);
			value.SetNormalAttackWeapon(item251, context);
			return;
		}
		case 83:
		{
			SpecialEffectList item250 = value.GetNormalAttackTrick();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item250);
			value.SetNormalAttackTrick(item250, context);
			return;
		}
		case 84:
		{
			SpecialEffectList item249 = value.GetExtraFlawCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item249);
			value.SetExtraFlawCount(item249, context);
			return;
		}
		case 85:
		{
			SpecialEffectList item248 = value.GetAttackCanBounce();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item248);
			value.SetAttackCanBounce(item248, context);
			return;
		}
		case 86:
		{
			SpecialEffectList item247 = value.GetAttackCanFightBack();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item247);
			value.SetAttackCanFightBack(item247, context);
			return;
		}
		case 87:
		{
			SpecialEffectList item246 = value.GetMakeFightBackInjuryMark();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item246);
			value.SetMakeFightBackInjuryMark(item246, context);
			return;
		}
		case 88:
		{
			SpecialEffectList item245 = value.GetLegSkillUseShoes();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item245);
			value.SetLegSkillUseShoes(item245, context);
			return;
		}
		case 89:
		{
			SpecialEffectList item244 = value.GetAttackerFinalDamageValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item244);
			value.SetAttackerFinalDamageValue(item244, context);
			return;
		}
		case 90:
		{
			SpecialEffectList item243 = value.GetDefenderHitStrength();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item243);
			value.SetDefenderHitStrength(item243, context);
			return;
		}
		case 91:
		{
			SpecialEffectList item242 = value.GetDefenderHitTechnique();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item242);
			value.SetDefenderHitTechnique(item242, context);
			return;
		}
		case 92:
		{
			SpecialEffectList item241 = value.GetDefenderHitSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item241);
			value.SetDefenderHitSpeed(item241, context);
			return;
		}
		case 93:
		{
			SpecialEffectList item240 = value.GetDefenderHitMind();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item240);
			value.SetDefenderHitMind(item240, context);
			return;
		}
		case 94:
		{
			SpecialEffectList item239 = value.GetDefenderAvoidStrength();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item239);
			value.SetDefenderAvoidStrength(item239, context);
			return;
		}
		case 95:
		{
			SpecialEffectList item238 = value.GetDefenderAvoidTechnique();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item238);
			value.SetDefenderAvoidTechnique(item238, context);
			return;
		}
		case 96:
		{
			SpecialEffectList item237 = value.GetDefenderAvoidSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item237);
			value.SetDefenderAvoidSpeed(item237, context);
			return;
		}
		case 97:
		{
			SpecialEffectList item236 = value.GetDefenderAvoidMind();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item236);
			value.SetDefenderAvoidMind(item236, context);
			return;
		}
		case 98:
		{
			SpecialEffectList item235 = value.GetDefenderPenetrateOuter();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item235);
			value.SetDefenderPenetrateOuter(item235, context);
			return;
		}
		case 99:
		{
			SpecialEffectList item234 = value.GetDefenderPenetrateInner();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item234);
			value.SetDefenderPenetrateInner(item234, context);
			return;
		}
		case 100:
		{
			SpecialEffectList item233 = value.GetDefenderPenetrateResistOuter();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item233);
			value.SetDefenderPenetrateResistOuter(item233, context);
			return;
		}
		case 101:
		{
			SpecialEffectList item232 = value.GetDefenderPenetrateResistInner();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item232);
			value.SetDefenderPenetrateResistInner(item232, context);
			return;
		}
		case 102:
		{
			SpecialEffectList item231 = value.GetAcceptDirectDamage();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item231);
			value.SetAcceptDirectDamage(item231, context);
			return;
		}
		case 103:
		{
			SpecialEffectList item230 = value.GetAcceptBounceDamage();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item230);
			value.SetAcceptBounceDamage(item230, context);
			return;
		}
		case 104:
		{
			SpecialEffectList item229 = value.GetAcceptFightBackDamage();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item229);
			value.SetAcceptFightBackDamage(item229, context);
			return;
		}
		case 105:
		{
			SpecialEffectList item228 = value.GetAcceptPoisonLevel();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item228);
			value.SetAcceptPoisonLevel(item228, context);
			return;
		}
		case 106:
		{
			SpecialEffectList item227 = value.GetAcceptPoisonValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item227);
			value.SetAcceptPoisonValue(item227, context);
			return;
		}
		case 107:
		{
			SpecialEffectList item226 = value.GetDefenderHitOdds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item226);
			value.SetDefenderHitOdds(item226, context);
			return;
		}
		case 108:
		{
			SpecialEffectList item225 = value.GetDefenderFightBackHitOdds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item225);
			value.SetDefenderFightBackHitOdds(item225, context);
			return;
		}
		case 109:
		{
			SpecialEffectList item224 = value.GetDefenderPursueOdds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item224);
			value.SetDefenderPursueOdds(item224, context);
			return;
		}
		case 110:
		{
			SpecialEffectList item223 = value.GetAcceptMaxInjuryCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item223);
			value.SetAcceptMaxInjuryCount(item223, context);
			return;
		}
		case 111:
		{
			SpecialEffectList item222 = value.GetBouncePower();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item222);
			value.SetBouncePower(item222, context);
			return;
		}
		case 112:
		{
			SpecialEffectList item221 = value.GetFightBackPower();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item221);
			value.SetFightBackPower(item221, context);
			return;
		}
		case 113:
		{
			SpecialEffectList item220 = value.GetDirectDamageInnerRatio();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item220);
			value.SetDirectDamageInnerRatio(item220, context);
			return;
		}
		case 114:
		{
			SpecialEffectList item219 = value.GetDefenderFinalDamageValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item219);
			value.SetDefenderFinalDamageValue(item219, context);
			return;
		}
		case 115:
		{
			SpecialEffectList item218 = value.GetDirectDamageValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item218);
			value.SetDirectDamageValue(item218, context);
			return;
		}
		case 116:
		{
			SpecialEffectList item217 = value.GetDirectInjuryMark();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item217);
			value.SetDirectInjuryMark(item217, context);
			return;
		}
		case 117:
		{
			SpecialEffectList item216 = value.GetGoneMadInjury();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item216);
			value.SetGoneMadInjury(item216, context);
			return;
		}
		case 118:
		{
			SpecialEffectList item215 = value.GetHealInjurySpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item215);
			value.SetHealInjurySpeed(item215, context);
			return;
		}
		case 119:
		{
			SpecialEffectList item214 = value.GetHealInjuryBuff();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item214);
			value.SetHealInjuryBuff(item214, context);
			return;
		}
		case 120:
		{
			SpecialEffectList item213 = value.GetHealInjuryDebuff();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item213);
			value.SetHealInjuryDebuff(item213, context);
			return;
		}
		case 121:
		{
			SpecialEffectList item212 = value.GetHealPoisonSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item212);
			value.SetHealPoisonSpeed(item212, context);
			return;
		}
		case 122:
		{
			SpecialEffectList item211 = value.GetHealPoisonBuff();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item211);
			value.SetHealPoisonBuff(item211, context);
			return;
		}
		case 123:
		{
			SpecialEffectList item210 = value.GetHealPoisonDebuff();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item210);
			value.SetHealPoisonDebuff(item210, context);
			return;
		}
		case 124:
		{
			SpecialEffectList item209 = value.GetFleeSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item209);
			value.SetFleeSpeed(item209, context);
			return;
		}
		case 125:
		{
			SpecialEffectList item208 = value.GetMaxFlawCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item208);
			value.SetMaxFlawCount(item208, context);
			return;
		}
		case 126:
		{
			SpecialEffectList item207 = value.GetCanAddFlaw();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item207);
			value.SetCanAddFlaw(item207, context);
			return;
		}
		case 127:
		{
			SpecialEffectList item206 = value.GetFlawLevel();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item206);
			value.SetFlawLevel(item206, context);
			return;
		}
		case 128:
		{
			SpecialEffectList item205 = value.GetFlawLevelCanReduce();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item205);
			value.SetFlawLevelCanReduce(item205, context);
			return;
		}
		case 129:
		{
			SpecialEffectList item204 = value.GetFlawCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item204);
			value.SetFlawCount(item204, context);
			return;
		}
		case 130:
		{
			SpecialEffectList item203 = value.GetMaxAcupointCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item203);
			value.SetMaxAcupointCount(item203, context);
			return;
		}
		case 131:
		{
			SpecialEffectList item202 = value.GetCanAddAcupoint();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item202);
			value.SetCanAddAcupoint(item202, context);
			return;
		}
		case 132:
		{
			SpecialEffectList item201 = value.GetAcupointLevel();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item201);
			value.SetAcupointLevel(item201, context);
			return;
		}
		case 133:
		{
			SpecialEffectList item200 = value.GetAcupointLevelCanReduce();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item200);
			value.SetAcupointLevelCanReduce(item200, context);
			return;
		}
		case 134:
		{
			SpecialEffectList item199 = value.GetAcupointCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item199);
			value.SetAcupointCount(item199, context);
			return;
		}
		case 135:
		{
			SpecialEffectList item198 = value.GetAddNeiliAllocation();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item198);
			value.SetAddNeiliAllocation(item198, context);
			return;
		}
		case 136:
		{
			SpecialEffectList item197 = value.GetCostNeiliAllocation();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item197);
			value.SetCostNeiliAllocation(item197, context);
			return;
		}
		case 137:
		{
			SpecialEffectList item196 = value.GetCanChangeNeiliAllocation();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item196);
			value.SetCanChangeNeiliAllocation(item196, context);
			return;
		}
		case 138:
		{
			SpecialEffectList item195 = value.GetCanGetTrick();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item195);
			value.SetCanGetTrick(item195, context);
			return;
		}
		case 139:
		{
			SpecialEffectList item194 = value.GetGetTrickType();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item194);
			value.SetGetTrickType(item194, context);
			return;
		}
		case 140:
		{
			SpecialEffectList item193 = value.GetAttackBodyPart();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item193);
			value.SetAttackBodyPart(item193, context);
			return;
		}
		case 141:
		{
			SpecialEffectList item192 = value.GetWeaponEquipAttack();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item192);
			value.SetWeaponEquipAttack(item192, context);
			return;
		}
		case 142:
		{
			SpecialEffectList item191 = value.GetWeaponEquipDefense();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item191);
			value.SetWeaponEquipDefense(item191, context);
			return;
		}
		case 143:
		{
			SpecialEffectList item190 = value.GetArmorEquipAttack();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item190);
			value.SetArmorEquipAttack(item190, context);
			return;
		}
		case 144:
		{
			SpecialEffectList item189 = value.GetArmorEquipDefense();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item189);
			value.SetArmorEquipDefense(item189, context);
			return;
		}
		case 145:
		{
			SpecialEffectList item188 = value.GetAttackRangeForward();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item188);
			value.SetAttackRangeForward(item188, context);
			return;
		}
		case 146:
		{
			SpecialEffectList item187 = value.GetAttackRangeBackward();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item187);
			value.SetAttackRangeBackward(item187, context);
			return;
		}
		case 147:
		{
			SpecialEffectList item186 = value.GetMoveCanBeStopped();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item186);
			value.SetMoveCanBeStopped(item186, context);
			return;
		}
		case 148:
		{
			SpecialEffectList item185 = value.GetCanForcedMove();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item185);
			value.SetCanForcedMove(item185, context);
			return;
		}
		case 149:
		{
			SpecialEffectList item184 = value.GetMobilityCanBeRemoved();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item184);
			value.SetMobilityCanBeRemoved(item184, context);
			return;
		}
		case 150:
		{
			SpecialEffectList item183 = value.GetMobilityCostByEffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item183);
			value.SetMobilityCostByEffect(item183, context);
			return;
		}
		case 151:
		{
			SpecialEffectList item182 = value.GetMoveDistance();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item182);
			value.SetMoveDistance(item182, context);
			return;
		}
		case 152:
		{
			SpecialEffectList item181 = value.GetJumpPrepareFrame();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item181);
			value.SetJumpPrepareFrame(item181, context);
			return;
		}
		case 153:
		{
			SpecialEffectList item180 = value.GetBounceInjuryMark();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item180);
			value.SetBounceInjuryMark(item180, context);
			return;
		}
		case 154:
		{
			SpecialEffectList item179 = value.GetSkillHasCost();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item179);
			value.SetSkillHasCost(item179, context);
			return;
		}
		case 155:
		{
			SpecialEffectList item178 = value.GetCombatStateEffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item178);
			value.SetCombatStateEffect(item178, context);
			return;
		}
		case 156:
		{
			SpecialEffectList item177 = value.GetChangeNeedUseSkill();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item177);
			value.SetChangeNeedUseSkill(item177, context);
			return;
		}
		case 157:
		{
			SpecialEffectList item176 = value.GetChangeDistanceIsMove();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item176);
			value.SetChangeDistanceIsMove(item176, context);
			return;
		}
		case 158:
		{
			SpecialEffectList item175 = value.GetReplaceCharHit();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item175);
			value.SetReplaceCharHit(item175, context);
			return;
		}
		case 159:
		{
			SpecialEffectList item174 = value.GetCanAddPoison();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item174);
			value.SetCanAddPoison(item174, context);
			return;
		}
		case 160:
		{
			SpecialEffectList item173 = value.GetCanReducePoison();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item173);
			value.SetCanReducePoison(item173, context);
			return;
		}
		case 161:
		{
			SpecialEffectList item172 = value.GetReducePoisonValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item172);
			value.SetReducePoisonValue(item172, context);
			return;
		}
		case 162:
		{
			SpecialEffectList item171 = value.GetPoisonCanAffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item171);
			value.SetPoisonCanAffect(item171, context);
			return;
		}
		case 163:
		{
			SpecialEffectList item170 = value.GetPoisonAffectCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item170);
			value.SetPoisonAffectCount(item170, context);
			return;
		}
		case 164:
		{
			SpecialEffectList item169 = value.GetCostTricks();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item169);
			value.SetCostTricks(item169, context);
			return;
		}
		case 165:
		{
			SpecialEffectList item168 = value.GetJumpMoveDistance();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item168);
			value.SetJumpMoveDistance(item168, context);
			return;
		}
		case 166:
		{
			SpecialEffectList item167 = value.GetCombatStateToAdd();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item167);
			value.SetCombatStateToAdd(item167, context);
			return;
		}
		case 167:
		{
			SpecialEffectList item166 = value.GetCombatStatePower();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item166);
			value.SetCombatStatePower(item166, context);
			return;
		}
		case 168:
		{
			SpecialEffectList item165 = value.GetBreakBodyPartInjuryCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item165);
			value.SetBreakBodyPartInjuryCount(item165, context);
			return;
		}
		case 169:
		{
			SpecialEffectList item164 = value.GetBodyPartIsBroken();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item164);
			value.SetBodyPartIsBroken(item164, context);
			return;
		}
		case 170:
		{
			SpecialEffectList item163 = value.GetMaxTrickCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item163);
			value.SetMaxTrickCount(item163, context);
			return;
		}
		case 171:
		{
			SpecialEffectList item162 = value.GetMaxBreathPercent();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item162);
			value.SetMaxBreathPercent(item162, context);
			return;
		}
		case 172:
		{
			SpecialEffectList item161 = value.GetMaxStancePercent();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item161);
			value.SetMaxStancePercent(item161, context);
			return;
		}
		case 173:
		{
			SpecialEffectList item160 = value.GetExtraBreathPercent();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item160);
			value.SetExtraBreathPercent(item160, context);
			return;
		}
		case 174:
		{
			SpecialEffectList item159 = value.GetExtraStancePercent();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item159);
			value.SetExtraStancePercent(item159, context);
			return;
		}
		case 175:
		{
			SpecialEffectList item158 = value.GetMoveCostMobility();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item158);
			value.SetMoveCostMobility(item158, context);
			return;
		}
		case 176:
		{
			SpecialEffectList item157 = value.GetDefendSkillKeepTime();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item157);
			value.SetDefendSkillKeepTime(item157, context);
			return;
		}
		case 177:
		{
			SpecialEffectList item156 = value.GetBounceRange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item156);
			value.SetBounceRange(item156, context);
			return;
		}
		case 178:
		{
			SpecialEffectList item155 = value.GetMindMarkKeepTime();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item155);
			value.SetMindMarkKeepTime(item155, context);
			return;
		}
		case 179:
		{
			SpecialEffectList item154 = value.GetSkillMobilityCostPerFrame();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item154);
			value.SetSkillMobilityCostPerFrame(item154, context);
			return;
		}
		case 180:
		{
			SpecialEffectList item153 = value.GetCanAddWug();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item153);
			value.SetCanAddWug(item153, context);
			return;
		}
		case 181:
		{
			SpecialEffectList item152 = value.GetHasGodWeaponBuff();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item152);
			value.SetHasGodWeaponBuff(item152, context);
			return;
		}
		case 182:
		{
			SpecialEffectList item151 = value.GetHasGodArmorBuff();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item151);
			value.SetHasGodArmorBuff(item151, context);
			return;
		}
		case 183:
		{
			SpecialEffectList item150 = value.GetTeammateCmdRequireGenerateValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item150);
			value.SetTeammateCmdRequireGenerateValue(item150, context);
			return;
		}
		case 184:
		{
			SpecialEffectList item149 = value.GetTeammateCmdEffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item149);
			value.SetTeammateCmdEffect(item149, context);
			return;
		}
		case 185:
		{
			SpecialEffectList item148 = value.GetFlawRecoverSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item148);
			value.SetFlawRecoverSpeed(item148, context);
			return;
		}
		case 186:
		{
			SpecialEffectList item147 = value.GetAcupointRecoverSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item147);
			value.SetAcupointRecoverSpeed(item147, context);
			return;
		}
		case 187:
		{
			SpecialEffectList item146 = value.GetMindMarkRecoverSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item146);
			value.SetMindMarkRecoverSpeed(item146, context);
			return;
		}
		case 188:
		{
			SpecialEffectList item145 = value.GetInjuryAutoHealSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item145);
			value.SetInjuryAutoHealSpeed(item145, context);
			return;
		}
		case 189:
		{
			SpecialEffectList item144 = value.GetCanRecoverBreath();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item144);
			value.SetCanRecoverBreath(item144, context);
			return;
		}
		case 190:
		{
			SpecialEffectList item143 = value.GetCanRecoverStance();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item143);
			value.SetCanRecoverStance(item143, context);
			return;
		}
		case 191:
		{
			SpecialEffectList item142 = value.GetFatalDamageValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item142);
			value.SetFatalDamageValue(item142, context);
			return;
		}
		case 192:
		{
			SpecialEffectList item141 = value.GetFatalDamageMarkCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item141);
			value.SetFatalDamageMarkCount(item141, context);
			return;
		}
		case 193:
		{
			SpecialEffectList item140 = value.GetCanFightBackDuringPrepareSkill();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item140);
			value.SetCanFightBackDuringPrepareSkill(item140, context);
			return;
		}
		case 194:
		{
			SpecialEffectList item139 = value.GetSkillPrepareSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item139);
			value.SetSkillPrepareSpeed(item139, context);
			return;
		}
		case 195:
		{
			SpecialEffectList item138 = value.GetBreathRecoverSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item138);
			value.SetBreathRecoverSpeed(item138, context);
			return;
		}
		case 196:
		{
			SpecialEffectList item137 = value.GetStanceRecoverSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item137);
			value.SetStanceRecoverSpeed(item137, context);
			return;
		}
		case 197:
		{
			SpecialEffectList item136 = value.GetMobilityRecoverSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item136);
			value.SetMobilityRecoverSpeed(item136, context);
			return;
		}
		case 198:
		{
			SpecialEffectList item135 = value.GetChangeTrickProgressAddValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item135);
			value.SetChangeTrickProgressAddValue(item135, context);
			return;
		}
		case 199:
		{
			SpecialEffectList item134 = value.GetPower();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item134);
			value.SetPower(item134, context);
			return;
		}
		case 200:
		{
			SpecialEffectList item133 = value.GetMaxPower();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item133);
			value.SetMaxPower(item133, context);
			return;
		}
		case 201:
		{
			SpecialEffectList item132 = value.GetPowerCanReduce();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item132);
			value.SetPowerCanReduce(item132, context);
			return;
		}
		case 202:
		{
			SpecialEffectList item131 = value.GetUseRequirement();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item131);
			value.SetUseRequirement(item131, context);
			return;
		}
		case 203:
		{
			SpecialEffectList item130 = value.GetCurrInnerRatio();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item130);
			value.SetCurrInnerRatio(item130, context);
			return;
		}
		case 204:
		{
			SpecialEffectList item129 = value.GetCostBreathAndStance();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item129);
			value.SetCostBreathAndStance(item129, context);
			return;
		}
		case 205:
		{
			SpecialEffectList item128 = value.GetCostBreath();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item128);
			value.SetCostBreath(item128, context);
			return;
		}
		case 206:
		{
			SpecialEffectList item127 = value.GetCostStance();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item127);
			value.SetCostStance(item127, context);
			return;
		}
		case 207:
		{
			SpecialEffectList item126 = value.GetCostMobility();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item126);
			value.SetCostMobility(item126, context);
			return;
		}
		case 208:
		{
			SpecialEffectList item125 = value.GetSkillCostTricks();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item125);
			value.SetSkillCostTricks(item125, context);
			return;
		}
		case 209:
		{
			SpecialEffectList item124 = value.GetEffectDirection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item124);
			value.SetEffectDirection(item124, context);
			return;
		}
		case 210:
		{
			SpecialEffectList item123 = value.GetEffectDirectionCanChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item123);
			value.SetEffectDirectionCanChange(item123, context);
			return;
		}
		case 211:
		{
			SpecialEffectList item122 = value.GetGridCost();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item122);
			value.SetGridCost(item122, context);
			return;
		}
		case 212:
		{
			SpecialEffectList item121 = value.GetPrepareTotalProgress();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item121);
			value.SetPrepareTotalProgress(item121, context);
			return;
		}
		case 213:
		{
			SpecialEffectList item120 = value.GetSpecificGridCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item120);
			value.SetSpecificGridCount(item120, context);
			return;
		}
		case 214:
		{
			SpecialEffectList item119 = value.GetGenericGridCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item119);
			value.SetGenericGridCount(item119, context);
			return;
		}
		case 215:
		{
			SpecialEffectList item118 = value.GetCanInterrupt();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item118);
			value.SetCanInterrupt(item118, context);
			return;
		}
		case 216:
		{
			SpecialEffectList item117 = value.GetInterruptOdds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item117);
			value.SetInterruptOdds(item117, context);
			return;
		}
		case 217:
		{
			SpecialEffectList item116 = value.GetCanSilence();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item116);
			value.SetCanSilence(item116, context);
			return;
		}
		case 218:
		{
			SpecialEffectList item115 = value.GetSilenceOdds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item115);
			value.SetSilenceOdds(item115, context);
			return;
		}
		case 219:
		{
			SpecialEffectList item114 = value.GetCanCastWithBrokenBodyPart();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item114);
			value.SetCanCastWithBrokenBodyPart(item114, context);
			return;
		}
		case 220:
		{
			SpecialEffectList item113 = value.GetAddPowerCanBeRemoved();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item113);
			value.SetAddPowerCanBeRemoved(item113, context);
			return;
		}
		case 221:
		{
			SpecialEffectList item112 = value.GetSkillType();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item112);
			value.SetSkillType(item112, context);
			return;
		}
		case 222:
		{
			SpecialEffectList item111 = value.GetEffectCountCanChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item111);
			value.SetEffectCountCanChange(item111, context);
			return;
		}
		case 223:
		{
			SpecialEffectList item110 = value.GetCanCastInDefend();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item110);
			value.SetCanCastInDefend(item110, context);
			return;
		}
		case 224:
		{
			SpecialEffectList item109 = value.GetHitDistribution();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item109);
			value.SetHitDistribution(item109, context);
			return;
		}
		case 225:
		{
			SpecialEffectList item108 = value.GetCanCastOnLackBreath();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item108);
			value.SetCanCastOnLackBreath(item108, context);
			return;
		}
		case 226:
		{
			SpecialEffectList item107 = value.GetCanCastOnLackStance();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item107);
			value.SetCanCastOnLackStance(item107, context);
			return;
		}
		case 227:
		{
			SpecialEffectList item106 = value.GetCostBreathOnCast();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item106);
			value.SetCostBreathOnCast(item106, context);
			return;
		}
		case 228:
		{
			SpecialEffectList item105 = value.GetCostStanceOnCast();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item105);
			value.SetCostStanceOnCast(item105, context);
			return;
		}
		case 229:
		{
			SpecialEffectList item104 = value.GetCanUseMobilityAsBreath();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item104);
			value.SetCanUseMobilityAsBreath(item104, context);
			return;
		}
		case 230:
		{
			SpecialEffectList item103 = value.GetCanUseMobilityAsStance();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item103);
			value.SetCanUseMobilityAsStance(item103, context);
			return;
		}
		case 231:
		{
			SpecialEffectList item102 = value.GetCastCostNeiliAllocation();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item102);
			value.SetCastCostNeiliAllocation(item102, context);
			return;
		}
		case 232:
		{
			SpecialEffectList item101 = value.GetAcceptPoisonResist();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item101);
			value.SetAcceptPoisonResist(item101, context);
			return;
		}
		case 233:
		{
			SpecialEffectList item100 = value.GetMakePoisonResist();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item100);
			value.SetMakePoisonResist(item100, context);
			return;
		}
		case 234:
		{
			SpecialEffectList item99 = value.GetCanCriticalHit();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item99);
			value.SetCanCriticalHit(item99, context);
			return;
		}
		case 235:
		{
			SpecialEffectList item98 = value.GetCanCostNeiliAllocationEffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item98);
			value.SetCanCostNeiliAllocationEffect(item98, context);
			return;
		}
		case 236:
		{
			SpecialEffectList item97 = value.GetConsummateLevelRelatedMainAttributesHitValues();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item97);
			value.SetConsummateLevelRelatedMainAttributesHitValues(item97, context);
			return;
		}
		case 237:
		{
			SpecialEffectList item96 = value.GetConsummateLevelRelatedMainAttributesAvoidValues();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item96);
			value.SetConsummateLevelRelatedMainAttributesAvoidValues(item96, context);
			return;
		}
		case 238:
		{
			SpecialEffectList item95 = value.GetConsummateLevelRelatedMainAttributesPenetrations();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item95);
			value.SetConsummateLevelRelatedMainAttributesPenetrations(item95, context);
			return;
		}
		case 239:
		{
			SpecialEffectList item94 = value.GetConsummateLevelRelatedMainAttributesPenetrationResists();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item94);
			value.SetConsummateLevelRelatedMainAttributesPenetrationResists(item94, context);
			return;
		}
		case 240:
		{
			SpecialEffectList item93 = value.GetSkillAlsoAsFiveElements();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item93);
			value.SetSkillAlsoAsFiveElements(item93, context);
			return;
		}
		case 241:
		{
			SpecialEffectList item92 = value.GetInnerInjuryImmunity();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item92);
			value.SetInnerInjuryImmunity(item92, context);
			return;
		}
		case 242:
		{
			SpecialEffectList item91 = value.GetOuterInjuryImmunity();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item91);
			value.SetOuterInjuryImmunity(item91, context);
			return;
		}
		case 243:
		{
			SpecialEffectList item90 = value.GetPoisonAffectThreshold();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item90);
			value.SetPoisonAffectThreshold(item90, context);
			return;
		}
		case 244:
		{
			SpecialEffectList item89 = value.GetLockDistance();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item89);
			value.SetLockDistance(item89, context);
			return;
		}
		case 245:
		{
			SpecialEffectList item88 = value.GetResistOfAllPoison();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item88);
			value.SetResistOfAllPoison(item88, context);
			return;
		}
		case 246:
		{
			SpecialEffectList item87 = value.GetMakePoisonTarget();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item87);
			value.SetMakePoisonTarget(item87, context);
			return;
		}
		case 247:
		{
			SpecialEffectList item86 = value.GetAcceptPoisonTarget();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item86);
			value.SetAcceptPoisonTarget(item86, context);
			return;
		}
		case 248:
		{
			SpecialEffectList item85 = value.GetCertainCriticalHit();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item85);
			value.SetCertainCriticalHit(item85, context);
			return;
		}
		case 249:
		{
			SpecialEffectList item84 = value.GetMindMarkCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item84);
			value.SetMindMarkCount(item84, context);
			return;
		}
		case 250:
		{
			SpecialEffectList item83 = value.GetCanFightBackWithHit();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item83);
			value.SetCanFightBackWithHit(item83, context);
			return;
		}
		case 251:
		{
			SpecialEffectList item82 = value.GetInevitableHit();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item82);
			value.SetInevitableHit(item82, context);
			return;
		}
		case 252:
		{
			SpecialEffectList item81 = value.GetAttackCanPursue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item81);
			value.SetAttackCanPursue(item81, context);
			return;
		}
		case 253:
		{
			SpecialEffectList item80 = value.GetCombatSkillDataEffectList();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item80);
			value.SetCombatSkillDataEffectList(item80, context);
			return;
		}
		case 254:
		{
			SpecialEffectList item79 = value.GetCriticalOdds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item79);
			value.SetCriticalOdds(item79, context);
			return;
		}
		case 255:
		{
			SpecialEffectList item78 = value.GetStanceCostByEffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item78);
			value.SetStanceCostByEffect(item78, context);
			return;
		}
		case 256:
		{
			SpecialEffectList item77 = value.GetBreathCostByEffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item77);
			value.SetBreathCostByEffect(item77, context);
			return;
		}
		case 257:
		{
			SpecialEffectList item76 = value.GetPowerAddRatio();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item76);
			value.SetPowerAddRatio(item76, context);
			return;
		}
		case 258:
		{
			SpecialEffectList item75 = value.GetPowerReduceRatio();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item75);
			value.SetPowerReduceRatio(item75, context);
			return;
		}
		case 259:
		{
			SpecialEffectList item74 = value.GetPoisonAffectProduceValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item74);
			value.SetPoisonAffectProduceValue(item74, context);
			return;
		}
		case 260:
		{
			SpecialEffectList item73 = value.GetCanReadingOnMonthChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item73);
			value.SetCanReadingOnMonthChange(item73, context);
			return;
		}
		case 261:
		{
			SpecialEffectList item72 = value.GetMedicineEffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item72);
			value.SetMedicineEffect(item72, context);
			return;
		}
		case 262:
		{
			SpecialEffectList item71 = value.GetXiangshuInfectionDelta();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item71);
			value.SetXiangshuInfectionDelta(item71, context);
			return;
		}
		case 263:
		{
			SpecialEffectList item70 = value.GetHealthDelta();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item70);
			value.SetHealthDelta(item70, context);
			return;
		}
		case 264:
		{
			SpecialEffectList item69 = value.GetWeaponSilenceFrame();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item69);
			value.SetWeaponSilenceFrame(item69, context);
			return;
		}
		case 265:
		{
			SpecialEffectList item68 = value.GetSilenceFrame();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item68);
			value.SetSilenceFrame(item68, context);
			return;
		}
		case 266:
		{
			SpecialEffectList item67 = value.GetCurrAgeDelta();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item67);
			value.SetCurrAgeDelta(item67, context);
			return;
		}
		case 267:
		{
			SpecialEffectList item66 = value.GetGoneMadInAllBreak();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item66);
			value.SetGoneMadInAllBreak(item66, context);
			return;
		}
		case 268:
		{
			SpecialEffectList item65 = value.GetMakeLoveRateOnMonthChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item65);
			value.SetMakeLoveRateOnMonthChange(item65, context);
			return;
		}
		case 269:
		{
			SpecialEffectList item64 = value.GetCanAutoHealOnMonthChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item64);
			value.SetCanAutoHealOnMonthChange(item64, context);
			return;
		}
		case 270:
		{
			SpecialEffectList item63 = value.GetHappinessDelta();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item63);
			value.SetHappinessDelta(item63, context);
			return;
		}
		case 271:
		{
			SpecialEffectList item62 = value.GetTeammateCmdCanUse();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item62);
			value.SetTeammateCmdCanUse(item62, context);
			return;
		}
		case 272:
		{
			SpecialEffectList item61 = value.GetMixPoisonInfinityAffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item61);
			value.SetMixPoisonInfinityAffect(item61, context);
			return;
		}
		case 273:
		{
			SpecialEffectList item60 = value.GetAttackRangeMaxAcupoint();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item60);
			value.SetAttackRangeMaxAcupoint(item60, context);
			return;
		}
		case 274:
		{
			SpecialEffectList item59 = value.GetMaxMobilityPercent();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item59);
			value.SetMaxMobilityPercent(item59, context);
			return;
		}
		case 275:
		{
			SpecialEffectList item58 = value.GetMakeMindDamage();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item58);
			value.SetMakeMindDamage(item58, context);
			return;
		}
		case 276:
		{
			SpecialEffectList item57 = value.GetAcceptMindDamage();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item57);
			value.SetAcceptMindDamage(item57, context);
			return;
		}
		case 277:
		{
			SpecialEffectList item56 = value.GetHitAddByTempValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item56);
			value.SetHitAddByTempValue(item56, context);
			return;
		}
		case 278:
		{
			SpecialEffectList item55 = value.GetAvoidAddByTempValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item55);
			value.SetAvoidAddByTempValue(item55, context);
			return;
		}
		case 279:
		{
			SpecialEffectList item54 = value.GetIgnoreEquipmentOverload();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item54);
			value.SetIgnoreEquipmentOverload(item54, context);
			return;
		}
		case 280:
		{
			SpecialEffectList item53 = value.GetCanCostEnemyUsableTricks();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item53);
			value.SetCanCostEnemyUsableTricks(item53, context);
			return;
		}
		case 281:
		{
			SpecialEffectList item52 = value.GetIgnoreArmor();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item52);
			value.SetIgnoreArmor(item52, context);
			return;
		}
		case 282:
		{
			SpecialEffectList item51 = value.GetUnyieldingFallen();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item51);
			value.SetUnyieldingFallen(item51, context);
			return;
		}
		case 283:
		{
			SpecialEffectList item50 = value.GetNormalAttackPrepareFrame();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item50);
			value.SetNormalAttackPrepareFrame(item50, context);
			return;
		}
		case 284:
		{
			SpecialEffectList item49 = value.GetCanCostUselessTricks();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item49);
			value.SetCanCostUselessTricks(item49, context);
			return;
		}
		case 285:
		{
			SpecialEffectList item48 = value.GetDefendSkillCanAffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item48);
			value.SetDefendSkillCanAffect(item48, context);
			return;
		}
		case 286:
		{
			SpecialEffectList item47 = value.GetAssistSkillCanAffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item47);
			value.SetAssistSkillCanAffect(item47, context);
			return;
		}
		case 287:
		{
			SpecialEffectList item46 = value.GetAgileSkillCanAffect();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item46);
			value.SetAgileSkillCanAffect(item46, context);
			return;
		}
		case 288:
		{
			SpecialEffectList item45 = value.GetAllMarkChangeToMind();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item45);
			value.SetAllMarkChangeToMind(item45, context);
			return;
		}
		case 289:
		{
			SpecialEffectList item44 = value.GetMindMarkChangeToFatal();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item44);
			value.SetMindMarkChangeToFatal(item44, context);
			return;
		}
		case 290:
		{
			SpecialEffectList item43 = value.GetCanCast();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item43);
			value.SetCanCast(item43, context);
			return;
		}
		case 291:
		{
			SpecialEffectList item42 = value.GetInevitableAvoid();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item42);
			value.SetInevitableAvoid(item42, context);
			return;
		}
		case 292:
		{
			SpecialEffectList item41 = value.GetPowerEffectReverse();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item41);
			value.SetPowerEffectReverse(item41, context);
			return;
		}
		case 293:
		{
			SpecialEffectList item40 = value.GetFeatureBonusReverse();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item40);
			value.SetFeatureBonusReverse(item40, context);
			return;
		}
		case 294:
		{
			SpecialEffectList item39 = value.GetWugFatalDamageValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item39);
			value.SetWugFatalDamageValue(item39, context);
			return;
		}
		case 295:
		{
			SpecialEffectList item38 = value.GetCanRecoverHealthOnMonthChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item38);
			value.SetCanRecoverHealthOnMonthChange(item38, context);
			return;
		}
		case 296:
		{
			SpecialEffectList item37 = value.GetTakeRevengeRateOnMonthChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item37);
			value.SetTakeRevengeRateOnMonthChange(item37, context);
			return;
		}
		case 297:
		{
			SpecialEffectList item36 = value.GetConsummateLevelBonus();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item36);
			value.SetConsummateLevelBonus(item36, context);
			return;
		}
		case 298:
		{
			SpecialEffectList item35 = value.GetNeiliDelta();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item35);
			value.SetNeiliDelta(item35, context);
			return;
		}
		case 299:
		{
			SpecialEffectList item34 = value.GetCanMakeLoveSpecialOnMonthChange();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item34);
			value.SetCanMakeLoveSpecialOnMonthChange(item34, context);
			return;
		}
		case 300:
		{
			SpecialEffectList item33 = value.GetHealAcupointSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item33);
			value.SetHealAcupointSpeed(item33, context);
			return;
		}
		case 301:
		{
			SpecialEffectList item32 = value.GetMaxChangeTrickCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item32);
			value.SetMaxChangeTrickCount(item32, context);
			return;
		}
		case 302:
		{
			SpecialEffectList item31 = value.GetConvertCostBreathAndStance();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item31);
			value.SetConvertCostBreathAndStance(item31, context);
			return;
		}
		case 303:
		{
			SpecialEffectList item30 = value.GetPersonalitiesAll();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item30);
			value.SetPersonalitiesAll(item30, context);
			return;
		}
		case 304:
		{
			SpecialEffectList item29 = value.GetFinalFatalDamageMarkCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item29);
			value.SetFinalFatalDamageMarkCount(item29, context);
			return;
		}
		case 305:
		{
			SpecialEffectList item28 = value.GetInfinityMindMarkProgress();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item28);
			value.SetInfinityMindMarkProgress(item28, context);
			return;
		}
		case 306:
		{
			SpecialEffectList item27 = value.GetCombatSkillAiScorePower();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item27);
			value.SetCombatSkillAiScorePower(item27, context);
			return;
		}
		case 307:
		{
			SpecialEffectList item26 = value.GetNormalAttackChangeToUnlockAttack();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item26);
			value.SetNormalAttackChangeToUnlockAttack(item26, context);
			return;
		}
		case 308:
		{
			SpecialEffectList item25 = value.GetAttackBodyPartOdds();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item25);
			value.SetAttackBodyPartOdds(item25, context);
			return;
		}
		case 309:
		{
			SpecialEffectList item24 = value.GetChangeDurability();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item24);
			value.SetChangeDurability(item24, context);
			return;
		}
		case 310:
		{
			SpecialEffectList item23 = value.GetEquipmentBonus();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item23);
			value.SetEquipmentBonus(item23, context);
			return;
		}
		case 311:
		{
			SpecialEffectList item22 = value.GetEquipmentWeight();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item22);
			value.SetEquipmentWeight(item22, context);
			return;
		}
		case 312:
		{
			SpecialEffectList item21 = value.GetRawCreateEffectList();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item21);
			value.SetRawCreateEffectList(item21, context);
			return;
		}
		case 313:
		{
			SpecialEffectList item20 = value.GetJiTrickAsWeaponTrickCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item20);
			value.SetJiTrickAsWeaponTrickCount(item20, context);
			return;
		}
		case 314:
		{
			SpecialEffectList item19 = value.GetUselessTrickAsJiTrickCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item19);
			value.SetUselessTrickAsJiTrickCount(item19, context);
			return;
		}
		case 315:
		{
			SpecialEffectList item18 = value.GetEquipmentPower();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item18);
			value.SetEquipmentPower(item18, context);
			return;
		}
		case 316:
		{
			SpecialEffectList item17 = value.GetHealFlawSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item17);
			value.SetHealFlawSpeed(item17, context);
			return;
		}
		case 317:
		{
			SpecialEffectList item16 = value.GetUnlockSpeed();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item16);
			value.SetUnlockSpeed(item16, context);
			return;
		}
		case 318:
		{
			SpecialEffectList item15 = value.GetFlawBonusFactor();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item15);
			value.SetFlawBonusFactor(item15, context);
			return;
		}
		case 319:
		{
			SpecialEffectList item14 = value.GetCanCostShaTricks();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item14);
			value.SetCanCostShaTricks(item14, context);
			return;
		}
		case 320:
		{
			SpecialEffectList item13 = value.GetDefenderDirectFinalDamageValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item13);
			value.SetDefenderDirectFinalDamageValue(item13, context);
			return;
		}
		case 321:
		{
			SpecialEffectList item12 = value.GetNormalAttackRecoveryFrame();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item12);
			value.SetNormalAttackRecoveryFrame(item12, context);
			return;
		}
		case 322:
		{
			SpecialEffectList item11 = value.GetFinalGoneMadInjury();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item11);
			value.SetFinalGoneMadInjury(item11, context);
			return;
		}
		case 323:
		{
			SpecialEffectList item10 = value.GetAttackerDirectFinalDamageValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item10);
			value.SetAttackerDirectFinalDamageValue(item10, context);
			return;
		}
		case 324:
		{
			SpecialEffectList item9 = value.GetCanCostTrickDuringPreparingSkill();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item9);
			value.SetCanCostTrickDuringPreparingSkill(item9, context);
			return;
		}
		case 325:
		{
			SpecialEffectList item8 = value.GetValidItemList();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item8);
			value.SetValidItemList(item8, context);
			return;
		}
		case 326:
		{
			SpecialEffectList item7 = value.GetAcceptDamageCanAdd();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item7);
			value.SetAcceptDamageCanAdd(item7, context);
			return;
		}
		case 327:
		{
			SpecialEffectList item6 = value.GetMakeDamageCanReduce();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item6);
			value.SetMakeDamageCanReduce(item6, context);
			return;
		}
		case 328:
		{
			SpecialEffectList item5 = value.GetNormalAttackGetTrickCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetNormalAttackGetTrickCount(item5, context);
			return;
		}
		}
		if (fieldId >= 329)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 329)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_AffectedDatas(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_affectedDatas.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 329)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesAffectedDatas.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesAffectedDatas.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetMaxStrength(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDexterity(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetMaxConcentration(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetMaxVitality(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetMaxEnergy(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetMaxIntelligence(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetRecoveryOfStance(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetRecoveryOfBreath(), dataPool), 
			9 => GameData.Serializer.Serializer.Serialize(value.GetMoveSpeed(), dataPool), 
			10 => GameData.Serializer.Serializer.Serialize(value.GetRecoveryOfFlaw(), dataPool), 
			11 => GameData.Serializer.Serializer.Serialize(value.GetCastSpeed(), dataPool), 
			12 => GameData.Serializer.Serializer.Serialize(value.GetRecoveryOfBlockedAcupoint(), dataPool), 
			13 => GameData.Serializer.Serializer.Serialize(value.GetWeaponSwitchSpeed(), dataPool), 
			14 => GameData.Serializer.Serializer.Serialize(value.GetAttackSpeed(), dataPool), 
			15 => GameData.Serializer.Serializer.Serialize(value.GetInnerRatio(), dataPool), 
			16 => GameData.Serializer.Serializer.Serialize(value.GetRecoveryOfQiDisorder(), dataPool), 
			17 => GameData.Serializer.Serializer.Serialize(value.GetMinorAttributeFixMaxValue(), dataPool), 
			18 => GameData.Serializer.Serializer.Serialize(value.GetMinorAttributeFixMinValue(), dataPool), 
			19 => GameData.Serializer.Serializer.Serialize(value.GetResistOfHotPoison(), dataPool), 
			20 => GameData.Serializer.Serializer.Serialize(value.GetResistOfGloomyPoison(), dataPool), 
			21 => GameData.Serializer.Serializer.Serialize(value.GetResistOfColdPoison(), dataPool), 
			22 => GameData.Serializer.Serializer.Serialize(value.GetResistOfRedPoison(), dataPool), 
			23 => GameData.Serializer.Serializer.Serialize(value.GetResistOfRottenPoison(), dataPool), 
			24 => GameData.Serializer.Serializer.Serialize(value.GetResistOfIllusoryPoison(), dataPool), 
			25 => GameData.Serializer.Serializer.Serialize(value.GetDisplayAge(), dataPool), 
			26 => GameData.Serializer.Serializer.Serialize(value.GetNeiliProportionOfFiveElements(), dataPool), 
			27 => GameData.Serializer.Serializer.Serialize(value.GetWeaponMaxPower(), dataPool), 
			28 => GameData.Serializer.Serializer.Serialize(value.GetWeaponUseRequirement(), dataPool), 
			29 => GameData.Serializer.Serializer.Serialize(value.GetWeaponAttackRange(), dataPool), 
			30 => GameData.Serializer.Serializer.Serialize(value.GetArmorMaxPower(), dataPool), 
			31 => GameData.Serializer.Serializer.Serialize(value.GetArmorUseRequirement(), dataPool), 
			32 => GameData.Serializer.Serializer.Serialize(value.GetHitStrength(), dataPool), 
			33 => GameData.Serializer.Serializer.Serialize(value.GetHitTechnique(), dataPool), 
			34 => GameData.Serializer.Serializer.Serialize(value.GetHitSpeed(), dataPool), 
			35 => GameData.Serializer.Serializer.Serialize(value.GetHitMind(), dataPool), 
			36 => GameData.Serializer.Serializer.Serialize(value.GetHitCanChange(), dataPool), 
			37 => GameData.Serializer.Serializer.Serialize(value.GetHitChangeEffectPercent(), dataPool), 
			38 => GameData.Serializer.Serializer.Serialize(value.GetAvoidStrength(), dataPool), 
			39 => GameData.Serializer.Serializer.Serialize(value.GetAvoidTechnique(), dataPool), 
			40 => GameData.Serializer.Serializer.Serialize(value.GetAvoidSpeed(), dataPool), 
			41 => GameData.Serializer.Serializer.Serialize(value.GetAvoidMind(), dataPool), 
			42 => GameData.Serializer.Serializer.Serialize(value.GetAvoidCanChange(), dataPool), 
			43 => GameData.Serializer.Serializer.Serialize(value.GetAvoidChangeEffectPercent(), dataPool), 
			44 => GameData.Serializer.Serializer.Serialize(value.GetPenetrateOuter(), dataPool), 
			45 => GameData.Serializer.Serializer.Serialize(value.GetPenetrateInner(), dataPool), 
			46 => GameData.Serializer.Serializer.Serialize(value.GetPenetrateResistOuter(), dataPool), 
			47 => GameData.Serializer.Serializer.Serialize(value.GetPenetrateResistInner(), dataPool), 
			48 => GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationAttack(), dataPool), 
			49 => GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationAgile(), dataPool), 
			50 => GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationDefense(), dataPool), 
			51 => GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationAssist(), dataPool), 
			52 => GameData.Serializer.Serializer.Serialize(value.GetHappiness(), dataPool), 
			53 => GameData.Serializer.Serializer.Serialize(value.GetMaxHealth(), dataPool), 
			54 => GameData.Serializer.Serializer.Serialize(value.GetHealthCost(), dataPool), 
			55 => GameData.Serializer.Serializer.Serialize(value.GetMoveSpeedCanChange(), dataPool), 
			56 => GameData.Serializer.Serializer.Serialize(value.GetAttackerHitStrength(), dataPool), 
			57 => GameData.Serializer.Serializer.Serialize(value.GetAttackerHitTechnique(), dataPool), 
			58 => GameData.Serializer.Serializer.Serialize(value.GetAttackerHitSpeed(), dataPool), 
			59 => GameData.Serializer.Serializer.Serialize(value.GetAttackerHitMind(), dataPool), 
			60 => GameData.Serializer.Serializer.Serialize(value.GetAttackerAvoidStrength(), dataPool), 
			61 => GameData.Serializer.Serializer.Serialize(value.GetAttackerAvoidTechnique(), dataPool), 
			62 => GameData.Serializer.Serializer.Serialize(value.GetAttackerAvoidSpeed(), dataPool), 
			63 => GameData.Serializer.Serializer.Serialize(value.GetAttackerAvoidMind(), dataPool), 
			64 => GameData.Serializer.Serializer.Serialize(value.GetAttackerPenetrateOuter(), dataPool), 
			65 => GameData.Serializer.Serializer.Serialize(value.GetAttackerPenetrateInner(), dataPool), 
			66 => GameData.Serializer.Serializer.Serialize(value.GetAttackerPenetrateResistOuter(), dataPool), 
			67 => GameData.Serializer.Serializer.Serialize(value.GetAttackerPenetrateResistInner(), dataPool), 
			68 => GameData.Serializer.Serializer.Serialize(value.GetAttackHitType(), dataPool), 
			69 => GameData.Serializer.Serializer.Serialize(value.GetMakeDirectDamage(), dataPool), 
			70 => GameData.Serializer.Serializer.Serialize(value.GetMakeBounceDamage(), dataPool), 
			71 => GameData.Serializer.Serializer.Serialize(value.GetMakeFightBackDamage(), dataPool), 
			72 => GameData.Serializer.Serializer.Serialize(value.GetMakePoisonLevel(), dataPool), 
			73 => GameData.Serializer.Serializer.Serialize(value.GetMakePoisonValue(), dataPool), 
			74 => GameData.Serializer.Serializer.Serialize(value.GetAttackerHitOdds(), dataPool), 
			75 => GameData.Serializer.Serializer.Serialize(value.GetAttackerFightBackHitOdds(), dataPool), 
			76 => GameData.Serializer.Serializer.Serialize(value.GetAttackerPursueOdds(), dataPool), 
			77 => GameData.Serializer.Serializer.Serialize(value.GetMakedInjuryChangeToOld(), dataPool), 
			78 => GameData.Serializer.Serializer.Serialize(value.GetMakedPoisonChangeToOld(), dataPool), 
			79 => GameData.Serializer.Serializer.Serialize(value.GetMakeDamageType(), dataPool), 
			80 => GameData.Serializer.Serializer.Serialize(value.GetCanMakeInjuryToNoInjuryPart(), dataPool), 
			81 => GameData.Serializer.Serializer.Serialize(value.GetMakePoisonType(), dataPool), 
			82 => GameData.Serializer.Serializer.Serialize(value.GetNormalAttackWeapon(), dataPool), 
			83 => GameData.Serializer.Serializer.Serialize(value.GetNormalAttackTrick(), dataPool), 
			84 => GameData.Serializer.Serializer.Serialize(value.GetExtraFlawCount(), dataPool), 
			85 => GameData.Serializer.Serializer.Serialize(value.GetAttackCanBounce(), dataPool), 
			86 => GameData.Serializer.Serializer.Serialize(value.GetAttackCanFightBack(), dataPool), 
			87 => GameData.Serializer.Serializer.Serialize(value.GetMakeFightBackInjuryMark(), dataPool), 
			88 => GameData.Serializer.Serializer.Serialize(value.GetLegSkillUseShoes(), dataPool), 
			89 => GameData.Serializer.Serializer.Serialize(value.GetAttackerFinalDamageValue(), dataPool), 
			90 => GameData.Serializer.Serializer.Serialize(value.GetDefenderHitStrength(), dataPool), 
			91 => GameData.Serializer.Serializer.Serialize(value.GetDefenderHitTechnique(), dataPool), 
			92 => GameData.Serializer.Serializer.Serialize(value.GetDefenderHitSpeed(), dataPool), 
			93 => GameData.Serializer.Serializer.Serialize(value.GetDefenderHitMind(), dataPool), 
			94 => GameData.Serializer.Serializer.Serialize(value.GetDefenderAvoidStrength(), dataPool), 
			95 => GameData.Serializer.Serializer.Serialize(value.GetDefenderAvoidTechnique(), dataPool), 
			96 => GameData.Serializer.Serializer.Serialize(value.GetDefenderAvoidSpeed(), dataPool), 
			97 => GameData.Serializer.Serializer.Serialize(value.GetDefenderAvoidMind(), dataPool), 
			98 => GameData.Serializer.Serializer.Serialize(value.GetDefenderPenetrateOuter(), dataPool), 
			99 => GameData.Serializer.Serializer.Serialize(value.GetDefenderPenetrateInner(), dataPool), 
			100 => GameData.Serializer.Serializer.Serialize(value.GetDefenderPenetrateResistOuter(), dataPool), 
			101 => GameData.Serializer.Serializer.Serialize(value.GetDefenderPenetrateResistInner(), dataPool), 
			102 => GameData.Serializer.Serializer.Serialize(value.GetAcceptDirectDamage(), dataPool), 
			103 => GameData.Serializer.Serializer.Serialize(value.GetAcceptBounceDamage(), dataPool), 
			104 => GameData.Serializer.Serializer.Serialize(value.GetAcceptFightBackDamage(), dataPool), 
			105 => GameData.Serializer.Serializer.Serialize(value.GetAcceptPoisonLevel(), dataPool), 
			106 => GameData.Serializer.Serializer.Serialize(value.GetAcceptPoisonValue(), dataPool), 
			107 => GameData.Serializer.Serializer.Serialize(value.GetDefenderHitOdds(), dataPool), 
			108 => GameData.Serializer.Serializer.Serialize(value.GetDefenderFightBackHitOdds(), dataPool), 
			109 => GameData.Serializer.Serializer.Serialize(value.GetDefenderPursueOdds(), dataPool), 
			110 => GameData.Serializer.Serializer.Serialize(value.GetAcceptMaxInjuryCount(), dataPool), 
			111 => GameData.Serializer.Serializer.Serialize(value.GetBouncePower(), dataPool), 
			112 => GameData.Serializer.Serializer.Serialize(value.GetFightBackPower(), dataPool), 
			113 => GameData.Serializer.Serializer.Serialize(value.GetDirectDamageInnerRatio(), dataPool), 
			114 => GameData.Serializer.Serializer.Serialize(value.GetDefenderFinalDamageValue(), dataPool), 
			115 => GameData.Serializer.Serializer.Serialize(value.GetDirectDamageValue(), dataPool), 
			116 => GameData.Serializer.Serializer.Serialize(value.GetDirectInjuryMark(), dataPool), 
			117 => GameData.Serializer.Serializer.Serialize(value.GetGoneMadInjury(), dataPool), 
			118 => GameData.Serializer.Serializer.Serialize(value.GetHealInjurySpeed(), dataPool), 
			119 => GameData.Serializer.Serializer.Serialize(value.GetHealInjuryBuff(), dataPool), 
			120 => GameData.Serializer.Serializer.Serialize(value.GetHealInjuryDebuff(), dataPool), 
			121 => GameData.Serializer.Serializer.Serialize(value.GetHealPoisonSpeed(), dataPool), 
			122 => GameData.Serializer.Serializer.Serialize(value.GetHealPoisonBuff(), dataPool), 
			123 => GameData.Serializer.Serializer.Serialize(value.GetHealPoisonDebuff(), dataPool), 
			124 => GameData.Serializer.Serializer.Serialize(value.GetFleeSpeed(), dataPool), 
			125 => GameData.Serializer.Serializer.Serialize(value.GetMaxFlawCount(), dataPool), 
			126 => GameData.Serializer.Serializer.Serialize(value.GetCanAddFlaw(), dataPool), 
			127 => GameData.Serializer.Serializer.Serialize(value.GetFlawLevel(), dataPool), 
			128 => GameData.Serializer.Serializer.Serialize(value.GetFlawLevelCanReduce(), dataPool), 
			129 => GameData.Serializer.Serializer.Serialize(value.GetFlawCount(), dataPool), 
			130 => GameData.Serializer.Serializer.Serialize(value.GetMaxAcupointCount(), dataPool), 
			131 => GameData.Serializer.Serializer.Serialize(value.GetCanAddAcupoint(), dataPool), 
			132 => GameData.Serializer.Serializer.Serialize(value.GetAcupointLevel(), dataPool), 
			133 => GameData.Serializer.Serializer.Serialize(value.GetAcupointLevelCanReduce(), dataPool), 
			134 => GameData.Serializer.Serializer.Serialize(value.GetAcupointCount(), dataPool), 
			135 => GameData.Serializer.Serializer.Serialize(value.GetAddNeiliAllocation(), dataPool), 
			136 => GameData.Serializer.Serializer.Serialize(value.GetCostNeiliAllocation(), dataPool), 
			137 => GameData.Serializer.Serializer.Serialize(value.GetCanChangeNeiliAllocation(), dataPool), 
			138 => GameData.Serializer.Serializer.Serialize(value.GetCanGetTrick(), dataPool), 
			139 => GameData.Serializer.Serializer.Serialize(value.GetGetTrickType(), dataPool), 
			140 => GameData.Serializer.Serializer.Serialize(value.GetAttackBodyPart(), dataPool), 
			141 => GameData.Serializer.Serializer.Serialize(value.GetWeaponEquipAttack(), dataPool), 
			142 => GameData.Serializer.Serializer.Serialize(value.GetWeaponEquipDefense(), dataPool), 
			143 => GameData.Serializer.Serializer.Serialize(value.GetArmorEquipAttack(), dataPool), 
			144 => GameData.Serializer.Serializer.Serialize(value.GetArmorEquipDefense(), dataPool), 
			145 => GameData.Serializer.Serializer.Serialize(value.GetAttackRangeForward(), dataPool), 
			146 => GameData.Serializer.Serializer.Serialize(value.GetAttackRangeBackward(), dataPool), 
			147 => GameData.Serializer.Serializer.Serialize(value.GetMoveCanBeStopped(), dataPool), 
			148 => GameData.Serializer.Serializer.Serialize(value.GetCanForcedMove(), dataPool), 
			149 => GameData.Serializer.Serializer.Serialize(value.GetMobilityCanBeRemoved(), dataPool), 
			150 => GameData.Serializer.Serializer.Serialize(value.GetMobilityCostByEffect(), dataPool), 
			151 => GameData.Serializer.Serializer.Serialize(value.GetMoveDistance(), dataPool), 
			152 => GameData.Serializer.Serializer.Serialize(value.GetJumpPrepareFrame(), dataPool), 
			153 => GameData.Serializer.Serializer.Serialize(value.GetBounceInjuryMark(), dataPool), 
			154 => GameData.Serializer.Serializer.Serialize(value.GetSkillHasCost(), dataPool), 
			155 => GameData.Serializer.Serializer.Serialize(value.GetCombatStateEffect(), dataPool), 
			156 => GameData.Serializer.Serializer.Serialize(value.GetChangeNeedUseSkill(), dataPool), 
			157 => GameData.Serializer.Serializer.Serialize(value.GetChangeDistanceIsMove(), dataPool), 
			158 => GameData.Serializer.Serializer.Serialize(value.GetReplaceCharHit(), dataPool), 
			159 => GameData.Serializer.Serializer.Serialize(value.GetCanAddPoison(), dataPool), 
			160 => GameData.Serializer.Serializer.Serialize(value.GetCanReducePoison(), dataPool), 
			161 => GameData.Serializer.Serializer.Serialize(value.GetReducePoisonValue(), dataPool), 
			162 => GameData.Serializer.Serializer.Serialize(value.GetPoisonCanAffect(), dataPool), 
			163 => GameData.Serializer.Serializer.Serialize(value.GetPoisonAffectCount(), dataPool), 
			164 => GameData.Serializer.Serializer.Serialize(value.GetCostTricks(), dataPool), 
			165 => GameData.Serializer.Serializer.Serialize(value.GetJumpMoveDistance(), dataPool), 
			166 => GameData.Serializer.Serializer.Serialize(value.GetCombatStateToAdd(), dataPool), 
			167 => GameData.Serializer.Serializer.Serialize(value.GetCombatStatePower(), dataPool), 
			168 => GameData.Serializer.Serializer.Serialize(value.GetBreakBodyPartInjuryCount(), dataPool), 
			169 => GameData.Serializer.Serializer.Serialize(value.GetBodyPartIsBroken(), dataPool), 
			170 => GameData.Serializer.Serializer.Serialize(value.GetMaxTrickCount(), dataPool), 
			171 => GameData.Serializer.Serializer.Serialize(value.GetMaxBreathPercent(), dataPool), 
			172 => GameData.Serializer.Serializer.Serialize(value.GetMaxStancePercent(), dataPool), 
			173 => GameData.Serializer.Serializer.Serialize(value.GetExtraBreathPercent(), dataPool), 
			174 => GameData.Serializer.Serializer.Serialize(value.GetExtraStancePercent(), dataPool), 
			175 => GameData.Serializer.Serializer.Serialize(value.GetMoveCostMobility(), dataPool), 
			176 => GameData.Serializer.Serializer.Serialize(value.GetDefendSkillKeepTime(), dataPool), 
			177 => GameData.Serializer.Serializer.Serialize(value.GetBounceRange(), dataPool), 
			178 => GameData.Serializer.Serializer.Serialize(value.GetMindMarkKeepTime(), dataPool), 
			179 => GameData.Serializer.Serializer.Serialize(value.GetSkillMobilityCostPerFrame(), dataPool), 
			180 => GameData.Serializer.Serializer.Serialize(value.GetCanAddWug(), dataPool), 
			181 => GameData.Serializer.Serializer.Serialize(value.GetHasGodWeaponBuff(), dataPool), 
			182 => GameData.Serializer.Serializer.Serialize(value.GetHasGodArmorBuff(), dataPool), 
			183 => GameData.Serializer.Serializer.Serialize(value.GetTeammateCmdRequireGenerateValue(), dataPool), 
			184 => GameData.Serializer.Serializer.Serialize(value.GetTeammateCmdEffect(), dataPool), 
			185 => GameData.Serializer.Serializer.Serialize(value.GetFlawRecoverSpeed(), dataPool), 
			186 => GameData.Serializer.Serializer.Serialize(value.GetAcupointRecoverSpeed(), dataPool), 
			187 => GameData.Serializer.Serializer.Serialize(value.GetMindMarkRecoverSpeed(), dataPool), 
			188 => GameData.Serializer.Serializer.Serialize(value.GetInjuryAutoHealSpeed(), dataPool), 
			189 => GameData.Serializer.Serializer.Serialize(value.GetCanRecoverBreath(), dataPool), 
			190 => GameData.Serializer.Serializer.Serialize(value.GetCanRecoverStance(), dataPool), 
			191 => GameData.Serializer.Serializer.Serialize(value.GetFatalDamageValue(), dataPool), 
			192 => GameData.Serializer.Serializer.Serialize(value.GetFatalDamageMarkCount(), dataPool), 
			193 => GameData.Serializer.Serializer.Serialize(value.GetCanFightBackDuringPrepareSkill(), dataPool), 
			194 => GameData.Serializer.Serializer.Serialize(value.GetSkillPrepareSpeed(), dataPool), 
			195 => GameData.Serializer.Serializer.Serialize(value.GetBreathRecoverSpeed(), dataPool), 
			196 => GameData.Serializer.Serializer.Serialize(value.GetStanceRecoverSpeed(), dataPool), 
			197 => GameData.Serializer.Serializer.Serialize(value.GetMobilityRecoverSpeed(), dataPool), 
			198 => GameData.Serializer.Serializer.Serialize(value.GetChangeTrickProgressAddValue(), dataPool), 
			199 => GameData.Serializer.Serializer.Serialize(value.GetPower(), dataPool), 
			200 => GameData.Serializer.Serializer.Serialize(value.GetMaxPower(), dataPool), 
			201 => GameData.Serializer.Serializer.Serialize(value.GetPowerCanReduce(), dataPool), 
			202 => GameData.Serializer.Serializer.Serialize(value.GetUseRequirement(), dataPool), 
			203 => GameData.Serializer.Serializer.Serialize(value.GetCurrInnerRatio(), dataPool), 
			204 => GameData.Serializer.Serializer.Serialize(value.GetCostBreathAndStance(), dataPool), 
			205 => GameData.Serializer.Serializer.Serialize(value.GetCostBreath(), dataPool), 
			206 => GameData.Serializer.Serializer.Serialize(value.GetCostStance(), dataPool), 
			207 => GameData.Serializer.Serializer.Serialize(value.GetCostMobility(), dataPool), 
			208 => GameData.Serializer.Serializer.Serialize(value.GetSkillCostTricks(), dataPool), 
			209 => GameData.Serializer.Serializer.Serialize(value.GetEffectDirection(), dataPool), 
			210 => GameData.Serializer.Serializer.Serialize(value.GetEffectDirectionCanChange(), dataPool), 
			211 => GameData.Serializer.Serializer.Serialize(value.GetGridCost(), dataPool), 
			212 => GameData.Serializer.Serializer.Serialize(value.GetPrepareTotalProgress(), dataPool), 
			213 => GameData.Serializer.Serializer.Serialize(value.GetSpecificGridCount(), dataPool), 
			214 => GameData.Serializer.Serializer.Serialize(value.GetGenericGridCount(), dataPool), 
			215 => GameData.Serializer.Serializer.Serialize(value.GetCanInterrupt(), dataPool), 
			216 => GameData.Serializer.Serializer.Serialize(value.GetInterruptOdds(), dataPool), 
			217 => GameData.Serializer.Serializer.Serialize(value.GetCanSilence(), dataPool), 
			218 => GameData.Serializer.Serializer.Serialize(value.GetSilenceOdds(), dataPool), 
			219 => GameData.Serializer.Serializer.Serialize(value.GetCanCastWithBrokenBodyPart(), dataPool), 
			220 => GameData.Serializer.Serializer.Serialize(value.GetAddPowerCanBeRemoved(), dataPool), 
			221 => GameData.Serializer.Serializer.Serialize(value.GetSkillType(), dataPool), 
			222 => GameData.Serializer.Serializer.Serialize(value.GetEffectCountCanChange(), dataPool), 
			223 => GameData.Serializer.Serializer.Serialize(value.GetCanCastInDefend(), dataPool), 
			224 => GameData.Serializer.Serializer.Serialize(value.GetHitDistribution(), dataPool), 
			225 => GameData.Serializer.Serializer.Serialize(value.GetCanCastOnLackBreath(), dataPool), 
			226 => GameData.Serializer.Serializer.Serialize(value.GetCanCastOnLackStance(), dataPool), 
			227 => GameData.Serializer.Serializer.Serialize(value.GetCostBreathOnCast(), dataPool), 
			228 => GameData.Serializer.Serializer.Serialize(value.GetCostStanceOnCast(), dataPool), 
			229 => GameData.Serializer.Serializer.Serialize(value.GetCanUseMobilityAsBreath(), dataPool), 
			230 => GameData.Serializer.Serializer.Serialize(value.GetCanUseMobilityAsStance(), dataPool), 
			231 => GameData.Serializer.Serializer.Serialize(value.GetCastCostNeiliAllocation(), dataPool), 
			232 => GameData.Serializer.Serializer.Serialize(value.GetAcceptPoisonResist(), dataPool), 
			233 => GameData.Serializer.Serializer.Serialize(value.GetMakePoisonResist(), dataPool), 
			234 => GameData.Serializer.Serializer.Serialize(value.GetCanCriticalHit(), dataPool), 
			235 => GameData.Serializer.Serializer.Serialize(value.GetCanCostNeiliAllocationEffect(), dataPool), 
			236 => GameData.Serializer.Serializer.Serialize(value.GetConsummateLevelRelatedMainAttributesHitValues(), dataPool), 
			237 => GameData.Serializer.Serializer.Serialize(value.GetConsummateLevelRelatedMainAttributesAvoidValues(), dataPool), 
			238 => GameData.Serializer.Serializer.Serialize(value.GetConsummateLevelRelatedMainAttributesPenetrations(), dataPool), 
			239 => GameData.Serializer.Serializer.Serialize(value.GetConsummateLevelRelatedMainAttributesPenetrationResists(), dataPool), 
			240 => GameData.Serializer.Serializer.Serialize(value.GetSkillAlsoAsFiveElements(), dataPool), 
			241 => GameData.Serializer.Serializer.Serialize(value.GetInnerInjuryImmunity(), dataPool), 
			242 => GameData.Serializer.Serializer.Serialize(value.GetOuterInjuryImmunity(), dataPool), 
			243 => GameData.Serializer.Serializer.Serialize(value.GetPoisonAffectThreshold(), dataPool), 
			244 => GameData.Serializer.Serializer.Serialize(value.GetLockDistance(), dataPool), 
			245 => GameData.Serializer.Serializer.Serialize(value.GetResistOfAllPoison(), dataPool), 
			246 => GameData.Serializer.Serializer.Serialize(value.GetMakePoisonTarget(), dataPool), 
			247 => GameData.Serializer.Serializer.Serialize(value.GetAcceptPoisonTarget(), dataPool), 
			248 => GameData.Serializer.Serializer.Serialize(value.GetCertainCriticalHit(), dataPool), 
			249 => GameData.Serializer.Serializer.Serialize(value.GetMindMarkCount(), dataPool), 
			250 => GameData.Serializer.Serializer.Serialize(value.GetCanFightBackWithHit(), dataPool), 
			251 => GameData.Serializer.Serializer.Serialize(value.GetInevitableHit(), dataPool), 
			252 => GameData.Serializer.Serializer.Serialize(value.GetAttackCanPursue(), dataPool), 
			253 => GameData.Serializer.Serializer.Serialize(value.GetCombatSkillDataEffectList(), dataPool), 
			254 => GameData.Serializer.Serializer.Serialize(value.GetCriticalOdds(), dataPool), 
			255 => GameData.Serializer.Serializer.Serialize(value.GetStanceCostByEffect(), dataPool), 
			256 => GameData.Serializer.Serializer.Serialize(value.GetBreathCostByEffect(), dataPool), 
			257 => GameData.Serializer.Serializer.Serialize(value.GetPowerAddRatio(), dataPool), 
			258 => GameData.Serializer.Serializer.Serialize(value.GetPowerReduceRatio(), dataPool), 
			259 => GameData.Serializer.Serializer.Serialize(value.GetPoisonAffectProduceValue(), dataPool), 
			260 => GameData.Serializer.Serializer.Serialize(value.GetCanReadingOnMonthChange(), dataPool), 
			261 => GameData.Serializer.Serializer.Serialize(value.GetMedicineEffect(), dataPool), 
			262 => GameData.Serializer.Serializer.Serialize(value.GetXiangshuInfectionDelta(), dataPool), 
			263 => GameData.Serializer.Serializer.Serialize(value.GetHealthDelta(), dataPool), 
			264 => GameData.Serializer.Serializer.Serialize(value.GetWeaponSilenceFrame(), dataPool), 
			265 => GameData.Serializer.Serializer.Serialize(value.GetSilenceFrame(), dataPool), 
			266 => GameData.Serializer.Serializer.Serialize(value.GetCurrAgeDelta(), dataPool), 
			267 => GameData.Serializer.Serializer.Serialize(value.GetGoneMadInAllBreak(), dataPool), 
			268 => GameData.Serializer.Serializer.Serialize(value.GetMakeLoveRateOnMonthChange(), dataPool), 
			269 => GameData.Serializer.Serializer.Serialize(value.GetCanAutoHealOnMonthChange(), dataPool), 
			270 => GameData.Serializer.Serializer.Serialize(value.GetHappinessDelta(), dataPool), 
			271 => GameData.Serializer.Serializer.Serialize(value.GetTeammateCmdCanUse(), dataPool), 
			272 => GameData.Serializer.Serializer.Serialize(value.GetMixPoisonInfinityAffect(), dataPool), 
			273 => GameData.Serializer.Serializer.Serialize(value.GetAttackRangeMaxAcupoint(), dataPool), 
			274 => GameData.Serializer.Serializer.Serialize(value.GetMaxMobilityPercent(), dataPool), 
			275 => GameData.Serializer.Serializer.Serialize(value.GetMakeMindDamage(), dataPool), 
			276 => GameData.Serializer.Serializer.Serialize(value.GetAcceptMindDamage(), dataPool), 
			277 => GameData.Serializer.Serializer.Serialize(value.GetHitAddByTempValue(), dataPool), 
			278 => GameData.Serializer.Serializer.Serialize(value.GetAvoidAddByTempValue(), dataPool), 
			279 => GameData.Serializer.Serializer.Serialize(value.GetIgnoreEquipmentOverload(), dataPool), 
			280 => GameData.Serializer.Serializer.Serialize(value.GetCanCostEnemyUsableTricks(), dataPool), 
			281 => GameData.Serializer.Serializer.Serialize(value.GetIgnoreArmor(), dataPool), 
			282 => GameData.Serializer.Serializer.Serialize(value.GetUnyieldingFallen(), dataPool), 
			283 => GameData.Serializer.Serializer.Serialize(value.GetNormalAttackPrepareFrame(), dataPool), 
			284 => GameData.Serializer.Serializer.Serialize(value.GetCanCostUselessTricks(), dataPool), 
			285 => GameData.Serializer.Serializer.Serialize(value.GetDefendSkillCanAffect(), dataPool), 
			286 => GameData.Serializer.Serializer.Serialize(value.GetAssistSkillCanAffect(), dataPool), 
			287 => GameData.Serializer.Serializer.Serialize(value.GetAgileSkillCanAffect(), dataPool), 
			288 => GameData.Serializer.Serializer.Serialize(value.GetAllMarkChangeToMind(), dataPool), 
			289 => GameData.Serializer.Serializer.Serialize(value.GetMindMarkChangeToFatal(), dataPool), 
			290 => GameData.Serializer.Serializer.Serialize(value.GetCanCast(), dataPool), 
			291 => GameData.Serializer.Serializer.Serialize(value.GetInevitableAvoid(), dataPool), 
			292 => GameData.Serializer.Serializer.Serialize(value.GetPowerEffectReverse(), dataPool), 
			293 => GameData.Serializer.Serializer.Serialize(value.GetFeatureBonusReverse(), dataPool), 
			294 => GameData.Serializer.Serializer.Serialize(value.GetWugFatalDamageValue(), dataPool), 
			295 => GameData.Serializer.Serializer.Serialize(value.GetCanRecoverHealthOnMonthChange(), dataPool), 
			296 => GameData.Serializer.Serializer.Serialize(value.GetTakeRevengeRateOnMonthChange(), dataPool), 
			297 => GameData.Serializer.Serializer.Serialize(value.GetConsummateLevelBonus(), dataPool), 
			298 => GameData.Serializer.Serializer.Serialize(value.GetNeiliDelta(), dataPool), 
			299 => GameData.Serializer.Serializer.Serialize(value.GetCanMakeLoveSpecialOnMonthChange(), dataPool), 
			300 => GameData.Serializer.Serializer.Serialize(value.GetHealAcupointSpeed(), dataPool), 
			301 => GameData.Serializer.Serializer.Serialize(value.GetMaxChangeTrickCount(), dataPool), 
			302 => GameData.Serializer.Serializer.Serialize(value.GetConvertCostBreathAndStance(), dataPool), 
			303 => GameData.Serializer.Serializer.Serialize(value.GetPersonalitiesAll(), dataPool), 
			304 => GameData.Serializer.Serializer.Serialize(value.GetFinalFatalDamageMarkCount(), dataPool), 
			305 => GameData.Serializer.Serializer.Serialize(value.GetInfinityMindMarkProgress(), dataPool), 
			306 => GameData.Serializer.Serializer.Serialize(value.GetCombatSkillAiScorePower(), dataPool), 
			307 => GameData.Serializer.Serializer.Serialize(value.GetNormalAttackChangeToUnlockAttack(), dataPool), 
			308 => GameData.Serializer.Serializer.Serialize(value.GetAttackBodyPartOdds(), dataPool), 
			309 => GameData.Serializer.Serializer.Serialize(value.GetChangeDurability(), dataPool), 
			310 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentBonus(), dataPool), 
			311 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentWeight(), dataPool), 
			312 => GameData.Serializer.Serializer.Serialize(value.GetRawCreateEffectList(), dataPool), 
			313 => GameData.Serializer.Serializer.Serialize(value.GetJiTrickAsWeaponTrickCount(), dataPool), 
			314 => GameData.Serializer.Serializer.Serialize(value.GetUselessTrickAsJiTrickCount(), dataPool), 
			315 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentPower(), dataPool), 
			316 => GameData.Serializer.Serializer.Serialize(value.GetHealFlawSpeed(), dataPool), 
			317 => GameData.Serializer.Serializer.Serialize(value.GetUnlockSpeed(), dataPool), 
			318 => GameData.Serializer.Serializer.Serialize(value.GetFlawBonusFactor(), dataPool), 
			319 => GameData.Serializer.Serializer.Serialize(value.GetCanCostShaTricks(), dataPool), 
			320 => GameData.Serializer.Serializer.Serialize(value.GetDefenderDirectFinalDamageValue(), dataPool), 
			321 => GameData.Serializer.Serializer.Serialize(value.GetNormalAttackRecoveryFrame(), dataPool), 
			322 => GameData.Serializer.Serializer.Serialize(value.GetFinalGoneMadInjury(), dataPool), 
			323 => GameData.Serializer.Serializer.Serialize(value.GetAttackerDirectFinalDamageValue(), dataPool), 
			324 => GameData.Serializer.Serializer.Serialize(value.GetCanCostTrickDuringPreparingSkill(), dataPool), 
			325 => GameData.Serializer.Serializer.Serialize(value.GetValidItemList(), dataPool), 
			326 => GameData.Serializer.Serializer.Serialize(value.GetAcceptDamageCanAdd(), dataPool), 
			327 => GameData.Serializer.Serializer.Serialize(value.GetMakeDamageCanReduce(), dataPool), 
			328 => GameData.Serializer.Serializer.Serialize(value.GetNormalAttackGetTrickCount(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_AffectedDatas(int objectId, ushort fieldId)
	{
		if (_affectedDatas.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 329)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesAffectedDatas.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesAffectedDatas.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_AffectedDatas(int objectId, ushort fieldId)
	{
		if (!_affectedDatas.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 329)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesAffectedDatas.IsModified(value.DataStatesOffset, fieldId);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		foreach (KeyValuePair<long, SpecialEffectWrapper> item in _effectDict)
		{
			long key = item.Key;
			SpecialEffectWrapper value = item.Value;
			if (value != null)
			{
				int serializedSize = value.GetSerializedSize();
				byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(17, 0, key, serializedSize);
				ptr += value.Serialize(ptr);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(17, 0, key, 0);
			}
		}
		byte* ptr2 = OperationAdder.FixedSingleValue_Set(17, 1, 8);
		*(long*)ptr2 = _nextEffectId;
		ptr2 += 8;
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(17, 0));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(17, 1));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 1:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 2:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 2:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
	{
		int argsOffset = operation.ArgsOffset;
		switch (operation.MethodId)
		{
		case 0:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 2)
			{
				int item7 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				short item8 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				List<CastBoostEffectDisplayData> allCostNeiliEffectData = GetAllCostNeiliEffectData(item7, item8);
				return GameData.Serializer.Serializer.Serialize(allCostNeiliEffectData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 3)
			{
				int item4 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				short item5 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				short item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				CostNeiliEffect(context, item4, item5, item6);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 2)
			{
				int item9 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				short item10 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				bool item11 = CanCostTrickDuringPreparingSkill(item9, item10);
				return GameData.Serializer.Serializer.Serialize(item11, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 2)
			{
				int item = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				int item2 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				bool item3 = CostTrickDuringPreparingSkill(context, item, item2);
				return GameData.Serializer.Serializer.Serialize(item3, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
	{
		switch (dataId)
		{
		case 0:
			return;
		case 1:
			return;
		case 2:
			return;
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 2:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 2:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to verify modification state of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to verify modification state of dataId {dataId}");
		case 2:
			throw new Exception($"Not allow to verify modification state of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		case 2:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _affectedDatas, list))
				{
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						BaseGameDataObject baseGameDataObject = list[i];
						List<DataUid> targetUids = influence.TargetUids;
						int count2 = targetUids.Count;
						for (int j = 0; j < count2; j++)
						{
							baseGameDataObject.InvalidateSelfAndInfluencedCache((ushort)targetUids[j].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesAffectedDatas, _dataStatesAffectedDatas, influence, context);
				}
				list.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesAffectedDatas, _dataStatesAffectedDatas, influence, context);
			}
			break;
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 0:
		case 1:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		uint num;
		switch (operation.DataId)
		{
		case 0:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _effectDict);
			goto IL_0077;
		case 1:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _nextEffectId);
			goto IL_0077;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 2:
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_0077:
			if (_pendingLoadingOperationIds == null)
			{
				break;
			}
			num = _pendingLoadingOperationIds.Peek();
			if (num == operation.Id)
			{
				_pendingLoadingOperationIds.Dequeue();
				if (_pendingLoadingOperationIds.Count <= 0)
				{
					_pendingLoadingOperationIds = null;
					InitializeInternalDataOfCollections();
					OnLoadedArchiveData();
					DomainManager.Global.CompleteLoading(17);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
		foreach (KeyValuePair<int, AffectedData> affectedData in _affectedDatas)
		{
			AffectedData value = affectedData.Value;
			value.CollectionHelperData = HelperDataAffectedDatas;
			value.DataStatesOffset = _dataStatesAffectedDatas.Create();
		}
	}

	public static void RegisterResetHandler(Action action)
	{
		ResetOnInitializeGameDataModuleEffects.Add(action);
	}

	private static void InvokeResetHandlers()
	{
		foreach (Action resetOnInitializeGameDataModuleEffect in ResetOnInitializeGameDataModuleEffects)
		{
			resetOnInitializeGameDataModuleEffect();
		}
	}
}
