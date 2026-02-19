using System;
using System.Collections.Generic;
using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Mod;

public class ModInfo : ISerializableGameData, IEquatable<ModInfo>
{
	[SerializableGameDataField]
	public string DirectoryName;

	[SerializableGameDataField]
	public string Title;

	[SerializableGameDataField]
	public ModId ModId;

	[SerializableGameDataField]
	public List<string> BackendPlugins;

	[SerializableGameDataField]
	public List<string> BackendPluginsLegacy;

	[SerializableGameDataField]
	public List<string> BackendPatches;

	[SerializableGameDataField]
	public List<string> EventPackages;

	[SerializableGameDataField]
	public SerializableModData ModSettings;

	public ModInfo()
	{
		BackendPlugins = new List<string>();
		BackendPluginsLegacy = new List<string>();
		BackendPatches = new List<string>();
		EventPackages = new List<string>();
		ModSettings = new SerializableModData();
	}

	public void Upload()
	{
	}

	public bool Equals(ModInfo other)
	{
		return ModId.Equals(other?.ModId);
	}

	public override int GetHashCode()
	{
		return ModId.GetHashCode();
	}

	public string GetVersionString()
	{
		ulong subUlong = BitOperation.GetSubUlong(ModId.Version, 0, 16);
		ulong subUlong2 = BitOperation.GetSubUlong(ModId.Version, 16, 16);
		ulong subUlong3 = BitOperation.GetSubUlong(ModId.Version, 32, 16);
		ulong subUlong4 = BitOperation.GetSubUlong(ModId.Version, 48, 16);
		return new Version((ushort)subUlong, (ushort)subUlong2, (ushort)subUlong3, (ushort)subUlong4).ToString();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 20;
		num = ((DirectoryName == null) ? (num + 2) : (num + (2 + 2 * DirectoryName.Length)));
		num = ((Title == null) ? (num + 2) : (num + (2 + 2 * Title.Length)));
		if (BackendPlugins != null)
		{
			num += 2;
			int count = BackendPlugins.Count;
			for (int i = 0; i < count; i++)
			{
				string text = BackendPlugins[i];
				num = ((text == null) ? (num + 2) : (num + (2 + 2 * text.Length)));
			}
		}
		else
		{
			num += 2;
		}
		if (BackendPatches != null)
		{
			num += 2;
			int count2 = BackendPatches.Count;
			for (int j = 0; j < count2; j++)
			{
				string text2 = BackendPatches[j];
				num = ((text2 == null) ? (num + 2) : (num + (2 + 2 * text2.Length)));
			}
		}
		else
		{
			num += 2;
		}
		if (EventPackages != null)
		{
			num += 2;
			int count3 = EventPackages.Count;
			for (int k = 0; k < count3; k++)
			{
				string text3 = EventPackages[k];
				num = ((text3 == null) ? (num + 2) : (num + (2 + 2 * text3.Length)));
			}
		}
		else
		{
			num += 2;
		}
		num = ((ModSettings == null) ? (num + 2) : (num + (2 + ModSettings.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (DirectoryName != null)
		{
			int length = DirectoryName.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* directoryName = DirectoryName)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)directoryName[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (Title != null)
		{
			int length2 = Title.Length;
			Tester.Assert(length2 <= 65535);
			*(ushort*)ptr = (ushort)length2;
			ptr += 2;
			fixed (char* title = Title)
			{
				for (int j = 0; j < length2; j++)
				{
					((short*)ptr)[j] = (short)title[j];
				}
			}
			ptr += 2 * length2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += ModId.Serialize(ptr);
		if (BackendPlugins != null)
		{
			int count = BackendPlugins.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int k = 0; k < count; k++)
			{
				string text = BackendPlugins[k];
				if (text != null)
				{
					int length3 = text.Length;
					Tester.Assert(length3 <= 65535);
					*(ushort*)ptr = (ushort)length3;
					ptr += 2;
					fixed (char* ptr2 = text)
					{
						for (int l = 0; l < length3; l++)
						{
							((short*)ptr)[l] = (short)ptr2[l];
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
		if (BackendPatches != null)
		{
			int count2 = BackendPatches.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int m = 0; m < count2; m++)
			{
				string text2 = BackendPatches[m];
				if (text2 != null)
				{
					int length4 = text2.Length;
					Tester.Assert(length4 <= 65535);
					*(ushort*)ptr = (ushort)length4;
					ptr += 2;
					fixed (char* ptr3 = text2)
					{
						for (int n = 0; n < length4; n++)
						{
							((short*)ptr)[n] = (short)ptr3[n];
						}
					}
					ptr += 2 * length4;
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
		if (EventPackages != null)
		{
			int count3 = EventPackages.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int num = 0; num < count3; num++)
			{
				string text3 = EventPackages[num];
				if (text3 != null)
				{
					int length5 = text3.Length;
					Tester.Assert(length5 <= 65535);
					*(ushort*)ptr = (ushort)length5;
					ptr += 2;
					fixed (char* ptr4 = text3)
					{
						for (int num2 = 0; num2 < length5; num2++)
						{
							((short*)ptr)[num2] = (short)ptr4[num2];
						}
					}
					ptr += 2 * length5;
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
		if (ModSettings != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num3 = ModSettings.Serialize(ptr);
			ptr += num3;
			Tester.Assert(num3 <= 65535);
			*(ushort*)intPtr = (ushort)num3;
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
			int num2 = 2 * num;
			DirectoryName = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			DirectoryName = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			int num4 = 2 * num3;
			Title = Encoding.Unicode.GetString(ptr, num4);
			ptr += num4;
		}
		else
		{
			Title = null;
		}
		ptr += ModId.Deserialize(ptr);
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (BackendPlugins == null)
			{
				BackendPlugins = new List<string>(num5);
			}
			else
			{
				BackendPlugins.Clear();
			}
			for (int i = 0; i < num5; i++)
			{
				ushort num6 = *(ushort*)ptr;
				ptr += 2;
				if (num6 > 0)
				{
					int num7 = 2 * num6;
					BackendPlugins.Add(Encoding.Unicode.GetString(ptr, num7));
					ptr += num7;
				}
				else
				{
					BackendPlugins.Add(null);
				}
			}
		}
		else
		{
			BackendPlugins?.Clear();
		}
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		if (num8 > 0)
		{
			if (BackendPatches == null)
			{
				BackendPatches = new List<string>(num8);
			}
			else
			{
				BackendPatches.Clear();
			}
			for (int j = 0; j < num8; j++)
			{
				ushort num9 = *(ushort*)ptr;
				ptr += 2;
				if (num9 > 0)
				{
					int num10 = 2 * num9;
					BackendPatches.Add(Encoding.Unicode.GetString(ptr, num10));
					ptr += num10;
				}
				else
				{
					BackendPatches.Add(null);
				}
			}
		}
		else
		{
			BackendPatches?.Clear();
		}
		ushort num11 = *(ushort*)ptr;
		ptr += 2;
		if (num11 > 0)
		{
			if (EventPackages == null)
			{
				EventPackages = new List<string>(num11);
			}
			else
			{
				EventPackages.Clear();
			}
			for (int k = 0; k < num11; k++)
			{
				ushort num12 = *(ushort*)ptr;
				ptr += 2;
				if (num12 > 0)
				{
					int num13 = 2 * num12;
					EventPackages.Add(Encoding.Unicode.GetString(ptr, num13));
					ptr += num13;
				}
				else
				{
					EventPackages.Add(null);
				}
			}
		}
		else
		{
			EventPackages?.Clear();
		}
		ushort num14 = *(ushort*)ptr;
		ptr += 2;
		if (num14 > 0)
		{
			if (ModSettings == null)
			{
				ModSettings = new SerializableModData();
			}
			ptr += ModSettings.Deserialize(ptr);
		}
		else
		{
			ModSettings = null;
		}
		int num15 = (int)(ptr - pData);
		if (num15 > 4)
		{
			return (num15 + 3) / 4 * 4;
		}
		return num15;
	}
}
