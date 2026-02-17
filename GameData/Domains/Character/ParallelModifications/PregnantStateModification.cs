using System;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000833 RID: 2099
	public class PregnantStateModification
	{
		// Token: 0x04001FC0 RID: 8128
		public PregnantStateModification.ChildState State = PregnantStateModification.ChildState.Invalid;

		// Token: 0x04001FC1 RID: 8129
		public bool Dystocia;

		// Token: 0x04001FC2 RID: 8130
		public bool LostMother;

		// Token: 0x04001FC3 RID: 8131
		public bool DreamedOfCricket;

		// Token: 0x04001FC4 RID: 8132
		public bool PrenatalEvent;

		// Token: 0x02000C0B RID: 3083
		public enum ChildState
		{
			// Token: 0x04003420 RID: 13344
			Invalid,
			// Token: 0x04003421 RID: 13345
			Dead,
			// Token: 0x04003422 RID: 13346
			AliveHuman,
			// Token: 0x04003423 RID: 13347
			AliveCricket
		}
	}
}
