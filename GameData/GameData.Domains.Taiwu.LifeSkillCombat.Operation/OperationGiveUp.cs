namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

public class OperationGiveUp : OperationBase
{
	public OperationGiveUp()
	{
	}

	public override string Inspect()
	{
		return string.Empty;
	}

	public override int GetSerializedSize()
	{
		int serializedSize = base.GetSerializedSize();
		return (serializedSize <= 4) ? serializedSize : ((serializedSize + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Serialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Deserialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public OperationGiveUp(sbyte playerId, int stamp)
		: base(playerId, stamp)
	{
	}
}
