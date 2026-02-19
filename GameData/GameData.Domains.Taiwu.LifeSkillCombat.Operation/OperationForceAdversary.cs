using GameData.Domains.Information;
using GameData.Domains.Item.Display;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

public class OperationForceAdversary : OperationBase
{
	public SecretInformationDisplayPackage SecretInformationDisplayPackage;

	public ItemDisplayData ItemDisplayData;

	public override string Inspect()
	{
		if (SecretInformationDisplayPackage != null)
		{
			return $"via SecretInformationMetaDataId {SecretInformationDisplayPackage.SecretInformationDisplayDataList[0].SecretInformationMetaDataId}";
		}
		if (ItemDisplayData != null)
		{
			return $"via Item ${ItemDisplayData.Key}";
		}
		return string.Empty;
	}

	public OperationForceAdversary()
	{
	}

	public override int GetSerializedSize()
	{
		int serializedSize = base.GetSerializedSize();
		serializedSize++;
		if (SecretInformationDisplayPackage != null)
		{
			serializedSize += SecretInformationDisplayPackage.GetSerializedSize();
		}
		serializedSize++;
		if (ItemDisplayData != null)
		{
			serializedSize += ItemDisplayData.GetSerializedSize();
		}
		return (serializedSize <= 4) ? serializedSize : ((serializedSize + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Serialize(ptr);
		*ptr = ((SecretInformationDisplayPackage != null) ? ((byte)1) : ((byte)0));
		ptr++;
		if (SecretInformationDisplayPackage != null)
		{
			ptr += SecretInformationDisplayPackage.Serialize(ptr);
		}
		*ptr = ((ItemDisplayData != null) ? ((byte)1) : ((byte)0));
		ptr++;
		if (ItemDisplayData != null)
		{
			ptr += ItemDisplayData.Serialize(ptr);
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Deserialize(ptr);
		bool flag = *ptr != 0;
		ptr++;
		SecretInformationDisplayPackage = null;
		if (flag)
		{
			SecretInformationDisplayPackage = new SecretInformationDisplayPackage();
			ptr += SecretInformationDisplayPackage.Deserialize(ptr);
		}
		bool flag2 = *ptr != 0;
		ptr++;
		ItemDisplayData = null;
		if (flag2)
		{
			ItemDisplayData = new ItemDisplayData();
			ptr += ItemDisplayData.Deserialize(ptr);
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public OperationForceAdversary(sbyte playerId, int stamp, SecretInformationDisplayPackage secretInformationDisplayPackage)
		: base(playerId, stamp)
	{
		SecretInformationDisplayPackage = secretInformationDisplayPackage;
	}

	public OperationForceAdversary(sbyte playerId, int stamp, ItemDisplayData itemDisplayData)
		: base(playerId, stamp)
	{
		ItemDisplayData = itemDisplayData;
	}

	public OperationForceAdversary(sbyte playerId, int stamp)
		: base(playerId, stamp)
	{
	}
}
