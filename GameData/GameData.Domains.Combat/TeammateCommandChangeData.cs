using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData]
public class TeammateCommandChangeData : ISerializableGameData
{
	[SerializableGameDataField]
	public TeammateCommandChangeDataPart SelfTeam;

	[SerializableGameDataField]
	public TeammateCommandChangeDataPart EnemyTeam;

	public IReadOnlyList<sbyte> GetCharTeammateCommands(int charId)
	{
		return GetCharTeammateCommandsInternal(charId);
	}

	private List<sbyte> GetCharTeammateCommandsInternal(int charId)
	{
		for (int i = 0; i < SelfTeam.TeammateCharIds.Count; i++)
		{
			int num = SelfTeam.TeammateCharIds[i];
			if (num == charId)
			{
				return SelfTeam.ReplaceTeammateCommands[i].Items;
			}
		}
		for (int j = 0; j < EnemyTeam.TeammateCharIds.Count; j++)
		{
			int num2 = EnemyTeam.TeammateCharIds[j];
			if (num2 == charId)
			{
				return EnemyTeam.ReplaceTeammateCommands[j].Items;
			}
		}
		return null;
	}

	public bool SetCharTeammateCommands(int charId, IEnumerable<sbyte> cmdTypes)
	{
		List<sbyte> charTeammateCommandsInternal = GetCharTeammateCommandsInternal(charId);
		if (charTeammateCommandsInternal == null)
		{
			return false;
		}
		charTeammateCommandsInternal.Clear();
		if (cmdTypes != null)
		{
			charTeammateCommandsInternal.AddRange(cmdTypes);
		}
		return true;
	}

	public TeammateCommandChangeData()
	{
	}

	public TeammateCommandChangeData(TeammateCommandChangeData other)
	{
		SelfTeam = new TeammateCommandChangeDataPart(other.SelfTeam);
		EnemyTeam = new TeammateCommandChangeDataPart(other.EnemyTeam);
	}

	public void Assign(TeammateCommandChangeData other)
	{
		SelfTeam = new TeammateCommandChangeDataPart(other.SelfTeam);
		EnemyTeam = new TeammateCommandChangeDataPart(other.EnemyTeam);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((SelfTeam == null) ? (num + 2) : (num + (2 + SelfTeam.GetSerializedSize())));
		num = ((EnemyTeam == null) ? (num + 2) : (num + (2 + EnemyTeam.GetSerializedSize())));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (SelfTeam != null)
		{
			byte* ptr2 = ptr;
			ptr += 2;
			int num = SelfTeam.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr2 = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (EnemyTeam != null)
		{
			byte* ptr3 = ptr;
			ptr += 2;
			int num2 = EnemyTeam.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr3 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
			if (SelfTeam == null)
			{
				SelfTeam = new TeammateCommandChangeDataPart();
			}
			ptr += SelfTeam.Deserialize(ptr);
		}
		else
		{
			SelfTeam = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (EnemyTeam == null)
			{
				EnemyTeam = new TeammateCommandChangeDataPart();
			}
			ptr += EnemyTeam.Deserialize(ptr);
		}
		else
		{
			EnemyTeam = null;
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
