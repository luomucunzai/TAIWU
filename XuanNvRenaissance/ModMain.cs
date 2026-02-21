using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Organization;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaiwuModdingLib.Core.Plugin;
using GameData.Utilities;
using System.Reflection;
using GameData.Domains.Item;
using Config;
using GameData.ArchiveData;
using GameData.Domains.Building;
using Redzen.Random;

namespace XuanNvRenaissance
{
    [PluginConfig("璇女峰文艺复兴", "black_wing", "1.2.3")]
    public class XuanNvRenaissanceMod : TaiwuRemakeHarmonyPlugin
    {
        public const short XuanNvSectId = 8;
        public const sbyte FemaleGenderId = 0;

        // Settings variables with defaults
        public static int globalAgeMin = 14;
        public static int globalAgeMax = 30;
        public static int globalCharmMin = 900;
        public static int globalCharmMax = 900;
        public static int globalPureEssence = 18;
        public static int globalCombatQual = 150;
        public static int globalLifeQual = 150;
        public static int globalMainAttrFloor = 100;
        public static string globalFeatureIds = "164"; // 忠贞不渝
        public static string globalSkillIds = "";

        public static readonly Dictionary<sbyte, int> gradeTargetTemplates = new Dictionary<sbyte, int>();

        public override void Initialize()
        {
            base.Initialize(); // Initializes Harmony and patches all
            LoadModSettings();
        }

        public override void OnModSettingUpdate()
        {
            LoadModSettings();
        }

