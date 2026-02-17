using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Config;
using GameData.Common;
using GameData.ArchiveData;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Display;
using GameData.Domains.Organization;
using GameData.Domains.CombatSkill;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using TaiwuModdingLib.Core.Plugin;
using NLog;

namespace RealTimeModifyCharacterBackend
{
    [PluginConfig("璇女峰文艺复兴", "black_wing", "12.0.9")]
    public class ModMain : TaiwuRemakeHarmonyPlugin
    {
        public const short XuanNvSectId = 8;
        public const sbyte FemaleGenderId = 0;

        // Settings variables
        public static int globalCharmMin = 900;
        public static int globalCharmMax = 900;
        public static string globalFeatureIds = "164"; // 忠贞不渝
        public static string globalSkillIds = "";
        public static int globalCombatQualification = 130;
        public static int globalLifeQualification = 130;
        public static Dictionary<sbyte, int> gradeTargetTemplates = new Dictionary<sbyte, int>();

        public override void Initialize()
        {
            base.Initialize(); // Initializes Harmony and calls PatchAll
            LoadModSettings();
        }

        public override void OnModSettingUpdate()
        {
            LoadModSettings();
        }

        private void LoadModSettings()
        {
            try {
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCharmMin", ref globalCharmMin);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCharmMax", ref globalCharmMax);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalFeatureIds", ref globalFeatureIds);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalSkillIds", ref globalSkillIds);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCombatQualification", ref globalCombatQualification);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalLifeQualification", ref globalLifeQualification);

