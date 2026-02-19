using GameData.Serializer;

namespace GameData.Domains.Character;

[SerializableGameData(NotForArchive = true)]
public class SimulateEatingEffectResult : ISerializableGameData
{
	[SerializableGameDataField]
	public int MaxDisorderOfQi;

	[SerializableGameDataField]
	public int MinDisorderOfQi;

	[SerializableGameDataField]
	public PoisonInts Poisons;

	[SerializableGameDataField]
	public int Health;

	[SerializableGameDataField]
	public Injuries Injuries;

	public SimulateEatingEffectResult()
	{
	}

	public SimulateEatingEffectResult(SimulateEatingEffectResult other)
	{
		MaxDisorderOfQi = other.MaxDisorderOfQi;
		MinDisorderOfQi = other.MinDisorderOfQi;
		Poisons = other.Poisons;
		Health = other.Health;
		Injuries = other.Injuries;
	}

	public void Assign(SimulateEatingEffectResult other)
	{
		MaxDisorderOfQi = other.MaxDisorderOfQi;
		MinDisorderOfQi = other.MinDisorderOfQi;
		Poisons = other.Poisons;
		Health = other.Health;
		Injuries = other.Injuries;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 52;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = MaxDisorderOfQi;
		ptr += 4;
		*(int*)ptr = MinDisorderOfQi;
		ptr += 4;
		ptr += Poisons.Serialize(ptr);
		*(int*)ptr = Health;
		ptr += 4;
		ptr += Injuries.Serialize(ptr);
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
		MaxDisorderOfQi = *(int*)ptr;
		ptr += 4;
		MinDisorderOfQi = *(int*)ptr;
		ptr += 4;
		ptr += Poisons.Deserialize(ptr);
		Health = *(int*)ptr;
		ptr += 4;
		ptr += Injuries.Deserialize(ptr);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
