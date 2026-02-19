using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

[SerializableGameData]
public struct MakeResult : ISerializableGameData
{
	[SerializableGameDataField]
	private int _targetStageIndex;

	[SerializableGameDataField]
	public MakeResultStage[] MakeResultItemArray;

	[SerializableGameDataField]
	public string UpgradeBuildingName;

	[SerializableGameDataField]
	public bool UpgradeBuildingCanUse;

	public MakeResultStage TargetResultStage => MakeResultItemArray[_targetStageIndex];

	public int TargetStageIndex => _targetStageIndex;

	public MakeResult(int targetStageIndex, MakeResultStage[] makeResultItemArray, string upgradeBuildingName, bool upgradeBuildingCanUse)
	{
		_targetStageIndex = targetStageIndex;
		MakeResultItemArray = makeResultItemArray;
		UpgradeBuildingName = upgradeBuildingName;
		UpgradeBuildingCanUse = upgradeBuildingCanUse;
	}

	public MakeResult(MakeResult other)
	{
		_targetStageIndex = other._targetStageIndex;
		MakeResultStage[] makeResultItemArray = other.MakeResultItemArray;
		int num = makeResultItemArray.Length;
		MakeResultItemArray = new MakeResultStage[num];
		for (int i = 0; i < num; i++)
		{
			MakeResultItemArray[i] = new MakeResultStage(makeResultItemArray[i]);
		}
		UpgradeBuildingName = other.UpgradeBuildingName;
		UpgradeBuildingCanUse = other.UpgradeBuildingCanUse;
	}

	public void Assign(MakeResult other)
	{
		_targetStageIndex = other._targetStageIndex;
		MakeResultStage[] makeResultItemArray = other.MakeResultItemArray;
		int num = makeResultItemArray.Length;
		MakeResultItemArray = new MakeResultStage[num];
		for (int i = 0; i < num; i++)
		{
			MakeResultItemArray[i] = new MakeResultStage(makeResultItemArray[i]);
		}
		UpgradeBuildingName = other.UpgradeBuildingName;
		UpgradeBuildingCanUse = other.UpgradeBuildingCanUse;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 5;
		if (MakeResultItemArray != null)
		{
			num += 2;
			int num2 = MakeResultItemArray.Length;
			for (int i = 0; i < num2; i++)
			{
				num += MakeResultItemArray[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		num = ((UpgradeBuildingName == null) ? (num + 2) : (num + (2 + 2 * UpgradeBuildingName.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _targetStageIndex;
		ptr += 4;
		if (MakeResultItemArray != null)
		{
			int num = MakeResultItemArray.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				int num2 = MakeResultItemArray[i].Serialize(ptr);
				ptr += num2;
				Tester.Assert(num2 <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (UpgradeBuildingName != null)
		{
			int length = UpgradeBuildingName.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* upgradeBuildingName = UpgradeBuildingName)
			{
				for (int j = 0; j < length; j++)
				{
					((short*)ptr)[j] = (short)upgradeBuildingName[j];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (UpgradeBuildingCanUse ? ((byte)1) : ((byte)0));
		ptr++;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_targetStageIndex = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (MakeResultItemArray == null || MakeResultItemArray.Length != num)
			{
				MakeResultItemArray = new MakeResultStage[num];
			}
			for (int i = 0; i < num; i++)
			{
				MakeResultStage makeResultStage = default(MakeResultStage);
				ptr += makeResultStage.Deserialize(ptr);
				MakeResultItemArray[i] = makeResultStage;
			}
		}
		else
		{
			MakeResultItemArray = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			int num3 = 2 * num2;
			UpgradeBuildingName = Encoding.Unicode.GetString(ptr, num3);
			ptr += num3;
		}
		else
		{
			UpgradeBuildingName = null;
		}
		UpgradeBuildingCanUse = *ptr != 0;
		ptr++;
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
