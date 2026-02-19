using GameData.Domains.Character.Relation;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

public class RelatedCharactersForRelations : ISerializableGameData
{
	[SerializableGameDataField]
	public CharacterSet Parents;

	[SerializableGameDataField]
	public CharacterSet Children;

	[SerializableGameDataField]
	public CharacterSet BrothersAndSisters;

	[SerializableGameDataField]
	public CharacterSet SwornBrothersAndSisters;

	[SerializableGameDataField]
	public CharacterSet HusbandsAndWives;

	[SerializableGameDataField]
	public CharacterSet Mentors;

	[SerializableGameDataField]
	public CharacterSet Friends;

	[SerializableGameDataField]
	public CharacterSet Adored;

	[SerializableGameDataField]
	public CharacterSet RelatedAdored;

	[SerializableGameDataField]
	public CharacterSet Enemies;

	[SerializableGameDataField]
	public CharacterSet RelatedEnemies;

	[SerializableGameDataField]
	public CharacterSet FactionMembers;

	[SerializableGameDataField]
	public int FactionLeaderId;

	public RelatedCharactersForRelations(RelatedCharacters relatedChars)
	{
		Parents.AddRange(relatedChars.BloodParents.GetCollection());
		Parents.AddRange(relatedChars.StepParents.GetCollection());
		Parents.AddRange(relatedChars.AdoptiveParents.GetCollection());
		Children.AddRange(relatedChars.BloodChildren.GetCollection());
		Children.AddRange(relatedChars.StepChildren.GetCollection());
		Children.AddRange(relatedChars.AdoptiveChildren.GetCollection());
		BrothersAndSisters.AddRange(relatedChars.BloodBrothersAndSisters.GetCollection());
		BrothersAndSisters.AddRange(relatedChars.StepBrothersAndSisters.GetCollection());
		BrothersAndSisters.AddRange(relatedChars.AdoptiveBrothersAndSisters.GetCollection());
		SwornBrothersAndSisters.AddRange(relatedChars.SwornBrothersAndSisters.GetCollection());
		HusbandsAndWives.AddRange(relatedChars.HusbandsAndWives.GetCollection());
		Mentors.AddRange(relatedChars.Mentors.GetCollection());
		Friends.AddRange(relatedChars.Friends.GetCollection());
		Adored.AddRange(relatedChars.Adored.GetCollection());
		Enemies.AddRange(relatedChars.Enemies.GetCollection());
		FactionLeaderId = -1;
	}

	public RelatedCharactersForRelations()
	{
		FactionLeaderId = -1;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += Parents.GetSerializedSize();
		num += Children.GetSerializedSize();
		num += BrothersAndSisters.GetSerializedSize();
		num += SwornBrothersAndSisters.GetSerializedSize();
		num += HusbandsAndWives.GetSerializedSize();
		num += Mentors.GetSerializedSize();
		num += Friends.GetSerializedSize();
		num += Adored.GetSerializedSize();
		num += RelatedAdored.GetSerializedSize();
		num += Enemies.GetSerializedSize();
		num += RelatedEnemies.GetSerializedSize();
		num += FactionMembers.GetSerializedSize();
		num += 4;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		int num = Parents.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		int num2 = Children.Serialize(ptr);
		ptr += num2;
		Tester.Assert(num2 <= 65535);
		int num3 = BrothersAndSisters.Serialize(ptr);
		ptr += num3;
		Tester.Assert(num3 <= 65535);
		int num4 = SwornBrothersAndSisters.Serialize(ptr);
		ptr += num4;
		Tester.Assert(num4 <= 65535);
		int num5 = HusbandsAndWives.Serialize(ptr);
		ptr += num5;
		Tester.Assert(num5 <= 65535);
		int num6 = Mentors.Serialize(ptr);
		ptr += num6;
		Tester.Assert(num6 <= 65535);
		int num7 = Friends.Serialize(ptr);
		ptr += num7;
		Tester.Assert(num7 <= 65535);
		int num8 = Adored.Serialize(ptr);
		ptr += num8;
		Tester.Assert(num8 <= 65535);
		int num9 = RelatedAdored.Serialize(ptr);
		ptr += num9;
		Tester.Assert(num9 <= 65535);
		int num10 = Enemies.Serialize(ptr);
		ptr += num10;
		Tester.Assert(num10 <= 65535);
		int num11 = RelatedEnemies.Serialize(ptr);
		ptr += num11;
		Tester.Assert(num11 <= 65535);
		int num12 = FactionMembers.Serialize(ptr);
		ptr += num12;
		Tester.Assert(num12 <= 65535);
		*(int*)ptr = FactionLeaderId;
		ptr += 4;
		int num13 = (int)(ptr - pData);
		if (num13 > 4)
		{
			return (num13 + 3) / 4 * 4;
		}
		return num13;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += Parents.Deserialize(ptr);
		ptr += Children.Deserialize(ptr);
		ptr += BrothersAndSisters.Deserialize(ptr);
		ptr += SwornBrothersAndSisters.Deserialize(ptr);
		ptr += HusbandsAndWives.Deserialize(ptr);
		ptr += Mentors.Deserialize(ptr);
		ptr += Friends.Deserialize(ptr);
		ptr += Adored.Deserialize(ptr);
		ptr += RelatedAdored.Deserialize(ptr);
		ptr += Enemies.Deserialize(ptr);
		ptr += RelatedEnemies.Deserialize(ptr);
		ptr += FactionMembers.Deserialize(ptr);
		FactionLeaderId = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
