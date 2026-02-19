using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Debate;

public class ActivatedStrategy : ISerializableGameData
{
	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public int PawnId;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public bool IsCastedByTaiwu;

	[SerializableGameDataField]
	public bool IsRevealed;

	public ActivatedStrategy(int id, int pawnId, short templateId, bool isCastedByTaiwu)
	{
		Id = id;
		PawnId = pawnId;
		TemplateId = templateId;
		IsCastedByTaiwu = isCastedByTaiwu;
		IsRevealed = false;
	}

	public DebateStrategyItem GetConfig()
	{
		return DebateStrategy.Instance[TemplateId];
	}

	public EDebateStrategyTriggerType GetTriggerType()
	{
		return GetConfig().TriggerType;
	}

	public bool GetIsInertia()
	{
		DebateStrategyItem config = GetConfig();
		if (config.EffectList == null || config.EffectList.Count == 0)
		{
			return false;
		}
		foreach (IntPair effect in config.EffectList)
		{
			if (effect.First == 35)
			{
				return true;
			}
		}
		return false;
	}

	public ActivatedStrategy()
	{
	}

	public ActivatedStrategy(ActivatedStrategy other)
	{
		Id = other.Id;
		PawnId = other.PawnId;
		TemplateId = other.TemplateId;
		IsCastedByTaiwu = other.IsCastedByTaiwu;
		IsRevealed = other.IsRevealed;
	}

	public void Assign(ActivatedStrategy other)
	{
		Id = other.Id;
		PawnId = other.PawnId;
		TemplateId = other.TemplateId;
		IsCastedByTaiwu = other.IsCastedByTaiwu;
		IsRevealed = other.IsRevealed;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = Id;
		byte* num = pData + 4;
		*(int*)num = PawnId;
		byte* num2 = num + 4;
		*(short*)num2 = TemplateId;
		byte* num3 = num2 + 2;
		*num3 = (IsCastedByTaiwu ? ((byte)1) : ((byte)0));
		byte* num4 = num3 + 1;
		*num4 = (IsRevealed ? ((byte)1) : ((byte)0));
		int num5 = (int)(num4 + 1 - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(int*)ptr;
		ptr += 4;
		PawnId = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		IsCastedByTaiwu = *ptr != 0;
		ptr++;
		IsRevealed = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