        private void LoadModSettings()
        {
            try
            {
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalAgeMin", ref globalAgeMin);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalAgeMax", ref globalAgeMax);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCharmMin", ref globalCharmMin);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCharmMax", ref globalCharmMax);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalPureEssence", ref globalPureEssence);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCombatQual", ref globalCombatQual);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalLifeQual", ref globalLifeQual);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalMainAttrFloor", ref globalMainAttrFloor);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalFeatureIds", ref globalFeatureIds);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalSkillIds", ref globalSkillIds);

                gradeTargetTemplates.Clear();
                for (sbyte i = 0; i < 9; i++)
                {
                    int selectedIndex = 0; // Default to Rank 1 (index 0)
                    DomainManager.Mod.GetSetting(base.ModIdStr, $"Grade{i + 1}_TargetTemplate", ref selectedIndex);
                    gradeTargetTemplates[i] = selectedIndex;
                }
            }
            catch (Exception ex)
            {
                AdaptableLog.Error($"XuanNvRenaissance: LoadModSettings Error: {ex.Message}");
            }
        }

        [HarmonyPatch]
        public static class XuanNuHooks
        {
            private static int Clamp(int value, int min, int max) => (value < min) ? min : (value > max ? max : value);

            // 1. Rank Mapping: Control the config lookup for sect ranks
            [HarmonyPatch(typeof(OrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
            [HarmonyPrefix]
            public static void GetOrgMemberConfig_Prefix(sbyte orgTemplateId, ref sbyte grade)
            {
                if (orgTemplateId == XuanNvSectId)
                {
                    if (gradeTargetTemplates.TryGetValue(grade, out int mappedGrade))
                        grade = (sbyte)mappedGrade;
                }
            }

            // 2. Persistent Charm Postfix
            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "CalcAttraction")]
            [HarmonyPostfix]
            public static void CalcAttraction_Postfix(GameData.Domains.Character.Character __instance, ref short __result)
            {
                if (__instance.GetOrganizationInfo().OrgTemplateId == XuanNvSectId)
                {
                    __result = (short)Clamp(__result, globalCharmMin, globalCharmMax);
                }
            }

            // 3. Complete Control Over Individual Generation
            [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter")]
            [HarmonyPostfix]
            public static unsafe void CreateIntelligentCharacter_Postfix(DataContext context, GameData.Domains.Character.Character __result)
            {
                if (__result == null) return;

                OrganizationInfo orgInfo = __result.GetOrganizationInfo();
                if (orgInfo.OrgTemplateId != XuanNvSectId) return;

                ApplyXuanNvHighSpec(__result, context);
            }

            // 4. Recruit Character Postfix
            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "GenerateRecruitCharacterData")]
            [HarmonyPostfix]
            public static unsafe void GenerateRecruitCharacterData_Postfix(IRandomSource random, sbyte peopleLevel, BuildingBlockKey blockKey, BuildingBlockData blockData, ref RecruitCharacterData __result)
            {
                if (__result == null) return;

                var settlement = DomainManager.Organization.GetSettlementByLocation(blockKey.GetLocation());
                if (settlement != null && settlement.GetOrgTemplateId() == XuanNvSectId)
                {
                    short age = (short)random.Next(globalAgeMin, globalAgeMax + 1);
                    short charm = (short)random.Next(globalCharmMin, globalCharmMax + 1);

                    __result.Age = age;
                    __result.Gender = FemaleGenderId;
                    __result.Transgender = false;
                    __result.BaseAttraction = charm;

                    // Generate young face
                    sbyte bodyType = (sbyte)(age < 40 ? 0 : (age < 65 ? 1 : 2));
                    __result.AvatarData = AvatarManager.Instance.GetRandomAvatar(random, FemaleGenderId, false, bodyType, charm);

                    sbyte grade = (sbyte)Clamp(peopleLevel - 1, 0, 8);

                    int combatQualBase = Clamp(globalCombatQual - grade * 10, 20, 200);
                    int lifeQualBase = Clamp(globalLifeQual - grade * 10, 20, 200);

                    CombatSkillShorts cQuals = __result.CombatSkillQualifications;
                    short* p = cQuals.Items;
                    for (int i = 0; i < 14; i++)
                        if (p[i] < combatQualBase) p[i] = (short)combatQualBase;
                    __result.CombatSkillQualifications = cQuals;

                    LifeSkillShorts lQuals = __result.LifeSkillQualifications;
                    short* pl = lQuals.Items;
                    for (int i = 0; i < 16; i++)
                        if (pl[i] < lifeQualBase) pl[i] = (short)lifeQualBase;
                    __result.LifeSkillQualifications = lQuals;

                    MainAttributes attrs = __result.MainAttributes;
                    short* pa = attrs.Items;
                    for (int i = 0; i < 6; i++)
                        if (pa[i] < globalMainAttrFloor) pa[i] = (short)globalMainAttrFloor;
                    __result.MainAttributes = attrs;

                    if (!string.IsNullOrWhiteSpace(globalFeatureIds))
                    {
                        if (__result.FeatureIds == null) __result.FeatureIds = new List<short>();
                        foreach (string idStr in globalFeatureIds.Split(','))
                        {
                            if (short.TryParse(idStr.Trim(), out short fid))
                                if (!__result.FeatureIds.Contains(fid)) __result.FeatureIds.Add(fid);
                        }
                    }
                    __result.Recalculate();
                }
            }

            public static unsafe void ApplyXuanNvHighSpec(GameData.Domains.Character.Character character, DataContext context)
            {
                OrganizationInfo orgInfo = character.GetOrganizationInfo();
                sbyte grade = orgInfo.Grade;

                // --- 1. Age ---
                short age = (short)context.Random.Next(globalAgeMin, globalAgeMax + 1);
                character.SetActualAge(age, context);
                character.SetCurrAge(age, context);

                // --- 2. Identity & Morality ---
                character.OfflineSetGenderInfo(FemaleGenderId, false);
                Util.InvalidateField(character, 3, context); // Gender
                character.SetBaseMorality(700, context);

                // --- 3. Charm & Appearance (New Face) ---
                short finalCharm = (short)context.Random.Next(globalCharmMin, globalCharmMax + 1);
                sbyte bodyType = (sbyte)(age < 40 ? 0 : (age < 65 ? 1 : 2));
                AvatarData newAvatar = AvatarManager.Instance.GetRandomAvatar(context.Random, FemaleGenderId, false, bodyType, finalCharm);
                character.SetAvatar(newAvatar, context);
                Util.InvalidateField(character, 1, context); // Attraction

                // --- 4. Health & Injuries (100% Healthy) ---
                Injuries emptyInjuries = default(Injuries);
                emptyInjuries.Initialize();
                character.SetInjuries(emptyInjuries, context);
                character.SetHealth(character.GetMaxHealth(), context);

                // --- 5. Pure Essence ---
                if (globalPureEssence > 0)
                {
                    if (character.GetConsummateLevel() < globalPureEssence)
                        character.SetConsummateLevel((sbyte)globalPureEssence, context);
                }

                // --- 6. Qualifications ---
                int combatQualBase = Clamp(globalCombatQual - grade * 10, 20, 200);
                int lifeQualBase = Clamp(globalLifeQual - grade * 10, 20, 200);

                CombatSkillShorts cQuals = character.GetBaseCombatSkillQualifications();
                short* pc = cQuals.Items;
                bool cChanged = false;
                for (int i = 0; i < 14; i++)
                {
                    if (pc[i] < combatQualBase) { pc[i] = (short)combatQualBase; cChanged = true; }
                }
                if (cChanged) character.SetBaseCombatSkillQualifications(ref cQuals, context);

                LifeSkillShorts lQuals = character.GetBaseLifeSkillQualifications();
                short* pl = lQuals.Items;
                bool lChanged = false;
                for (int i = 0; i < 16; i++)
                {
                    if (pl[i] < lifeQualBase) { pl[i] = (short)lifeQualBase; lChanged = true; }
                }
                if (lChanged) character.SetBaseLifeSkillQualifications(ref lQuals, context);

                // --- 7. Main Attributes ---
                if (globalMainAttrFloor > 0)
                {
                    MainAttributes attrs = character.GetBaseMainAttributes();
                    short* pa = attrs.Items;
                    bool attrChanged = false;
                    for (int i = 0; i < 6; i++)
                        if (pa[i] < globalMainAttrFloor) { pa[i] = (short)globalMainAttrFloor; attrChanged = true; }
                    if (attrChanged) character.SetBaseMainAttributes(attrs, context);
                }

                // --- 8. Features ---
                if (!string.IsNullOrWhiteSpace(globalFeatureIds))
                {
                    foreach (string idStr in globalFeatureIds.Split(','))
                    {
                        if (short.TryParse(idStr.Trim(), out short fid))
                            if (!character.GetFeatureIds().Contains(fid)) character.AddFeature(context, fid, true);
                    }
                }

                // --- 9. Skills ---
                if (string.IsNullOrWhiteSpace(globalSkillIds))
                {
                    foreach (CombatSkillItem skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                    {
                        if (skillCfg.SectId == XuanNvSectId && skillCfg.Grade >= grade)
                            LearnAndMasterSkill(character, skillCfg.TemplateId, context);
                    }
                }
                else
                {
                    foreach (string idStr in globalSkillIds.Split(','))
                    {
                        if (short.TryParse(idStr.Trim(), out short sid))
                            LearnAndMasterSkill(character, sid, context);
                    }
                }

                // Synchronization
                Util.InvokeMethod(character, "InvalidateAllCaches", new object[] { context });
            }

            private static void LearnAndMasterSkill(GameData.Domains.Character.Character character, short skillId, DataContext context)
            {
                var learnedSkills = character.GetLearnedCombatSkills();
                GameData.Domains.CombatSkill.CombatSkill skill = null;

                if (learnedSkills.Contains(skillId))
                {
                    skill = DomainManager.CombatSkill.GetElement_CombatSkills((character.GetId(), skillId));
                    if (skill != null) skill.SetReadingState(32767, context);
                }
                else
                {
                    skill = character.LearnNewCombatSkill(context, skillId, 32767);
                }

                if (skill != null)
                {
                    skill.SetPracticeLevel((sbyte)100, context);

                    ushort actState = 0;
                    actState = CombatSkillStateHelper.SetPageActive(actState, 1); // Benevolent outline
                    for (byte i = 5; i < 10; i++)
                        actState = CombatSkillStateHelper.SetPageActive(actState, i); // Orthodox normal

                    skill.SetActivationState(actState, context);

                    for (byte pIdx = 0; pIdx < 15; pIdx++)
                        DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, character.GetId(), skillId, pIdx);
                }
            }
        }

        [HarmonyPatch]
        public static class RealTimeModifyPatch
        {
            [HarmonyTargetMethod]
            static MethodBase TargetMethod()
            {
                return AccessTools.Method("GameData.GameDataBridge.GameDataBridge:ProcessMethodCall");
            }

            [HarmonyPrefix]
            public static bool ProcessMethodCall_Prefix(Operation operation, RawDataPool argDataPool, DataContext context)
            {
                if (operation.DomainId != 66) return true;

                Type bridgeType = AccessTools.TypeByName("GameData.GameDataBridge.GameDataBridge, GameData");
                var pendingField = AccessTools.Field(bridgeType, "_pendingNotifications");
                NotificationCollection notificationCollection = (NotificationCollection)pendingField.GetValue(null);

                if (!GameData.ArchiveData.Common.IsInWorld()) return true;

                int result = HandleOperation(operation, argDataPool, notificationCollection.DataPool, context);
                if (result >= 0)
                {
                    notificationCollection.Notifications.Add(Notification.CreateMethodReturn(operation.ListenerId, operation.DomainId, operation.MethodId, result));
                }
                return false;
            }

            private static int HandleOperation(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext dataContext)
            {
                int result = -1;
                int argsOffset = operation.ArgsOffset;
                switch (operation.MethodId)
                {
                    case 1: // Add Features
                        {
                            string charNameOrId = "";
                            List<int> fids = new List<int>();
                            int nextOffset = argsOffset + GameDataSerializer.Deserialize(argDataPool, argsOffset, ref charNameOrId);
                            GameDataSerializer.Deserialize(argDataPool, nextOffset, ref fids);

                            var character = Util.GetCharacter(charNameOrId);
                            if (character != null)
                            {
                                foreach (int fid in fids) character.AddFeature(dataContext, (short)fid, true);
                            }
                        }
                        break;
                    case 3: // Set Morality
                        {
                            string charNameOrId = "";
                            int val = 0;
                            int nextOffset = argsOffset + GameDataSerializer.Deserialize(argDataPool, argsOffset, ref charNameOrId);
                            GameDataSerializer.Deserialize(argDataPool, nextOffset, ref val);

                            var character = Util.GetCharacter(charNameOrId);
                            if (character != null) character.SetBaseMorality((short)val, dataContext);
                        }
                        break;
                }
                return result;
            }
        }

        private static class GameDataSerializer
        {
            private static MethodInfo _deserializeMethod_string;
            private static MethodInfo _deserializeMethod_list_int;
            private static MethodInfo _deserializeMethod_int;

            private static Type SerializerType => AccessTools.TypeByName("GameData.Serializer.Serializer, GameData");

            public static int Deserialize(RawDataPool dataPool, int offset, ref string value)
            {
                if (_deserializeMethod_string == null)
                    _deserializeMethod_string = AccessTools.Method(SerializerType, "Deserialize", new Type[] { typeof(RawDataPool), typeof(int), typeof(string).MakeByRefType() });

                object[] args = new object[] { dataPool, offset, value };
                int result = (int)_deserializeMethod_string.Invoke(null, args);
                value = (string)args[2];
                return result;
            }

            public static int Deserialize(RawDataPool dataPool, int offset, ref List<int> value)
            {
                if (_deserializeMethod_list_int == null)
                    _deserializeMethod_list_int = AccessTools.Method(SerializerType, "Deserialize", new Type[] { typeof(RawDataPool), typeof(int), typeof(List<int>).MakeByRefType() });

                object[] args = new object[] { dataPool, offset, value };
                int result = (int)_deserializeMethod_list_int.Invoke(null, args);
                value = (List<int>)args[2];
                return result;
            }

            public static int Deserialize(RawDataPool dataPool, int offset, ref int value)
            {
                if (_deserializeMethod_int == null)
                    _deserializeMethod_int = AccessTools.Method(SerializerType, "Deserialize", new Type[] { typeof(RawDataPool), typeof(int), typeof(int).MakeByRefType() });

                object[] args = new object[] { dataPool, offset, value };
                int result = (int)_deserializeMethod_int.Invoke(null, args);
                value = (int)args[2];
                return result;
            }
        }

        public static class Util
        {
            public static GameData.Domains.Character.Character GetCharacter(string charNameOrId)
            {
                if (string.IsNullOrEmpty(charNameOrId)) return null;
                if (int.TryParse(charNameOrId, out int id))
                {
                    if (DomainManager.Character.TryGetElement_Objects(id, out GameData.Domains.Character.Character character)) return character;
                }
                return null;
            }

            public static object InvokeMethod(object obj, string methodName, object[] args)
            {
                var method = AccessTools.Method(obj.GetType(), methodName);
                return method?.Invoke(obj, args);
            }

            public static void InvalidateField(GameData.Domains.Character.Character character, ushort fieldId, DataContext context)
            {
                AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache").Invoke(character, new object[] { (ushort)fieldId, context });
            }
        }
    }
}
