using System;
using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using HarmonyLib;

namespace RealTimeModifyCharacterBackend
{
    public unsafe static class Util
    {
        public static GameData.Domains.Character.Character getCharacter(string charNameOrId)
        {
            if (string.IsNullOrEmpty(charNameOrId))
            {
                return null;
            }
            int id;
            if (!int.TryParse(charNameOrId, out id))
            {
                foreach (GameData.Domains.Character.Character character in ((Dictionary<int, GameData.Domains.Character.Character>)AccessTools.Field(typeof(CharacterDomain), "_objects").GetValue(DomainManager.Character)).Values)
                {
                    try
                    {
                        NameRelatedData nameRelatedData = new NameRelatedData
                        {
                            CharTemplateId = character.GetTemplateId(),
                            FullName = character.GetFullName(),
                            Gender = character.GetGender(),
                            MonasticTitle = character.GetMonasticTitle(),
                            MonkType = character.GetMonkType(),
                            OrgGrade = character.GetOrganizationInfo().Grade,
                            OrgTemplateId = character.GetOrganizationInfo().OrgTemplateId
                        };
                        string name_UIShow = Util.GetName_UIShow(ref nameRelatedData, character.GetId() == DomainManager.Taiwu.GetTaiwuCharId(), true);
                        string name_UIShow2 = Util.GetName_UIShow(ref nameRelatedData, character.GetId() == DomainManager.Taiwu.GetTaiwuCharId(), false);
                        if (charNameOrId.Equals(name_UIShow) || charNameOrId.Equals(name_UIShow2))
                        {
                            id = character.GetId();
                            break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            GameData.Domains.Character.Character character2;
            return ((id > -1 && DomainManager.Character.TryGetElement_Objects(id, out character2)) ? character2 : null);
        }

        public static void SetCombatSkillQualificationGrowthType(GameData.Domains.Character.Character character, sbyte growthType, DataContext context)
        {
            AccessTools.Field(typeof(GameData.Domains.Character.Character), "_combatSkillQualificationGrowthType").SetValue(character, growthType);
            AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[] { typeof(ushort), typeof(DataContext) }).Invoke(character, new object[] { (ushort)33, context });
            if (character.CollectionHelperData.IsArchive)
            {
                byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 134U, 1);
                *ptr = (byte)growthType;
            }
        }

        public static void SetLifeSkillQualificationGrowthType(GameData.Domains.Character.Character character, sbyte growthType, DataContext context)
        {
            AccessTools.Field(typeof(GameData.Domains.Character.Character), "_lifeSkillQualificationGrowthType").SetValue(character, growthType);
            AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[] { typeof(ushort), typeof(DataContext) }).Invoke(character, new object[] { (ushort)31, context });
            if (character.CollectionHelperData.IsArchive)
            {
                byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 105U, 1);
                *ptr = (byte)growthType;
            }
        }

