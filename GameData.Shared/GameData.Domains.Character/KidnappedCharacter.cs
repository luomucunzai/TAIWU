using System;
using Config;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

public class KidnappedCharacter : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharId;

	[SerializableGameDataField]
	public ItemKey RopeItemKey;

	[SerializableGameDataField]
	public int KidnapBeginDate;

	[SerializableGameDataField]
	public sbyte ExtraResistance;

	public const sbyte MinResistance = 0;

	public const sbyte MaxResistance = sbyte.MaxValue;

	public const sbyte EscapeThreshold = 100;

	public KidnappedCharacter(int charId, sbyte initialExtraResistance, ItemKey ropeItemKey, int kidnapBeginDate)
	{
		CharId = charId;
		RopeItemKey = ropeItemKey;
		KidnapBeginDate = kidnapBeginDate;
		ExtraResistance = initialExtraResistance;
	}

	public void OfflineChangeResistance(int delta)
	{
		ExtraResistance = (sbyte)MathUtils.Clamp(ExtraResistance + delta, 0, 127);
	}

	public int CalcEscapeRate(int totalResistance, int exceedingAmount)
	{
		int ropeReduceEscapeRate = GetRopeReduceEscapeRate();
		int num = (totalResistance - 100) * (100 - ropeReduceEscapeRate) / 100 + exceedingAmount * 20;
		int num2 = Math.Max(num, 0);
		MiscItem miscItem = Misc.Instance[RopeItemKey.TemplateId];
		string text = $"{9 - miscItem.Grade}品{miscItem.Name}";
		AdaptableLog.Info($"逃跑概率：({totalResistance} - {(sbyte)100}) * (100 - {ropeReduceEscapeRate}[{text}]) / 100 + {exceedingAmount} * 20 = {num}%，最终取{num2}%");
		return num2;
	}

	public int GetRopeReduceEscapeRate()
	{
		return Misc.Instance[RopeItemKey.TemplateId].ReduceEscapeRate;
	}

	public KidnappedCharacter()
	{
	}

	public KidnappedCharacter(KidnappedCharacter other)
	{
		CharId = other.CharId;
		RopeItemKey = other.RopeItemKey;
		KidnapBeginDate = other.KidnapBeginDate;
		ExtraResistance = other.ExtraResistance;
	}

	public void Assign(KidnappedCharacter other)
	{
		CharId = other.CharId;
		RopeItemKey = other.RopeItemKey;
		KidnapBeginDate = other.KidnapBeginDate;
		ExtraResistance = other.ExtraResistance;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 17;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharId;
		ptr += 4;
		ptr += RopeItemKey.Serialize(ptr);
		*(int*)ptr = KidnapBeginDate;
		ptr += 4;
		*ptr = (byte)ExtraResistance;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharId = *(int*)ptr;
		ptr += 4;
		ptr += RopeItemKey.Deserialize(ptr);
		KidnapBeginDate = *(int*)ptr;
		ptr += 4;
		ExtraResistance = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
