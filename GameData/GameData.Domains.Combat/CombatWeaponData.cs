using System;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForDisplayModule = true)]
public class CombatWeaponData : BaseGameDataObject, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 4;

		public const uint WeaponTricks_Offset = 4u;

		public const int WeaponTricks_Size = 6;

		public const uint CanChangeTo_Offset = 10u;

		public const int CanChangeTo_Size = 1;

		public const uint Durability_Offset = 11u;

		public const int Durability_Size = 2;

		public const uint CdFrame_Offset = 13u;

		public const int CdFrame_Size = 2;

		public const uint AutoAttackEffect_Offset = 15u;

		public const int AutoAttackEffect_Size = 3;

		public const uint PestleEffect_Offset = 18u;

		public const int PestleEffect_Size = 3;

		public const uint FixedCdLeftFrame_Offset = 21u;

		public const int FixedCdLeftFrame_Size = 2;

		public const uint FixedCdTotalFrame_Offset = 23u;

		public const int FixedCdTotalFrame_Size = 2;
	}

	[CollectionObjectField(false, true, false, false, false)]
	private int _id;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 6)]
	private sbyte[] _weaponTricks;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _canChangeTo;

	[CollectionObjectField(false, true, false, false, false)]
	private short _durability;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _innerRatio;

	[CollectionObjectField(false, true, false, false, false)]
	private short _cdFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private short _fixedCdLeftFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private short _fixedCdTotalFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private SkillEffectKey _autoAttackEffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SkillEffectKey _pestleEffect;

	private long _pestleEffectId;

	public const int FixedSize = 25;

	public const int DynamicCount = 0;

	public CombatCharacter Character { get; }

	public int Index { get; private set; }

	public bool NotInAnyCd => _cdFrame == 0 && _fixedCdLeftFrame == 0;

	public GameData.Domains.Item.Weapon Item => DomainManager.Item.GetElement_Weapons(_id);

	public short TemplateId => Item.GetTemplateId();

	public WeaponItem Template => Config.Weapon.Instance[TemplateId];

	[SingleValueDependency(5, new ushort[] { 27 }, Condition = InfluenceCondition.CombatWeaponIsTaiwuWeapon)]
	[SingleValueDependency(8, new ushort[] { 31 }, Condition = InfluenceCondition.CombatWeaponIsNotTaiwuWeapon)]
	private sbyte CalcInnerRatio()
	{
		if (!DomainManager.Combat.IsCharInCombat(Character.GetId()))
		{
			return _innerRatio;
		}
		if (Character.IsTaiwu)
		{
			return DomainManager.Taiwu.GetWeaponCurrInnerRatios()[Index];
		}
		WeaponExpectInnerRatioData expectRatioData = DomainManager.Combat.GetExpectRatioData();
		sbyte value = expectRatioData.GetValue(Character.GetId(), Index);
		return (value < 0) ? Template.DefaultInnerRatio : Character.GetCharacter().CalcWeaponInnerRatio(TemplateId, value);
	}

	public CombatWeaponData(ItemKey key, CombatCharacter character)
		: this()
	{
		_id = key.Id;
		Character = character;
		_pestleEffectId = -1L;
	}

	public void Init(DataContext context, int index)
	{
		Index = index;
		SetDurability(Item.GetCurrDurability(), context);
		SetCdFrame(0, context);
		SetFixedCdLeftFrame(0, context);
		SetFixedCdTotalFrame(0, context);
		SetCanChangeTo(index >= 3 || GetDurability() > 0, context);
		SetAutoAttackEffect(new SkillEffectKey(-1, isDirect: false), context);
		SetPestleEffect(new SkillEffectKey(-1, isDirect: false), context);
	}

	public void SetPestleEffect(DataContext context, int charId, string effectName, SkillEffectKey effectKey)
	{
		if (!_pestleEffect.Equals(effectKey))
		{
			if (_pestleEffect.SkillId >= 0)
			{
				RemovePestleEffect(context);
			}
			SetPestleEffect(effectKey, context);
			_pestleEffectId = DomainManager.SpecialEffect.Add(context, charId, effectName);
		}
	}

	public void RemovePestleEffect(DataContext context)
	{
		DomainManager.SpecialEffect.Remove(context, _pestleEffectId);
		SetPestleEffect(new SkillEffectKey(-1, isDirect: false), context);
		_pestleEffectId = -1L;
	}

	public int GetId()
	{
		return _id;
	}

	public unsafe void SetId(int id, DataContext context)
	{
		_id = id;
		SetModifiedAndInvalidateInfluencedCache(0, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 0u, 4);
			*(int*)ptr = _id;
			ptr += 4;
		}
	}

	public sbyte[] GetWeaponTricks()
	{
		return _weaponTricks;
	}

	public unsafe void SetWeaponTricks(sbyte[] weaponTricks, DataContext context)
	{
		_weaponTricks = weaponTricks;
		SetModifiedAndInvalidateInfluencedCache(1, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 4u, 6);
			for (int i = 0; i < 6; i++)
			{
				ptr[i] = (byte)_weaponTricks[i];
			}
			ptr += 6;
		}
	}

	public bool GetCanChangeTo()
	{
		return _canChangeTo;
	}

	public unsafe void SetCanChangeTo(bool canChangeTo, DataContext context)
	{
		_canChangeTo = canChangeTo;
		SetModifiedAndInvalidateInfluencedCache(2, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 10u, 1);
			*ptr = (_canChangeTo ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public short GetDurability()
	{
		return _durability;
	}

	public unsafe void SetDurability(short durability, DataContext context)
	{
		_durability = durability;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 11u, 2);
			*(short*)ptr = _durability;
			ptr += 2;
		}
	}

	public short GetCdFrame()
	{
		return _cdFrame;
	}

	public unsafe void SetCdFrame(short cdFrame, DataContext context)
	{
		_cdFrame = cdFrame;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 13u, 2);
			*(short*)ptr = _cdFrame;
			ptr += 2;
		}
	}

	public SkillEffectKey GetAutoAttackEffect()
	{
		return _autoAttackEffect;
	}

	public unsafe void SetAutoAttackEffect(SkillEffectKey autoAttackEffect, DataContext context)
	{
		_autoAttackEffect = autoAttackEffect;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 15u, 3);
			ptr += _autoAttackEffect.Serialize(ptr);
		}
	}

	public SkillEffectKey GetPestleEffect()
	{
		return _pestleEffect;
	}

	public unsafe void SetPestleEffect(SkillEffectKey pestleEffect, DataContext context)
	{
		_pestleEffect = pestleEffect;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 18u, 3);
			ptr += _pestleEffect.Serialize(ptr);
		}
	}

	public short GetFixedCdLeftFrame()
	{
		return _fixedCdLeftFrame;
	}

	public unsafe void SetFixedCdLeftFrame(short fixedCdLeftFrame, DataContext context)
	{
		_fixedCdLeftFrame = fixedCdLeftFrame;
		SetModifiedAndInvalidateInfluencedCache(7, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 21u, 2);
			*(short*)ptr = _fixedCdLeftFrame;
			ptr += 2;
		}
	}

	public short GetFixedCdTotalFrame()
	{
		return _fixedCdTotalFrame;
	}

	public unsafe void SetFixedCdTotalFrame(short fixedCdTotalFrame, DataContext context)
	{
		_fixedCdTotalFrame = fixedCdTotalFrame;
		SetModifiedAndInvalidateInfluencedCache(8, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 23u, 2);
			*(short*)ptr = _fixedCdTotalFrame;
			ptr += 2;
		}
	}

	public sbyte GetInnerRatio()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 9))
		{
			return _innerRatio;
		}
		_innerRatio = CalcInnerRatio();
		dataStates.SetCached(DataStatesOffset, 9);
		return _innerRatio;
	}

	public CombatWeaponData()
	{
		_weaponTricks = new sbyte[6];
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 25;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _id;
		ptr += 4;
		if (_weaponTricks.Length != 6)
		{
			throw new Exception("Elements count of field _weaponTricks is not equal to declaration");
		}
		for (int i = 0; i < 6; i++)
		{
			ptr[i] = (byte)_weaponTricks[i];
		}
		ptr += 6;
		*ptr = (_canChangeTo ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = _durability;
		ptr += 2;
		*(short*)ptr = _cdFrame;
		ptr += 2;
		ptr += _autoAttackEffect.Serialize(ptr);
		ptr += _pestleEffect.Serialize(ptr);
		*(short*)ptr = _fixedCdLeftFrame;
		ptr += 2;
		*(short*)ptr = _fixedCdTotalFrame;
		ptr += 2;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_id = *(int*)ptr;
		ptr += 4;
		if (_weaponTricks.Length != 6)
		{
			throw new Exception("Elements count of field _weaponTricks is not equal to declaration");
		}
		for (int i = 0; i < 6; i++)
		{
			_weaponTricks[i] = (sbyte)ptr[i];
		}
		ptr += 6;
		_canChangeTo = *ptr != 0;
		ptr++;
		_durability = *(short*)ptr;
		ptr += 2;
		_cdFrame = *(short*)ptr;
		ptr += 2;
		ptr += _autoAttackEffect.Deserialize(ptr);
		ptr += _pestleEffect.Deserialize(ptr);
		_fixedCdLeftFrame = *(short*)ptr;
		ptr += 2;
		_fixedCdTotalFrame = *(short*)ptr;
		ptr += 2;
		return (int)(ptr - pData);
	}
}
