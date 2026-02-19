using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Serializer;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForDisplayModule = true)]
public class CombatSkillData : BaseGameDataObject, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 8;

		public const uint CanUse_Offset = 8u;

		public const int CanUse_Size = 1;

		public const uint LeftCdFrame_Offset = 9u;

		public const int LeftCdFrame_Size = 2;

		public const uint TotalCdFrame_Offset = 11u;

		public const int TotalCdFrame_Size = 2;

		public const uint ConstAffecting_Offset = 13u;

		public const int ConstAffecting_Size = 1;

		public const uint ShowAffectTips_Offset = 14u;

		public const int ShowAffectTips_Size = 1;

		public const uint Silencing_Offset = 15u;

		public const int Silencing_Size = 1;
	}

	[CollectionObjectField(false, true, false, false, false)]
	private CombatSkillKey _id;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _canUse;

	[CollectionObjectField(false, true, false, false, false)]
	private short _leftCdFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private short _totalCdFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _silencing;

	[CollectionObjectField(false, false, true, false, false)]
	private readonly List<CombatSkillBanReasonData> _banReason = new List<CombatSkillBanReasonData>();

	[CollectionObjectField(false, false, true, false, false)]
	private readonly List<CombatSkillEffectData> _effectData = new List<CombatSkillEffectData>();

	[CollectionObjectField(false, false, true, false, false)]
	private bool _canAffect;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _constAffecting;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _showAffectTips;

	public const int FixedSize = 16;

	public const int DynamicCount = 0;

	[ObjectCollectionDependency(8, 29, new ushort[] { 1 }, Scope = InfluenceScope.Self)]
	private void CalcBanReason(List<CombatSkillBanReasonData> banReason)
	{
		banReason.Clear();
		if (_canUse)
		{
			return;
		}
		int charId = _id.CharId;
		short skillId = _id.SkillTemplateId;
		if (!DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out var combatChar) || !DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillId), out var combatSkill))
		{
			return;
		}
		CombatDomain combat = DomainManager.Combat;
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		if (!combat.SkillCanUseInCurrCombat(charId, combatSkillItem))
		{
			AddReason(ECombatSkillBanReasonType.CombatConfigBan);
			return;
		}
		if (_leftCdFrame != 0)
		{
			AddReason(ECombatSkillBanReasonType.Silencing);
			return;
		}
		if (!combat.HasSkillNeedBodyPart(combatChar, skillId))
		{
			AddReason(ECombatSkillBanReasonType.BodyPartBroken);
			return;
		}
		if (combatSkillItem.EquipType == 1 && !combat.WeaponHasNeedTrick(combatChar, skillId, combat.GetUsingWeaponData(combatChar)))
		{
			AddReason(ECombatSkillBanReasonType.WeaponTrickMismatch);
			return;
		}
		if (!combat.SkillCostEnough(combatChar, skillId))
		{
			foreach (ECombatSkillBanReasonType item in DomainManager.Combat.CalcSkillCostEnoughBanReasons(combatChar, skillId))
			{
				AddReason(item);
			}
		}
		if (banReason.Count == 0)
		{
			AddReason(ECombatSkillBanReasonType.SpecialEffectBan);
		}
		void AddReason(ECombatSkillBanReasonType banReasonType)
		{
			banReason.Add(new CombatSkillBanReasonData(banReasonType, skillId, combatSkill, combatChar));
		}
	}

	[ObjectCollectionDependency(17, 2, new ushort[] { 253 }, Scope = InfluenceScope.CombatSkillDataAffectedByTheSpecialEffects)]
	private void CalcEffectData(List<CombatSkillEffectData> effectData)
	{
		effectData.Clear();
		DomainManager.SpecialEffect.ModifyData(_id.CharId, _id.SkillTemplateId, 253, effectData);
	}

	[ObjectCollectionDependency(17, 2, new ushort[] { 287, 285, 286 }, Scope = InfluenceScope.CombatSkillDataAffectedByTheSpecialEffects)]
	private bool CalcCanAffect()
	{
		sbyte equipType = Config.CombatSkill.Instance[_id.SkillTemplateId].EquipType;
		if (1 == 0)
		{
		}
		ushort num = equipType switch
		{
			2 => 287, 
			3 => 285, 
			4 => 286, 
			_ => ushort.MaxValue, 
		};
		if (1 == 0)
		{
		}
		ushort num2 = num;
		return num2 == ushort.MaxValue || DomainManager.SpecialEffect.ModifyData(_id.CharId, _id.SkillTemplateId, num2, dataValue: true);
	}

	public CombatSkillData(CombatSkillKey id)
		: this()
	{
		_id = id;
	}

	public void RaiseSkillSilence(DataContext context)
	{
		SetSilencing(silencing: true, context);
		Events.RaiseSkillSilence(context, _id);
	}

	public void RaiseSkillSilenceEnd(DataContext context)
	{
		SetSilencing(silencing: false, context);
		Events.RaiseSkillSilenceEnd(context, _id);
	}

	public CombatSkillKey GetId()
	{
		return _id;
	}

	public unsafe void SetId(CombatSkillKey id, DataContext context)
	{
		_id = id;
		SetModifiedAndInvalidateInfluencedCache(0, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 0u, 8);
			ptr += _id.Serialize(ptr);
		}
	}

	public bool GetCanUse()
	{
		return _canUse;
	}

	public unsafe void SetCanUse(bool canUse, DataContext context)
	{
		_canUse = canUse;
		SetModifiedAndInvalidateInfluencedCache(1, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 8u, 1);
			*ptr = (_canUse ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public short GetLeftCdFrame()
	{
		return _leftCdFrame;
	}

	public unsafe void SetLeftCdFrame(short leftCdFrame, DataContext context)
	{
		_leftCdFrame = leftCdFrame;
		SetModifiedAndInvalidateInfluencedCache(2, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 9u, 2);
			*(short*)ptr = _leftCdFrame;
			ptr += 2;
		}
	}

	public short GetTotalCdFrame()
	{
		return _totalCdFrame;
	}

	public unsafe void SetTotalCdFrame(short totalCdFrame, DataContext context)
	{
		_totalCdFrame = totalCdFrame;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 11u, 2);
			*(short*)ptr = _totalCdFrame;
			ptr += 2;
		}
	}

	public bool GetConstAffecting()
	{
		return _constAffecting;
	}

	public unsafe void SetConstAffecting(bool constAffecting, DataContext context)
	{
		_constAffecting = constAffecting;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 13u, 1);
			*ptr = (_constAffecting ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public bool GetShowAffectTips()
	{
		return _showAffectTips;
	}

	public unsafe void SetShowAffectTips(bool showAffectTips, DataContext context)
	{
		_showAffectTips = showAffectTips;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 14u, 1);
			*ptr = (_showAffectTips ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public bool GetSilencing()
	{
		return _silencing;
	}

	public unsafe void SetSilencing(bool silencing, DataContext context)
	{
		_silencing = silencing;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 15u, 1);
			*ptr = (_silencing ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public List<CombatSkillBanReasonData> GetBanReason()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 7))
		{
			return _banReason;
		}
		CalcBanReason(_banReason);
		dataStates.SetCached(DataStatesOffset, 7);
		return _banReason;
	}

	public List<CombatSkillEffectData> GetEffectData()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 8))
		{
			return _effectData;
		}
		CalcEffectData(_effectData);
		dataStates.SetCached(DataStatesOffset, 8);
		return _effectData;
	}

	public bool GetCanAffect()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 9))
		{
			return _canAffect;
		}
		_canAffect = CalcCanAffect();
		dataStates.SetCached(DataStatesOffset, 9);
		return _canAffect;
	}

	public CombatSkillData()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 16;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += _id.Serialize(ptr);
		*ptr = (_canUse ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = _leftCdFrame;
		ptr += 2;
		*(short*)ptr = _totalCdFrame;
		ptr += 2;
		*ptr = (_constAffecting ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_showAffectTips ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_silencing ? ((byte)1) : ((byte)0));
		ptr++;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += _id.Deserialize(ptr);
		_canUse = *ptr != 0;
		ptr++;
		_leftCdFrame = *(short*)ptr;
		ptr += 2;
		_totalCdFrame = *(short*)ptr;
		ptr += 2;
		_constAffecting = *ptr != 0;
		ptr++;
		_showAffectTips = *ptr != 0;
		ptr++;
		_silencing = *ptr != 0;
		ptr++;
		return (int)(ptr - pData);
	}
}
