using System;
using System.Runtime.InteropServices;

namespace GameData.Domains.Character.TemporaryModification
{
	// Token: 0x0200081C RID: 2076
	[StructLayout(LayoutKind.Explicit)]
	public struct RevertibleCharacterPropertyValue
	{
		// Token: 0x04001F07 RID: 7943
		[FieldOffset(0)]
		public object Object;

		// Token: 0x04001F08 RID: 7944
		[FieldOffset(8)]
		public sbyte Sbyte;

		// Token: 0x04001F09 RID: 7945
		[FieldOffset(8)]
		public byte Byte;

		// Token: 0x04001F0A RID: 7946
		[FieldOffset(8)]
		public short Short;

		// Token: 0x04001F0B RID: 7947
		[FieldOffset(8)]
		public ushort Ushort;

		// Token: 0x04001F0C RID: 7948
		[FieldOffset(8)]
		public int Int;

		// Token: 0x04001F0D RID: 7949
		[FieldOffset(8)]
		public uint Uint;

		// Token: 0x04001F0E RID: 7950
		[FieldOffset(8)]
		public long Long;

		// Token: 0x04001F0F RID: 7951
		[FieldOffset(8)]
		public ulong Ulong;

		// Token: 0x04001F10 RID: 7952
		[FieldOffset(8)]
		public MainAttributes MainAttributes;

		// Token: 0x04001F11 RID: 7953
		[FieldOffset(8)]
		public Injuries Injuries;

		// Token: 0x04001F12 RID: 7954
		[FieldOffset(8)]
		public LifeSkillShorts LifeSkills;

		// Token: 0x04001F13 RID: 7955
		[FieldOffset(8)]
		public CombatSkillShorts CombatSkills;

		// Token: 0x04001F14 RID: 7956
		[FieldOffset(8)]
		public ResourceInts Resources;

		// Token: 0x04001F15 RID: 7957
		[FieldOffset(8)]
		public PoisonInts Poisons;

		// Token: 0x04001F16 RID: 7958
		[FieldOffset(8)]
		public NeiliAllocation ExtraNeiliAllocation;
	}
}
