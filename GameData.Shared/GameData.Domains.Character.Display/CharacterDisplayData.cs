using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class CharacterDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharacterId;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public byte CreatingType;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public FullName FullName;

	[SerializableGameDataField]
	public byte MonkType;

	[SerializableGameDataField]
	public MonasticTitle MonasticTitle;

	[SerializableGameDataField]
	public AvatarRelatedData AvatarRelatedData;

	[SerializableGameDataField]
	public short PhysiologicalAge;

	[SerializableGameDataField]
	public short CurrAge;

	[SerializableGameDataField]
	public OrganizationInfo OrgInfo;

	[SerializableGameDataField]
	public sbyte BehaviorType;

	[SerializableGameDataField]
	public sbyte FameType;

	[SerializableGameDataField]
	public short FavorabilityToTaiwu;

	[SerializableGameDataField]
	public bool IsApproveTaiwu;

	[SerializableGameDataField]
	public short ApproveTaiwu;

	[SerializableGameDataField]
	public short InfluencePower;

	[SerializableGameDataField]
	public int Contribution;

	[SerializableGameDataField]
	public int ContributionPerMonth;

	[SerializableGameDataField]
	public List<short> TitleIds;

	[SerializableGameDataField]
	public bool CompletelyInfected;

	[SerializableGameDataField]
	public byte ValidKidnapSlotCount;

	[SerializableGameDataField]
	public sbyte AliveState;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public int BirthDate;

	[SerializableGameDataField]
	public byte ExternalRelationState;

	[SerializableGameDataField]
	public sbyte LegendaryBookOwnerState;

	[SerializableGameDataField]
	public int CustomDisplayNameId;

	[SerializableGameDataField]
	public byte SettlementTreasuryGuardInfo;

	[SerializableGameDataField]
	public sbyte BountyPunishmentSeverity;

	[SerializableGameDataField]
	public sbyte BountyOrgTemplate;

	[SerializableGameDataField]
	public bool CanNotSpeak;

	[SerializableGameDataField]
	public bool IsFollowedByTaiwu;

	[SerializableGameDataField]
	public int NickNameId;

	[SerializableGameDataField]
	public int ExtraNameTextTemplateId;

	[SerializableGameDataField]
	public sbyte IdealSect;

	[SerializableGameDataField]
	public sbyte CurrOrgTemplate;

	[SerializableGameDataField]
	public uint DarkAshProtector;

	[SerializableGameDataField]
	public DarkAshCounter DarkAshCounter;

	[SerializableGameDataField]
	public CharacterDisplayData OrganizationMemberPotentialSuccessor;

	[SerializableGameDataField]
	public sbyte Happiness;

	public bool IsSettlementTreasuryGuard => SettlementTreasuryGuardLevel != 0;

	public byte SettlementTreasuryGuardLevel => (byte)(SettlementTreasuryGuardInfo & 3);

	public bool SettlementTreasuryGuardWorking => (SettlementTreasuryGuardInfo & 4) != 0;

	public CharacterDisplayData()
	{
	}

	public CharacterDisplayData(CharacterDisplayData other)
	{
		CharacterId = other.CharacterId;
		TemplateId = other.TemplateId;
		CreatingType = other.CreatingType;
		Gender = other.Gender;
		FullName = other.FullName;
		MonkType = other.MonkType;
		MonasticTitle = other.MonasticTitle;
		AvatarRelatedData = new AvatarRelatedData(other.AvatarRelatedData);
		PhysiologicalAge = other.PhysiologicalAge;
		CurrAge = other.CurrAge;
		OrgInfo = other.OrgInfo;
		BehaviorType = other.BehaviorType;
		FameType = other.FameType;
		FavorabilityToTaiwu = other.FavorabilityToTaiwu;
		IsApproveTaiwu = other.IsApproveTaiwu;
		ApproveTaiwu = other.ApproveTaiwu;
		InfluencePower = other.InfluencePower;
		Contribution = other.Contribution;
		ContributionPerMonth = other.ContributionPerMonth;
		TitleIds = ((other.TitleIds == null) ? null : new List<short>(other.TitleIds));
		CompletelyInfected = other.CompletelyInfected;
		ValidKidnapSlotCount = other.ValidKidnapSlotCount;
		AliveState = other.AliveState;
		Location = other.Location;
		BirthDate = other.BirthDate;
		ExternalRelationState = other.ExternalRelationState;
		LegendaryBookOwnerState = other.LegendaryBookOwnerState;
		CustomDisplayNameId = other.CustomDisplayNameId;
		SettlementTreasuryGuardInfo = other.SettlementTreasuryGuardInfo;
		BountyPunishmentSeverity = other.BountyPunishmentSeverity;
		BountyOrgTemplate = other.BountyOrgTemplate;
		CanNotSpeak = other.CanNotSpeak;
		IsFollowedByTaiwu = other.IsFollowedByTaiwu;
		NickNameId = other.NickNameId;
		ExtraNameTextTemplateId = other.ExtraNameTextTemplateId;
		IdealSect = other.IdealSect;
		CurrOrgTemplate = other.CurrOrgTemplate;
		DarkAshProtector = other.DarkAshProtector;
		DarkAshCounter = other.DarkAshCounter;
		OrganizationMemberPotentialSuccessor = new CharacterDisplayData(other.OrganizationMemberPotentialSuccessor);
		Happiness = other.Happiness;
	}

	public void Assign(CharacterDisplayData other)
	{
		CharacterId = other.CharacterId;
		TemplateId = other.TemplateId;
		CreatingType = other.CreatingType;
		Gender = other.Gender;
		FullName = other.FullName;
		MonkType = other.MonkType;
		MonasticTitle = other.MonasticTitle;
		AvatarRelatedData = new AvatarRelatedData(other.AvatarRelatedData);
		PhysiologicalAge = other.PhysiologicalAge;
		CurrAge = other.CurrAge;
		OrgInfo = other.OrgInfo;
		BehaviorType = other.BehaviorType;
		FameType = other.FameType;
		FavorabilityToTaiwu = other.FavorabilityToTaiwu;
		IsApproveTaiwu = other.IsApproveTaiwu;
		ApproveTaiwu = other.ApproveTaiwu;
		InfluencePower = other.InfluencePower;
		Contribution = other.Contribution;
		ContributionPerMonth = other.ContributionPerMonth;
		TitleIds = ((other.TitleIds == null) ? null : new List<short>(other.TitleIds));
		CompletelyInfected = other.CompletelyInfected;
		ValidKidnapSlotCount = other.ValidKidnapSlotCount;
		AliveState = other.AliveState;
		Location = other.Location;
		BirthDate = other.BirthDate;
		ExternalRelationState = other.ExternalRelationState;
		LegendaryBookOwnerState = other.LegendaryBookOwnerState;
		CustomDisplayNameId = other.CustomDisplayNameId;
		SettlementTreasuryGuardInfo = other.SettlementTreasuryGuardInfo;
		BountyPunishmentSeverity = other.BountyPunishmentSeverity;
		BountyOrgTemplate = other.BountyOrgTemplate;
		CanNotSpeak = other.CanNotSpeak;
		IsFollowedByTaiwu = other.IsFollowedByTaiwu;
		NickNameId = other.NickNameId;
		ExtraNameTextTemplateId = other.ExtraNameTextTemplateId;
		IdealSect = other.IdealSect;
		CurrOrgTemplate = other.CurrOrgTemplate;
		DarkAshProtector = other.DarkAshProtector;
		DarkAshCounter = other.DarkAshCounter;
		OrganizationMemberPotentialSuccessor = new CharacterDisplayData(other.OrganizationMemberPotentialSuccessor);
		Happiness = other.Happiness;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 173;
		num = ((TitleIds == null) ? (num + 2) : (num + (2 + 2 * TitleIds.Count)));
		num += DarkAshCounter.GetSerializedSize();
		num = ((OrganizationMemberPotentialSuccessor == null) ? (num + 2) : (num + (2 + OrganizationMemberPotentialSuccessor.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharacterId;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*ptr = CreatingType;
		ptr++;
		*ptr = (byte)Gender;
		ptr++;
		ptr += FullName.Serialize(ptr);
		*ptr = MonkType;
		ptr++;
		ptr += MonasticTitle.Serialize(ptr);
		ptr += AvatarRelatedData.Serialize(ptr);
		*(short*)ptr = PhysiologicalAge;
		ptr += 2;
		*(short*)ptr = CurrAge;
		ptr += 2;
		ptr += OrgInfo.Serialize(ptr);
		*ptr = (byte)BehaviorType;
		ptr++;
		*ptr = (byte)FameType;
		ptr++;
		*(short*)ptr = FavorabilityToTaiwu;
		ptr += 2;
		*ptr = (IsApproveTaiwu ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = ApproveTaiwu;
		ptr += 2;
		*(short*)ptr = InfluencePower;
		ptr += 2;
		*(int*)ptr = Contribution;
		ptr += 4;
		*(int*)ptr = ContributionPerMonth;
		ptr += 4;
		if (TitleIds != null)
		{
			int count = TitleIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = TitleIds[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (CompletelyInfected ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = ValidKidnapSlotCount;
		ptr++;
		*ptr = (byte)AliveState;
		ptr++;
		ptr += Location.Serialize(ptr);
		*(int*)ptr = BirthDate;
		ptr += 4;
		*ptr = ExternalRelationState;
		ptr++;
		*ptr = (byte)LegendaryBookOwnerState;
		ptr++;
		*(int*)ptr = CustomDisplayNameId;
		ptr += 4;
		*ptr = SettlementTreasuryGuardInfo;
		ptr++;
		*ptr = (byte)BountyPunishmentSeverity;
		ptr++;
		*ptr = (byte)BountyOrgTemplate;
		ptr++;
		*ptr = (CanNotSpeak ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsFollowedByTaiwu ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = NickNameId;
		ptr += 4;
		*(int*)ptr = ExtraNameTextTemplateId;
		ptr += 4;
		*ptr = (byte)IdealSect;
		ptr++;
		*ptr = (byte)CurrOrgTemplate;
		ptr++;
		*(uint*)ptr = DarkAshProtector;
		ptr += 4;
		int num = DarkAshCounter.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		if (OrganizationMemberPotentialSuccessor != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num2 = OrganizationMemberPotentialSuccessor.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)Happiness;
		ptr++;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharacterId = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		CreatingType = *ptr;
		ptr++;
		Gender = (sbyte)(*ptr);
		ptr++;
		ptr += FullName.Deserialize(ptr);
		MonkType = *ptr;
		ptr++;
		ptr += MonasticTitle.Deserialize(ptr);
		if (AvatarRelatedData == null)
		{
			AvatarRelatedData = new AvatarRelatedData();
		}
		ptr += AvatarRelatedData.Deserialize(ptr);
		PhysiologicalAge = *(short*)ptr;
		ptr += 2;
		CurrAge = *(short*)ptr;
		ptr += 2;
		ptr += OrgInfo.Deserialize(ptr);
		BehaviorType = (sbyte)(*ptr);
		ptr++;
		FameType = (sbyte)(*ptr);
		ptr++;
		FavorabilityToTaiwu = *(short*)ptr;
		ptr += 2;
		IsApproveTaiwu = *ptr != 0;
		ptr++;
		ApproveTaiwu = *(short*)ptr;
		ptr += 2;
		InfluencePower = *(short*)ptr;
		ptr += 2;
		Contribution = *(int*)ptr;
		ptr += 4;
		ContributionPerMonth = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (TitleIds == null)
			{
				TitleIds = new List<short>(num);
			}
			else
			{
				TitleIds.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				TitleIds.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			TitleIds?.Clear();
		}
		CompletelyInfected = *ptr != 0;
		ptr++;
		ValidKidnapSlotCount = *ptr;
		ptr++;
		AliveState = (sbyte)(*ptr);
		ptr++;
		ptr += Location.Deserialize(ptr);
		BirthDate = *(int*)ptr;
		ptr += 4;
		ExternalRelationState = *ptr;
		ptr++;
		LegendaryBookOwnerState = (sbyte)(*ptr);
		ptr++;
		CustomDisplayNameId = *(int*)ptr;
		ptr += 4;
		SettlementTreasuryGuardInfo = *ptr;
		ptr++;
		BountyPunishmentSeverity = (sbyte)(*ptr);
		ptr++;
		BountyOrgTemplate = (sbyte)(*ptr);
		ptr++;
		CanNotSpeak = *ptr != 0;
		ptr++;
		IsFollowedByTaiwu = *ptr != 0;
		ptr++;
		NickNameId = *(int*)ptr;
		ptr += 4;
		ExtraNameTextTemplateId = *(int*)ptr;
		ptr += 4;
		IdealSect = (sbyte)(*ptr);
		ptr++;
		CurrOrgTemplate = (sbyte)(*ptr);
		ptr++;
		DarkAshProtector = *(uint*)ptr;
		ptr += 4;
		ptr += DarkAshCounter.Deserialize(ptr);
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (OrganizationMemberPotentialSuccessor == null)
			{
				OrganizationMemberPotentialSuccessor = new CharacterDisplayData();
			}
			ptr += OrganizationMemberPotentialSuccessor.Deserialize(ptr);
		}
		else
		{
			OrganizationMemberPotentialSuccessor = null;
		}
		Happiness = (sbyte)(*ptr);
		ptr++;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
