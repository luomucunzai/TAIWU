using System;
using Config;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.DLC.FiveLoong;

[SerializableGameData(IsExtensible = true)]
public class Jiao : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Key = 0;

		public const ushort Gender = 1;

		public const ushort TamePoint = 2;

		public const ushort NurturanceTemplateId = 3;

		public const ushort CanBreed = 4;

		public const ushort GrowthStage = 5;

		public const ushort EvolveRemainingMonth = 6;

		public const ushort Height = 7;

		public const ushort Weight = 8;

		public const ushort LifeSpan = 9;

		public const ushort NameId = 10;

		public const ushort Behavior = 11;

		public const ushort Properties = 12;

		public const ushort Id = 13;

		public const ushort MotherId = 14;

		public const ushort AggressiveTimes = 15;

		public const ushort NegativeTimes = 16;

		public const ushort TemplateId = 17;

		public const ushort FatherId = 18;

		public const ushort LastChoiceIsAggressive = 19;

		public const ushort LastEvolutionResult = 20;

		public const ushort RandomSeed = 21;

		public const ushort PettingCoolDown = 22;

		public const ushort Generation = 23;

		public const ushort NextPeriod = 24;

		public const ushort Count = 25;

		public static readonly string[] FieldId2FieldName = new string[25]
		{
			"Key", "Gender", "TamePoint", "NurturanceTemplateId", "CanBreed", "GrowthStage", "EvolveRemainingMonth", "Height", "Weight", "LifeSpan",
			"NameId", "Behavior", "Properties", "Id", "MotherId", "AggressiveTimes", "NegativeTimes", "TemplateId", "FatherId", "LastChoiceIsAggressive",
			"LastEvolutionResult", "RandomSeed", "PettingCoolDown", "Generation", "NextPeriod"
		};
	}

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public ItemKey Key;

	[SerializableGameDataField]
	public bool Gender;

	[SerializableGameDataField]
	public int TamePoint;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public short NurturanceTemplateId;

	[SerializableGameDataField]
	public bool CanBreed;

	[SerializableGameDataField]
	public sbyte GrowthStage;

	[SerializableGameDataField]
	public int EvolveRemainingMonth;

	[SerializableGameDataField]
	public (int Inherited, int Fostered) Height;

	[SerializableGameDataField]
	public (int Inherited, int Fostered) Weight;

	[SerializableGameDataField]
	public (int Inherited, int Fostered) LifeSpan;

	[SerializableGameDataField]
	public int FatherId;

	[SerializableGameDataField]
	public int MotherId;

	[SerializableGameDataField]
	public int AggressiveTimes;

	[SerializableGameDataField]
	public int NegativeTimes;

	[SerializableGameDataField]
	public bool LastChoiceIsAggressive;

	[SerializableGameDataField]
	public short LastEvolutionResult;

	[SerializableGameDataField]
	public int RandomSeed;

	[SerializableGameDataField]
	public int PettingCoolDown;

	[SerializableGameDataField]
	public int Generation;

	[SerializableGameDataField]
	public int NextPeriod;

	[SerializableGameDataField]
	public int NameId;

	[SerializableGameDataField]
	public sbyte Behavior;

	[SerializableGameDataField]
	public JiaoProperty Properties;

	public Jiao()
	{
		Id = -1;
		Key = ItemKey.Invalid;
		Gender = false;
		TamePoint = -1;
		NurturanceTemplateId = -1;
		TemplateId = -1;
		CanBreed = false;
		GrowthStage = 0;
		EvolveRemainingMonth = 0;
		Height = (Inherited: 0, Fostered: 0);
		Weight = (Inherited: 0, Fostered: 0);
		LifeSpan = (Inherited: 0, Fostered: 0);
		NameId = -1;
		Behavior = -1;
		Properties = new JiaoProperty();
		FatherId = -1;
		MotherId = -1;
		AggressiveTimes = 0;
		NegativeTimes = 0;
		LastEvolutionResult = -1;
		RandomSeed = -1;
		PettingCoolDown = -1;
		Generation = 0;
		NextPeriod = -1;
	}

	public Jiao(ItemKey key, bool isMale, int tamePoint, short templateId, sbyte behavior)
	{
		Id = key.Id;
		Key = key;
		Gender = isMale;
		TamePoint = tamePoint;
		NurturanceTemplateId = -1;
		TemplateId = templateId;
		CanBreed = true;
		GrowthStage = 0;
		EvolveRemainingMonth = 0;
		Height = (Inherited: 0, Fostered: 0);
		Weight = (Inherited: 0, Fostered: 0);
		LifeSpan = (Inherited: 0, Fostered: 0);
		NameId = -1;
		Behavior = behavior;
		Properties = new JiaoProperty();
		FatherId = -1;
		MotherId = -1;
		AggressiveTimes = 0;
		NegativeTimes = 0;
		LastEvolutionResult = -1;
		RandomSeed = -1;
		PettingCoolDown = -1;
		Generation = 0;
		NextPeriod = -1;
	}

	public Jiao(ItemKey key, bool isMale, int tamePoint, short templateId, sbyte behavior, int fatherId, int motherId)
	{
		Id = key.Id;
		Key = key;
		Gender = isMale;
		TamePoint = tamePoint;
		NurturanceTemplateId = -1;
		TemplateId = templateId;
		CanBreed = true;
		GrowthStage = 0;
		EvolveRemainingMonth = 0;
		Height = (Inherited: 0, Fostered: 0);
		Weight = (Inherited: 0, Fostered: 0);
		LifeSpan = (Inherited: 0, Fostered: 0);
		NameId = -1;
		Behavior = behavior;
		Properties = new JiaoProperty();
		FatherId = fatherId;
		MotherId = motherId;
		AggressiveTimes = 0;
		NegativeTimes = 0;
		LastEvolutionResult = -1;
		RandomSeed = -1;
		PettingCoolDown = -1;
		Generation = 0;
		NextPeriod = -1;
	}

	public Jiao(Jiao jiao, ItemKey key)
	{
		Id = key.Id;
		Key = key;
		Gender = jiao.Gender;
		TamePoint = jiao.TamePoint;
		NurturanceTemplateId = jiao.NurturanceTemplateId;
		TemplateId = jiao.TemplateId;
		CanBreed = jiao.CanBreed;
		GrowthStage = jiao.GrowthStage;
		EvolveRemainingMonth = jiao.EvolveRemainingMonth;
		Height = jiao.Height;
		Weight = jiao.Weight;
		LifeSpan = jiao.LifeSpan;
		NameId = jiao.NameId;
		Behavior = jiao.Behavior;
		Properties = new JiaoProperty();
		Properties.DeepCopy(jiao.Properties);
		FatherId = jiao.FatherId;
		MotherId = jiao.MotherId;
		AggressiveTimes = jiao.AggressiveTimes;
		NegativeTimes = jiao.NegativeTimes;
		LastEvolutionResult = jiao.LastEvolutionResult;
		RandomSeed = jiao.RandomSeed;
		PettingCoolDown = jiao.PettingCoolDown;
		Generation = jiao.Generation;
		NextPeriod = jiao.NextPeriod;
	}

	public void ResetGrowth()
	{
		EvolveRemainingMonth = JiaoNurturance.Instance[NurturanceTemplateId].NurturanceCostMonth;
		NextPeriod = JiaoNurturance.Instance[NurturanceTemplateId].StageCostMonth;
		Properties.ResetGrowth();
		Height = (Inherited: Height.Inherited, Fostered: 0);
		Weight = (Inherited: Weight.Inherited, Fostered: 0);
		LifeSpan = (Inherited: LifeSpan.Inherited, Fostered: 0);
	}

	public string GetNameText()
	{
		return GetNameRelatedData().GetName();
	}

	public JiaoLoongNameRelatedData GetNameRelatedData()
	{
		return new JiaoLoongNameRelatedData
		{
			ItemType = Key.ItemType,
			ItemTemplateId = Key.TemplateId,
			NameId = NameId,
			CharTemplateId = (short)((GrowthStage == 0) ? (-1) : Config.Jiao.Instance[TemplateId].IndexOfCharacterTemplate)
		};
	}

	public int GetPresentProperty(short propertyTemplateId)
	{
		JiaoItem jiaoItem = Config.Jiao.Instance[TemplateId];
		return propertyTemplateId switch
		{
			9 => jiaoItem.Length + Height.Inherited + Height.Fostered / 100, 
			10 => jiaoItem.Weight + Weight.Inherited + Weight.Fostered / 100, 
			11 => jiaoItem.Life + LifeSpan.Inherited + LifeSpan.Fostered / 100, 
			_ => throw new Exception($"wrong property id: {propertyTemplateId}"), 
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 165;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 25;
		ptr += 2;
		ptr += Key.Serialize(ptr);
		*ptr = (Gender ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = TamePoint;
		ptr += 4;
		*(short*)ptr = NurturanceTemplateId;
		ptr += 2;
		*ptr = (CanBreed ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)GrowthStage;
		ptr++;
		*(int*)ptr = EvolveRemainingMonth;
		ptr += 4;
		ptr += SerializationHelper.Serialize<int, int>(ptr, Height);
		ptr += SerializationHelper.Serialize<int, int>(ptr, Weight);
		ptr += SerializationHelper.Serialize<int, int>(ptr, LifeSpan);
		*(int*)ptr = NameId;
		ptr += 4;
		*ptr = (byte)Behavior;
		ptr++;
		ptr += Properties.Serialize(ptr);
		*(int*)ptr = Id;
		ptr += 4;
		*(int*)ptr = MotherId;
		ptr += 4;
		*(int*)ptr = AggressiveTimes;
		ptr += 4;
		*(int*)ptr = NegativeTimes;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*(int*)ptr = FatherId;
		ptr += 4;
		*ptr = (LastChoiceIsAggressive ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = LastEvolutionResult;
		ptr += 2;
		*(int*)ptr = RandomSeed;
		ptr += 4;
		*(int*)ptr = PettingCoolDown;
		ptr += 4;
		*(int*)ptr = Generation;
		ptr += 4;
		*(int*)ptr = NextPeriod;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ptr += Key.Deserialize(ptr);
		}
		if (num > 1)
		{
			Gender = *ptr != 0;
			ptr++;
		}
		if (num > 2)
		{
			TamePoint = *(int*)ptr;
			ptr += 4;
		}
		if (num > 3)
		{
			NurturanceTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 4)
		{
			CanBreed = *ptr != 0;
			ptr++;
		}
		if (num > 5)
		{
			GrowthStage = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 6)
		{
			EvolveRemainingMonth = *(int*)ptr;
			ptr += 4;
		}
		if (num > 7)
		{
			ptr += SerializationHelper.Deserialize<int, int>(ptr, ref Height);
		}
		if (num > 8)
		{
			ptr += SerializationHelper.Deserialize<int, int>(ptr, ref Weight);
		}
		if (num > 9)
		{
			ptr += SerializationHelper.Deserialize<int, int>(ptr, ref LifeSpan);
		}
		if (num > 10)
		{
			NameId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 11)
		{
			Behavior = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 12)
		{
			if (Properties == null)
			{
				Properties = new JiaoProperty();
			}
			ptr += Properties.Deserialize(ptr);
		}
		if (num > 13)
		{
			Id = *(int*)ptr;
			ptr += 4;
		}
		if (num > 14)
		{
			MotherId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 15)
		{
			AggressiveTimes = *(int*)ptr;
			ptr += 4;
		}
		if (num > 16)
		{
			NegativeTimes = *(int*)ptr;
			ptr += 4;
		}
		if (num > 17)
		{
			TemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 18)
		{
			FatherId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 19)
		{
			LastChoiceIsAggressive = *ptr != 0;
			ptr++;
		}
		if (num > 20)
		{
			LastEvolutionResult = *(short*)ptr;
			ptr += 2;
		}
		if (num > 21)
		{
			RandomSeed = *(int*)ptr;
			ptr += 4;
		}
		if (num > 22)
		{
			PettingCoolDown = *(int*)ptr;
			ptr += 4;
		}
		if (num > 23)
		{
			Generation = *(int*)ptr;
			ptr += 4;
		}
		if (num > 24)
		{
			NextPeriod = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
