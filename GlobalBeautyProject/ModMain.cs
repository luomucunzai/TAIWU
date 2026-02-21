using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Organization;
using HarmonyLib;
using TaiwuModdingLib.Core.Plugin;
using Redzen.Random;
using GameData.Domains.Building;
using GameData.Domains.Character.AvatarSystem;
using System.Linq;
using System.Reflection;

namespace GlobalBeautyProject
{
    [PluginConfig("天公不作美", "black_wing & AI Improved", "1.1.4")]
    public class GlobalBeautyProjectMod : TaiwuRemakeHarmonyPlugin
    {
        public static bool isXuanNvModEnabled;
        public static bool isSectEnabled;
        public static bool isVillagerEnabled; // Kept setting for compatibility, but logic will change if requested
        public static bool isFixedBoostEnabled;
        public static int fixedBoostValue = 100;
        public static bool isRangeModeEnabled;
        public static int rangeMin = 600;
        public static int rangeMax = 900;

        public override void Initialize()
        {
            base.Initialize();
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
                DomainManager.Mod.GetSetting(base.ModIdStr, "isxuannvmod", ref isXuanNvModEnabled);
                DomainManager.Mod.GetSetting(base.ModIdStr, "isSect", ref isSectEnabled);
                DomainManager.Mod.GetSetting(base.ModIdStr, "isnoSect", ref isVillagerEnabled);
                DomainManager.Mod.GetSetting(base.ModIdStr, "isFixedCharmValue", ref isFixedBoostEnabled);
                DomainManager.Mod.GetSetting(base.ModIdStr, "isRangeCharm", ref isRangeModeEnabled);

                string s = fixedBoostValue.ToString();
                if (DomainManager.Mod.GetSetting(base.ModIdStr, "addFixedCharmValue", ref s))
                    int.TryParse(s, out fixedBoostValue);

                s = rangeMin.ToString();
                if (DomainManager.Mod.GetSetting(base.ModIdStr, "MinRangeCharm", ref s))
                    int.TryParse(s, out rangeMin);

                s = rangeMax.ToString();
                if (DomainManager.Mod.GetSetting(base.ModIdStr, "MaxRangeCharm", ref s))
                    int.TryParse(s, out rangeMax);
            }
            catch (Exception ex)
            {
                AdaptableLog.Error($"GlobalBeautyProject: LoadModSettings Error: {ex.Message}");
            }
        }

        public static bool ShouldApply(sbyte orgTemplateId)
        {
            // Xuan Nu
            if (orgTemplateId == 8) return isXuanNvModEnabled;

            // Other Sects: 1-15
            if (orgTemplateId >= 1 && orgTemplateId <= 15) return isSectEnabled;

            // Villager range (21-38) check removed per user request: "不要求村庄" (Don't require villages)
            // If the user still wants to toggle it via settings but default is off, we can check isVillagerEnabled
            // but usually this means "don't apply to them".
            if (orgTemplateId >= 21 && orgTemplateId <= 38) return isVillagerEnabled;

            return false;
        }

        public static short CalculateNewAttraction(short currentAttraction, IRandomSource random)
        {
            if (isRangeModeEnabled)
            {
                int min = Math.Min(rangeMin, rangeMax);
                int max = Math.Max(rangeMin, rangeMax);
                return (short)random.Next(min, max + 1);
            }
            else if (isFixedBoostEnabled)
            {
                return (short)Math.Min(900, Math.Max(0, (int)currentAttraction + fixedBoostValue));
            }
            return currentAttraction;
        }

        [HarmonyPatch]
        public static class BeautyHooks
        {
            [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter")]
            [HarmonyPrefix]
            public static void CreateIntelligentCharacter_Prefix(DataContext context, ref IntelligentCharacterCreationInfo info)
            {
                if (ShouldApply(info.OrgInfo.OrgTemplateId))
                {
                    short current = info.BaseAttraction;
                    if (current < 0) current = 350;
                    info.BaseAttraction = CalculateNewAttraction(current, context.Random);
                }
            }

            [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter")]
            [HarmonyPostfix]
            public static void CreateIntelligentCharacter_Postfix(DataContext context, GameData.Domains.Character.Character __result)
            {
                if (__result == null) return;
                if (ShouldApply(__result.GetOrganizationInfo().OrgTemplateId))
                {
                    short targetCharm = __result.GetBaseAttraction();
                    sbyte bodyType = (sbyte)(__result.GetActualAge() < 30 ? 0 : (__result.GetActualAge() < 50 ? 1 : 2));

                    // Force matching face for boosted charm
                    AvatarData newAvatar = AvatarManager.Instance.GetRandomAvatar(context.Random, __result.GetGender(), __result.GetTransgender(), bodyType, targetCharm);
                    __result.SetAvatar(newAvatar, context);

                    // Invalidate Attraction cache (Field ID 1)
                    AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache").Invoke(__result, new object[] { (ushort)1, context });
                }
            }

            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "CalcAttraction")]
            [HarmonyPostfix]
            public static void CalcAttraction_Postfix(GameData.Domains.Character.Character __instance, ref short __result)
            {
                if (ShouldApply(__instance.GetOrganizationInfo().OrgTemplateId))
                {
                    if (isRangeModeEnabled)
                    {
                        int min = Math.Min(rangeMin, rangeMax);
                        int max = Math.Max(rangeMin, rangeMax);
                        if (__result < min) __result = (short)min;
                        else if (__result > max) __result = (short)max;
                    }
                    else if (isFixedBoostEnabled)
                    {
                        __result = (short)Math.Min(900, (int)__result + fixedBoostValue);
                    }
                }
            }

            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "GenerateRecruitCharacterData")]
            [HarmonyPostfix]
            public static void GenerateRecruitCharacterData_Postfix(IRandomSource random, sbyte peopleLevel, BuildingBlockKey blockKey, BuildingBlockData blockData, ref RecruitCharacterData __result)
            {
                if (__result == null) return;

                var settlement = DomainManager.Organization.GetSettlementByLocation(blockKey.GetLocation());
                if (settlement != null && ShouldApply(settlement.GetOrgTemplateId()))
                {
                    __result.BaseAttraction = CalculateNewAttraction(__result.BaseAttraction, random);
                    sbyte bodyType = (sbyte)(__result.Age < 30 ? 0 : (__result.Age < 50 ? 1 : 2));
                    __result.AvatarData = AvatarManager.Instance.GetRandomAvatar(random, __result.Gender, __result.Transgender, bodyType, __result.BaseAttraction);
                    __result.Recalculate();
                }
            }
        }
    }
}
