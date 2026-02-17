using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character
{
	// Token: 0x02000807 RID: 2055
	[SerializableGameData(NotForDisplayModule = true, NoCopyConstructors = true, IsExtensible = true)]
	public class CharacterDataPackage : ISerializableGameData
	{
		// Token: 0x06007017 RID: 28695 RVA: 0x003FE7C0 File Offset: 0x003FC9C0
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06007018 RID: 28696 RVA: 0x003FE7D4 File Offset: 0x003FC9D4
		public int GetSerializedSize()
		{
			int totalSize = 2;
			bool flag = this.Character != null;
			if (flag)
			{
				totalSize += 4 + this.Character.GetSerializedSize();
			}
			else
			{
				totalSize += 4;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, CombatSkill>(this.CombatSkills);
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<short, int>(this.CombatSkillProficiencies);
			bool flag2 = this.ItemGroupPackage != null;
			if (flag2)
			{
				totalSize += 4 + this.ItemGroupPackage.GetSerializedSize();
			}
			else
			{
				totalSize += 4;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007019 RID: 28697 RVA: 0x003FE85C File Offset: 0x003FCA5C
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 4;
			byte* pCurrData = pData + 2;
			bool flag = this.Character != null;
			if (flag)
			{
				byte* pSubDataCount = pCurrData;
				pCurrData += 4;
				int fieldSize = this.Character.Serialize(pCurrData);
				pCurrData += fieldSize;
				Tester.Assert(fieldSize <= int.MaxValue, "");
				*(int*)pSubDataCount = fieldSize;
			}
			else
			{
				*(int*)pCurrData = 0;
				pCurrData += 4;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, CombatSkill>(pCurrData, ref this.CombatSkills);
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<short, int>(pCurrData, ref this.CombatSkillProficiencies);
			bool flag2 = this.ItemGroupPackage != null;
			if (flag2)
			{
				byte* pSubDataCount2 = pCurrData;
				pCurrData += 4;
				int fieldSize2 = this.ItemGroupPackage.Serialize(pCurrData);
				pCurrData += fieldSize2;
				Tester.Assert(fieldSize2 <= int.MaxValue, "");
				*(int*)pSubDataCount2 = fieldSize2;
			}
			else
			{
				*(int*)pCurrData = 0;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600701A RID: 28698 RVA: 0x003FE954 File Offset: 0x003FCB54
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				int fieldSize = *(int*)pCurrData;
				pCurrData += 4;
				bool flag2 = fieldSize > 0;
				if (flag2)
				{
					bool flag3 = this.Character == null;
					if (flag3)
					{
						this.Character = new Character();
					}
					pCurrData += this.Character.Deserialize(pCurrData);
				}
				else
				{
					this.Character = null;
				}
			}
			bool flag4 = fieldCount > 1;
			if (flag4)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, CombatSkill>(pCurrData, ref this.CombatSkills);
			}
			bool flag5 = fieldCount > 2;
			if (flag5)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<short, int>(pCurrData, ref this.CombatSkillProficiencies);
			}
			bool flag6 = fieldCount > 3;
			if (flag6)
			{
				int fieldSize2 = *(int*)pCurrData;
				pCurrData += 4;
				bool flag7 = fieldSize2 > 0;
				if (flag7)
				{
					bool flag8 = this.ItemGroupPackage == null;
					if (flag8)
					{
						this.ItemGroupPackage = new ItemGroupPackage();
					}
					pCurrData += this.ItemGroupPackage.Deserialize(pCurrData);
				}
				else
				{
					this.ItemGroupPackage = null;
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001E4F RID: 7759
		[SerializableGameDataField(SubDataMaxCount = 2147483647)]
		public Character Character;

		// Token: 0x04001E50 RID: 7760
		[SerializableGameDataField]
		public Dictionary<short, CombatSkill> CombatSkills;

		// Token: 0x04001E51 RID: 7761
		[SerializableGameDataField]
		public Dictionary<short, int> CombatSkillProficiencies;

		// Token: 0x04001E52 RID: 7762
		[SerializableGameDataField(SubDataMaxCount = 2147483647)]
		public ItemGroupPackage ItemGroupPackage;

		// Token: 0x02000BE4 RID: 3044
		private static class FieldIds
		{
			// Token: 0x04003379 RID: 13177
			public const ushort Character = 0;

			// Token: 0x0400337A RID: 13178
			public const ushort CombatSkills = 1;

			// Token: 0x0400337B RID: 13179
			public const ushort CombatSkillProficiencies = 2;

			// Token: 0x0400337C RID: 13180
			public const ushort ItemGroupPackage = 3;

			// Token: 0x0400337D RID: 13181
			public const ushort Count = 4;

			// Token: 0x0400337E RID: 13182
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"Character",
				"CombatSkills",
				"CombatSkillProficiencies",
				"ItemGroupPackage"
			};
		}
	}
}
