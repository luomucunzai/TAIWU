using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Item;

[SerializableGameData(NotForDisplayModule = true)]
public class Cricket : ItemBase, ISerializableGameData
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

		public const uint ColorId_Offset = 11u;

		public const int ColorId_Size = 2;

		public const uint PartId_Offset = 13u;

		public const int PartId_Size = 2;

		public const uint Injuries_Offset = 15u;

		public const int Injuries_Size = 10;

		public const uint WinsCount_Offset = 25u;

		public const int WinsCount_Size = 2;

		public const uint LossesCount_Offset = 27u;

		public const int LossesCount_Size = 2;

		public const uint BestEnemyColorId_Offset = 29u;

		public const int BestEnemyColorId_Size = 2;

		public const uint BestEnemyPartId_Offset = 31u;

		public const int BestEnemyPartId_Size = 2;

		public const uint Age_Offset = 33u;

		public const int Age_Size = 1;
	}

	[CollectionObjectField(false, true, false, true, false)]
	private short _colorId;

	[CollectionObjectField(false, true, false, true, false)]
	private short _partId;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 5)]
	private short[] _injuries;

	[CollectionObjectField(false, true, false, false, false)]
	private short _winsCount;

	[CollectionObjectField(false, true, false, false, false)]
	private short _lossesCount;

	[CollectionObjectField(false, true, false, false, false)]
	private short _bestEnemyColorId;

	[CollectionObjectField(false, true, false, false, false)]
	private short _bestEnemyPartId;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _age;

	public const int FixedSize = 34;

	public const int DynamicCount = 0;

	private static List<(short templateId, short rate)>[] _cricketTemplateRates;

	public bool IsAlive => _age < CalcMaxAge() && CurrDurability > 0;

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

	public short GetColorId()
	{
		return _colorId;
	}

	public short GetPartId()
	{
		return _partId;
	}

	public short[] GetInjuries()
	{
		return _injuries;
	}

	public unsafe void SetInjuries(short[] injuries, DataContext context)
	{
		_injuries = injuries;
		SetModifiedAndInvalidateInfluencedCache(7, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 15u, 10);
			for (int i = 0; i < 5; i++)
			{
				((short*)ptr)[i] = _injuries[i];
			}
			ptr += 10;
		}
	}

	public short GetWinsCount()
	{
		return _winsCount;
	}

	public unsafe void SetWinsCount(short winsCount, DataContext context)
	{
		_winsCount = winsCount;
		SetModifiedAndInvalidateInfluencedCache(8, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 25u, 2);
			*(short*)ptr = _winsCount;
			ptr += 2;
		}
	}

	public short GetLossesCount()
	{
		return _lossesCount;
	}

	public unsafe void SetLossesCount(short lossesCount, DataContext context)
	{
		_lossesCount = lossesCount;
		SetModifiedAndInvalidateInfluencedCache(9, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 27u, 2);
			*(short*)ptr = _lossesCount;
			ptr += 2;
		}
	}

	public short GetBestEnemyColorId()
	{
		return _bestEnemyColorId;
	}

	public unsafe void SetBestEnemyColorId(short bestEnemyColorId, DataContext context)
	{
		_bestEnemyColorId = bestEnemyColorId;
		SetModifiedAndInvalidateInfluencedCache(10, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 29u, 2);
			*(short*)ptr = _bestEnemyColorId;
			ptr += 2;
		}
	}

	public short GetBestEnemyPartId()
	{
		return _bestEnemyPartId;
	}

	public unsafe void SetBestEnemyPartId(short bestEnemyPartId, DataContext context)
	{
		_bestEnemyPartId = bestEnemyPartId;
		SetModifiedAndInvalidateInfluencedCache(11, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 31u, 2);
			*(short*)ptr = _bestEnemyPartId;
			ptr += 2;
		}
	}

	public sbyte GetAge()
	{
		return _age;
	}

	public unsafe void SetAge(sbyte age, DataContext context)
	{
		_age = age;
		SetModifiedAndInvalidateInfluencedCache(12, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 33u, 1);
			*ptr = (byte)_age;
			ptr++;
		}
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetName()
	{
		return Config.Cricket.Instance[TemplateId].Name;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetItemType()
	{
		return Config.Cricket.Instance[TemplateId].ItemType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetItemSubType()
	{
		return Config.Cricket.Instance[TemplateId].ItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetGrade()
	{
		return Config.Cricket.Instance[TemplateId].Grade;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetIcon()
	{
		return Config.Cricket.Instance[TemplateId].Icon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetDesc()
	{
		return Config.Cricket.Instance[TemplateId].Desc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetTransferable()
	{
		return Config.Cricket.Instance[TemplateId].Transferable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetStackable()
	{
		return Config.Cricket.Instance[TemplateId].Stackable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetWagerable()
	{
		return Config.Cricket.Instance[TemplateId].Wagerable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRefinable()
	{
		return Config.Cricket.Instance[TemplateId].Refinable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetPoisonable()
	{
		return Config.Cricket.Instance[TemplateId].Poisonable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRepairable()
	{
		return Config.Cricket.Instance[TemplateId].Repairable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseWeight()
	{
		return Config.Cricket.Instance[TemplateId].BaseWeight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseValue()
	{
		return Config.Cricket.Instance[TemplateId].BaseValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBasePrice()
	{
		return Config.Cricket.Instance[TemplateId].BasePrice;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetDropRate()
	{
		return Config.Cricket.Instance[TemplateId].DropRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetResourceType()
	{
		return Config.Cricket.Instance[TemplateId].ResourceType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetPreservationDuration()
	{
		return Config.Cricket.Instance[TemplateId].PreservationDuration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetGiftLevel()
	{
		return Config.Cricket.Instance[TemplateId].GiftLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseFavorabilityChange()
	{
		return Config.Cricket.Instance[TemplateId].BaseFavorabilityChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetBaseHappinessChange()
	{
		return Config.Cricket.Instance[TemplateId].BaseHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsSpecial()
	{
		return Config.Cricket.Instance[TemplateId].IsSpecial;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRandomCreate()
	{
		return Config.Cricket.Instance[TemplateId].AllowRandomCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInheritable()
	{
		return Config.Cricket.Instance[TemplateId].Inheritable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.Cricket.Instance[TemplateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMerchantLevel()
	{
		return Config.Cricket.Instance[TemplateId].MerchantLevel;
	}

	public Cricket()
	{
		_injuries = new short[5];
	}

	public Cricket(short templateId)
	{
		CricketItem cricketItem = Config.Cricket.Instance[templateId];
		TemplateId = cricketItem.TemplateId;
		MaxDurability = cricketItem.MaxDurability;
		_injuries = new short[5];
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 34;
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
		*(short*)ptr = _colorId;
		ptr += 2;
		*(short*)ptr = _partId;
		ptr += 2;
		if (_injuries.Length != 5)
		{
			throw new Exception("Elements count of field _injuries is not equal to declaration");
		}
		for (int i = 0; i < 5; i++)
		{
			((short*)ptr)[i] = _injuries[i];
		}
		ptr += 10;
		*(short*)ptr = _winsCount;
		ptr += 2;
		*(short*)ptr = _lossesCount;
		ptr += 2;
		*(short*)ptr = _bestEnemyColorId;
		ptr += 2;
		*(short*)ptr = _bestEnemyPartId;
		ptr += 2;
		*ptr = (byte)_age;
		ptr++;
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
		_colorId = *(short*)ptr;
		ptr += 2;
		_partId = *(short*)ptr;
		ptr += 2;
		if (_injuries.Length != 5)
		{
			throw new Exception("Elements count of field _injuries is not equal to declaration");
		}
		for (int i = 0; i < 5; i++)
		{
			_injuries[i] = ((short*)ptr)[i];
		}
		ptr += 10;
		_winsCount = *(short*)ptr;
		ptr += 2;
		_lossesCount = *(short*)ptr;
		ptr += 2;
		_bestEnemyColorId = *(short*)ptr;
		ptr += 2;
		_bestEnemyPartId = *(short*)ptr;
		ptr += 2;
		_age = (sbyte)(*ptr);
		ptr++;
		return (int)(ptr - pData);
	}

	public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
	{
		return 0;
	}

	public override int GetValue()
	{
		if (CurrDurability <= 0)
		{
			return 0;
		}
		int val = ((_partId > 0) ? Math.Max(CricketParts.Instance[_colorId].Value, CricketParts.Instance[_partId].Value) : CricketParts.Instance[_colorId].Value);
		int val2 = ((_bestEnemyColorId > 0) ? Math.Max(CricketParts.Instance[_bestEnemyColorId].Value, CricketParts.Instance[_bestEnemyPartId].Value) : 0);
		return Math.Max(val, val2);
	}

	public bool IsCombinedCricket()
	{
		return _partId != 0;
	}

	public CricketPartsItem GetColorData()
	{
		return CricketParts.Instance[_colorId];
	}

	public CricketPartsItem GetPartsData()
	{
		return CricketParts.Instance[_partId];
	}

	public int CalcCatchLucky()
	{
		short num = GetColorData().CatchInfluence;
		if (IsCombinedCricket())
		{
			num += GetPartsData().CatchInfluence;
		}
		return num;
	}

	public int CalcCombatValue()
	{
		sbyte b = CalcGrade(_bestEnemyColorId, _bestEnemyPartId);
		return Math.Max(_winsCount - _lossesCount, 0) * (b + 1) * ((_lossesCount > 0) ? 1 : 2);
	}

	public bool UpdateCricketAge(DataContext context)
	{
		if (CurrDurability <= 0)
		{
			return false;
		}
		_age++;
		SetAge(_age, context);
		if (IsAlive)
		{
			return false;
		}
		SetCurrDurability(0, context);
		return true;
	}

	public void Rebirth(DataContext context)
	{
		_age = 0;
		SetAge(_age, context);
		SetCurrDurability(MaxDurability, context);
	}

	public bool Match(CricketCombatConfig config)
	{
		if (!IsAlive)
		{
			return false;
		}
		if (config.OnlyNoInjury && _injuries.Any((short x) => x > 0))
		{
			return false;
		}
		sbyte grade = GetGrade();
		return grade >= config.MinGrade && grade <= config.MaxGrade;
	}

	public Cricket(IRandomSource random, short colorId, short partId, int itemId)
		: this(0)
	{
		Initialize(random, colorId, partId, itemId);
	}

	public Cricket(IRandomSource random, short templateId, int itemId)
		: this(random, templateId, itemId, isSpecial: false)
	{
	}

	public Cricket(IRandomSource random, short templateId, int itemId, bool isSpecial)
		: this(templateId)
	{
		sbyte grade = (sbyte)templateId;
		var (colorId, partId) = GenerateRandomColorAndPart(random, grade, isSpecial);
		Initialize(random, colorId, partId, itemId);
	}

	private void Initialize(IRandomSource random, short colorId, short partId, int itemId)
	{
		_colorId = colorId;
		_partId = partId;
		Id = itemId;
		sbyte b = CalcGrade(colorId, partId);
		TemplateId = b;
		int num = b + 1 + CalcHp() / 20;
		num = Math.Max(num * random.Next(65, 136) / 100, 1);
		MaxDurability = (short)num;
		CurrDurability = (short)num;
		_bestEnemyColorId = -1;
		_bestEnemyPartId = -1;
		_age = (sbyte)(CalcMaxAge() * random.Next(0, 21) / 100);
		int num2 = Math.Min(3, num - 1);
		for (int i = 0; i < num2; i++)
		{
			if (random.CheckPercentProb(10))
			{
				CurrDurability--;
				if (random.CheckPercentProb(66))
				{
					int num3 = random.Next(2);
					_injuries[num3] += 5;
				}
				else
				{
					int num4 = 2 + random.Next(3);
					_injuries[num4]++;
				}
			}
		}
	}

	public static void InitializeCricketWeights()
	{
		if (_cricketTemplateRates == null)
		{
			_cricketTemplateRates = new List<(short, short)>[27];
		}
		for (int i = 0; i < 27; i++)
		{
			List<(short, short)>[] cricketTemplateRates = _cricketTemplateRates;
			int num = i;
			if (cricketTemplateRates[num] == null)
			{
				cricketTemplateRates[num] = new List<(short, short)>();
			}
			_cricketTemplateRates[i].Clear();
		}
		int num2 = 0;
		_cricketTemplateRates[num2++].AddRange(from a in CricketParts.Instance
			where a.NpcSpecialRate != 0
			select ((short TemplateId, short))(TemplateId: a.TemplateId, a.NpcSpecialRate));
		sbyte grade;
		for (grade = 7; grade < 9; grade++)
		{
			_cricketTemplateRates[num2++].AddRange(from a in CricketParts.Instance
				where a.Level == grade
				select ((short TemplateId, short))(TemplateId: a.TemplateId, a.Rate));
		}
		sbyte grade2;
		for (grade2 = 1; grade2 < 7; grade2++)
		{
			_cricketTemplateRates[num2++].AddRange(from a in CricketParts.Instance.Where(delegate(CricketPartsItem a)
				{
					ECricketPartsType type = a.Type;
					return type >= ECricketPartsType.Cyan && type <= ECricketPartsType.White && a.Level <= grade2;
				})
				select ((short TemplateId, short))(TemplateId: a.TemplateId, a.Rate));
			_cricketTemplateRates[num2++].AddRange(from a in CricketParts.Instance.Where(delegate(CricketPartsItem a)
				{
					ECricketPartsType type = a.Type;
					return type >= ECricketPartsType.Cyan && type <= ECricketPartsType.White && a.Level == grade2;
				})
				select ((short TemplateId, short))(TemplateId: a.TemplateId, a.Rate));
			_cricketTemplateRates[num2++].AddRange(from a in CricketParts.Instance
				where a.Type == ECricketPartsType.Parts && a.Level <= grade2
				select ((short TemplateId, short))(TemplateId: a.TemplateId, a.Rate));
			_cricketTemplateRates[num2++].AddRange(from a in CricketParts.Instance
				where a.Type == ECricketPartsType.Parts && a.Level == grade2
				select ((short TemplateId, short))(TemplateId: a.TemplateId, a.Rate));
		}
	}

	private static (short colorId, short partId) GenerateRandomColorAndPart(IRandomSource random, sbyte grade, bool isSpecial)
	{
		short num = 0;
		short item = 0;
		if (grade >= 7)
		{
			int num2 = ((!isSpecial) ? (grade - 7 + 1) : 0);
			num = RandomUtils.GetRandomResult(_cricketTemplateRates[num2], random);
		}
		else if (grade >= 1)
		{
			bool flag = grade == 6 || random.CheckPercentProb(75);
			int num3 = (grade - 1) * 4 + 3;
			int num4 = (flag ? (num3 + 3) : (num3 + 2));
			int num5 = (flag ? num3 : (num3 + 1));
			num = RandomUtils.GetRandomResult(_cricketTemplateRates[num5], random);
			item = RandomUtils.GetRandomResult(_cricketTemplateRates[num4], random);
		}
		else
		{
			num = 0;
		}
		return (colorId: num, partId: item);
	}

	private static sbyte CalcGrade(short colorId, short partId)
	{
		if (colorId < 0)
		{
			return 0;
		}
		sbyte b = CricketParts.Instance[colorId].Level;
		if (partId > 0)
		{
			b = Math.Max(CricketParts.Instance[partId].Level, b);
		}
		return b;
	}

	private int CalcHp()
	{
		short num = CricketParts.Instance[_colorId].HP;
		if (_partId > 0)
		{
			num += CricketParts.Instance[_partId].HP;
		}
		return num;
	}

	public int CalcMaxAge()
	{
		int num = CricketParts.Instance[_colorId].Life;
		if (_partId > 0)
		{
			num += CricketParts.Instance[_partId].Life;
		}
		return num + DomainManager.Extra.GetCricketExtraAge(Id);
	}
}
