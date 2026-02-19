using GameData.Domains.Character.Display;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

[SerializableGameData(NoCopyConstructors = true)]
public class MerchantInfoMerchantData : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharId;

	[SerializableGameDataField]
	public NameRelatedData NameRelatedData;

	[SerializableGameDataField]
	public sbyte BehaviorType;

	[SerializableGameDataField]
	public short Favorability;

	[SerializableGameDataField]
	public short MerchantTemplateId;

	[SerializableGameDataField]
	public short CurrentAreaTemplateId;

	[SerializableGameDataField]
	public short OrgTemplateId;

	[SerializableGameDataField]
	public FullBlockName FullBlockName;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 45;
		num += FullBlockName.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharId;
		ptr += 4;
		ptr += NameRelatedData.Serialize(ptr);
		*ptr = (byte)BehaviorType;
		ptr++;
		*(short*)ptr = Favorability;
		ptr += 2;
		*(short*)ptr = MerchantTemplateId;
		ptr += 2;
		*(short*)ptr = CurrentAreaTemplateId;
		ptr += 2;
		*(short*)ptr = OrgTemplateId;
		ptr += 2;
		int num = FullBlockName.Serialize(ptr);
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
		CharId = *(int*)ptr;
		ptr += 4;
		ptr += NameRelatedData.Deserialize(ptr);
		BehaviorType = (sbyte)(*ptr);
		ptr++;
		Favorability = *(short*)ptr;
		ptr += 2;
		MerchantTemplateId = *(short*)ptr;
		ptr += 2;
		CurrentAreaTemplateId = *(short*)ptr;
		ptr += 2;
		OrgTemplateId = *(short*)ptr;
		ptr += 2;
		ptr += FullBlockName.Deserialize(ptr);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
