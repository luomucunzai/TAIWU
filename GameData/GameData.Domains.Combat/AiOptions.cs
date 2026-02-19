using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class AiOptions : ISerializableGameData
{
	[SerializableGameDataField]
	public bool AutoAttack;

	[SerializableGameDataField]
	public bool AutoChangeWeapon;

	[SerializableGameDataField]
	public bool AutoChangeWeaponInnerRatio;

	[SerializableGameDataField]
	public bool AutoChangeTrick;

	[SerializableGameDataField]
	public bool AutoUnlock;

	[SerializableGameDataField]
	public bool SkipRawCreate;

	[SerializableGameDataField]
	public bool AutoMove;

	[SerializableGameDataField]
	public bool TryDodge;

	[SerializableGameDataField]
	public bool SaveMoveTarget;

	[SerializableGameDataField]
	public bool AutoCostNeiliAllocation;

	[SerializableGameDataField]
	public bool AutoInterrupt;

	[SerializableGameDataField]
	public bool AutoClearAgile;

	[SerializableGameDataField]
	public bool AutoClearDefense;

	[SerializableGameDataField]
	public bool AutoCostTrick;

	[SerializableGameDataField]
	public bool[] AutoCastSkill = new bool[3];

	[SerializableGameDataField]
	public bool[] AutoUseOtherAction = new bool[3];

	[SerializableGameDataField]
	public bool[] AutoUseTeammateCommand = new bool[25];

	public void Reset()
	{
		AutoAttack = true;
		AutoChangeWeapon = true;
		AutoChangeWeaponInnerRatio = true;
		AutoChangeTrick = true;
		AutoUnlock = false;
		SkipRawCreate = false;
		AutoMove = true;
		TryDodge = true;
		SaveMoveTarget = false;
		AutoCostNeiliAllocation = false;
		AutoInterrupt = false;
		AutoClearAgile = false;
		AutoClearDefense = false;
		for (int i = 0; i < AutoCastSkill.Length; i++)
		{
			AutoCastSkill[i] = true;
		}
		for (int j = 0; j < AutoUseOtherAction.Length; j++)
		{
			AutoUseOtherAction[j] = true;
		}
		for (int k = 0; k < AutoUseTeammateCommand.Length; k++)
		{
			AutoUseTeammateCommand[k] = true;
		}
	}

	public AiOptions()
	{
	}

	public AiOptions(AiOptions other)
	{
		AutoAttack = other.AutoAttack;
		AutoChangeWeapon = other.AutoChangeWeapon;
		AutoChangeWeaponInnerRatio = other.AutoChangeWeaponInnerRatio;
		AutoChangeTrick = other.AutoChangeTrick;
		AutoUnlock = other.AutoUnlock;
		SkipRawCreate = other.SkipRawCreate;
		AutoMove = other.AutoMove;
		TryDodge = other.TryDodge;
		SaveMoveTarget = other.SaveMoveTarget;
		AutoCostNeiliAllocation = other.AutoCostNeiliAllocation;
		AutoInterrupt = other.AutoInterrupt;
		AutoClearAgile = other.AutoClearAgile;
		AutoClearDefense = other.AutoClearDefense;
		AutoCostTrick = other.AutoCostTrick;
		bool[] autoCastSkill = other.AutoCastSkill;
		int num = autoCastSkill.Length;
		AutoCastSkill = new bool[num];
		for (int i = 0; i < num; i++)
		{
			AutoCastSkill[i] = autoCastSkill[i];
		}
		bool[] autoUseOtherAction = other.AutoUseOtherAction;
		int num2 = autoUseOtherAction.Length;
		AutoUseOtherAction = new bool[num2];
		for (int j = 0; j < num2; j++)
		{
			AutoUseOtherAction[j] = autoUseOtherAction[j];
		}
		bool[] autoUseTeammateCommand = other.AutoUseTeammateCommand;
		int num3 = autoUseTeammateCommand.Length;
		AutoUseTeammateCommand = new bool[num3];
		for (int k = 0; k < num3; k++)
		{
			AutoUseTeammateCommand[k] = autoUseTeammateCommand[k];
		}
	}

	public void Assign(AiOptions other)
	{
		AutoAttack = other.AutoAttack;
		AutoChangeWeapon = other.AutoChangeWeapon;
		AutoChangeWeaponInnerRatio = other.AutoChangeWeaponInnerRatio;
		AutoChangeTrick = other.AutoChangeTrick;
		AutoUnlock = other.AutoUnlock;
		SkipRawCreate = other.SkipRawCreate;
		AutoMove = other.AutoMove;
		TryDodge = other.TryDodge;
		SaveMoveTarget = other.SaveMoveTarget;
		AutoCostNeiliAllocation = other.AutoCostNeiliAllocation;
		AutoInterrupt = other.AutoInterrupt;
		AutoClearAgile = other.AutoClearAgile;
		AutoClearDefense = other.AutoClearDefense;
		AutoCostTrick = other.AutoCostTrick;
		bool[] autoCastSkill = other.AutoCastSkill;
		int num = autoCastSkill.Length;
		AutoCastSkill = new bool[num];
		for (int i = 0; i < num; i++)
		{
			AutoCastSkill[i] = autoCastSkill[i];
		}
		bool[] autoUseOtherAction = other.AutoUseOtherAction;
		int num2 = autoUseOtherAction.Length;
		AutoUseOtherAction = new bool[num2];
		for (int j = 0; j < num2; j++)
		{
			AutoUseOtherAction[j] = autoUseOtherAction[j];
		}
		bool[] autoUseTeammateCommand = other.AutoUseTeammateCommand;
		int num3 = autoUseTeammateCommand.Length;
		AutoUseTeammateCommand = new bool[num3];
		for (int k = 0; k < num3; k++)
		{
			AutoUseTeammateCommand[k] = autoUseTeammateCommand[k];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 14;
		num = ((AutoCastSkill == null) ? (num + 2) : (num + (2 + AutoCastSkill.Length)));
		num = ((AutoUseOtherAction == null) ? (num + 2) : (num + (2 + AutoUseOtherAction.Length)));
		num = ((AutoUseTeammateCommand == null) ? (num + 2) : (num + (2 + AutoUseTeammateCommand.Length)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (AutoAttack ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AutoChangeWeapon ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AutoChangeWeaponInnerRatio ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AutoChangeTrick ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AutoUnlock ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (SkipRawCreate ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AutoMove ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (TryDodge ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (SaveMoveTarget ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AutoCostNeiliAllocation ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AutoInterrupt ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AutoClearAgile ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AutoClearDefense ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AutoCostTrick ? ((byte)1) : ((byte)0));
		ptr++;
		if (AutoCastSkill != null)
		{
			int num = AutoCastSkill.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				ptr[i] = (AutoCastSkill[i] ? ((byte)1) : ((byte)0));
			}
			ptr += num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (AutoUseOtherAction != null)
		{
			int num2 = AutoUseOtherAction.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int j = 0; j < num2; j++)
			{
				ptr[j] = (AutoUseOtherAction[j] ? ((byte)1) : ((byte)0));
			}
			ptr += num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (AutoUseTeammateCommand != null)
		{
			int num3 = AutoUseTeammateCommand.Length;
			Tester.Assert(num3 <= 65535);
			*(ushort*)ptr = (ushort)num3;
			ptr += 2;
			for (int k = 0; k < num3; k++)
			{
				ptr[k] = (AutoUseTeammateCommand[k] ? ((byte)1) : ((byte)0));
			}
			ptr += num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		AutoAttack = *ptr != 0;
		ptr++;
		AutoChangeWeapon = *ptr != 0;
		ptr++;
		AutoChangeWeaponInnerRatio = *ptr != 0;
		ptr++;
		AutoChangeTrick = *ptr != 0;
		ptr++;
		AutoUnlock = *ptr != 0;
		ptr++;
		SkipRawCreate = *ptr != 0;
		ptr++;
		AutoMove = *ptr != 0;
		ptr++;
		TryDodge = *ptr != 0;
		ptr++;
		SaveMoveTarget = *ptr != 0;
		ptr++;
		AutoCostNeiliAllocation = *ptr != 0;
		ptr++;
		AutoInterrupt = *ptr != 0;
		ptr++;
		AutoClearAgile = *ptr != 0;
		ptr++;
		AutoClearDefense = *ptr != 0;
		ptr++;
		AutoCostTrick = *ptr != 0;
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (AutoCastSkill == null || AutoCastSkill.Length != num)
			{
				AutoCastSkill = new bool[num];
			}
			for (int i = 0; i < num; i++)
			{
				AutoCastSkill[i] = ptr[i] != 0;
			}
			ptr += (int)num;
		}
		else
		{
			AutoCastSkill = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (AutoUseOtherAction == null || AutoUseOtherAction.Length != num2)
			{
				AutoUseOtherAction = new bool[num2];
			}
			for (int j = 0; j < num2; j++)
			{
				AutoUseOtherAction[j] = ptr[j] != 0;
			}
			ptr += (int)num2;
		}
		else
		{
			AutoUseOtherAction = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (AutoUseTeammateCommand == null || AutoUseTeammateCommand.Length != num3)
			{
				AutoUseTeammateCommand = new bool[num3];
			}
			for (int k = 0; k < num3; k++)
			{
				AutoUseTeammateCommand[k] = ptr[k] != 0;
			}
			ptr += (int)num3;
		}
		else
		{
			AutoUseTeammateCommand = null;
		}
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}
}
