namespace GameData.Common.SingleValueCollection;

public readonly struct SingleValueCollectionModification<TKey> where TKey : unmanaged
{
	public readonly sbyte Type;

	public readonly TKey Id;

	public SingleValueCollectionModification(sbyte type, TKey id)
	{
		Type = type;
		Id = id;
	}
}
