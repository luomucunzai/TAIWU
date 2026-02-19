using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Combat;

[AutoGenerateSerializableGameData(NotForArchive = true)]
public class CompleteDamageStepDisplayData : ISerializableGameData
{
	[SerializableGameDataField(ArrayElementsCount = 7)]
	public OuterAndInnerDamageStepDisplayData[] BodyPart = new OuterAndInnerDamageStepDisplayData[7];

	[SerializableGameDataField]
	public DamageStepDisplayData Mind;

	[SerializableGameDataField]
	public DamageStepDisplayData Fatal;

	[SerializableGameDataField]
	public short CharacterTemplateId;

	[SerializableGameDataField]
	public sbyte CharacterConsummateLevel;

	public CompleteDamageStepDisplayData()
	{
	}

	public CompleteDamageStepDisplayData(CompleteDamageStepDisplayData other)
	{
		OuterAndInnerDamageStepDisplayData[] bodyPart = other.BodyPart;
		int num = bodyPart.Length;
		BodyPart = new OuterAndInnerDamageStepDisplayData[num];
		for (int i = 0; i < num; i++)
		{
			BodyPart[i] = bodyPart[i];
		}
		Mind = other.Mind;
		Fatal = other.Fatal;
		CharacterTemplateId = other.CharacterTemplateId;
		CharacterConsummateLevel = other.CharacterConsummateLevel;
	}

	public void Assign(CompleteDamageStepDisplayData other)
	{
		OuterAndInnerDamageStepDisplayData[] bodyPart = other.BodyPart;
		int num = bodyPart.Length;
		BodyPart = new OuterAndInnerDamageStepDisplayData[num];
		for (int i = 0; i < num; i++)
		{
			BodyPart[i] = bodyPart[i];
		}
		Mind = other.Mind;
		Fatal = other.Fatal;
		CharacterTemplateId = other.CharacterTemplateId;
		CharacterConsummateLevel = other.CharacterConsummateLevel;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 387;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		Tester.Assert(BodyPart.Length == 7);
		for (int i = 0; i < 7; i++)
		{
			ptr += BodyPart[i].Serialize(ptr);
		}
		ptr += Mind.Serialize(ptr);
		ptr += Fatal.Serialize(ptr);
		*(short*)ptr = CharacterTemplateId;
		ptr += 2;
		*ptr = (byte)CharacterConsummateLevel;
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
		if (BodyPart == null || BodyPart.Length != 7)
		{
			BodyPart = new OuterAndInnerDamageStepDisplayData[7];
		}
		for (int i = 0; i < 7; i++)
		{
			BodyPart[i] = default(OuterAndInnerDamageStepDisplayData);
			ptr += BodyPart[i].Deserialize(ptr);
		}
		ptr += Mind.Deserialize(ptr);
		ptr += Fatal.Deserialize(ptr);
		CharacterTemplateId = *(short*)ptr;
		ptr += 2;
		CharacterConsummateLevel = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
