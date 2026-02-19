using System.Collections.Generic;
using GameData.Domains.Organization.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

[SerializableGameData(IsExtensible = true, NotForArchive = true)]
public struct BuildingManageYieldTipsData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort ManageProduceValuationMin = 0;

		public const ushort ManageProduceValuationMax = 1;

		public const ushort ResourceOutputValuation = 2;

		public const ushort ProduceResourceType = 3;

		public const ushort ProduceDependencies = 4;

		public const ushort ManagerAttainment = 5;

		public const ushort SafetyOrCultureFactorSettlementsAndPickValue = 6;

		public const ushort BuildingProduceDependencyData = 7;

		public const ushort Count = 8;

		public static readonly string[] FieldId2FieldName = new string[8] { "ManageProduceValuationMin", "ManageProduceValuationMax", "ResourceOutputValuation", "ProduceResourceType", "ProduceDependencies", "ManagerAttainment", "SafetyOrCultureFactorSettlementsAndPickValue", "BuildingProduceDependencyData" };
	}

	[SerializableGameDataField]
	public int ManageProduceValuationMin;

	[SerializableGameDataField]
	public int ManageProduceValuationMax;

	[SerializableGameDataField]
	public int ResourceOutputValuation;

	[SerializableGameDataField]
	public sbyte ProduceResourceType;

	[SerializableGameDataField]
	public int ManagerAttainment;

	[SerializableGameDataField]
	public BuildingProduceDependencyData BuildingProduceDependencyData;

	[SerializableGameDataField]
	public Dictionary<BuildingBlockKey, BuildingProduceDependencyData> ProduceDependencies;

	[SerializableGameDataField]
	public Dictionary<int, SettlementDisplayData> SafetyOrCultureFactorSettlementsAndPickValue;

	public BuildingManageYieldTipsData(int arg1)
	{
		ManageProduceValuationMin = 0;
		ManageProduceValuationMax = 0;
		ResourceOutputValuation = 0;
		ProduceResourceType = 0;
		ManagerAttainment = 0;
		BuildingProduceDependencyData = default(BuildingProduceDependencyData);
		ProduceDependencies = new Dictionary<BuildingBlockKey, BuildingProduceDependencyData>();
		SafetyOrCultureFactorSettlementsAndPickValue = new Dictionary<int, SettlementDisplayData>();
	}

	public BuildingManageYieldTipsData(BuildingManageYieldTipsData other)
	{
		ManageProduceValuationMin = other.ManageProduceValuationMin;
		ManageProduceValuationMax = other.ManageProduceValuationMax;
		ResourceOutputValuation = other.ResourceOutputValuation;
		ProduceResourceType = other.ProduceResourceType;
		ProduceDependencies = ((other.ProduceDependencies == null) ? null : new Dictionary<BuildingBlockKey, BuildingProduceDependencyData>(other.ProduceDependencies));
		ManagerAttainment = other.ManagerAttainment;
		SafetyOrCultureFactorSettlementsAndPickValue = ((other.SafetyOrCultureFactorSettlementsAndPickValue == null) ? null : new Dictionary<int, SettlementDisplayData>(other.SafetyOrCultureFactorSettlementsAndPickValue));
		BuildingProduceDependencyData = other.BuildingProduceDependencyData;
	}

	public void Assign(BuildingManageYieldTipsData other)
	{
		ManageProduceValuationMin = other.ManageProduceValuationMin;
		ManageProduceValuationMax = other.ManageProduceValuationMax;
		ResourceOutputValuation = other.ResourceOutputValuation;
		ProduceResourceType = other.ProduceResourceType;
		ProduceDependencies = ((other.ProduceDependencies == null) ? null : new Dictionary<BuildingBlockKey, BuildingProduceDependencyData>(other.ProduceDependencies));
		ManagerAttainment = other.ManagerAttainment;
		SafetyOrCultureFactorSettlementsAndPickValue = ((other.SafetyOrCultureFactorSettlementsAndPickValue == null) ? null : new Dictionary<int, SettlementDisplayData>(other.SafetyOrCultureFactorSettlementsAndPickValue));
		BuildingProduceDependencyData = other.BuildingProduceDependencyData;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 19;
		num += DictionaryOfCustomTypePair.GetSerializedSize<BuildingBlockKey, BuildingProduceDependencyData>(ProduceDependencies);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, SettlementDisplayData>(SafetyOrCultureFactorSettlementsAndPickValue);
		num += BuildingProduceDependencyData.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 8;
		ptr += 2;
		*(int*)ptr = ManageProduceValuationMin;
		ptr += 4;
		*(int*)ptr = ManageProduceValuationMax;
		ptr += 4;
		*(int*)ptr = ResourceOutputValuation;
		ptr += 4;
		*ptr = (byte)ProduceResourceType;
		ptr++;
		ptr += DictionaryOfCustomTypePair.Serialize<BuildingBlockKey, BuildingProduceDependencyData>(ptr, ref ProduceDependencies);
		*(int*)ptr = ManagerAttainment;
		ptr += 4;
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, SettlementDisplayData>(ptr, ref SafetyOrCultureFactorSettlementsAndPickValue);
		int num = BuildingProduceDependencyData.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ManageProduceValuationMin = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			ManageProduceValuationMax = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			ResourceOutputValuation = *(int*)ptr;
			ptr += 4;
		}
		if (num > 3)
		{
			ProduceResourceType = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 4)
		{
			ptr += DictionaryOfCustomTypePair.Deserialize<BuildingBlockKey, BuildingProduceDependencyData>(ptr, ref ProduceDependencies);
		}
		if (num > 5)
		{
			ManagerAttainment = *(int*)ptr;
			ptr += 4;
		}
		if (num > 6)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, SettlementDisplayData>(ptr, ref SafetyOrCultureFactorSettlementsAndPickValue);
		}
		if (num > 7)
		{
			ptr += BuildingProduceDependencyData.Deserialize(ptr);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