        public static void SetBirthMonth(GameData.Domains.Character.Character character, sbyte birthMonth, DataContext context)
        {
            AccessTools.Field(typeof(GameData.Domains.Character.Character), "_birthMonth").SetValue(character, birthMonth);
            AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[] { typeof(ushort), typeof(DataContext) }).Invoke(character, new object[] { (ushort)5, context });
            if (character.CollectionHelperData.IsArchive)
            {
                byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 10U, 1);
                *ptr = (byte)birthMonth;
            }
        }

        public static void SetBisexual(GameData.Domains.Character.Character character, bool bisexual, DataContext context)
        {
            AccessTools.Field(typeof(GameData.Domains.Character.Character), "_bisexual").SetValue(character, bisexual);
            AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[] { typeof(ushort), typeof(DataContext) }).Invoke(character, new object[] { (ushort)14, context });
            if (character.CollectionHelperData.IsArchive)
            {
                byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 27U, 1);
                *ptr = (byte)(bisexual ? 1 : 0);
            }
        }

        public static void SetGender(GameData.Domains.Character.Character character, sbyte gender, DataContext context)
        {
            AccessTools.Field(typeof(GameData.Domains.Character.Character), "_gender").SetValue(character, gender);
            AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[] { typeof(ushort), typeof(DataContext) }).Invoke(character, new object[] { (ushort)3, context });
            if (character.CollectionHelperData.IsArchive)
            {
                byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 7U, 1);
                *ptr = (byte)gender;
            }
        }

        public static void SetTransGender(GameData.Domains.Character.Character character, bool transgender, DataContext context)
        {
            AccessTools.Field(typeof(GameData.Domains.Character.Character), "_transgender").SetValue(character, transgender);
            AccessTools.Method(typeof(GameData.Domains.Character.Character), "SetModifiedAndInvalidateInfluencedCache", new Type[] { typeof(ushort), typeof(DataContext) }).Invoke(character, new object[] { (ushort)13, context });
            if (character.CollectionHelperData.IsArchive)
            {
                byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField<int>(character.CollectionHelperData.DomainId, character.CollectionHelperData.DataId, character.GetId(), 26U, 1);
                *ptr = (byte)(transgender ? 1 : 0);
            }
        }

        public static void updateAvatarData(ref AvatarData target, AvatarData reference)
        {
            target.AvatarId = reference.AvatarId;
            target.ColorSkinId = reference.ColorSkinId;
            target.ColorClothId = reference.ColorClothId;
            target.Wrinkle1Id = reference.Wrinkle1Id;
            target.Wrinkle2Id = reference.Wrinkle2Id;
            target.Wrinkle3Id = reference.Wrinkle3Id;
            target.EyesLeftId = reference.EyesLeftId;
            target.EyesRightId = reference.EyesRightId;
            target.FrontHairId = reference.FrontHairId;
            target.ColorFrontHairId = reference.ColorFrontHairId;
            target.BackHairId = reference.BackHairId;
            target.ColorBackHairId = reference.ColorBackHairId;
            target.EyebrowId = reference.EyebrowId;
            target.EyebrowDistancePercent = reference.EyebrowDistancePercent;
            target.EyebrowHeight = reference.EyebrowHeight;
            target.EyebrowScale = reference.EyebrowScale;
            target.EyebrowAngle = reference.EyebrowAngle;
            target.ColorEyebrowId = reference.ColorEyebrowId;
            target.EyesMainId = reference.EyesMainId;
            target.EyesDistancePercent = reference.EyesDistancePercent;
            target.EyesHeightPercent = reference.EyesHeightPercent;
            target.EyesScale = reference.EyesScale;
            target.EyesAngle = reference.EyesAngle;
            target.ColorEyeballId = reference.ColorEyeballId;
            target.NoseId = reference.NoseId;
            target.NoseScale = reference.NoseScale;
            target.NoseHeightPercent = reference.NoseHeightPercent;
            target.MouthId = reference.MouthId;
            target.MouthHeightPercent = reference.MouthHeightPercent;
            target.MouthScale = reference.MouthScale;
            target.ColorMouthId = reference.ColorMouthId;
            target.Feature1Id = reference.Feature1Id;
            target.Feature2Id = reference.Feature2Id;
            target.ColorFeature1Id = reference.ColorFeature1Id;
            target.ColorFeature2Id = reference.ColorFeature2Id;
            target.Beard1Id = reference.Beard1Id;
            target.Beard2Id = reference.Beard2Id;
            target.ColorBeard1Id = reference.ColorBeard1Id;
            target.ColorBeard2Id = reference.ColorBeard2Id;
        }

        public static string GetName_UIShow(ref NameRelatedData data, bool isTaiwu, bool getRealName)
        {
            ValueTuple<string, string> name = internalGetName_UIShow(ref data, isTaiwu, getRealName);
            string text = (name.Item1 ?? string.Empty) + name.Item2;
            string text2 = internalGetMonasticTitle(ref data, isTaiwu);
            return string.IsNullOrEmpty(text2) ? text : text2;
        }

        private static ValueTuple<string, string> internalGetName_UIShow(ref NameRelatedData data, bool isTaiwu, bool getRealName)
        {
            if (data.CharTemplateId < 0)
            {
                return new ValueTuple<string, string>(null, DomainManager.World.GetElement_CustomTexts(1572));
            }
            CharacterItem characterItem = Config.Character.Instance[data.CharTemplateId];
            if (!isTaiwu)
            {
                if (characterItem.CreatingType == 2)
                {
                    if (data.FullName.SurnameId < 0 && data.FullName.GivenNameGroupId < 0)
                    {
                        return new ValueTuple<string, string>(characterItem.Surname, characterItem.GivenName);
                    }
                }
                else if (characterItem.CreatingType != 1)
                {
                    return new ValueTuple<string, string>(characterItem.Surname, characterItem.GivenName);
                }
            }
            ValueTuple<string, string> name = data.FullName.GetName(data.Gender, DomainManager.World.GetCustomTexts());
            string text = name.Item1;
            string item = name.Item2;
            if (getRealName)
            {
                return new ValueTuple<string, string>(text, item);
            }
            if (isTaiwu)
            {
                text = "太吾";
            }
            else
            {
                short index = Organization.Instance[data.OrgTemplateId].Members[(int)data.OrgGrade];
                short surnameId = OrganizationMember.Instance[index].SurnameId;
                if (surnameId >= 0)
                {
                    text = LocalSurnames.Instance.SurnameCore[(int)surnameId].Surname;
                }
            }
            return new ValueTuple<string, string>(text, item);
        }

        private static string internalGetMonasticTitle(ref NameRelatedData data, bool isTaiwu)
        {
            if (data.MonkType == 0)
            {
                return null;
            }
            if ((data.MonkType & 128) > 0)
            {
                MonasticTitleItem[] monasticTitles = LocalMonasticTitles.Instance.MonasticTitles;
                string name = monasticTitles[(int)data.MonasticTitle.SeniorityId].Name;
                string name2 = monasticTitles[(int)data.MonasticTitle.SuffixId].Name;
                short index = Organization.Instance[data.OrgTemplateId].Members[(int)data.OrgGrade];
                string str = OrganizationMember.Instance[index].MonasticTitleSuffixes[(int)data.Gender];
                return name + name2 + str;
            }
            ValueTuple<string, string> nameInfo = internalGetName_UIShow(ref data, isTaiwu, false);
            string str2 = (!string.IsNullOrEmpty(nameInfo.Item1)) ? nameInfo.Item1 : nameInfo.Item2;
            ushort elementId = ((data.MonkType & 1) == 0) ? ((data.Gender == 1) ? (ushort)991 : (ushort)990) : ((data.Gender == 1) ? (ushort)989 : (ushort)988);
            string element_CustomTexts = DomainManager.World.GetElement_CustomTexts((int)elementId);
            return str2 + element_CustomTexts;
        }
    }

    public struct NameRelatedData
    {
        public short CharTemplateId;
        public FullName FullName;
        public sbyte Gender;
        public MonasticTitle MonasticTitle;
        public sbyte MonkType;
        public sbyte OrgGrade;
        public sbyte OrgTemplateId;
    }
}
