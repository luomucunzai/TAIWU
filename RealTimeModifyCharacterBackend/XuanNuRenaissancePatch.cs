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
        // 1. Blueprint Hijacking: Force Grade 0 for Xuan Nu
        [HarmonyPatch(typeof(OrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
        [HarmonyPrefix]
        public static void GetOrgMemberConfig_Prefix(sbyte orgTemplateId, ref sbyte grade)
        {
            if (orgTemplateId == 8) // Xuan Nu Sect
            {
                grade = 0; // Force Grade 0 (1st Rank)
            }
        }

        // 2. Template Hijacking: Force high-level template for Xuan Nu
        // This determines base attributes like HP and Six-Dims baseline.
        [HarmonyPatch(typeof(OrganizationDomain), "GetCharacterTemplateId", new Type[] { typeof(sbyte), typeof(sbyte), typeof(sbyte) })]
        [HarmonyPrefix]
        public static bool GetCharacterTemplateId_Prefix(sbyte orgTemplateId, ref short __result)
        {
            if (orgTemplateId == 8)
            {
                __result = 373; // 璇女羽衣使 (Feather Robe Envoy - High level template)
                return false; // Skip original method
            }
            return true;
        }

        // 3. Character Generation: Highest Specifications
        [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter", new Type[] { typeof(DataContext), typeof(IntelligentCharacterCreationInfo).MakeByRefType() })]
        [HarmonyPostfix]
        public static unsafe void CreateIntelligentCharacter_Postfix(Character __result, DataContext context)
        {
            if (__result == null) return;

            OrganizationInfo orgInfo = __result.GetOrganizationInfo();
            if (orgInfo.OrgTemplateId == 8) // Xuan Nu Sect
            {
                int charId = __result.GetId();

                // Gender Lock to Female (1)
                if (__result.GetGender() != 1)
                {
                    Util.SetGender(__result, 1, context);
                }

                // Aptitude Floor (Combat 130)
                CombatSkillShorts combatQuals = __result.GetBaseCombatSkillQualifications();
                bool changedCombat = false;
                fixed (short* pQuals = &combatQuals.Items.FixedElementField)
                {
                    for (int i = 0; i < 14; i++)
                    {
                        if (pQuals[i] < 130)
                        {
                            pQuals[i] = 130;
                            changedCombat = true;
                        }
                    }
                }
                if (changedCombat)
                {
                    __result.SetBaseCombatSkillQualifications(ref combatQuals, context);
                }

                // Aptitude Floor (Life Skill: Music 150, others 130)
                LifeSkillShorts lifeQuals = __result.GetBaseLifeSkillQualifications();
                bool changedLife = false;
                fixed (short* pQuals = &lifeQuals.Items.FixedElementField)
                {
                    // Index 0 is Music (音律)
                    if (pQuals[0] < 150)
                    {
                        pQuals[0] = 150;
                        changedLife = true;
                    }
                    for (int i = 1; i < 16; i++)
                    {
                        if (pQuals[i] < 130)
                        {
                            pQuals[i] = 130;
                            changedLife = true;
                        }
                    }
                }
                if (changedLife)
                {
                    __result.SetBaseLifeSkillQualifications(ref lifeQuals, context);
                }

                // Inject 18 Purity (Consummate Level)
                __result.SetConsummateLevel(18, context);

                // Grant all Xuan Nu martial arts and Activate All Rewards (Pages 5-14)
                CombatSkillDomain skillDomain = DomainManager.CombatSkill;
                Dictionary<short, CombatSkill> existingSkills = skillDomain.GetCharCombatSkills(charId);

                foreach (CombatSkillItem skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                {
                    if (skillCfg.SectId == 8)
                    {
                        short skillId = skillCfg.TemplateId;
                        if (!existingSkills.ContainsKey(skillId))
                        {
                            // Create as fully mastered (ReadingState 65535 covers all 15/16 bits)
                            skillDomain.CreateCombatSkill(charId, skillId, 65535);

                            CombatSkill newSkill;
                            if (skillDomain.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillId), out newSkill))
                            {
                                newSkill.SetPracticeLevel(100, context);
                            }

                            // Activate all rewards for pages 5-14
                            // This ensures full Neili bonuses and passive attributes are applied.
                            for (byte pageIdx = 5; pageIdx <= 14; pageIdx++)
                            {
                                skillDomain.TryActivateCombatSkillBookPageWhenSetReadingState(context, charId, skillId, pageIdx);
                            }
                        }
                    }
                }

                // Final Refresh: Force full cache invalidation
                var method = typeof(Character).GetMethod("SetModifiedAndInvalidateInfluencedCache",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                if (method != null)
                {
                    method.Invoke(__result, new object[] { (ushort)0, context });
                }
            }
        }
    }
}
