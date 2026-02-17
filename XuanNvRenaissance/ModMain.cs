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

// Resolve ambiguity
using GCharacter = GameData.Domains.Character.Character;

namespace XuanNvMemberUpgrade.FinalFramework
{
    [PluginConfig("璇女峰文艺复兴", "black_wing", "12.0.9_FinalFix")]
    public class XuanNvMemberUpgradeMod : TaiwuRemakePlugin
    {
        private const short XuanNvSectId = 8;
        private const sbyte FemaleGenderId = 0;

        private static Harmony harmony;

        private static int globalCharmMin;
        private static int globalCharmMax;
        private static string globalFeatureIds;
        private static string globalSkillIds;
        private static int globalCombatQualification;
        private static int globalLifeQualification;

        private static readonly Dictionary<sbyte, int> gradeTargetTemplates = new Dictionary<sbyte, int>();

        public override void Initialize()
        {
            harmony = Harmony.CreateAndPatchAll(typeof(XuanNuRenaissanceHooks));
            LoadModSettings();
        }

        public override void Dispose() { harmony?.UnpatchSelf(); }

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
            } catch { }
        }

        [HarmonyPatch]
        public static class XuanNuRenaissanceHooks
        {
            [HarmonyPatch(typeof(OrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
            [HarmonyPrefix]
            public static void GetOrgMemberConfig_Prefix(sbyte orgTemplateId, ref sbyte grade)
            {
                if (orgTemplateId == XuanNvSectId)
                {
                    if (gradeTargetTemplates.TryGetValue(grade, out int mappedGrade))
                        grade = (sbyte)mappedGrade;
                    else
                        grade = 0;
                }
            }

            [HarmonyPatch(typeof(OrganizationDomain), "GetCharacterTemplateId", new Type[] { typeof(sbyte), typeof(sbyte), typeof(sbyte) })]
            [HarmonyPrefix]
            public static bool GetCharacterTemplateId_Prefix(sbyte orgTemplateId, ref short __result)
            {
                if (orgTemplateId == XuanNvSectId)
                {
                    __result = 373;
                    return false;
                }
                return true;
            }

            [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter")]
            [HarmonyPostfix]
            public static unsafe void CreateIntelligentCharacter_Postfix(DataContext context, GCharacter __result)
            {
                if (__result == null) return;

                OrganizationInfo orgInfo = __result.GetOrganizationInfo();
                if (orgInfo.OrgTemplateId != XuanNvSectId) return;

                // Identity
                SetPrivateField(__result, "_gender", FemaleGenderId);
                SetPrivateField(__result, "_transgender", false);
                var avatar = __result.GetAvatar();
                avatar.ChangeGender(FemaleGenderId);
                __result.SetAvatar(avatar, context);
                __result.SetBaseMorality(700, context);

                // Features
                if (!string.IsNullOrWhiteSpace(globalFeatureIds))
                {
                    foreach (string idStr in globalFeatureIds.Split(','))
                        if (short.TryParse(idStr.Trim(), out short fid))
                            if (!__result.GetFeatureIds().Contains(fid))
                                __result.AddFeature(context, fid, true);
                }

                // Charm
                int minCharm = Math.Min(globalCharmMin, globalCharmMax);
                int maxCharm = Math.Max(globalCharmMin, globalCharmMax);
                if (maxCharm > 0)
                {
                    short finalCharm = (short)context.Random.Next(minCharm, maxCharm + 1);
                    SetPrivateField(__result, "_baseAttraction", finalCharm);
                }

                // Aptitude - FIXED POINTER LOGIC
                if (globalCombatQualification > 0)
                {
                    CombatSkillShorts cQuals = __result.GetBaseCombatSkillQualifications();
                    bool changed = false;
                    // cQuals.Items is a fixed buffer (short*)
                    for (int i = 0; i < 14; i++)
                    {
                        if (cQuals.Items[i] < globalCombatQualification)
                        {
                            cQuals.Items[i] = (short)globalCombatQualification;
                            changed = true;
                        }
                    }
                    if (changed) __result.SetBaseCombatSkillQualifications(ref cQuals, context);
                }

                if (globalLifeQualification > 0)
                {
                    LifeSkillShorts lQuals = __result.GetBaseLifeSkillQualifications();
                    bool changed = false;
                    // lQuals.Items is a fixed buffer (short*)
                    if (lQuals.Items[0] < globalLifeQualification) { lQuals.Items[0] = (short)globalLifeQualification; changed = true; }
                    for (int i = 1; i < 16; i++)
                    {
                        if (lQuals.Items[i] < globalLifeQualification)
                        {
                            lQuals.Items[i] = (short)globalLifeQualification;
                            changed = true;
                        }
                    }
                    if (changed) __result.SetBaseLifeSkillQualifications(ref lQuals, context);
                }

                // Skills
                __result.SetConsummateLevel(18, context);
                if (string.IsNullOrWhiteSpace(globalSkillIds))
                {
                    foreach (CombatSkillItem skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                        if (skillCfg.SectId == XuanNvSectId) LearnAndMasterSkill(__result, skillCfg.TemplateId, context);
                }
                else
                {
                    foreach (string idStr in globalSkillIds.Split(','))
                        if (short.TryParse(idStr.Trim(), out short sid)) LearnAndMasterSkill(__result, sid, context);
                }

                // Refresh
                InvokeMethod(__result, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)0, context });
            }

            private static void LearnAndMasterSkill(GCharacter character, short skillId, DataContext context)
            {
                if (character.GetLearnedCombatSkills().Contains(skillId)) return;
                var skill = character.LearnNewCombatSkill(context, skillId, 65535);
                if (skill != null)
                {
                    InvokeMethod(skill, "SetPracticeLevel", new object[] { (sbyte)100, context });
                    for (byte pIdx = 5; pIdx <= 14; pIdx++)
                        DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, character.GetId(), skillId, pIdx);
                }
            }

            private static void SetPrivateField(object obj, string fieldName, object value)
            {
                var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                field?.SetValue(obj, value);
            }

            private static object InvokeMethod(object obj, string methodName, object[] args)
            {
                var method = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                return method?.Invoke(obj, args);
            }
        }
    }
}
