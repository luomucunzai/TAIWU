using System;
using System.Text;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[Serializable]
public class EventActorData : ISerializableGameData
{
	[SerializableGameDataField]
	public short TemplateId;

	public FullName FullName;

	[SerializableGameDataField]
	public string DisplayName;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public byte Age;

	[SerializableGameDataField]
	public AvatarData AvatarData;

	[SerializableGameDataField]
	public short ClothDisplayId;

	public EventActorData(short templateId)
	{
		TemplateId = templateId;
		AvatarData = new AvatarData();
	}

	public EventActorData()
	{
	}

	public EventActorData(EventActorData other)
	{
		TemplateId = other.TemplateId;
		DisplayName = other.DisplayName;
		Gender = other.Gender;
		Age = other.Age;
		AvatarData = new AvatarData(other.AvatarData);
		ClothDisplayId = other.ClothDisplayId;
	}

	public void Assign(EventActorData other)
	{
		TemplateId = other.TemplateId;
		DisplayName = other.DisplayName;
		Gender = other.Gender;
		Age = other.Age;
		AvatarData = new AvatarData(other.AvatarData);
		ClothDisplayId = other.ClothDisplayId;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 82;
		num = ((DisplayName == null) ? (num + 2) : (num + (2 + 2 * DisplayName.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = TemplateId;
		ptr += 2;
		if (DisplayName != null)
		{
			int length = DisplayName.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* displayName = DisplayName)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)displayName[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)Gender;
		ptr++;
		*ptr = Age;
		ptr++;
		ptr += AvatarData.Serialize(ptr);
		*(short*)ptr = ClothDisplayId;
		ptr += 2;
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
		TemplateId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			DisplayName = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			DisplayName = null;
		}
		Gender = (sbyte)(*ptr);
		ptr++;
		Age = *ptr;
		ptr++;
		if (AvatarData == null)
		{
			AvatarData = new AvatarData();
		}
		ptr += AvatarData.Deserialize(ptr);
		ClothDisplayId = *(short*)ptr;
		ptr += 2;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
