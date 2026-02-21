using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.AvatarSystem.AvatarRes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaiwuModdingLib.Core.Plugin;
using System.Reflection;
using Config;
using Redzen.Random;
using System.Runtime.InteropServices;

namespace XuanNvRenaissance
{
    [PluginConfig("璇女峰文艺复兴", "black_wing", "1.4.5")]
    public class XuanNvRenaissanceMod : TaiwuRemakeHarmonyPlugin
    {
        public const short XuanNvSectId = 8;
        public const sbyte FemaleGenderId = 0;

        public static int globalAgeMin = 16;
        public static int globalAgeMax = 20;
        public static int globalCharmMin = 900;
        public static int globalCharmMax = 900;
        public static int globalPureEssence = 18;
        public static int globalCombatQual = 150;
        public static int globalLifeQual = 150;
        public static int globalMainAttrFloor = 100;
        public static string globalFeatureIds = "164";
        public static string globalSkillIds = "";

        public static bool enableMonthlyEvents = true;
        public static int eventChanceMultiplier = 100;
        public static bool enableColdPoolHeal = true;
        public static bool enableArtisticFavor = true;
        public static bool enableHeavenlyRecruit = true;
        public static bool enablePureEssenceGain = true;

        public static readonly Dictionary<sbyte, int> gradeTargetTemplates = new Dictionary<sbyte, int>();

        public override void Initialize()
        {
            base.Initialize();
            LoadModSettings();
        }

        public override void OnModSettingUpdate() => LoadModSettings();

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

                DomainManager.Mod.GetSetting(base.ModIdStr, "enableMonthlyEvents", ref enableMonthlyEvents);
                DomainManager.Mod.GetSetting(base.ModIdStr, "eventChanceMultiplier", ref eventChanceMultiplier);
                DomainManager.Mod.GetSetting(base.ModIdStr, "enableColdPoolHeal", ref enableColdPoolHeal);
                DomainManager.Mod.GetSetting(base.ModIdStr, "enableArtisticFavor", ref enableArtisticFavor);
                DomainManager.Mod.GetSetting(base.ModIdStr, "enableHeavenlyRecruit", ref enableHeavenlyRecruit);
                DomainManager.Mod.GetSetting(base.ModIdStr, "enablePureEssenceGain", ref enablePureEssenceGain);

