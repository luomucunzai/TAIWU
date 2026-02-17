using System;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000724 RID: 1828
	public class AiTree
	{
		// Token: 0x060068AC RID: 26796 RVA: 0x003B7A28 File Offset: 0x003B5C28
		public AiTree(IAiParticipant participant, AiData template)
		{
			this._participant = participant;
			this._aiData = template;
		}

		// Token: 0x060068AD RID: 26797 RVA: 0x003B7A4C File Offset: 0x003B5C4C
		public void Update()
		{
			bool disableAi = this._participant.DisableAi;
			if (!disableAi)
			{
				this._aiData.Update(this._aiMemory, this._participant);
			}
		}

		// Token: 0x060068AE RID: 26798 RVA: 0x003B7A83 File Offset: 0x003B5C83
		public void ClearMemories()
		{
			this._aiData.Reset();
			this._aiMemory.Clear();
		}

		// Token: 0x04001CB3 RID: 7347
		private readonly IAiParticipant _participant;

		// Token: 0x04001CB4 RID: 7348
		private readonly AiMemoryNew _aiMemory = new AiMemoryNew();

		// Token: 0x04001CB5 RID: 7349
		private readonly AiData _aiData;
	}
}
