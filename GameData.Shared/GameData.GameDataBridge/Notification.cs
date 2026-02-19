using System.Runtime.InteropServices;
using GameData.Common;

namespace GameData.GameDataBridge;

[StructLayout(LayoutKind.Explicit)]
public struct Notification
{
	[FieldOffset(0)]
	public byte Type;

	[FieldOffset(4)]
	public DataUid Uid;

	[FieldOffset(4)]
	public int ListenerId;

	[FieldOffset(8)]
	public ushort DomainId;

	[FieldOffset(10)]
	public ushort MethodId;

	[FieldOffset(4)]
	public ushort DisplayEventType;

	[FieldOffset(20)]
	public int ValueOffset;

	public static Notification CreateDataModification(DataUid uid, int valueOffset)
	{
		Notification result = default(Notification);
		result.Type = 0;
		result.ListenerId = 0;
		result.DomainId = 0;
		result.MethodId = 0;
		result.DisplayEventType = 0;
		result.Uid = uid;
		result.ValueOffset = valueOffset;
		return result;
	}

	public static Notification CreateMethodReturn(int listenerId, ushort domainId, ushort methodId, int returnValueOffset)
	{
		Notification result = default(Notification);
		result.Type = 1;
		result.Uid = default(DataUid);
		result.DisplayEventType = 0;
		result.ListenerId = listenerId;
		result.DomainId = domainId;
		result.MethodId = methodId;
		result.ValueOffset = returnValueOffset;
		return result;
	}

	public static Notification CreateDisplayEvent(DisplayEventType displayEventType, int valueOffset)
	{
		Notification result = default(Notification);
		result.Type = 2;
		result.Uid = default(DataUid);
		result.ListenerId = 0;
		result.DomainId = 0;
		result.MethodId = 0;
		result.DisplayEventType = (ushort)displayEventType;
		result.ValueOffset = valueOffset;
		return result;
	}
}