                gradeTargetTemplates.Clear();
                for (sbyte i = 0; i < 9; i++)
                {
                    int selectedIndex = 0;
                    DomainManager.Mod.GetSetting(base.ModIdStr, $"Grade{i + 1}_TargetTemplate", ref selectedIndex);
                    gradeTargetTemplates[i] = selectedIndex;
                }
            } catch (Exception) { }
        }
    }

    [HarmonyPatch]
    public static class XuanNuRenaissanceHooks
    {
        [HarmonyPatch(typeof(OrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
        [HarmonyPrefix]
        public static void GetOrgMemberConfig_Prefix(sbyte orgTemplateId, ref sbyte grade)
        {
            if (orgTemplateId == ModMain.XuanNvSectId)
            {
                if (ModMain.gradeTargetTemplates.TryGetValue(grade, out int mappedGrade))
                    grade = (sbyte)mappedGrade;
                else
                    grade = 0;
            }
        }

        [HarmonyPatch(typeof(OrganizationDomain), "GetCharacterTemplateId", new Type[] { typeof(sbyte), typeof(sbyte), typeof(sbyte) })]
        [HarmonyPrefix]
        public static bool GetCharacterTemplateId_Prefix(sbyte orgTemplateId, ref short __result)
        {
            if (orgTemplateId == ModMain.XuanNvSectId)
            {
                __result = 373;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter")]
        [HarmonyPostfix]
        public static unsafe void CreateIntelligentCharacter_Postfix(Character __result, DataContext context)
        {
            if (__result == null) return;

            OrganizationInfo orgInfo = __result.GetOrganizationInfo();
            if (orgInfo.OrgTemplateId == ModMain.XuanNvSectId)
            {
                int charId = __result.GetId();

                Util.SetGender(__result, ModMain.FemaleGenderId, context);
                Util.SetTransGender(__result, false, context);
                var avatar = __result.GetAvatar();
                avatar.ChangeGender(ModMain.FemaleGenderId);
                __result.SetAvatar(avatar, context);

                if (!string.IsNullOrWhiteSpace(ModMain.globalFeatureIds))
                {
                    foreach (string idStr in ModMain.globalFeatureIds.Split(','))
                        if (short.TryParse(idStr.Trim(), out short fid))
                            if (!__result.GetFeatureIds().Contains(fid))
                                __result.AddFeature(context, fid, true);
                }

                int minCharm = Math.Min(ModMain.globalCharmMin, ModMain.globalCharmMax);
                int maxCharm = Math.Max(ModMain.globalCharmMin, ModMain.globalCharmMax);
                short finalCharm = (short)context.Random.Next(minCharm, maxCharm + 1);
                Util.SetPrivateField(__result, "_baseAttraction", finalCharm);
                __result.SetBaseMorality(700, context);

                // --- Aptitude Enhancement with fixed pointer logic ---
                if (ModMain.globalCombatQualification > 0)
                {
                    CombatSkillShorts cQuals = __result.GetBaseCombatSkillQualifications();
                    bool changed = false;
                    short* p = &cQuals.Items.FixedElementField;
                    for (int i = 0; i < 14; i++)
                        if (p[i] < ModMain.globalCombatQualification) { p[i] = (short)ModMain.globalCombatQualification; changed = true; }
                    if (changed) __result.SetBaseCombatSkillQualifications(ref cQuals, context);
                }

                if (ModMain.globalLifeQualification > 0)
                {
                    LifeSkillShorts lQuals = __result.GetBaseLifeSkillQualifications();
                    bool changed = false;
                    short* p = &lQuals.Items.FixedElementField;
                    for (int i = 0; i < 16; i++)
                        if (p[i] < ModMain.globalLifeQualification) { p[i] = (short)ModMain.globalLifeQualification; changed = true; }
                    if (changed) __result.SetBaseLifeSkillQualifications(ref lQuals, context);
                }

                __result.SetConsummateLevel(18, context);

                if (string.IsNullOrWhiteSpace(ModMain.globalSkillIds))
                {
                    foreach (CombatSkillItem skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                        if (skillCfg.SectId == ModMain.XuanNvSectId) LearnAndMasterSkill(__result, skillCfg.TemplateId, context);
                }
                else
                {
                    foreach (string idStr in ModMain.globalSkillIds.Split(','))
                        if (short.TryParse(idStr.Trim(), out short sid)) LearnAndMasterSkill(__result, sid, context);
                }

                Util.InvokeMethod(__result, "InvalidateSelfAndInfluencedCache", new object[] { (ushort)0, context });
            }
        }

        private static void LearnAndMasterSkill(Character character, short skillId, DataContext context)
        {
            if (character.GetLearnedCombatSkills().Contains(skillId)) return;
            var skill = character.LearnNewCombatSkill(context, skillId, 65535);
            if (skill != null)
            {
                Util.InvokeMethod(skill, "SetPracticeLevel", new object[] { (sbyte)100, context });
                for (byte pIdx = 5; pIdx <= 14; pIdx++)
                    DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, character.GetId(), skillId, pIdx);
            }
        }
    }

    [HarmonyPatch(typeof(GameDataBridge), "ProcessMethodCall")]
    public static class RealTimeModifyCharacterPatch
    {
        [HarmonyPrefix]
        public static bool ProcessMethodCallPatch(Operation operation, RawDataPool argDataPool, DataContext context)
        {
            if (operation.DomainId != 66) return true;
            NotificationCollection notificationCollection = (NotificationCollection)AccessTools.Field(typeof(GameDataBridge), "_pendingNotifications").GetValue(context);
            if (!Common.IsInWorld()) return true;
            int result = HandleOperation(operation, argDataPool, notificationCollection.DataPool, context);
            if (result >= 0)
                notificationCollection.Notifications.Add(Notification.CreateMethodReturn(operation.ListenerId, operation.DomainId, operation.MethodId, result));
            return false;
        }

        private static int HandleOperation(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
        {
            int result = -1;
            int offset = operation.ArgsOffset;
            switch (operation.MethodId)
            {
                case 1: // Add Features
                case 2: // Remove Features
                {
                    string nameOrId = "";
                    List<int> fids = new List<int>();
                    int next = offset + Serializer.Deserialize(argDataPool, offset, ref nameOrId);
                    Serializer.Deserialize(argDataPool, next, ref fids);
                    Character character = Util.getCharacter(nameOrId);
                    if (character != null)
                        foreach (int fid in fids) if (operation.MethodId == 1) character.AddFeature(context, (short)fid, true); else character.RemoveFeature(context, (short)fid);
                    break;
                }
                case 3: // Set Morality
                {
                    string nameOrId = "";
                    int val = -9999;
                    int next = offset + Serializer.Deserialize(argDataPool, offset, ref nameOrId);
                    Serializer.Deserialize(argDataPool, next, ref val);
                    Character character = Util.getCharacter(nameOrId);
                    if (character != null && val >= -9000) character.SetBaseMorality((short)val, context);
                    break;
                }
                case 4: // Set Name
                {
                    string nameOrId = "", s1 = "", s2 = "";
                    int n1 = offset + Serializer.Deserialize(argDataPool, offset, ref nameOrId);
                    int n2 = n1 + Serializer.Deserialize(argDataPool, n1, ref s1);
                    Serializer.Deserialize(argDataPool, n2, ref s2);
                    Character character = Util.getCharacter(nameOrId);
                    if (character != null) {
                        CharacterItem cfg = Config.Character.Instance[character.GetTemplateId()];
                        if (cfg.CreatingType == 1 || cfg.CreatingType == 2) {
                            int sid = DomainManager.World.RegisterCustomText(context, s1), gid = DomainManager.World.RegisterCustomText(context, s2);
                            FullName fn = character.GetFullName();
                            character.SetFullName(new FullName(sid, gid, fn.SurnameId, fn.GivenNameGroupId, fn.GivenNameSuffixId, fn.GivenNameType), context);
                        }
                    }
                    break;
                }
                case 5: // Get Display Data
                {
                    string nameOrId = "";
                    Serializer.Deserialize(argDataPool, offset, ref nameOrId);
                    Character character = Util.getCharacter(nameOrId);
                    if (character != null) {
                        result = Serializer.Serialize(DomainManager.Character.GetCharacterDisplayData(character.GetId()), returnDataPool);
                        Serializer.Serialize(character.GetBirthMonth(), returnDataPool);
                    }
                    break;
                }
                case 6: // Set Identity/Avatar
                {
                    int cid = 0; sbyte g = 0, m = 0; AvatarData ad = new AvatarData();
                    int n1 = offset + Serializer.Deserialize(argDataPool, offset, ref cid);
                    int n2 = n1 + Serializer.Deserialize(argDataPool, n1, ref g);
                    int n3 = n2 + Serializer.Deserialize(argDataPool, n2, ref m);
                    Serializer.Deserialize(argDataPool, n3, ref ad);
                    Character character = Util.getCharacter(cid.ToString());
                    if (character != null) {
                        AvatarData cur = character.GetAvatar(); Util.updateAvatarData(ref cur, ad);
                        cur.InitializeGrowableElementsShowingAbilitiesAndStates(character);
                        character.SetAvatar(cur, context);
                        Util.SetGender(character, g, context); Util.SetTransGender(character, g != cur.GetGender(), context);
                        if (m != character.GetBirthMonth()) {
                            Util.SetBirthMonth(character, m, context);
                            Util.InvokeMethod(character, "OfflineInitializeBaseNeiliProportionOfFiveElements", null);
                            NeiliProportionOfFiveElements prop = (NeiliProportionOfFiveElements)Util.GetPrivateField(character, "_baseNeiliProportionOfFiveElements");
                            character.SetBaseNeiliProportionOfFiveElements(prop, context);
                            character.AddFeature(context, CharacterDomain.GetBirthdayFeatureId(m), true);
                        }
                    }
                    break;
                }
                case 7: case 8: // Set Growth
                {
                    string nameOrId = ""; int val = -1;
                    int next = offset + Serializer.Deserialize(argDataPool, offset, ref nameOrId);
                    Serializer.Deserialize(argDataPool, next, ref val);
                    Character character = Util.getCharacter(nameOrId);
                    if (character != null && val >= 0) if (operation.MethodId == 7) Util.SetCombatSkillQualificationGrowthType(character, (sbyte)val, context); else Util.SetLifeSkillQualificationGrowthType(character, (sbyte)val, context);
                    break;
                }
                case 9: // Set Bisexual
                {
                    string nameOrId = ""; int val = -1;
                    int next = offset + Serializer.Deserialize(argDataPool, offset, ref nameOrId);
                    Serializer.Deserialize(argDataPool, next, ref val);
                    Character character = Util.getCharacter(nameOrId);
                    if (character != null && val >= 0) Util.SetBisexual(character, val != 0, context);
                    break;
                }
                case 10: // Get Bisexual
                {
                    string nameOrId = ""; Serializer.Deserialize(argDataPool, offset, ref nameOrId);
                    Character character = Util.getCharacter(nameOrId);
                    result = Serializer.Serialize(character != null ? (character.GetBisexual() ? 1 : 0) : -1, returnDataPool);
                    break;
                }
                case 11: case 12: // Fame
                {
                    string nameOrId = ""; short fid = -1;
                    int next = offset + Serializer.Deserialize(argDataPool, offset, ref nameOrId);
                    Serializer.Deserialize(argDataPool, next, ref fid);
                    Character character = Util.getCharacter(nameOrId);
                    if (character != null && FameAction.Instance.GetItem(fid) != null) if (operation.MethodId == 11) EventHelper.RecordRoleFameAction(character, fid, -1, 1); else EventHelper.ChangeFameActionDuration(character, fid, int.MinValue, false);
                    break;
                }
                case 13: // Get Advanced Data
                {
                    string nameOrId = ""; Serializer.Deserialize(argDataPool, offset, ref nameOrId);
                    Character character = Util.getCharacter(nameOrId);
                    if (character != null) {
                        result = Serializer.Serialize(character.GetFeatureIds(), returnDataPool);
                        Serializer.Serialize(DomainManager.Character.GetCharacterDisplayData(character.GetId()), returnDataPool);
                    }
                    break;
                }
            }
            return result;
        }
    }

    public static class Util
    {
        public static Character getCharacter(string nameOrId)
        {
            if (string.IsNullOrEmpty(nameOrId)) return null;
            if (int.TryParse(nameOrId, out int id)) { if (DomainManager.Character.TryGetElement_Objects(id, out Character character)) return character; }
            foreach (var character in ((Dictionary<int, Character>)AccessTools.Field(typeof(CharacterDomain), "_objects").GetValue(DomainManager.Character)).Values) {
                try {
                    NameRelatedData data = new NameRelatedData { CharTemplateId = character.GetTemplateId(), FullName = character.GetFullName(), Gender = character.GetGender(), MonasticTitle = character.GetMonasticTitle(), MonkType = character.GetMonkType(), OrgGrade = character.GetOrganizationInfo().Grade, OrgTemplateId = character.GetOrganizationInfo().OrgTemplateId };
                    if (nameOrId.Equals(GetName_UIShow(ref data, character.GetId() == DomainManager.Taiwu.GetTaiwuCharId(), true)) || nameOrId.Equals(GetName_UIShow(ref data, character.GetId() == DomainManager.Taiwu.GetTaiwuCharId(), false))) return character;
                } catch { }
            }
            return null;
        }

        public static void SetGender(Character character, sbyte gender, DataContext context) { SetPrivateField(character, "_gender", gender); InvokeMethod(character, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)3, context }); if (character.CollectionHelperData.IsArchive) unsafe { byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 7U, 1); *ptr = (byte)gender; } }
        public static void SetTransGender(Character character, bool trans, DataContext context) { SetPrivateField(character, "_transgender", trans); InvokeMethod(character, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)13, context }); if (character.CollectionHelperData.IsArchive) unsafe { byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 26U, 1); *ptr = (byte)(trans ? 1 : 0); } }
        public static void SetBirthMonth(Character character, sbyte month, DataContext context) { SetPrivateField(character, "_birthMonth", month); InvokeMethod(character, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)5, context }); if (character.CollectionHelperData.IsArchive) unsafe { byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 10U, 1); *ptr = (byte)month; } }
        public static void SetBisexual(Character character, bool bi, DataContext context) { SetPrivateField(character, "_bisexual", bi); InvokeMethod(character, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)14, context }); if (character.CollectionHelperData.IsArchive) unsafe { byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 27U, 1); *ptr = BitConverter.GetBytes(bi)[0]; } }
        public static void SetCombatSkillQualificationGrowthType(Character character, sbyte type, DataContext context) { SetPrivateField(character, "_combatSkillQualificationGrowthType", type); InvokeMethod(character, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)33, context }); if (character.CollectionHelperData.IsArchive) unsafe { byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 134U, 1); *ptr = (byte)type; } }
        public static void SetLifeSkillQualificationGrowthType(Character character, sbyte type, DataContext context) { SetPrivateField(character, "_lifeSkillQualificationGrowthType", type); InvokeMethod(character, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)31, context }); if (character.CollectionHelperData.IsArchive) unsafe { byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 105U, 1); *ptr = (byte)type; } }

        public static string GetName_UIShow(ref NameRelatedData data, bool isTaiwu, bool getRealName) { var name = internalGetName_UIShow(ref data, isTaiwu, getRealName); string text = (name.Item1 ?? string.Empty) + name.Item2; string text2 = internalGetMonasticTitle(ref data, isTaiwu); return string.IsNullOrEmpty(text2) ? text : text2; }
        private static ValueTuple<string, string> internalGetName_UIShow(ref NameRelatedData data, bool isTaiwu, bool getRealName) { if (data.CharTemplateId < 0) return new ValueTuple<string, string>(null, DomainManager.World.GetElement_CustomTexts(1572)); CharacterItem item = Config.Character.Instance[data.CharTemplateId]; if (!isTaiwu && item.CreatingType != 1) { if (item.CreatingType == 2 && data.FullName.SurnameId < 0 && data.FullName.GivenNameGroupId < 0) return new ValueTuple<string, string>(item.Surname, item.GivenName); if (item.CreatingType != 2) return new ValueTuple<string, string>(item.Surname, item.GivenName); } var name = data.FullName.GetName(data.Gender, DomainManager.World.GetCustomTexts()); if (getRealName) return name; string surname = isTaiwu ? "太吾" : name.Item1; if (!isTaiwu) { short memberId = Organization.Instance[data.OrgTemplateId].Members[(int)data.OrgGrade]; short sId = OrganizationMember.Instance[memberId].SurnameId; if (sId >= 0) surname = LocalSurnames.Instance.SurnameCore[(int)sId].Surname; } return new ValueTuple<string, string>(surname, name.Item2); }
        private static string internalGetMonasticTitle(ref NameRelatedData data, bool isTaiwu) { if (data.MonkType == 0) return null; if ((data.MonkType & 128) > 0) { var titles = LocalMonasticTitles.Instance.MonasticTitles; string s1 = titles[(int)data.MonasticTitle.SeniorityId].Name; string s2 = titles[(int)data.MonasticTitle.SuffixId].Name; short mId = Organization.Instance[data.OrgTemplateId].Members[(int)data.OrgGrade]; string suffix = OrganizationMember.Instance[mId].MonasticTitleSuffixes[(int)data.Gender]; return s1 + s2 + suffix; } var name = internalGetName_UIShow(ref data, isTaiwu, false); string baseName = !string.IsNullOrEmpty(name.Item1) ? name.Item1 : name.Item2; ushort eId = ((data.MonkType & 1) == 0) ? (data.Gender == 1 ? (ushort)991 : (ushort)990) : (data.Gender == 1 ? (ushort)989 : (ushort)988); return baseName + DomainManager.World.GetElement_CustomTexts((int)eId); }

        public static void updateAvatarData(ref AvatarData target, AvatarData reference) { target.AvatarId = reference.AvatarId; target.ColorSkinId = reference.ColorSkinId; target.ColorClothId = reference.ColorClothId; target.Wrinkle1Id = reference.Wrinkle1Id; target.Wrinkle2Id = reference.Wrinkle2Id; target.Wrinkle3Id = reference.Wrinkle3Id; target.EyesLeftId = reference.EyesLeftId; target.EyesRightId = reference.EyesRightId; target.FrontHairId = reference.FrontHairId; target.ColorFrontHairId = reference.ColorFrontHairId; target.BackHairId = reference.BackHairId; target.ColorBackHairId = reference.ColorBackHairId; target.EyebrowId = reference.EyebrowId; target.EyebrowDistancePercent = reference.EyebrowDistancePercent; target.EyebrowHeight = reference.EyebrowHeight; target.EyebrowScale = reference.EyebrowScale; target.EyebrowAngle = reference.EyebrowAngle; target.ColorEyebrowId = reference.ColorEyebrowId; target.EyesMainId = reference.EyesMainId; target.EyesDistancePercent = reference.EyesDistancePercent; target.EyesHeightPercent = reference.EyesHeightPercent; target.EyesScale = reference.EyesScale; target.EyesAngle = reference.EyesAngle; target.ColorEyeballId = reference.ColorEyeballId; target.NoseId = reference.NoseId; target.NoseScale = reference.NoseScale; target.NoseHeightPercent = reference.NoseHeightPercent; target.MouthId = reference.MouthId; target.MouthHeightPercent = reference.MouthHeightPercent; target.MouthScale = reference.MouthScale; target.ColorMouthId = reference.ColorMouthId; target.Feature1Id = reference.Feature1Id; target.Feature2Id = reference.Feature2Id; target.ColorFeature1Id = reference.ColorFeature1Id; target.ColorFeature2Id = reference.ColorFeature2Id; target.Beard1Id = reference.Beard1Id; target.Beard2Id = reference.Beard2Id; target.ColorBeard1Id = reference.ColorBeard1Id; target.ColorBeard2Id = reference.ColorBeard2Id; }
        public static object GetPrivateField(object obj, string fieldName) { var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic); return field?.GetValue(obj); }
        public static void SetPrivateField(object obj, string fieldName, object value) { var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public); field?.SetValue(obj, value); }
        public static object InvokeMethod(object obj, string methodName, object[] args) { var method = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public); return method?.Invoke(obj, args); }
    }

    public struct NameRelatedData { public short CharTemplateId; public FullName FullName; public sbyte Gender; public MonasticTitle MonasticTitle; public sbyte MonkType; public sbyte OrgGrade; public sbyte OrgTemplateId; }
}
