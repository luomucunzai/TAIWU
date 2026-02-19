using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

[SerializableGameData(NotForArchive = true)]
public class TravelPreviewDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public short ToAreaId;

	[SerializableGameDataField]
	public List<short> NeedUnlockStations;

	[SerializableGameDataField]
	public int AuthorityCost;

	[SerializableGameDataField]
	public int MoneyCost;

	[SerializableGameDataField]
	public int DaysCost;

	[SerializableGameDataField]
	public int CurrentAuthority;

	public TravelPreviewDisplayData()
	{
	}

	public TravelPreviewDisplayData(TravelPreviewDisplayData other)
	{
		ToAreaId = other.ToAreaId;
		NeedUnlockStations = ((other.NeedUnlockStations == null) ? null : new List<short>(other.NeedUnlockStations));
		AuthorityCost = other.AuthorityCost;
		MoneyCost = other.MoneyCost;
		DaysCost = other.DaysCost;
		CurrentAuthority = other.CurrentAuthority;
	}

	public void Assign(TravelPreviewDisplayData other)
	{
		ToAreaId = other.ToAreaId;
		NeedUnlockStations = ((other.NeedUnlockStations == null) ? null : new List<short>(other.NeedUnlockStations));
		AuthorityCost = other.AuthorityCost;
		MoneyCost = other.MoneyCost;
		DaysCost = other.DaysCost;
		CurrentAuthority = other.CurrentAuthority;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 18;
		num = ((NeedUnlockStations == null) ? (num + 2) : (num + (2 + 2 * NeedUnlockStations.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = ToAreaId;
		ptr += 2;
		if (NeedUnlockStations != null)
		{
			int count = NeedUnlockStations.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = NeedUnlockStations[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = AuthorityCost;
		ptr += 4;
		*(int*)ptr = MoneyCost;
		ptr += 4;
		*(int*)ptr = DaysCost;
		ptr += 4;
		*(int*)ptr = CurrentAuthority;
		ptr += 4;
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
		ToAreaId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (NeedUnlockStations == null)
			{
				NeedUnlockStations = new List<short>(num);
			}
			else
			{
				NeedUnlockStations.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				NeedUnlockStations.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			NeedUnlockStations?.Clear();
		}
		AuthorityCost = *(int*)ptr;
		ptr += 4;
		MoneyCost = *(int*)ptr;
		ptr += 4;
		DaysCost = *(int*)ptr;
		ptr += 4;
		CurrentAuthority = *(int*)ptr;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
