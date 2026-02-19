using System;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.World.SectMainStory;

[SerializableGameData(IsExtensible = true)]
public class SectBaihuaLifeLinkData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort LifeGateCharIds = 0;

		public const ushort DeathGateCharIds = 1;

		public const ushort Cooldown = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "LifeGateCharIds", "DeathGateCharIds", "Cooldown" };
	}

	[SerializableGameDataField]
	public int[] LifeGateCharIds;

	[SerializableGameDataField]
	public int[] DeathGateCharIds;

	[SerializableGameDataField]
	public sbyte Cooldown;

	public const int InitialGateCharCount = 4;

	public const int BonusGateCharCount = 4;

	public const int MaxGateCharCount = 8;

	public bool IsInitialized()
	{
		if (LifeGateCharIds != null)
		{
			return DeathGateCharIds != null;
		}
		return false;
	}

	public void Initialize()
	{
		LifeGateCharIds = new int[4];
		DeathGateCharIds = new int[4];
		Array.Fill(LifeGateCharIds, -1);
		Array.Fill(DeathGateCharIds, -1);
	}

	public void Upgrade()
	{
		int[] lifeGateCharIds = LifeGateCharIds;
		int[] deathGateCharIds = DeathGateCharIds;
		LifeGateCharIds = new int[lifeGateCharIds.Length + 4];
		DeathGateCharIds = new int[deathGateCharIds.Length + 4];
		for (int i = 0; i < lifeGateCharIds.Length; i++)
		{
			LifeGateCharIds[i] = lifeGateCharIds[i];
		}
		for (int j = 0; j < deathGateCharIds.Length; j++)
		{
			DeathGateCharIds[j] = deathGateCharIds[j];
		}
		for (int k = lifeGateCharIds.Length; k < LifeGateCharIds.Length; k++)
		{
			LifeGateCharIds[k] = -1;
		}
		for (int l = deathGateCharIds.Length; l < DeathGateCharIds.Length; l++)
		{
			DeathGateCharIds[l] = -1;
		}
	}

	public SectBaihuaLifeLinkData()
	{
	}

	public SectBaihuaLifeLinkData(SectBaihuaLifeLinkData other)
	{
		int[] lifeGateCharIds = other.LifeGateCharIds;
		int num = lifeGateCharIds.Length;
		LifeGateCharIds = new int[num];
		for (int i = 0; i < num; i++)
		{
			LifeGateCharIds[i] = lifeGateCharIds[i];
		}
		int[] deathGateCharIds = other.DeathGateCharIds;
		int num2 = deathGateCharIds.Length;
		DeathGateCharIds = new int[num2];
		for (int j = 0; j < num2; j++)
		{
			DeathGateCharIds[j] = deathGateCharIds[j];
		}
		Cooldown = other.Cooldown;
	}

	public void Assign(SectBaihuaLifeLinkData other)
	{
		int[] lifeGateCharIds = other.LifeGateCharIds;
		int num = lifeGateCharIds.Length;
		LifeGateCharIds = new int[num];
		for (int i = 0; i < num; i++)
		{
			LifeGateCharIds[i] = lifeGateCharIds[i];
		}
		int[] deathGateCharIds = other.DeathGateCharIds;
		int num2 = deathGateCharIds.Length;
		DeathGateCharIds = new int[num2];
		for (int j = 0; j < num2; j++)
		{
			DeathGateCharIds[j] = deathGateCharIds[j];
		}
		Cooldown = other.Cooldown;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		num = ((LifeGateCharIds == null) ? (num + 2) : (num + (2 + 4 * LifeGateCharIds.Length)));
		num = ((DeathGateCharIds == null) ? (num + 2) : (num + (2 + 4 * DeathGateCharIds.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 3;
		ptr += 2;
		if (LifeGateCharIds != null)
		{
			int num = LifeGateCharIds.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				((int*)ptr)[i] = LifeGateCharIds[i];
			}
			ptr += 4 * num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (DeathGateCharIds != null)
		{
			int num2 = DeathGateCharIds.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int j = 0; j < num2; j++)
			{
				((int*)ptr)[j] = DeathGateCharIds[j];
			}
			ptr += 4 * num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)Cooldown;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (LifeGateCharIds == null || LifeGateCharIds.Length != num2)
				{
					LifeGateCharIds = new int[num2];
				}
				for (int i = 0; i < num2; i++)
				{
					LifeGateCharIds[i] = ((int*)ptr)[i];
				}
				ptr += 4 * num2;
			}
			else
			{
				LifeGateCharIds = null;
			}
		}
		if (num > 1)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (DeathGateCharIds == null || DeathGateCharIds.Length != num3)
				{
					DeathGateCharIds = new int[num3];
				}
				for (int j = 0; j < num3; j++)
				{
					DeathGateCharIds[j] = ((int*)ptr)[j];
				}
				ptr += 4 * num3;
			}
			else
			{
				DeathGateCharIds = null;
			}
		}
		if (num > 2)
		{
			Cooldown = (sbyte)(*ptr);
			ptr++;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
