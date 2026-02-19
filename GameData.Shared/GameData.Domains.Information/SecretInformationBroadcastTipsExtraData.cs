using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Information;

public class SecretInformationBroadcastTipsExtraData : ISerializableGameData
{
	[SerializableGameDataField]
	public int MetaDataId;

	[SerializableGameDataField]
	public List<int> StartEnemyRelationCharactersToActor;

	[SerializableGameDataField]
	public List<int> StartEnemyRelationCharactersToReactor;

	[SerializableGameDataField]
	public List<int> StartEnemyRelationCharactersToSecactor;

	[SerializableGameDataField]
	public List<int> StartEnemyRelationCharactersToSource;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((StartEnemyRelationCharactersToActor == null) ? (num + 2) : (num + (2 + 4 * StartEnemyRelationCharactersToActor.Count)));
		num = ((StartEnemyRelationCharactersToReactor == null) ? (num + 2) : (num + (2 + 4 * StartEnemyRelationCharactersToReactor.Count)));
		num = ((StartEnemyRelationCharactersToSecactor == null) ? (num + 2) : (num + (2 + 4 * StartEnemyRelationCharactersToSecactor.Count)));
		num = ((StartEnemyRelationCharactersToSource == null) ? (num + 2) : (num + (2 + 4 * StartEnemyRelationCharactersToSource.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = MetaDataId;
		ptr += 4;
		if (StartEnemyRelationCharactersToActor != null)
		{
			int count = StartEnemyRelationCharactersToActor.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = StartEnemyRelationCharactersToActor[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (StartEnemyRelationCharactersToReactor != null)
		{
			int count2 = StartEnemyRelationCharactersToReactor.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((int*)ptr)[j] = StartEnemyRelationCharactersToReactor[j];
			}
			ptr += 4 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (StartEnemyRelationCharactersToSecactor != null)
		{
			int count3 = StartEnemyRelationCharactersToSecactor.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((int*)ptr)[k] = StartEnemyRelationCharactersToSecactor[k];
			}
			ptr += 4 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (StartEnemyRelationCharactersToSource != null)
		{
			int count4 = StartEnemyRelationCharactersToSource.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int l = 0; l < count4; l++)
			{
				((int*)ptr)[l] = StartEnemyRelationCharactersToSource[l];
			}
			ptr += 4 * count4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		MetaDataId = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (StartEnemyRelationCharactersToActor == null)
			{
				StartEnemyRelationCharactersToActor = new List<int>(num);
			}
			else
			{
				StartEnemyRelationCharactersToActor.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				StartEnemyRelationCharactersToActor.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			StartEnemyRelationCharactersToActor?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (StartEnemyRelationCharactersToReactor == null)
			{
				StartEnemyRelationCharactersToReactor = new List<int>(num2);
			}
			else
			{
				StartEnemyRelationCharactersToReactor.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				StartEnemyRelationCharactersToReactor.Add(((int*)ptr)[j]);
			}
			ptr += 4 * num2;
		}
		else
		{
			StartEnemyRelationCharactersToReactor?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (StartEnemyRelationCharactersToSecactor == null)
			{
				StartEnemyRelationCharactersToSecactor = new List<int>(num3);
			}
			else
			{
				StartEnemyRelationCharactersToSecactor.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				StartEnemyRelationCharactersToSecactor.Add(((int*)ptr)[k]);
			}
			ptr += 4 * num3;
		}
		else
		{
			StartEnemyRelationCharactersToSecactor?.Clear();
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (StartEnemyRelationCharactersToSource == null)
			{
				StartEnemyRelationCharactersToSource = new List<int>(num4);
			}
			else
			{
				StartEnemyRelationCharactersToSource.Clear();
			}
			for (int l = 0; l < num4; l++)
			{
				StartEnemyRelationCharactersToSource.Add(((int*)ptr)[l]);
			}
			ptr += 4 * num4;
		}
		else
		{
			StartEnemyRelationCharactersToSource?.Clear();
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
