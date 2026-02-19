using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

public struct PrioritizedActionCooldownSbytes : ISerializableGameData
{
	public unsafe fixed sbyte Items[9];

	public unsafe void Initialize()
	{
		fixed (sbyte* items = Items)
		{
			*(long*)items = 0L;
			items[8] = 0;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 9;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (sbyte* items = Items)
		{
			*(long*)pData = *(long*)items;
			pData[8] = (byte)items[8];
		}
		return 9;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (sbyte* items = Items)
		{
			*(long*)items = *(long*)pData;
			items[8] = (sbyte)pData[8];
		}
		return 9;
	}

	public unsafe void AddAllActionCooldown(sbyte cooldown)
	{
		for (sbyte b = 0; b < 9; b++)
		{
			int num = Items[b] + cooldown;
			if (num > 127)
			{
				num = 127;
			}
			Items[b] = (sbyte)num;
		}
	}

	public unsafe void SetAllActionCooldown(sbyte cooldown)
	{
		for (sbyte b = 0; b < 9; b++)
		{
			Items[b] = cooldown;
		}
	}

	public unsafe void ClearCooldown(sbyte prioritizedActionType)
	{
		Items[prioritizedActionType] = 0;
	}

	public unsafe bool UpdateAllCooldown()
	{
		bool result = false;
		for (sbyte b = 0; b < 9; b++)
		{
			if (Items[b] > 0)
			{
				ref sbyte reference = ref Items[b];
				reference--;
				result = true;
			}
		}
		return result;
	}

	public unsafe bool IsOffCooldown(sbyte prioritizedActionType)
	{
		return Items[prioritizedActionType] <= 0;
	}
}
