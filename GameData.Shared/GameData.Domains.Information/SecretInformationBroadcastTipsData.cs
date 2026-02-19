using System.Collections.Generic;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Information;

public class SecretInformationBroadcastTipsData : ISerializableGameData
{
	[SerializableGameDataField]
	public int MetaDataId;

	public SecretInformationDisplayData DisplayData;

	public static SecretInformationDisplayPackage DisplayPackage;

	public static Dictionary<int, NameRelatedData> NameRelatedDataMap;

	[SerializableGameDataField]
	public byte BroadcastType;

	[SerializableGameDataField]
	public List<int> FameActionsOfMain;

	[SerializableGameDataField]
	public List<int> FameActionsOfTarget1;

	[SerializableGameDataField]
	public List<int> FameActionsOfTarget2;

	[SerializableGameDataField]
	public List<int> HappinessUpCharacters;

	[SerializableGameDataField]
	public List<int> HappinessDownCharacters;

	[SerializableGameDataField]
	public List<int> FavorToMainUpCharacters;

	[SerializableGameDataField]
	public List<int> FavorToMainDownCharacters;

	[SerializableGameDataField]
	public List<int> FavorToTarget1UpCharacters;

	[SerializableGameDataField]
	public List<int> FavorToTarget1DownCharacters;

	[SerializableGameDataField]
	public List<int> FavorToTarget2UpCharacters;

	[SerializableGameDataField]
	public List<int> FavorToTarget2DownCharacters;

	public SecretInformationBroadcastTipsData()
	{
	}

	public SecretInformationBroadcastTipsData(SecretInformationBroadcastTipsData other)
	{
		MetaDataId = other.MetaDataId;
		BroadcastType = other.BroadcastType;
		FameActionsOfMain = ((other.FameActionsOfMain == null) ? null : new List<int>(other.FameActionsOfMain));
		FameActionsOfTarget1 = ((other.FameActionsOfTarget1 == null) ? null : new List<int>(other.FameActionsOfTarget1));
		FameActionsOfTarget2 = ((other.FameActionsOfTarget2 == null) ? null : new List<int>(other.FameActionsOfTarget2));
		HappinessUpCharacters = ((other.HappinessUpCharacters == null) ? null : new List<int>(other.HappinessUpCharacters));
		HappinessDownCharacters = ((other.HappinessDownCharacters == null) ? null : new List<int>(other.HappinessDownCharacters));
		FavorToMainUpCharacters = ((other.FavorToMainUpCharacters == null) ? null : new List<int>(other.FavorToMainUpCharacters));
		FavorToMainDownCharacters = ((other.FavorToMainDownCharacters == null) ? null : new List<int>(other.FavorToMainDownCharacters));
		FavorToTarget1UpCharacters = ((other.FavorToTarget1UpCharacters == null) ? null : new List<int>(other.FavorToTarget1UpCharacters));
		FavorToTarget1DownCharacters = ((other.FavorToTarget1DownCharacters == null) ? null : new List<int>(other.FavorToTarget1DownCharacters));
		FavorToTarget2UpCharacters = ((other.FavorToTarget2UpCharacters == null) ? null : new List<int>(other.FavorToTarget2UpCharacters));
		FavorToTarget2DownCharacters = ((other.FavorToTarget2DownCharacters == null) ? null : new List<int>(other.FavorToTarget2DownCharacters));
	}

