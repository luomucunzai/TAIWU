using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using HarmonyLib;

// Token: 0x02000005 RID: 5
[NullableContext(1)]
[Nullable(0)]
public class RealTimeModifyCharacterPatch
{
	// Token: 0x06000005 RID: 5 RVA: 0x00002094 File Offset: 0x00000294
	[HarmonyPatch(typeof(GameDataBridge), "ProcessMethodCall")]
	[HarmonyPrefix]
	public static bool ProcessMethodCallPatch(Operation operation, RawDataPool argDataPool, DataContext context)
	{
		bool flag = operation.DomainId != 66;
		bool result;
		if (flag)
		{
			result = true;
		}
		else
		{
			NotificationCollection notificationCollection = (NotificationCollection)AccessTools.Field(typeof(GameDataBridge), "_pendingNotifications").GetValue(context);
			bool flag2 = !Common.IsInWorld();
			if (flag2)
			{
				result = true;
			}
			else
			{
				int num = RealTimeModifyCharacterPatch.HandleOperation(operation, argDataPool, notificationCollection.DataPool, context);
				bool flag3 = num >= 0;
				if (flag3)
				{
					notificationCollection.Notifications.Add(Notification.CreateMethodReturn(operation.ListenerId, operation.DomainId, operation.MethodId, num));
				}
				result = false;
			}
		}
		return result;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002130 File Offset: 0x00000330
	private static int HandleOperation(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext dataContext)
	{
		int result = -1;
		int argsOffset = operation.ArgsOffset;
		switch (operation.MethodId)
		{
		case 1:
		{
			List<int> list = new List<int>();
			string charNameOrId = "";
			int num = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId);
			int num2 = num + Serializer.Deserialize(argDataPool, num, ref list);
			GameData.Domains.Character.Character character = Util.getCharacter(charNameOrId);
			bool flag = character != null;
			if (flag)
			{
				foreach (int num3 in list)
				{
					character.AddFeature(dataContext, (short)num3, true);
				}
			}
			break;
		}
		case 2:
		{
			List<int> list2 = new List<int>();
			string charNameOrId2 = "";
			int num4 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId2);
			int num2 = num4 + Serializer.Deserialize(argDataPool, num4, ref list2);
			GameData.Domains.Character.Character character2 = Util.getCharacter(charNameOrId2);
			bool flag2 = character2 != null;
			if (flag2)
			{
				foreach (int num5 in list2)
				{
					character2.RemoveFeature(dataContext, (short)num5);
				}
			}
			break;
		}
		case 3:
		{
			int num6 = -9999;
			string charNameOrId3 = "";
			int num7 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId3);
			int num2 = num7 + Serializer.Deserialize(argDataPool, num7, ref num6);
			GameData.Domains.Character.Character character3 = Util.getCharacter(charNameOrId3);
			bool flag3 = character3 != null && num6 >= -9000;
			if (flag3)
			{
				short baseMorality = (short)num6;
				character3.SetBaseMorality(baseMorality, dataContext);
			}
			break;
		}
		case 4:
		{
			string charNameOrId4 = "";
			string text = "";
			string text2 = "";
			int num8 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId4);
			int num9 = num8 + Serializer.Deserialize(argDataPool, num8, ref text);
			int num2 = num9 + Serializer.Deserialize(argDataPool, num9, ref text2);
			GameData.Domains.Character.Character character4 = Util.getCharacter(charNameOrId4);
			bool flag4 = character4 != null;
			if (flag4)
			{
				CharacterItem characterItem = Config.Character.Instance[character4.GetTemplateId()];
				bool flag5 = characterItem.CreatingType == 1 || characterItem.CreatingType == 2;
				if (flag5)
				{
					int customSurnameId = DomainManager.World.RegisterCustomText(dataContext, text);
					int customGivenNameId = DomainManager.World.RegisterCustomText(dataContext, text2);
					FullName fullName = character4.GetFullName();
					FullName fullName2 = new FullName(customSurnameId, customGivenNameId, fullName.SurnameId, fullName.GivenNameGroupId, fullName.GivenNameSuffixId, fullName.GivenNameType);
					character4.SetFullName(fullName2, dataContext);
				}
			}
			break;
		}
		case 5:
		{
			string charNameOrId5 = "";
			int num2 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId5);
			GameData.Domains.Character.Character character5 = Util.getCharacter(charNameOrId5);
			bool flag6 = character5 != null;
			if (flag6)
			{
				int id = character5.GetId();
				CharacterDisplayData characterDisplayData = DomainManager.Character.GetCharacterDisplayData(id);
				sbyte birthMonth = character5.GetBirthMonth();
				result = Serializer.Serialize(characterDisplayData, returnDataPool);
				Serializer.Serialize(birthMonth, returnDataPool);
			}
			break;
		}
		case 6:
		{
			int num10 = 0;
			sbyte b = 0;
			sbyte b2 = 0;
			AvatarData reference = new AvatarData();
			int num11 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref num10);
			int num12 = num11 + Serializer.Deserialize(argDataPool, num11, ref b);
			int num13 = num12 + Serializer.Deserialize(argDataPool, num12, ref b2);
			int num2 = num13 + Serializer.Deserialize(argDataPool, num13, ref reference);
			GameData.Domains.Character.Character character6 = Util.getCharacter(num10.ToString() ?? "");
			bool flag7 = character6 != null;
			if (flag7)
			{
				AvatarData avatar = character6.GetAvatar();
				Util.updateAvatarData(ref avatar, reference);
				avatar.InitializeGrowableElementsShowingAbilitiesAndStates(character6);
				character6.SetAvatar(avatar, dataContext);
				bool transgender = b != avatar.GetGender();
				Util.SetGender(character6, b, dataContext);
				Util.SetTransGender(character6, transgender, dataContext);
				bool flag8 = b2 != character6.GetBirthMonth();
				if (flag8)
				{
					Util.SetBirthMonth(character6, b2, dataContext);
					AccessTools.Method(typeof(GameData.Domains.Character.Character), "OfflineInitializeBaseNeiliProportionOfFiveElements", null, null).Invoke(character6, null);
					NeiliProportionOfFiveElements baseNeiliProportionOfFiveElements = (NeiliProportionOfFiveElements)AccessTools.Field(typeof(GameData.Domains.Character.Character), "_baseNeiliProportionOfFiveElements").GetValue(character6);
					character6.SetBaseNeiliProportionOfFiveElements(baseNeiliProportionOfFiveElements, dataContext);
					character6.AddFeature(dataContext, CharacterDomain.GetBirthdayFeatureId(b2), true);
				}
			}
			break;
		}
		case 7:
		{
			string charNameOrId6 = "";
			int num14 = -1;
			int num15 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId6);
			int num2 = num15 + Serializer.Deserialize(argDataPool, num15, ref num14);
			bool flag9 = num14 >= 0;
			if (flag9)
			{
				GameData.Domains.Character.Character character7 = Util.getCharacter(charNameOrId6);
				bool flag10 = character7 != null;
				if (flag10)
				{
					Util.SetCombatSkillQualificationGrowthType(character7, (sbyte)num14, dataContext);
				}
			}
			break;
		}
		case 8:
		{
			string charNameOrId7 = "";
			int num16 = -1;
			int num17 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId7);
			int num2 = num17 + Serializer.Deserialize(argDataPool, num17, ref num16);
			bool flag11 = num16 >= 0;
			if (flag11)
			{
				GameData.Domains.Character.Character character8 = Util.getCharacter(charNameOrId7);
				bool flag12 = character8 != null;
				if (flag12)
				{
					Util.SetLifeSkillQualificationGrowthType(character8, (sbyte)num16, dataContext);
				}
			}
			break;
		}
		case 9:
		{
			string charNameOrId8 = "";
			int num18 = -1;
			int num19 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId8);
			int num2 = num19 + Serializer.Deserialize(argDataPool, num19, ref num18);
			GameData.Domains.Character.Character character9 = Util.getCharacter(charNameOrId8);
			bool flag13 = character9 != null && num18 >= 0;
			if (flag13)
			{
				bool bisexual = num18 != 0;
				Util.SetBisexual(character9, bisexual, dataContext);
			}
			break;
		}
		case 10:
		{
			string charNameOrId9 = "";
			int num2 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId9);
			GameData.Domains.Character.Character character10 = Util.getCharacter(charNameOrId9);
			int item = -1;
			bool flag14 = character10 != null;
			if (flag14)
			{
				item = (character10.GetBisexual() ? 1 : 0);
			}
			result = Serializer.Serialize(item, returnDataPool);
			break;
		}
		case 11:
		{
			string charNameOrId10 = "";
			short num20 = -1;
			int num21 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId10);
			int num2 = num21 + Serializer.Deserialize(argDataPool, num21, ref num20);
			GameData.Domains.Character.Character character11 = Util.getCharacter(charNameOrId10);
			bool flag15 = character11 != null && FameAction.Instance.GetItem(num20) != null;
			if (flag15)
			{
				EventHelper.RecordRoleFameAction(character11, num20, -1, 1);
			}
			break;
		}
		case 12:
		{
			string charNameOrId11 = "";
			short num22 = -1;
			int num23 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId11);
			int num2 = num23 + Serializer.Deserialize(argDataPool, num23, ref num22);
			GameData.Domains.Character.Character character12 = Util.getCharacter(charNameOrId11);
			bool flag16 = character12 != null && FameAction.Instance.GetItem(num22) != null;
			if (flag16)
			{
				EventHelper.ChangeFameActionDuration(character12, num22, int.MinValue, false);
			}
			break;
		}
		case 13:
		{
			string charNameOrId12 = "";
			int num2 = argsOffset + Serializer.Deserialize(argDataPool, argsOffset, ref charNameOrId12);
			GameData.Domains.Character.Character character13 = Util.getCharacter(charNameOrId12);
			CharacterDisplayData item2 = null;
			List<short> item3 = new List<short>();
			bool flag17 = character13 != null;
			if (flag17)
			{
				item2 = DomainManager.Character.GetCharacterDisplayData(character13.GetId());
				item3 = character13.GetFeatureIds();
			}
			result = Serializer.Serialize(item3, returnDataPool);
			Serializer.Serialize(item2, returnDataPool);
			break;
		}
		}
		return result;
	}
}
