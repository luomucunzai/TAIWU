using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Map;

[SerializableGameData(NotForArchive = true)]
public struct MapHealSimulateResult : ISerializableGameData
{
	[SerializableGameDataField]
	private int _serializeType;

	[SerializableGameDataField]
	public int CostHerb;

	[SerializableGameDataField]
	public int CostMoney;

	[SerializableGameDataField]
	public int CostSpiritualDebt;

	[SerializableGameDataField]
	public int HealEffect;

	[SerializableGameDataField]
	public int MaxRequireAttainment;

	public EHealActionType Type => (EHealActionType)_serializeType;

	public MapHealSimulateResult(EHealActionType type, int costHerb, int costMoney, int healEffect, int costSpiritualDebt, int maxRequireAttainment)
	{
		_serializeType = (int)type;
		CostHerb = costHerb;
		CostMoney = costMoney;
		HealEffect = healEffect;
		CostSpiritualDebt = costSpiritualDebt;
		MaxRequireAttainment = maxRequireAttainment;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 24;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = _serializeType;
		byte* num = pData + 4;
		*(int*)num = CostHerb;
		byte* num2 = num + 4;
		*(int*)num2 = CostMoney;
		byte* num3 = num2 + 4;
		*(int*)num3 = CostSpiritualDebt;
		byte* num4 = num3 + 4;
		*(int*)num4 = HealEffect;
		byte* num5 = num4 + 4;
		*(int*)num5 = MaxRequireAttainment;
		int num6 = (int)(num5 + 4 - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_serializeType = *(int*)ptr;
		ptr += 4;
		CostHerb = *(int*)ptr;
		ptr += 4;
		CostMoney = *(int*)ptr;
		ptr += 4;
		CostSpiritualDebt = *(int*)ptr;
		ptr += 4;
		HealEffect = *(int*)ptr;
		ptr += 4;
		MaxRequireAttainment = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
