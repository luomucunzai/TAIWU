using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using GameData.Domains;
using GCharacter = GameData.Domains.Character.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Organization;
using GCombatSkill = GameData.Domains.CombatSkill.CombatSkill;
using GameData.Domains.CombatSkill;
using GameData.Common;
using Config;
using System.Reflection;

namespace RealTimeModifyCharacterBackend
{
    public static class XuanNuRenaissancePatch
    {
        private const sbyte XN_ID = 8;
        private const short FID_ZHONG_ZHEN = 164; // 忠贞不渝

        // 1. Blueprint Hijacking: All Xuan Nu members use Grade 0 (1st Rank) config
        [HarmonyPatch(typeof(OrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
        [HarmonyPrefix]
        public static void GetOrgMemberConfig_Prefix(sbyte orgTemplateId, ref sbyte grade)
        {
            if (orgTemplateId == XN_ID) grade = 0;
        }

        // 2. Template Hijacking: Force high-level template for Xuan Nu
        [HarmonyPatch(typeof(OrganizationDomain), "GetCharacterTemplateId", new Type[] { typeof(sbyte), typeof(sbyte), typeof(sbyte) })]
        [HarmonyPrefix]
        public static bool GetCharacterTemplateId_Prefix(sbyte orgTemplateId, ref short __result)
        {
            if (orgTemplateId == XN_ID)
            {
                __result = 373; // 璇女羽衣使
                return false;
            }
            return true;
        }

        // 3. Core Implementation Logic
        [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter", new Type[] { typeof(DataContext), typeof(IntelligentCharacterCreationInfo).MakeByRefType() })]
        [HarmonyPostfix]
        public static unsafe void CreateIntelligentCharacter_Postfix(GCharacter __result, DataContext context)
        {
            if (__result == null) return;

            OrganizationInfo orgInfo = __result.GetOrganizationInfo();
            if (orgInfo.OrgTemplateId == XN_ID)
            {
                int charId = __result.GetId();
                sbyte grade = orgInfo.Grade;

                // --- Gender & Appearance (0 is Female in Taiwu Remake) ---
                if (__result.GetGender() != 0)
                {
                    Util.SetGender(__result, 0, context);
                    __result.GetAvatar()?.ChangeGender(0);
                }
                Util.SetTransGender(__result, false, context);

                // --- Feature: Zhong Zhen Bu Yu (164) ---
                if (!__result.GetFeatureIds().Contains(FID_ZHONG_ZHEN))
                {
                    __result.AddFeature(context, FID_ZHONG_ZHEN, true);
                }

                // --- Attraction & Behavior (Celestial / Benevolent) ---
                // Force base attraction to 900
                AccessTools.Field(typeof(GCharacter), "_baseAttraction").SetValue(__result, (short)900);
                // Set morality to 700 (Benevolent / 仁善)
                __result.SetBaseMorality(700, context);

                // --- Tiered Aptitude Enhancement (1.5x / 1.35x / 1.1x) ---
                float multiplier = (grade == 0) ? 1.5f : (grade == 1 ? 1.35f : 1.1f);
                short combatFloor = (short)(130 * multiplier);
                short lifeFloor = (short)(130 * multiplier);
                short musicFloor = (short)(150 * multiplier);

                // Combat Skill Qualifications
                CombatSkillShorts cQuals = __result.GetBaseCombatSkillQualifications();
                bool cChanged = false;
                fixed (short* pC = &cQuals.Items.FixedElementField)
                {
                    for (int i = 0; i < 14; i++)
                    {
                        if (pC[i] < combatFloor) { pC[i] = combatFloor; cChanged = true; }
                    }
                }
                if (cChanged) __result.SetBaseCombatSkillQualifications(ref cQuals, context);

                // Life Skill Qualifications
                LifeSkillShorts lQuals = __result.GetBaseLifeSkillQualifications();
                bool lChanged = false;
                fixed (short* pL = &lQuals.Items.FixedElementField)
                {
                    if (pL[0] < musicFloor) { pL[0] = musicFloor; lChanged = true; }
                    for (int i = 1; i < 16; i++)
                    {
                        if (pL[i] < lifeFloor) { pL[i] = lifeFloor; lChanged = true; }
                    }
                }
                if (lChanged) __result.SetBaseLifeSkillQualifications(ref lQuals, context);

                // --- Purity & Martial Arts Mastery ---
                __result.SetConsummateLevel(18, context);
                CombatSkillDomain skillDomain = DomainManager.CombatSkill;

                List<short> learnedSkills = __result.GetLearnedCombatSkills();

                foreach (CombatSkillItem skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                {
                    if (skillCfg.SectId == XN_ID)
                    {
                        if (!learnedSkills.Contains(skillCfg.TemplateId))
                        {
                            skillDomain.CreateCombatSkill(charId, skillCfg.TemplateId, 65535);
                            GCombatSkill inst;
                            if (skillDomain.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillCfg.TemplateId), out inst))
                            {
                                inst.SetPracticeLevel(100, context);
                            }
                            // Activate all page rewards (5-14)
                            for (byte pIdx = 5; pIdx <= 14; pIdx++)
                                skillDomain.TryActivateCombatSkillBookPageWhenSetReadingState(context, charId, skillCfg.TemplateId, pIdx);
                        }
                    }
                }

                // --- Force Full Cache Refresh ---
                __result.InvalidateSelfAndInfluencedCache(0, context);
            }
        }
    }
}
