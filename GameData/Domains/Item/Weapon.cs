using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Config;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x02000673 RID: 1651
	[SerializableGameData(NotForDisplayModule = true)]
	public class Weapon : EquipmentBase, ISerializableGameData
	{
		// Token: 0x060052B3 RID: 21171 RVA: 0x002CEEB8 File Offset: 0x002CD0B8
		[ObjectCollectionDependency(6, 0, new ushort[]
		{
			6,
			8
		}, Scope = InfluenceScope.Self)]
		private short CalcPenetrationFactor()
		{
			int value = (int)this.GetBasePenetrationFactor();
			value = value * base.GetMaterialResourceBonusValuePercentage(2) / 100;
			bool flag = !ModificationStateHelper.IsActive(this.ModificationState, 2);
			short result;
			if (flag)
			{
				result = (short)value;
			}
			else
			{
				int refineBonus = DomainManager.Item.GetRefinedEffects(base.GetItemKey()).GetWeaponPropertyBonus(ERefiningEffectWeaponType.Penetration);
				refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
				value += (int)((short)refineBonus);
				result = (short)value;
			}
			return result;
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x002CEF28 File Offset: 0x002CD128
		[ObjectCollectionDependency(6, 0, new ushort[]
		{
			6,
			3,
			8
		}, Scope = InfluenceScope.Self)]
		private short CalcEquipmentAttack()
		{
			int value = (int)this.GetBaseEquipmentAttack();
			value = value * base.GetMaterialResourceBonusValuePercentage(0) / 100;
			value = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value, (EquipmentEffectItem x) => (int)x.EquipmentAttackChange);
			bool flag = !ModificationStateHelper.IsActive(this.ModificationState, 2);
			short result;
			if (flag)
			{
				result = (short)value;
			}
			else
			{
				int refineBonus = DomainManager.Item.GetRefinedEffects(base.GetItemKey()).GetWeaponPropertyBonus(ERefiningEffectWeaponType.EquipmentAttack);
				refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
				value = value * (100 + refineBonus) / 100;
				result = (short)value;
			}
			return result;
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x002CEFD0 File Offset: 0x002CD1D0
		[ObjectCollectionDependency(6, 0, new ushort[]
		{
			6,
			3,
			8
		}, Scope = InfluenceScope.Self)]
		private short CalcEquipmentDefense()
		{
			int value = (int)this.GetBaseEquipmentDefense();
			value = value * base.GetMaterialResourceBonusValuePercentage(1) / 100;
			value = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value, (EquipmentEffectItem x) => (int)x.EquipmentDefenseChange);
			bool flag = !ModificationStateHelper.IsActive(this.ModificationState, 2);
			short result;
			if (flag)
			{
				result = (short)value;
			}
			else
			{
				int refineBonus = DomainManager.Item.GetRefinedEffects(base.GetItemKey()).GetWeaponPropertyBonus(ERefiningEffectWeaponType.EquipmentDefense);
				refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
				value = value * (100 + refineBonus) / 100;
				result = (short)value;
			}
			return result;
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x002CF078 File Offset: 0x002CD278
		[ObjectCollectionDependency(6, 0, new ushort[]
		{
			6,
			3,
			8
		}, Scope = InfluenceScope.Self)]
		private int CalcWeight()
		{
			int value = this.GetBaseWeight();
			value = value * (170 - base.GetMaterialResourceBonusValuePercentage(4)) / 100;
			value = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value, (EquipmentEffectItem x) => (int)x.WeightChange);
			bool flag = !ModificationStateHelper.IsActive(this.ModificationState, 2);
			int result;
			if (flag)
			{
				result = value;
			}
			else
			{
				int refineBonus = DomainManager.Item.GetRefinedEffects(base.GetItemKey()).GetWeaponPropertyBonus(ERefiningEffectWeaponType.Weight);
				refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
				value = value * (100 + refineBonus) / 100;
				result = value;
			}
			return result;
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x002CF124 File Offset: 0x002CD324
		public Weapon(IRandomSource random, short templateId, int itemId) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
			bool randomTrick = this.GetRandomTrick();
			if (randomTrick)
			{
				CollectionUtils.Shuffle<sbyte>(random, this._tricks);
			}
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x002CF178 File Offset: 0x002CD378
		public override int GetFavorabilityChange()
		{
			int value = Weapon.Instance[this.TemplateId].BaseFavorabilityChange;
			bool flag = this.EquipmentEffectId >= 0;
			if (flag)
			{
				EquipmentEffectItem equipmentEffect = EquipmentEffect.Instance[this.EquipmentEffectId];
				value += value * (int)equipmentEffect.FavorChange / 100;
			}
			return value;
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x002CF1D4 File Offset: 0x002CD3D4
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return 0;
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x002CF1E8 File Offset: 0x002CD3E8
		public unsafe HitOrAvoidShorts GetHitFactors()
		{
			HitOrAvoidShorts value = this.GetBaseHitFactors();
			foreach (EquipmentEffectItem effect in DomainManager.Item.GetEquipmentEffects(this))
			{
				for (int i = 0; i < effect.HitFactors.Length; i++)
				{
					ref HitOrAvoidShorts ptr = ref value;
					int index = i;
					ptr[index] += effect.HitFactors[i];
				}
			}
			bool flag = ModificationStateHelper.IsActive(this.ModificationState, 2);
			if (flag)
			{
				RefiningEffects refiningEffects = DomainManager.Item.GetRefinedEffects(base.GetItemKey());
				for (int hitType = 0; hitType < 4; hitType++)
				{
					int refineBonus = refiningEffects.GetWeaponPropertyBonus(ERefiningEffectWeaponType.HitRateStrength + hitType);
					refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
					*(ref value.Items.FixedElementField + (IntPtr)hitType * 2) = (short)((int)(*(ref value.Items.FixedElementField + (IntPtr)hitType * 2)) + Math.Abs((int)(*(ref value.Items.FixedElementField + (IntPtr)hitType * 2)) * refineBonus / 100));
				}
			}
			return value;
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x002CF324 File Offset: 0x002CD524
		public unsafe HitOrAvoidShorts GetHitFactors(int charId)
		{
			HitOrAvoidShorts hitFactors = this.GetHitFactors();
			CValuePercent usePower = (int)DomainManager.Character.GetItemPower(charId, base.GetItemKey());
			for (int hitType = 0; hitType < 4; hitType++)
			{
				int hitValue = (int)(*(ref hitFactors.Items.FixedElementField + (IntPtr)hitType * 2));
				bool flag = hitValue > 0;
				if (flag)
				{
					*(ref hitFactors.Items.FixedElementField + (IntPtr)hitType * 2) = (short)(hitValue * usePower);
				}
			}
			return hitFactors;
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x002CF3A4 File Offset: 0x002CD5A4
		public bool TricksMatchCombatSkill(CombatSkillItem combatSkillCfg)
		{
			foreach (NeedTrick skillTrick in combatSkillCfg.TrickCost)
			{
				int count = 0;
				foreach (sbyte weaponTrick in this._tricks)
				{
					bool flag = weaponTrick == skillTrick.TrickType;
					if (flag)
					{
						count++;
					}
				}
				bool flag2 = count < (int)skillTrick.NeedCount;
				if (flag2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x002CF464 File Offset: 0x002CD664
		public unsafe override void SetMaxDurability(short maxDurability, DataContext context)
		{
			this.MaxDurability = maxDurability;
			base.SetModifiedAndInvalidateInfluencedCache(2, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 6U, 2);
				*(short*)pData = this.MaxDurability;
				pData += 2;
			}
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x002CF4C4 File Offset: 0x002CD6C4
		public unsafe override void SetEquipmentEffectId(short equipmentEffectId, DataContext context)
		{
			this.EquipmentEffectId = equipmentEffectId;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 8U, 2);
				*(short*)pData = this.EquipmentEffectId;
				pData += 2;
			}
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x002CF524 File Offset: 0x002CD724
		public List<sbyte> GetTricks()
		{
			return this._tricks;
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x002CF53C File Offset: 0x002CD73C
		public unsafe void SetTricks(List<sbyte> tricks, DataContext context)
		{
			this._tricks = tricks;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._tricks.Count;
				int contentSize = elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 0, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pData[i] = (byte)this._tricks[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x002CF5DC File Offset: 0x002CD7DC
		public unsafe override void SetCurrDurability(short currDurability, DataContext context)
		{
			this.CurrDurability = currDurability;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 10U, 2);
				*(short*)pData = this.CurrDurability;
				pData += 2;
			}
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x002CF63C File Offset: 0x002CD83C
		public unsafe override void SetModificationState(byte modificationState, DataContext context)
		{
			this.ModificationState = modificationState;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 12U, 1);
				*pData = this.ModificationState;
				pData++;
			}
		}

		// Token: 0x060052C3 RID: 21187 RVA: 0x002CF69C File Offset: 0x002CD89C
		public unsafe override void SetEquippedCharId(int equippedCharId, DataContext context)
		{
			this.EquippedCharId = equippedCharId;
			base.SetModifiedAndInvalidateInfluencedCache(7, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 13U, 4);
				*(int*)pData = this.EquippedCharId;
				pData += 4;
			}
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x002CF6FC File Offset: 0x002CD8FC
		public unsafe override void SetMaterialResources(MaterialResources materialResources, DataContext context)
		{
			this.MaterialResources = materialResources;
			base.SetModifiedAndInvalidateInfluencedCache(8, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 17U, 12);
				pData += this.MaterialResources.Serialize(pData);
			}
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x002CF760 File Offset: 0x002CD960
		public short GetPenetrationFactor()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 9);
			short penetrationFactor;
			if (flag)
			{
				penetrationFactor = this._penetrationFactor;
			}
			else
			{
				short value = this.CalcPenetrationFactor();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._penetrationFactor = value;
					dataStates.SetCached(this.DataStatesOffset, 9);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				penetrationFactor = this._penetrationFactor;
			}
			return penetrationFactor;
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x002CF808 File Offset: 0x002CDA08
		public short GetEquipmentAttack()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 10);
			short equipmentAttack;
			if (flag)
			{
				equipmentAttack = this._equipmentAttack;
			}
			else
			{
				short value = this.CalcEquipmentAttack();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._equipmentAttack = value;
					dataStates.SetCached(this.DataStatesOffset, 10);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				equipmentAttack = this._equipmentAttack;
			}
			return equipmentAttack;
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x002CF8B0 File Offset: 0x002CDAB0
		public short GetEquipmentDefense()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 11);
			short equipmentDefense;
			if (flag)
			{
				equipmentDefense = this._equipmentDefense;
			}
			else
			{
				short value = this.CalcEquipmentDefense();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._equipmentDefense = value;
					dataStates.SetCached(this.DataStatesOffset, 11);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				equipmentDefense = this._equipmentDefense;
			}
			return equipmentDefense;
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x002CF958 File Offset: 0x002CDB58
		public override int GetWeight()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 12);
			int weight;
			if (flag)
			{
				weight = this._weight;
			}
			else
			{
				int value = this.CalcWeight();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._weight = value;
					dataStates.SetCached(this.DataStatesOffset, 12);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				weight = this._weight;
			}
			return weight;
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x002CFA00 File Offset: 0x002CDC00
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return Weapon.Instance[this.TemplateId].Name;
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x002CFA28 File Offset: 0x002CDC28
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return Weapon.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x002CFA50 File Offset: 0x002CDC50
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return Weapon.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x002CFA78 File Offset: 0x002CDC78
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return Weapon.Instance[this.TemplateId].Grade;
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x002CFAA0 File Offset: 0x002CDCA0
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return Weapon.Instance[this.TemplateId].Icon;
		}

		// Token: 0x060052CE RID: 21198 RVA: 0x002CFAC8 File Offset: 0x002CDCC8
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return Weapon.Instance[this.TemplateId].Desc;
		}

		// Token: 0x060052CF RID: 21199 RVA: 0x002CFAF0 File Offset: 0x002CDCF0
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return Weapon.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x002CFB18 File Offset: 0x002CDD18
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return Weapon.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x060052D1 RID: 21201 RVA: 0x002CFB40 File Offset: 0x002CDD40
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return Weapon.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x002CFB68 File Offset: 0x002CDD68
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return Weapon.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x002CFB90 File Offset: 0x002CDD90
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return Weapon.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x002CFBB8 File Offset: 0x002CDDB8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return Weapon.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x002CFBE0 File Offset: 0x002CDDE0
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return Weapon.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x002CFC08 File Offset: 0x002CDE08
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return Weapon.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x002CFC30 File Offset: 0x002CDE30
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return Weapon.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x002CFC58 File Offset: 0x002CDE58
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return Weapon.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x002CFC80 File Offset: 0x002CDE80
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return Weapon.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x002CFCA8 File Offset: 0x002CDEA8
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return Weapon.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x002CFCD0 File Offset: 0x002CDED0
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return Weapon.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x002CFCF8 File Offset: 0x002CDEF8
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetEquipmentType()
		{
			return Weapon.Instance[this.TemplateId].EquipmentType;
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x002CFD20 File Offset: 0x002CDF20
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseEquipmentAttack()
		{
			return Weapon.Instance[this.TemplateId].BaseEquipmentAttack;
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x002CFD48 File Offset: 0x002CDF48
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseEquipmentDefense()
		{
			return Weapon.Instance[this.TemplateId].BaseEquipmentDefense;
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x002CFD70 File Offset: 0x002CDF70
		[CollectionObjectField(true, false, false, false, false)]
		public ref readonly PoisonsAndLevels GetInnatePoisons()
		{
			return ref Weapon.Instance[this.TemplateId].InnatePoisons;
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x002CFD98 File Offset: 0x002CDF98
		[CollectionObjectField(true, false, false, false, false)]
		public List<PropertyAndValue> GetRequiredCharacterProperties()
		{
			return Weapon.Instance[this.TemplateId].RequiredCharacterProperties;
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x002CFDC0 File Offset: 0x002CDFC0
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetWeaponAction()
		{
			return Weapon.Instance[this.TemplateId].WeaponAction;
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x002CFDE8 File Offset: 0x002CDFE8
		[CollectionObjectField(true, false, false, false, false)]
		public string GetCombatPictureR()
		{
			return Weapon.Instance[this.TemplateId].CombatPictureR;
		}

		// Token: 0x060052E3 RID: 21219 RVA: 0x002CFE10 File Offset: 0x002CE010
		[CollectionObjectField(true, false, false, false, false)]
		public string GetCombatPictureL()
		{
			return Weapon.Instance[this.TemplateId].CombatPictureL;
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x002CFE38 File Offset: 0x002CE038
		[CollectionObjectField(true, false, false, false, false)]
		public List<string> GetHitSounds()
		{
			return Weapon.Instance[this.TemplateId].HitSounds;
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x002CFE60 File Offset: 0x002CE060
		[CollectionObjectField(true, false, false, false, false)]
		public string GetSwingSoundsSuffix()
		{
			return Weapon.Instance[this.TemplateId].SwingSoundsSuffix;
		}

		// Token: 0x060052E6 RID: 21222 RVA: 0x002CFE88 File Offset: 0x002CE088
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetPlayArmorHitSound()
		{
			return Weapon.Instance[this.TemplateId].PlayArmorHitSound;
		}

		// Token: 0x060052E7 RID: 21223 RVA: 0x002CFEB0 File Offset: 0x002CE0B0
		[CollectionObjectField(true, false, false, false, false)]
		public List<TrickDistanceAdjust> GetTrickDistanceAdjusts()
		{
			return Weapon.Instance[this.TemplateId].TrickDistanceAdjusts;
		}

		// Token: 0x060052E8 RID: 21224 RVA: 0x002CFED8 File Offset: 0x002CE0D8
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetRandomTrick()
		{
			return Weapon.Instance[this.TemplateId].RandomTrick;
		}

		// Token: 0x060052E9 RID: 21225 RVA: 0x002CFF00 File Offset: 0x002CE100
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetCanChangeTrick()
		{
			return Weapon.Instance[this.TemplateId].CanChangeTrick;
		}

		// Token: 0x060052EA RID: 21226 RVA: 0x002CFF28 File Offset: 0x002CE128
		[CollectionObjectField(true, false, false, false, false)]
		public short GetPursueAttackFactor()
		{
			return Weapon.Instance[this.TemplateId].PursueAttackFactor;
		}

		// Token: 0x060052EB RID: 21227 RVA: 0x002CFF50 File Offset: 0x002CE150
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetAttackPreparePointCost()
		{
			return Weapon.Instance[this.TemplateId].AttackPreparePointCost;
		}

		// Token: 0x060052EC RID: 21228 RVA: 0x002CFF78 File Offset: 0x002CE178
		[CollectionObjectField(true, false, false, false, false)]
		public short GetMinDistance()
		{
			return Weapon.Instance[this.TemplateId].MinDistance;
		}

		// Token: 0x060052ED RID: 21229 RVA: 0x002CFFA0 File Offset: 0x002CE1A0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetMaxDistance()
		{
			return Weapon.Instance[this.TemplateId].MaxDistance;
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x002CFFC8 File Offset: 0x002CE1C8
		[CollectionObjectField(true, false, false, false, false)]
		public HitOrAvoidShorts GetBaseHitFactors()
		{
			return Weapon.Instance[this.TemplateId].BaseHitFactors;
		}

		// Token: 0x060052EF RID: 21231 RVA: 0x002CFFF0 File Offset: 0x002CE1F0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBasePenetrationFactor()
		{
			return Weapon.Instance[this.TemplateId].BasePenetrationFactor;
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x002D0018 File Offset: 0x002CE218
		[CollectionObjectField(true, false, false, false, false)]
		public short GetStanceIncrement()
		{
			return Weapon.Instance[this.TemplateId].StanceIncrement;
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x002D0040 File Offset: 0x002CE240
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetDefaultInnerRatio()
		{
			return Weapon.Instance[this.TemplateId].DefaultInnerRatio;
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x002D0068 File Offset: 0x002CE268
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetInnerRatioAdjustRange()
		{
			return Weapon.Instance[this.TemplateId].InnerRatioAdjustRange;
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x002D0090 File Offset: 0x002CE290
		[CollectionObjectField(true, false, false, false, false)]
		public List<string> GetBlockParticles()
		{
			return Weapon.Instance[this.TemplateId].BlockParticles;
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x002D00B8 File Offset: 0x002CE2B8
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return Weapon.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x002D00E0 File Offset: 0x002CE2E0
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return Weapon.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x002D0108 File Offset: 0x002CE308
		[CollectionObjectField(true, false, false, false, false)]
		public short GetMakeItemSubType()
		{
			return Weapon.Instance[this.TemplateId].MakeItemSubType;
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x002D0130 File Offset: 0x002CE330
		[CollectionObjectField(true, false, false, false, false)]
		public string GetIdleAni()
		{
			return Weapon.Instance[this.TemplateId].IdleAni;
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x002D0158 File Offset: 0x002CE358
		[CollectionObjectField(true, false, false, false, false)]
		public string GetForwardAni()
		{
			return Weapon.Instance[this.TemplateId].ForwardAni;
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x002D0180 File Offset: 0x002CE380
		[CollectionObjectField(true, false, false, false, false)]
		public string GetBackwardAni()
		{
			return Weapon.Instance[this.TemplateId].BackwardAni;
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x002D01A8 File Offset: 0x002CE3A8
		[CollectionObjectField(true, false, false, false, false)]
		public string GetFastBackwardAni()
		{
			return Weapon.Instance[this.TemplateId].FastBackwardAni;
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x002D01D0 File Offset: 0x002CE3D0
		[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 4)]
		public string[] GetAvoidAnis()
		{
			return Weapon.Instance[this.TemplateId].AvoidAnis;
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x002D01F8 File Offset: 0x002CE3F8
		[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 3)]
		public string[] GetHittedAnis()
		{
			return Weapon.Instance[this.TemplateId].HittedAnis;
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x002D0220 File Offset: 0x002CE420
		[CollectionObjectField(true, false, false, false, false)]
		public string GetFatalParticle()
		{
			return Weapon.Instance[this.TemplateId].FatalParticle;
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x002D0248 File Offset: 0x002CE448
		[CollectionObjectField(true, false, false, false, false)]
		public string GetTeammateCmdAniPostfix()
		{
			return Weapon.Instance[this.TemplateId].TeammateCmdAniPostfix;
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x002D0270 File Offset: 0x002CE470
		[CollectionObjectField(true, false, false, false, false)]
		public List<string> GetBlockAnis()
		{
			return Weapon.Instance[this.TemplateId].BlockAnis;
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x002D0298 File Offset: 0x002CE498
		[CollectionObjectField(true, false, false, false, false)]
		public List<string> GetBlockSounds()
		{
			return Weapon.Instance[this.TemplateId].BlockSounds;
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x002D02C0 File Offset: 0x002CE4C0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetChangeTrickPercent()
		{
			return Weapon.Instance[this.TemplateId].ChangeTrickPercent;
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x002D02E8 File Offset: 0x002CE4E8
		[CollectionObjectField(true, false, false, false, false)]
		public string GetFastForwardAni()
		{
			return Weapon.Instance[this.TemplateId].FastForwardAni;
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x002D0310 File Offset: 0x002CE510
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetDetachable()
		{
			return Weapon.Instance[this.TemplateId].Detachable;
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x002D0338 File Offset: 0x002CE538
		[CollectionObjectField(true, false, false, false, false)]
		public int GetUnlockEffect()
		{
			return Weapon.Instance[this.TemplateId].UnlockEffect;
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x002D0360 File Offset: 0x002CE560
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return Weapon.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x002D0388 File Offset: 0x002CE588
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRawCreate()
		{
			return Weapon.Instance[this.TemplateId].AllowRawCreate;
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x002D03B0 File Offset: 0x002CE5B0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return Weapon.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x002D03D8 File Offset: 0x002CE5D8
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return Weapon.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x002D0400 File Offset: 0x002CE600
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return Weapon.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x002D0428 File Offset: 0x002CE628
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return Weapon.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x002D0450 File Offset: 0x002CE650
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowCrippledCreate()
		{
			return Weapon.Instance[this.TemplateId].AllowCrippledCreate;
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x002D0478 File Offset: 0x002CE678
		[CollectionObjectField(true, false, false, false, false)]
		public short GetEquipmentCombatPowerValueFactor()
		{
			return Weapon.Instance[this.TemplateId].EquipmentCombatPowerValueFactor;
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x002D04A0 File Offset: 0x002CE6A0
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBaseStartupFrames()
		{
			return Weapon.Instance[this.TemplateId].BaseStartupFrames;
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x002D04C8 File Offset: 0x002CE6C8
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBaseRecoveryFrames()
		{
			return Weapon.Instance[this.TemplateId].BaseRecoveryFrames;
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x002D04EF File Offset: 0x002CE6EF
		public Weapon()
		{
			this._spinLock = new SpinLock(false);
			base..ctor();
			this._tricks = new List<sbyte>();
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x002D0510 File Offset: 0x002CE710
		public Weapon(short templateId)
		{
			this._spinLock = new SpinLock(false);
			base..ctor();
			WeaponItem template = Weapon.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
			this.EquipmentEffectId = template.EquipmentEffectId;
			this._tricks = new List<sbyte>(template.Tricks);
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x002D0574 File Offset: 0x002CE774
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x002D0588 File Offset: 0x002CE788
		public int GetSerializedSize()
		{
			int totalSize = 33;
			int elementsCount = this._tricks.Count;
			int contentSize = elementsCount;
			int dataSize = 2 + contentSize;
			return totalSize + dataSize;
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x002D05B8 File Offset: 0x002CE7B8
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.Id;
			byte* pCurrData = pData + 4;
			*(short*)pCurrData = this.TemplateId;
			pCurrData += 2;
			*(short*)pCurrData = this.MaxDurability;
			pCurrData += 2;
			*(short*)pCurrData = this.EquipmentEffectId;
			pCurrData += 2;
			*(short*)pCurrData = this.CurrDurability;
			pCurrData += 2;
			*pCurrData = this.ModificationState;
			pCurrData++;
			*(int*)pCurrData = this.EquippedCharId;
			pCurrData += 4;
			pCurrData += this.MaterialResources.Serialize(pCurrData);
			int elementsCount = this._tricks.Count;
			int contentSize = elementsCount;
			bool flag = contentSize > 4194300;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_tricks");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount);
			pCurrData += 2;
			for (int i = 0; i < elementsCount; i++)
			{
				pCurrData[i] = (byte)this._tricks[i];
			}
			pCurrData += contentSize;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x002D06EC File Offset: 0x002CE8EC
		public unsafe int Deserialize(byte* pData)
		{
			this.Id = *(int*)pData;
			byte* pCurrData = pData + 4;
			this.TemplateId = *(short*)pCurrData;
			pCurrData += 2;
			this.MaxDurability = *(short*)pCurrData;
			pCurrData += 2;
			this.EquipmentEffectId = *(short*)pCurrData;
			pCurrData += 2;
			this.CurrDurability = *(short*)pCurrData;
			pCurrData += 2;
			this.ModificationState = *pCurrData;
			pCurrData++;
			this.EquippedCharId = *(int*)pCurrData;
			pCurrData += 4;
			pCurrData += this.MaterialResources.Deserialize(pCurrData);
			pCurrData += 4;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			this._tricks.Clear();
			for (int i = 0; i < (int)elementsCount; i++)
			{
				this._tricks.Add(*(sbyte*)(pCurrData + i));
			}
			pCurrData += elementsCount;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04001616 RID: 5654
		[CollectionObjectField(true, true, false, false, false)]
		private List<sbyte> _tricks;

		// Token: 0x04001617 RID: 5655
		[CollectionObjectField(false, false, true, false, false)]
		private short _penetrationFactor;

		// Token: 0x04001618 RID: 5656
		[CollectionObjectField(false, false, true, false, false)]
		private short _equipmentAttack;

		// Token: 0x04001619 RID: 5657
		[CollectionObjectField(false, false, true, false, false)]
		private short _equipmentDefense;

		// Token: 0x0400161A RID: 5658
		[CollectionObjectField(false, false, true, false, false)]
		private int _weight;

		// Token: 0x0400161B RID: 5659
		public const int FixedSize = 29;

		// Token: 0x0400161C RID: 5660
		public const int DynamicCount = 1;

		// Token: 0x0400161D RID: 5661
		private SpinLock _spinLock;

		// Token: 0x02000AC6 RID: 2758
		internal class FixedFieldInfos
		{
			// Token: 0x04002CC4 RID: 11460
			public const uint Id_Offset = 0U;

			// Token: 0x04002CC5 RID: 11461
			public const int Id_Size = 4;

			// Token: 0x04002CC6 RID: 11462
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002CC7 RID: 11463
			public const int TemplateId_Size = 2;

			// Token: 0x04002CC8 RID: 11464
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002CC9 RID: 11465
			public const int MaxDurability_Size = 2;

			// Token: 0x04002CCA RID: 11466
			public const uint EquipmentEffectId_Offset = 8U;

			// Token: 0x04002CCB RID: 11467
			public const int EquipmentEffectId_Size = 2;

			// Token: 0x04002CCC RID: 11468
			public const uint CurrDurability_Offset = 10U;

			// Token: 0x04002CCD RID: 11469
			public const int CurrDurability_Size = 2;

			// Token: 0x04002CCE RID: 11470
			public const uint ModificationState_Offset = 12U;

			// Token: 0x04002CCF RID: 11471
			public const int ModificationState_Size = 1;

			// Token: 0x04002CD0 RID: 11472
			public const uint EquippedCharId_Offset = 13U;

			// Token: 0x04002CD1 RID: 11473
			public const int EquippedCharId_Size = 4;

			// Token: 0x04002CD2 RID: 11474
			public const uint MaterialResources_Offset = 17U;

			// Token: 0x04002CD3 RID: 11475
			public const int MaterialResources_Size = 12;
		}
	}
}
