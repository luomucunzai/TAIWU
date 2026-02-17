using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x0200071E RID: 1822
	public class AiMemoryNew
	{
		// Token: 0x0600689E RID: 26782 RVA: 0x003B77AF File Offset: 0x003B59AF
		public void Clear()
		{
			this.Ints.Clear();
			this.Strings.Clear();
			this.Booleans.Clear();
			this._skillPriorities.Clear();
		}

		// Token: 0x0600689F RID: 26783 RVA: 0x003B77E4 File Offset: 0x003B59E4
		public void SetPriority(short skillId, EAiPriority priority)
		{
			bool flag = priority == EAiPriority.Middle;
			if (flag)
			{
				this._skillPriorities.Remove(skillId);
			}
			else
			{
				this._skillPriorities[skillId] = priority;
			}
		}

		// Token: 0x060068A0 RID: 26784 RVA: 0x003B7818 File Offset: 0x003B5A18
		public EAiPriority GetPriority(short skillId)
		{
			EAiPriority priority;
			return this._skillPriorities.TryGetValue(skillId, out priority) ? priority : EAiPriority.Middle;
		}

		// Token: 0x04001CA9 RID: 7337
		public readonly Dictionary<string, int> Ints = new Dictionary<string, int>();

		// Token: 0x04001CAA RID: 7338
		public readonly Dictionary<string, string> Strings = new Dictionary<string, string>();

		// Token: 0x04001CAB RID: 7339
		public readonly Dictionary<string, bool> Booleans = new Dictionary<string, bool>();

		// Token: 0x04001CAC RID: 7340
		private readonly Dictionary<short, EAiPriority> _skillPriorities = new Dictionary<short, EAiPriority>();
	}
}
