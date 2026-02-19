using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.CombatSkill;

[SerializableGameData(NotForArchive = true)]
public struct CombatSkillEffectDescriptionDisplayData : ISerializableGameData
{
	public static readonly CombatSkillEffectDescriptionDisplayData Invalid = new CombatSkillEffectDescriptionDisplayData
	{
		EffectId = -1,
		AffectRequirePower = null
	};

	[SerializableGameDataField]
	public int EffectId;

	[SerializableGameDataField]
	public List<int> AffectRequirePower;

	public CombatSkillEffectDescriptionDisplayData(CombatSkillEffectDescriptionDisplayData other)
	{
		EffectId = other.EffectId;
		AffectRequirePower = ((other.AffectRequirePower != null) ? new List<int>(other.AffectRequirePower) : null);
	}

	public void Assign(CombatSkillEffectDescriptionDisplayData other)
	{
		EffectId = other.EffectId;
		AffectRequirePower = ((other.AffectRequirePower != null) ? new List<int>(other.AffectRequirePower) : null);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((AffectRequirePower == null) ? (num + 2) : (num + (2 + 4 * AffectRequirePower.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = EffectId;
		ptr += 4;
		if (AffectRequirePower != null)
		{
			int count = AffectRequirePower.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = AffectRequirePower[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
		EffectId = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (AffectRequirePower == null)
			{
				AffectRequirePower = new List<int>(num);
			}
			else
			{
				AffectRequirePower.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				AffectRequirePower.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			AffectRequirePower?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
