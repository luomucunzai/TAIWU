using System;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character
{
	// Token: 0x02000804 RID: 2052
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
	public class AbridgedCharacter : ISerializableGameData
	{
		// Token: 0x06006BDC RID: 27612 RVA: 0x003C793C File Offset: 0x003C5B3C
		public AvatarRelatedData GenerateAvatarRelatedData()
		{
			return new AvatarRelatedData
			{
				AvatarData = new AvatarData(this.Avatar),
				DisplayAge = this.CurrAge,
				ClothingDisplayId = this.ClothingDisplayId
			};
		}

		// Token: 0x06006BDD RID: 27613 RVA: 0x003C797C File Offset: 0x003C5B7C
		public AbridgedCharacter()
		{
			this.Id = -1;
			this.Avatar = new AvatarData();
		}

		// Token: 0x06006BDE RID: 27614 RVA: 0x003C7998 File Offset: 0x003C5B98
		public AbridgedCharacter(AbridgedCharacter other)
		{
			this.Id = other.Id;
			this.CharTemplateId = other.CharTemplateId;
			this.Gender = other.Gender;
			this.MonkType = other.MonkType;
			this.Avatar = new AvatarData(other.Avatar);
			this.ClothingDisplayId = other.ClothingDisplayId;
			this.FullName = other.FullName;
			this.OrganizationInfo = other.OrganizationInfo;
			this.MonasticTitle = other.MonasticTitle;
			this.CustomDisplayNameId = other.CustomDisplayNameId;
			this.SelfRelationToTaiwu = other.SelfRelationToTaiwu;
			this.TaiwuRelationToSelf = other.TaiwuRelationToSelf;
			this.AliveState = other.AliveState;
			this.CurrAge = other.CurrAge;
			this.ActualAge = other.ActualAge;
			this.Location = other.Location;
			this.BirthDate = other.BirthDate;
		}

		// Token: 0x06006BDF RID: 27615 RVA: 0x003C7A80 File Offset: 0x003C5C80
		public void Assign(AbridgedCharacter other)
		{
			this.Id = other.Id;
			this.CharTemplateId = other.CharTemplateId;
			this.Gender = other.Gender;
			this.MonkType = other.MonkType;
			this.Avatar = new AvatarData(other.Avatar);
			this.ClothingDisplayId = other.ClothingDisplayId;
			this.FullName = other.FullName;
			this.OrganizationInfo = other.OrganizationInfo;
			this.MonasticTitle = other.MonasticTitle;
			this.CustomDisplayNameId = other.CustomDisplayNameId;
			this.SelfRelationToTaiwu = other.SelfRelationToTaiwu;
			this.TaiwuRelationToSelf = other.TaiwuRelationToSelf;
			this.AliveState = other.AliveState;
			this.CurrAge = other.CurrAge;
			this.ActualAge = other.ActualAge;
			this.Location = other.Location;
			this.BirthDate = other.BirthDate;
		}

		// Token: 0x06006BE0 RID: 27616 RVA: 0x003C7B60 File Offset: 0x003C5D60
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06006BE1 RID: 27617 RVA: 0x003C7B74 File Offset: 0x003C5D74
		public int GetSerializedSize()
		{
			int totalSize = 131;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006BE2 RID: 27618 RVA: 0x003C7B9C File Offset: 0x003C5D9C
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 17;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.Id;
			pCurrData += 4;
			*(short*)pCurrData = this.CharTemplateId;
			pCurrData += 2;
			*pCurrData = (byte)this.Gender;
			pCurrData++;
			*pCurrData = this.MonkType;
			pCurrData++;
			pCurrData += this.Avatar.Serialize(pCurrData);
			*(short*)pCurrData = this.ClothingDisplayId;
			pCurrData += 2;
			pCurrData += this.FullName.Serialize(pCurrData);
			pCurrData += this.OrganizationInfo.Serialize(pCurrData);
			pCurrData += this.MonasticTitle.Serialize(pCurrData);
			*(int*)pCurrData = this.CustomDisplayNameId;
			pCurrData += 4;
			*(short*)pCurrData = (short)this.SelfRelationToTaiwu;
			pCurrData += 2;
			*(short*)pCurrData = (short)this.TaiwuRelationToSelf;
			pCurrData += 2;
			*pCurrData = (byte)this.AliveState;
			pCurrData++;
			*(short*)pCurrData = this.CurrAge;
			pCurrData += 2;
			*(short*)pCurrData = this.ActualAge;
			pCurrData += 2;
			pCurrData += this.Location.Serialize(pCurrData);
			*(int*)pCurrData = this.BirthDate;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006BE3 RID: 27619 RVA: 0x003C7CAC File Offset: 0x003C5EAC
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.Id = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				this.CharTemplateId = *(short*)pCurrData;
				pCurrData += 2;
			}
			bool flag3 = fieldCount > 2;
			if (flag3)
			{
				this.Gender = *(sbyte*)pCurrData;
				pCurrData++;
			}
			bool flag4 = fieldCount > 3;
			if (flag4)
			{
				this.MonkType = *pCurrData;
				pCurrData++;
			}
			bool flag5 = fieldCount > 4;
			if (flag5)
			{
				bool flag6 = this.Avatar == null;
				if (flag6)
				{
					this.Avatar = new AvatarData();
				}
				pCurrData += this.Avatar.Deserialize(pCurrData);
			}
			bool flag7 = fieldCount > 5;
			if (flag7)
			{
				this.ClothingDisplayId = *(short*)pCurrData;
				pCurrData += 2;
			}
			bool flag8 = fieldCount > 6;
			if (flag8)
			{
				pCurrData += this.FullName.Deserialize(pCurrData);
			}
			bool flag9 = fieldCount > 7;
			if (flag9)
			{
				pCurrData += this.OrganizationInfo.Deserialize(pCurrData);
			}
			bool flag10 = fieldCount > 8;
			if (flag10)
			{
				pCurrData += this.MonasticTitle.Deserialize(pCurrData);
			}
			bool flag11 = fieldCount > 9;
			if (flag11)
			{
				this.CustomDisplayNameId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag12 = fieldCount > 10;
			if (flag12)
			{
				this.SelfRelationToTaiwu = *(ushort*)pCurrData;
				pCurrData += 2;
			}
			bool flag13 = fieldCount > 11;
			if (flag13)
			{
				this.TaiwuRelationToSelf = *(ushort*)pCurrData;
				pCurrData += 2;
			}
			bool flag14 = fieldCount > 12;
			if (flag14)
			{
				this.AliveState = *(sbyte*)pCurrData;
				pCurrData++;
			}
			bool flag15 = fieldCount > 13;
			if (flag15)
			{
				this.CurrAge = *(short*)pCurrData;
				pCurrData += 2;
			}
			bool flag16 = fieldCount > 14;
			if (flag16)
			{
				this.ActualAge = *(short*)pCurrData;
				pCurrData += 2;
			}
			bool flag17 = fieldCount > 15;
			if (flag17)
			{
				pCurrData += this.Location.Deserialize(pCurrData);
			}
			bool flag18 = fieldCount > 16;
			if (flag18)
			{
				this.BirthDate = *(int*)pCurrData;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001DAE RID: 7598
		[SerializableGameDataField]
		public int Id;

		// Token: 0x04001DAF RID: 7599
		[SerializableGameDataField]
		public short CharTemplateId;

		// Token: 0x04001DB0 RID: 7600
		[SerializableGameDataField]
		public sbyte Gender;

		// Token: 0x04001DB1 RID: 7601
		[SerializableGameDataField]
		public short CurrAge;

		// Token: 0x04001DB2 RID: 7602
		[SerializableGameDataField]
		public short ActualAge;

		// Token: 0x04001DB3 RID: 7603
		[SerializableGameDataField]
		public byte MonkType;

		// Token: 0x04001DB4 RID: 7604
		[SerializableGameDataField]
		public AvatarData Avatar;

		// Token: 0x04001DB5 RID: 7605
		[SerializableGameDataField]
		public short ClothingDisplayId;

		// Token: 0x04001DB6 RID: 7606
		[SerializableGameDataField]
		public FullName FullName;

		// Token: 0x04001DB7 RID: 7607
		[SerializableGameDataField]
		public OrganizationInfo OrganizationInfo;

		// Token: 0x04001DB8 RID: 7608
		[SerializableGameDataField]
		public MonasticTitle MonasticTitle;

		// Token: 0x04001DB9 RID: 7609
		[SerializableGameDataField]
		public int CustomDisplayNameId;

		// Token: 0x04001DBA RID: 7610
		[SerializableGameDataField]
		public ushort SelfRelationToTaiwu;

		// Token: 0x04001DBB RID: 7611
		[SerializableGameDataField]
		public ushort TaiwuRelationToSelf;

		// Token: 0x04001DBC RID: 7612
		[SerializableGameDataField]
		public sbyte AliveState;

		// Token: 0x04001DBD RID: 7613
		[SerializableGameDataField]
		public Location Location;

		// Token: 0x04001DBE RID: 7614
		[SerializableGameDataField]
		public int BirthDate;

		// Token: 0x02000BBC RID: 3004
		private static class FieldIds
		{
			// Token: 0x04003281 RID: 12929
			public const ushort Id = 0;

			// Token: 0x04003282 RID: 12930
			public const ushort CharTemplateId = 1;

			// Token: 0x04003283 RID: 12931
			public const ushort Gender = 2;

			// Token: 0x04003284 RID: 12932
			public const ushort MonkType = 3;

			// Token: 0x04003285 RID: 12933
			public const ushort Avatar = 4;

			// Token: 0x04003286 RID: 12934
			public const ushort ClothingDisplayId = 5;

			// Token: 0x04003287 RID: 12935
			public const ushort FullName = 6;

			// Token: 0x04003288 RID: 12936
			public const ushort OrganizationInfo = 7;

			// Token: 0x04003289 RID: 12937
			public const ushort MonasticTitle = 8;

			// Token: 0x0400328A RID: 12938
			public const ushort CustomDisplayNameId = 9;

			// Token: 0x0400328B RID: 12939
			public const ushort SelfRelationToTaiwu = 10;

			// Token: 0x0400328C RID: 12940
			public const ushort TaiwuRelationToSelf = 11;

			// Token: 0x0400328D RID: 12941
			public const ushort AliveState = 12;

			// Token: 0x0400328E RID: 12942
			public const ushort CurrAge = 13;

			// Token: 0x0400328F RID: 12943
			public const ushort ActualAge = 14;

			// Token: 0x04003290 RID: 12944
			public const ushort Location = 15;

			// Token: 0x04003291 RID: 12945
			public const ushort BirthDate = 16;

			// Token: 0x04003292 RID: 12946
			public const ushort Count = 17;

			// Token: 0x04003293 RID: 12947
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"Id",
				"CharTemplateId",
				"Gender",
				"MonkType",
				"Avatar",
				"ClothingDisplayId",
				"FullName",
				"OrganizationInfo",
				"MonasticTitle",
				"CustomDisplayNameId",
				"SelfRelationToTaiwu",
				"TaiwuRelationToSelf",
				"AliveState",
				"CurrAge",
				"ActualAge",
				"Location",
				"BirthDate"
			};
		}
	}
}
