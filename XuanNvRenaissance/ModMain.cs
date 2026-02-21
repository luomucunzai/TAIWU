using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Organization;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaiwuModdingLib.Core.Plugin;
using GameData.Utilities;
using System.Reflection;
using GameData.Domains.Item;
using Config;
using GameData.ArchiveData;
using GameData.Domains.Building;
using Redzen.Random;
using System.Diagnostics;
using GameData.Domains.World.Notification;
using GameData.Domains.Map;

namespace XuanNvRenaissance
{
    [PluginConfig("璇女峰文艺复兴", "black_wing", "1.3.4")]
    public class XuanNvRenaissanceMod : TaiwuRemakeHarmonyPlugin
    {
        public const short XuanNvSectId = 8;
        public const sbyte FemaleGenderId = 0;

        // Settings variables with defaults
        public static int globalAgeMin = 16;
        public static int globalAgeMax = 20;
        public static int globalCharmMin = 900;
        public static int globalCharmMax = 900;
        public static int globalPureEssence = 18;
        public static int globalCombatQual = 150;
        public static int globalLifeQual = 150;
        public static int globalMainAttrFloor = 100;
        public static string globalFeatureIds = "164"; // 忠贞不渝
        public static string globalSkillIds = "";

        // Monthly Event Settings
        public static bool enableMonthlyEvents = true;
        public static int eventChanceMultiplier = 100; // Percentage
        public static bool enableColdPoolHeal = true;
        public static bool enableArtisticFavor = true;
        public static bool enableHeavenlyRecruit = true;
        public static bool enablePureEssenceGain = true;

        public static readonly Dictionary<sbyte, int> gradeTargetTemplates = new Dictionary<sbyte, int>();

