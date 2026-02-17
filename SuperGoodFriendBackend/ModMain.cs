using System;
using System.Collections.Generic;
using GameData.Domains;
using HarmonyLib;
using NLog;
using TaiwuModdingLib.Core.Plugin;

namespace SuperGoodFriendBackend
{
	// Token: 0x02000003 RID: 3
	[PluginConfig("谷中密友团", "剑圣(skyswordkill)", "1.0.0")]
	public class ModMain : TaiwuRemakePlugin
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002BCF File Offset: 0x00000DCF
		public override void Initialize()
		{
			this.harmony = new Harmony("SuperGoodFriendBackend");
			this.harmony.PatchAll(typeof(CreateFriendPatch));
			this.OnModSettingUpdate();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002BFF File Offset: 0x00000DFF
		public override void Dispose()
		{
			this.harmony.UnpatchSelf();
			this.harmony = null;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002C18 File Offset: 0x00000E18
		public override void OnModSettingUpdate()
		{
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FriendCount", ref ModMain.FriendCount);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FixedSex", ref ModMain.FixedSex);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FixedBoyFamilyName", ref ModMain.FixedBoyFamilyName);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FixedBoyGivenName", ref ModMain.FixedBoyGivenName);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FixedGirlFamilyName", ref ModMain.FixedGirlFamilyName);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FixedGirlGivenName", ref ModMain.FixedGirlGivenName);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_BoyTransType", ref ModMain.BoyTransType);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_BoyLoveType", ref ModMain.BoyLoveType);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_GirlTransType", ref ModMain.GirlTransType);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_GirlLoveType", ref ModMain.GirlLoveType);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_StartAgeMin", ref ModMain.StartAgeMin);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_StartAgeMax", ref ModMain.StartAgeMax);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_BoyFriendsStartRelationWithEachOther", ref ModMain.BoyFriendsStartRelationWithEachOther);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_GirlFriendsStartRelationWithEachOther", ref ModMain.GirlFriendsStartRelationWithEachOther);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_BoyFriendsStartRelationWithPlayer", ref ModMain.BoyFriendsStartRelationWithPlayer);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_GirlFriendsStartRelationWithPlayer", ref ModMain.GirlFriendsStartRelationWithPlayer);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_DifferentSexFriendsStartRelationWithEachOther", ref ModMain.DifferentSexFriendsStartRelationWithEachOther);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Bool_EnableFriendCombatSkillQualifications", ref ModMain.EnableFriendCombatSkillQualifications);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FriendCombatSkillQualificationsMin", ref ModMain.FriendCombatSkillQualificationsMin);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FriendCombatSkillQualificationsMax", ref ModMain.FriendCombatSkillQualificationsMax);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Bool_EnableFriendLifeSkillQualifications", ref ModMain.EnableFriendLifeSkillQualifications);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FriendLifeSkillQualificationsMin", ref ModMain.FriendLifeSkillQualificationsMin);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FriendLifeSkillQualificationsMax", ref ModMain.FriendLifeSkillQualificationsMax);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Bool_EnableFriendMainAttributes", ref ModMain.EnableFriendMainAttributes);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FriendMainAttributesMin", ref ModMain.FriendMainAttributesMin);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Int_FriendMainAttributesMax", ref ModMain.FriendMainAttributesMax);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Bool_EnableBoyBodyType0", ref ModMain.EnableBoyBodyType0);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Bool_EnableBoyBodyType1", ref ModMain.EnableBoyBodyType1);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Bool_EnableBoyBodyType2", ref ModMain.EnableBoyBodyType2);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Bool_EnableGirlBodyType0", ref ModMain.EnableGirlBodyType0);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Bool_EnableGirlBodyType1", ref ModMain.EnableGirlBodyType1);
			DomainManager.Mod.GetSetting(base.ModIdStr, "Bool_EnableGirlBodyType2", ref ModMain.EnableGirlBodyType2);
			DomainManager.Mod.GetSetting(base.ModIdStr, "String_BoyFamilyNameSet", ref ModMain.BoyFamilyNameSet);
			DomainManager.Mod.GetSetting(base.ModIdStr, "String_BoyGivenNameSet", ref ModMain.BoyGivenNameSet);
			DomainManager.Mod.GetSetting(base.ModIdStr, "String_GirlFamilyNameSet", ref ModMain.GirlFamilyNameSet);
			DomainManager.Mod.GetSetting(base.ModIdStr, "String_GirlGivenNameSet", ref ModMain.GirlGivenNameSet);
			ModMain.BoyBodyTypes = new List<int>();
			bool enableBoyBodyType = ModMain.EnableBoyBodyType0;
			if (enableBoyBodyType)
			{
				ModMain.BoyBodyTypes.Add(0);
			}
			bool enableBoyBodyType2 = ModMain.EnableBoyBodyType1;
			if (enableBoyBodyType2)
			{
				ModMain.BoyBodyTypes.Add(1);
			}
			bool enableBoyBodyType3 = ModMain.EnableBoyBodyType2;
			if (enableBoyBodyType3)
			{
				ModMain.BoyBodyTypes.Add(2);
			}
			bool flag = ModMain.BoyBodyTypes.Count == 0;
			if (flag)
			{
				ModMain.BoyBodyTypes.Add(0);
				ModMain.BoyBodyTypes.Add(1);
				ModMain.BoyBodyTypes.Add(2);
			}
			ModMain.GirlBodyTypes = new List<int>();
			bool enableGirlBodyType = ModMain.EnableGirlBodyType0;
			if (enableGirlBodyType)
			{
				ModMain.GirlBodyTypes.Add(0);
			}
			bool enableGirlBodyType2 = ModMain.EnableGirlBodyType1;
			if (enableGirlBodyType2)
			{
				ModMain.GirlBodyTypes.Add(1);
			}
			bool enableGirlBodyType3 = ModMain.EnableGirlBodyType2;
			if (enableGirlBodyType3)
			{
				ModMain.GirlBodyTypes.Add(2);
			}
			bool flag2 = ModMain.GirlBodyTypes.Count == 0;
			if (flag2)
			{
				ModMain.GirlBodyTypes.Add(0);
				ModMain.GirlBodyTypes.Add(1);
				ModMain.GirlBodyTypes.Add(2);
			}
			this.SetNameSet(ModMain.BoyFamilyNameSet, ModMain.BoyFamilyNameSetArray);
			this.SetNameSet(ModMain.BoyGivenNameSet, ModMain.BoyGivenNameSetArray);
			this.SetNameSet(ModMain.GirlFamilyNameSet, ModMain.GirlFamilyNameSetArray);
			this.SetNameSet(ModMain.GirlGivenNameSet, ModMain.GirlGivenNameSetArray);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00003140 File Offset: 0x00001340
		private void SetNameSet(string names, List<string> nameSet)
		{
			nameSet.Clear();
			ModMain.Logger.Info("原始集合：" + names);
			char[] separator = new char[]
			{
				',',
				'，'
			};
			foreach (string text in names.Split(separator))
			{
				string text2 = text.Trim();
				bool flag = !string.IsNullOrEmpty(text2);
				if (flag)
				{
					nameSet.Add(text2);
					ModMain.Logger.Info("添加 : " + text2);
				}
			}
		}

		// Token: 0x04000009 RID: 9
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400000A RID: 10
		private Harmony harmony;

		// Token: 0x0400000B RID: 11
		public static int FixedSex = 0;

		// Token: 0x0400000C RID: 12
		public static int FixedBoyFamilyName = 0;

		// Token: 0x0400000D RID: 13
		public static int FixedBoyGivenName = 0;

		// Token: 0x0400000E RID: 14
		public static int FixedGirlFamilyName = 0;

		// Token: 0x0400000F RID: 15
		public static int FixedGirlGivenName = 0;

		// Token: 0x04000010 RID: 16
		public static string BoyFamilyNameSet = "";

		// Token: 0x04000011 RID: 17
		public static string BoyGivenNameSet = "";

		// Token: 0x04000012 RID: 18
		public static string GirlFamilyNameSet = "";

		// Token: 0x04000013 RID: 19
		public static string GirlGivenNameSet = "";

		// Token: 0x04000014 RID: 20
		public static List<string> BoyFamilyNameSetArray = new List<string>();

		// Token: 0x04000015 RID: 21
		public static List<string> BoyGivenNameSetArray = new List<string>();

		// Token: 0x04000016 RID: 22
		public static List<string> GirlFamilyNameSetArray = new List<string>();

		// Token: 0x04000017 RID: 23
		public static List<string> GirlGivenNameSetArray = new List<string>();

		// Token: 0x04000018 RID: 24
		public static int FriendCount = 1;

		// Token: 0x04000019 RID: 25
		public static int BoyTransType = 0;

		// Token: 0x0400001A RID: 26
		public static int BoyLoveType = 0;

		// Token: 0x0400001B RID: 27
		public static int GirlTransType = 0;

		// Token: 0x0400001C RID: 28
		public static int GirlLoveType = 0;

		// Token: 0x0400001D RID: 29
		public static bool EnableBoyBodyType0 = true;

		// Token: 0x0400001E RID: 30
		public static bool EnableBoyBodyType1 = true;

		// Token: 0x0400001F RID: 31
		public static bool EnableBoyBodyType2 = true;

		// Token: 0x04000020 RID: 32
		public static bool EnableGirlBodyType0 = true;

		// Token: 0x04000021 RID: 33
		public static bool EnableGirlBodyType1 = true;

		// Token: 0x04000022 RID: 34
		public static bool EnableGirlBodyType2 = true;

		// Token: 0x04000023 RID: 35
		public static List<int> BoyBodyTypes = new List<int>();

		// Token: 0x04000024 RID: 36
		public static List<int> GirlBodyTypes = new List<int>();

		// Token: 0x04000025 RID: 37
		public static int StartAgeMin = 13;

		// Token: 0x04000026 RID: 38
		public static int StartAgeMax = 13;

		// Token: 0x04000027 RID: 39
		public static int BoyFriendsStartRelationWithEachOther = 0;

		// Token: 0x04000028 RID: 40
		public static int GirlFriendsStartRelationWithEachOther = 0;

		// Token: 0x04000029 RID: 41
		public static int BoyFriendsStartRelationWithPlayer = 0;

		// Token: 0x0400002A RID: 42
		public static int GirlFriendsStartRelationWithPlayer = 0;

		// Token: 0x0400002B RID: 43
		public static int DifferentSexFriendsStartRelationWithEachOther = 0;

		// Token: 0x0400002C RID: 44
		public static bool EnableFriendCombatSkillQualifications;

		// Token: 0x0400002D RID: 45
		public static int FriendCombatSkillQualificationsMin;

		// Token: 0x0400002E RID: 46
		public static int FriendCombatSkillQualificationsMax;

		// Token: 0x0400002F RID: 47
		public static bool EnableFriendLifeSkillQualifications;

		// Token: 0x04000030 RID: 48
		public static int FriendLifeSkillQualificationsMin;

		// Token: 0x04000031 RID: 49
		public static int FriendLifeSkillQualificationsMax;

		// Token: 0x04000032 RID: 50
		public static bool EnableFriendMainAttributes;

		// Token: 0x04000033 RID: 51
		public static int FriendMainAttributesMin;

		// Token: 0x04000034 RID: 52
		public static int FriendMainAttributesMax;
	}
}
