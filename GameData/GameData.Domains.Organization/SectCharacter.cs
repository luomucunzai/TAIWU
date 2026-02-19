using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.Organization;

[SerializableGameData(NotForDisplayModule = true)]
public class SectCharacter : SettlementCharacter, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 4;

		public const uint OrgTemplateId_Offset = 4u;

		public const int OrgTemplateId_Size = 1;

		public const uint SettlementId_Offset = 5u;

		public const int SettlementId_Size = 2;

		public const uint ApprovedTaiwu_Offset = 7u;

		public const int ApprovedTaiwu_Size = 1;

		public const uint InfluencePower_Offset = 8u;

		public const int InfluencePower_Size = 2;

		public const uint InfluencePowerBonus_Offset = 10u;

		public const int InfluencePowerBonus_Size = 2;
	}

	public const int FixedSize = 12;

	public const int DynamicCount = 0;

	public SectCharacter(int charId, sbyte orgTemplateId, short settlementId)
		: base(charId, orgTemplateId, settlementId)
	{
	}

	public unsafe override void SetSettlementId(short settlementId, DataContext context)
	{
		SettlementId = settlementId;
		SetModifiedAndInvalidateInfluencedCache(2, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 5u, 2);
			*(short*)ptr = SettlementId;
			ptr += 2;
		}
	}

	public unsafe override void SetApprovedTaiwu(bool approvedTaiwu, DataContext context)
	{
		ApprovedTaiwu = approvedTaiwu;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 7u, 1);
			*ptr = (ApprovedTaiwu ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public unsafe override void SetInfluencePower(short influencePower, DataContext context)
	{
		InfluencePower = influencePower;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 8u, 2);
			*(short*)ptr = InfluencePower;
			ptr += 2;
		}
	}

	public unsafe override void SetInfluencePowerBonus(short influencePowerBonus, DataContext context)
	{
		InfluencePowerBonus = influencePowerBonus;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 10u, 2);
			*(short*)ptr = InfluencePowerBonus;
			ptr += 2;
		}
	}

	public SectCharacter()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 12;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Id;
		ptr += 4;
		*ptr = (byte)OrgTemplateId;
		ptr++;
		*(short*)ptr = SettlementId;
		ptr += 2;
		*ptr = (ApprovedTaiwu ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = InfluencePower;
		ptr += 2;
		*(short*)ptr = InfluencePowerBonus;
		ptr += 2;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(int*)ptr;
		ptr += 4;
		OrgTemplateId = (sbyte)(*ptr);
		ptr++;
		SettlementId = *(short*)ptr;
		ptr += 2;
		ApprovedTaiwu = *ptr != 0;
		ptr++;
		InfluencePower = *(short*)ptr;
		ptr += 2;
		InfluencePowerBonus = *(short*)ptr;
		ptr += 2;
		return (int)(ptr - pData);
	}
}
