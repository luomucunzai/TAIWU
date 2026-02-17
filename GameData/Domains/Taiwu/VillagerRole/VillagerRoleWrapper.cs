using System;
using System.Runtime.CompilerServices;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x02000054 RID: 84
	[SerializableGameData(NoCopyConstructors = true, NotForDisplayModule = true)]
	public class VillagerRoleWrapper : ISerializableGameData
	{
		// Token: 0x06001441 RID: 5185 RVA: 0x0013E374 File Offset: 0x0013C574
		public T GetRoleAs<T>() where T : VillagerRoleBase
		{
			return this.VillagerRole as T;
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0013E396 File Offset: 0x0013C596
		public VillagerRoleWrapper(short templateId)
		{
			this.TemplateId = -1;
			this.SetRoleTemplateId(templateId);
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0013E3AF File Offset: 0x0013C5AF
		public VillagerRoleWrapper()
		{
			this.TemplateId = -1;
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0013E3C0 File Offset: 0x0013C5C0
		public void SetRoleTemplateId(short templateId)
		{
			bool flag = this.TemplateId == templateId;
			if (!flag)
			{
				this.TemplateId = templateId;
				this.VillagerRole = VillagerRoleWrapper.CreateVillagerRoleObject(templateId);
				this.VillagerRole.ArrangementTemplateId = -1;
			}
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0013E3FC File Offset: 0x0013C5FC
		private static VillagerRoleBase CreateVillagerRoleObject(short templateId)
		{
			VillagerRoleBase result;
			switch (templateId)
			{
			case 0:
				result = new VillagerRoleFarmer();
				break;
			case 1:
				result = new VillagerRoleCraftsman();
				break;
			case 2:
				result = new VillagerRoleDoctor();
				break;
			case 3:
				result = new VillagerRoleMerchant();
				break;
			case 4:
				result = new VillagerRoleLiterati();
				break;
			case 5:
				result = new VillagerRoleSwordTombKeeper();
				break;
			case 6:
				result = new VillagerRoleHead();
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("templateId ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(templateId);
				defaultInterpolatedStringHandler.AppendLiteral(" is out of range.");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0013E4A8 File Offset: 0x0013C6A8
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0013E4BC File Offset: 0x0013C6BC
		public int GetSerializedSize()
		{
			int totalSize = 2 + this.VillagerRole.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0013E4EC File Offset: 0x0013C6EC
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = this.TemplateId;
			byte* pCurrData = pData + 2;
			pCurrData += this.VillagerRole.Serialize(pCurrData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0013E534 File Offset: 0x0013C734
		public unsafe int Deserialize(byte* pData)
		{
			this.TemplateId = *(short*)pData;
			byte* pCurrData = pData + 2;
			this.VillagerRole = VillagerRoleWrapper.CreateVillagerRoleObject(this.TemplateId);
			pCurrData += this.VillagerRole.Deserialize(pCurrData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04000327 RID: 807
		[SerializableGameDataField]
		public short TemplateId;

		// Token: 0x04000328 RID: 808
		[SerializableGameDataField]
		public VillagerRoleBase VillagerRole;
	}
}
