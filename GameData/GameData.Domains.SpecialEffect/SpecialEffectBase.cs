using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect;

public class SpecialEffectBase : ISpecialEffectModifier, IFrameCounterHandler
{
	public long Id;

	public int Type;

	public Dictionary<AffectedDataKey, EDataModifyType> AffectDatas;

	public GameData.Domains.Character.Character CharObj;

	private readonly List<FrameCounter> _frameCounters = new List<FrameCounter>();

	private List<DataUid> _listeningDataUids;

	public static SpecialEffectBase Invalid => new SpecialEffectBase();

	public int CharacterId { get; set; }

	public CombatCharacter CombatChar => DomainManager.Combat.GetElement_CombatCharacterDict(CharacterId);

	public CombatCharacter EnemyChar => DomainManager.Combat.GetCombatCharacter(!CombatChar.IsAlly);

	public CombatCharacter CurrEnemyChar => DomainManager.Combat.GetCombatCharacter(!CombatChar.IsAlly, tryGetCoverCharacter: true);

	public string DataHandlerKey => $"{GetType().Name}{Id}";

	protected bool IsCurrent => DomainManager.Combat.IsCurrentCombatCharacter(CombatChar);

	protected bool IsEntering => DomainManager.Combat.GetMainCharacter(CombatChar.IsAlly).TeammateBeforeMainChar == CharacterId || DomainManager.Combat.GetMainCharacter(CombatChar.IsAlly).TeammateAfterMainChar == CharacterId;

	protected SpecialEffectBase()
	{
		CreateFrameCounters();
	}

	protected SpecialEffectBase(int characterId, int type)
	{
		CharacterId = characterId;
		Type = type;
		CharObj = DomainManager.Character.GetElement_Objects(CharacterId);
		CreateFrameCounters();
	}

