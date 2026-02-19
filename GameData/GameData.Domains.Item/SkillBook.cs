using System;
using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Item;

[SerializableGameData(NotForDisplayModule = true)]
public class SkillBook : ItemBase, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 4;

		public const uint TemplateId_Offset = 4u;

		public const int TemplateId_Size = 2;

		public const uint MaxDurability_Offset = 6u;

		public const int MaxDurability_Size = 2;

		public const uint CurrDurability_Offset = 8u;

		public const int CurrDurability_Size = 2;

		public const uint ModificationState_Offset = 10u;

		public const int ModificationState_Size = 1;

		public const uint PageTypes_Offset = 11u;

		public const int PageTypes_Size = 1;

		public const uint PageIncompleteState_Offset = 12u;

		public const int PageIncompleteState_Size = 2;
	}

	[CollectionObjectField(false, true, false, false, false)]
	private byte _pageTypes;

	[CollectionObjectField(false, true, false, false, false)]
	private ushort _pageIncompleteState;

	public const int FixedSize = 14;

	public const int DynamicCount = 0;

	public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
	{
		return 0;
	}

	public byte GetPageCount()
	{
		return (byte)((GetLifeSkillType() >= 0) ? 5 : 6);
	}

	public bool IsCombatSkillBook()
	{
		return GetCombatSkillType() >= 0;
	}

	public bool CanFix()
	{
		ushort pageIncompleteState = GetPageIncompleteState();
		int num = (IsCombatSkillBook() ? 6 : 5);
		bool result = false;
		for (byte b = 0; b < num; b++)
		{
			sbyte pageIncompleteState2 = SkillBookStateHelper.GetPageIncompleteState(pageIncompleteState, b);
			if (pageIncompleteState2 == 1 || pageIncompleteState2 == 2)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public (sbyte pageNum, short needProgress) GetFixProgress()
	{
		int num = (IsCombatSkillBook() ? 6 : 5);
		ushort pageIncompleteState = GetPageIncompleteState();
		sbyte grade = ItemTemplateHelper.GetGrade(10, TemplateId);
		short num2 = GlobalConfig.Instance.FixBookTotalProgress[grade];
		sbyte item = -1;
		for (sbyte b = 0; b < num; b++)
		{
			switch (SkillBookStateHelper.GetPageIncompleteState(pageIncompleteState, (byte)b))
			{
			case 1:
				item = b;
				break;
			case 2:
				item = b;
				num2 *= 3;
				break;
			default:
				continue;
			}
			break;
		}
		num2 = Math.Min(short.MaxValue, num2);
		return (pageNum: item, needProgress: num2);
	}

	public void SetOutlinePageType(DataContext context, sbyte behaviorType)
	{
		_pageTypes = SkillBookStateHelper.SetOutlinePageType(_pageTypes, behaviorType);
		SetPageTypes(_pageTypes, context);
	}

	public void SetNormalPageType(DataContext context, byte pageId, sbyte direction)
	{
		_pageTypes = SkillBookStateHelper.SetNormalPageType(_pageTypes, pageId, direction);
		SetPageTypes(_pageTypes, context);
	}

	public SkillBook(IRandomSource random, short templateId, int itemId, sbyte completePagesCount = -1, sbyte lostPagesCount = -1, sbyte outlinePageType = -1, sbyte normalPagesDirectProb = 50, bool outlineAlwaysComplete = true)
		: this(templateId)
	{
		Id = itemId;
		MaxDurability = ItemBase.GenerateMaxDurability(random, MaxDurability);
		CurrDurability = MaxDurability;
		SkillBookItem skillBookItem = Config.SkillBook.Instance[TemplateId];
		sbyte b = SkillGroup.FromItemSubType(skillBookItem.ItemSubType);
		if (b == 1)
		{
			_pageTypes = GenerateCombatPageTypes(random, outlinePageType, normalPagesDirectProb);
		}
		_pageIncompleteState = GeneratePageIncompleteState(random, b, skillBookItem.Grade, completePagesCount, lostPagesCount, outlineAlwaysComplete);
	}

	public SkillBook(IRandomSource random, short templateId, int itemId, byte pageTypes, sbyte completePagesCount = -1, sbyte lostPagesCount = -1, bool outlineAlwaysComplete = true)
		: this(templateId)
	{
		Id = itemId;
		MaxDurability = ItemBase.GenerateMaxDurability(random, MaxDurability);
		CurrDurability = MaxDurability;
		SkillBookItem skillBookItem = Config.SkillBook.Instance[TemplateId];
		sbyte skillGroup = SkillGroup.FromItemSubType(skillBookItem.ItemSubType);
		sbyte behaviorType = (sbyte)Math.Clamp((int)SkillBookStateHelper.GetOutlinePageType(pageTypes), 0, 4);
		_pageTypes = SkillBookStateHelper.SetOutlinePageType(pageTypes, behaviorType);
		_pageIncompleteState = GeneratePageIncompleteState(random, skillGroup, skillBookItem.Grade, completePagesCount, lostPagesCount, outlineAlwaysComplete);
	}

	public SkillBook(IRandomSource random, short templateId, int itemId, ushort activationState)
		: this(templateId)
	{
		Id = itemId;
		MaxDurability = ItemBase.GenerateMaxDurability(random, MaxDurability);
		CurrDurability = MaxDurability;
		SkillBookItem skillBookItem = Config.SkillBook.Instance[TemplateId];
		_pageTypes = GenerateCombatPageTypes(activationState);
		_pageIncompleteState = GeneratePageIncompleteState(random, 1, skillBookItem.Grade, -1, -1, outlineAlwaysComplete: true);
	}

	public static byte GenerateCombatPageTypes(IRandomSource random, sbyte outlinePageType, sbyte normalPagesDirectProb)
	{
		byte pageTypes = 0;
		if (outlinePageType == -1)
		{
			outlinePageType = GameData.Domains.Character.BehaviorType.GetRandomBehaviorType(random);
		}
		outlinePageType = (sbyte)Math.Clamp((int)outlinePageType, 0, 4);
		pageTypes = SkillBookStateHelper.SetOutlinePageType(pageTypes, outlinePageType);
		for (byte b = 1; b < 6; b++)
		{
			sbyte direction = ((!random.CheckPercentProb(normalPagesDirectProb)) ? ((sbyte)1) : ((sbyte)0));
			pageTypes = SkillBookStateHelper.SetNormalPageType(pageTypes, b, direction);
		}
		return pageTypes;
	}

	public static byte GenerateCombatPageTypes(ushort activationState)
	{
		byte pageTypes = 0;
		sbyte activeOutlinePageType = CombatSkillStateHelper.GetActiveOutlinePageType(activationState);
		if (activeOutlinePageType == -1)
		{
			throw new Exception("Failed to get the type of the active outline page");
		}
		activeOutlinePageType = (sbyte)Math.Clamp((int)activeOutlinePageType, 0, 4);
		pageTypes = SkillBookStateHelper.SetOutlinePageType(pageTypes, activeOutlinePageType);
		for (byte b = 1; b < 6; b++)
		{
			sbyte pageActiveDirection = CombatSkillStateHelper.GetPageActiveDirection(activationState, b);
			if (pageActiveDirection == -1)
			{
				throw new Exception("Failed to get the type of the active normal page");
			}
			pageTypes = SkillBookStateHelper.SetNormalPageType(pageTypes, b, pageActiveDirection);
		}
		return pageTypes;
	}

	public unsafe static ushort GeneratePageIncompleteState(IRandomSource random, sbyte skillGroup, sbyte grade, sbyte completePagesCount, sbyte lostPagesCount, bool outlineAlwaysComplete)
	{
		int num = ((skillGroup == 1) ? 5 : 5);
		if (completePagesCount < 0)
		{
			float num2 = 3f - (float)grade / 4f;
			float num3 = Math.Max(0f, num2 - 1f);
			float num4 = Math.Min(num, num2 + 1f);
			completePagesCount = ((!(num3 > num4)) ? ((sbyte)Math.Round(RedzenHelper.NormalDistribute(random, num2, 0.5f, num3, num4))) : ((sbyte)num4));
		}
		if (lostPagesCount < 0)
		{
			float num5 = -1f + (float)grade / 2.667f;
			float num6 = Math.Max(0f, num5 - 1f);
			float num7 = Math.Min(num - completePagesCount, num5 + 1f);
			lostPagesCount = ((!(num6 > num7)) ? ((sbyte)Math.Round(RedzenHelper.NormalDistribute(random, num5, 0.5f, num6, num7))) : ((sbyte)num7));
		}
		int num8 = num - completePagesCount - lostPagesCount;
		if (num8 < 0)
		{
			throw new Exception($"IncompletePagesCount is less than zero: {num8}");
		}
		sbyte* ptr = stackalloc sbyte[(int)(uint)num];
		for (int i = 0; i < num; i++)
		{
			ptr[i] = -1;
		}
		for (int j = 0; j < num; j++)
		{
			if (completePagesCount <= 0)
			{
				break;
			}
			int percentProb = 70 - j * 10;
			if (random.CheckPercentProb(percentProb))
			{
				ptr[j] = 0;
				completePagesCount--;
			}
		}
		int num9 = completePagesCount + num8 + lostPagesCount;
		sbyte* ptr2 = stackalloc sbyte[(int)(uint)num9];
		for (int k = 0; k < completePagesCount; k++)
		{
			ptr2[k] = 0;
		}
		for (sbyte b = completePagesCount; b < completePagesCount + num8; b++)
		{
			ptr2[b] = 1;
		}
		for (int l = completePagesCount + num8; l < num9; l++)
		{
			ptr2[l] = 2;
		}
		CollectionUtils.Shuffle(random, ptr2, num9);
		int m = 0;
		int num10 = 0;
		for (; m < num; m++)
		{
			if (ptr[m] == -1)
			{
				ptr[m] = ptr2[num10++];
			}
		}
		ushort num11 = 0;
		byte b2 = 0;
		if (skillGroup == 1)
		{
			sbyte state = (sbyte)((!outlineAlwaysComplete && random.CheckPercentProb(90)) ? 2 : 0);
			num11 = SkillBookStateHelper.SetPageIncompleteState(num11, 0, state);
			b2 = 1;
		}
		for (int n = 0; n < num; n++)
		{
			byte pageId = (byte)(b2 + n);
			sbyte state2 = ptr[n];
			num11 = SkillBookStateHelper.SetPageIncompleteState(num11, pageId, state2);
		}
		return num11;
	}

	public unsafe override void SetMaxDurability(short maxDurability, DataContext context)
	{
		MaxDurability = maxDurability;
		SetModifiedAndInvalidateInfluencedCache(2, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 6u, 2);
			*(short*)ptr = MaxDurability;
			ptr += 2;
		}
	}

	public unsafe override void SetCurrDurability(short currDurability, DataContext context)
	{
		CurrDurability = currDurability;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 8u, 2);
			*(short*)ptr = CurrDurability;
			ptr += 2;
		}
	}

	public unsafe override void SetModificationState(byte modificationState, DataContext context)
	{
		ModificationState = modificationState;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 10u, 1);
			*ptr = ModificationState;
			ptr++;
		}
	}

	public byte GetPageTypes()
	{
		return _pageTypes;
	}

	public unsafe void SetPageTypes(byte pageTypes, DataContext context)
	{
		_pageTypes = pageTypes;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 11u, 1);
			*ptr = _pageTypes;
			ptr++;
		}
	}

	public ushort GetPageIncompleteState()
	{
		return _pageIncompleteState;
	}

	public unsafe void SetPageIncompleteState(ushort pageIncompleteState, DataContext context)
	{
		_pageIncompleteState = pageIncompleteState;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 12u, 2);
			*(ushort*)ptr = _pageIncompleteState;
			ptr += 2;
		}
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetName()
	{
		return Config.SkillBook.Instance[TemplateId].Name;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetItemType()
	{
		return Config.SkillBook.Instance[TemplateId].ItemType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetItemSubType()
	{
		return Config.SkillBook.Instance[TemplateId].ItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetGrade()
	{
		return Config.SkillBook.Instance[TemplateId].Grade;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetIcon()
	{
		return Config.SkillBook.Instance[TemplateId].Icon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetDesc()
	{
		return Config.SkillBook.Instance[TemplateId].Desc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetTransferable()
	{
		return Config.SkillBook.Instance[TemplateId].Transferable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetStackable()
	{
		return Config.SkillBook.Instance[TemplateId].Stackable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetWagerable()
	{
		return Config.SkillBook.Instance[TemplateId].Wagerable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRefinable()
	{
		return Config.SkillBook.Instance[TemplateId].Refinable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetPoisonable()
	{
		return Config.SkillBook.Instance[TemplateId].Poisonable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRepairable()
	{
		return Config.SkillBook.Instance[TemplateId].Repairable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseWeight()
	{
		return Config.SkillBook.Instance[TemplateId].BaseWeight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseValue()
	{
		return Config.SkillBook.Instance[TemplateId].BaseValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBasePrice()
	{
		return Config.SkillBook.Instance[TemplateId].BasePrice;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetDropRate()
	{
		return Config.SkillBook.Instance[TemplateId].DropRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetResourceType()
	{
		return Config.SkillBook.Instance[TemplateId].ResourceType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetPreservationDuration()
	{
		return Config.SkillBook.Instance[TemplateId].PreservationDuration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetLifeSkillType()
	{
		return Config.SkillBook.Instance[TemplateId].LifeSkillType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetLifeSkillTemplateId()
	{
		return Config.SkillBook.Instance[TemplateId].LifeSkillTemplateId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetCombatSkillType()
	{
		return Config.SkillBook.Instance[TemplateId].CombatSkillType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetCombatSkillTemplateId()
	{
		return Config.SkillBook.Instance[TemplateId].CombatSkillTemplateId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetLegacyPoint()
	{
		return Config.SkillBook.Instance[TemplateId].LegacyPoint;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<short> GetReferenceBooksWithBonus()
	{
		return Config.SkillBook.Instance[TemplateId].ReferenceBooksWithBonus;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetGiftLevel()
	{
		return Config.SkillBook.Instance[TemplateId].GiftLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseFavorabilityChange()
	{
		return Config.SkillBook.Instance[TemplateId].BaseFavorabilityChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetBaseHappinessChange()
	{
		return Config.SkillBook.Instance[TemplateId].BaseHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRandomCreate()
	{
		return Config.SkillBook.Instance[TemplateId].AllowRandomCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsSpecial()
	{
		return Config.SkillBook.Instance[TemplateId].IsSpecial;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.SkillBook.Instance[TemplateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInheritable()
	{
		return Config.SkillBook.Instance[TemplateId].Inheritable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetBreakBonusEffect()
	{
		return Config.SkillBook.Instance[TemplateId].BreakBonusEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMerchantLevel()
	{
		return Config.SkillBook.Instance[TemplateId].MerchantLevel;
	}

	public SkillBook()
	{
	}

	public SkillBook(short templateId)
	{
		SkillBookItem skillBookItem = Config.SkillBook.Instance[templateId];
		TemplateId = skillBookItem.TemplateId;
		MaxDurability = skillBookItem.MaxDurability;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 14;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*(short*)ptr = MaxDurability;
		ptr += 2;
		*(short*)ptr = CurrDurability;
		ptr += 2;
		*ptr = ModificationState;
		ptr++;
		*ptr = _pageTypes;
		ptr++;
		*(ushort*)ptr = _pageIncompleteState;
		ptr += 2;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		MaxDurability = *(short*)ptr;
		ptr += 2;
		CurrDurability = *(short*)ptr;
		ptr += 2;
		ModificationState = *ptr;
		ptr++;
		_pageTypes = *ptr;
		ptr++;
		_pageIncompleteState = *(ushort*)ptr;
		ptr += 2;
		return (int)(ptr - pData);
	}
}
