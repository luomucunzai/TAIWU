using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public class SilenceData : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<short, SilenceFrameData> CombatSkill = new Dictionary<short, SilenceFrameData>();

	[SerializableGameDataField]
	public List<ItemKey> WeaponKeys = new List<ItemKey>();

	[SerializableGameDataField]
	public List<SilenceFrameData> WeaponFrames = new List<SilenceFrameData>();

	public SilenceData()
	{
	}

	public SilenceData(SilenceData other)
	{
		CombatSkill = ((other.CombatSkill == null) ? null : new Dictionary<short, SilenceFrameData>(other.CombatSkill));
		WeaponKeys = ((other.WeaponKeys == null) ? null : new List<ItemKey>(other.WeaponKeys));
		WeaponFrames = ((other.WeaponFrames == null) ? null : new List<SilenceFrameData>(other.WeaponFrames));
	}

	public void Assign(SilenceData other)
	{
		CombatSkill = ((other.CombatSkill == null) ? null : new Dictionary<short, SilenceFrameData>(other.CombatSkill));
		WeaponKeys = ((other.WeaponKeys == null) ? null : new List<ItemKey>(other.WeaponKeys));
		WeaponFrames = ((other.WeaponFrames == null) ? null : new List<SilenceFrameData>(other.WeaponFrames));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SilenceFrameData>(CombatSkill);
		num = ((WeaponKeys == null) ? (num + 2) : (num + (2 + 8 * WeaponKeys.Count)));
		num = ((WeaponFrames == null) ? (num + 2) : (num + (2 + 8 * WeaponFrames.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SilenceFrameData>(ptr, ref CombatSkill);
		if (WeaponKeys != null)
		{
			int count = WeaponKeys.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += WeaponKeys[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (WeaponFrames != null)
		{
			int count2 = WeaponFrames.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr += WeaponFrames[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SilenceFrameData>(ptr, ref CombatSkill);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (WeaponKeys == null)
			{
				WeaponKeys = new List<ItemKey>(num);
			}
			else
			{
				WeaponKeys.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ItemKey item = default(ItemKey);
				ptr += item.Deserialize(ptr);
				WeaponKeys.Add(item);
			}
		}
		else
		{
			WeaponKeys?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (WeaponFrames == null)
			{
				WeaponFrames = new List<SilenceFrameData>(num2);
			}
			else
			{
				WeaponFrames.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				SilenceFrameData item2 = default(SilenceFrameData);
				ptr += item2.Deserialize(ptr);
				WeaponFrames.Add(item2);
			}
		}
		else
		{
			WeaponFrames?.Clear();
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
