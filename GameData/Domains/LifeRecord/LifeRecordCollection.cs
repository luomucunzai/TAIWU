using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.LifeRecord
{
	// Token: 0x02000656 RID: 1622
	[SerializableGameData(NotForDisplayModule = true)]
	public class LifeRecordCollection : WriteableRecordCollection
	{
		// Token: 0x06004969 RID: 18793 RVA: 0x00298983 File Offset: 0x00296B83
		public LifeRecordCollection() : this(65536)
		{
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x00298992 File Offset: 0x00296B92
		public LifeRecordCollection(int initialCapacity) : base(initialCapacity)
		{
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x002989A0 File Offset: 0x00296BA0
		private unsafe int BeginAddingRecord(int selfCharId, int date, short type)
		{
			int offset = this.Size;
			int newSize = this.Size + 4 + 1 + 4 + 2;
			base.EnsureCapacity(newSize);
			this.Size = newSize;
			byte[] array;
			byte* pRawData;
			if ((array = this.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			byte* pCurrData = pRawData + offset;
			*(int*)pCurrData = selfCharId;
			*(int*)(pCurrData + 4 + 1) = date;
			*(short*)(pCurrData + 4 + 1 + 4) = type;
			array = null;
			return offset + 4;
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x00298A18 File Offset: 0x00296C18
		private static Exception CreateRelatedRecordIdException(short relatedRecordId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unsupported relatedRecordId: ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(relatedRecordId);
			return new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x00298A58 File Offset: 0x00296C58
		public void AddDie(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 0);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x00298A80 File Offset: 0x00296C80
		public void AddXiangshuPartiallyInfected(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x00298AA8 File Offset: 0x00296CA8
		public void AddXiangshuCompletelyInfected(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 2);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x00298AD0 File Offset: 0x00296CD0
		public void AddMotherLoseFetus(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 3);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x00298AF8 File Offset: 0x00296CF8
		public void AddFatherLoseFetus(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 4);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x00298B2C File Offset: 0x00296D2C
		public void AddAbandonChild(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 5);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 6);
			base.AppendCharacter(selfCharId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x00298B80 File Offset: 0x00296D80
		public void AddGiveBirthToCricket(int selfCharId, int date, Location location, short colorId, short partId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 7);
			base.AppendLocation(location);
			base.AppendCricket(colorId, partId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x00298BB4 File Offset: 0x00296DB4
		public void AddGiveBirthToBoy(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 8);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x00298BE8 File Offset: 0x00296DE8
		public void AddGiveBirthToGirl(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 9);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x00298C1C File Offset: 0x00296E1C
		public void AddBecomeFatherToNewBornBoy(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 10);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x00298C58 File Offset: 0x00296E58
		public void AddBecomeFatherToNewBornGirl(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 11);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x00298C94 File Offset: 0x00296E94
		public void AddBuildGrave(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 12);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x00298CC8 File Offset: 0x00296EC8
		public void AddMonkBreakRule(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 13);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00298CFC File Offset: 0x00296EFC
		public void AddKidnappedCharacterEscaped(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 14);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 15);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x00298D54 File Offset: 0x00296F54
		public void AddReadBookSucceed(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 16);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00298D94 File Offset: 0x00296F94
		public void AddReadBookFail(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 17);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00298DD4 File Offset: 0x00296FD4
		public void AddBreakoutSucceed(int selfCharId, int date, Location location, short combatSkillTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 18);
			base.AppendLocation(location);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x00298E08 File Offset: 0x00297008
		public void AddBreakoutFail(int selfCharId, int date, Location location, short combatSkillTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 19);
			base.AppendLocation(location);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x00298E3C File Offset: 0x0029703C
		public void AddLearnCombatSkill(int selfCharId, int date, Location location, short combatSkillTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 20);
			base.AppendLocation(location);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x00298E70 File Offset: 0x00297070
		public void AddLearnLifeSkill(int selfCharId, int date, Location location, short lifeSkillTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 21);
			base.AppendLocation(location);
			base.AppendLifeSkill(lifeSkillTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x00298EA4 File Offset: 0x002970A4
		public void AddRepairItem(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 22);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x00298ED8 File Offset: 0x002970D8
		public void AddAddPoisonToItem(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 23);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x00298F18 File Offset: 0x00297118
		public void AddLoseOverloadingResource(int selfCharId, int date, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 24);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x00298F54 File Offset: 0x00297154
		public void AddLoseOverloadingItem(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 25);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00298F88 File Offset: 0x00297188
		public void AddMakeEnemy(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 26);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 28);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x00298FE0 File Offset: 0x002971E0
		public void AddSeverEnemy(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 27);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 29);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x00299038 File Offset: 0x00297238
		public void AddAdore(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 30);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x0029906C File Offset: 0x0029726C
		public void AddLoveAtFirstSight(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 31);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 31);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x002990C4 File Offset: 0x002972C4
		public void AddConfessLoveSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 32);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 34);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x0029911C File Offset: 0x0029731C
		public void AddConfessLoveFail(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 33);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 35);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x00299174 File Offset: 0x00297374
		public void AddBreakupMutually(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 36);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 36);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x002991CC File Offset: 0x002973CC
		public void AddDumpLover(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 37);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 38);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00299224 File Offset: 0x00297424
		public void AddProposeMarriageSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 39);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 39);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x0029927C File Offset: 0x0029747C
		public void AddProposeMarriageFail(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 40);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 41);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x002992D4 File Offset: 0x002974D4
		public void AddBecomeFriend(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 42);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 42);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x0029932C File Offset: 0x0029752C
		public void AddSeverFriendship(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 43);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 43);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x00299384 File Offset: 0x00297584
		public void AddBecomeSwornBrotherOrSister(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 44);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 44);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x002993DC File Offset: 0x002975DC
		public void AddSeverSwornBrotherhood(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 45);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 45);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x00299434 File Offset: 0x00297634
		public void AddGetAdoptedByFather(int selfCharId, int date, int charId, Location location, short relatedRecordId)
		{
			bool flag = relatedRecordId != 48 && relatedRecordId != 49;
			if (flag)
			{
				throw LifeRecordCollection.CreateRelatedRecordIdException(relatedRecordId);
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 46);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, relatedRecordId);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x002994A8 File Offset: 0x002976A8
		public void AddGetAdoptedByMother(int selfCharId, int date, int charId, Location location, short relatedRecordId)
		{
			bool flag = relatedRecordId != 48 && relatedRecordId != 49;
			if (flag)
			{
				throw LifeRecordCollection.CreateRelatedRecordIdException(relatedRecordId);
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 47);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, relatedRecordId);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x0029951C File Offset: 0x0029771C
		public void AddAdoptSon(int selfCharId, int date, int charId, Location location, short relatedRecordId)
		{
			bool flag = relatedRecordId != 46 && relatedRecordId != 47;
			if (flag)
			{
				throw LifeRecordCollection.CreateRelatedRecordIdException(relatedRecordId);
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 48);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, relatedRecordId);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00299590 File Offset: 0x00297790
		public void AddAdoptDaughter(int selfCharId, int date, int charId, Location location, short relatedRecordId)
		{
			bool flag = relatedRecordId != 46 && relatedRecordId != 47;
			if (flag)
			{
				throw LifeRecordCollection.CreateRelatedRecordIdException(relatedRecordId);
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 49);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, relatedRecordId);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00299604 File Offset: 0x00297804
		public void AddCreateFaction(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 50);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00299630 File Offset: 0x00297830
		public void AddJoinFaction(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 51);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x00299664 File Offset: 0x00297864
		public void AddLeaveFaction(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 52);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x00299698 File Offset: 0x00297898
		public void AddFactionRecruitSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 53);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 55);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x002996F0 File Offset: 0x002978F0
		public void AddFactionRecruitFail(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 54);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 56);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x00299748 File Offset: 0x00297948
		public void AddDecideToJoinSect(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 57);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x0029977C File Offset: 0x0029797C
		public void AddDecideToFullfillAppointment(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 58);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x002997A8 File Offset: 0x002979A8
		public void AddDecideToProtect(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 59);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x002997DC File Offset: 0x002979DC
		public void AddDecideToRescue(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 60);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00299810 File Offset: 0x00297A10
		public void AddDecideToMourn(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 61);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x00299844 File Offset: 0x00297A44
		public void AddDecideToVisit(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 62);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x00299878 File Offset: 0x00297A78
		public void AddDecideToFindLostItem(int selfCharId, int date, Location location, Location location1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 63);
			base.AppendLocation(location);
			base.AppendLocation(location1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x002998AC File Offset: 0x00297AAC
		public void AddDecideToFindSpecialMaterial(int selfCharId, int date, Location location, Location location1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 64);
			base.AppendLocation(location);
			base.AppendLocation(location1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x002998E0 File Offset: 0x00297AE0
		public void AddDecideToRevenge(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 65);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x00299914 File Offset: 0x00297B14
		public void AddDecideToParticipateAdventure(int selfCharId, int date, Location location, short adventureTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 66);
			base.AppendLocation(location);
			base.AppendAdventure(adventureTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x00299948 File Offset: 0x00297B48
		public void AddJoinSectFail(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 67);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x00299974 File Offset: 0x00297B74
		public void AddJoinSectSucceed(int selfCharId, int date, Location location, short settlementId, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 68);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x002999B8 File Offset: 0x00297BB8
		public void AddCanNoLongerFullFillAppointment(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 69);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x002999E4 File Offset: 0x00297BE4
		public void AddWaitForAppointment(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 70);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x00299A18 File Offset: 0x00297C18
		public void AddFullFillAppointment(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 71);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049AB RID: 18859 RVA: 0x00299A4C File Offset: 0x00297C4C
		public void AddFinishProtection(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 72);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049AC RID: 18860 RVA: 0x00299A80 File Offset: 0x00297C80
		public void AddOfferProtection(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 73);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x00299AB4 File Offset: 0x00297CB4
		public void AddFinishRescue(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 74);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x00299AE8 File Offset: 0x00297CE8
		public void AddFinishMourning(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 75);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049AF RID: 18863 RVA: 0x00299B1C File Offset: 0x00297D1C
		public void AddMaintainGrave(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 76);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049B0 RID: 18864 RVA: 0x00299B50 File Offset: 0x00297D50
		public void AddUpgradeGrave(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 77);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x00299B84 File Offset: 0x00297D84
		public void AddFinishVisit(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 78);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049B2 RID: 18866 RVA: 0x00299BB8 File Offset: 0x00297DB8
		public void AddFinishFIndingLostItem(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 79);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049B3 RID: 18867 RVA: 0x00299BE4 File Offset: 0x00297DE4
		public void AddFinishFIndingSpecialMaterial(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 80);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x00299C10 File Offset: 0x00297E10
		public void AddFindLostItemSucceed(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 81);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x00299C44 File Offset: 0x00297E44
		public void AddFindLostItemFail(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 82);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x00299C70 File Offset: 0x00297E70
		public void AddFindSpecialMaterialSucceed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 83);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x00299C9C File Offset: 0x00297E9C
		public void AddFinishTakingRevenge(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 84);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x00299CD0 File Offset: 0x00297ED0
		public void AddMajorVictoryInCombat(int selfCharId, int date, int charId, Location location, sbyte combatType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 85);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 86);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049B9 RID: 18873 RVA: 0x00299D38 File Offset: 0x00297F38
		public void AddMajorFailureInCombat(int selfCharId, int date, int charId, Location location, sbyte combatType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 86);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 85);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049BA RID: 18874 RVA: 0x00299DA0 File Offset: 0x00297FA0
		public void AddVictoryInCombat(int selfCharId, int date, int charId, Location location, sbyte combatType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 87);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 88);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049BB RID: 18875 RVA: 0x00299E08 File Offset: 0x00298008
		public void AddFailureInCombat(int selfCharId, int date, int charId, Location location, sbyte combatType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 88);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 87);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049BC RID: 18876 RVA: 0x00299E70 File Offset: 0x00298070
		public void AddEnemyEscape(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 89);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 90);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049BD RID: 18877 RVA: 0x00299EC8 File Offset: 0x002980C8
		public void AddLoseAndEscape(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 90);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 89);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x00299F20 File Offset: 0x00298120
		public void AddKillInPublic(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 91);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 713);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x00299F7C File Offset: 0x0029817C
		public void AddKillInPrivate(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 92);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 712);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x00299FD8 File Offset: 0x002981D8
		public void AddKidnapInPublic(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 93);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 96);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x0029A044 File Offset: 0x00298244
		public void AddKidnapInPrivate(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 94);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 97);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x0029A0B0 File Offset: 0x002982B0
		public void AddReleaseLoser(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 95);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 98);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049C3 RID: 18883 RVA: 0x0029A108 File Offset: 0x00298308
		public void AddAgreeToProtect(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 99);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049C4 RID: 18884 RVA: 0x0029A13C File Offset: 0x0029833C
		public void AddRefuseToProtect(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 100);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049C5 RID: 18885 RVA: 0x0029A170 File Offset: 0x00298370
		public void AddFinishAdventure(int selfCharId, int date, Location location, short adventureTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 101);
			base.AppendLocation(location);
			base.AppendAdventure(adventureTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049C6 RID: 18886 RVA: 0x0029A1A4 File Offset: 0x002983A4
		public void AddRequestHealOuterInjurySucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 102);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 138);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049C7 RID: 18887 RVA: 0x0029A214 File Offset: 0x00298414
		public void AddRequestHealInnerInjurySucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 103);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 139);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049C8 RID: 18888 RVA: 0x0029A284 File Offset: 0x00298484
		public void AddRequestDetoxPoisonSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, sbyte poisonType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 104);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendPoisonType(poisonType);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 140);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendPoisonType(poisonType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x0029A308 File Offset: 0x00298508
		public void AddRequestHealthSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 105);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 141);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x0029A378 File Offset: 0x00298578
		public void AddRequestHealDisorderOfQiSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 106);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 142);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049CB RID: 18891 RVA: 0x0029A3E8 File Offset: 0x002985E8
		public void AddRequestNeiliSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 107);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 143);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049CC RID: 18892 RVA: 0x0029A458 File Offset: 0x00298658
		public void AddRequestKillWugSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 108);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 144);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x0029A4E0 File Offset: 0x002986E0
		public void AddRequestFoodSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 109);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 145);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x0029A550 File Offset: 0x00298750
		public void AddRequestTeaWineSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 110);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 146);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x0029A5C0 File Offset: 0x002987C0
		public void AddRequestResourceSucceed(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 111);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 147);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x0029A640 File Offset: 0x00298840
		public void AddRequestItemSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 112);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 148);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x0029A6B0 File Offset: 0x002988B0
		public void AddRequestRepairItemSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 113);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 149);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x0029A720 File Offset: 0x00298920
		public void AddRequestAddPoisonToItemSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 114);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 150);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x0029A790 File Offset: 0x00298990
		public void AddRequestInstructionOnLifeSkillSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 115);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 151);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x0029A814 File Offset: 0x00298A14
		public void AddRequestInstructionOnCombatSkillSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 116);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 152);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x0029A898 File Offset: 0x00298A98
		public void AddRequestInstructionOnLifeSkillFailToLearn(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 117);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 153);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x0029A91C File Offset: 0x00298B1C
		public void AddRequestInstructionOnCombatSkillFailToLearn(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 118);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 154);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x0029A9A0 File Offset: 0x00298BA0
		public void AddRequestInstructionOnReadingSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 119);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 155);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x0029AA24 File Offset: 0x00298C24
		public void AddRequestInstructionOnBreakoutSucceed(int selfCharId, int date, int charId, Location location, short combatSkillTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 120);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 156);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x0029AA90 File Offset: 0x00298C90
		public void AddRequestHealOuterInjuryFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 121);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 157);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x0029AB00 File Offset: 0x00298D00
		public void AddRequestHealInnerInjuryFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 122);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 158);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x0029AB70 File Offset: 0x00298D70
		public void AddRequestDetoxPoisonFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, sbyte poisonType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 123);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendPoisonType(poisonType);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 159);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendPoisonType(poisonType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x0029ABF4 File Offset: 0x00298DF4
		public void AddRequestHealthFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 124);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 160);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049DD RID: 18909 RVA: 0x0029AC64 File Offset: 0x00298E64
		public void AddRequestHealDisorderOfQiFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 125);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 161);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049DE RID: 18910 RVA: 0x0029ACD4 File Offset: 0x00298ED4
		public void AddRequestNeiliFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 126);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 162);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049DF RID: 18911 RVA: 0x0029AD44 File Offset: 0x00298F44
		public void AddRequestKillWugFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 127);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 163);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049E0 RID: 18912 RVA: 0x0029ADCC File Offset: 0x00298FCC
		public void AddRequestFoodFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 128);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 164);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049E1 RID: 18913 RVA: 0x0029AE40 File Offset: 0x00299040
		public void AddRequestTeaWineFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 129);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 165);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x0029AEB4 File Offset: 0x002990B4
		public void AddRequestResourceFail(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 130);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 166);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049E3 RID: 18915 RVA: 0x0029AF34 File Offset: 0x00299134
		public void AddRequestItemFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 131);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 167);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049E4 RID: 18916 RVA: 0x0029AFA8 File Offset: 0x002991A8
		public void AddRequestRepairItemFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 132);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 168);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x0029B01C File Offset: 0x0029921C
		public void AddRequestAddPoisonToItemFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 133);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 169);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x0029B090 File Offset: 0x00299290
		public void AddRequestInstructionOnLifeSkillFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 134);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 170);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x0029B114 File Offset: 0x00299314
		public void AddRequestInstructionOnCombatSkillFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 135);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 171);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049E8 RID: 18920 RVA: 0x0029B198 File Offset: 0x00299398
		public void AddRequestInstructionOnReadingFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 136);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 172);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x0029B21C File Offset: 0x0029941C
		public void AddRequestInstructionOnBreakoutFail(int selfCharId, int date, int charId, Location location, short combatSkillTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 137);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 173);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x0029B28C File Offset: 0x0029948C
		public void AddRescueKidnappedCharacterSecretlyFail1(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 174);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x0029B2CC File Offset: 0x002994CC
		public void AddRescueKidnappedCharacterSecretlyFail2(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 175);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x0029B30C File Offset: 0x0029950C
		public void AddRescueKidnappedCharacterSecretlyFail3(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 176);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x0029B34C File Offset: 0x0029954C
		public void AddRescueKidnappedCharacterSecretlyFail4(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 177);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x0029B38C File Offset: 0x0029958C
		public void AddRescueKidnappedCharacterSecretlySucceed(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 178);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 180);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x0029B3FC File Offset: 0x002995FC
		public void AddRescueKidnappedCharacterSecretlySucceedAndEscaped(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 179);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 180);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x0029B46C File Offset: 0x0029966C
		public void AddRescueKidnappedCharacterWithWitFail1(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 181);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x0029B4AC File Offset: 0x002996AC
		public void AddRescueKidnappedCharacterWithWitFail2(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 182);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x0029B4EC File Offset: 0x002996EC
		public void AddRescueKidnappedCharacterWithWitFail3(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 183);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x0029B52C File Offset: 0x0029972C
		public void AddRescueKidnappedCharacterWithWitFail4(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 184);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x0029B56C File Offset: 0x0029976C
		public void AddRescueKidnappedCharacterWithWitSucceed(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 185);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 187);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x0029B5DC File Offset: 0x002997DC
		public void AddRescueKidnappedCharacterWithWitSucceedAndEscaped(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 186);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 187);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x0029B64C File Offset: 0x0029984C
		public void AddRescueKidnappedCharacterWithForceFail1(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 188);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x0029B68C File Offset: 0x0029988C
		public void AddRescueKidnappedCharacterWithForceFail2(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 189);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x0029B6CC File Offset: 0x002998CC
		public void AddRescueKidnappedCharacterWithForceFail3(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 190);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x0029B70C File Offset: 0x0029990C
		public void AddRescueKidnappedCharacterWithForceFail4(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 191);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x0029B74C File Offset: 0x0029994C
		public void AddRescueKidnappedCharacterWithForceSucceed(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 192);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 194);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x0029B7BC File Offset: 0x002999BC
		public void AddRescueKidnappedCharacterWithForceSucceedAndEscaped(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 193);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 194);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x0029B82C File Offset: 0x00299A2C
		public void AddPoisonEnemyFail1(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 195);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x0029B86C File Offset: 0x00299A6C
		public void AddPoisonEnemyFail2(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 196);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x0029B8AC File Offset: 0x00299AAC
		public void AddPoisonEnemyFail3(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 197);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x0029B8EC File Offset: 0x00299AEC
		public void AddPoisonEnemyFail4(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 198);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x0029B92C File Offset: 0x00299B2C
		public void AddPoisonEnemySucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 199);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 201);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x0029B9A0 File Offset: 0x00299BA0
		public void AddPoisonEnemySucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 200);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 201);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x0029BA14 File Offset: 0x00299C14
		public void AddPlotHarmEnemyFail1(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 202);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x0029BA4C File Offset: 0x00299C4C
		public void AddPlotHarmEnemyFail2(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 203);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x0029BA84 File Offset: 0x00299C84
		public void AddPlotHarmEnemyFail3(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 204);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x0029BABC File Offset: 0x00299CBC
		public void AddPlotHarmEnemyFail4(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 205);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x0029BAF4 File Offset: 0x00299CF4
		public void AddPlotHarmEnemySucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 206);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 208);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x0029BB50 File Offset: 0x00299D50
		public void AddPlotHarmEnemySucceedAndEscaped(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 207);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 208);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x0029BBAC File Offset: 0x00299DAC
		public void AddStealResourceFail1(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 209);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x0029BBEC File Offset: 0x00299DEC
		public void AddStealResourceFail2(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 210);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x0029BC2C File Offset: 0x00299E2C
		public void AddStealResourceFail3(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 211);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x0029BC6C File Offset: 0x00299E6C
		public void AddStealResourceFail4(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 212);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x0029BCAC File Offset: 0x00299EAC
		public void AddStealResourceSucceed(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 213);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 216);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x0029BD2C File Offset: 0x00299F2C
		public void AddStealResourceSucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 214);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 216);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x0029BDAC File Offset: 0x00299FAC
		public void AddStealResourceFailAndBeatenUp(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 215);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 217);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x0029BE2C File Offset: 0x0029A02C
		public void AddScamResourceFail1(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 218);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x0029BE6C File Offset: 0x0029A06C
		public void AddScamResourceFail2(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 219);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x0029BEAC File Offset: 0x0029A0AC
		public void AddScamResourceFail3(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 220);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x0029BEEC File Offset: 0x0029A0EC
		public void AddScamResourceFail4(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 221);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x0029BF2C File Offset: 0x0029A12C
		public void AddScamResourceSucceed(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 222);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 225);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x0029BFAC File Offset: 0x0029A1AC
		public void AddScamResourceSucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 223);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 225);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x0029C02C File Offset: 0x0029A22C
		public void AddScamResourceFailAndBeatenUp(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 224);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 226);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x0029C0AC File Offset: 0x0029A2AC
		public void AddRobResourceFail1(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 227);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x0029C0EC File Offset: 0x0029A2EC
		public void AddRobResourceFail2(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 228);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x0029C12C File Offset: 0x0029A32C
		public void AddRobResourceFail3(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 229);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x0029C16C File Offset: 0x0029A36C
		public void AddRobResourceFail4(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 230);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x0029C1AC File Offset: 0x0029A3AC
		public void AddRobResourceSucceed(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 231);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 234);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x0029C22C File Offset: 0x0029A42C
		public void AddRobResourceSucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 232);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 234);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x0029C2AC File Offset: 0x0029A4AC
		public void AddRobResourceFailAndBeatenUp(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 233);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 235);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x0029C32C File Offset: 0x0029A52C
		public void AddStealItemFail1(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 236);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x0029C36C File Offset: 0x0029A56C
		public void AddStealItemFail2(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 237);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x0029C3AC File Offset: 0x0029A5AC
		public void AddStealItemFail3(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 238);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x0029C3EC File Offset: 0x0029A5EC
		public void AddStealItemFail4(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 239);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x0029C42C File Offset: 0x0029A62C
		public void AddStealItemSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 240);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 243);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x0029C4A0 File Offset: 0x0029A6A0
		public void AddStealItemSucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 241);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 243);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x0029C514 File Offset: 0x0029A714
		public void AddStealItemSucceedAndBeatenUp(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 242);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 244);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x0029C588 File Offset: 0x0029A788
		public void AddScamItemFail1(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 245);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x0029C5C8 File Offset: 0x0029A7C8
		public void AddScamItemFail2(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 246);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x0029C608 File Offset: 0x0029A808
		public void AddScamItemFail3(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 247);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x0029C648 File Offset: 0x0029A848
		public void AddScamItemFail4(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 248);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x0029C688 File Offset: 0x0029A888
		public void AddScamItemSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 249);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 252);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x0029C6FC File Offset: 0x0029A8FC
		public void AddScamItemSucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 250);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 252);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x0029C770 File Offset: 0x0029A970
		public void AddScamItemFailAndBeatenUp(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 251);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 253);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x0029C7E4 File Offset: 0x0029A9E4
		public void AddRobItemFail1(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 254);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x0029C824 File Offset: 0x0029AA24
		public void AddRobItemFail2(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 255);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A2D RID: 18989 RVA: 0x0029C864 File Offset: 0x0029AA64
		public void AddRobItemFail3(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 256);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x0029C8A4 File Offset: 0x0029AAA4
		public void AddRobItemFail4(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 257);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A2F RID: 18991 RVA: 0x0029C8E4 File Offset: 0x0029AAE4
		public void AddRobItemSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 258);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 261);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A30 RID: 18992 RVA: 0x0029C958 File Offset: 0x0029AB58
		public void AddRobItemSucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 259);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 261);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x0029C9CC File Offset: 0x0029ABCC
		public void AddRobItemFailAndBeatenUp(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 260);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 262);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x0029CA40 File Offset: 0x0029AC40
		public void AddRobResourceFromGraveSucceed(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 263);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x0029CA88 File Offset: 0x0029AC88
		public void AddRobResourceFromGraveFail(int selfCharId, int date, int charId, Location location, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 264);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x0029CAC8 File Offset: 0x0029ACC8
		public void AddRobItemFromGraveSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 265);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x0029CB08 File Offset: 0x0029AD08
		public void AddRobItemFromGraveFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 266);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A36 RID: 18998 RVA: 0x0029CB48 File Offset: 0x0029AD48
		public void AddStealLifeSkillFail1(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 267);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A37 RID: 18999 RVA: 0x0029CB94 File Offset: 0x0029AD94
		public void AddStealLifeSkillFail2(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 268);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A38 RID: 19000 RVA: 0x0029CBE0 File Offset: 0x0029ADE0
		public void AddStealLifeSkillFail3(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 269);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x0029CC2C File Offset: 0x0029AE2C
		public void AddStealLifeSkillFail4(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 270);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x0029CC78 File Offset: 0x0029AE78
		public void AddStealLifeSkillSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 271);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 273);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A3B RID: 19003 RVA: 0x0029CCFC File Offset: 0x0029AEFC
		public void AddStealLifeSkillSucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 272);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 273);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A3C RID: 19004 RVA: 0x0029CD80 File Offset: 0x0029AF80
		public void AddScamLifeSkillFail1(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 274);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x0029CDCC File Offset: 0x0029AFCC
		public void AddScamLifeSkillFail2(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 275);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x0029CE18 File Offset: 0x0029B018
		public void AddScamLifeSkillFail3(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 276);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x0029CE64 File Offset: 0x0029B064
		public void AddScamLifeSkillFail4(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 277);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A40 RID: 19008 RVA: 0x0029CEB0 File Offset: 0x0029B0B0
		public void AddScamLifeSkillSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 278);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 280);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x0029CF34 File Offset: 0x0029B134
		public void AddScamLifeSkillSucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 279);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 280);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x0029CFB8 File Offset: 0x0029B1B8
		public void AddStealCombatSkillFail1(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 281);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x0029D004 File Offset: 0x0029B204
		public void AddStealCombatSkillFail2(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 282);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x0029D050 File Offset: 0x0029B250
		public void AddStealCombatSkillFail3(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 283);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x0029D09C File Offset: 0x0029B29C
		public void AddStealCombatSkillFail4(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 284);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x0029D0E8 File Offset: 0x0029B2E8
		public void AddStealCombatSkillSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 285);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 287);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x0029D16C File Offset: 0x0029B36C
		public void AddStealCombatSkillSucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 286);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 287);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A48 RID: 19016 RVA: 0x0029D1F0 File Offset: 0x0029B3F0
		public void AddScamCombatSkillFail1(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 288);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x0029D23C File Offset: 0x0029B43C
		public void AddScamCombatSkillFail2(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 289);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x0029D288 File Offset: 0x0029B488
		public void AddScamCombatSkillFail3(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 290);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A4B RID: 19019 RVA: 0x0029D2D4 File Offset: 0x0029B4D4
		public void AddScamCombatSkillFail4(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 291);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x0029D320 File Offset: 0x0029B520
		public void AddScamCombatSkillSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 292);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 294);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x0029D3A4 File Offset: 0x0029B5A4
		public void AddScamCombatSkillSucceedAndEscaped(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 293);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 294);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x0029D428 File Offset: 0x0029B628
		public void AddLifeSkillBattleWin(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 295);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x0029D460 File Offset: 0x0029B660
		public void AddLifeSkillBattleLose(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 296);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x0029D498 File Offset: 0x0029B698
		public void AddExchangeResource(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value, sbyte resourceType1, int value1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 297);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.AppendResource(resourceType1);
			base.AppendInteger(value1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x0029D4F4 File Offset: 0x0029B6F4
		public void AddGiveResource(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 298);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 303);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x0029D574 File Offset: 0x0029B774
		public void AddPurchaseItem(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 299);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A53 RID: 19027 RVA: 0x0029D5C8 File Offset: 0x0029B7C8
		public void AddSellItem(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 300);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x0029D61C File Offset: 0x0029B81C
		public void AddGiveItem(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 301);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 304);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x0029D690 File Offset: 0x0029B890
		public void AddGivePoisonousItem(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 302);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 305);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x0029D704 File Offset: 0x0029B904
		public void AddRefusePoisonousGift(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 305);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 302);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x0029D778 File Offset: 0x0029B978
		public void AddLearnLifeSkillWithInstructionSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 308);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 306);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x0029D7FC File Offset: 0x0029B9FC
		public void AddLearnLifeSkillWithInstructionFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 309);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 306);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x0029D880 File Offset: 0x0029BA80
		public void AddLearnCombatSkillWithInstructionSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 310);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 307);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x0029D904 File Offset: 0x0029BB04
		public void AddLearnCombatSkillWithInstructionFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 311);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 307);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x0029D988 File Offset: 0x0029BB88
		public void AddInviteToDrinkSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 312);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 327);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x0029D9FC File Offset: 0x0029BBFC
		public void AddInviteToDrinkFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 313);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 328);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x0029DA70 File Offset: 0x0029BC70
		public void AddSellSucceed(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 314);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 329);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x0029DAE4 File Offset: 0x0029BCE4
		public void AddSellFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 315);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 330);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x0029DB58 File Offset: 0x0029BD58
		public void AddCureSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 316);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 331);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x0029DBB4 File Offset: 0x0029BDB4
		public void AddRepairItemSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 317);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 332);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x0029DC10 File Offset: 0x0029BE10
		public void AddBarbSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 318);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 333);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A62 RID: 19042 RVA: 0x0029DC6C File Offset: 0x0029BE6C
		public void AddBarbMistake(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 319);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 334);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x0029DCC8 File Offset: 0x0029BEC8
		public void AddBarbFail(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 320);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 335);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x0029DD24 File Offset: 0x0029BF24
		public void AddAskForMoneySucceed(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 321);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 336);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x0029DDA4 File Offset: 0x0029BFA4
		public void AddAskForMoneyFail(int selfCharId, int date, int charId, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 322);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 337);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x0029DE24 File Offset: 0x0029C024
		public void AddEntertainWithMusic(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 323);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 338);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x0029DE80 File Offset: 0x0029C080
		public void AddEntertainWithChess(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 324);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 339);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x0029DEDC File Offset: 0x0029C0DC
		public void AddEntertainWithPoem(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 325);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 340);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x0029DF38 File Offset: 0x0029C138
		public void AddEntertainWithPainting(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 326);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 341);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x0029DF94 File Offset: 0x0029C194
		public void AddMakeItem(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 342);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x0029DFCC File Offset: 0x0029C1CC
		public void AddTaoismAwakeningSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 343);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 347);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A6C RID: 19052 RVA: 0x0029E028 File Offset: 0x0029C228
		public void AddTaoismAwakeningFail(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 344);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 348);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A6D RID: 19053 RVA: 0x0029E084 File Offset: 0x0029C284
		public void AddBuddismAwakeningSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 345);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 349);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A6E RID: 19054 RVA: 0x0029E0E0 File Offset: 0x0029C2E0
		public void AddBuddismAwakeningFail(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 346);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 350);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x0029E13C File Offset: 0x0029C33C
		public void AddCollectTeaWineSucceed(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 351);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x0029E174 File Offset: 0x0029C374
		public void AddCollectTeaWineFail(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 352);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x0029E1A0 File Offset: 0x0029C3A0
		public void AddDivinationSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 353);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x0029E1D8 File Offset: 0x0029C3D8
		public void AddDivinationFail(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 354);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x0029E204 File Offset: 0x0029C404
		public void AddCricketBattleWin(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 355);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 356);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x0029E260 File Offset: 0x0029C460
		public void AddCricketBattleLose(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 356);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 355);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A75 RID: 19061 RVA: 0x0029E2BC File Offset: 0x0029C4BC
		public void AddMakeLoveIllegal(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 357);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 357);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A76 RID: 19062 RVA: 0x0029E318 File Offset: 0x0029C518
		public void AddRapeFail(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 358);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 361);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x0029E374 File Offset: 0x0029C574
		public void AddRapeSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 359);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 362);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x0029E3D0 File Offset: 0x0029C5D0
		public void AddReleaseKidnappedCharacter(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 360);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 363);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x0029E42C File Offset: 0x0029C62C
		public void AddMerchantGetNewProduct(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 364);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x0029E458 File Offset: 0x0029C658
		public void AddUnexpectedResourceGain(int selfCharId, int date, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 365);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x0029E498 File Offset: 0x0029C698
		public void AddUnexpectedItemGain(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 366);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x0029E4D0 File Offset: 0x0029C6D0
		public void AddUnexpectedSkillBookGain(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 367);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x0029E508 File Offset: 0x0029C708
		public void AddUnexpectedHealthCure(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 368);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x0029E540 File Offset: 0x0029C740
		public void AddUnexpectedOuterInjuryCure(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 369);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x0029E578 File Offset: 0x0029C778
		public void AddUnexpectedInnerInjuryCure(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 370);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x0029E5B0 File Offset: 0x0029C7B0
		public void AddUnexpectedPoisonCure(int selfCharId, int date, Location location, sbyte poisonType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 371);
			base.AppendLocation(location);
			base.AppendPoisonType(poisonType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x0029E5E8 File Offset: 0x0029C7E8
		public void AddUnexpectedDisorderOfQiCure(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 372);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x0029E614 File Offset: 0x0029C814
		public void AddUnexpectedResourceLose(int selfCharId, int date, Location location, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 373);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x0029E654 File Offset: 0x0029C854
		public void AddUnexpectedItemLose(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 374);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x0029E68C File Offset: 0x0029C88C
		public void AddUnexpectedSkillBookLose(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 375);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x0029E6C4 File Offset: 0x0029C8C4
		public void AddUnexpectedHealthHarm(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 376);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x0029E6FC File Offset: 0x0029C8FC
		public void AddUnexpectedOuterInjuryHarm(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 377);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x0029E734 File Offset: 0x0029C934
		public void AddUnexpectedInnerInjuryHarm(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 378);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x0029E76C File Offset: 0x0029C96C
		public void AddUnexpectedPoisonHarm(int selfCharId, int date, Location location, sbyte poisonType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 379);
			base.AppendLocation(location);
			base.AppendPoisonType(poisonType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x0029E7A4 File Offset: 0x0029C9A4
		public void AddUnexpectedDisorderOfQiHarm(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 380);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x0029E7D0 File Offset: 0x0029C9D0
		public void AddKillHereticRandomEnemy(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 381);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x0029E808 File Offset: 0x0029CA08
		public void AddKillRighteousRandomEnemy(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 382);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x0029E840 File Offset: 0x0029CA40
		public void AddDefeatedByHereticRandomEnemy(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 383);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x0029E878 File Offset: 0x0029CA78
		public void AddDefeatedByRighteousRandomEnemy(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 384);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x0029E8B0 File Offset: 0x0029CAB0
		public void AddMonvBad(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 385);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x0029E8DC File Offset: 0x0029CADC
		public void AddDayueYaochangBad(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 386);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x0029E908 File Offset: 0x0029CB08
		public void AddJinHuangerBad(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 387);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x0029E934 File Offset: 0x0029CB34
		public void AddYiyihouBad(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 388);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x0029E960 File Offset: 0x0029CB60
		public void AddWeiQiBad(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 389);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x0029E98C File Offset: 0x0029CB8C
		public void AddYixiangBad(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 390);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x0029E9B8 File Offset: 0x0029CBB8
		public void AddShufangBad(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 391);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x0029E9E4 File Offset: 0x0029CBE4
		public void AddJixiBad(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 392);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x0029EA10 File Offset: 0x0029CC10
		public void AddMonvGood(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 393);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x0029EA3C File Offset: 0x0029CC3C
		public void AddDayueYaochangGood(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 394);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x0029EA68 File Offset: 0x0029CC68
		public void AddJinHuangerGood(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 395);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x0029EA94 File Offset: 0x0029CC94
		public void AddYiyihouGood(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 396);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x0029EAC0 File Offset: 0x0029CCC0
		public void AddWeiQiGood(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 397);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x0029EAEC File Offset: 0x0029CCEC
		public void AddYixiangGood(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 398);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x0029EB18 File Offset: 0x0029CD18
		public void AddXuefengGood(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 399);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x0029EB44 File Offset: 0x0029CD44
		public void AddShufangGood(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 400);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x0029EB70 File Offset: 0x0029CD70
		public void AddPregnantWithSamsara0(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 401);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x0029EB9C File Offset: 0x0029CD9C
		public void AddPregnantWithSamsara1(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 402);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x0029EBC8 File Offset: 0x0029CDC8
		public void AddPregnantWithSamsara2(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 403);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x0029EBF4 File Offset: 0x0029CDF4
		public void AddPregnantWithSamsara3(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 404);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x0029EC20 File Offset: 0x0029CE20
		public void AddPregnantWithSamsara4(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 405);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x0029EC4C File Offset: 0x0029CE4C
		public void AddPregnantWithSamsara5(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 406);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x0029EC78 File Offset: 0x0029CE78
		public void AddGainAuthority(int selfCharId, int date, int charId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 407);
			base.AppendCharacter(charId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x0029ECB0 File Offset: 0x0029CEB0
		public void AddSectPunishNormal(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 408);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x0029ECD4 File Offset: 0x0029CED4
		public void AddSectPunishElope(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 409);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x0029ECF8 File Offset: 0x0029CEF8
		public void AddExpelVillager(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 410);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 413);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x0029ED54 File Offset: 0x0029CF54
		public void AddSavedFromInfection(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 411);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 691);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x0029EDB0 File Offset: 0x0029CFB0
		public void AddChangeGrade(int selfCharId, int date, Location location, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 412);
			base.AppendLocation(location);
			base.AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x0029EDEC File Offset: 0x0029CFEC
		public void AddInsteadSectPunishElope(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 414);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x0029EE24 File Offset: 0x0029D024
		public void AddAvoidSectPunishElope(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 415);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x0029EE50 File Offset: 0x0029D050
		public void AddJoinJoustForSpouse(int selfCharId, int date, Location location, Location location1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 416);
			base.AppendLocation(location);
			base.AppendLocation(location1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x0029EE88 File Offset: 0x0029D088
		public void AddGetHusbandByJoustForSpouse(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 417);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x0029EEC0 File Offset: 0x0029D0C0
		public void AddGetWifeByJoustForSpouse(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 418);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x0029EEF8 File Offset: 0x0029D0F8
		public void AddNoHusbandByJoustForSpouse(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 419);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x0029EF24 File Offset: 0x0029D124
		public void AddSectCompetitionBeWinner(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 420);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x0029EF5C File Offset: 0x0029D15C
		public void AddSectCompetitionBeParticipant(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 421);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x0029EF94 File Offset: 0x0029D194
		public void AddSectCompetitionBeHost(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 422);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x0029EFCC File Offset: 0x0029D1CC
		public void AddWulinConferenceBeParticipant(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 423);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x0029F004 File Offset: 0x0029D204
		public void AddWulinConferenceBeWinner(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 424);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x0029F030 File Offset: 0x0029D230
		public void AddWulinConferenceBeWinnerButTaiwu(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 425);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x0029F05C File Offset: 0x0029D25C
		public void AddWulinConferenceBeHost(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 426);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x0029F094 File Offset: 0x0029D294
		public void AddWulinConferenceBeKilledByYufu(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 427);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x0029F0C0 File Offset: 0x0029D2C0
		public void AddWulinConferenceDonation(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 428);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x0029F0F8 File Offset: 0x0029D2F8
		public void AddBeAttackedAndDieByWuYingLing(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 429);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x0029F124 File Offset: 0x0029D324
		public void AddNaturalDisasterGiveDeath(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 430);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0029F150 File Offset: 0x0029D350
		public void AddNaturalDisasterHappen(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 431);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x0029F17C File Offset: 0x0029D37C
		public void AddNaturalDisasterButSurvive(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 432);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ABD RID: 19133 RVA: 0x0029F1A8 File Offset: 0x0029D3A8
		public void AddNormalInformationChangeLovingItemSubType(int selfCharId, int date, Location location, int charId, short itemSubType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 433);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendItemSubType(itemSubType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x0029F1E8 File Offset: 0x0029D3E8
		public void AddNormalInformationChangeHatingItemSubType(int selfCharId, int date, Location location, int charId, short itemSubType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 434);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendItemSubType(itemSubType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ABF RID: 19135 RVA: 0x0029F228 File Offset: 0x0029D428
		public void AddNormalInformationChangeIdealSect(int selfCharId, int date, Location location, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 435);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AC0 RID: 19136 RVA: 0x0029F268 File Offset: 0x0029D468
		public void AddNormalInformationChangeBaseMorality(int selfCharId, int date, Location location, int charId, sbyte behaviorType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 436);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendBehaviorType(behaviorType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AC1 RID: 19137 RVA: 0x0029F2A8 File Offset: 0x0029D4A8
		public void AddNormalInformationChangeLifeSkillTypeInterest(int selfCharId, int date, Location location, int charId, sbyte lifeSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 437);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendLifeSkillType(lifeSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AC2 RID: 19138 RVA: 0x0029F2E8 File Offset: 0x0029D4E8
		public void AddRobGraveEncounterSkeleton(int selfCharId, int date, Location location, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 438);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AC3 RID: 19139 RVA: 0x0029F320 File Offset: 0x0029D520
		public void AddRobGraveFailed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 439);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AC4 RID: 19140 RVA: 0x0029F358 File Offset: 0x0029D558
		public void AddSectPunishLevelLowest(int selfCharId, int date, short punishmentType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 440);
			base.AppendPunishmentType(punishmentType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x0029F384 File Offset: 0x0029D584
		public void AddPrincipalSectPunishLevelMiddle(int selfCharId, int date, short punishmentType, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 441);
			base.AppendPunishmentType(punishmentType);
			base.AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AC6 RID: 19142 RVA: 0x0029F3C0 File Offset: 0x0029D5C0
		public void AddPrincipalSectPunishLevelHighest(int selfCharId, int date, short punishmentType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 442);
			base.AppendPunishmentType(punishmentType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x0029F3EC File Offset: 0x0029D5EC
		public void AddNonPrincipalSectPunishLevelLowest(int selfCharId, int date, short punishmentType, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 443);
			base.AppendPunishmentType(punishmentType);
			base.AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AC8 RID: 19144 RVA: 0x0029F428 File Offset: 0x0029D628
		public void AddNonPrincipalSectPunishLevelHighest(int selfCharId, int date, short punishmentType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 444);
			base.AppendPunishmentType(punishmentType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AC9 RID: 19145 RVA: 0x0029F454 File Offset: 0x0029D654
		public void AddBecomeSwornSiblingByThreatened(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 445);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x0029F494 File Offset: 0x0029D694
		public void AddMarriedByThreatened(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 446);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ACB RID: 19147 RVA: 0x0029F4D4 File Offset: 0x0029D6D4
		public void AddGetAdoptedFatherByThreatened(int selfCharId, int date, int charId, Location location, int charId1, short relatedRecordId)
		{
			bool flag = relatedRecordId != 461 && relatedRecordId != 462;
			if (flag)
			{
				throw LifeRecordCollection.CreateRelatedRecordIdException(relatedRecordId);
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 447);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, relatedRecordId);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ACC RID: 19148 RVA: 0x0029F564 File Offset: 0x0029D764
		public void AddGetAdoptedMotherByThreatened(int selfCharId, int date, int charId, Location location, int charId1, short relatedRecordId)
		{
			bool flag = relatedRecordId != 461 && relatedRecordId != 462;
			if (flag)
			{
				throw LifeRecordCollection.CreateRelatedRecordIdException(relatedRecordId);
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 448);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, relatedRecordId);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ACD RID: 19149 RVA: 0x0029F5F4 File Offset: 0x0029D7F4
		public void AddGetAdoptedSonByThreatened(int selfCharId, int date, int charId, Location location, int charId1, short relatedRecordId)
		{
			bool flag = relatedRecordId != 459 && relatedRecordId != 460;
			if (flag)
			{
				throw LifeRecordCollection.CreateRelatedRecordIdException(relatedRecordId);
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 449);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, relatedRecordId);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x0029F684 File Offset: 0x0029D884
		public void AddGetAdoptedDaughterByThreatened(int selfCharId, int date, int charId, Location location, int charId1, short relatedRecordId)
		{
			bool flag = relatedRecordId != 459 && relatedRecordId != 460;
			if (flag)
			{
				throw LifeRecordCollection.CreateRelatedRecordIdException(relatedRecordId);
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 450);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, relatedRecordId);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x0029F714 File Offset: 0x0029D914
		public void AddAddMentorByThreatened(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 451);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x0029F754 File Offset: 0x0029D954
		public void AddSeverSwornSiblingByThreatened(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 452);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x0029F794 File Offset: 0x0029D994
		public void AddDivorceByThreatened(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 453);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AD2 RID: 19154 RVA: 0x0029F7D4 File Offset: 0x0029D9D4
		public void AddSeverMentorByThreatened(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 454);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x0029F814 File Offset: 0x0029DA14
		public void AddSeverAdoptiveFatherByThreatened(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 455);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x0029F854 File Offset: 0x0029DA54
		public void AddSeverAdoptiveMotherByThreatened(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 456);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x0029F894 File Offset: 0x0029DA94
		public void AddSeverAdoptiveSonByThreatened(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 457);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x0029F8D4 File Offset: 0x0029DAD4
		public void AddSeverAdoptiveDaughterByThreatened(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 458);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x0029F914 File Offset: 0x0029DB14
		public void AddApproveTaiwuByThreatened(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 463);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x0029F94C File Offset: 0x0029DB4C
		public void AddFourSeasonsAdventureBeParticipant(int selfCharId, int date, Location location, short adventureTemplateId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 464);
			base.AppendLocation(location);
			base.AppendAdventure(adventureTemplateId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x0029F98C File Offset: 0x0029DB8C
		public void AddFourSeasonsAdventureBeWinner(int selfCharId, int date, Location location, short adventureTemplateId, short titleTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 465);
			base.AppendLocation(location);
			base.AppendAdventure(adventureTemplateId);
			base.AppendCharacterTitle(titleTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x0029F9CC File Offset: 0x0029DBCC
		public void AddEndAdored(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 466);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x0029FA04 File Offset: 0x0029DC04
		public void AddGetMentor(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 467);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 468);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x0029FA60 File Offset: 0x0029DC60
		public void AddGetMentee(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 468);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 467);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x0029FABC File Offset: 0x0029DCBC
		public void AddSeverAdoptiveParent(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 469);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x0029FAF4 File Offset: 0x0029DCF4
		public void AddSeverAdoptiveChild(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 470);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 469);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x0029FB50 File Offset: 0x0029DD50
		public void AddSeverMentor(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 471);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 472);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x0029FBAC File Offset: 0x0029DDAC
		public void AddSeverMentee(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 472);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x0029FBE4 File Offset: 0x0029DDE4
		public void AddDivorce(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 473);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 473);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x0029FC40 File Offset: 0x0029DE40
		public void AddThreatenSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 474);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x0029FC78 File Offset: 0x0029DE78
		public void AddAdmonishSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 475);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x0029FCB0 File Offset: 0x0029DEB0
		public void AddChangeBehaviorTypeByAdmonishedGood(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 476);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x0029FCE8 File Offset: 0x0029DEE8
		public void AddReduceDebtByAdmonished(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 477);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x0029FD20 File Offset: 0x0029DF20
		public void AddReduceDebtByThreatened(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 478);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x0029FD58 File Offset: 0x0029DF58
		public void AddChangeBehaviorTypeByAdmonishedBad(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 479);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x0029FD90 File Offset: 0x0029DF90
		public void AddGainLegendaryBook(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 480);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x0029FDC8 File Offset: 0x0029DFC8
		public void AddBoostedByLegendaryBooks(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 481);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AEA RID: 19178 RVA: 0x0029FE00 File Offset: 0x0029E000
		public void AddActCrazy(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 482);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x0029FE2C File Offset: 0x0029E02C
		public void AddLegendaryBookShocked(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 483);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x0029FE64 File Offset: 0x0029E064
		public void AddLegendaryBookInsane(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 484);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x0029FE9C File Offset: 0x0029E09C
		public void AddLegendaryBookConsumed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 485);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x0029FEC8 File Offset: 0x0029E0C8
		public void AddDecideToContestForLegendaryBook(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 486);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x0029FF08 File Offset: 0x0029E108
		public void AddFinishContestForLegendaryBook(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 487);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x0029FF34 File Offset: 0x0029E134
		public void AddLegendaryBookChallengeWin(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 488);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 491);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x0029FFA8 File Offset: 0x0029E1A8
		public void AddLegendaryBookChallengeLose(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 489);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 490);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x002A001C File Offset: 0x0029E21C
		public void AddAcceptLegendaryBookChallengeEscape(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 492);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 493);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x002A0090 File Offset: 0x0029E290
		public void AddLegendaryBookChallengeSelfEscaped(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 524);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 525);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x002A0104 File Offset: 0x0029E304
		public void AddRefuseRequestLegendaryBookChallenge(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 494);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 495);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x002A0178 File Offset: 0x0029E378
		public void AddAcceptRequestLegendaryBook(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 496);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 497);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x002A01EC File Offset: 0x0029E3EC
		public void AddRequestLegendaryBookFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 498);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 499);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x002A0260 File Offset: 0x0029E460
		public void AddAcceptRequestExchangeLegendaryBook(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 500);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 501);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x002A02F8 File Offset: 0x0029E4F8
		public void AddRefuseRequestExchangeLegendaryBook(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 502);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 503);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x002A036C File Offset: 0x0029E56C
		public void AddGiveLegendaryBookFail(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 504);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 505);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x002A03E0 File Offset: 0x0029E5E0
		public void AddDefeatLegendaryBookInsaneJust(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 506);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 511);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x002A0454 File Offset: 0x0029E654
		public void AddDefeatLegendaryBookInsaneKind(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 507);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 512);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x002A04B0 File Offset: 0x0029E6B0
		public void AddDefeatLegendaryBookInsaneEven(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 508);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 513);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x002A050C File Offset: 0x0029E70C
		public void AddDefeatLegendaryBookInsaneRebel(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 509);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 514);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x002A0568 File Offset: 0x0029E768
		public void AddDefeatLegendaryBookInsaneEgoistic(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 510);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 515);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x002A05DC File Offset: 0x0029E7DC
		public void AddShockedInsaneEscaped(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 516);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 517);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x002A0638 File Offset: 0x0029E838
		public void AddUnderAttackEscaped(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 518);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 519);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x002A0694 File Offset: 0x0029E894
		public void AddDefeatConsumed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 520);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 521);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B02 RID: 19202 RVA: 0x002A06F0 File Offset: 0x0029E8F0
		public void AddAcceptRequestExchangeLegendaryBookByExp(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 522);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 523);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x002A0774 File Offset: 0x0029E974
		public void AddResignPositionToStudyLegendaryBook(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, short settlementId, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 526);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendSettlement(settlementId);
			base.AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x002A07C4 File Offset: 0x0029E9C4
		public void AddSoundOutLoverMind(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 527);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 528);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x002A0820 File Offset: 0x0029EA20
		public void AddRedeemMindSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 529);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 531);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x002A087C File Offset: 0x0029EA7C
		public void AddRedeemMindFail(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 530);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 532);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x002A08D8 File Offset: 0x0029EAD8
		public void AddFirstDateWithLover(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 533);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 534);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B08 RID: 19208 RVA: 0x002A0934 File Offset: 0x0029EB34
		public void AddSelectLoverToken(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 535);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 536);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x002A09A8 File Offset: 0x0029EBA8
		public void AddDateWithLover(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 537);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 538);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x002A0A04 File Offset: 0x0029EC04
		public void AddTillDeathDoUsPart(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 539);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x002A0A3C File Offset: 0x0029EC3C
		public void AddCelebrateBirthday(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 540);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 541);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x002A0A98 File Offset: 0x0029EC98
		public void AddCelebrateAnniversary(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 542);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x002A0AD0 File Offset: 0x0029ECD0
		public void AddBeCaughtCheating(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 543);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 544);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x002A0B2C File Offset: 0x0029ED2C
		public void AddPregnancyWithWife(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 545);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 546);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x002A0B88 File Offset: 0x0029ED88
		public void AddTeaTasting(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 547);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x002A0BC0 File Offset: 0x0029EDC0
		public void AddTeaTastingLifeSkillBattleWin(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 548);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 549);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x002A0C1C File Offset: 0x0029EE1C
		public void AddTeaTastingDisorderOfQi(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 550);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x002A0C54 File Offset: 0x0029EE54
		public void AddWineTasting(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 551);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x002A0C8C File Offset: 0x0029EE8C
		public void AddWineTastingLifeSkillBattleWin(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 552);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 553);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x002A0CE8 File Offset: 0x0029EEE8
		public void AddWineTastingDisorderOfQi(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 554);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x002A0D20 File Offset: 0x0029EF20
		public void AddFirstNameChanged(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 555);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B16 RID: 19222 RVA: 0x002A0D60 File Offset: 0x0029EF60
		public void AddLifeSkillModel(int selfCharId, int date, Location location, sbyte lifeSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 556);
			base.AppendLocation(location);
			base.AppendLifeSkillType(lifeSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x002A0D98 File Offset: 0x0029EF98
		public void AddCombatSkillModel(int selfCharId, int date, Location location, sbyte combatSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 557);
			base.AppendLocation(location);
			base.AppendCombatSkillType(combatSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x002A0DD0 File Offset: 0x0029EFD0
		public void AddPromoteReputation(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 558);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 559);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x002A0E2C File Offset: 0x0029F02C
		public void AddCapabilityCultivated(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 560);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x002A0E64 File Offset: 0x0029F064
		public void AddBroughtToTaiwuByBeggars(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 561);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x002A0E9C File Offset: 0x0029F09C
		public void AddDiscardRevengeForCivilianSkill(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 562);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B1C RID: 19228 RVA: 0x002A0ED4 File Offset: 0x0029F0D4
		public void AddCivilianSkillDissolveResentment(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 563);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x002A0F00 File Offset: 0x0029F100
		public void AddPersuadeWithdrawlFromOrganization(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 564);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 565);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x002A0F5C File Offset: 0x0029F15C
		public void AddFreeMedicalConsultation(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 566);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x002A0F88 File Offset: 0x0029F188
		public void AddOfferTreasures(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 567);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 568);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x002A0FE4 File Offset: 0x0029F1E4
		public void AddForcefulPurchase(int selfCharId, int date, int charId, Location location, int value, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 569);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 570);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x002A1068 File Offset: 0x0029F268
		public void AddBegForMoney(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 571);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x002A10A0 File Offset: 0x0029F2A0
		public void AddAbsurdlyForceToLeave(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 572);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 573);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x002A10FC File Offset: 0x0029F2FC
		public void AddDiagnoseWithMedicine(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 574);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 575);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B24 RID: 19236 RVA: 0x002A1158 File Offset: 0x0029F358
		public void AddDiagnoseWithNonMedicine(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 576);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 577);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B25 RID: 19237 RVA: 0x002A11B4 File Offset: 0x0029F3B4
		public void AddExtendLifeSpan(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 578);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 579);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B26 RID: 19238 RVA: 0x002A1210 File Offset: 0x0029F410
		public void AddPersuadeToBecomeMonk(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 580);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 581);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B27 RID: 19239 RVA: 0x002A126C File Offset: 0x0029F46C
		public void AddFailToPersuadeToBecomeMonk(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 582);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x002A12A4 File Offset: 0x0029F4A4
		public void AddExpiateDeadSouls(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 583);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x002A12D0 File Offset: 0x0029F4D0
		public void AddExociseXiangshuInfectionVictoryInCombat(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 584);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 585);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x002A132C File Offset: 0x0029F52C
		public void AddExociseXiangshuInfectionVictoryInCombatDefeated(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 604);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x002A1364 File Offset: 0x0029F564
		public void AddTribulationSucceeded(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 586);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x002A1390 File Offset: 0x0029F590
		public void AddTribulationFailed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 587);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x002A13BC File Offset: 0x0029F5BC
		public void AddTribulationCanceled(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 588);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x002A13E8 File Offset: 0x0029F5E8
		public void AddTribulationContinued(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 589);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x002A1414 File Offset: 0x0029F614
		public void AddGuidingEvilToGoodSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 590);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 591);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x002A1470 File Offset: 0x0029F670
		public void AddGuidingEvilToGoodFail(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 592);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x002A14A8 File Offset: 0x0029F6A8
		public void AddVisitBuddhismTemples(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 593);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x002A14D4 File Offset: 0x0029F6D4
		public void AddEpiphanyThruVisitTemples(int selfCharId, int date, Location location, short combatSkillTemplateId, short lifeSkillTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 594);
			base.AppendLocation(location);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.AppendLifeSkill(lifeSkillTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x002A1514 File Offset: 0x0029F714
		public void AddEpiphanyThruVisitTemplesCombatSkill(int selfCharId, int date, Location location, short combatSkillTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 595);
			base.AppendLocation(location);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x002A154C File Offset: 0x0029F74C
		public void AddEpiphanyThruVisitTemplesLifeSkill(int selfCharId, int date, Location location, short lifeSkillTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 596);
			base.AppendLocation(location);
			base.AppendLifeSkill(lifeSkillTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x002A1584 File Offset: 0x0029F784
		public void AddEpiphanyThruVisitTemplesExperience(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 605);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x002A15BC File Offset: 0x0029F7BC
		public void AddDivineUnexpectedGain(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 597);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x002A15F4 File Offset: 0x0029F7F4
		public void AddDivineUnexpectedHarm(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 598);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x002A162C File Offset: 0x0029F82C
		public void AddExchangeFates(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 599);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 600);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B39 RID: 19257 RVA: 0x002A1688 File Offset: 0x0029F888
		public void AddImmortalityGained(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 601);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x002A16C0 File Offset: 0x0029F8C0
		public void AddImmortalityLost(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 602);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x002A16EC File Offset: 0x0029F8EC
		public void AddImmortalityRegained(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 603);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x002A1718 File Offset: 0x0029F918
		public void AddTaiwuReincarnation(int selfCharId, int date, int charId, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 606);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x002A1758 File Offset: 0x0029F958
		public void AddTaiwuReincarnationPregnancy(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 607);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B3E RID: 19262 RVA: 0x002A1784 File Offset: 0x0029F984
		public void AddMixPoisonHotRedRotten(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 608);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B3F RID: 19263 RVA: 0x002A17B0 File Offset: 0x0029F9B0
		public void AddMixPoisonHotRottenIllusory(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 609);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x002A17DC File Offset: 0x0029F9DC
		public void AddMixPoisonHotRottenGloomy(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 610);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B41 RID: 19265 RVA: 0x002A1814 File Offset: 0x0029FA14
		public void AddMixPoisonHotRottenCold(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 611);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x002A1840 File Offset: 0x0029FA40
		public void AddMixPoisonRedRottenIllusory(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 612);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x002A186C File Offset: 0x0029FA6C
		public void AddMixPoisonRedRottenGloomy(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 613);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x002A1898 File Offset: 0x0029FA98
		public void AddMixPoisonRedRottenCold(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 614);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x002A18C4 File Offset: 0x0029FAC4
		public void AddMixPoisonHotRedIllusory(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 615);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x002A18F0 File Offset: 0x0029FAF0
		public void AddMixPoisonHotRedGloomy(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 616);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x002A191C File Offset: 0x0029FB1C
		public void AddMixPoisonHotRedCold(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 617);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x002A1948 File Offset: 0x0029FB48
		public void AddMixPoisonGloomyColdIllusory(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 618);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x002A1974 File Offset: 0x0029FB74
		public void AddMixPoisonRottenGloomyCold(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 619);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x002A19A0 File Offset: 0x0029FBA0
		public void AddMixPoisonHotGloomyCold(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 620);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B4B RID: 19275 RVA: 0x002A19CC File Offset: 0x0029FBCC
		public void AddMixPoisonRedGloomyCold(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 621);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x002A19F8 File Offset: 0x0029FBF8
		public void AddMixPoisonRottenColdIllusory(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 622);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x002A1A24 File Offset: 0x0029FC24
		public void AddMixPoisonHotColdIllusory(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 623);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x002A1A50 File Offset: 0x0029FC50
		public void AddMixPoisonRedColdIllusory(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 624);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x002A1A7C File Offset: 0x0029FC7C
		public void AddMixPoisonRottenGloomyIllusory(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 625);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x002A1AA8 File Offset: 0x0029FCA8
		public void AddMixPoisonHotGloomyIllusory(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 626);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x002A1AD4 File Offset: 0x0029FCD4
		public void AddMixPoisonRedGloomyIllusory(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 627);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x002A1B00 File Offset: 0x0029FD00
		public void AddDiggingXiangshuMinionCombatLost(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 628);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x002A1B2C File Offset: 0x0029FD2C
		public void AddDiggingXiangshuMinionCombatWon(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 629);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x002A1B64 File Offset: 0x0029FD64
		public void AddSectMainStoryXuehouJixiKills(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 630);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x002A1B90 File Offset: 0x0029FD90
		public void AddSectMainStoryWudangTreasure(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 631);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x002A1BC8 File Offset: 0x0029FDC8
		public void AddSectMainStoryXuannvJoinOrg(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 632);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x002A1BF4 File Offset: 0x0029FDF4
		public void AddSectMainStoryYuanshanGetAbsorbed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 633);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x002A1C20 File Offset: 0x0029FE20
		public void AddSectMainStoryYuanshanResistSucceed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 634);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x002A1C4C File Offset: 0x0029FE4C
		public void AddSectMainStoryYuanshanResistOrdinary(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 635);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x002A1C78 File Offset: 0x0029FE78
		public void AddSectMainStoryYuanshanResistFailed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 636);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x002A1CA4 File Offset: 0x0029FEA4
		public void AddSectMainStoryXuehouZombieKills(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 637);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x002A1CC8 File Offset: 0x0029FEC8
		public void AddSectMainStoryShixiangSkillEnemy(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 638);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x002A1CEC File Offset: 0x0029FEEC
		public void AddSectMainStoryWuxianMethysis0(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 639);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B5E RID: 19294 RVA: 0x002A1D18 File Offset: 0x0029FF18
		public void AddSectMainStoryWuxianPoison(int selfCharId, int date, Location location, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 640);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B5F RID: 19295 RVA: 0x002A1D50 File Offset: 0x0029FF50
		public void AddSectMainStoryWuxianAssault(int selfCharId, int date, Location location, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 641);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x002A1D88 File Offset: 0x0029FF88
		public void AddSectMainStoryWuxianMethysis1(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 642);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x002A1DB4 File Offset: 0x0029FFB4
		public void AddSectMainStoryEmeiInfighting(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 643);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x002A1DE0 File Offset: 0x0029FFE0
		public void AddSectMainStoryJieqingAssassin(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 644);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x002A1E0C File Offset: 0x002A000C
		public void AddWulinConferencePraiseAndGifts(int selfCharId, int date, short settlementId, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 645);
			base.AppendSettlement(settlementId);
			base.AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x002A1E48 File Offset: 0x002A0048
		public void AddNormalInformationChangeIdealSectNegative(int selfCharId, int date, Location location, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 646);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x002A1E88 File Offset: 0x002A0088
		public void AddSectMainStoryXuehouJixiRescueTaiwu(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 647);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x002A1EB4 File Offset: 0x002A00B4
		public void AddSectMainStoryRanshanJoinThreeFactionCompetetion(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 648);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x002A1EEC File Offset: 0x002A00EC
		public void AddSectMainStoryRanshanThreeFactionCompetetionWin(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 649);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x002A1F24 File Offset: 0x002A0124
		public void AddSectMainStoryRanshanThreeFactionCompetetionLose(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 650);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x002A1F5C File Offset: 0x002A015C
		public void AddGainExpByStroll(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 651);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x002A1F88 File Offset: 0x002A0188
		public void AddGainExpByReadingOldBook(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 652);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x002A1FC0 File Offset: 0x002A01C0
		public void AddPunishedAlongsideSpouse(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 653);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x002A1FEC File Offset: 0x002A01EC
		public void AddDecideToAdoptFoundling(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 654);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x002A2024 File Offset: 0x002A0224
		public void AddAdoptFoundlingFail(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 655);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x002A205C File Offset: 0x002A025C
		public void AddAdoptFoundlingSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 656);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 657);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x002A20B8 File Offset: 0x002A02B8
		public void AddClaimFoundlingSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 658);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 659);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x002A2114 File Offset: 0x002A0314
		public void AddSectMainStoryWudangVillagerKilled(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 660);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x002A2140 File Offset: 0x002A0340
		public void AddSectMainStoryShixiangFallIll(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 661);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x002A2164 File Offset: 0x002A0364
		public void AddKillAnimal(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 662);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x002A219C File Offset: 0x002A039C
		public void AddDefeatedByAnimal(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 663);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x002A21D4 File Offset: 0x002A03D4
		public void AddEnterEnemyNest(int selfCharId, int date, Location location, short adventureTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 664);
			base.AppendLocation(location);
			base.AppendAdventure(adventureTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x002A220C File Offset: 0x002A040C
		public void AddDieFromEnemyNest(int selfCharId, int date, Location location, short adventureTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 665);
			base.AppendLocation(location);
			base.AppendAdventure(adventureTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x002A2244 File Offset: 0x002A0444
		public void AddEscapeFromEnemyNest(int selfCharId, int date, int charId, Location location, short adventureTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 666);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendAdventure(adventureTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 692);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendAdventure(adventureTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x002A22B4 File Offset: 0x002A04B4
		public void AddGetSecretSpreadInVeryHighProbability(int selfCharId, int date, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 667);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x002A22EC File Offset: 0x002A04EC
		public void AddGetSecretSpreadInHighProbability(int selfCharId, int date, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 668);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x002A2324 File Offset: 0x002A0524
		public void AddGetSecretSpreadInLowProbability(int selfCharId, int date, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 669);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x002A235C File Offset: 0x002A055C
		public void AddGetSecretSpreadInVeryLowProbability(int selfCharId, int date, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 670);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B7B RID: 19323 RVA: 0x002A2394 File Offset: 0x002A0594
		public void AddSpreadSecretFail(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 671);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B7C RID: 19324 RVA: 0x002A23D4 File Offset: 0x002A05D4
		public void AddSpreadSecretSuccess(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 672);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x002A2414 File Offset: 0x002A0614
		public void AddHeardSecretSpreadInVeryHighProbability(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 673);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x002A2454 File Offset: 0x002A0654
		public void AddHeardSecretSpreadInHighProbability(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 674);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x002A2494 File Offset: 0x002A0694
		public void AddHeardSecretSpreadInLowProbability(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 675);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x002A24D4 File Offset: 0x002A06D4
		public void AddHeardSecretSpreadInVeryLowProbability(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 676);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x002A2514 File Offset: 0x002A0714
		public void AddRequestKeepSecretFail(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 677);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x002A2554 File Offset: 0x002A0754
		public void AddRequestKeepSecretSuccess(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 678);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x002A2594 File Offset: 0x002A0794
		public void AddBeRequestedToKeepSecret(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 679);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x002A25D4 File Offset: 0x002A07D4
		public void AddThreadNeedleMatchFail(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 680);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x002A2614 File Offset: 0x002A0814
		public void AddThreadNeedleSeparateFail(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 681);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x002A2654 File Offset: 0x002A0854
		public void AddThreadNeedleMatchSuccess(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 682);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x002A2694 File Offset: 0x002A0894
		public void AddThreadNeedleSeparateSuccess(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 683);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x002A26D4 File Offset: 0x002A08D4
		public void AddThreadNeedleBeMatched1(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 684);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x002A2714 File Offset: 0x002A0914
		public void AddThreadNeedleBeSeparated1(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 685);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x002A2754 File Offset: 0x002A0954
		public void AddThreadNeedleBeMatched2(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 686);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x002A2794 File Offset: 0x002A0994
		public void AddThreadNeedleBeSeparated2(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 687);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x002A27D4 File Offset: 0x002A09D4
		public void AddSpreadSecretKnown(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 688);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x002A2814 File Offset: 0x002A0A14
		public void AddSectMainStoryXuannvBirthOfMirrorCreatedImposture(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 689);
			base.AppendCharacterRealName(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x002A2840 File Offset: 0x002A0A40
		public void AddEscapeFromEnemyNestBySelf(int selfCharId, int date, Location location, short adventureTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 690);
			base.AppendLocation(location);
			base.AppendAdventure(adventureTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x002A2878 File Offset: 0x002A0A78
		public void AddSaveFromEnemyNestFailed(int selfCharId, int date, int charId, Location location, short adventureTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 695);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendAdventure(adventureTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x002A28B8 File Offset: 0x002A0AB8
		public void AddTameCarrierSucceed(int selfCharId, int date, sbyte itemType, short itemTemplateId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 693);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x002A28F0 File Offset: 0x002A0AF0
		public void AddTameCarrierFail(int selfCharId, int date, sbyte itemType, short itemTemplateId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 694);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x002A2928 File Offset: 0x002A0B28
		public void AddReleaseCarrier(int selfCharId, int date, sbyte itemType, short itemTemplateId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 696);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x002A2960 File Offset: 0x002A0B60
		public void AddDLCLoongRidingEffectQiuniuAudience(int selfCharId, int date, int charId, Location location, int jiaoLoongId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 697);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendJiaoLoong(jiaoLoongId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x002A29A0 File Offset: 0x002A0BA0
		public void AddDLCLoongRidingEffectQiuniu(int selfCharId, int date, Location location, int jiaoLoongId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 698);
			base.AppendLocation(location);
			base.AppendJiaoLoong(jiaoLoongId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x002A29D8 File Offset: 0x002A0BD8
		public void AddDLCLoongRidingEffectYazi(int selfCharId, int date, int charId, Location location, int jiaoLoongId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 699);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendJiaoLoong(jiaoLoongId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x002A2A18 File Offset: 0x002A0C18
		public void AddDLCLoongRidingEffectChaofeng(int selfCharId, int date, Location location, int jiaoLoongId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 700);
			base.AppendLocation(location);
			base.AppendJiaoLoong(jiaoLoongId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x002A2A50 File Offset: 0x002A0C50
		public void AddDLCLoongRidingEffectPulao(int selfCharId, int date, int jiaoLoongId, short colorId, short partId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 701);
			base.AppendJiaoLoong(jiaoLoongId);
			base.AppendCricket(colorId, partId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x002A2A88 File Offset: 0x002A0C88
		public void AddDLCLoongRidingEffectSuanni(int selfCharId, int date, Location location, int jiaoLoongId, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 702);
			base.AppendLocation(location);
			base.AppendJiaoLoong(jiaoLoongId);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x002A2AD4 File Offset: 0x002A0CD4
		public void AddDLCLoongRidingEffectBaxia(int selfCharId, int date, Location location, int jiaoLoongId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 703);
			base.AppendLocation(location);
			base.AppendJiaoLoong(jiaoLoongId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x002A2B0C File Offset: 0x002A0D0C
		public void AddDLCLoongRidingEffectBian(int selfCharId, int date, Location location, int jiaoLoongId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 704);
			base.AppendLocation(location);
			base.AppendJiaoLoong(jiaoLoongId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x002A2B44 File Offset: 0x002A0D44
		public void AddDLCLoongRidingEffectFuxi(int selfCharId, int date, Location location, int jiaoLoongId, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 705);
			base.AppendLocation(location);
			base.AppendJiaoLoong(jiaoLoongId);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x002A2B90 File Offset: 0x002A0D90
		public void AddDLCLoongRidingEffectChiwen(int selfCharId, int date, Location location, int jiaoLoongId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 706);
			base.AppendLocation(location);
			base.AppendJiaoLoong(jiaoLoongId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x002A2BC8 File Offset: 0x002A0DC8
		public void AddDefeatLoong(int selfCharId, int date, short charTemplateId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 707);
			base.AppendCharacterTemplate(charTemplateId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x002A2C00 File Offset: 0x002A0E00
		public void AddDefeatedByLoong(int selfCharId, int date, short charTemplateId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 708);
			base.AppendCharacterTemplate(charTemplateId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x002A2C38 File Offset: 0x002A0E38
		public void AddDLCLoongRidingEffectYazi2(int selfCharId, int date, Location location, int jiaoLoongId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 709);
			base.AppendLocation(location);
			base.AppendJiaoLoong(jiaoLoongId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x002A2C70 File Offset: 0x002A0E70
		public void AddDieFromAge(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 710);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x002A2C9C File Offset: 0x002A0E9C
		public void AddDieFromPoorHealth(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 711);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BA2 RID: 19362 RVA: 0x002A2CC8 File Offset: 0x002A0EC8
		public void AddKilledAfterXiangshuInfected(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 714);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BA3 RID: 19363 RVA: 0x002A2CF4 File Offset: 0x002A0EF4
		public void AddAssassinated(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 715);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x002A2D20 File Offset: 0x002A0F20
		public void AddKilledByXiangshu(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 716);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x002A2D4C File Offset: 0x002A0F4C
		public void AddPurchaseItem1(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 717);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x002A2D84 File Offset: 0x002A0F84
		public void AddSellItem1(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 718);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x002A2DBC File Offset: 0x002A0FBC
		public void AddCleanBodyReincarnationSuccess(int selfCharId, int date, int charId, Location location, sbyte combatType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 719);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 720);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x002A2E2C File Offset: 0x002A102C
		public void AddEvilBodyReincarnationSuccess(int selfCharId, int date, int charId, Location location, sbyte combatType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 721);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 722);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendCombatType(combatType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x002A2E9C File Offset: 0x002A109C
		public void AddWugKingForestSpiritBecomeEnemy(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 723);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 737);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x002A2F10 File Offset: 0x002A1110
		public void AddSecretMakeEnemy(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 724);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformationTemplate(secretInfoTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 725);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendSecretInformationTemplate(secretInfoTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x002A2F80 File Offset: 0x002A1180
		public void AddCleanBodyDefeatAnimal(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 726);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x002A2FB8 File Offset: 0x002A11B8
		public void AddEvilBodyDefeatAnimal(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 727);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x002A2FF0 File Offset: 0x002A11F0
		public void AddCleanBodyDefeatHereticRandomEnemy(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 728);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x002A3028 File Offset: 0x002A1228
		public void AddEvilBodyDefeatHereticRandomEnemy(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 729);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x002A3060 File Offset: 0x002A1260
		public void AddCleanBodyDefeatRighteousRandomEnemy(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 730);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x002A3098 File Offset: 0x002A1298
		public void AddEvilBodyDefeatRighteousRandomEnemy(int selfCharId, int date, Location location, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 731);
			base.AppendLocation(location);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x002A30D0 File Offset: 0x002A12D0
		public void AddWuxianParanoiaAdded(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 732);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BB2 RID: 19378 RVA: 0x002A30FC File Offset: 0x002A12FC
		public void AddWuxianParanoiaAttack(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 733);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x002A3134 File Offset: 0x002A1334
		public void AddWuxianParanoiaErased(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 734);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x002A3160 File Offset: 0x002A1360
		public void AddWugKingRedEyeLoseItem(int selfCharId, int date, sbyte itemType, short itemTemplateId, Location location, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 735);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendLocation(location);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x002A31A4 File Offset: 0x002A13A4
		public void AddWugForestSpiritReduceFavorability(int selfCharId, int date, sbyte itemType, short itemTemplateId, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 736);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x002A31DC File Offset: 0x002A13DC
		public void AddWugKingBlackBloodChangeDisorderOfQi(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 738);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x002A320C File Offset: 0x002A140C
		public void AddWugDevilInsideXiangshuInfection(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 739);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x002A323C File Offset: 0x002A143C
		public void AddWugCorpseWormChangeHealth(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 740);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x002A326C File Offset: 0x002A146C
		public void AddWugKingIceSilkwormLoseNeili(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 741);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x002A329C File Offset: 0x002A149C
		public void AddWugKingGoldenSilkwormEatGrownWug(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 742);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x002A32D8 File Offset: 0x002A14D8
		public void AddWugAzureMarrowAddPoison(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 743);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x002A3308 File Offset: 0x002A1508
		public void AddWugAzureMarrowAddWug(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 744);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 745);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x002A3390 File Offset: 0x002A1590
		public void AddWuxianParanoiaErased2(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 746);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x002A33B4 File Offset: 0x002A15B4
		public void AddWuxianDecreasedMood(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 747);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x002A33D8 File Offset: 0x002A15D8
		public void AddWuxianDecreasedFavorability(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 748);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x002A33FC File Offset: 0x002A15FC
		public void AddWuxianQiDecline(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 749);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x002A3420 File Offset: 0x002A1620
		public void AddWuxianPoisoning(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 750);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x002A344C File Offset: 0x002A164C
		public void AddWuxianLoseItem(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 751);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x002A3478 File Offset: 0x002A1678
		public void AddWugDevilInsideChangeHappiness(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 752);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x002A34A8 File Offset: 0x002A16A8
		public void AddWugRedEyeChangeToGrown(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 753);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x002A34E4 File Offset: 0x002A16E4
		public void AddWugForestSpiritChangeToGrown(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 754);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x002A3520 File Offset: 0x002A1720
		public void AddWugBlackBloodChangeToGrown(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 755);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x002A355C File Offset: 0x002A175C
		public void AddWugDevilInsideChangeToGrown(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 756);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x002A3598 File Offset: 0x002A1798
		public void AddWugCorpseWormChangeToGrown(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 757);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 758);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x002A3620 File Offset: 0x002A1820
		public void AddWugIceSilkwormChangeToGrown(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 759);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x002A365C File Offset: 0x002A185C
		public void AddWugGoldenSilkwormChangeToGrown(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 760);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x002A3698 File Offset: 0x002A1898
		public void AddWugAzureMarrowChangeToGrown(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 761);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 762);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x002A3720 File Offset: 0x002A1920
		public void AddManageLearnLifeSkillSuccess(int selfCharId, int date, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 763);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x002A3758 File Offset: 0x002A1958
		public void AddManageLearnCombatSkillSuccess(int selfCharId, int date, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 764);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x002A3790 File Offset: 0x002A1990
		public void AddManageLearnLifeSkillFail(int selfCharId, int date, sbyte lifeSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 765);
			base.AppendLifeSkillType(lifeSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x002A37BC File Offset: 0x002A19BC
		public void AddManageLearnCombatSkillFail(int selfCharId, int date, sbyte combatSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 766);
			base.AppendCombatSkillType(combatSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x002A37E8 File Offset: 0x002A19E8
		public void AddManageLifeSkillAbilityUp(int selfCharId, int date, sbyte lifeSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 767);
			base.AppendLifeSkillType(lifeSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x002A3814 File Offset: 0x002A1A14
		public void AddManageCombatSkillAbilityUp(int selfCharId, int date, sbyte combatSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 768);
			base.AppendCombatSkillType(combatSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x002A3840 File Offset: 0x002A1A40
		public void AddSmallVillagerXiangshuCompletelyInfected(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 769);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x002A386C File Offset: 0x002A1A6C
		public void AddSmallVillagerSavedFromInfection(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 770);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 771);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x002A38C8 File Offset: 0x002A1AC8
		public void AddStorageResourceToTreasury(int selfCharId, int date, short settlementId, sbyte resourceType, int value, int value1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 772);
			base.AppendSettlement(settlementId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.AppendInteger(value1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x002A3910 File Offset: 0x002A1B10
		public void AddStorageItemToTreasury(int selfCharId, int date, short settlementId, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 773);
			base.AppendSettlement(settlementId);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x002A3950 File Offset: 0x002A1B50
		public void AddTakeResourceFromTreasury(int selfCharId, int date, short settlementId, sbyte resourceType, int value, int value1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 774);
			base.AppendSettlement(settlementId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.AppendInteger(value1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x002A3998 File Offset: 0x002A1B98
		public void AddTakeItemFromTreasury(int selfCharId, int date, short settlementId, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 775);
			base.AppendSettlement(settlementId);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x002A39D8 File Offset: 0x002A1BD8
		public void AddTaiwuStorageResourceToTreasury(int selfCharId, int date, short settlementId, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 822);
			base.AppendSettlement(settlementId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x002A3A18 File Offset: 0x002A1C18
		public void AddTaiwuStorageItemToTreasury(int selfCharId, int date, short settlementId, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 823);
			base.AppendSettlement(settlementId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x002A3A50 File Offset: 0x002A1C50
		public void AddTaiwuTakeResourceFromTreasury(int selfCharId, int date, short settlementId, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 824);
			base.AppendSettlement(settlementId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x002A3A90 File Offset: 0x002A1C90
		public void AddTaiwuTakeItemFromTreasury(int selfCharId, int date, short settlementId, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 825);
			base.AppendSettlement(settlementId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x002A3AC8 File Offset: 0x002A1CC8
		public void AddDecideToGuardTreasury(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 776);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x002A3B00 File Offset: 0x002A1D00
		public void AddFinishGuardingTreasury(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 777);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x002A3B38 File Offset: 0x002A1D38
		public void AddIntrudeTreasuryCancelSupportMakeEnemy(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 778);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x002A3B70 File Offset: 0x002A1D70
		public void AddIntrudeTreasuryBeCancelSupportMakeEnemy(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 779);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x002A3B9C File Offset: 0x002A1D9C
		public void AddIntrudeTreasuryCancelSupport(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 780);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x002A3BD4 File Offset: 0x002A1DD4
		public void AddIntrudeTreasuryBeCancelSupport(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 781);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x002A3C00 File Offset: 0x002A1E00
		public void AddIntrudeTreasuryMakeEnemyOthers(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 782);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x002A3C38 File Offset: 0x002A1E38
		public void AddIntrudeTreasuryBeMakeEnemyOthers(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 783);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x002A3C64 File Offset: 0x002A1E64
		public void AddIntrudeTreasuryLostMorale(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 784);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x002A3C9C File Offset: 0x002A1E9C
		public void AddIntrudeTreasuryBeLostMorale(int selfCharId, int date, short settlementId, float floatValue)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 785);
			base.AppendSettlement(settlementId);
			base.AppendFloat(floatValue);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x002A3CD4 File Offset: 0x002A1ED4
		public void AddIntrudeTreasuryBeLostMorale2(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 786);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x002A3D00 File Offset: 0x002A1F00
		public void AddPlunderTreasuryCancelSupportMakeEnemy(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 787);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x002A3D38 File Offset: 0x002A1F38
		public void AddPlunderTreasuryBeCancelSupportMakeEnemy(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 788);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x002A3D64 File Offset: 0x002A1F64
		public void AddPlunderTreasuryCancelSupport(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 789);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x002A3D9C File Offset: 0x002A1F9C
		public void AddPlunderTreasuryBeCancelSupport(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 790);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x002A3DC8 File Offset: 0x002A1FC8
		public void AddPlunderTreasuryMakeEnemyOthers(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 791);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x002A3E00 File Offset: 0x002A2000
		public void AddPlunderTreasuryBeMakeEnemyOthers(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 792);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x002A3E2C File Offset: 0x002A202C
		public void AddPlunderTreasuryLostMorale(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 793);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x002A3E64 File Offset: 0x002A2064
		public void AddPlunderTreasuryBeLostMorale(int selfCharId, int date, short settlementId, float floatValue)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 794);
			base.AppendSettlement(settlementId);
			base.AppendFloat(floatValue);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x002A3E9C File Offset: 0x002A209C
		public void AddPlunderTreasuryBeLostMorale2(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 795);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x002A3EC8 File Offset: 0x002A20C8
		public void AddDonateTreasuryProvideSupport(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 796);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x002A3F00 File Offset: 0x002A2100
		public void AddDonateTreasuryBeProvideSupport(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 797);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x002A3F2C File Offset: 0x002A212C
		public void AddDonateTreasuryGetMorale(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 798);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x002A3F64 File Offset: 0x002A2164
		public void AddDonateTreasuryBeGetMorale(int selfCharId, int date, short settlementId, float floatValue)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 799);
			base.AppendSettlement(settlementId);
			base.AppendFloat(floatValue);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x002A3F9C File Offset: 0x002A219C
		public void AddDonateTreasuryGetMorale2(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 800);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x002A3FC8 File Offset: 0x002A21C8
		public void AddTreasuryDistributeResource(int selfCharId, int date, short settlementId, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 826);
			base.AppendSettlement(settlementId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x002A4008 File Offset: 0x002A2208
		public void AddTreasuryDistributeItem(int selfCharId, int date, short settlementId, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 801);
			base.AppendSettlement(settlementId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x002A4040 File Offset: 0x002A2240
		public void AddPoisonEnemyFail12(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 802);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x002A4078 File Offset: 0x002A2278
		public void AddPoisonEnemyFail22(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 803);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x002A40B0 File Offset: 0x002A22B0
		public void AddPoisonEnemyFail32(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 804);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x002A40E8 File Offset: 0x002A22E8
		public void AddPoisonEnemyFail42(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 805);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x002A4120 File Offset: 0x002A2320
		public void AddPoisonEnemySucceed2(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 806);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 808);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x002A417C File Offset: 0x002A237C
		public void AddPoisonEnemySucceedAndEscaped2(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 807);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 808);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x002A41D8 File Offset: 0x002A23D8
		public void AddPlotHarmEnemyFail12(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 809);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x002A4218 File Offset: 0x002A2418
		public void AddPlotHarmEnemyFail22(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 810);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x002A4258 File Offset: 0x002A2458
		public void AddPlotHarmEnemyFail32(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 811);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x002A4298 File Offset: 0x002A2498
		public void AddPlotHarmEnemyFail42(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 812);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x002A42D8 File Offset: 0x002A24D8
		public void AddPlotHarmEnemySucceed2(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 813);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 815);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x002A434C File Offset: 0x002A254C
		public void AddPlotHarmEnemySucceedAndEscaped2(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 814);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 815);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x002A43C0 File Offset: 0x002A25C0
		public void AddSectMainStoryBaihuaManiaLow(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 816);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x002A43EC File Offset: 0x002A25EC
		public void AddSectMainStoryBaihuaManiaHigh(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 817);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C05 RID: 19461 RVA: 0x002A4418 File Offset: 0x002A2618
		public void AddSectMainStoryBaihuaManiaAttack(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 818);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 819);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C06 RID: 19462 RVA: 0x002A4474 File Offset: 0x002A2674
		public void AddSectMainStoryBaihuaManiaCure(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 820);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 821);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x002A44D0 File Offset: 0x002A26D0
		public void AddGiveUpLegendaryBookSuccessHuaJu(int selfCharId, int date, sbyte itemType, short itemTemplateId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 827);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x002A4508 File Offset: 0x002A2708
		public void AddGiveUpLegendaryBookSuccessXuanZhi(int selfCharId, int date, sbyte itemType, short itemTemplateId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 828);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x002A4540 File Offset: 0x002A2740
		public void AddGiveUpLegendaryBookSuccessYingJiao(int selfCharId, int date, sbyte itemType, short itemTemplateId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 829);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C0A RID: 19466 RVA: 0x002A4578 File Offset: 0x002A2778
		public void AddSecretMakeEnemy2(int selfCharId, int date, int charId, Location location, short secretInfoTemplateId, int secretInfoId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 830);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 831);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendSecretInformation(secretInfoTemplateId, secretInfoId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C0B RID: 19467 RVA: 0x002A45EC File Offset: 0x002A27EC
		public void AddDecideToHuntFugitive(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 832);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x002A4624 File Offset: 0x002A2824
		public void AddFinishHuntFugitive(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 833);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x002A465C File Offset: 0x002A285C
		public void AddDecideToEscapePunishment(int selfCharId, int date, Location location, short punishmentType, short settlementId, Location location1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 834);
			base.AppendLocation(location);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.AppendLocation(location1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x002A46A4 File Offset: 0x002A28A4
		public void AddFinishEscapePunishment(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 835);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C0F RID: 19471 RVA: 0x002A46D0 File Offset: 0x002A28D0
		public void AddDecideToSeekAsylum(int selfCharId, int date, Location location, short punishmentType, short settlementId, short settlementId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 836);
			base.AppendLocation(location);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.AppendSettlement(settlementId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C10 RID: 19472 RVA: 0x002A4718 File Offset: 0x002A2918
		public void AddFinishSeekAsylum(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 837);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C11 RID: 19473 RVA: 0x002A4750 File Offset: 0x002A2950
		public void AddSeekAsylumSuccess(int selfCharId, int date, Location location, short settlementId, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 838);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C12 RID: 19474 RVA: 0x002A4794 File Offset: 0x002A2994
		public void AddDecideToEscortPrisoner(int selfCharId, int date, int charId, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 839);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C13 RID: 19475 RVA: 0x002A47D4 File Offset: 0x002A29D4
		public void AddEscortPrisonerSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 840);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C14 RID: 19476 RVA: 0x002A480C File Offset: 0x002A2A0C
		public void AddImprisonedShaoLin(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 841);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x002A4838 File Offset: 0x002A2A38
		public void AddImprisonedEmei1(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 842);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x002A4864 File Offset: 0x002A2A64
		public void AddImprisonedEmei2(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 843);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x002A4890 File Offset: 0x002A2A90
		public void AddImprisonedBaihua(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 844);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x002A48BC File Offset: 0x002A2ABC
		public void AddImprisonedWudang(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 845);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x002A48E8 File Offset: 0x002A2AE8
		public void AddImprisonedYuanshan(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 846);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x002A4914 File Offset: 0x002A2B14
		public void AddImprisonedShingXiang(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 847);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x002A4940 File Offset: 0x002A2B40
		public void AddImprisonedRanShan(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 848);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C1C RID: 19484 RVA: 0x002A496C File Offset: 0x002A2B6C
		public void AddImprisonedXuanNv(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 849);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x002A4998 File Offset: 0x002A2B98
		public void AddImprisonedZhuJian(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 850);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x002A49C4 File Offset: 0x002A2BC4
		public void AddImprisonedKongSang(int selfCharId, int date, short settlementId, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 851);
			base.AppendSettlement(settlementId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C1F RID: 19487 RVA: 0x002A49FC File Offset: 0x002A2BFC
		public void AddImprisonedJinGang(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 852);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x002A4A28 File Offset: 0x002A2C28
		public void AddImprisonedWuXian(int selfCharId, int date, short settlementId, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 853);
			base.AppendSettlement(settlementId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x002A4A60 File Offset: 0x002A2C60
		public void AddImprisonedJieQing1(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 854);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x002A4A8C File Offset: 0x002A2C8C
		public void AddImprisonedJieQing2(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 855);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x002A4AB8 File Offset: 0x002A2CB8
		public void AddImprisonedFuLong(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 856);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x002A4AE4 File Offset: 0x002A2CE4
		public void AddImprisonedXueHou(int selfCharId, int date, short settlementId, sbyte bodyPartType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 857);
			base.AppendSettlement(settlementId);
			base.AppendBodyPartType(bodyPartType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x002A4B1C File Offset: 0x002A2D1C
		public void AddIntrudePrisonCancelSupportMakeEnemyNpc(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 858);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x002A4B54 File Offset: 0x002A2D54
		public void AddIntrudePrisonCancelSupportMakeEnemyTaiwu(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 859);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x002A4B80 File Offset: 0x002A2D80
		public void AddIntrudePrisonCancelSupportNpc(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 860);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x002A4BB8 File Offset: 0x002A2DB8
		public void AddIntrudePrisonCancelSupportTaiwu(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 861);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x002A4BE4 File Offset: 0x002A2DE4
		public void AddIntrudePrisonMakeEnemyOthersNpc(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 862);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x002A4C1C File Offset: 0x002A2E1C
		public void AddIntrudePrisonMakeEnemyOthersTaiwu(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 863);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x002A4C48 File Offset: 0x002A2E48
		public void AddRequestTheReleaseOfTheCriminalTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 865);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 864);
			base.AppendCharacter(selfCharId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x002A4CA4 File Offset: 0x002A2EA4
		public void AddImprisonedXiangshuInfectedSupportIncreaseAndFavorabilityNpc(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 866);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x002A4CD0 File Offset: 0x002A2ED0
		public void AddImprisonedXiangshuInfectedSupportIncreaseAndFavorabilityTaiwu(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 867);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x002A4CFC File Offset: 0x002A2EFC
		public void AddImprisonedXiangshuInfectedIncreaseFavorabilityNpc(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 868);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x002A4D28 File Offset: 0x002A2F28
		public void AddImprisonedXiangshuInfectedIncreaseFavorabilityTaiwu(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 869);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x002A4D54 File Offset: 0x002A2F54
		public void AddImprisonedXiangshuInfectedTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 871);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 870);
			base.AppendCharacter(selfCharId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x002A4DB0 File Offset: 0x002A2FB0
		public void AddRobbedFromPrisonNpc(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 872);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x002A4DE8 File Offset: 0x002A2FE8
		public void AddPrisonBreakIntrudePrisonCancelSupportMakeEnemyNpc(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 873);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x002A4E28 File Offset: 0x002A3028
		public void AddPrisonBreakIntrudePrisonCancelSupportMakeEnemyTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 874);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C34 RID: 19508 RVA: 0x002A4E60 File Offset: 0x002A3060
		public void AddPrisonBreakIntrudePrisonCancelSupportNpc(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 875);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x002A4EA0 File Offset: 0x002A30A0
		public void AddPrisonBreakIntrudePrisonCancelSupportTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 876);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x002A4ED8 File Offset: 0x002A30D8
		public void AddPrisonBreakIntrudePrisonMakeEnemyOthersNpc(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 877);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x002A4F18 File Offset: 0x002A3118
		public void AddPrisonBreakIntrudePrisonMakeEnemyOthersTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 878);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x002A4F50 File Offset: 0x002A3150
		public void AddResistArrestIntrudePrisonCancelSupportMakeEnemyNpc(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 879);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x002A4F90 File Offset: 0x002A3190
		public void AddResistArresPrisonBreakIntrudePrisonCancelSupportMakeEnemyTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 880);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x002A4FC8 File Offset: 0x002A31C8
		public void AddResistArresPrisonBreakIntrudePrisonCancelSupportNpc(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 881);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x002A5008 File Offset: 0x002A3208
		public void AddResistArresPrisonBreakIntrudePrisonCancelSupportTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 882);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x002A5040 File Offset: 0x002A3240
		public void AddResistArresPrisonBreakIntrudePrisonMakeEnemyOthersNpc(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 883);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x002A5080 File Offset: 0x002A3280
		public void AddResistArresPrisonBreakIntrudePrisonMakeEnemyOthersTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 884);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x002A50B8 File Offset: 0x002A32B8
		public void AddArrestFailedCaptor(int selfCharId, int date, int charId, Location location, short settlementId, short relatedRecordId)
		{
			bool flag = relatedRecordId != 886 && relatedRecordId != 1035;
			if (flag)
			{
				throw LifeRecordCollection.CreateRelatedRecordIdException(relatedRecordId);
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 885);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, relatedRecordId);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C3F RID: 19519 RVA: 0x002A5148 File Offset: 0x002A3348
		public void AddResistArresEngageInBattleTaiwu(int selfCharId, int date, int charId, int charId1, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 887);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C40 RID: 19520 RVA: 0x002A5190 File Offset: 0x002A3390
		public void AddArrestedSuccessfullyCaptor(int selfCharId, int date, int charId, Location location, short settlementId, short relatedRecordId)
		{
			bool flag = relatedRecordId != 889 && relatedRecordId != 1036;
			if (flag)
			{
				throw LifeRecordCollection.CreateRelatedRecordIdException(relatedRecordId);
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 888);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, relatedRecordId);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x002A5220 File Offset: 0x002A3420
		public void AddReceiveCriminalsCaptor(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 890);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x002A5260 File Offset: 0x002A3460
		public void AddReceiveCriminalsTaiwu(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 891);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x002A52A0 File Offset: 0x002A34A0
		public void AddReceiveCriminalsCriminal(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 892);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x002A52E0 File Offset: 0x002A34E0
		public void AddBuyHandOverTheCriminalTaiwu(int selfCharId, int date, int charId, int charId1, short settlementId, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 894);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 893);
			base.AppendCharacter(selfCharId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x002A5374 File Offset: 0x002A3574
		public void AddLifeSkillBattleHandOverTheCriminalTaiwu(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 896);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 895);
			base.AppendCharacter(selfCharId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x002A53E4 File Offset: 0x002A35E4
		public void AddLifeSkillBattleLoseHandOverTheCriminalTaiwu(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 898);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 897);
			base.AppendCharacter(selfCharId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C47 RID: 19527 RVA: 0x002A5454 File Offset: 0x002A3654
		public void AddVictoryInCombatHandOverTheCriminalTaiwu(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 900);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 899);
			base.AppendCharacter(selfCharId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C48 RID: 19528 RVA: 0x002A54C4 File Offset: 0x002A36C4
		public void AddFailureInCombatHandOverTheCriminalTaiwu(int selfCharId, int date, int charId, int charId1, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 902);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 901);
			base.AppendCharacter(selfCharId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C49 RID: 19529 RVA: 0x002A5534 File Offset: 0x002A3734
		public void AddSectMainStoryFulongFightSucceed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 903);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x002A5560 File Offset: 0x002A3760
		public void AddSectMainStoryFulongFightFail(int selfCharId, int date, Location location, sbyte resourceType, int value, sbyte resourceType1, int value1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 904);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.AppendResource(resourceType1);
			base.AppendInteger(value1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x002A55B0 File Offset: 0x002A37B0
		public void AddSectMainStoryFulongRobbery(int selfCharId, int date, Location location, sbyte resourceType, int value, sbyte resourceType1, int value1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 905);
			base.AppendLocation(location);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.AppendResource(resourceType1);
			base.AppendInteger(value1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C4C RID: 19532 RVA: 0x002A5600 File Offset: 0x002A3800
		public void AddSectMainStoryFulongRobberKilledByTaiwu(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 906);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x002A562C File Offset: 0x002A382C
		public void AddSectMainStoryFulongProtect(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 907);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x002A5664 File Offset: 0x002A3864
		public void AddHonestSectPunishLevel1(int selfCharId, int date, short punishmentType, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 908);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x002A569C File Offset: 0x002A389C
		public void AddHonestSectPunishLevel2(int selfCharId, int date, short punishmentType, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 909);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x002A56DC File Offset: 0x002A38DC
		public void AddHonestSectPunishLevel3(int selfCharId, int date, short punishmentType, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 910);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x002A571C File Offset: 0x002A391C
		public void AddHonestSectPunishLevel4(int selfCharId, int date, short punishmentType, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 911);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C52 RID: 19538 RVA: 0x002A575C File Offset: 0x002A395C
		public void AddHonestSectPunishLevel5(int selfCharId, int date, short punishmentType, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 912);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x002A5794 File Offset: 0x002A3994
		public void AddHonestSectPunishTogetherWithSpouseLevel5(int selfCharId, int date, short punishmentType, short settlementId, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 913);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x002A57D4 File Offset: 0x002A39D4
		public void AddArrestedSectPunishLevel1(int selfCharId, int date, short punishmentType, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 914);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C55 RID: 19541 RVA: 0x002A580C File Offset: 0x002A3A0C
		public void AddArrestedSectPunishLevel2(int selfCharId, int date, short punishmentType, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 915);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x002A584C File Offset: 0x002A3A4C
		public void AddArrestedSectPunishLevel3(int selfCharId, int date, short punishmentType, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 916);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x002A588C File Offset: 0x002A3A8C
		public void AddArrestedSectPunishLevel4(int selfCharId, int date, short punishmentType, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 917);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x002A58CC File Offset: 0x002A3ACC
		public void AddArrestedSectPunishLevel5(int selfCharId, int date, short punishmentType, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 918);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x002A5904 File Offset: 0x002A3B04
		public void AddArrestedSectPunishTogetherWithSpouseLevel5(int selfCharId, int date, short punishmentType, short settlementId, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 919);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x002A5944 File Offset: 0x002A3B44
		public void AddBeImplicatedSectPunishLevel5(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 920);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x002A597C File Offset: 0x002A3B7C
		public void AddBeReleasedUponCompletionOfASentence(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 921);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C5C RID: 19548 RVA: 0x002A59A8 File Offset: 0x002A3BA8
		public void AddPrisonBreak(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 922);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x002A59D4 File Offset: 0x002A3BD4
		public void AddSendingToPrison1Taiwu(int selfCharId, int date, int charId, short settlementId, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 923);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C5E RID: 19550 RVA: 0x002A5A1C File Offset: 0x002A3C1C
		public void AddSendingToPrison2Taiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 924);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C5F RID: 19551 RVA: 0x002A5A54 File Offset: 0x002A3C54
		public void AddSendingToPrisonCriminal(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 925);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C60 RID: 19552 RVA: 0x002A5A8C File Offset: 0x002A3C8C
		public void AddSentToPrisonTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 926);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 927);
			base.AppendCharacter(selfCharId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x002A5AE8 File Offset: 0x002A3CE8
		public void AddCatchCriminalsWinTaiwu(int selfCharId, int date, int charId, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 928);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 929);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x002A5B58 File Offset: 0x002A3D58
		public void AddCatchCriminalsFailedTaiwu(int selfCharId, int date, int charId, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 930);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 931);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x002A5BC8 File Offset: 0x002A3DC8
		public void AddBuyHandOverTheCriminalTaiwuByExp(int selfCharId, int date, int charId, int charId1, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 933);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 932);
			base.AppendCharacter(selfCharId);
			base.AppendCharacter(charId1);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x002A5C48 File Offset: 0x002A3E48
		public void AddSendingToPrison1TaiwuByExp(int selfCharId, int date, int charId, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 934);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x002A5C88 File Offset: 0x002A3E88
		public void AddVillagerMigrateResources(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 935);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C66 RID: 19558 RVA: 0x002A5CC0 File Offset: 0x002A3EC0
		public void AddVillagerCookingIngredient(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 936);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x002A5D04 File Offset: 0x002A3F04
		public void AddVillagerMakingItem(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 937);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C68 RID: 19560 RVA: 0x002A5D48 File Offset: 0x002A3F48
		public void AddVillagerRepairItem0(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 938);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x002A5D80 File Offset: 0x002A3F80
		public void AddVillagerRepairItem1(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 939);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 960);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x002A5DF4 File Offset: 0x002A3FF4
		public void AddVillagerDisassembleItem0(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 940);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x002A5E2C File Offset: 0x002A402C
		public void AddVillagerDisassembleItem1(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 941);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x002A5E70 File Offset: 0x002A4070
		public void AddVillagerRefiningMedicine(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 942);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x002A5EB4 File Offset: 0x002A40B4
		public void AddVillagerDetoxify0(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 943);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C6E RID: 19566 RVA: 0x002A5EF8 File Offset: 0x002A40F8
		public void AddVillagerDetoxify1(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, sbyte itemType2, short itemTemplateId2)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 944);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.AppendItem(itemType2, itemTemplateId2);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x002A5F48 File Offset: 0x002A4148
		public void AddVillagerEnvenomedItem(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 945);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x002A5F8C File Offset: 0x002A418C
		public void AddVillagerSoldItem(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 946);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x002A5FE0 File Offset: 0x002A41E0
		public void AddVillagerBuyItem(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 947);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C72 RID: 19570 RVA: 0x002A6034 File Offset: 0x002A4234
		public void AddVillagerSeverEnemy(int selfCharId, int date, int charId, int charId1, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 948);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C73 RID: 19571 RVA: 0x002A6074 File Offset: 0x002A4274
		public void AddVillagerEmotionUp(int selfCharId, int date, int charId, int charId1, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 949);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C74 RID: 19572 RVA: 0x002A60B4 File Offset: 0x002A42B4
		public void AddVillagerMakeFriends(int selfCharId, int date, int charId, int charId1, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 950);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x002A60F4 File Offset: 0x002A42F4
		public void AddVillagerGetMarried(int selfCharId, int date, int charId, int charId1, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 951);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x002A6134 File Offset: 0x002A4334
		public void AddVillagerBecomeBrothers(int selfCharId, int date, int charId, int charId1, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 952);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x002A6174 File Offset: 0x002A4374
		public void AddVillagerAdopt(int selfCharId, int date, int charId, int charId1, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 953);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x002A61B4 File Offset: 0x002A43B4
		public void AddVillagerTreatment0(int selfCharId, int date, int charId, Location location, int value, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 954);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 956);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x002A6234 File Offset: 0x002A4434
		public void AddVillagerTreatment1(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 955);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 957);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x002A6290 File Offset: 0x002A4490
		public void AddXiangshuInfectedPrisonTaiwuVillage(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 958);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x002A62BC File Offset: 0x002A44BC
		public void AddXiangshuInfectedPrisonSettlement(int selfCharId, int date, Location location, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 959);
			base.AppendLocation(location);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x002A62F4 File Offset: 0x002A44F4
		public void AddTaiwuVillagerTakeItem(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 961);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x002A6324 File Offset: 0x002A4524
		public void AddTaiwuVillagerStorageItem(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 962);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x002A6354 File Offset: 0x002A4554
		public void AddTaiwuVillagerStorageResources(int selfCharId, int date, int value, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 963);
			base.AppendInteger(value);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x002A638C File Offset: 0x002A458C
		public void AddTaiwuVillagerTakeResources(int selfCharId, int date, int value, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 964);
			base.AppendInteger(value);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x002A63C4 File Offset: 0x002A45C4
		public void AddLiteratiEntertainingUp(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 965);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x002A63FC File Offset: 0x002A45FC
		public void AddLiteratiEntertainingDown(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 966);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x002A6434 File Offset: 0x002A4634
		public void AddLiteratiBuildingRelationshipUp(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 967);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x002A646C File Offset: 0x002A466C
		public void AddLiteratiBuildingRelationshipDown(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 968);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x002A64A4 File Offset: 0x002A46A4
		public void AddLiteratiSpreadingInfluenceUp(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 969);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x002A64D0 File Offset: 0x002A46D0
		public void AddLiteratiSpreadingInfluenceDown(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 970);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x002A64FC File Offset: 0x002A46FC
		public void AddSwordTombKeeperBuildingRelationshipUp(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 971);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x002A6534 File Offset: 0x002A4734
		public void AddSwordTombKeeperBuildingRelationshipDown(int selfCharId, int date, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 972);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C88 RID: 19592 RVA: 0x002A656C File Offset: 0x002A476C
		public void AddSwordTombKeeperSpreadingInfluenceUp(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 973);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x002A6598 File Offset: 0x002A4798
		public void AddSwordTombKeeperSpreadingInfluenceDown(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 974);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x002A65C4 File Offset: 0x002A47C4
		public void AddInquireSwordTomb(int selfCharId, int date, sbyte xiangshuAvatarId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 975);
			base.AppendSwordTomb(xiangshuAvatarId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x002A65F0 File Offset: 0x002A47F0
		public void AddGuardingSwordTomb(int selfCharId, int date, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 976);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x002A661C File Offset: 0x002A481C
		public void AddVillagerPrioritizedActions(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 977);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x002A6648 File Offset: 0x002A4848
		public void AddVillagerPrioritizedActionsStop(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 978);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x002A6674 File Offset: 0x002A4874
		public void AddEnvenomedItemOverload(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, sbyte itemType2, short itemTemplateId2, sbyte itemType3, short itemTemplateId3)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 979);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.AppendItem(itemType2, itemTemplateId2);
			base.AppendItem(itemType3, itemTemplateId3);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x002A66CC File Offset: 0x002A48CC
		public void AddDetoxifyItemOverload(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, sbyte itemType2, short itemTemplateId2, sbyte itemType3, short itemTemplateId3, sbyte itemType4, short itemTemplateId4)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 980);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.AppendItem(itemType2, itemTemplateId2);
			base.AppendItem(itemType3, itemTemplateId3);
			base.AppendItem(itemType4, itemTemplateId4);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C90 RID: 19600 RVA: 0x002A6730 File Offset: 0x002A4930
		public void AddVillagerEnvenomedItemOverload(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, sbyte itemType2, short itemTemplateId2, sbyte itemType3, short itemTemplateId3)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 981);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.AppendItem(itemType2, itemTemplateId2);
			base.AppendItem(itemType3, itemTemplateId3);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x002A6788 File Offset: 0x002A4988
		public void AddVillagerDetoxifyItemOverload(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, sbyte itemType2, short itemTemplateId2, sbyte itemType3, short itemTemplateId3, sbyte itemType4, short itemTemplateId4)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 982);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.AppendItem(itemType2, itemTemplateId2);
			base.AppendItem(itemType3, itemTemplateId3);
			base.AppendItem(itemType4, itemTemplateId4);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x002A67EC File Offset: 0x002A49EC
		public void AddVillagerCookingIngredientFailed0(int selfCharId, int date, Location location, short buildingTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 983);
			base.AppendLocation(location);
			base.AppendBuilding(buildingTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x002A6824 File Offset: 0x002A4A24
		public void AddVillagerCookingIngredientFailed1(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 984);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C94 RID: 19604 RVA: 0x002A6850 File Offset: 0x002A4A50
		public void AddVillagerMakingItemFailed0(int selfCharId, int date, Location location, short buildingTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 985);
			base.AppendLocation(location);
			base.AppendBuilding(buildingTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C95 RID: 19605 RVA: 0x002A6888 File Offset: 0x002A4A88
		public void AddVillagerMakingItemFailed1(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 986);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x002A68B4 File Offset: 0x002A4AB4
		public void AddVillagerRepairFailed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 987);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x002A68E0 File Offset: 0x002A4AE0
		public void AddVillagerDisassembleItemFailed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 988);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x002A690C File Offset: 0x002A4B0C
		public void AddVillagerRefiningMedicineFailed0(int selfCharId, int date, Location location, short buildingTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 989);
			base.AppendLocation(location);
			base.AppendBuilding(buildingTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x002A6944 File Offset: 0x002A4B44
		public void AddVillagerRefiningMedicineFailed1(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 990);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x002A6970 File Offset: 0x002A4B70
		public void AddVillagerAddPoisonToItemFailed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 991);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x002A699C File Offset: 0x002A4B9C
		public void AddVillagerDetoxItemFailed(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 992);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x002A69C8 File Offset: 0x002A4BC8
		public void AddVillagerDistanceFailed0(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 993);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x002A69F4 File Offset: 0x002A4BF4
		public void AddVillagerDistanceFailed1(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 994);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x002A6A20 File Offset: 0x002A4C20
		public void AddVillagerDistanceFailed2(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 995);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x002A6A4C File Offset: 0x002A4C4C
		public void AddVillagerAttainmentsFailed(int selfCharId, int date, Location location, sbyte lifeSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 996);
			base.AppendLocation(location);
			base.AppendLifeSkillType(lifeSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x002A6A84 File Offset: 0x002A4C84
		public void AddTaiwuPunishmentTongyong(int selfCharId, int date, short settlementId, int value, Location location, int value1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 997);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.AppendLocation(location);
			base.AppendInteger(value1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x002A6ACC File Offset: 0x002A4CCC
		public void AddTaiwuPunishmentShaolin(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 998);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x002A6AF0 File Offset: 0x002A4CF0
		public void AddTaiwuPunishmentEmei(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 999);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x002A6B1C File Offset: 0x002A4D1C
		public void AddTaiwuPunishmentBaihua(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1000);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x002A6B48 File Offset: 0x002A4D48
		public void AddTaiwuPunishmentWudang(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1001);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x002A6B74 File Offset: 0x002A4D74
		public void AddTaiwuPunishmentYuanshan(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1002);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x002A6B98 File Offset: 0x002A4D98
		public void AddTaiwuPunishmentShingXiang(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1003);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x002A6BC4 File Offset: 0x002A4DC4
		public void AddTaiwuPunishmentRanShan(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1004);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x002A6BF0 File Offset: 0x002A4DF0
		public void AddTaiwuPunishmentXuanNv(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1005);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x002A6C1C File Offset: 0x002A4E1C
		public void AddTaiwuPunishmentZhuJian(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1006);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x002A6C40 File Offset: 0x002A4E40
		public void AddTaiwuPunishmentKongSang(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1007);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x002A6C6C File Offset: 0x002A4E6C
		public void AddTaiwuPunishmentJinGang(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1008);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CAC RID: 19628 RVA: 0x002A6C98 File Offset: 0x002A4E98
		public void AddTaiwuPunishmentWuXian(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1009);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x002A6CC4 File Offset: 0x002A4EC4
		public void AddTaiwuPunishmentJieQing(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1010);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x002A6CF0 File Offset: 0x002A4EF0
		public void AddTaiwuPunishmentFuLong(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1011);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x002A6D1C File Offset: 0x002A4F1C
		public void AddTaiwuPunishmentXueHou(int selfCharId, int date, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1012);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x002A6D48 File Offset: 0x002A4F48
		public void AddSectPunishLevel5Expel(int selfCharId, int date, short punishmentType, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1013);
			base.AppendPunishmentType(punishmentType);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x002A6D80 File Offset: 0x002A4F80
		public void AddBeImplicatedSectPunishLevel5New(int selfCharId, int date, int charId, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1014);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CB2 RID: 19634 RVA: 0x002A6DC0 File Offset: 0x002A4FC0
		public void AddBeImplicatedSectPunishLevel5Expel(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1015);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x002A6DF8 File Offset: 0x002A4FF8
		public void AddResistArrestIntrudePrisonCancelSupportMakeEnemyNpcGuard(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1016);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CB4 RID: 19636 RVA: 0x002A6E30 File Offset: 0x002A5030
		public void AddResistArresPrisonBreakIntrudePrisonCancelSupportMakeEnemyTaiwuWanted(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1017);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CB5 RID: 19637 RVA: 0x002A6E5C File Offset: 0x002A505C
		public void AddResistArresPrisonBreakIntrudePrisonCancelSupportNpcGuard(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1018);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x002A6E94 File Offset: 0x002A5094
		public void AddResistArresPrisonBreakIntrudePrisonCancelSupportTaiwuWanted(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1019);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CB7 RID: 19639 RVA: 0x002A6EC0 File Offset: 0x002A50C0
		public void AddResistArresPrisonBreakIntrudePrisonMakeEnemyOthersNpcGuard(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1020);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x002A6EF8 File Offset: 0x002A50F8
		public void AddResistArresPrisonBreakIntrudePrisonMakeEnemyOthersTaiwuWanted(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1021);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x002A6F24 File Offset: 0x002A5124
		public void AddForgiveForCivilianSkill(int selfCharId, int date, int charId, Location location, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1022);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x002A6F64 File Offset: 0x002A5164
		public void AddBeggarEatSomeoneFood(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1023);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1024);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x002A6FD8 File Offset: 0x002A51D8
		public void AddAristocratReleasePrisoner(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1025);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x002A7004 File Offset: 0x002A5204
		public void AddPrisonerBeReleaseByAristocrat(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1026);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x002A703C File Offset: 0x002A523C
		public void AddJieQingPunishmentAssassinSetOut(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1027);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x002A7068 File Offset: 0x002A5268
		public void AddJieQingPunishmentAssassinSucceed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1028);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1029);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x002A70C4 File Offset: 0x002A52C4
		public void AddJieQingPunishmentAssassinFailed(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1030);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1031);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x002A7120 File Offset: 0x002A5320
		public void AddJieQingPunishmentAssassinGiveUp(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1032);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x002A714C File Offset: 0x002A534C
		public void AddExociseXiangshuInfectionVictoryInCombatDie(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1033);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1034);
			base.AppendCharacter(selfCharId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x002A7198 File Offset: 0x002A5398
		public void AddLifeSkillBattleWinAndAvoidArrestTaiwu(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1038);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1037);
			base.AppendCharacter(selfCharId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x002A71E4 File Offset: 0x002A53E4
		public void AddLifeSkillBattleLoseAndWasArrestedTaiwu(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1040);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1039);
			base.AppendCharacter(selfCharId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CC4 RID: 19652 RVA: 0x002A7230 File Offset: 0x002A5430
		public void AddBribeSucceededInAvoidingArrestTaiwuByAuthority(int selfCharId, int date, int charId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1042);
			base.AppendCharacter(charId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1041);
			base.AppendCharacter(selfCharId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x002A728C File Offset: 0x002A548C
		public void AddBribeSucceededInAvoidingArrestTaiwuByExp(int selfCharId, int date, int charId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1044);
			base.AppendCharacter(charId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1043);
			base.AppendCharacter(selfCharId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x002A72E8 File Offset: 0x002A54E8
		public void AddBribeSucceededInAvoidingArrestTaiwuByMoney(int selfCharId, int date, int charId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1046);
			base.AppendCharacter(charId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1045);
			base.AppendCharacter(selfCharId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x002A7344 File Offset: 0x002A5544
		public void AddSubmitToCaptureMeeklyTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1047);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1048);
			base.AppendCharacter(selfCharId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x002A73A0 File Offset: 0x002A55A0
		public void AddNormalInformationChangeProfession(int selfCharId, int date, Location location, int charId, int professionTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1049);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendProfession(professionTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x002A73E0 File Offset: 0x002A55E0
		public void AddFeedTheAnimal(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1050);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x002A741C File Offset: 0x002A561C
		public void AddProfessionDoctorLifeTransition(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1051);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1052);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x002A7478 File Offset: 0x002A5678
		public void AddCombatSkillKeyPointComprehensionByExp(int selfCharId, int date, short combatSkillTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1053);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x002A74A4 File Offset: 0x002A56A4
		public void AddCombatSkillKeyPointComprehensionByItems(int selfCharId, int date, short combatSkillTemplateId, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1054);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x002A74DC File Offset: 0x002A56DC
		public void AddCombatSkillKeyPointComprehensionByLoveRelationship(int selfCharId, int date, short combatSkillTemplateId, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1055);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x002A7514 File Offset: 0x002A5714
		public void AddCombatSkillKeyPointComprehensionByHatredRelationship(int selfCharId, int date, short combatSkillTemplateId, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1056);
			base.AppendCombatSkill(combatSkillTemplateId);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CCF RID: 19663 RVA: 0x002A754C File Offset: 0x002A574C
		public void AddSpiritualDebtKongsangPoisoned(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1057);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CD0 RID: 19664 RVA: 0x002A7570 File Offset: 0x002A5770
		public void AddMartialArtistSkill3NPCItemDropCaseA(int selfCharId, int date, Location location, int charId, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1058);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x002A75B0 File Offset: 0x002A57B0
		public void AddMartialArtistSkill3NPCItemDropCaseB(int selfCharId, int date, Location location, int charId, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1059);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x002A75F0 File Offset: 0x002A57F0
		public void AddSectPunishElopeSucceedJust(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1060);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x002A7628 File Offset: 0x002A5828
		public void AddSectPunishElopeSucceedKind(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1061);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x002A7660 File Offset: 0x002A5860
		public void AddSectPunishElopeSucceedEven(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1062);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x002A7698 File Offset: 0x002A5898
		public void AddSectPunishElopeSucceed(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1063);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x002A76D0 File Offset: 0x002A58D0
		public void AddVillagerGetRefineItem(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1064);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x002A7700 File Offset: 0x002A5900
		public void AddVillagerUpgradeRefineItem(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1065);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x002A773C File Offset: 0x002A593C
		public void AddVillagerTreatmentTaiwu(int selfCharId, int date, int charId, Location location, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1066);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x002A777C File Offset: 0x002A597C
		public void AddVillagerReduceXiangshuInfect(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1067);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x002A77A0 File Offset: 0x002A59A0
		public void AddVillagerEarnMoney(int selfCharId, int date, int charId, sbyte resourceType, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1068);
			base.AppendCharacter(charId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1069);
			base.AppendCharacter(selfCharId);
			base.AppendResource(resourceType);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x002A7810 File Offset: 0x002A5A10
		public void AddVillagerGetMerchantFavorability(int selfCharId, int date, int charId, Location location, sbyte merchantType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1072);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendMerchantType(merchantType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x002A7850 File Offset: 0x002A5A50
		public void AddVillagerGetMerchantFavorabilityTaiwu(int selfCharId, int date, int charId, Location location, sbyte merchantType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1073);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendMerchantType(merchantType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CDD RID: 19677 RVA: 0x002A7890 File Offset: 0x002A5A90
		public void AddLiteratiBeEntertainedUp(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1074);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CDE RID: 19678 RVA: 0x002A78C8 File Offset: 0x002A5AC8
		public void AddLiteratiBeEntertainedDown(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1075);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CDF RID: 19679 RVA: 0x002A7900 File Offset: 0x002A5B00
		public void AddLiteratiSpreadingInfluenceCultureUp(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1076);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CE0 RID: 19680 RVA: 0x002A792C File Offset: 0x002A5B2C
		public void AddLiteratiSpreadingInfluenceCultureDown(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1077);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x002A7958 File Offset: 0x002A5B58
		public void AddLiteratiSpreadingInfluenceSafetyUp(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1078);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x002A7984 File Offset: 0x002A5B84
		public void AddLiteratiSpreadingInfluenceSafetyDown(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1079);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CE3 RID: 19683 RVA: 0x002A79B0 File Offset: 0x002A5BB0
		public void AddLiteratiConnectRelationshipUp(int selfCharId, int date, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1080);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CE4 RID: 19684 RVA: 0x002A79E8 File Offset: 0x002A5BE8
		public void AddLiteratiConnectRelationshipDown(int selfCharId, int date, short settlementId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1081);
			base.AppendSettlement(settlementId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CE5 RID: 19685 RVA: 0x002A7A20 File Offset: 0x002A5C20
		public void AddLiteratiConnectRelationshipUpTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1082);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CE6 RID: 19686 RVA: 0x002A7A58 File Offset: 0x002A5C58
		public void AddLiteratiConnectRelationshipDownTaiwu(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1083);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CE7 RID: 19687 RVA: 0x002A7A90 File Offset: 0x002A5C90
		public void AddLiteratiBeConnectedRelationshipUp(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1084);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x002A7AC8 File Offset: 0x002A5CC8
		public void AddLiteratiBeConnectedRelationshipDown(int selfCharId, int date, int charId, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1085);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x002A7B00 File Offset: 0x002A5D00
		public void AddGuardingSwordTombXiangshuInfectUp(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1086);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CEA RID: 19690 RVA: 0x002A7B2C File Offset: 0x002A5D2C
		public void AddGuardingSwordTombSucceed(int selfCharId, int date, short charTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1087);
			base.AppendCharacterTemplate(charTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x002A7B58 File Offset: 0x002A5D58
		public void AddVillagerMakeEnemy(int selfCharId, int date, int charId, int charId1, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1088);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CEC RID: 19692 RVA: 0x002A7B98 File Offset: 0x002A5D98
		public void AddVillagerConfessLoveSucceed(int selfCharId, int date, int charId, int charId1, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1089);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CED RID: 19693 RVA: 0x002A7BD8 File Offset: 0x002A5DD8
		public void AddOrderProduct(int selfCharId, int date, int charId, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1090);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1095);
			base.AppendCharacter(selfCharId);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x002A7C34 File Offset: 0x002A5E34
		public void AddReceiveProduct(int selfCharId, int date, int charId, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1091);
			base.AppendCharacter(charId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1096);
			base.AppendCharacter(selfCharId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CEF RID: 19695 RVA: 0x002A7C94 File Offset: 0x002A5E94
		public void AddCaptureOrder(int selfCharId, int date, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1093);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1094);
			base.AppendCharacter(selfCharId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x002A7CF0 File Offset: 0x002A5EF0
		public void AddCaptureOrderIntermediator(int selfCharId, int date, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1097);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x002A7D28 File Offset: 0x002A5F28
		public void AddOrderProductForOthers(int selfCharId, int date, int charId, int charId1, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1098);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1099);
			base.AppendCharacter(selfCharId);
			base.AppendCharacter(charId1);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x002A7D98 File Offset: 0x002A5F98
		public void AddDeliveredOrderProduct(int selfCharId, int date, int charId, int charId1, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1100);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1101);
			base.AppendCharacter(selfCharId);
			base.AppendCharacter(charId1);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x002A7E0C File Offset: 0x002A600C
		public void AddAcquisitionDiscard(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1092);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x002A7E3C File Offset: 0x002A603C
		public void AddShopBuildingBaseDevelopLifeSkill(int selfCharId, int date, short buildingTemplateId, sbyte lifeSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1102);
			base.AppendBuilding(buildingTemplateId);
			base.AppendLifeSkillType(lifeSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x002A7E74 File Offset: 0x002A6074
		public void AddShopBuildingBaseDevelopCombatSkill(int selfCharId, int date, short buildingTemplateId, sbyte combatSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1103);
			base.AppendBuilding(buildingTemplateId);
			base.AppendCombatSkillType(combatSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x002A7EAC File Offset: 0x002A60AC
		public void AddShopBuildingPersonalityDevelopLifeSkill(int selfCharId, int date, short buildingTemplateId, sbyte lifeSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1104);
			base.AppendBuilding(buildingTemplateId);
			base.AppendLifeSkillType(lifeSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x002A7EE4 File Offset: 0x002A60E4
		public void AddShopBuildingPersonalityDevelopCombatSkill(int selfCharId, int date, short buildingTemplateId, sbyte combatSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1105);
			base.AppendBuilding(buildingTemplateId);
			base.AppendCombatSkillType(combatSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x002A7F1C File Offset: 0x002A611C
		public void AddShopBuildingLeaderDevelopLifeSkill(int selfCharId, int date, int charId, short buildingTemplateId, sbyte lifeSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1106);
			base.AppendCharacter(charId);
			base.AppendBuilding(buildingTemplateId);
			base.AppendLifeSkillType(lifeSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x002A7F5C File Offset: 0x002A615C
		public void AddShopBuildingLeaderDevelopCombatSkill(int selfCharId, int date, int charId, short buildingTemplateId, sbyte combatSkillType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1107);
			base.AppendCharacter(charId);
			base.AppendBuilding(buildingTemplateId);
			base.AppendCombatSkillType(combatSkillType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x002A7F9C File Offset: 0x002A619C
		public void AddShopBuildingLearnLifeSkill(int selfCharId, int date, short buildingTemplateId, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1108);
			base.AppendBuilding(buildingTemplateId);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x002A7FDC File Offset: 0x002A61DC
		public void AddShopBuildingLearnCombatSkill(int selfCharId, int date, short buildingTemplateId, sbyte itemType, short itemTemplateId, int value)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1109);
			base.AppendBuilding(buildingTemplateId);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendInteger(value);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x002A801C File Offset: 0x002A621C
		public void AddJoinTaiwuVillageAfterTaiwuVillageStoneClaimed(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1110);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x002A8048 File Offset: 0x002A6248
		public void AddTaiwuVillagerFinishedReading(int selfCharId, int date, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1111);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x002A8078 File Offset: 0x002A6278
		public void AddTaiwuVillagerSalaryReceived(int selfCharId, int date, short buildingTemplateId, int value, sbyte resourceType)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1112);
			base.AppendBuilding(buildingTemplateId);
			base.AppendInteger(value);
			base.AppendResource(resourceType);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x002A80B8 File Offset: 0x002A62B8
		public void AddChangeGradeDrop(int selfCharId, int date, Location location, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1113);
			base.AppendLocation(location);
			base.AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x002A80F4 File Offset: 0x002A62F4
		public void AddFarmerCollectMaterial(int selfCharId, int date, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1114);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x002A812C File Offset: 0x002A632C
		public void AddJoinOrganization(int selfCharId, int date, short settlementId, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1115);
			base.AppendSettlement(settlementId);
			base.AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x002A8168 File Offset: 0x002A6368
		public void AddBreakAwayOrganization(int selfCharId, int date, short settlementId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1116);
			base.AppendSettlement(settlementId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x002A8194 File Offset: 0x002A6394
		public void AddChangeOrganization(int selfCharId, int date, short settlementId, short settlementId1, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1117);
			base.AppendSettlement(settlementId);
			base.AppendSettlement(settlementId1);
			base.AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x002A81D8 File Offset: 0x002A63D8
		public void AddVillagerFavorabilityUp(int selfCharId, int date, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1118);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x002A8210 File Offset: 0x002A6410
		public void AddVillagerFavorabilityDown(int selfCharId, int date, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1119);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x002A8248 File Offset: 0x002A6448
		public void AddVillagerFavorabilityUpPerson(int selfCharId, int date, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1120);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x002A8280 File Offset: 0x002A6480
		public void AddVillagerFavorabilityDownPerson(int selfCharId, int date, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1121);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D08 RID: 19720 RVA: 0x002A82B8 File Offset: 0x002A64B8
		public void AddTeamUpProtection(int selfCharId, int date, Location location, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1122);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x002A82F8 File Offset: 0x002A64F8
		public void AddTeamUpRescue(int selfCharId, int date, Location location, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1123);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x002A8338 File Offset: 0x002A6538
		public void AddTeamUpMourn(int selfCharId, int date, Location location, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1124);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D0B RID: 19723 RVA: 0x002A8378 File Offset: 0x002A6578
		public void AddTeamUpVisitFriendOrFamily(int selfCharId, int date, Location location, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1125);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x002A83B8 File Offset: 0x002A65B8
		public void AddTeamUpFindTreasure(int selfCharId, int date, Location location, int charId, Location location1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1126);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendLocation(location1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x002A83F8 File Offset: 0x002A65F8
		public void AddTeamUpFindSpecialMaterial(int selfCharId, int date, Location location, int charId, Location location1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1127);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendLocation(location1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x002A8438 File Offset: 0x002A6638
		public void AddTeamUpTakeRevenge(int selfCharId, int date, Location location, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1128);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D0F RID: 19727 RVA: 0x002A8478 File Offset: 0x002A6678
		public void AddTeamUpContestForLegendaryBook(int selfCharId, int date, Location location, int charId, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1129);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D10 RID: 19728 RVA: 0x002A84B8 File Offset: 0x002A66B8
		public void AddTeamUpEscapeFromPrison(int selfCharId, int date, Location location, int charId, Location location1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1130);
			base.AppendLocation(location);
			base.AppendCharacter(charId);
			base.AppendLocation(location1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D11 RID: 19729 RVA: 0x002A84F8 File Offset: 0x002A66F8
		public void AddTeamUpSeekAsylum(int selfCharId, int date, short settlementId, int charId, short settlementId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1131);
			base.AppendSettlement(settlementId);
			base.AppendCharacter(charId);
			base.AppendSettlement(settlementId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D12 RID: 19730 RVA: 0x002A8538 File Offset: 0x002A6738
		public void AddGetInfected(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1132);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D13 RID: 19731 RVA: 0x002A8564 File Offset: 0x002A6764
		public void AddDieByInfected(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1133);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x002A8590 File Offset: 0x002A6790
		public void AddInheritLegacy(int selfCharId, int date, int charId, Location location, sbyte itemType, short itemTemplateId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1134);
			base.AppendCharacter(charId);
			base.AppendLocation(location);
			base.AppendItem(itemType, itemTemplateId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x002A85D0 File Offset: 0x002A67D0
		public void AddBanquet_1(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1135);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x002A860C File Offset: 0x002A680C
		public void AddBanquet_2(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1136);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x002A8648 File Offset: 0x002A6848
		public void AddBanquet_3(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, short Feast)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1137);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.AppendFeast(Feast);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x002A868C File Offset: 0x002A688C
		public void AddBanquet_4(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, short Feast)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1138);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.AppendFeast(Feast);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x002A86D0 File Offset: 0x002A68D0
		public void AddBanquet_5(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1139);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x002A870C File Offset: 0x002A690C
		public void AddBanquet_6(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1140);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x002A8748 File Offset: 0x002A6948
		public void AddBanquet_7(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, short Feast)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1141);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.AppendFeast(Feast);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x002A878C File Offset: 0x002A698C
		public void AddBanquet_8(int selfCharId, int date, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, short Feast)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1142);
			base.AppendItem(itemType, itemTemplateId);
			base.AppendItem(itemType1, itemTemplateId1);
			base.AppendFeast(Feast);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x002A87D0 File Offset: 0x002A69D0
		public void AddBanquet_9(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1143);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x002A87F4 File Offset: 0x002A69F4
		public void AddBanquet_10(int selfCharId, int date)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1144);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x002A8818 File Offset: 0x002A6A18
		public void AddSectMainStoryWudangInjured(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1145);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x002A8844 File Offset: 0x002A6A44
		public void AddExtendDarkAshTime(int selfCharId, int date, Location location)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1146);
			base.AppendLocation(location);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x002A8870 File Offset: 0x002A6A70
		public void AddAdoreInMarriage(int selfCharId, int date, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1147);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x002A889C File Offset: 0x002A6A9C
		public void AddSameAreaDistantMarriage(int selfCharId, int date, short settlementId, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1148);
			base.AppendSettlement(settlementId);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x002A88D4 File Offset: 0x002A6AD4
		public void AddSameStateDistantMarriage(int selfCharId, int date, short settlementId, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1149);
			base.AppendSettlement(settlementId);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D24 RID: 19748 RVA: 0x002A890C File Offset: 0x002A6B0C
		public void AddDifferentStateDistantMarriage(int selfCharId, int date, short settlementId, int charId)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1150);
			base.AppendSettlement(settlementId);
			base.AppendCharacter(charId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x002A8944 File Offset: 0x002A6B44
		public void AddDeathRecord(GameData.Domains.Character.Character character, CharacterDeathTypeItem deathType, ref CharacterDeathInfo deathInfo)
		{
			bool flag = deathType.DefaultLifeRecord < 0;
			if (!flag)
			{
				int selfCharId = character.GetId();
				int targetCharId = deathInfo.KillerId;
				LifeRecordItem srcLifeRecordCfg = LifeRecord.Instance[deathType.DefaultLifeRecord];
				int beginOffset = this.BeginAddingRecord(selfCharId, deathInfo.DeathDate, deathType.DefaultLifeRecord);
				this.<AddDeathRecord>g__FillParameters|957_0(srcLifeRecordCfg, targetCharId, ref deathInfo);
				base.EndAddingRecord(beginOffset);
				List<short> relatedIds = srcLifeRecordCfg.RelatedIds;
				bool flag2 = relatedIds != null && relatedIds.Count > 0 && targetCharId >= 0;
				if (flag2)
				{
					short relatedId = srcLifeRecordCfg.RelatedIds[0];
					int beginOffset2 = this.BeginAddingRecord(targetCharId, deathInfo.DeathDate, relatedId);
					this.<AddDeathRecord>g__FillParameters|957_0(srcLifeRecordCfg, selfCharId, ref deathInfo);
					base.EndAddingRecord(beginOffset2);
				}
			}
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x002A8A08 File Offset: 0x002A6C08
		public void AddBecomeEnemyRecord(GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar, BecomeEnemyTypeItem becomeEnemyType, ref CharacterBecomeEnemyInfo becomeEnemyInfo)
		{
			bool flag = becomeEnemyType.DefaultLifeRecord < 0;
			if (!flag)
			{
				int selfCharId = selfChar.GetId();
				int targetCharId = targetChar.GetId();
				LifeRecordItem srcLifeRecordCfg = LifeRecord.Instance[becomeEnemyType.DefaultLifeRecord];
				int beginOffset = this.BeginAddingRecord(selfCharId, becomeEnemyInfo.Date, becomeEnemyType.DefaultLifeRecord);
				this.<AddBecomeEnemyRecord>g__FillParameters|958_0(srcLifeRecordCfg, targetCharId, ref becomeEnemyInfo);
				base.EndAddingRecord(beginOffset);
				List<short> relatedIds = srcLifeRecordCfg.RelatedIds;
				bool flag2 = relatedIds != null && relatedIds.Count > 0;
				if (flag2)
				{
					short relatedId = srcLifeRecordCfg.RelatedIds[0];
					int beginOffset2 = this.BeginAddingRecord(targetCharId, becomeEnemyInfo.Date, relatedId);
					this.<AddBecomeEnemyRecord>g__FillParameters|958_0(srcLifeRecordCfg, selfCharId, ref becomeEnemyInfo);
					base.EndAddingRecord(beginOffset2);
				}
			}
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x002A8ACC File Offset: 0x002A6CCC
		public void AddSectPunishmentRecord(PunishmentTypeItem punishmentTypeCfg, PunishmentSeverityItem severityCfg, short settlementId, bool isArrested, GameData.Domains.Character.Character selfChar, int implicatedCharId)
		{
			int selfCharId = selfChar.GetId();
			int date = DomainManager.World.GetCurrDate();
			bool flag = punishmentTypeCfg.TemplateId == 20;
			if (flag)
			{
				this.AddBeImplicatedSectPunishLevel5New(selfCharId, date, implicatedCharId, settlementId, severityCfg.PrisonTime);
			}
			else
			{
				short recordId = isArrested ? severityCfg.ArrestedRecord : severityCfg.NormalRecord;
				int beginOffset = this.BeginAddingRecord(selfCharId, date, recordId);
				base.AppendPunishmentType(punishmentTypeCfg.TemplateId);
				base.AppendSettlement(settlementId);
				bool flag2 = severityCfg.PrisonTime >= 0;
				if (flag2)
				{
					base.AppendInteger(severityCfg.PrisonTime);
				}
				bool flag3 = implicatedCharId >= 0;
				if (flag3)
				{
					base.AppendCharacter(implicatedCharId);
				}
				base.EndAddingRecord(beginOffset);
			}
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x002A8B84 File Offset: 0x002A6D84
		public void AddMixedPoisonEffectRecord(GameData.Domains.Character.Character character, MixPoisonEffectItem mixPoisonEffectCfg)
		{
			bool flag = mixPoisonEffectCfg.LifeRecord < 0;
			if (!flag)
			{
				int date = DomainManager.World.GetCurrDate();
				int selfCharId = character.GetId();
				Location location = character.GetValidLocation();
				LifeRecordItem lifeRecordCfg = LifeRecord.Instance[mixPoisonEffectCfg.LifeRecord];
				Tester.Assert(lifeRecordCfg.Parameters[0] == "Location", "");
				Tester.Assert(string.IsNullOrEmpty(lifeRecordCfg.Parameters[1]), "");
				int beginOffset = this.BeginAddingRecord(selfCharId, date, mixPoisonEffectCfg.LifeRecord);
				base.AppendLocation(location);
				base.EndAddingRecord(beginOffset);
			}
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x002A8C28 File Offset: 0x002A6E28
		public void AddCaptureOrderCorrectly(int selfCharId, int date, int charId, int charId1)
		{
			int beginOffset = this.BeginAddingRecord(selfCharId, date, 1093);
			base.AppendCharacter(charId);
			base.AppendCharacter(charId1);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId1, date, 1094);
			base.AppendCharacter(charId);
			base.AppendCharacter(selfCharId);
			base.EndAddingRecord(beginOffset);
			beginOffset = this.BeginAddingRecord(charId, date, 1097);
			base.AppendCharacter(charId1);
			base.AppendCharacter(selfCharId);
			base.EndAddingRecord(beginOffset);
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x002A8CAC File Offset: 0x002A6EAC
		public void AddTeamUp(GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character leader, short actionTemplateId, bool addBackwards)
		{
			int selfCharId = selfChar.GetId();
			int leaderId = leader.GetId();
			BasePrioritizedAction selfAction;
			bool flag = selfCharId == leaderId || actionTemplateId < 0 || !DomainManager.Character.TryGetCharacterPrioritizedAction(selfCharId, out selfAction);
			if (!flag)
			{
				if (!true)
				{
				}
				short num;
				switch (actionTemplateId)
				{
				case 2:
					num = 1122;
					goto IL_EA;
				case 3:
					num = 1123;
					goto IL_EA;
				case 4:
					num = 1124;
					goto IL_EA;
				case 5:
					num = 1125;
					goto IL_EA;
				case 6:
					num = 1126;
					goto IL_EA;
				case 7:
					num = 1127;
					goto IL_EA;
				case 8:
					num = 1128;
					goto IL_EA;
				case 9:
					num = 1129;
					goto IL_EA;
				case 18:
					num = 1130;
					goto IL_EA;
				case 19:
					num = 1131;
					goto IL_EA;
				}
				num = -1;
				IL_EA:
				if (!true)
				{
				}
				short defKey = num;
				bool flag2 = defKey < 0;
				if (!flag2)
				{
					int date = DomainManager.World.GetCurrDate();
					Location location = selfChar.GetLocation();
					switch (defKey)
					{
					case 1122:
					case 1123:
					case 1124:
					case 1125:
					case 1128:
					{
						int targetCharId = selfAction.Target.TargetCharId;
						int beginOffset = this.BeginAddingRecord(selfCharId, date, defKey);
						base.AppendLocation(location);
						base.AppendCharacter(leaderId);
						base.AppendCharacter(targetCharId);
						base.EndAddingRecord(beginOffset);
						break;
					}
					case 1126:
					case 1127:
					case 1130:
					{
						Location targetLocation = selfAction.Target.GetRealTargetLocation();
						int beginOffset2 = this.BeginAddingRecord(selfCharId, date, defKey);
						base.AppendLocation(location);
						base.AppendCharacter(leaderId);
						base.AppendLocation(targetLocation);
						base.EndAddingRecord(beginOffset2);
						break;
					}
					case 1129:
					{
						ContestForLegendaryBookAction action = selfAction as ContestForLegendaryBookAction;
						bool flag3 = action != null;
						if (flag3)
						{
							sbyte bookType = action.LegendaryBookType;
							int beginOffset3 = this.BeginAddingRecord(selfCharId, date, defKey);
							base.AppendLocation(location);
							base.AppendCharacter(leaderId);
							base.AppendItem(12, (short)(211 + (int)bookType));
							base.EndAddingRecord(beginOffset3);
						}
						break;
					}
					case 1131:
					{
						SeekAsylumAction action2 = selfAction as SeekAsylumAction;
						bool flag4 = action2 != null;
						if (flag4)
						{
							sbyte sectOrgTemplateId = DomainManager.Organization.GetFugitiveBountySect(selfCharId);
							Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(sectOrgTemplateId);
							short settlementId = sect.GetId();
							short settlementId2 = action2.SettlementId;
							int beginOffset4 = this.BeginAddingRecord(selfCharId, date, defKey);
							base.AppendSettlement(settlementId);
							base.AppendCharacter(leaderId);
							base.AppendSettlement(settlementId2);
							base.EndAddingRecord(beginOffset4);
						}
						break;
					}
					}
					if (addBackwards)
					{
						this.AddTeamUp(leader, selfChar, actionTemplateId, false);
					}
				}
			}
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x002A8F78 File Offset: 0x002A7178
		public int AddFeastRecord(GameData.Domains.Character.Character selfChar, ItemKey dish, ItemKey gift, short feastType, bool loveItem)
		{
			int selfCharId = selfChar.GetId();
			int date = DomainManager.World.GetCurrDate();
			bool flag = (int)selfChar.GetHappiness() <= GlobalConfig.Instance.FeastLowHappiness;
			short defKey;
			if (flag)
			{
				bool flag2 = feastType == 9;
				if (flag2)
				{
					defKey = ((!loveItem) ? 1135 : 1136);
				}
				else
				{
					defKey = ((!loveItem) ? 1137 : 1138);
				}
			}
			else
			{
				bool flag3 = feastType == 9;
				if (flag3)
				{
					defKey = ((!loveItem) ? 1139 : 1140);
				}
				else
				{
					defKey = ((!loveItem) ? 1141 : 1142);
				}
			}
			int beginOffset = this.BeginAddingRecord(selfCharId, date, defKey);
			base.AppendItem(dish.ItemType, dish.TemplateId);
			base.AppendItem(gift.ItemType, gift.TemplateId);
			bool flag4 = feastType != 9;
			if (flag4)
			{
				base.AppendFeast(feastType);
			}
			base.EndAddingRecord(beginOffset);
			return beginOffset;
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x002A9070 File Offset: 0x002A7270
		[CompilerGenerated]
		private void <AddDeathRecord>g__FillParameters|957_0(LifeRecordItem recordCfg, int relatedCharId, ref CharacterDeathInfo deathInfo)
		{
			bool flag = !recordCfg.IsSourceRecord;
			if (flag)
			{
				recordCfg = LifeRecord.Instance[recordCfg.RelatedIds[0]];
			}
			for (int i = 0; i < recordCfg.Parameters.Length; i++)
			{
				string paramTypeStr = recordCfg.Parameters[i];
				bool flag2 = string.IsNullOrEmpty(paramTypeStr);
				if (flag2)
				{
					break;
				}
				sbyte paramType = ParameterType.Parse(paramTypeStr);
				sbyte b = paramType;
				sbyte b2 = b;
				if (b2 != 0)
				{
					if (b2 != 1)
					{
						if (b2 != 10)
						{
							throw new Exception("Unrecognized parameter type for death record " + recordCfg.Name);
						}
						base.AppendAdventure(deathInfo.AdventureId);
					}
					else
					{
						base.AppendLocation(deathInfo.Location);
					}
				}
				else
				{
					base.AppendCharacter(relatedCharId);
				}
			}
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x002A913C File Offset: 0x002A733C
		[CompilerGenerated]
		private void <AddBecomeEnemyRecord>g__FillParameters|958_0(LifeRecordItem recordCfg, int relatedCharId, ref CharacterBecomeEnemyInfo becomeEnemyInfo)
		{
			bool flag = !recordCfg.IsSourceRecord;
			if (flag)
			{
				recordCfg = LifeRecord.Instance[recordCfg.RelatedIds[0]];
			}
			for (int i = 0; i < recordCfg.Parameters.Length; i++)
			{
				string paramTypeStr = recordCfg.Parameters[i];
				bool flag2 = string.IsNullOrEmpty(paramTypeStr);
				if (flag2)
				{
					break;
				}
				sbyte paramType = ParameterType.Parse(paramTypeStr);
				sbyte b = paramType;
				sbyte b2 = b;
				switch (b2)
				{
				case 0:
					base.AppendCharacter(relatedCharId);
					break;
				case 1:
					base.AppendLocation(becomeEnemyInfo.Location);
					break;
				case 2:
					base.AppendItem(8, becomeEnemyInfo.WugTemplateId);
					break;
				default:
					if (b2 != 30)
					{
						throw new Exception("Unrecognized parameter type for death record " + recordCfg.Name);
					}
					base.AppendSecretInformationTemplate(becomeEnemyInfo.SecretInformationTemplateId);
					break;
				}
			}
		}

		// Token: 0x0400155A RID: 5466
		private const int DefaultCapacity = 65536;
	}
}
