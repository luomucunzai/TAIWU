using GameData.Domains.Character.Relation;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Information;

[SerializableGameData(NotForDisplayModule = true)]
public class SecretInformationCharacterRelationshipSnapshot : ISerializableGameData
{
	[SerializableGameDataField]
	public RelatedCharacters RelatedCharacters;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((RelatedCharacters == null) ? (num + 2) : (num + (2 + RelatedCharacters.GetSerializedSize())));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (RelatedCharacters != null)
		{
			byte* ptr2 = ptr;
			ptr += 2;
			int num = RelatedCharacters.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr2 = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (RelatedCharacters == null)
			{
				RelatedCharacters = new RelatedCharacters();
			}
			ptr += RelatedCharacters.Deserialize(ptr);
		}
		else
		{
			RelatedCharacters = null;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