	public int GetSerializedSize()
	{
		int num = 16 + GetSubClassSerializedSize();
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(long*)ptr = Id;
		ptr += 8;
		*(int*)ptr = CharacterId;
		ptr += 4;
		*(int*)ptr = Type;
		ptr += 4;
		ptr += SerializeSubClass(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(long*)ptr;
		ptr += 8;
		CharacterId = *(int*)ptr;
		ptr += 4;
		Type = *(int*)ptr;
		ptr += 4;
		DomainManager.Character.TryGetElement_Objects(CharacterId, out CharObj);
		ptr += DeserializeSubClass(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	protected virtual int GetSubClassSerializedSize()
	{
		return 0;
	}

	protected unsafe virtual int SerializeSubClass(byte* pData)
	{
		return 0;
	}

	protected unsafe virtual int DeserializeSubClass(byte* pData)
	{
		return 0;
	}

	public virtual void OnEnable(DataContext context)
	{
		SetupFrameCounters();
	}

	public virtual void OnDataAdded(DataContext context)
	{
	}

	public virtual void OnDisable(DataContext context)
	{
		CloseFrameCounters();
		ClearMonitors();
	}

	public void OnDreamBack(DataContext context)
	{
		OnDisable(context);
		AffectDatas = null;
	}

	public void CreateAffectedData(ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		CreateAffectedData(CharacterId, fieldId, modifyType, combatSkillId);
	}

	public void CreateAffectedData(int charId, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		AffectedDataKey affectedDataKey = new AffectedDataKey(charId, fieldId, combatSkillId);
		if (AffectDatas == null)
		{
			AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		}
		if (!AffectDatas.TryAdd(affectedDataKey, modifyType))
		{
			AdaptableLog.Warning($"{CharObj} overwrite {affectedDataKey} from {AffectDatas[affectedDataKey]} to {modifyType}");
			AffectDatas[affectedDataKey] = modifyType;
		}
	}

	public void CreateAffectedAllEnemyData(ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		int[] characterList = DomainManager.Combat.GetCharacterList(!CombatChar.IsAlly);
		for (int i = 0; i < characterList.Length; i++)
		{
			if (characterList[i] >= 0)
			{
				CreateAffectedData(characterList[i], fieldId, modifyType, combatSkillId);
			}
		}
	}

	public void CreateAffectedAllCombatCharData(ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		DomainManager.Combat.GetAllCharInCombat(list);
		for (int i = 0; i < list.Count; i++)
		{
			CreateAffectedData(list[i], fieldId, modifyType, combatSkillId);
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	protected void AppendAffectedData(DataContext context, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		AppendAffectedData(context, CharacterId, fieldId, modifyType, combatSkillId);
	}

	protected void AppendAffectedData(DataContext context, int charId, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		AffectedDataKey affectedDataKey = new AffectedDataKey(charId, fieldId, combatSkillId);
		if (AffectDatas == null)
		{
			AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		}
		AffectDatas.Add(affectedDataKey, modifyType);
		DomainManager.SpecialEffect.AppendDataUid(context, this, affectedDataKey);
	}

	protected void AppendAffectedCurrEnemyData(DataContext context, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		AppendAffectedData(context, CurrEnemyChar.GetId(), fieldId, modifyType, combatSkillId);
	}

	protected void AppendAffectedAllEnemyData(DataContext context, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		int[] characterList = DomainManager.Combat.GetCharacterList(!CombatChar.IsAlly);
		for (int i = 0; i < characterList.Length; i++)
		{
			if (characterList[i] >= 0)
			{
				AppendAffectedData(context, characterList[i], fieldId, modifyType, combatSkillId);
			}
		}
	}

	protected void AppendAffectedAllCombatCharData(DataContext context, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		DomainManager.Combat.GetAllCharInCombat(list);
		for (int i = 0; i < list.Count; i++)
		{
			AppendAffectedData(context, list[i], fieldId, modifyType, combatSkillId);
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	protected void RemoveAffectedData(DataContext context, int charId, ushort fieldId)
	{
		AffectedDataKey affectedDataKey = new AffectedDataKey(charId, fieldId, -1);
		DomainManager.SpecialEffect.RemoveDataUid(context, this, affectedDataKey);
		AffectDatas?.Remove(affectedDataKey);
	}

	protected void ClearAffectedData(DataContext context)
	{
		DomainManager.SpecialEffect.RemoveDataUid(context, this);
		AffectDatas?.Clear();
	}

	public virtual int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		return 0;
	}

	protected void InvalidateCache(DataContext context, ushort fieldId)
	{
		InvalidateCache(context, CharacterId, fieldId);
	}

	protected void InvalidateAllEnemyCache(DataContext context, ushort fieldId)
	{
		int[] characterList = DomainManager.Combat.GetCharacterList(!CombatChar.IsAlly);
		foreach (int num in characterList)
		{
			if (num >= 0)
			{
				InvalidateCache(context, num, fieldId);
			}
		}
	}

	protected void InvalidateCache(DataContext context, int charId, ushort fieldId)
	{
		DomainManager.SpecialEffect.InvalidateCache(context, charId, fieldId);
	}

	public virtual bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		return dataValue;
	}

	public virtual int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		return dataValue;
	}

	public virtual long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		return dataValue;
	}

	public virtual HitOrAvoidInts GetModifiedValue(AffectedDataKey dataKey, HitOrAvoidInts dataValue)
	{
		return dataValue;
	}

	public virtual NeiliProportionOfFiveElements GetModifiedValue(AffectedDataKey dataKey, NeiliProportionOfFiveElements dataValue)
	{
		return dataValue;
	}

	public virtual OuterAndInnerInts GetModifiedValue(AffectedDataKey dataKey, OuterAndInnerInts dataValue)
	{
		return dataValue;
	}

	public virtual List<NeedTrick> GetModifiedValue(AffectedDataKey dataKey, List<NeedTrick> dataValue)
	{
		return dataValue;
	}

	public virtual (sbyte, sbyte) GetModifiedValue(AffectedDataKey dataKey, (sbyte, sbyte) dataValue)
	{
		return dataValue;
	}

	public virtual List<ItemKeyAndCount> GetModifiedValue(AffectedDataKey dataKey, List<ItemKeyAndCount> dataValue)
	{
		return dataValue;
	}

	public virtual List<CastBoostEffectDisplayData> GetModifiedValue(AffectedDataKey dataKey, List<CastBoostEffectDisplayData> dataValue)
	{
		return dataValue;
	}

	public virtual List<CombatSkillEffectData> GetModifiedValue(AffectedDataKey dataKey, List<CombatSkillEffectData> dataValue)
	{
		return dataValue;
	}

	public virtual BoolArray8 GetModifiedValue(AffectedDataKey dataKey, BoolArray8 dataValue)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return dataValue;
	}

	public virtual CombatCharacter GetModifiedValue(AffectedDataKey dataKey, CombatCharacter dataValue)
	{
		return dataValue;
	}

	public virtual List<int> GetModifiedValue(AffectedDataKey dataKey, List<int> dataValue)
	{
		return dataValue;
	}

	public bool FiveElementsEquals(short skillId, sbyte fiveElement)
	{
		return CombatSkillDomain.FiveElementEquals(CharacterId, skillId, fiveElement);
	}

	public bool FiveElementsEquals(AffectedDataKey dataKey, sbyte fiveElement)
	{
		return CombatSkillDomain.FiveElementEquals(dataKey.CharId, dataKey.CombatSkillId, fiveElement);
	}

	public bool FiveElementsEquals(CombatSkillKey skillKey, sbyte fiveElement)
	{
		return CombatSkillDomain.FiveElementEquals(skillKey.CharId, skillKey.SkillTemplateId, fiveElement);
	}

	public bool FiveElementsEqualsEnemy(short skillId, sbyte fiveElement)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!CombatChar.IsAlly);
		return CombatSkillDomain.FiveElementEquals(combatCharacter.GetId(), skillId, fiveElement);
	}

