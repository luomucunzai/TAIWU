using System;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x0200068E RID: 1678
	public class AiOptions : ISerializableGameData
	{
		// Token: 0x06005F68 RID: 24424 RVA: 0x00364714 File Offset: 0x00362914
		public void Reset()
		{
			this.AutoAttack = true;
			this.AutoChangeWeapon = true;
			this.AutoChangeWeaponInnerRatio = true;
			this.AutoChangeTrick = true;
			this.AutoUnlock = false;
			this.SkipRawCreate = false;
			this.AutoMove = true;
			this.TryDodge = true;
			this.SaveMoveTarget = false;
			this.AutoCostNeiliAllocation = false;
			this.AutoInterrupt = false;
			this.AutoClearAgile = false;
			this.AutoClearDefense = false;
			for (int i = 0; i < this.AutoCastSkill.Length; i++)
			{
				this.AutoCastSkill[i] = true;
			}
			for (int j = 0; j < this.AutoUseOtherAction.Length; j++)
			{
				this.AutoUseOtherAction[j] = true;
			}
			for (int k = 0; k < this.AutoUseTeammateCommand.Length; k++)
			{
				this.AutoUseTeammateCommand[k] = true;
			}
		}

		// Token: 0x06005F69 RID: 24425 RVA: 0x003647E4 File Offset: 0x003629E4
		public AiOptions()
		{
		}

		// Token: 0x06005F6A RID: 24426 RVA: 0x00364814 File Offset: 0x00362A14
		public AiOptions(AiOptions other)
		{
			this.AutoAttack = other.AutoAttack;
			this.AutoChangeWeapon = other.AutoChangeWeapon;
			this.AutoChangeWeaponInnerRatio = other.AutoChangeWeaponInnerRatio;
			this.AutoChangeTrick = other.AutoChangeTrick;
			this.AutoUnlock = other.AutoUnlock;
			this.SkipRawCreate = other.SkipRawCreate;
			this.AutoMove = other.AutoMove;
			this.TryDodge = other.TryDodge;
			this.SaveMoveTarget = other.SaveMoveTarget;
			this.AutoCostNeiliAllocation = other.AutoCostNeiliAllocation;
			this.AutoInterrupt = other.AutoInterrupt;
			this.AutoClearAgile = other.AutoClearAgile;
			this.AutoClearDefense = other.AutoClearDefense;
			this.AutoCostTrick = other.AutoCostTrick;
			bool[] item = other.AutoCastSkill;
			int elementsCount = item.Length;
			this.AutoCastSkill = new bool[elementsCount];
			for (int i = 0; i < elementsCount; i++)
			{
				this.AutoCastSkill[i] = item[i];
			}
			bool[] item2 = other.AutoUseOtherAction;
			int elementsCount2 = item2.Length;
			this.AutoUseOtherAction = new bool[elementsCount2];
			for (int j = 0; j < elementsCount2; j++)
			{
				this.AutoUseOtherAction[j] = item2[j];
			}
			bool[] item3 = other.AutoUseTeammateCommand;
			int elementsCount3 = item3.Length;
			this.AutoUseTeammateCommand = new bool[elementsCount3];
			for (int k = 0; k < elementsCount3; k++)
			{
				this.AutoUseTeammateCommand[k] = item3[k];
			}
		}

		// Token: 0x06005F6B RID: 24427 RVA: 0x003649B0 File Offset: 0x00362BB0
		public void Assign(AiOptions other)
		{
			this.AutoAttack = other.AutoAttack;
			this.AutoChangeWeapon = other.AutoChangeWeapon;
			this.AutoChangeWeaponInnerRatio = other.AutoChangeWeaponInnerRatio;
			this.AutoChangeTrick = other.AutoChangeTrick;
			this.AutoUnlock = other.AutoUnlock;
			this.SkipRawCreate = other.SkipRawCreate;
			this.AutoMove = other.AutoMove;
			this.TryDodge = other.TryDodge;
			this.SaveMoveTarget = other.SaveMoveTarget;
			this.AutoCostNeiliAllocation = other.AutoCostNeiliAllocation;
			this.AutoInterrupt = other.AutoInterrupt;
			this.AutoClearAgile = other.AutoClearAgile;
			this.AutoClearDefense = other.AutoClearDefense;
			this.AutoCostTrick = other.AutoCostTrick;
			bool[] item = other.AutoCastSkill;
			int elementsCount = item.Length;
			this.AutoCastSkill = new bool[elementsCount];
			for (int i = 0; i < elementsCount; i++)
			{
				this.AutoCastSkill[i] = item[i];
			}
			bool[] item2 = other.AutoUseOtherAction;
			int elementsCount2 = item2.Length;
			this.AutoUseOtherAction = new bool[elementsCount2];
			for (int j = 0; j < elementsCount2; j++)
			{
				this.AutoUseOtherAction[j] = item2[j];
			}
			bool[] item3 = other.AutoUseTeammateCommand;
			int elementsCount3 = item3.Length;
			this.AutoUseTeammateCommand = new bool[elementsCount3];
			for (int k = 0; k < elementsCount3; k++)
			{
				this.AutoUseTeammateCommand[k] = item3[k];
			}
		}

		// Token: 0x06005F6C RID: 24428 RVA: 0x00364B20 File Offset: 0x00362D20
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06005F6D RID: 24429 RVA: 0x00364B34 File Offset: 0x00362D34
		public int GetSerializedSize()
		{
			int totalSize = 14;
			bool flag = this.AutoCastSkill != null;
			if (flag)
			{
				totalSize += 2 + this.AutoCastSkill.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this.AutoUseOtherAction != null;
			if (flag2)
			{
				totalSize += 2 + this.AutoUseOtherAction.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag3 = this.AutoUseTeammateCommand != null;
			if (flag3)
			{
				totalSize += 2 + this.AutoUseTeammateCommand.Length;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06005F6E RID: 24430 RVA: 0x00364BC0 File Offset: 0x00362DC0
		public unsafe int Serialize(byte* pData)
		{
			*pData = (this.AutoAttack ? 1 : 0);
			byte* pCurrData = pData + 1;
			*pCurrData = (this.AutoChangeWeapon ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.AutoChangeWeaponInnerRatio ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.AutoChangeTrick ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.AutoUnlock ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.SkipRawCreate ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.AutoMove ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.TryDodge ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.SaveMoveTarget ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.AutoCostNeiliAllocation ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.AutoInterrupt ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.AutoClearAgile ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.AutoClearDefense ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.AutoCostTrick ? 1 : 0);
			pCurrData++;
			bool flag = this.AutoCastSkill != null;
			if (flag)
			{
				int elementsCount = this.AutoCastSkill.Length;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pCurrData[i] = (this.AutoCastSkill[i] ? 1 : 0);
				}
				pCurrData += elementsCount;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag2 = this.AutoUseOtherAction != null;
			if (flag2)
			{
				int elementsCount2 = this.AutoUseOtherAction.Length;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					pCurrData[j] = (this.AutoUseOtherAction[j] ? 1 : 0);
				}
				pCurrData += elementsCount2;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag3 = this.AutoUseTeammateCommand != null;
			if (flag3)
			{
				int elementsCount3 = this.AutoUseTeammateCommand.Length;
				Tester.Assert(elementsCount3 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount3);
				pCurrData += 2;
				for (int k = 0; k < elementsCount3; k++)
				{
					pCurrData[k] = (this.AutoUseTeammateCommand[k] ? 1 : 0);
				}
				pCurrData += elementsCount3;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06005F6F RID: 24431 RVA: 0x00364DE4 File Offset: 0x00362FE4
		public unsafe int Deserialize(byte* pData)
		{
			this.AutoAttack = (*pData != 0);
			byte* pCurrData = pData + 1;
			this.AutoChangeWeapon = (*pCurrData != 0);
			pCurrData++;
			this.AutoChangeWeaponInnerRatio = (*pCurrData != 0);
			pCurrData++;
			this.AutoChangeTrick = (*pCurrData != 0);
			pCurrData++;
			this.AutoUnlock = (*pCurrData != 0);
			pCurrData++;
			this.SkipRawCreate = (*pCurrData != 0);
			pCurrData++;
			this.AutoMove = (*pCurrData != 0);
			pCurrData++;
			this.TryDodge = (*pCurrData != 0);
			pCurrData++;
			this.SaveMoveTarget = (*pCurrData != 0);
			pCurrData++;
			this.AutoCostNeiliAllocation = (*pCurrData != 0);
			pCurrData++;
			this.AutoInterrupt = (*pCurrData != 0);
			pCurrData++;
			this.AutoClearAgile = (*pCurrData != 0);
			pCurrData++;
			this.AutoClearDefense = (*pCurrData != 0);
			pCurrData++;
			this.AutoCostTrick = (*pCurrData != 0);
			pCurrData++;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.AutoCastSkill == null || this.AutoCastSkill.Length != (int)elementsCount;
				if (flag2)
				{
					this.AutoCastSkill = new bool[(int)elementsCount];
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					this.AutoCastSkill[i] = (pCurrData[i] != 0);
				}
				pCurrData += elementsCount;
			}
			else
			{
				this.AutoCastSkill = null;
			}
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this.AutoUseOtherAction == null || this.AutoUseOtherAction.Length != (int)elementsCount2;
				if (flag4)
				{
					this.AutoUseOtherAction = new bool[(int)elementsCount2];
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					this.AutoUseOtherAction[j] = (pCurrData[j] != 0);
				}
				pCurrData += elementsCount2;
			}
			else
			{
				this.AutoUseOtherAction = null;
			}
			ushort elementsCount3 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag5 = elementsCount3 > 0;
			if (flag5)
			{
				bool flag6 = this.AutoUseTeammateCommand == null || this.AutoUseTeammateCommand.Length != (int)elementsCount3;
				if (flag6)
				{
					this.AutoUseTeammateCommand = new bool[(int)elementsCount3];
				}
				for (int k = 0; k < (int)elementsCount3; k++)
				{
					this.AutoUseTeammateCommand[k] = (pCurrData[k] != 0);
				}
				pCurrData += elementsCount3;
			}
			else
			{
				this.AutoUseTeammateCommand = null;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001937 RID: 6455
		[SerializableGameDataField]
		public bool AutoAttack;

		// Token: 0x04001938 RID: 6456
		[SerializableGameDataField]
		public bool AutoChangeWeapon;

		// Token: 0x04001939 RID: 6457
		[SerializableGameDataField]
		public bool AutoChangeWeaponInnerRatio;

		// Token: 0x0400193A RID: 6458
		[SerializableGameDataField]
		public bool AutoChangeTrick;

		// Token: 0x0400193B RID: 6459
		[SerializableGameDataField]
		public bool AutoUnlock;

		// Token: 0x0400193C RID: 6460
		[SerializableGameDataField]
		public bool SkipRawCreate;

		// Token: 0x0400193D RID: 6461
		[SerializableGameDataField]
		public bool AutoMove;

		// Token: 0x0400193E RID: 6462
		[SerializableGameDataField]
		public bool TryDodge;

		// Token: 0x0400193F RID: 6463
		[SerializableGameDataField]
		public bool SaveMoveTarget;

		// Token: 0x04001940 RID: 6464
		[SerializableGameDataField]
		public bool AutoCostNeiliAllocation;

		// Token: 0x04001941 RID: 6465
		[SerializableGameDataField]
		public bool AutoInterrupt;

		// Token: 0x04001942 RID: 6466
		[SerializableGameDataField]
		public bool AutoClearAgile;

		// Token: 0x04001943 RID: 6467
		[SerializableGameDataField]
		public bool AutoClearDefense;

		// Token: 0x04001944 RID: 6468
		[SerializableGameDataField]
		public bool AutoCostTrick;

		// Token: 0x04001945 RID: 6469
		[SerializableGameDataField]
		public bool[] AutoCastSkill = new bool[3];

		// Token: 0x04001946 RID: 6470
		[SerializableGameDataField]
		public bool[] AutoUseOtherAction = new bool[3];

		// Token: 0x04001947 RID: 6471
		[SerializableGameDataField]
		public bool[] AutoUseTeammateCommand = new bool[25];
	}
}
