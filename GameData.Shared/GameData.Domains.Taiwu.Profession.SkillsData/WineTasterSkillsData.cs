using GameData.Serializer;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class WineTasterSkillsData : IProfessionSkillsData, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort VillagersLastLearnSkillDate = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "VillagersLastLearnSkillDate" };
	}

	[SerializableGameDataField]
	public int VillagersLastLearnSkillDate;

	public void Initialize()
	{
		VillagersLastLearnSkillDate = 0;
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
		Assign(sourceData as WineTasterSkillsData);
	}

	public WineTasterSkillsData()
	{
	}

	public WineTasterSkillsData(WineTasterSkillsData other)
	{
		VillagersLastLearnSkillDate = other.VillagersLastLearnSkillDate;
	}

	public void Assign(WineTasterSkillsData other)
	{
		VillagersLastLearnSkillDate = other.VillagersLastLearnSkillDate;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 1;
		byte* num = pData + 2;
		*(int*)num = VillagersLastLearnSkillDate;
		int num2 = (int)(num + 4 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			VillagersLastLearnSkillDate = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
