using GameData.Serializer;

namespace GameData.Domains.Character.Display;

public class CharacterAttributeDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public MainAttributes CurMainAttributes;

	[SerializableGameDataField]
	public MainAttributes MaxMainAttributes;

	[SerializableGameDataField]
	public HitOrAvoidInts AtkHitAttribute;

	[SerializableGameDataField]
	public OuterAndInnerInts AtkPenetrability;

	[SerializableGameDataField]
	public HitOrAvoidInts DefHitAttribute;

	[SerializableGameDataField]
	public OuterAndInnerInts DefPenetrability;

	[SerializableGameDataField]
	public OuterAndInnerShorts RecoveryOfStanceAndBreath;

	[SerializableGameDataField]
	public short MoveSpeed;

	[SerializableGameDataField]
	public short RecoveryOfFlaw;

	[SerializableGameDataField]
	public short CastSpeed;

	[SerializableGameDataField]
	public short RecoveryOfBlockedAcupoint;

	[SerializableGameDataField]
	public short WeaponSwitchSpeed;

	[SerializableGameDataField]
	public short AttackSpeed;

	[SerializableGameDataField]
	public short InnerRatio;

	[SerializableGameDataField]
	public short RecoveryOfQiDisorder;

	public CharacterAttributeDisplayData()
	{
	}

	public CharacterAttributeDisplayData(CharacterAttributeDisplayData other)
	{
		CurMainAttributes = other.CurMainAttributes;
		MaxMainAttributes = other.MaxMainAttributes;
		AtkHitAttribute = other.AtkHitAttribute;
		AtkPenetrability = other.AtkPenetrability;
		DefHitAttribute = other.DefHitAttribute;
		DefPenetrability = other.DefPenetrability;
		RecoveryOfStanceAndBreath = other.RecoveryOfStanceAndBreath;
		MoveSpeed = other.MoveSpeed;
		RecoveryOfFlaw = other.RecoveryOfFlaw;
		CastSpeed = other.CastSpeed;
		RecoveryOfBlockedAcupoint = other.RecoveryOfBlockedAcupoint;
		WeaponSwitchSpeed = other.WeaponSwitchSpeed;
		AttackSpeed = other.AttackSpeed;
		InnerRatio = other.InnerRatio;
		RecoveryOfQiDisorder = other.RecoveryOfQiDisorder;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 92;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += CurMainAttributes.Serialize(ptr);
		ptr += MaxMainAttributes.Serialize(ptr);
		ptr += AtkHitAttribute.Serialize(ptr);
		ptr += AtkPenetrability.Serialize(ptr);
		ptr += DefHitAttribute.Serialize(ptr);
		ptr += DefPenetrability.Serialize(ptr);
		ptr += RecoveryOfStanceAndBreath.Serialize(ptr);
		*(short*)ptr = MoveSpeed;
		ptr += 2;
		*(short*)ptr = RecoveryOfFlaw;
		ptr += 2;
		*(short*)ptr = CastSpeed;
		ptr += 2;
		*(short*)ptr = RecoveryOfBlockedAcupoint;
		ptr += 2;
		*(short*)ptr = WeaponSwitchSpeed;
		ptr += 2;
		*(short*)ptr = AttackSpeed;
		ptr += 2;
		*(short*)ptr = InnerRatio;
		ptr += 2;
		*(short*)ptr = RecoveryOfQiDisorder;
		ptr += 2;
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
		ptr += CurMainAttributes.Deserialize(ptr);
		ptr += MaxMainAttributes.Deserialize(ptr);
		ptr += AtkHitAttribute.Deserialize(ptr);
		ptr += AtkPenetrability.Deserialize(ptr);
		ptr += DefHitAttribute.Deserialize(ptr);
		ptr += DefPenetrability.Deserialize(ptr);
		ptr += RecoveryOfStanceAndBreath.Deserialize(ptr);
		MoveSpeed = *(short*)ptr;
		ptr += 2;
		RecoveryOfFlaw = *(short*)ptr;
		ptr += 2;
		CastSpeed = *(short*)ptr;
		ptr += 2;
		RecoveryOfBlockedAcupoint = *(short*)ptr;
		ptr += 2;
		WeaponSwitchSpeed = *(short*)ptr;
		ptr += 2;
		AttackSpeed = *(short*)ptr;
		ptr += 2;
		InnerRatio = *(short*)ptr;
		ptr += 2;
		RecoveryOfQiDisorder = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
