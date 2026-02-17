using System;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006BE RID: 1726
	[SerializableGameData(NotForDisplayModule = true)]
	public class CombatWeaponData : BaseGameDataObject, ISerializableGameData
	{
		// Token: 0x0600665C RID: 26204 RVA: 0x003ABA54 File Offset: 0x003A9C54
		[SingleValueDependency(5, new ushort[]
		{
			27
		}, Condition = InfluenceCondition.CombatWeaponIsTaiwuWeapon)]
		[SingleValueDependency(8, new ushort[]
		{
			31
		}, Condition = InfluenceCondition.CombatWeaponIsNotTaiwuWeapon)]
		private sbyte CalcInnerRatio()
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(this.Character.GetId(), true);
			sbyte result;
			if (flag)
			{
				result = this._innerRatio;
			}
			else
			{
				bool isTaiwu = this.Character.IsTaiwu;
				if (isTaiwu)
				{
					result = DomainManager.Taiwu.GetWeaponCurrInnerRatios()[this.Index];
				}
				else
				{
					WeaponExpectInnerRatioData expectData = DomainManager.Combat.GetExpectRatioData();
					sbyte expectRatio = expectData.GetValue(this.Character.GetId(), this.Index);
					result = ((expectRatio < 0) ? this.Template.DefaultInnerRatio : this.Character.GetCharacter().CalcWeaponInnerRatio(this.TemplateId, expectRatio));
				}
			}
			return result;
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600665D RID: 26205 RVA: 0x003ABAFC File Offset: 0x003A9CFC
		public CombatCharacter Character { get; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600665E RID: 26206 RVA: 0x003ABB04 File Offset: 0x003A9D04
		// (set) Token: 0x0600665F RID: 26207 RVA: 0x003ABB0C File Offset: 0x003A9D0C
		public int Index { get; private set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06006660 RID: 26208 RVA: 0x003ABB15 File Offset: 0x003A9D15
		public bool NotInAnyCd
		{
			get
			{
				return this._cdFrame == 0 && this._fixedCdLeftFrame == 0;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06006661 RID: 26209 RVA: 0x003ABB2B File Offset: 0x003A9D2B
		public GameData.Domains.Item.Weapon Item
		{
			get
			{
				return DomainManager.Item.GetElement_Weapons(this._id);
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06006662 RID: 26210 RVA: 0x003ABB3D File Offset: 0x003A9D3D
		public short TemplateId
		{
			get
			{
				return this.Item.GetTemplateId();
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06006663 RID: 26211 RVA: 0x003ABB4A File Offset: 0x003A9D4A
		public WeaponItem Template
		{
			get
			{
				return Config.Weapon.Instance[this.TemplateId];
			}
		}

		// Token: 0x06006664 RID: 26212 RVA: 0x003ABB5C File Offset: 0x003A9D5C
		public CombatWeaponData(ItemKey key, CombatCharacter character) : this()
		{
			this._id = key.Id;
			this.Character = character;
			this._pestleEffectId = -1L;
		}

		// Token: 0x06006665 RID: 26213 RVA: 0x003ABB84 File Offset: 0x003A9D84
		public void Init(DataContext context, int index)
		{
			this.Index = index;
			this.SetDurability(this.Item.GetCurrDurability(), context);
			this.SetCdFrame(0, context);
			this.SetFixedCdLeftFrame(0, context);
			this.SetFixedCdTotalFrame(0, context);
			this.SetCanChangeTo(index >= 3 || this.GetDurability() > 0, context);
			this.SetAutoAttackEffect(new SkillEffectKey(-1, false), context);
			this.SetPestleEffect(new SkillEffectKey(-1, false), context);
		}

		// Token: 0x06006666 RID: 26214 RVA: 0x003ABC00 File Offset: 0x003A9E00
		public void SetPestleEffect(DataContext context, int charId, string effectName, SkillEffectKey effectKey)
		{
			bool flag = this._pestleEffect.Equals(effectKey);
			if (!flag)
			{
				bool flag2 = this._pestleEffect.SkillId >= 0;
				if (flag2)
				{
					this.RemovePestleEffect(context);
				}
				this.SetPestleEffect(effectKey, context);
				this._pestleEffectId = DomainManager.SpecialEffect.Add(context, charId, effectName);
			}
		}

		// Token: 0x06006667 RID: 26215 RVA: 0x003ABC5B File Offset: 0x003A9E5B
		public void RemovePestleEffect(DataContext context)
		{
			DomainManager.SpecialEffect.Remove(context, this._pestleEffectId);
			this.SetPestleEffect(new SkillEffectKey(-1, false), context);
			this._pestleEffectId = -1L;
		}

		// Token: 0x06006668 RID: 26216 RVA: 0x003ABC88 File Offset: 0x003A9E88
		public int GetId()
		{
			return this._id;
		}

		// Token: 0x06006669 RID: 26217 RVA: 0x003ABCA0 File Offset: 0x003A9EA0
		public unsafe void SetId(int id, DataContext context)
		{
			this._id = id;
			base.SetModifiedAndInvalidateInfluencedCache(0, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 0U, 4);
				*(int*)pData = this._id;
				pData += 4;
			}
		}

		// Token: 0x0600666A RID: 26218 RVA: 0x003ABD00 File Offset: 0x003A9F00
		public sbyte[] GetWeaponTricks()
		{
			return this._weaponTricks;
		}

		// Token: 0x0600666B RID: 26219 RVA: 0x003ABD18 File Offset: 0x003A9F18
		public unsafe void SetWeaponTricks(sbyte[] weaponTricks, DataContext context)
		{
			this._weaponTricks = weaponTricks;
			base.SetModifiedAndInvalidateInfluencedCache(1, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 4U, 6);
				for (int i = 0; i < 6; i++)
				{
					pData[i] = (byte)this._weaponTricks[i];
				}
				pData += 6;
			}
		}

		// Token: 0x0600666C RID: 26220 RVA: 0x003ABD8C File Offset: 0x003A9F8C
		public bool GetCanChangeTo()
		{
			return this._canChangeTo;
		}

		// Token: 0x0600666D RID: 26221 RVA: 0x003ABDA4 File Offset: 0x003A9FA4
		public unsafe void SetCanChangeTo(bool canChangeTo, DataContext context)
		{
			this._canChangeTo = canChangeTo;
			base.SetModifiedAndInvalidateInfluencedCache(2, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 10U, 1);
				*pData = (this._canChangeTo ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x0600666E RID: 26222 RVA: 0x003ABE04 File Offset: 0x003AA004
		public short GetDurability()
		{
			return this._durability;
		}

		// Token: 0x0600666F RID: 26223 RVA: 0x003ABE1C File Offset: 0x003AA01C
		public unsafe void SetDurability(short durability, DataContext context)
		{
			this._durability = durability;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 11U, 2);
				*(short*)pData = this._durability;
				pData += 2;
			}
		}

		// Token: 0x06006670 RID: 26224 RVA: 0x003ABE7C File Offset: 0x003AA07C
		public short GetCdFrame()
		{
			return this._cdFrame;
		}

		// Token: 0x06006671 RID: 26225 RVA: 0x003ABE94 File Offset: 0x003AA094
		public unsafe void SetCdFrame(short cdFrame, DataContext context)
		{
			this._cdFrame = cdFrame;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 13U, 2);
				*(short*)pData = this._cdFrame;
				pData += 2;
			}
		}

		// Token: 0x06006672 RID: 26226 RVA: 0x003ABEF4 File Offset: 0x003AA0F4
		public SkillEffectKey GetAutoAttackEffect()
		{
			return this._autoAttackEffect;
		}

		// Token: 0x06006673 RID: 26227 RVA: 0x003ABF0C File Offset: 0x003AA10C
		public unsafe void SetAutoAttackEffect(SkillEffectKey autoAttackEffect, DataContext context)
		{
			this._autoAttackEffect = autoAttackEffect;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 15U, 3);
				pData += this._autoAttackEffect.Serialize(pData);
			}
		}

		// Token: 0x06006674 RID: 26228 RVA: 0x003ABF70 File Offset: 0x003AA170
		public SkillEffectKey GetPestleEffect()
		{
			return this._pestleEffect;
		}

		// Token: 0x06006675 RID: 26229 RVA: 0x003ABF88 File Offset: 0x003AA188
		public unsafe void SetPestleEffect(SkillEffectKey pestleEffect, DataContext context)
		{
			this._pestleEffect = pestleEffect;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 18U, 3);
				pData += this._pestleEffect.Serialize(pData);
			}
		}

		// Token: 0x06006676 RID: 26230 RVA: 0x003ABFEC File Offset: 0x003AA1EC
		public short GetFixedCdLeftFrame()
		{
			return this._fixedCdLeftFrame;
		}

		// Token: 0x06006677 RID: 26231 RVA: 0x003AC004 File Offset: 0x003AA204
		public unsafe void SetFixedCdLeftFrame(short fixedCdLeftFrame, DataContext context)
		{
			this._fixedCdLeftFrame = fixedCdLeftFrame;
			base.SetModifiedAndInvalidateInfluencedCache(7, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 21U, 2);
				*(short*)pData = this._fixedCdLeftFrame;
				pData += 2;
			}
		}

		// Token: 0x06006678 RID: 26232 RVA: 0x003AC064 File Offset: 0x003AA264
		public short GetFixedCdTotalFrame()
		{
			return this._fixedCdTotalFrame;
		}

		// Token: 0x06006679 RID: 26233 RVA: 0x003AC07C File Offset: 0x003AA27C
		public unsafe void SetFixedCdTotalFrame(short fixedCdTotalFrame, DataContext context)
		{
			this._fixedCdTotalFrame = fixedCdTotalFrame;
			base.SetModifiedAndInvalidateInfluencedCache(8, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 23U, 2);
				*(short*)pData = this._fixedCdTotalFrame;
				pData += 2;
			}
		}

		// Token: 0x0600667A RID: 26234 RVA: 0x003AC0DC File Offset: 0x003AA2DC
		public sbyte GetInnerRatio()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 9);
			sbyte innerRatio;
			if (flag)
			{
				innerRatio = this._innerRatio;
			}
			else
			{
				this._innerRatio = this.CalcInnerRatio();
				dataStates.SetCached(this.DataStatesOffset, 9);
				innerRatio = this._innerRatio;
			}
			return innerRatio;
		}

		// Token: 0x0600667B RID: 26235 RVA: 0x003AC136 File Offset: 0x003AA336
		public CombatWeaponData()
		{
			this._weaponTricks = new sbyte[6];
		}

		// Token: 0x0600667C RID: 26236 RVA: 0x003AC14C File Offset: 0x003AA34C
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x0600667D RID: 26237 RVA: 0x003AC160 File Offset: 0x003AA360
		public int GetSerializedSize()
		{
			return 25;
		}

		// Token: 0x0600667E RID: 26238 RVA: 0x003AC178 File Offset: 0x003AA378
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this._id;
			byte* pCurrData = pData + 4;
			bool flag = this._weaponTricks.Length != 6;
			if (flag)
			{
				throw new Exception("Elements count of field _weaponTricks is not equal to declaration");
			}
			for (int i = 0; i < 6; i++)
			{
				pCurrData[i] = (byte)this._weaponTricks[i];
			}
			pCurrData += 6;
			*pCurrData = (this._canChangeTo ? 1 : 0);
			pCurrData++;
			*(short*)pCurrData = this._durability;
			pCurrData += 2;
			*(short*)pCurrData = this._cdFrame;
			pCurrData += 2;
			pCurrData += this._autoAttackEffect.Serialize(pCurrData);
			pCurrData += this._pestleEffect.Serialize(pCurrData);
			*(short*)pCurrData = this._fixedCdLeftFrame;
			pCurrData += 2;
			*(short*)pCurrData = this._fixedCdTotalFrame;
			pCurrData += 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x0600667F RID: 26239 RVA: 0x003AC238 File Offset: 0x003AA438
		public unsafe int Deserialize(byte* pData)
		{
			this._id = *(int*)pData;
			byte* pCurrData = pData + 4;
			bool flag = this._weaponTricks.Length != 6;
			if (flag)
			{
				throw new Exception("Elements count of field _weaponTricks is not equal to declaration");
			}
			for (int i = 0; i < 6; i++)
			{
				this._weaponTricks[i] = *(sbyte*)(pCurrData + i);
			}
			pCurrData += 6;
			this._canChangeTo = (*pCurrData != 0);
			pCurrData++;
			this._durability = *(short*)pCurrData;
			pCurrData += 2;
			this._cdFrame = *(short*)pCurrData;
			pCurrData += 2;
			pCurrData += this._autoAttackEffect.Deserialize(pCurrData);
			pCurrData += this._pestleEffect.Deserialize(pCurrData);
			this._fixedCdLeftFrame = *(short*)pCurrData;
			pCurrData += 2;
			this._fixedCdTotalFrame = *(short*)pCurrData;
			pCurrData += 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04001BD0 RID: 7120
		[CollectionObjectField(false, true, false, false, false)]
		private int _id;

		// Token: 0x04001BD1 RID: 7121
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 6)]
		private sbyte[] _weaponTricks;

		// Token: 0x04001BD2 RID: 7122
		[CollectionObjectField(false, true, false, false, false)]
		private bool _canChangeTo;

		// Token: 0x04001BD3 RID: 7123
		[CollectionObjectField(false, true, false, false, false)]
		private short _durability;

		// Token: 0x04001BD4 RID: 7124
		[CollectionObjectField(false, false, true, false, false)]
		private sbyte _innerRatio;

		// Token: 0x04001BD5 RID: 7125
		[CollectionObjectField(false, true, false, false, false)]
		private short _cdFrame;

		// Token: 0x04001BD6 RID: 7126
		[CollectionObjectField(false, true, false, false, false)]
		private short _fixedCdLeftFrame;

		// Token: 0x04001BD7 RID: 7127
		[CollectionObjectField(false, true, false, false, false)]
		private short _fixedCdTotalFrame;

		// Token: 0x04001BD8 RID: 7128
		[CollectionObjectField(false, true, false, false, false)]
		private SkillEffectKey _autoAttackEffect;

		// Token: 0x04001BD9 RID: 7129
		[CollectionObjectField(false, true, false, false, false)]
		private SkillEffectKey _pestleEffect;

		// Token: 0x04001BDC RID: 7132
		private long _pestleEffectId;

		// Token: 0x04001BDD RID: 7133
		public const int FixedSize = 25;

		// Token: 0x04001BDE RID: 7134
		public const int DynamicCount = 0;

		// Token: 0x02000B6B RID: 2923
		internal class FixedFieldInfos
		{
			// Token: 0x04003099 RID: 12441
			public const uint Id_Offset = 0U;

			// Token: 0x0400309A RID: 12442
			public const int Id_Size = 4;

			// Token: 0x0400309B RID: 12443
			public const uint WeaponTricks_Offset = 4U;

			// Token: 0x0400309C RID: 12444
			public const int WeaponTricks_Size = 6;

			// Token: 0x0400309D RID: 12445
			public const uint CanChangeTo_Offset = 10U;

			// Token: 0x0400309E RID: 12446
			public const int CanChangeTo_Size = 1;

			// Token: 0x0400309F RID: 12447
			public const uint Durability_Offset = 11U;

			// Token: 0x040030A0 RID: 12448
			public const int Durability_Size = 2;

			// Token: 0x040030A1 RID: 12449
			public const uint CdFrame_Offset = 13U;

			// Token: 0x040030A2 RID: 12450
			public const int CdFrame_Size = 2;

			// Token: 0x040030A3 RID: 12451
			public const uint AutoAttackEffect_Offset = 15U;

			// Token: 0x040030A4 RID: 12452
			public const int AutoAttackEffect_Size = 3;

			// Token: 0x040030A5 RID: 12453
			public const uint PestleEffect_Offset = 18U;

			// Token: 0x040030A6 RID: 12454
			public const int PestleEffect_Size = 3;

			// Token: 0x040030A7 RID: 12455
			public const uint FixedCdLeftFrame_Offset = 21U;

			// Token: 0x040030A8 RID: 12456
			public const int FixedCdLeftFrame_Size = 2;

			// Token: 0x040030A9 RID: 12457
			public const uint FixedCdTotalFrame_Offset = 23U;

			// Token: 0x040030AA RID: 12458
			public const int FixedCdTotalFrame_Size = 2;
		}
	}
}
