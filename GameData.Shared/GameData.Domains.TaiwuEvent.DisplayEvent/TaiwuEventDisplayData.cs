using System.Collections.Generic;
using System.Text;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NotRestrictCollectionSerializedSize = true, NoCopyConstructors = true)]
public class TaiwuEventDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public string EventGuid;

	[SerializableGameDataField]
	public CharacterDisplayData MainCharacter;

	[SerializableGameDataField]
	public CharacterDisplayData TargetCharacter;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public TaiwuEventDisplayExtraData ExtraData;

	[SerializableGameDataField]
	public string EventContent;

	[SerializableGameDataField]
	public List<TaiwuEventCharacterNameDecodeData> NameDecodeDataList;

	[SerializableGameDataField]
	public List<string> ExtraFormatLanguageKeys;

	[SerializableGameDataField]
	public string EventTexture;

	[SerializableGameDataField]
	public sbyte MaskControlCode;

	[SerializableGameDataField]
	public ushort MaskTweenTime;

	[SerializableGameDataField]
	public sbyte EscOptionIndex;

	[SerializableGameDataField]
	public List<EventOptionInfo> EventOptionInfos;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((EventGuid == null) ? (num + 2) : (num + (2 + 2 * EventGuid.Length)));
		num = ((MainCharacter == null) ? (num + 2) : (num + (2 + MainCharacter.GetSerializedSize())));
		num = ((TargetCharacter == null) ? (num + 2) : (num + (2 + TargetCharacter.GetSerializedSize())));
		num = ((ExtraData == null) ? (num + 4) : (num + (4 + ExtraData.GetSerializedSize())));
		num = ((EventContent == null) ? (num + 2) : (num + (2 + 2 * EventContent.Length)));
		num = ((NameDecodeDataList == null) ? (num + 2) : (num + (2 + 36 * NameDecodeDataList.Count)));
		if (ExtraFormatLanguageKeys != null)
		{
			num += 2;
			int count = ExtraFormatLanguageKeys.Count;
			for (int i = 0; i < count; i++)
			{
				string text = ExtraFormatLanguageKeys[i];
				num = ((text == null) ? (num + 2) : (num + (2 + 2 * text.Length)));
			}
		}
		else
		{
			num += 2;
		}
		num = ((EventTexture == null) ? (num + 2) : (num + (2 + 2 * EventTexture.Length)));
		if (EventOptionInfos != null)
		{
			num += 2;
			int count2 = EventOptionInfos.Count;
			for (int j = 0; j < count2; j++)
			{
				num += EventOptionInfos[j].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (EventGuid != null)
		{
			int length = EventGuid.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* eventGuid = EventGuid)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)eventGuid[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (MainCharacter != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = MainCharacter.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (TargetCharacter != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = TargetCharacter.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ExtraData != null)
		{
			byte* intPtr3 = ptr;
			ptr += 4;
			int num3 = ExtraData.Serialize(ptr);
			ptr += num3;
			Tester.Assert(num3 <= int.MaxValue);
			*(int*)intPtr3 = num3;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (EventContent != null)
		{
			int length2 = EventContent.Length;
			Tester.Assert(length2 <= 65535);
			*(ushort*)ptr = (ushort)length2;
			ptr += 2;
			fixed (char* eventContent = EventContent)
			{
				for (int j = 0; j < length2; j++)
				{
					((short*)ptr)[j] = (short)eventContent[j];
				}
			}
			ptr += 2 * length2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (NameDecodeDataList != null)
		{
			int count = NameDecodeDataList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int k = 0; k < count; k++)
			{
				ptr += NameDecodeDataList[k].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ExtraFormatLanguageKeys != null)
		{
			int count2 = ExtraFormatLanguageKeys.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int l = 0; l < count2; l++)
			{
				string text = ExtraFormatLanguageKeys[l];
				if (text != null)
				{
					int length3 = text.Length;
					Tester.Assert(length3 <= 65535);
					*(ushort*)ptr = (ushort)length3;
					ptr += 2;
					fixed (char* ptr2 = text)
					{
						for (int m = 0; m < length3; m++)
						{
							((short*)ptr)[m] = (short)ptr2[m];
						}
					}
					ptr += 2 * length3;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (EventTexture != null)
		{
			int length4 = EventTexture.Length;
			Tester.Assert(length4 <= 65535);
			*(ushort*)ptr = (ushort)length4;
			ptr += 2;
			fixed (char* eventTexture = EventTexture)
			{
				for (int n = 0; n < length4; n++)
				{
					((short*)ptr)[n] = (short)eventTexture[n];
				}
			}
			ptr += 2 * length4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)MaskControlCode;
		ptr++;
		*(ushort*)ptr = MaskTweenTime;
		ptr += 2;
		*ptr = (byte)EscOptionIndex;
		ptr++;
		if (EventOptionInfos != null)
		{
			int count3 = EventOptionInfos.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int num4 = 0; num4 < count3; num4++)
			{
				int num5 = EventOptionInfos[num4].Serialize(ptr);
				ptr += num5;
				Tester.Assert(num5 <= 65535);
			}
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			EventGuid = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			EventGuid = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (MainCharacter == null)
			{
				MainCharacter = new CharacterDisplayData();
			}
			ptr += MainCharacter.Deserialize(ptr);
		}
		else
		{
			MainCharacter = null;
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (TargetCharacter == null)
			{
				TargetCharacter = new CharacterDisplayData();
			}
			ptr += TargetCharacter.Deserialize(ptr);
		}
		else
		{
			TargetCharacter = null;
		}
		int num5 = *(int*)ptr;
		ptr += 4;
		if (num5 > 0)
		{
			if (ExtraData == null)
			{
				ExtraData = new TaiwuEventDisplayExtraData();
			}
			ptr += ExtraData.Deserialize(ptr);
		}
		else
		{
			ExtraData = null;
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			int num7 = 2 * num6;
			EventContent = Encoding.Unicode.GetString(ptr, num7);
			ptr += num7;
		}
		else
		{
			EventContent = null;
		}
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		if (num8 > 0)
		{
			if (NameDecodeDataList == null)
			{
				NameDecodeDataList = new List<TaiwuEventCharacterNameDecodeData>(num8);
			}
			else
			{
				NameDecodeDataList.Clear();
			}
			for (int i = 0; i < num8; i++)
			{
				TaiwuEventCharacterNameDecodeData item = default(TaiwuEventCharacterNameDecodeData);
				ptr += item.Deserialize(ptr);
				NameDecodeDataList.Add(item);
			}
		}
		else
		{
			NameDecodeDataList?.Clear();
		}
		ushort num9 = *(ushort*)ptr;
		ptr += 2;
		if (num9 > 0)
		{
			if (ExtraFormatLanguageKeys == null)
			{
				ExtraFormatLanguageKeys = new List<string>(num9);
			}
			else
			{
				ExtraFormatLanguageKeys.Clear();
			}
			for (int j = 0; j < num9; j++)
			{
				ushort num10 = *(ushort*)ptr;
				ptr += 2;
				if (num10 > 0)
				{
					int num11 = 2 * num10;
					ExtraFormatLanguageKeys.Add(Encoding.Unicode.GetString(ptr, num11));
					ptr += num11;
				}
				else
				{
					ExtraFormatLanguageKeys.Add(null);
				}
			}
		}
		else
		{
			ExtraFormatLanguageKeys?.Clear();
		}
		ushort num12 = *(ushort*)ptr;
		ptr += 2;
		if (num12 > 0)
		{
			int num13 = 2 * num12;
			EventTexture = Encoding.Unicode.GetString(ptr, num13);
			ptr += num13;
		}
		else
		{
			EventTexture = null;
		}
		MaskControlCode = (sbyte)(*ptr);
		ptr++;
		MaskTweenTime = *(ushort*)ptr;
		ptr += 2;
		EscOptionIndex = (sbyte)(*ptr);
		ptr++;
		ushort num14 = *(ushort*)ptr;
		ptr += 2;
		if (num14 > 0)
		{
			if (EventOptionInfos == null)
			{
				EventOptionInfos = new List<EventOptionInfo>(num14);
			}
			else
			{
				EventOptionInfos.Clear();
			}
			for (int k = 0; k < num14; k++)
			{
				EventOptionInfo item2 = default(EventOptionInfo);
				ptr += item2.Deserialize(ptr);
				EventOptionInfos.Add(item2);
			}
		}
		else
		{
			EventOptionInfos?.Clear();
		}
		int num15 = (int)(ptr - pData);
		if (num15 > 4)
		{
			return (num15 + 3) / 4 * 4;
		}
		return num15;
	}
}
