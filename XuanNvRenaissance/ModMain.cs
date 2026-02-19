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
using GameData.GameDataBridge;
using GameData.Serializer;

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
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCharmMin", ref globalCharmMin);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCharmMax", ref globalCharmMax);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalFeatureIds", ref globalFeatureIds);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalSkillIds", ref globalSkillIds);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCombatQualification", ref globalCombatQualification);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalLifeQualification", ref globalLifeQualification);

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
            // 1. Rank Mapping: Control the config lookup for sect ranks
            [HarmonyPatch(typeof(OrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
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
            [HarmonyPatch(typeof(OrganizationDomain), "GetCharacterTemplateId", new Type[] { typeof(sbyte), typeof(sbyte), typeof(sbyte) })]
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

            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "CalcAttraction")]
            [HarmonyPostfix]
            public static void CalcAttraction_Postfix(GameData.Domains.Character.Character __instance, ref short __result)
            {
                if (__instance.GetOrganizationInfo().OrgTemplateId == XuanNvSectId)
                {
                    if (globalCharmMax > 0)
                        __result = (short)globalCharmMax;
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

                // --- 1. Master Identity ---
                __result.OfflineSetGenderInfo(FemaleGenderId, false);
                Util.InvalidateField(__result, (ushort)3, context); // Gender
                Util.InvalidateField(__result, (ushort)13, context); // Transgender

                AvatarData avatar = __result.GetAvatar();
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
                    CombatSkillShorts cQuals = __result.GetBaseCombatSkillQualifications();
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
                    LifeSkillShorts lQuals = __result.GetBaseLifeSkillQualifications();
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

            private static void LearnAndMasterSkill(GameData.Domains.Character.Character character, short skillId, DataContext context)
            {
                if (character.GetLearnedCombatSkills().Contains(skillId)) return;

                var skill = character.LearnNewCombatSkill(context, skillId, 65535);
                if (skill != null)
                {
                    skill.SetPracticeLevel((sbyte)100, context);
                    for (byte pIdx = 0; pIdx < 15; pIdx++)
                        DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, character.GetId(), skillId, pIdx);
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
                return AccessTools.Method("GameData.GameDataBridge.GameDataBridge:ProcessMethodCall");
            }

            [HarmonyPrefix]
            public static bool ProcessMethodCall_Prefix(Operation operation, RawDataPool argDataPool, DataContext context)
            {
                if (operation.DomainId != 66) return true;

                var notificationCollection = (NotificationCollection)AccessTools.Field(typeof(GameData.GameDataBridge.GameDataBridge), "_pendingNotifications").GetValue(null);

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
                    case 1: // Add Features: (string charNameOrId, List<int> featureIds)
                        {
                            string charNameOrId = "";
                            List<int> fids = new List<int>();
                            int nextOffset = argsOffset + GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId);
                            GameData.Serializer.Serializer.Deserialize(argDataPool, nextOffset, ref fids);

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
                            int nextOffset = argsOffset + GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId);
                            GameData.Serializer.Serializer.Deserialize(argDataPool, nextOffset, ref val);

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
            public static List<GameData.Domains.Character.Character> GetAllXuanNvMembers()
            {
                var members = new List<GameData.Domains.Character.Character>();
                var charDomain = DomainManager.Character;
                var objects = (Dictionary<int, GameData.Domains.Character.Character>)AccessTools.Field(typeof(CharacterDomain), "_objects").GetValue(charDomain);
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

            public static GameData.Domains.Character.Character GetCharacter(string charNameOrId)
            {
                if (string.IsNullOrEmpty(charNameOrId)) return null;
                if (int.TryParse(charNameOrId, out int id))
                {
                    if (DomainManager.Character.TryGetElement_Objects(id, out GameData.Domains.Character.Character character)) return character;
                }
                return null;
            }

            public static void InvalidateField(GameData.Domains.Character.Character character, ushort fieldId, DataContext context)
            {
                AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache").Invoke(character, new object[] { fieldId, context });
            }
        }
    }
}
