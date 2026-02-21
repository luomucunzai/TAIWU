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

namespace GlobalBeautyProject
{
    [PluginConfig("天公不作美", "black_wing & AI Improved", "1.1.0")]
    public class GlobalBeautyProjectMod : TaiwuRemakeHarmonyPlugin
    {
        public static bool isXuanNvModEnabled;
        public static bool isSectEnabled;
        public static bool isVillagerEnabled;
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

                DomainManager.Mod.GetSetting(base.ModIdStr, "addFixedCharmValue", ref fixedBoostValue);
                DomainManager.Mod.GetSetting(base.ModIdStr, "MinRangeCharm", ref rangeMin);
                DomainManager.Mod.GetSetting(base.ModIdStr, "MaxRangeCharm", ref rangeMax);
            }
            catch (Exception ex)
            {
                AdaptableLog.Error($"GlobalBeautyProject: LoadModSettings Error: {ex.Message}");
            }
        }

        public static bool ShouldApply(sbyte orgTemplateId)
        {
            if (orgTemplateId == 8) return isXuanNvModEnabled;
            if (OrganizationDomain.IsSect(orgTemplateId)) return isSectEnabled;
            return isVillagerEnabled;
        }

        public static short CalculateNewAttraction(short currentAttraction, IRandomSource random)
        {
            short val = currentAttraction;
            if (isRangeModeEnabled)
            {
                int min = Math.Min(rangeMin, rangeMax);
                int max = Math.Max(rangeMin, rangeMax);
                val = (short)random.Next(min, max + 1);
            }
            else if (isFixedBoostEnabled)
            {
                val = (short)(currentAttraction + fixedBoostValue);
            }
            return (short)Math.Min(900, Math.Max(0, (int)val));
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
                    // If BaseAttraction is < 0, the game would normally generate it.
                    // We force it here so the game uses our value for avatar generation too.
                    short current = info.BaseAttraction;
                    if (current < 0) current = 350; // Use a middle value as base if not specified

                    info.BaseAttraction = CalculateNewAttraction(current, context.Random);
                }
            }

            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "CalcAttraction")]
            [HarmonyPostfix]
            public static void CalcAttraction_Postfix(GameData.Domains.Character.Character __instance, ref short __result)
            {
                if (ShouldApply(__instance.GetOrganizationInfo().OrgTemplateId))
                {
                    // Ensure the charm stays high even if factors like clothing change
                    if (isRangeModeEnabled)
                    {
                        int min = Math.Min(rangeMin, rangeMax);
                        int max = Math.Max(rangeMin, rangeMax);
                        if (__result < min) __result = (short)min;
                        if (__result > max) __result = (short)max;
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
                    // RecruitCharacterData also has an AvatarData which we might want to adjust,
                    // but the game usually generates it from BaseAttraction later if we modify it here.
                }
            }
        }
    }
}
