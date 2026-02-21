using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaiwuModdingLib.Core.Plugin;
using GameData.Utilities;
using System.Reflection;
using Config;
using GameData.ArchiveData;
using GameData.Domains.Organization;
using Redzen.Random;
using GameData.Domains.Building;
using System.Runtime.InteropServices;

namespace GlobalBeautyProject
{
    [PluginConfig("全门派绝世容颜", "black_wing", "1.4.2")]
    public class GlobalBeautyMod : TaiwuRemakeHarmonyPlugin
    {
        public static int globalCharmMin = 900;
        public static int globalCharmMax = 900;
        public static bool enableVillages = false;
        public static bool syncFaceParts = true;

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
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCharmMin", ref globalCharmMin);
                DomainManager.Mod.GetSetting(base.ModIdStr, "globalCharmMax", ref globalCharmMax);
                DomainManager.Mod.GetSetting(base.ModIdStr, "enableVillages", ref enableVillages);
                DomainManager.Mod.GetSetting(base.ModIdStr, "syncFaceParts", ref syncFaceParts);
            }
            catch (Exception ex)
            {
                AdaptableLog.Error($"GlobalBeautyProject: LoadModSettings Error: {ex.Message}");
            }
        }

        public static sbyte GetWeightedBodyType(IRandomSource random)
        {
            int val = random.Next(100);
            if (val < 25) return 0; // 小体型 -> 25%
            if (val < 85) return 1; // 中体型 -> 60%
            return 2; // 大体型 -> 15%
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
        public static class BeautyHooks
        {
            private static bool ShouldApply(GameData.Domains.Character.Character character)
            {
                if (character == null) return false;
                var orgInfo = character.GetOrganizationInfo();
                sbyte orgId = orgInfo.OrgTemplateId;
                if (orgId == 0) return false; // Skip Taiwu
                if (orgId >= 21 && orgId <= 38) return enableVillages;
                return true; // Sects (1-15) and others
            }

            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "CalcAttraction")]
            [HarmonyPostfix]
            public static void CalcAttraction_Postfix(GameData.Domains.Character.Character __instance, ref short __result)
            {
                if (ShouldApply(__instance))
                {
                    __result = (short)Math.Max((int)__result, globalCharmMin);
                    __result = (short)Math.Min((int)__result, Math.Max(globalCharmMin, globalCharmMax));
                }
            }

            [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter")]
            [HarmonyPostfix]
            public static void CreateIntelligentCharacter_Postfix(DataContext context, GameData.Domains.Character.Character __result)
            {
                if (__result == null || !ShouldApply(__result)) return;

                short finalCharm = (short)context.Random.Next(globalCharmMin, globalCharmMax + 1);
                sbyte bodyType = GetWeightedBodyType(context.Random);

                if (syncFaceParts)
                {
                    AvatarData newAvatar = AvatarManager.Instance.GetRandomAvatar(context.Random, __result.GetGender(), __result.GetTransgender(), bodyType, finalCharm);
                    EnsureHair(newAvatar, context.Random);
                    __result.SetAvatar(newAvatar, context);
                    AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache").Invoke(__result, new object[] { (ushort)1, context });
                }
            }

            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "GenerateRecruitCharacterData")]
            [HarmonyPostfix]
            public static void GenerateRecruitCharacterData_Postfix(IRandomSource random, sbyte peopleLevel, BuildingBlockKey blockKey, BuildingBlockData blockData, ref RecruitCharacterData __result)
            {
                if (__result == null) return;
                var settlement = DomainManager.Organization.GetSettlementByLocation(blockKey.GetLocation());
                if (settlement == null) return;

                sbyte orgId = settlement.GetOrgTemplateId();
                bool apply = (orgId != 0 && (orgId < 21 || orgId > 38 || enableVillages));

                if (apply)
                {
                    short charm = (short)random.Next(globalCharmMin, globalCharmMax + 1);
                    __result.BaseAttraction = charm;

                    if (syncFaceParts)
                    {
                        sbyte bodyType = GetWeightedBodyType(random);
                        __result.AvatarData = AvatarManager.Instance.GetRandomAvatar(random, __result.Gender, __result.Transgender, bodyType, charm);
                        EnsureHair(__result.AvatarData, random);
                    }
                    __result.Recalculate();
                }
            }
        }
    }
}