	protected DataUid ParseCombatCharacterDataUid(ushort fieldId)
	{
		return ParseCombatCharacterDataUid(CharacterId, fieldId);
	}

	protected DataUid ParseCombatCharacterDataUid(int charId, ushort fieldId)
	{
		return new DataUid(8, 10, (ulong)charId, fieldId);
	}

	protected DataUid ParseCharDataUid(ushort fieldId)
	{
		return ParseCharDataUid(CharacterId, fieldId);
	}

	protected DataUid ParseCharDataUid(int charId, ushort fieldId)
	{
		return new DataUid(4, 0, (ulong)charId, fieldId);
	}

	protected DataUid ParseNeiliAllocationDataUid()
	{
		return ParseNeiliAllocationDataUid(CharacterId);
	}

	protected DataUid ParseNeiliAllocationDataUid(int charId)
	{
		return ParseCombatCharacterDataUid(charId, 3);
	}

	protected int ChangeStanceValue(DataContext context, CombatCharacter character, int addValue)
	{
		return DomainManager.Combat.ChangeStanceValue(context, character, addValue, changedByEffect: true, CombatChar);
	}

	protected void AbsorbStanceValue(DataContext context, CombatCharacter character, CValuePercent percent)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		int num = Math.Min(4000 * percent, character.GetStanceValue());
		num = ChangeStanceValue(context, character, -num);
		ChangeStanceValue(context, CombatChar, Math.Abs(num));
	}

	protected int ChangeBreathValue(DataContext context, CombatCharacter character, int addValue)
	{
		return DomainManager.Combat.ChangeBreathValue(context, character, addValue, changedByEffect: true, CombatChar);
	}

	protected void AbsorbBreathValue(DataContext context, CombatCharacter character, CValuePercent percent)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		int num = Math.Min(30000 * percent, character.GetBreathValue());
		num = ChangeBreathValue(context, character, -num);
		ChangeBreathValue(context, CombatChar, Math.Abs(num));
	}

	protected void ChangeMobilityValue(DataContext context, CombatCharacter character, int addValue)
	{
		DomainManager.Combat.ChangeMobilityValue(context, character, addValue, changedByEffect: true, CombatChar);
	}

	protected bool ClearAffectingAgileSkill(DataContext context, CombatCharacter character)
	{
		return DomainManager.Combat.ClearAffectingAgileSkillByEffect(context, character, CombatChar);
	}

	protected void ChangeDurability(DataContext context, CombatCharacter character, ItemKey itemKey, int delta)
	{
		DomainManager.Combat.ChangeDurability(context, character, itemKey, delta, EChangeDurabilitySourceType.Effect);
	}

	private void CreateFrameCounters()
	{
		int num = 0;
		foreach (int item in CalcFrameCounterPeriods())
		{
			_frameCounters.Add(new FrameCounter(this, item, num++));
		}
	}

	private void SetupFrameCounters()
	{
		foreach (FrameCounter frameCounter in _frameCounters)
		{
			frameCounter.Setup();
		}
	}

	private void CloseFrameCounters()
	{
		foreach (FrameCounter frameCounter in _frameCounters)
		{
			frameCounter.Close();
		}
	}

	protected virtual IEnumerable<int> CalcFrameCounterPeriods()
	{
		yield break;
	}

	public virtual bool IsOn(int counterType)
	{
		return true;
	}

	public virtual void OnProcess(DataContext context, int counterType)
	{
	}

	protected bool IsMonitored(DataUid uid)
	{
		return _listeningDataUids?.Contains(uid) ?? false;
	}

	protected void AutoMonitor(DataUid uid, Action<DataContext, DataUid> action)
	{
		if (!IsMonitored(uid))
		{
			if (_listeningDataUids == null)
			{
				_listeningDataUids = new List<DataUid>();
			}
			_listeningDataUids.Add(uid);
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(uid, DataHandlerKey, action);
		}
	}

	protected void InterruptMonitor(DataUid uid)
	{
		if (IsMonitored(uid))
		{
			_listeningDataUids.Remove(uid);
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(uid, DataHandlerKey);
		}
	}

	protected void ClearMonitors()
	{
		List<DataUid> listeningDataUids = _listeningDataUids;
		if (listeningDataUids == null || listeningDataUids.Count <= 0)
		{
			return;
		}
		foreach (DataUid listeningDataUid in _listeningDataUids)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(listeningDataUid, DataHandlerKey);
		}
		_listeningDataUids.Clear();
	}
}
