using System;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class PrioritizedActionWrapper : ISerializableGameData
{
	[Obsolete]
	[SerializableGameDataField]
	public sbyte ObsoleteActionType;

	[SerializableGameDataField]
	public short ActionType;

	[SerializableGameDataField]
	public BasePrioritizedAction Action;

	public PrioritizedActionWrapper()
	{
		ActionType = -1;
		Action = null;
	}

	public PrioritizedActionWrapper(sbyte actionType, NpcTravelTarget target)
	{
		ActionType = actionType;
		Action = PrioritizedActionTypeHelper.CreatePrioritizedAction(actionType);
		Action.Target = target;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		if (ActionType >= 0)
		{
			num += Action.GetSerializedSize();
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = 15;
		ptr++;
		*(short*)ptr = ActionType;
		ptr += 2;
		if (ActionType >= 0)
		{
			ptr += Action.Serialize(ptr);
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ObsoleteActionType = (sbyte)(*ptr);
		ptr++;
		if (ObsoleteActionType > 14)
		{
			ActionType = *(short*)ptr;
			ptr += 2;
		}
		else
		{
			ActionType = ObsoleteActionType;
		}
		if (ActionType >= 0)
		{
			Action = PrioritizedActionTypeHelper.CreatePrioritizedAction(ActionType);
			ptr += Action.Deserialize(ptr);
		}
		else
		{
			Action = null;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
