using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x02000671 RID: 1649
	[SerializableGameData(NotForDisplayModule = true)]
	public class SkillBook : ItemBase, ISerializableGameData
	{
		// Token: 0x0600523C RID: 21052 RVA: 0x002CD518 File Offset: 0x002CB718
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return 0;
		}

		// Token: 0x0600523D RID: 21053 RVA: 0x002CD52C File Offset: 0x002CB72C
		public byte GetPageCount()
		{
			return (this.GetLifeSkillType() >= 0) ? 5 : 6;
		}

		// Token: 0x0600523E RID: 21054 RVA: 0x002CD54C File Offset: 0x002CB74C
		public bool IsCombatSkillBook()
		{
			return this.GetCombatSkillType() >= 0;
		}

		// Token: 0x0600523F RID: 21055 RVA: 0x002CD56C File Offset: 0x002CB76C
		public bool CanFix()
		{
			ushort pageIncompleteState = this.GetPageIncompleteState();
			int pageCount = this.IsCombatSkillBook() ? 6 : 5;
			bool canFix = false;
			byte i = 0;
			while ((int)i < pageCount)
			{
				sbyte state = SkillBookStateHelper.GetPageIncompleteState(pageIncompleteState, i);
				bool flag = state == 1 || state == 2;
				if (flag)
				{
					canFix = true;
					break;
				}
				i += 1;
			}
			return canFix;
		}

		// Token: 0x06005240 RID: 21056 RVA: 0x002CD5D0 File Offset: 0x002CB7D0
		[return: TupleElementNames(new string[]
		{
			"pageNum",
			"needProgress"
		})]
		public ValueTuple<sbyte, short> GetFixProgress()
		{
			int pageCount = this.IsCombatSkillBook() ? 6 : 5;
			ushort pageIncompleteState = this.GetPageIncompleteState();
			sbyte grade = ItemTemplateHelper.GetGrade(10, this.TemplateId);
			short needProgress = GlobalConfig.Instance.FixBookTotalProgress[(int)grade];
			sbyte incompletePage = -1;
			sbyte i = 0;
			while ((int)i < pageCount)
			{
				sbyte state = SkillBookStateHelper.GetPageIncompleteState(pageIncompleteState, (byte)i);
				bool flag = state == 1;
				if (flag)
				{
					incompletePage = i;
					break;
				}
				bool flag2 = state == 2;
				if (flag2)
				{
					incompletePage = i;
					needProgress *= 3;
					break;
				}
				i += 1;
			}
			needProgress = Math.Min(short.MaxValue, needProgress);
			return new ValueTuple<sbyte, short>(incompletePage, needProgress);
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x002CD677 File Offset: 0x002CB877
		public void SetOutlinePageType(DataContext context, sbyte behaviorType)
		{
			this._pageTypes = SkillBookStateHelper.SetOutlinePageType(this._pageTypes, behaviorType);
			this.SetPageTypes(this._pageTypes, context);
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x002CD69A File Offset: 0x002CB89A
		public void SetNormalPageType(DataContext context, byte pageId, sbyte direction)
		{
			this._pageTypes = SkillBookStateHelper.SetNormalPageType(this._pageTypes, pageId, direction);
			this.SetPageTypes(this._pageTypes, context);
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x002CD6C0 File Offset: 0x002CB8C0
		public SkillBook(IRandomSource random, short templateId, int itemId, sbyte completePagesCount = -1, sbyte lostPagesCount = -1, sbyte outlinePageType = -1, sbyte normalPagesDirectProb = 50, bool outlineAlwaysComplete = true) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
			SkillBookItem config = SkillBook.Instance[this.TemplateId];
			sbyte skillGroup = SkillGroup.FromItemSubType(config.ItemSubType);
			bool flag = skillGroup == 1;
			if (flag)
			{
				this._pageTypes = SkillBook.GenerateCombatPageTypes(random, outlinePageType, normalPagesDirectProb);
			}
			this._pageIncompleteState = SkillBook.GeneratePageIncompleteState(random, skillGroup, config.Grade, completePagesCount, lostPagesCount, outlineAlwaysComplete);
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x002CD74C File Offset: 0x002CB94C
		public SkillBook(IRandomSource random, short templateId, int itemId, byte pageTypes, sbyte completePagesCount = -1, sbyte lostPagesCount = -1, bool outlineAlwaysComplete = true) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
			SkillBookItem config = SkillBook.Instance[this.TemplateId];
			sbyte skillGroup = SkillGroup.FromItemSubType(config.ItemSubType);
			sbyte outlinePageType = (sbyte)Math.Clamp((int)SkillBookStateHelper.GetOutlinePageType(pageTypes), 0, 4);
			this._pageTypes = SkillBookStateHelper.SetOutlinePageType(pageTypes, outlinePageType);
			this._pageIncompleteState = SkillBook.GeneratePageIncompleteState(random, skillGroup, config.Grade, completePagesCount, lostPagesCount, outlineAlwaysComplete);
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x002CD7DC File Offset: 0x002CB9DC
		public SkillBook(IRandomSource random, short templateId, int itemId, ushort activationState) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
			SkillBookItem config = SkillBook.Instance[this.TemplateId];
			this._pageTypes = SkillBook.GenerateCombatPageTypes(activationState);
			this._pageIncompleteState = SkillBook.GeneratePageIncompleteState(random, 1, config.Grade, -1, -1, true);
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x002CD84C File Offset: 0x002CBA4C
		public static byte GenerateCombatPageTypes(IRandomSource random, sbyte outlinePageType, sbyte normalPagesDirectProb)
		{
			byte pageTypes = 0;
			bool flag = outlinePageType == -1;
			if (flag)
			{
				outlinePageType = GameData.Domains.Character.BehaviorType.GetRandomBehaviorType(random);
			}
			outlinePageType = (sbyte)Math.Clamp((int)outlinePageType, 0, 4);
			pageTypes = SkillBookStateHelper.SetOutlinePageType(pageTypes, outlinePageType);
			for (byte pageId = 1; pageId < 6; pageId += 1)
			{
				sbyte direction = random.CheckPercentProb((int)normalPagesDirectProb) ? 0 : 1;
				pageTypes = SkillBookStateHelper.SetNormalPageType(pageTypes, pageId, direction);
			}
			return pageTypes;
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x002CD8B4 File Offset: 0x002CBAB4
		public static byte GenerateCombatPageTypes(ushort activationState)
		{
			byte pageTypes = 0;
			sbyte outlinePageType = CombatSkillStateHelper.GetActiveOutlinePageType(activationState);
			bool flag = outlinePageType == -1;
			if (flag)
			{
				throw new Exception("Failed to get the type of the active outline page");
			}
			outlinePageType = (sbyte)Math.Clamp((int)outlinePageType, 0, 4);
			pageTypes = SkillBookStateHelper.SetOutlinePageType(pageTypes, outlinePageType);
			for (byte pageId = 1; pageId < 6; pageId += 1)
			{
				sbyte direction = CombatSkillStateHelper.GetPageActiveDirection(activationState, pageId);
				bool flag2 = direction == -1;
				if (flag2)
				{
					throw new Exception("Failed to get the type of the active normal page");
				}
				pageTypes = SkillBookStateHelper.SetNormalPageType(pageTypes, pageId, direction);
			}
			return pageTypes;
		}

		// Token: 0x06005248 RID: 21064 RVA: 0x002CD938 File Offset: 0x002CBB38
		public unsafe static ushort GeneratePageIncompleteState(IRandomSource random, sbyte skillGroup, sbyte grade, sbyte completePagesCount, sbyte lostPagesCount, bool outlineAlwaysComplete)
		{
			int normalPagesCount = (skillGroup == 1) ? 5 : 5;
			bool flag = completePagesCount < 0;
			if (flag)
			{
				float mean = 3f - (float)grade / 4f;
				float min = Math.Max(0f, mean - 1f);
				float max = Math.Min((float)normalPagesCount, mean + 1f);
				bool flag2 = min > max;
				if (flag2)
				{
					completePagesCount = (sbyte)max;
				}
				else
				{
					completePagesCount = (sbyte)Math.Round((double)RedzenHelper.NormalDistribute(random, mean, 0.5f, min, max));
				}
			}
			bool flag3 = lostPagesCount < 0;
			if (flag3)
			{
				float mean2 = -1f + (float)grade / 2.667f;
				float min2 = Math.Max(0f, mean2 - 1f);
				float max2 = Math.Min((float)(normalPagesCount - (int)completePagesCount), mean2 + 1f);
				bool flag4 = min2 > max2;
				if (flag4)
				{
					lostPagesCount = (sbyte)max2;
				}
				else
				{
					lostPagesCount = (sbyte)Math.Round((double)RedzenHelper.NormalDistribute(random, mean2, 0.5f, min2, max2));
				}
			}
			int incompletePagesCount = normalPagesCount - (int)completePagesCount - (int)lostPagesCount;
			bool flag5 = incompletePagesCount < 0;
			if (flag5)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("IncompletePagesCount is less than zero: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(incompletePagesCount);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			sbyte* pStates = stackalloc sbyte[(UIntPtr)normalPagesCount];
			for (int i = 0; i < normalPagesCount; i++)
			{
				pStates[i] = -1;
			}
			int j = 0;
			while (j < normalPagesCount && completePagesCount > 0)
			{
				int completeProb = 70 - j * 10;
				bool flag6 = random.CheckPercentProb(completeProb);
				if (flag6)
				{
					pStates[j] = 0;
					completePagesCount -= 1;
				}
				j++;
			}
			int leftPagesCount = (int)completePagesCount + incompletePagesCount + (int)lostPagesCount;
			sbyte* pLeftStates = stackalloc sbyte[(UIntPtr)leftPagesCount];
			for (int k = 0; k < (int)completePagesCount; k++)
			{
				pLeftStates[k] = 0;
			}
			sbyte l = completePagesCount;
			while ((int)l < (int)completePagesCount + incompletePagesCount)
			{
				pLeftStates[l] = 1;
				l += 1;
			}
			for (int m = (int)completePagesCount + incompletePagesCount; m < leftPagesCount; m++)
			{
				pLeftStates[m] = 2;
			}
			CollectionUtils.Shuffle<sbyte>(random, pLeftStates, leftPagesCount);
			int n = 0;
			int leftPageId = 0;
			while (n < normalPagesCount)
			{
				bool flag7 = pStates[n] == -1;
				if (flag7)
				{
					pStates[n] = pLeftStates[leftPageId++];
				}
				n++;
			}
			ushort states = 0;
			byte pageBeginId = 0;
			bool flag8 = skillGroup == 1;
			if (flag8)
			{
				sbyte outlineState = (!outlineAlwaysComplete && random.CheckPercentProb(90)) ? 2 : 0;
				states = SkillBookStateHelper.SetPageIncompleteState(states, 0, outlineState);
				pageBeginId = 1;
			}
			for (int i2 = 0; i2 < normalPagesCount; i2++)
			{
				byte pageId = (byte)((int)pageBeginId + i2);
				sbyte state = pStates[i2];
				states = SkillBookStateHelper.SetPageIncompleteState(states, pageId, state);
			}
			return states;
		}

		// Token: 0x06005249 RID: 21065 RVA: 0x002CDBF0 File Offset: 0x002CBDF0
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

		// Token: 0x0600524A RID: 21066 RVA: 0x002CDC50 File Offset: 0x002CBE50
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

		// Token: 0x0600524B RID: 21067 RVA: 0x002CDCB0 File Offset: 0x002CBEB0
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

		// Token: 0x0600524C RID: 21068 RVA: 0x002CDD10 File Offset: 0x002CBF10
		public byte GetPageTypes()
		{
			return this._pageTypes;
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x002CDD28 File Offset: 0x002CBF28
		public unsafe void SetPageTypes(byte pageTypes, DataContext context)
		{
			this._pageTypes = pageTypes;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 11U, 1);
				*pData = this._pageTypes;
				pData++;
			}
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x002CDD88 File Offset: 0x002CBF88
		public ushort GetPageIncompleteState()
		{
			return this._pageIncompleteState;
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x002CDDA0 File Offset: 0x002CBFA0
		public unsafe void SetPageIncompleteState(ushort pageIncompleteState, DataContext context)
		{
			this._pageIncompleteState = pageIncompleteState;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 12U, 2);
				*(short*)pData = (short)this._pageIncompleteState;
				pData += 2;
			}
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x002CDE00 File Offset: 0x002CC000
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return SkillBook.Instance[this.TemplateId].Name;
		}

		// Token: 0x06005251 RID: 21073 RVA: 0x002CDE28 File Offset: 0x002CC028
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return SkillBook.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x06005252 RID: 21074 RVA: 0x002CDE50 File Offset: 0x002CC050
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return SkillBook.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x002CDE78 File Offset: 0x002CC078
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return SkillBook.Instance[this.TemplateId].Grade;
		}

		// Token: 0x06005254 RID: 21076 RVA: 0x002CDEA0 File Offset: 0x002CC0A0
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return SkillBook.Instance[this.TemplateId].Icon;
		}

		// Token: 0x06005255 RID: 21077 RVA: 0x002CDEC8 File Offset: 0x002CC0C8
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return SkillBook.Instance[this.TemplateId].Desc;
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x002CDEF0 File Offset: 0x002CC0F0
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return SkillBook.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x002CDF18 File Offset: 0x002CC118
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return SkillBook.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x002CDF40 File Offset: 0x002CC140
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return SkillBook.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x002CDF68 File Offset: 0x002CC168
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return SkillBook.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x002CDF90 File Offset: 0x002CC190
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return SkillBook.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x002CDFB8 File Offset: 0x002CC1B8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return SkillBook.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x002CDFE0 File Offset: 0x002CC1E0
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return SkillBook.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x002CE008 File Offset: 0x002CC208
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return SkillBook.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x0600525E RID: 21086 RVA: 0x002CE030 File Offset: 0x002CC230
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return SkillBook.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x0600525F RID: 21087 RVA: 0x002CE058 File Offset: 0x002CC258
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return SkillBook.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x06005260 RID: 21088 RVA: 0x002CE080 File Offset: 0x002CC280
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return SkillBook.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x06005261 RID: 21089 RVA: 0x002CE0A8 File Offset: 0x002CC2A8
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return SkillBook.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x002CE0D0 File Offset: 0x002CC2D0
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetLifeSkillType()
		{
			return SkillBook.Instance[this.TemplateId].LifeSkillType;
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x002CE0F8 File Offset: 0x002CC2F8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetLifeSkillTemplateId()
		{
			return SkillBook.Instance[this.TemplateId].LifeSkillTemplateId;
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x002CE120 File Offset: 0x002CC320
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetCombatSkillType()
		{
			return SkillBook.Instance[this.TemplateId].CombatSkillType;
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x002CE148 File Offset: 0x002CC348
		[CollectionObjectField(true, false, false, false, false)]
		public short GetCombatSkillTemplateId()
		{
			return SkillBook.Instance[this.TemplateId].CombatSkillTemplateId;
		}

		// Token: 0x06005266 RID: 21094 RVA: 0x002CE170 File Offset: 0x002CC370
		[CollectionObjectField(true, false, false, false, false)]
		public short GetLegacyPoint()
		{
			return SkillBook.Instance[this.TemplateId].LegacyPoint;
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x002CE198 File Offset: 0x002CC398
		[CollectionObjectField(true, false, false, false, false)]
		public List<short> GetReferenceBooksWithBonus()
		{
			return SkillBook.Instance[this.TemplateId].ReferenceBooksWithBonus;
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x002CE1C0 File Offset: 0x002CC3C0
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return SkillBook.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x002CE1E8 File Offset: 0x002CC3E8
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return SkillBook.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x0600526A RID: 21098 RVA: 0x002CE210 File Offset: 0x002CC410
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return SkillBook.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x0600526B RID: 21099 RVA: 0x002CE238 File Offset: 0x002CC438
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return SkillBook.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x002CE260 File Offset: 0x002CC460
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return SkillBook.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x002CE288 File Offset: 0x002CC488
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return SkillBook.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x002CE2B0 File Offset: 0x002CC4B0
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return SkillBook.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x002CE2D8 File Offset: 0x002CC4D8
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetBreakBonusEffect()
		{
			return SkillBook.Instance[this.TemplateId].BreakBonusEffect;
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x002CE300 File Offset: 0x002CC500
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return SkillBook.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x06005271 RID: 21105 RVA: 0x002CE327 File Offset: 0x002CC527
		public SkillBook()
		{
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x002CE334 File Offset: 0x002CC534
		public SkillBook(short templateId)
		{
			SkillBookItem template = SkillBook.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x002CE370 File Offset: 0x002CC570
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x002CE384 File Offset: 0x002CC584
		public int GetSerializedSize()
		{
			return 14;
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x002CE39C File Offset: 0x002CC59C
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
			*pCurrData = this._pageTypes;
			pCurrData++;
			*(short*)pCurrData = (short)this._pageIncompleteState;
			pCurrData += 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x002CE40C File Offset: 0x002CC60C
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
			this._pageTypes = *pCurrData;
			pCurrData++;
			this._pageIncompleteState = *(ushort*)pCurrData;
			pCurrData += 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04001610 RID: 5648
		[CollectionObjectField(false, true, false, false, false)]
		private byte _pageTypes;

		// Token: 0x04001611 RID: 5649
		[CollectionObjectField(false, true, false, false, false)]
		private ushort _pageIncompleteState;

		// Token: 0x04001612 RID: 5650
		public const int FixedSize = 14;

		// Token: 0x04001613 RID: 5651
		public const int DynamicCount = 0;

		// Token: 0x02000AC4 RID: 2756
		internal class FixedFieldInfos
		{
			// Token: 0x04002CAC RID: 11436
			public const uint Id_Offset = 0U;

			// Token: 0x04002CAD RID: 11437
			public const int Id_Size = 4;

			// Token: 0x04002CAE RID: 11438
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002CAF RID: 11439
			public const int TemplateId_Size = 2;

			// Token: 0x04002CB0 RID: 11440
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002CB1 RID: 11441
			public const int MaxDurability_Size = 2;

			// Token: 0x04002CB2 RID: 11442
			public const uint CurrDurability_Offset = 8U;

			// Token: 0x04002CB3 RID: 11443
			public const int CurrDurability_Size = 2;

			// Token: 0x04002CB4 RID: 11444
			public const uint ModificationState_Offset = 10U;

			// Token: 0x04002CB5 RID: 11445
			public const int ModificationState_Size = 1;

			// Token: 0x04002CB6 RID: 11446
			public const uint PageTypes_Offset = 11U;

			// Token: 0x04002CB7 RID: 11447
			public const int PageTypes_Size = 1;

			// Token: 0x04002CB8 RID: 11448
			public const uint PageIncompleteState_Offset = 12U;

			// Token: 0x04002CB9 RID: 11449
			public const int PageIncompleteState_Size = 2;
		}
	}
}
