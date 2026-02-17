using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat.MixPoison
{
	// Token: 0x0200070E RID: 1806
	public static class MixPoisonEffectImplements
	{
		// Token: 0x06006833 RID: 26675 RVA: 0x003B3D98 File Offset: 0x003B1F98
		[MixPoisonEffect(12)]
		public static bool MixPoisonEffect012(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			int changeValue = 300 * (int)(poisonMarkList[0] + poisonMarkList[1] + poisonMarkList[2]);
			DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, combatChar, changeValue, false);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1654, 0);
			return true;
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x003B3DE4 File Offset: 0x003B1FE4
		[MixPoisonEffect(8)]
		public static bool MixPoisonEffect013(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			int reducePercent = (int)((poisonMarkList[0] + poisonMarkList[1] + poisonMarkList[3]) * 5 - 5);
			DomainManager.Combat.ChangeMobilityValue(context, combatChar, -MoveSpecialConstants.MaxMobility * reducePercent / 100, true, combatChar, false);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1650, 0);
			return true;
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x003B3E3C File Offset: 0x003B203C
		[MixPoisonEffect(2)]
		public static bool MixPoisonEffect014(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			int affectOdds = (int)(poisonMarkList[4] * 20);
			bool flag = !context.Random.CheckPercentProb(affectOdds);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !combatChar.ChangeToEmptyHandOrOther(context);
				if (flag2)
				{
					result = false;
				}
				else
				{
					ItemKey[] weapons = combatChar.GetWeapons();
					for (int i = 0; i < 7; i++)
					{
						bool flag3 = i != combatChar.GetUsingWeaponIndex() && weapons[i].IsValid();
						if (flag3)
						{
							combatChar.GetWeaponData(i).SetCdFrame(30000, context);
						}
					}
					DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1644, 0);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x003B3EEC File Offset: 0x003B20EC
		[MixPoisonEffect(18)]
		public unsafe static bool MixPoisonEffect015(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
			int changeValue = -10 * (int)poisonMarkList[5];
			List<byte> neiliAllocationTypeRandomPool = ObjectPool<List<byte>>.Instance.Get();
			bool affected = false;
			neiliAllocationTypeRandomPool.Clear();
			for (byte type = 0; type < 4; type += 1)
			{
				bool flag = *neiliAllocation[(int)type] > 0;
				if (flag)
				{
					neiliAllocationTypeRandomPool.Add(type);
				}
			}
			bool flag2 = neiliAllocationTypeRandomPool.Count > 0;
			if (flag2)
			{
				combatChar.ChangeNeiliAllocation(context, neiliAllocationTypeRandomPool[context.Random.Next(0, neiliAllocationTypeRandomPool.Count)], changeValue, true, true);
				DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1660, 0);
				affected = true;
			}
			ObjectPool<List<byte>>.Instance.Return(neiliAllocationTypeRandomPool);
			return affected;
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x003B3FB0 File Offset: 0x003B21B0
		[MixPoisonEffect(9)]
		public static bool MixPoisonEffect023(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			Injuries injuries = combatChar.GetInjuries();
			int markCount = (int)(poisonMarkList[0] + poisonMarkList[2] + poisonMarkList[3]);
			int addInjuryOdds = 20 * markCount;
			List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
			bodyPartRandomPool.Clear();
			for (sbyte part = 0; part < 7; part += 1)
			{
				bool flag = injuries.Get(part, false) < 6;
				if (flag)
				{
					bodyPartRandomPool.Add(part);
				}
			}
			bool flag2 = bodyPartRandomPool.Count == 0;
			if (flag2)
			{
				for (sbyte part2 = 0; part2 < 7; part2 += 1)
				{
					bodyPartRandomPool.Add(part2);
				}
			}
			sbyte affectPart = bodyPartRandomPool.GetRandom(context.Random);
			bool flag3 = context.Random.CheckPercentProb(addInjuryOdds) && injuries.Get(affectPart, false) < 6;
			if (flag3)
			{
				DomainManager.Combat.AddInjury(context, combatChar, affectPart, false, 1, true, true);
				DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1651, 0);
			}
			else
			{
				DomainManager.Combat.AddFlaw(context, combatChar, (sbyte)Math.Max(markCount / 3 - 1, 0), new CombatSkillKey(-1, -1), affectPart, 1, true);
				DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1651, 1);
			}
			ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
			return true;
		}

		// Token: 0x06006838 RID: 26680 RVA: 0x003B40FC File Offset: 0x003B22FC
		[MixPoisonEffect(3)]
		public static bool MixPoisonEffect024(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			int markCount = (int)poisonMarkList[4];
			DomainManager.Combat.PoisonProduce(context, combatChar, 0, markCount);
			combatChar.AddPoisonAffectValue(3, (short)(20 * markCount), true);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1645, 0);
			return true;
		}

		// Token: 0x06006839 RID: 26681 RVA: 0x003B4148 File Offset: 0x003B2348
		[MixPoisonEffect(15)]
		public static bool MixPoisonEffect025(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			int markCount = (int)poisonMarkList[5];
			DomainManager.Combat.PoisonProduce(context, combatChar, 5, markCount);
			combatChar.AddPoisonAffectValue(1, (short)(markCount * 5 + 5), true);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1657, 0);
			return true;
		}

		// Token: 0x0600683A RID: 26682 RVA: 0x003B4194 File Offset: 0x003B2394
		[MixPoisonEffect(0)]
		public static bool MixPoisonEffect034(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			DomainManager.Combat.AppendFatalDamageMark(context, combatChar, (int)poisonMarkList[4], -1, -1, false, EDamageType.None);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1642, 0);
			return true;
		}

		// Token: 0x0600683B RID: 26683 RVA: 0x003B41D4 File Offset: 0x003B23D4
		[MixPoisonEffect(7)]
		public static bool MixPoisonEffect035(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			for (int i = 0; i < (int)poisonMarkList[5]; i++)
			{
				DomainManager.Combat.AddWeaponAttackSelfInjury(context, combatChar, combatChar.GetUsingWeaponIndex());
			}
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1649, 0);
			return true;
		}

		// Token: 0x0600683C RID: 26684 RVA: 0x003B4224 File Offset: 0x003B2424
		[MixPoisonEffect(1)]
		public static bool MixPoisonEffect045(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			short skillId = combatChar.GetRandomBanableSkillId(context.Random, null, 2);
			bool flag = skillId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.Combat.ClearAffectingAgileSkillByEffect(context, combatChar, null);
				DomainManager.Combat.SilenceSkill(context, combatChar, skillId, (int)((short)(600 * (int)(poisonMarkList[4] + poisonMarkList[5]))), 100);
				DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1643, 0);
				result = true;
			}
			return result;
		}

		// Token: 0x0600683D RID: 26685 RVA: 0x003B4298 File Offset: 0x003B2498
		[MixPoisonEffect(13)]
		public static bool MixPoisonEffect123(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			Injuries injuries = combatChar.GetInjuries();
			int markCount = (int)(poisonMarkList[1] + poisonMarkList[2] + poisonMarkList[3]);
			int addInjuryOdds = 20 * markCount;
			List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
			bodyPartRandomPool.Clear();
			for (sbyte part = 0; part < 7; part += 1)
			{
				bool flag = injuries.Get(part, true) < 6;
				if (flag)
				{
					bodyPartRandomPool.Add(part);
				}
			}
			bool flag2 = bodyPartRandomPool.Count == 0;
			if (flag2)
			{
				for (sbyte part2 = 0; part2 < 7; part2 += 1)
				{
					bodyPartRandomPool.Add(part2);
				}
			}
			sbyte affectPart = bodyPartRandomPool.GetRandom(context.Random);
			bool flag3 = context.Random.CheckPercentProb(addInjuryOdds) && injuries.Get(affectPart, false) < 6;
			if (flag3)
			{
				DomainManager.Combat.AddInjury(context, combatChar, affectPart, true, 1, true, true);
				DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1655, 0);
			}
			else
			{
				DomainManager.Combat.AddAcupoint(context, combatChar, (sbyte)Math.Max(markCount / 3 - 1, 0), new CombatSkillKey(-1, -1), affectPart, 1, true);
				DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1655, 1);
			}
			ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
			return true;
		}

		// Token: 0x0600683E RID: 26686 RVA: 0x003B43E8 File Offset: 0x003B25E8
		[MixPoisonEffect(11)]
		public static bool MixPoisonEffect124(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			List<short> skillIdList = ObjectPool<List<short>>.Instance.Get();
			skillIdList.Clear();
			skillIdList.AddRange(combatChar.GetAttackSkillList());
			skillIdList.RemoveAll((short id) => id < 0);
			bool flag = skillIdList.Count > 0;
			if (flag)
			{
				for (int i = 0; i < (int)poisonMarkList[4]; i++)
				{
					DomainManager.Combat.AddGoneMadInjury(context, combatChar, skillIdList.GetRandom(context.Random), 0);
				}
				DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1653, 0);
			}
			ObjectPool<List<short>>.Instance.Return(skillIdList);
			return true;
		}

		// Token: 0x0600683F RID: 26687 RVA: 0x003B44A0 File Offset: 0x003B26A0
		[MixPoisonEffect(10)]
		public static bool MixPoisonEffect125(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			DomainManager.Combat.AppendMindDefeatMark(context, combatChar, (int)(poisonMarkList[5] * 2), -1, false);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1652, 0);
			return true;
		}

		// Token: 0x06006840 RID: 26688 RVA: 0x003B44E0 File Offset: 0x003B26E0
		[MixPoisonEffect(5)]
		public static bool MixPoisonEffect134(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			int markCount = (int)poisonMarkList[4];
			DomainManager.Combat.PoisonProduce(context, combatChar, 4, markCount);
			combatChar.AddPoisonAffectValue(0, (short)(markCount + 1), true);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1647, 0);
			return true;
		}

		// Token: 0x06006841 RID: 26689 RVA: 0x003B452C File Offset: 0x003B272C
		[MixPoisonEffect(19)]
		public static bool MixPoisonEffect135(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			int markCount = (int)poisonMarkList[5];
			DomainManager.Combat.PoisonProduce(context, combatChar, 1, markCount);
			combatChar.AddPoisonAffectValue(2, (short)(20 * markCount), true);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1661, 0);
			return true;
		}

		// Token: 0x06006842 RID: 26690 RVA: 0x003B4578 File Offset: 0x003B2778
		[MixPoisonEffect(17)]
		public static bool MixPoisonEffect145(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			short skillId = combatChar.GetRandomBanableSkillId(context.Random, null, 3);
			bool flag = skillId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.Combat.ClearAffectingDefenseSkill(context, combatChar);
				DomainManager.Combat.SilenceSkill(context, combatChar, skillId, (int)((short)(600 * (int)(poisonMarkList[4] + poisonMarkList[5]))), 100);
				DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1659, 0);
				result = true;
			}
			return result;
		}

		// Token: 0x06006843 RID: 26691 RVA: 0x003B45E8 File Offset: 0x003B27E8
		[MixPoisonEffect(6)]
		public unsafe static bool MixPoisonEffect234(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			int poisonValue = *combatChar.GetPoison()[4];
			sbyte currLevel = PoisonsAndLevels.CalcPoisonedLevel(poisonValue);
			int spreadValue = poisonValue * 10 * (int)poisonMarkList[4] / 100;
			int[] charIdList = combatChar.IsAlly ? DomainManager.Combat.GetSelfTeam() : DomainManager.Combat.GetEnemyTeam();
			DomainManager.Combat.AddCombatState(context, combatChar, 2, 146, (int)(100 * poisonMarkList[4]));
			foreach (int charId in charIdList)
			{
				bool flag = charId < 0 || charId == combatChar.GetId();
				if (!flag)
				{
					DomainManager.Combat.AddPoison(context, combatChar, DomainManager.Combat.GetElement_CombatCharacterDict(charId), 4, currLevel, spreadValue, -1, true, false, default(ItemKey), false, false, false);
				}
			}
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1648, 0);
			return true;
		}

		// Token: 0x06006844 RID: 26692 RVA: 0x003B46D4 File Offset: 0x003B28D4
		[MixPoisonEffect(16)]
		public unsafe static bool MixPoisonEffect235(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			int poisonValue = *combatChar.GetPoison()[5];
			sbyte currLevel = PoisonsAndLevels.CalcPoisonedLevel(poisonValue);
			int spreadValue = poisonValue * 10 * (int)poisonMarkList[5] / 100;
			int[] charIdList = combatChar.IsAlly ? DomainManager.Combat.GetSelfTeam() : DomainManager.Combat.GetEnemyTeam();
			DomainManager.Combat.AddCombatState(context, combatChar, 2, 147, (int)(100 * poisonMarkList[5]));
			foreach (int charId in charIdList)
			{
				bool flag = charId < 0 || charId == combatChar.GetId();
				if (!flag)
				{
					DomainManager.Combat.AddPoison(context, combatChar, DomainManager.Combat.GetElement_CombatCharacterDict(charId), 5, currLevel, spreadValue, -1, true, false, default(ItemKey), false, false, false);
				}
			}
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1658, 0);
			return true;
		}

		// Token: 0x06006845 RID: 26693 RVA: 0x003B47C0 File Offset: 0x003B29C0
		[MixPoisonEffect(14)]
		public static bool MixPoisonEffect245(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			short skillId = combatChar.GetRandomBanableSkillId(context.Random, null, 4);
			bool flag = skillId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.Combat.ClearAffectingDefenseSkill(context, combatChar);
				DomainManager.Combat.SilenceSkill(context, combatChar, skillId, (int)((short)(600 * (int)(poisonMarkList[4] + poisonMarkList[5]))), 100);
				DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1656, 0);
				result = true;
			}
			return result;
		}

		// Token: 0x06006846 RID: 26694 RVA: 0x003B4830 File Offset: 0x003B2A30
		[MixPoisonEffect(4)]
		public static bool MixPoisonEffect345(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
		{
			short skillId = combatChar.GetRandomBanableSkillId(context.Random, null, 1);
			bool flag = skillId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.Combat.ClearAffectingDefenseSkill(context, combatChar);
				DomainManager.Combat.SilenceSkill(context, combatChar, skillId, (int)((short)(600 * (int)(poisonMarkList[4] + poisonMarkList[5]))), 100);
				DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1646, 0);
				result = true;
			}
			return result;
		}
	}
}
