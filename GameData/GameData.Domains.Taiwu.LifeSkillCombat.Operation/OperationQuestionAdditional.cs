using System.Collections.Generic;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

public class OperationQuestionAdditional : OperationQuestion
{
	public sbyte RequiredCardTemplateId { get; private set; }

	public OperationQuestionAdditional()
	{
	}

	public override string Inspect()
	{
		return $"{base.Inspect()} with cardId[{RequiredCardTemplateId}]";
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
		*ptr = (byte)RequiredCardTemplateId;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Deserialize(ptr);
		RequiredCardTemplateId = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public override IEnumerable<sbyte> PickEffectiveEffectCards(IEnumerable<sbyte> wantUseEffectCardIds)
	{
		foreach (sbyte item in base.PickEffectiveEffectCards(wantUseEffectCardIds))
		{
			yield return item;
		}
		yield return RequiredCardTemplateId;
	}

	public OperationQuestionAdditional(sbyte playerId, int stamp, int gridIndex, int basePoint, IEnumerable<sbyte> wantUseEffectCards, sbyte requiredCardTemplateId)
		: base(playerId, stamp, gridIndex, basePoint, wantUseEffectCards)
	{
		RequiredCardTemplateId = requiredCardTemplateId;
	}
}