	public void Assign(SecretInformationBroadcastTipsData other)
	{
		MetaDataId = other.MetaDataId;
		BroadcastType = other.BroadcastType;
		FameActionsOfMain = ((other.FameActionsOfMain == null) ? null : new List<int>(other.FameActionsOfMain));
		FameActionsOfTarget1 = ((other.FameActionsOfTarget1 == null) ? null : new List<int>(other.FameActionsOfTarget1));
		FameActionsOfTarget2 = ((other.FameActionsOfTarget2 == null) ? null : new List<int>(other.FameActionsOfTarget2));
		HappinessUpCharacters = ((other.HappinessUpCharacters == null) ? null : new List<int>(other.HappinessUpCharacters));
		HappinessDownCharacters = ((other.HappinessDownCharacters == null) ? null : new List<int>(other.HappinessDownCharacters));
		FavorToMainUpCharacters = ((other.FavorToMainUpCharacters == null) ? null : new List<int>(other.FavorToMainUpCharacters));
		FavorToMainDownCharacters = ((other.FavorToMainDownCharacters == null) ? null : new List<int>(other.FavorToMainDownCharacters));
		FavorToTarget1UpCharacters = ((other.FavorToTarget1UpCharacters == null) ? null : new List<int>(other.FavorToTarget1UpCharacters));
		FavorToTarget1DownCharacters = ((other.FavorToTarget1DownCharacters == null) ? null : new List<int>(other.FavorToTarget1DownCharacters));
		FavorToTarget2UpCharacters = ((other.FavorToTarget2UpCharacters == null) ? null : new List<int>(other.FavorToTarget2UpCharacters));
		FavorToTarget2DownCharacters = ((other.FavorToTarget2DownCharacters == null) ? null : new List<int>(other.FavorToTarget2DownCharacters));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 5;
		num = ((FameActionsOfMain == null) ? (num + 2) : (num + (2 + 4 * FameActionsOfMain.Count)));
		num = ((FameActionsOfTarget1 == null) ? (num + 2) : (num + (2 + 4 * FameActionsOfTarget1.Count)));
		num = ((FameActionsOfTarget2 == null) ? (num + 2) : (num + (2 + 4 * FameActionsOfTarget2.Count)));
		num = ((HappinessUpCharacters == null) ? (num + 2) : (num + (2 + 4 * HappinessUpCharacters.Count)));
		num = ((HappinessDownCharacters == null) ? (num + 2) : (num + (2 + 4 * HappinessDownCharacters.Count)));
		num = ((FavorToMainUpCharacters == null) ? (num + 2) : (num + (2 + 4 * FavorToMainUpCharacters.Count)));
		num = ((FavorToMainDownCharacters == null) ? (num + 2) : (num + (2 + 4 * FavorToMainDownCharacters.Count)));
		num = ((FavorToTarget1UpCharacters == null) ? (num + 2) : (num + (2 + 4 * FavorToTarget1UpCharacters.Count)));
		num = ((FavorToTarget1DownCharacters == null) ? (num + 2) : (num + (2 + 4 * FavorToTarget1DownCharacters.Count)));
		num = ((FavorToTarget2UpCharacters == null) ? (num + 2) : (num + (2 + 4 * FavorToTarget2UpCharacters.Count)));
		num = ((FavorToTarget2DownCharacters == null) ? (num + 2) : (num + (2 + 4 * FavorToTarget2DownCharacters.Count)));
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
		*ptr = BroadcastType;
		ptr++;
		if (FameActionsOfMain != null)
		{
			int count = FameActionsOfMain.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = FameActionsOfMain[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FameActionsOfTarget1 != null)
		{
			int count2 = FameActionsOfTarget1.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((int*)ptr)[j] = FameActionsOfTarget1[j];
			}
			ptr += 4 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FameActionsOfTarget2 != null)
		{
			int count3 = FameActionsOfTarget2.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((int*)ptr)[k] = FameActionsOfTarget2[k];
			}
			ptr += 4 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (HappinessUpCharacters != null)
		{
			int count4 = HappinessUpCharacters.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int l = 0; l < count4; l++)
			{
				((int*)ptr)[l] = HappinessUpCharacters[l];
			}
			ptr += 4 * count4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (HappinessDownCharacters != null)
		{
			int count5 = HappinessDownCharacters.Count;
			Tester.Assert(count5 <= 65535);
			*(ushort*)ptr = (ushort)count5;
			ptr += 2;
			for (int m = 0; m < count5; m++)
			{
				((int*)ptr)[m] = HappinessDownCharacters[m];
			}
			ptr += 4 * count5;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FavorToMainUpCharacters != null)
		{
			int count6 = FavorToMainUpCharacters.Count;
			Tester.Assert(count6 <= 65535);
			*(ushort*)ptr = (ushort)count6;
			ptr += 2;
			for (int n = 0; n < count6; n++)
			{
				((int*)ptr)[n] = FavorToMainUpCharacters[n];
			}
			ptr += 4 * count6;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FavorToMainDownCharacters != null)
		{
			int count7 = FavorToMainDownCharacters.Count;
			Tester.Assert(count7 <= 65535);
			*(ushort*)ptr = (ushort)count7;
			ptr += 2;
			for (int num = 0; num < count7; num++)
			{
				((int*)ptr)[num] = FavorToMainDownCharacters[num];
			}
			ptr += 4 * count7;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FavorToTarget1UpCharacters != null)
		{
			int count8 = FavorToTarget1UpCharacters.Count;
			Tester.Assert(count8 <= 65535);
			*(ushort*)ptr = (ushort)count8;
			ptr += 2;
			for (int num2 = 0; num2 < count8; num2++)
			{
				((int*)ptr)[num2] = FavorToTarget1UpCharacters[num2];
			}
			ptr += 4 * count8;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FavorToTarget1DownCharacters != null)
		{
			int count9 = FavorToTarget1DownCharacters.Count;
			Tester.Assert(count9 <= 65535);
			*(ushort*)ptr = (ushort)count9;
			ptr += 2;
			for (int num3 = 0; num3 < count9; num3++)
			{
				((int*)ptr)[num3] = FavorToTarget1DownCharacters[num3];
			}
			ptr += 4 * count9;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FavorToTarget2UpCharacters != null)
		{
			int count10 = FavorToTarget2UpCharacters.Count;
			Tester.Assert(count10 <= 65535);
			*(ushort*)ptr = (ushort)count10;
			ptr += 2;
			for (int num4 = 0; num4 < count10; num4++)
			{
				((int*)ptr)[num4] = FavorToTarget2UpCharacters[num4];
			}
			ptr += 4 * count10;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FavorToTarget2DownCharacters != null)
		{
			int count11 = FavorToTarget2DownCharacters.Count;
			Tester.Assert(count11 <= 65535);
			*(ushort*)ptr = (ushort)count11;
			ptr += 2;
			for (int num5 = 0; num5 < count11; num5++)
			{
				((int*)ptr)[num5] = FavorToTarget2DownCharacters[num5];
			}
			ptr += 4 * count11;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		MetaDataId = *(int*)ptr;
		ptr += 4;
		BroadcastType = *ptr;
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (FameActionsOfMain == null)
			{
				FameActionsOfMain = new List<int>(num);
			}
			else
			{
				FameActionsOfMain.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				FameActionsOfMain.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			FameActionsOfMain?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (FameActionsOfTarget1 == null)
			{
				FameActionsOfTarget1 = new List<int>(num2);
			}
			else
			{
				FameActionsOfTarget1.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				FameActionsOfTarget1.Add(((int*)ptr)[j]);
			}
			ptr += 4 * num2;
		}
		else
		{
			FameActionsOfTarget1?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (FameActionsOfTarget2 == null)
			{
				FameActionsOfTarget2 = new List<int>(num3);
			}
			else
			{
				FameActionsOfTarget2.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				FameActionsOfTarget2.Add(((int*)ptr)[k]);
			}
			ptr += 4 * num3;
		}
		else
		{
			FameActionsOfTarget2?.Clear();
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (HappinessUpCharacters == null)
			{
				HappinessUpCharacters = new List<int>(num4);
			}
			else
			{
				HappinessUpCharacters.Clear();
			}
			for (int l = 0; l < num4; l++)
			{
				HappinessUpCharacters.Add(((int*)ptr)[l]);
			}
			ptr += 4 * num4;
		}
		else
		{
			HappinessUpCharacters?.Clear();
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (HappinessDownCharacters == null)
			{
				HappinessDownCharacters = new List<int>(num5);
			}
			else
			{
				HappinessDownCharacters.Clear();
			}
			for (int m = 0; m < num5; m++)
			{
				HappinessDownCharacters.Add(((int*)ptr)[m]);
			}
			ptr += 4 * num5;
		}
		else
		{
			HappinessDownCharacters?.Clear();
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (FavorToMainUpCharacters == null)
			{
				FavorToMainUpCharacters = new List<int>(num6);
			}
			else
			{
				FavorToMainUpCharacters.Clear();
			}
			for (int n = 0; n < num6; n++)
			{
				FavorToMainUpCharacters.Add(((int*)ptr)[n]);
			}
			ptr += 4 * num6;
		}
		else
		{
			FavorToMainUpCharacters?.Clear();
		}
		ushort num7 = *(ushort*)ptr;
		ptr += 2;
		if (num7 > 0)
		{
			if (FavorToMainDownCharacters == null)
			{
				FavorToMainDownCharacters = new List<int>(num7);
			}
			else
			{
				FavorToMainDownCharacters.Clear();
			}
			for (int num8 = 0; num8 < num7; num8++)
			{
				FavorToMainDownCharacters.Add(((int*)ptr)[num8]);
			}
			ptr += 4 * num7;
		}
		else
		{
			FavorToMainDownCharacters?.Clear();
		}
		ushort num9 = *(ushort*)ptr;
		ptr += 2;
		if (num9 > 0)
		{
			if (FavorToTarget1UpCharacters == null)
			{
				FavorToTarget1UpCharacters = new List<int>(num9);
			}
			else
			{
				FavorToTarget1UpCharacters.Clear();
			}
			for (int num10 = 0; num10 < num9; num10++)
			{
				FavorToTarget1UpCharacters.Add(((int*)ptr)[num10]);
			}
			ptr += 4 * num9;
		}
		else
		{
			FavorToTarget1UpCharacters?.Clear();
		}
		ushort num11 = *(ushort*)ptr;
		ptr += 2;
		if (num11 > 0)
		{
			if (FavorToTarget1DownCharacters == null)
			{
				FavorToTarget1DownCharacters = new List<int>(num11);
			}
			else
			{
				FavorToTarget1DownCharacters.Clear();
			}
			for (int num12 = 0; num12 < num11; num12++)
			{
				FavorToTarget1DownCharacters.Add(((int*)ptr)[num12]);
			}
			ptr += 4 * num11;
		}
		else
		{
			FavorToTarget1DownCharacters?.Clear();
		}
		ushort num13 = *(ushort*)ptr;
		ptr += 2;
		if (num13 > 0)
		{
			if (FavorToTarget2UpCharacters == null)
			{
				FavorToTarget2UpCharacters = new List<int>(num13);
			}
			else
			{
				FavorToTarget2UpCharacters.Clear();
			}
			for (int num14 = 0; num14 < num13; num14++)
			{
				FavorToTarget2UpCharacters.Add(((int*)ptr)[num14]);
			}
			ptr += 4 * num13;
		}
		else
		{
			FavorToTarget2UpCharacters?.Clear();
		}
		ushort num15 = *(ushort*)ptr;
		ptr += 2;
		if (num15 > 0)
		{
			if (FavorToTarget2DownCharacters == null)
			{
				FavorToTarget2DownCharacters = new List<int>(num15);
			}
			else
			{
				FavorToTarget2DownCharacters.Clear();
			}
			for (int num16 = 0; num16 < num15; num16++)
			{
				FavorToTarget2DownCharacters.Add(((int*)ptr)[num16]);
			}
			ptr += 4 * num15;
		}
		else
		{
			FavorToTarget2DownCharacters?.Clear();
		}
		int num17 = (int)(ptr - pData);
		if (num17 > 4)
		{
			return (num17 + 3) / 4 * 4;
		}
		return num17;
	}
}
