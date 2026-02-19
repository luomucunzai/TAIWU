using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public class TeammateCommandChangeDataPart : ISerializableGameData
{
	[SerializableGameDataField]
	public List<int> TeammateCharIds = new List<int>();

	[SerializableGameDataField]
	public List<SByteList> OriginTeammateCommands = new List<SByteList>();

	[SerializableGameDataField]
	public List<SByteList> ReplaceTeammateCommands = new List<SByteList>();

	[SerializableGameDataField]
	public Dictionary<int, int> BetrayedCharIds = new Dictionary<int, int>();

	public TeammateCommandChangeDataPart()
	{
	}

	public TeammateCommandChangeDataPart(TeammateCommandChangeDataPart other)
	{
		TeammateCharIds = ((other.TeammateCharIds == null) ? null : new List<int>(other.TeammateCharIds));
		if (other.OriginTeammateCommands != null)
		{
			List<SByteList> originTeammateCommands = other.OriginTeammateCommands;
			int count = originTeammateCommands.Count;
			OriginTeammateCommands = new List<SByteList>(count);
			for (int i = 0; i < count; i++)
			{
				OriginTeammateCommands.Add(new SByteList(originTeammateCommands[i]));
			}
		}
		else
		{
			OriginTeammateCommands = null;
		}
		if (other.ReplaceTeammateCommands != null)
		{
			List<SByteList> replaceTeammateCommands = other.ReplaceTeammateCommands;
			int count2 = replaceTeammateCommands.Count;
			ReplaceTeammateCommands = new List<SByteList>(count2);
			for (int j = 0; j < count2; j++)
			{
				ReplaceTeammateCommands.Add(new SByteList(replaceTeammateCommands[j]));
			}
		}
		else
		{
			ReplaceTeammateCommands = null;
		}
		BetrayedCharIds = ((other.BetrayedCharIds == null) ? null : new Dictionary<int, int>(other.BetrayedCharIds));
	}

	public void Assign(TeammateCommandChangeDataPart other)
	{
		TeammateCharIds = ((other.TeammateCharIds == null) ? null : new List<int>(other.TeammateCharIds));
		if (other.OriginTeammateCommands != null)
		{
			List<SByteList> originTeammateCommands = other.OriginTeammateCommands;
			int count = originTeammateCommands.Count;
			OriginTeammateCommands = new List<SByteList>(count);
			for (int i = 0; i < count; i++)
			{
				OriginTeammateCommands.Add(new SByteList(originTeammateCommands[i]));
			}
		}
		else
		{
			OriginTeammateCommands = null;
		}
		if (other.ReplaceTeammateCommands != null)
		{
			List<SByteList> replaceTeammateCommands = other.ReplaceTeammateCommands;
			int count2 = replaceTeammateCommands.Count;
			ReplaceTeammateCommands = new List<SByteList>(count2);
			for (int j = 0; j < count2; j++)
			{
				ReplaceTeammateCommands.Add(new SByteList(replaceTeammateCommands[j]));
			}
		}
		else
		{
			ReplaceTeammateCommands = null;
		}
		BetrayedCharIds = ((other.BetrayedCharIds == null) ? null : new Dictionary<int, int>(other.BetrayedCharIds));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((TeammateCharIds == null) ? (num + 2) : (num + (2 + 4 * TeammateCharIds.Count)));
		if (OriginTeammateCommands != null)
		{
			num += 2;
			int count = OriginTeammateCommands.Count;
			for (int i = 0; i < count; i++)
			{
				num += OriginTeammateCommands[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		if (ReplaceTeammateCommands != null)
		{
			num += 2;
			int count2 = ReplaceTeammateCommands.Count;
			for (int j = 0; j < count2; j++)
			{
				num += ReplaceTeammateCommands[j].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, int>((IReadOnlyDictionary<int, int>)BetrayedCharIds);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (TeammateCharIds != null)
		{
			int count = TeammateCharIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = TeammateCharIds[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (OriginTeammateCommands != null)
		{
			int count2 = OriginTeammateCommands.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				int num = OriginTeammateCommands[j].Serialize(ptr);
				ptr += num;
				Tester.Assert(num <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ReplaceTeammateCommands != null)
		{
			int count3 = ReplaceTeammateCommands.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				int num2 = ReplaceTeammateCommands[k].Serialize(ptr);
				ptr += num2;
				Tester.Assert(num2 <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypePair.Serialize<int, int>(ptr, ref BetrayedCharIds);
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (TeammateCharIds == null)
			{
				TeammateCharIds = new List<int>(num);
			}
			else
			{
				TeammateCharIds.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				TeammateCharIds.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			TeammateCharIds?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (OriginTeammateCommands == null)
			{
				OriginTeammateCommands = new List<SByteList>(num2);
			}
			else
			{
				OriginTeammateCommands.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				SByteList item = default(SByteList);
				ptr += item.Deserialize(ptr);
				OriginTeammateCommands.Add(item);
			}
		}
		else
		{
			OriginTeammateCommands?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (ReplaceTeammateCommands == null)
			{
				ReplaceTeammateCommands = new List<SByteList>(num3);
			}
			else
			{
				ReplaceTeammateCommands.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				SByteList item2 = default(SByteList);
				ptr += item2.Deserialize(ptr);
				ReplaceTeammateCommands.Add(item2);
			}
		}
		else
		{
			ReplaceTeammateCommands?.Clear();
		}
		ptr += DictionaryOfBasicTypePair.Deserialize<int, int>(ptr, ref BetrayedCharIds);
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}
}
