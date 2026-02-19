using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Relation;

[SerializableGameData(NotForDisplayModule = true)]
public class RelatedCharacters : ISerializableGameData
{
	[SerializableGameDataField]
	public CharacterSet General;

	[SerializableGameDataField]
	public CharacterSet BloodParents;

	[SerializableGameDataField]
	public CharacterSet BloodChildren;

	[SerializableGameDataField]
	public CharacterSet BloodBrothersAndSisters;

	[SerializableGameDataField]
	public CharacterSet StepParents;

	[SerializableGameDataField]
	public CharacterSet StepChildren;

	[SerializableGameDataField]
	public CharacterSet StepBrothersAndSisters;

	[SerializableGameDataField]
	public CharacterSet AdoptiveParents;

	[SerializableGameDataField]
	public CharacterSet AdoptiveChildren;

	[SerializableGameDataField]
	public CharacterSet AdoptiveBrothersAndSisters;

	[SerializableGameDataField]
	public CharacterSet SwornBrothersAndSisters;

	[SerializableGameDataField]
	public CharacterSet HusbandsAndWives;

	[SerializableGameDataField]
	public CharacterSet Mentors;

	[SerializableGameDataField]
	public CharacterSet Mentees;

	[SerializableGameDataField]
	public CharacterSet Friends;

	[SerializableGameDataField]
	public CharacterSet Adored;

	[SerializableGameDataField]
	public CharacterSet Enemies;

	public RelatedCharacters()
	{
		General = default(CharacterSet);
		BloodParents = default(CharacterSet);
		BloodChildren = default(CharacterSet);
		BloodBrothersAndSisters = default(CharacterSet);
		StepParents = default(CharacterSet);
		StepChildren = default(CharacterSet);
		StepBrothersAndSisters = default(CharacterSet);
		AdoptiveParents = default(CharacterSet);
		AdoptiveChildren = default(CharacterSet);
		AdoptiveBrothersAndSisters = default(CharacterSet);
		SwornBrothersAndSisters = default(CharacterSet);
		HusbandsAndWives = default(CharacterSet);
		Mentors = default(CharacterSet);
		Mentees = default(CharacterSet);
		Friends = default(CharacterSet);
		Adored = default(CharacterSet);
		Enemies = default(CharacterSet);
	}

	public void Add(int relatedCharId, ushort relationType)
	{
		CharacterSet characterSet = GetCharacterSet(relationType);
		if (characterSet.Add(relatedCharId))
		{
			SetCharacterSet(relationType, characterSet);
		}
	}

	public void Remove(int relatedCharId, ushort relationType)
	{
		CharacterSet characterSet = GetCharacterSet(relationType);
		if (characterSet.Remove(relatedCharId).Item1)
		{
			SetCharacterSet(relationType, characterSet);
		}
	}

	public void OfflineClear()
	{
		General.Clear();
		BloodParents.Clear();
		BloodChildren.Clear();
		BloodBrothersAndSisters.Clear();
		StepParents.Clear();
		StepChildren.Clear();
		StepBrothersAndSisters.Clear();
		AdoptiveParents.Clear();
		AdoptiveChildren.Clear();
		AdoptiveBrothersAndSisters.Clear();
		SwornBrothersAndSisters.Clear();
		HusbandsAndWives.Clear();
		Mentors.Clear();
		Mentees.Clear();
		Friends.Clear();
		Adored.Clear();
		Enemies.Clear();
	}

	public CharacterSet GetCharacterSet(ushort relationType)
	{
		return relationType switch
		{
			0 => General, 
			1 => BloodParents, 
			2 => BloodChildren, 
			4 => BloodBrothersAndSisters, 
			8 => StepParents, 
			16 => StepChildren, 
			32 => StepBrothersAndSisters, 
			64 => AdoptiveParents, 
			128 => AdoptiveChildren, 
			256 => AdoptiveBrothersAndSisters, 
			512 => SwornBrothersAndSisters, 
			1024 => HusbandsAndWives, 
			2048 => Mentors, 
			4096 => Mentees, 
			8192 => Friends, 
			16384 => Adored, 
			32768 => Enemies, 
			_ => throw new Exception($"Unsupported relationType {relationType}"), 
		};
	}

