using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x02000662 RID: 1634
	[SerializableGameData(NotForDisplayModule = true)]
	public class Cricket : ItemBase, ISerializableGameData
	{
		// Token: 0x06004F28 RID: 20264 RVA: 0x002B31A8 File Offset: 0x002B13A8
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

		// Token: 0x06004F29 RID: 20265 RVA: 0x002B3208 File Offset: 0x002B1408
		public unsafe override void SetCurrDurability(short currDurability, DataContext context)
		{
			this.CurrDurability = currDurability;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 8U, 2);
				*(short*)pData = this.CurrDurability;
				pData += 2;
			}
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x002B3268 File Offset: 0x002B1468
		public unsafe override void SetModificationState(byte modificationState, DataContext context)
		{
			this.ModificationState = modificationState;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 10U, 1);
				*pData = this.ModificationState;
				pData++;
			}
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x002B32C8 File Offset: 0x002B14C8
		public short GetColorId()
		{
			return this._colorId;
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x002B32E0 File Offset: 0x002B14E0
		public short GetPartId()
		{
			return this._partId;
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x002B32F8 File Offset: 0x002B14F8
		public short[] GetInjuries()
		{
			return this._injuries;
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x002B3310 File Offset: 0x002B1510
		public unsafe void SetInjuries(short[] injuries, DataContext context)
		{
			this._injuries = injuries;
			base.SetModifiedAndInvalidateInfluencedCache(7, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 15U, 10);
				for (int i = 0; i < 5; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = this._injuries[i];
				}
				pData += 10;
			}
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x002B338C File Offset: 0x002B158C
		public short GetWinsCount()
		{
			return this._winsCount;
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x002B33A4 File Offset: 0x002B15A4
		public unsafe void SetWinsCount(short winsCount, DataContext context)
		{
			this._winsCount = winsCount;
			base.SetModifiedAndInvalidateInfluencedCache(8, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 25U, 2);
				*(short*)pData = this._winsCount;
				pData += 2;
			}
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x002B3404 File Offset: 0x002B1604
		public short GetLossesCount()
		{
			return this._lossesCount;
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x002B341C File Offset: 0x002B161C
		public unsafe void SetLossesCount(short lossesCount, DataContext context)
		{
			this._lossesCount = lossesCount;
			base.SetModifiedAndInvalidateInfluencedCache(9, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 27U, 2);
				*(short*)pData = this._lossesCount;
				pData += 2;
			}
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x002B3480 File Offset: 0x002B1680
		public short GetBestEnemyColorId()
		{
			return this._bestEnemyColorId;
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x002B3498 File Offset: 0x002B1698
		public unsafe void SetBestEnemyColorId(short bestEnemyColorId, DataContext context)
		{
			this._bestEnemyColorId = bestEnemyColorId;
			base.SetModifiedAndInvalidateInfluencedCache(10, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 29U, 2);
				*(short*)pData = this._bestEnemyColorId;
				pData += 2;
			}
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x002B34FC File Offset: 0x002B16FC
		public short GetBestEnemyPartId()
		{
			return this._bestEnemyPartId;
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x002B3514 File Offset: 0x002B1714
		public unsafe void SetBestEnemyPartId(short bestEnemyPartId, DataContext context)
		{
			this._bestEnemyPartId = bestEnemyPartId;
			base.SetModifiedAndInvalidateInfluencedCache(11, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 31U, 2);
				*(short*)pData = this._bestEnemyPartId;
				pData += 2;
			}
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x002B3578 File Offset: 0x002B1778
		public sbyte GetAge()
		{
			return this._age;
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x002B3590 File Offset: 0x002B1790
		public unsafe void SetAge(sbyte age, DataContext context)
		{
			this._age = age;
			base.SetModifiedAndInvalidateInfluencedCache(12, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 33U, 1);
				*pData = (byte)this._age;
				pData++;
			}
		}

		// Token: 0x06004F39 RID: 20281 RVA: 0x002B35F4 File Offset: 0x002B17F4
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return Cricket.Instance[this.TemplateId].Name;
		}

		// Token: 0x06004F3A RID: 20282 RVA: 0x002B361C File Offset: 0x002B181C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return Cricket.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x002B3644 File Offset: 0x002B1844
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return Cricket.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x002B366C File Offset: 0x002B186C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return Cricket.Instance[this.TemplateId].Grade;
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x002B3694 File Offset: 0x002B1894
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return Cricket.Instance[this.TemplateId].Icon;
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x002B36BC File Offset: 0x002B18BC
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return Cricket.Instance[this.TemplateId].Desc;
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x002B36E4 File Offset: 0x002B18E4
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return Cricket.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x002B370C File Offset: 0x002B190C
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return Cricket.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x002B3734 File Offset: 0x002B1934
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return Cricket.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x002B375C File Offset: 0x002B195C
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return Cricket.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x002B3784 File Offset: 0x002B1984
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return Cricket.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x002B37AC File Offset: 0x002B19AC
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return Cricket.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x002B37D4 File Offset: 0x002B19D4
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return Cricket.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x002B37FC File Offset: 0x002B19FC
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return Cricket.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x002B3824 File Offset: 0x002B1A24
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return Cricket.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x002B384C File Offset: 0x002B1A4C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return Cricket.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x002B3874 File Offset: 0x002B1A74
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return Cricket.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x002B389C File Offset: 0x002B1A9C
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return Cricket.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x002B38C4 File Offset: 0x002B1AC4
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return Cricket.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x002B38EC File Offset: 0x002B1AEC
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return Cricket.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x002B3914 File Offset: 0x002B1B14
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return Cricket.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x002B393C File Offset: 0x002B1B3C
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return Cricket.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x002B3964 File Offset: 0x002B1B64
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return Cricket.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x002B398C File Offset: 0x002B1B8C
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return Cricket.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x002B39B4 File Offset: 0x002B1BB4
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return Cricket.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x002B39DC File Offset: 0x002B1BDC
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return Cricket.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x002B3A03 File Offset: 0x002B1C03
		public Cricket()
		{
			this._injuries = new short[5];
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x002B3A1C File Offset: 0x002B1C1C
		public Cricket(short templateId)
		{
			CricketItem template = Cricket.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
			this._injuries = new short[5];
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x002B3A64 File Offset: 0x002B1C64
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x002B3A78 File Offset: 0x002B1C78
		public int GetSerializedSize()
		{
			return 34;
		}

		// Token: 0x06004F57 RID: 20311 RVA: 0x002B3A90 File Offset: 0x002B1C90
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.Id;
			byte* pCurrData = pData + 4;
			*(short*)pCurrData = this.TemplateId;
			pCurrData += 2;
			*(short*)pCurrData = this.MaxDurability;
			pCurrData += 2;
			*(short*)pCurrData = this.CurrDurability;
			pCurrData += 2;
			*pCurrData = this.ModificationState;
			pCurrData++;
			*(short*)pCurrData = this._colorId;
			pCurrData += 2;
			*(short*)pCurrData = this._partId;
			pCurrData += 2;
			bool flag = this._injuries.Length != 5;
			if (flag)
			{
				throw new Exception("Elements count of field _injuries is not equal to declaration");
			}
			for (int i = 0; i < 5; i++)
			{
				*(short*)(pCurrData + (IntPtr)i * 2) = this._injuries[i];
			}
			pCurrData += 10;
			*(short*)pCurrData = this._winsCount;
			pCurrData += 2;
			*(short*)pCurrData = this._lossesCount;
			pCurrData += 2;
			*(short*)pCurrData = this._bestEnemyColorId;
			pCurrData += 2;
			*(short*)pCurrData = this._bestEnemyPartId;
			pCurrData += 2;
			*pCurrData = (byte)this._age;
			pCurrData++;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x002B3B80 File Offset: 0x002B1D80
		public unsafe int Deserialize(byte* pData)
		{
			this.Id = *(int*)pData;
			byte* pCurrData = pData + 4;
			this.TemplateId = *(short*)pCurrData;
			pCurrData += 2;
			this.MaxDurability = *(short*)pCurrData;
			pCurrData += 2;
			this.CurrDurability = *(short*)pCurrData;
			pCurrData += 2;
			this.ModificationState = *pCurrData;
			pCurrData++;
			this._colorId = *(short*)pCurrData;
			pCurrData += 2;
			this._partId = *(short*)pCurrData;
			pCurrData += 2;
			bool flag = this._injuries.Length != 5;
			if (flag)
			{
				throw new Exception("Elements count of field _injuries is not equal to declaration");
			}
			for (int i = 0; i < 5; i++)
			{
				this._injuries[i] = *(short*)(pCurrData + (IntPtr)i * 2);
			}
			pCurrData += 10;
			this._winsCount = *(short*)pCurrData;
			pCurrData += 2;
			this._lossesCount = *(short*)pCurrData;
			pCurrData += 2;
			this._bestEnemyColorId = *(short*)pCurrData;
			pCurrData += 2;
			this._bestEnemyPartId = *(short*)pCurrData;
			pCurrData += 2;
			this._age = *(sbyte*)pCurrData;
			pCurrData++;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x002B3C70 File Offset: 0x002B1E70
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return 0;
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x002B3C84 File Offset: 0x002B1E84
		public override int GetValue()
		{
			bool flag = this.CurrDurability <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int selfValue = (this._partId > 0) ? Math.Max(CricketParts.Instance[this._colorId].Value, CricketParts.Instance[this._partId].Value) : CricketParts.Instance[this._colorId].Value;
				int bestEnemyValue = (this._bestEnemyColorId > 0) ? Math.Max(CricketParts.Instance[this._bestEnemyColorId].Value, CricketParts.Instance[this._bestEnemyPartId].Value) : 0;
				result = Math.Max(selfValue, bestEnemyValue);
			}
			return result;
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x002B3D44 File Offset: 0x002B1F44
		public bool IsCombinedCricket()
		{
			return this._partId != 0;
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x002B3D60 File Offset: 0x002B1F60
		public CricketPartsItem GetColorData()
		{
			return CricketParts.Instance[this._colorId];
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x002B3D84 File Offset: 0x002B1F84
		public CricketPartsItem GetPartsData()
		{
			return CricketParts.Instance[this._partId];
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x002B3DA8 File Offset: 0x002B1FA8
		public int CalcCatchLucky()
		{
			short value = this.GetColorData().CatchInfluence;
			bool flag = this.IsCombinedCricket();
			if (flag)
			{
				value += this.GetPartsData().CatchInfluence;
			}
			return (int)value;
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x002B3DE0 File Offset: 0x002B1FE0
		public int CalcCombatValue()
		{
			sbyte enemyGrade = Cricket.CalcGrade(this._bestEnemyColorId, this._bestEnemyPartId);
			return Math.Max((int)(this._winsCount - this._lossesCount), 0) * (int)(enemyGrade + 1) * ((this._lossesCount <= 0) ? 2 : 1);
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x002B3E2C File Offset: 0x002B202C
		public bool UpdateCricketAge(DataContext context)
		{
			bool flag = this.CurrDurability <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._age += 1;
				this.SetAge(this._age, context);
				bool isAlive = this.IsAlive;
				if (isAlive)
				{
					result = false;
				}
				else
				{
					this.SetCurrDurability(0, context);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x002B3E87 File Offset: 0x002B2087
		public void Rebirth(DataContext context)
		{
			this._age = 0;
			this.SetAge(this._age, context);
			this.SetCurrDurability(this.MaxDurability, context);
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06004F62 RID: 20322 RVA: 0x002B3EAD File Offset: 0x002B20AD
		public bool IsAlive
		{
			get
			{
				return (int)this._age < this.CalcMaxAge() && this.CurrDurability > 0;
			}
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x002B3ECC File Offset: 0x002B20CC
		public bool Match(CricketCombatConfig config)
		{
			bool flag = !this.IsAlive;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2;
				if (config.OnlyNoInjury)
				{
					flag2 = this._injuries.Any((short x) => x > 0);
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					result = false;
				}
				else
				{
					sbyte grade = this.GetGrade();
					result = (grade >= config.MinGrade && grade <= config.MaxGrade);
				}
			}
			return result;
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x002B3F4B File Offset: 0x002B214B
		public Cricket(IRandomSource random, short colorId, short partId, int itemId) : this(0)
		{
			this.Initialize(random, colorId, partId, itemId);
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x002B3F62 File Offset: 0x002B2162
		public Cricket(IRandomSource random, short templateId, int itemId) : this(random, templateId, itemId, false)
		{
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x002B3F70 File Offset: 0x002B2170
		public Cricket(IRandomSource random, short templateId, int itemId, bool isSpecial) : this(templateId)
		{
			sbyte grade = (sbyte)templateId;
			ValueTuple<short, short> valueTuple = Cricket.GenerateRandomColorAndPart(random, grade, isSpecial);
			short colorId = valueTuple.Item1;
			short partId = valueTuple.Item2;
			this.Initialize(random, colorId, partId, itemId);
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x002B3FAC File Offset: 0x002B21AC
		private void Initialize(IRandomSource random, short colorId, short partId, int itemId)
		{
			this._colorId = colorId;
			this._partId = partId;
			this.Id = itemId;
			sbyte grade = Cricket.CalcGrade(colorId, partId);
			this.TemplateId = (short)grade;
			int durability = (int)(grade + 1) + this.CalcHp() / 20;
			durability = Math.Max(durability * random.Next(65, 136) / 100, 1);
			this.MaxDurability = (short)durability;
			this.CurrDurability = (short)durability;
			this._bestEnemyColorId = -1;
			this._bestEnemyPartId = -1;
			this._age = (sbyte)(this.CalcMaxAge() * random.Next(0, 21) / 100);
			int maxInjuriesCount = Math.Min(3, durability - 1);
			for (int i = 0; i < maxInjuriesCount; i++)
			{
				bool flag = !random.CheckPercentProb(10);
				if (!flag)
				{
					this.CurrDurability -= 1;
					bool flag2 = random.CheckPercentProb(66);
					if (flag2)
					{
						int injuryType = random.Next(2);
						short[] injuries = this._injuries;
						int num = injuryType;
						injuries[num] += 5;
					}
					else
					{
						int injuryType2 = 2 + random.Next(3);
						short[] injuries2 = this._injuries;
						int num2 = injuryType2;
						injuries2[num2] += 1;
					}
				}
			}
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x002B40D0 File Offset: 0x002B22D0
		public static void InitializeCricketWeights()
		{
			if (Cricket._cricketTemplateRates == null)
			{
				Cricket._cricketTemplateRates = new List<ValueTuple<short, short>>[27];
			}
			for (int i = 0; i < 27; i++)
			{
				List<ValueTuple<short, short>>[] cricketTemplateRates = Cricket._cricketTemplateRates;
				int num = i;
				if (cricketTemplateRates[num] == null)
				{
					cricketTemplateRates[num] = new List<ValueTuple<short, short>>();
				}
				Cricket._cricketTemplateRates[i].Clear();
			}
			int cachingIndex = 0;
			Cricket._cricketTemplateRates[cachingIndex++].AddRange(from a in CricketParts.Instance
			where a.NpcSpecialRate != 0
			select new ValueTuple<short, short>(a.TemplateId, (short)a.NpcSpecialRate));
			sbyte grade3;
			sbyte grade2;
			for (grade3 = 7; grade3 < 9; grade3 = grade2 + 1)
			{
				Cricket._cricketTemplateRates[cachingIndex++].AddRange(from a in CricketParts.Instance
				where a.Level == grade3
				select new ValueTuple<short, short>(a.TemplateId, (short)a.Rate));
				grade2 = grade3;
			}
			sbyte grade;
			for (grade = 1; grade < 7; grade = grade2 + 1)
			{
				Cricket._cricketTemplateRates[cachingIndex++].AddRange(from a in CricketParts.Instance.Where(delegate(CricketPartsItem a)
				{
					ECricketPartsType type = a.Type;
					return type >= ECricketPartsType.Cyan && type <= ECricketPartsType.White && a.Level <= grade;
				})
				select new ValueTuple<short, short>(a.TemplateId, (short)a.Rate));
				Cricket._cricketTemplateRates[cachingIndex++].AddRange(from a in CricketParts.Instance.Where(delegate(CricketPartsItem a)
				{
					ECricketPartsType type = a.Type;
					return type >= ECricketPartsType.Cyan && type <= ECricketPartsType.White && a.Level == grade;
				})
				select new ValueTuple<short, short>(a.TemplateId, (short)a.Rate));
				Cricket._cricketTemplateRates[cachingIndex++].AddRange(from a in CricketParts.Instance
				where a.Type == ECricketPartsType.Parts && a.Level <= grade
				select new ValueTuple<short, short>(a.TemplateId, (short)a.Rate));
				Cricket._cricketTemplateRates[cachingIndex++].AddRange(from a in CricketParts.Instance
				where a.Type == ECricketPartsType.Parts && a.Level == grade
				select new ValueTuple<short, short>(a.TemplateId, (short)a.Rate));
				grade2 = grade;
			}
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x002B437C File Offset: 0x002B257C
		[return: TupleElementNames(new string[]
		{
			"colorId",
			"partId"
		})]
		private static ValueTuple<short, short> GenerateRandomColorAndPart(IRandomSource random, sbyte grade, bool isSpecial)
		{
			short partId = 0;
			bool flag = grade >= 7;
			short colorId;
			if (flag)
			{
				int index = (int)(isSpecial ? 0 : (grade - 7 + 1));
				colorId = RandomUtils.GetRandomResult<short>(Cricket._cricketTemplateRates[index], random);
			}
			else
			{
				bool flag2 = grade >= 1;
				if (flag2)
				{
					bool lockPart = grade == 6 || random.CheckPercentProb(75);
					int indexRoot = (int)((grade - 1) * 4 + 3);
					int indexPart = lockPart ? (indexRoot + 3) : (indexRoot + 2);
					int indexColor = lockPart ? indexRoot : (indexRoot + 1);
					colorId = RandomUtils.GetRandomResult<short>(Cricket._cricketTemplateRates[indexColor], random);
					partId = RandomUtils.GetRandomResult<short>(Cricket._cricketTemplateRates[indexPart], random);
				}
				else
				{
					colorId = 0;
				}
			}
			return new ValueTuple<short, short>(colorId, partId);
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x002B4430 File Offset: 0x002B2630
		private static sbyte CalcGrade(short colorId, short partId)
		{
			bool flag = colorId < 0;
			sbyte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte value = CricketParts.Instance[colorId].Level;
				bool flag2 = partId > 0;
				if (flag2)
				{
					value = Math.Max(CricketParts.Instance[partId].Level, value);
				}
				result = value;
			}
			return result;
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x002B4480 File Offset: 0x002B2680
		private int CalcHp()
		{
			short value = CricketParts.Instance[this._colorId].HP;
			bool flag = this._partId > 0;
			if (flag)
			{
				value += CricketParts.Instance[this._partId].HP;
			}
			return (int)value;
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x002B44D0 File Offset: 0x002B26D0
		public int CalcMaxAge()
		{
			int value = (int)CricketParts.Instance[this._colorId].Life;
			bool flag = this._partId > 0;
			if (flag)
			{
				value += (int)CricketParts.Instance[this._partId].Life;
			}
			return value + (int)DomainManager.Extra.GetCricketExtraAge(this.Id);
		}

		// Token: 0x04001594 RID: 5524
		[CollectionObjectField(false, true, false, true, false)]
		private short _colorId;

		// Token: 0x04001595 RID: 5525
		[CollectionObjectField(false, true, false, true, false)]
		private short _partId;

		// Token: 0x04001596 RID: 5526
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 5)]
		private short[] _injuries;

		// Token: 0x04001597 RID: 5527
		[CollectionObjectField(false, true, false, false, false)]
		private short _winsCount;

		// Token: 0x04001598 RID: 5528
		[CollectionObjectField(false, true, false, false, false)]
		private short _lossesCount;

		// Token: 0x04001599 RID: 5529
		[CollectionObjectField(false, true, false, false, false)]
		private short _bestEnemyColorId;

		// Token: 0x0400159A RID: 5530
		[CollectionObjectField(false, true, false, false, false)]
		private short _bestEnemyPartId;

		// Token: 0x0400159B RID: 5531
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _age;

		// Token: 0x0400159C RID: 5532
		public const int FixedSize = 34;

		// Token: 0x0400159D RID: 5533
		public const int DynamicCount = 0;

		// Token: 0x0400159E RID: 5534
		[TupleElementNames(new string[]
		{
			"templateId",
			"rate"
		})]
		private static List<ValueTuple<short, short>>[] _cricketTemplateRates;

		// Token: 0x02000AA7 RID: 2727
		internal class FixedFieldInfos
		{
			// Token: 0x04002BE5 RID: 11237
			public const uint Id_Offset = 0U;

			// Token: 0x04002BE6 RID: 11238
			public const int Id_Size = 4;

			// Token: 0x04002BE7 RID: 11239
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002BE8 RID: 11240
			public const int TemplateId_Size = 2;

			// Token: 0x04002BE9 RID: 11241
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002BEA RID: 11242
			public const int MaxDurability_Size = 2;

			// Token: 0x04002BEB RID: 11243
			public const uint CurrDurability_Offset = 8U;

			// Token: 0x04002BEC RID: 11244
			public const int CurrDurability_Size = 2;

			// Token: 0x04002BED RID: 11245
			public const uint ModificationState_Offset = 10U;

			// Token: 0x04002BEE RID: 11246
			public const int ModificationState_Size = 1;

			// Token: 0x04002BEF RID: 11247
			public const uint ColorId_Offset = 11U;

			// Token: 0x04002BF0 RID: 11248
			public const int ColorId_Size = 2;

			// Token: 0x04002BF1 RID: 11249
			public const uint PartId_Offset = 13U;

			// Token: 0x04002BF2 RID: 11250
			public const int PartId_Size = 2;

			// Token: 0x04002BF3 RID: 11251
			public const uint Injuries_Offset = 15U;

			// Token: 0x04002BF4 RID: 11252
			public const int Injuries_Size = 10;

			// Token: 0x04002BF5 RID: 11253
			public const uint WinsCount_Offset = 25U;

			// Token: 0x04002BF6 RID: 11254
			public const int WinsCount_Size = 2;

			// Token: 0x04002BF7 RID: 11255
			public const uint LossesCount_Offset = 27U;

			// Token: 0x04002BF8 RID: 11256
			public const int LossesCount_Size = 2;

			// Token: 0x04002BF9 RID: 11257
			public const uint BestEnemyColorId_Offset = 29U;

			// Token: 0x04002BFA RID: 11258
			public const int BestEnemyColorId_Size = 2;

			// Token: 0x04002BFB RID: 11259
			public const uint BestEnemyPartId_Offset = 31U;

			// Token: 0x04002BFC RID: 11260
			public const int BestEnemyPartId_Size = 2;

			// Token: 0x04002BFD RID: 11261
			public const uint Age_Offset = 33U;

			// Token: 0x04002BFE RID: 11262
			public const int Age_Size = 1;
		}
	}
}
