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

        // 1. Blueprint Hijacking
        [HarmonyPatch(typeof(OrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
        [HarmonyPrefix]
        public static void GetOrgMemberConfig_Prefix(sbyte orgTemplateId, ref sbyte grade)
        {
            if (orgTemplateId == XN_ID) grade = 0;
        }

        // 2. Template Hijacking
        [HarmonyPatch(typeof(OrganizationDomain), "GetCharacterTemplateId", new Type[] { typeof(sbyte), typeof(sbyte), typeof(sbyte) })]
        [HarmonyPrefix]
        public static bool GetCharacterTemplateId_Prefix(sbyte orgTemplateId, ref short __result)
        {
            if (orgTemplateId == XN_ID)
            {
                __result = 373;
                return false;
            }
            return true;
        }

        // 3. Core Implementation
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

                Util.SetGender(__result, (sbyte)0, context);
                Util.SetTransGender(__result, false, context);
                var avatar = __result.GetAvatar();
                avatar.ChangeGender(0);
                __result.SetAvatar(avatar, context);

                if (!__result.GetFeatureIds().Contains(FID_ZHONG_ZHEN))
                {
                    __result.AddFeature(context, FID_ZHONG_ZHEN, true);
                }

                SetPrivateField(__result, "_baseAttraction", (short)900);
                __result.SetBaseMorality(700, context);

                float multiplier = (grade == 0) ? 1.5f : (grade == 1 ? 1.35f : 1.1f);
                short combatFloor = (short)(130 * multiplier);
                short lifeFloor = (short)(130 * multiplier);
                short musicFloor = (short)(150 * multiplier);

                // --- Pointer arithmetic fix for Combat Aptitude ---
                // We access the Items fixed buffer directly. Since __result.GetBaseCombatSkillQualifications()
                // returns a copy or a local variable, and we are in an unsafe context,
                // the FixedElementField pointer can be accessed without 'fixed' if the object is pinned.
                // However, the error "already fixed" suggests that the compiler sees cQuals.Items as already fixed.
                CombatSkillShorts cQuals = __result.GetBaseCombatSkillQualifications();
                bool cChanged = false;

                // Use the pattern that worked for the game's own code.
                for (int i = 0; i < 14; i++)
                {
                    short* p = &cQuals.Items.FixedElementField;
                    if (p[i] < combatFloor)
                    {
                        p[i] = combatFloor;
                        cChanged = true;
                    }
                }
                if (cChanged) __result.SetBaseCombatSkillQualifications(ref cQuals, context);

                // --- Pointer arithmetic fix for Life Aptitude ---
                LifeSkillShorts lQuals = __result.GetBaseLifeSkillQualifications();
                bool lChanged = false;
                short* pL = &lQuals.Items.FixedElementField;
                if (pL[0] < musicFloor) { pL[0] = musicFloor; lChanged = true; }
                for (int i = 1; i < 16; i++)
                {
                    if (pL[i] < lifeFloor) { pL[i] = lifeFloor; lChanged = true; }
                }
                if (lChanged) __result.SetBaseLifeSkillQualifications(ref lQuals, context);

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
                            GameData.Domains.CombatSkill.CombatSkill inst;
                            if (skillDomain.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillCfg.TemplateId), out inst))
                            {
                                inst.SetPracticeLevel(100, context);
                            }
                            for (byte pIdx = 5; pIdx <= 14; pIdx++)
                                skillDomain.TryActivateCombatSkillBookPageWhenSetReadingState(context, charId, skillCfg.TemplateId, pIdx);
                        }
                    }
                }

                InvokeMethod(__result, "InvalidateSelfAndInfluencedCache", new object[] { (ushort)0, context });
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
