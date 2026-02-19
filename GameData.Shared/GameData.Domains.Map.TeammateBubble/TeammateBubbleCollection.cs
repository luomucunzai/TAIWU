using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Utilities;

namespace GameData.Domains.Map.TeammateBubble;

public class TeammateBubbleCollection : WriteableRecordCollection
{
	public void GetRenderInfos(List<TeammateBubbleRenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			TeammateBubbleRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
			if (renderInfo != null)
			{
				renderInfos.Add(renderInfo);
			}
		}
	}

	public new unsafe TeammateBubbleRenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			int index = *(int*)(ptr + 1);
			int subType = ((int*)(ptr + 1))[1];
			short num = ((short*)(ptr + 1 + 4))[2];
			ptr += 11;
			TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[num];
			if (teammateBubbleItem == null)
			{
				AdaptableLog.Warning($"Unable to render teammate bubble with template id {num}");
				return null;
			}
			TeammateBubbleRenderInfo teammateBubbleRenderInfo = new TeammateBubbleRenderInfo(num, GetStringByType(teammateBubbleItem, subType), index);
			string[] parameters = teammateBubbleItem.Parameters;
			int i = 0;
			for (int num2 = parameters.Length; i < num2; i++)
			{
				string text = parameters[i];
				if (string.IsNullOrEmpty(text))
				{
					break;
				}
				sbyte b = ParameterType.Parse(text);
				int item = ReadonlyRecordCollection.ReadArgumentAndGetIndex(b, &ptr, argumentCollection);
				teammateBubbleRenderInfo.Arguments.Add((b, item));
			}
			return teammateBubbleRenderInfo;
		}
	}

	private string GetStringByType(TeammateBubbleItem config, int subType)
	{
		return subType switch
		{
			0 => config.SpecialDesc0, 
			1 => config.SpecialDesc1, 
			2 => config.SpecialDesc2, 
			3 => config.SpecialDesc3, 
			4 => config.SpecialDesc4, 
			5 => config.FamilyDesc, 
			6 => config.FriendDesc, 
			7 => config.BehaviorDesc[0], 
			8 => config.BehaviorDesc[1], 
			9 => config.BehaviorDesc[2], 
			10 => config.BehaviorDesc[3], 
			11 => config.BehaviorDesc[4], 
			_ => string.Empty, 
		};
	}

	private unsafe int BeginAddingRecord(int index, short recordType, int subtype)
	{
		int size = Size;
		int num = Size + 1 + 4 + 4 + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*(int*)(num2 + 1) = index;
			((int*)(num2 + 1))[1] = subtype;
			((short*)(num2 + 1 + 4))[2] = recordType;
		}
		return size;
	}

	public int AddNoneParameterBubble(TeammateBubbleItem config, int index, int subtype)
	{
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[0]));
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[1]));
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		EndAddingRecord(num);
		return num;
	}

	public int AddSingleCharacterParameterBubble(TeammateBubbleItem config, int index, int subtype, int charId)
	{
		Tester.Assert(config.Parameters[0] == "Character");
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[1]));
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCharacterIdentityBubble(TeammateBubbleItem config, int index, int subtype, int charId, OrganizationInfo orgInfo, sbyte gender)
	{
		Tester.Assert(config.Parameters[0] == "Settlement");
		Tester.Assert(config.Parameters[1] == "OrgGrade");
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		AppendSettlement(orgInfo.SettlementId);
		AppendOrgGrade(orgInfo.OrgTemplateId, orgInfo.Grade, orgInfo.Principal, gender);
		EndAddingRecord(num);
		return num;
	}

	public int AddSingleCharacterTemplateParameterBubble(TeammateBubbleItem config, int index, int subtype, short templateId)
	{
		Tester.Assert(config.Parameters[0] == "Character");
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[1]));
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		AppendCharacterTemplate(templateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddChickenParameterBubble(TeammateBubbleItem config, int index, int subtype, Location location, short chickenTemplateId)
	{
		Tester.Assert(config.Parameters[0] == "Location");
		Tester.Assert(config.Parameters[1] == "Chicken");
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		AppendLocation(location);
		AppendChicken(chickenTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddLocationParameterBubble(TeammateBubbleItem config, int index, int subtype, Location location)
	{
		Tester.Assert(config.Parameters[0] == "Location");
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[1]));
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddLocationParameterBubble(TeammateBubbleItem config, int index, int subtype, Location location, short adventureTemplateId)
	{
		Tester.Assert(config.Parameters[0] == "Location");
		Tester.Assert(config.Parameters[1] == "Adventure");
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		AppendLocation(location);
		AppendAdventure(adventureTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddAdventureParameterBubble(TeammateBubbleItem config, int index, int subtype, short adventureTemplateId)
	{
		Tester.Assert(config.Parameters[0] == "Adventure");
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[1]));
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		AppendAdventure(adventureTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddSecretInformationCharacterParameterBubble(TeammateBubbleItem config, int index, int subtype, int charId, short templateId)
	{
		Tester.Assert(config.Parameters[0] == "Character");
		Tester.Assert(config.Parameters[1] == "SecretInformation");
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		AppendCharacter(charId);
		AppendSecretInformationTemplate(templateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddLegendaryBookInsaneCharacterParameterBubble(TeammateBubbleItem config, int index, int subtype, int charId, ItemKey bookKey)
	{
		Tester.Assert(config.Parameters[0] == "Character");
		Tester.Assert(config.Parameters[1] == "Item");
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		AppendCharacter(charId);
		AppendItem(bookKey.ItemType, bookKey.TemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddSingleCombatSkillTypeParameterBubble(TeammateBubbleItem config, int index, int subtype, sbyte type)
	{
		Tester.Assert(config.Parameters[0] == "CombatSkillType");
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[1]));
		Tester.Assert(string.IsNullOrEmpty(config.Parameters[2]));
		int num = BeginAddingRecord(index, config.TemplateId, subtype);
		AppendCombatSkillType(type);
		EndAddingRecord(num);
		return num;
	}
}
