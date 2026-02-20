extern alias GameDataBackend;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaiwuModdingLib.Core.Plugin;
using System.Reflection;
using Config;
using GBridge = GameDataBackend::GameData.GameDataBridge.GameDataBridge;
using GSerializer = GameDataBackend::GameData.Serializer.Serializer;
using GDataContext = GameDataBackend::GameData.Common.DataContext;
using GRawDataPool = GameDataBackend::GameData.Utilities.RawDataPool;
using GCharacter = GameDataBackend::GameData.Domains.Character.Character;
using GCharacterDomain = GameDataBackend::GameData.Domains.Character.CharacterDomain;
using GOrganizationDomain = GameDataBackend::GameData.Domains.Organization.OrganizationDomain;
using GOrganizationInfo = GameDataBackend::GameData.Domains.Organization.OrganizationInfo;
using GAvatarData = GameDataBackend::GameData.Domains.Character.AvatarSystem.AvatarData;
using GCombatSkillShorts = GameDataBackend::GameData.Domains.Character.CombatSkillShorts;
using GLifeSkillShorts = GameDataBackend::GameData.Domains.Character.LifeSkillShorts;

namespace XuanNvRenaissance
{
    [PluginConfig("璇女峰文艺复兴", "black_wing", "1.0.0")]
    public class XuanNvRenaissanceMod : TaiwuRemakeHarmonyPlugin
    {
        public const short XuanNvSectId = 8;
        public const sbyte FemaleGenderId = 0;

        // Settings variables with defaults
        public static int globalCharmMin = 900;
        public static int globalCharmMax = 900;
        public static string globalFeatureIds = "164"; // 忠贞不渝
        public static string globalSkillIds = "";
        public static int globalCombatQualification = 130;
        public static int globalLifeQualification = 130;

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
                GameDataBackend::GameData.Domains.DomainManager.Mod.GetSetting(base.ModIdStr, "globalCharmMin", ref globalCharmMin);
                GameDataBackend::GameData.Domains.DomainManager.Mod.GetSetting(base.ModIdStr, "globalCharmMax", ref globalCharmMax);
                GameDataBackend::GameData.Domains.DomainManager.Mod.GetSetting(base.ModIdStr, "globalFeatureIds", ref globalFeatureIds);
                GameDataBackend::GameData.Domains.DomainManager.Mod.GetSetting(base.ModIdStr, "globalSkillIds", ref globalSkillIds);
                GameDataBackend::GameData.Domains.DomainManager.Mod.GetSetting(base.ModIdStr, "globalCombatQualification", ref globalCombatQualification);
                GameDataBackend::GameData.Domains.DomainManager.Mod.GetSetting(base.ModIdStr, "globalLifeQualification", ref globalLifeQualification);

