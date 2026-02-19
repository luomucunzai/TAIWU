using Config;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

public class OperationUseEffectCard : OperationBase
{
	public struct UseEffectCardInfo
	{
		public sbyte EffectCardTemplateId;

		public int CellIndex;

		public int CellIndex2;

		public sbyte TargetBookStateIndex;

		public sbyte TargetBookOwnerPlayerId;
	}

	public UseEffectCardInfo Info { get; private set; }

	public override string Inspect()
	{
		return $"{LifeSkillCombatEffect.Instance[Info.EffectCardTemplateId].Name} {Info.CellIndex}_{Info.CellIndex2} {Info.TargetBookStateIndex}_{Info.TargetBookOwnerPlayerId}";
	}

	public OperationUseEffectCard()
	{
	}

	public OperationUseEffectCard(sbyte playerId, int stamp, UseEffectCardInfo info)
	{
		base.PlayerId = playerId;
		Stamp = stamp;
		Info = info;
	}

	public override int GetSerializedSize()
	{
		int serializedSize = base.GetSerializedSize();
		serializedSize++;
		serializedSize += 4;
		serializedSize += 4;
		serializedSize++;
		serializedSize++;
		return (serializedSize <= 4) ? serializedSize : ((serializedSize + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Serialize(ptr);
		*ptr = (byte)Info.EffectCardTemplateId;
		ptr++;
		*(int*)ptr = Info.CellIndex;
		ptr += 4;
		*(int*)ptr = Info.CellIndex2;
		ptr += 4;
		*ptr = (byte)Info.TargetBookStateIndex;
		ptr++;
		*ptr = (byte)Info.TargetBookOwnerPlayerId;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Deserialize(ptr);
		UseEffectCardInfo info = new UseEffectCardInfo
		{
			EffectCardTemplateId = (sbyte)(*ptr)
		};
		ptr++;
		info.CellIndex = *(int*)ptr;
		ptr += 4;
		info.CellIndex2 = *(int*)ptr;
		ptr += 4;
		info.TargetBookStateIndex = (sbyte)(*ptr);
		ptr++;
		info.TargetBookOwnerPlayerId = (sbyte)(*ptr);
		ptr++;
		Info = info;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
