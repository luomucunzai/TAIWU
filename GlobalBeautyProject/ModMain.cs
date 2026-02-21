using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using HarmonyLib;
using System;
using System.Linq;
using TaiwuModdingLib.Core.Plugin;
using Config;
using Redzen.Random;
using System.Reflection;

namespace GlobalBeautyProject
{
    [PluginConfig("全门派绝世容颜", "black_wing", "1.4.9")]
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

        public override void OnModSettingUpdate() => LoadModSettings();

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
            if (val < 25) return (sbyte)0;
            if (val < 85) return (sbyte)1;
            return (sbyte)2;
        }

        public static void EnsureHair(ref AvatarData avatar, IRandomSource random)
        {
            AvatarGroup group = AvatarManager.Instance.GetAvatarGroup(avatar.AvatarId);
            if (group != null)
            {
                var hairIds = group.GetRandomHairsNoSkinHead(random);
                avatar.FrontHairId = hairIds.frontId;
                avatar.BackHairId = hairIds.backId;
            }
        }

        public static object SafeInvoke(object obj, string methodName, object[] args)
        {
            if (obj == null) return null;
            MethodInfo method = null;
            Type type = (obj is Type t) ? t : obj.GetType();
            object target = (obj is Type) ? null : obj;

            method = AccessTools.Method(type, methodName);
            if (method == null) return null;

            ParameterInfo[] parameters = method.GetParameters();
            object[] convertedArgs = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == null)
                {
                    convertedArgs[i] = null;
                }
                else if (i < parameters.Length)
                {
                    Type paramType = parameters[i].ParameterType;
                    if (paramType.IsInstanceOfType(args[i]))
                    {
                        convertedArgs[i] = args[i];
                    }
                    else
                    {
                        try {
                            convertedArgs[i] = Convert.ChangeType(args[i], paramType);
                        } catch (Exception ex) {
                            AdaptableLog.Error($"SafeInvoke Conversion Error: Method={methodName}, ArgIndex={i}, SourceType={args[i].GetType().Name}, TargetType={paramType.Name}, Value={args[i]}, Error={ex.Message}");
                            convertedArgs[i] = args[i];
                        }
                    }
                }
                else
                {
                    convertedArgs[i] = args[i];
                }
            }
            try {
                return method.Invoke(target, convertedArgs);
            } catch (Exception ex) {
                AdaptableLog.Error($"SafeInvoke Execution Error: Method={methodName}, Error={ex.Message}");
                return null;
            }
        }

        [HarmonyPatch]
        public static class BeautyHooks
        {
            private static bool ShouldApply(GameData.Domains.Character.Character character)
            {
                if (character == null) return false;
                sbyte orgId = character.GetOrganizationInfo().OrgTemplateId;
                if (orgId == 0) return false;
                if (orgId >= 21 && orgId <= 38) return enableVillages;
                return true;
            }

            [HarmonyPatch(typeof(GameData.Domains.Character.Character), "CalcAttraction")]
            [HarmonyPostfix]
            public static void CalcAttraction_Postfix(GameData.Domains.Character.Character __instance, ref short __result)
            {
                if (ShouldApply(__instance))
                {
                    if (__result < (short)globalCharmMin) __result = (short)globalCharmMin;
                    short max = (short)Math.Max(globalCharmMin, globalCharmMax);
                    if (__result > max && max >= globalCharmMin) __result = max;
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
                    sbyte gender = __result.GetGender();
                    bool transgender = __result.GetTransgender();
                    AvatarData newAvatar = AvatarManager.Instance.GetRandomAvatar(context.Random, gender, transgender, bodyType, finalCharm);
                    EnsureHair(ref newAvatar, context.Random);
                    __result.SetAvatar(newAvatar, context);

                    SafeInvoke(__result, "SetModifiedAndInvalidateInfluencedCache", new object[] { (ushort)1, context });
                }
            }
        }
    }
}