                gradeTargetTemplates.Clear();
                for (sbyte i = 0; i < 9; i++)
                {
                    int selectedIndex = 0; // Default to Rank 1 (index 0)
                    GameDataBackend::GameData.Domains.DomainManager.Mod.GetSetting(base.ModIdStr, $"Grade{i + 1}_TargetTemplate", ref selectedIndex);
                    gradeTargetTemplates[i] = selectedIndex;
                }
            }
            catch (Exception ex)
            {
                GameDataBackend::GameData.Utilities.AdaptableLog.Error($"XuanNvRenaissance: LoadModSettings Error: {ex.Message}");
            }
        }

        [HarmonyPatch]
        public static class XuanNuHooks
        {
            // 1. Rank Mapping: Control the config lookup for sect ranks
            [HarmonyPatch(typeof(GOrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
            [HarmonyPrefix]
            public static void GetOrgMemberConfig_Prefix(sbyte orgTemplateId, ref sbyte grade)
            {
                if (orgTemplateId == XuanNvSectId)
                {
                    if (gradeTargetTemplates.TryGetValue(grade, out int mappedGrade))
                        grade = (sbyte)mappedGrade;
                    else
                        grade = 0; // Default to Rank 1
                }
            }

            // 2. Template Forcing: Master the base statistics blueprint
            [HarmonyPatch(typeof(GOrganizationDomain), "GetCharacterTemplateId", new Type[] { typeof(sbyte), typeof(sbyte), typeof(sbyte) })]
            [HarmonyPrefix]
            public static bool GetCharacterTemplateId_Prefix(sbyte orgTemplateId, ref short __result)
            {
                if (orgTemplateId == XuanNvSectId)
                {
                    __result = 373; // 璇女羽衣使 (High specification template)
                    return false;
                }
                return true;
            }

            [HarmonyPatch(typeof(GCharacter), "CalcAttraction")]
            [HarmonyPostfix]
            public static void CalcAttraction_Postfix(GCharacter __instance, ref short __result)
            {
                if (__instance.GetOrganizationInfo().OrgTemplateId == XuanNvSectId)
                {
                    if (globalCharmMax > 0)
                        __result = (short)globalCharmMax;
                }
            }

            // 3. Complete Control Over Individual Generation
            [HarmonyPatch(typeof(GCharacterDomain), "CreateIntelligentCharacter")]
            [HarmonyPostfix]
            public static unsafe void CreateIntelligentCharacter_Postfix(GDataContext context, GCharacter __result)
            {
                if (__result == null) return;

                GOrganizationInfo orgInfo = __result.GetOrganizationInfo();
                if (orgInfo.OrgTemplateId != XuanNvSectId) return;

                // --- 1. Master Identity ---
                __result.OfflineSetGenderInfo(FemaleGenderId, false);
                Util.InvalidateField(__result, (ushort)3, context); // Gender
                Util.InvalidateField(__result, (ushort)13, context); // Transgender

                GAvatarData avatar = __result.GetAvatar();
                avatar.ChangeGender(FemaleGenderId);
                __result.SetAvatar(avatar, context);

                // Morality: Force Benevolent (700)
                __result.SetBaseMorality(700, context);

                // --- 2. Master Features ---
                if (!string.IsNullOrWhiteSpace(globalFeatureIds))
                {
                    foreach (string idStr in globalFeatureIds.Split(','))
                    {
                        if (short.TryParse(idStr.Trim(), out short fid))
                        {
                            if (!__result.GetFeatureIds().Contains(fid))
                                __result.AddFeature(context, fid, true);
                        }
                    }
                }

                // --- 3. Master Aptitudes (Floor Logic) ---
                if (globalCombatQualification > 0)
                {
                    GCombatSkillShorts cQuals = __result.GetBaseCombatSkillQualifications();
                    bool changed = false;
                    short* pItems = cQuals.Items;
                    for (int i = 0; i < 14; i++)
                    {
                        if (pItems[i] < globalCombatQualification)
                        {
                            pItems[i] = (short)globalCombatQualification;
                            changed = true;
                        }
                    }
                    if (changed)
                    {
                        __result.SetBaseCombatSkillQualifications(ref cQuals, context);
                    }
                }

                if (globalLifeQualification > 0)
                {
                    GLifeSkillShorts lQuals = __result.GetBaseLifeSkillQualifications();
                    bool changed = false;
                    short* pItems = lQuals.Items;
                    for (int i = 0; i < 16; i++)
                    {
                        if (pItems[i] < globalLifeQualification)
                        {
                            pItems[i] = (short)globalLifeQualification;
                            changed = true;
                        }
                    }
                    if (changed)
                    {
                        __result.SetBaseLifeSkillQualifications(ref lQuals, context);
                    }
                }

                // --- 4. Master Skills ---
                __result.SetConsummateLevel(18, context);

                if (string.IsNullOrWhiteSpace(globalSkillIds))
                {
                    foreach (CombatSkillItem skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                    {
                        if (skillCfg.SectId == XuanNvSectId)
                            LearnAndMasterSkill(__result, skillCfg.TemplateId, context);
                    }
                }
                else
                {
                    foreach (string idStr in globalSkillIds.Split(','))
                    {
                        if (short.TryParse(idStr.Trim(), out short sid))
                            LearnAndMasterSkill(__result, sid, context);
                    }
                }

                // Force Invalidation to apply all changes
                Util.InvalidateField(__result, (ushort)1, context); // Attraction
            }

            private static void LearnAndMasterSkill(GCharacter character, short skillId, GDataContext context)
            {
                if (character.GetLearnedCombatSkills().Contains(skillId)) return;

                var skill = character.LearnNewCombatSkill(context, skillId, 65535);
                if (skill != null)
                {
                    skill.SetPracticeLevel((sbyte)100, context);
                    for (byte pIdx = 0; pIdx < 15; pIdx++)
                        GameDataBackend::GameData.Domains.DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, character.GetId(), skillId, pIdx);
                }
            }
        }

        // Domain 66 Patch for Real-Time modification calls
        [HarmonyPatch]
        public static class RealTimeModifyPatch
        {
            [HarmonyTargetMethod]
            static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(GBridge), "ProcessMethodCall");
            }

            [HarmonyPrefix]
            public static bool ProcessMethodCall_Prefix(GameDataBackend::GameData.GameDataBridge.Operation operation, GRawDataPool argDataPool, GDataContext context)
            {
                if (operation.DomainId != 66) return true;

                var notificationCollection = (GameDataBackend::GameData.GameDataBridge.NotificationCollection)AccessTools.Field(typeof(GBridge), "_pendingNotifications").GetValue(null);

                if (!GameDataBackend::GameData.ArchiveData.Common.IsInWorld()) return true;

                int result = HandleOperation(operation, argDataPool, notificationCollection.DataPool, context);
                if (result >= 0)
                {
                    notificationCollection.Notifications.Add(GameDataBackend::GameData.GameDataBridge.Notification.CreateMethodReturn(operation.ListenerId, operation.DomainId, operation.MethodId, result));
                }
                return false;
            }

            private static int HandleOperation(GameDataBackend::GameData.GameDataBridge.Operation operation, GRawDataPool argDataPool, GRawDataPool returnDataPool, GDataContext dataContext)
            {
                int result = -1;
                int argsOffset = operation.ArgsOffset;
                switch (operation.MethodId)
                {
                    case 1: // Add Features: (string charNameOrId, List<int> featureIds)
                        {
                            string charNameOrId = "";
                            List<int> fids = new List<int>();
                            int nextOffset = argsOffset + GSerializer.Deserialize(argDataPool, argsOffset, ref charNameOrId);
                            GSerializer.Deserialize(argDataPool, nextOffset, ref fids);

                            var character = Util.GetCharacter(charNameOrId);
                            if (character != null)
                            {
                                foreach (int fid in fids) character.AddFeature(dataContext, (short)fid, true);
                            }
                        }
                        break;
                    case 3: // Set Morality: (string charNameOrId, int value)
                        {
                            string charNameOrId = "";
                            int val = 0;
                            int nextOffset = argsOffset + GSerializer.Deserialize(argDataPool, argsOffset, ref charNameOrId);
                            GSerializer.Deserialize(argDataPool, nextOffset, ref val);

                            var character = Util.GetCharacter(charNameOrId);
                            if (character != null) character.SetBaseMorality((short)val, dataContext);
                        }
                        break;
                }
                return result;
            }
        }

        public static class Util
        {
            public static List<GCharacter> GetAllXuanNvMembers()
            {
                var members = new List<GCharacter>();
                var charDomain = GameDataBackend::GameData.Domains.DomainManager.Character;
                var objects = (Dictionary<int, GCharacter>)AccessTools.Field(typeof(GCharacterDomain), "_objects").GetValue(charDomain);
                if (objects != null)
                {
                    foreach (var character in objects.Values)
                    {
                        if (character.GetOrganizationInfo().OrgTemplateId == XuanNvSectId)
                        {
                            members.Add(character);
                        }
                    }
                }
                return members;
            }

            public static GCharacter GetCharacter(string charNameOrId)
            {
                if (string.IsNullOrEmpty(charNameOrId)) return null;
                if (int.TryParse(charNameOrId, out int id))
                {
                    if (GameDataBackend::GameData.Domains.DomainManager.Character.TryGetElement_Objects(id, out GCharacter character)) return character;
                }
                return null;
            }

            public static void InvalidateField(GCharacter character, ushort fieldId, GDataContext context)
            {
                AccessTools.Method(typeof(GCharacter), "SetModifiedAndInvalidateInfluencedCache").Invoke(character, new object[] { fieldId, context });
            }
        }
    }
}
