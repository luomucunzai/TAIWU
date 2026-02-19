using System;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.VillagerRole;

[SerializableGameData(NoCopyConstructors = true, NotForDisplayModule = true)]
public class VillagerRoleWrapper : ISerializableGameData
{
	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public VillagerRoleBase VillagerRole;

	public T GetRoleAs<T>() where T : VillagerRoleBase
	{
		return VillagerRole as T;
	}

	public VillagerRoleWrapper(short templateId)
	{
		TemplateId = -1;
		SetRoleTemplateId(templateId);
	}

	public VillagerRoleWrapper()
	{
		TemplateId = -1;
	}

	public void SetRoleTemplateId(short templateId)
	{
		if (TemplateId != templateId)
		{
			TemplateId = templateId;
			VillagerRole = CreateVillagerRoleObject(templateId);
			VillagerRole.ArrangementTemplateId = -1;
		}
	}

	private static VillagerRoleBase CreateVillagerRoleObject(short templateId)
	{
		return templateId switch
		{
			0 => new VillagerRoleFarmer(), 
			1 => new VillagerRoleCraftsman(), 
			2 => new VillagerRoleDoctor(), 
			3 => new VillagerRoleMerchant(), 
			4 => new VillagerRoleLiterati(), 
			5 => new VillagerRoleSwordTombKeeper(), 
			6 => new VillagerRoleHead(), 
			_ => throw new ArgumentOutOfRangeException($"templateId {templateId} is out of range."), 
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2 + VillagerRole.GetSerializedSize();
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = TemplateId;
		ptr += 2;
		ptr += VillagerRole.Serialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TemplateId = *(short*)ptr;
		ptr += 2;
		VillagerRole = CreateVillagerRoleObject(TemplateId);
		ptr += VillagerRole.Deserialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
