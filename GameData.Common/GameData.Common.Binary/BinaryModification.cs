namespace GameData.Common.Binary;

public struct BinaryModification
{
	public sbyte Type;

	public int Offset;

	public int Size;

	public BinaryModification(sbyte type, int offset, int size)
	{
		Type = type;
		Offset = offset;
		Size = size;
	}
}