                gradeTargetTemplates.Clear();
                for (sbyte i = 0; i < 9; i++)
                {
                    int selectedIndex = i;
                    DomainManager.Mod.GetSetting(base.ModIdStr, $"Grade{i + 1}_TargetTemplate", ref selectedIndex);
                    gradeTargetTemplates[i] = selectedIndex;
                }
            }
            catch (Exception ex)
            {
                AdaptableLog.Error($"XuanNvRenaissance: LoadModSettings Error: {ex.Message}");
            }
        }

        public static sbyte GetWeightedBodyType(IRandomSource random)
        {
            int val = random.Next(100);
            if (val < 25) return (sbyte)0;
            if (val < 85) return (sbyte)1;
            return (sbyte)2;
        }

        public static void EnsureHair(AvatarData avatar, IRandomSource random)
        {
            AvatarGroup group = AvatarManager.Instance.GetAvatarGroup(avatar.AvatarId);
            if (group != null)
            {
                var hairIds = group.GetRandomHairsNoSkinHead(random);
                avatar.FrontHairId = hairIds.frontId;
                avatar.BackHairId = hairIds.backId;
            }
        }

        [HarmonyPatch]
        public static class XuanNuHooks
        {
            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "CalcAttraction")]
            [HarmonyPostfix]
            public static void CalcAttraction_Postfix(GameData.Domains.Character.Character __instance, ref short __result)
            {
                if (__instance.GetOrganizationInfo().OrgTemplateId == XuanNvSectId)
                {
                    if (__result < (short)globalCharmMin) __result = (short)globalCharmMin;
                    if (__result > (short)globalCharmMax && globalCharmMax >= globalCharmMin) __result = (short)globalCharmMax;
                }
            }

            [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter")]
            [HarmonyPostfix]
            public static void CreateIntelligentCharacter_Postfix(DataContext context, GameData.Domains.Character.Character __result)
            {
                if (__result == null || __result.GetOrganizationInfo().OrgTemplateId != XuanNvSectId) return;
                ApplyXuanNvHighSpec(__result, context);
            }

            public static void ApplyXuanNvHighSpec(GameData.Domains.Character.Character character, DataContext context)
            {
                sbyte originalGrade = character.GetOrganizationInfo().Grade;
                sbyte effectiveGrade = originalGrade;
                if (gradeTargetTemplates.TryGetValue(originalGrade, out int mappedGrade)) effectiveGrade = (sbyte)mappedGrade;

                short age = (short)context.Random.Next(globalAgeMin, globalAgeMax + 1);
                Util.SafeInvoke(character, "SetActualAge", new object[] { age, context });
                Util.SafeInvoke(character, "SetCurrAge", new object[] { age, context });

                character.OfflineSetGenderInfo(FemaleGenderId, false);
                Util.SafeInvoke(character, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)3, context });
                Util.SafeInvoke(character, "SetBaseMorality", new object[] { (short)700, context });
                Util.SafeInvoke(character, "SetXiangshuInfection", new object[] { (short)0, context });

                short finalCharm = (short)context.Random.Next(globalCharmMin, globalCharmMax + 1);
                sbyte bodyType = GetWeightedBodyType(context.Random);
                AvatarData newAvatar = AvatarManager.Instance.GetRandomAvatar(context.Random, FemaleGenderId, false, bodyType, finalCharm);
                EnsureHair(newAvatar, context.Random);
                character.SetAvatar(newAvatar, context);
                Util.SafeInvoke(character, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)1, context });

                Util.SafeInvoke(character, "SetHealth", new object[] { character.GetMaxHealth(), context });

                if (globalPureEssence > 0)
                {
                    sbyte level = (sbyte)globalPureEssence;
                    if (character.GetConsummateLevel() < level)
                        Util.SafeInvoke(character, "SetConsummateLevel", new object[] { level, context });
                }

                int combatQualBase = globalCombatQual - effectiveGrade * 10;
                combatQualBase = Math.Max(combatQualBase, effectiveGrade <= 2 ? 85 : 20);

                int lifeQualBase = globalLifeQual - effectiveGrade * 10;
                lifeQualBase = Math.Max(lifeQualBase, effectiveGrade <= 2 ? 85 : 20);

                Util.EnforceQualFloors(character, (short)combatQualBase, (short)lifeQualBase, context);

                if (globalMainAttrFloor > 0)
                {
                    MainAttributes attrs = character.GetBaseMainAttributes();
                    if (Util.EnforceAttributeFloor(ref attrs, (short)globalMainAttrFloor))
                        character.SetBaseMainAttributes(attrs, context);
                }

                if (!string.IsNullOrWhiteSpace(globalFeatureIds))
                {
                    var existing = character.GetFeatureIds();
                    foreach (string idStr in globalFeatureIds.Split(','))
                        if (short.TryParse(idStr.Trim(), out short fid) && !existing.Contains(fid))
                            character.AddFeature(context, fid, true);
                }

                if (string.IsNullOrWhiteSpace(globalSkillIds))
                {
                    foreach (var skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                        if (skillCfg.SectId == XuanNvSectId && skillCfg.Grade >= effectiveGrade)
                            LearnAndMasterSkill(character, skillCfg.TemplateId, context);
                }
                else
                {
                    foreach (string idStr in globalSkillIds.Split(','))
                        if (short.TryParse(idStr.Trim(), out short sid))
                            LearnAndMasterSkill(character, sid, context);
                }

                Util.SafeInvoke(character, "InvalidateAllCaches", new object[] { context });
            }

            private static void LearnAndMasterSkill(GameData.Domains.Character.Character character, short skillId, DataContext context)
            {
                object skill = character.GetLearnedCombatSkills().Contains(skillId)
                    ? Util.SafeInvoke(DomainManager.CombatSkill, "GetElement_CombatSkills", new object[] { new ValueTuple<int, short>(character.GetId(), skillId) })
                    : character.LearnNewCombatSkill(context, skillId, (ushort)32767);

                if (skill != null)
                {
                    Util.SafeInvoke(skill, "SetPracticeLevel", new object[] { (sbyte)100, context });
                    ushort actState = (ushort)(1 | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9));
                    Util.SafeInvoke(skill, "SetActivationState", new object[] { actState, context });
                    for (byte pIdx = 0; pIdx < 15; pIdx++)
                        DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, character.GetId(), skillId, pIdx);
                }
            }
        }

        [HarmonyPatch]
        public static class XuanNvMonthlyEvents
        {
            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "PeriAdvanceMonth_UpdateStatus")]
            [HarmonyPostfix]
            public static void PeriAdvanceMonth_UpdateStatus_Postfix(GameData.Domains.Character.Character __instance, DataContext context)
            {
                if (!enableMonthlyEvents || __instance == null || __instance.GetOrganizationInfo().OrgTemplateId != XuanNvSectId) return;

                IRandomSource random = context.Random;
                int multiplier = Math.Max(0, eventChanceMultiplier);

                if (enableColdPoolHeal && random.CheckPercentProb(10 * multiplier / 100))
                {
                    if (__instance.GetDisorderOfQi() > 0)
                        __instance.ChangeDisorderOfQi(context, -50);
                }

                if (enablePureEssenceGain && random.CheckPercentProb(2 * multiplier / 100) && __instance.GetConsummateLevel() < (sbyte)18)
                {
                    sbyte next = (sbyte)(__instance.GetConsummateLevel() + 1);
                    Util.SafeInvoke(__instance, "SetConsummateLevel", new object[] { next, context });
                }
            }
        }

        public static class Util
        {
            public static object SafeInvoke(object obj, string methodName, object[] args)
            {
                if (obj == null) return null;
                MethodInfo method = null;
                Type type = (obj is Type t) ? t : obj.GetType();
                object target = (obj is Type) ? null : obj;

                method = AccessTools.Method(type, methodName);
                if (method == null) return null;

                ParameterInfo[] parameters = method.GetParameters();
                object[] convertedArgs = new object[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == null)
                    {
                        convertedArgs[i] = null;
                    }
                    else if (i < parameters.Length)
                    {
                        Type paramType = parameters[i].ParameterType;
                        if (paramType.IsInstanceOfType(args[i]))
                        {
                            convertedArgs[i] = args[i];
                        }
                        else
                        {
                            try {
                                convertedArgs[i] = Convert.ChangeType(args[i], paramType);
                            } catch {
                                convertedArgs[i] = args[i];
                            }
                        }
                    }
                    else
                    {
                        convertedArgs[i] = args[i];
                    }
                }
                return method.Invoke(target, convertedArgs);
            }

            public static void EnforceQualFloors(GameData.Domains.Character.Character character, short cFloor, short lFloor, DataContext context)
            {
                var cQuals = character.GetBaseCombatSkillQualifications();
                var lQuals = character.GetBaseLifeSkillQualifications();

                CombatQualsMirror cMirror = ToCombatQualsMirror(cQuals);
                LifeQualsMirror lMirror = ToLifeQualsMirror(lQuals);

                bool cChanged = false;
                for (int i = 0; i < 14; i++) if (cMirror.Items[i] < cFloor) { cMirror.Items[i] = cFloor; cChanged = true; }
                if (cChanged) { cQuals = FromCombatQualsMirror(cMirror); character.SetBaseCombatSkillQualifications(ref cQuals, context); }

                bool lChanged = false;
                for (int i = 0; i < 16; i++) if (lMirror.Items[i] < lFloor) { lMirror.Items[i] = lFloor; lChanged = true; }
                if (lChanged) { lQuals = FromLifeQualsMirror(lMirror); character.SetBaseLifeSkillQualifications(ref lQuals, context); }
            }

            public static bool EnforceAttributeFloor(ref MainAttributes attrs, short floor)
            {
                MainAttributesMirror mirror = ToAttrMirror(attrs);
                bool changed = false;
                if (mirror.Strength < floor) { mirror.Strength = floor; changed = true; }
                if (mirror.Agility < floor) { mirror.Agility = floor; changed = true; }
                if (mirror.Vitality < floor) { mirror.Vitality = floor; changed = true; }
                if (mirror.Intelligence < floor) { mirror.Intelligence = floor; changed = true; }
                if (mirror.Mind < floor) { mirror.Mind = floor; changed = true; }
                if (mirror.Charm < floor) { mirror.Charm = floor; changed = true; }
                if (changed) attrs = FromAttrMirror(mirror);
                return changed;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MainAttributesMirror { public short Strength, Agility, Vitality, Intelligence, Mind, Charm; }

            [StructLayout(LayoutKind.Sequential)]
            public struct CombatQualsMirror { [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)] public short[] Items; }

            [StructLayout(LayoutKind.Sequential)]
            public struct LifeQualsMirror { [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] public short[] Items; }

            private static MainAttributesMirror ToAttrMirror(MainAttributes a) { IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(a)); Marshal.StructureToPtr(a, p, false); var m = (MainAttributesMirror)Marshal.PtrToStructure(p, typeof(MainAttributesMirror)); Marshal.FreeHGlobal(p); return m; }
            private static MainAttributes FromAttrMirror(MainAttributesMirror m) { IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MainAttributes))); Marshal.StructureToPtr(m, p, false); var a = (MainAttributes)Marshal.PtrToStructure(p, typeof(MainAttributes)); Marshal.FreeHGlobal(p); return a; }

            private static CombatQualsMirror ToCombatQualsMirror(CombatSkillShorts q) { IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(q)); Marshal.StructureToPtr(q, p, false); var m = (CombatQualsMirror)Marshal.PtrToStructure(p, typeof(CombatQualsMirror)); Marshal.FreeHGlobal(p); return m; }
            private static CombatSkillShorts FromCombatQualsMirror(CombatQualsMirror m) { IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CombatSkillShorts))); Marshal.StructureToPtr(m, p, false); var q = (CombatSkillShorts)Marshal.PtrToStructure(p, typeof(CombatSkillShorts)); Marshal.FreeHGlobal(p); return q; }

            private static LifeQualsMirror ToLifeQualsMirror(LifeSkillShorts q) { IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(q)); Marshal.StructureToPtr(q, p, false); var m = (LifeQualsMirror)Marshal.PtrToStructure(p, typeof(LifeQualsMirror)); Marshal.FreeHGlobal(p); return m; }
            private static LifeSkillShorts FromLifeQualsMirror(LifeQualsMirror m) { IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LifeSkillShorts))); Marshal.StructureToPtr(m, p, false); var q = (LifeSkillShorts)Marshal.PtrToStructure(p, typeof(LifeSkillShorts)); Marshal.FreeHGlobal(p); return q; }
        }
    }
}