	public void SetCharacterSet(ushort relationType, CharacterSet set)
	{
		switch (relationType)
		{
		case 0:
			General = set;
			break;
		case 1:
			BloodParents = set;
			break;
		case 2:
			BloodChildren = set;
			break;
		case 4:
			BloodBrothersAndSisters = set;
			break;
		case 8:
			StepParents = set;
			break;
		case 16:
			StepChildren = set;
			break;
		case 32:
			StepBrothersAndSisters = set;
			break;
		case 64:
			AdoptiveParents = set;
			break;
		case 128:
			AdoptiveChildren = set;
			break;
		case 256:
			AdoptiveBrothersAndSisters = set;
			break;
		case 512:
			SwornBrothersAndSisters = set;
			break;
		case 1024:
			HusbandsAndWives = set;
			break;
		case 2048:
			Mentors = set;
			break;
		case 4096:
			Mentees = set;
			break;
		case 8192:
			Friends = set;
			break;
		case 16384:
			Adored = set;
			break;
		case 32768:
			Enemies = set;
			break;
		default:
			throw new Exception($"Unsupported relationType {relationType}");
		}
	}

	public void GetAllRelatedCharIds(HashSet<int> charIds, bool includeGeneral = true)
	{
		if (includeGeneral)
		{
			charIds.UnionWith(General.GetCollection());
		}
		charIds.UnionWith(BloodParents.GetCollection());
		charIds.UnionWith(BloodChildren.GetCollection());
		charIds.UnionWith(BloodBrothersAndSisters.GetCollection());
		charIds.UnionWith(StepParents.GetCollection());
		charIds.UnionWith(StepChildren.GetCollection());
		charIds.UnionWith(StepBrothersAndSisters.GetCollection());
		charIds.UnionWith(AdoptiveParents.GetCollection());
		charIds.UnionWith(AdoptiveChildren.GetCollection());
		charIds.UnionWith(AdoptiveBrothersAndSisters.GetCollection());
		charIds.UnionWith(SwornBrothersAndSisters.GetCollection());
		charIds.UnionWith(HusbandsAndWives.GetCollection());
		charIds.UnionWith(Mentors.GetCollection());
		charIds.UnionWith(Mentees.GetCollection());
		charIds.UnionWith(Friends.GetCollection());
		charIds.UnionWith(Adored.GetCollection());
		charIds.UnionWith(Enemies.GetCollection());
	}

	public void GetAllTwoWayRelatedCharIds(HashSet<int> charIds)
	{
		charIds.UnionWith(BloodParents.GetCollection());
		charIds.UnionWith(BloodChildren.GetCollection());
		charIds.UnionWith(BloodBrothersAndSisters.GetCollection());
		charIds.UnionWith(StepParents.GetCollection());
		charIds.UnionWith(StepChildren.GetCollection());
		charIds.UnionWith(StepBrothersAndSisters.GetCollection());
		charIds.UnionWith(AdoptiveParents.GetCollection());
		charIds.UnionWith(AdoptiveChildren.GetCollection());
		charIds.UnionWith(AdoptiveBrothersAndSisters.GetCollection());
		charIds.UnionWith(SwornBrothersAndSisters.GetCollection());
		charIds.UnionWith(HusbandsAndWives.GetCollection());
		charIds.UnionWith(Mentors.GetCollection());
		charIds.UnionWith(Mentees.GetCollection());
		charIds.UnionWith(Friends.GetCollection());
	}

