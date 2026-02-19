using GameData.Serializer;

namespace GameData.Domains.Character.Relation;

[SerializableGameData(NotForDisplayModule = true)]
public struct RelatedCharacter : ISerializableGameData
{
	public ushort RelationType;

	public short Favorability;

	public int EstablishmentDate;

	public RelatedCharacter(ushort relationType, short favorability, int establishmentDate)
	{
		RelationType = relationType;
		Favorability = favorability;
		EstablishmentDate = establishmentDate;
	}

	public sbyte GetFavorabilityType()
	{
		return FavorabilityType.GetFavorabilityType(Favorability);
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 8;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(ushort*)pData = RelationType;
		((short*)pData)[1] = Favorability;
		((int*)pData)[1] = EstablishmentDate;
		return 8;
	}

	public unsafe int Deserialize(byte* pData)
	{
		RelationType = *(ushort*)pData;
		Favorability = ((short*)pData)[1];
		EstablishmentDate = ((int*)pData)[1];
		return 8;
	}
}
