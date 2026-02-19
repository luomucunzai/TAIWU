using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.DLC.FiveLoong;

[SerializableGameData(IsExtensible = true)]
public class JiaoLoongDisplayData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort IsJiao = 1;

		public const ushort Jiao = 2;

		public const ushort Loong = 3;

		public const ushort EvolutionChoice = 4;

		public const ushort TemplateId = 5;

		public const ushort TamePoint = 6;

		public const ushort MaxTamePoint = 7;

		public const ushort ItemDisplayData = 8;

		public const ushort Count = 9;

		public static readonly string[] FieldId2FieldName = new string[9] { "Id", "IsJiao", "Jiao", "Loong", "EvolutionChoice", "TemplateId", "TamePoint", "MaxTamePoint", "ItemDisplayData" };
	}

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public ItemDisplayData ItemDisplayData;

	[SerializableGameDataField]
	public bool IsJiao;

	[SerializableGameDataField]
	public Jiao Jiao;

	[SerializableGameDataField]
	public ChildrenOfLoong Loong;

	[SerializableGameDataField]
	public int EvolutionChoice;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public int TamePoint;

	[SerializableGameDataField]
	public int MaxTamePoint;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 21;
		num = ((Jiao == null) ? (num + 2) : (num + (2 + Jiao.GetSerializedSize())));
		num = ((Loong == null) ? (num + 2) : (num + (2 + Loong.GetSerializedSize())));
		num = ((ItemDisplayData == null) ? (num + 2) : (num + (2 + ItemDisplayData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 9;
		ptr += 2;
		*(int*)ptr = Id;
		ptr += 4;
		*ptr = (IsJiao ? ((byte)1) : ((byte)0));
		ptr++;
		if (Jiao != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = Jiao.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (Loong != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = Loong.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = EvolutionChoice;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*(int*)ptr = TamePoint;
		ptr += 4;
		*(int*)ptr = MaxTamePoint;
		ptr += 4;
		if (ItemDisplayData != null)
		{
			byte* intPtr3 = ptr;
			ptr += 2;
			int num3 = ItemDisplayData.Serialize(ptr);
			ptr += num3;
			Tester.Assert(num3 <= 65535);
			*(ushort*)intPtr3 = (ushort)num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			Id = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			IsJiao = *ptr != 0;
			ptr++;
		}
		if (num > 2)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (Jiao == null)
				{
					Jiao = new Jiao();
				}
				ptr += Jiao.Deserialize(ptr);
			}
			else
			{
				Jiao = null;
			}
		}
		if (num > 3)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (Loong == null)
				{
					Loong = new ChildrenOfLoong();
				}
				ptr += Loong.Deserialize(ptr);
			}
			else
			{
				Loong = null;
			}
		}
		if (num > 4)
		{
			EvolutionChoice = *(int*)ptr;
			ptr += 4;
		}
		if (num > 5)
		{
			TemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 6)
		{
			TamePoint = *(int*)ptr;
			ptr += 4;
		}
		if (num > 7)
		{
			MaxTamePoint = *(int*)ptr;
			ptr += 4;
		}
		if (num > 8)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (ItemDisplayData == null)
				{
					ItemDisplayData = new ItemDisplayData();
				}
				ptr += ItemDisplayData.Deserialize(ptr);
			}
			else
			{
				ItemDisplayData = null;
			}
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
