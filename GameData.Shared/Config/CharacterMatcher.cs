using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class CharacterMatcher : ConfigData<CharacterMatcherItem, byte>
{
	public static class DefKey
	{
		public const byte CanInteractAsIntelligentCharacter = 0;

		public const byte CanWork = 1;

		public const byte CanAdoptChild = 2;

		public const byte CanBeRevengeTarget = 3;

		public const byte CanPerformInfectedAction = 4;

		public const byte CanReceiveSecretInformation = 5;

		public const byte JuniorXiangshuFollowingCharacter = 6;

		public const byte CanStartSexRelation = 8;

		public const byte CanBeUndertaker = 9;

		public const byte AvailableForLegendaryBookAdventure = 10;

		public const byte PrepareCharacterForSpiritualWanderPlace = 11;

		public const byte CanHaveBounty = 12;

		public const byte CanBeAttackedByInfectedCharacter = 13;

		public const byte CanBeAttackedByRandomEnemy = 14;

		public const byte CanBeRemovedFromGroup = 15;

		public const byte CanBeMerchantAutoActionTarget = 16;

		public const byte CanBeJixiKillingTarget = 17;

		public const byte VillagerAvailableForWork = 18;

		public const byte ChildVillagerAvailableForWork = 7;

		public const byte InteractWithShixiangMemberEventTarget = 19;

		public const byte EmeiPotentialVictims = 20;

		public const byte InSettlement = 55;

		public const byte CanJoinFeast = 56;
	}

	public static class DefValue
	{
		public static CharacterMatcherItem CanInteractAsIntelligentCharacter => Instance[(byte)0];

		public static CharacterMatcherItem CanWork => Instance[(byte)1];

		public static CharacterMatcherItem CanAdoptChild => Instance[(byte)2];

		public static CharacterMatcherItem CanBeRevengeTarget => Instance[(byte)3];

		public static CharacterMatcherItem CanPerformInfectedAction => Instance[(byte)4];

		public static CharacterMatcherItem CanReceiveSecretInformation => Instance[(byte)5];

		public static CharacterMatcherItem JuniorXiangshuFollowingCharacter => Instance[(byte)6];

		public static CharacterMatcherItem CanStartSexRelation => Instance[(byte)8];

		public static CharacterMatcherItem CanBeUndertaker => Instance[(byte)9];

		public static CharacterMatcherItem AvailableForLegendaryBookAdventure => Instance[(byte)10];

		public static CharacterMatcherItem PrepareCharacterForSpiritualWanderPlace => Instance[(byte)11];

		public static CharacterMatcherItem CanHaveBounty => Instance[(byte)12];

		public static CharacterMatcherItem CanBeAttackedByInfectedCharacter => Instance[(byte)13];

		public static CharacterMatcherItem CanBeAttackedByRandomEnemy => Instance[(byte)14];

		public static CharacterMatcherItem CanBeRemovedFromGroup => Instance[(byte)15];

		public static CharacterMatcherItem CanBeMerchantAutoActionTarget => Instance[(byte)16];

		public static CharacterMatcherItem CanBeJixiKillingTarget => Instance[(byte)17];

		public static CharacterMatcherItem VillagerAvailableForWork => Instance[(byte)18];

		public static CharacterMatcherItem ChildVillagerAvailableForWork => Instance[(byte)7];

		public static CharacterMatcherItem InteractWithShixiangMemberEventTarget => Instance[(byte)19];

		public static CharacterMatcherItem EmeiPotentialVictims => Instance[(byte)20];

		public static CharacterMatcherItem InSettlement => Instance[(byte)55];

		public static CharacterMatcherItem CanJoinFeast => Instance[(byte)56];
	}

	public static CharacterMatcher Instance = new CharacterMatcher();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "Organization", "SubConditions", "TemplateId" };

	internal override int ToInt(byte value)
	{
		return value;
	}

	internal override byte ToTemplateId(int value)
	{
		return (byte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new CharacterMatcherItem(0, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(1, ECharacterMatcherAgeType.Adult, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(2, ECharacterMatcherAgeType.Adult, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[5]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.CanHaveChild,
			ECharacterMatcherSubCondition.NotTaiwu,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(3, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[2]
		{
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(4, ECharacterMatcherAgeType.NotRestricted, ECharacterMatcherIdentityType.XiangshuInfected, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[2]
		{
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(5, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(6, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotInTaiwuGroup,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(7, ECharacterMatcherAgeType.Child, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[4]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotInTaiwuGroup,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(8, ECharacterMatcherAgeType.Adult, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(9, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(10, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[6]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotInTaiwuGroup,
			ECharacterMatcherSubCondition.NotAssignedWithWork,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling,
			ECharacterMatcherSubCondition.CombatPowerTop30
		}));
		_dataArray.Add(new CharacterMatcherItem(11, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotInTaiwuGroup,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(12, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotXiangshuInfected, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.NotTaiwu,
			ECharacterMatcherSubCondition.NotInSettlementPrison
		}));
		_dataArray.Add(new CharacterMatcherItem(13, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[0]));
		_dataArray.Add(new CharacterMatcherItem(14, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[0]));
		_dataArray.Add(new CharacterMatcherItem(15, ECharacterMatcherAgeType.Adult, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[0]));
		_dataArray.Add(new CharacterMatcherItem(16, ECharacterMatcherAgeType.Adult, ECharacterMatcherIdentityType.NotTaiwuVillage, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(17, ECharacterMatcherAgeType.Adult, ECharacterMatcherIdentityType.NotTaiwuVillage, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1] { ECharacterMatcherSubCondition.NotTaiwuFriendlyRelation }));
		_dataArray.Add(new CharacterMatcherItem(18, ECharacterMatcherAgeType.Adult, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotInTaiwuGroup
		}));
		_dataArray.Add(new CharacterMatcherItem(19, ECharacterMatcherAgeType.Adult, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, 6, new ECharacterMatcherSubCondition[4]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotHighestGrade,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(20, ECharacterMatcherAgeType.Adult, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, 2, new ECharacterMatcherSubCondition[0]));
		_dataArray.Add(new CharacterMatcherItem(21, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.Sect, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(22, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[2]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.NotTaiwuFriendlyRelation
		}));
		_dataArray.Add(new CharacterMatcherItem(23, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[0]));
		_dataArray.Add(new CharacterMatcherItem(24, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.Female, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.CanHaveChild,
			ECharacterMatcherSubCondition.NotLegendaryBookConsumed,
			ECharacterMatcherSubCondition.NotInSettlementPrison
		}));
		_dataArray.Add(new CharacterMatcherItem(25, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[4]
		{
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotLegendaryBookConsumed,
			ECharacterMatcherSubCondition.NotInSettlementPrison,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(26, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.Sect, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[4]
		{
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotLegendaryBookConsumed,
			ECharacterMatcherSubCondition.NotInSettlementPrison,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(27, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[4]
		{
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotLegendaryBookConsumed,
			ECharacterMatcherSubCondition.NotInSettlementPrison,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(28, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[0]));
		_dataArray.Add(new CharacterMatcherItem(29, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(30, ECharacterMatcherAgeType.NotRestricted, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(31, ECharacterMatcherAgeType.NotRestricted, ECharacterMatcherIdentityType.NotSect, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[0]));
		_dataArray.Add(new CharacterMatcherItem(32, ECharacterMatcherAgeType.NotRestricted, ECharacterMatcherIdentityType.Sect, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[0]));
		_dataArray.Add(new CharacterMatcherItem(33, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.Sect, ECharacterMatcherGenderType.NotRestricted, 6, new ECharacterMatcherSubCondition[2]
		{
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(34, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.Sect, ECharacterMatcherGenderType.NotRestricted, 6, new ECharacterMatcherSubCondition[0]));
		_dataArray.Add(new CharacterMatcherItem(35, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.Sect, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[5]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotLegendaryBookConsumed,
			ECharacterMatcherSubCondition.NotInSettlementPrison,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(36, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[2]
		{
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(37, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.Sect, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(38, ECharacterMatcherAgeType.Adult, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(39, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(40, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(41, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(42, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(43, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(44, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(45, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(46, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(47, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(48, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(49, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(50, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(51, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(52, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1]));
		_dataArray.Add(new CharacterMatcherItem(53, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[3]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated,
			ECharacterMatcherSubCondition.NotCrossAreaTraveling
		}));
		_dataArray.Add(new CharacterMatcherItem(54, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[0]));
		_dataArray.Add(new CharacterMatcherItem(55, ECharacterMatcherAgeType.NotRestricted, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[1] { ECharacterMatcherSubCondition.InSettlement }));
		_dataArray.Add(new CharacterMatcherItem(56, ECharacterMatcherAgeType.NonBaby, ECharacterMatcherIdentityType.NotRestricted, ECharacterMatcherGenderType.NotRestricted, -1, new ECharacterMatcherSubCondition[2]
		{
			ECharacterMatcherSubCondition.NotActingCrazy,
			ECharacterMatcherSubCondition.CanBeLocated
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CharacterMatcherItem>(57);
		CreateItems0();
	}
}
