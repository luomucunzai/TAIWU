using System;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Taiwu
{
	// Token: 0x02000045 RID: 69
	[SerializableGameDataSourceGenerator.AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotForDisplayModule = true)]
	public class VillagerRoleRecordElement : ISerializableGameData
	{
		// Token: 0x06001369 RID: 4969 RVA: 0x00138608 File Offset: 0x00136808
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0013861C File Offset: 0x0013681C
		public int GetSerializedSize()
		{
			int totalSize = 14;
			totalSize += this.Personalities.GetSerializedSize();
			totalSize += this.CombatSkillAttainments.GetSerializedSize();
			totalSize += this.LifeSkillAttainments.GetSerializedSize();
			bool flag = this.Avatar != null;
			if (flag)
			{
				totalSize += 2 + this.Avatar.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			totalSize += this.Name.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0013869C File Offset: 0x0013689C
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 9;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.CharacterId;
			pCurrData += 4;
			*(short*)pCurrData = this.CharacterTemplateId;
			pCurrData += 2;
			*(short*)pCurrData = this.RoleTemplateId;
			pCurrData += 2;
			int fieldSize = this.Personalities.Serialize(pCurrData);
			pCurrData += fieldSize;
			Tester.Assert(fieldSize <= 65535, "");
			int fieldSize2 = this.CombatSkillAttainments.Serialize(pCurrData);
			pCurrData += fieldSize2;
			Tester.Assert(fieldSize2 <= 65535, "");
			int fieldSize3 = this.LifeSkillAttainments.Serialize(pCurrData);
			pCurrData += fieldSize3;
			Tester.Assert(fieldSize3 <= 65535, "");
			bool flag = this.Avatar != null;
			if (flag)
			{
				byte* pSubDataCount = pCurrData;
				pCurrData += 2;
				int fieldSize4 = this.Avatar.Serialize(pCurrData);
				pCurrData += fieldSize4;
				Tester.Assert(fieldSize4 <= 65535, "");
				*(short*)pSubDataCount = (short)((ushort)fieldSize4);
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int fieldSize5 = this.Name.Serialize(pCurrData);
			pCurrData += fieldSize5;
			Tester.Assert(fieldSize5 <= 65535, "");
			*(int*)pCurrData = this.Date;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00138804 File Offset: 0x00136A04
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.CharacterId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				this.CharacterTemplateId = *(short*)pCurrData;
				pCurrData += 2;
			}
			bool flag3 = fieldCount > 2;
			if (flag3)
			{
				this.RoleTemplateId = *(short*)pCurrData;
				pCurrData += 2;
			}
			bool flag4 = fieldCount > 3;
			if (flag4)
			{
				pCurrData += this.Personalities.Deserialize(pCurrData);
			}
			bool flag5 = fieldCount > 4;
			if (flag5)
			{
				pCurrData += this.CombatSkillAttainments.Deserialize(pCurrData);
			}
			bool flag6 = fieldCount > 5;
			if (flag6)
			{
				pCurrData += this.LifeSkillAttainments.Deserialize(pCurrData);
			}
			bool flag7 = fieldCount > 6;
			if (flag7)
			{
				ushort classCount = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag8 = classCount > 0;
				if (flag8)
				{
					this.Avatar = new AvatarRelatedData();
					pCurrData += this.Avatar.Deserialize(pCurrData);
				}
				else
				{
					this.Avatar = null;
				}
			}
			bool flag9 = fieldCount > 7;
			if (flag9)
			{
				pCurrData += this.Name.Deserialize(pCurrData);
			}
			bool flag10 = fieldCount > 8;
			if (flag10)
			{
				this.Date = *(int*)pCurrData;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400030B RID: 779
		[SerializableGameDataField(FieldIndex = 0)]
		public int CharacterId;

		// Token: 0x0400030C RID: 780
		[SerializableGameDataField(FieldIndex = 1)]
		public short CharacterTemplateId;

		// Token: 0x0400030D RID: 781
		[SerializableGameDataField(FieldIndex = 2)]
		public short RoleTemplateId;

		// Token: 0x0400030E RID: 782
		[SerializableGameDataField(FieldIndex = 3)]
		public Personalities Personalities;

		// Token: 0x0400030F RID: 783
		[SerializableGameDataField(FieldIndex = 4)]
		public CombatSkillShorts CombatSkillAttainments;

		// Token: 0x04000310 RID: 784
		[SerializableGameDataField(FieldIndex = 5)]
		public LifeSkillShorts LifeSkillAttainments;

		// Token: 0x04000311 RID: 785
		[SerializableGameDataField(FieldIndex = 6)]
		public AvatarRelatedData Avatar;

		// Token: 0x04000312 RID: 786
		[SerializableGameDataField(FieldIndex = 7)]
		public NameRelatedData Name;

		// Token: 0x04000313 RID: 787
		[SerializableGameDataField(FieldIndex = 8)]
		public int Date;

		// Token: 0x0200095B RID: 2395
		public static class FieldIds
		{
			// Token: 0x04002743 RID: 10051
			public const ushort CharacterId = 0;

			// Token: 0x04002744 RID: 10052
			public const ushort CharacterTemplateId = 1;

			// Token: 0x04002745 RID: 10053
			public const ushort RoleTemplateId = 2;

			// Token: 0x04002746 RID: 10054
			public const ushort Personalities = 3;

			// Token: 0x04002747 RID: 10055
			public const ushort CombatSkillAttainments = 4;

			// Token: 0x04002748 RID: 10056
			public const ushort LifeSkillAttainments = 5;

			// Token: 0x04002749 RID: 10057
			public const ushort Avatar = 6;

			// Token: 0x0400274A RID: 10058
			public const ushort Name = 7;

			// Token: 0x0400274B RID: 10059
			public const ushort Date = 8;

			// Token: 0x0400274C RID: 10060
			public const ushort Count = 9;

			// Token: 0x0400274D RID: 10061
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"CharacterId",
				"CharacterTemplateId",
				"RoleTemplateId",
				"Personalities",
				"CombatSkillAttainments",
				"LifeSkillAttainments",
				"Avatar",
				"Name",
				"Date"
			};
		}
	}
}
