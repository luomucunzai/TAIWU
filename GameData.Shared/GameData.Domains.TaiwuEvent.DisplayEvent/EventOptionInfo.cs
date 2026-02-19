using System;
using System.Collections.Generic;
using System.Text;
using GameData.Domains.TaiwuEvent.EventOption;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[Serializable]
[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public struct EventOptionInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte OptionType;

	[SerializableGameDataField]
	public string OptionKey;

	[SerializableGameDataField]
	public string OptionContent;

	[SerializableGameDataField]
	public List<OptionAvailableInfo> OptionAvailableConditions;

	[SerializableGameDataField]
	public List<OptionAvailableConditionInfo> OptionAvailableConditionInfos;

	[SerializableGameDataField]
	public List<OptionConsumeInfo> OptionConsumeInfos;

	[SerializableGameDataField]
	public sbyte OptionState;

	[SerializableGameDataField]
	public sbyte Behavior;

	[SerializableGameDataField]
	public List<string> ExtraFormatLanguageKeys;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		num = ((OptionKey == null) ? (num + 2) : (num + (2 + 2 * OptionKey.Length)));
		num = ((OptionContent == null) ? (num + 2) : (num + (2 + 2 * OptionContent.Length)));
		if (OptionAvailableConditions != null)
		{
			num += 2;
			int count = OptionAvailableConditions.Count;
			for (int i = 0; i < count; i++)
			{
				num += OptionAvailableConditions[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		if (OptionAvailableConditionInfos != null)
		{
			num += 2;
			int count2 = OptionAvailableConditionInfos.Count;
			for (int j = 0; j < count2; j++)
			{
				OptionAvailableConditionInfo optionAvailableConditionInfo = OptionAvailableConditionInfos[j];
				num = ((optionAvailableConditionInfo == null) ? (num + 2) : (num + (2 + optionAvailableConditionInfo.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num = ((OptionConsumeInfos == null) ? (num + 2) : (num + (2 + 12 * OptionConsumeInfos.Count)));
		if (ExtraFormatLanguageKeys != null)
		{
			num += 2;
			int count3 = ExtraFormatLanguageKeys.Count;
			for (int k = 0; k < count3; k++)
			{
				string text = ExtraFormatLanguageKeys[k];
				num = ((text == null) ? (num + 2) : (num + (2 + 2 * text.Length)));
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
		*ptr = (byte)OptionType;
		ptr++;
		if (OptionKey != null)
		{
			int length = OptionKey.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* optionKey = OptionKey)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)optionKey[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (OptionContent != null)
		{
			int length2 = OptionContent.Length;
			Tester.Assert(length2 <= 65535);
			*(ushort*)ptr = (ushort)length2;
			ptr += 2;
			fixed (char* optionContent = OptionContent)
			{
				for (int j = 0; j < length2; j++)
				{
					((short*)ptr)[j] = (short)optionContent[j];
				}
			}
			ptr += 2 * length2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (OptionAvailableConditions != null)
		{
			int count = OptionAvailableConditions.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int k = 0; k < count; k++)
			{
				int num = OptionAvailableConditions[k].Serialize(ptr);
				ptr += num;
				Tester.Assert(num <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (OptionAvailableConditionInfos != null)
		{
			int count2 = OptionAvailableConditionInfos.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int l = 0; l < count2; l++)
			{
				OptionAvailableConditionInfo optionAvailableConditionInfo = OptionAvailableConditionInfos[l];
				if (optionAvailableConditionInfo != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num2 = optionAvailableConditionInfo.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr = (ushort)num2;
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
		if (OptionConsumeInfos != null)
		{
			int count3 = OptionConsumeInfos.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int m = 0; m < count3; m++)
			{
				ptr += OptionConsumeInfos[m].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)OptionState;
		ptr++;
		*ptr = (byte)Behavior;
		ptr++;
		if (ExtraFormatLanguageKeys != null)
		{
			int count4 = ExtraFormatLanguageKeys.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int n = 0; n < count4; n++)
			{
				string text = ExtraFormatLanguageKeys[n];
				if (text != null)
				{
					int length3 = text.Length;
					Tester.Assert(length3 <= 65535);
					*(ushort*)ptr = (ushort)length3;
					ptr += 2;
					fixed (char* ptr2 = text)
					{
						for (int num3 = 0; num3 < length3; num3++)
						{
							((short*)ptr)[num3] = (short)ptr2[num3];
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
		OptionType = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			OptionKey = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			OptionKey = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			int num4 = 2 * num3;
			OptionContent = Encoding.Unicode.GetString(ptr, num4);
			ptr += num4;
		}
		else
		{
			OptionContent = null;
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (OptionAvailableConditions == null)
			{
				OptionAvailableConditions = new List<OptionAvailableInfo>(num5);
			}
			else
			{
				OptionAvailableConditions.Clear();
			}
			for (int i = 0; i < num5; i++)
			{
				OptionAvailableInfo item = default(OptionAvailableInfo);
				ptr += item.Deserialize(ptr);
				OptionAvailableConditions.Add(item);
			}
		}
		else
		{
			OptionAvailableConditions?.Clear();
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (OptionAvailableConditionInfos == null)
			{
				OptionAvailableConditionInfos = new List<OptionAvailableConditionInfo>(num6);
			}
			else
			{
				OptionAvailableConditionInfos.Clear();
			}
			for (int j = 0; j < num6; j++)
			{
				ushort num7 = *(ushort*)ptr;
				ptr += 2;
				if (num7 > 0)
				{
					OptionAvailableConditionInfo optionAvailableConditionInfo = new OptionAvailableConditionInfo();
					ptr += optionAvailableConditionInfo.Deserialize(ptr);
					OptionAvailableConditionInfos.Add(optionAvailableConditionInfo);
				}
				else
				{
					OptionAvailableConditionInfos.Add(null);
				}
			}
		}
		else
		{
			OptionAvailableConditionInfos?.Clear();
		}
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		if (num8 > 0)
		{
			if (OptionConsumeInfos == null)
			{
				OptionConsumeInfos = new List<OptionConsumeInfo>(num8);
			}
			else
			{
				OptionConsumeInfos.Clear();
			}
			for (int k = 0; k < num8; k++)
			{
				OptionConsumeInfo item2 = default(OptionConsumeInfo);
				ptr += item2.Deserialize(ptr);
				OptionConsumeInfos.Add(item2);
			}
		}
		else
		{
			OptionConsumeInfos?.Clear();
		}
		OptionState = (sbyte)(*ptr);
		ptr++;
		Behavior = (sbyte)(*ptr);
		ptr++;
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
			for (int l = 0; l < num9; l++)
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
		int num12 = (int)(ptr - pData);
		if (num12 > 4)
		{
			return (num12 + 3) / 4 * 4;
		}
		return num12;
	}
}