	public void GetAllPrioritizedCharIds(HashSet<int> charIds)
	{
		charIds.UnionWith(BloodParents.GetCollection());
		charIds.UnionWith(BloodChildren.GetCollection());
		charIds.UnionWith(BloodBrothersAndSisters.GetCollection());
		charIds.UnionWith(StepParents.GetCollection());
		charIds.UnionWith(StepChildren.GetCollection());
		charIds.UnionWith(StepBrothersAndSisters.GetCollection());
		charIds.UnionWith(AdoptiveParents.GetCollection());
		charIds.UnionWith(AdoptiveChildren.GetCollection());
		charIds.UnionWith(AdoptiveBrothersAndSisters.GetCollection());
		charIds.UnionWith(SwornBrothersAndSisters.GetCollection());
		charIds.UnionWith(HusbandsAndWives.GetCollection());
		charIds.UnionWith(Mentors.GetCollection());
		charIds.UnionWith(Friends.GetCollection());
		charIds.UnionWith(Adored.GetCollection());
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += General.GetSerializedSize();
		num += BloodParents.GetSerializedSize();
		num += BloodChildren.GetSerializedSize();
		num += BloodBrothersAndSisters.GetSerializedSize();
		num += StepParents.GetSerializedSize();
		num += StepChildren.GetSerializedSize();
		num += StepBrothersAndSisters.GetSerializedSize();
		num += AdoptiveParents.GetSerializedSize();
		num += AdoptiveChildren.GetSerializedSize();
		num += AdoptiveBrothersAndSisters.GetSerializedSize();
		num += SwornBrothersAndSisters.GetSerializedSize();
		num += HusbandsAndWives.GetSerializedSize();
		num += Mentors.GetSerializedSize();
		num += Mentees.GetSerializedSize();
		num += Friends.GetSerializedSize();
		num += Adored.GetSerializedSize();
		num += Enemies.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		int num = General.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		int num2 = BloodParents.Serialize(ptr);
		ptr += num2;
		Tester.Assert(num2 <= 65535);
		int num3 = BloodChildren.Serialize(ptr);
		ptr += num3;
		Tester.Assert(num3 <= 65535);
		int num4 = BloodBrothersAndSisters.Serialize(ptr);
		ptr += num4;
		Tester.Assert(num4 <= 65535);
		int num5 = StepParents.Serialize(ptr);
		ptr += num5;
		Tester.Assert(num5 <= 65535);
		int num6 = StepChildren.Serialize(ptr);
		ptr += num6;
		Tester.Assert(num6 <= 65535);
		int num7 = StepBrothersAndSisters.Serialize(ptr);
		ptr += num7;
		Tester.Assert(num7 <= 65535);
		int num8 = AdoptiveParents.Serialize(ptr);
		ptr += num8;
		Tester.Assert(num8 <= 65535);
		int num9 = AdoptiveChildren.Serialize(ptr);
		ptr += num9;
		Tester.Assert(num9 <= 65535);
		int num10 = AdoptiveBrothersAndSisters.Serialize(ptr);
		ptr += num10;
		Tester.Assert(num10 <= 65535);
		int num11 = SwornBrothersAndSisters.Serialize(ptr);
		ptr += num11;
		Tester.Assert(num11 <= 65535);
		int num12 = HusbandsAndWives.Serialize(ptr);
		ptr += num12;
		Tester.Assert(num12 <= 65535);
		int num13 = Mentors.Serialize(ptr);
		ptr += num13;
		Tester.Assert(num13 <= 65535);
		int num14 = Mentees.Serialize(ptr);
		ptr += num14;
		Tester.Assert(num14 <= 65535);
		int num15 = Friends.Serialize(ptr);
		ptr += num15;
		Tester.Assert(num15 <= 65535);
		int num16 = Adored.Serialize(ptr);
		ptr += num16;
		Tester.Assert(num16 <= 65535);
		int num17 = Enemies.Serialize(ptr);
		ptr += num17;
		Tester.Assert(num17 <= 65535);
		int num18 = (int)(ptr - pData);
		if (num18 > 4)
		{
			return (num18 + 3) / 4 * 4;
		}
		return num18;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += General.Deserialize(ptr, usePoolObject: false);
		ptr += BloodParents.Deserialize(ptr, usePoolObject: false);
		ptr += BloodChildren.Deserialize(ptr, usePoolObject: false);
		ptr += BloodBrothersAndSisters.Deserialize(ptr, usePoolObject: false);
		ptr += StepParents.Deserialize(ptr, usePoolObject: false);
		ptr += StepChildren.Deserialize(ptr, usePoolObject: false);
		ptr += StepBrothersAndSisters.Deserialize(ptr, usePoolObject: false);
		ptr += AdoptiveParents.Deserialize(ptr, usePoolObject: false);
		ptr += AdoptiveChildren.Deserialize(ptr, usePoolObject: false);
		ptr += AdoptiveBrothersAndSisters.Deserialize(ptr, usePoolObject: false);
		ptr += SwornBrothersAndSisters.Deserialize(ptr, usePoolObject: false);
		ptr += HusbandsAndWives.Deserialize(ptr, usePoolObject: false);
		ptr += Mentors.Deserialize(ptr, usePoolObject: false);
		ptr += Mentees.Deserialize(ptr, usePoolObject: false);
		ptr += Friends.Deserialize(ptr, usePoolObject: false);
		ptr += Adored.Deserialize(ptr, usePoolObject: false);
		ptr += Enemies.Deserialize(ptr, usePoolObject: false);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
