using System.Runtime.InteropServices;

namespace GameData.GameDataBridge;

[StructLayout(LayoutKind.Explicit)]
public struct Operation
{
	[FieldOffset(0)]
	public byte Type;

	[FieldOffset(4)]
	public ushort DomainId;

	[FieldOffset(6)]
	public ushort DataId;

	[FieldOffset(8)]
	public ulong SubId0;

	[FieldOffset(16)]
	public uint SubId1;

	[FieldOffset(20)]
	public int ValueOffset;

	[FieldOffset(6)]
	public ushort MethodId;

	[FieldOffset(8)]
	public int ArgsCount;

	[FieldOffset(12)]
	public int ArgsOffset;

	[FieldOffset(16)]
	public int ListenerId;

	public static Operation CreateDataMonitor(ushort domainId, ushort dataId, ulong subId0, uint subId1)
	{
		Operation result = default(Operation);
		result.Type = 0;
		result.DomainId = domainId;
		result.MethodId = 0;
		result.DataId = dataId;
		result.ArgsCount = 0;
		result.ArgsOffset = 0;
		result.SubId0 = subId0;
		result.ListenerId = 0;
		result.SubId1 = subId1;
		result.ValueOffset = 0;
		return result;
	}

	public static Operation CreateDataUnMonitor(ushort domainId, ushort dataId, ulong subId0, uint subId1)
	{
		Operation result = default(Operation);
		result.Type = 1;
		result.DomainId = domainId;
		result.MethodId = 0;
		result.DataId = dataId;
		result.ArgsCount = 0;
		result.ArgsOffset = 0;
		result.SubId0 = subId0;
		result.ListenerId = 0;
		result.SubId1 = subId1;
		result.ValueOffset = 0;
		return result;
	}

	public static Operation CreateDataModification(ushort domainId, ushort dataId, ulong subId0, uint subId1, int valueOffset)
	{
		Operation result = default(Operation);
		result.Type = 2;
		result.DomainId = domainId;
		result.MethodId = 0;
		result.DataId = dataId;
		result.ArgsCount = 0;
		result.ArgsOffset = 0;
		result.SubId0 = subId0;
		result.ListenerId = 0;
		result.SubId1 = subId1;
		result.ValueOffset = valueOffset;
		return result;
	}

	public static Operation CreateMethodCall(int listenerId, ushort domainId, ushort methodId, int argsCount, int argsOffset)
	{
		Operation result = default(Operation);
		result.Type = 3;
		result.DomainId = domainId;
		result.DataId = 0;
		result.MethodId = methodId;
		result.SubId0 = 0uL;
		result.ArgsCount = argsCount;
		result.ArgsOffset = argsOffset;
		result.SubId1 = 0u;
		result.ListenerId = listenerId;
		result.ValueOffset = 0;
		return result;
	}
}
