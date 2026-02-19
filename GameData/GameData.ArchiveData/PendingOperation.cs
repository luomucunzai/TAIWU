namespace GameData.ArchiveData;

public readonly struct PendingOperation
{
	public readonly uint OperationId;

	public readonly int Offset;

	public readonly OperationBlock Block;

	public PendingOperation(uint operationId, int offset, OperationBlock block)
	{
		OperationId = operationId;
		Offset = offset;
		Block = block;
	}
}
