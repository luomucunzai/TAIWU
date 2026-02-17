using System;
using System.Collections.Generic;
using HarmonyLib;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Organization;
using GameData.Domains.CombatSkill;
using GameData.Common;
using Config;

namespace RealTimeModifyCharacterBackend
{
    public static class XuanNuRenaissancePatch
    {
        // 1. Blueprint Hijacking: All Xuan Nu members use Grade 0 (1st Rank) config
        [HarmonyPatch(typeof(OrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
        [HarmonyPrefix]
        public static void GetOrgMemberConfig_Prefix(sbyte orgTemplateId, ref sbyte grade)
        {
            if (orgTemplateId == 8) // Xuan Nu Sect
            {
                grade = 0; // Force Grade 0 (1st Rank)
            }
        }

        // 2. Character Generation: Inject 18 Purity and Grant All Xuan Nu Martial Arts
        [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter", new Type[] { typeof(DataContext), typeof(IntelligentCharacterCreationInfo).MakeByRefType() })]
        [HarmonyPostfix]
        public static void CreateIntelligentCharacter_Postfix(Character __result, DataContext context)
        {
            if (__result == null) return;

            OrganizationInfo orgInfo = __result.GetOrganizationInfo();
            if (orgInfo.OrgTemplateId == 8) // Xuan Nu Sect
            {
                // Inject 18 Purity (Consummate Level)
                __result.SetConsummateLevel(18, context);

                // Grant all Xuan Nu martial arts
                CombatSkillDomain skillDomain = DomainManager.CombatSkill;

                // Get existing skills to avoid duplicates
                Dictionary<short, CombatSkill> existingSkills = skillDomain.GetCharCombatSkills(__result.GetId());

                foreach (CombatSkillItem skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                {
                    if (skillCfg.SectId == 8)
                    {
                        short skillId = skillCfg.TemplateId;
                        if (!existingSkills.ContainsKey(skillId))
                        {
                            // Create and set as fully mastered
                            // ReadingState 1023 covers 5 outline bits + 5 normal bits
                            CombatSkill skill = skillDomain.CreateCombatSkill(__result.GetId(), skillId, 1023);
                            skill.SetPracticeLevel(100, context);
                        }
                    }
                }
            }
        }
    }
}
