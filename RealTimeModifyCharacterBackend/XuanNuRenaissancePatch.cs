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

        // 2. Character Generation: Highest Specifications
        [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter", new Type[] { typeof(DataContext), typeof(IntelligentCharacterCreationInfo).MakeByRefType() })]
        [HarmonyPostfix]
        public static unsafe void CreateIntelligentCharacter_Postfix(Character __result, DataContext context)
        {
            if (__result == null) return;

            OrganizationInfo orgInfo = __result.GetOrganizationInfo();
            if (orgInfo.OrgTemplateId == 8) // Xuan Nu Sect
            {
                int charId = __result.GetId();

                // Point 2: Gender Lock to Female (1)
                if (__result.GetGender() != 1)
                {
                    Util.SetGender(__result, 1, context);
                }

                // Point 1: Aptitude Floor (130)
                // Use fixed pointer to safely modify the qualifications in place if possible,
                // or modify a copy and set it back.
                CombatSkillShorts qualifications = __result.GetBaseCombatSkillQualifications();
                bool changedQuals = false;
                fixed (short* pQuals = &qualifications.Items.FixedElementField)
                {
                    for (int i = 0; i < 14; i++)
                    {
                        if (pQuals[i] < 130)
                        {
                            pQuals[i] = 130;
                            changedQuals = true;
                        }
                    }
                }
                if (changedQuals)
                {
                    __result.SetBaseCombatSkillQualifications(ref qualifications, context);
                }

                // Point 3: Inject 18 Purity (Consummate Level)
                __result.SetConsummateLevel(18, context);

                // Point 4: Grant all Xuan Nu martial arts and Activate Rewards
                CombatSkillDomain skillDomain = DomainManager.CombatSkill;
                Dictionary<short, CombatSkill> existingSkills = skillDomain.GetCharCombatSkills(charId);

                foreach (CombatSkillItem skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                {
                    if (skillCfg.SectId == 8)
                    {
                        short skillId = skillCfg.TemplateId;
                        if (!existingSkills.ContainsKey(skillId))
                        {
                            // Create as fully mastered (ReadingState 1023)
                            skillDomain.CreateCombatSkill(charId, skillId, 1023);

                            // Set PracticeLevel to 100
                            // In this game's Domain architecture, some objects have setters that handle the domain logic.
                            // Based on Character.cs, SetConsummateLevel handles its own archive/domain logic.
                            // We'll try to find the skill object we just created to set practice level.
                            CombatSkill newSkill;
                            if (skillDomain.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillId), out newSkill))
                            {
                                newSkill.SetPracticeLevel(100, context);
                            }

                            // Activate rewards for normal pages 5-9 (Direct/正练)
                            for (byte pageIdx = 5; pageIdx <= 9; pageIdx++)
                            {
                                skillDomain.TryActivateCombatSkillBookPageWhenSetReadingState(context, charId, skillId, pageIdx);
                            }
                        }
                    }
                }

                // Final Refresh: Invalidate all caches.
                // Using reflection to call the internal method for a deep refresh.
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
