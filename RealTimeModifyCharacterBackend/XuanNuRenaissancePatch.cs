using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Organization;
using GameData.Domains.CombatSkill;
using GameData.Common;
using Config;
using System.Reflection;
using GameData.ArchiveData;

namespace RealTimeModifyCharacterBackend
{
    public static class XuanNuRenaissancePatch
    {
        private const sbyte XN_ID = 8;
        private const short FID_ZHONG_ZHEN = 164; // 忠贞不渝

        // 1. Blueprint Hijacking: Force Grade 0 (1st Rank) config for all Xuan Nu
        [HarmonyPatch(typeof(OrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
        [HarmonyPrefix]
        public static void GetOrgMemberConfig_Prefix(sbyte orgTemplateId, ref sbyte grade)
        {
            if (orgTemplateId == XN_ID) grade = 0;
        }

        // 2. Template Hijacking: Force high-level template (373) for Xuan Nu base stats
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

        // 3. Core Implementation Logic: "The Ultimate Specification"
        [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter")]
        [HarmonyPostfix]
        public static unsafe void CreateIntelligentCharacter_Postfix(GameData.Domains.Character.Character __result, DataContext context)
        {
            if (__result == null) return;

            OrganizationInfo orgInfo = __result.GetOrganizationInfo();
            if (orgInfo.OrgTemplateId == XN_ID)
            {
                int charId = __result.GetId();
                sbyte grade = orgInfo.Grade;

                // --- 1. Identity & Gender (0 is Female) ---
                SetGender(__result, (sbyte)0, context);
                SetTransGender(__result, false, context);
                var avatar = __result.GetAvatar();
                avatar.ChangeGender(0);
                __result.SetAvatar(avatar, context);

                // --- 2. Features & Personality ---
                if (!__result.GetFeatureIds().Contains(FID_ZHONG_ZHEN))
                {
                    __result.AddFeature(context, FID_ZHONG_ZHEN, true);
                }
                // Attraction 900 (Celestial)
                SetPrivateField(__result, "_baseAttraction", (short)900);
                // Morality 700 (Benevolent)
                __result.SetBaseMorality(700, context);

                // --- 3. Aptitude Enhancement (Tiered Scaling) ---
                float multiplier = (grade == 0) ? 1.5f : (grade == 1 ? 1.35f : 1.1f);
                short combatFloor = (short)(130 * multiplier);
                short lifeFloor = (short)(130 * multiplier);
                short musicFloor = (short)(150 * multiplier);

                // Combat Aptitude
                // Note: GetBaseCombatSkillQualifications returns ref CombatSkillShorts.
                // In Taiwu Remake, Items is typically a pointer (short*) when decompiled or accessed in unsafe context.
                // The error "already fixed" confirms it is treated as a fixed address/pointer already.
                CombatSkillShorts cQuals = __result.GetBaseCombatSkillQualifications();
                bool cChanged = false;
                short* pC = cQuals.Items;
                for (int i = 0; i < 14; i++)
                {
                    if (pC[i] < combatFloor) { pC[i] = combatFloor; cChanged = true; }
                }
                if (cChanged) __result.SetBaseCombatSkillQualifications(ref cQuals, context);

                // Life Aptitude
                LifeSkillShorts lQuals = __result.GetBaseLifeSkillQualifications();
                bool lChanged = false;
                short* pL = lQuals.Items;
                // Music is index 0
                if (pL[0] < musicFloor) { pL[0] = musicFloor; lChanged = true; }
                for (int i = 1; i < 16; i++)
                {
                    if (pL[i] < lifeFloor) { pL[i] = lifeFloor; lChanged = true; }
                }
                if (lChanged) __result.SetBaseLifeSkillQualifications(ref lQuals, context);

                // --- 4. Mastery & Skills ---
                __result.SetConsummateLevel(18, context);
                CombatSkillDomain skillDomain = DomainManager.CombatSkill;
                List<short> learnedSkills = __result.GetLearnedCombatSkills();

                foreach (CombatSkillItem skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                {
                    if (skillCfg.SectId == XN_ID)
                    {
                        if (!learnedSkills.Contains(skillCfg.TemplateId))
                        {
                            // Create as fully mastered (ReadingState 65535)
                            skillDomain.CreateCombatSkill(charId, skillCfg.TemplateId, 65535);
                            GameData.Domains.CombatSkill.CombatSkill inst;
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

                // --- 5. Force Full Attribute Cache Refresh ---
                InvokeMethod(__result, "InvalidateSelfAndInfluencedCache", new object[] { (ushort)0, context });
            }
        }

        // --- Consolidated Identity Helpers (Reflection based for robustness) ---
        private static void SetGender(GameData.Domains.Character.Character character, sbyte gender, DataContext context)
        {
            SetPrivateField(character, "_gender", gender);
            InvokeMethod(character, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)3, context });
            if (character.CollectionHelperData.IsArchive)
            {
                unsafe {
                    byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 7U, 1);
                    *ptr = (byte)gender;
                }
            }
        }

        private static void SetTransGender(GameData.Domains.Character.Character character, bool transgender, DataContext context)
        {
            SetPrivateField(character, "_transgender", transgender);
            InvokeMethod(character, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)13, context });
            if (character.CollectionHelperData.IsArchive)
            {
                unsafe {
                    byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 26U, 1);
                    *ptr = (byte)(transgender ? 1 : 0);
                }
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