        public override void Initialize()
        {
            base.Initialize(); // Initializes Harmony and patches all
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
                    int selectedIndex = i; // Default
                    DomainManager.Mod.GetSetting(base.ModIdStr, $"Grade{i + 1}_TargetTemplate", ref selectedIndex);
                    gradeTargetTemplates[i] = selectedIndex;
                }
            }
            catch (Exception ex)
            {
                AdaptableLog.Error($"XuanNvRenaissance: LoadModSettings Error: {ex.Message}");
            }
        }

        [HarmonyPatch]
        public static class XuanNuHooks
        {
            private static int Clamp(int value, int min, int max) => (value < min) ? min : (value > max ? max : value);

            // 1. Rank Mapping
            [HarmonyPatch(typeof(OrganizationDomain), "GetOrgMemberConfig", new Type[] { typeof(sbyte), typeof(sbyte) })]
            [HarmonyPrefix]
            public static void GetOrgMemberConfig_Prefix(sbyte orgTemplateId, ref sbyte grade)
            {
                if (orgTemplateId == XuanNvSectId)
                {
                    if (gradeTargetTemplates.TryGetValue(grade, out int mappedGrade))
                        grade = (sbyte)mappedGrade;
                }
            }

            // 2. Persistent Charm
            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "CalcAttraction")]
            [HarmonyPostfix]
            public static void CalcAttraction_Postfix(GameData.Domains.Character.Character __instance, ref short __result)
            {
                if (__instance.GetOrganizationInfo().OrgTemplateId == XuanNvSectId)
                {
                    __result = (short)Clamp(__result, globalCharmMin, globalCharmMax);
                }
            }

            // 3. Complete Control Over Individual Generation
            [HarmonyPatch(typeof(CharacterDomain), "CreateIntelligentCharacter")]
            [HarmonyPostfix]
            public static unsafe void CreateIntelligentCharacter_Postfix(DataContext context, GameData.Domains.Character.Character __result)
            {
                if (__result == null) return;
                if (__result.GetOrganizationInfo().OrgTemplateId != XuanNvSectId) return;

                ApplyXuanNvHighSpec(__result, context);
            }

            // 4. Recruit Character Postfix
            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "GenerateRecruitCharacterData")]
            [HarmonyPostfix]
            public static unsafe void GenerateRecruitCharacterData_Postfix(IRandomSource random, sbyte peopleLevel, BuildingBlockKey blockKey, BuildingBlockData blockData, ref RecruitCharacterData __result)
            {
                if (__result == null) return;

                var settlement = DomainManager.Organization.GetSettlementByLocation(blockKey.GetLocation());
                if (settlement != null && settlement.GetOrgTemplateId() == XuanNvSectId)
                {
                    short age = (short)random.Next(globalAgeMin, globalAgeMax + 1);
                    short charm = (short)random.Next(globalCharmMin, globalCharmMax + 1);

                    __result.Age = age;
                    __result.Gender = FemaleGenderId;
                    __result.Transgender = false;
                    __result.BaseAttraction = charm;

                    sbyte bodyType = (sbyte)(age < 30 ? 0 : (age < 50 ? 1 : 2));
                    __result.AvatarData = AvatarManager.Instance.GetRandomAvatar(random, FemaleGenderId, false, bodyType, charm);

                    sbyte grade = (sbyte)Clamp(peopleLevel - 1, 0, 8);

                    int combatQualBase = globalCombatQual - grade * 10;
                    if (grade <= 2) combatQualBase = Math.Max(combatQualBase, 85);
                    else combatQualBase = Math.Max(combatQualBase, 20);

                    int lifeQualBase = globalLifeQual - grade * 10;
                    if (grade <= 2) lifeQualBase = Math.Max(lifeQualBase, 85);
                    else lifeQualBase = Math.Max(lifeQualBase, 20);

                    CombatSkillShorts cQuals = __result.CombatSkillQualifications;
                    short* pc = cQuals.Items;
                    for (int i = 0; i < 14; i++)
                        if (pc[i] < combatQualBase) pc[i] = (short)combatQualBase;
                    __result.CombatSkillQualifications = cQuals;

                    LifeSkillShorts lQuals = __result.LifeSkillQualifications;
                    short* pl = lQuals.Items;
                    for (int i = 0; i < 16; i++)
                        if (pl[i] < lifeQualBase) pl[i] = (short)lifeQualBase;
                    __result.LifeSkillQualifications = lQuals;

                    MainAttributes attrs = __result.MainAttributes;
                    short* pa = attrs.Items;
                    for (int i = 0; i < 6; i++)
                        if (pa[i] < globalMainAttrFloor) pa[i] = (short)globalMainAttrFloor;
                    __result.MainAttributes = attrs;

                    if (!string.IsNullOrWhiteSpace(globalFeatureIds))
                    {
                        if (__result.FeatureIds == null) __result.FeatureIds = new List<short>();
                        foreach (string idStr in globalFeatureIds.Split(','))
                        {
                            if (short.TryParse(idStr.Trim(), out short fid))
                                if (!__result.FeatureIds.Contains(fid)) __result.FeatureIds.Add(fid);
                        }
                    }
                    __result.Recalculate();
                }
            }

            public static unsafe void ApplyXuanNvHighSpec(GameData.Domains.Character.Character character, DataContext context)
            {
                OrganizationInfo orgInfo = character.GetOrganizationInfo();
                sbyte grade = orgInfo.Grade;

                short age = (short)context.Random.Next(globalAgeMin, globalAgeMax + 1);
                character.SetActualAge(age, context);
                character.SetCurrAge(age, context);

                character.OfflineSetGenderInfo(FemaleGenderId, false);
                Util.InvalidateField(character, 3, context);
                character.SetBaseMorality(700, context);
                character.SetXiangshuInfection(0, context);

                short finalCharm = (short)context.Random.Next(globalCharmMin, globalCharmMax + 1);
                sbyte bodyType = (sbyte)(age < 30 ? 0 : (age < 50 ? 1 : 2));
                AvatarData newAvatar = AvatarManager.Instance.GetRandomAvatar(context.Random, FemaleGenderId, false, bodyType, finalCharm);
                character.SetAvatar(newAvatar, context);
                Util.InvalidateField(character, 1, context);

                Injuries emptyInjuries = default(Injuries);
                emptyInjuries.Initialize();
                character.SetInjuries(emptyInjuries, context);
                character.SetDisorderOfQi(0, context);
                character.SetHealth(character.GetMaxHealth(), context);

                if (globalPureEssence > 0)
                {
                    if (character.GetConsummateLevel() < globalPureEssence)
                        character.SetConsummateLevel((sbyte)globalPureEssence, context);
                }

                int combatQualBase = globalCombatQual - grade * 10;
                if (grade <= 2) combatQualBase = Math.Max(combatQualBase, 85);
                else combatQualBase = Math.Max(combatQualBase, 20);

                int lifeQualBase = globalLifeQual - grade * 10;
                if (grade <= 2) lifeQualBase = Math.Max(lifeQualBase, 85);
                else lifeQualBase = Math.Max(lifeQualBase, 20);

                CombatSkillShorts pcQuals = character.GetBaseCombatSkillQualifications();
                short* pc = pcQuals.Items;
                bool cChanged = false;
                for (int i = 0; i < 14; i++)
                {
                    if (pc[i] < combatQualBase) { pc[i] = (short)combatQualBase; cChanged = true; }
                }
                if (cChanged) character.SetBaseCombatSkillQualifications(ref pcQuals, context);

                LifeSkillShorts plQuals = character.GetBaseLifeSkillQualifications();
                short* pl = plQuals.Items;
                bool lChanged = false;
                for (int i = 0; i < 16; i++)
                {
                    if (pl[i] < lifeQualBase) { pl[i] = (short)lifeQualBase; lChanged = true; }
                }
                if (lChanged) character.SetBaseLifeSkillQualifications(ref plQuals, context);

                if (globalMainAttrFloor > 0)
                {
                    MainAttributes attrs = character.GetBaseMainAttributes();
                    short* pa = attrs.Items;
                    bool attrChanged = false;
                    for (int i = 0; i < 6; i++)
                        if (pa[i] < globalMainAttrFloor) { pa[i] = (short)globalMainAttrFloor; attrChanged = true; }
                    if (attrChanged) character.SetBaseMainAttributes(attrs, context);
                }

                if (!string.IsNullOrWhiteSpace(globalFeatureIds))
                {
                    foreach (string idStr in globalFeatureIds.Split(','))
                    {
                        if (short.TryParse(idStr.Trim(), out short fid))
                            if (!character.GetFeatureIds().Contains(fid)) character.AddFeature(context, fid, true);
                    }
                }

                if (string.IsNullOrWhiteSpace(globalSkillIds))
                {
                    foreach (CombatSkillItem skillCfg in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
                    {
                        if (skillCfg.SectId == XuanNvSectId && skillCfg.Grade >= grade)
                            LearnAndMasterSkill(character, skillCfg.TemplateId, context);
                    }
                }
                else
                {
                    foreach (string idStr in globalSkillIds.Split(','))
                    {
                        if (short.TryParse(idStr.Trim(), out short sid))
                            LearnAndMasterSkill(character, sid, context);
                    }
                }

                Util.InvokeMethod(character, "InvalidateAllCaches", new object[] { context });
            }

            private static void LearnAndMasterSkill(GameData.Domains.Character.Character character, short skillId, DataContext context)
            {
                var learnedSkills = character.GetLearnedCombatSkills();
                GameData.Domains.CombatSkill.CombatSkill skill = null;

                if (learnedSkills.Contains(skillId))
                {
                    skill = DomainManager.CombatSkill.GetElement_CombatSkills((character.GetId(), skillId));
                    if (skill != null) skill.SetReadingState(32767, context);
                }
                else
                {
                    skill = character.LearnNewCombatSkill(context, skillId, 32767);
                }

                if (skill != null)
                {
                    skill.SetPracticeLevel((sbyte)100, context);
                    ushort actState = 0;
                    actState = CombatSkillStateHelper.SetPageActive(actState, 1);
                    for (byte i = 5; i < 10; i++) actState = CombatSkillStateHelper.SetPageActive(actState, i);
                    skill.SetActivationState(actState, context);

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
            public static unsafe void PeriAdvanceMonth_UpdateStatus_Postfix(GameData.Domains.Character.Character __instance, DataContext context)
            {
                if (!enableMonthlyEvents || __instance == null) return;
                if (__instance.GetOrganizationInfo().OrgTemplateId != XuanNvSectId) return;

                IRandomSource random = context.Random;
                int multiplier = Math.Max(0, eventChanceMultiplier);

                // 1. Cold Pool Meditation
                if (enableColdPoolHeal && random.CheckPercentProb(10 * multiplier / 100))
                {
                    var settlement = DomainManager.Organization.GetSettlementByLocation(__instance.GetValidLocation());
                    if (settlement != null && settlement.GetOrgTemplateId() == XuanNvSectId)
                    {
                        if (__instance.GetDisorderOfQi() > 0)
                        {
                            __instance.ChangeDisorderOfQi(context, -50);
                            DomainManager.World.GetMonthlyNotificationCollection().AddUnexpectedlyHealQi(__instance.GetId(), __instance.GetValidLocation());
                        }

                        Injuries injuries = __instance.GetInjuries();
                        if (injuries.Any())
                        {
                            Injuries delta = default(Injuries);
                            delta.Initialize();
                            for (sbyte i = 0; i < 7; i++)
                            {
                                if (injuries.Get(i, false) > 0) delta.Set(i, false, 20);
                                if (injuries.Get(i, true) > 0) delta.Set(i, true, 20);
                            }
                            __instance.ChangeInjuries(context, delta);
                            DomainManager.World.GetMonthlyNotificationCollection().AddUnexpectedlyHealOuterInjury(__instance.GetId(), __instance.GetValidLocation(), 20);
                        }
                    }
                }

                // 2. Artistic Performance
                if (enableArtisticFavor && random.CheckPercentProb(10 * multiplier / 100))
                {
                    if (__instance.GetLifeSkillQualification(0) > 80) // Music
                    {
                        var location = __instance.GetValidLocation();
                        var block = DomainManager.Map.GetBlock(location);
                        if (block != null && block.CharacterSet != null && block.CharacterSet.Count > 1)
                        {
                            var nearbyCharIds = block.CharacterSet.ToList();
                            int targetId = nearbyCharIds[random.Next(nearbyCharIds.Count)];
                            if (targetId != __instance.GetId())
                            {
                                if (DomainManager.Character.TryGetElement_Objects(targetId, out var targetChar))
                                {
                                    DomainManager.Character.ChangeFavorability(context, __instance, targetChar, 5000);
                                    DomainManager.World.GetMonthlyNotificationCollection().AddAmuseOthersByMusic(__instance.GetId(), location, targetId);
                                }
                            }
                        }
                    }
                }

                // 3. Pure Essence Gain
                if (enablePureEssenceGain && random.CheckPercentProb(2 * multiplier / 100))
                {
                    if (__instance.GetConsummateLevel() < 18)
                    {
                        __instance.SetConsummateLevel((sbyte)(__instance.GetConsummateLevel() + 1), context);
                        DomainManager.World.GetMonthlyNotificationCollection().AddUnexpectedlyGetHealth(__instance.GetId(), __instance.GetValidLocation(), 1);
                    }
                }

                // 4. Heavenly Recruitment (Only for Sect Leader)
                if (enableHeavenlyRecruit && __instance.GetOrganizationInfo().Grade == 0 && random.CheckPercentProb(5 * multiplier / 100))
                {
                    short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(XuanNvSectId);
                    if (settlementId >= 0)
                    {
                        var info = new IntelligentCharacterCreationInfo
                        {
                            OrgInfo = new OrganizationInfo(XuanNvSectId, 8, false, settlementId),
                            Gender = FemaleGenderId,
                            Location = __instance.GetValidLocation()
                        };
                        var newChar = DomainManager.Character.CreateIntelligentCharacter(context, ref info);
                        if (newChar != null)
                        {
                            DomainManager.World.GetMonthlyNotificationCollection().AddJoinOrganization(newChar.GetId(), settlementId);
                        }
                    }
                }
            }
        }

        public static class Util
        {
            public static GameData.Domains.Character.Character GetCharacter(string charNameOrId)
            {
                if (string.IsNullOrEmpty(charNameOrId)) return null;
                if (int.TryParse(charNameOrId, out int id))
                    if (DomainManager.Character.TryGetElement_Objects(id, out GameData.Domains.Character.Character character)) return character;
                return null;
            }

            public static object InvokeMethod(object obj, string methodName, object[] args) => AccessTools.Method(obj.GetType(), methodName)?.Invoke(obj, args);

            public static void InvalidateField(GameData.Domains.Character.Character character, ushort fieldId, DataContext context)
            {
                AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache").Invoke(character, new object[] { (ushort)fieldId, context });
            }
        }
    }
}
