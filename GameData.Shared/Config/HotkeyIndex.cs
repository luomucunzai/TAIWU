using System;

namespace Config;

[Serializable]
public struct HotkeyIndex
{
	public byte GroupId;

	public byte CommandId;

	public HotkeyIndex(byte groupId, byte commandId)
	{
		GroupId = groupId;
		CommandId = commandId;
	}
}
