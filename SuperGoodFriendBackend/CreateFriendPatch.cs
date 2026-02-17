using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.ParallelModifications;
using GameData.Domains.Map;
using HarmonyLib;
using NLog;

namespace SuperGoodFriendBackend
{
	// Token: 0x02000002 RID: 2
	public static class CreateFriendPatch
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[HarmonyPatch(typeof(CharacterDomain), "CreateProtagonist")]
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> CreateProtagonist_Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			CreateFriendPatch.<CreateProtagonist_Transpiler>d__8 <CreateProtagonist_Transpiler>d__ = new CreateFriendPatch.<CreateProtagonist_Transpiler>d__8(-2);
			<CreateProtagonist_Transpiler>d__.<>3__instructions = instructions;
			return <CreateProtagonist_Transpiler>d__;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		[HarmonyPatch(typeof(CharacterDomain), "CreateCloseFriend")]
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> CreateCloseFriend_Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			CreateFriendPatch.<CreateCloseFriend_Transpiler>d__9 <CreateCloseFriend_Transpiler>d__ = new CreateFriendPatch.<CreateCloseFriend_Transpiler>d__9(-2);
			<CreateCloseFriend_Transpiler>d__.<>3__instructions = instructions;
			return <CreateCloseFriend_Transpiler>d__;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
		public unsafe static Character CreateCloseFriendEx(CharacterDomain instance, DataContext context, short charTemplateId, short morality, Character protagonistChar, ProtagonistCreationInfo info)
		{
			Logger logger = CreateFriendPatch.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 6);
			defaultInterpolatedStringHandler.AppendFormatted<CharacterDomain>(instance);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<DataContext>(context);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(charTemplateId);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(morality);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<Character>(protagonistChar);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<ProtagonistCreationInfo>(info);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sbyte gender = protagonistChar.GetGender();
			Random random = new Random();
			Character character = null;
			CreateFriendPatch.s_boyFamilyNames.Clear();
			CreateFriendPatch.s_boyFamilyNames.AddRange(ModMain.BoyFamilyNameSetArray);
			CreateFriendPatch.s_boyGivenNames.Clear();
			CreateFriendPatch.s_boyGivenNames.AddRange(ModMain.BoyGivenNameSetArray);
			CreateFriendPatch.s_girlFamilyNames.Clear();
			CreateFriendPatch.s_girlFamilyNames.AddRange(ModMain.GirlFamilyNameSetArray);
			CreateFriendPatch.s_girlGivenNames.Clear();
			CreateFriendPatch.s_girlGivenNames.AddRange(ModMain.GirlGivenNameSetArray);
			CreateFriendPatch.s_player = protagonistChar;
			CreateFriendPatch.s_boyFriends.Clear();
			CreateFriendPatch.s_girlFriends.Clear();
			for (int i = 0; i < ModMain.FriendCount; i++)
			{
				sbyte gender2 = Gender.Flip(gender);
				switch (ModMain.FixedSex)
				{
				case 1:
					gender2 = gender;
					break;
				case 2:
					gender2 = 1;
					break;
				case 3:
					gender2 = 0;
					break;
				case 4:
					gender2 = (sbyte)random.Next(0, 2);
					break;
				}
				charTemplateId = MapDomain.GetCharacterTemplateId(info.TaiwuVillageStateTemplateId, gender2);
				character = instance.CreateCloseFriend(context, charTemplateId, morality, protagonistChar);
				bool enableFriendCombatSkillQualifications = ModMain.EnableFriendCombatSkillQualifications;
				if (enableFriendCombatSkillQualifications)
				{
					int friendCombatSkillQualificationsMin = ModMain.FriendCombatSkillQualificationsMin;
					int maxValue = (ModMain.FriendCombatSkillQualificationsMax > friendCombatSkillQualificationsMin) ? (ModMain.FriendCombatSkillQualificationsMax + 1) : (friendCombatSkillQualificationsMin + 1);
					CombatSkillShorts combatSkillShorts = default(CombatSkillShorts);
					for (int j = 0; j < 14; j++)
					{
						short num = (short)random.Next(friendCombatSkillQualificationsMin, maxValue);
						*(ref combatSkillShorts.Items.FixedElementField + (IntPtr)j * 2) = num;
					}
					character.SetBaseCombatSkillQualifications(ref combatSkillShorts, context);
				}
				bool enableFriendLifeSkillQualifications = ModMain.EnableFriendLifeSkillQualifications;
				if (enableFriendLifeSkillQualifications)
				{
					int friendLifeSkillQualificationsMin = ModMain.FriendLifeSkillQualificationsMin;
					int maxValue2 = (ModMain.FriendLifeSkillQualificationsMax > friendLifeSkillQualificationsMin) ? (ModMain.FriendLifeSkillQualificationsMax + 1) : (friendLifeSkillQualificationsMin + 1);
					LifeSkillShorts lifeSkillShorts = default(LifeSkillShorts);
					for (int k = 0; k < 16; k++)
					{
						short num2 = (short)random.Next(friendLifeSkillQualificationsMin, maxValue2);
						*(ref lifeSkillShorts.Items.FixedElementField + (IntPtr)k * 2) = num2;
					}
					character.SetBaseLifeSkillQualifications(ref lifeSkillShorts, context);
				}
				bool enableFriendMainAttributes = ModMain.EnableFriendMainAttributes;
				if (enableFriendMainAttributes)
				{
					int friendMainAttributesMin = ModMain.FriendMainAttributesMin;
					int maxValue3 = (ModMain.FriendMainAttributesMax > friendMainAttributesMin) ? (ModMain.FriendMainAttributesMax + 1) : (friendMainAttributesMin + 1);
					MainAttributes baseMainAttributes = default(MainAttributes);
					for (int l = 0; l < 6; l++)
					{
						short num3 = (short)random.Next(friendMainAttributesMin, maxValue3);
						*(ref baseMainAttributes.Items.FixedElementField + (IntPtr)l * 2) = num3;
					}
					character.SetBaseMainAttributes(baseMainAttributes, context);
				}
			}
			CreateFriendPatch.s_boyFriends.Clear();
			CreateFriendPatch.s_girlFriends.Clear();
			return character;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000023BC File Offset: 0x000005BC
		public static Character ComplementCreateIntelligentCharacterEx(CharacterDomain instance, DataContext context, CreateIntelligentCharacterModification mod, bool autoComplement)
		{
			Character self = mod.Self;
			Random random = new Random();
			sbyte gender = self.GetGender();
			bool flag = false;
			switch ((gender != 0) ? ModMain.BoyTransType : ModMain.GirlTransType)
			{
			case 1:
				flag = true;
				break;
			case 2:
				flag = (random.Next(2) == 0);
				break;
			case 3:
				flag = (random.Next(5) == 0);
				break;
			}
			bool flag2 = flag;
			if (flag2)
			{
				List<int> list = (gender == 0) ? ModMain.BoyBodyTypes : ModMain.GirlBodyTypes;
				int num = list[random.Next(list.Count)];
				self.GetAvatar().ChangeGender(Gender.Flip(gender));
				self.GetAvatar().ChangeBodyType((sbyte)num);
			}
			else
			{
				List<int> list2 = (gender == 0) ? ModMain.GirlBodyTypes : ModMain.BoyBodyTypes;
				int num2 = list2[random.Next(list2.Count)];
				self.GetAvatar().ChangeGender(gender);
				self.GetAvatar().ChangeBodyType((sbyte)num2);
			}
			Character value = Traverse.Create(instance).Method("ComplementCreateIntelligentCharacter", new object[]
			{
				context,
				mod,
				autoComplement
			}).GetValue<Character>();
			int id = self.GetId();
			bool flag3 = gender == 0;
			if (flag3)
			{
				CreateFriendPatch.CreateRelation(context, ModMain.GirlFriendsStartRelationWithPlayer, id, CreateFriendPatch.s_player.GetId());
				foreach (Character character in CreateFriendPatch.s_girlFriends)
				{
					int id2 = character.GetId();
					CreateFriendPatch.CreateRelation(context, ModMain.GirlFriendsStartRelationWithEachOther, id, id2);
				}
				foreach (Character character2 in CreateFriendPatch.s_boyFriends)
				{
					int id3 = character2.GetId();
					CreateFriendPatch.CreateRelation(context, ModMain.DifferentSexFriendsStartRelationWithEachOther, id, id3);
				}
				CreateFriendPatch.s_girlFriends.Add(self);
			}
			else
			{
				CreateFriendPatch.CreateRelation(context, ModMain.BoyFriendsStartRelationWithPlayer, id, CreateFriendPatch.s_player.GetId());
				foreach (Character character3 in CreateFriendPatch.s_girlFriends)
				{
					int id4 = character3.GetId();
					CreateFriendPatch.CreateRelation(context, ModMain.DifferentSexFriendsStartRelationWithEachOther, id, id4);
				}
				foreach (Character character4 in CreateFriendPatch.s_boyFriends)
				{
					int id5 = character4.GetId();
					CreateFriendPatch.CreateRelation(context, ModMain.BoyFriendsStartRelationWithEachOther, id, id5);
				}
				CreateFriendPatch.s_boyFriends.Add(self);
			}
			return value;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000026C8 File Offset: 0x000008C8
		public static void CreateRelation(DataContext context, int customRelationType, int charA, int charB)
		{
			CharacterDomain character = DomainManager.Character;
			switch (customRelationType)
			{
			case 1:
				character.AddRelation(context, charA, charB, 512, int.MinValue);
				character.DirectlySetFavorabilities(context, charA, charB, 30000, 30000);
				break;
			case 2:
				character.AddRelation(context, charA, charB, 4, int.MinValue);
				character.DirectlySetFavorabilities(context, charA, charB, 30000, 30000);
				break;
			case 3:
				character.AddRelation(context, charA, charB, 16384, int.MinValue);
				character.AddRelation(context, charB, charA, 16384, int.MinValue);
				character.DirectlySetFavorabilities(context, charA, charB, 30000, 30000);
				break;
			case 4:
				character.AddRelation(context, charA, charB, 8192, int.MinValue);
				character.DirectlySetFavorabilities(context, charA, charB, 30000, 30000);
				break;
			case 5:
				character.AddRelation(context, charA, charB, 32768, int.MinValue);
				character.AddRelation(context, charB, charA, 32768, int.MinValue);
				character.DirectlySetFavorabilities(context, charA, charB, -30000, -30000);
				break;
			case 6:
				character.AddRelation(context, charA, charB, 1024, int.MinValue);
				character.DirectlySetFavorabilities(context, charA, charB, 30000, 30000);
				break;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002830 File Offset: 0x00000A30
		public static CreateIntelligentCharacterModification ParallelCreateIntelligentCharacterEx(DataContext context, ref IntelligentCharacterCreationInfo info, bool recordModification = true)
		{
			Random random = new Random();
			info.Age = ((ModMain.StartAgeMin < ModMain.StartAgeMax) ? ((short)random.Next(ModMain.StartAgeMin, ModMain.StartAgeMax + 1)) : ((short)ModMain.StartAgeMin));
			return CharacterDomain.ParallelCreateIntelligentCharacter(context, ref info, recordModification);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002880 File Offset: 0x00000A80
		[HarmonyPostfix]
		[HarmonyPatch(typeof(Character), "OfflineSetCloseFriendFields")]
		public static void OfflineSetCloseFriendFields_Postfix(Character __instance, DataContext context, ref FullName ____fullName)
		{
			Random random = new Random();
			sbyte gender = __instance.GetGender();
			bool flag = gender == 0;
			int num;
			int value;
			int value2;
			List<string> list;
			List<string> list2;
			if (flag)
			{
				num = ModMain.GirlLoveType;
				value = ModMain.FixedGirlFamilyName;
				value2 = ModMain.FixedGirlGivenName;
				list = CreateFriendPatch.s_girlFamilyNames;
				list2 = CreateFriendPatch.s_girlGivenNames;
			}
			else
			{
				num = ModMain.BoyLoveType;
				value = ModMain.FixedBoyFamilyName;
				value2 = ModMain.FixedBoyGivenName;
				list = CreateFriendPatch.s_boyFamilyNames;
				list2 = CreateFriendPatch.s_boyGivenNames;
			}
			Traverse<bool> traverse = Traverse.Create(__instance).Field<bool>("_bisexual");
			switch (num)
			{
			case 0:
				traverse.Value = false;
				break;
			case 1:
				traverse.Value = true;
				break;
			case 2:
				traverse.Value = (random.Next(2) == 0);
				break;
			default:
				traverse.Value = (random.Next(5) == 0);
				break;
			}
			FullName fullName = __instance.GetFullName();
			string text = null;
			bool flag2 = list.Count > 0;
			if (flag2)
			{
				switch (value)
				{
				case 1:
					text = list[random.Next(list.Count)];
					break;
				case 2:
				{
					int index = random.Next(list.Count);
					text = list[index];
					list.RemoveAt(index);
					break;
				}
				case 3:
					text = list[0];
					list.RemoveAt(0);
					break;
				}
			}
			bool flag3 = !string.IsNullOrEmpty(text);
			if (flag3)
			{
				fullName.Type |= 1;
				fullName.Type |= 4;
				fullName.CustomSurnameId = DomainManager.World.RegisterCustomText(context, text);
			}
			string text2 = null;
			bool flag4 = list2.Count > 0;
			if (flag4)
			{
				switch (value2)
				{
				case 1:
					text2 = list2[random.Next(list2.Count)];
					break;
				case 2:
				{
					int index2 = random.Next(list2.Count);
					text2 = list2[index2];
					list2.RemoveAt(index2);
					break;
				}
				case 3:
					text2 = list2[0];
					list2.RemoveAt(0);
					break;
				}
			}
			bool flag5 = !string.IsNullOrEmpty(text2);
			if (flag5)
			{
				fullName.Type |= 1;
				fullName.Type |= 8;
				fullName.CustomGivenNameId = DomainManager.World.RegisterCustomText(context, text2);
			}
			____fullName = fullName;
			Logger logger = CreateFriendPatch.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 5);
			defaultInterpolatedStringHandler.AppendLiteral("Type:");
			defaultInterpolatedStringHandler.AppendFormatted<int>(value);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(value2);
			defaultInterpolatedStringHandler.AppendLiteral(" Gender:");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(gender);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x04000001 RID: 1
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04000002 RID: 2
		private static List<string> s_boyFamilyNames = new List<string>();

		// Token: 0x04000003 RID: 3
		private static List<string> s_girlFamilyNames = new List<string>();

		// Token: 0x04000004 RID: 4
		private static List<string> s_boyGivenNames = new List<string>();

		// Token: 0x04000005 RID: 5
		private static List<string> s_girlGivenNames = new List<string>();

		// Token: 0x04000006 RID: 6
		private static List<Character> s_boyFriends = new List<Character>();

		// Token: 0x04000007 RID: 7
		private static List<Character> s_girlFriends = new List<Character>();

		// Token: 0x04000008 RID: 8
		private static Character s_player;
	}
}
