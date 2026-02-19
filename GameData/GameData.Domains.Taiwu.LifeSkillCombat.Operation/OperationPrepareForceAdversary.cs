namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

public class OperationPrepareForceAdversary : OperationBase
{
	public enum ForceAdversaryOperation : sbyte
	{
		Silent,
		GiveUp
	}

	public ForceAdversaryOperation Type { get; private set; }

	public OperationPrepareForceAdversary()
	{
	}

	public override string Inspect()
	{
		return string.Empty;
	}

	public override int GetSerializedSize()
	{
		int serializedSize = base.GetSerializedSize();
		serializedSize++;
		return (serializedSize <= 4) ? serializedSize : ((serializedSize + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Serialize(ptr);
		*ptr = (byte)Type;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Deserialize(ptr);
		Type = (ForceAdversaryOperation)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public OperationPrepareForceAdversary(sbyte playerId, int stamp, ForceAdversaryOperation type)
		: base(playerId, stamp)
	{
		Type = type;
	}
}
